﻿Option Explicit On
Option Strict On
Imports System.Windows.Forms

Public Class MainForm

    Const BitsPerByte As Integer = 8

    '''<summary>DB that holds all relevant information.</summary>
    Private DB As New cDB
    Private DB_meta As New cDB_meta
    Private WithEvents DB_ServiceContract As cDB_ServiceContract

    '''<summary>Indicate that a property was changed and parameters need to be updated in the camera.</summary>
    Private PropertyChanged As Boolean = False

    '''<summary>Handle to the camera.</summary>
    Private CamHandle As IntPtr = IntPtr.Zero

    Private UsedReadMode As eReadOutMode = eReadOutMode.Unvalid
    Private UsedCameraId As System.Text.StringBuilder
    Private UsedStreamMode As UInteger = UInteger.MaxValue

    Private RTFGen As New Atomic.cRTFGenerator
    Private FocusWindow As cImgForm = Nothing

    '''<summary>Accumulated statistics.</summary>
    Private LoopStat As AstroNET.Statistics.sStatistics
    '''<summary>Number of single statistics in the accumulated statistics.</summary>
    Private LoopStatCount As Integer = 0

    '''<summary>Run a single capture.</summary>
    Private Sub RunCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunCaptureToolStripMenuItem.Click
        QHYCapture(DB.FileName, True)
    End Sub

    '''<summary>Command for a QHY capture run.</summary>
    Public Sub QHYCapture(ByVal FITSFileStart As String, ByVal CloseAtEnd As Boolean)

        Dim SDKVersion(3) As UInteger
        Dim Chip_Physical As sSize_Dbl      'chip physical size [mm]
        Dim Chip_Pixel As sSize_UInt        'chip size [pixel]
        Dim Pixel_Size As sSize_Dbl         'chip pixel size [um]
        Dim EffArea As sRect_UInt
        Dim OverArea As sRect_UInt
        Dim bpp As UInteger = 0

        'Start
        DB.StopFlag = False
        DB.RunningFlag = True
        Dim Stopper As New cStopper
        Stopper.Start()

        'Try to get a suitable camera and continue if found
        If InitQHY(DB.CamToUse, Stopper) = False Then Log("No suitable camera found!")
        If CamHandle <> IntPtr.Zero Then

            'Get chip properties
            QHY.QHYCamera.GetQHYCCDChipInfo(CamHandle, Chip_Physical.Width, Chip_Physical.Height, Chip_Pixel.Width, Chip_Pixel.Height, Pixel_Size.Width, Pixel_Size.Height, bpp)
            QHY.QHYCamera.GetQHYCCDEffectiveArea(CamHandle, EffArea.X, EffArea.Y, EffArea.Width, EffArea.Height)
            QHY.QHYCamera.GetQHYCCDOverScanArea(CamHandle, OverArea.X, OverArea.Y, OverArea.Width, OverArea.Height)
            Stopper.Stamp("Get chip properties")

            'Log chip properties
            If DB.Log_CamProp = True Then
                QHY.QHYCamera.GetQHYCCDSDKVersion(SDKVersion(0), SDKVersion(1), SDKVersion(2), SDKVersion(3))                   'Get the SDK version
                Log("SDK version: " & BuildSDKVersion(SDKVersion))                                                              'Display the SDK version

                Log("Chip info (bpp: " & bpp.ValRegIndep & ")")
                Log("  Chip  W x H    :" & Chip_Physical.Width.ValRegIndep & " x " & Chip_Physical.Height.ValRegIndep & " mm")
                Log("  Image W x H    :" & Chip_Pixel.Width.ValRegIndep & " x " & Chip_Pixel.Height.ValRegIndep & " pixel")
                Log("  Pixel W x H    :" & Pixel_Size.Width.ValRegIndep & " x " & Pixel_Size.Height.ValRegIndep & " um")
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

            'Prepare buffers
            LoopStat = New AstroNET.Statistics.sStatistics
            Dim PinHandler As cIntelIPP.cPinHandler
            Dim CamRawBuffer As Byte() = {}
            Dim CamRawBufferPtr As IntPtr = Nothing
            Stopper.Stamp("Prepare buffers")

            'Filter wheel
            Dim FilterActive As eFilter = eFilter.Invalid
            If DB.FilterSlot <> eFilter.Invalid Then
                FilterActive = ActiveFilter(CamHandle, DB.FilterSlot)
            End If
            Stopper.Stamp("Select filter")

            Dim LastCaptureEnd As DateTime = DateTime.MinValue                          'last capture end to calculate a FPS number
            Dim TotalCaptureTime As Double = 0
            For CaptureIdx As UInt32 = 1 To DB.CaptureCount

                'Calculate ROI parameters that will be passed to camera
                Dim ROIForCapture As System.Drawing.Rectangle = CalculateROI(Chip_Pixel)

                'Set exposure parameters (first time / on property change / always if configured)
                If (CaptureIdx = 1) Or (DB.ConfigAlways = True) Or PropertyChanged = True Then SetExpParameters(ROIForCapture)
                Stopper.Stamp("Set exposure parameters")

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
                tsslMain.Text = "Taking capture " & CaptureIdx.ValRegIndep & "/" & DB.CaptureCount.ValRegIndep
                Dim SingleCaptureData As New cSingleCaptureData
                With SingleCaptureData
                    .CaptureIdx = CaptureIdx
                    .FilterActive = FilterActive
                    .CamReadOutMode = New Text.StringBuilder : QHY.QHYCamera.GetQHYCCDReadModeName(CamHandle, DB.ReadOutMode, .CamReadOutMode)
                    .ExpTime = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE) / 1000000
                    .Gain = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN)
                    .Offset = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET)
                    .Brightness = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_BRIGHTNESS)
                    .ObsStartTemp = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
                End With

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
                IdleExposureTime(DB.ExposureTime)

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

                'FPS calculation
                SingleCaptureData.ObsEnd = Now
                If LastCaptureEnd <> DateTime.MinValue Then
                    Dim ThisDuration As Double = (SingleCaptureData.ObsEnd - LastCaptureEnd).TotalSeconds
                    TotalCaptureTime += ThisDuration
                    tsmiFPSIndicator.Text = Format(1 / ThisDuration, "0.0") & " FPS, mean: " & Format(CaptureIdx / TotalCaptureTime, "0.0") & " FPS"
                End If
                LastCaptureEnd = SingleCaptureData.ObsEnd

                Dim BytesToTransfer_calculated As Long = Captured_W * Captured_H * CInt(CaptureBits / BitsPerByte)
                LogVerbose("Calculation says       : " & BytesToTransfer_calculated.ValRegIndep.PadLeft(12) & " byte to transfer.")
                LogVerbose("Loaded image with " & Captured_W.ValRegIndep & "x" & Captured_H.ValRegIndep & " pixel @ " & CaptureBits & " bit resolution")
                Stopper.Stamp("GetQHYCCDSingleFrame")

                '================================================================================
                'Remove overscan - do NOT run if an ROU is set
                Dim SingleStatCalc As New AstroNET.Statistics(DB.IPP)
                SingleStatCalc.DataProcessor_UInt16.ImageData = ChangeAspectIPP(DB.IPP, CamRawBuffer, CInt(Captured_W), CInt(Captured_H))      'only convert flat byte buffer to UInt16 matrix data
                If DB.RemoveOverscan = True And DB.ROISet = False Then
                    Dim NoOverscan(CInt(EffArea.Width - 1), CInt(EffArea.Height - 1)) As UInt16
                    Dim Status_GetROI As cIntelIPP.IppStatus = DB.IPP.Copy(SingleStatCalc.DataProcessor_UInt16.ImageData, NoOverscan, CInt(EffArea.X), CInt(EffArea.Y), CInt(EffArea.Width), CInt(EffArea.Height))
                    Dim Status_SetData As cIntelIPP.IppStatus = DB.IPP.Copy(NoOverscan, SingleStatCalc.DataProcessor_UInt16.ImageData, 0, 0, NoOverscan.GetUpperBound(0) + 1, NoOverscan.GetUpperBound(1) + 1)
                    If Status_GetROI <> cIntelIPP.IppStatus.NoErr Or Status_SetData <> cIntelIPP.IppStatus.NoErr Then
                        LogError("Overscan removal FAILED")
                    End If
                End If
                SingleCaptureData.NAXIS1 = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(0) + 1
                SingleCaptureData.NAXIS2 = SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(1) + 1
                Stopper.Stamp("ChangeAspect")

                '================================================================================
                'Run statistics
                Dim SingleStat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics
                LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat, LoopStat) : LoopStatCount += 1
                Stopper.Stamp("Statistics - calc")

                'Display statistics
                Dim DisplaySumStat As Boolean = False
                If DB.Log_ClearStat = True Then RTFGen.Clear()
                If DB.CaptureCount > 1 And DB.Log_ClearStat = True Then DisplaySumStat = True
                RTFGen.AddEntry("Capture #" & CaptureIdx.ValRegIndep & " statistics:", Drawing.Color.Black, True, True)
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

                Dim CurveMode As cZEDGraphService.eCurveMode = cZEDGraphService.eCurveMode.LinesAndPoints
                Dim CurrentCurveWidth As Integer = 1
                Dim MeanCurveWidth As Integer = 2
                If IsNothing(DB.Plotter) = True Then DB.Plotter = New cZEDGraphService(zgcMain)
                DB.Plotter.Clear()
                'Mean statistics
                If DB.CaptureCount > 1 And LoopStatCount > 1 And DB.PlotMeanStatistics = True Then
                    If IsNothing(LoopStat.BayerHistograms) = False Then
                        DB.Plotter.PlotXvsY("R mean", LoopStat.BayerHistograms(0, 0), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, CurveMode, MeanCurveWidth))
                        DB.Plotter.PlotXvsY("G1 mean", LoopStat.BayerHistograms(0, 1), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, CurveMode, MeanCurveWidth))
                        DB.Plotter.PlotXvsY("G2 mean", LoopStat.BayerHistograms(1, 0), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, CurveMode, MeanCurveWidth))
                        DB.Plotter.PlotXvsY("B mean", LoopStat.BayerHistograms(1, 1), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, CurveMode, MeanCurveWidth))
                    End If
                    If IsNothing(LoopStat.MonochromHistogram) = False Then
                        DB.Plotter.PlotXvsY("Mono mean", LoopStat.MonochromHistogram, LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, CurveMode, MeanCurveWidth))
                    End If
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
                If DB.ShowLiveImage = True Then
                    Dim SizeInfo As String = String.Empty
                    SizeInfo &= (SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(0) + 1).ValRegIndep & "x"
                    SizeInfo &= (SingleStatCalc.DataProcessor_UInt16.ImageData.GetUpperBound(1) + 1).ValRegIndep
                    If IsNothing(FocusWindow) = True Then
                        FocusWindow = New cImgForm
                        FocusWindow.Show("Focus Window <" & SizeInfo & ">")
                    End If
                    UpdateFocusWindow(FocusWindow, SingleStatCalc.DataProcessor_UInt16.ImageData, SingleStat.MonoStatistics.Min.Key, SingleStat.MonoStatistics.Max.Key)
                    Stopper.Stamp("Focus window")
                End If

                '================================================================================
                'Store image

                If DB.StoreImage = True Then

                    Dim Path As String = System.IO.Path.Combine(DB.MyPath, DB_meta.GUID)
                    If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)

                    'Compose all FITS keyword entries
                    Dim FileNameToWrite As String = FITSFileStart
                    Dim CustomElement As Collections.Generic.List(Of String()) = GenerateFITSHeader(SingleCaptureData, Pixel_Size, FileNameToWrite)

                    Dim FITSName As String = System.IO.Path.Combine(Path, FileNameToWrite & "." & DB.FITSExtension)
                    cFITSWriter.Write(FITSName, SingleStatCalc.DataProcessor_UInt16.ImageData, cFITSWriter.eBitPix.Int16, CustomElement)
                    If DB.AutoOpenImage = True Then System.Diagnostics.Process.Start(FITSName)

                End If
                Stopper.Stamp("Store image")

                If DB.StopFlag = True Then Exit For

            Next CaptureIdx

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

    '''<summary>Keep the GUI alive during exposure.</summary>
    '''<param name="ExposureTime">Expected time for the display.</param>
    Private Sub IdleExposureTime(ByVal ExposureTime As Double)
        If ExposureTime > 1 Then
            Dim ExpStart As DateTime = Now
            tspbProgress.Maximum = CInt(ExposureTime)
            Do
                System.Threading.Thread.Sleep(100)
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
    End Sub

    '''<summary>Calculate all entries from the FITS header.</summary>
    '''<param name="SingleCaptureData">Capture configuration.</param>
    '''<param name="FileNameToWrite">File name with replacement parameters to use.</param>
    Private Function GenerateFITSHeader(ByVal SingleCaptureData As cSingleCaptureData, ByVal Pixel_Size As sSize_Dbl, ByRef FileNameToWrite As String) As Collections.Generic.List(Of String())

        Dim CustomElement As New Collections.Generic.List(Of String())

        'Precalculation
        Dim PLATESZ1 As Double = (Pixel_Size.Width * SingleCaptureData.NAXIS1) / 1000                           '[mm]
        Dim PLATESZ2 As Double = (Pixel_Size.Height * SingleCaptureData.NAXIS1) / 1000                          '[mm]
        Dim FOV1 As Double = 2 * Math.Atan(PLATESZ1 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)
        Dim FOV2 As Double = 2 * Math.Atan(PLATESZ2 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)

        AddFITSHeaderCard(CustomElement, eFITSKeywords.OBS_ID, cFITSKeywords.GetString(DB_meta.GUID))

        AddFITSHeaderCard(CustomElement, eFITSKeywords.OBJECT, cFITSKeywords.GetString(DB_meta.ObjectName))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.RA, cFITSKeywords.GetString(DB_meta.TelescopeRightAscension))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.DEC, cFITSKeywords.GetString(DB_meta.TelescopeDeclination))

        AddFITSHeaderCard(CustomElement, eFITSKeywords.AUTHOR, cFITSKeywords.GetString(DB_meta.Author))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.ORIGIN, cFITSKeywords.GetString(DB_meta.Origin))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.TELESCOP, cFITSKeywords.GetString(DB_meta.Telescope))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.TELAPER, cFITSKeywords.GetDouble(DB_meta.TelescopeAperture / 1000.0))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.TELFOC, cFITSKeywords.GetDouble(DB_meta.TelescopeFocalLength / 1000.0))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.INSTRUME, cFITSKeywords.GetString(UsedCameraId.ToString))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.PIXSIZE1, cFITSKeywords.GetDouble(Pixel_Size.Width))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.PIXSIZE2, cFITSKeywords.GetDouble(Pixel_Size.Height))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.PLATESZ1, cFITSKeywords.GetDouble(PLATESZ1 / 10))                        'calculated from the image data as ROI may be set ...
        AddFITSHeaderCard(CustomElement, eFITSKeywords.PLATESZ2, cFITSKeywords.GetDouble(PLATESZ2 / 10))                        'calculated from the image data as ROI may be set ...
        AddFITSHeaderCard(CustomElement, eFITSKeywords.FOV1, cFITSKeywords.GetDouble(FOV1))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.FOV2, cFITSKeywords.GetDouble(FOV2))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.COLORTYP, "0")                                                           '<- check

        AddFITSHeaderCard(CustomElement, eFITSKeywords.DATE_OBS, cFITSKeywords.GetDateWithTime(SingleCaptureData.ObsStart))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.DATE_END, cFITSKeywords.GetDateWithTime(SingleCaptureData.ObsEnd))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.TIME_OBS, cFITSKeywords.GetTime(SingleCaptureData.ObsStart))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.TIME_END, cFITSKeywords.GetTime(SingleCaptureData.ObsEnd))

        AddFITSHeaderCard(CustomElement, eFITSKeywords.CRPIX1, cFITSKeywords.GetDouble(0.5 * (SingleCaptureData.NAXIS1 + 1)))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.CRPIX2, cFITSKeywords.GetDouble(0.5 * (SingleCaptureData.NAXIS2 + 1)))

        AddFITSHeaderCard(CustomElement, eFITSKeywords.IMAGETYP, cFITSKeywords.GetString(DB_meta.ExposureType))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.EXPTIME, cFITSKeywords.GetDouble(SingleCaptureData.ExpTime))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.GAIN, cFITSKeywords.GetDouble(SingleCaptureData.Gain))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.OFFSET, cFITSKeywords.GetDouble(SingleCaptureData.Offset))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.BRIGHTNESS, cFITSKeywords.GetDouble(SingleCaptureData.Brightness))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.SETTEMP, cFITSKeywords.GetDouble(DB.TargetTemp))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.CCDTEMP, cFITSKeywords.GetDouble(SingleCaptureData.ObsStartTemp))

        AddFITSHeaderCard(CustomElement, eFITSKeywords.QHY_MODE, cFITSKeywords.GetString(SingleCaptureData.CamReadOutMode.ToString))
        AddFITSHeaderCard(CustomElement, eFITSKeywords.PROGRAM, cFITSKeywords.GetString(Me.Text))

        'Create FITS file name
        FileNameToWrite = FileNameToWrite.Replace("$IDX$", Format(SingleCaptureData.CaptureIdx, "000"))
        FileNameToWrite = FileNameToWrite.Replace("$CNT$", Format(DB.CaptureCount, "000"))
        FileNameToWrite = FileNameToWrite.Replace("$EXP$", SingleCaptureData.ExpTime.ValRegIndep)
        FileNameToWrite = FileNameToWrite.Replace("$GAIN$", SingleCaptureData.Gain.ValRegIndep)
        FileNameToWrite = FileNameToWrite.Replace("$OFFS$", SingleCaptureData.Offset.ValRegIndep)
        FileNameToWrite = FileNameToWrite.Replace("$FILT$", [Enum].GetName(GetType(eFilter), SingleCaptureData.FilterActive))
        FileNameToWrite = FileNameToWrite.Replace("$RMODE$", [Enum].GetName(GetType(eReadOutMode), DB.ReadOutMode))

        Return CustomElement

    End Function

    '''<summary>Calculate the size of the requested ROI.</summary>
    '''<param name="Chip_Pixel">Size of the CCD ship [pixel].</param>
    Private Function CalculateROI(ByVal Chip_Pixel As sSize_UInt) As System.Drawing.Rectangle
        Dim ROIForCapture As New System.Drawing.Rectangle
        With ROIForCapture
            .X = DB.ROI.X
            .Y = DB.ROI.Y
            .Width = DB.ROI.Width
            If (DB.ROI.X + DB.ROI.Width > Chip_Pixel.Width) Or (.Width = 0) Then .Width = CInt(Chip_Pixel.Width - DB.ROI.X)
            .Height = DB.ROI.Height
            If (DB.ROI.Y + DB.ROI.Height > Chip_Pixel.Height) Or (.Height = 0) Then .Height = CInt(Chip_Pixel.Height - DB.ROI.Y)
        End With
        Return ROIForCapture
    End Function

    '''<summary>Set the exposure parameters</summary>
    Private Sub SetExpParameters(ByVal ROIForCapture As System.Drawing.Rectangle)
        CallOK("SetQHYCCDBinMode", QHY.QHYCamera.SetQHYCCDBinMode(CamHandle, DB.Binning, DB.Binning))
        CallOK("SetQHYCCDResolution", QHY.QHYCamera.SetQHYCCDResolution(CamHandle, CUInt(ROIForCapture.X), CUInt(ROIForCapture.Y), CUInt(ROIForCapture.Width \ DB.Binning), CUInt(ROIForCapture.Height \ DB.Binning)))
        CallOK("CONTROL_TRANSFERBIT", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_TRANSFERBIT, DB.ReadResolution))
        CallOK("CONTROL_GAIN", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN, DB.Gain))
        CallOK("CONTROL_OFFSET", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET, DB.Offset))
        CallOK("CONTROL_USBTRAFFIC", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_USBTRAFFIC, DB.USBTraffic))
        CallOK("CONTROL_DDR", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_DDR, CInt(IIf(DB.DDR_RAM = True, 1.0, 0.0))))
        CallOK("CONTROL_EXPOSURE", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE, DB.ExposureTime * 1000000))
        PropertyChanged = False
    End Sub

    Private Sub AddFITSHeaderCard(ByRef Container As Collections.Generic.List(Of String()), ByVal Keyword As eFITSKeywords, ByVal VAlue As String)
        If String.IsNullOrEmpty(VAlue) = False Then
            Dim FITSKey As New cFITSKey
            Container.Add(New String() {FITSKey(Keyword), VAlue, FITSKey.Comment(Keyword)})
        End If
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

    Private Sub LogVerbose(ByVal Text As Collections.Generic.List(Of String))
        For Each Line As String In Text
            LogVerbose(Line)
        Next Line
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

        'Get build data
        Dim BuildDate As String = String.Empty
        Dim AllResources As String() = System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceNames
        For Each Entry As String In AllResources
            If Entry.EndsWith(".BuildDate.txt") Then
                BuildDate = " (Build of " & (New System.IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream(Entry)).ReadToEnd.Trim).Replace(",", ".") & ")"
                Exit For
            End If
        Next Entry
        Me.Text &= BuildDate

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
        Dim WebServicePort As String = DB.INI.Get("Connections", "WebInterfacePort", "1250")
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
            .Gain = 20
            .ROI = New Drawing.Rectangle(.ROI.X, .ROI.Y, 100, 100)
            .CaptureCount = UInt32.MaxValue
            .StoreImage = False
            .Log_ClearStat = True
            .DDR_RAM = False
            .ConfigAlways = False
            .FilterSlot = eFilter.Invalid
        End With
        RefreshProperties()
    End Sub

    '''<summary>Update the content of the focus window.</summary>
    '''<param name="Form">Focus window.</param>
    '''<param name="Data">Data to display.</param>
    '''<param name="MaxData">Maximum in the data in order to normalize correct.</param>
    Private Sub UpdateFocusWindow(ByRef Form As cImgForm, ByRef Data(,) As UInt16, ByVal MinData As Long, ByVal MaxData As Long)
        Dim OutputImage As New cLockBitmap(Data.GetUpperBound(0), Data.GetUpperBound(1))
        If MaxData = 0 Then MaxData = 1
        OutputImage.LockBits()
        Dim Stride As Integer = OutputImage.BitmapData.Stride
        Dim BytePerPixel As Integer = OutputImage.ColorBytesPerPixel
        Dim YOffset As Integer = 0
        For Y As Integer = 0 To OutputImage.Height - 1
            Dim BaseOffset As Integer = YOffset
            For X As Integer = 0 To OutputImage.Width - 1
                Dim DispVal As Integer = CInt((Data(X, Y) - MinData) * (255 / (MaxData - MinData)))
                Dim Coloring As Drawing.Color = cColorMaps.Bone(DispVal)
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
                SelectFilter(CamHandle)
                System.Threading.Thread.Sleep(500)
                RetVal = CheckFilter(CamHandle)
            Loop Until RetVal = FilterToSelect Or DB.StopFlag = True Or DB.FilterSlot = eFilter.Invalid
        Else
            RetVal = FilterToSelect
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
    Private Function SelectFilter(ByRef CamHandle As IntPtr) As eFilter
        Dim RetVal As eFilter = eFilter.Invalid
        Dim FilterState(63) As Byte
        Dim Pinner As New cIntelIPP.cPinHandler
        Dim FilterStatePtr As IntPtr = Pinner.Pin(FilterState)
        If DB.FilterSlot <> eFilter.Invalid Then
            FilterState(0) = CByte(Asc((DB.FilterSlot - 1).ToString.Trim))
            If QHY.QHYCamera.SendOrder2QHYCCDCFW(CamHandle, FilterStatePtr, 1) = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then
                Log("  Filter requested: " & DB.FilterSlot.ToString.Trim)
                RetVal = DB.FilterSlot
            Else
                LogError("  !!! Filter select failed: " & DB.FilterSlot.ToString.Trim)
            End If
        End If
        Pinner = Nothing
        Return RetVal
    End Function

    Private Function InitQHY(ByVal CamID As String, ByRef Stopper As cStopper) As Boolean

        'Init if not yet done
        If CamHandle = IntPtr.Zero Or UsedReadMode = eReadOutMode.Unvalid Or UsedStreamMode = UInteger.MaxValue Then

            If CallOK(QHY.QHYCamera.InitQHYCCDResource) = True Then                                                                         'Init DLL itself
                Stopper.Stamp("InitQHYCCDResource")
                Dim CameraCount As UInteger = QHY.QHYCamera.ScanQHYCCD                                                                      'Scan for connected cameras
                Stopper.Stamp("ScanQHYCCD")
                If CameraCount > 0 Then                                                                                                     'If there is a camera found

                    Dim CamScanReport As New Collections.Generic.List(Of String)

                    'Get all cameras
                    CamScanReport.Add("Found " & CameraCount.ValRegIndep & " cameras:")
                    Dim AllCameras As New Collections.Generic.Dictionary(Of Integer, System.Text.StringBuilder)
                    For Idx As Integer = 0 To CInt(CameraCount - 1)
                        Dim CurrentCamID As New System.Text.StringBuilder(0)                                                                'Prepare camera ID holder
                        If CallOK(QHY.QHYCamera.GetQHYCCDId(Idx, CurrentCamID)) = True Then                                                               'Fetch camera ID
                            AllCameras.Add(Idx, CurrentCamID)
                            CamScanReport.Add("  Camera #" & (Idx + 1).ValRegIndep & ": <" & CurrentCamID.ToString & ">")
                        Else
                            CamScanReport.Add("  Camera #" & (Idx + 1).ValRegIndep & ": <<?????>>")
                        End If
                    Next Idx

                    'Find correct camera
                    UsedCameraId = New System.Text.StringBuilder
                    For Each CamIdx As Integer In AllCameras.Keys
                        If AllCameras(CamIdx).ToString.Contains(CamID) = True Then
                            UsedCameraId = New System.Text.StringBuilder(AllCameras(CamIdx).ToString)
                            Exit For
                        End If
                    Next CamIdx

                    'Exit if camera is not correct
                    If UsedCameraId.Length = 0 Then
                        Log(CamScanReport)
                        Return False
                    Else
                        LogVerbose(CamScanReport)
                    End If

                    LogVerbose("Found QHY camera to use: <" & UsedCameraId.ToString & ">")              'Display fetched camera ID
                    CamHandle = QHY.QHYCamera.OpenQHYCCD(UsedCameraId)                                  'Open the camera
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
                        Log("Init QHY camera  <" & UsedCameraId.ToString & "> ...")
                        If CallOK(QHY.QHYCamera.SetQHYCCDReadMode(CamHandle, DB.ReadOutMode)) = True Then
                            If CallOK(QHY.QHYCamera.SetQHYCCDStreamMode(CamHandle, DB.StreamMode)) = True Then                                      'Set single capture mode
                                If CallOK(QHY.QHYCamera.InitQHYCCD(CamHandle)) = True Then                                                          'Init the camera with the selected mode, ...
                                    'Camera is open
                                    UsedReadMode = DB.ReadOutMode
                                    UsedStreamMode = DB.StreamMode
                                Else
                                    LogError("InitQHYCCD FAILED!")
                                    CamHandle = IntPtr.Zero
                                End If
                            Else
                                LogError("SetQHYCCDStreamMode FAILED!")
                                CamHandle = IntPtr.Zero
                            End If
                        Else
                            LogError("SetQHYCCDReadMode to <" & DB.ReadOutMode & "> FAILED!")
                        End If
                    Else
                        LogError("OpenQHYCCD FAILED!")
                        CamHandle = IntPtr.Zero
                        Return False
                    End If
                Else
                    LogError("Init DLL OK but no camera found!")
                    CamHandle = IntPtr.Zero
                    Return False
                End If
            Else
                LogError("Init QHY did fail!")
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
        Dim ROISize As Integer = CInt(InputBox("Size:", "ROI size", "100")) : ROISize = ROISize \ 2
        If ROISize > 0 Then
            With DB
                .ROI = New Drawing.Rectangle((9600 \ 2) - ROISize, (6422 \ 2) - ROISize, 2 * ROISize, 2 * ROISize)
            End With
        End If
        RefreshProperties()
    End Sub

    Private Sub Load10MicronData()
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

    Private Sub TestSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestSeriesToolStripMenuItem.Click
        DB.StopFlag = False
        DB.StoreImage = True
        DB.AutoOpenImage = False
        DB.ExposureTime = 60
        For Each Mode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            DB.ReadOutMode = Mode
            For Each Filter As eFilter In New eFilter() {eFilter.H_alpha, eFilter.L, eFilter.R, eFilter.G, eFilter.B}
                DB.FilterSlot = Filter
                For Gain As Double = 20 To 100 Step 20
                    DB.Gain = Gain
                    Load10MicronData()
                    RefreshProperties()
                    QHYCapture(DB.FileName, False)
                    If DB.StopFlag = True Then Exit For
                Next Gain
                If DB.StopFlag = True Then Exit For
            Next Filter
            If DB.StopFlag = True Then Exit For
        Next Mode
        CloseCamera()
    End Sub

    Private Sub ExploreCurrentCampaignToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExploreCurrentCampaignToolStripMenuItem.Click
        Dim FolderToOpen As String = System.IO.Path.Combine(DB.MyPath, DB_meta.GUID)
        If System.IO.Directory.Exists(FolderToOpen) = True Then System.Diagnostics.Process.Start(FolderToOpen)
    End Sub

    Private Sub ResetLoopStatisticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetLoopStatisticsToolStripMenuItem.Click
        LoopStat = New AstroNET.Statistics.sStatistics
        LoopStatCount = 0
    End Sub

    Private Sub LoadPositionFrom10MicronToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles tsmiLoad10MicronData.Click
        Load10MicronData()
    End Sub

    Private Sub pgMain_Click(sender As Object, e As EventArgs) Handles pgMain.Click

    End Sub

    Private Sub pgMain_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles pgMain.PropertyValueChanged
        PropertyChanged = True
    End Sub

End Class