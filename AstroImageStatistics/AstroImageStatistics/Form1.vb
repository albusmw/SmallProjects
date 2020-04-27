﻿Option Explicit On
Option Strict On

Public Class Form1

    Const UInt32One As UInt32 = 1

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
    '''<summary>Last FITS header.</summary>
    Private LastFITSHeader As cFITSHeaderParser
    '''<summary>Statistics of the last plot.</summary>
    Private LastStat As AstroNET.Statistics.sStatistics

    '''<summary>Statistics of all processed files.</summary>
    Private AllStat As New Dictionary(Of String, AstroNET.Statistics.sStatistics)
    '''<summary>Statistics of all processed files.</summary>
    Private AllHeaders As New Dictionary(Of String, Dictionary(Of eFITSKeywords, Object))

    '''<summary>Statistics processor (for the last file).</summary>
    Dim SingleStatCalc As AstroNET.Statistics

    '''<summary>Statistics for pixel with identical Y value.</summary>
    Dim StatPerRow() As Ato.cSingleValueStatistics
    '''<summary>Statistics for pixel with identical X value.</summary>
    Dim StatPerCol() As Ato.cSingleValueStatistics

    '''<summary>Storage for a simple stack processing.</summary>
    Private StackingStatistics(,) As Ato.cSingleValueStatistics

    Private Sub OpenFileToAnalyseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFileToAnalyseToolStripMenuItem.Click
        ofdMain.Filter = "FIT(s) files (*.fit?)|*.fit?"
        If ofdMain.ShowDialog <> DialogResult.OK Then Exit Sub
        For Each File As String In ofdMain.FileNames
            LoadFile(File)
        Next File
    End Sub

    Private Sub LoadFile(ByVal FileName As String)

        Dim FileNameOnly As String = System.IO.Path.GetFileName(FileName)
        Dim Stopper As New cStopper
        Dim FITSReader As New cFITSReader
        SingleStatCalc = New AstroNET.Statistics(IPP)

        Running()

        If DB.AutoClearLog = True Then
            LogContent.Clear()
            UpdateLog()
        End If

        '=========================================================================================================
        'Log header data and detect if NAXIS3 is set
        Log("Loading file <" & FileName & "> ...")
        Log("  -> <" & System.IO.Path.GetFileNameWithoutExtension(FileName) & ">")
        LastFile = FileName
        Log("FITS header:")
        LastFITSHeader = New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(FileName))
        Dim FITSHeaderDict As Dictionary(Of eFITSKeywords, Object) = LastFITSHeader.GetCardsAsDictionary
        Dim ContentToPrint As New List(Of String)
        For Each Entry As eFITSKeywords In FITSHeaderDict.Keys
            ContentToPrint.Add("  " & FITSKeyword.GetKeyword(Entry).PadRight(10) & "=" & CStr(FITSHeaderDict(Entry)).Trim.PadLeft(40))
        Next Entry
        Log(ContentToPrint)
        Log(New String("-"c, 107))

        '=========================================================================================================
        'Perform the read operation
        Dim UBound0 As Integer = -1
        Dim UBound1 As Integer = -1
        SingleStatCalc.ResetAllProcessors()
        Select Case LastFITSHeader.BitPix
            Case 8
                SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data = FITSReader.ReadInUInt8(FileName, DB.UseIPP)
                UBound0 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(1)
            Case 16
                SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data = FITSReader.ReadInUInt16(FileName, DB.UseIPP)
                If LastFITSHeader.NAXIS3 > 1 Then
                    For Idx As Integer = 1 To LastFITSHeader.NAXIS3 - 1
                        Dim NewDataStartIdx As Integer = FITSReader.DataStartIdx + CInt(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Length * 2)    'move to next plane
                        SingleStatCalc.DataProcessor_UInt16.ImageData(Idx).Data = FITSReader.ReadInUInt16(FileName, NewDataStartIdx, DB.UseIPP)
                    Next Idx
                End If
                UBound0 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(1)
            Case 32
                SingleStatCalc.DataProcessor_Int32.ImageData = FITSReader.ReadInInt32(FileName, DB.UseIPP)
                UBound0 = SingleStatCalc.DataProcessor_Int32.ImageData.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_Int32.ImageData.GetUpperBound(1)
            Case -32
                SingleStatCalc.DataProcessor_Float32.ImageData = FITSReader.ReadInFloat32(FileName, DB.UseIPP)
                UBound0 = SingleStatCalc.DataProcessor_Float32.ImageData.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_Float32.ImageData.GetUpperBound(1)
            Case Else
                Log("!!! File format <" & LastFITSHeader.BitPix.ToString.Trim & "> not yet supported!")
                Exit Sub
        End Select
        Stopper.Stamp(FileNameOnly & ": Reading")

        '=========================================================================================================
        'Calculate the statistics
        CalculateStatistics(SingleStatCalc.DataMode)
        Stopper.Stamp(FileNameOnly & ": Statistics")

        'Record statistics
        If AllStat.ContainsKey(FileName) = False Then
            AllStat.Add(FileName, LastStat)
        Else
            AllStat(FileName) = LastStat
        End If
        If AllHeaders.ContainsKey(FileName) = False Then
            AllHeaders.Add(FileName, FITSHeaderDict)
        Else
            AllHeaders(FileName) = FITSHeaderDict
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
                Select Case LastFITSHeader.BitPix
                    Case 8, 16
                        For Idx1 As Integer = 0 To UBound0
                            For Idx2 As Integer = 0 To UBound1
                                StackingStatistics(Idx1, Idx2).AddValue(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(Idx1, Idx2))
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

        '=========================================================================================================
        'Plot statistics and remember this file as last processed file
        If DB.AutoOpenStatGraph = True Then PlotStatistics(FileName, LastStat)
        'Me.Focus()

        Idle()

    End Sub

    Private Sub CalculateStatistics(ByVal DataMode As String)
        LastStat = SingleStatCalc.ImageStatistics(DataMode)
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
        Dim Disp As New cZEDGraphForm
        Disp.PlotData("Test", New Double() {1, 2, 3, 4})
        Select Case Stats.DataMode
            Case "Int"
                'Plot histogram
                Disp.Plotter.Clear()
                If IsNothing(Stats.BayerHistograms_Int) = False Then
                    Disp.Plotter.PlotXvsY("R[0,0]", Stats.BayerHistograms_Int(0, 0), 1, New cZEDGraphService.sGraphStyle(Color.Red, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY("G1[0,1]", Stats.BayerHistograms_Int(0, 1), 1, New cZEDGraphService.sGraphStyle(Color.LightGreen, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY("G2[1,0]", Stats.BayerHistograms_Int(1, 0), 1, New cZEDGraphService.sGraphStyle(Color.Green, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY("B[1,1]", Stats.BayerHistograms_Int(1, 1), 1, New cZEDGraphService.sGraphStyle(Color.Blue, DB.PlotStyle, 1))
                End If
                If IsNothing(Stats.MonochromHistogram_Int) = False Then
                    Disp.Plotter.PlotXvsY("Mono histo", Stats.MonochromHistogram_Int, 1, New cZEDGraphService.sGraphStyle(Color.Black, DB.PlotStyle, 1))
                End If
                Disp.Plotter.ManuallyScaleXAxis(Stats.MonoStatistics_Int.Min.Key, Stats.MonoStatistics_Int.Max.Key)
            Case "Single"
                'Plot histogram
                Disp.Plotter.Clear()
                If IsNothing(Stats.BayerHistograms_Float32) = False Then
                    Disp.Plotter.PlotXvsY("R[0,0]", Stats.BayerHistograms_Float32(0, 0), 1, New cZEDGraphService.sGraphStyle(Color.Red, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY("G1[0,1]", Stats.BayerHistograms_Float32(0, 1), 1, New cZEDGraphService.sGraphStyle(Color.LightGreen, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY("G2[1,0]", Stats.BayerHistograms_Float32(1, 0), 1, New cZEDGraphService.sGraphStyle(Color.Green, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY("B[1,1]", Stats.BayerHistograms_Float32(1, 1), 1, New cZEDGraphService.sGraphStyle(Color.Blue, DB.PlotStyle, 1))
                End If
                If IsNothing(Stats.MonochromHistogram_Float32) = False Then
                    Disp.Plotter.PlotXvsY("Mono histo", Stats.MonochromHistogram_Float32, 1, New cZEDGraphService.sGraphStyle(Color.Black, DB.PlotStyle, 1))
                End If
                Disp.Plotter.ManuallyScaleXAxis(Stats.MonoStatistics_Int.Min.Key, Stats.MonoStatistics_Int.Max.Key)
        End Select
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

    Private Sub PlotStatistics(ByVal FileName As String, ByRef Stats As Dictionary(Of Double, AstroImageStatistics.AstroNET.Statistics.sSingleChannelStatistics_Int))
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

        'Drop auswerten
        With My.Application
            If .CommandLineArgs.Count > 0 Then
                Dim FileName As String = .CommandLineArgs.Item(0)
                If System.IO.File.Exists(FileName) Then LoadFile(FileName)
            End If
        End With


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
        If StackedStatPresent() = True Then
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
        If StackedStatPresent() = True Then
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
        If StackedStatPresent() = True Then
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
        If StackedStatPresent() = True Then
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

    Private Function StackedStatPresent() As Boolean
        If IsNothing(StackingStatistics) = True Then Return False
        If StackingStatistics.LongLength = 0 Then Return False
        Return True
    End Function

    Private Sub RowAndColumnStatisticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RowAndColumnStatisticsToolStripMenuItem.Click

        Running()

        Dim DataProcessed As Boolean = False

        '1. Load data
        Select Case SingleStatCalc.DataMode
            Case "UInt16"
                With SingleStatCalc.DataProcessor_UInt16
                    ReDim StatPerRow(.ImageData(0).Data.GetUpperBound(1)) : InitStat(StatPerRow)
                    ReDim StatPerCol(.ImageData(0).Data.GetUpperBound(0)) : InitStat(StatPerCol)
                    Parallel.For(0, .ImageData(0).Data.GetUpperBound(0) + 1, Sub(Idx1)
                                                                                 For Idx2 As Integer = 0 To .ImageData(0).Data.GetUpperBound(1)
                                                                                     StatPerRow(Idx2).AddValue(.ImageData(0).Data(Idx1, Idx2))
                                                                                     StatPerCol(Idx1).AddValue(.ImageData(0).Data(Idx1, Idx2))
                                                                                 Next Idx2
                                                                             End Sub)
                    DataProcessed = True
                End With
            Case "Int32"
                With SingleStatCalc.DataProcessor_Int32
                    ReDim StatPerRow(.ImageData.GetUpperBound(1)) : InitStat(StatPerRow)
                    ReDim StatPerCol(.ImageData.GetUpperBound(0)) : InitStat(StatPerCol)
                    Parallel.For(0, .ImageData.GetUpperBound(0) + 1, Sub(Idx1)
                                                                         For Idx2 As Integer = 0 To .ImageData.GetUpperBound(1)
                                                                             StatPerRow(Idx2).AddValue(.ImageData(Idx1, Idx2))
                                                                             StatPerCol(Idx1).AddValue(.ImageData(Idx1, Idx2))
                                                                         Next Idx2
                                                                     End Sub)
                    DataProcessed = True
                End With
        End Select

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
                For Each Key As Long In LastStat.MonochromHistogram_Int.Keys
                    Dim Values As New List(Of Object)
                    Values.Add(Key)
                    Values.Add(LastStat.MonochromHistogram_Int(Key))
                    If LastStat.BayerHistograms_Int(0, 0).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms_Int(0, 0)(Key)) Else Values.Add(String.Empty)
                    If LastStat.BayerHistograms_Int(0, 1).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms_Int(0, 1)(Key)) Else Values.Add(String.Empty)
                    If LastStat.BayerHistograms_Int(1, 0).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms_Int(1, 0)(Key)) Else Values.Add(String.Empty)
                    If LastStat.BayerHistograms_Int(1, 1).ContainsKey(Key) Then Values.Add(LastStat.BayerHistograms_Int(1, 1)(Key)) Else Values.Add(String.Empty)
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
            For Each Key As UInteger In LastStat.MonoStatistics_Int.HistXDist.Keys
                HistDens.Add(New Object() {Key, LastStat.MonoStatistics_Int.HistXDist(Key)})
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
        If SingleStatCalc.DataProcessor_UInt16.ImageData(0).Length > 0 Then
            Dim ModusRef As Long = Long.MinValue
            For BayerIdx1 As Integer = 0 To 1
                For BayerIdx2 As Integer = 0 To 1
                    ModusRef = Math.Max(ModusRef, LastStat.BayerStatistics_Int(BayerIdx1, BayerIdx2).Modus.Key)
                Next BayerIdx2
            Next BayerIdx1
            For BayerIdx1 As Integer = 0 To 1
                For BayerIdx2 As Integer = 0 To 1
                    ClipCount(BayerIdx1, BayerIdx2) = 0
                    Dim Norm As Double = ModusRef / LastStat.BayerStatistics_Int(BayerIdx1, BayerIdx2).Modus.Key
                    If ModusRef <> LastStat.BayerStatistics_Int(BayerIdx1, BayerIdx2).Modus.Key Then                                                        'skip channels that do not need a change
                        For PixelIdx1 As Integer = BayerIdx1 To SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(0) Step 2
                            For PixelIdx2 As Integer = BayerIdx2 To SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(1) Step 2
                                Dim NewValue As Double = Math.Round(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(PixelIdx1, PixelIdx2) * Norm)
                                If NewValue > UInt16.MaxValue Then
                                    SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(PixelIdx1, PixelIdx2) = UInt16.MaxValue
                                    ClipCount(BayerIdx1, BayerIdx2) += 1
                                Else
                                    SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(PixelIdx1, PixelIdx2) = CUShort(NewValue)
                                End If
                            Next PixelIdx2
                        Next PixelIdx1
                    End If
                Next BayerIdx2
            Next BayerIdx1
        End If

        'Log
        For BayerIdx1 As Integer = 0 To 1
            For BayerIdx2 As Integer = 0 To 1
                Log("Clip count for channel [" & BayerIdx1.ValRegIndep & ":" & BayerIdx2.ValRegIndep & "]: " & ClipCount(BayerIdx1, BayerIdx2).ValRegIndep)
            Next BayerIdx2
        Next BayerIdx1

        CalculateStatistics(SingleStatCalc.DataMode)
        Idle()

    End Sub

    Private Sub tsmiSaveImageData_Click(sender As Object, e As EventArgs) Handles tsmiSaveImageData.Click
        'TODO: Save also non-UInt16 data
        With sfdMain
            .Filter = "FITS 16-bit fixed|*.fits|FITS 32-bit fixed|*.fits|FITS 32-bit float|*.fits|TIFF 16-bit|*.tif|JPG|*.jpg|PNG|*.png"
            If .ShowDialog = DialogResult.OK Then
                Running()
                With SingleStatCalc.DataProcessor_UInt16
                    Select Case sfdMain.FilterIndex
                        Case 1
                            'FITS 16 bit fixed
                            cFITSWriter.Write(sfdMain.FileName, .ImageData(0).Data, cFITSWriter.eBitPix.Int16, LastFITSHeader.GetCardsAsList)
                        Case 2
                            'FITS 32 bit fixed
                            cFITSWriter.Write(sfdMain.FileName, .ImageData(0).Data, cFITSWriter.eBitPix.Int32, LastFITSHeader.GetCardsAsList)
                        Case 3
                            'FITS 32 bit float
                            cFITSWriter.Write(sfdMain.FileName, .ImageData(0).Data, cFITSWriter.eBitPix.Single, LastFITSHeader.GetCardsAsList)
                        Case 4
                            'TIFF
                            If .ImageData.Count = 1 Then
                                ImageFileFormatSpecific.SaveTIFF_Format16bppGrayScale(sfdMain.FileName, .ImageData(0).Data)
                            Else
                                ImageFileFormatSpecific.SaveTIFF_Format48bppColor(sfdMain.FileName, .ImageData)
                            End If
                        Case 5
                            'JPG
                            Dim myEncoderParameters As New System.Drawing.Imaging.EncoderParameters(1)
                            myEncoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, DB.ImageQuality)
                            cLockBitmap.CalculateOutputBitmap(.ImageData(0).Data, LastStat.MonoStatistics_Int.Max.Key).BitmapToProcess.Save(sfdMain.FileName, GetEncoderInfo("image/jpeg"), myEncoderParameters)
                        Case 6
                            'PNG - try to do 8bit but only get a palett with 256 values but still 24-bit ...
                            Dim myEncoderParameters As New System.Drawing.Imaging.EncoderParameters(2)
                            myEncoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, DB.ImageQuality)
                            myEncoderParameters.Param(1) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 8)
                            cLockBitmap.CalculateOutputBitmap(.ImageData(0).Data, LastStat.MonoStatistics_Int.Max.Key).BitmapToProcess.Save(sfdMain.FileName, GetEncoderInfo("image/png"), myEncoderParameters)

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
        ImageProcessing.MakeHistoStraight(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)
        CalculateStatistics(SingleStatCalc.DataMode)
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

    Private Sub ADUQuantizationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ADUQuantizationToolStripMenuItem.Click
        Dim Disp As New cZEDGraphForm
        Dim PlotData As Generic.Dictionary(Of Long, UInt64) = AstroNET.Statistics.GetQuantizationHisto(LastStat.MonochromHistogram_Int)
        Dim XAxis As Double() = PlotData.KeyList.ToDouble
        Disp.PlotData("Test", New Double() {1, 2, 3, 4})
        'Plot data
        Disp.Plotter.Clear()
        Disp.Plotter.PlotXvsY("Mono", XAxis, PlotData.ValueList.ToArray.ToDouble, New cZEDGraphService.sGraphStyle(Color.Black, DB.PlotStyle, 1))
        Disp.Plotter.GridOnOff(True, True)
        Disp.Plotter.ManuallyScaleXAxis(XAxis(0), XAxis(XAxis.GetUpperBound(0)))
        Disp.Plotter.AutoScaleYAxisLog()
        Disp.Plotter.ForceUpdate()
        'Set style of the window
        Disp.Plotter.SetCaptions(String.Empty, "ADU step size", "# found")
        Disp.Plotter.MaximizePlotArea()
        Disp.Hoster.Icon = Me.Icon
    End Sub

    Private Sub tsmiPlateSolve_Click(sender As Object, e As EventArgs) Handles tsmiPlateSolve.Click

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
            If Entry.Keyword = eFITSKeywords.RA Then File_RA_JNow = CStr(Entry.Value).Trim("'"c).Trim.Trim("'"c)
            If Entry.Keyword = eFITSKeywords.DEC Then File_Dec_JNow = CStr(Entry.Value).Trim("'"c).Trim.Trim("'"c)
            If Entry.Keyword = eFITSKeywords.FOV1 Then File_FOV1 = CStr(Entry.Value).Trim
            If Entry.Keyword = eFITSKeywords.FOV2 Then File_FOV2 = CStr(Entry.Value).Trim
        Next Entry

        'Data from QHYCapture (10Micron) are in JNow, so convert to J2000 for PlateSolve
        Dim File_RA_J2000 As Double = Double.NaN
        Dim File_Dec_J2000 As Double = Double.NaN
        JNowToJ2000(AstroParser.ParseRA(File_RA_JNow), AstroParser.ParseDeclination(File_Dec_JNow), File_RA_J2000, File_Dec_J2000)

        'Run plate solve
        Dim FITSFile1 As String = ofdMain.FileName
        Dim ErrorCode1 As String = String.Empty
        Dim SolverIn_RA As String() = File_RA_JNow.Trim.Trim("'"c).Split(":"c)
        Dim SolverIn_Dec As String() = File_Dec_JNow.Trim.Trim("'"c).Split(":"c)
        cPlateSolve.PlateSolvePath = DB.PlateSolve2Path
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







        '----------------------------------------------------------------------------------------------------
        'Code taken from ASCOM_CamTest

        'Dim Solver As New cPlateSolve
        'Dim BasePath As String = "\\DS1819\astro\2019-12-05\IC405\23_57_38"

        'Dim SolverRawOut As String() = {}

        'Dim InitRA As Double = Double.NaN
        'Dim InitDec As Double = Double.NaN

        'For Each File As String In System.IO.Directory.GetFiles(BasePath, "*.fits")

        '    Log(">>> " & File)

        '    Dim Changer As New cFITSHeaderChanger
        '    Dim FITSHeader As New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(File))

        '    'On first run set assumed position, else set parameters of 1st run
        '    If Double.IsNaN(InitRA) = True Then Solver.SetRA(5, 16, 12) Else Solver.RA = InitRA
        '    If Double.IsNaN(InitDec) = True Then Solver.SetDec(34, 16, 0) Else Solver.Dec = InitDec

        '    'Set X and Y dimensions
        '    Solver.SetDimX(2541, 4.88, FITSHeader.Width)
        '    Solver.SetDimY(2541, 4.88, FITSHeader.Height)
        '    Solver.HoldOpenTime = 0

        '    'Solve
        '    Dim SolverStatus As String = Solver.Solve(File, SolverRawOut)
        '    If Double.IsNaN(InitRA) = True Then InitRA = Solver.SolvedRA
        '    If Double.IsNaN(InitDec) = True Then InitDec = Solver.SolvedDec
        '    Log(Solver.ErrorRA.ValRegIndep & " : " & Solver.ErrorDec.ValRegIndep)

        '    'Set keywords after solving as e.g. http://bf-astro.com/eXcalibrator/excalibrator.htm may need it
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CTYPE1, "'RA---SIN'"))
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CTYPE2, "'DEC--SIN'"))
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CRPIX1, 0.5 * (FITSHeader.Width + 1)))                            'pixels
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CRPIX2, 0.5 * (FITSHeader.Height + 1)))                           'pixels
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CDELT1, (Solver.DimX * Solver.RadToGrad) / FITSHeader.Width))     'degrees/pixel
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CDELT2, (Solver.DimY * Solver.RadToGrad) / FITSHeader.Height))    'degrees/pixel
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CROTA1, 0.0))                                                     'degrees
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CROTA2, 0.0))                                                     'degrees
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CRVAL1, Solver.SolvedRA * Solver.RadToGrad))                      'Right Ascension [degrees]
        '    FITSHeader.Add(New cFITSHeaderParser.sHeaderElement(eFITSKeywords.CRVAL2, Solver.SolvedDec * Solver.RadToGrad))                     'Declination [degrees]

        '    'Store new header elements
        '    Changer.ChangeHeader(File, File & "_SOLVED.fits", FITSHeader.GetCardsAsDictionary)

        'Next File





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
        Dim Vignette As New Dictionary(Of Double, Double)
        If DB.VigResolution = 0 Then
            Select Case SingleStatCalc.DataMode
                Case "UInt16"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)
                Case "UInt32"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData)
            End Select
        Else
            Select Case SingleStatCalc.DataMode
                Case "UInt16"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, DB.VigResolution)
                Case "UInt32"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData, DB.VigResolution)
            End Select
        End If
        Vignette = Vignette.SortDictionary
        Log(Stopper.Stamp("Vignette"))

        'Display the vignette
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Vignette", Vignette)

    End Sub

    Private Sub tsmiCorrectVignette_Click(sender As Object, e As EventArgs) Handles tsmiCorrectVignette.Click

        Running()

        'Calculate the vignette
        Dim Stopper As New cStopper
        Stopper.Start()
        Dim Vignette As New Dictionary(Of Double, Double)
        If DB.VigResolution = 0 Then
            Select Case SingleStatCalc.DataMode
                Case "UInt16"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)
                Case "UInt32"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData)
            End Select
        Else
            Select Case SingleStatCalc.DataMode
                Case "UInt16"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, DB.VigResolution)
                Case "UInt32"
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData, DB.VigResolution)
            End Select
        End If
        Vignette = Vignette.SortDictionary
        Log(Stopper.Stamp("Vignette"))

        'Display the vignette
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Vignette", Vignette)

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
        Select Case SingleStatCalc.DataMode
            Case "UInt16"
                ImageProcessing.CorrectVignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, VignetteCorrection)
            Case "UInt32"
                ImageProcessing.CorrectVignette(SingleStatCalc.DataProcessor_UInt32.ImageData, VignetteCorrection)
        End Select

        Stopper.Stamp("Vignette correction")

        CalculateStatistics(SingleStatCalc.DataMode)
        Idle()

    End Sub

    Private Sub FITSGrepToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FITSGrepToolStripMenuItem.Click
        Dim X As New frmFITSGrep : X.Show()
    End Sub

    Private Sub ASCOMDynamicallyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ASCOMDynamicallyToolStripMenuItem.Click
        'Working but NOVAS31 does not ...
        Dim Astrometry As Object = System.Reflection.Assembly.Load("ASCOM.Astrometry").CreateInstance("ASCOM.Astrometry.Transform.Transform")
        Dim dynamicType As Type = Astrometry.GetType
        Dim dynamicObject As Object = Activator.CreateInstance(dynamicType)
        Dim NOVAS31 As Object = System.Reflection.Assembly.Load("ASCOM.Astrometry").CreateInstance("ASCOM.Astrometry.NOVAS.NOVAS31")
        Dim JulianDate As Double = CDbl(dynamicType.InvokeMember("JulianDate", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.InvokeMethod, Type.DefaultBinder, NOVAS31, New Object() {CShort(Now.Year), CShort(Now.Month), CShort(Now.Day), Now.Hour}))
        dynamicType.InvokeMember("JulianDateUTC", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty, Type.DefaultBinder, Astrometry, New Object() {(New ASCOM.Astrometry.NOVAS.NOVAS31).JulianDate(CShort(Now.Year), CShort(Now.Month), CShort(Now.Day), Now.Hour)})
        dynamicType.InvokeMember("SetApparent", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.InvokeMethod, Type.DefaultBinder, Astrometry, New Object() {CDbl(1.0), CDbl(2.0)})
        Dim J2000RA As Double = CDbl(dynamicType.GetProperty("RAJ2000").GetValue(Astrometry))
        Dim DecJ2000 As Double = CDbl(dynamicType.GetProperty("DecJ2000").GetValue(Astrometry))
    End Sub

    Private Sub ClearStatisticsMemoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearStatisticsMemoryToolStripMenuItem.Click
        AllStat.Clear()
    End Sub

    Private Sub FocusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FocusToolStripMenuItem.Click
        Dim Plot_X As New List(Of Double)
        Dim Plot_Y As New List(Of Double)
        For Each FileName As String In AllStat.Keys
            Dim FocusPos As String = System.IO.Path.GetFileNameWithoutExtension(FileName).Split("_"c)(1)
            Plot_X.Add(Val(FocusPos))
            Plot_Y.Add(Val(AllStat(FileName).MonoStatistics_Int.Max.Key))
        Next FileName
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Focus - MAX VALUE", Plot_X.ToArray, Plot_Y.ToArray)
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F1 Then
            Dim RTFTextBox As New cRTFTextBox
            Dim AllResources As String() = System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceNames
            For Each Entry As String In AllResources
                If Entry.EndsWith(".HelpContent.rtf") Then
                    RTFTextBox.ShowText(New System.IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream(Entry)).ReadToEnd.Trim)
                    Exit For
                End If
            Next Entry
            e.Handled = True
        Else
            e.Handled = False
        End If
    End Sub

    Private Sub HotPixelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HotPixelToolStripMenuItem.Click
        Dim FixedPixelCount As UInt32 = 0
        Dim HotPixelLimit As Double = 5
        Running()
        With SingleStatCalc.DataProcessor_UInt16.ImageData(0)
            For Idx1 As Integer = 1 To .NAXIS1 - 2
                For Idx2 As Integer = 1 To .NAXIS2 - 2
                    Dim SurSum As New Ato.cSingleValueStatistics(Ato.cSingleValueStatistics.eValueType.Linear) : SurSum.StoreRawValues = True
                    SurSum.AddValue(.Data(Idx1 - 1, Idx2 - 1))
                    SurSum.AddValue(.Data(Idx1 - 1, Idx2))
                    SurSum.AddValue(.Data(Idx1 - 1, Idx2 - +1))
                    SurSum.AddValue(.Data(Idx1, Idx2 - 1))
                    SurSum.AddValue(.Data(Idx1, Idx2 + 1))
                    SurSum.AddValue(.Data(Idx1 + 1, Idx2 - 1))
                    SurSum.AddValue(.Data(Idx1 + 1, Idx2))
                    SurSum.AddValue(.Data(Idx1 + 1, Idx2 + 1))
                    If .Data(Idx1, Idx2) > HotPixelLimit * SurSum.Percentile(50) Then
                        .Data(Idx1, Idx2) = CUShort(SurSum.Percentile(50))
                        FixedPixelCount += UInt32One
                    End If
                Next Idx2
            Next Idx1
        End With
        Log("Fixed " & FixedPixelCount.ValRegIndep & " pixel")
        Idle()
    End Sub

    Private Sub FITSTestFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FITSTestFilesToolStripMenuItem.Click
        cFITSWriter.WriteTestFile_UInt16_Cross(System.IO.Path.Combine(MyPath, "UInt16_Cross_mono.fits"))
        cFITSWriter.WriteTestFile_UInt16_Cross_RGB(System.IO.Path.Combine(MyPath, "UInt16_Cross_rgb.fits"))
        cFITSWriter.WriteTestFile_UInt16_XYIdent(System.IO.Path.Combine(MyPath, "UInt16_XYIdent.fits"))
        Process.Start(MyPath)
    End Sub

    Private Sub MultifileAreaCompareToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MultifileAreaCompareToolStripMenuItem.Click

        'Read the same segment from all files and compose a new combined image

        Dim TileSize As Integer = 100                       'size for 1 tile
        Dim OffsetX As Integer = 9029 - (TileSize \ 2)      'X offset start position
        Dim OffsetY As Integer = 2943 - (TileSize \ 2)      'Y offset start position

        Dim BaseDirectory As String = System.IO.Path.GetDirectoryName(LastFile)
        Dim FITSReader As New cFITSReader

        Dim AllFiles As New List(Of String)(System.IO.Directory.GetFiles(BaseDirectory, "QHY600_L*.fits"))

        Dim MosaikWidth As Integer = CInt(Math.Ceiling(Math.Sqrt(AllFiles.Count)))              'Number of tiles in X direction
        Dim MosaikHeight As Integer = CInt(Math.Ceiling(AllFiles.Count / MosaikWidth))          'Number of tiles in Y direction
        Dim Mosaik(MosaikWidth * TileSize + (MosaikWidth - 1) - 1, MosaikHeight * TileSize + (MosaikHeight - 1) - 1) As UInt16

        Dim WidthPtr As Integer = 0 : Dim WidthIdx As Integer = 0
        Dim HeightPtr As Integer = 0
        For Each File As String In AllFiles
            Dim Data(,) As UInt16 = FITSReader.ReadInUInt16(File, DB.UseIPP, OffsetX, TileSize, OffsetY, TileSize)
            For X As Integer = 0 To TileSize - 1
                For Y As Integer = 0 To TileSize - 1
                    Mosaik(WidthPtr + X, HeightPtr + Y) = Data(X, Y)
                Next Y
            Next X
            WidthPtr += TileSize + 1 : WidthIdx += 1
            If WidthIdx >= MosaikWidth Then
                HeightPtr += TileSize + 1
                WidthPtr = 0
                WidthIdx = 0
            End If
        Next File

        Dim FileToGenerate As String = System.IO.Path.Combine(MyPath, "Mosaik.fits")
        cFITSWriter.Write(FileToGenerate, Mosaik, cFITSWriter.eBitPix.Int16)
        Process.Start(FileToGenerate)

    End Sub

End Class
