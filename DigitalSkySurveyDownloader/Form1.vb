Option Explicit On
Option Strict On

Public Class MainForm

    Dim BaseURL As String = "http://archive.eso.org/dss/dss/image?"
    Dim Format_GIF As String = "image/gif".Replace("/", "%2F")
    Dim Format_FITS As String = "application/x-fits".Replace("/", "%2F")

    Dim Downloader As New AstroCalc.NET.Common.cDownloader
    Dim StorageRoot As String = "\\DS001\astro\catalogs\DSS\DSS1\"

    Dim lbStatusClick As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Button1.Enabled = False
        lbStatusClick = False
        Downloader.InitWebClient()

        'Prepare all data
        Dim QueryString As New List(Of String)

        For Dec_Deg As Integer = -88 To 90 Step 1
            For Each Dec_Min As Integer In New Integer() {0, 30}
                For RA_HH As Integer = 0 To 23
                    For RA_Min As Integer = 0 To 58 Step 2
                        QueryString.Add("ra=" & Format(RA_HH, "00") & "+" & Format(RA_Min, "00") & "+00" & "&dec=" & Format(Dec_Deg, "00") & "+" & Format(Dec_Min, "00") & "+00")
                    Next RA_Min
                Next RA_HH
            Next Dec_Min
        Next Dec_Deg
        pbMain.Maximum = QueryString.Count + 1

        Dim CurrentIdx As Integer = 0
        Dim Failed As Integer = 0
        Dim TotalGByte As Double = 0
        For Each SingleQuery As String In QueryString
            CurrentIdx += 1
            Dim StorageFileName As String = StorageRoot & SingleQuery & ".FITS"
            pbMain.Value = CurrentIdx
            lbStatus.Text = SingleQuery & System.Environment.NewLine & "(" & CurrentIdx.ToString.Trim & "/" & QueryString.Count.ToString.Trim & ", " & Failed.ToString.Trim & " failed)" & System.Environment.NewLine & Format(TotalGByte, "0.00") & " GByte total"
            System.Windows.Forms.Application.DoEvents()
            If System.IO.File.Exists(StorageFileName) = False Then
                Dim Query As String = BaseURL
                Query &= SingleQuery & "&"
                Query &= "equinox=" & "J2000" & "&"
                Query &= "name=" & "" & "&"
                Query &= "x=" & "30" & "&"
                Query &= "y=" & "30" & "&"
                Query &= "Sky-Survey=" & "DSS1" & "&"
                Query &= "mime-type=" & Format_FITS & "&"
                Query &= "statsmode=" & "WEBFORM"
                If Downloader.DownloadFile(Query, StorageFileName) = False Then
                    Failed += 1
                    If System.IO.File.Exists(StorageFileName) Then System.IO.File.Delete(StorageFileName)
                Else
                    Dim FileSize As New System.IO.FileInfo(StorageFileName)
                    TotalGByte = (FileSize.Length / (1024 * 1024 * 1024)) * QueryString.Count
                End If
            End If
            If lbStatusClick = True Then Exit For
        Next SingleQuery

        pbMain.Value = 0
        Button1.Enabled = True

    End Sub

    Private Sub lbStatus_Click(sender As Object, e As EventArgs) Handles lbStatus.Click
        lbStatusClick = True
    End Sub

End Class
