Option Explicit On
Option Strict On

Public Class frmLog

  Public Sub Clear()
    tbLog.Text = String.Empty
    System.Windows.Forms.Application.DoEvents()
  End Sub

  Public Sub PartLine(ByVal TextLine As String)
    tbLog.Text &= TextLine
    DoEvents()
  End Sub

  Public Sub FullLine(ByVal TextLine As String)
    tbLog.Text &= TextLine & System.Environment.NewLine
    DoEvents()
  End Sub

  Private Sub DoEvents()
    tbLog.ScrollToCaret()
    System.Windows.Forms.Application.DoEvents()
  End Sub

  Private Sub frmLog_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.UserClosing Then e.Cancel = True
  End Sub

End Class