Partial Public Class MediaInfo

    Public Enum eStreamKind As UInt32
        ' Fields
        Audio = 2
        Chapters = 4
        General = 0
        Image = 5
        Max = 7
        Menu = 6
        [Text] = 3
        Visual = 1
    End Enum

    Public Enum eInfoKind As UInt32
        ' Fields
        HowTo = 7
        Info = 6
        Max = 8
        Measure = 2
        MeasureText = 5
        Name = 0
        NameText = 4
        Options = 3
        [Text] = 1
    End Enum


    ' Methods
    Public Sub Close()
        MediaInfo.MediaInfo_Close(Me.Handle)
    End Sub

    Public Function Count_Get(ByVal StreamKind As eStreamKind, ByVal StreamNumber As UInt32) As Integer
        If (StreamNumber = UInt32.MaxValue) Then
            Dim num2 As Long = 0
            num2 = (num2 - 1)
            Return MediaInfo.MediaInfo_Count_Get(Me.Handle, StreamKind, num2)
        End If
        Return MediaInfo.MediaInfo_Count_Get(Me.Handle, StreamKind, StreamNumber)
    End Function

    Protected Overrides Sub Finalize()
        MediaInfo.MediaInfo_Delete(Me.Handle)
    End Sub

    Public Function GetInfo(ByVal StreamKind As eStreamKind, ByVal StreamNumber As Integer, ByVal Parameter As String, ByVal KindOfInfo As eInfoKind, ByVal KindOfSearch As eInfoKind) As String
        Return Runtime.InteropServices.Marshal.PtrToStringUni(MediaInfo.MediaInfo_Get(Me.Handle, StreamKind, StreamNumber, Parameter, KindOfInfo, KindOfSearch))
    End Function

    Public Function Inform() As String
        Return Runtime.InteropServices.Marshal.PtrToStringUni(MediaInfo.MediaInfo_Inform(Me.Handle, 0))
    End Function

    
    Public Function Open(ByVal FileName As String) As Integer
        Return MediaInfo.MediaInfo_Open(Me.Handle, FileName)
    End Function

    Public Function Option_(ByVal Option__ As String, Optional ByVal Value As String = "") As String
        Return Runtime.InteropServices.Marshal.PtrToStringUni(MediaInfo.MediaInfo_Option(Me.Handle, Option__, Value))
    End Function

    Public Function State_Get() As Integer
        Return MediaInfo.MediaInfo_State_Get(Me.Handle)
    End Function

    ' Fields
    Private Handle As IntPtr = MediaInfo.MediaInfo_New

End Class
