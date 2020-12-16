Option Explicit On
Option Strict On

Public Class MainForm

    Dim Downloader As New cDownloader

    Private Sub tbAdr_TextChanged(sender As Object, e As EventArgs) Handles tbAdr.TextChanged
        wbMain.Navigate(tbAdr.Text)
    End Sub

    Private Sub btnLinks_Click(sender As Object, e As EventArgs) Handles btnLinks.Click
        'Init everything
        tbLinks.Text = String.Empty
        Downloader.InitWebClient()
    End Sub

    Private Sub StrassennamenÜberVIVOToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StrassennamenÜberVIVOToolStripMenuItem.Click
        Dim Names As New List(Of String)
        For Each Entry As String In System.IO.File.ReadAllLines("C:\Users\albusmw\Dropbox\Holzkirchen Strassennamen.txt")
            Names.Add(Entry.PartAfter(">").PartBefore("<"))
        Next
        System.IO.File.WriteAllLines("C:\Users\albusmw\Dropbox\Holzkirchen Strassennamenxx.txt", Names.ToArray)
    End Sub

    Private Sub ParseSitzungsProtokolle()
        Dim LinkCount As Integer = wbMain.Document.Links.Count
        Dim Downloaded As Integer = 0
        'Navigate through all links
        For Each Element As HtmlElement In wbMain.Document.Links
            Downloaded += 1
            'Get link and filter
            Dim Link As String = Element.GetAttribute("href")
            If Link.StartsWith("https://ris.komuna.net/holzkirchen/Meeting.mvc/Details") Then
                'Extract "Sitzungsnummer", "Typ der Sitzung" and Date
                Dim Number As String = Link.Substring(Link.LastIndexOf("/") + 1)
                Link = "https://ris.komuna.net/holzkirchen/Meeting.mvc/ps/" & Number    'Link is "Druckansicht", not the original page
                Dim PageContent As String = Downloader.DownloadString(Link)
                Dim SearchTyp As String = "Druckansicht: "
                Dim Sitzung As String = PageContent.Substring(PageContent.IndexOf(SearchTyp) + SearchTyp.Length, 100)
                Sitzung = Sitzung.Substring(0, Sitzung.IndexOf("<")).Replace("/", "-")
                Dim SearchDatum As String = "Datum:&nbsp;"
                Dim Datum = PageContent.Substring(PageContent.IndexOf(SearchDatum) + SearchDatum.Length, 20)
                Datum = Datum.Substring(0, Datum.IndexOf("<"))
                'Download all 
                System.IO.File.WriteAllText(Sitzung & " - " & Datum & " (" & Number & ").html", PageContent, System.Text.Encoding.Unicode)
                tbLinks.Text &= Link & System.Environment.NewLine
                System.Windows.Forms.Application.DoEvents()
            End If
            'Indicate progress
            tbProgress.Text = "Processed " & Downloaded.ToString.Trim & "/" & LinkCount.ToString.Trim & " links"
        Next Element
    End Sub

End Class
