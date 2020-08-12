Option Explicit On
Option Strict On
Imports System.IO
Imports DropToCopy.Atomic

Public Class Form1

    Private WithEvents Dropper As Ato.DragDrop
    Private MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
    Private LogPath As String = System.IO.Path.Combine(MyPath, "CopyLog.log")

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
                    System.IO.File.AppendAllLines(LogPath, New String() {Format(Now, "dd.MM.yyyy HH:mm:ss") & "|" & tbDropHere.Text})
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

    Private Sub btnSource_Click(sender As Object, e As EventArgs) Handles btnSource.Click
        If System.IO.Directory.Exists(tbSourceRoot.Text) Then Process.Start(tbSourceRoot.Text)
    End Sub

    Private Sub btnDest_Click(sender As Object, e As EventArgs) Handles btnDest.Click
        If System.IO.Directory.Exists(tbDestinationRoot.Text) Then Process.Start(tbDestinationRoot.Text)
    End Sub

    Private Sub btnEXELocation_Click(sender As Object, e As EventArgs) Handles btnEXELocation.Click
        Process.Start(MyPath)
    End Sub

    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click
        Dim Scanner As New Ato.RecursivDirScanner(tbDestinationRoot.Text)
        Dim Out As New cRTFGenerator : Out.DefaultFontSize = 8
        Scanner.Scan("*.*")
        For Each DropBoxFile As String In Scanner.AllFiles
            Dim RelativePath As String = DropBoxFile.Replace(tbDestinationRoot.Text, String.Empty)
            Dim FoxDocFile As String = tbSourceRoot.Text & RelativePath
            If System.IO.File.Exists(FoxDocFile) = True Then
                'Exists
            Else
                Out.AddEntry("X|" & FoxDocFile, Color.Red)
            End If
        Next DropBoxFile
        Dim X As New cRTFTextBox
        X.ShowText(Out.GetRTFText)
    End Sub

End Class
