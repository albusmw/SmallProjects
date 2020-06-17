﻿Option Explicit On
Option Strict On
Imports System.Windows.Forms

Partial Public Class MainForm

    '''<summary>DB that holds all relevant information.</summary>
    Private DB As New cDB
    '''<summary>DB that holds meta information.</summary>
    Private DB_meta As New cDB_meta
    '''<summary>WCF interface.</summary>
    Private WithEvents DB_ServiceContract As cDB_ServiceContract
    '''<summary>Indicate that a property was changed and parameters need to be updated in the camera.</summary>
    Private PropertyChanged As Boolean = False
    '''<summary>RTF report generator.</summary>
    Private RTFGen As New Atomic.cRTFGenerator
    '''<summary>Focus window.</summary>
    Private FocusWindow As cImgForm = Nothing
    '''<summary>Accumulated statistics.</summary>
    Private LoopStat As AstroNET.Statistics.sStatistics
    '''<summary>Number of single statistics in the accumulated statistics.</summary>
    Private LoopStatCount As Integer = 0

    Private WithEvents ZWOASI As New cZWOASI

    '''<summary>Run a single capture.</summary>
    Private Sub RunCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs)
        QHYCapture(True)
    End Sub

    '''<summary>Command for a QHY capture run.</summary>
    Public Sub QHYCapture(ByVal CloseAtEnd As Boolean)

        Const BitsPerByte As Integer = 8
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
        DB.Stopper.Start()

        'Try to get a suitable camera and continue if found
        If InitQHY(DB.CamToUse) = False Then Log("No suitable camera found!")
        If DB.CamHandle <> IntPtr.Zero Then

            'Get chip properties
            QHY.QHYCamera.GetQHYCCDChipInfo(DB.CamHandle, Chip_Physical.Width, Chip_Physical.Height, Chip_Pixel.Width, Chip_Pixel.Height, Pixel_Size.Width, Pixel_Size.Height, bpp)
            QHY.QHYCamera.GetQHYCCDEffectiveArea(DB.CamHandle, EffArea.X, EffArea.Y, EffArea.Width, EffArea.Height)
            QHY.QHYCamera.GetQHYCCDOverScanArea(DB.CamHandle, OverArea.X, OverArea.Y, OverArea.Width, OverArea.Height)
            DB.Stopper.Stamp("Get chip properties")

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
                LogControlValues()
                DB.Stopper.Stamp("GetQHYCCDParams")

                Log("==============================================================================")
            End If

            Dim ChannelToRead As UInteger = 0

            'Prepare buffers
            LoopStat = New AstroNET.Statistics.sStatistics
            Dim PinHandler As cIntelIPP.cPinHandler
            Dim CamRawBuffer As Byte() = {}
            Dim CamRawBufferPtr As IntPtr = Nothing
            DB.Stopper.Stamp("Prepare buffers")

            'Select filter
            Dim FilterActive As eFilter = eFilter.Invalid
            If DB.FilterSlot <> eFilter.Invalid And DB.UseFilterWheel = True Then
                FilterActive = ActiveFilter(DB.CamHandle, DB.FilterSlot, DB.FilterWheelTimeOut)
            End If
            DB.Stopper.Stamp("Select filter")

            'Enter capture loop
            Dim EndTimeStamps As New List(Of DateTime)
            Dim TotalCaptureTime As Double = 0
            Dim LastCaptureData As New cSingleCaptureData

            For CaptureIdx As UInt32 = 1 To CUInt(DB.CaptureCount)                              '1 extra round for async

                '================================================================================
                ' START EXPOSURE ON FIRST ENTRY
                '================================================================================

                If CaptureIdx = 1 Then
                    LastCaptureData = StartExposure(CaptureIdx, FilterActive, Chip_Pixel)
                End If

                '================================================================================
                ' WAIT FOR END AND READ BUFFERS
                '================================================================================

                IdleExposureTime(DB.ExposureTime)

                'Get the buffer size from the DLL - typically too big but does not care ...
                Dim BytesToTransfer_reported As UInteger = QHY.QHYCamera.GetQHYCCDMemLength(DB.CamHandle)
                LogVerbose("GetQHYCCDMemLength says: " & BytesToTransfer_reported.ValRegIndep.PadLeft(12) & " byte to transfer.")
                If CamRawBuffer.Length <> BytesToTransfer_reported Then
                    PinHandler = New cIntelIPP.cPinHandler
                    ReDim CamRawBuffer(CInt(BytesToTransfer_reported - 1))
                    CamRawBufferPtr = PinHandler.Pin(CamRawBuffer)
                End If
                DB.Stopper.Stamp("GetQHYCCDMemLength & pinning")

                'Read image data from camera - ALWAYS WITH OVERSCAN
                Dim Captured_W As UInteger = 0 : Dim Captured_H As UInteger = 0 : Dim CaptureBits As UInteger = 0
                Dim LiveModePollCount As Integer = 0
                LED_reading(True)
                If DB.StreamMode = eStreamMode.SingleFrame Then
                    CallOK("GetQHYCCDSingleFrame", QHY.QHYCamera.GetQHYCCDSingleFrame(DB.CamHandle, Captured_W, Captured_H, CaptureBits, ChannelToRead, CamRawBufferPtr))
                    LED_capture(False)
                Else
                    Dim LiveModeReady As UInteger = UInteger.MaxValue
                    Do
                        LiveModeReady = QHY.QHYCamera.GetQHYCCDLiveFrame(DB.CamHandle, Captured_W, Captured_H, CaptureBits, ChannelToRead, CamRawBufferPtr)
                        LiveModePollCount += 1
                        DE()
                    Loop Until (LiveModeReady = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS) Or DB.StopFlag = True
                End If
                LED_reading(False)
                LastCaptureData.ObsEnd = Now
                EndTimeStamps.Add(LastCaptureData.ObsEnd)

                Dim BytesToTransfer_calculated As Long = Captured_W * Captured_H * CInt(CaptureBits / BitsPerByte)
                LogVerbose("Calculation says       : " & BytesToTransfer_calculated.ValRegIndep.PadLeft(12) & " byte to transfer.")
                LogVerbose("Loaded image with " & Captured_W.ValRegIndep & "x" & Captured_H.ValRegIndep & " pixel @ " & CaptureBits & " bit resolution")
                DB.Stopper.Stamp("GetQHYCCDSingleFrame (" & LiveModePollCount.ValRegIndep & " x)")

                'Remove overscan - do NOT run if an ROU is set
                Dim SingleStatCalc As New AstroNET.Statistics(DB.IPP)
                SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data = ChangeAspectIPP(DB.IPP, CamRawBuffer, CInt(Captured_W), CInt(Captured_H))      'only convert flat byte buffer to UInt16 matrix data
                If DB.RemoveOverscan = True And DB.ROISet = False Then
                    Dim NoOverscan(CInt(EffArea.Width - 1), CInt(EffArea.Height - 1)) As UInt16
                    Dim Status_GetROI As cIntelIPP.IppStatus = DB.IPP.Copy(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, NoOverscan, CInt(EffArea.X), CInt(EffArea.Y), CInt(EffArea.Width), CInt(EffArea.Height))
                    Dim Status_SetData As cIntelIPP.IppStatus = DB.IPP.Copy(NoOverscan, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, 0, 0, NoOverscan.GetUpperBound(0) + 1, NoOverscan.GetUpperBound(1) + 1)
                    If Status_GetROI <> cIntelIPP.IppStatus.NoErr Or Status_SetData <> cIntelIPP.IppStatus.NoErr Then
                        LogError("Overscan removal FAILED")
                    End If
                End If
                LastCaptureData.NAXIS1 = CUInt(SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS1)
                LastCaptureData.NAXIS2 = CUInt(SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS2)
                DB.Stopper.Stamp("ChangeAspect")

                'Software binning - if > 1 data type is moved from UInt16 to UInt32
                If DB.SoftwareBinning > 1 Then
                    SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data = ImageProcessing.Binning(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, DB.SoftwareBinning)
                    SingleStatCalc.Reset_UInt16()
                End If

                '================================================================================
                'RETRIGGER CAPTURE
                '================================================================================

                Dim SingleCaptureData As cSingleCaptureData = LastCaptureData
                If (CaptureIdx <= DB.CaptureCount) And (DB.StopFlag = False) Then
                    LastCaptureData = StartExposure(CaptureIdx, FilterActive, Chip_Pixel)
                End If

                '================================================================================
                'STATISTICS AND PLOTTING
                '================================================================================

                'FPS calculation
                If EndTimeStamps.Count > 2 Then
                    Dim ThisDuration As Double = (EndTimeStamps(EndTimeStamps.Count - 1) - EndTimeStamps(EndTimeStamps.Count - 2)).TotalSeconds
                    TotalCaptureTime += ThisDuration
                    tsmiFPSIndicator.Text = Format(1 / ThisDuration, "0.0") & " FPS, mean: " & Format(CaptureIdx / TotalCaptureTime, "0.0") & " FPS"
                End If

                'Calculate statistics
                Dim SingleStat As AstroNET.Statistics.sStatistics
                If DB.CalcStatistics = True Then SingleStat = SingleStatCalc.ImageStatistics(SingleStatCalc.DataModeType)
                SingleStat.MonoStatistics_Int.Width = SingleCaptureData.NAXIS1 : SingleStat.MonoStatistics_Int.Height = SingleCaptureData.NAXIS2

                LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat.DataMode, SingleStat, LoopStat) : LoopStatCount += 1
                DB.Stopper.Stamp("Statistics - calc")

                'Display statistics
                Dim DisplaySumStat As Boolean = False
                If DB.Log_ClearStat = True Then RTFGen.Clear()
                If DB.CaptureCount > 1 And DB.Log_ClearStat = True Then DisplaySumStat = True
                Dim SingStat As List(Of String) = SingleStat.StatisticsReport
                Dim TotaStat As List(Of String) = LoopStat.StatisticsReport
                If IsNothing(SingStat) = False Then
                    RTFGen.AddEntry("Capture #" & CaptureIdx.ValRegIndep & " statistics:", Drawing.Color.Black, True, True)
                    For Idx As Integer = 0 To SingStat.Count - 1
                        Dim Line As String = SingStat(Idx)
                        If DisplaySumStat = True Then Line &= "#" & TotaStat(Idx).Substring(AstroNET.Statistics.sSingleChannelStatistics_Int.ReportHeaderLength + 2)
                        RTFGen.AddEntry(Line, Drawing.Color.Black, True, False)
                    Next Idx
                    RTFGen.ForceRefresh()
                    DE()
                End If
                DB.Stopper.Stamp("Statistics - text")

                '================================================================================
                'Plot histogram

                'Set caption
                Dim PlotTitle As New List(Of String)
                PlotTitle.Add(DB.UsedCameraId.ToString)
                PlotTitle.Add(LastCaptureData.ExpTime.ToString.Trim & " s")
                PlotTitle.Add("Gain " & LastCaptureData.Gain.ToString.Trim)
                PlotTitle.Add("Filter " & [Enum].GetName(GetType(eFilter), LastCaptureData.FilterActive))
                PlotTitle.Add("Temperature " & LastCaptureData.ObsStartTemp.ToString.Trim & " °C")
                DB.Plotter.SetCaptions(Join(PlotTitle.ToArray, ", "), "ADU value", "# of pixel")

                Dim CurveMode As cZEDGraphService.eCurveMode = cZEDGraphService.eCurveMode.LinesAndPoints
                Dim CurrentCurveWidth As Integer = 1
                Dim MeanCurveWidth As Integer = 2
                If IsNothing(DB.Plotter) = True Then DB.Plotter = New cZEDGraphService(zgcMain)
                DB.Plotter.Clear()
                If DB.PlotMeanStatistics = True Or DB.PlotSingleStatistics = True Then
                    'Mean statistics
                    If DB.CaptureCount > 1 And LoopStatCount > 1 And DB.PlotMeanStatistics = True Then
                        If IsNothing(LoopStat.BayerHistograms_Int) = False Then
                            DB.Plotter.PlotXvsY("R mean", LoopStat.BayerHistograms_Int(0, 0), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, CurveMode, MeanCurveWidth))
                            DB.Plotter.PlotXvsY("G1 mean", LoopStat.BayerHistograms_Int(0, 1), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, CurveMode, MeanCurveWidth))
                            DB.Plotter.PlotXvsY("G2 mean", LoopStat.BayerHistograms_Int(1, 0), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, CurveMode, MeanCurveWidth))
                            DB.Plotter.PlotXvsY("B mean", LoopStat.BayerHistograms_Int(1, 1), LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, CurveMode, MeanCurveWidth))
                        End If
                        If IsNothing(LoopStat.MonochromHistogram_Int) = False Then
                            DB.Plotter.PlotXvsY("Mono mean", LoopStat.MonochromHistogram_Int, LoopStatCount, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, CurveMode, MeanCurveWidth))
                        End If
                    End If
                    'Current statistics
                    If DB.PlotSingleStatistics = True And IsNothing(SingleStat.BayerHistograms_Int) = False Then
                        DB.Plotter.PlotXvsY("R[0,0]", SingleStat.BayerHistograms_Int(0, 0), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Red, CurveMode, CurrentCurveWidth))
                        DB.Plotter.PlotXvsY("G1[0,1]", SingleStat.BayerHistograms_Int(0, 1), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.LightGreen, CurveMode, CurrentCurveWidth))
                        DB.Plotter.PlotXvsY("G2[1,0]", SingleStat.BayerHistograms_Int(1, 0), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.DarkGreen, CurveMode, CurrentCurveWidth))
                        DB.Plotter.PlotXvsY("B[1,1]", SingleStat.BayerHistograms_Int(1, 1), 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Blue, CurveMode, CurrentCurveWidth))
                        DB.Plotter.PlotXvsY("Mono", SingleStat.MonochromHistogram_Int, 1, New cZEDGraphService.sGraphStyle(System.Drawing.Color.Black, CurveMode, CurrentCurveWidth))
                    End If
                    Select Case DB.PlotLimitMode
                        Case eXAxisScalingMode.Auto
                            DB.Plotter.ManuallyScaleXAxis(LoopStat.MonoStatistics_Int.Min.Key, LoopStat.MonoStatistics_Int.Max.Key)
                        Case eXAxisScalingMode.FullRange16Bit
                            DB.Plotter.ManuallyScaleXAxis(0, 65536)
                        Case eXAxisScalingMode.LeaveAsIs
                            'Just do nothing ...
                    End Select

                    DB.Plotter.AutoScaleYAxisLog()
                    DB.Plotter.GridOnOff(True, True)
                    DB.Plotter.ForceUpdate()
                End If
                DB.Stopper.Stamp("Statistics - plot")

                '================================================================================
                'Display focus image if required
                If DB.ShowLiveImage = True Then
                    Dim NewWindowRequired As Boolean = False
                    If IsNothing(FocusWindow) = True Then
                        NewWindowRequired = True
                    Else
                        If FocusWindow.Hoster.IsDisposed = True Then NewWindowRequired = True
                    End If
                    If NewWindowRequired = True Then
                        FocusWindow = New cImgForm
                        FocusWindow.Show("Focus Window <" & SingleStatCalc.Dimensions & ">")
                    End If
                    Select Case SingleStatCalc.DataMode
                        Case AstroNET.Statistics.eDataMode.UInt16
                            FocusWindow.ShowData(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, SingleStat.MonoStatistics_Int.Min.Key, SingleStat.MonoStatistics_Int.Max.Key)
                        Case AstroNET.Statistics.eDataMode.UInt32
                            FocusWindow.ShowData(SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, SingleStat.MonoStatistics_Int.Min.Key, SingleStat.MonoStatistics_Int.Max.Key)
                    End Select


                    DB.Stopper.Stamp("Focus window")
                End If

                '================================================================================
                'Store image

                If DB.StoreImage = True Then

                    Dim Path As String = System.IO.Path.Combine(DB.StoragePath, DB_meta.GUID)
                    If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)

                    'Compose all FITS keyword entries
                    Dim FileNameToWrite As String = DB.FileName
                    Dim CustomElement As Dictionary(Of eFITSKeywords, Object) = GenerateFITSHeader(SingleCaptureData, Pixel_Size, FileNameToWrite)

                    DB.LastStoredFile = System.IO.Path.Combine(Path, FileNameToWrite & "." & DB.FITSExtension)
                    Select Case SingleStatCalc.DataMode
                        Case AstroNET.Statistics.eDataMode.UInt16
                            cFITSWriter.Write(DB.LastStoredFile, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, cFITSWriter.eBitPix.Int16, CustomElement)
                        Case AstroNET.Statistics.eDataMode.UInt32
                            cFITSWriter.Write(DB.LastStoredFile, SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, cFITSWriter.eBitPix.Int32, CustomElement)
                    End Select
                    If DB.AutoOpenImage = True Then System.Diagnostics.Process.Start(DB.LastStoredFile)

                End If
                DB.Stopper.Stamp("Store image")

                If DB.StopFlag = True Then Exit For

            Next CaptureIdx

            '================================================================================
            'Stop live mode if used

            If DB.StreamMode = eStreamMode.LiveFrame Then
                CallOK("StopQHYCCDLive", QHY.QHYCamera.StopQHYCCDLive(DB.CamHandle))
            End If

            'Release buffer handles
            PinHandler = Nothing

        End If

        'Close camera if selected 
        If CloseAtEnd = True Then CloseCamera()
        DB.Stopper.Stamp("CloseCamera")

        '================================================================================
        'Display timing log

        If DB.Log_Timing = True Then
            Log("--------------------------------------------------------------")
            Log("TIMING:")
            LogNoTime(DB.Stopper.GetLog)
            Log("--------------------------------------------------------------")
        End If

        tsslMain.Text = "--IDLE--"
        DB.RunningFlag = False

    End Sub

    '''<summary>Keep the GUI alive during exposure.</summary>
    '''<param name="ExposureTime">Expected time for the display.</param>
    Private Sub IdleExposureTime(ByVal ExposureTime As Double)
        If ExposureTime > 0.1 Then
            Dim ExpStart As DateTime = Now
            Dim TimePassed As Double = Double.NaN
            tspbProgress.Maximum = CInt(ExposureTime * 10)
            Do
                System.Threading.Thread.Sleep(100)
                TimePassed = (Now - ExpStart).TotalSeconds
                If TimePassed < ExposureTime Then
                    Dim ProgBarVal As Integer = CInt(TimePassed * 10)
                    If ProgBarVal <= tspbProgress.Maximum Then tspbProgress.Value = ProgBarVal
                    tsslProgress.Text = Format(TimePassed, "0.0").Trim & "/" & ExposureTime.ToString.Trim & " seconds exposed"
                Else
                    tspbProgress.Value = 0
                    tsslProgress.Text = "---"
                End If
                DE()
            Loop Until TimePassed >= ExposureTime
        End If
    End Sub

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
        Dim RetVal As New List(Of String)
        For Each Entry As UInteger In Digits
            RetVal.Add(Entry.ValRegIndep)
        Next Entry
        Return Join(RetVal.ToArray, ".")
    End Function

    '===============================================================================================
    ' Logging and error handling
    '===============================================================================================

    Private Function CallOK(ByVal ErrorCode As ZWO.ASICameraDll.ASI_ERROR_CODE) As Boolean
        If ErrorCode = ZWO.ASICameraDll.ASI_ERROR_CODE.ASI_SUCCESS Then
            Return True
        Else
            LogError("ZWO ASI ERROR: <" & ErrorCode.ToString.Trim & "> #####")
            Return False
        End If
    End Function

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
        Log(Text & ": " & Ticker.ElapsedMilliseconds.ValRegIndep & " ms")
    End Sub

    Private Sub LogError(ByVal Text As String)
        Log("########### " & Text & " ###########")
    End Sub

    Private Sub LogVerbose(ByVal Text As String)
        If DB.Log_Verbose = True Then Log(Text)
    End Sub

    Private Sub LogVerbose(ByVal Text As List(Of String))
        For Each Line As String In Text
            LogVerbose(Line)
        Next Line
    End Sub

    Private Sub Log(ByVal Text As List(Of String))
        For Each Line As String In Text
            Line = Format(Now, "HH.mm.ss:fff") & "|" & Line
            If DB.Log_Generic.Length = 0 Then
                DB.Log_Generic.Append(Text)
            Else
                DB.Log_Generic.Append(System.Environment.NewLine & Line)
            End If
        Next Line
        DisplayLog()
    End Sub

    '''<summary>Append list of text but do not add time (e.g. as this is a time-stamp log).</summary>
    '''<param name="Text"></param>
    Private Sub LogNoTime(ByVal Text As List(Of String))
        For Each Line As String In Text
            If DB.Log_Generic.Length = 0 Then
                DB.Log_Generic.Append(Text)
            Else
                DB.Log_Generic.Append(System.Environment.NewLine & Line)
            End If
        Next Line
        DisplayLog()
    End Sub

    Private Sub LogStart(ByVal Text As String)
        Log(Text)
    End Sub

    Private Sub Log(ByVal Text As String)
        Text = Format(Now, "HH.mm.ss:fff") & "|" & Text
        If DB.Log_Generic.Length = 0 Then
            DB.Log_Generic.Append(Text)
        Else
            DB.Log_Generic.Append(System.Environment.NewLine & Text)
        End If
        DisplayLog()
    End Sub

    Private Sub DisplayLog()
        With tbLogOutput
            .Text = DB.Log_Generic.ToString
            If tbLogOutput.Text.Length > 0 Then
                .SelectionStart = .Text.Length - 1
                .SelectionLength = 0
            End If
            .ScrollToCaret()
        End With
        DE()
    End Sub

    Private Sub EndAction()
        Log("=========================================")
        tsslMain.Text = "--IDLE--"
        DE()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub ExplorerHereToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExplorerHereToolStripMenuItem.Click
        Diagnostics.Process.Start(DB.EXEPath)
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        tsmiNewGUID_Click(Nothing, Nothing)

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
        Dim IPPLoadError As String = String.Empty
        Dim IPPPathToUse As String = cIntelIPP.SearchDLLToUse(cIntelIPP.PossiblePaths(DB.EXEPath).ToArray, IPPLoadError)
        If String.IsNullOrEmpty(IPPLoadError) = True Then
            DB.IPP = New cIntelIPP(IPPPathToUse)
            cFITSWriter.UseIPPForWriting = True
        Else
            cFITSWriter.UseIPPForWriting = False
        End If
        cFITSWriter.IPPPath = DB.IPP.IPPPath

        'Start WCF
        'netsh http add urlacl url=http://+:1250/ user=DESKTOP-I7\albusmw
        DB_ServiceContract = New cDB_ServiceContract(DB, DB_meta)
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

        'Show DB
        pgMain.SelectedObject = DB

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub tSetTemp_Tick(sender As Object, e As EventArgs) Handles tSetTemp.Tick
        If DB.CamHandle <> IntPtr.Zero Then
            QHY.QHYCamera.ControlQHYCCDTemp(DB.CamHandle, DB.TargetTemp)
        End If
    End Sub

    Private Sub tsbCapture_Click(sender As Object, e As EventArgs) Handles tsbCapture.Click
        If CType(sender, System.Windows.Forms.ToolStripButton).Enabled = True Then QHYCapture(True)
    End Sub

    Private Sub tsbStopCapture_Click(sender As Object, e As EventArgs) Handles tsbStopCapture.Click
        DB.StopFlag = True
    End Sub

    Private Sub AllReadoutModesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllReadoutModesToolStripMenuItem.Click
        DB.StopFlag = False
        For Each Mode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            DB.ReadOutMode = Mode
            RefreshProperties()
            QHYCapture(False)
            If DB.StopFlag = True Then Exit For
        Next Mode
        CloseCamera()
    End Sub

    Private Sub ExposureTimeSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExposureTimeSeriesToolStripMenuItem.Click
        DB.StopFlag = False
        'For lust of all exposure times
        Dim AllExposureTimes As New List(Of Double)
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
            QHYCapture(False)
            If DB.StopFlag = True Then Exit For
        Next Exposure
        CloseCamera()
    End Sub

    Private Sub GainVariationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GainVariationToolStripMenuItem.Click
        DB.StopFlag = False
        For Gain As Double = 0 To 200 Step 1
            DB.Gain = Gain
            For Exp As Integer = 1 To 60 Step 1
                DB.ExposureTime = Exp
                RefreshProperties()
                QHYCapture(False)
            Next Exp
            If DB.StopFlag = True Then Exit For
        Next Gain
        CloseCamera()
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
            .ROI = New Drawing.Rectangle(100, 100, 100, 100)
            .CaptureCount = Int32.MaxValue
            .StoreImage = False
            .Log_ClearStat = True
            .DDR_RAM = False
            .ConfigAlways = False
            .FilterSlot = eFilter.Invalid
        End With
        RefreshProperties()
    End Sub

    Private Sub FilterSelectionToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim Stopper As New cStopper
        If InitQHY(DB.CamToUse) = False Then
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
        Dim FoundPorts As New Dictionary(Of String, String)
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
        Dim FinalList As New Dictionary(Of String, String)
        For Each Entry As String In FoundPorts.Keys
            If FoundPorts(Entry).Length > 0 Then FinalList.Add(Entry, FoundPorts(Entry))
        Next
        MsgBox(FoundPorts.Count.ToString.Trim & " ports found!")
    End Sub

    Private Sub TestSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs)
        DB.StopFlag = False
        DB.StoreImage = True
        DB.AutoOpenImage = False
        DB.ExposureTime = 60
        For Each ReadOutMode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            If ReadOutMode <> eReadOutMode.Invalid Then
                DB.ReadOutMode = ReadOutMode
                For Each Filter As eFilter In New eFilter() {eFilter.H_alpha}
                    DB.FilterSlot = Filter
                    For Gain As Double = 0 To 200 Step 1
                        DB.Gain = Gain
                        Load10MicronData()
                        RefreshProperties()
                        QHYCapture(False)
                        If DB.StopFlag = True Then Exit For
                    Next Gain
                    If DB.StopFlag = True Then Exit For
                Next Filter
                If DB.StopFlag = True Then Exit For
            End If
        Next ReadOutMode
        CloseCamera()
    End Sub

    Private Sub ExploreCurrentCampaignToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExploreCurrentCampaignToolStripMenuItem.Click
        Dim FolderToOpen As String = System.IO.Path.Combine(DB.StoragePath, DB_meta.GUID)
        If System.IO.Directory.Exists(FolderToOpen) = True Then System.Diagnostics.Process.Start(FolderToOpen)
    End Sub

    Private Sub tsmiResetLoopStat_Click(sender As Object, e As EventArgs) Handles tsmiResetLoopStat.Click
        LoopStat = New AstroNET.Statistics.sStatistics
        LoopStatCount = 0
    End Sub

    Private Sub pgMain_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles pgMain.PropertyValueChanged
        PropertyChanged = True
    End Sub

    Private Sub StoreStatisticsAsEXCELFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StoreStatisticsAsEXCELFileToolStripMenuItem.Click

        Dim AddHisto As Boolean = True

        With sfdMain
            .Filter = "EXCEL file (*.xlsx)|*.xlsx"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With

        Using workbook As New ClosedXML.Excel.XLWorkbook

            '1.) Histogram
            If AddHisto = True Then
                Dim XY As New List(Of Object())
                For Each Key As Long In LoopStat.MonochromHistogram_Int.Keys
                    Dim Values As New List(Of Object)
                    Values.Add(Key)
                    Values.Add(LoopStat.MonochromHistogram_Int(Key))
                    If LoopStat.BayerHistograms_Int(0, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(0, 0)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms_Int(0, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(0, 1)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms_Int(1, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(1, 0)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms_Int(1, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(1, 1)(Key)) Else Values.Add(String.Empty)
                    XY.Add(Values.ToArray)
                Next Key
                Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram")
                worksheet.Cell(1, 1).InsertData(New List(Of String)({"Pixel value", "Count Mono", "Count Bayer_0_0", "Count Bayer_0_1", "Count Bayer_1_0", "Count Bayer_1_1"}), True)
                worksheet.Cell(2, 1).InsertData(XY)
                For Each col In worksheet.ColumnsUsed
                    col.AdjustToContents()
                Next col
            End If

            '2.) Histo density
            Dim HistDens As New List(Of Object())
            For Each Key As UInteger In LoopStat.MonoStatistics_Int.HistXDist.Keys
                HistDens.Add(New Object() {Key, LoopStat.MonoStatistics_Int.HistXDist(Key)})
            Next Key
            Dim worksheet2 As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram Density")
            worksheet2.Cell(1, 1).InsertData(New List(Of String)({"Step size", "Count"}), True)
            worksheet2.Cell(2, 1).InsertData(HistDens)
            For Each col In worksheet2.ColumnsUsed
                col.AdjustToContents()
            Next col

            '4) Save and open
            Dim FileToGenerate As String = IO.Path.Combine(DB.StoragePath, sfdMain.FileName)
            workbook.SaveAs(FileToGenerate)

        End Using

    End Sub

    Private Sub tsmiNewGUID_Click(sender As Object, e As EventArgs) Handles tsmiNewGUID.Click
        DB_meta.GUID = Format(Now, "yyyy_MM_dd_HH_mm_ss")
        RefreshProperties()
    End Sub

    Private Sub tsmiLoad10MicronData_Click(sender As Object, e As EventArgs) Handles tsmiLoad10MicronData.Click
        Load10MicronData()
    End Sub

    Private Sub RunXMLSequenceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunXMLSequenceToolStripMenuItem.Click
        With ofdMain
            .Filter = "XML definitions (*.qhycapture.xml)|*.qhycapture.xml"
            .Multiselect = False
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With
        RunXMLSequence(ofdMain.FileName)
    End Sub

    Private Sub tsmiASIZWO_Click(sender As Object, e As EventArgs) Handles tsmiASIZWO.Click

        'Get number of cameras, if there is at least one continue
        Dim Cameras As Integer = ZWO.ASICameraDll.ASIGetNumOfConnectedCameras()

        Const DoNotSet As Integer = Integer.MinValue

        If Cameras > 0 Then

            Dim CamHandle As Integer = 0
            Log("Opening first camera ...")

            Dim CameraInfo As ZWO.ASICameraDll.ASI_CAMERA_INFO = Nothing
            Dim CameraID As ZWO.ASICameraDll.ASI_ID = Nothing ': CameraID.ID = New Byte() {Asc("M"), Asc("A"), Asc("R"), Asc("T"), Asc("I"), Asc("N"), Asc("_"), Asc("W")}
            Dim NumberOfControls As Integer = 0

            CallOK(ZWO.ASICameraDll.ASIOpenCamera(CamHandle))
            CallOK(ZWO.ASICameraDll.ASIGetCameraProperty(CameraInfo, CamHandle))

            'Get (and set before) the camera ID - this should only be done once for each camera ...
            'CallOK(ZWO.ASICameraDll.ASISetID(CamHandle, CameraID))
            CallOK(ZWO.ASICameraDll.ASIGetID(CamHandle, CameraID))

            'Display all camera info elements
            Log("Camera Info for <" & CameraID.IDAsString & ">:")
            Log("  " & "Name".PadRight(20) & ": " & CameraInfo.NameAsString)
            Log("  " & "CameraID".PadRight(20) & ": " & CameraInfo.CameraID)
            Log("  " & "MaxHeight".PadRight(20) & ": " & CameraInfo.MaxHeight)
            Log("  " & "MaxWidth".PadRight(20) & ": " & CameraInfo.MaxWidth)
            Log("  " & "IsColorCam".PadRight(20) & ": " & CameraInfo.IsColorCam)
            Log("  " & "BayerPattern".PadRight(20) & ": " & CameraInfo.BayerPattern)
            Log("  " & "SupportedBins".PadRight(20) & ": " & ZWOASI.SupportedBins(CameraInfo.SupportedBins))
            Log("  " & "SupportedVideoFormat".PadRight(20) & ": " & ZWOASI.SupportedVideoFormat(CameraInfo.SupportedVideoFormat))
            Log("  " & "PixelSize".PadRight(20) & ": " & CameraInfo.PixelSize)
            Log("  " & "MechanicalShutter".PadRight(20) & ": " & CameraInfo.MechanicalShutter)
            Log("  " & "ST4Port".PadRight(20) & ": " & CameraInfo.ST4Port)
            Log("  " & "IsCoolerCam".PadRight(20) & ": " & CameraInfo.IsCoolerCam)
            Log("  " & "IsUSB3Host".PadRight(20) & ": " & CameraInfo.IsUSB3Host)
            Log("  " & "IsUSB3Camera".PadRight(20) & ": " & CameraInfo.IsUSB3Camera)
            Log("  " & "ElecPerADU".PadRight(20) & ": " & CameraInfo.ElecPerADU)
            Log("  " & "BitDepth".PadRight(20) & ": " & CameraInfo.BitDepth)
            Log("  " & "IsTriggerCam".PadRight(20) & ": " & CameraInfo.IsTriggerCam)
            Log("=======================================")

            'Open camera
            CallOK(ZWO.ASICameraDll.ASIInitCamera(CamHandle))

            'Get different number of controls
            ZWO.ASICameraDll.ASIGetNumOfControls(0, NumberOfControls)
            Log(NumberOfControls.ValRegIndep & " controls")

            'Read out every control that are available and set to default
            Log("ControlCaps:")
            For ControlIdx As Integer = 0 To NumberOfControls - 1
                Dim ControlCap As ZWO.ASICameraDll.ASI_CONTROL_CAPS = Nothing
                ZWO.ASICameraDll.ASIGetControlCaps(CamHandle, ControlIdx, ControlCap)
                Log("  " & ControlCap.NameAsString.PadRight(30) & ": " & TabFormat(ControlCap.MinValue) & " ... " & TabFormat(ControlCap.MaxValue) & ",default: " & TabFormat(ControlCap.DefaultValue) & "(" & ControlCap.DescriptionAsString & ")")
                ZWO.ASICameraDll.ASISetControlValue(CamHandle, ControlCap.ControlType, ControlCap.DefaultValue)
            Next ControlIdx
            Log("=======================================")

            'Read out all set values
            Log("ControlValues:")
            For Each X As ZWO.ASICameraDll.ASI_CONTROL_TYPE In [Enum].GetValues(GetType(ZWO.ASICameraDll.ASI_CONTROL_TYPE))
                Log("  " & X.ToString.Trim.PadRight(30) & ": " & ZWO.ASICameraDll.ASIGetControlValue(CamHandle, X))
            Next X
            Log("=======================================")

            'Read out all set values
            Log("SpecialValues:")
            Dim Offset_HighestDR As Integer : Dim Offset_UnityGain As Integer : Dim Gain_LowestRN As Integer : Dim Offset_LowestRN As Integer
            ZWO.ASICameraDll.ASIGetGainOffset(CamHandle, Offset_HighestDR, Offset_UnityGain, Gain_LowestRN, Offset_LowestRN)
            Log("  " & "Offset configuration:")
            Log("  " & "  HighestDR         : " & Offset_HighestDR.ValRegIndep)
            Log("  " & "  UnityGain         : " & Offset_UnityGain.ValRegIndep)
            Log("  " & "  LowestRN          : " & Offset_LowestRN.ValRegIndep)
            Log("  " & "    @               : " & Gain_LowestRN.ValRegIndep)
            Log("=======================================")

            'Run exposure sequence configured
            Log("Exposing ...")
            Dim ROIWidth As Integer = -1
            Dim ROIHeight As Integer = -1
            Dim ROIBin As Integer = -1
            Dim ROIImgType As ZWO.ASICameraDll.ASI_IMG_TYPE = ZWO.ASICameraDll.ASI_IMG_TYPE.ASI_IMG_END
            Dim StartPosX As Integer = -1
            Dim StartPosY As Integer = -1

            CallOK(ZWO.ASICameraDll.ASISetROIFormat(CamHandle, CameraInfo.MaxWidth, CameraInfo.MaxHeight, 1, ZWO.ASICameraDll.ASI_IMG_TYPE.ASI_IMG_RAW16))
            CallOK(ZWO.ASICameraDll.ASIGetROIFormat(CamHandle, ROIWidth, ROIHeight, ROIBin, ROIImgType))
            CallOK(ZWO.ASICameraDll.ASISetStartPos(CamHandle, 0, 0))
            CallOK(ZWO.ASICameraDll.ASIGetStartPos(CamHandle, StartPosX, StartPosY))

            'Prepare logging
            Dim Path As String = System.IO.Path.Combine(DB.StoragePath, DB_meta.GUID)
            If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)
            Dim CSVLogPath As String = System.IO.Path.Combine(Path, "ZWO_Statisic_Log.csv")
            System.IO.File.WriteAllText(System.IO.Path.Combine(Path, "InitialLog.log"), tbLogOutput.Text)

            'Prepare buffers
            Dim CamRawBuffer((CameraInfo.MaxWidth * CameraInfo.MaxHeight * 2) - 1) As Byte
            Dim CamRawGAC As Runtime.InteropServices.GCHandle = Runtime.InteropServices.GCHandle.Alloc(CamRawBuffer, Runtime.InteropServices.GCHandleType.Pinned)
            Dim CamRawPtr As IntPtr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(CamRawBuffer, 0)
            Dim CamRawBufferBytes As Integer = CInt(CamRawBuffer.LongLength * 2)

            'Prepare statistics
            Dim SingleStatCalc As New AstroNET.Statistics(DB.IPP)
            Dim ExpCounter As Integer = 0
            Dim SweepCount As Integer = 10

            Dim CSVLog As New Ato.cCSVBuilder

            For Each TargetTemp As Integer In New Integer() {DoNotSet}

                'Cooling
                If TargetTemp <> Integer.MinValue Then ZWOASI.CoolASICamera(CamHandle, TargetTemp, 0.2, 120)

                For Each ExpTimeToSet As Integer In New Integer() {1}

                    CallOK(ZWO.ASICameraDll.ASISetControlValue(CamHandle, ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_EXPOSURE, ExpTimeToSet))

                    For Each GainToSet As Integer In New Integer() {1}

                        If GainToSet <> DoNotSet Then CallOK(ZWO.ASICameraDll.ASISetControlValue(CamHandle, ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_GAIN, GainToSet))

                        For Each GammaToSet As Integer In New Integer() {50}

                            If GammaToSet <> DoNotSet Then CallOK(ZWO.ASICameraDll.ASISetControlValue(CamHandle, ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_GAMMA, GammaToSet))

                            For Each BrightnessToSet As Integer In New Integer() {50}

                                If BrightnessToSet <> DoNotSet Then CallOK(ZWO.ASICameraDll.ASISetControlValue(CamHandle, ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_BRIGHTNESS, BrightnessToSet))

                                'Configure all control values
                                ZWO.ASICameraDll.ASISetControlValue(CamHandle, ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_WB_B, 0)
                                ZWO.ASICameraDll.ASISetControlValue(CamHandle, ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_WB_R, 0)
                                ZWO.ASICameraDll.ASISetControlValue(CamHandle, ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_MONO_BIN, 1)

                                'Compose logging info
                                Dim LogInfo As String = "T=" & (TargetTemp / 10).ValRegIndep & "|Exp=" & (ExpTimeToSet / 1000) & "ms|Gain=" & GainToSet.ValRegIndep & "|Offset=" & BrightnessToSet.ValRegIndep & "|Gamma=" & GammaToSet.ValRegIndep

                                Dim LoopStat As New AstroNET.Statistics.sStatistics

                                For LoopCnt As Integer = 1 To SweepCount

                                    'Log current parameters
                                    ExpCounter += 1

                                    CSVLog.StartRow()
                                    CSVLog.AddColumnValue("ExpCounter", ExpCounter)
                                    For Each X As ZWO.ASICameraDll.ASI_CONTROL_TYPE In [Enum].GetValues(GetType(ZWO.ASICameraDll.ASI_CONTROL_TYPE))
                                        Dim ColumnName As String = X.ToString.Replace("ASI_", String.Empty)
                                        Select Case X
                                            Case ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_EXPOSURE
                                                CSVLog.AddColumnValue(ColumnName, ZWO.ASICameraDll.ASIGetControlValue(CamHandle, X) / 1000000)      '[s]
                                            Case ZWO.ASICameraDll.ASI_CONTROL_TYPE.ASI_TEMPERATURE
                                                CSVLog.AddColumnValue(ColumnName, ZWO.ASICameraDll.ASIGetControlValue(CamHandle, X) / 10)          '[°C]
                                            Case Else
                                                CSVLog.AddColumnValue(ColumnName, ZWO.ASICameraDll.ASIGetControlValue(CamHandle, X))
                                        End Select
                                    Next X

                                    'Start test exposure
                                    Log("Exposure " & LoopCnt.ValRegIndep & "/" & SweepCount & ":" & LogInfo)
                                    Dim Ticker As New Stopwatch : Ticker.Reset() : Ticker.Start()
                                    CallOK(ZWO.ASICameraDll.ASIStartExposure(CamHandle, ZWO.ASICameraDll.ASI_BOOL.ASI_FALSE))
                                    Dim ExpStatus As ZWO.ASICameraDll.ASI_EXPOSURE_STATUS = ZWO.ASICameraDll.ASI_EXPOSURE_STATUS.ASI_EXP_FAILED
                                    Dim ExpFailedCount As Integer = 0
                                    Do
                                        System.Threading.Thread.Sleep(1)
                                        'Application.DoEvents()

                                        If CallOK(ZWO.ASICameraDll.ASIGetExpStatus(CamHandle, ExpStatus)) = False Then
                                            Exit Do
                                        Else
                                            Select Case ExpStatus
                                                Case ZWO.ASICameraDll.ASI_EXPOSURE_STATUS.ASI_EXP_FAILED
                                                    'Restart exposure
                                                    ExpFailedCount += 1
                                                    ZWO.ASICameraDll.ASIStopExposure(CamHandle)
                                                    ZWO.ASICameraDll.ASIStartExposure(CamHandle, ZWO.ASICameraDll.ASI_BOOL.ASI_FALSE)
                                                    Log("###### EXPOSING FAILED! ######")
                                                Case ZWO.ASICameraDll.ASI_EXPOSURE_STATUS.ASI_EXP_IDLE
                                                    Log("Exposure idle, existing ...")
                                                Case ZWO.ASICameraDll.ASI_EXPOSURE_STATUS.ASI_EXP_WORKING
                                                    'Still working ...
                                                Case ZWO.ASICameraDll.ASI_EXPOSURE_STATUS.ASI_EXP_SUCCESS
                                                    Exit Do
                                            End Select
                                        End If
                                    Loop Until 1 = 0
                                    Ticker.Stop()
                                    LogTiming("Exposure duration", Ticker)

                                    'Get data - due to some unknown reason the buffer must be "X times bigger" compared to the expected size; X = 3 works ...
                                    If ExpStatus = ZWO.ASICameraDll.ASI_EXPOSURE_STATUS.ASI_EXP_SUCCESS Then

                                        'Read data
                                        Log("Reading data ...")
                                        Ticker.Reset() : Ticker.Start()
                                        CallOK(ZWO.ASICameraDll.ASIGetDataAfterExp(CamHandle, CamRawPtr, CamRawBufferBytes))
                                        Ticker.Stop()
                                        LogTiming("", Ticker)

                                        'Correct aspect and calculate statistics
                                        Log("Statistics ...")
                                        Ticker.Reset() : Ticker.Start()
                                        SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data = ChangeAspectIPP(DB.IPP, CamRawBuffer, CameraInfo.MaxWidth, CameraInfo.MaxHeight)
                                        Dim SingleStat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics(SingleStat.DataMode)
                                        LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat.DataMode, SingleStat, LoopStat)
                                        Ticker.Stop()
                                        LogTiming("", Ticker)

                                        'Plot histogram
                                        DB.Plotter.Clear()
                                        DB.Plotter.PlotXvsY("R", LoopStat.BayerHistograms_Int(0, 0), New cZEDGraphService.sGraphStyle(Color.Red, 1))
                                        DB.Plotter.PlotXvsY("G1", LoopStat.BayerHistograms_Int(0, 1), New cZEDGraphService.sGraphStyle(Color.LightGreen, 1))
                                        DB.Plotter.PlotXvsY("G2", LoopStat.BayerHistograms_Int(1, 0), New cZEDGraphService.sGraphStyle(Color.DarkGreen, 1))
                                        DB.Plotter.PlotXvsY("B", LoopStat.BayerHistograms_Int(1, 1), New cZEDGraphService.sGraphStyle(Color.Blue, 1))
                                        DB.Plotter.PlotXvsY("Mono histo", LoopStat.MonochromHistogram_Int, New cZEDGraphService.sGraphStyle(Color.Black, 1))
                                        DB.Plotter.ManuallyScaleXAxis(LoopStat.MonoStatistics_Int.Min.Key, LoopStat.MonoStatistics_Int.Max.Key)

                                        DB.Plotter.AutoScaleYAxisLog()
                                        DB.Plotter.GridOnOff(True, True)
                                        DB.Plotter.ForceUpdate()

                                        'Write image data
                                        If DB.StoreImage = True Then
                                            Log("Storing ...")
                                            Ticker.Reset() : Ticker.Start()
                                            Dim FITSName As String = System.IO.Path.Combine(Path, "EXP_" & Format(ExpCounter, "000000000").Trim & ".fits")
                                            cFITSWriter.Write(FITSName, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, cFITSWriter.eBitPix.Int16)
                                            Ticker.Stop()
                                            LogTiming("", Ticker)
                                            'Process.Start(FITSName)
                                        End If

                                        'Log and display statistics
                                        CSVLog.AddColumnValue("ExpFailedCount", ExpFailedCount.ValRegIndep)
                                        CSVLog.AddColumnValue("Mean", LoopStat.MonoStatistics_Int.Mean)
                                        CSVLog.AddColumnValue("Median", LoopStat.MonoStatistics_Int.Median)
                                        CSVLog.AddColumnValue("Min", LoopStat.MonoStatistics_Int.Min.Key)
                                        CSVLog.AddColumnValue("Max", LoopStat.MonoStatistics_Int.Max.Key)
                                        CSVLog.AddColumnValue("StdDev", LoopStat.MonoStatistics_Int.StdDev)
                                        CSVLog.AddColumnValue("DifferentADUValues", LoopStat.MonoStatistics_Int.DifferentADUValues)
                                        For Each Perc As Byte In LoopStat.MonoStatistics_Int.Percentile.Keys
                                            CSVLog.AddColumnValue("Perc_" & Perc.ValRegIndep, LoopStat.MonoStatistics_Int.Percentile(Perc))
                                        Next Perc

                                    End If

                                    CSVLog.AddColumnValue("Timing", Ticker.ElapsedMilliseconds)
                                    CSVLog.AddColumnValue("ExpStatus", ExpStatus.ToString.Trim)

                                    System.IO.File.WriteAllText(CSVLogPath, CSVLog.CreateCSV)

                                Next LoopCnt

                            Next BrightnessToSet

                        Next GammaToSet

                    Next GainToSet

                Next ExpTimeToSet

            Next TargetTemp

            'Release buffers
            CamRawGAC.Free()

            'Close camera
            Log("Closing camera ...")
            CallOK(ZWO.ASICameraDll.ASICloseCamera(CamHandle))
            GC.Collect()
            Log("=======================================")

        End If

    End Sub

    Private Function TabFormat(ByVal Text As Integer) As String
        Return Text.ValRegIndep.PadLeft(10)
    End Function

    Private Sub FITSWriterWithKeywordsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FITSWriterWithKeywordsToolStripMenuItem.Click
        cFITSWriter.WriteTestFile_Float32("C:\TEMP\FITSHeader.fits")
    End Sub

    Private Sub tStatusUpdate_Tick(sender As Object, e As EventArgs) Handles tStatusUpdate.Tick
        tsslMemory.Text = "Memory: " & Format(GetMyMemSize, "0.0") & " MByte"
    End Sub

    '''<summary>Get the memory consumption [MByte] of this EXE.</summary>
    '''<remarks>This functions needs a significant time !!!!!</remarks>
    Private Function GetMyMemSize() As Double
        Return Process.GetCurrentProcess.PrivateMemorySize64 / 1048576
    End Function

    Private Sub BiasCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiSpeedTest.Click
        With DB
            .StreamMode = eStreamMode.LiveFrame
            .ExposureTime = 0.0001
            .Gain = 0
            .CaptureCount = 50
            .StoreImage = False
            .Log_ClearStat = True
            .DDR_RAM = False
            .ConfigAlways = False
            .FilterSlot = eFilter.Invalid
            .CalcStatistics = False
            .PlotSingleStatistics = False
            .PlotMeanStatistics = False
            .RemoveOverscan = False
        End With
        RefreshProperties()
    End Sub

    Private Sub OpenLastStoredFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLastStoredFileToolStripMenuItem.Click
        If System.IO.File.Exists(DB.LastStoredFile) Then Process.Start(DB.LastStoredFile)
    End Sub

    Private Sub tsmiClearLog_Click(sender As Object, e As EventArgs) Handles tsmiClearLog.Click
        DB.Log_Generic.Clear()
        DisplayLog()
    End Sub

End Class