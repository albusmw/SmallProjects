Option Explicit On
Option Strict On

Public Class MainForm

    Private DB As New cDB

    Private Sub RunCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunCaptureToolStripMenuItem.Click

        Dim Stopper As New cStopper
        Stopper.Start()

        Dim SDKVersion(3) As UInteger

        Dim Chip_Physical_W As Double = Double.NaN
        Dim Chip_Physical_H As Double = Double.NaN
        Dim Chip_Pixel_W As UInteger = 0
        Dim Chip_Pixel_H As UInteger = 0
        Dim Pixel_Size_W As Double = Double.NaN
        Dim Pixel_Size_H As Double = Double.NaN

        Dim EffArea_X As UInteger = 0
        Dim EffArea_Y As UInteger = 0
        Dim EffArea_W As UInteger = 0
        Dim EffArea_H As UInteger = 0

        Dim OverArea_X As UInteger = 0
        Dim OverArea_Y As UInteger = 0
        Dim OverArea_W As UInteger = 0
        Dim OverArea_H As UInteger = 0

        Dim bpp As UInteger = 0
        Dim CamNumToUse As Integer = 0

        'Single Image Capture Mode Workflow

        'GetQHYCCDSingleFrame(camhandle,& w,&h,&bpp,& channels, ImgData);

        If CallOK(QHY.QHYCamera.InitQHYCCDResource) = True Then                                                             'Init DLL itself
            Stopper.Stamp("InitQHYCCDResource")
            Dim CameraCount As UInteger = QHY.QHYCamera.ScanQHYCCD                                                                      'Scan for connected cameras
            Stopper.Stamp("ScanQHYCCD")
            If CameraCount > 0 Then                                                                                                     'If there is a camera found
                Dim CameraId As New System.Text.StringBuilder(0)                                                                        'Prepare camera ID holder
                CallOK(QHY.QHYCamera.GetQHYCCDId(CamNumToUse, CameraId))                                                                'Fetch camera ID
                Log("Found QHY camera <" & CameraId.ToString & ">")                                                                     'Display fetched camera ID
                Dim CamHandle As IntPtr = QHY.QHYCamera.OpenQHYCCD(CameraId)                                                            'Open the camera

                'NEW SDK READOUT MODE
                Dim ReadoutModesCount As UInteger = 0
                CallOK(QHY.QHYCamera.GetQHYCCDNumberOfReadModes(CamHandle, ReadoutModesCount))

                Dim AllReadOutModes As New Collections.Generic.List(Of String)
                For ReadoutMode As UInteger = 0 To CUInt(ReadoutModesCount - 1)
                    Dim ReadoutModeName As New Text.StringBuilder
                    Dim ResX As UInteger = 0 : Dim ResY As UInteger = 0
                    CallOK(QHY.QHYCamera.GetQHYCCDReadModeName(CamHandle, ReadoutMode, ReadoutModeName))
                    CallOK(QHY.QHYCamera.GetQHYCCDReadModeResolution(CamHandle, ReadoutMode, ResX, ResY))
                    AllReadOutModes.Add(ReadoutMode.ValRegIndep & ": " & ReadoutModeName.ToString & " (" & ResX.ValRegIndep & "x" & ResY.ValRegIndep & ")")
                Next ReadoutMode
                Log("Available read-out modes:")
                Log(AllReadOutModes)
                CallOK(QHY.QHYCamera.SetQHYCCDReadMode(CamHandle, 0))

                If CallOK(QHY.QHYCamera.SetQHYCCDStreamMode(CamHandle, 0)) = True Then                                                  'Set single capture mode
                    If CallOK(QHY.QHYCamera.InitQHYCCD(CamHandle)) = True Then                                                          'Init the camera with the selected mode, ...

                        'Display info for the cam
                        QHY.QHYCamera.GetQHYCCDChipInfo(CamHandle, Chip_Physical_W, Chip_Physical_H, Chip_Pixel_W, Chip_Pixel_H, Pixel_Size_W, Pixel_Size_H, bpp)                   'Get chip info
                        Log("Chip info (bpp: " & bpp.ValRegIndep & ")")
                        Log("  Chip  W x H    :" & Chip_Physical_W.ValRegIndep & " x " & Chip_Physical_H.ValRegIndep & " mm")
                        Log("  Image W x H    :" & Chip_Pixel_W.ValRegIndep & " x " & Chip_Pixel_H.ValRegIndep & " pixel")
                        Log("  Pixel W x H    :" & Pixel_Size_W.ValRegIndep & " x " & Pixel_Size_H.ValRegIndep & " um")
                        QHY.QHYCamera.GetQHYCCDEffectiveArea(CamHandle, EffArea_X, EffArea_Y, EffArea_W, EffArea_H)
                        Log("CCD Effective Area:")
                        Log("  Start X:Y    :" & EffArea_X.ValRegIndep & ":" & EffArea_Y.ValRegIndep)
                        Log("  Size  X x Y  :" & EffArea_W.ValRegIndep & " x " & EffArea_H.ValRegIndep)
                        QHY.QHYCamera.GetQHYCCDOverScanArea(CamHandle, OverArea_X, OverArea_Y, OverArea_W, OverArea_H)
                        Log("CCD Overscan Area:")
                        Log("  Start X:Y    :" & OverArea_X.ValRegIndep & ":" & OverArea_Y.ValRegIndep)
                        Log("  Size  X x Y  :" & OverArea_W.ValRegIndep & " x " & OverArea_H.ValRegIndep)

                        Log("==============================================================================")
                        QHY.QHYCamera.GetQHYCCDSDKVersion(SDKVersion(0), SDKVersion(1), SDKVersion(2), SDKVersion(3))                   'Get the SDK version
                        Log("SDK version: " & BuildSDKVersion(SDKVersion))                                                            'Display the SDK version
                        Log("ControlValues:")                                                                                           'Start reading all control values
                        Stopper.Start()

                        'Display all properties available
                        For Each X As QHY.QHYCamera.CONTROL_ID In [Enum].GetValues(GetType(QHY.QHYCamera.CONTROL_ID))                   'Move over all Control ID's
                            If QHY.QHYCamera.IsQHYCCDControlAvailable(CamHandle, X) <> QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then    'If control is available
                                Log("  " & X.ToString.Trim.PadRight(40) & ": NOT AVAILABLE")
                            Else
                                Dim Min As Double = Double.NaN
                                Dim Max As Double = Double.NaN
                                Dim Stepping As Double = Double.NaN
                                Dim CurrentValue As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, X)
                                If QHY.QHYCamera.GetQHYCCDParamMinMaxStep(CamHandle, X, Min, Max, Stepping) = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then
                                    Log("  " & X.ToString.Trim.PadRight(40) & ": " & Min.ValRegIndep & " ... <" & Stepping.ValRegIndep & "> ... " & Max.ValRegIndep & ", current: " & CurrentValue.ValRegIndep)
                                Else
                                    Select Case CurrentValue
                                        Case UInteger.MaxValue
                                            Log("  " & X.ToString.Trim.PadRight(40) & ": BOOL, current: TRUE")
                                        Case 0
                                            Log("  " & X.ToString.Trim.PadRight(40) & ": BOOL, current: FALSE")
                                        Case Else
                                            Log("  " & X.ToString.Trim.PadRight(40) & ": Discret, current: " & CurrentValue.ValRegIndep)
                                    End Select
                                End If
                            End If
                        Next X
                        Stopper.Stamp("GetQHYCCDParam")
                        Log("==============================================================================")

                        'Prepare buffers
                        Dim ReadResolution As UInteger = 16
                        Dim ChannelToRead As UInteger = 0

                        Stopper.Stamp("Prepare buffers")

                        'Set exposure parameters
                        CallOK("SetQHYCCDBinMode", QHY.QHYCamera.SetQHYCCDBinMode(CamHandle, DB.Binning, DB.Binning))
                        CallOK("SetQHYCCDResolution", QHY.QHYCamera.SetQHYCCDResolution(CamHandle, 0, 0, Chip_Pixel_W \ DB.Binning, Chip_Pixel_H \ DB.Binning))
                        CallOK("CONTROL_TRANSFERBIT", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_TRANSFERBIT, ReadResolution))
                        CallOK("CONTROL_GAIN", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN, DB.Gain))
                        CallOK("CONTROL_OFFSET", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET, DB.Offset))
                        CallOK("CONTROL_EXPOSURE", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE, DB.ExposureTime * 1000000))

                        Dim LoopStat As New AstroNET.Statistics.sStatistics
                        Dim PinHandler As cIntelIPP.cPinHandler
                        Dim CamRawBuffer As Byte() = {}
                        Dim CamRawBufferPtr As IntPtr = Nothing

                        For LoopCnt As Integer = 1 To DB.CaptureCount

                            'Expose
                            Stopper.Start()
                            CallOK("ExpQHYCCDSingleFrame", QHY.QHYCamera.ExpQHYCCDSingleFrame(CamHandle))
                            Stopper.Stamp("ExpQHYCCDSingleFrame")

                            'Get the buffer size from the DLL - typically too big but does not care ...
                            Dim BytesToTransfer_reported As UInteger = QHY.QHYCamera.GetQHYCCDMemLength(CamHandle)
                            Stopper.Stamp("GetQHYCCDMemLength")
                            Log("GetQHYCCDMemLength says: " & BytesToTransfer_reported.ValRegIndep.PadLeft(12) & " byte to transfer.")
                            If CamRawBuffer.Length <> BytesToTransfer_reported Then
                                PinHandler = New cIntelIPP.cPinHandler
                                ReDim CamRawBuffer(CInt(BytesToTransfer_reported - 1))
                                CamRawBufferPtr = PinHandler.Pin(CamRawBuffer)
                            End If

                            'Read image data from camera - ALWAYS WITH OVERSCAN
                            Dim Capture_W As UInteger = 0 : Dim Capture_H As UInteger = 0 : Dim CaptureBits As UInteger = 0
                            CallOK("GetQHYCCDSingleFrame", QHY.QHYCamera.GetQHYCCDSingleFrame(CamHandle, Capture_W, Capture_H, CaptureBits, ChannelToRead, CamRawBufferPtr))
                            Dim BytesToTransfer_calculated As Long = Capture_W * Capture_H * CInt(CaptureBits / 8)
                            Log("Calculation says       : " & BytesToTransfer_calculated.ValRegIndep.PadLeft(12) & " byte to transfer.")
                            Log("Loaded image with " & Capture_W.ValRegIndep & "x" & Capture_H.ValRegIndep & " pixel @ " & CaptureBits & " bit resolution")
                            Stopper.Stamp("GetQHYCCDSingleFrame")

                            'Remove overscan
                            Dim SingleStatCalc As New AstroNET.Statistics(DB.IPP)
                            If DB.RemoveOverscan = False Then
                                SingleStatCalc.DataProcessor_UInt16.ImageData = ChangeAspectIPP(DB.IPP, CamRawBuffer, CInt(Capture_W), CInt(Capture_H))      'only convert flat byte buffer to UInt16 matrix data
                            Else
                                Dim Overscan_X As UInteger = EffArea_X \ DB.Binning
                                Dim Overscan_Y As UInteger = EffArea_Y \ DB.Binning
                                Dim TempBuffer(,) As UInt16 = ChangeAspectIPP(DB.IPP, CamRawBuffer, CInt(Capture_W), CInt(Capture_H))                        'convert flat to UInt16 matrix in a temporary buffer
                                DB.IPP.Copy(TempBuffer, SingleStatCalc.DataProcessor_UInt16.ImageData, 0, CInt(Overscan_X - 1), CInt(Capture_H - Overscan_Y), CInt(Capture_W - Overscan_X))
                            End If
                            Stopper.Stamp("ChangeAspect")

                            'Run statistics
                            Dim SingleStat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics
                            LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat, LoopStat)
                            Log("Capture #" & LoopCnt.ValRegIndep & " statistics:")
                            Log(SingleStat.StatisticsReport)
                            Stopper.Stamp("Statistics")

                            'Plot histogram
                            If IsNothing(DB.Plotter) = True Then DB.Plotter = New cZEDGraphService(zgcMain)
                            DB.Plotter.Clear()
                            DB.Plotter.PlotXvsY("R", LoopStat.BayerHistograms(0, 0), New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, 1))
                            DB.Plotter.PlotXvsY("G1", LoopStat.BayerHistograms(0, 1), New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, 1))
                            DB.Plotter.PlotXvsY("G2", LoopStat.BayerHistograms(1, 0), New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, 1))
                            DB.Plotter.PlotXvsY("B", LoopStat.BayerHistograms(1, 1), New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, 1))
                            DB.Plotter.PlotXvsY("Mono histo", LoopStat.MonochromHistogram, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, 1))
                            DB.Plotter.ManuallyScaleXAxis(LoopStat.MonoStatistics.Min, LoopStat.MonoStatistics.Max)

                            DB.Plotter.AutoScaleYAxisLog()
                            DB.Plotter.GridOnOff(True, True)
                            DB.Plotter.ForceUpdate()
                            Stopper.Stamp("Plotter")

                            'Store image
                            If DB.StoreImage = True Then
                                Dim Path As String = System.IO.Path.Combine(DB.MyPath, DB.GUID)
                                If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)
                                Dim FITSName As String = System.IO.Path.Combine(Path, "QHY_EXP_" & Format(LoopCnt, "0000000")).Trim & ".fits"
                                Dim CustomElement As New Collections.Generic.List(Of String())
                                CustomElement.Add(New String() {eFITSKeywords.EXPTIME, cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE) / 1000000)})
                                CustomElement.Add(New String() {eFITSKeywords.GAIN, cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN))})
                                cFITSWriter.Write(FITSName, SingleStatCalc.DataProcessor_UInt16.ImageData, cFITSWriter.eBitPix.Int16, CustomElement)
                                If DB.AutoOpenImage = True Then System.Diagnostics.Process.Start(FITSName)
                                Stopper.Stamp("Store")
                            End If

                        Next LoopCnt

                        'Display timing log
                        Log("--------------------------------------------------------------")
                        Log("TIMING:")
                        Log(Stopper.GetLog)
                        Log("--------------------------------------------------------------")

                        'Release buffer handles
                        PinHandler = Nothing

                    End If

                End If

                QHY.QHYCamera.CloseQHYCCD(CamHandle)
                QHY.QHYCamera.ReleaseQHYCCDResource()

            Else
                Log("Init OK but no camera found!")
            End If
        Else
            Log("Init QHY did fail!")
        End If

    End Sub

    '===============================================================================================
    ' Image and signal processing
    '===============================================================================================

    'Re-orient the image data in the buffer ("first in X direction" becomes "first in Y direction")
    Private Function ChangeAspectIPP(ByRef IntelIPP As cIntelIPP, ByRef RawBuffer() As Byte, ByVal TargetWidth As Integer, ByVal TargetHeight As Integer) As UInt16(,)
        Dim RetVal(TargetWidth - 1, TargetHeight - 1) As UInt16
        IntelIPP.Transpose(RawBuffer, RetVal)
        Return RetVal
    End Function

    '===============================================================================================
    ' Utility functions
    '===============================================================================================

    Private Function BuildSDKVersion(ByRef Digits() As UInteger) As String
        Dim RetVal As New Collections.Generic.List(Of String)
        For Each Entry As UInteger In Digits
            RetVal.Add(Entry.ValRegIndep)
        Next Entry
        Return Join(RetVal.ToArray, ".")
    End Function

    '===============================================================================================
    ' Logging and error handling
    '===============================================================================================

    Private Function CallOK(ByVal ErrorCode As UInteger) As Boolean
        Return CallOK(String.Empty, ErrorCode)
    End Function

    Private Function CallOK(ByVal Action As String, ByVal ErrorCode As UInteger) As Boolean
        If ErrorCode = 0 Then
            Return True
        Else
            Log("########## QHY ERROR on <" & Action & ">: <" & ErrorCode.ValRegIndep & "> #####")
            Return False
        End If
    End Function

    Private Sub LogTime(ByVal Text As String, ByVal DurationMS As Long)
        Log(Text & " took " & DurationMS.ValRegIndep & " ms")
    End Sub

    Private Sub LogTiming(ByVal Text As String, ByVal Ticker As System.Diagnostics.Stopwatch)
        Log(Text & ": " & Ticker.ElapsedMilliseconds.ValRegIndep & " ms", False)
    End Sub

    Private Sub Log(ByVal Text As String)
        Log(Text, False)
    End Sub

    Private Sub Log(ByVal Text As Collections.Generic.List(Of String))
        For Each Line As String In Text
            Log(Line, False)
        Next Line
    End Sub

    Private Sub Log(ByVal Text() As String)
        For Each Line As String In Text
            Log(Line, False)
        Next Line
    End Sub

    Private Sub LogStart(ByVal Text As String)
        Log(Text, False)
    End Sub

    Private Sub Log(ByVal Text As String, ByVal LogInStatus As Boolean)
        Text = Format(Now, "HH.mm.ss:fff") & "|" & Text
        With tbLogOutput
            If .Text.Length = 0 Then
                .Text = Text
            Else
                .Text &= System.Environment.NewLine & Text
            End If
            .SelectionStart = .Text.Length - 1
            .SelectionLength = 0
            .ScrollToCaret()
            If LogInStatus = True Then tsslMain.Text = Text
        End With
        DE()
    End Sub

    Private Sub EndAction()
        Log("=========================================", False)
        tsslMain.Text = "--IDLE--"
        DE()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub ExplorerHereToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExplorerHereToolStripMenuItem.Click
        Diagnostics.Process.Start(DB.MyPath)
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Load IPP
        Try
            For Each IPPRoot As String In DB.IPPRoots
                If System.IO.Directory.Exists(IPPRoot) = True Then
                    Try
                        DB.IPP = New cIntelIPP(IPPRoot)
                        If DB.IPP.DLLHandleValid = True Then Exit For
                    Catch ex As Exception
                        MsgBox("IPP <" & IPPRoot & "> not found!")
                    End Try
                End If
            Next IPPRoot
            If DB.IPP.DLLHandleValid = False Then
                MsgBox("IPP not found!")
            End If
        Catch ex As Exception
            MsgBox("Generic error: <" & ex.Message & ">")
        End Try
        cFITSWriter.IPPPath = DB.IPP.IPPPath
        cFITSWriter.UseIPPForWriting = True
        'Other objects
        DB.Plotter = New cZEDGraphService(zgcMain)
        pgMain.SelectedObject = DB
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

End Class