Option Explicit On
Option Strict On

Public Class Form1

    Private LogContent As New System.Text.StringBuilder

    Private DB As New cDB

    '''<summary>Handle to Intel IPP functions.</summary>
    Private IPP As cIntelIPP
    '''<summary>Location of the EXE.</summary>
    Private MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
    '''<summary>Drag-and-drop handler.</summary>
    Private WithEvents DD As Ato.DragDrop

    '''<summary>Last file opened.</summary>
    Private LastFile As String = String.Empty

    Private LastStat As AstroNET.Statistics.sStatistics

    '''<summary>Statistics processor (for the last file).</summary>
    Dim SingleStatCalc As AstroNET.Statistics

    '''<summary>Statistics for pixel with identical Y value.</summary>
    Dim StatPerRow() As Ato.cSingleValueStatistics
    '''<summary>Statistics for pixel with identical X value.</summary>
    Dim StatPerCol() As Ato.cSingleValueStatistics

    '''<summary>Storage for a simple stack processing.</summary>
    Private StackingStatistics(,) As Ato.cSingleValueStatistics

    Private StatVsGain As New Dictionary(Of Double, AstroImageStatistics.AstroNET.Statistics.sSingleChannelStatistics)

    Private Sub OpenFileToAnalyseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFileToAnalyseToolStripMenuItem.Click
        If ofdMain.ShowDialog <> DialogResult.OK Then Exit Sub
        For Each File As String In ofdMain.FileNames
            LoadFile(File)
        Next File
    End Sub

    Private Sub LoadFile(ByVal FileName As String)

        Dim FileNameOnly As String = System.IO.Path.GetFileName(FileName)
        Dim FITSHeader As New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(FileName))

        Dim Stopper As New cStopper
        Dim FITSReader As New cFITSReader
        SingleStatCalc = New AstroNET.Statistics(IPP)

        If DB.AutoClearLog = True Then
            LogContent.Clear()
            UpdateLog
        End If

        'Log header data and detect if NAXIS3 is set
        Log("Loading file <" & FileName & "> ...")
        Log("FITS header:")
        Dim FITSHeaderDict As Dictionary(Of String, Object) = FITSHeader.GetListAsDictionary
        Dim ContentToPrint As New List(Of String)
        For Each Entry As String In FITSHeaderDict.Keys
            ContentToPrint.Add("  " & Entry.PadRight(10) & "=" & CStr(FITSHeaderDict(Entry)).Trim.PadLeft(40))
        Next Entry
        Log(ContentToPrint)
        If FITSHeader.NAXIS > 2 Then
            Log("!!! FITS file contains debayered data which are NOT displayed correct right now")
        End If
        Log(New String("-"c, 107))

        'Perform the read operation
        Dim UBound0 As Integer = -1
        Dim UBound1 As Integer = -1
        Select Case FITSHeader.BitPix
            Case 8
                SingleStatCalc.DataProcessor_UInt16.ImageData = FITSReader.ReadInUInt8(FileName, DB.UseIPP)
                SingleStatCalc.DataProcessor_Int32.ImageData = {{}}
                UBound0 = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(1)
            Case 16
                SingleStatCalc.DataProcessor_UInt16.ImageData = FITSReader.ReadInUInt16(FileName, DB.UseIPP)
                SingleStatCalc.DataProcessor_Int32.ImageData = {{}}
                UBound0 = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(1)
            Case 32
                SingleStatCalc.DataProcessor_Int32.ImageData = FITSReader.ReadInInt32(FileName, DB.UseIPP)
                SingleStatCalc.DataProcessor_UInt16.ImageData = {{}}
                UBound0 = SingleStatCalc.DataProcessor_Int32.ImageData.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_Int32.ImageData.GetUpperBound(1)
            Case Else
                Log("!!! File format <" & FITSHeader.BitPix.ToString.Trim & "> not yet supported!")
                Exit Sub
        End Select
        Stopper.Stamp(FileNameOnly & ": Reading")

        'Calculate the statistics
        LastStat = SingleStatCalc.ImageStatistics
        Stopper.Stamp(FileNameOnly & ": Statistics")
        Log("Statistics:")
        Log("  ", LastStat.StatisticsReport.ToArray())
        Log(New String("="c, 107))

        'Trace statistics vs gain
        If FITSHeaderDict.ContainsKey("GAIN") Then
            Dim Gain As Double = CDbl(FITSHeaderDict("GAIN"))
            If StatVsGain.ContainsKey(Gain) = False Then
                StatVsGain.Add(Gain, LastStat.MonoStatistics)
            Else
                StatVsGain(Gain) = LastStat.MonoStatistics
            End If
        End If

        'Run the "stacking" (statistics for each point) is selected
        If DB.Stacking = True Then
            'Init new
            If IsNothing(StackingStatistics) = True Then
                ReDim StackingStatistics(UBound0, UBound1)
                For Idx1 As Integer = 0 To UBound0
                    For Idx2 As Integer = 0 To UBound1
                        StackingStatistics(Idx1, Idx2) = New Ato.cSingleValueStatistics(Ato.cSingleValueStatistics.eValueType.Linear)
                        StackingStatistics(Idx1, Idx2).StoreRawValues = False
                    Next Idx2
                Next Idx1
            End If
            'Add up statistics if dimension is matching
            If StackingStatistics.GetUpperBound(0) = UBound0 And StackingStatistics.GetUpperBound(1) = UBound1 Then
                Select Case FITSHeader.BitPix
                    Case 8, 16
                        For Idx1 As Integer = 0 To UBound0
                            For Idx2 As Integer = 0 To UBound1
                                StackingStatistics(Idx1, Idx2).AddValue(SingleStatCalc.DataProcessor_UInt16.ImageData(Idx1, Idx2))
                            Next Idx2
                        Next Idx1
                    Case 32
                        For Idx1 As Integer = 0 To UBound0
                            For Idx2 As Integer = 0 To UBound1
                                StackingStatistics(Idx1, Idx2).AddValue(SingleStatCalc.DataProcessor_Int32.ImageData(Idx1, Idx2))
                            Next Idx2
                        Next Idx1
                End Select
            Else
                Log("!!! Dimension mismatch between the different images!")
            End If
            Stopper.Stamp(FileNameOnly & ": Stacking")
        End If

        'Plot statistics and remember this file as last processed file
        If DB.AutoOpenStatGraph = True Then PlotStatistics(FileName, LastStat)
        LastFile = FileName
        Me.Focus()

    End Sub

    '''<summary>Take an IPP path if there is not yet one set.</summary>
    Private Sub TestIPPPath(ByRef CurrentPath As String, ByVal Path As String)
        If System.IO.Directory.Exists(Path) Then
            If String.IsNullOrEmpty(CurrentPath) = True Then CurrentPath = Path
        End If
    End Sub

    '''<summary>Open a simple form with a ZEDGraph on it and plots the statistical data.</summary>
    '''<param name="FileName">Filename that is plotted (indicated in the header).</param>
    '''<param name="Stats">Statistics data to plot.</param>
    Private Sub PlotStatistics(ByVal FileName As String, ByRef Stats As AstroNET.Statistics.sStatistics)
        If IsNothing(Stats.BayerHistograms) = True And IsNothing(Stats.MonochromHistogram) = True Then Exit Sub
        Dim Disp As New cZEDGraphForm
        Disp.PlotData(New Double() {1, 2, 3, 4})
        'Plot histogram
        Disp.Plotter.Clear()
        If IsNothing(Stats.BayerHistograms) = False Then
            Disp.Plotter.PlotXvsY("R", Stats.BayerHistograms(0, 0), New cZEDGraphService.sGraphStyle(Color.Red, 1))
            Disp.Plotter.PlotXvsY("G1", Stats.BayerHistograms(0, 1), New cZEDGraphService.sGraphStyle(Color.LightGreen, 1))
            Disp.Plotter.PlotXvsY("G2", Stats.BayerHistograms(1, 0), New cZEDGraphService.sGraphStyle(Color.DarkGreen, 1))
            Disp.Plotter.PlotXvsY("B", Stats.BayerHistograms(1, 1), New cZEDGraphService.sGraphStyle(Color.Blue, 1))
        End If
        If IsNothing(Stats.MonochromHistogram) = False Then
            Disp.Plotter.PlotXvsY("Mono histo", Stats.MonochromHistogram, New cZEDGraphService.sGraphStyle(Color.Black, 1))
        End If
        Disp.Plotter.ManuallyScaleXAxis(Stats.MonoStatistics.Min.Key, Stats.MonoStatistics.Max.Key)
        Disp.Plotter.AutoScaleYAxisLog()
        Disp.Plotter.GridOnOff(True, True)
        Disp.Plotter.ForceUpdate()
        'Set style of the window
        Disp.Plotter.SetCaptions(String.Empty, "Pixel value", "# of pixel with this value")
        Disp.Plotter.MaximizePlotArea()
        Disp.Hoster.Text = FileName
        Disp.Hoster.Icon = Me.Icon
        'Position window below the main window
        If DB.StackGraphs = True Then
            Disp.Hoster.Left = Me.Left
            Disp.Hoster.Top = Me.Top + Me.Height
            Disp.Hoster.Height = Me.Height
            Disp.Hoster.Width = Me.Width
        End If
    End Sub

    Private Sub PlotStatistics(ByVal FileName As String, ByRef Stats() As Ato.cSingleValueStatistics)
        Dim Disp As New cZEDGraphForm
        Disp.PlotData(New Double() {1, 2, 3, 4})
        'Plot data
        Dim XAxis() As Double = Ato.cSingleValueStatistics.GetAspectVectorXAxis(Stats)
        Disp.Plotter.Clear()
        Disp.Plotter.PlotXvsY("Mean", XAxis, Ato.cSingleValueStatistics.GetAspectVector(Stats, Ato.cSingleValueStatistics.eAspects.Mean), New cZEDGraphService.sGraphStyle(Color.Black, 1))
        Disp.Plotter.PlotXvsY("Max", XAxis, Ato.cSingleValueStatistics.GetAspectVector(Stats, Ato.cSingleValueStatistics.eAspects.Maximum), New cZEDGraphService.sGraphStyle(Color.Red, 1))
        Disp.Plotter.PlotXvsY("Min", XAxis, Ato.cSingleValueStatistics.GetAspectVector(Stats, Ato.cSingleValueStatistics.eAspects.Minimum), New cZEDGraphService.sGraphStyle(Color.Green, 1))
        Disp.Plotter.ManuallyScaleXAxis(XAxis(0), XAxis(XAxis.GetUpperBound(0)))
        Disp.Plotter.GridOnOff(True, True)
        Disp.Plotter.ForceUpdate()
        'Set style of the window
        Disp.Plotter.SetCaptions(String.Empty, "Pixel index", "Statistics value")
        Disp.Plotter.MaximizePlotArea()
        Disp.Hoster.Text = FileName
        Disp.Hoster.Icon = Me.Icon
        'Position window below the main window
        Disp.Hoster.Left = Me.Left
        Disp.Hoster.Top = Me.Top + Me.Height
        Disp.Hoster.Height = Me.Height
        Disp.Hoster.Width = Me.Width
    End Sub

    Private Sub PlotStatistics(ByVal FileName As String, ByRef Stats As Dictionary(Of Double, AstroImageStatistics.AstroNET.Statistics.sSingleChannelStatistics))
        Dim Disp As New cZEDGraphForm
        Disp.PlotData(New Double() {1, 2, 3, 4})
        'Plot data
        Dim XAxis As New List(Of Double)
        Dim YAxis As New List(Of Double)
        For Each Entry As Double In Stats.Keys
            XAxis.Add(Entry)
            YAxis.Add(Stats(Entry).Mean)
        Next Entry
        Disp.Plotter.Clear()
        Disp.Plotter.PlotXvsY("StdDev", XAxis.ToArray, YAxis.ToArray, New cZEDGraphService.sGraphStyle(Color.Black, 1))
        Disp.Plotter.GridOnOff(True, True)
        Disp.Plotter.ForceUpdate()
        'Set style of the window
        Disp.Plotter.SetCaptions(String.Empty, "Gain", "StdDev")
        Disp.Plotter.MaximizePlotArea()
        Disp.Hoster.Text = FileName
        Disp.Hoster.Icon = Me.Icon
        'Position window below the main window
        Disp.Hoster.Left = Me.Left
        Disp.Hoster.Top = Me.Top + Me.Height
        Disp.Hoster.Height = Me.Height
        Disp.Hoster.Width = Me.Width
    End Sub

    Private Sub RemoveOverscanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveOverscanToolStripMenuItem.Click

        Dim Capture_W As Integer = 6000
        Dim Capture_H As Integer = 4000
        Dim ROI_X As Integer = 24
        Dim ROI_Y As Integer = 36
        Dim ROI_Width As Integer = Capture_W - ROI_X - 55
        Dim ROI_Height As Integer = Capture_H - ROI_Y - 66

        Dim CapturePixel As Integer = Capture_W * Capture_H
        Dim CaptureBytes As Integer = CapturePixel * 2

        Dim Stopp As New cStopper

        'Create test data
        'Dim CamRawBuffer(CaptureBytes - 1) As Byte : ImgArrayFunction.FillImageWhiteRightDown(CamRawBuffer)
        'Dim FullImage(,) As UInt16 = ImgArrayFunction.ChangeAspectIPP(IPP, CamRawBuffer, CInt(Capture_W), CInt(Capture_H))                        'convert flat to UInt16 matrix in a temporary buffer
        Stopp.Start()
        Dim FullImage(Capture_W - 1, Capture_H - 1) As UInt16
        ImgArrayFunction.FillImageWhiteRightDown(FullImage)
        Log(Stopp.Stamp("Test image"))

        'Log some basic info
        Log("Full image has dimension <" & (FullImage.GetUpperBound(0) + 1).ValRegIndep & "x" & (FullImage.GetUpperBound(1) + 1).ValRegIndep & ">")
        Log("0:0 is " & FullImage(0, 0).ValRegIndep)
        Log(ROI_X.ValRegIndep & ":" & ROI_Y.ValRegIndep & " is " & FullImage(ROI_X, ROI_Y).ValRegIndep)

        Stopp.Start()
        Dim ROI(ROI_Width - 1, ROI_Height - 1) As UInt16
        Dim Status As cIntelIPP.IppStatus = IPP.Copy(FullImage, ROI, ROI_X, ROI_Y, ROI_Width, ROI_Height)
        Log(Stopp.Stamp("Get ROI"))
        Log("ROI has dimension <" & (ROI.GetUpperBound(0) + 1).ValRegIndep & "x" & (ROI.GetUpperBound(1) + 1).ValRegIndep & ">")
        Log("0:0 is " & ROI(0, 0).ValRegIndep)

        cFITSWriter.Write(System.IO.Path.Combine(MyPath, "IPPCopy_1.fits"), FullImage, cFITSWriter.eBitPix.Int16)
        cFITSWriter.Write(System.IO.Path.Combine(MyPath, "IPPCopy_2.fits"), ROI, cFITSWriter.eBitPix.Int16)

        'Check with direct (show) VB code
        Dim ROI_OK As String = ImgArrayFunction.CheckROICorrect(FullImage, ROI, ROI_X, ROI_Y, ROI_Width, ROI_Height)
        If String.IsNullOrEmpty(ROI_OK) = True Then
            Log("ROI correct.")
        Else
            Log("!!! ROI ERROR: <" & ROI_OK & ">")
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Get build data
        Dim BuildDate As String = String.Empty
        Dim AllResources As String() = System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceNames
        For Each Entry As String In AllResources
            If Entry.EndsWith(".BuildDate.txt") Then
                BuildDate = " (Build of " & (New System.IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream(Entry)).ReadToEnd.Trim).Replace(",", ".") & ")"
                Exit For
            End If
        Next Entry
        Me.Text &= BuildDate

        'IPP laden
        DB.MyIPPPath = String.Empty
        TestIPPPath(DB.MyIPPPath, System.IO.Path.Combine(MyPath, "ipp"))
        TestIPPPath(DB.MyIPPPath, "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2020.0.166\windows\redist\intel64_win\ipp")
        TestIPPPath(DB.MyIPPPath, "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.5.281\windows\redist\intel64_win\ipp")
        TestIPPPath(DB.MyIPPPath, "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp")
        IPP = New cIntelIPP(DB.MyIPPPath)
        cFITSReader.IPPPath = DB.MyIPPPath
        DD = New Ato.DragDrop(tbLogOutput, False)
        pgMain.SelectedObject = DB

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub OpenEXELocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenEXELocationToolStripMenuItem.Click
        Process.Start(MyPath)
    End Sub

    Private Sub Log(ByVal Text As String)
        Log(Text, False, True)
    End Sub

    Private Sub Log(ByVal Text As List(Of String))
        Log(Text.ToArray)
    End Sub

    Private Sub Log(ByVal Indent As String, ByVal Text() As String)
        For Each Line As String In Text
            Log(Indent & Line, False, False)
        Next Line
        UpdateLog()
    End Sub

    Private Sub Log(ByVal Text() As String)
        For Each Line As String In Text
            Log(Line, False, False)
        Next Line
        UpdateLog()
    End Sub

    Private Sub Log(ByVal Text As String, ByVal LogInStatus As Boolean, ByVal AutoUpdate As Boolean)
        Text = Format(Now, "HH.mm.ss:fff") & "|" & Text
        With LogContent
            If .Length = 0 Then
                .Append(Text)
            Else
                .Append(System.Environment.NewLine & Text)
            End If
            If AutoUpdate = True Then UpdateLog()
            If LogInStatus = True Then tsslMain.Text = Text
        End With
        DE()
    End Sub

    Private Sub UpdateLog()
        With tbLogOutput
            .Text = LogContent.ToString
            If .Text.Length > 0 Then
                .SelectionStart = .Text.Length - 1
                .SelectionLength = 0
                .ScrollToCaret()
            End If
        End With
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub DD_DropOccured(Files() As String) Handles DD.DropOccured
        'Handle drag-and-drop for all dropped FIT(s) files
        For Each File As String In Files
            If System.IO.Path.GetExtension(File).ToUpper.StartsWith(".FIT") Then LoadFile(File)
        Next File
    End Sub

    Private Sub WriteTestDataToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles WriteTestDataToolStripMenuItem1.Click
        cFITSWriter.WriteTestFile_Int8("FITS_BitPix8.FITS")
        cFITSWriter.WriteTestFile_Int16("FITS_BitPix16.FITS") ': Process.Start("FITS_BitPix16.FITS")
        cFITSWriter.WriteTestFile_Int32("FITS_BitPix32.FITS") ': Process.Start("FITS_BitPix32.FITS")
        cFITSWriter.WriteTestFile_Float32("FITS_BitPix32f.FITS") ': Process.Start("FITS_BitPix32f.FITS")
        cFITSWriter.WriteTestFile_Float64("FITS_BitPix64f.FITS") : Process.Start("FITS_BitPix64f.FITS")
        'MsgBox("OK")
    End Sub

    Private Sub tsmiOpenLastFile_Click(sender As Object, e As EventArgs) Handles tsmiOpenLastFile.Click
        If System.IO.File.Exists(LastFile) = True Then Process.Start(LastFile)
    End Sub

    Private Sub tsmiSaveMeanFile_Click(sender As Object, e As EventArgs) Handles tsmiSaveMeanFile.Click
        If StackingStatistics.LongLength > 0 Then
            Dim ImageData(StackingStatistics.GetUpperBound(0), StackingStatistics.GetUpperBound(1)) As Integer
            For Idx1 As Integer = 0 To StackingStatistics.GetUpperBound(0)
                For Idx2 As Integer = 0 To StackingStatistics.GetUpperBound(1)
                    ImageData(Idx1, Idx2) = CInt(StackingStatistics(Idx1, Idx2).Mean)
                Next Idx2
            Next Idx1
            Dim FileToGenerate As String = System.IO.Path.Combine(MyPath, "Stacking_Mean.fits")
            cFITSWriter.Write(FileToGenerate, ImageData, cFITSWriter.eBitPix.Int32)
            Process.Start(FileToGenerate)
        End If
    End Sub

    Private Sub StdDevImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StdDevImageToolStripMenuItem.Click
        If StackingStatistics.LongLength > 0 Then
            Dim ImageData(StackingStatistics.GetUpperBound(0), StackingStatistics.GetUpperBound(1)) As Integer
            For Idx1 As Integer = 0 To StackingStatistics.GetUpperBound(0)
                For Idx2 As Integer = 0 To StackingStatistics.GetUpperBound(1)
                    ImageData(Idx1, Idx2) = CInt(StackingStatistics(Idx1, Idx2).Sigma)
                Next Idx2
            Next Idx1
            Dim FileToGenerate As String = System.IO.Path.Combine(MyPath, "Stacking_StdDev.fits")
            cFITSWriter.Write(FileToGenerate, ImageData, cFITSWriter.eBitPix.Int32)
            Process.Start(FileToGenerate)
        End If
    End Sub

    Private Sub SumImageDoubleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SumImageDoubleToolStripMenuItem.Click
        If StackingStatistics.LongLength > 0 Then
            Dim ImageData(StackingStatistics.GetUpperBound(0), StackingStatistics.GetUpperBound(1)) As Double
            For Idx1 As Integer = 0 To StackingStatistics.GetUpperBound(0)
                For Idx2 As Integer = 0 To StackingStatistics.GetUpperBound(1)
                    ImageData(Idx1, Idx2) = CInt(StackingStatistics(Idx1, Idx2).Mean * StackingStatistics(Idx1, Idx2).ValueCount)
                Next Idx2
            Next Idx1
            Dim FileToGenerate As String = System.IO.Path.Combine(MyPath, "Stacking_Sum.fits")
            cFITSWriter.Write(FileToGenerate, ImageData, cFITSWriter.eBitPix.Double)
            Process.Start(FileToGenerate)
        End If
    End Sub

    Private Sub MaxMinInt32ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaxMinInt32ToolStripMenuItem.Click
        If StackingStatistics.LongLength > 0 Then
            Dim ImageData(StackingStatistics.GetUpperBound(0), StackingStatistics.GetUpperBound(1)) As Integer
            For Idx1 As Integer = 0 To StackingStatistics.GetUpperBound(0)
                For Idx2 As Integer = 0 To StackingStatistics.GetUpperBound(1)
                    ImageData(Idx1, Idx2) = CInt(StackingStatistics(Idx1, Idx2).MaxMin)
                Next Idx2
            Next Idx1
            Dim FileToGenerate As String = System.IO.Path.Combine(MyPath, "Stacking_MaxMin.fits")
            cFITSWriter.Write(FileToGenerate, ImageData, cFITSWriter.eBitPix.Int32)
            Process.Start(FileToGenerate)
        End If
    End Sub

    Private Sub RowAndColumnStatisticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RowAndColumnStatisticsToolStripMenuItem.Click
        Dim DataProcessed As Boolean = False
        '1. Load data
        If SingleStatCalc.DataProcessor_UInt16.ImageData.LongLength > 0 Then
            With SingleStatCalc.DataProcessor_UInt16
                ReDim StatPerRow(.ImageData.GetUpperBound(1)) : InitStat(StatPerRow)
                ReDim StatPerCol(.ImageData.GetUpperBound(0)) : InitStat(StatPerCol)
                For Idx1 As Integer = 0 To .ImageData.GetUpperBound(0)
                    For Idx2 As Integer = 0 To .ImageData.GetUpperBound(1)
                        StatPerRow(Idx2).AddValue(.ImageData(Idx1, Idx2))
                        StatPerCol(Idx1).AddValue(.ImageData(Idx1, Idx2))
                    Next Idx2
                Next Idx1
                DataProcessed = True
            End With
        End If
        If SingleStatCalc.DataProcessor_Int32.ImageData.LongLength > 0 Then
            With SingleStatCalc.DataProcessor_Int32
                ReDim StatPerRow(.ImageData.GetUpperBound(1)) : InitStat(StatPerRow)
                ReDim StatPerCol(.ImageData.GetUpperBound(0)) : InitStat(StatPerCol)
                For Idx1 As Integer = 0 To .ImageData.GetUpperBound(0)
                    For Idx2 As Integer = 0 To .ImageData.GetUpperBound(1)
                        StatPerRow(Idx1).AddValue(.ImageData(Idx1, Idx2))
                        StatPerCol(Idx2).AddValue(.ImageData(Idx1, Idx2))
                    Next Idx2
                Next Idx1
                DataProcessed = True
            End With
        End If
        '2. Plot data
        If DataProcessed = True Then
            PlotStatistics(LastFile & " - ROW STAT", StatPerRow)
            PlotStatistics(LastFile & " - COL STAT", StatPerCol)
        End If
    End Sub

    Private Sub InitStat(ByRef Vector() As Ato.cSingleValueStatistics)
        For Idx As Integer = 0 To Vector.GetUpperBound(0)
            Vector(Idx) = New Ato.cSingleValueStatistics(Ato.cSingleValueStatistics.eValueType.Linear)
        Next Idx
    End Sub

    Private Sub PlotStatisticsVsGainToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlotStatisticsVsGainToolStripMenuItem.Click
        PlotStatistics(LastFile, StatVsGain)
    End Sub

    Private Sub ReplotStatisticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReplotStatisticsToolStripMenuItem.Click
        PlotStatistics(LastFile, LastStat)
    End Sub

    Private Sub TranslateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AfiineTranslateToolStripMenuItem.Click
        IntelIPP_NewCode.Translate("C:\Users\albus\Dropbox\Astro\!Bilder\Test-Daten\Debayer\Stack_16bits_936frames_152s.fits")
    End Sub

    Private Sub StoreStatisticsEXCELFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StoreStatisticsEXCELFileToolStripMenuItem.Click

        Dim AddHisto As Boolean = True

        Using workbook As New ClosedXML.Excel.XLWorkbook

            Dim XY As New List(Of Object())

            '1.) Histogram
            If AddHisto = True Then
                For Each Key As Long In LastStat.MonochromHistogram.Keys
                    Dim Values As New List(Of Object)
                    Values.Add(Key)
                    Values.Add(LastStat.MonochromHistogram(Key))
                    If LastStat.BayerHistograms(0, 0).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms(0, 0)(Key)) Else Values.Add(String.Empty)
                    If LastStat.BayerHistograms(0, 1).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms(0, 1)(Key)) Else Values.Add(String.Empty)
                    If LastStat.BayerHistograms(1, 0).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms(1, 0)(Key)) Else Values.Add(String.Empty)
                    If LastStat.BayerHistograms(1, 1).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms(1, 1)(Key)) Else Values.Add(String.Empty)
                    XY.Add(Values.ToArray)
                Next Key
                Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram")
                worksheet.Cell(1, 1).InsertData(New List(Of String)({"Pixel value", "Count Mono", "Count Bayer_0_0", "Count Bayer_0_1", "Count Bayer_1_0", "Count Bayer_1_1"}), True)
                worksheet.Cell(2, 1).InsertData(XY)
                For Each col In worksheet.ColumnsUsed
                    col.AdjustToContents()
                Next col
            End If

            '2.) Row and column
            If IsNothing(StatPerRow) = False Then
                XY.Clear()
                For Idx As Integer = 0 To StatPerRow.GetUpperBound(0)
                    With StatPerRow(Idx)
                        XY.Add(New Object() {Idx + 1, .Minimum, .Mean, .Maximum, .Sigma})
                    End With
                Next Idx
                Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Row Statistics")
                worksheet.Cell(1, 1).InsertData(New List(Of String)({"Row #", "Min", "Mean", "Max", "Sigma"}), True)
                worksheet.Cell(2, 1).InsertData(XY)
                For Each col In worksheet.ColumnsUsed
                    col.AdjustToContents()
                Next col
            End If
            If IsNothing(StatPerCol) = False Then
                XY.Clear()
                For Idx As Integer = 0 To StatPerCol.GetUpperBound(0)
                    With StatPerCol(Idx)
                        XY.Add(New Object() {Idx + 1, .Minimum, .Mean, .Maximum, .Sigma})
                    End With
                Next Idx
                Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Column Statistics")
                worksheet.Cell(1, 1).InsertData(New List(Of String)({"Column #", "Min", "Mean", "Max", "Sigma"}), True)
                worksheet.Cell(2, 1).InsertData(XY)
                For Each col In worksheet.ColumnsUsed
                    col.AdjustToContents()
                Next col
            End If

            '3.) Save and open
            Dim FileToGenerate As String = IO.Path.Combine(MyPath, "AstroImageStatistics.xlsx")
            workbook.SaveAs(FileToGenerate)
            Process.Start(FileToGenerate)
        End Using

    End Sub

    Private Sub ResetStackingToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ResetStackingToolStripMenuItem1.Click
        StackingStatistics = Nothing
    End Sub

End Class
