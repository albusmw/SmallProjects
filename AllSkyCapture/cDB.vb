Option Explicit On
Option Strict On

Public Class cDB

    Private ThisNightParam As AstroCalc.NET.Sun.sSunRaiseAndSet

    '''<summary>Location of the EXE.</summary>
    Private ReadOnly MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

    Private Const Cat_TimeControl As String = "1.) Time control"
    Private Const Cat_ExposureControl As String = "2.) Exposure control"
    Private Const Cat_Inprints As String = "3.) Inprints"
    Private Const Cat_FileSystem As String = "4.) File system"
    Private Const Cat_Misc As String = "5.) Misc"
    Private Const Cat_Sun As String = "6.) Sun controlled"

    Public ReadOnly GainNotSet As Short = Short.MinValue + 1

    <ComponentModel.Category(Cat_TimeControl)>
    <ComponentModel.DisplayName("1.) Capture interval [s]")>
    <ComponentModel.Description("Set to 0 to disable.")>
    <ComponentModel.DefaultValue(0.0)>
    Public Property CaptureInterval As Double = 0.0

    <ComponentModel.Category(Cat_TimeControl)>
    <ComponentModel.DisplayName("2.) Max sun height")>
    <ComponentModel.Description("Capture only for specified sun height.")>
    <ComponentModel.DefaultValue(5.0)>
    Public Property MaxSunHeight As Double = 5.0

    <ComponentModel.Category(Cat_TimeControl)>
    <ComponentModel.DisplayName("3.) File name format")>
    <ComponentModel.Description("File name format or %%%% for digits only.")>
    <ComponentModel.DefaultValue("dd.MM.yyyy_HH.mm.ss")>
    Public Property FileNameFormat As String = "dd.MM.yyyy_HH.mm.ss"

    <ComponentModel.Category(Cat_ExposureControl)>
    <ComponentModel.DisplayName("1.) Selected ASCOM cam")>
    <ComponentModel.DefaultValue("ASCOM.ASICamera2.Camera")>
    Public Property ASCOMCam As String = "ASCOM.ASICamera2.Camera"

    <ComponentModel.Category(Cat_ExposureControl)>
    <ComponentModel.DisplayName("2.) Exposure time")>
    <ComponentModel.DefaultValue(5.0)>
    Public Property ExposureTime As Double = 10.0

    <ComponentModel.Category(Cat_ExposureControl)>
    <ComponentModel.DisplayName("3.) Gain")>
    <ComponentModel.DefaultValue(300)>
    Public Property SelectedGain As Short = 100


    <ComponentModel.Category(Cat_Inprints)>
    <ComponentModel.DisplayName("1.) Font size")>
    <ComponentModel.DefaultValue(24)>
    Public Property Inprint_FontSize As Integer = 24

    <ComponentModel.Category(Cat_Inprints)>
    <ComponentModel.DisplayName("2) Station")>
    <ComponentModel.DefaultValue("Sternwarte Holzkirchen")>
    Public Property Inprint_station As String = "Sternwarte Holzkirchen"

    <ComponentModel.Category(Cat_FileSystem)>
    <ComponentModel.DisplayName("1) Storage root")>
    <ComponentModel.DefaultValue("C:\DATA_IO\AllSky")>
    Public Property StorageRoot As String = "C:\DATA_IO\AllSky"

    <ComponentModel.Category(Cat_FileSystem)>
    <ComponentModel.DisplayName("2) Default file name")>
    <ComponentModel.DefaultValue("AllSkyImage")>
    Public Property CurrentImageName As String = "AllSkyImage"

    <ComponentModel.Category(Cat_FileSystem)>
    <ComponentModel.DisplayName("3.1) Save as JPG")>
    <ComponentModel.DefaultValue(True)>
    Public Property SaveAsJPG As Boolean = True

    <ComponentModel.Category(Cat_FileSystem)>
    <ComponentModel.DisplayName("3.2) Save as PNG")>
    <ComponentModel.DefaultValue(False)>
    Public Property SaveAsPNG As Boolean = False

    <ComponentModel.Category(Cat_FileSystem)>
    <ComponentModel.DisplayName("4) Save grayscales as indexed")>
    <ComponentModel.DefaultValue(True)>
    Public Property SaveIndexedGrayscale As Boolean = True

    <ComponentModel.Category(Cat_Misc)>
    <ComponentModel.DisplayName("1) FFMPEG EXE path")>
    Public Property FFMPEGEXE As String = "C:\BIN\ffmpeg-4.3.1-essentials_build\bin\ffmpeg.exe"

    <ComponentModel.Category(Cat_Misc)>
    <ComponentModel.DisplayName("2) Station latitude")>
    <ComponentModel.DefaultValue(47.878355)>
    Public Property Station_Latitude As Double = 47.878355

    <ComponentModel.Category(Cat_Misc)>
    <ComponentModel.DisplayName("3) Station longitude")>
    <ComponentModel.DefaultValue(11.691598)>
    Public Property Station_Longitude As Double = 11.691598

    '==================================================================================================

    <ComponentModel.Category(Cat_Sun)>
    <ComponentModel.DisplayName("1) Sun set")>
    Public ReadOnly Property SunSet As DateTime
        Get
            Return ThisNightParam.SunSet
        End Get
    End Property

    <ComponentModel.Category(Cat_Sun)>
    <ComponentModel.DisplayName("2) Sun raise")>
    Public ReadOnly Property SunRaise As DateTime
        Get
            Return ThisNightParam.SunRaise
        End Get
    End Property

    '==================================================================================================

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property SunAzimut As Double
        Get
            Return MySunAzimut
        End Get
    End Property
    Private MySunAzimut As Double = Double.NaN

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property SunHeight As Double
        Get
            Return MySunHeight
        End Get
    End Property
    Private MySunHeight As Double = Double.NaN

    '''<summary>Update database value for azimut and height.</summary>
    Public Sub CalcSunPos()
        AstroCalc.NET.Sun.SunPos(Now, Station_Longitude, Station_Latitude, MySunAzimut, MySunHeight)
    End Sub

    '''<summary>Calculate sun parameters for "this night".</summary>
    Public Sub CalcSunParam()
        ThisNightParam = AstroCalc.NET.Sun.NightPreCalc(Station_Longitude, Station_Latitude)
    End Sub

End Class
