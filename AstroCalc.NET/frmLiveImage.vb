Option Explicit On
Option Strict On

Public Class frmLiveImage

    Private Sub SDOHMIContinuumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SDOHMIContinuumToolStripMenuItem.Click
        Dim URL As String = "http://sohowww.nascom.nasa.gov/data/realtime/hmi_igr/1024/latest.jpg"
        LoadImage(URL)
    End Sub

    Private Sub TimesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimesToolStripMenuItem.Click
        Dim Lat As String = CStr(Globals.Latitude).Trim
        Dim Lon As String = CStr(Globals.Longitude).Trim
        Dim URL As String = "http://ftp.astron.ac.cn/v4/bin/astro.php?lon=" & Lon & "&lat=" & Lat & "&lang=en&ac=0&unit=metric&output=internal&tzshift=0"
        LoadImage(URL)
    End Sub

    Private Sub LoadImage(ByVal URL As String)
      DB.InitWebClient(MainForm.UseProxyToolStripMenuItem.Checked)
    Dim ImageDate As Byte() = DB.Downloader.DownloadData(URL)
    pbMain.Image = Image.FromStream(New IO.MemoryStream(ImageDate))
    End Sub

End Class