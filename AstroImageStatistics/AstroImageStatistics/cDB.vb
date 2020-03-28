Option Explicit On
Option Strict On

Public Class cDB

    Private Const Cat_analysis As String = "1.) Analysis"
    Private Const Cat_log As String = "2.) Logging"
    Private Const Cat_plot As String = "3.) Plotting"
    Private Const Cat_save As String = "4.) Saving"

    <ComponentModel.Category(Cat_analysis)>
    <ComponentModel.DisplayName("1.) Use IPP?")>
    <ComponentModel.DefaultValue(True)>
    Public Property UseIPP As Boolean = True

    <ComponentModel.Category(Cat_analysis)>
    <ComponentModel.DisplayName("2.) Stacking")>
    <ComponentModel.DefaultValue(False)>
    Public Property Stacking As Boolean = False

    <ComponentModel.Category(Cat_log)>
    <ComponentModel.DisplayName("1.) Clean log on any analysis?")>
    <ComponentModel.DefaultValue(True)>
    Public Property AutoClearLog As Boolean = True

    <ComponentModel.Category(Cat_plot)>
    <ComponentModel.DisplayName("1.) Auto-open graph")>
    <ComponentModel.DefaultValue(False)>
    Public Property AutoOpenStatGraph As Boolean = False

    <ComponentModel.Category(Cat_plot)>
    <ComponentModel.DisplayName("2.) Plot style")>
    Public Property PlotStyle As cZEDGraphService.eCurveMode = cZEDGraphService.eCurveMode.LinesAndPoints

    <ComponentModel.Category(Cat_plot)>
    <ComponentModel.DisplayName("3.) Stack graphs below form")>
    <ComponentModel.Description("Position graphs below the main window (exact overlay of different graph windows)")>
    <ComponentModel.DefaultValue(False)>
    Public Property StackGraphs As Boolean = False

    <ComponentModel.Category(Cat_save)>
    <ComponentModel.DisplayName("1.) Image quality")>
    <ComponentModel.Description("Image quality parameter to use")>
    <ComponentModel.DefaultValue(80L)>
    Public Property ImageQuality As Int64 = 80L

    <ComponentModel.DisplayName("Used IPP path")>
    Public ReadOnly Property IPPPath As String
        Get
            Return MyIPPPath
        End Get
    End Property
    Public MyIPPPath As String = String.Empty

End Class
