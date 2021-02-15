Option Explicit On
Option Strict On
Imports System.Runtime.InteropServices

Public Class M
    '''<summary>DB that holds all relevant information.</summary>
    Public Shared WithEvents DB As New cDB
    '''<summary>DB that holds meta information.</summary>
    Public Shared WithEvents Meta As New cDB_meta
    '''<summary>DB that holds report information.</summary>
    Public Shared WithEvents Report As New cAstroStatDisp
End Class

'''<summary>Characteristics data of one single capture.</summary>
Public Class cSingleCaptureInfo
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
    Public NAXIS1 As Integer = 0
    '''<summary>Number of pixel in Y direction (bigger axis).</summary>
    Public NAXIS2 As Integer = 0
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
    <ComponentModel.Description("8-bit")>
    Res8Bit = 0
    <ComponentModel.Description("16-bit")>
    Res16Bit = 16
End Enum

'''<summary>Mode for X axis scaling mode.</summary>
Public Enum eXAxisScalingMode
    <ComponentModel.Description("Automatic")>
    Auto
    <ComponentModel.Description("Full 16 bit range")>
    FullRange16Bit
    <ComponentModel.Description("Leave as is")>
    LeaveAsIs
End Enum

'''<summary>Filter as to be send as ASCII string.</summary>
Public Enum eFilter As Byte
    <ComponentModel.Description("Unchanged")>
    Invalid = 0
    <ComponentModel.Description("Light")>
    L = 1
    <ComponentModel.Description("Red")>
    R = 2
    <ComponentModel.Description("Green")>
    G = 3
    <ComponentModel.Description("Blue")>
    B = 4
    <ComponentModel.Description("H alpha")>
    H_alpha = 5
    <ComponentModel.Description("S-II")>
    S_II = 6
    <ComponentModel.Description("O_III")>
    O_III = 7
    <ComponentModel.Description("Empty")>
    Empty = 9
End Enum

'''<summary>Available stream modes.</summary>
Public Enum eStreamMode As UInteger
    <ComponentModel.Description("Single frame")>
    SingleFrame = 0
    <ComponentModel.Description("Live")>
    LiveFrame = 1
    <ComponentModel.Description("--INVALID--")>
    Invalid = UInteger.MaxValue
End Enum

