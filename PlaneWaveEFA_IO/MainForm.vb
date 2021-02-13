Option Explicit On
Option Strict On

Public Class MainForm

    '''<summary>Database for all properties.</summary>
    Private DB As New cDB
    '''<summary>Database for all hardware related values.</summary>
    Private PWIValues As New cPWIValues

    '''<summary>Hardware communication interface.</summary>
    Private WithEvents PWI_IO As New Ato.cPWI_IO
    '''<summary>WCF interface.</summary>
    Private WithEvents DB_ServiceContract As cDB_ServiceContract

    '''<summary>COM interface - EFA.</summary>
    Private EFA_ComPort As IO.Ports.SerialPort = Nothing
    '''<summary>COM interface - DeltaT heater.</summary>
    Private DeltaT_ComPort As IO.Ports.SerialPort = Nothing

    Private Sub Run()

        lbLog.Items.Clear()

        Try

            '-----------------------------------------------------
            'Open COM ports
            If IsNothing(EFA_ComPort) = True Then
                EFA_ComPort = My.Computer.Ports.OpenSerialPort(DB.COMPort_EFA, 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)
                EFA_ComPort.DiscardOutBuffer() : EFA_ComPort.DiscardInBuffer()
                EFA_ComPort.Handshake = IO.Ports.Handshake.None
                EFA_ComPort.DtrEnable = True
            End If
            If IsNothing(DeltaT_ComPort) = True Then
                DeltaT_ComPort = My.Computer.Ports.OpenSerialPort(DB.COMPort_DeltaT, 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)
                DeltaT_ComPort.DiscardOutBuffer() : EFA_ComPort.DiscardInBuffer()
                DeltaT_ComPort.Handshake = IO.Ports.Handshake.None
                DeltaT_ComPort.DtrEnable = True
            End If

            '-----------------------------------------------------
            'Execute pending commands
            If DB.RunFocuserMove Then
                DB.RunFocuserMove = False
                MoveFocuser()
            End If

            '=====================================================
            'Transmit

            'Position is correct ...
            PWIValues.Focuser_Raw = PWI_IO.GetPosition(EFA_ComPort)
            PWIValues.Focuser_um = PWI_IO.GetPositionMicrons(PWIValues.Focuser_Raw)
            Log("Raw position reading: <" & PWIValues.Focuser_Raw & ">")
            Log("   -> Decoded       : <" & PWIValues.Focuser_um & " µm>")
            LogSep("-"c)

            'GOTO over (focuser not moving any more) is correct ...
            Log("GOTO over       : <" & PWI_IO.GetGotoOver(EFA_ComPort) & ">")
            LogSep("-"c)

            'Temperature measurement is correct ...
            Dim Status As String = String.Empty
            PWIValues.T_primary = PWI_IO.GetEFATemp(EFA_ComPort, Ato.cPWI_IO.eAddr.TemperatureSensor, 0, Status)
            LogSep("-"c)
            PWIValues.T_ambient = PWI_IO.GetEFATemp(EFA_ComPort, Ato.cPWI_IO.eAddr.TemperatureSensor, 1, Status)
            LogSep("-"c)
            PWIValues.T_secondary = PWI_IO.GetEFATemp(EFA_ComPort, Ato.cPWI_IO.eAddr.TemperatureSensor, 2, Status)
            LogSep("-"c)

            'Fans set and get is correct
            PWIValues.Fan = PWI_IO.GetFans(EFA_ComPort)
            LogSep("-"c)

            '-----------------------------------------------------
            'Close again
            If DB.CloseAlways Then
                If IsNothing(EFA_ComPort) = False Then If EFA_ComPort.IsOpen Then EFA_ComPort.Close()
                If IsNothing(DeltaT_ComPort) = False Then If DeltaT_ComPort.IsOpen Then DeltaT_ComPort.Close()
                EFA_ComPort = Nothing
                DeltaT_ComPort = Nothing
            End If

        Catch ex As TimeoutException

            Log("Error on Serial Port: <" & ex.Message & ">")

        End Try

        Log("======================================================")

        pgValues.SelectedObject = PWIValues
        pgValues.Refresh()

    End Sub

    '''<summary>Move focuser as configured in the properties.</summary>
    Private Sub MoveFocuser()
        Log("Move focuser +    : <" & PWI_IO.MoveFocuser(EFA_ComPort, CBool(IIf(DB.Focuser_SlewRate > 0, True, False)), CByte(Math.Abs(DB.Focuser_SlewRate))) & ">")
        LogSep("-"c)
        System.Threading.Thread.Sleep(DB.Focuser_SlewTime)
        Log("Stop focuser +    : <" & PWI_IO.MoveFocuser(EFA_ComPort, True, 0) & ">")
        LogSep("-"c)
    End Sub

    Private Sub SetFan()

        Log("Set fans OFF    : <" & PWI_IO.SetFans(EFA_ComPort, False) & ">")
        LogSep("-"c)
        PWIValues.Fan = PWI_IO.GetFans(EFA_ComPort)
        LogSep("-"c)

    End Sub

    Private Sub Log(ByVal Text As String)
        Log(0, Text)
    End Sub

    Private Sub Log(ByVal Indent As Integer, ByVal Text As String)
        lbLog.Items.Add(Space(Indent) & Text)
        lbLog.SelectedIndex = lbLog.Items.Count - 1
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub LogSep(ByVal Text As Char)
        Log(New String(Text, 80))
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Load INI
        DB.INI.Load(DB.MyINI)

        'Start WCF
        InitWCF()

        'Display database objects
        pgMain.SelectedObject = DB
        pgValues.SelectedObject = PWIValues

    End Sub

    Private Sub PWI_IO_Log(Message As String) Handles PWI_IO.Log
        Log(Message)
    End Sub

    Private Sub PWI_IO_LogCOMIO(Message As String) Handles PWI_IO.LogCOMIO
        If DB.LogComIO Then Log(20, Message)
    End Sub

    Private Sub tUpdater_Tick(sender As Object, e As EventArgs) Handles tUpdater.Tick
        If DB.PollInterval > 0.0 Then
            'Update poll interval
            If tUpdater.Interval <> CInt(DB.PollInterval * 1000) Then
                tUpdater.Interval = CInt(DB.PollInterval * 1000)
            End If
            'Read settings
            Run()
        End If
    End Sub

    '''<summary>Start the Windows Communication Foundation (WCF) interface.</summary>
    Private Sub InitWCF()

        'netsh http add urlacl url=http://+:1260/ user=DESKTOP-I7\albusmw
        DB_ServiceContract = New cDB_ServiceContract(DB, PWIValues)
        Dim WebServicePort As String = DB.INI.Get("Connections", "WebInterfacePort", "1260")
        If WebServicePort <> "0" Then
            Dim WebServiceAdr As String = "http://localhost:" & WebServicePort & "/"
            DB.SetupWCF = New ServiceModel.Web.WebServiceHost(GetType(cDB_ServiceContract), New Uri(WebServiceAdr))
            DB.serviceBehavior = DB.SetupWCF.Description.Behaviors.Find(Of ServiceModel.Description.ServiceDebugBehavior)
            DB.serviceBehavior.HttpHelpPageEnabled = True
            DB.serviceBehavior.IncludeExceptionDetailInFaults = True
            DB.SetupWCF.Open()
        End If

    End Sub

    Private Sub tsmiFile_OpenEXE_Click(sender As Object, e As EventArgs) Handles tsmiFile_OpenEXE.Click
        Process.Start(DB.EXEPath)
    End Sub

    Private Sub tsmiFile_Exit_Click(sender As Object, e As EventArgs) Handles tsmiFile_Exit.Click
        End
    End Sub

    Private Sub tsmiFile_WCF_Click(sender As Object, e As EventArgs) Handles tsmiFile_WCF.Click
        System.Diagnostics.Process.Start("http://localhost:1260/GetParameterList")
    End Sub

    Private Sub tsmiRun_SingleQuery_Click(sender As Object, e As EventArgs) Handles tsmiRun_SingleQuery.Click
        Run()
    End Sub

    Private Sub tsmiFile_CopyLog_Click(sender As Object, e As EventArgs) Handles tsmiFile_CopyLog.Click
        Dim CB As New List(Of String)
        For Each Entry As String In lbLog.Items
            CB.Add(Entry)
        Next Entry
        Clipboard.Clear()
        Clipboard.SetText(Join(CB.ToArray, System.Environment.NewLine))
        MsgBox("OK")
    End Sub

    Private Sub FocuserMoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FocuserMoveToolStripMenuItem.Click
        DB.RunFocuserMove = True
    End Sub

End Class
