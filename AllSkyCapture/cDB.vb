Option Explicit On
Option Strict On

Public Class cDB

    '''<summary>Location of the EXE.</summary>
    Private ReadOnly MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

    Private Const Cat_general As String = "1.) General"

    Public ReadOnly GainNotSet As Short = Short.MinValue + 1

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("1.) Storage root")>
    <ComponentModel.DefaultValue("")>
    Public Property StorageRoot As String = MyPath

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("2.) Default file name")>
    <ComponentModel.DefaultValue("AllSkyImage")>
    Public Property CurrentImageName As String = "AllSkyImage"

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("3.1) Save as JPG")>
    <ComponentModel.DefaultValue(True)>
    Public Property SaveAsJPG As Boolean = True

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("3.2) Save as PNG")>
    <ComponentModel.DefaultValue(False)>
    Public Property SaveAsPNG As Boolean = False

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("4.) Selected ASCOM cam")>
    <ComponentModel.DefaultValue("")>
    Public Property ASCOMCam As String = "ASCOM.ASICamera2.Camera"

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("5.) Exposure time")>
    <ComponentModel.DefaultValue(5.0)>
    Public Property ExposureTime As Double = 5.0

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("6.) Gain")>
    <ComponentModel.DefaultValue(300)>
    Public Property SelectedGain As Short = 300

    <ComponentModel.Category(Cat_general)>
    <ComponentModel.DisplayName("7.) Auto-exposure interval [s]")>
    <ComponentModel.Description("Set to 0 to disable.")>
    <ComponentModel.DefaultValue(0.0)>
    Public Property AutoExpInterval As Double = 0.0

End Class
