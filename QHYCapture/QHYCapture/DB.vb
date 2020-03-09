Option Explicit On
Option Strict On

Public Structure sRect_UInt
    Dim X As UInteger
    Dim Y As UInteger
    Dim W As UInteger
    Dim H As UInteger
End Structure

Public Enum eReadOutMode As UInteger
    Photographic = 0
    HighGain = 1
    ExtendFullwell = 2
    Unvalid = UInteger.MaxValue
End Enum

'''<summary>Database holding relevant information.</summary>
Public Class cDB

    Const Cat1 As String = "1. Exposure"
    Const Cat2 As String = "2. Image storage"
    Const Cat3 As String = "3. Imaging hardware"
    Const Cat4 As String = "4. Object description"
    Const Cat5 As String = "5. Debug and logging"

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    Public IPPRoots As String() = {"C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.5.281\windows\redist\intel64\ipp", "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp\"}
    Public IPP As cIntelIPP

    Public Plotter As cZEDGraphService

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("1. Exposure type")>
    <ComponentModel.Description("Light, Bias, Dark, Flat, or Tricolor.")>
    Public Property ExposureType As String = "Light Frame"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("2. Read-out mode")>
    <ComponentModel.Description("Photographic, high-gain.")>
    Public Property ReadOutMode As eReadOutMode = eReadOutMode.Photographic

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("2.1. Strema mode")>
    <ComponentModel.Description("Photo (0) or Video(1).")>
    Public Property StreamMode As UInteger = 0

    '''<summary>Exposure time [s].</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("3. Exposure time [s]")>
    <ComponentModel.Description("Exposure time [s]")>
    Public Property ExposureTime As Double = 1.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("4. Gain")>
    Public Property Gain As Double = 26.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("5. Offset")>
    Public Property Offset As Double = 50.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("6. Target T - enter <-100 for do-not-use")>
    Public Property TargetTemp As Double = -300

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("7. Binning")>
    <ComponentModel.Description("Binning (NxN)")>
    Public Property Binning As UInteger = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("8. # of captures")>
    Public Property CaptureCount As Integer = 1

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("1. GUID of the sequence")>
    <ComponentModel.Description("Sequence GUID")>
    Public Property GUID As String = System.Guid.NewGuid().ToString.Replace("-", String.Empty)

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("2. Author")>
    <ComponentModel.Description("Author")>
    Public Property Author As String = "Martin Weiss"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("3. Origin")>
    <ComponentModel.Description("Origin")>
    Public Property Origin As String = "Sternwarte Holzkirchen"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("4. Store captured image?")>
    Public Property StoreImage As Boolean = True

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("5. File name start")>
    Public Property FileName As String = "QHY_capture_"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("6. FITS extenstion")>
    Public Property FITSExtension As String = "fits"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("7. Open image automatically?")>
    Public Property AutoOpenImage As Boolean = True

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("1. Telescope used")>
    Public Property Telescope As String = "Planewave CDK 12.5 with reducer"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("2. Telescope aperture [mm]")>
    Public Property TelescopeAperture As Double = 130.0

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("3. Telescope focal length [mm]")>
    Public Property TelescopeFocalLength As Double = 800.0

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("1. Object name")>
    Public Property ObjectName As String = "---"

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("2. Object RA")>
    Public Property ObjectRA As String = "HH:MM:SS.sss"

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("3. Object DEC")>
    Public Property ObjectDEC As String = "dd:mm:ss.sss"

    <ComponentModel.Category(Cat5)>
    <ComponentModel.DisplayName("1. Log camera properties?")>
    Public Property Log_CamProp As Boolean = False

    <ComponentModel.Category(Cat5)>
    <ComponentModel.DisplayName("2. Log timing?")>
    Public Property Log_Timing As Boolean = False


    Public Property RemoveOverscan As Boolean = False

End Class
