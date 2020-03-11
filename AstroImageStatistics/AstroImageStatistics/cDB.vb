Option Explicit On
Option Strict On

Public Class cDB


    <ComponentModel.Category("2.) Plotting")>
    <ComponentModel.DisplayName("a) Open statistics graph?")>
    Public Property AutoOpenStatGraph As Boolean = False

    <ComponentModel.Category("2.) Plotting")>
    <ComponentModel.DisplayName("b) Stack graphs below form?")>
    Public Property StackGraphs As Boolean = False

    <ComponentModel.DisplayName("Clean log on any analysis?")>
    Public Property AutoClearLog As Boolean = True

    <ComponentModel.DisplayName("Use IPP?")>
    Public Property UseIPP As Boolean = True

    <ComponentModel.DisplayName("Stacking?")>
    Public Property Stacking As Boolean = False

End Class
