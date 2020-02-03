Option Explicit On
Option Strict On

Public Class Form1

    Private Sub OpenFileToAnalyseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFileToAnalyseToolStripMenuItem.Click

        With ofdMain
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With

        Dim File As String = ofdMain.FileName
        Dim FileNameOnly As String = System.IO.Path.GetFileName(File)

        Dim FITSHeader As New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(File))

        Dim IPPPath As String = String.Empty
        TestIPPPath(IPPPath, "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.5.281\windows\redist\intel64_win\ipp")
        TestIPPPath(IPPPath, "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp")

        Dim IPP As New cIntelIPP(IPPPath)
        cFITSReader.IPPPath = IPPPath

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

End Class
