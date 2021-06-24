Option Explicit On
Option Strict On

'''<summary>Helper functions for access to the FlyCapture2Managed class.</summary>
'''<remarks>Location: C:\Program Files\Point Grey Research\FlyCap2 Viewer\bin64\FlyCapture2Managed_v100.dll</remarks>
Public Class FlyCapture

    Public Shared Function SetOnOffProperty(ByVal Camera As FlyCapture2Managed.ManagedCamera, ByVal PropertyToSet As FlyCapture2Managed.PropertyType, ByVal State As Boolean) As String
        Dim RetVal As String = String.Empty
        Try
            Dim Prop As New FlyCapture2Managed.CameraProperty(PropertyToSet) : Prop.onOff = State
            Camera.SetProperty(Prop)
            Dim ReadBack As FlyCapture2Managed.CameraProperty = Camera.GetProperty(PropertyToSet)
            If ReadBack.onOff <> State Then RetVal = "OnOff: " & PropertyToSet.ToString.Trim & " set to <" & State.ToString.Trim & "> but got back <" & ReadBack.onOff.ToString.Trim & ">"
        Catch ex As Exception
            RetVal = "!!!" & ex.Message
        End Try
        Return RetVal
    End Function

    Public Shared Function SetautoManualModeProperty(ByVal Camera As FlyCapture2Managed.ManagedCamera, ByVal PropertyToSet As FlyCapture2Managed.PropertyType, ByVal State As Boolean) As String
        Dim RetVal As String = String.Empty
        Try
            Dim Prop As New FlyCapture2Managed.CameraProperty(PropertyToSet) : Prop.autoManualMode = State
            Camera.SetProperty(Prop)
            Dim ReadBack As FlyCapture2Managed.CameraProperty = Camera.GetProperty(PropertyToSet)
            If ReadBack.autoManualMode <> State Then RetVal = "AutoManualMode: " & PropertyToSet.ToString.Trim & " set to <" & State.ToString.Trim & "> but got back <" & ReadBack.autoManualMode.ToString.Trim & ">"
        Catch ex As Exception
            RetVal = "!!!" & ex.Message
        End Try
        Return RetVal
    End Function

    Public Shared Function SetabsValueProperty(ByVal Camera As FlyCapture2Managed.ManagedCamera, ByVal PropertyToSet As FlyCapture2Managed.PropertyType, ByVal Value As Single) As String
        Dim RetVal As String = String.Empty
        Try
            Dim Prop As New FlyCapture2Managed.CameraProperty(PropertyToSet)
            If PropertyToSet <> FlyCapture2Managed.PropertyType.Sharpness Then Prop.absControl = True
            Prop.absValue = Value
            Camera.SetProperty(Prop)
            Dim ReadBack As FlyCapture2Managed.CameraProperty = Camera.GetProperty(PropertyToSet)
            If ReadBack.absValue <> Value Then RetVal = "AbsValue: " & PropertyToSet.ToString.Trim & " set to <" & Value.ToString.Trim & "> but got back <" & ReadBack.absValue.ToString.Trim & ">"
        Catch ex As Exception
            RetVal = "!!!" & ex.Message
        End Try
        Return RetVal
    End Function

End Class
