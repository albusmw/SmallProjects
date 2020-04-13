Option Explicit On
Option Strict On
Imports System.Windows.Forms

Partial Public Class MainForm

    '''<summary>Execute an XML file sequence.</summary>
    Private Sub RunXMLSequence(ByVal SpecFile As String)
        Dim BoolTrue As New List(Of String)({"TRUE", "YES", "1"})
        Dim SpecDoc As New Xml.XmlDocument : SpecDoc.Load(SpecFile)
        Dim BindFlagsSet As Reflection.BindingFlags = Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty
        'Reflect database
        Dim DB_Type As Type = DB.GetType
        Dim DB_props As New List(Of String)
        For Each SingleProperty As Reflection.PropertyInfo In DB.GetType.GetProperties()
            DB_props.Add(SingleProperty.Name)
        Next SingleProperty
        'Reflect meta database
        Dim DB_meta_Type As Type = DB_meta.GetType
        Dim DB_meta_props As New List(Of String)
        For Each SingleProperty As Reflection.PropertyInfo In DB_meta.GetType.GetProperties()
            DB_meta_props.Add(SingleProperty.Name)
        Next SingleProperty
        'Move over all exposure specifications
        For Each ExpNode As Xml.XmlNode In SpecDoc.SelectNodes("/sequence/exp")
            Dim CloseCam As Boolean = True                                      'close camera after series?
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
                        If DB_props.Contains(ExpAttrib.Name) Then DB_Type.InvokeMember(ExpAttrib.Name, BindFlagsSet, Type.DefaultBinder, DB, New Object() {PropValue})
                        If DB_meta_props.Contains(ExpAttrib.Name) Then DB_meta_Type.InvokeMember(ExpAttrib.Name, BindFlagsSet, Type.DefaultBinder, DB_meta, New Object() {PropValue})
                        If ExpAttrib.Name = "CloseCam" Then CloseCam = CBool(PropValue)                             'close camera at this exposure end?
                    Catch ex As Exception
                        Log("Failed for <" & ExpAttrib.Name & ">: " & ex.Message)
                    End Try
                End If
            Next ExpAttrib
            RefreshProperties()
            'Start exposure if specified
            If DB.CaptureCount > 0 Then
                QHYCapture(True)
                If CloseCam = True Then CloseCamera()
            End If
        Next ExpNode
        CloseCamera()
    End Sub

    '''<summary>Start the exposure.</summary>
    Private Function StartExposure(ByVal CaptureIdx As UInt32, ByVal FilterActive As eFilter, ByVal Chip_Pixel As sSize_UInt) As cSingleCaptureData

        Dim SingleCaptureData As New cSingleCaptureData

        'Set exposure parameters (first time / on property change / always if configured)
        If (CaptureIdx = 1) Or (DB.ConfigAlways = True) Or PropertyChanged = True Then SetExpParameters(CalculateROI(Chip_Pixel))
        DB.Stopper.Stamp("Set exposure parameters")

        'Cancel any running exposure
        CallOK("CancelQHYCCDExposing", QHY.QHYCamera.CancelQHYCCDExposing(CamHandle))
        CallOK("CancelQHYCCDExposingAndReadout", QHY.QHYCamera.CancelQHYCCDExposingAndReadout(CamHandle))
        DB.Stopper.Stamp("Cancel exposure")

        'Temperature
        SetTemperature(DB.TargetTemp, 60)

        'Load all parameter from the camera
        tsslMain.Text = "Taking capture " & CaptureIdx.ValRegIndep & "/" & DB.CaptureCount.ValRegIndep

        If DB_meta.Load10MicronDataAlways = True Then Load10MicronData()

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

        'Start expose (single or live frame mode)
        DB.Stopper.Start()
        If DB.StreamMode = eStreamMode.SingleFrame Then
            CallOK("ExpQHYCCDSingleFrame", QHY.QHYCamera.ExpQHYCCDSingleFrame(CamHandle))
            DB.Stopper.Stamp("ExpQHYCCDSingleFrame")
        Else
            CallOK("BeginQHYCCDLive", QHY.QHYCamera.BeginQHYCCDLive(CamHandle))
            DB.Stopper.Stamp("BeginQHYCCDLive")
        End If

        Return SingleCaptureData

    End Function

    '''<summary>Log all control values.</summary>
    Private Sub LogControlValues()

        'Display all properties available
        For Each CONTROL_ID As QHY.QHYCamera.CONTROL_ID In [Enum].GetValues(GetType(QHY.QHYCamera.CONTROL_ID))                   'Move over all Control ID's
            If QHY.QHYCamera.IsQHYCCDControlAvailable(CamHandle, CONTROL_ID) <> QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then    'If control is available
                Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": NOT AVAILABLE")
            Else
                Dim Min As Double = Double.NaN
                Dim Max As Double = Double.NaN
                Dim Stepping As Double = Double.NaN
                Dim CurrentValue As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, CONTROL_ID)
                If QHY.QHYCamera.GetQHYCCDParamMinMaxStep(CamHandle, CONTROL_ID, Min, Max, Stepping) = QHY.QHYCamera.QHYCCD_ERROR.QHYCCD_SUCCESS Then
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
    Private Function InitQHY(ByVal CamID As String) As Boolean

        'Init if not yet done
        If CamHandle = IntPtr.Zero Or UsedReadMode = eReadOutMode.Invalid Or UsedStreamMode = UInteger.MaxValue Then

            If CallOK(QHY.QHYCamera.InitQHYCCDResource) = True Then                                                                         'Init DLL itself
                DB.Stopper.Stamp("InitQHYCCDResource")
                Dim CameraCount As UInteger = QHY.QHYCamera.ScanQHYCCD                                                                      'Scan for connected cameras
                DB.Stopper.Stamp("ScanQHYCCD")
                If CameraCount > 0 Then                                                                                                     'If there is a camera found

                    Dim CamScanReport As New List(Of String)

                    'Get all cameras
                    CamScanReport.Add("Found " & CameraCount.ValRegIndep & " cameras:")
                    Dim AllCameras As New Dictionary(Of Integer, System.Text.StringBuilder)
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
                        Dim AllReadOutModes As New List(Of String)
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
        DB.Stopper.Stamp("InitQHY")
        Return True

    End Function

    '''<summary>Set the requested temperature.</summary>
    Private Function SetTemperature(ByVal TempToSet As Double, ByVal TimeOut As Integer) As Double
        Dim CurrentTemp As Double = Double.NaN
        Dim TimeOutT As New Diagnostics.Stopwatch : TimeOutT.Reset() : TimeOutT.Start()
        If TempToSet > -100 Then
            Do
                CurrentTemp = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURTEMP)
                Dim CurrentPWM As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CURPWM)
                tsslMain.Text = "Temp is current" & CurrentTemp.ValRegIndep & ", Target: " & TempToSet.ValRegIndep & ", cooler @ " & CurrentPWM.ValRegIndep & " %"
                If CurrentTemp <= TempToSet Then Exit Do
                System.Threading.Thread.Sleep(500)
                DE()
            Loop Until TimeOutT.ElapsedMilliseconds > TimeOut * 1000
        End If
        DB.Stopper.Stamp("Set temperature")
        Return CurrentTemp
    End Function

    '''<summary>Activate a certain filter.</summary>
    '''<param name="CamHandle">Handle to the camera.</param>
    '''<param name="FilterToSelect">Filter to select.</param>
    '''<param name="TimeOut">Time [s] to complete the operation.</param>
    Private Function ActiveFilter(ByRef CamHandle As IntPtr, ByVal FilterToSelect As eFilter, ByVal TimeOut As Integer) As eFilter
        Dim RetVal As eFilter = eFilter.Invalid
        Dim TimeOutT As New Diagnostics.Stopwatch : TimeOutT.Reset() : TimeOutT.Start()
        Dim NumberOfSlots As Double = QHY.QHYCamera.GetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_CFWSLOTSNUM)
        LogVerbose("Filter wheel with <" & NumberOfSlots.ValRegIndep & "> sloted detected")
        If CheckFilter(CamHandle) <> FilterToSelect Then
            Do
                SelectFilter(CamHandle)
                System.Threading.Thread.Sleep(500)
                RetVal = CheckFilter(CamHandle)
            Loop Until RetVal = FilterToSelect Or DB.StopFlag = True Or DB.FilterSlot = eFilter.Invalid Or TimeOutT.ElapsedMilliseconds > TimeOut * 1000
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

    ''<summary>Set the exposure parameters</summary>
    Private Sub SetExpParameters(ByVal ROIForCapture As System.Drawing.Rectangle)
        CallOK("SetQHYCCDBinMode", QHY.QHYCamera.SetQHYCCDBinMode(CamHandle, CUInt(DB.Binning), CUInt(DB.Binning)))
        CallOK("SetQHYCCDResolution", QHY.QHYCamera.SetQHYCCDResolution(CamHandle, CUInt(ROIForCapture.X), CUInt(ROIForCapture.Y), CUInt(ROIForCapture.Width \ DB.Binning), CUInt(ROIForCapture.Height \ DB.Binning)))
        CallOK("CONTROL_TRANSFERBIT", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_TRANSFERBIT, DB.ReadResolution))
        CallOK("CONTROL_GAIN", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_GAIN, DB.Gain))
        CallOK("CONTROL_OFFSET", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_OFFSET, DB.Offset))
        CallOK("CONTROL_USBTRAFFIC", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_USBTRAFFIC, DB.USBTraffic))
        CallOK("CONTROL_DDR", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_DDR, CInt(IIf(DB.DDR_RAM = True, 1.0, 0.0))))
        CallOK("CONTROL_EXPOSURE", QHY.QHYCamera.SetQHYCCDParam(CamHandle, QHY.QHYCamera.CONTROL_ID.CONTROL_EXPOSURE, DB.ExposureTime * 1000000))
        PropertyChanged = False
    End Sub

    '''<summary>Close the camera connection.</summary>
    Private Sub CloseCamera()
        If CamHandle <> IntPtr.Zero Then
            Log("Closing camera ...")
            QHY.QHYCamera.CancelQHYCCDExposingAndReadout(CamHandle)
            QHY.QHYCamera.CloseQHYCCD(CamHandle)
            QHY.QHYCamera.ReleaseQHYCCDResource()
            CamHandle = IntPtr.Zero
        End If
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

    '''<summary>Load the data from the 10Micron mount.</summary>
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

    '''<summary>Calculate all entries from the FITS header.</summary>
    '''<param name="SingleCaptureData">Capture configuration.</param>
    '''<param name="FileNameToWrite">File name with replacement parameters to use.</param>
    Private Function GenerateFITSHeader(ByVal SingleCaptureData As cSingleCaptureData, ByVal Pixel_Size As sSize_Dbl, ByRef FileNameToWrite As String) As Dictionary(Of eFITSKeywords, Object)

        Dim CustomElement As New Dictionary(Of eFITSKeywords, Object)

        'Precalculation
        Dim PLATESZ1 As Double = (Pixel_Size.Width * SingleCaptureData.NAXIS1) / 1000                           '[mm]
        Dim PLATESZ2 As Double = (Pixel_Size.Height * SingleCaptureData.NAXIS2) / 1000                          '[mm]
        Dim FOV1 As Double = 2 * Math.Atan(PLATESZ1 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)
        Dim FOV2 As Double = 2 * Math.Atan(PLATESZ2 / (2 * DB_meta.TelescopeFocalLength)) * (180 / Math.PI)

        CustomElement.Add(eFITSKeywords.OBS_ID, (DB_meta.GUID))

        CustomElement.Add(eFITSKeywords.OBJECT, DB_meta.ObjectName)
        CustomElement.Add(eFITSKeywords.RA, DB_meta.TelescopeRightAscension)
        CustomElement.Add(eFITSKeywords.DEC, DB_meta.TelescopeDeclination)

        CustomElement.Add(eFITSKeywords.AUTHOR, DB_meta.Author)
        CustomElement.Add(eFITSKeywords.ORIGIN, DB_meta.Origin)
        CustomElement.Add(eFITSKeywords.TELESCOP, DB_meta.Telescope)
        CustomElement.Add(eFITSKeywords.TELAPER, DB_meta.TelescopeAperture / 1000.0)
        CustomElement.Add(eFITSKeywords.TELFOC, DB_meta.TelescopeFocalLength / 1000.0)
        CustomElement.Add(eFITSKeywords.INSTRUME, UsedCameraId.ToString)
        CustomElement.Add(eFITSKeywords.PIXSIZE1, Pixel_Size.Width)
        CustomElement.Add(eFITSKeywords.PIXSIZE2, Pixel_Size.Height)
        CustomElement.Add(eFITSKeywords.PLATESZ1, PLATESZ1 / 10)                        'calculated from the image data as ROI may be set ...
        CustomElement.Add(eFITSKeywords.PLATESZ2, PLATESZ2 / 10)                        'calculated from the image data as ROI may be set ...
        CustomElement.Add(eFITSKeywords.FOV1, FOV1)
        CustomElement.Add(eFITSKeywords.FOV2, FOV2)
        CustomElement.Add(eFITSKeywords.COLORTYP, "0")                                                           '<- check

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
        CustomElement.Add(eFITSKeywords.SETTEMP, DB.TargetTemp)
        CustomElement.Add(eFITSKeywords.CCDTEMP, SingleCaptureData.ObsStartTemp)
        CustomElement.Add(eFITSKeywords.FOCUS, SingleCaptureData.TelescopeFocus)

        CustomElement.Add(eFITSKeywords.QHY_MODE, SingleCaptureData.CamReadOutMode.ToString)
        CustomElement.Add(eFITSKeywords.PROGRAM, Me.Text)

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

    '''<summary>Add a certain FITS header card.</summary>
    Private Sub AddFITSHeaderCard(ByRef Container As List(Of String()), ByVal Keyword As eFITSKeywords, ByVal VAlue As String)
        If String.IsNullOrEmpty(VAlue) = False Then
            Dim FITSKey As New cFITSKey
            Container.Add(New String() {FITSKey(Keyword), VAlue, FITSKey.Comment(Keyword)})
        End If
    End Sub

End Class
