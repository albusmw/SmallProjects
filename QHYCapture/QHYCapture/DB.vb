Option Explicit On
Option Strict On

Public Structure sRect_UInt
    Dim X As UInteger
    Dim Y As UInteger
    Dim Width As UInteger
    Dim Height As UInteger
End Structure

'''<summary>Filter as to be send as ASCII string.</summary>
Public Enum eFilter As Byte
    L = 0
    R = 1
    G = 2
    B = 3
    H_alpha = 4
End Enum

Public Enum eStreamMode As UInteger
    SingleFrame = 0
    LiveFrame = 1
End Enum

Public Enum eReadOutMode As UInteger
    Photographic = 0
    HighGain = 1
    ExtendFullwell = 2
    Unvalid = UInteger.MaxValue
End Enum

'''<summary>Database holding relevant information.</summary>
Public Class cDB

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property MyINI As String = System.IO.Path.Combine(New String() {MyPath, "Config.INI"})

    <ComponentModel.Browsable(False)>
    Public Log_Generic As New Text.StringBuilder

    '''<summary>INI access object.</summary>
    Public INI As New Ato.cINI_IO

    '''<summary>Flag that indicates the sequence is running.</summary>
    <ComponentModel.Browsable(False)>
    Public Property RunningFlag As Boolean = False

    '''<summary>Flag that indicates to halt the capture sequence.</summary>
    <ComponentModel.Browsable(False)>
    Public Property StopFlag As Boolean = False

    'WCF
    Public SetupWCF As ServiceModel.Web.WebServiceHost
    Public serviceBehavior As ServiceModel.Description.ServiceDebugBehavior

    Public IPPRoots As String() = {"C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.5.281\windows\redist\intel64\ipp", "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2019.1.144\windows\redist\intel64_win\ipp\"}
    Public IPP As cIntelIPP

    Public Plotter As cZEDGraphService

    Const Cat1 As String = "1. Imaging hardware"
    Const Cat2 As String = "2. Exposure"
    Const Cat3 As String = "3. Image storage"
    Const Cat4 As String = "4. Debug and logging"

    '''<summary>Camera to search for.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   1. Camera to search")>
    <ComponentModel.Description("Search string for the camera - string must occure in the CameraID.")>
    <ComponentModel.DefaultValue("QHY600M")>
    Public Property CamToUse As String = "QHY600M"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   2. Read-out mode")>
    <ComponentModel.Description("Photographic, high-gain.")>
    Public Property ReadOutMode As eReadOutMode = eReadOutMode.Photographic

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   3. Stream mode")>
    <ComponentModel.Description("Photo (0) or Video(1).")>
    Public Property StreamMode As eStreamMode = eStreamMode.SingleFrame

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   4. Target Temp")>
    <ComponentModel.Description("Enter <-100 for do-not-use")>
    <ComponentModel.DefaultValue(-300)>
    Public Property TargetTemp As Double = -300

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   5. Binning")>
    <ComponentModel.Description("Binning (NxN)")>
    <ComponentModel.DefaultValue(1)>
    Public Property Binning As UInteger = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   6. ROI")>
    <ComponentModel.Description("ROI (without binning)")>
    Public Property ROI As New Drawing.Rectangle(0, 0, 0, 0)

    '===================================================================================================

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   1. # of captures")>
    <ComponentModel.DefaultValue(1)>
    Public Property CaptureCount As Integer = 1

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   2. Filter slot")>
    Public Property FilerSlot As eFilter = eFilter.H_alpha

    '''<summary>Exposure time [s].</summary>
    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   3. Exposure time [s]")>
    <ComponentModel.Description("Exposure time [s]")>
    <ComponentModel.DefaultValue(1.0)>
    Public Property ExposureTime As Double = 1.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   4. Gain")>
    <ComponentModel.Description("Gain to set")>
    <ComponentModel.DefaultValue(26.0)>
    Public Property Gain As Double = 26.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   5. Offset")>
    <ComponentModel.Description("Offset to set")>
    <ComponentModel.DefaultValue(50.0)>
    Public Property Offset As Double = 50.0

    '===================================================================================================

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   1. Store captured image?")>
    <ComponentModel.Description("Store the captured image")>
    <ComponentModel.DefaultValue(True)>
    Public Property StoreImage As Boolean = True

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   2. Remove overscan?")>
    <ComponentModel.Description("Remove the overscan area in the stored file")>
    <ComponentModel.DefaultValue(False)>
    Public Property RemoveOverscan As Boolean = False

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   3. File name start")>
    <ComponentModel.Description("File name start to use")>
    <ComponentModel.DefaultValue("QHY_capture_")>
    Public Property FileName As String = "QHY_capture_"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   4. FITS extenstion")>
    <ComponentModel.Description("Extension to use for FITS files")>
    <ComponentModel.DefaultValue("fits")>
    Public Property FITSExtension As String = "fits"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   5. Open image automatically?")>
    <ComponentModel.Description("Automaticall open a stored FITS file with the default editor")>
    <ComponentModel.DefaultValue(True)>
    Public Property AutoOpenImage As Boolean = True

    '===================================================================================================

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   1. Log camera properties?")>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_CamProp As Boolean = False

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   2. Log timing?")>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_Timing As Boolean = False

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   3. Log verbose?")>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_Verbose As Boolean = False

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   4. Clear statistics log?")>
    <ComponentModel.Description("Clear statistics log on every measurement")>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_ClearStat As Boolean = False

End Class

'''<summary>Database holding meta data information.</summary>
Public Class cDB_meta

    Const Cat1 As String = "1. Generic"
    Const Cat2 As String = "2. Object"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   0. 10Micron IP")>
    <ComponentModel.Description("IP of the 10Micron mount")>
    Public Property IP_10Micron As String = "192.168.10.119"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   1. GUID of the sequence")>
    <ComponentModel.Description("Sequence GUID")>
    Public Property GUID As String = System.Guid.NewGuid().ToString.Replace("-", String.Empty)

    '''<summary>Exposure type.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   2. Exposure type")>
    <ComponentModel.Description("Light, Bias, Dark, Flat, or Tricolor.")>
    Public Property ExposureType As String = "Light Frame"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   2. Author")>
    <ComponentModel.Description("Author to add to the meta data.")>
    Public Property Author As String = "Martin Weiss"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   3. Origin")>
    <ComponentModel.Description("Origin to add to the meta data.")>
    Public Property Origin As String = "Sternwarte Holzkirchen"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   4. Telescope used")>
    <ComponentModel.Description("Telescope name to add to the meta data.")>
    Public Property Telescope As String = "Planewave CDK 12.5 with reducer"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   5. Telescope aperture [mm]")>
    <ComponentModel.Description("Telescope aperture to add to the meta data.")>
    Public Property TelescopeAperture As Double = 317.0

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   6. Telescope focal length [mm]")>
    <ComponentModel.Description("Telescope focal length to add to the meta data.")>
    Public Property TelescopeFocalLength As Double = 1676.4

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   1. Name")>
    <ComponentModel.Description("Name of the object (NGC1234, ...).")>
    <ComponentModel.DefaultValue("---")>
    Public Property ObjectName As String = "---"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   2. RA")>
    <ComponentModel.DefaultValue("HH:MM:SS.sss")>
    Public Property ObjectRA As String = "HH:MM:SS.sss"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   3. DEC")>
    <ComponentModel.DefaultValue("dd:mm:ss.sss")>
    Public Property ObjectDEC As String = "dd:mm:ss.sss"

End Class
