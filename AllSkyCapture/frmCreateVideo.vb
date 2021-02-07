Option Explicit On
Option Strict On

Public Class frmCreateVideo

    Private Class cDB

        <ComponentModel.DisplayName("1.) FFMPEG EXE path")>
        <ComponentModel.DefaultValue("C:\Program Files\MediathekView\bin\ffmpeg.exe")>
        Public Property FFMPEGEXE As String = "C:\Program Files\MediathekView\bin\ffmpeg.exe"

        <ComponentModel.DisplayName("2.) Image root folder")>
        <ComponentModel.DefaultValue("\\192.168.100.10\astro\AllSky")>
        Public Property ImageRoot As String = "\\192.168.100.10\astro\AllSky"

        <ComponentModel.DisplayName("3.) File header")>
        <ComponentModel.DefaultValue("AllSkyImage")>
        Public Property FileHeader As String = "AllSkyImage"

        <ComponentModel.DisplayName("4.) File list name")>
        <ComponentModel.DefaultValue("C:\TEMP\FFMPEG_AllSky\AllSkyImages.txt")>
        Public Property FileListName As String = "C:\TEMP\FFMPEG_AllSky\AllSkyImages.txt"

        <ComponentModel.DisplayName("5.) Video file name")>
        <ComponentModel.DefaultValue("C:\TEMP\FFMPEG_AllSky\AllSky.mp4")>
        Public Property VideoFileName As String = "C:\TEMP\FFMPEG_AllSky\AllSky.mp4"

        <ComponentModel.DisplayName("6.) Night start hour")>
        <ComponentModel.DefaultValue(18)>
        Public Property NightStart As Integer = 18

        <ComponentModel.DisplayName("7.) Night end hour")>
        <ComponentModel.DefaultValue(6)>
        Public Property NightEnd As Integer = 6

    End Class

    Private DB As New cDB

    Private NightFromTo As New Dictionary(Of Date, Dictionary(Of DateTime, String))
    Private FilesFromNight As New List(Of String)

    Private Sub ScanForFiles()

        Dim AllSkyDataFormat As String = "dd.MM.yyyy_HH.mm.ss"
        Dim FileCount As Integer = 0
        Dim OKCount As Integer = 0
        Dim SeqStartHour As Integer = DB.NightStart - 12
        Dim SeqEndHour As Integer = DB.NightEnd + 12

        'Get all files in the path and form a list of all "sequence dates" available
        NightFromTo.Clear()
        For Each File As String In System.IO.Directory.GetFiles(DB.ImageRoot, DB.FileHeader & "*.jpg")
            FileCount += 1
            Dim DateTimeString As String = System.IO.Path.GetFileNameWithoutExtension(File).Replace(DB.FileHeader, String.Empty)
            Dim Moment As DateTime
            If DateTime.TryParseExact(DateTimeString, AllSkyDataFormat, Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, Moment) Then
                Dim MomentForSequenceStart As DateTime = Moment.AddHours(-12)
                If MomentForSequenceStart.Hour >= SeqStartHour And MomentForSequenceStart.Hour < SeqEndHour Then
                    Dim SequenceDate As New Date(MomentForSequenceStart.Year, MomentForSequenceStart.Month, MomentForSequenceStart.Day)
                    If NightFromTo.ContainsKey(SequenceDate) = False Then NightFromTo.Add(SequenceDate, New Dictionary(Of DateTime, String))
                    NightFromTo(SequenceDate).Add(Moment, File)
                End If
                OKCount += 1
            End If
        Next File
        NightFromTo = NightFromTo.SortDictionary

        'List all available sequence dates
        lbSequences.Items.Clear()
        For Each Entry As Date In NightFromTo.Keys
            lbSequences.Items.Add(Entry & " (" & NightFromTo(Entry).Count.ValRegIndep & " files)")
        Next Entry

    End Sub

    Private Sub btnSearchFiles_Click(sender As Object, e As EventArgs) Handles btnSearchFiles.Click
        CType(sender, Button).Enabled = False
        ScanForFiles()
        CType(sender, Button).Enabled = True
    End Sub

    Private Sub frmCreateVideo_Load(sender As Object, e As EventArgs) Handles Me.Load
        pgMain.SelectedObject = DB
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click


        Dim FFMPEG As New ProcessStartInfo
        With FFMPEG
            .FileName = DB.FFMPEGEXE
            .WorkingDirectory = System.IO.Path.GetDirectoryName(DB.FFMPEGEXE)
            .Arguments = "-y -r 25 -f concat -safe 0 -i " & DB.FileListName & " -c:v libx264 -vf ""fps=25,format=yuv420p"" -report " & DB.VideoFileName
            .UseShellExecute = True
            .RedirectStandardError = False
        End With
        Dim Runner As Process = Process.Start(FFMPEG)
        Runner.WaitForExit()
        'MsgBox(Runner.StandardError.ReadToEnd)

    End Sub

    Private Sub btnGenerateFileList_Click(sender As Object, e As EventArgs) Handles btnGenerateFileList.Click
        Dim FFMPEGList As New List(Of String)
        For Each Entry As String In FilesFromNight
            FFMPEGList.Add("file '" & Entry & "'")
        Next
        System.IO.File.WriteAllLines(DB.FileListName, FFMPEGList.ToArray)
    End Sub

    Private Sub tsmiFile_DeleteSequenceFiles_Click(sender As Object, e As EventArgs) Handles tsmiFile_DeleteSequenceFiles.Click
        If MsgBox("Do you really want to delete <" & FilesFromNight.Count.ValRegIndep & "> files?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "SURE?") = MsgBoxResult.Yes Then
            Dim FailedCount As Integer = 0
            For Each File As String In FilesFromNight
                Try
                    System.IO.File.Delete(File)
                Catch ex As Exception
                    FailedCount += 1
                End Try
            Next File
            If FailedCount > 0 Then
                MsgBox("Failed to delete <" & FailedCount.ValRegIndep & "> files!", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
            Else
                MsgBox("All files deleted.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)
            End If
            ScanForFiles()
        End If
    End Sub

    Private Sub lbSequences_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbSequences.SelectedIndexChanged

        Dim SelectedMoment As Date
        FilesFromNight.Clear()
        btnGenerateFileList.Enabled = False
        If Date.TryParse(CStr(lbSequences.SelectedItem).Substring(0, 10), SelectedMoment) = True Then
            NightFromTo(SelectedMoment) = NightFromTo(SelectedMoment).SortDictionary
            For Each DateTimeToAdd As DateTime In NightFromTo(SelectedMoment).Keys
                FilesFromNight.Add(NightFromTo(SelectedMoment)(DateTimeToAdd))
            Next DateTimeToAdd

            btnGenerateFileList.Enabled = True
            btnGenerateFileList.Text = "Generate file list (" & FilesFromNight.Count.ValRegIndep & " file added"
        End If

    End Sub

End Class

