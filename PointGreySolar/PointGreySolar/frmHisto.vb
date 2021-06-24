Option Explicit On
Option Strict On

'''<summary>Generic form to calculcate and show image statistics as graph and text.</summary>
Public Class frmHisto

    Public Stat As AstroNET.Statistics.sStatistics
    Public HistoPlot As cZEDGraphForm

    Public Property CalcStat_Mono As Boolean = True
    Public Property CalcStat_Bayer As Boolean = False
    Public Property BayerChannelNames As New List(Of String)({"R", "G1", "G2", "B"})
    Public Property PlotStyle As cZEDGraphService.eCurveMode = cZEDGraphService.eCurveMode.LinesAndPoints

    Public Sub X() Handles Me.Load
        HistoPlot = New cZEDGraphForm(scMain.Panel1)
    End Sub

    Public Sub CalcStat(ByRef Container As AstroNET.Statistics)

        Stat = Container.ImageStatistics(Container.DataFixFloat)
        tbStat.Text = Join(Stat.StatisticsReport(CalcStat_Mono, CalcStat_Bayer, BayerChannelNames).ToArray(), System.Environment.NewLine)
        PlotHisto(Stat)

    End Sub

    Private Sub PlotHisto(ByRef Stats As AstroNET.Statistics.sStatistics)
        'AddHandler Disp.PointValueHandler, AddressOf PointValueHandler
        'Disp.PlotData("Test", New Double() {1, 2, 3, 4}, Color.Red)
        'Disp.Init()
        Dim XAxisMargin As Integer = 10                                    'axis margin to see the most outer values
        Select Case Stats.DataMode
            Case AstroNET.Statistics.sStatistics.eDataMode.Fixed
                'Plot histogram
                HistoPlot.Plotter.Clear()
                If IsNothing(Stats.BayerHistograms_Int) = False And CalcStat_Bayer Then
                    HistoPlot.Plotter.PlotXvsY(BayerChannelNames(0) & "[0,0]", Stats.BayerHistograms_Int(0, 0), 1, New cZEDGraphService.sGraphStyle(Color.Red, PlotStyle, 1))
                    HistoPlot.Plotter.PlotXvsY(BayerChannelNames(1) & "[0,1]", Stats.BayerHistograms_Int(0, 1), 1, New cZEDGraphService.sGraphStyle(Color.LightGreen, PlotStyle, 1))
                    HistoPlot.Plotter.PlotXvsY(BayerChannelNames(2) & "[1,0]", Stats.BayerHistograms_Int(1, 0), 1, New cZEDGraphService.sGraphStyle(Color.Green, PlotStyle, 1))
                    HistoPlot.Plotter.PlotXvsY(BayerChannelNames(3) & "[1,1]", Stats.BayerHistograms_Int(1, 1), 1, New cZEDGraphService.sGraphStyle(Color.Blue, PlotStyle, 1))
                End If
                If IsNothing(Stats.MonochromHistogram_Int) = False And CalcStat_Mono Then
                    HistoPlot.Plotter.PlotXvsY("Mono histo", Stats.MonochromHistogram_Int, 1, New cZEDGraphService.sGraphStyle(Color.Black, PlotStyle, 1))
                End If
                HistoPlot.Plotter.ManuallyScaleXAxis(UInt16.MinValue - XAxisMargin, UInt16.MaxValue + XAxisMargin)
        End Select
        HistoPlot.Plotter.AutoScaleYAxisLog()
        HistoPlot.Plotter.GridOnOff(True, True)
        HistoPlot.Plotter.ForceUpdate()
        'Set style of the window
        HistoPlot.Plotter.SetCaptions(String.Empty, "Pixel value", "# of pixel with this value")
        HistoPlot.Plotter.MaximizePlotArea()
        HistoPlot.Tag = "Statistics"
        'Position window below the main window
    End Sub

End Class