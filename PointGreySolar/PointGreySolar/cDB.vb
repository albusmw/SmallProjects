Option Explicit On
Option Strict On

Imports System.ComponentModel

Public Class cDB

    '''<summary>Container for the 16-bit data.</summary>
    Public Container As AstroNET.Statistics

    '''<summary>Histogram display (graph and statistics text).</summary>
    Public Histo As New frmHisto

    '''<summary>IPP class, path and load error message.</summary>
    Public IPP As cIntelIPP
    Public IPPPathToUse As String = String.Empty
    Public IPPLoadError As String = String.Empty

    Public Sub New()
        IPPPathToUse = cIntelIPP.SearchDLLToUse(cIntelIPP.PossiblePaths(EXEPath).ToArray, IPPLoadError)
        IPP = New cIntelIPP(IPPPathToUse)
    End Sub

    <Category("1. Capture control")>
    <DisplayName("1.1) Number of captures")>
    Public Property CaptureCount As Integer = 1

    <Category("1. Capture control")>
    <DisplayName("1.2) Exposure time [ms]")>
    Public Property Exposure As Single = 10.0

    <Category("1. Capture control")>
    <DisplayName("1.3) Gain")>
    Public Property Gain As Single = 0.0

    <Category("1. Capture control")>
    <DisplayName("1.4) Brightness")>
    Public Property Brightness As Single = 0.0

    <Category("1. Capture control")>
    <DisplayName("1.5) Conversion format")>
    Public Property ConvertToFormat As FlyCapture2Managed.PixelFormat = FlyCapture2Managed.PixelFormat.PixelFormatBgr

    <Category("2. Statistics")>
    <DisplayName("2.1) Calculate statistics?")>
    <ComponentModel.DefaultValue(True)>
    Public Property CalcStatistics As Boolean = True

    <Category("2. Statistics")>
    <ComponentModel.DisplayName("a) Mono statistics")>
    <ComponentModel.Description("Calculate the mono statistics (can be of interest if e.g. color balance is applied to a mono image which would be wrong ...)")>
    <ComponentModel.DefaultValue(True)>
    Public Property CalcStat_Mono As Boolean
        Get
            Return Histo.CalcStat_Mono
        End Get
        Set(value As Boolean)
            Histo.CalcStat_Mono = value
        End Set
    End Property

    <Category("2. Statistics")>
    <ComponentModel.DisplayName("b) Bayer statistics")>
    <ComponentModel.Description("Calculate the bayer statistics (can be of interest if e.g. color balance is applied to a mono image which would be wrong ...)")>
    <ComponentModel.DefaultValue(False)>
    Public Property CalcStat_Bayer As Boolean
        Get
            Return Histo.CalcStat_Bayer
        End Get
        Set(value As Boolean)
            Histo.CalcStat_Bayer = value
        End Set
    End Property

    <Category("3. Saving")>
    <DisplayName("3.1) Save TIFF image?")>
    <ComponentModel.DefaultValue(False)>
    Public Property SaveTIFF As Boolean = False

    <Category("3. Saving")>
    <DisplayName("3.1) Save images - FITS?")>
    <ComponentModel.DefaultValue(False)>
    Public Property SaveImagesFITS As Boolean = False

    <Category("3. Saving")>
    <DisplayName("3.2) Storage path?")>
    Public Property StoragePath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

    <Category("3. Saving")>
    <DisplayName("3.3) FlyCap2 storage format?")>
    Public Property ConvImageFormat As FlyCapture2Managed.ImageFileFormat = FlyCapture2Managed.ImageFileFormat.Tiff

    <Category("3. Saving")>
    <DisplayName("3.4) EXE path?")>
    Public ReadOnly Property EXEPath As String
        Get
            Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        End Get
    End Property

    <Category("4. Debug")>
    <DisplayName("4.1) Auto-open DLL error log?")>
    <ComponentModel.DefaultValue(False)>
    Public Property AutoOpenDLLErrorLog As Boolean = False

End Class
