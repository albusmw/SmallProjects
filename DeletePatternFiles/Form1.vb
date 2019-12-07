Option Explicit On
Option Strict On

Public Class Form1

    Private FilesFailed As Long
    Private FileCount As Long
    Private FileSizeSum As Long

    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click

        CType(sender, Button).Enabled = False
        DE

        Dim Root As String = tbFolder.Text
        FilesFailed = 0
        FileCount = 0
        FileSizeSum = 0

        DelRecurse(Root, tbPattern.Text)

        UpdateStatus()
        CType(sender, Button).Enabled = True
        DE

    End Sub

    Private Sub DelRecurse(ByRef Folder As String, ByVal SearchPattern As String)

        Try
            For Each SingleFolder As String In System.IO.Directory.GetDirectories(Folder)
                Try
                    For Each SingleFile As String In System.IO.Directory.GetFiles(SingleFolder, SearchPattern)
                        Try
                            Dim OldFileSize As Long = New System.IO.FileInfo(SingleFile).Length
                            Debug.Print(FileCount & " -> " & FileSizeSum & "-> " & SingleFile)
                            System.IO.File.Delete(SingleFile)
                            If System.IO.File.Exists(SingleFile) = False Then
                                FileCount += 1
                                FileSizeSum += OldFileSize
                            Else
                                FilesFailed += 1
                            End If
                            UpdateStatus()
                        Catch ex As Exception
                            FilesFailed += 1
                            UpdateStatus()
                        End Try
                    Next SingleFile
                Catch ex As Exception
                    'Do nothing ...
                End Try
                DelRecurse(SingleFolder, SearchPattern)
            Next SingleFolder
        Catch ex As Exception

        End Try

    End Sub

    Private Sub UpdateStatus()
        tbStatus.Text = FileCount.ToString.Trim & " files deleted, failed on " & FilesFailed.ToString.ToString & ", deleted: " & (FileSizeSum \ 1048576).ToString.Trim & " MByte"
        DE()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class
