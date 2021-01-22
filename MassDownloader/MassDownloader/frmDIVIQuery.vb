Option Explicit On
Option Strict On

Public Class frmDIVIQuery

    Private Class AGS_s
        Public Const Miesbach As String = "09182"
    End Class

    Dim Downloader As New cDownloader
    Dim PageLoaded As Boolean = False

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        Downloader.InitWebClient()

        'Dim DayLinks1 As List(Of String) = GetDayLinks()

        Dim DayLinks2 As New List(Of String)
        For Offset As Integer = 6621 To 5621 Step -1
            Dim LinkToNavigate As String = "https://edoc.rki.de/handle/176904/" & Offset.ToString.Trim
            NavigateAndWait(LinkToNavigate)
            Dim AllLinks As List(Of String) = GetAllLinks()
            For Each Link As String In AllLinks
                If Link.Contains("teilbare_divi_daten.csv") Then
                    If DayLinks2.Contains(Link) = False Then DayLinks2.Add(Link)
                End If
            Next Link
        Next Offset

        For Each Link As String In DayLinks2
            Dim FileName As String = Link.Substring(Link.LastIndexOf("/") + 1)
            FileName = FileName.Substring(0, FileName.IndexOf("?"))
            If System.IO.File.Exists(FileName) = False Then
                Dim Content As String = Downloader.DownloadString(Link)
                System.IO.File.WriteAllText(FileName, Content)
            End If
        Next Link

        MsgBox("DONE")
        Exit Sub

        For Each SinglePage As String In DayLinks2
            PageLoaded = False
            wbMain.Navigate(SinglePage)
            Do
                System.Windows.Forms.Application.DoEvents()
            Loop Until PageLoaded = True
            For Each Element As HtmlElement In wbMain.Document.Links
                Dim Link As String = Element.GetAttribute("href")
                If Link.Contains("teilbare_divi_daten.csv") Then
                    Dim Content As String = Downloader.DownloadString(Link)
                    Dim FileName As String = Link.Substring(Link.LastIndexOf("/") + 1)
                    FileName = FileName.Substring(0, FileName.IndexOf("?"))
                    System.IO.File.WriteAllText(FileName, Content)
                End If
            Next Element
        Next SinglePage

    End Sub

    'Get all "Tagesreport" links
    Private Function GetDayLinks() As List(Of String)
        Dim RetVal As New List(Of String)
        For Offset As Integer = 0 To 300 Step 20
            NavigateAndWait("https://edoc.rki.de/handle/176904/7012/recent-submissions?offset=" & Offset.ToString.Trim)
            For Each Element As HtmlElement In wbMain.Document.Links
                Dim Link As String = Element.GetAttribute("href")
                If Link.StartsWith("https://edoc.rki.de/handle/176904") Then
                    If RetVal.Contains(Link) = False Then RetVal.Add(Link)
                End If
            Next Element
        Next Offset
        Return RetVal
    End Function

    Private Function GetAllLinks() As List(Of String)
        Dim RetVal As New List(Of String)
        For Each Element As HtmlElement In wbMain.Document.Links
            Dim Link As String = Element.GetAttribute("href")
            RetVal.Add(Link)
        Next Element
        Return RetVal
    End Function

    Private Sub wbMain_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles wbMain.DocumentCompleted
        PageLoaded = True
    End Sub

    Private Sub btnProcessCSV_Click(sender As Object, e As EventArgs) Handles btnProcessCSV.Click
        Dim AGS As String = AGS_s.Miesbach
        Dim Land As String = AGS.Substring(0, 2)
        Dim SingleCSVLines As New List(Of String)
        Dim ProcessedFiles As Integer = 0
        Dim ProcessedLines As Integer = 0
        For Each File As String In System.IO.Directory.GetFiles("C:\GIT\SmallProjects\MassDownloader\MassDownloader\bin\Debug", "*_teilbare_divi_daten.csv")
            ProcessedFiles += 1
            For Each Line As String In System.IO.File.ReadAllLines(File)
                If SingleCSVLines.Count = 0 Then SingleCSVLines.Add(Line.Replace(",", ";"))               'add CSV header
                If Line.StartsWith(Land & "," & AGS) Then
                    SingleCSVLines.Add(Line.Replace(",", ";"))
                    ProcessedLines += 1
                End If
            Next Line
        Next File
        If SingleCSVLines.Count = 1 Then
            MsgBox("AGS <" & AGS & "> not found!", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly)
        Else
            MsgBox(ProcessedFiles.ValRegIndep & " files processed, " & ProcessedLines.ValRegIndep & " lines generated", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)
        End If
        Dim CSVFile As String = "AGS_" & AGS & ".csv"
        System.IO.File.WriteAllLines(CSVFile, SingleCSVLines)
        Process.Start(CSVFile)
    End Sub

    Private Sub NavigateAndWait(ByVal URL As String)
        PageLoaded = False
        wbMain.Navigate(URL)
        Do
            System.Windows.Forms.Application.DoEvents()
        Loop Until PageLoaded = True
    End Sub

End Class