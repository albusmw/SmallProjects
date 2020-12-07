Option Explicit On
Option Strict On

Public Class cDB
    '''<summary>COM port of the EFA unit.</summary>
    Public Property COMPort_EFA As String = "COM7"
    '''<summary>COM port of the EFA unit.</summary>
    Public Property COMPort_DeltaT As String = "COM6"
End Class

Public Class cPWIValues
    <System.ComponentModel.DisplayName("a) T primary [°C]")>
    Public Property T_primary As Double = Double.NaN
    <System.ComponentModel.DisplayName("b) T secondary [°C]")>
    Public Property T_secondary As Double = Double.NaN
    <System.ComponentModel.DisplayName("c) T ambient [°C]")>
    Public Property T_ambient As Double = Double.NaN
    <System.ComponentModel.DisplayName("d) FAN")>
    Public Property Fan As Ato.PlaneWaveEFA.eOnOff = Ato.PlaneWaveEFA.eOnOff.Unknown
End Class
