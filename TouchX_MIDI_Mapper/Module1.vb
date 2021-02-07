Option Explicit On
Option Strict On

Module Module1

    Dim WithEvents Monitor As cMIDIMonitor

    Sub Main()
        Monitor = New cMIDIMonitor
        If Monitor.MIDIDeviceCount > 0 Then Monitor.SelectMidiDevice(0)
        Do
            System.Threading.Thread.Sleep(10)
        Loop Until 1 = 0
    End Sub

    Private Sub Monitor_NewData(Channel As Integer, Value As Integer) Handles Monitor.Data
        Console.WriteLine(Channel.ToString.Trim & ": " & Value.ToString.Trim)
    End Sub

    Private Sub Monitor_VerboseLog(Text As String) Handles Monitor.VerbLog
        Console.WriteLine("  >> " & Text)
    End Sub

End Module
