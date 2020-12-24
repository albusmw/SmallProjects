Option Explicit On
Option Strict On

Public Class cFITSGrepper

    Public ReadOnly Property Progress() As sProgress
        Get
            Return MyProgress
        End Get
    End Property
    Private MyProgress As sProgress = New sProgress(-1, -1, String.Empty)

    Public Structure sProgress
        Public Current As Integer
        Public Total As Integer
        Public Message As String
        Public Sub New(ByVal NewMessage As String)
            Current = -1
            Total = -1
            Message = NewMessage
        End Sub
        Public Sub New(ByVal NewCurrent As Integer, ByVal NewTotal As Integer, ByVal NewMessage As String)
            Current = NewCurrent
            Total = NewTotal
            Message = NewMessage
        End Sub
    End Structure

    Private WithEvents DirScanner As Ato.RecursivDirScanner

    '''<summary>Report generated during thee grep process.</summary>
    Public Report As New List(Of String)

    Public Sub Grep(ByVal RootFolder As String, ByVal Filter As String, ByRef dgvFiles As DataGridView)

        'Init
        Report.Clear()
        Dim FileCount As Integer = 0

        '=====================================================================================================================================
        ' Get all files to read the header in
        '=====================================================================================================================================

        'Run search - everything
        ReportSave(New sProgress("Running Everything search ..."))
        Dim QueryResults As New List(Of String)
        QueryResults.AddRange(Everything.GetSearchResult(Chr(34) & RootFolder & Chr(34) & " " & Filter & ".fit|*.fits"))

        'Run "normal" recursive search if no results
        If QueryResults.Count = 0 Then
            ReportSave(New sProgress("Running traditional search ..."))
            DirScanner = New Ato.RecursivDirScanner(RootFolder)
            DirScanner.Scan(Filter & ".fit?")
            QueryResults.AddRange(DirScanner.AllFiles)
        End If

        'Report search results and prepare next step
        QueryResults.Sort()
        Report.Add(QueryResults.Count.ToString.Trim & " files found")
        MyProgress = New sProgress(0, QueryResults.Count + 1, String.Empty)

        '=====================================================================================================================================
        ' Read all header bytes
        '=====================================================================================================================================

        'Read headers from disc
        ReportSave(New sProgress("Read all headers parallel"))
        Dim AllHeaders As New Concurrent.ConcurrentDictionary(Of String, Byte())
        FileCount = 0
        Parallel.ForEach(QueryResults, Sub(FileName)
                                           FileCount += 1
                                           ReportSave(New sProgress(FileCount, QueryResults.Count, "Reading header <" & FileName & ">"))
                                           Dim HeaderBytes(cFITSHeaderChanger.ReadHeaderByteCount - 1) As Byte
                                           System.IO.File.OpenRead(FileName).Read(HeaderBytes, 0, HeaderBytes.Length)
                                           AllHeaders.TryAdd(FileName, HeaderBytes)
                                       End Sub)
        Report.Add(AllHeaders.Count.ToString.Trim & " headers read")
        ReportSave(New sProgress(-1, -1, String.Empty))

        '=====================================================================================================================================
        ' Parse all headers to get all FITS headers
        '=====================================================================================================================================

        'From all headers, get all keywords and all found values for each keyword
        ReportSave(New sProgress("Parse all headers"))
        Dim AllFileHeaders As New Concurrent.ConcurrentDictionary(Of String, Dictionary(Of eFITSKeywords, Object))
        Dim CorruptFiles As New Concurrent.ConcurrentBag(Of String)
        FileCount = 0
        Parallel.ForEach(AllHeaders.Keys, Sub(FileName)
                                              FileCount += 1
                                              ReportSave(New sProgress(FileCount, QueryResults.Count, "Parsing <" & FileName & ">"))
                                              Dim DataStartPos As Integer = -1
                                              Dim AllCards As Dictionary(Of eFITSKeywords, Object) = (New cFITSHeaderParser(cFITSHeaderChanger.ParseHeader(AllHeaders(FileName), DataStartPos))).GetCardsAsDictionary
                                              If AllCards.Count > 0 Then
                                                  AllFileHeaders.TryAdd(FileName, AllCards)
                                              Else
                                                  CorruptFiles.Add(FileName)
                                              End If
                                          End Sub)
        Report.Add(AllFileHeaders.Count.ToString.Trim & " headers parsed")
        For Each CorruptFile As String In CorruptFiles
            Report.Add("!! Corrupt file: <" & CorruptFile & ">")
        Next CorruptFile
        ReportSave(New sProgress(-1, -1, String.Empty))

        'Get a list of all keywords found
        Dim AllFoundKeywordValues As New Dictionary(Of eFITSKeywords, List(Of Object))      'keywords found over all search results
        For Each File As String In AllFileHeaders.Keys
            For Each Keyword As eFITSKeywords In AllFileHeaders(File).Keys
                If Not AllFoundKeywordValues.ContainsKey(Keyword) Then
                    AllFoundKeywordValues.Add(Keyword, New List(Of Object))
                End If
            Next Keyword
        Next File
        Report.Add(AllFoundKeywordValues.Count.ToString.Trim & " FITS keywords found in total")

        'Find identical keywords (keyword must be present in each file and must also be the same in each file)
        ReportSave(New sProgress("Finding identical keywords ..."))
        Dim NotInAllFiles As New List(Of eFITSKeywords)                                     'keywords that are not present in all files
        For Each File As String In AllFileHeaders.Keys
            For Each FITSKeyword As eFITSKeywords In AllFoundKeywordValues.Keys
                If AllFileHeaders(File).ContainsKey(FITSKeyword) Then
                    Dim KeywordValue As Object = AllFileHeaders(File)(FITSKeyword)
                    If AllFoundKeywordValues(FITSKeyword).Contains(KeywordValue) = False Then AllFoundKeywordValues(FITSKeyword).Add(KeywordValue)
                Else
                    NotInAllFiles.Add(FITSKeyword)
                End If
            Next FITSKeyword
        Next File

        '=====================================================================================================================================
        ' Generate table
        '=====================================================================================================================================

        'Clear
        dgvFiles.Columns.Clear()
        dgvFiles.Rows.Clear()

        'Generate all columns (keywords)
        ReportSave(New sProgress("Generating table"))
        Dim EmptyString As String = "----"
        dgvFiles.SuspendLayout()
        dgvFiles.Columns.Add("FileName", "FileName")
        For Each Keyword As eFITSKeywords In AllFoundKeywordValues.Keys
            If AllFoundKeywordValues(Keyword).Count > 1 Or NotInAllFiles.Contains(Keyword) Then
                Dim KeywordString As String = FITSKeyword.KeywordString(Keyword)
                dgvFiles.Columns.Add(KeywordString, KeywordString)
            End If
        Next Keyword

        'Generate all rows (files)
        dgvFiles.Rows.Add(AllFileHeaders.Count - 1)
        Dim Ptr As Integer = -1
        For Each FileName As String In AllFileHeaders.Keys
            Ptr += 1
            dgvFiles.Rows(Ptr).Cells("FileName").Value = FileName
            For Each Keyword As eFITSKeywords In AllFileHeaders(FileName).Keys
                If AllFoundKeywordValues(Keyword).Count > 1 Or NotInAllFiles.Contains(Keyword) Then
                    dgvFiles.Rows(Ptr).Cells(FITSKeyword.KeywordString(Keyword)).Value = AllFileHeaders(FileName)(Keyword)
                End If
            Next Keyword
        Next FileName
        dgvFiles.ResumeLayout() : dgvFiles.Invalidate() : dgvFiles.Refresh()

        Dim KeywordSummary As New List(Of String)
        Dim OneForAllFile As String = QueryResults(0)
        For Each Keyword As eFITSKeywords In AllFoundKeywordValues.Keys
            If (AllFoundKeywordValues(Keyword).Count = 1) And (NotInAllFiles.Contains(Keyword) = False) Then
                KeywordSummary.Add(" === " & GetCard(AllFileHeaders(OneForAllFile), Keyword, EmptyString))
            Else
                KeywordSummary.Add(" <=> " & FITSKeyword.KeywordString(Keyword) & ": " & AllFoundKeywordValues(Keyword).Count.ValRegIndep & " values")
            End If
        Next Keyword
        KeywordSummary.Sort()
        Report.Add("Keyword summary: ")
        Report.AddRange(KeywordSummary)
        ReportSave(New sProgress(-1, -1, String.Empty))

    End Sub

    Private Function GetCard(ByVal FITSHeader As Dictionary(Of eFITSKeywords, Object), ByVal Keyword As eFITSKeywords, ByVal EmptyString As String) As String
        If FITSHeader.ContainsKey(Keyword) Then
            Return FITSKeyword.KeywordString(Keyword) & "=" & cFITSKeywords.AsString(FITSHeader(Keyword)).Trim
        Else
            Return FITSKeyword.KeywordString(Keyword) & "=" & EmptyString
        End If
    End Function

    Private Sub DirScanner_CurrentlyScanning(DirectoryName As String) Handles DirScanner.CurrentlyScanning
        MyProgress = New sProgress("Scan <" & DirectoryName & ">")
    End Sub

    Public Sub ReportSave(value As sProgress)
        MyProgress = value
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class
