Option Explicit On
Option Strict On

Public Class frmMain

    Private WithEvents Monitor As New cMIDIMonitor

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Monitor = New cMIDIMonitor
        If Monitor.MIDIDeviceCount > 0 Then Monitor.SelectMidiDevice(0)
    End Sub

    Private Sub Monitor_NewData(Channel As Integer, Value As Integer) Handles Monitor.Data
        tbLog.Text &= "NewData: " & Channel.ToString.Trim & ": " & Value.ToString.Trim & System.Environment.NewLine
    End Sub

    Private Sub Monitor_VerboseLog(Text As String) Handles Monitor.VerbLog
        tbLog.Text &= "VerboseLog:  >> " & Text & System.Environment.NewLine
    End Sub

End Class