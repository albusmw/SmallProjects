Option Explicit On
Option Strict On

Public Class MainForm

    Private DB As New cDB
    Private PWIValues As New cPWIValues

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        lbLog.Items.Clear()
        Run()
        pgValues.SelectedObject = PWIValues
        pgValues.Refresh()
    End Sub

    Private Sub Run()

        Try

            '-----------------------------------------------------
            'Open COM ports
            Dim EFA_ComPort As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(DB.COMPort_EFA, 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)
            EFA_ComPort.DiscardOutBuffer() : EFA_ComPort.DiscardInBuffer()
            EFA_ComPort.Handshake = IO.Ports.Handshake.None
            EFA_ComPort.DtrEnable = True
            Dim DeltaT_ComPort As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(DB.COMPort_DeltaT, 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)
            DeltaT_ComPort.DiscardOutBuffer() : EFA_ComPort.DiscardInBuffer()
            DeltaT_ComPort.Handshake = IO.Ports.Handshake.None
            DeltaT_ComPort.DtrEnable = True

            '=====================================================
            'Transmit

            'Position is correct ...
            Dim Position As Integer = GetPosition(EFA_ComPort)
            Log("Raw position reading: <" & Position & ">")
            Log("   -> Decoded       : <" & Ato.PlaneWaveEFA.GetPositionMicrons(Position) & " µm>")
            LogSep("-"c)

            'GOTO over (focuser not moving any more) is correct ...
            Log("GOTO over       : <" & GetGotoOver(EFA_ComPort) & ">")
            LogSep("-"c)

            'Temperature measurement is correct ...
            Dim Status As String = String.Empty
            PWIValues.T_primary = GetEFATemp(EFA_ComPort, 0, Status)
            LogSep("-"c)
            PWIValues.T_ambient = GetEFATemp(EFA_ComPort, 1, Status)
            LogSep("-"c)
            PWIValues.T_secondary = GetEFATemp(EFA_ComPort, 2, Status)
            LogSep("-"c)

            'Fans set and get is correct
            PWIValues.Fan = GetFans(EFA_ComPort)
            LogSep("-"c)
            Log("Set fans OFF    : <" & SetFans(EFA_ComPort, False) & ">")
            LogSep("-"c)
            PWIValues.Fan = GetFans(EFA_ComPort)
            LogSep("-"c)

            '-----------------------------------------------------
            'Close again
            If IsNothing(EFA_ComPort) = False Then If EFA_ComPort.IsOpen Then EFA_ComPort.Close()
            If IsNothing(DeltaT_ComPort) = False Then If DeltaT_ComPort.IsOpen Then DeltaT_ComPort.Close()
            EFA_ComPort = Nothing
            DeltaT_ComPort = Nothing

        Catch ex As TimeoutException

            Log("Error on Serial Port: <" & ex.Message & ">")

        End Try

        Log("======================================================")

    End Sub

    Private Function GetPosition(ByRef Port As IO.Ports.SerialPort) As Integer

        Log("Running command <Get position> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.MTR_GET_POS

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & EFACommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Return Ato.PlaneWaveEFA.MTR_GET_POS_decode(AnswerBuffer)

    End Function

    Private Function GetGotoOver(ByRef Port As IO.Ports.SerialPort) As Boolean

        Log("Running command <Get GOTO over> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.MTR_GOTO_OVER

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & EFACommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Return Ato.PlaneWaveEFA.MTR_GOTO_OVER_decode(AnswerBuffer)

    End Function

    Private Function GetEFATemp(ByRef Port As IO.Ports.SerialPort, ByVal Sensor As Byte, ByRef Status As String) As Double

        Log("Running command <Get T of sensor " & Sensor.ToString.Trim & "> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.TEMP_GET(Sensor)

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & EFACommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Dim RetVal As Double = Ato.PlaneWaveEFA.TEMP_GET_decode(AnswerBuffer, Status)
        Log("T[" & Sensor.ToString.Trim & "            : <" & RetVal & "> (" & Status & ")")
        Return RetVal

    End Function

    Private Function GetFans(ByRef Port As IO.Ports.SerialPort) As Ato.PlaneWaveEFA.eOnOff

        Log("Running command <Get Fans status> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.FANS_GET

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & EFACommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Dim RetVal As Ato.PlaneWaveEFA.eOnOff = Ato.PlaneWaveEFA.FANS_GET_decode(AnswerBuffer)
        Log("Fans            : <" & RetVal & ">")
        Return RetVal

    End Function

    Private Function SetFans(ByRef Port As IO.Ports.SerialPort, ByVal State As Boolean) As Boolean

        Log("Running command <Set Fans status " & CStr(State) & "> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.FANS_SET(State)

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & EFACommunication(Port, CommandBuffer, AnswerBuffer))

        Return True

    End Function

    Private Function EFACommunication(ByRef EFAPort As IO.Ports.SerialPort, ByRef CommandBuffer As Byte(), ByRef AnswerBuffer As Byte()) As Boolean
        LogWrite(CommandBuffer)
        EFAPort.Write(CommandBuffer, 0, CommandBuffer.Length)
        Dim RetVal As Boolean = Ato.PlaneWaveEFA.ValidateEcho(EFAPort, CommandBuffer)
        AnswerBuffer = Ato.PlaneWaveEFA.ReadAnswer(EFAPort)
        LogRead(AnswerBuffer)
        Return RetVal
    End Function


    Private Sub LogWrite(ByRef Buffer As Byte())
        If IsNothing(Buffer) = True Then
            Log("  >> 0 byte")
        Else
            Log("  >> " & Ato.PlaneWaveEFA.AsHex(Buffer) & " (" & Buffer.Length.ToString.Trim & " byte)")
        End If
    End Sub

    Private Sub LogRead(ByRef Buffer As Byte())
        If IsNothing(Buffer) = True Then
            Log("  << 0 byte")
        Else
            Log("  << " & Ato.PlaneWaveEFA.AsHex(Buffer) & " (" & Buffer.Length.ToString.Trim & " byte)")
        End If

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim CB As New List(Of String)
        For Each Entry As String In lbLog.Items
            CB.Add(Entry)
        Next Entry
        Clipboard.Clear()
        Clipboard.SetText(Join(CB.ToArray, System.Environment.NewLine))
        MsgBox("OK")
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        pgMain.SelectedObject = DB
        pgValues.SelectedObject = PWIValues
    End Sub
End Class
