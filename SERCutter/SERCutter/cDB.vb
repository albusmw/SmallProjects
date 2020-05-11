Option Explicit On
Option Strict On
Imports System.ComponentModel

Public Class cDB

    Private Const Cat_basics As String = "1.) Basic setup"
    Private Const Cat_tracking As String = "2.) Tracking"
    Private Const Cat_misc As String = "3.) Misc"

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("a.) Input file")>
    <ComponentModel.DefaultValue("")>
    Public Property InputFile As String = "C:\TEMP\H_alpha\14_08_07.ser"

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("b.) Output file")>
    <ComponentModel.DefaultValue("")>
    Public Property OutputFile As String = "C:\TEMP\H_alpha\CUTOUT.ser"

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("c.) Center X coordinate")>
    <ComponentModel.DefaultValue(100)>
    Public Property CenterX As Integer = 100

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("d.) Center Y coordinate")>
    <ComponentModel.DefaultValue(100)>
    Public Property CenterY As Integer = 100

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("e.) Cut-out width")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutWidth As Integer = 100

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("f.) Cut-out height")>
    <ComponentModel.DefaultValue(100)>
    Public Property CutOutHeight As Integer = 100

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("g.) Single-image FITS path")>
    <ComponentModel.Description("File name for FITS files - leave blank do disable and use ##### for frame counter")>
    <ComponentModel.DefaultValue("SingleFrame_#####.fits")>
    Public Property FITSOutFileName As String = "SingleFrame_#####.fits"

    <ComponentModel.Category(Cat_basics)>
    <ComponentModel.DisplayName("h.) Sum image FITS file name")>
    <ComponentModel.DefaultValue("SUM.fits")>
    Public Property FITSSumFile As String = String.Empty

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

    '''<summary>Property indicating if the FITS sum file shall be calculated.</summary>
    <Browsable(False)>
    Public ReadOnly Property CalcFITSSumFile As Boolean
        Get
            If String.IsNullOrEmpty(FITSSumFile) = True Then Return False Else Return True
        End Get
    End Property

End Class
