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

        Running()

        If DB.AutoClearLog = True Then
            LogContent.Clear()
            UpdateLog
        End If

        'Log header data and detect if NAXIS3 is set
        Log("Loading file <" & FileName & "> ...")
        Log("  -> <" & System.IO.Path.GetFileNameWithoutExtension(FileName) & ">")
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
            Case -32
                Log("Special mode - trying to get fixed point ...")
                Dim Data(,) As Single = FITSReader.ReadInFloat32(FileName, False)
                'Calculate the histogramm
                Dim ADUHistogramm As New Dictionary(Of Single, UInt32)
                For Idx1 As Integer = 0 To Data.GetUpperBound(0)
                    For Idx2 As Integer = 0 To Data.GetUpperBound(1)
                        If ADUHistogramm.ContainsKey(Data(Idx1, Idx2)) = True Then
                            ADUHistogramm(Data(Idx1, Idx2)) = ADUHistogramm(Data(Idx1, Idx2)) + CType(1, UInt32)
                        Else
                            ADUHistogramm.Add(Data(Idx1, Idx2), 1)
                        End If
                    Next Idx2
                Next Idx1
                ADUHistogramm = cGenerics.SortDictionary(ADUHistogramm)
                Dim Result As Collections.Generic.Dictionary(Of Single, UInt32) = AstroNET.Statistics.GetQuantizationHisto(ADUHistogramm)
                Dim QuantError As Single = 0
                For Each Entry As Single In ADUHistogramm.Keys
                    QuantError += Math.Abs(CSng(Math.Round(Entry * 10, 0)) - (Entry * 10))
                Next Entry
                QuantError /= Result.Count
                If QuantError = 0 Then
                    ReDim SingleStatCalc.DataProcessor_Int32.ImageData(Data.GetUpperBound(0), Data.GetUpperBound(1))
                    For Idx1 As Integer = 0 To Data.GetUpperBound(0)
                        For Idx2 As Integer = 0 To Data.GetUpperBound(1)
                            SingleStatCalc.DataProcessor_Int32.ImageData(Idx1, Idx2) = CInt(Data(Idx1, Idx2) * 10)
                        Next Idx2
                    Next Idx1
                    SingleStatCalc.DataProcessor_UInt16.ImageData = {{}}
                End If
                MsgBox(ADUHistogramm.Count & " ADU values found!")
            Case Else
                Log("!!! File format <" & FITSHeader.BitPix.ToString.Trim & "> not yet supported!")
                Exit Sub
        End Select
        Stopper.Stamp(FileNameOnly & ": Reading")

        'Calculate the statistics
        CalculateStatistics()
        Stopper.Stamp(FileNameOnly & ": Statistics")

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

        Idle()

    End Sub

    Private Sub CalculateStatistics()
        LastStat = SingleStatCalc.ImageStatistics
        Log("Statistics:")
        Log("  ", LastStat.StatisticsReport.ToArray())
        Log(New String("="c, 109))
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
        Disp.PlotData("Test", New Double() {1, 2, 3, 4})
        'Plot histogram
        Disp.Plotter.Clear()
        If IsNothing(Stats.BayerHistograms) = False Then
            Disp.Plotter.PlotXvsY("R", Stats.BayerHistograms(0, 0), 1, New cZEDGraphService.sGraphStyle(Color.Red, DB.PlotStyle, 1))
            Disp.Plotter.PlotXvsY("G1", Stats.BayerHistograms(0, 1), 1, New cZEDGraphService.sGraphStyle(Color.LightGreen, DB.PlotStyle, 1))
            Disp.Plotter.PlotXvsY("G2", Stats.BayerHistograms(1, 0), 1, New cZEDGraphService.sGraphStyle(Color.DarkGreen, DB.PlotStyle, 1))
            Disp.Plotter.PlotXvsY("B", Stats.BayerHistograms(1, 1), 1, New cZEDGraphService.sGraphStyle(Color.Blue, DB.PlotStyle, 1))
        End If
        If IsNothing(Stats.MonochromHistogram) = False Then
            Disp.Plotter.PlotXvsY("Mono histo", Stats.MonochromHistogram, 1, New cZEDGraphService.sGraphStyle(Color.Black, DB.PlotStyle, 1))
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
        Disp.PlotData("Test", New Double() {1, 2, 3, 4})
        'Plot data
        Dim XAxis() As Double = Ato.cSingleValueStatistics.GetAspectVectorXAxis(Stats)
        Disp.Plotter.Clear()
        Disp.Plotter.PlotXvsY("Mean", XAxis, Ato.cSingleValueStatistics.GetAspectVector(Stats, Ato.cSingleValueStatistics.eAspects.Mean), New cZEDGraphService.sGraphStyle(Color.Black, DB.PlotStyle, 1))
        Disp.Plotter.PlotXvsY("Max", XAxis, Ato.cSingleValueStatistics.GetAspectVector(Stats, Ato.cSingleValueStatistics.eAspects.Maximum), New cZEDGraphService.sGraphStyle(Color.Red, DB.PlotStyle, 1))
        Disp.Plotter.PlotXvsY("Min", XAxis, Ato.cSingleValueStatistics.GetAspectVector(Stats, Ato.cSingleValueStatistics.eAspects.Minimum), New cZEDGraphService.sGraphStyle(Color.Green, DB.PlotStyle, 1))
        Disp.Plotter.PlotXvsY("Sigma", XAxis, Ato.cSingleValueStatistics.GetAspectVector(Stats, Ato.cSingleValueStatistics.eAspects.Sigma), New cZEDGraphService.sGraphStyle(Color.Orange, DB.PlotStyle, 1), True)
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
        Disp.PlotData("Test", New Double() {1, 2, 3, 4})
        'Plot data
        Dim XAxis As New List(Of Double)
        Dim YAxis As New List(Of Double)
        For Each Entry As Double In Stats.Keys
            XAxis.Add(Entry)
            YAxis.Add(Stats(Entry).Mean)
        Next Entry
        Disp.Plotter.Clear()
        Disp.Plotter.PlotXvsY("StdDev", XAxis.ToArray, YAxis.ToArray, New cZEDGraphService.sGraphStyle(Color.Black, cZEDGraphService.eCurveMode.Dots, 1))
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
        Running()
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
        Idle()
    End Sub

    Private Sub InitStat(ByRef Vector() As Ato.cSingleValueStatistics)
        For Idx As Integer = 0 To Vector.GetUpperBound(0)
            Vector(Idx) = New Ato.cSingleValueStatistics(Ato.cSingleValueStatistics.eValueType.Linear)
        Next Idx
    End Sub

    Private Sub PlotStatisticsVsGainToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlotStatisticsVsGainToolStripMenuItem.Click
        Running()
        PlotStatistics(LastFile, StatVsGain)
        Idle()
    End Sub

    Private Sub ReplotStatisticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReplotStatisticsToolStripMenuItem.Click
        Running()
        PlotStatistics(LastFile, LastStat)
        Idle()
    End Sub

    Private Sub TranslateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AfiineTranslateToolStripMenuItem.Click
        IntelIPP_NewCode.Translate("C:\Users\albus\Dropbox\Astro\!Bilder\Test-Daten\Debayer\Stack_16bits_936frames_152s.fits")
    End Sub

    Private Sub StoreStatisticsEXCELFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StoreStatisticsEXCELFileToolStripMenuItem.Click

        Dim AddHisto As Boolean = True

        With sfdMain
            .Filter = "EXCEL file (*.xlsx)|*.xlsx"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With

        Using workbook As New ClosedXML.Excel.XLWorkbook

            '1.) Histogram
            If AddHisto = True Then
                Dim XY As New List(Of Object())
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

            '2.) Histo density
            Dim HistDens As New List(Of Object())
            For Each Key As UInteger In LastStat.MonoStatistics.HistXDist.Keys
                HistDens.Add(New Object() {Key, LastStat.MonoStatistics.HistXDist(Key)})
            Next Key
            Dim worksheet2 As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram Density")
            worksheet2.Cell(1, 1).InsertData(New List(Of String)({"Step size", "Count"}), True)
            worksheet2.Cell(2, 1).InsertData(HistDens)
            For Each col In worksheet2.ColumnsUsed
                col.AdjustToContents()
            Next col

            '3.) Row and column
            If IsNothing(StatPerRow) = False Then
                Dim XY As New List(Of Object())
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
                Dim XY As New List(Of Object())
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

            '4) Save and open
            Dim FileToGenerate As String = IO.Path.Combine(MyPath, sfdMain.FileName)
            workbook.SaveAs(FileToGenerate)
            Process.Start(FileToGenerate)

        End Using

    End Sub

    Private Sub ResetStackingToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ResetStackingToolStripMenuItem1.Click
        StackingStatistics = Nothing
    End Sub

    Private Sub AdjustRGBChannelsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdjustRGBChannelsToolStripMenuItem.Click
        'Calculate the maximum modus (the most propable value in the channel) and normalize all channels to this channel
        Running()
        Dim ClipCount(1, 1) As Integer
        If SingleStatCalc.DataProcessor_UInt16.ImageData.LongLength > 0 Then
            Dim ModusRef As Long = Long.MinValue
            For BayerIdx1 As Integer = 0 To 1
                For BayerIdx2 As Integer = 0 To 1
                    ModusRef = Math.Max(ModusRef, LastStat.BayerStatistics(BayerIdx1, BayerIdx2).Modus.Key)
                Next BayerIdx2
            Next BayerIdx1
            For BayerIdx1 As Integer = 0 To 1
                For BayerIdx2 As Integer = 0 To 1
                    ClipCount(BayerIdx1, BayerIdx2) = 0
                    Dim Norm As Double = ModusRef / LastStat.BayerStatistics(BayerIdx1, BayerIdx2).Modus.Key
                    If ModusRef <> LastStat.BayerStatistics(BayerIdx1, BayerIdx2).Modus.Key Then
                        For PixelIdx1 As Integer = BayerIdx1 To SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(0) Step 2
                            For PixelIdx2 As Integer = BayerIdx2 To SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(1) Step 2
                                Dim NewValue As Double = Math.Round(SingleStatCalc.DataProcessor_UInt16.ImageData(PixelIdx1, PixelIdx2) * Norm)
                                If NewValue > UInt16.MaxValue Then
                                    SingleStatCalc.DataProcessor_UInt16.ImageData(PixelIdx1, PixelIdx2) = UInt16.MaxValue
                                    ClipCount(BayerIdx1, BayerIdx2) += 1
                                Else
                                    SingleStatCalc.DataProcessor_UInt16.ImageData(PixelIdx1, PixelIdx2) = CUShort(NewValue)
                                End If
                            Next PixelIdx2
                        Next PixelIdx1
                    End If
                Next BayerIdx2
            Next BayerIdx1
        End If
        For BayerIdx1 As Integer = 0 To 1
            For BayerIdx2 As Integer = 0 To 1
                Log("Clip count for channel [" & BayerIdx1.ValRegIndep & "] : " & BayerIdx2.ValRegIndep & ": " & ClipCount(BayerIdx1, BayerIdx2).ValRegIndep)
            Next BayerIdx2
        Next BayerIdx1
        CalculateStatistics()
        Idle()
    End Sub

    Private Sub tsmiSaveImageData_Click(sender As Object, e As EventArgs) Handles tsmiSaveImageData.Click
        'TODO: Save also non-UInt16 data
        With sfdMain
            .Filter = "FITS 16-bit fixed|*.fits|FITS 32-bit fixed|*.fits|FITS 32-bit float|*.fits|TIFF|*.tif|JPG|*.jpg|PNG|*.png"
            If .ShowDialog = DialogResult.OK Then
                Running()
                With SingleStatCalc.DataProcessor_UInt16
                    Select Case sfdMain.FilterIndex
                        Case 1
                            'FITS 16 bit fixed
                            cFITSWriter.Write(sfdMain.FileName, .ImageData, cFITSWriter.eBitPix.Int16)
                        Case 2
                            'FITS 32 bit fixed
                            cFITSWriter.Write(sfdMain.FileName, .ImageData, cFITSWriter.eBitPix.Int32)
                        Case 3
                            'FITS 32 bit float
                            cFITSWriter.Write(sfdMain.FileName, .ImageData, cFITSWriter.eBitPix.Single)
                        Case 4
                            'TIFF
                            Dim stream As New IO.FileStream(sfdMain.FileName, IO.FileMode.Create)
                            Dim encoder As New System.Windows.Media.Imaging.TiffBitmapEncoder()
                            encoder.Compression = Windows.Media.Imaging.TiffCompressOption.Zip
                            encoder.Frames.Add(Windows.Media.Imaging.BitmapFrame.Create(New System.IO.MemoryStream(cLockBitmap.CalculateOutputBitmap(.ImageData, LastStat.MonoStatistics.Max.Key).Pixels)))
                            encoder.Save(stream)
                        Case 5
                            'JPG
                            Dim myEncoderParameters As New System.Drawing.Imaging.EncoderParameters(1)
                            myEncoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, DB.ImageQuality)
                            cLockBitmap.CalculateOutputBitmap(.ImageData, LastStat.MonoStatistics.Max.Key).BitmapToProcess.Save(sfdMain.FileName, GetEncoderInfo("image/jpeg"), myEncoderParameters)
                        Case 6
                            'PNG - try to do 8bit but only get a palett with 256 values but still 24-bit ...
                            Dim myEncoderParameters As New System.Drawing.Imaging.EncoderParameters(2)
                            myEncoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, DB.ImageQuality)
                            myEncoderParameters.Param(1) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 8)
                            cLockBitmap.CalculateOutputBitmap(.ImageData, LastStat.MonoStatistics.Max.Key).BitmapToProcess.Save(sfdMain.FileName, GetEncoderInfo("image/png"), myEncoderParameters)

                            Dim X As New System.Windows.Media.Imaging.PngBitmapEncoder

                    End Select
                End With

                Idle()
            End If
        End With
    End Sub

    '''<summary>Get an encoder by its MIME name</summary>
    '''<param name="mimeType"></param>
    '''<returns></returns>
    Private Function GetEncoderInfo(ByVal mimeType As String) As Imaging.ImageCodecInfo
        Dim RetVal As Imaging.ImageCodecInfo = Nothing
        Dim AllEncoder As New List(Of String)
        For Each Encoder As Imaging.ImageCodecInfo In Imaging.ImageCodecInfo.GetImageEncoders
            If Encoder.MimeType = mimeType Then RetVal = Encoder
            AllEncoder.Add(Encoder.MimeType)
        Next Encoder
        Return RetVal
    End Function

    Private Sub StretcherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StretcherToolStripMenuItem.Click
        Running()
        ImageProcessing.MakeHistoStraight(SingleStatCalc.DataProcessor_UInt16.ImageData)
        CalculateStatistics()
        Idle()
    End Sub

    '''<summary>Processing running.</summary>
    Private Sub Running()
        tsslRunning.ForeColor = Drawing.Color.Red
        DE()
    End Sub

    '''<summary>Processing idle.</summary>
    Private Sub Idle()
        tsslRunning.ForeColor = Drawing.Color.Silver
        DE()
    End Sub

    Private Sub BitGrayscaleFileGenerationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BitGrayscaleFileGenerationToolStripMenuItem.Click
        Dim FileName As String = System.IO.Path.Combine(MyPath, "Test_16bpp_gray.png")
        ImageFileFormatSpecific.Test48BPP(FileName)
        Process.Start("C:\Program Files\IrfanView\i_view64.exe", FileName)
    End Sub

    Private Sub ADUQuantizationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ADUQuantizationToolStripMenuItem.Click
        Dim Disp As New cZEDGraphForm
        Dim PlotData As Generic.Dictionary(Of Long, UInt32) = AstroNET.Statistics.GetQuantizationHisto(LastStat.MonochromHistogram)
        Dim XAxis As Double() = PlotData.KeyList.ToDouble
        Disp.PlotData("Test", New Double() {1, 2, 3, 4})
        'Plot data
        Disp.Plotter.Clear()
        Disp.Plotter.PlotXvsY("Mono", XAxis, PlotData.ValueList.ToDouble, New cZEDGraphService.sGraphStyle(Color.Black, DB.PlotStyle, 1))
        Disp.Plotter.GridOnOff(True, True)
        Disp.Plotter.ManuallyScaleXAxis(XAxis(0), XAxis(XAxis.GetUpperBound(0)))
        Disp.Plotter.AutoScaleYAxisLog()
        Disp.Plotter.ForceUpdate()
        'Set style of the window
        Disp.Plotter.SetCaptions(String.Empty, "ADU step size", "# found")
        Disp.Plotter.MaximizePlotArea()
        Disp.Hoster.Icon = Me.Icon
    End Sub

    Private Sub SolveImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SolveImageToolStripMenuItem.Click

        Dim Solver As New cPlateSolve
        Dim FileToRun As String = LastFile
        Dim Binning As Integer = 1

        'Get the FITS header information
        Dim X As List(Of cFITSHeaderParser.sHeaderElement) = cFITSHeaderChanger.ReadHeader(FileToRun)
        Dim File_RA_JNow As String = String.Empty
        Dim File_Dec_JNow As String = String.Empty
        Dim File_FOV1 As String = String.Empty
        Dim File_FOV2 As String = String.Empty
        For Each Entry As cFITSHeaderParser.sHeaderElement In X
            If Entry.Keyword.Trim.ToUpper = FITSKeyword.GetKeyword(eFITSKeywords.RA) Then File_RA_JNow = Entry.Value.Trim("'"c).Trim.Trim("'"c)
            If Entry.Keyword.Trim.ToUpper = FITSKeyword.GetKeyword(eFITSKeywords.DEC) Then File_Dec_JNow = Entry.Value.Trim("'"c).Trim.Trim("'"c)
            If Entry.Keyword.Trim.ToUpper = FITSKeyword.GetKeyword(eFITSKeywords.FOV1) Then File_FOV1 = Entry.Value.Trim
            If Entry.Keyword.Trim.ToUpper = FITSKeyword.GetKeyword(eFITSKeywords.FOV2) Then File_FOV2 = Entry.Value.Trim
        Next Entry

        'Data from QHYCapture are in JNow, so convert to J2000 for PlateSolve
        Dim File_RA_J2000 As Double = Double.NaN
        Dim File_Dec_J2000 As Double = Double.NaN
        JNowToJ2000(AstroParser.ParseRA(File_RA_JNow), AstroParser.ParseDeclination(File_Dec_JNow), File_RA_J2000, File_Dec_J2000)

        'Run plate solve
        Dim FITSFile1 As String = ofdMain.FileName
        Dim ErrorCode1 As String = String.Empty
        Dim SolverIn_RA As String() = File_RA_JNow.Trim.Trim("'"c).Split(":"c)
        Dim SolverIn_Dec As String() = File_Dec_JNow.Trim.Trim("'"c).Split(":"c)
        cPlateSolve.PlateSolvePath = "C:\Bin\PlateSolve2\PlateSolve2.exe"
        With Solver
            .SetRA(CInt(SolverIn_RA(0)), CInt(SolverIn_RA(1)), Val(SolverIn_RA(2)))                     'theoretical position (Wikipedia, J2000.0)
            .SetDec(CInt(SolverIn_Dec(0)), CInt(SolverIn_Dec(1)), Val(SolverIn_Dec(2)))                 'theoretical position (Wikipedia, J2000.0)
            .SetDimX(Val(File_FOV1) * 60)                                                   'constant for system [telescope-camera]
            .SetDimY(Val(File_FOV2) * 60)                                                   'constant for system [telescope-camera]
            .HoldOpenTime = 100
            Dim RawOut As String() = {}
            ErrorCode1 = .Solve(LastFile, RawOut)
        End With

        'Convert
        Dim RadToH As Double = 12 / Math.PI
        Dim RadToGrad As Double = (180 / Math.PI)
        Dim JNow_RA_solved As Double = Double.NaN
        Dim JNow_Dec_solved As Double = Double.NaN
        J2000ToJNow(Solver.SolvedRA * RadToH, Solver.SolvedDec * RadToGrad, JNow_RA_solved, JNow_Dec_solved)

        Dim Output As New List(Of String)
        Output.Add("Start with   RA <" & File_RA_JNow & ">, DEC <" & File_Dec_JNow & "> (JNow)")
        Output.Add("             RA <" & Ato.AstroCalc.FormatHMS(File_RA_J2000) & ">, DEC <" & Ato.AstroCalc.Format360Degree(File_Dec_J2000) & "> (J2000)")
        Output.Add("Solved as    RA <" & Ato.AstroCalc.FormatHMS(Solver.SolvedRA * RadToH) & ">, DEC <" & Ato.AstroCalc.Format360Degree(Solver.SolvedDec * RadToGrad) & "> (J2000)")
        Output.Add("Converted to RA <" & Ato.AstroCalc.FormatHMS(JNow_RA_solved) & ">, DEC <" & Ato.AstroCalc.Format360Degree(JNow_Dec_solved) & "> (JNow)")

        MsgBox(Join(Output.ToArray, System.Environment.NewLine))


    End Sub

    '''<summary>Convert JNow to J2000 epoch.</summary>
    '''<seealso cref="https://ascom-standards.org/Help/Developer/html/T_ASCOM_Astrometry_Transform_Transform.htm"/>
    Public Shared Sub JNowToJ2000(ByVal JNowRA As Double, ByVal JNowDec As Double, ByRef J2000RA As Double, ByRef J2000Dec As Double)
        Dim X As New ASCOM.Astrometry.Transform.Transform
        X.JulianDateUTC = (New ASCOM.Astrometry.NOVAS.NOVAS31).JulianDate(CShort(Now.Year), CShort(Now.Month), CShort(Now.Day), Now.Hour)
        X.SetApparent(JNowRA, JNowDec)
        J2000RA = X.RAJ2000
        J2000Dec = X.DecJ2000
    End Sub

    '''<summary>Convert J2000 to JNow epoch.</summary>
    '''<seealso cref="https://ascom-standards.org/Help/Developer/html/T_ASCOM_Astrometry_Transform_Transform.htm"/>
    Public Shared Function J2000ToJNow(ByVal J2000RA As Double, ByVal J2000Dec As Double, ByRef JNowRA As Double, ByRef JNowDec As Double) As Short
        Dim X As New ASCOM.Astrometry.Transform.Transform
        X.JulianDateUTC = (New ASCOM.Astrometry.NOVAS.NOVAS31).JulianDate(CShort(Now.Year), CShort(Now.Month), CShort(Now.Day), Now.Hour)
        X.SetJ2000(J2000RA, J2000Dec)
        JNowRA = X.RAApparent
        JNowDec = X.DECApparent
    End Function

    Private Sub tsmiCalcVignette_Click(sender As Object, e As EventArgs) Handles tsmiCalcVignette.Click

        'Calculate the vignette
        Dim Stopper As New cStopper
        Stopper.Start()
        Dim Vignette As Dictionary(Of Double, Double) = cGenerics.SortDictionary(AstroNET.Statistics.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData, 1000))
        Log(Stopper.Stamp("Vignette"))

        'Display the vignette
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Vignette", cGenerics.SortDictionary(Vignette))

    End Sub

    Private Sub tsmiCorrectVignette_Click(sender As Object, e As EventArgs) Handles tsmiCorrectVignette.Click

        Running()

        'Calculate the vignette
        Dim Stopper As New cStopper
        Stopper.Start()
        Dim Vignette As Dictionary(Of Double, Double) = cGenerics.SortDictionary(AstroNET.Statistics.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData, 1000))
        Log(Stopper.Stamp("Vignette"))

        'Display the vignette
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Vignette", cGenerics.SortDictionary(Vignette))

        'Calculate the fitting
        Dim Polynomial() As Double = {}
        SignalProcessing.RegressPoly(Vignette.KeyList.ToArray, Vignette.ValueList.ToArray, 8, Polynomial)
        Dim Vignette_Y_Fit As Double() = SignalProcessing.ApplyPoly(Vignette.KeyList.ToArray, Polynomial)
        Disp1.PlotData("Fitting", Vignette.KeyList.ToArray, Vignette_Y_Fit)
        IPP.DivC(Vignette_Y_Fit, IPP.Max(Vignette_Y_Fit))                                                       'Norm to maximum
        Dim NormMin As Double = IPP.Min(Vignette_Y_Fit)
        IPP.DivC(Vignette_Y_Fit, NormMin)
        Dim VignetteCorrection As New Dictionary(Of Double, Double)
        Dim YPtr As Integer = 0
        For Each Entry As Double In Vignette.KeyList
            VignetteCorrection.Add(Entry, Vignette_Y_Fit(YPtr))
            YPtr += 1
        Next Entry
        Stopper.Stamp("Vignette fitting")

        'Correct the vignette
        AstroNET.Statistics.CorrectVignette(SingleStatCalc.DataProcessor_UInt16.ImageData, VignetteCorrection)
        Stopper.Stamp("Vignette correction")

        CalculateStatistics()
        Idle()

    End Sub

End Class
