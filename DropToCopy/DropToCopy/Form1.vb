Option Explicit On
Option Strict On

Public Class Form1

    Private WithEvents Dropper As Ato.DragDrop

    Private Sub Dropper_DropOccured(Files() As String) Handles Dropper.DropOccured

        For Each SingleFile As String In Files
            Dim TargetFile As String = Replace(SingleFile, tbSourceRoot.Text, tbDestinationRoot.Text,,, CompareMethod.Text)
            Dim TargetDirectory As String = System.IO.Path.GetDirectoryName(TargetFile)
            If System.IO.File.Exists(TargetFile) = True Then
                tbDropHere.BackColor = Color.Red
            Else
                tbDropHere.BackColor = Color.Orange
                Try
                    If System.IO.Directory.Exists(TargetDirectory) = False Then System.IO.Directory.CreateDirectory(TargetDirectory)
                    System.IO.File.Copy(SingleFile, TargetFile)
                    tbDropHere.Text = TargetFile.Replace(tbDestinationRoot.Text, String.Empty)
                    tbLog.Text &= tbDropHere.Text & System.Environment.NewLine
                    tbDropHere.BackColor = Color.Green
                Catch ex As Exception
                    tbDropHere.Text = ex.Message
                    tbDropHere.BackColor = Color.Red
                End Try
            End If
        Next SingleFile

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dropper = New Ato.DragDrop(tbDropHere)
        Dropper.FillList = False
    End Sub

End Class
