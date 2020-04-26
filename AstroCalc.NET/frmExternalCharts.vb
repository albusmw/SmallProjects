Option Explicit On
Option Strict On

Public Class frmExternalCharts

  Private Sub frmExternalCharts_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    cbSelection.Items.Clear()
    For Each Entry As sConstellation In DB.Constellation.Catalog
      cbSelection.Items.Add(Entry.IAU & " (" & Entry.Latin & " - " & Entry.German & ")")
    Next Entry

  End Sub

  Private Sub cbSelection_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbSelection.SelectedIndexChanged
    Dim Selected As String = cbSelection.Text.Substring(0, cbSelection.Text.IndexOf(" (")).Trim.ToUpper
    Dim URL As String = "http://www.iau.org/static/public/constellations/gif/" & Selected & ".gif"
    LoadImage(URL)
  End Sub

  Private Sub LoadImage(ByVal URL As String)
    DB.InitWebClient(MainForm.UseProxyToolStripMenuItem.Checked)
    Dim ImageDate As Byte() = DB.Downloader.DownloadData(URL)
    pbMain.Image = Image.FromStream(New IO.MemoryStream(ImageDate))
  End Sub

End Class