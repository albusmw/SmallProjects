Option Explicit On
Option Strict On

'''<summary>Characteristics data of one single capture.</summary>
Public Class cSingleCaptureData
    '''<summary>Running capture index.</summary>
    Public CaptureIdx As UInt32 = 0
    '''<summary>Temperature [°C] at start of exposure.</summary>
    Public ObsStartTemp As Double = Double.NaN
    '''<summary>Selected filter.</summary>
    Public FilterActive As eFilter = eFilter.Invalid
    '''<summary>Telescope focus position.</summary>
    Public TelescopeFocus As Double = Double.NaN
    '''<summary>Time at observation start.</summary>
    Public ObsStart As DateTime = Now
    '''<summary>Time at observation end.</summary>
    Public ObsEnd As DateTime = DateTime.MinValue
    '''<summary>Selected QHY read-out mode.</summary>
    Public CamReadOutMode As New Text.StringBuilder
    '''<summary>Exposure time [s].</summary>
    Public ExpTime As Double = Double.NaN
    '''<summary>Selected gain value.</summary>
    Public Gain As Double = Double.NaN
    '''<summary>Selected offset value.</summary>
    Public Offset As Double = Double.NaN
    '''<summary>Selected brightness value.</summary>
    Public Brightness As Double = Double.NaN
    '''<summary>Number of pixel in X direction (bigger axis).</summary>
    Public NAXIS1 As UInteger = 0
    '''<summary>Number of pixel in Y direction (bigger axis).</summary>
    Public NAXIS2 As UInteger = 0
End Class

'''<summary>Size description.</summary>
Public Structure sSize_UInt
    Dim Width As UInteger
    Dim Height As UInteger
End Structure

'''<summary>Size description.</summary>
Public Structure sSize_Dbl
    Dim Width As Double
    Dim Height As Double
End Structure

'''<summary>Rectangle description.</summary>
Public Structure sRect_UInt
    Dim X As UInteger
    Dim Y As UInteger
    Dim Width As UInteger
    Dim Height As UInteger
End Structure

'''<summary>Read-out resolution.</summary>
Public Enum eReadResolution
    Res8Bit = 0
    Res16Bit = 16
End Enum

'''<summary>Mode for X axis scaling mode.</summary>
Public Enum eXAxisScalingMode
    Auto
    MaxScale
    LeaveAsIs
End Enum

'''<summary>Filter as to be send as ASCII string.</summary>
Public Enum eFilter As Byte
    Invalid = 0
    L = 1
    R = 2
    G = 3
    B = 4
    H_alpha = 5
End Enum

'''<summary>Available stream modes.</summary>
Public Enum eStreamMode As UInteger
    SingleFrame = 0
    LiveFrame = 1
    Invalid = UInteger.MaxValue
End Enum

'''<summary>Available readout modes.</summary>
Public Enum eReadOutMode As UInteger
    Photographic = 0
    HighGain = 1
    ExtendFullwell = 2
    Invalid = UInteger.MaxValue
End Enum

