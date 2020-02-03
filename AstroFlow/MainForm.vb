Option Explicit On
Option Strict On

Public Class MainForm

    Dim IPPPath As String = "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp\"
    Dim MyIPP As New cIntelIPP(IPPPath & "ipps.dll", IPPPath & "ippvm.dll")

    Dim Stopper As New Stopwatch

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click

        'Select which file to open
        With ofdMain
            .Filter = "FITS files (*.fit(s))|*.fit?"
            .Multiselect = True
            If .ShowDialog <> DialogResult.OK Then Exit Sub
            If System.IO.File.Exists(.FileName) = False Then Exit Sub
        End With

        'Get the base folder name and prepare the sum image
        Dim BaseFolder As String = System.IO.Path.GetDirectoryName(ofdMain.FileNames(0))
        Dim SumImage(,) As Int32 = {}

        Dim TotalStats(1, 1) As Dictionary(Of Int32, UInt32)

        Dim FileCount As Integer = 0
        For Each File As String In ofdMain.FileNames

            'Open file and read in data as Int32 (known before)
            AddLog("Loading <" & File & ">")
            Dim ImageData(,) As Int32 = {}
            Dim FITSReader1 As New cFITSReader
            FITSReader1.IPPPath = IPPPath
            FITSReader1.ReadInRaw(File, ImageData)
            Dim FITSHeader As List(Of cFITSHeaderChanger.sHeaderElement) = cFITSHeaderChanger.ReadHeader(File)

            NewCode.ScaleData(FITSHeader, ImageData)

            FileCount += 1

            'Prepare bayer statistics
            Dim Idxs As New List(Of String)
            Idxs.Add("0:0")
            Idxs.Add("0:1")
            Idxs.Add("1:0")
            Idxs.Add("1:1")
            Dim StatPerBayerChannel(1, 1) As Dictionary(Of Int32, UInt32)

            'Calculate bayer statistics in parallel
            System.Threading.Tasks.Parallel.ForEach(Idxs, Sub(Idx As String)
                                                              Dim Idx0 As Integer = CInt(Idx.Split(CType(":", Char()))(0))
                                                              Dim Idx1 As Integer = CInt(Idx.Split(CType(":", Char()))(1))
                                                              Dim NewStat As Dictionary(Of Integer, UInt32) = ImageProcessing.BayerStatistics(ImageData, Idx0, 2, Idx1, 2)
                                                              SyncLock StatPerBayerChannel
                                                                  StatPerBayerChannel(Idx0, Idx1) = NewStat
                                                              End SyncLock
                                                          End Sub)

            'Calculate statistics over all (combine all bayer channels to 1 big channel) for 1 channel
            Dim StatOverAll As New Dictionary(Of Integer, UInt32)
            For Idx0 As Integer = 0 To 1
                For Idx1 As Integer = 0 To 1
                    For Each Key As Integer In StatPerBayerChannel(Idx0, Idx1).Keys
                        If StatOverAll.ContainsKey(Key) Then
                            StatOverAll(Key) += StatPerBayerChannel(Idx0, Idx1)(Key)
                        Else
                            StatOverAll.Add(Key, StatPerBayerChannel(Idx0, Idx1)(Key))
                        End If
                    Next Key
                Next Idx1
            Next Idx0
            StatOverAll = ImageProcessing.SortDictionary(StatOverAll)

            'Display results and accumulate statistics
            Dim StatAccu(10) As String
            StatAccu(0) = "Channel statistics:"
            StatAccu(1) = "Parameter       | <0:0 (R)> | <0:1 (G)> | <1:0 (G)> | <0:0 (B)> | <X:Y (B)> |"
            StatAccu(2) = "# of samples    |"
            StatAccu(3) = "Different values|"
            StatAccu(4) = "  Minimum       |"
            StatAccu(5) = "  Maximum       |"
            StatAccu(6) = "  Mean          |"
            StatAccu(7) = "Different deltas|"
            StatAccu(8) = "  Minimum       |"
            StatAccu(9) = "  Maximum       |"
            StatAccu(10) = "  Mean          |"
            Dim PadSize As Integer = 11

            'Prepare statistics for all bayer channels
            For Idx1 As Integer = 0 To 1
                For Idx2 As Integer = 0 To 1

                    'Statistics of the quantization
                    Dim DiffHisto As New Dictionary(Of Int32, UInt32)
                    ImageProcessing.HistogramParameters(StatPerBayerChannel(Idx1, Idx2), DiffHisto)

                    StatAccu(2) &= ImageProcessing.HistoCount(StatPerBayerChannel(Idx1, Idx2)).ToString.Trim.PadLeft(PadSize) & "|"
                    StatAccu(3) &= StatPerBayerChannel(Idx1, Idx2).Count.ToString.Trim.PadLeft(PadSize) & "|"
                    StatAccu(4) &= StatPerBayerChannel(Idx1, Idx2).First.Key.ToString.Trim.PadLeft(PadSize) & "|"
                    StatAccu(5) &= StatPerBayerChannel(Idx1, Idx2).Last.Key.ToString.PadLeft(PadSize) & "|"
                    StatAccu(6) &= Format(ImageProcessing.HistoMean(StatPerBayerChannel(Idx1, Idx2)), "0.000").ToString.Trim.PadLeft(PadSize) & "|"
                    StatAccu(7) &= DiffHisto.Count.ToString.Trim.PadLeft(PadSize) & "|"
                    StatAccu(8) &= DiffHisto.First.Key.ToString.Trim.PadLeft(PadSize) & "|"
                    StatAccu(9) &= DiffHisto.Last.Key.ToString.Trim.PadLeft(PadSize) & "|"
                    StatAccu(10) &= Format(ImageProcessing.HistoMean(DiffHisto), "0.000").ToString.Trim.PadLeft(PadSize) & "|"

                    'Accumulate total bayer statistics for all used images
                    If FileCount = 1 Then
                        TotalStats(Idx1, Idx2) = New Dictionary(Of Integer, UInt32)
                        For Each Entry As Integer In StatPerBayerChannel(Idx1, Idx2).Keys
                            TotalStats(Idx1, Idx2).Add(Entry, StatPerBayerChannel(Idx1, Idx2)(Entry))
                        Next Entry
                    Else
                        For Each Entry As Integer In StatPerBayerChannel(Idx1, Idx2).Keys
                            If TotalStats(Idx1, Idx2).ContainsKey(Entry) = False Then
                                TotalStats(Idx1, Idx2).Add(Entry, StatPerBayerChannel(Idx1, Idx2)(Entry))
                            Else
                                TotalStats(Idx1, Idx2)(Entry) += StatPerBayerChannel(Idx1, Idx2)(Entry)
                            End If
                        Next Entry
                    End If

                Next Idx2
            Next Idx1

            'Prepare statistics for "total" channel of 1 file
            'Statistics of the quantization
            Dim DiffHistoTotal As New Dictionary(Of Int32, UInt32)
            ImageProcessing.HistogramParameters(StatOverAll, DiffHistoTotal)

            StatAccu(2) &= ImageProcessing.HistoCount(StatOverAll).ToString.Trim.PadLeft(PadSize) & "|"
            StatAccu(3) &= StatOverAll.Count.ToString.Trim.PadLeft(PadSize) & "|"
            StatAccu(4) &= StatOverAll.First.Key.ToString.Trim.PadLeft(PadSize) & "|"
            StatAccu(5) &= StatOverAll.Last.Key.ToString.PadLeft(PadSize) & "|"
            StatAccu(6) &= Format(ImageProcessing.HistoMean(StatOverAll), "0.000").ToString.Trim.PadLeft(PadSize) & "|"
            StatAccu(7) &= DiffHistoTotal.Count.ToString.Trim.PadLeft(PadSize) & "|"
            StatAccu(8) &= DiffHistoTotal.First.Key.ToString.Trim.PadLeft(PadSize) & "|"
            StatAccu(9) &= DiffHistoTotal.Last.Key.ToString.Trim.PadLeft(PadSize) & "|"
            StatAccu(10) &= Format(ImageProcessing.HistoMean(DiffHistoTotal), "0.000").ToString.Trim.PadLeft(PadSize) & "|"


            AddLog(StatAccu)

                    '===================================================================================================
                    'Plate solve

                    If DB.Settings.Solver_Use = True Then

                        Dim Solver As New cPlateSolve
                        cPlateSolve.PlateSolvePath = DB.Settings.Solver_Path
                        Dim SolverBaseRA As Double = Double.NaN
                        Dim SolverBaseDec As Double = Double.NaN

                        '1.) Save as LUM file
                        Dim ByteDataFile As String = System.IO.Path.Combine(DB.MyPath, "TEMP.FITS")
                        If System.IO.File.Exists(ByteDataFile) = True Then System.IO.File.Delete(ByteDataFile)
                        cFITSWriter.Write(ByteDataFile, ImgProcessing.Lum8Bit(ImageData), cFITSWriter.eBitPix.Byte, New List(Of String()))

                        '2.) Get the FITS header information
                        Dim File_RA As String() = {}
                        Dim File_Dec As String = String.Empty
                        Dim Binning As Integer = 1
                        If FileCount = 1 Then
                            For Each Entry As cFITSHeaderChanger.sHeaderElement In FITSHeader
                                If Entry.Keyword.Trim.ToUpper = "RA" Then File_RA = Experimental.ParseRA(Entry.Value.Trim)
                                If Entry.Keyword.Trim.ToUpper = "DEC" Then File_Dec = Entry.Value.Trim
                            Next Entry
                            Solver.SetRA(CInt(File_RA(0)), CInt(File_RA(1)), Val(File_RA(2)))
                            Solver.SetDec(Val(File_Dec), 0, 0)
                        Else
                            Solver.RA = SolverBaseRA
                            Solver.Dec = SolverBaseDec
                        End If

                        '3.) Run the solver
                        Dim RawOut As String() = {}
                        With Solver
                            .SetDimX(48.93 / Binning)                                                           'constant for system [telescope-camera]
                            .SetDimY(32.71 / Binning)                                                           'constant for system [telescope-camera]
                            .HoldOpenTime = 0
                            Dim ErrorCode As String = .Solve(ByteDataFile, RawOut)
                            If FileCount > 1 Then AddLog("Error: " & Format(Solver.PixelErrorRA, "0.000").Trim & " pixel RA, " & Format(Solver.PixelErrorDec, "0.000").Trim & " pixel DEC")
                        End With

                        '4.) Store initial values
                        If FileCount = 1 Then
                            SolverBaseRA = Solver.SolvedRA
                            SolverBaseDec = Solver.SolvedDec
                        End If

                    End If

                    'Sum image
                    If IsNothing(SumImage) = True Then
                        SumImage = MyIPP.Copy(ImageData)
                    Else
                        MyIPP.Add(ImageData, SumImage)
                    End If

                    'PLAY CODE
                    ' Dim X As Image = (New cImageDisplay).CalculateImageFromData(ImageData).BitmapToProcess
                    'X.Save(ofdMain.FileName & ".png")

                    AddLogSep("-"c)

                Next File

                'Make float
                'Dim SumImageFloat(,) As Double = {}
                'MyIPP.Convert(SumImage, SumImageFloat)
                'MyIPP.DivC(SumImageFloat, CDbl(FileCount))

                'Store sum image
                Dim SumImageFile As String = BaseFolder & "Summary.FITS"
        AddLog("Storing sum image to <" & SumImageFile & ">")
        cFITSWriter.Write(SumImageFile, SumImage, cFITSWriter.eBitPix.Int32)
        Process.Start(SumImageFile)
        AddLogSep("="c)

        'Display statistics
        Experimental.DisplayGraph(TotalStats)

        'PLAY CODE
        'ImageProcessing.InterpolateBayerChannel(SumImage, 0, 2, 0, 2)

    End Sub

    Private Sub AddLogSep(ByVal Character As Char)
        AddLog(New String() {New String(Character, 80)})
    End Sub

    Private Sub AddLog(ByVal Line As String, ByRef Stopper As Stopwatch)
        AddLog(New String() {Line & ": " & Stopper.ElapsedMilliseconds.ToString.Trim & " ms"})
        Stopper.Reset()
    End Sub

    Private Sub AddLog(ByVal Line As String)
        AddLog(New String() {Line})
    End Sub

    Private Sub AddLog(ByVal Lines() As String)
        If tbLog.Text.Length > 0 Then tbLog.Text &= System.Environment.NewLine
        With tbLog
            For Idx As Integer = 0 To Lines.GetUpperBound(0)
                If Idx = 0 Then
                    tbLog.Text &= Format(Now, "HH:mm:ss.fff") & "|" & Lines(Idx)
                Else
                    tbLog.Text &= "            " & "|" & Lines(Idx)
                End If
                If Idx <> Lines.GetUpperBound(0) Then tbLog.Text &= System.Environment.NewLine
            Next Idx
        End With
        tbLog.SelectionStart = tbLog.Text.Length - 1
        tbLog.ScrollToCaret()
        DE()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Const AlphaMask As Int32 = &HFF000000
    Const RedMask As Int32 = &HFF0000
    Const greenMask As Int32 = &HFF00
    Const blueMask As Int32 = &HFF

    Private Sub FITSGrepToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FITSGrepToolStripMenuItem.Click
        Dim MyGrep As New GrepForm
        MyGrep.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        pgMain.SelectedObject = DB.Settings
    End Sub

    Private Sub CompressionTestToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CompressionTestToolStripMenuItem.Click

        'Calculates the difference of 2 images and stores the difference image
        AddLog("Starting compression test ...")

        'Load image 1
        With ofdMain
            .Title = "Select image 1"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With
        Dim File1 As String = ofdMain.FileName
        Dim ImageData0(,) As Int32 = {}
        Dim FITSReader1 As New cFITSReader
        FITSReader1.IPPPath = IPPPath
        FITSReader1.ReadInRaw(File1, ImageData0)
        Dim FITSHeader1 As List(Of cFITSHeaderChanger.sHeaderElement) = cFITSHeaderChanger.ReadHeader(File1)
        NewCode.ScaleData(FITSHeader1, ImageData0)

        'Load image 2
        With ofdMain
            .Title = "Select image 2"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With
        Dim File2 As String = ofdMain.FileName
        Dim ImageData1(,) As Int32 = {}
        Dim FITSReader2 As New cFITSReader
        FITSReader2.IPPPath = IPPPath
        FITSReader2.ReadInRaw(File2, ImageData1)
        Dim FITSHeader2 As List(Of cFITSHeaderChanger.sHeaderElement) = cFITSHeaderChanger.ReadHeader(File2)
        NewCode.ScaleData(FITSHeader2, ImageData1)

        'Calculate difference
        Dim ImageDiff(ImageData0.GetUpperBound(0), ImageData0.GetUpperBound(1)) As Int32
        For Idx0 As Integer = 0 To ImageData0.GetUpperBound(0)
            For Idx1 As Integer = 0 To ImageData0.GetUpperBound(1)
                ImageDiff(Idx0, Idx1) = ImageData1(Idx0, Idx1) - ImageData0(Idx0, Idx1)
            Next Idx1
        Next Idx0

        'TODO here: compress to "Int8" if possible ...

        'Store
        Dim OutFile As String = "C:\WORK_ASTRO\compression_difference.fits"
        cFITSWriter.Write(OutFile, ImageDiff, cFITSWriter.eBitPix.Int16)
        Process.Start(OutFile)

        AddLog("Compression test done.")

    End Sub

    Private Sub ByerPreviewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByerPreviewToolStripMenuItem.Click

        Dim TestFile As String = "\\DS001\astro\2018_11_18\00_17_44\M103\FRAME_010.FITS"
        Dim X As New cFITSReader
        Dim ImageData(,) As Double = {}
        X.ReadIn(TestFile, ImageData)

        Dim Stat(,) As Dictionary(Of Double, UInt32) = Nothing
        Stat = ImageProcessing.BayerStatistics(ImageData)

        Dim PictureData(,) As Color = Nothing
        ImgProcessing.BayerPreview(ImageData, PictureData)

    End Sub

End Class
