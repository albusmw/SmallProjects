Option Explicit On
Option Strict On

Public Class Form1

    Dim COM_IO As New cSkyWatcher_IO

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        COM_IO.Init(tbCOMPort.Text)
        CheckAnswer(COM_IO.SetStepPeriod(cSkyWatcher_IO.Axis_Az, "000FFF"))
        CheckAnswer(COM_IO.StartMotion(cSkyWatcher_IO.Axis_Az))
        CheckAnswer(COM_IO.StartMotion(cSkyWatcher_IO.Axis_Alt))
        MsgBox("Commands send", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
        pgMain.SelectedObject = COM_IO
    End Sub

    Private Sub CheckAnswer(ByVal Answer As String)
        If Answer.StartsWith("=") Then
            'Normal answer - do nothing ...
        Else
            'Error answer
            MsgBox("<" & Answer & "> (" & Answer.Length.ToString.Trim & " byte)")
        End If
    End Sub

    Private Sub DisplayAnswer(ByVal Answer As String)
        MsgBox("<" & Answer & "> (" & Answer.Length.ToString.Trim & " byte)")
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class
