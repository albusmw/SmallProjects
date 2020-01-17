Option Explicit On
Option Strict On

Public Class cDB

    Public ReadOnly Property MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Public TempFolder As String = System.IO.Path.GetTempPath
    Public Log As New System.Text.StringBuilder
    Public LogFolder As String = String.Empty

    'ASCOM
    Public ASCOM_Focuser_Chooser As New ASCOM.Utilities.Chooser
    Public ASCOM_Camera_Chooser As New ASCOM.Utilities.Chooser
    Public ASCOM_Focuser As ASCOM.DriverAccess.Focuser = Nothing
    Public ASCOM_Camera As ASCOM.DriverAccess.Camera = Nothing

    Public Property ROIConfirm As Boolean = True
    Public Property ROICenterX As Integer = -1
    Public Property ROICenterY As Integer = -1

    Public Property ExpTime As Double = 5                   '[seconds]
    Public Property Gain As Short = 400                     '[seconds]
    Public Property StartPosition As Integer = 9600
    Public Property EndPosition As Integer = 10400
    Public Property StepSize As Integer = 10
    Public Property ROIDelta As Integer = 40
    Public Property SavePNG As Boolean = False

    Public ExpStopWatch As New Stopwatch

    Public RecJ2000 As String = ""
    Public DecJ2000 As Double = 0.0
    Public StopFlag As Boolean = False

End Class
