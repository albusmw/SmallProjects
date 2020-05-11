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
        Dim AllHeaders As New Dictionary(Of String, Dictionary(Of eFITSKeywords, Object))
        For Each FileName As String In QueryResults
            tsslMain.Text = FileName : tsslMain.Invalidate()
            Dim DataStartPos As Integer = -1
            AllHeaders.Add(FileName.Trim, (New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(FileName.Trim, DataStartPos))).GetCardsAsDictionary)
            tspbMain.Value += 1
        Next FileName
        tsslMain.Text = String.Empty

        'Output results
        Dim Output As New List(Of String)
        Dim EmptyString As String = "----"
        For Each FileName As String In AllHeaders.Keys
            Output.Add(FileName.PadRight(MaxPathLength) & " : " & GetCard(AllHeaders(FileName), eFITSKeywords.BITPIX, EmptyString) & "|" & GetCard(AllHeaders(FileName), eFITSKeywords.NAXIS3, EmptyString) & "|" & GetCard(AllHeaders(FileName), eFITSKeywords.AUTHOR, EmptyString))
        Next FileName

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