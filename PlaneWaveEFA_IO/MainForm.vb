Option Explicit On
Option Strict On

Public Class MainForm

  Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
    lbLog.Items.Clear()
    Run()
  End Sub

  Private Sub Run()

    Try

      '-----------------------------------------------------
      'Open COM port
      Dim EFA As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort("COM5", 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)
      EFA.DiscardOutBuffer() : EFA.DiscardInBuffer()
      EFA.Handshake = IO.Ports.Handshake.None
      EFA.DtrEnable = True

      '=====================================================
      'Transmit


      Dim Position As Integer = GetPosition(EFA)
      Log("Decoded position: <" & Position & ">")
      Log("                  <" & Ato.PlaneWaveEFA.GetPositionMicrons(Position) & " µm>")

      Log("GOTO over       : <" & GetGotoOver(EFA) & ">")
      Log("T primary       : <" & GetTemperature(EFA, 0) & ">")
      Log("T ambient       : <" & GetTemperature(EFA, 1) & ">")
      Log("T secondary     : <" & GetTemperature(EFA, 2) & ">")

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

    'Log("Running command <Get position> ...")
    Ato.PlaneWaveEFA.PrepareCOM(Port)
    Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.MTR_GET_POS
    
    Dim AnswerBuffer As Byte() = {}
    COMCommunication(Port, CommandBuffer, AnswerBuffer)

    '-----------------------------------------------------
    'Decode
    Return Ato.PlaneWaveEFA.MTR_GET_POS_decode(AnswerBuffer)

  End Function

  Private Function GetGotoOver(ByRef Port As IO.Ports.SerialPort) As Boolean

    ' Log("Running command <Get GOTO over> ...")
    Ato.PlaneWaveEFA.PrepareCOM(Port)
    Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.MTR_GOTO_OVER

    Dim AnswerBuffer As Byte() = {}
    COMCommunication(Port, CommandBuffer, AnswerBuffer)

    '-----------------------------------------------------
    'Decode
    Return Ato.PlaneWaveEFA.MTR_GOTO_OVER_decode(AnswerBuffer)

  End Function

  Private Function GetTemperature(ByRef Port As IO.Ports.SerialPort, ByVal Sensor As Byte) As Double

    'Log("Running command <Get GOTO over> ...")
    Ato.PlaneWaveEFA.PrepareCOM(Port)
    Dim CommandBuffer As Byte() = Ato.PlaneWaveEFA.TEMP_GET(Sensor)

    Dim AnswerBuffer As Byte() = {}
    COMCommunication(Port, CommandBuffer, AnswerBuffer)

    '-----------------------------------------------------
    'Decode
    Return Ato.PlaneWaveEFA.TEMP_GET_decode(AnswerBuffer)

  End Function

  Private Sub COMCommunication(ByRef Port As IO.Ports.SerialPort, ByRef CommandBuffer As Byte(), ByRef AnswerBuffer As Byte())
    'LogWrite(CommandBuffer)
    Port.Write(CommandBuffer, 0, CommandBuffer.Length)
    Ato.PlaneWaveEFA.ValidateEcho(Port, CommandBuffer)
    AnswerBuffer = Ato.PlaneWaveEFA.ReadAnswer(Port)
    LogRead(AnswerBuffer)
  End Sub


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
    lbLog.Items.Add(Text)
    lbLog.SelectedIndex = lbLog.Items.Count - 1
    System.Windows.Forms.Application.DoEvents()
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

End Class
