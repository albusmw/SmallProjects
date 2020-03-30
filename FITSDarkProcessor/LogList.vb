Option Explicit On
Option Strict On

Public Class LogList

  Public Property AddTime() As Boolean
    Get
      Return MyAddTime
    End Get
    Set(value As Boolean)
      MyAddTime = value
    End Set
  End Property
  Private MyAddTime As Boolean = True

  Public Sub AddEntry(ByVal Text As String)
    If AddTime Then Text = Format(Now, "HH:mm:ss.fff") & "|" & Text
    tbLog.Text &= Text & System.Environment.NewLine
    tbLog.Select(tbLog.Text.Length, 0)
    tbLog.ScrollToCaret()
    System.Windows.Forms.Application.DoEvents()
  End Sub

  Public Sub AddEntry(ByVal Texts As String())
    Dim DateToLog As DateTime = Now
    Dim NewLog As String = tbLog.Text
    For Each Entry As String In Texts
      Text = Entry
      If AddTime Then Text = Format(DateToLog, "HH:mm:ss.fff") & "|" & Text
      NewLog &= Text & System.Environment.NewLine
    Next Entry
    tbLog.Text = NewLog
    tbLog.Select(tbLog.Text.Length, 0)
    tbLog.ScrollToCaret()
    System.Windows.Forms.Application.DoEvents()
  End Sub

End Class
