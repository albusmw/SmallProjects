Option Explicit On
Option Strict On

Public Class Form1

    Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal pDst As IntPtr,
                                                                 ByVal pSrc As IntPtr,
                                                                 ByVal ByteLen As Long)

    Private LastOpenedFiles As New frmLastOpenedFiles

    Const UInt32One As UInt32 = 1

    Private LogContent As New System.Text.StringBuilder

    Private DB As New cDB

    '''<summary>Location of the EXE.</summary>
    Private MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
    '''<summary>Drag-and-drop handler.</summary>
    Private WithEvents DD As Ato.DragDrop

    '''<summary>Last file opened.</summary>
    Private LastFile As String = String.Empty
    '''<summary>Last FITS header.</summary>
    Private LastFITSHeader As cFITSHeaderParser
    '''<summary>Statistics of the last plot.</summary>
    Public LastStat As AstroNET.Statistics.sStatistics

    '''<summary>Statistics of all processed files.</summary>
    Private AllStat As New Dictionary(Of String, AstroNET.Statistics.sStatistics)
    '''<summary>Statistics of all processed files.</summary>
    Private AllHeaders As New Dictionary(Of String, Dictionary(Of eFITSKeywords, Object))

    '''<summary>Statistics processor (for the last file).</summary>
    Public SingleStatCalc As AstroNET.Statistics

    '''<summary>Statistics for pixel with identical Y value.</summary>
    Dim StatPerRow() As Ato.cSingleValueStatistics
    '''<summary>Statistics for pixel with identical X value.</summary>
    Dim StatPerCol() As Ato.cSingleValueStatistics

    '''<summary>Storage for a simple stack processing.</summary>
    Private StackingStatistics(,) As Ato.cSingleValueStatistics

    Private Sub OpenFileToAnalyseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFileToAnalyseToolStripMenuItem.Click
        ofdMain.Filter = "FIT(s) files (FIT/FITS/FTS)|*.FIT;*.FITS;*.FTS"
        If ofdMain.ShowDialog <> DialogResult.OK Then Exit Sub
        For Each File As String In ofdMain.FileNames
            LoadFile(File)
        Next File
    End Sub

    Private Sub LoadFile(ByVal FileName As String)

        Dim FileNameOnly As String = System.IO.Path.GetFileName(FileName)
        Dim Stopper As New cStopper
        Dim FITSReader As New cFITSReader
        Dim DataStartPos As Integer = 0

        SingleStatCalc = New AstroNET.Statistics(DB.IPP)

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
        LastFITSHeader = New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(FileName, DataStartPos))
        Dim FITSHeaderDict As Dictionary(Of eFITSKeywords, Object) = LastFITSHeader.GetCardsAsDictionary
        Dim ContentToPrint As New List(Of String)
        For Each Entry As eFITSKeywords In FITSHeaderDict.Keys
            ContentToPrint.Add("  " & FITSKeyword.GetKeyword(Entry)(0).PadRight(10) & "=" & CStr(FITSHeaderDict(Entry)).Trim.PadLeft(40))
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
                With SingleStatCalc.DataProcessor_UInt16
                    .ImageData(0).Data = FITSReader.ReadInUInt16(FileName, DB.UseIPP, DB.ForceDirect)
                    If LastFITSHeader.NAXIS3 > 1 Then
                        For Idx As Integer = 1 To LastFITSHeader.NAXIS3 - 1
                            DataStartPos += CInt(.ImageData(Idx - 1).Length * LastFITSHeader.BytesPerSample)        'move to next plane
                            .ImageData(Idx).Data = FITSReader.ReadInUInt16(FileName, DataStartPos, DB.UseIPP, DB.ForceDirect)
                        Next Idx
                    End If
                    UBound0 = .ImageData(0).Data.GetUpperBound(0)
                    UBound1 = .ImageData(0).Data.GetUpperBound(1)
                End With
            Case 32
                SingleStatCalc.DataProcessor_Int32.ImageData = FITSReader.ReadInInt32(FileName, DB.UseIPP)
                UBound0 = SingleStatCalc.DataProcessor_Int32.ImageData.GetUpperBound(0)
                UBound1 = SingleStatCalc.DataProcessor_Int32.ImageData.GetUpperBound(1)
            Case -32
                With SingleStatCalc.DataProcessor_Float32
                    .ImageData(0).Data = FITSReader.ReadInFloat32(FileName, DB.UseIPP)
                    If LastFITSHeader.NAXIS3 > 1 Then
                        For Idx As Integer = 1 To LastFITSHeader.NAXIS3 - 1
                            DataStartPos += CInt(.ImageData(Idx - 1).Length * LastFITSHeader.BytesPerSample)        'move to next plane
                            .ImageData(Idx).Data = FITSReader.ReadInFloat32(FileName, DB.UseIPP)
                        Next Idx
                    End If
                    UBound0 = .ImageData(0).Data.GetUpperBound(0)
                    UBound1 = .ImageData(0).Data.GetUpperBound(1)
                End With
            Case Else
                Log("!!! File format <" & LastFITSHeader.BitPix.ToString.Trim & "> not yet supported!")
                Exit Sub
        End Select
        Stopper.Stamp(FileNameOnly & ": Reading")

        '=========================================================================================================
        'Calculate the statistics
        CalculateStatistics(SingleStatCalc.DataFixFloat, DB.BayerPatternNames)
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

    Private Sub CalculateStatistics(ByVal DataMode As AstroNET.Statistics.sStatistics.eDataMode, ByVal ChannelNames As List(Of String))
        LastStat = SingleStatCalc.ImageStatistics(DataMode)
        Log("Statistics:")
        Log("  ", LastStat.StatisticsReport(False, ChannelNames).ToArray())
        Log(New String("="c, 109))
    End Sub

    '''<summary>A point of the graph was selected - give information.</summary>
    Public Function PointValueHandler(ByVal Curve As String, ByVal X As Double, ByVal Y As Double) As String
        Dim Text As New List(Of String)
        Dim Indent As String = "   "
        Text.Add("Curve <" & Curve & ">")
        Text.Add("X <" & X.ValRegIndep & ">")
        Text.Add("Y <" & Y.ValRegIndep & ">")
        Dim HistKey As Long = CLng(X)
        If Curve.Contains("[") = True Then
            Dim HIdx0 As Integer = CInt(Curve.Substring(Curve.IndexOf("[") + 1, 1))
            Dim HIdx1 As Integer = CInt(Curve.Substring(Curve.IndexOf("[") + 3, 1))
        End If
        With LastStat
            'Get the histogram data from mono histo
            Text.Add("MONO:")
            If .MonochromHistogram_Int.ContainsKey(HistKey) Then
                Dim ValuesAbove As UInt64 = .MonochromHistogram_Int_ValuesAbove(HistKey)
                Text.Add(Indent & (100 * (X / .MonoStatistics_Int.Max.Key)).ValRegIndep("0.00") & " % of MAX")
                Text.Add(Indent & (100 * (X / UInt16.MaxValue)).ValRegIndep("0.00") & " % of UInt16")
                Text.Add(Indent & ValuesAbove.ValRegIndep & " values are greater")
                Text.Add(Indent & (100 * (ValuesAbove / .MonoStatistics_Int.Samples)).ValRegIndep("0.00") & " % are greater")
            End If
            'Get the histogram data from Text "G1[1,0]"
            For HIdx0 As Integer = 0 To 1
                For HIdx1 As Integer = 0 To 1
                    Text.Add("BAYER[" & HIdx0.ValRegIndep & ":" & HIdx1.ValRegIndep & "]:")
                    If .BayerHistograms_Int_Present(HIdx0, HIdx1, HistKey) Then
                        Dim ValuesAbove As UInt64 = .BayerHistograms_Int_ValuesAbove(HIdx0, HIdx1, HistKey)
                        Text.Add(Indent & (100 * (X / .BayerStatistics_Int(HIdx0, HIdx1).Max.Key)).ValRegIndep("0.00") & " % of MAX")                'percentage of maximum value
                        Text.Add(Indent & (100 * (X / UInt16.MaxValue)).ValRegIndep("0.00") & " % of UInt16")                                        'percentage of UInt16 range
                        Text.Add(Indent & ValuesAbove.ValRegIndep & " values are greater")
                        Text.Add(Indent & (100 * (ValuesAbove / .BayerStatistics_Int(HIdx0, HIdx1).Samples)).ValRegIndep("0.00") & " % are greater")
                    Else
                        Text.Add(Indent & "-----")
                        Text.Add(Indent & "-----")
                        Text.Add(Indent & "-----")
                        Text.Add(Indent & "-----")
                    End If
                Next HIdx1
            Next HIdx0
        End With
        tbDetails.Text = Join(Text.ToArray, System.Environment.NewLine)
        Return Curve
    End Function

    '''<summary>Open a simple form with a ZEDGraph on it and plots the statistical data.</summary>
    '''<param name="FileName">Filename that is plotted (indicated in the header).</param>
    '''<param name="Stats">Statistics data to plot.</param>
    Private Sub PlotStatistics(ByVal FileName As String, ByRef Stats As AstroNET.Statistics.sStatistics)
        Dim Disp As New cZEDGraphForm
        AddHandler Disp.PointValueHandler, AddressOf PointValueHandler
        Disp.PlotData("Test", New Double() {1, 2, 3, 4}, Color.Red)
        Select Case Stats.DataMode
            Case AstroNET.Statistics.sStatistics.eDataMode.Fixed
                'Plot histogram
                Disp.Plotter.Clear()
                If IsNothing(Stats.BayerHistograms_Int) = False Then
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(0) & "[0,0]", Stats.BayerHistograms_Int(0, 0), 1, New cZEDGraphService.sGraphStyle(Color.Red, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(1) & "[0,1]", Stats.BayerHistograms_Int(0, 1), 1, New cZEDGraphService.sGraphStyle(Color.LightGreen, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(2) & "[1,0]", Stats.BayerHistograms_Int(1, 0), 1, New cZEDGraphService.sGraphStyle(Color.Green, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(3) & "[1,1]", Stats.BayerHistograms_Int(1, 1), 1, New cZEDGraphService.sGraphStyle(Color.Blue, DB.PlotStyle, 1))
                End If
                If IsNothing(Stats.MonochromHistogram_Int) = False Then
                    Disp.Plotter.PlotXvsY("Mono histo", Stats.MonochromHistogram_Int, 1, New cZEDGraphService.sGraphStyle(Color.Black, DB.PlotStyle, 1))
                End If
                Disp.Plotter.ManuallyScaleXAxis(Stats.MonoStatistics_Int.Min.Key, Stats.MonoStatistics_Int.Max.Key)
            Case AstroNET.Statistics.sStatistics.eDataMode.Float
                'Plot histogram
                Disp.Plotter.Clear()
                If IsNothing(Stats.BayerHistograms_Float32) = False Then
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(0) & "[0,0]", Stats.BayerHistograms_Float32(0, 0), 1, New cZEDGraphService.sGraphStyle(Color.Red, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(1) & "[0,1]", Stats.BayerHistograms_Float32(0, 1), 1, New cZEDGraphService.sGraphStyle(Color.LightGreen, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(2) & "[1,0]", Stats.BayerHistograms_Float32(1, 0), 1, New cZEDGraphService.sGraphStyle(Color.Green, DB.PlotStyle, 1))
                    Disp.Plotter.PlotXvsY(DB.BayerPatternName(3) & "[1,1]", Stats.BayerHistograms_Float32(1, 1), 1, New cZEDGraphService.sGraphStyle(Color.Blue, DB.PlotStyle, 1))
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
        Disp.PlotData("Test", New Double() {1, 2, 3, 4}, Color.Red)
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
        Disp.PlotData("Test", New Double() {1, 2, 3, 4}, Color.Red)
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

        'Load IPP
        Dim IPPLoadError As String = String.Empty
        Dim IPPPathToUse As String = cIntelIPP.SearchDLLToUse(cIntelIPP.PossiblePaths(MyPath).ToArray, IPPLoadError)
        If String.IsNullOrEmpty(IPPLoadError) = True Then
            DB.IPP = New cIntelIPP(IPPPathToUse)
            cFITSWriter.UseIPPForWriting = True
        Else
            cFITSWriter.UseIPPForWriting = False
        End If
        cFITSWriter.IPPPath = DB.IPP.IPPPath
        cFITSReader.IPPPath = DB.IPP.IPPPath

        DD = New Ato.DragDrop(tbLogOutput, False)
        pgMain.SelectedObject = DB

        'If a file is droped to the EXE (icon), use this as filename
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
        If System.IO.File.Exists(LastFile) = True Then
            Try
                Process.Start(LastFile)
            Catch ex As Exception
                Log("Error opening <" & LastFile & ">: <" & ex.Message & ">")
            End Try
        End If
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
            Case AstroNET.Statistics.eDataMode.UInt16
                With SingleStatCalc.DataProcessor_UInt16
                    ReDim StatPerRow(.ImageData(0).Data.GetUpperBound(1)) : InitStat(StatPerRow)
                    ReDim StatPerCol(.ImageData(0).Data.GetUpperBound(0)) : InitStat(StatPerCol)
                    For Idx1 As Integer = 0 To .ImageData(0).Data.GetUpperBound(0)
                        For Idx2 As Integer = 0 To .ImageData(0).Data.GetUpperBound(1)
                            StatPerRow(Idx2).AddValue(.ImageData(0).Data(Idx1, Idx2))
                            StatPerCol(Idx1).AddValue(.ImageData(0).Data(Idx1, Idx2))
                        Next Idx2
                    Next Idx1
                    DataProcessed = True
                End With
            Case AstroNET.Statistics.eDataMode.UInt32
                With SingleStatCalc.DataProcessor_UInt32
                    ReDim StatPerRow(.ImageData(0).Data.GetUpperBound(1)) : InitStat(StatPerRow)
                    ReDim StatPerCol(.ImageData(0).Data.GetUpperBound(0)) : InitStat(StatPerCol)
                    For Idx1 As Integer = 0 To .ImageData(0).Data.GetUpperBound(0)
                        For Idx2 As Integer = 0 To .ImageData(0).Data.GetUpperBound(1)
                            StatPerRow(Idx2).AddValue(.ImageData(0).Data(Idx1, Idx2))
                            StatPerCol(Idx1).AddValue(.ImageData(0).Data(Idx1, Idx2))
                        Next Idx2
                    Next Idx1
                    DataProcessed = True
                End With
            Case AstroNET.Statistics.eDataMode.Int32
                With SingleStatCalc.DataProcessor_Int32
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
                        If Idx = 492 Then
                            Console.WriteLine("!!!")
                        End If
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

    Private Sub tsmiAdjustRGB_Click(sender As Object, e As EventArgs) Handles tsmiAdjustRGB.Click

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

        CalculateStatistics(SingleStatCalc.DataFixFloat, DB.BayerPatternNames)
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
                            cLockBitmap.GetGrayscaleImage(.ImageData(0).Data, LastStat.MonoStatistics_Int.Max.Key).BitmapToProcess.Save(sfdMain.FileName, GetEncoderInfo("image/jpeg"), myEncoderParameters)
                        Case 6
                            'PNG - try to do 8bit but only get a palett with 256 values but still 24-bit ...
                            Dim myEncoderParameters As New System.Drawing.Imaging.EncoderParameters(2)
                            myEncoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, DB.ImageQuality)
                            myEncoderParameters.Param(1) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 8)
                            cLockBitmap.GetGrayscaleImage(.ImageData(0).Data, LastStat.MonoStatistics_Int.Max.Key).BitmapToProcess.Save(sfdMain.FileName, GetEncoderInfo("image/png"), myEncoderParameters)

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

    Private Sub tsmiStretch_Click(sender As Object, e As EventArgs) Handles tsmiStretch.Click
        Running()
        ImageProcessing.MakeHistoStraight(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)
        CalculateStatistics(SingleStatCalc.DataFixFloat, DB.BayerPatternNames)
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
        Disp.PlotData("Test", New Double() {1, 2, 3, 4}, Color.Red)
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
        Dim DataStartPos As Integer = -1
        Dim X As List(Of cFITSHeaderParser.sHeaderElement) = cFITSHeaderChanger.ReadHeader(FileToRun, DataStartPos)
        Dim File_RA_JNow As String = Nothing
        Dim File_Dec_JNow As String = Nothing
        Dim File_FOV1 As Object = Nothing
        Dim File_FOV2 As Object = Nothing
        For Each Entry As cFITSHeaderParser.sHeaderElement In X
            If Entry.Keyword = eFITSKeywords.RA Then File_RA_JNow = CStr(Entry.Value).Trim("'"c).Trim.Trim("'"c)
            If Entry.Keyword = eFITSKeywords.DEC Then File_Dec_JNow = CStr(Entry.Value).Trim("'"c).Trim.Trim("'"c)
            If Entry.Keyword = eFITSKeywords.FOV1 Then File_FOV1 = Entry.Value
            If Entry.Keyword = eFITSKeywords.FOV2 Then File_FOV2 = Entry.Value
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
            .SetRA(File_RA_J2000)                                                                       'theoretical position (Wikipedia, J2000.0)
            .SetDec(File_Dec_J2000)                                                                     'theoretical position (Wikipedia, J2000.0)
            .SetDimX(Val(File_FOV1) * cPlateSolve.DegToMin)                                             'constant for system [telescope-camera]
            .SetDimY(Val(File_FOV2) * cPlateSolve.DegToMin)                                             'constant for system [telescope-camera]
            .HoldOpenTime = DB.PlateSolve2HoldOpen
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
        Output.Add("Start with   RA <" & File_RA_JNow & ">, DEC <" & File_Dec_JNow & "> (JNow file string)")
        Output.Add("             RA <" & Ato.AstroCalc.FormatHMS(File_RA_J2000) & ">, DEC <" & Ato.AstroCalc.Format360Degree(File_Dec_J2000) & "> (J2000)")
        Output.Add("Solved as    RA <" & Ato.AstroCalc.FormatHMS(Solver.SolvedRA * RadToH) & ">, DEC <" & Ato.AstroCalc.Format360Degree(Solver.SolvedDec * RadToGrad) & "> (J2000)")
        Output.Add("Converted to RA <" & Ato.AstroCalc.FormatHMS(JNow_RA_solved) & ">, DEC <" & Ato.AstroCalc.Format360Degree(JNow_Dec_solved) & "> (JNow)")
        Output.Add("Angle           <" & Solver.RotationAngle.ValRegIndep & ">")
        Output.Add("Error        RA <" & Solver.ErrorRA.ValRegIndep & " "">, DEC < " & Solver.ErrorDec.ValRegIndep & " "">")
        Output.Add("             RA <" & Solver.PixelErrorRA.ValRegIndep & " pixel>, DEC < " & Solver.PixelErrorDec.ValRegIndep & " pixel>")

        Log("PLATE SOLVE: > ", Output.ToArray)

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
    '''<param name="JNowRA">RA in apparent co-ordinates [hours].</param>
    '''<param name="JNowDec">DEC in apparent co-ordinates [deg].</param>
    '''<param name="J2000RA">J2000 Right Ascension [hours].</param>
    '''<param name="J2000Dec">J2000 Declination [deg].</param>
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
    Public Shared Sub J2000ToJNow(ByVal J2000RA As Double, ByVal J2000Dec As Double, ByRef JNowRA As Double, ByRef JNowDec As Double)
        Dim X As New ASCOM.Astrometry.Transform.Transform
        X.JulianDateUTC = (New ASCOM.Astrometry.NOVAS.NOVAS31).JulianDate(CShort(Now.Year), CShort(Now.Month), CShort(Now.Day), Now.Hour)
        X.SetJ2000(J2000RA, J2000Dec)
        JNowRA = X.RAApparent
        JNowDec = X.DECApparent
    End Sub

    Private Sub tsmiCalcVignette_Click(sender As Object, e As EventArgs) Handles tsmiCalcVignette.Click

        'Calculate the vignette
        Dim Stopper As New cStopper
        Stopper.Start()
        Dim Vignette As New Dictionary(Of Double, Double)
        If DB.VigResolution = 0 Then
            Select Case SingleStatCalc.DataMode
                Case AstroNET.Statistics.eDataMode.UInt16
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)
                Case AstroNET.Statistics.eDataMode.UInt32
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data)
            End Select
        Else
            Select Case SingleStatCalc.DataMode
                Case AstroNET.Statistics.eDataMode.UInt16
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, DB.VigResolution)
                Case AstroNET.Statistics.eDataMode.UInt32
                    Vignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, DB.VigResolution)
            End Select
        End If
        Vignette = Vignette.SortDictionary
        Log(Stopper.Stamp("Vignette"))

        'Display the vignette
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Vignette", Vignette, Color.Red)

    End Sub

    Private Sub tsmiCorrectVignette_Click(sender As Object, e As EventArgs) Handles tsmiCorrectVignette.Click

        Running()

        'Calculate the vignette
        Dim Stopper As New cStopper
        Stopper.Start()
        Dim FullVignette As New Dictionary(Of Double, Double)
        If DB.VigResolution = 0 Then
            Select Case SingleStatCalc.DataMode
                Case AstroNET.Statistics.eDataMode.UInt16
                    FullVignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)
                Case AstroNET.Statistics.eDataMode.UInt32
                    FullVignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data)
            End Select
        Else
            Select Case SingleStatCalc.DataMode
                Case AstroNET.Statistics.eDataMode.UInt16
                    FullVignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, DB.VigResolution)
                Case AstroNET.Statistics.eDataMode.UInt32
                    FullVignette = ImageProcessing.Vignette(SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, DB.VigResolution)
            End Select
        End If

        FullVignette = FullVignette.SortDictionary
        Log(Stopper.Stamp("Vignette"))

        'Display the vignette graph
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Vignette", FullVignette, Color.Red)

        'Get only relevant data - ring around the center
        Dim Vignette As New Dictionary(Of Double, Double)
        For Each Entry As Double In FullVignette.Keys
            If Entry >= DB.VigStartDistance And Entry <= DB.VigStopDistance Then
                Vignette.Add(Entry, FullVignette(Entry))
            End If
        Next Entry

        'Calculate the fitting
        Dim Polynomial() As Double = {}
        SignalProcessing.RegressPoly(Vignette, DB.VigPolyOrder, Polynomial)
        Dim Vignette_Y_Fit As Double() = SignalProcessing.ApplyPoly(Vignette.KeyList.ToArray, Polynomial)
        Disp1.PlotData("Fitting", Vignette.KeyList.ToArray, Vignette_Y_Fit, Color.Green)
        DB.IPP.DivC(Vignette_Y_Fit, DB.IPP.Max(Vignette_Y_Fit))                                                       'Norm to maximum
        Dim NormMin As Double = DB.IPP.Min(Vignette_Y_Fit)
        DB.IPP.DivC(Vignette_Y_Fit, NormMin)
        Dim VignetteCorrection As New Dictionary(Of Double, Double)
        Dim YPtr As Integer = 0
        For Each Entry As Double In Vignette.KeyList
            VignetteCorrection.Add(Entry, Vignette_Y_Fit(YPtr))
            YPtr += 1
        Next Entry
        Stopper.Stamp("Vignette fitting")

        'Correct the vignette
        Select Case SingleStatCalc.DataMode
            Case AstroNET.Statistics.eDataMode.UInt16
                ImageProcessing.CorrectVignette(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, VignetteCorrection)
            Case AstroNET.Statistics.eDataMode.UInt32
                ImageProcessing.CorrectVignette(SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, VignetteCorrection)
        End Select

        Stopper.Stamp("Vignette correction")

        CalculateStatistics(SingleStatCalc.DataFixFloat, DB.BayerPatternNames)
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
        Dim Disp1 As New cZEDGraphForm : Disp1.PlotData("Focus - MAX VALUE", Plot_X.ToArray, Plot_Y.ToArray, Color.Red)
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
        'For each pixel take the area around and check if the value is significantly too high
        Dim FixedPixelCount As UInt32 = 0
        Dim HotPixelLimit As Double = 5
        Running()
        With SingleStatCalc.DataProcessor_UInt16.ImageData(0)
            For Idx1 As Integer = 1 To .NAXIS1 - 2
                For Idx2 As Integer = 1 To .NAXIS2 - 2
                    Dim SurSum As New Ato.cSingleValueStatistics(Ato.cSingleValueStatistics.eValueType.Linear) : SurSum.StoreRawValues = True
                    SurSum.AddValue(.Data(Idx1 - 1, Idx2 - 1))
                    SurSum.AddValue(.Data(Idx1 - 1, Idx2))
                    SurSum.AddValue(.Data(Idx1 - 1, Idx2 + 1))
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

        'Find top 1% of values and create a dictionary for the values and all pixel with this value
        Dim TopVal As New Dictionary(Of UInt16, List(Of Point))
        Dim ValueAbove As UInt16 = CUShort(LastStat.MonoStatistics_Int.Percentile(99))
        With SingleStatCalc.DataProcessor_UInt16.ImageData(0)
            For Idx1 As Integer = 0 To .NAXIS1 - 1
                For Idx2 As Integer = 0 To .NAXIS2 - 1
                    If .Data(Idx1, Idx2) >= ValueAbove Then
                        If TopVal.ContainsKey(.Data(Idx1, Idx2)) = False Then
                            TopVal.Add(.Data(Idx1, Idx2), New List(Of Point)({New Point(Idx1, Idx2)}))
                        Else
                            TopVal(.Data(Idx1, Idx2)).Add(New Point(Idx1, Idx2))
                        End If
                    End If
                Next Idx2
            Next Idx1
        End With
        TopVal = TopVal.SortDictionaryInverse

        'Filter values that have a high surrounding
        Dim TopValFiltered As New Dictionary(Of UInt16, List(Of Point))
        With SingleStatCalc.DataProcessor_UInt16.ImageData(0)
            For Each Value As UInt16 In TopVal.Keys
                For Each Pixel As Point In TopVal(Value)
                    Dim Idx1 As Integer = Pixel.X
                    Dim Idx2 As Integer = Pixel.Y
                    Dim SurSum As New Ato.cSingleValueStatistics(Ato.cSingleValueStatistics.eValueType.Linear) : SurSum.StoreRawValues = True
                    SurSum.AddValue(.Data(Idx1, Idx2))
                    If Idx1 > 0 And Idx2 > 0 Then SurSum.AddValue(.Data(Idx1 - 1, Idx2 - 1))
                    If Idx1 > 0 Then SurSum.AddValue(.Data(Idx1 - 1, Idx2))
                    If Idx1 > 0 And Idx2 < .Data.GetUpperBound(1) Then SurSum.AddValue(.Data(Idx1 - 1, Idx2 + 1))
                    If Idx2 > 0 Then SurSum.AddValue(.Data(Idx1, Idx2 - 1))
                    If Idx2 < .Data.GetUpperBound(1) Then SurSum.AddValue(.Data(Idx1, Idx2 + 1))
                    If Idx1 < .Data.GetUpperBound(0) And Idx2 > 0 Then SurSum.AddValue(.Data(Idx1 + 1, Idx2 - 1))
                    If Idx1 < .Data.GetUpperBound(0) Then SurSum.AddValue(.Data(Idx1 + 1, Idx2))
                    If Idx1 < .Data.GetUpperBound(0) And Idx2 < .Data.GetUpperBound(1) Then SurSum.AddValue(.Data(Idx1 + 1, Idx2 + 1))
                    Dim Mean As UInt16 = CType(SurSum.Mean, UInt16)
                    If TopValFiltered.ContainsKey(Mean) = False Then
                        TopValFiltered.Add(Mean, New List(Of Point)({Pixel}))
                    Else
                        TopValFiltered(Mean).Add(Pixel)
                    End If
                Next Pixel
            Next Value
        End With
        TopValFiltered = TopValFiltered.SortDictionaryInverse

        Dim Navigator As New frmNavigator
        Navigator.IPP = DB.IPP
        Navigator.tbRootFile.Text = LastFile
        Navigator.lbPixel.Items.Clear()
        If TopValFiltered.Count > 0 Then
            For Each Pixel As Point In TopValFiltered(TopValFiltered.KeyList(0))
                Navigator.lbPixel.Items.Add(Pixel.X.ToString.Trim & ":" & Pixel.Y.ToString.Trim)
            Next Pixel
        End If
        Navigator.Show()
        Navigator.ShowMosaik()

    End Sub

    Private Sub tsmiOpenRecentFiles_Click(sender As Object, e As EventArgs) Handles tsmiOpenRecentFiles.Click
        With LastOpenedFiles
            .Files.Clear()
            .Files.Add("C:\Users\albusmw\Dropbox\Astro\!Bilder\Test-Daten\Debayer\Stack_16bits_936frames_152s_Own.fits")
            .Files.Add("C:\!Work\Astro\FOCUS_PW\FLAT\FOCUS_11298_00010.fits")
            If .ShowDialog = DialogResult.OK Then
                LoadFile(.SelectedFile)
            End If
        End With
    End Sub

    Private Sub MaxImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaxImageToolStripMenuItem.Click
        If StackedStatPresent() = True Then
            Dim ImageData(StackingStatistics.GetUpperBound(0), StackingStatistics.GetUpperBound(1)) As Integer
            For Idx1 As Integer = 0 To StackingStatistics.GetUpperBound(0)
                For Idx2 As Integer = 0 To StackingStatistics.GetUpperBound(1)
                    ImageData(Idx1, Idx2) = CInt(StackingStatistics(Idx1, Idx2).Maximum)
                Next Idx2
            Next Idx1
            Dim FileToGenerate As String = System.IO.Path.Combine(MyPath, "Stacking_Mean.fits")
            cFITSWriter.Write(FileToGenerate, ImageData, cFITSWriter.eBitPix.Int32)
            Process.Start(FileToGenerate)
        End If
    End Sub

    Private Sub SaveAllfilesStatisticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiSaveAllFilesStat.Click

        Dim AddHisto As Boolean = True

        With sfdMain
            .Filter = "EXCEL file (*.xlsx)|*.xlsx"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With

        'Get combined hist mono X axis (INT only)
        Dim AllADUValues As New List(Of Long)
        Dim AllFiles As New List(Of String)
        AllFiles.Add("ADU value")
        For Each SingleFile As String In AllStat.Keys
            AllFiles.Add(System.IO.Path.GetFileNameWithoutExtension(SingleFile))
            For Each ADUValue As Long In AllStat(SingleFile).MonochromHistogram_Int.Keys
                If AllADUValues.Contains(ADUValue) = False Then AllADUValues.Add(ADUValue)
            Next ADUValue
        Next SingleFile
        AllADUValues.Sort()

        Using workbook As New ClosedXML.Excel.XLWorkbook

            'Generate data
            Dim XY As New List(Of Object())
            For Each ADUValue As Long In AllADUValues
                Dim Values As New List(Of Object)
                Values.Add(ADUValue)
                For Each SingleFile As String In AllStat.Keys
                    If AllStat(SingleFile).MonochromHistogram_Int.ContainsKey(ADUValue) Then Values.Add(AllStat(SingleFile).MonochromHistogram_Int(ADUValue)) Else Values.Add(String.Empty)
                Next SingleFile
                XY.Add(Values.ToArray)
            Next ADUValue
            Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram")
            worksheet.Cell(1, 1).InsertData(AllFiles, True)
            worksheet.Cell(2, 1).InsertData(XY)
            For Each col In worksheet.ColumnsUsed
                col.AdjustToContents()
            Next col

            'Save and open
            Dim FileToGenerate As String = IO.Path.Combine(MyPath, sfdMain.FileName)
            workbook.SaveAs(FileToGenerate)
            Process.Start(FileToGenerate)

        End Using

    End Sub

    Private Sub NEFReadingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NEFReadingToolStripMenuItem.Click

        Dim Reader As New cNEFReader
        Reader.Read("\\DS1819\astro\2020_07_20\DSC_0286.NEF")

    End Sub

    Private Sub tsmiSetPixelToValue_Click(sender As Object, e As EventArgs) Handles tsmiSetPixelToValue.Click

        Dim FixedPixelCount As UInt32 = 0
        Dim Limit As UShort = CUShort(InputBox("Upper limit (included)", "65536"))
        Dim SetTo As UShort = CUShort(InputBox("Set to", "0"))
        Running()
        Select Case SingleStatCalc.DataMode
            Case AstroNET.Statistics.eDataMode.UInt16
                With SingleStatCalc.DataProcessor_UInt16.ImageData(0)
                    For Idx1 As Integer = 0 To .NAXIS1 - 1
                        For Idx2 As Integer = 0 To .NAXIS2 - 1
                            If .Data(Idx1, Idx2) >= Limit Then
                                FixedPixelCount += UInt32One
                                .Data(Idx1, Idx2) = SetTo
                            End If
                        Next Idx2
                    Next Idx1
                End With
            Case Else

        End Select
        Log("Fixed " & FixedPixelCount.ValRegIndep & " pixel changed")
        Idle()

    End Sub

    Private Sub tsmiUseOpenCV_Click(sender As Object, e As EventArgs) Handles tsmiUseOpenCV.Click

        'https://shimat.github.io/opencvsharp_docs/html/d69c29a1-7fb1-4f78-82e9-79be971c3d03.htm

        Dim ImagePath As String = "C:\!Work\Astro\IC5146\rawframes\frame_1.png"

        Using src As New OpenCvSharp.Mat(ImagePath, OpenCvSharp.ImreadModes.Grayscale)

            Dim dst As New OpenCvSharp.Mat
            OpenCvSharp.Cv2.CvtColor(src, dst, OpenCvSharp.ColorConversionCodes.GRAY2BGR)      'Converts image from one color space to another

            CppStyleStarDetector(src, dst) ' C++-style

            'For transformation details see https://docs.opencv.org/master/da/d6e/tutorial_py_geometric_transformations.html
            'Dim Transformer As OpenCvSharp.Mat = OpenCvSharp.Cv2.GetRotationMatrix2D(New OpenCvSharp.Point2f(CSng((src.Width - 1) / 2), CSng((src.Height - 1) / 2)), 45.0, 1.0)

            'Transformer = New  OpenCvSharp.Mat(2, 3, OpenCvSharp.MatType.CV_64FC1, New Double(,) {{1, 2, 3}, {4, 5, 6}})
            'Dim dsize As OpenCvSharp.Size

            'Dim dst As OpenCvSharp.Mat = src.WarpAffine(Transformer, dsize, OpenCvSharp.InterpolationFlags.Lanczos4, OpenCvSharp.BorderTypes.Constant)


            Using w1 As New OpenCvSharp.Window("img", src),
                  w2 As New OpenCvSharp.Window("features", dst)
                OpenCvSharp.Cv2.WaitKey()
            End Using
        End Using

    End Sub

    ''' <summary>
    ''' Extracts keypoints by C++-style code (cv::StarDetector)
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="dst"></param>
    Private Sub CppStyleStarDetector(ByVal src As OpenCvSharp.Mat, ByVal dst As OpenCvSharp.Mat)

        Dim detector As OpenCvSharp.XFeatures2D.StarDetector = OpenCvSharp.XFeatures2D.StarDetector.Create()
        Dim keypoints() As OpenCvSharp.KeyPoint = detector.Detect(src, Nothing)

        If keypoints IsNot Nothing Then
            For Each kpt As OpenCvSharp.KeyPoint In keypoints
                Dim r As Single = kpt.Size / 2
                Dim a = kpt.Pt
                OpenCvSharp.Cv2.Circle(dst, New OpenCvSharp.Point(kpt.Pt.X, kpt.Pt.Y), CInt(Math.Truncate(r)), New OpenCvSharp.Scalar(0, 255, 0), 1, OpenCvSharp.LineTypes.Link8, 0)
                OpenCvSharp.Cv2.Line(dst, New OpenCvSharp.Point(kpt.Pt.X + r, kpt.Pt.Y + r), New OpenCvSharp.Point(kpt.Pt.X - r, kpt.Pt.Y - r), New OpenCvSharp.Scalar(0, 255, 0), 1, OpenCvSharp.LineTypes.Link8, 0)
                OpenCvSharp.Cv2.Line(dst, New OpenCvSharp.Point(kpt.Pt.X - r, kpt.Pt.Y + r), New OpenCvSharp.Point(kpt.Pt.X + r, kpt.Pt.Y - r), New OpenCvSharp.Scalar(0, 255, 0), 1, OpenCvSharp.LineTypes.Link8, 0)
            Next kpt
        End If

    End Sub

    Private Sub OpenCVMedianFilterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenCVMedianFilterToolStripMenuItem.Click

        Using src As New OpenCvSharp.Mat(SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS2, SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS1, OpenCvSharp.MatType.CV_16UC1, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)

            Dim dst As New OpenCvSharp.Mat
            OpenCvSharp.Cv2.MedianBlur(src, dst, 3)

            Dim MyHandle As Runtime.InteropServices.GCHandle = Runtime.InteropServices.GCHandle.Alloc(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, Runtime.InteropServices.GCHandleType.Pinned)
            Dim WritePtr As IntPtr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, 0)
            Dim Length As Long = SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.LongLength
            CopyMemory(WritePtr, dst.Data, Length)
            MyHandle.Free()

            'Using w1 As New OpenCvSharp.Window("srv", src),
            '      w2 As New OpenCvSharp.Window("dst", dst)
            '    OpenCvSharp.Cv2.WaitKey()
            'End Using

        End Using

    End Sub

    Private Sub DisplayImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisplayImageToolStripMenuItem.Click
        Dim ImageDisplay As New frmImageDisplay
        ImageDisplay.FileToDisplay = LastFile
        ImageDisplay.Show()
        ImageDisplay.SingleStatCalc = SingleStatCalc
        ImageDisplay.StatToUsed = LastStat
        ImageDisplay.MyIPP = DB.IPP
        ImageDisplay.Props.MinCutOff = LastStat.MonoStatistics_Int.Min.Key
        ImageDisplay.Props.MaxCutOff = LastStat.MonoStatistics_Int.Max.Key
        ImageDisplay.GenerateDisplayImage()
    End Sub

    Private Sub CoordsForALADINCallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CoordsForALADINCallToolStripMenuItem.Click

        Dim FileToRun As String = LastFile

        'Get the FITS header information
        Dim DataStartPos As Integer = -1
        Dim X As List(Of cFITSHeaderParser.sHeaderElement) = cFITSHeaderChanger.ReadHeader(FileToRun, DataStartPos)
        Dim File_RA_JNow As String = Nothing
        Dim File_Dec_JNow As String = Nothing
        For Each Entry As cFITSHeaderParser.sHeaderElement In X
            If Entry.Keyword = eFITSKeywords.RA Then File_RA_JNow = CStr(Entry.Value).Trim("'"c).Trim.Trim("'"c)
            If Entry.Keyword = eFITSKeywords.DEC Then File_Dec_JNow = CStr(Entry.Value).Trim("'"c).Trim.Trim("'"c)
        Next Entry

        'Data from QHYCapture (10Micron) are in JNow, so convert to J2000 for PlateSolve
        Dim File_RA_J2000 As Double = Double.NaN
        Dim File_Dec_J2000 As Double = Double.NaN
        JNowToJ2000(AstroParser.ParseRA(File_RA_JNow), AstroParser.ParseDeclination(File_Dec_JNow), File_RA_J2000, File_Dec_J2000)

        Dim AladinCall As String = Ato.AstroCalc.FormatHMS(File_RA_J2000) & " " & Ato.AstroCalc.Format360Degree(File_Dec_J2000)
        Clipboard.Clear:Clipboard.SetText(AladinCall)

    End Sub

End Class
