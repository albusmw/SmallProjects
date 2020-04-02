Option Explicit On
Option Strict On
Imports System.Windows.Forms

Partial Public Class MainForm

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
        DB.Stopper.Start()

        'Try to get a suitable camera and continue if found
        If InitQHY(DB.CamToUse) = False Then Log("No suitable camera found!")
        If CamHandle <> IntPtr.Zero Then

            'Get chip properties
            QHY.QHYCamera.GetQHYCCDChipInfo(CamHandle, Chip_Physical.Width, Chip_Physical.Height, Chip_Pixel.Width, Chip_Pixel.Height, Pixel_Size.Width, Pixel_Size.Height, bpp)
            QHY.QHYCamera.GetQHYCCDEffectiveArea(CamHandle, EffArea.X, EffArea.Y, EffArea.Width, EffArea.Height)
            QHY.QHYCamera.GetQHYCCDOverScanArea(CamHandle, OverArea.X, OverArea.Y, OverArea.Width, OverArea.Height)
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
                LogControlValues
                DB.Stopper.Stamp("GetQHYCCDParams")

                Log("==============================================================================")
            End If

            Dim ReadResolution As UInteger = 16
            Dim ChannelToRead As UInteger = 0

            'Prepare buffers
            LoopStat = New AstroNET.Statistics.sStatistics
            Dim PinHandler As cIntelIPP.cPinHandler
            Dim CamRawBuffer As Byte() = {}
            Dim CamRawBufferPtr As IntPtr = Nothing
            DB.Stopper.Stamp("Prepare buffers")

            'Select filter
            Dim FilterActive As eFilter = eFilter.Invalid
            If DB.FilterSlot <> eFilter.Invalid Then
                FilterActive = ActiveFilter(CamHandle, DB.FilterSlot, 15)
            End If
            DB.Stopper.Stamp("Select filter")

            'Enter capture loop
            Dim LastCaptureEnd As DateTime = DateTime.MinValue                          'last capture end to calculate a FPS number
            Dim TotalCaptureTime As Double = 0
            For CaptureIdx As UInt32 = 1 To DB.CaptureCount

                '================================================================================
                ' START EXPOSURE
                '================================================================================

                Dim SingleCaptureData As cSingleCaptureData = StartExposure(CaptureIdx, FilterActive, Chip_Pixel)

                '================================================================================
                ' WAIT FOR END AND READ BUFFERS
                '================================================================================

                IdleExposureTime(DB.ExposureTime)

                'Get the buffer size from the DLL - typically too big but does not care ...
                Dim BytesToTransfer_reported As UInteger = QHY.QHYCamera.GetQHYCCDMemLength(CamHandle)
                LogVerbose("GetQHYCCDMemLength says: " & BytesToTransfer_reported.ValRegIndep.PadLeft(12) & " byte to transfer.")
                If CamRawBuffer.Length <> BytesToTransfer_reported Then
                    PinHandler = New cIntelIPP.cPinHandler
                    ReDim CamRawBuffer(CInt(BytesToTransfer_reported - 1))
                    CamRawBufferPtr = PinHandler.Pin(CamRawBuffer)
                End If
                DB.Stopper.Stamp("GetQHYCCDMemLength & pinning")

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
                DB.Stopper.Stamp("GetQHYCCDSingleFrame")

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
                DB.Stopper.Stamp("ChangeAspect")

                '================================================================================
                'STATISTICS AND PLOTTING
                '================================================================================

                Dim SingleStat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics
                LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat, LoopStat) : LoopStatCount += 1
                DB.Stopper.Stamp("Statistics - calc")

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
                DB.Stopper.Stamp("Statistics - text")

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
                DB.Stopper.Stamp("Statistics - plot")

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
                    DB.Stopper.Stamp("Focus window")
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
                DB.Stopper.Stamp("Store image")

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
        DB.Stopper.Stamp("CloseCamera")

        '================================================================================
        'Display timing log

        If DB.Log_Timing = True Then
            Log("--------------------------------------------------------------")
            Log("TIMING:")
            Log(DB.Stopper.GetLog)
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

    '

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

    Private Sub StoreStatisticsAsEXCELFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StoreStatisticsAsEXCELFileToolStripMenuItem.Click

        Dim AddHisto As Boolean = True

        With sfdMain
            .Filter = "EXCEL file (*.xlsx)|*.xlsx"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With

        Using workbook As New ClosedXML.Excel.XLWorkbook

            '1.) Histogram
            If AddHisto = True Then
                Dim XY As New Collections.Generic.List(Of Object())
                For Each Key As Long In LoopStat.MonochromHistogram.Keys
                    Dim Values As New Collections.Generic.List(Of Object)
                    Values.Add(Key)
                    Values.Add(LoopStat.MonochromHistogram(Key))
                    If LoopStat.BayerHistograms(0, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms(0, 0)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms(0, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms(0, 1)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms(1, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms(1, 0)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms(1, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms(1, 1)(Key)) Else Values.Add(String.Empty)
                    XY.Add(Values.ToArray)
                Next Key
                Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram")
                worksheet.Cell(1, 1).InsertData(New Collections.Generic.List(Of String)({"Pixel value", "Count Mono", "Count Bayer_0_0", "Count Bayer_0_1", "Count Bayer_1_0", "Count Bayer_1_1"}), True)
                worksheet.Cell(2, 1).InsertData(XY)
                For Each col In worksheet.ColumnsUsed
                    col.AdjustToContents()
                Next col
            End If

            '2.) Histo density
            Dim HistDens As New Collections.Generic.List(Of Object())
            For Each Key As UInteger In LoopStat.MonoStatistics.HistXDist.Keys
                HistDens.Add(New Object() {Key, LoopStat.MonoStatistics.HistXDist(Key)})
            Next Key
            Dim worksheet2 As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram Density")
            worksheet2.Cell(1, 1).InsertData(New Collections.Generic.List(Of String)({"Step size", "Count"}), True)
            worksheet2.Cell(2, 1).InsertData(HistDens)
            For Each col In worksheet2.ColumnsUsed
                col.AdjustToContents()
            Next col

            '4) Save and open
            Dim FileToGenerate As String = IO.Path.Combine(DB.MyPath, sfdMain.FileName)
            workbook.SaveAs(FileToGenerate)

        End Using

    End Sub

End Class