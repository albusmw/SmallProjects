Option Explicit On
Option Strict On

Public Class MainForm

    '''<summary>DB that holds all relevant information.</summary>
    Private DB As New cDB
    Private DB_meta As New cDB_meta
    Private WithEvents DB_ServiceContract As cDB_ServiceContract

    Private CamHandle As IntPtr = IntPtr.Zero
    Private UsedReadMode As eReadOutMode = eReadOutMode.Unvalid
    Private UsedCameraId As System.Text.StringBuilder
    Private UsedStreamMode As UInteger = UInteger.MaxValue

    Private RTFGen As New Atomic.cRTFGenerator
    Private FocusWindow As cImgForm = Nothing

    '''<summary>Run a single capture.</summary>
    Private Sub RunCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunCaptureToolStripMenuItem.Click
        DB.StopFlag = False
        QHYCapture(DB.FileName, True)
    End Sub

    '''<summary>Command for a QHY capture run.</summary>
    Public Sub QHYCapture(ByVal FITSFileStart As String, ByVal CloseAtEnd As Boolean)

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

        'Start
        Dim Stopper As New cStopper
        Stopper.Start()
        DB.RunningFlag = True

        'Try to get a suitable camera and continue if found
        If InitQHY(DB.CamToUse, Stopper) = False Then Log("No suitable camera found!")
        If CamHandle <> IntPtr.Zero Then

            'Get chip properties
            QHY.QHYCamera.GetQHYCCDChipInfo(CamHandle, Chip_Physical_W, Chip_Physical_H, Chip_Pixel_W, Chip_Pixel_H, Pixel_Size_W, Pixel_Size_H, bpp)
            QHY.QHYCamera.GetQHYCCDEffectiveArea(CamHandle, EffArea.X, EffArea.Y, EffArea.Width, EffArea.Height)
            QHY.QHYCamera.GetQHYCCDOverScanArea(CamHandle, OverArea.X, OverArea.Y, OverArea.Width, OverArea.Height)
            Stopper.Stamp("Get chip properties")

            'Log chip properties
            If DB.Log_CamProp = True Then
                QHY.QHYCamera.GetQHYCCDSDKVersion(SDKVersion(0), SDKVersion(1), SDKVersion(2), SDKVersion(3))                   'Get the SDK version
                Log("SDK version: " & BuildSDKVersion(SDKVersion))                                                              'Display the SDK version

                Log("Chip info (bpp: " & bpp.ValRegIndep & ")")
                Log("  Chip  W x H    :" & Chip_Physical_W.ValRegIndep & " x " & Chip_Physical_H.ValRegIndep & " mm")
                Log("  Image W x H    :" & Chip_Pixel_W.ValRegIndep & " x " & Chip_Pixel_H.ValRegIndep & " pixel")
                Log("  Pixel W x H    :" & Pixel_Size_W.ValRegIndep & " x " & Pixel_Size_H.ValRegIndep & " um")
                Log("CCD Effective Area:")
                Log("  Start X:Y    :" & EffArea.X.ValRegIndep & ":" & EffArea.Y.ValRegIndep)
                Log("  Size  X x Y  :" & EffArea.Width.ValRegIndep & " x " & EffArea.Height.ValRegIndep)
                Log("CCD Overscan Area:")
                Log("  Start X:Y    :" & OverArea.X.ValRegIndep & ":" & OverArea.Y.ValRegIndep)
                Log("  Size  X x Y  :" & OverArea.Width.ValRegIndep & " x " & OverArea.Height.ValRegIndep)
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

                Stopper.Stamp("GetQHYCCDParams")

                Log("==============================================================================")
            End If


            Dim ReadResolution As UInteger = 16
            Dim ChannelToRead As UInteger = 0

            'Calculate ROI parameters that will be passed to camera
            Dim ROI As New System.Drawing.Rectangle
            With ROI
                .X = DB.ROI.X
                .Y = DB.ROI.Y
                .Width = DB.ROI.Width
                If (DB.ROI.X + DB.ROI.Width > Chip_Pixel_W) Or (.Width = 0) Then .Width = CInt(Chip_Pixel_W - DB.ROI.X)
                .Height = DB.ROI.Height
                If (DB.ROI.Y + DB.ROI.Height > Chip_Pixel_H) Or (.Height = 0) Then .Height = CInt(Chip_Pixel_H - DB.ROI.Y)
            End With

            'Set exposure parameters
            CallOK("SetQHYCCDBinMode", QHY.QHYCamera.SetQHYCCDBinMode(CamHandle, DB.Binning, DB.Binning))
            CallOK("SetQHYCCDResolution", QHY.QHYCamera.SetQHYCCDResolution(CamHandle, CUInt(ROI.X), CUInt(ROI.Y), CUInt(ROI.Width \ DB.Binning), CUInt(ROI.Height \ DB.Binning)))
            CallOK("CONTROL_TRANSFERBIT", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_TRANSFERBIT, ReadResolution))
            CallOK("CONTROL_GAIN", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN, DB.Gain))
            CallOK("CONTROL_OFFSET", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET, DB.Offset))
            CallOK("CONTROL_EXPOSURE", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE, DB.ExposureTime * 1000000))
            Stopper.Stamp("Set exposure parameters")

            'Prepare buffers
            Dim LoopStat As New AstroNET.Statistics.sStatistics
            Dim PinHandler As cIntelIPP.cPinHandler
            Dim CamRawBuffer As Byte() = {}
            Dim CamRawBufferPtr As IntPtr = Nothing
            Stopper.Stamp("Prepare buffers")

            'Filter wheel
            If DB.FilterSlot <> eFilter.Invalid Then
                ActiveFilter(CamHandle, DB.FilterSlot)
            End If
            Stopper.Stamp("Select filter")

            For LoopCnt As Integer = 1 To DB.CaptureCount

                'Cancel any running exposure
                CallOK("CancelQHYCCDExposing", QHY.QHYCamera.CancelQHYCCDExposing(CamHandle))
                CallOK("CancelQHYCCDExposingAndReadout", QHY.QHYCamera.CancelQHYCCDExposingAndReadout(CamHandle))
                Stopper.Stamp("Cancel exposure")

                '================================================================================
                'Check and set temperature
                If DB.TargetTemp > -100 Then
                    Do
                        Dim CurrentTemp As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
                        Dim CurrentPWM As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURPWM)
                        tsslMain.Text = "Temp is current" & CurrentTemp.ValRegIndep & ", Target: " & DB.TargetTemp.ValRegIndep & ", cooler @ " & CurrentPWM.ValRegIndep & " %"
                        If CurrentTemp <= DB.TargetTemp Then Exit Do
                        System.Threading.Thread.Sleep(500)
                        DE()
                    Loop Until 1 = 0
                End If
                Stopper.Stamp("Set temperature")

                '================================================================================
                'Start expose (single or live frame mode)
                tsslMain.Text = "Taking capture " & LoopCnt.ValRegIndep & "/" & DB.CaptureCount.ValRegIndep
                Dim ObsStart As DateTime = Now
                Dim ObsEnd As DateTime = DateTime.MinValue
                Dim ObsStartTemp As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
                Stopper.Start()
                If DB.StreamMode = eStreamMode.SingleFrame Then
                    CallOK("ExpQHYCCDSingleFrame", QHY.QHYCamera.ExpQHYCCDSingleFrame(CamHandle))
                    Stopper.Stamp("ExpQHYCCDSingleFrame")
                Else
                    CallOK("BeginQHYCCDLive", QHY.QHYCamera.BeginQHYCCDLive(CamHandle))
                    Stopper.Stamp("BeginQHYCCDLive")
                End If

                '================================================================================
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

                '================================================================================
                'Get the buffer size from the DLL - typically too big but does not care ...
                Dim BytesToTransfer_reported As UInteger = QHY.QHYCamera.GetQHYCCDMemLength(CamHandle)
                LogVerbose("GetQHYCCDMemLength says: " & BytesToTransfer_reported.ValRegIndep.PadLeft(12) & " byte to transfer.")
                If CamRawBuffer.Length <> BytesToTransfer_reported Then
                    PinHandler = New cIntelIPP.cPinHandler
                    ReDim CamRawBuffer(CInt(BytesToTransfer_reported - 1))
                    CamRawBufferPtr = PinHandler.Pin(CamRawBuffer)
                End If
                Stopper.Stamp("GetQHYCCDMemLength & pinning")

                '================================================================================
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
                        DE()
                    Loop Until (LiveModeReady = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS) Or DB.StopFlag = True
                End If
                Dim BytesToTransfer_calculated As Long = Captured_W * Captured_H * CInt(CaptureBits / 8)
                LogVerbose("Calculation says       : " & BytesToTransfer_calculated.ValRegIndep.PadLeft(12) & " byte to transfer.")
                LogVerbose("Loaded image with " & Captured_W.ValRegIndep & "x" & Captured_H.ValRegIndep & " pixel @ " & CaptureBits & " bit resolution")
                Stopper.Stamp("GetQHYCCDSingleFrame")

                '================================================================================
                'Remove overscan
                Dim SingleStatCalc As New AstroNET.Statistics(DB.IPP)
                SingleStatCalc.DataProcessor_UInt16.ImageData = ChangeAspectIPP(DB.IPP, CamRawBuffer, CInt(Captured_W), CInt(Captured_H))      'only convert flat byte buffer to UInt16 matrix data
                If DB.RemoveOverscan = True Then
                    Dim NoOverscan(CInt(EffArea.Width - 1), CInt(EffArea.Height - 1)) As UInt16
                    Dim Status_GetROI As cIntelIPP.IppStatus = DB.IPP.Copy(SingleStatCalc.DataProcessor_UInt16.ImageData, NoOverscan, CInt(EffArea.X), CInt(EffArea.Y), CInt(EffArea.Width), CInt(EffArea.Height))
                    Dim Status_SetData As cIntelIPP.IppStatus = DB.IPP.Copy(NoOverscan, SingleStatCalc.DataProcessor_UInt16.ImageData, 0, 0, NoOverscan.GetUpperBound(0) + 1, NoOverscan.GetUpperBound(1) + 1)
                    If Status_GetROI <> cIntelIPP.IppStatus.NoErr Or Status_SetData <> cIntelIPP.IppStatus.NoErr Then
                        LogError("Overscan removal FAILED")
                    End If
                End If
                Stopper.Stamp("ChangeAspect")

                '================================================================================
                'Run statistics
                Dim SingleStat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics
                LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat, LoopStat)
                Stopper.Stamp("Statistics - calc")

                'Display statistics
                Dim DisplaySumStat As Boolean = False
                If DB.Log_ClearStat = True Then RTFGen.Clear()
                If DB.CaptureCount > 1 And DB.Log_ClearStat = True Then DisplaySumStat = True
                RTFGen.AddEntry("Capture #" & LoopCnt.ValRegIndep & " statistics:", Drawing.Color.Black, True, True)
                Dim SingStat As Collections.Generic.List(Of String) = SingleStat.StatisticsReport
                Dim TotaStat As Collections.Generic.List(Of String) = LoopStat.StatisticsReport
                For Idx As Integer = 0 To SingStat.Count - 1
                    Dim Line As String = SingStat(Idx)
                    If DisplaySumStat = True Then Line &= "#" & TotaStat(Idx).Substring(AstroNET.Statistics.sSingleChannelStatistics.ReportHeaderLength + 2)
                    RTFGen.AddEntry(Line, Drawing.Color.Black, True, False)
                Next Idx
                RTFGen.ForceRefresh()
                DE()
                Stopper.Stamp("Statistics - text")

                '================================================================================
                'Plot histogram

                Dim NormFactor As Double = LoopCnt
                Dim CurveMode As cZEDGraphService.eCurveMode = cZEDGraphService.eCurveMode.LinesAndPoints
                Dim CurrentCurveWidth As Integer = 1
                Dim MeanCurveWidth As Integer = 2
                If IsNothing(DB.Plotter) = True Then DB.Plotter = New cZEDGraphService(zgcMain)
                DB.Plotter.Clear()
                'Mean statistics
                If DB.CaptureCount > 1 And LoopCnt > 1 And DB.PlotMeanStatistics = True Then
                    DB.Plotter.PlotXvsY("R mean", LoopStat.BayerHistograms(0, 0), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("G1 mean", LoopStat.BayerHistograms(0, 1), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("G2 mean", LoopStat.BayerHistograms(1, 0), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("B mean", LoopStat.BayerHistograms(1, 1), NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, CurveMode, MeanCurveWidth))
                    DB.Plotter.PlotXvsY("Mono mean", LoopStat.MonochromHistogram, NormFactor, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, CurveMode, MeanCurveWidth))
                End If
                'Current statistics
                If DB.PlotSingleStatistics = True Then
                    DB.Plotter.PlotXvsY("R", SingleStat.BayerHistograms(0, 0), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("G1", SingleStat.BayerHistograms(0, 1), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("G2", SingleStat.BayerHistograms(1, 0), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("B", SingleStat.BayerHistograms(1, 1), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, CurveMode, CurrentCurveWidth))
                    DB.Plotter.PlotXvsY("Mono", SingleStat.MonochromHistogram, 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, CurveMode, CurrentCurveWidth))
                End If
                If DB.PlotLimitsFixed = True Then
                    DB.Plotter.ManuallyScaleXAxis(0, 65536)
                Else
                    DB.Plotter.ManuallyScaleXAxis(LoopStat.MonoStatistics.Min.Key, LoopStat.MonoStatistics.Max.Key)
                End If

                DB.Plotter.AutoScaleYAxisLog()
                DB.Plotter.GridOnOff(True, True)
                DB.Plotter.ForceUpdate()
                Stopper.Stamp("Statistics - plot")

                '================================================================================
                'Display focus image if required

                If SingleStat.MonoStatistics.Samples <= 1000000 Then
                    If IsNothing(FocusWindow) = True Then
                        FocusWindow = New cImgForm
                        FocusWindow.Show("Focus Window")
                    End If
                    UpdateFocusWindow(FocusWindow, SingleStatCalc.DataProcessor_UInt16.ImageData, SingleStat.MonoStatistics.Max.Key)
                    Stopper.Stamp("Focus window")
                End If

                '================================================================================
                'Store image

                If DB.StoreImage = True Then

                    Dim Path As String = System.IO.Path.Combine(DB.MyPath, DB_meta.GUID)
                    If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)
                    Dim FileCounter As String = String.Empty : If DB.CaptureCount > 1 Then FileCounter = "_#" & Format(LoopCnt, "000")
                    Dim FITSName As String = System.IO.Path.Combine(Path, FITSFileStart & FileCounter).Trim & "." & DB.FITSExtension

                    'Precalculation
                    Dim NAXIS1 As Integer = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(0) + 1
                    Dim NAXIS2 As Integer = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(1) + 1
                    Dim PLATESZ1 As Double = (Pixel_Size_W * NAXIS1) / 1000                         '[mm]
                    Dim PLATESZ2 As Double = (Pixel_Size_H * NAXIS2) / 1000                         '[mm]
                    Dim FOV1 As Double = 2 * Math.Atan(PLATESZ1 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)
                    Dim FOV2 As Double = 2 * Math.Atan(PLATESZ2 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)
                    Dim CamReadOutMode As New Text.StringBuilder : QHY.QHYCamera.GetQHYCCDReadModeName(CamHandle, DB.ReadOutMode, CamReadOutMode)

                    'Compose all FITS keyword entries
                    Dim CustomElement As New Collections.Generic.List(Of String())
                    Dim FITSKey As New cFITSKey

                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.OBS_ID), cFITSKeywords.GetString(DB_meta.GUID)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.PROGRAM), cFITSKeywords.GetString(Me.Text)})

                    AddNoEmptyElement(CustomElement, FITSKey(eFITSKeywords.OBJECT), cFITSKeywords.GetString(DB_meta.ObjectName))
                    AddNoEmptyElement(CustomElement, FITSKey(eFITSKeywords.RA), cFITSKeywords.GetString(DB_meta.TelescopeRightAscension))
                    AddNoEmptyElement(CustomElement, FITSKey(eFITSKeywords.DEC), cFITSKeywords.GetString(DB_meta.TelescopeDeclination))

                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.AUTHOR), cFITSKeywords.GetString(DB_meta.Author)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.ORIGIN), cFITSKeywords.GetString(DB_meta.Origin)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.TELESCOP), cFITSKeywords.GetString(DB_meta.Telescope)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.TELAPER), cFITSKeywords.GetDouble(DB_meta.TelescopeAperture / 1000.0)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.TELFOC), cFITSKeywords.GetDouble(DB_meta.TelescopeFocalLength / 1000.0)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.INSTRUME), cFITSKeywords.GetString(UsedCameraId.ToString)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.PIXSIZE1), cFITSKeywords.GetDouble(Pixel_Size_W)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.PIXSIZE2), cFITSKeywords.GetDouble(Pixel_Size_H)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.PLATESZ1), cFITSKeywords.GetDouble(PLATESZ1 / 10)})                'calculated from the image data as ROI may be set ...
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.PLATESZ2), cFITSKeywords.GetDouble(PLATESZ2 / 10)})                'calculated from the image data as ROI may be set ...
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.FOV1), cFITSKeywords.GetDouble(FOV1)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.FOV2), cFITSKeywords.GetDouble(FOV2)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.COLORTYP), "0"})                                                   '<- check

                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.DATE_OBS), cFITSKeywords.GetDateWithTime(ObsStart)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.DATE_END), cFITSKeywords.GetDateWithTime(ObsEnd)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.TIME_OBS), cFITSKeywords.GetTime(ObsStart)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.TIME_END), cFITSKeywords.GetTime(ObsEnd)})

                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.CRPIX1), cFITSKeywords.GetDouble(0.5 * (NAXIS1 + 1))})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.CRPIX2), cFITSKeywords.GetDouble(0.5 * (NAXIS2 + 1))})

                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.IMAGETYP), cFITSKeywords.GetString(DB_meta.ExposureType)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.EXPTIME), cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE) / 1000000)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.GAIN), cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN))})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.OFFSET), cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET))})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.BRIGHTNESS), cFITSKeywords.GetDouble(QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_BRIGHTNESS))})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.SETTEMP), cFITSKeywords.GetDouble(DB.TargetTemp)})
                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.CCDTEMP), cFITSKeywords.GetDouble(ObsStartTemp)})

                    CustomElement.Add(New String() {FITSKey(eFITSKeywords.QHY_MODE), cFITSKeywords.GetString(CamReadOutMode.ToString)})

                    'Create FITS file
                    cFITSWriter.Write(FITSName, SingleStatCalc.DataProcessor_UInt16.ImageData, cFITSWriter.eBitPix.Int16, CustomElement)
                    If DB.AutoOpenImage = True Then System.Diagnostics.Process.Start(FITSName)

                End If
                Stopper.Stamp("Store image")

                If DB.StopFlag = True Then Exit For

            Next LoopCnt

            '================================================================================
            'Stop live mode if used

            If DB.StreamMode = eStreamMode.LiveFrame Then
                CallOK("StopQHYCCDLive", QHY.QHYCamera.StopQHYCCDLive(CamHandle))
            End If

            'Release buffer handles
            PinHandler = Nothing

        End If

        'Close camera if selected 
        If CloseAtEnd = True Then CloseCamera()
        Stopper.Stamp("CloseCamera")

        '================================================================================
        'Display timing log

        If DB.Log_Timing = True Then
            Log("--------------------------------------------------------------")
            Log("TIMING:")
            Log(Stopper.GetLog)
            Log("--------------------------------------------------------------")
        End If

        tsslMain.Text = "--IDLE--"
        DB.RunningFlag = False

    End Sub

    Private Sub CloseCamera()
        If CamHandle <> IntPtr.Zero Then
            Log("Closing camera ...")
            QHY.QHYCamera.CancelQHYCCDExposingAndReadout(CamHandle)
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
            LogError("QHY ERROR on <" & Action & ">: <0x" & Hex(ErrorCode) & ">")
            Return False
        End If
    End Function

    Private Sub LogTime(ByVal Text As String, ByVal DurationMS As Long)
        Log(Text & " took " & DurationMS.ValRegIndep & " ms")
    End Sub

    Private Sub LogTiming(ByVal Text As String, ByVal Ticker As System.Diagnostics.Stopwatch)
        Log(Text & ": " & Ticker.ElapsedMilliseconds.ValRegIndep & " ms", False)
    End Sub

    Private Sub LogError(ByVal Text As String)
        Log("########### " & Text & " ###########", False)
    End Sub

    Private Sub Log(ByVal Text As String)
        Log(Text, False)
    End Sub

    Private Sub LogVerbose(ByVal Text As String)
        If DB.Log_Verbose = True Then Log(Text)
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
        RefreshProperties()

        'Set toolstrip icons
        tsbCapture.Image = ilMain.Images.Item("Capture.png")
        tsbStopCapture.Image = ilMain.Images.Item("StopCapture.png")

        'RTF statistics
        RTFGen.AttachToControl(rtbStatistics)
        RTFGen.RTFInit("Courier New", 8)

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
        RefreshProperties()
    End Sub

    Private Sub AllReadoutModesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllReadoutModesToolStripMenuItem.Click
        DB.StopFlag = False
        For Each Mode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            DB.ReadOutMode = Mode
            RefreshProperties()
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
            RefreshProperties()
            QHYCapture("QHY_EXP_" & Exposure.ToString.Trim, False)
            If DB.StopFlag = True Then Exit For
        Next Exposure
        CloseCamera()
    End Sub

    Private Sub GainVariationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GainVariationToolStripMenuItem.Click
        DB.StopFlag = False
        For Gain As Double = 0 To 200 Step 5
            DB.Gain = Gain
            RefreshProperties()
            QHYCapture("QHY_GAIN_" & Gain.ValRegIndep("000"), False)
            If DB.StopFlag = True Then Exit For
        Next Gain
        CloseCamera()
    End Sub

    Private Sub NoRealObjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NoRealObjectToolStripMenuItem.Click
        DB_meta.ObjectName = String.Empty
        DB_meta.TelescopeRightAscension = String.Empty
        DB_meta.TelescopeDeclination = String.Empty
        DB_meta.Telescope = String.Empty
        RefreshProperties()
    End Sub

    Private Sub TestWebInterfaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestWebInterfaceToolStripMenuItem.Click
        'Test call for the web interface
        System.Diagnostics.Process.Start("http://localhost:1250/GetParameterList")
    End Sub

    Private Sub DB_ServiceContract_ValueChanged() Handles DB_ServiceContract.ValueChanged
        RefreshProperties()
    End Sub

    Private Sub DB_ServiceContract_StartExposure() Handles DB_ServiceContract.StartExposure
        tsbCapture_Click(tsbCapture, Nothing)
    End Sub

    Private Sub FastLiveModeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FastLiveModeToolStripMenuItem.Click
        With DB
            .StreamMode = eStreamMode.LiveFrame
            .ExposureTime = 0.01
            .Gain = 120
            .ROI = New Drawing.Rectangle(.ROI.X, .ROI.Y, 1000, 1000)
            .CaptureCount = 10000
            .StoreImage = False
            .Log_ClearStat = True
        End With
        RefreshProperties()
    End Sub

    '''<summary>Update the content of the focus window.</summary>
    '''<param name="Form">Focus window.</param>
    '''<param name="Data">Data to display.</param>
    '''<param name="MaxData">Maximum in the data in order to normalize correct.</param>
    Private Sub UpdateFocusWindow(ByRef Form As cImgForm, ByRef Data(,) As UInt16, ByVal MaxData As Long)
        Dim OutputImage As New cLockBitmap(Data.GetUpperBound(0), Data.GetUpperBound(1))
        If MaxData = 0 Then MaxData = 1
        OutputImage.LockBits()
        Dim Stride As Integer = OutputImage.BitmapData.Stride
        Dim BytePerPixel As Integer = OutputImage.ColorBytesPerPixel
        Dim YOffset As Integer = 0
        For Y As Integer = 0 To OutputImage.Height - 1
            Dim BaseOffset As Integer = YOffset
            For X As Integer = 0 To OutputImage.Width - 1
                Dim DispVal As Integer = CInt(Data(X, Y) * (255 / MaxData))
                Dim Coloring As Drawing.Color = cColorMaps.Jet(DispVal)
                OutputImage.Pixels(BaseOffset) = Coloring.R
                OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                BaseOffset += BytePerPixel
            Next X
            YOffset += Stride
        Next Y
        OutputImage.UnlockBits()
        Form.Image.Image = OutputImage.BitmapToProcess
    End Sub

    Private Function ActiveFilter(ByRef CamHandle As IntPtr, ByVal FilterToSelect As eFilter) As eFilter
        Dim RetVal As eFilter = eFilter.Invalid
        If CheckFilter(CamHandle) <> FilterToSelect Then
            Do
                SelectFilter(CamHandle, FilterToSelect)
                System.Threading.Thread.Sleep(500)
                RetVal = CheckFilter(CamHandle)
            Loop Until RetVal = FilterToSelect
        End If
        Return RetVal
    End Function

    '''<summary>Read the current filter wheel position.</summary>
    '''<returns>Filter that is IN or invalid if there was something wrong.</returns>
    '''<seealso cref="https://note.youdao.com/share/?token=48C579B49B5840609AB9B6D7D375B742&gid=7195236"/>
    Private Function CheckFilter(ByRef CamHandle As IntPtr) As eFilter
        Dim RetVal As eFilter = eFilter.Invalid
        Dim FilterState(63) As Byte
        Dim Pinner As New cIntelIPP.cPinHandler
        Dim FilterStatePtr As IntPtr = Pinner.Pin(FilterState)
        Dim NumberOfSlots As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CFWSLOTSNUM)
        LogVerbose("Filter wheel with <" & NumberOfSlots.ValRegIndep & "> sloted detected")
        If QHY.QHYCamera.GetQHYCCDCFWStatus(CamHandle, FilterStatePtr) = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then
            Dim Filter As Char = Chr(FilterState(0))        ' '0' means filter position 1, '1' means filter position 2, ...
            Select Case Filter
                Case "0"c To "9"c
                    RetVal = CType(Val(Filter.ToString) + 1, eFilter)
                    Log("Filter wheel position <" & [Enum].GetName(GetType(eFilter), RetVal) & ">")
                Case "N"c
                    Log("Filter wheel running ...")
                Case Else
                    Log("Filter wheel position answer <" & Filter.ToString & "> is ??????")
            End Select
        Else
            LogError("Filter wheel found but could not read status!")
        End If
        Pinner = Nothing
        Return RetVal
    End Function

    '''<summary>Select a certain filter.</summary>
    Private Function SelectFilter(ByRef CamHandle As IntPtr, ByVal FilterToSelect As eFilter) As eFilter
        Dim RetVal As eFilter = eFilter.Invalid
        Dim FilterState(63) As Byte
        Dim Pinner As New cIntelIPP.cPinHandler
        Dim FilterStatePtr As IntPtr = Pinner.Pin(FilterState)
        If FilterToSelect <> eFilter.Invalid Then
            FilterState(0) = CByte(Asc((FilterToSelect - 1).ToString.Trim))
            If QHY.QHYCamera.SendOrder2QHYCCDCFW(CamHandle, FilterStatePtr, 1) = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then
                Log("  Filter requested: " & FilterToSelect.ToString.Trim)
                RetVal = FilterToSelect
            Else
                LogError("  !!! Filter select failed: " & DB.FilterSlot.ToString.Trim)
            End If
        End If
        Pinner = Nothing
        Return FilterToSelect
    End Function

    Private Function InitQHY(ByVal CamID As String, ByRef Stopper As cStopper) As Boolean

        'Init if not yet done
        If CamHandle = IntPtr.Zero Or UsedReadMode = eReadOutMode.Unvalid Or UsedStreamMode = UInteger.MaxValue Then

            If CallOK(QHY.QHYCamera.InitQHYCCDResource) = True Then                                                                         'Init DLL itself
                Stopper.Stamp("InitQHYCCDResource")
                Dim CameraCount As UInteger = QHY.QHYCamera.ScanQHYCCD                                                                      'Scan for connected cameras
                Stopper.Stamp("ScanQHYCCD")
                If CameraCount > 0 Then                                                                                                     'If there is a camera found

                    'Find correct camera
                    Log("Found " & CameraCount.ValRegIndep & " cameras:")
                    Dim AllCameras As New Collections.Generic.Dictionary(Of Integer, System.Text.StringBuilder)
                    For Idx As Integer = 0 To CInt(CameraCount - 1)
                        Dim CurrentCamID As New System.Text.StringBuilder(0)                                                                'Prepare camera ID holder
                        CallOK(QHY.QHYCamera.GetQHYCCDId(Idx, CurrentCamID))                                                                'Fetch camera ID
                        AllCameras.Add(Idx, CurrentCamID)
                        Log("  Camera #" & (Idx + 1).ValRegIndep & ": <" & CurrentCamID.ToString & ">")
                    Next Idx
                    For Each CamIdx As Integer In AllCameras.Keys
                        If AllCameras(CamIdx).ToString.Contains(CamID) = True Then
                            UsedCameraId = New System.Text.StringBuilder(AllCameras(CamIdx).ToString)
                            Exit For
                        End If
                    Next CamIdx

                    If IsNothing(UsedCameraId) = True Then Return False
                    If UsedCameraId.Length = 0 Then Return False

                    Log("Found QHY camera to use: <" & UsedCameraId.ToString & ">")                                                                 'Display fetched camera ID
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
                        Return False
                    End If
                Else
                    Log("Init OK but no camera found!")
                    CamHandle = IntPtr.Zero
                    Return False
                End If
            Else
                Log("Init QHY did fail!")
            End If
        End If

        'Everything OK
        Stopper.Stamp("InitQHY")
        Return True

    End Function

    Private Sub FilterSelectionToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim Stopper As New cStopper
        If InitQHY(DB.CamToUse, Stopper) = False Then
            Log("No suitable camera found!")
        Else
            'Filter test
        End If
        CloseCamera()
    End Sub

    Private Sub CenterROIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CenterROIToolStripMenuItem.Click
        Dim Delta As Integer = 100
        With DB
            .ROI = New Drawing.Rectangle((9600 \ 2) - Delta, (6422 \ 2) - Delta, 2 * Delta, 2 * Delta)
        End With
        RefreshProperties()
    End Sub

    Private Sub LoadPositionFrom10MicronToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadPositionFrom10MicronToolStripMenuItem.Click
        Dim Client10Micron As New Net.Sockets.TcpClient(DB_meta.IP_10Micron, 3490)
        Dim Stream10Micron As Net.Sockets.NetworkStream = Client10Micron.GetStream
        c10Micron.SendQuery(Stream10Micron, c10Micron.SetCommand.SetUltraHighPrecision)
        DB_meta.SiteLatitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.SiteLatitude)
        DB_meta.SiteLongitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.SiteLongitude)
        DB_meta.TelescopeRightAscension = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeRightAscension)
        DB_meta.TelescopeDeclination = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeDeclination)
        DB_meta.TelescopeAltitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeAltitude)
        DB_meta.TelescopeAzimuth = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeAzimuth)
        RefreshProperties()
    End Sub

    '''<summary>Refresh all property grid displays.</summary>
    Private Sub RefreshProperties()
        pgMain.SelectedObject = DB
        pgMeta.SelectedObject = DB_meta
        DE()
    End Sub

    Private Sub FITSCommentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FITSCommentToolStripMenuItem.Click
        Dim FITSKey As New cFITSKey
        MsgBox(FITSKey(eFITSKeywords.COLORTYP))
    End Sub

    Private Sub USBTreeReaderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles USBTreeReaderToolStripMenuItem.Click
        Dim USBTreeExe As String = Everything.GetSearchResult("UsbTreeView.exe")(0)
        Dim DumpFile As String = "C:\Users\albusmw\Dropbox\UsbTreeView_report_STERNWARTE.txt"
        If System.IO.File.Exists(DumpFile) = True Then System.IO.File.Delete(DumpFile)
        Dim EXE As Diagnostics.Process = Diagnostics.Process.Start(USBTreeExe, "/R=""" & DumpFile & """")
        EXE.WaitForExit()
        Dim FoundPorts As New Collections.Generic.Dictionary(Of String, String)
        Dim LastPort As String = String.Empty
        For Each Line As String In System.IO.File.ReadAllLines(DumpFile)
            If Line.StartsWith("Port Chain") Then
                LastPort = Line.Replace("Port Chain", String.Empty).Trim.TrimStart(":"c).Trim
                FoundPorts.Add(LastPort, String.Empty)
            End If
            If Line.Trim.StartsWith("Device Description") Then
                FoundPorts(LastPort) = Line.Replace("Device Description", String.Empty).Trim.TrimStart(":"c).Trim
            End If
            If Line.Trim.StartsWith("COM-Port") Then
                FoundPorts(LastPort) &= " - " & Line.Replace("COM-Port", String.Empty).Trim.TrimStart(":"c).Trim
            End If
        Next Line
        'Remove empty entries with no device descriptor
        Dim FinalList As New Collections.Generic.Dictionary(Of String, String)
        For Each Entry As String In FoundPorts.Keys
            If FoundPorts(Entry).Length > 0 Then FinalList.Add(Entry, FoundPorts(Entry))
        Next
        MsgBox(FoundPorts.Count.ToString.Trim & " ports found!")
    End Sub

End Class