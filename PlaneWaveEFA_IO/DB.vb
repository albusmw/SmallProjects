Option Explicit On
Option Strict On

Public Class cDB

    '''<summary>COM port of the EFA unit.</summary>
    <ComponentModel.Category("1. Hardware")>
    <ComponentModel.DisplayName("a) EFA COM Port")>
    <ComponentModel.Description("COM port of the EFA unit")>
    <ComponentModel.DefaultValue("COM7")>
    Public Property COMPort_EFA As String = "COM7"

    '''<summary>COM port of the EFA unit.</summary>
    <ComponentModel.Category("1. Hardware")>
    <ComponentModel.DisplayName("b) DeltaT COM Port")>
    <ComponentModel.Description("COM port of the DeltaT unit")>
    <ComponentModel.DefaultValue("COM6")>
    Public Property COMPort_DeltaT As String = "COM6"

    '''<summary>COM port of the EFA unit.</summary>
    <ComponentModel.Category("1. Hardware")>
    <ComponentModel.DisplayName("c) Poll interval [s]")>
    <ComponentModel.Description("Poll interval of the hardware status")>
    <ComponentModel.DefaultValue(1.0)>
    Public Property PollInterval As Double = 1.0

    '''<summary>COM port of the EFA unit.</summary>
    <ComponentModel.Category("1. Hardware")>
    <ComponentModel.DisplayName("d) Close always?")>
    <ComponentModel.Description("Close COM port after each read?")>
    <ComponentModel.DefaultValue(False)>
    Public Property CloseAlways As Boolean = False

    '''<summary>Slew rate for in / out movement.</summary>
    Public Property Focuser_SlewRate As Integer = 4

    '''<summary>Slew time for in / out movement.</summary>
    Public Property Focuser_SlewTime As Byte = 100

    '''<summary>COM port of the EFA unit.</summary>
    <ComponentModel.Category("3. Debug")>
    <ComponentModel.DisplayName("a) Log COM I/O?")>
    <ComponentModel.Description("Log all detailed communication?")>
    <ComponentModel.DefaultValue(False)>
    Public Property LogComIO As Boolean = False

    '''<summary>INI access object.</summary>
    Public INI As New Ato.cINI_IO

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property EXEPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property MyINI As String = System.IO.Path.Combine(New String() {EXEPath, "Config.INI"})

    'WCF
    Public SetupWCF As ServiceModel.Web.WebServiceHost
    Public serviceBehavior As ServiceModel.Description.ServiceDebugBehavior

    'Execute 1 focuser move as configured
    Public RunFocuserMove As Boolean = False

End Class

Public Class cPWIValues

    <ComponentModel.Category("1. Focuser")>
    <ComponentModel.DisplayName("a) Focuser raw position")>
    Public Property Focuser_Raw As Integer = Integer.MinValue

    <ComponentModel.Category("1. Focuser")>
    <ComponentModel.DisplayName("b) Focuser um position")>
    Public Property Focuser_um As Double = Double.NaN

    <ComponentModel.Category("3. Readings")>
    <ComponentModel.DisplayName("a) T primary [°C]")>
    Public Property T_primary As Double = Double.NaN

    <ComponentModel.Category("3. Readings")>
    <System.ComponentModel.DisplayName("b) T secondary [°C]")>
    Public Property T_secondary As Double = Double.NaN

    <ComponentModel.Category("3. Readings")>
    <System.ComponentModel.DisplayName("c) T ambient [°C]")>
    Public Property T_ambient As Double = Double.NaN

    <ComponentModel.Category("3. Readings")>
    <System.ComponentModel.DisplayName("d) FAN")>
    Public Property Fan As Ato.cPWI_IO.eOnOff = Ato.cPWI_IO.eOnOff.Unknown

End Class
