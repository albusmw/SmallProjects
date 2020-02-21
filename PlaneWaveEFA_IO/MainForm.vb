Option Explicit On
Option Strict On

Public Class MainForm

    Private DB As New cDB

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        lbLog.Items.Clear()
        Run()
    End Sub

    Private Sub Run()

        Try

            '-----------------------------------------------------
            'Open COM port
            Dim EFA As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(DB.COMPort_EFA, 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)
            EFA.DiscardOutBuffer() : EFA.DiscardInBuffer()
            EFA.Handshake = IO.Ports.Handshake.None
            EFA.DtrEnable = True

            '=====================================================
            'Transmit

            'Position is correct ...
            Dim Position As Integer = GetPosition(EFA)
            Log("Raw position reading: <" & Position & ">")
            Log("   -> Decoded       : <" & Ato.PlaneWaveEFA.GetPositionMicrons(Position) & " µm>")
            LogSep("-"c)

            'GOTO over (focuser not moving any more) is correct ...
            Log("GOTO over       : <" & GetGotoOver(EFA) & ">")
            LogSep("-"c)

            'Temperature measurement is correct ...
            Log("T primary       : <" & GetTemperature(EFA, 0) & ">")
            LogSep("-"c)
            Log("T ambient       : <" & GetTemperature(EFA, 1) & ">")
            LogSep("-"c)
            Log("T secondary     : <" & GetTemperature(EFA, 2) & ">")
            LogSep("-"c)

            'Fans set and get is correct
            Log("Fans            : <" & GetFans(EFA) & ">")
            LogSep("-"c)
            Log("Set fans OFF    : <" & SetFans(EFA, False) & ">")
            LogSep("-"c)
            Log("Fans            : <" & GetFans(EFA) & ">")
            LogSep("-"c)

            '-----------------------------------------------------
            'Close again
            If IsNothing(EFA) = False Then
                If EFA.IsOpen Then EFA.Close()
            End If
            EFA = Nothing

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
        Log(" -> Status: " & COMCommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Return Ato.PlaneWaveEFA.MTR_GET_POS_decode(AnswerBuffer)

    End Function

    Private Function GetGotoOver(ByRef Port As IO.Ports.SerialPort) As Boolean

        Log("Running command <Get GOTO over> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.MTR_GOTO_OVER

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & COMCommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Return Ato.PlaneWaveEFA.MTR_GOTO_OVER_decode(AnswerBuffer)

    End Function

    Private Function GetTemperature(ByRef Port As IO.Ports.SerialPort, ByVal Sensor As Byte) As Double

        Log("Running command <Get T of sensor " & Sensor.ToString.Trim & "> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.TEMP_GET(Sensor)

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & COMCommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Return Ato.PlaneWaveEFA.TEMP_GET_decode(AnswerBuffer)

    End Function

    Private Function GetFans(ByRef Port As IO.Ports.SerialPort) As Boolean

        Log("Running command <Get Fans status> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.FANS_GET

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & COMCommunication(Port, CommandBuffer, AnswerBuffer))

        '-----------------------------------------------------
        'Decode
        Return Ato.PlaneWaveEFA.FANS_GET_decode(AnswerBuffer)

    End Function

    Private Function SetFans(ByRef Port As IO.Ports.SerialPort, ByVal State As Boolean) As Boolean

        Log("Running command <Set Fans status " & CStr(State) & "> ...")
        Ato.PlaneWaveEFA.PrepareCOM(Port)
        Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.FANS_SET(State)

        Dim AnswerBuffer As Byte() = {}
        Log(" -> Status: " & COMCommunication(Port, CommandBuffer, AnswerBuffer))

        Return True

    End Function

    Private Function COMCommunication(ByRef Port As IO.Ports.SerialPort, ByRef CommandBuffer As Byte(), ByRef AnswerBuffer As Byte()) As Boolean
        LogWrite(CommandBuffer)
        Port.Write(CommandBuffer, 0, CommandBuffer.Length)
        Dim RetVal As Boolean = Ato.PlaneWaveEFA.ValidateEcho(Port, CommandBuffer)
        AnswerBuffer = Ato.PlaneWaveEFA.ReadAnswer(Port)
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
    End Sub
End Class
