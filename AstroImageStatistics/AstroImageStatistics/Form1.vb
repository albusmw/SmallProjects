Option Explicit On
Option Strict On

Public Class Form1

    Private IPP As cIntelIPP
    Private MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

    Private Sub OpenFileToAnalyseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFileToAnalyseToolStripMenuItem.Click

        With ofdMain
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With

        Dim File As String = ofdMain.FileName
        Dim FileNameOnly As String = System.IO.Path.GetFileName(File)

        Dim FITSHeader As New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(File))

        Dim Stopper As New cStopper
        Dim FITSReader As New cFITSReader
        Dim SingleStatCalc As New AstroNET.Statistics(IPP)

        Select Case FITSHeader.BitPix
            Case 16
                SingleStatCalc.DataProcessor_UInt16.ImageData = FITSReader.ReadInUInt16(File, True)
            Case 32
                SingleStatCalc.DataProcessor_Int32.ImageData = FITSReader.ReadInInt32(File, False)
            Case Else
                MsgBox("File format <" & FITSHeader.BitPix.ToString.Trim & "> not yet supported!")
                Exit Sub
        End Select
        Stopper.Stamp(FileNameOnly & ": Reading")
        Dim Stat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics
        Stopper.Stamp(FileNameOnly & ": Statistics")
        Log("Statistics for <" & File & ">:")
        Log(Stat.StatisticsReport.ToArray())
        Log("-----------------------------------------------------------------------------------")

        PlotStatistics(File, Stat)

    End Sub

    Private Sub TestIPPPath(ByRef CurrentPath As String, ByVal Path As String)
        If System.IO.Directory.Exists(Path) Then
            If String.IsNullOrEmpty(CurrentPath) = True Then CurrentPath = Path
        End If
    End Sub

    '''<summary>Open a simple form with a ZEDGraph on it and plots the statistical data.</summary>
    '''<param name="Stats">Statistics data to plot.</param>
    Private Sub PlotStatistics(ByVal FileName As String, ByRef Stats As AstroNET.Statistics.sStatistics)
        Dim Disp As New cZEDGraphForm
        Disp.PlotData(New Double() {1, 2, 3, 4})
        'Plot histogram
        Disp.Plotter.Clear()
        Disp.Plotter.PlotXvsY("R", Stats.BayerHistograms(0, 0), New cZEDGraphService.sGraphStyle(Color.Red, 1))
        Disp.Plotter.PlotXvsY("G1", Stats.BayerHistograms(0, 1), New cZEDGraphService.sGraphStyle(Color.LightGreen, 1))
        Disp.Plotter.PlotXvsY("G2", Stats.BayerHistograms(1, 0), New cZEDGraphService.sGraphStyle(Color.DarkGreen, 1))
        Disp.Plotter.PlotXvsY("B", Stats.BayerHistograms(1, 1), New cZEDGraphService.sGraphStyle(Color.Blue, 1))
        Disp.Plotter.PlotXvsY("Mono histo", Stats.MonochromHistogram, New cZEDGraphService.sGraphStyle(Color.Black, 1))
        Disp.Plotter.ManuallyScaleXAxis(Stats.MonoStatistics.Min, Stats.MonoStatistics.Max)
        Disp.Plotter.AutoScaleYAxisLog()
        Disp.Plotter.GridOnOff(True, True)
        Disp.Plotter.ForceUpdate()
        Disp.Hoster.Text = FileName
    End Sub

    Private Sub WriteTestDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WriteTestDataToolStripMenuItem.Click
        cFITSWriter.WriteTestFile_Int8("FITS_BitPix8.FITS")
        cFITSWriter.WriteTestFile_Int16("FITS_BitPix16.FITS") ': Process.Start("FITS_BitPix16.FITS")
        cFITSWriter.WriteTestFile_Int32("FITS_BitPix32.FITS") ': Process.Start("FITS_BitPix32.FITS")
        cFITSWriter.WriteTestFile_Float32("FITS_BitPix32f.FITS") ': Process.Start("FITS_BitPix32f.FITS")
        cFITSWriter.WriteTestFile_Float64("FITS_BitPix64f.FITS") : Process.Start("FITS_BitPix64f.FITS")
        'MsgBox("OK")
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
        Dim IPPPath As String = String.Empty
        TestIPPPath(IPPPath, "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.5.281\windows\redist\intel64_win\ipp")
        TestIPPPath(IPPPath, "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp")
        IPP = New cIntelIPP(IPPPath)
        cFITSReader.IPPPath = IPPPath
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub OpenEXELocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenEXELocationToolStripMenuItem.Click
        Process.Start(MyPath)
    End Sub

    Private Sub Log(ByVal Text As String)
        Log(Text, False)
    End Sub

    Private Sub Log(ByVal Text As List(Of String))
        For Each Line As String In Text
            Log(Line, False)
        Next Line
    End Sub

    Private Sub Log(ByVal Text() As String)
        For Each Line As String In Text
            Log(Line, False)
        Next Line
    End Sub

    Private Sub Log(ByVal Text As String, ByVal LogInStatus As Boolean)
        Text = Format(Now, "HH.mm.ss:fff") & "|" & Text
        With tbLogOutput
            If .Text.Length = 0 Then
                .Text = Text
            Else
                .Text &= System.Environment.NewLine & Text
            End If
            .SelectionStart = .Text.Length - 1
            .SelectionLength = 0
            .ScrollToCaret()
            If LogInStatus = True Then tsslMain.Text = Text
        End With
        DE()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    'cFITSWriter.WriteTestFile_Int8("FITS_BitPix8.FITS") : Process.Start("FITS_BitPix8.FITS")
    'cFITSWriter.WriteTestFile_Int16("FITS_BitPix16.FITS") : Process.Start("FITS_BitPix16.FITS")
    'cFITSWriter.WriteTestFile_Int32("FITS_BitPix32.FITS") : Process.Start("FITS_BitPix32.FITS")
    'cFITSWriter.WriteTestFile_Float32("FITS_BitPix32f.FITS") : Process.Start("FITS_BitPix32f.FITS")

End Class
