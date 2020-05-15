Option Explicit On
Option Strict On
Imports System.ComponentModel

Public Class cDB

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
    Public Property OutputFile As String = "C:\TEMP\H_alpha\Sun_cutout.ser"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("c.) Single-image FITS path")>
    <ComponentModel.Description("File name for FITS files - leave blank to disable and use ##### for frame counter")>
    <ComponentModel.DefaultValue("SingleFrame_#####.fits")>
    Public Property FITSOutFileName As String = "SingleFrame_#####.fits"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("d.) Max-hold image FITS file name")>
    <ComponentModel.Description("File name for max-hold FITS files - leave blank to disable")>
    <ComponentModel.DefaultValue("SUM.fits")>
    Public Property FITSMaxHoldFile As String = "MaxHold.fits"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("e.) Sum image FITS file name")>
    <ComponentModel.Description("File name for center sum FITS files - leave blank to disable")>
    <ComponentModel.DefaultValue("SUM.fits")>
    Public Property FITSCenterSumFile As String = "CenterSum.fits"

    <ComponentModel.Category(Cat_files)>
    <ComponentModel.DisplayName("f.) 1st FITS file name")>
    <ComponentModel.Description("File name for first FITS files - leave blank to disable")>
    <ComponentModel.DefaultValue("")>
    Public Property FirstFITSFile As String = String.Empty

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("a.) Center X coordinate")>
    <ComponentModel.DefaultValue(100)>
    Public Property CenterX As Integer = 100

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("b.) Center Y coordinate")>
    <ComponentModel.DefaultValue(100)>
    Public Property CenterY As Integer = 100

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("c.) Cut-out width")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutWidth As Integer = 1500

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("d.) Cut-out height")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutHeight As Integer = 1500

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("e.) Sun center tracking")>
    <ComponentModel.Description("Continuos sun center tracking")>
    <ComponentModel.DefaultValue(True)>
    Public Property SunCenterTracking As Boolean = True

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("e.1) Weighted tracking")>
    <ComponentModel.Description("Use pixel intensity as weight")>
    <ComponentModel.DefaultValue(False)>
    Public Property SunCenterTracking_weighted As Boolean = False

    <ComponentModel.Category(Cat_cut)>
    <ComponentModel.DisplayName("e.2) Tracking threshold")>
    <ComponentModel.Description("Only pixel above this value are included in the tracking")>
    <ComponentModel.DefaultValue(20000)>
    Public Property SunCenterTracking_threshold As Integer = 20000

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("a.) Tracking?")>
    <ComponentModel.DefaultValue(False)>
    Public Property Tracking As Boolean = False

    <ComponentModel.Category(Cat_tracking)>
    <ComponentModel.DisplayName("b.) Slide width")>
    <ComponentModel.DefaultValue(32)>
    Public Property SlideWidth As Integer = 32

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
