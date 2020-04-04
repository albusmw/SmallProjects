Option Explicit On
Option Strict On

'''<summary>Form to run everything DLL and display some header information.</summary>
Public Class frmFITSGrep

    Public FoundFiles As New List(Of String)

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        'Init
        CType(sender, Button).Enabled = False : DE()
        Dim NL As String = System.Environment.NewLine
        Dim FilePattern As String = "*.fit|*.fits"
        FoundFiles.Clear()

        'Run search
        Dim QueryResults As List(Of String) = Everything.GetSearchResult(Chr(34) & tbRootFolder.Text & Chr(34) & " " & FilePattern)
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
        Dim AllHeaders As New Dictionary(Of String, Dictionary(Of String, String))
        For Each FileName As String In QueryResults
            AllHeaders.Add(FileName.Trim, (New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(FileName.Trim))).GetCardsAsDictionary)
            tspbMain.Value += 1
        Next FileName

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

    Private Function GetCard(ByVal FITSHeader As Dictionary(Of String, String), ByVal Card As eFITSKeywords, ByVal EmptyString As String) As String
        Dim Keyword As String = FITSKeyword.GetKeyword(Card)
        If FITSHeader.ContainsKey(Keyword) Then
            Return Keyword & "=" & FITSHeader(Keyword).Trim
        Else
            Return Keyword & "=" & EmptyString
        End If
    End Function

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class