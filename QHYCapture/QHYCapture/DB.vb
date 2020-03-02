Option Explicit On
Option Strict On

Public Structure sRect_UInt
    Dim X As UInteger
    Dim Y As UInteger
    Dim W As UInteger
    Dim H As UInteger
End Structure

'''<summary>Database holding relevant information.</summary>
Public Class cDB

    Const Cat1 As String = "1. Exposure"
    Const Cat2 As String = "2. Image storage"
    Const Cat3 As String = "3. Imaging hardware"

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    Public IPPRoots As String() = {"C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.5.281\windows\redist\intel64\ipp", "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp\"}
    Public IPP As cIntelIPP

    Public Plotter As cZEDGraphService

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("1. Exposure time [s]")>
    <ComponentModel.Description("Exposure time [s]")>
    Public Property ExposureTime As Double = 1.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("2. Gain")>
    Public Property Gain As Double = 0.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("3. Offset")>
    Public Property Offset As Double = 0.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("4. Binning")>
    <ComponentModel.Description("Binning (NxN)")>
    Public Property Binning As UInteger = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("5. # of captures")>
    Public Property CaptureCount As Integer = 1

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("1. GUID of the sequence")>
    <ComponentModel.Description("Exposure time [s]")>
    Public Property GUID As String = System.Guid.NewGuid().ToString

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("2. Author")>
    <ComponentModel.Description("Author")>
    Public Property Author As String = "Martin Weiss"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("3. Store captured image?")>
    Public Property StoreImage As Boolean = True

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("4. Open image automatically?")>
    Public Property AutoOpenImage As Boolean = True

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("1. Telescope used")>
    Public Property Telescope As String = "Planewave CDK 12.5 with reducer"


    Public Property RemoveOverscan As Boolean = False

End Class
