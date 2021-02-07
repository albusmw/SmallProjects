Option Explicit On
Option Strict On

Public Class MediaInfo_services

    Private Const InfoKindText As MediaInfo.eInfoKind = MediaInfo.eInfoKind.Text
    Private Const InfoKindName As MediaInfo.eInfoKind = MediaInfo.eInfoKind.Name

    Public Shared Function DescribeHeader(ByRef ListToUpdate As Windows.Forms.ListBox, ByVal LiveUpdate As Boolean) As String()
        Dim InfoAtoms As New List(Of String)
        InfoAtoms.Add("Filename")
        InfoAtoms.Add("Filedate")
        InfoAtoms.Add("Filelength")
        InfoAtoms.Add("Unique ID")
        InfoAtoms.Add("Videostream")
        InfoAtoms.Add("Bitrate")
        InfoAtoms.Add("Title")
        InfoAtoms.Add("AudioStreams")
        UpdateNow(ListToUpdate, InfoAtoms, LiveUpdate)
        Return InfoAtoms.ToArray
    End Function

    Public Shared Function DescribeFile(ByRef ListToUpdate As Windows.Forms.ListBox, ByVal FileName As String, ByVal LiveUpdate As Boolean) As String()

        Dim InfoAtoms As New List(Of String)
        Dim Info As New MediaInfo
        Info.Open(FileName)

        'Get file date and size
        Dim Fileinfos As New System.IO.FileInfo(FileName)
        InfoAtoms.Add(FileName)
        InfoAtoms.Add(Format(Fileinfos.CreationTime, "yyyy-MM-dd_HH:mm:ss"))
        InfoAtoms.Add(Fileinfos.Length.ToString.Trim)

        'Add unique ID
        InfoAtoms.Add(UniqueID(Info))

        'Add video stream information
        InfoAtoms.Add(VideoStreamInfo(Info))

        'Add bitrate
        InfoAtoms.Add(Bitrate(Info))

        'Add title
        InfoAtoms.Add(Title(Info))

        'Add audio stream information
        Dim Channel As Integer = -1
        Do
            Channel += 1
            Dim audioInfo As String = GetAudioInfo(Info, Channel)
            If String.IsNullOrEmpty(audioInfo) = True Then Exit Do
            InfoAtoms.Add(audioInfo)
        Loop Until 1 = 0

        'Close info handle
        Info.Close()

        'Update if live update is required
        UpdateNow(ListToUpdate, InfoAtoms, LiveUpdate)

        Return InfoAtoms.ToArray

    End Function

    Private Shared Sub UpdateNow(ByRef ListToUpdate As Windows.Forms.ListBox, ByRef Entries As List(Of String), ByVal DoUpdate As Boolean)
        If DoUpdate Then
            ListToUpdate.Items.Add(Strings.Join(Entries.ToArray, ";"))
            ListToUpdate.SelectedIndex = (ListToUpdate.Items.Count - 1)
            Windows.Forms.Application.DoEvents()
        End If
    End Sub

    Private Shared Function UniqueID(ByRef MI As MediaInfo) As String
        Dim RetVal As String = MI.GetInfo(MediaInfo.eStreamKind.General, 0, "UniqueID_String", InfoKindText, InfoKindName).ToUpper
        If RetVal.Contains(" (0X") = True Then RetVal = RetVal.Substring(0, RetVal.IndexOf(" (0X")).Trim
        Return RetVal
    End Function

    Private Shared Function VideoStreamInfo(ByRef MI As MediaInfo) As String
        Dim Info_width As String = MI.GetInfo(MediaInfo.eStreamKind.Visual, 0, "Width", InfoKindText, InfoKindName)
        If (MI.GetInfo(MediaInfo.eStreamKind.Visual, 0, "ScanType", InfoKindText, InfoKindName).ToUpper = "PROGRESSIVE") Then
            Info_width &= "p"
        Else
            Info_width &= "i"
        End If
        Dim Info_format As String = MI.GetInfo(MediaInfo.eStreamKind.Visual, 0, "Format", InfoKindText, InfoKindName)
        Return Info_width & " " & Info_format
    End Function

    Private Shared Function Bitrate(ByRef MI As MediaInfo) As String
        Dim item As String = MI.GetInfo(MediaInfo.eStreamKind.Visual, 0, "BitRate_String", InfoKindText, InfoKindName).Replace(" ", String.Empty).ToUpper
        If item.Contains("KBPS") Then
            item = (Strings.Format((Conversion.Val(item.Replace("KBPS", String.Empty)) / 1000), "0.0").Replace(",", ".") & " MBPS")
        End If
        item = item.Replace("MBPS", "MBit/s")
        Return item
    End Function

    Private Shared Function Title(ByRef MI As MediaInfo) As String
        Dim MovieTitle As String = MI.GetInfo(MediaInfo.eStreamKind.General, 0, "Movie", InfoKindText, InfoKindName).ToUpper
        If String.IsNullOrEmpty(MovieTitle) = True Then
            MovieTitle = MI.GetInfo(MediaInfo.eStreamKind.General, 0, "Title", InfoKindText, InfoKindName).ToUpper
        End If
        If String.IsNullOrEmpty(MovieTitle) = True Then MovieTitle = "---"
        Return MovieTitle
    End Function

    Private Shared Function GetAudioInfo(ByRef MI As MediaInfo, ByVal Channel As Integer) As String
        Dim str2 As String = MI.GetInfo(MediaInfo.eStreamKind.Audio, Channel, "Channel(s)", InfoKindText, InfoKindName)
        If String.IsNullOrEmpty(str2) Then
            Return String.Empty
        End If
        Dim str As String = MI.GetInfo(MediaInfo.eStreamKind.Audio, Channel, "ChannelPositions", InfoKindText, InfoKindName)
        If (str.IndexOf("/") > 0) Then
            str = str.Substring(0, str.IndexOf("/")).Trim
        End If
        If (str = "Front: L C R, Side: L R") Then
            str = "5.0"
        End If
        If (str = "Front: L C R, Side: L R, LFE") Then
            str = "5.1"
        End If
        If (str = "Front: L C R, Side: L R, Back: C, LFE") Then
            str = "6.1"
        End If
        If (str = "Front: L R") Then
            str = "2.0"
        End If
        If String.IsNullOrEmpty(str) Then
            str = str2
        End If
        Dim str4 As String = MI.GetInfo(MediaInfo.eStreamKind.Audio, Channel, "Language", InfoKindText, InfoKindName)
        If String.IsNullOrEmpty(str4) Then
            str4 = "unknown"
        End If
        Dim str3 As String = MI.GetInfo(MediaInfo.eStreamKind.Audio, Channel, "Format", InfoKindText, InfoKindName)
        Return String.Concat(New String() {str, " ", str4, " ", str3})
    End Function

End Class
