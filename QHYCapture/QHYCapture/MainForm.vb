Option Explicit On
Option Strict On

Public Class MainForm

    '''<summary>DB that holds all relevant information.</summary>
    Private DB As New cDB
    Private WithEvents DB_ServiceContract As cDB_ServiceContract

    Private CamHandle As IntPtr = IntPtr.Zero
    Private UsedReadMode As eReadOutMode = eReadOutMode.Unvalid
    Private UsedCameraId As System.Text.StringBuilder
    Private UsedStreamMode As UInteger = UInteger.MaxValue

    '''<summary>Run a single capture.</summary>
    Private Sub RunCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunCaptureToolStripMenuItem.Click
        DB.StopFlag = False
        QHYCapture(DB.FileName, True)
    End Sub

    '''<summary>Command for a QHY capture run.</summary>
    Public Sub QHYCapture(ByVal FITSFileStart As String, ByVal CloseAtEnd As Boolean)

        DB.RunningFlag = True

        Dim Stopper As New cStopper
        Stopper.Start()

        Dim SDKVersion(3) As UInteger

        Dim Chip_Physical_W As Double = Double.NaN
        Dim Chip_Physical_H As Double = Double.NaN
        Dim Chip_Pixel_W As UInteger = 0
        Dim Chip_Pixel_H As UInteger = 0
        Dim Pixel_Size_W As Double = Double.NaN
        Dim Pixel_Size_H As Double = Double.NaN

        Dim EffArea As sRect_UInt

        Dim OverArea As sRect_UInt

        Dim bpp As UInteger = 0
        Dim CamNumToUse As Integer = 0

        'Single Image Capture Mode Workflow

        'Init if not yet done
        If CamHandle = IntPtr.Zero Or UsedReadMode = eReadOutMode.Unvalid Or UsedStreamMode = UInteger.MaxValue Then

            If CallOK(QHY.QHYCamera.InitQHYCCDResource) = True Then                                                                         'Init DLL itself
                Stopper.Stamp("InitQHYCCDResource")
                Dim CameraCount As UInteger = QHY.QHYCamera.ScanQHYCCD                                                                      'Scan for connected cameras
                Stopper.Stamp("ScanQHYCCD")
                If CameraCount > 0 Then                                                                                                     'If there is a camera found
                    UsedCameraId = New System.Text.StringBuilder(0)                                                                         'Prepare camera ID holder
                    CallOK(QHY.QHYCamera.GetQHYCCDId(CamNumToUse, UsedCameraId))                                                            'Fetch camera ID
                    Log("Found QHY camera <" & UsedCameraId.ToString & ">")                                                                 'Display fetched camera ID
                    CamHandle = QHY.QHYCamera.OpenQHYCCD(UsedCameraId)                                                                      'Open the camera
                    If CamHandle <> IntPtr.Zero Then

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
                        If DB.Log_CamProp Then
                            Log("Available read-out modes:")
                            Log(AllReadOutModes)
                        End If

                        'Run the start-up init sequence
                        If CallOK(QHY.QHYCamera.SetQHYCCDReadMode(CamHandle, DB.ReadOutMode)) = True Then
                            If CallOK(QHY.QHYCamera.SetQHYCCDStreamMode(CamHandle, DB.StreamMode)) = True Then                                      'Set single capture mode
                                If CallOK(QHY.QHYCamera.InitQHYCCD(CamHandle)) = True Then                                                          'Init the camera with the selected mode, ...
                                    'Camera is open
                                    UsedReadMode = DB.ReadOutMode
                                    UsedStreamMode = DB.StreamMode
                                Else
                                    Log("InitQHYCCD FAILED!")
                                    CamHandle = IntPtr.Zero
                                End If
                            Else
                                Log("SetQHYCCDStreamMode FAILED!")
                                CamHandle = IntPtr.Zero
                            End If
                        Else
                            Log("SetQHYCCDReadMode to <" & DB.ReadOutMode & "> FAILED!")
                        End If
                    Else
                        Log("OpenQHYCCD FAILED!")
                        CamHandle = IntPtr.Zero
                    End If
                Else
                    Log("Init OK but no camera found!")
                    CamHandle = IntPtr.Zero
                End If
            Else
                Log("Init QHY did fail!")
            End If
        End If

        If CamHandle <> IntPtr.Zero Then

            'Get chip properties
            QHY.QHYCamera.GetQHYCCDChipInfo(CamHandle, Chip_Physical_W, Chip_Physical_H, Chip_Pixel_W, Chip_Pixel_H, Pixel_Size_W, Pixel_Size_H, bpp)                   'Get chip info
            QHY.QHYCamera.GetQHYCCDEffectiveArea(CamHandle, EffArea.X, EffArea.Y, EffArea.W, EffArea.H)
            QHY.QHYCamera.GetQHYCCDOverScanArea(CamHandle, OverArea.X, OverArea.Y, OverArea.W, OverArea.H)

            'Log chip properties
            If DB.Log_CamProp = True Then

                Stopper.Start()

                QHY.QHYCamera.GetQHYCCDSDKVersion(SDKVersion(0), SDKVersion(1), SDKVersion(2), SDKVersion(3))                   'Get the SDK version
                Log("SDK version: " & BuildSDKVersion(SDKVersion))                                                              'Display the SDK version

                Log("Chip info (bpp: " & bpp.ValRegIndep & ")")
                Log("  Chip  W x H    :" & Chip_Physical_W.ValRegIndep & " x " & Chip_Physical_H.ValRegIndep & " mm")
                Log("  Image W x H    :" & Chip_Pixel_W.ValRegIndep & " x " & Chip_Pixel_H.ValRegIndep & " pixel")
                Log("  Pixel W x H    :" & Pixel_Size_W.ValRegIndep & " x " & Pixel_Size_H.ValRegIndep & " um")
                Log("CCD Effective Area:")
                Log("  Start X:Y    :" & EffArea.X.ValRegIndep & ":" & EffArea.Y.ValRegIndep)
                Log("  Size  X x Y  :" & EffArea.W.ValRegIndep & " x " & EffArea.H.ValRegIndep)
                Log("CCD Overscan Area:")
                Log("  Start X:Y    :" & OverArea.X.ValRegIndep & ":" & OverArea.Y.ValRegIndep)
                Log("  Size  X x Y  :" & OverArea.W.ValRegIndep & " x " & OverArea.H.ValRegIndep)
                Log("==============================================================================")

                Log("ControlValues:")                                                                                           'Start reading all control values

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
            End If

            'Prepare buffers
            Dim ReadResolution As UInteger = 16
            Dim ChannelToRead As UInteger = 0

            'Calculate ROI parameters passed to camera
            Dim ROI As New System.Drawing.Rectangle
            With ROI
                .X = CInt(DB.ROI_X)
                .Y = CInt(DB.ROI_Y)
                .Width = CInt(DB.ROI_Width)
                If (DB.ROI_X + DB.ROI_Width > Chip_Pixel_W) Or (.Width = 0) Then .Width = CInt(Chip_Pixel_W - DB.ROI_X)
                .Height = CInt(DB.ROI_Height)
                If (DB.ROI_Y + DB.ROI_Height > Chip_Pixel_H) Or (.Height = 0) Then .Height = CInt(Chip_Pixel_H - DB.ROI_Y)
            End With

            Stopper.Stamp("Prepare buffers")

            'Set exposure parameters
            CallOK("SetQHYCCDBinMode", QHY.QHYCamera.SetQHYCCDBinMode(CamHandle, DB.Binning, DB.Binning))
            CallOK("SetQHYCCDResolution", QHY.QHYCamera.SetQHYCCDResolution(CamHandle, CUInt(ROI.X), CUInt(ROI.Y), CUInt(ROI.Width \ DB.Binning), CUInt(ROI.Height \ DB.Binning)))
            CallOK("CONTROL_TRANSFERBIT", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_TRANSFERBIT, ReadResolution))
            CallOK("CONTROL_GAIN", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN, DB.Gain))
            CallOK("CONTROL_OFFSET", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET, DB.Offset))
            CallOK("CONTROL_EXPOSURE", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE, DB.ExposureTime * 1000000))

            Dim LoopStat As New AstroNET.Statistics.sStatistics
            Dim PinHandler As cIntelIPP.cPinHandler
            Dim CamRawBuffer As Byte() = {}
            Dim CamRawBufferPtr As IntPtr = Nothing

            For LoopCnt As Integer = 1 To DB.CaptureCount

                'Cancel any running exposure
                CallOK("CancelQHYCCDExposing", QHY.QHYCamera.CancelQHYCCDExposing(CamHandle))
                CallOK("CancelQHYCCDExposingAndReadout", QHY.QHYCamera.CancelQHYCCDExposingAndReadout(CamHandle))

                'Check and set temperature
                If DB.TargetTemp > -100 Then
                    Do
                        Dim CurrentTemp As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
                        Dim CurrentPWM As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURPWM)
                        tsslMain.Text = "Temp is current" & CurrentTemp.ValRegIndep & ", Target: " & DB.TargetTemp.ValRegIndep & ", cooler @ " & CurrentPWM.ValRegIndep & " %"
                        If System.Math.Abs(CurrentTemp - DB.TargetTemp) < 0.1 Then Exit Do
                        System.Threading.Thread.Sleep(500)
                        DE()
                    Loop Until 1 = 0
                End If

                'Show status
                tsslMain.Text = "Taking capture " & LoopCnt.ValRegIndep & "/" & DB.CaptureCount.ValRegIndep

                'Start expose (single or live frame mode)
                Dim ObsStart As DateTime = Now
                Dim ObsStartTemp As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
                Stopper.Start()
                If DB.StreamMode = eStreamMode.SingleFrame Then
                    CallOK("ExpQHYCCDSingleFrame", QHY.QHYCamera.ExpQHYCCDSingleFrame(CamHandle))
                    Stopper.Stamp("ExpQHYCCDSingleFrame")
                Else
                    CallOK("BeginQHYCCDLive", QHY.QHYCamera.BeginQHYCCDLive(CamHandle))
                    Stopper.Stamp("BeginQHYCCDLive")
                End If

                'Idle exposure time
                If DB.ExposureTime > 1 Then
                    Dim ExpStart As DateTime = Now
                    tspbProgress.Maximum = CInt(DB.ExposureTime)
                    Do
                        System.Threading.Thread.Sleep(500)
                        Dim TimePassed As Double = (Now - ExpStart).TotalSeconds
                        If TimePassed < tspbProgress.Maximum Then
                            tspbProgress.Value = CInt(TimePassed)
                            tsslProgress.Text = Format(TimePassed, "0.0").Trim & "/" & tspbProgress.Maximum.ToString.Trim & " seconds exposed"
                        Else
                            tspbProgress.Value = 0
                            tsslProgress.Text = "---"
                            Exit Do
                        End If
                        DE()
                    Loop Until 1 = 0
                End If

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
                Dim Captured_W As UInteger = 0 : Dim Captured_H As UInteger = 0 : Dim CaptureBits As UInteger = 0
                If DB.StreamMode = eStreamMode.SingleFrame Then
                    CallOK("GetQHYCCDSingleFrame", QHY.QHYCamera.GetQHYCCDSingleFrame(CamHandle, Captured_W, Captured_H, CaptureBits, ChannelToRead, CamRawBufferPtr))
                Else
                    Dim LiveModeReady As UInteger = 123456
                    Dim LiveModePollCount As Integer = 0
                    Do
                        LiveModeReady = QHY.QHYCamera.GetQHYCCDLiveFrame(CamHandle, Captured_W, Captured_H, CaptureBits, ChannelToRead, CamRawBufferPtr)
                        LiveModePollCount += 1
                    Loop Until (LiveModeReady = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS) Or DB.StopFlag = True
                End If
                Dim BytesToTransfer_calculated As Long = Captured_W * Captured_H * CInt(CaptureBits / 8)
                Log("Calculation says       : " & BytesToTransfer_calculated.ValRegIndep.PadLeft(12) & " byte to transfer.")
                Log("Loaded image with " & Captured_W.ValRegIndep & "x" & Captured_H.ValRegIndep & " pixel @ " & CaptureBits & " bit resolution")
                Stopper.Stamp("GetQHYCCDSingleFrame")

                Dim ObsEnd As DateTime = Now
                'Remove overscan
                Dim SingleStatCalc As New AstroNET.Statistics(DB.IPP)
                If DB.RemoveOverscan = False Then
                    SingleStatCalc.DataProcessor_UInt16.ImageData = ChangeAspectIPP(DB.IPP, CamRawBuffer, CInt(Captured_W), CInt(Captured_H))      'only convert flat byte buffer to UInt16 matrix data
                Else
                    Dim Overscan_X As UInteger = EffArea.X \ DB.Binning
                    Dim Overscan_Y As UInteger = EffArea.Y \ DB.Binning
                    Dim TempBuffer(,) As UInt16 = ChangeAspectIPP(DB.IPP, CamRawBuffer, CInt(Captured_W), CInt(Captured_H))                        'convert flat to UInt16 matrix in a temporary buffer
                    DB.IPP.Copy(TempBuffer, SingleStatCalc.DataProcessor_UInt16.ImageData, 0, CInt(Overscan_X - 1), CInt(Captured_H - Overscan_Y), CInt(Captured_W - Overscan_X))
                End If
                Stopper.Stamp("ChangeAspect")

                'Run statistics
                Dim SingleStat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics
                LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat, LoopStat)

                'Display statistics
                If DB.Log_ClearStat = True Then DB.Log_Statistics.Clear()
                LogStatistics("Capture #" & LoopCnt.ValRegIndep & " statistics:")
                For Each Line As String In SingleStat.StatisticsReport
                    LogStatistics(Line)
                Next Line
                'on loop mode and statistics clear, display total statistics
                If DB.CaptureCount > 1 And DB.Log_ClearStat = True Then
                    LogStatistics("Total statistics:")
                    For Each Line As String In LoopStat.StatisticsReport
                        LogStatistics(Line)
                    Next Line
                End If

                Stopper.Stamp("Statistics")

                'Plot histogram
                Dim PlotCurrentStatistics As Boolean = True
                Dim PlotMeanStatistics As Boolean = True
                Dim NormFactor As Double = LoopCnt
                Dim CurveMode As cZEDGraphService.eCurveMode = cZEDGraphService.eCurveMode.LinesAndPoints
                Dim CurrentCurveWidth As Integer = 1
                Dim MeanCurveWidth As Integer = 2
                If IsNothing(DB.Plotter) = True Then DB.Plotter = New cZEDGraphService(zgcMain)
                DB.Plotter.Clear()
                'Plot mean statistics
                If DB.CaptureCount > 1 And PlotMeanStatistics = True Then
                    DB.Plotter.PlotXvsY("R mean", LoopStat.BayerHistograms(0, 0), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("G1 mean", LoopStat.BayerHistograms(0, 1), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("G2 mean", LoopStat.BayerHistograms(1, 0), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("B mean", LoopStat.BayerHistograms(1, 1), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("Mono mean", LoopStat.MonochromHistogram, NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, CurveMode, MeanCurveWidth))
                End If
                'Plot current statistics
                If PlotCurrentStatistics = True Then
                    DB.Plotter.PlotXvsY("R", SingleStat.BayerHistograms(0, 0), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("G1", SingleStat.BayerHistograms(0, 1), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("G2", SingleStat.BayerHistograms(1, 0), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("B", SingleStat.BayerHistograms(1, 1), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("Mono", SingleStat.MonochromHistogram, 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, CurveMode, CurrentCurveWidth))
                End If
                DB.Plotter.ManuallyScaleXAxis(LoopStat.MonoStatistics.Min.Key, LoopStat.MonoStatistics.Max.Key)

                DB.Plotter.AutoScaleYAxisLog()
                DB.Plotter.GridOnOff(True, True)
                DB.Plotter.ForceUpdate()
                Stopper.Stamp("Plotter")

                'Store image
                If DB.StoreImage = True Then

                    Dim Path As String = System.IO.Path.Combine(DB.MyPath, DB.GUID)
                    If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)
                    Dim FileCounter As String = String.Empty : If DB.CaptureCount > 1 Then FileCounter = "_#" & Format(LoopCnt, "000")
                    Dim FITSName As String = System.IO.Path.Combine(Path, FITSFileStart & FileCounter).Trim & "." & DB.FITSExtension

                    'Precalculation
                    Dim NAXIS1 As Integer = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(0) + 1
                    Dim NAXIS2 As Integer = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(1) + 1
                    Dim PLATESZ1 As Double = (Pixel_Size_W * NAXIS1) / 1000                         '[mm]
                    Dim PLATESZ2 As Double = (Pixel_Size_H * NAXIS2) / 1000                         '[mm]
                    Dim FOV1 As Double = 2 * Math.Atan(PLATESZ1 / (2 * DB.TelescopeFocalLength)) * (180 / Math.PI)
                    Dim FOV2 As Double = 2 * Math.Atan(PLATESZ2 / (2 * DB.TelescopeFocalLength)) * (180 / Math.PI)
                    Dim CamReadOutMode As New Text.StringBuilder : QHY.QHYCamera.GetQHYCCDReadModeName(CamHandle, DB.ReadOutMode, CamReadOutMode)

                    'Compose all FITS keyword entries
                    Dim CustomElement As New Collections.Generic.List(Of String())

                    CustomElement.Add(New String() {eFITSKeywords.OBS_ID, cFITSKeywords.GetString(DB.GUID)})
                    CustomElement.Add(New String() {eFITSKeywords.PROGRAM, cFITSKeywords.GetString(Me.Text)})

                    AddNoEmptyElement(CustomElement, eFITSKeywords.OBJECT, cFITSKeywords.GetString(DB.ObjectName))
                    AddNoEmptyElement(CustomElement, eFITSKeywords.RA, cFITSKeywords.GetString(DB.ObjectRA))
                    AddNoEmptyElement(CustomElement, eFITSKeywords.DEC, cFITSKeywords.GetString(DB.ObjectDEC))

                    CustomElement.Add(New String() {eFITSKeywords.AUTHOR, cFITSKeywords.GetString(DB.Author)})
                    CustomElement.Add(New String() {eFITSKeywords.ORIGIN, cFITSKeywords.GetString(DB.Origin)})
                    CustomElement.Add(New String() {eFITSKeywords.TELESCOP, cFITSKeywords.GetString(DB.Telescope)})
                    CustomElement.Add(New String() {eFITSKeywords.TELAPER, cFITSKeywords.GetDouble(DB.TelescopeAperture / 1000.0)})
                    CustomElement.Add(New String() {eFITSKeywords.TELFOC, cFITSKeywords.GetDouble(DB.TelescopeFocalLength / 1000.0)})
                    CustomElement.Add(New String() {eFITSKeywords.INSTRUME, cFITSKeywords.GetString(UsedCameraId.ToString)})
                    CustomElement.Add(New String() {eFITSKeywords.PIXSIZE1, cFITSKeywords.GetDouble(Pixel_Size_W)})
                    CustomElement.Add(New String() {eFITSKeywords.PIXSIZE2, cFITSKeywords.GetDouble(Pixel_Size_H)})
                    CustomElement.Add(New String() {eFITSKeywords.PLATESZ1, cFITSKeywords.GetDouble(PLATESZ1 / 10)})                'calculated from the image data as ROI may be set ...
                    CustomElement.Add(New String() {eFITSKeywords.PLATESZ2, cFITSKeywords.GetDouble(PLATESZ2 / 10)})                'calculated from the image data as ROI may be set ...
                    CustomElement.Add(New String() {eFITSKeywords.FOV1, cFITSKeywords.GetDouble(FOV1)})
                    CustomElement.Add(New String() {eFITSKeywords.FOV2, cFITSKeywords.GetDouble(FOV2)})
                    CustomElement.Add(New String() {eFITSKeywords.COLORTYP, "0"})                                                   '<- check

                    CustomElement.Add(New String() {eFITSKeywords.DATE_OBS, cFITSKeywords.GetDateWithTime(ObsStart)})
                    CustomElement.Add(New String() {eFITSKeywords.DATE_END, cFITSKeywords.GetDateWithTime(ObsEnd)})
                    CustomElement.Add(New String() {eFITSKeywords.TIME_OBS, cFITSKeywords.GetTime(ObsStart)})
                    CustomElement.Add(New String() {eFITSKeywords.TIME_END, cFITSKeywords.GetTime(ObsEnd)})

                    CustomElement.Add(New String() {eFITSKeywords.CRPIX1, cFITSKeywords.GetDouble(0.5 * (NAXIS1 + 1))})
                    CustomElement.Add(New String() {eFITSKeywords.CRPIX2, cFITSKeywords.GetDouble(0.5 * (NAXIS2 + 1))})

                    CustomElement.Add(New String() {eFITSKeywords.IMAGETYP, cFITSKeywords.GetString(DB.ExposureType)})
                    CustomElement.Add(New String() {eFITSKeywords.EXPTIME, cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE) / 1000000)})
                    CustomElement.Add(New String() {eFITSKeywords.GAIN, cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN))})
                    CustomElement.Add(New String() {eFITSKeywords.OFFSET, cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET))})
                    CustomElement.Add(New String() {eFITSKeywords.BRIGHTNESS, cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_BRIGHTNESS))})
                    CustomElement.Add(New String() {eFITSKeywords.SETTEMP, cFITSKeywords.GetDouble(DB.TargetTemp)})
                    CustomElement.Add(New String() {eFITSKeywords.CCDTEMP, cFITSKeywords.GetDouble(ObsStartTemp)})

                    CustomElement.Add(New String() {eFITSKeywords.QHY_MODE, cFITSKeywords.GetString(CamReadOutMode.ToString)})

                    'Create FITS file
                    cFITSWriter.Write(FITSName, SingleStatCalc.DataProcessor_UInt16.ImageData, cFITSWriter.eBitPix.Int16, CustomElement)
                    If DB.AutoOpenImage = True Then System.Diagnostics.Process.Start(FITSName)
                    Stopper.Stamp("Store")
                End If

                If DB.StopFlag = True Then Exit For

            Next LoopCnt

            'Stop live mode if used
            If DB.StreamMode = eStreamMode.LiveFrame Then
                CallOK("StopQHYCCDLive", QHY.QHYCamera.StopQHYCCDLive(CamHandle))
            End If

            'Display timing log
            If DB.Log_Timing = True Then
                Log("--------------------------------------------------------------")
                Log("TIMING:")
                Log(Stopper.GetLog)
                Log("--------------------------------------------------------------")
            End If

            'Release buffer handles
            PinHandler = Nothing

        End If

        'Close camera if selected
        If CloseAtEnd = True Then
            CloseCamera()
        End If

        tsslMain.Text = "--IDLE--"
        DB.RunningFlag = False

    End Sub

    Private Sub CloseCamera()
        If CamHandle <> IntPtr.Zero Then
            Log("Closing camera ...")
            QHY.QHYCamera.CloseQHYCCD(CamHandle)
            QHY.QHYCamera.ReleaseQHYCCDResource()
            CamHandle = IntPtr.Zero
        End If
    End Sub

    Private Sub AddNoEmptyElement(ByRef Elements As Collections.Generic.List(Of String()), ByVal Key As String, ByVal Value As String)
        If String.IsNullOrEmpty(Value) = False Then Elements.Add(New String() {Key, Value})
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
            Log("########## QHY ERROR on <" & Action & ">: <0x" & Hex(ErrorCode) & "> #####")
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

    Private Sub LogStatistics(ByVal Text As String)
        Text = Format(Now, "HH.mm.ss:fff") & "|" & Text
        If DB.Log_Statistics.Length = 0 Then
            DB.Log_Statistics.Append(Text)
        Else
            DB.Log_Statistics.Append(System.Environment.NewLine & Text)
        End If
        With tbStatistics
            .Text = DB.Log_Statistics.ToString
            .SelectionStart = .Text.Length - 1
            .SelectionLength = 0
            .ScrollToCaret()
        End With
        DE()
    End Sub

    Private Sub Log(ByVal Text As String, ByVal LogInStatus As Boolean)
        Text = Format(Now, "HH.mm.ss:fff") & "|" & Text
        If DB.Log_Generic.Length = 0 Then
            DB.Log_Generic.Append(Text)
        Else
            DB.Log_Generic.Append(System.Environment.NewLine & Text)
        End If
        With tbLogOutput
            .Text = DB.Log_Generic.ToString
            .SelectionStart = .Text.Length - 1
            .SelectionLength = 0
            .ScrollToCaret()
            If LogInStatus = True Then tsslMain.Text = Text
        End With
        DE()
    End Sub

    Private Function TimeStamp() As String
        Return Format(Now, "HH.mm.ss:fff")
    End Function

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

        'Load INI
        If System.IO.File.Exists(DB.MyINI) Then DB.INI.Load(DB.MyINI)

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
            MsgBox("Generic error on loading IPP: <" & ex.Message & ">")
        End Try
        cFITSWriter.IPPPath = DB.IPP.IPPPath
        cFITSWriter.UseIPPForWriting = True

        'Start WCF
        'netsh http add urlacl url=http://+:1250/ user=DESKTOP-I7\albusmw
        DB_ServiceContract = New cDB_ServiceContract(DB)
        Dim WebServicePort As String = DB.INI.Get("Connections", "WebInterfacePort", "0")
        If WebServicePort <> "0" Then
            Dim WebServiceAdr As String = "http://localhost:" & WebServicePort & "/"
            DB.SetupWCF = New ServiceModel.Web.WebServiceHost(GetType(cDB_ServiceContract), New Uri(WebServiceAdr))
            DB.serviceBehavior = DB.SetupWCF.Description.Behaviors.Find(Of ServiceModel.Description.ServiceDebugBehavior)
            DB.serviceBehavior.HttpHelpPageEnabled = True
            DB.serviceBehavior.IncludeExceptionDetailInFaults = True
            DB.SetupWCF.Open()
        End If

        'Other objects
        DB.Plotter = New cZEDGraphService(zgcMain)
        pgMain.SelectedObject = DB

        'Set toolstrip icons
        tsbCapture.Image = ilMain.Images.Item("Capture.png")
        tsbStopCapture.Image = ilMain.Images.Item("StopCapture.png")

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub tSetTemp_Tick(sender As Object, e As EventArgs) Handles tSetTemp.Tick
        If CamHandle <> IntPtr.Zero Then
            QHY.QHYCamera.ControlQHYCCDTemp(CamHandle, DB.TargetTemp)
        End If
    End Sub

    Private Sub tsbCapture_Click(sender As Object, e As EventArgs) Handles tsbCapture.Click
        If CType(sender, System.Windows.Forms.ToolStripButton).Enabled = True Then QHYCapture(DB.FileName, True)
    End Sub

    Private Sub tsbStopCapture_Click(sender As Object, e As EventArgs) Handles tsbStopCapture.Click
        DB.StopFlag = True
    End Sub

    Private Sub BiasCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BiasCaptureToolStripMenuItem.Click
        DB.ExposureTime = 0.000001
        DB.Gain = 200
        DB.Offset = 255
        DB.TargetTemp = -300
        pgMain.SelectedObject = DB : DE()
    End Sub

    Private Sub AllReadoutModesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllReadoutModesToolStripMenuItem.Click
        DB.StopFlag = False
        For Each Mode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            DB.ReadOutMode = Mode
            pgMain.SelectedObject = DB : DE()
            QHYCapture(DB.FileName & Mode.ToString.Trim, False)
            If DB.StopFlag = True Then Exit For
        Next Mode
        CloseCamera()
    End Sub

    Private Sub ExposureTimeSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExposureTimeSeriesToolStripMenuItem.Click
        DB.StopFlag = False
        'For lust of all exposure times
        Dim AllExposureTimes As New Collections.Generic.List(Of Double)
        For Exp As Integer = 0 To 2
            For Base As Double = 1 To 9
                AllExposureTimes.Add(Base * (10 ^ Exp))
            Next Base
        Next Exp
        AllExposureTimes.Sort()
        'Run series
        For Each Exposure As Double In AllExposureTimes
            DB.ExposureTime = Exposure
            pgMain.SelectedObject = DB : DE()
            QHYCapture("QHY_EXP_" & Exposure.ToString.Trim, False)
            If DB.StopFlag = True Then Exit For
        Next Exposure
        CloseCamera()
    End Sub

    Private Sub GainVariationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GainVariationToolStripMenuItem.Click
        DB.StopFlag = False
        For Gain As Double = 0 To 200 Step 5
            DB.Gain = Gain
            pgMain.SelectedObject = DB : DE()
            QHYCapture("QHY_GAIN_" & Gain.ValRegIndep("000"), False)
            If DB.StopFlag = True Then Exit For
        Next Gain
        CloseCamera()
    End Sub

    Private Sub NoRealObjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NoRealObjectToolStripMenuItem.Click
        DB.ObjectName = String.Empty
        DB.ObjectRA = String.Empty
        DB.ObjectDEC = String.Empty
        DB.Telescope = String.Empty
        pgMain.SelectedObject = DB : DE()
    End Sub

    Private Sub TestWebInterfaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestWebInterfaceToolStripMenuItem.Click
        'Test call for the web interface
        System.Diagnostics.Process.Start("http://localhost:1250/GetParameterList")
    End Sub

    Private Sub DB_ServiceContract_ValueChanged() Handles DB_ServiceContract.ValueChanged
        pgMain.SelectedObject = DB : DE()
    End Sub

    Private Sub DB_ServiceContract_StartExposure() Handles DB_ServiceContract.StartExposure
        tsbCapture_Click(tsbCapture, Nothing)
    End Sub

    Private Sub FastLiveModeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FastLiveModeToolStripMenuItem.Click
        With DB
            .StreamMode = eStreamMode.LiveFrame
            .ExposureTime = 0.000001
            .ROI_Width = 1000
            .ROI_Height = 1000
            .CaptureCount = 10000
            .StoreImage = False
            .Log_ClearStat = True
        End With
        pgMain.SelectedObject = DB : DE()
    End Sub

    Private Sub FocusWindowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FocusWindowToolStripMenuItem.Click
        Dim FocusWindow As New cImgForm
        FocusWindow.Show()
        Dim OutputImage As New cLockBitmap(20, 20)
        OutputImage.LockBits()
        Dim Stride As Integer = OutputImage.BitmapData.Stride
        Dim BytePerPixel As Integer = OutputImage.ColorBytesPerPixel
        Dim YOffset As Integer = 0
        For Y As Integer = 0 To OutputImage.Height - 1
            Dim BaseOffset As Integer = YOffset
            For X As Integer = 0 To OutputImage.Width - 1
                Dim Coloring As Drawing.Color = Drawing.Color.FromArgb(X * 6, Y * 6, X + Y)
                OutputImage.Pixels(BaseOffset) = Coloring.R
                OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                BaseOffset += BytePerPixel
            Next X
            YOffset += Stride
        Next Y
        OutputImage.UnlockBits()
        FocusWindow.Image.Image = OutputImage.BitmapToProcess
    End Sub

End Class