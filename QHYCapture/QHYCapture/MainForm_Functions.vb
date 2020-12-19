Option Explicit On
Option Strict On
Imports System.Windows.Forms

Partial Public Class MainForm

    Private Delegate Sub InvokeDelegate()

    '''<summary>Execute an XML file sequence.</summary>
    Private Sub RunXMLSequence(ByVal SpecFile As String, ByVal RunExposure As Boolean)
        Dim BoolTrue As New List(Of String)({"TRUE", "YES", "1"})
        Dim BindFlagsSet As Reflection.BindingFlags = Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty
        'Reflect database
        Dim DB_Type As Type = M.DB.GetType
        Dim DB_props As List(Of String) = GetAllPropertyNames(M.DB.GetType)
        'Reflect meta database
        Dim DB_meta_Type As Type = DB_meta.GetType
        Dim DB_meta_props As List(Of String) = GetAllPropertyNames(DB_meta.GetType)
        'Move over all exposure specifications in the file
        Dim SpecDoc As New Xml.XmlDocument
        Try
            SpecDoc.Load(SpecFile)
        Catch ex As Exception
            MsgBox("XML error: [" & ex.Message & "]")
            Exit Sub
        End Try
        For Each ExpNode As Xml.XmlNode In SpecDoc.SelectNodes("/sequence/exp")
            'Load all attributes from the file
            For Each ExpAttrib As Xml.XmlAttribute In ExpNode.Attributes
                Dim PropType As Type = Nothing
                Dim PropValue As Object = Nothing
                'Get property type and value
                Try
                    If DB_props.Contains(ExpAttrib.Name) Then PropType = DB_Type.GetProperty(ExpAttrib.Name).PropertyType
                    If DB_meta_props.Contains(ExpAttrib.Name) Then PropType = DB_meta_Type.GetProperty(ExpAttrib.Name).PropertyType
                    Select Case PropType
                        Case GetType(Int32)
                            PropValue = CType(ExpAttrib.Value, Int32)
                        Case GetType(UInt32)
                            PropValue = CType(ExpAttrib.Value, UInt32)
                        Case GetType(Double)
                            PropValue = Val(ExpAttrib.Value.Replace(",", "."))
                        Case GetType(String)
                            PropValue = ExpAttrib.Value
                        Case GetType(Boolean)
                            If BoolTrue.Contains(ExpAttrib.Value.ToUpper) Then PropValue = True Else PropValue = False
                        Case Else
                            'Dim X As Type = Type.GetType(PropTypeName)
                            PropValue = [Enum].Parse(PropType, ExpAttrib.Value)
                    End Select
                Catch ex As Exception
                    'Do nothing ...
                End Try
                If IsNothing(PropValue) = False Then
                    Try
                        If DB_props.Contains(ExpAttrib.Name) Then DB_Type.InvokeMember(ExpAttrib.Name, BindFlagsSet, Type.DefaultBinder, M.DB, New Object() {PropValue})
                        If DB_meta_props.Contains(ExpAttrib.Name) Then DB_meta_Type.InvokeMember(ExpAttrib.Name, BindFlagsSet, Type.DefaultBinder, DB_meta, New Object() {PropValue})
                    Catch ex As Exception
                        Log("Failed for <" & ExpAttrib.Name & ">: " & ex.Message)
                    End Try
                End If
            Next ExpAttrib
            RefreshProperties()
            'Start exposure if specified
            If M.DB.CaptureCount > 0 And RunExposure Then
                QHYCapture(M.DB.CloseCam)
            End If
            If M.DB.StopFlag = True Then Exit For
        Next ExpNode
        If RunExposure Then CloseCamera()
    End Sub

    '''<summary>Get a list of all available property names.</summary>
    Private Function GetAllPropertyNames(ByRef TypeToReflect As Type) As List(Of String)
        Dim RetVal As New List(Of String)
        Dim DescriptionAttribute As Type = GetType(System.ComponentModel.DescriptionAttribute)
        For Each SingleProperty As Reflection.PropertyInfo In TypeToReflect.GetProperties()
            Dim PropertyName As String = SingleProperty.Name
            RetVal.Add(PropertyName)
        Next SingleProperty
        Return RetVal
    End Function

    '''<summary>Start the exposure.</summary>
    Private Function StartExposure(ByVal CaptureIdx As UInt32, ByVal FilterActive As eFilter, ByVal Chip_Pixel As sSize_UInt) As cSingleCaptureInfo

        Dim SingleCaptureData As New cSingleCaptureInfo

        'Set exposure parameters (first time / on property change / always if configured)
        LED_update(tsslLED_config, True)
        If (CaptureIdx = 1) Or (M.DB.ConfigAlways = True) Or PropertyChanged = True Then SetExpParameters(CalculateROI(Chip_Pixel))
        LED_update(tsslLED_config, False)
        M.DB.Stopper.Stamp("Set exposure parameters")

        'Cancel any running exposure
        If M.DB.StreamMode = eStreamMode.SingleFrame Then
            CallOK("CancelQHYCCDExposing", QHY.QHYCamera.CancelQHYCCDExposing(M.DB.CamHandle))
            CallOK("CancelQHYCCDExposingAndReadout", QHY.QHYCamera.CancelQHYCCDExposingAndReadout(M.DB.CamHandle))
            M.DB.Stopper.Stamp("Cancel exposure")
        End If

        'Temperature
        LED_update(tsslLED_cooling, True)
        SetTemperature(M.DB.CoolingTimeOut)
        LED_update(tsslLED_cooling, False)

        'Load all parameter from the camera
        tsslMain.Text = "Taking capture " & CaptureIdx.ValRegIndep & "/" & M.DB.CaptureCount.ValRegIndep

        If DB_meta.Load10MicronDataAlways = True Then Load10MicronData()

        With SingleCaptureData
            .CaptureIdx = CaptureIdx
            .FilterActive = FilterActive
            .CamReadOutMode = New Text.StringBuilder : QHY.QHYCamera.GetQHYCCDReadModeName(M.DB.CamHandle, M.DB.ReadOutMode, .CamReadOutMode)
            .ExpTime = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE) / 1000000
            .Gain = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN)
            .Offset = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET)
            .Brightness = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_BRIGHTNESS)
            .ObsStartTemp = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
        End With

        'Start expose (single or live frame mode)
        LED_update(tsslLED_capture, True)
        M.DB.Stopper.Start()
        If M.DB.StreamMode = eStreamMode.SingleFrame Then
            CallOK("ExpQHYCCDSingleFrame", QHY.QHYCamera.ExpQHYCCDSingleFrame(M.DB.CamHandle))
            M.DB.Stopper.Stamp("ExpQHYCCDSingleFrame")
        Else
            If M.DB.LiveModeInitiated = False Then
                CallOK("BeginQHYCCDLive", QHY.QHYCamera.BeginQHYCCDLive(M.DB.CamHandle))
                M.DB.LiveModeInitiated = True
            End If
            M.DB.Stopper.Stamp("BeginQHYCCDLive")
        End If

        Return SingleCaptureData

    End Function

    '''<summary>Log all control values.</summary>
    Private Sub LogControlValues()

        'Display all properties available
        For Each CONTROL_ID As QHY.QHYCamera.CONTROL_ID In [Enum].GetValues(GetType(QHY.QHYCamera.CONTROL_ID))                   'Move over all Control ID's
            If QHY.QHYCamera.IsQHYCCDControlAvailable(M.DB.CamHandle, CONTROL_ID) <> QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then    'If control is available
                Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": NOT AVAILABLE")
            Else
                Dim Min As Double = Double.NaN
                Dim Max As Double = Double.NaN
                Dim Stepping As Double = Double.NaN
                Dim CurrentValue As Double = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, CONTROL_ID)
                If QHY.QHYCamera.GetQHYCCDParamMinMaxStep(M.DB.CamHandle, CONTROL_ID, Min, Max, Stepping) = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then
                    Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": " & Min.ValRegIndep & " ... <" & Stepping.ValRegIndep & "> ... " & Max.ValRegIndep & ", current: " & CurrentValue.ValRegIndep)
                Else
                    Select Case CurrentValue
                        Case UInteger.MaxValue
                            Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": BOOL, current: TRUE")
                        Case 0
                            Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": BOOL, current: FALSE")
                        Case Else
                            Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": Discret, current: " & CurrentValue.ValRegIndep)
                    End Select
                End If
            End If
        Next CONTROL_ID

    End Sub

    '''<summary>Init the camera with the passed handle.</summary>
    Private Function InitQHY(ByVal CamIDToSearch As String) As Boolean

        'Init if not yet done
        If M.DB.CamHandle = IntPtr.Zero Or M.DB.UsedReadMode = eReadOutMode.Invalid Or M.DB.UsedStreamMode = eStreamMode.Invalid Then

            If CallOK(QHY.QHYCamera.InitQHYCCDResource) = True Then                                                                 'Init DLL itself
                M.DB.Stopper.Stamp("InitQHYCCDResource")
                Dim CameraCount As UInteger = QHY.QHYCamera.ScanQHYCCD                                                              'Scan for connected cameras
                M.DB.Stopper.Stamp("ScanQHYCCD")
                If CameraCount > 0 Then                                                                                             'If there is a camera found

                    Dim CamScanReport As New List(Of String)

                    'Get all cameras
                    CamScanReport.Add("Found " & CameraCount.ValRegIndep & " cameras:")
                    Dim AllCameras As New Dictionary(Of Integer, System.Text.StringBuilder)
                    For Idx As Integer = 0 To CInt(CameraCount - 1)
                        Dim CurrentCamID As New System.Text.StringBuilder(0)                                                        'Prepare camera ID holder
                        If CallOK(QHY.QHYCamera.GetQHYCCDId(Idx, CurrentCamID)) = True Then                                         'Fetch camera ID
                            AllCameras.Add(Idx, CurrentCamID)
                            CamScanReport.Add("  Camera #" & (Idx + 1).ValRegIndep & ": <" & CurrentCamID.ToString & ">")
                        Else
                            CamScanReport.Add("  Camera #" & (Idx + 1).ValRegIndep & ": <<?????>>")
                        End If
                    Next Idx

                    'Find correct camera
                    M.DB.UsedCameraId = New System.Text.StringBuilder
                    For Each CamIdx As Integer In AllCameras.Keys
                        If AllCameras(CamIdx).ToString.Contains(CamIDToSearch) = True Or CamIDToSearch = "*" Then
                            M.DB.UsedCameraId = New System.Text.StringBuilder(AllCameras(CamIdx).ToString)
                            Exit For
                        End If
                    Next CamIdx

                    'Exit if camera is not correct
                    If M.DB.UsedCameraId.Length = 0 Then
                        Log(CamScanReport)
                        Return False
                    Else
                        LogVerbose(CamScanReport)
                    End If

                    'Open found camera
                    LogVerbose("Found QHY camera to use: <" & M.DB.UsedCameraId.ToString & ">")                                       'Display fetched camera ID
                    M.DB.CamHandle = QHY.QHYCamera.OpenQHYCCD(M.DB.UsedCameraId)                                                        'Open the camera
                    If M.DB.CamHandle <> IntPtr.Zero Then

                        'Get all supported read-out modes
                        Dim ReadoutModesCount As UInteger = 0
                        CallOK(QHY.QHYCamera.GetQHYCCDNumberOfReadModes(M.DB.CamHandle, ReadoutModesCount))
                        Dim AllReadOutModes As New List(Of String)
                        For ReadoutMode As UInteger = 0 To CUInt(ReadoutModesCount - 1)
                            Dim ReadoutModeName As New Text.StringBuilder
                            Dim ResX As UInteger = 0 : Dim ResY As UInteger = 0
                            CallOK(QHY.QHYCamera.GetQHYCCDReadModeName(M.DB.CamHandle, ReadoutMode, ReadoutModeName))
                            CallOK(QHY.QHYCamera.GetQHYCCDReadModeResolution(M.DB.CamHandle, ReadoutMode, ResX, ResY))
                            AllReadOutModes.Add(ReadoutMode.ValRegIndep & ": " & ReadoutModeName.ToString & " (" & ResX.ValRegIndep & "x" & ResY.ValRegIndep & ")")
                        Next ReadoutMode

                        If M.DB.Log_CamProp Then
                            Log("Available read-out modes:")
                            Log(AllReadOutModes)
                        End If

                        'Run the start-up init sequence
                        Log("Init QHY camera  <" & M.DB.UsedCameraId.ToString & "> ...")
                        If CallOK(QHY.QHYCamera.SetQHYCCDReadMode(M.DB.CamHandle, M.DB.ReadOutMode)) = True Then
                            If CallOK(QHY.QHYCamera.SetQHYCCDStreamMode(M.DB.CamHandle, M.DB.StreamMode)) = True Then                   'Set single capture mode
                                If CallOK(QHY.QHYCamera.InitQHYCCD(M.DB.CamHandle)) = True Then                                       'Init the camera with the selected mode, ...
                                    'Camera was opened correct
                                    M.DB.UsedReadMode = M.DB.ReadOutMode
                                    M.DB.UsedStreamMode = M.DB.StreamMode
                                    M.DB.LiveModeInitiated = False
                                Else
                                    LogError("InitQHYCCD FAILED!")
                                    M.DB.CamHandle = IntPtr.Zero
                                End If
                            Else
                                LogError("SetQHYCCDStreamMode FAILED!")
                                M.DB.CamHandle = IntPtr.Zero
                            End If
                        Else
                            LogError("SetQHYCCDReadMode to <" & M.DB.ReadOutMode & "> FAILED!")
                        End If
                    Else
                        LogError("OpenQHYCCD FAILED!")
                        M.DB.CamHandle = IntPtr.Zero
                        Return False
                    End If
                Else
                    LogError("Init DLL OK but no camera found!")
                    M.DB.CamHandle = IntPtr.Zero
                    Return False
                End If
            Else
                LogError("Init QHY did fail!")
            End If
        End If

        'Everything OK
        M.DB.Stopper.Stamp("InitQHY")
        Return True

    End Function

    '''<summary>Set the requested temperature.</summary>
    '''<param name="TimeOut">Time-out [s] for the complete cooling process</param>
    Private Function SetTemperature(ByVal TimeOut As Double) As Double
        Dim TimeOutT As New Diagnostics.Stopwatch : TimeOutT.Reset() : TimeOutT.Start()
        Dim CurrentTemp As Double = Double.NaN
        If M.DB.TargetTemp > -100 Then
            Do
                If GetTempState(CurrentTemp) = True Then Exit Do
                System.Threading.Thread.Sleep(500)
                DE()
            Loop Until (TimeOutT.ElapsedMilliseconds > TimeOut * 1000) Or M.DB.StopFlag = True
        End If
        M.DB.Stopper.Stamp("Set temperature")
        Return CurrentTemp
    End Function

    '''<summary>Get and display the requested temperature.</summary>
    Private Function GetTempState() As Boolean
        Dim DontCare As Double = Double.NaN
        Return GetTempState(DontCare)
    End Function

    '''<summary>Get and display the requested temperature.</summary>
    Private Function GetTempState(ByRef CurrentTemp As Double) As Boolean
        Dim RetVal As Boolean = False
        CurrentTemp = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
        Dim CurrentPWM As Double = QHY.QHYCamera.GetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURPWM)
        tsslTemperature.Text = "T = " & CurrentTemp.ValRegIndep & " °C (-> " & M.DB.TargetTemp.ValRegIndep & " °C, cooler @ " & CurrentPWM.ValRegIndep & " %)"
        If System.Math.Abs(CurrentTemp - M.DB.TargetTemp) <= M.DB.TargetTempTolerance Then
            RetVal = True
            tsslTemperature.BackColor = Color.Green
        Else
            RetVal = False
            tsslTemperature.BackColor = Color.Red
        End If
        Return RetVal
    End Function

    '''<summary>Activate a certain filter.</summary>
    '''<param name="CamHandle">Handle to the camera.</param>
    '''<param name="FilterToSelect">Filter to select.</param>
    '''<param name="TimeOut">Time [s] to complete the operation.</param>
    Private Function ActiveFilter(ByRef CamHandle As IntPtr, ByVal FilterToSelect As eFilter, ByVal TimeOut As Double) As eFilter
        Dim RetVal As eFilter = eFilter.Invalid
        Dim TimeOutT As New Diagnostics.Stopwatch : TimeOutT.Reset() : TimeOutT.Start()
        Dim NumberOfSlots As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CFWSLOTSNUM)
        LogVerbose("Filter wheel with <" & NumberOfSlots.ValRegIndep & "> sloted detected")
        If CheckFilter(CamHandle) <> FilterToSelect Then
            Do
                SelectFilter(CamHandle)
                System.Threading.Thread.Sleep(500)
                RetVal = CheckFilter(CamHandle)
            Loop Until RetVal = FilterToSelect Or M.DB.StopFlag = True Or M.DB.FilterSlot = eFilter.Invalid Or TimeOutT.ElapsedMilliseconds > TimeOut * 1000
        Else
            RetVal = FilterToSelect
        End If
        Return RetVal
    End Function

    '''<summary>Read the current filter wheel position.</summary>
    '''<param name="CamHandle">Handle to the camera.</param>
    '''<returns>Filter that is IN or invalid if there was something wrong.</returns>
    '''<seealso cref="https://note.youdao.com/share/?token=48C579B49B5840609AB9B6D7D375B742&gid=7195236"/>
    Private Function CheckFilter(ByRef CamHandle As IntPtr) As eFilter
        Dim RetVal As eFilter = eFilter.Invalid
        Dim FilterState(63) As Byte
        Dim Pinner As New cIntelIPP.cPinHandler
        Dim FilterStatePtr As IntPtr = Pinner.Pin(FilterState)
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
        If M.DB.FilterSlot <> eFilter.Invalid Then
            FilterState(0) = CByte(Asc((M.DB.FilterSlot - 1).ToString.Trim))
            If QHY.QHYCamera.SendOrder2QHYCCDCFW(CamHandle, FilterStatePtr, 1) = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then
                Log("  Filter requested: " & M.DB.FilterSlot.ToString.Trim)
                RetVal = M.DB.FilterSlot
            Else
                LogError("  !!! Filter select failed: " & M.DB.FilterSlot.ToString.Trim)
            End If
        End If
        Pinner = Nothing
        Return RetVal
    End Function

    ''<summary>Set the exposure parameters</summary>
    Private Sub SetExpParameters(ByVal ROIForCapture As System.Drawing.Rectangle)
        CallOK("SetQHYCCDBinMode", QHY.QHYCamera.SetQHYCCDBinMode(M.DB.CamHandle, CUInt(M.DB.HardwareBinning), CUInt(M.DB.HardwareBinning)))
        CallOK("SetQHYCCDResolution", QHY.QHYCamera.SetQHYCCDResolution(M.DB.CamHandle, CUInt(ROIForCapture.X), CUInt(ROIForCapture.Y), CUInt(ROIForCapture.Width \ M.DB.HardwareBinning), CUInt(ROIForCapture.Height \ M.DB.HardwareBinning)))
        CallOK("CONTROL_TRANSFERBIT", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_TRANSFERBIT, M.DB.ReadResolution))
        CallOK("CONTROL_GAIN", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN, M.DB.Gain))
        CallOK("CONTROL_OFFSET", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET, M.DB.Offset))
        CallOK("CONTROL_USBTRAFFIC", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_USBTRAFFIC, M.DB.USBTraffic))
        CallOK("CONTROL_DDR", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_DDR, CInt(IIf(M.DB.DDR_RAM = True, 1.0, 0.0))))
        CallOK("CONTROL_EXPOSURE", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE, M.DB.ExposureTime * 1000000))
        CallOK("CONTROL_BRIGHTNESS", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_BRIGHTNESS, M.DB.Brightness))
        CallOK("CONTROL_CONTRAST", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CONTRAST, M.DB.Contrast))
        CallOK("CONTROL_WBR", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_WBR, M.DB.WhiteBalance_Red))
        CallOK("CONTROL_WBG", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_WBG, M.DB.WhiteBalance_Green))
        CallOK("CONTROL_WBB", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_WBB, M.DB.WhiteBalance_Blue))
        CallOK("CONTROL_GAMMA", QHY.QHYCamera.SetQHYCCDParam(M.DB.CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAMMA, M.DB.Gamma))
        PropertyChanged = False
    End Sub

    '''<summary>Close the camera connection.</summary>
    Private Sub CloseCamera()
        If M.DB.CamHandle <> IntPtr.Zero Then
            Log("Closing camera ...")
            QHY.QHYCamera.CancelQHYCCDExposingAndReadout(M.DB.CamHandle)
            QHY.QHYCamera.CloseQHYCCD(M.DB.CamHandle)
            QHY.QHYCamera.ReleaseQHYCCDResource()
            M.DB.CamHandle = IntPtr.Zero
        End If
        LED_update(tsslLED_cooling, False)
        LED_update(tsslLED_capture, False)
        LED_update(tsslLED_reading, False)
    End Sub

    '''<summary>Load the data from the 10Micron mount.</summary>
    Private Sub Load10MicronData()
        Dim Client10Micron As New Net.Sockets.TcpClient(DB_meta.IP_10Micron, 3490)
        Dim Stream10Micron As Net.Sockets.NetworkStream = Client10Micron.GetStream
        c10Micron.SendCommand(Stream10Micron, c10Micron.SetCommand.SetUltraHighPrecision)
        DB_meta.SiteLatitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.SiteLatitude)
        DB_meta.SiteLongitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.SiteLongitude)
        DB_meta.TelescopeRightAscension = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeRightAscension)
        DB_meta.TelescopeDeclination = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeDeclination)
        DB_meta.TelescopeAltitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeAltitude)
        DB_meta.TelescopeAzimuth = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeAzimuth)
        RefreshProperties()
    End Sub

    '''<summary>Calculate all entries from the FITS header.</summary>
    '''<param name="SingleCaptureData">Capture configuration.</param>
    '''<param name="FileNameToWrite">File name with replacement parameters to use.</param>
    Private Function GenerateFITSHeader(ByVal SingleCaptureData As cSingleCaptureInfo, ByVal Pixel_Size As sSize_Dbl, ByRef FileNameToWrite As String) As Dictionary(Of eFITSKeywords, Object)

        Dim CustomElement As New Dictionary(Of eFITSKeywords, Object)

        'Precalculation
        Dim PLATESZ1 As Double = (Pixel_Size.Width * SingleCaptureData.NAXIS1) / 1000                           '[mm]
        Dim PLATESZ2 As Double = (Pixel_Size.Height * SingleCaptureData.NAXIS2) / 1000                          '[mm]
        Dim FOV1 As Double = 2 * Math.Atan(PLATESZ1 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)
        Dim FOV2 As Double = 2 * Math.Atan(PLATESZ2 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)
        Dim FilterName As String = [Enum].GetName(GetType(eFilter), SingleCaptureData.FilterActive)

        CustomElement.Add(eFITSKeywords.OBS_ID, (DB_meta.GUID))

        CustomElement.Add(eFITSKeywords.OBJECT, DB_meta.ObjectName)
        CustomElement.Add(eFITSKeywords.RA, DB_meta.TelescopeRightAscension)
        CustomElement.Add(eFITSKeywords.DEC, DB_meta.TelescopeDeclination)

        CustomElement.Add(eFITSKeywords.AUTHOR, DB_meta.Author)
        CustomElement.Add(eFITSKeywords.ORIGIN, DB_meta.Origin)
        CustomElement.Add(eFITSKeywords.TELESCOP, DB_meta.Telescope)
        CustomElement.Add(eFITSKeywords.TELAPER, DB_meta.TelescopeAperture / 1000.0)
        CustomElement.Add(eFITSKeywords.TELFOC, DB_meta.TelescopeFocalLength / 1000.0)
        CustomElement.Add(eFITSKeywords.INSTRUME, M.DB.UsedCameraId.ToString)
        CustomElement.Add(eFITSKeywords.PIXSIZE1, Pixel_Size.Width)
        CustomElement.Add(eFITSKeywords.PIXSIZE2, Pixel_Size.Height)
        CustomElement.Add(eFITSKeywords.PLATESZ1, PLATESZ1 / 10)                        'calculated from the image data as ROI may be set ...
        CustomElement.Add(eFITSKeywords.PLATESZ2, PLATESZ2 / 10)                        'calculated from the image data as ROI may be set ...
        CustomElement.Add(eFITSKeywords.FOV1, FOV1)
        CustomElement.Add(eFITSKeywords.FOV2, FOV2)
        CustomElement.Add(eFITSKeywords.COLORTYP, "0")                                                           '<- check
        CustomElement.Add(eFITSKeywords.FILTER, FilterName)

        CustomElement.Add(eFITSKeywords.DATE_OBS, cFITSKeywords.GetDateWithTime(SingleCaptureData.ObsStart))
        CustomElement.Add(eFITSKeywords.DATE_END, cFITSKeywords.GetDateWithTime(SingleCaptureData.ObsEnd))
        CustomElement.Add(eFITSKeywords.TIME_OBS, cFITSKeywords.GetTime(SingleCaptureData.ObsStart))
        CustomElement.Add(eFITSKeywords.TIME_END, cFITSKeywords.GetTime(SingleCaptureData.ObsEnd))

        CustomElement.Add(eFITSKeywords.CRPIX1, 0.5 * (SingleCaptureData.NAXIS1 + 1))
        CustomElement.Add(eFITSKeywords.CRPIX2, 0.5 * (SingleCaptureData.NAXIS2 + 1))

        CustomElement.Add(eFITSKeywords.IMAGETYP, DB_meta.ExposureType)
        CustomElement.Add(eFITSKeywords.EXPTIME, SingleCaptureData.ExpTime)
        CustomElement.Add(eFITSKeywords.GAIN, SingleCaptureData.Gain)
        CustomElement.Add(eFITSKeywords.OFFSET, SingleCaptureData.Offset)
        CustomElement.Add(eFITSKeywords.BRIGHTNESS, SingleCaptureData.Brightness)
        CustomElement.Add(eFITSKeywords.SETTEMP, M.DB.TargetTemp)
        CustomElement.Add(eFITSKeywords.CCDTEMP, SingleCaptureData.ObsStartTemp)
        CustomElement.Add(eFITSKeywords.FOCUS, SingleCaptureData.TelescopeFocus)

        CustomElement.Add(eFITSKeywords.QHY_MODE, SingleCaptureData.CamReadOutMode.ToString)
        CustomElement.Add(eFITSKeywords.PROGRAM, Me.Text)

        'Create FITS file name
        FileNameToWrite = FileNameToWrite.Replace("$IDX$", Format(SingleCaptureData.CaptureIdx, "000"))
        FileNameToWrite = FileNameToWrite.Replace("$CNT$", Format(M.DB.CaptureCount, "000"))
        FileNameToWrite = FileNameToWrite.Replace("$EXP$", SingleCaptureData.ExpTime.ValRegIndep)
        FileNameToWrite = FileNameToWrite.Replace("$GAIN$", SingleCaptureData.Gain.ValRegIndep)
        FileNameToWrite = FileNameToWrite.Replace("$OFFS$", SingleCaptureData.Offset.ValRegIndep)
        FileNameToWrite = FileNameToWrite.Replace("$FILT$", FilterName)
        FileNameToWrite = FileNameToWrite.Replace("$RMODE$", [Enum].GetName(GetType(eReadOutMode), M.DB.ReadOutMode))

        Return CustomElement

    End Function

    '''<summary>Add a certain FITS header card.</summary>
    Private Sub AddFITSHeaderCard(ByRef Container As List(Of String()), ByVal Keyword As eFITSKeywords, ByVal VAlue As String)
        If String.IsNullOrEmpty(VAlue) = False Then
            Dim FITSKey As New cFITSKey
            Container.Add(New String() {FITSKey(Keyword)(0), VAlue, FITSKey.Comment(Keyword)})
        End If
    End Sub

    '''<summary>Active or deactive the capture LED.</summary>
    Private Sub LED_update(ByRef LED As ToolStripStatusLabel, ByVal Status As Boolean)
        tsslLED_init.Enabled = False : tsslLED_init.BackColor = System.Drawing.SystemColors.Control
        tsslLED_config.Enabled = False : tsslLED_config.BackColor = System.Drawing.SystemColors.Control
        tsslLED_cooling.Enabled = False : tsslLED_cooling.BackColor = System.Drawing.SystemColors.Control
        tsslLED_capture.Enabled = False : tsslLED_capture.BackColor = System.Drawing.SystemColors.Control
        tsslLED_reading.Enabled = False : tsslLED_reading.BackColor = System.Drawing.SystemColors.Control
        LED.Enabled = Status
        LED.BackColor = CType(IIf(Status, Color.Red, System.Drawing.SystemColors.Control), Color)
        LED.Invalidate()
        ssMain.Update()
        System.Windows.Forms.Application.DoEvents()
        'ssMain.BeginInvoke(New InvokeDelegate(AddressOf InvokeMethod))
    End Sub

    Public Sub InvokeMethod()
        tsslLED_capture.Invalidate()
        ssMain.Update()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class