'''<summary>Available readout modes.</summary>
Public Enum eReadOutMode As UInteger
    <ComponentModel.Description("Photographic")>
    Photographic = 0
    <ComponentModel.Description("High Gain")>
    HighGain = 1
    <ComponentModel.Description("Extend Fullwell")>
    ExtendFullwell = 2
    <ComponentModel.Description("--INVALID--")>
    Invalid = UInteger.MaxValue
End Enum

'''<summary>Database holding relevant information.</summary>
Public Class cDB

    Public Event PropertyChanged()

    '''<summary>Handle to the camera.</summary>
    Public CamHandle As IntPtr = IntPtr.Zero
    '''<summary>Currently used camera ID.</summary>
    Public UsedCameraId As System.Text.StringBuilder
    '''<summary>Currently used read-out mode.</summary>
    Public UsedReadMode As eReadOutMode = eReadOutMode.Invalid
    '''<summary>Currently used stream mode.</summary>
    Public UsedStreamMode As eStreamMode = eStreamMode.Invalid
    '''<summary>Used to call BeginQHYLiveMode only once.</summary>
    Public LiveModeInitiated As Boolean = False
    '''<summary>Used to call BeginQHYLiveMode only once.</summary>
    Public LastStoredFile As String = String.Empty
    '''<summary>Monitor for the MIDI events.</summary>
    Private WithEvents MIDI As cMIDIMonitor

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

    '''<summary>Intel IPP access.</summary>
    Public IPP As cIntelIPP

    Const Cat1 As String = "1. Imaging hardware"
    Const Cat2 As String = "2. Exposure"
    Const Cat3 As String = "3. Image storage"
    Const Cat4 As String = "4. Statistics calculation"
    Const Cat5 As String = "5. Debug and logging"
    Const Cat6 As String = "6. Exposure - Advanced"
    Const Cat7 As String = "7. Focus star search"
    Const Indent As String = "  "

    Public Sub New()
        'MIDI monitor
        MIDI = New cMIDIMonitor
        If MIDI.MIDIDeviceCount > 0 Then MIDI.SelectMidiDevice(0)
    End Sub

    '''<summary>Camera to search for.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "1. Camera to search")>
    <ComponentModel.Description("Search string for the camera - string must occure in the CameraID. Use * to use first found camera.")>
    <ComponentModel.DefaultValue("*")>
    Public Property CamToUse As String = "600M"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "2. Read-out mode")>
    <ComponentModel.Description("Photographic or high-gain.")>
    <ComponentModel.DefaultValue(GetType(eReadOutMode), "Photographic")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property ReadOutMode As eReadOutMode = eReadOutMode.Photographic

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "3. Stream mode")>
    <ComponentModel.Description("Photo or Video.")>
    <ComponentModel.DefaultValue(GetType(eStreamMode), "SingleFrame")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property StreamMode As eStreamMode = eStreamMode.SingleFrame

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.1 Target Temp")>
    <ComponentModel.Description("Target temperature to cool to; enter <-100 for do-not-use")>
    <ComponentModel.DefaultValue(-10.0)>
    Public Property Temp_Target As Double = -10.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.2 Target Temp - tolerance")>
    <ComponentModel.Description("Tolerable temperature error")>
    <ComponentModel.DefaultValue(0.1)>
    Public Property Temp_Tolerance As Double = 0.1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.3 Target Temp - stable time")>
    <ComponentModel.Description("Time [s] to be within tolerance to mark cooling as done")>
    <ComponentModel.DefaultValue(60.0)>
    Public Property Temp_StableTime As Double = 60.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.4. Cooling time-out")>
    <ComponentModel.Description("Time [s] after which the cooling is finished even if the target temperature is NOT reached")>
    <ComponentModel.DefaultValue(60.0)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    Public Property Temp_TimeOutAndOK As Double = 60.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "5.1 Binning - Hardware")>
    <ComponentModel.Description("Hardware Binning (NxN)")>
    <ComponentModel.DefaultValue(1)>
    Public Property HardwareBinning As Integer = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "5.2 Binning - Software")>
    <ComponentModel.Description("Software Binning (NxN)")>
    <ComponentModel.DefaultValue(1)>
    Public Property SoftwareBinning As Integer = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "6. Read resolution")>
    <ComponentModel.Description("Bit per pixel resolution")>
    <ComponentModel.DefaultValue(eReadResolution.Res16Bit)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property ReadResolution As eReadResolution = eReadResolution.Res16Bit

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "7. ROI")>
    <ComponentModel.Description("ROI (without binning)")>
    <ComponentModel.DefaultValue(GetType(Drawing.Rectangle), "0, 0, 0, 0")>
    Public Property ROI As New Drawing.Rectangle(0, 0, 0, 0)

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "8. USB traffic")>
    <ComponentModel.Description("USB traffic - 0 is fastest, maximum value depends on the camera")>
    <ComponentModel.DefaultValue(20.0)>
    Public Property USBTraffic As Double = 20.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "9. DDR RAM")>
    <ComponentModel.Description("Use DDR RAM?")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property DDR_RAM As Boolean = True

    '===================================================================================================

    '''<summary>Number of exposured to take with identical settings.</summary>
    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "1. # of captures")>
    <ComponentModel.Description("Number of exposured to take with identical settings.")>
    <ComponentModel.DefaultValue(1)>
    Public Property CaptureCount As Integer = 1

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "2.1. Filter wheel")>
    <ComponentModel.Description("Configure if a real filter wheel should be controlled or if filter is just used in meta data, ....")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property UseFilterWheel As Boolean = True

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "2.2. Filter slot")>
    <ComponentModel.Description("Filter slot to select.")>
    <ComponentModel.DefaultValue(eFilter.Invalid)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property FilterSlot As eFilter = eFilter.Invalid

    '''<summary>Exposure time [s].</summary>
    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "3. Exposure time [s]")>
    <ComponentModel.Description("Exposure time [s] for each exposure")>
    <ComponentModel.DefaultValue(1.0)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    Public Property ExposureTime As Double = 1.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "4. Gain")>
    <ComponentModel.Description("Gain to set")>
    <ComponentModel.DefaultValue(26.0)>
    Public Property Gain As Double = 26.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "5. Offset")>
    <ComponentModel.Description("Offset to set")>
    <ComponentModel.DefaultValue(50.0)>
    Public Property Offset As Double = 50.0

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "6. Remove overscan")>
    <ComponentModel.Description("Remove the overscan area in the stored data and file")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property RemoveOverscan As Boolean = True

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "7. Config for each capture")>
    <ComponentModel.Description("Write all exposure data to the camera on each exposure start (takes some ms ...)")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property ConfigAlways As Boolean = True

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "8. Close cam on each xml exp")>
    <ComponentModel.Description("Close and re-open the camera after each exposure XML entry")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property CloseCam As Boolean = False

    '===================================================================================================

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "1. Store captured image")>
    <ComponentModel.Description("Store the captured image on harddisc")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StoreImage As Boolean = True

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "2. Storage path")>
    <ComponentModel.Description("Root path to store images")>
    <ComponentModel.DefaultValue("C:\DATA_IO\QHYCapture")>
    Public Property StoragePath As String = "C:\DATA_IO\QHYCapture"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "3. File name start")>
    <ComponentModel.Description("File name start to use")>
    <ComponentModel.DefaultValue("QHY600_$FILT$_$EXP$_$GAIN$_$OFFS$_$IDX$_$CNT$_$RMODE$")>
    Public Property FileName As String = "QHY600_$FILT$_$EXP$_$GAIN$_$OFFS$_$IDX$_$CNT$_$RMODE$"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "4. FITS extenstion")>
    <ComponentModel.Description("Extension to use for FITS files")>
    <ComponentModel.DefaultValue("fits")>
    Public Property FITSExtension As String = "fits"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "5. Open image automatically?")>
    <ComponentModel.Description("Automaticall open a stored FITS file with the default editor")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property AutoOpenImage As Boolean = False

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "6. Show live image")>
    <ComponentModel.Description("Show a live image?")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property ShowLiveImage As Boolean = False

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "7. Infinit stack mode")>
    <ComponentModel.Description("Stack all images (use for focus or drift analysis)")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StackAll As Boolean = False

    '===================================================================================================

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName(Indent & "1. Calculate mono statistics")>
    <ComponentModel.Description("Clear statistics log on every measurement")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StatMono As Boolean = True

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName(Indent & "2. Calculate color statistics")>
    <ComponentModel.Description("Clear statistics log on every measurement")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StatColor As Boolean = True

    '===================================================================================================

    <ComponentModel.Category(Cat5)>
    <ComponentModel.DisplayName("   d) Intel IPP path")>
    <ComponentModel.Description("This folder contains the IPP (Intel Performance Primitives) that are used to speed-up all calculation processes")>
    <ComponentModel.DefaultValue(False)>
    Public ReadOnly Property Log_IntelIPPPath As String
        Get
            If IsNothing(IPP) = True Then
                Return "--- (nothing)"
            Else
                Return IPP.IPPPath
            End If
        End Get
    End Property

    '===================================================================================================

    <ComponentModel.Category(Cat6)>
    <ComponentModel.DisplayName(Indent & "1. Brightness")>
    <ComponentModel.Description("Brightness control value of the camera - leave at 0.0 for optimum file output results")>
    <ComponentModel.DefaultValue(0.0)>
    Public Property Brightness As Double = 0.0

    <ComponentModel.Category(Cat6)>
    <ComponentModel.DisplayName(Indent & "2. Contrast")>
    <ComponentModel.Description("Contrast control value of the camera - leave at 0.0 for optimum file output results")>
    <ComponentModel.DefaultValue(0.0)>
    Public Property Contrast As Double = 0.0

    <ComponentModel.Category(Cat6)>
    <ComponentModel.DisplayName(Indent & "3. Gamma")>
    <ComponentModel.Description("Gamma control value of the camera - leave at 1.0 for optimum file output results")>
    <ComponentModel.DefaultValue(1.0)>
    Public Property Gamma As Double = 1.0

    <ComponentModel.Category(Cat6)>
    <ComponentModel.DisplayName(Indent & "4.1. White balance - Red")>
    <ComponentModel.Description("White balance RED control value of the camera - leave at 128.0 for optimum file output results")>
    <ComponentModel.DefaultValue(128.0)>
    Public Property WhiteBalance_Red As Double = 128.0

    <ComponentModel.Category(Cat6)>
    <ComponentModel.DisplayName(Indent & "4.2. White balance - Green")>
    <ComponentModel.Description("White balance GREEN control value of the camera - leave at 128.0 for optimum file output results")>
    <ComponentModel.DefaultValue(128.0)>
    Public Property WhiteBalance_Green As Double = 128.0

    <ComponentModel.Category(Cat6)>
    <ComponentModel.DisplayName(Indent & "4.3. White balance - Blue")>
    <ComponentModel.Description("White balance BLUE control value of the camera - leave at 128.0 for optimum file output results")>
    <ComponentModel.DefaultValue(128.0)>
    Public Property WhiteBalance_Blue As Double = 128.0

    <ComponentModel.Category(Cat6)>
    <ComponentModel.DisplayName(Indent & "5. Filter wheel time-out")>
    <ComponentModel.Description("Time [s] after which the filter wheel movement is stoped and the software goes on even if the filter is NOT in place")>
    <ComponentModel.DefaultValue(15.0)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    Public Property FilterWheelTimeOut As Double = 15.0

    '===================================================================================================

    <ComponentModel.Category(Cat7)>
    <ComponentModel.DisplayName(Indent & "1. Auto-star search for auto-focus")>
    <ComponentModel.Description("Auto-select ROI and center on found maximum")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StarSearch As Boolean = False

    <ComponentModel.Category(Cat7)>
    <ComponentModel.DisplayName(Indent & "2. Auto-star search ROI size")>
    <ComponentModel.Description("Size [pixel] of the ROI during start search")>
    <ComponentModel.DefaultValue(5)>
    Public Property StarSearch_Blur As Integer = 5

    <ComponentModel.Category(Cat7)>
    <ComponentModel.DisplayName(Indent & "3. Auto-star search ROI size")>
    <ComponentModel.Description("Size [pixel] of the ROI during start search")>
    <ComponentModel.DefaultValue(41)>
    Public Property StarSearch_ROI As Integer = 41

    '===================================================================================================

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property ROISet() As Boolean
        Get
            If ROI.X = 0 And ROI.Y = 0 And ROI.Width = 0 And ROI.Height = 0 Then Return False Else Return True
        End Get
    End Property

    '''<summary>Handle data entered via a MIDI input device.</summary>
    Private Sub MIDI_Increment(Channel As Integer, Value As Integer) Handles MIDI.Increment
        Select Case Channel
            Case 1
                Gain += Value
            Case 2
                WhiteBalance_Red += Value
            Case 3
                WhiteBalance_Green += Value
            Case 4
                WhiteBalance_Blue += Value
            Case 5
                Contrast += Value / 100
            Case 6
                Brightness += Value / 100
        End Select
        RaiseEvent PropertyChanged()
    End Sub

    Private Sub MIDI_Reset(Channel As Integer) Handles MIDI.Reset
        Select Case Channel
            Case 1
                Gain = 0
            Case 2
                WhiteBalance_Red = 128
            Case 3
                WhiteBalance_Green = 128
            Case 4
                WhiteBalance_Blue = 128
            Case 5
                Contrast = 0.0
            Case 6
                Brightness = 0.0
        End Select
        RaiseEvent PropertyChanged()
    End Sub

