Option Explicit On
Option Strict On

'''<summary>Database holding relevant information.</summary>
Public Class cDB

    Public ReadOnly Property MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    Public IPPRoots As String() = {"C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.5.281\windows\redist\intel64\ipp", "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp\"}
    Public IPP As cIntelIPP

    Public Plotter As cZEDGraphService

    Public Property GUID As String = (New Guid).ToString

    Public Property ExposureTime As Double = 1.0

    Public Property Gain As Double = 0.0

    Public Property Offset As Double = 0.0

    Public Property StoreImage As Boolean = True

    Public Property AutoOpenImage As Boolean = True

    Public Property CaptureCount As Integer = 1

    Public Property Binning As UInteger = 1

    Public Property RemoveOverscan As Boolean = False

End Class
