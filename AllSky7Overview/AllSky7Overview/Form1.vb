Option Explicit On
Option Strict On

Public Class Form1

    Public DB As New cDB
    Public Log As New cLogging

    Dim Username As String = "AMS58"
    Dim Password As String = "meteors"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each Entry As cDB.sCam In DB.Cams
            cbAllCams.Items.Add(Entry.URL)
        Next Entry
        cbAllCams.SelectedIndex = 0
        'pbMain.Image = GetImageFromURL("https://ams58/meteor_archive/AMS58/STACKS/2021_04_19/010356-night-stack.jpg")
    End Sub

    Private Function GetImageFromURL(ByVal url As String) As Image
        Dim X As New cDownloader
        X.InitWebClient(Username, Password)
        Return X.DownloadImage(url)
    End Function

    '''<summary>Get all camera serial numbers for the specified site.</summary>
    '''<param name="PageContent">Main page source code.</param>
    '''<returns>List of camera serials</returns>
    Private Function GetAllCameras(ByVal PageContent As String) As List(Of String)
        Dim Cams As New List(Of String)
        Dim FoundIdx As Integer = -1
        Do
            FoundIdx = PageContent.IndexOf("Cam #", FoundIdx + 1)
            If FoundIdx = -1 Then
                Exit Do
            Else
                Dim Cam As String = CStr(PageContent.Substring(FoundIdx + 5, 6)).Trim
                If Cams.Contains(Cam) = False Then Cams.Add(Cam)
                FoundIdx += 5
            End If
        Loop Until 1 = 0
        Return Cams
    End Function

    Private Sub AddNewLog(ByVal LastPtr As Long)
        Dim NL As String = System.Environment.NewLine
        ltbMain.Log(cLogging.sLogEntry.Verbose(Log.GetNewContent(LastPtr - 1)))
        ltbMain.ScrollToEnd()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub tsmiOnline_AllCameras_Click(sender As Object, e As EventArgs) Handles tsmiOnline_AllCameras.Click
        For Each AllSky7URL As String In cbAllCams.Items
            AddNewLog(Log.Log("Loading cameras of <" & AllSky7URL & ">"))
            'Get the correct URL and AMS number to load the page
            Dim AllSky7Camera As cDB.sCam = DB.GetAMSNumber(AllSky7URL)
            If AllSky7Camera.URL.Contains("holzkirchen.allsky7.net") Then AllSky7Camera.URL = "https://ams58/"
            'Download the content
            Dim X As New cDownloader
            X.InitWebClient(AllSky7Camera.UserName, AllSky7Camera.Password)
            Dim PageContent As String = System.Text.Encoding.UTF8.GetString(X.DownloadFile(AllSky7Camera.URL & cDB.InitialPage & "/" & AllSky7Camera.UserName & "/"))
            'Parse all cameras from the page
            Dim Cams As List(Of String) = GetAllCameras(PageContent)
            AddNewLog(Log.Log("  -> " & Join(Cams.ToArray, "/")))
        Next AllSky7URL
    End Sub

End Class
