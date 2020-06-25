Option Explicit On
Option Strict On

Public Class frmCooling

    Private Sub frmCooling_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Cooling for " & M.DB.UsedCameraId.ToString
    End Sub

    Private Sub tQuery_Tick(sender As Object, e As EventArgs) Handles tQuery.Tick
        Dim CurrentTemp As Double = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
        Dim TargetTemp As Double = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_COOLER)
        Dim CurrentPWM As Double = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURPWM)
        tbT_measured.Text = ValRegIndep(CurrentTemp, "#0.0")
        tbT_set.Text = ValRegIndep(TargetTemp, "#0.0")
        tbCoolerPWM.Text = ValRegIndep(CurrentPWM, "#0")
        Dim Delta As Double = CurrentTemp - TargetTemp
        Dim DeltaDisp As Integer = CInt(Math.Abs(Delta * 10))
        If DeltaDisp > 100 Then DeltaDisp = 100
        pbDelta.Value = DeltaDisp
        If Delta > 0 Then
            pbDelta.ForeColor = Color.Red
        Else
            pbDelta.ForeColor = Color.Blue
        End If
    End Sub

    Private Sub btnSetTemp_Click(sender As Object, e As EventArgs) Handles btnSetTemp.Click
        Dim NewTemp As Double
        If Double.TryParse(InputBox("New target: ", "New temperature", ValRegIndep(M.DB.TargetTemp)), NewTemp) = True Then M.DB.TargetTemp = NewTemp
    End Sub

End Class