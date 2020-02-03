Option Explicit On
Option Strict On

Imports System.ComponentModel

Public Class DB

    Public Shared ReadOnly MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

    Private Class Cat
        Public Const FileProcessing As String = "1.) File processing"
        Public Const Solver As String = "2.) Solver"
    End Class

    Public Class cSettings

        <Category(Cat.FileProcessing)>
        <DisplayName("a) Process BZERO?")>
        <DefaultValue(True)>
        Public Property FITS_UseBZERO As Boolean = True

        <Category(Cat.Solver)>
        <DisplayName("a) Run solver on each image?")>
        <DefaultValue(False)>
        Public Property Solver_Use As Boolean = False
        <Category(Cat.Solver)>
        <DisplayName("b) Solver path")>
        <DefaultValue("C:\TEMP\PlateSolve2.exe")>
        Public Property Solver_Path As String = "C:\TEMP\PlateSolve2.exe"

    End Class

    Public Shared Settings As New cSettings

End Class
