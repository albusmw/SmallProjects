Option Explicit On
Option Strict On

'''<summary>Form to run everything DLL and display some header information.</summary>
Public Class frmFITSGrep

    Public FoundFiles As New List(Of String)

    Private WithEvents DirScanner As Ato.RecursivDirScanner

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        tbOutput.Text = String.Empty

        'Init
        CType(sender, Button).Enabled = False : DE()
        Dim NL As String = System.Environment.NewLine
        FoundFiles.Clear()

        'Run search - everything
        Dim QueryResults As New List(Of String)
        QueryResults.AddRange(Everything.GetSearchResult(Chr(34) & tbRootFolder.Text & Chr(34) & " " & "*.fit|*.fits"))

        'Run "normal" recursive search if no results
        If QueryResults.Count = 0 Then
            DirScanner = New Ato.RecursivDirScanner(tbRootFolder.Text)
            DirScanner.Scan("*.fit?")
            QueryResults.AddRange(DirScanner.AllFiles)
            tsslMain.Text = String.Empty
        End If

        tbOutput.Text &= QueryResults.Count.ToString.Trim & " files found" & NL

        tspbMain.Maximum = QueryResults.Count
        tspbMain.Value = 0
        QueryResults.Sort()

        'Get maximum path length
        Dim MaxPathLength As Integer = 0
        For Each FileName As String In QueryResults
            If FileName.Length > MaxPathLength Then MaxPathLength = FileName.Length
        Next FileName

        'Get all headers
        Dim FITSFilesHeaders As New Dictionary(Of String, Dictionary(Of eFITSKeywords, Object))
        Dim AllKeywords As New Dictionary(Of eFITSKeywords, List(Of Object))
        For Each FileName As String In QueryResults
            tsslMain.Text = FileName : tsslMain.Invalidate()
            Dim DataStartPos As Integer = -1
            Dim Key As String = FileName.Trim
            FITSFilesHeaders.Add(Key, (New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(Key, DataStartPos))).GetCardsAsDictionary)
            For Each FITSKeyword As eFITSKeywords In FITSFilesHeaders(Key).Keys
                Dim KeywordValue As Object = FITSFilesHeaders(Key)(FITSKeyword)
                If AllKeywords.ContainsKey(FITSKeyword) = False Then AllKeywords.Add(FITSKeyword, New List(Of Object))
                If AllKeywords(FITSKeyword).Contains(KeywordValue) = False Then AllKeywords(FITSKeyword).Add(KeywordValue)
            Next FITSKeyword
            tspbMain.Value += 1
        Next FileName
        tsslMain.Text = String.Empty

        'Output results
        Dim Output As New List(Of String)
        Dim EmptyString As String = "----"
        For Each FileName As String In FITSFilesHeaders.Keys
            Dim OutLine As New List(Of String)
            OutLine.Add(FileName.PadRight(MaxPathLength))
            For Each FITSKeyword As eFITSKeywords In AllKeywords.Keys
                If AllKeywords(FITSKeyword).Count > 1 Then
                    OutLine.Add(GetCard(FITSFilesHeaders(FileName), FITSKeyword, EmptyString))
                End If
            Next FITSKeyword
            Output.Add(Join(OutLine.ToArray, "|"))
        Next FileName
        Output.Add("Keywords identical in ALL files:")
        Dim OneForAllFile As String = QueryResults(0)
        For Each FITSKey As eFITSKeywords In AllKeywords.Keys
            If AllKeywords(FITSKey).Count = 1 Then
                Output.Add(GetCard(FITSFilesHeaders(OneForAllFile), FITSKey, EmptyString))
            End If
        Next FITSKey

        'Finish
        tbOutput.Text &= Join(Output.ToArray, System.Environment.NewLine)
        tspbMain.Value = 0
        CType(sender, Button).Enabled = True : DE()

    End Sub

    Private Function GetCard(ByVal FITSHeader As Dictionary(Of eFITSKeywords, Object), ByVal Card As eFITSKeywords, ByVal EmptyString As String) As String
        If FITSHeader.ContainsKey(Card) Then
            Return FITSKeyword.GetKeyword(Card)(0) & "=" & cFITSKeywords.AsString(FITSHeader(Card)).Trim
        Else
            Return FITSKeyword.GetKeyword(Card)(0) & "=" & EmptyString
        End If
    End Function

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub DirScanner_CurrentlyScanning(DirectoryName As String) Handles DirScanner.CurrentlyScanning
        tsslMain.Text = "Scan <" & DirectoryName & ">"
    End Sub

End Class