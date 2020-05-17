Option Explicit On
Option Strict On
Imports System.ComponentModel

Public Class cDB

    <Browsable(False)>
    Public Property FileNamePlaceHolder As String = "<FileName>"

    Public Enum eTracker
        <ComponentModel.Description("Off")>
        Off
        <ComponentModel.Description("Full-body (e.g. sun)")>
        FullBody
        <ComponentModel.Description("Small object (e.g. ISS)")>
        SmallObject
    End Enum

    Private Const Cat_files As String = "1.) Files"
    Private Const Cat_cut As String = "2.) Cut"
    Private Const Cat_tracking As String = "3.) Tracking"
    Private Const Cat_misc As String = "4.) Misc"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("a.) Input file")>
    <ComponentModel.DefaultValue("")>
    Public Property InputFile As String = "C:\TEMP\H_alpha\Sun.ser"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("b.) Output file")>
    <ComponentModel.DefaultValue("")>
    Public Property OutputFile As String = "C:\!Work\Astro\SERCutter_solar\<FileName>__cutout.ser"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("c.) Single-image FITS path")>
    <ComponentModel.Description("File name for FITS files - leave blank to disable and use ##### for frame counter")>
    <ComponentModel.DefaultValue("")>
    Public Property FITSOutFileName As String = ""

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("d.) Sum image FITS file name")>
    <ComponentModel.Description("File name for center sum FITS files - leave blank to disable")>
    <ComponentModel.DefaultValue("<FileName>_CenterSum.fits")>
    Public Property FITSCenterSumFile As String = "<FileName>_CenterSum.fits"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("e.) 1st FITS file name")>
    <ComponentModel.Description("File name for first FITS files - leave blank to disable")>
    <ComponentModel.DefaultValue("<FileName>_First.fits")>
    Public Property FirstFITSFile As String = "<FileName>_First.fits"

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("a.) Center X coordinate")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutCenterX As Integer = 100

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("b.) Center Y coordinate")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutCenterY As Integer = 100

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("c.) Cut-out width")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutWidth As Integer = 900

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("d.) Cut-out height")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutHeight As Integer = 900

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("e.) Last frame to use")>
    <ComponentModel.DefaultValue(Integer.MaxValue)>
    Public Property CutOut_LastFrame As Integer = Integer.MaxValue

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("a.) Tracking mode")>
    <ComponentModelEx.EnumDefaultValue(eTracker.Off)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property Tracking As eTracker = eTracker.FullBody

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("b.) FullBody search center first")>
    <ComponentModel.Description("Center object (align first frame center to the tracked center)")>
    <ComponentModelEx.EnumDefaultValue(eTracker.Off)>
    <ComponentModel.DefaultValue(True)>
    Public Property TrackingSearchCenter As Boolean = True

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("c) FullBody threshold")>
    <ComponentModel.Description("Only pixel above this value are included in the tracking")>
    <ComponentModel.DefaultValue(20000)>
    Public Property FullBody_threshold As Integer = 20000

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("d) FullBody weighted")>
    <ComponentModel.Description("Use pixel intensity as weight")>
    <ComponentModel.DefaultValue(False)>
    Public Property FullBody_weighted As Boolean = False

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("e) Small-Object Slide width")>
    <ComponentModel.DefaultValue(32)>
    Public Property SlideWidth As Integer = 32

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("e.) Tracking center - current X")>
    <ComponentModel.DefaultValue(0)>
    Public ReadOnly Property TrackCenterX As Integer
        Get
            Return MyTrackCenterX
        End Get
    End Property
    Public MyTrackCenterX As Integer = 0

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("e.) Tracking center - current Y")>
    <ComponentModel.DefaultValue(0)>
    Public ReadOnly Property TrackCenterY As Integer
        Get
            Return MyTrackCenterY
        End Get
    End Property
    Public MyTrackCenterY As Integer = 0


    <ComponentModel.Category(Cat_misc)>
    <ComponentModel.DisplayName("a.) Use IPP?")>
    <ComponentModel.DefaultValue(True)>
    Public Property UseIPP As Boolean = True

    <ComponentModel.Category(Cat_misc)>
    <ComponentModel.DisplayName("b.) IPP path")>
    <ComponentModel.DefaultValue("")>
    Public Property IPPPath As String = String.Empty

    <Browsable(False)>
    Public ReadOnly Property MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

End Class
