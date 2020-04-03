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

        'Output results
        Dim EmptryString As String = "----"
        Dim Output As New List(Of String)
        For Each FileName As String In QueryResults
            FileName = FileName.Trim
            Dim FITSHeader As New cFITSHeaderParser(cFITSHeaderChanger.ReadHeader(FileName))
            Dim FITSHeaderDict As Dictionary(Of String, Object) = FITSHeader.GetListAsDictionary
            Output.Add(FileName.PadRight(MaxPathLength) & " : " & GetCard(FITSHeader, eFITSKeywords.BITPIX, EmptryString) & "|" & GetCard(FITSHeader, eFITSKeywords.NAXIS3, EmptryString) & "|" & GetCard(FITSHeader, eFITSKeywords.AUTHOR, EmptryString))
            tspbMain.Value += 1 : DE()
        Next FileName

        'Finish
        tbOutput.Text &= Join(Output.ToArray, System.Environment.NewLine)
        tspbMain.Value = 0
        CType(sender, Button).Enabled = True : DE()

    End Sub

    Private Function GetCard(ByRef FITSHeader As cFITSHeaderParser, ByVal Card As eFITSKeywords, ByVal EmptyString As String) As String
        Dim Keyword As String = FITSKeyword.GetKeyword(Card)
        Dim Value As String = FITSHeader.ElementValue(Keyword)
        If IsNothing(Value) = True Then
            Return Keyword & "=" & EmptyString
        Else
            Return Keyword & "=" & Value.Trim
        End If
    End Function

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class