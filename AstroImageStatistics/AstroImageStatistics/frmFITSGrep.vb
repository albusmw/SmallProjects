Option Explicit On
Option Strict On

'''<summary>Form to run everything DLL and display some header information.</summary>
Public Class frmFITSGrep

    Private WithEvents FITSGrepper As New cFITSGrepper

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        CType(sender, Button).Enabled = False : DE()
        FITSGrepper.Grep(tbRootFolder.Text, tbFilter.Text, dgvFiles)
        tbOutput.Text &= Join(FITSGrepper.Report.ToArray, System.Environment.NewLine)
        tspbMain.Value = 0
        UpdateStatus(String.Empty)
        CType(sender, Button).Enabled = True : DE()
    End Sub

    Private Sub RunAllFITSFilesParallel(ByVal QueryResults As List(Of String))
        Dim FITSFilesHeaders As New Dictionary(Of String, Dictionary(Of eFITSKeywords, Object))
        Parallel.For(0, QueryResults.Count, Sub(Idx)
                                                Dim FileName As String = QueryResults(Idx)
                                                Dim DataStartPos As Integer = -1
                                                Dim Key As String = FileName.Trim
                                                Dim AllCards As Dictionary(Of eFITSKeywords, Object) = (New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(Key, DataStartPos))).GetCardsAsDictionary
                                                SyncLock FITSFilesHeaders
                                                    FITSFilesHeaders.Add(Key, AllCards)
                                                End SyncLock

                                                SyncLock tspbMain
                                                    'UpdateStatus("File " & (Idx + 1).ValRegIndep & "/" & QueryResults.Count.ValRegIndep & ": " & FileName)
                                                    'tspbMain.Value += 1
                                                End SyncLock
                                            End Sub)
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub UpdateStatus(ByVal Text As String)
        tsslMain.Text = Text
        DE()
    End Sub

    Private Sub tbRootFolder_KeyUp(sender As Object, e As KeyEventArgs) Handles tbRootFolder.KeyUp
        If e.KeyCode = Keys.Enter And btnSearch.Enabled = True Then btnSearch_Click(btnSearch, Nothing)
    End Sub

    Private Sub tUpdate_Tick(sender As Object, e As EventArgs) Handles tUpdate.Tick
        If FITSGrepper.Progress.Total > -1 Then
            tspbMain.Maximum = FITSGrepper.Progress.Total
            tspbMain.Value = FITSGrepper.Progress.Current
            tsslProgress.Text = FITSGrepper.Progress.Current.ValRegIndep & "/" & FITSGrepper.Progress.Total.ValRegIndep
        Else
            tsslProgress.Text = "---/---"
            tspbMain.Value = 0
        End If
        UpdateStatus(FITSGrepper.Progress.Message)
    End Sub

End Class