Option Explicit On
Option Strict On

Public Class cDB

    Private Const Cat_analysis As String = "1.) Analysis"
    Private Const Cat_log As String = "2.) Plotting"
    Private Const Cat_plot As String = "3.) Plotting"

    <ComponentModel.Category(Cat_analysis)>
    <ComponentModel.DisplayName("1.) Use IPP?")>
    <ComponentModel.DefaultValue(True)>
    Public Property UseIPP As Boolean = True

    <ComponentModel.Category(Cat_analysis)>
    <ComponentModel.DisplayName("2.) Stacking")>
    Public Property Stacking As Boolean = False

    <ComponentModel.Category(Cat_log)>
    <ComponentModel.DisplayName("1.) Clean log on any analysis?")>
    Public Property AutoClearLog As Boolean = True

    <ComponentModel.Category(Cat_plot)>
    <ComponentModel.DisplayName("1.) Auto-open graph")>
    Public Property AutoOpenStatGraph As Boolean = False

    <ComponentModel.Category(Cat_plot)>
    <ComponentModel.DisplayName("2.) Stack graphs below form")>
    Public Property StackGraphs As Boolean = False

    <ComponentModel.DisplayName("Used IPP path")>
    Public ReadOnly Property IPPPath As String
        Get
            Return MyIPPPath
        End Get
    End Property
    Public MyIPPPath As String = String.Empty

End Class