'''<summary>Database holding relevant information.</summary>
Public Class cDB

    Public Stopper As New cStopper

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property EXEPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property MyINI As String = System.IO.Path.Combine(New String() {EXEPath, "Config.INI"})

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
    Const Cat4 As String = "4. Plot and statistics"
    Const Cat5 As String = "4. Debug and logging"

    '''<summary>Camera to search for.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   a) Camera to search")>
    <ComponentModel.Description("Search string for the camera - string must occure in the CameraID.")>
    <ComponentModel.DefaultValue("QHY600M")>
    Public Property CamToUse As String = "QHY600M"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   b) Read-out mode")>
    <ComponentModel.Description("Photographic, high-gain.")>
    <ComponentModel.DefaultValue(eReadOutMode.Photographic)>
    Public Property ReadOutMode As eReadOutMode = eReadOutMode.Photographic

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   c) Stream mode")>
    <ComponentModel.Description("Photo (0) or Video(1).")>
    Public Property StreamMode As eStreamMode = eStreamMode.SingleFrame

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   d) Target Temp")>
    <ComponentModel.Description("Enter <-100 for do-not-use")>
    <ComponentModel.DefaultValue(-300.0)>
    Public Property TargetTemp As Double = -300.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   e) Binning")>
    <ComponentModel.Description("Binning (NxN)")>
    <ComponentModel.DefaultValue(1)>
    Public Property Binning As Integer = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   f) Read resolution")>
    <ComponentModel.Description("Read resolution")>
    <ComponentModel.DefaultValue(eReadResolution.Res16Bit)>
    Public Property ReadResolution As eReadResolution = eReadResolution.Res16Bit

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   g) ROI")>
    <ComponentModel.Description("ROI (without binning)")>
    Public Property ROI As New Drawing.Rectangle(0, 0, 0, 0)

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   h) USB traffic")>
    <ComponentModel.Description("USB traffic - 0 ... <1> ... 60")>
    <ComponentModel.DefaultValue(0.0)>
    Public Property USBTraffic As Double = 0.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   i) DDR RAM")>
    <ComponentModel.Description("Use DDR RAM?")>
    <ComponentModel.DefaultValue(True)>
    Public Property DDR_RAM As Boolean = True

    '===================================================================================================

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   a) # of captures")>
    <ComponentModel.DefaultValue(1)>
    Public Property CaptureCount As Int32 = 1

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   b) Filter slot")>
    Public Property FilterSlot As eFilter = eFilter.H_alpha

    '''<summary>Exposure time [s].</summary>
    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   c) Exposure time [s]")>
    <ComponentModel.Description("Exposure time [s]")>
    <ComponentModel.DefaultValue(1.0)>
    Public Property ExposureTime As Double = 1.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   d) Gain")>
    <ComponentModel.Description("Gain to set - 0 ... <1> ... 200")>
    <ComponentModel.DefaultValue(26.0)>
    Public Property Gain As Double = 26.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   e) Offset")>
    <ComponentModel.Description("Offset to set - 0 ... <1> ... 255")>
    <ComponentModel.DefaultValue(50.0)>
    Public Property Offset As Double = 50.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   f) Remove overscan")>
    <ComponentModel.Description("Remove the overscan area in the stored file")>
    <ComponentModel.DefaultValue(False)>
    Public Property RemoveOverscan As Boolean = False

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("  g) Config for each capture")>
    <ComponentModel.Description("Write all exposure data on each exposure start?")>
    <ComponentModel.DefaultValue(True)>
    Public Property ConfigAlways As Boolean = True

    '===================================================================================================

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   a) Store captured image")>
    <ComponentModel.Description("Store the captured image")>
    <ComponentModel.DefaultValue(True)>
    Public Property StoreImage As Boolean = True

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   b) Storage path")>
    <ComponentModel.Description("Root path to store images")>
    <ComponentModel.DefaultValue("C:\DATA_IO\QHYCapture")>
    Public Property StoragePath As String = "C:\DATA_IO\QHYCapture"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   c) File name start")>
    <ComponentModel.Description("File name start to use")>
    <ComponentModel.DefaultValue("QHY600_$FILT$_$EXP$_$GAIN$_$OFFS$_$IDX$_$CNT$_$RMODE$")>
    Public Property FileName As String = "QHY600_$FILT$_$EXP$_$GAIN$_$OFFS$_$IDX$_$CNT$_$RMODE$"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   d) FITS extenstion")>
    <ComponentModel.Description("Extension to use for FITS files")>
    <ComponentModel.DefaultValue("fits")>
    Public Property FITSExtension As String = "fits"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   e) Open image automatically?")>
    <ComponentModel.Description("Automaticall open a stored FITS file with the default editor")>
    <ComponentModel.DefaultValue(False)>
    Public Property AutoOpenImage As Boolean = False

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName("   f) Show live image")>
    <ComponentModel.Description("Show a live image?")>
    <ComponentModel.DefaultValue(False)>
    Public Property ShowLiveImage As Boolean = False

    '===================================================================================================

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   a) Single statistics log")>
    <ComponentModel.Description("Clear statistics log on every measurement")>
    <ComponentModel.DefaultValue(True)>
    Public Property Log_ClearStat As Boolean = True

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   b) Plot single statistics")>
    <ComponentModel.DefaultValue(True)>
    Public Property PlotSingleStatistics As Boolean = True

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   c) Plot mean statistics")>
    <ComponentModel.DefaultValue(True)>
    Public Property PlotMeanStatistics As Boolean = True

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName("   d) Plot limits fixed")>
    <ComponentModel.Description("True to auto-scale on min and max ADU, false to scale on data min and max")>
    <ComponentModel.DefaultValue(False)>
    Public Property PlotLimitMode As eXAxisScalingMode = eXAxisScalingMode.Auto

    '===================================================================================================

    <ComponentModel.Category(Cat5)>
    <ComponentModel.DisplayName("   1. Log camera properties")>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_CamProp As Boolean = False

    <ComponentModel.Category(Cat5)>
    <ComponentModel.DisplayName("   2. Log timing")>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_Timing As Boolean = False

    <ComponentModel.Category(Cat5)>
    <ComponentModel.DisplayName("   3. Log verbose")>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_Verbose As Boolean = False



    '===================================================================================================

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property ROISet() As Boolean
        Get
            If ROI.X = 0 And ROI.Y = 0 And ROI.Width = 0 And ROI.Height = 0 Then Return False Else Return True
        End Get
    End Property

End Class

'''<summary>Database holding meta data information.</summary>
Public Class cDB_meta

    Const Cat1 As String = "1. Generic"
    Const Cat2 As String = "2. Object"
    Const NotSet As String = "-----"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   0. 10Micron IP")>
    <ComponentModel.Description("IP of the 10Micron mount")>
    Public Property IP_10Micron As String = "192.168.10.119"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   1. GUID of the sequence")>
    <ComponentModel.Description("Sequence GUID")>
    Public Property GUID As String = String.Empty

    '''<summary>Exposure type.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   2. Exposure type")>
    <ComponentModel.Description("Light, Bias, Dark, Flat, Tricolor or TestOnly.")>
    <ComponentModel.DefaultValue("TestOnly")>
    Public Property ExposureType As String = "TestOnly"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   2. Author")>
    <ComponentModel.Description("Author to add to the meta data.")>
    <ComponentModel.DefaultValue("Martin Weiss")>
    Public Property Author As String = "Martin Weiss"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   3. Site longitude")>
    <ComponentModel.Description("Longitude of the site.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property SiteLongitude As String = NotSet

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   4. Site latitude")>
    <ComponentModel.Description("Latitude of the site.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property SiteLatitude As String = NotSet

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   5. Origin")>
    <ComponentModel.Description("Origin to add to the meta data.")>
    <ComponentModel.DefaultValue("Sternwarte Holzkirchen")>
    Public Property Origin As String = "Sternwarte Holzkirchen"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   6. Telescope used")>
    <ComponentModel.Description("Telescope name to add to the meta data.")>
    <ComponentModel.DefaultValue("Planewave CDK 12.5 with reducer")>
    Public Property Telescope As String = "CDK 12.5 with reducer"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   7. Telescope aperture [mm]")>
    <ComponentModel.Description("Telescope aperture to add to the meta data.")>
    <ComponentModel.DefaultValue(317.0)>
    Public Property TelescopeAperture As Double = 317.0

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   8. Telescope focal length [mm]")>
    <ComponentModel.Description("Telescope focal length to add to the meta data.")>
    <ComponentModel.DefaultValue(1676.4)>
    Public Property TelescopeFocalLength As Double = 1676.4

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName("   9. Telescope focus position")>
    <ComponentModel.Description("Focuser position reported by the telescope.")>
    <ComponentModel.DefaultValue(Double.NaN)>
    Public Property TelescopeFocus As Double = Double.NaN

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   1. Name")>
    <ComponentModel.Description("Name of the object (NGC1234, ...).")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property ObjectName As String = NotSet

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   2. RA")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeRightAscension As String = NotSet

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   3. DEC")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeDeclination As String = NotSet

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   4. Altitude")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeAltitude As String = NotSet

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   5. Azimut")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeAzimuth As String = NotSet

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName("   6. Load 10Micron data")>
    <ComponentModel.DefaultValue(False)>
    Public Property Load10MicronDataAlways As Boolean = False

End Class