End Class

'''<summary>Database holding meta data information.</summary>
Public Class cDB_meta

    Const Cat_10Micron As String = "1. Generic (load via 10Micron)"
    Const Cat2 As String = "3. Object"
    Const Cat_log As String = "4. Software logging"
    Const Cat_read As String = "5. Camera properties"
    Const Indent As String = "  "
    Const NotSet As String = "-----"

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "1. Load 10Micron data?")>
    <ComponentModel.DefaultValue(True)>
    Public Property Load10MicronDataAlways As Boolean = True

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "1.1. 10Micron IP")>
    <ComponentModel.Description("IP of the 10Micron mount")>
    Public Property IP_10Micron As String = "192.168.10.119"

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "1.2. 10Micron Port")>
    <ComponentModel.Description("Port of the 10Micron mount")>
    Public Property IP_10Micron_Port As Integer = 3490

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "1.3. 10Micron Time-out")>
    <ComponentModel.Description("Time-out time for connecting the mount")>
    Public Property IP_10Micron_TimeOut As Integer = 2

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "2.1. Site longitude")>
    <ComponentModel.Description("Longitude of the site.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property SiteLongitude As String = NotSet

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "2.2. Site latitude")>
    <ComponentModel.Description("Latitude of the site.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property SiteLatitude As String = NotSet

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "3.1. RA")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeRightAscension As String = NotSet

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "3.2. DEC")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeDeclination As String = NotSet

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "4.1. Altitude")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeAltitude As String = NotSet

    <ComponentModel.Category(Cat_10Micron)>
    <ComponentModel.DisplayName(Indent & "4.2. Azimut")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeAzimuth As String = NotSet

    '================================================================================

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "1. Name")>
    <ComponentModel.Description("Name of the object (NGC1234, ...).")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property ObjectName As String = NotSet

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "2. GUID of the sequence")>
    <ComponentModel.Description("Sequence GUID")>
    Public Property GUID As String = String.Empty

    '''<summary>Exposure type.</summary>
    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "3. Exposure type")>
    <ComponentModel.Description("Light, Bias, Dark, Flat, Tricolor or TestOnly.")>
    <ComponentModel.DefaultValue("TestOnly")>
    Public Property ExposureType As String = "TestOnly"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "4. Author")>
    <ComponentModel.Description("Author to add to the meta data.")>
    <ComponentModel.DefaultValue("Martin Weiss")>
    Public Property Author As String = "Martin Weiss"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "6. Origin")>
    <ComponentModel.Description("Origin to add to the meta data.")>
    <ComponentModel.DefaultValue("Sternwarte Holzkirchen")>
    Public Property Origin As String = "Sternwarte Holzkirchen"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "7. Telescope used")>
    <ComponentModel.Description("Telescope name to add to the meta data.")>
    <ComponentModel.DefaultValue("CDK 12.5 with reducer")>
    Public Property Telescope As String = "CDK 12.5 with reducer"

    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "8. Telescope aperture [mm]")>
    <ComponentModel.Description("Telescope aperture to add to the meta data.")>
    <ComponentModel.DefaultValue(317.0)>
    Public Property TelescopeAperture As Double = 317.0

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "9. Telescope focal length [mm]")>
    <ComponentModel.Description("Telescope focal length to add to the meta data.")>
    <ComponentModel.DefaultValue(1676.4)>
    Public Property TelescopeFocalLength As Double = 1676.4

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat2)>
    <ComponentModel.DisplayName(Indent & "10. Telescope focus position")>
    <ComponentModel.Description("Focuser position reported by the telescope.")>
    <ComponentModel.DefaultValue(Double.NaN)>
    Public Property TelescopeFocus As Double = Double.NaN

    '===================================================================================================

    <ComponentModel.Category(Cat_log)>
    <ComponentModel.DisplayName(Indent & "1. Log camera properties")>
    <ComponentModel.Description("Log all supported camera properties with name and range in the begin - useful e.g. to see the value range for certain settings")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property Log_CamProp As Boolean = False

    <ComponentModel.Category(Cat_log)>
    <ComponentModel.DisplayName(Indent & "2. Log timing")>
    <ComponentModel.Description("Display a detailed timing log in the end")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property Log_Timing As Boolean = False

    <ComponentModel.Category(Cat_log)>
    <ComponentModel.DisplayName(Indent & "3. Log verbose")>
    <ComponentModel.Description("Log special settings")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property Log_Verbose As Boolean = False

    '===================================================================================================

    <ComponentModel.Category(Cat_read)>
    <ComponentModel.DisplayName(Indent & "1. Chip physical size [mm]")>
    <ComponentModel.Description("Value that was read from the driver")>
    Public ReadOnly Property Chip_Physical As sSize_Dbl
        Get
            Return MyChip_Physical
        End Get
    End Property
    Public MyChip_Physical As New sSize_Dbl

    <ComponentModel.Category(Cat_read)>
    <ComponentModel.DisplayName(Indent & "2. Chip size [pixel]")>
    <ComponentModel.Description("Value that was read from the driver")>
    Public ReadOnly Property Chip_Pixel As sSize_UInt
        Get
            Return MyChip_Pixel
        End Get
    End Property
    Public MyChip_Pixel As New sSize_UInt

    <ComponentModel.Category(Cat_read)>
    <ComponentModel.DisplayName(Indent & "3. Chip pixel size [um]")>
    <ComponentModel.Description("Value that was read from the driver")>
    Public ReadOnly Property Pixel_Size As sSize_Dbl
        Get
            Return MyPixel_Size
        End Get
    End Property
    Public MyPixel_Size As New sSize_Dbl

    <ComponentModel.Category(Cat_read)>
    <ComponentModel.DisplayName(Indent & "4. SDK version")>
    <ComponentModel.Description("Value that was read from the driver")>
    Public ReadOnly Property SDKVersionString As String
        Get
            Dim RetVal As New List(Of String)
            For Each Entry As UInteger In SDKVersion
                RetVal.Add(Entry.ValRegIndep)
            Next Entry
            Return Join(RetVal.ToArray, ".")
        End Get
    End Property
    Public SDKVersion(3) As UInteger

End Class
