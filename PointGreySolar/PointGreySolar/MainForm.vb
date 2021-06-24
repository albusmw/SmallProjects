Option Explicit On
Option Strict On

'C:\Program Files\Point Grey Research\FlyCapture2\shortcuts\documentation

Public Class MainForm

    Dim DB As New cDB
    Dim StopNow As Boolean = True
    Dim ImgProcess As New cImgProcess

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click

        ChDir("C:\Program Files\Point Grey Research\FlyCap2 Viewer\bin64")

        Dim busMgr As New FlyCapture2Managed.ManagedBusManager()
        Dim numCameras As UInt32 = busMgr.GetNumOfCameras() : AddLog(numCameras.ValRegIndep & " cameras found")
        Dim EmbedInfo As Boolean = True

        Dim ErrorList As New List(Of String)

        'Always run with 1st camera
        If numCameras > 0 Then

            Dim Cam As New FlyCapture2Managed.ManagedCamera
            Cam.Connect(busMgr.GetCameraFromIndex(0))
            Dim camInfo As FlyCapture2Managed.CameraInfo = Cam.GetCameraInfo()
            AddLog(" -> Opened <" & camInfo.modelName & ">")

            'Set embedded information
            SetEmbedded(Cam, False)

            Dim CAM_stats As FlyCapture2Managed.CameraStats = Cam.GetStats

            'Disable all automatic control
            LogFunctionCall(ErrorList, FlyCapture.SetautoManualModeProperty(Cam, FlyCapture2Managed.PropertyType.AutoExposure, False))
            LogFunctionCall(ErrorList, FlyCapture.SetautoManualModeProperty(Cam, FlyCapture2Managed.PropertyType.Shutter, False))
            LogFunctionCall(ErrorList, FlyCapture.SetautoManualModeProperty(Cam, FlyCapture2Managed.PropertyType.Gain, False))
            LogFunctionCall(ErrorList, FlyCapture.SetautoManualModeProperty(Cam, FlyCapture2Managed.PropertyType.FrameRate, False))

            'Configure full manual control
            LogFunctionCall(ErrorList, FlyCapture.SetOnOffProperty(Cam, FlyCapture2Managed.PropertyType.AutoExposure, True))
            LogFunctionCall(ErrorList, FlyCapture.SetOnOffProperty(Cam, FlyCapture2Managed.PropertyType.Gamma, True))
            LogFunctionCall(ErrorList, FlyCapture.SetOnOffProperty(Cam, FlyCapture2Managed.PropertyType.FrameRate, True))

            'Configure values
            LogFunctionCall(ErrorList, FlyCapture.SetabsValueProperty(Cam, FlyCapture2Managed.PropertyType.AutoExposure, 0.0))
            LogFunctionCall(ErrorList, FlyCapture.SetabsValueProperty(Cam, FlyCapture2Managed.PropertyType.Gamma, 1.0))
            LogFunctionCall(ErrorList, FlyCapture.SetabsValueProperty(Cam, FlyCapture2Managed.PropertyType.FrameRate, 20.0))

            'Special format (16 bit per pixel, full ROI)
            Set16BitFullROI(Cam)

            Dim FC2_RAW As New FlyCapture2Managed.ManagedImage()
            Dim FS2_Converted As New FlyCapture2Managed.ManagedImage()
            DB.Container = New AstroNET.Statistics(DB.IPP)
            DB.Container.ResetAllProcessors()

            '=============================================================================
            'Start the capture loop
            ErrorList.Add("========== CAPTURE START ==========")
            Cam.StartCapture()
            Dim StartTime As String = Format(Now, "yyyy.dd.MM_HH.mm.ss.ffff")
            StopNow = False
            For imageCnt As Integer = 0 To (DB.CaptureCount - 1)

                Dim UniqueFileName As String = "PG_solar_" & camInfo.serialNumber.ToString.Trim & "_" & StartTime & "_" & Format(imageCnt, "00000")

                'Config all parameters
                LogFunctionCall(ErrorList, FlyCapture.SetabsValueProperty(Cam, FlyCapture2Managed.PropertyType.Shutter, DB.Exposure))
                LogFunctionCall(ErrorList, FlyCapture.SetabsValueProperty(Cam, FlyCapture2Managed.PropertyType.Gain, DB.Gain))
                LogFunctionCall(ErrorList, FlyCapture.SetabsValueProperty(Cam, FlyCapture2Managed.PropertyType.Brightness, DB.Brightness))

                'Retrieve the image
                Try
                    Cam.RetrieveBuffer(FC2_RAW)
                Catch ex As FlyCapture2Managed.FC2Exception
                    tsslCaptureProgress.Text = String.Format("!!! error retrieving buffer: {0}", ex.Message)
                    Continue For
                End Try

                'Get the timestamp
                Dim timeStamp As FlyCapture2Managed.TimeStamp = FC2_RAW.timeStamp
                tsslCaptureDetails.Text = " <" & FC2_RAW.rows.ValRegIndep & "x" & FC2_RAW.cols.ValRegIndep & "> @ " & FC2_RAW.bitsPerPixel.ValRegIndep & "bit/pixel, Gain " & Cam.GetProperty(FlyCapture2Managed.PropertyType.Gain).absValue.ValRegIndep

                'Get the Bitmap object. Bitmaps are only valid if the pixel format of the ManagedImage is RGB or RGBU.
                FC2_RAW.Convert(DB.ConvertToFormat, FS2_Converted)

                'Display the image in the capture form
                pbCapture.Image = FS2_Converted.bitmap

                'Save the image if requested
                If DB.SaveTIFF = True Then
                    Using SaveImage As New FlyCapture2Managed.ManagedImage()
                        FC2_RAW.Convert(FlyCapture2Managed.PixelFormat.PixelFormatRgb16, SaveImage)
                        SaveImage.Save(System.IO.Path.Combine(DB.StoragePath, UniqueFileName) & ".tiff", FlyCapture2Managed.ImageFileFormat.Tiff)
                    End Using
                End If

                '=============================================================================
                'Get the raw buffer data (> 8 bit per pixel)
                Dim FC2_RAW_Ptr As IntPtr = Global.GetPointer.GetIntPtr(FC2_RAW)
                Dim TotalPixel As Integer = CInt((FC2_RAW.rows * FC2_RAW.cols) - 1)
                ReDim DB.Container.DataProcessor_UInt16.ImageData(0).Data(CInt(FC2_RAW.rows - 1), CInt(FC2_RAW.cols - 1))
                DB.IPP.Copy(FC2_RAW_Ptr, DB.Container.DataProcessor_UInt16.ImageData(0).Data, TotalPixel)
                DB.IPP.Transpose(DB.Container.DataProcessor_UInt16.ImageData(0).Data)           'in-place only works for width = height
                '=============================================================================

                '=============================================================================
                'Save FITS image
                If DB.SaveImagesFITS = True Then
                    Dim FITSHeader As New cFITSHeaderParser
                    cFITSWriter.Write(System.IO.Path.Combine(DB.StoragePath, UniqueFileName) & ".fits", DB.Container.DataProcessor_UInt16.ImageData(0).Data, cFITSWriter.eBitPix.Int16, FITSHeader.GetCardsAsList)
                End If
                '=============================================================================

                'Display and process capture status
                tsslLoopStatus.Text = "Capture " & imageCnt.ToString.Trim & "/" & DB.CaptureCount.ToString.Trim
                tsslCaptureProgress.Text = String.Format("Grabbed image {0} - {1} s,  {2} count, {3} offset", imageCnt, timeStamp.cycleSeconds, timeStamp.cycleCount, timeStamp.cycleOffset)

                'Calculate statistics
                If DB.CalcStatistics Then
                    DB.Histo.Show()
                    DB.Histo.CalcStat(DB.Container)
                End If

                'Update and exit on request
                System.Windows.Forms.Application.DoEvents()
                If StopNow = True Then Exit For

            Next imageCnt

            'Stop capturing images
            Cam.StopCapture()

            'Disconnect the camera
            Cam.Disconnect()

            'Idle and dump error listing
            tsslLoopStatus.Text = "--IDLE--"
            tsslCaptureProgress.Text = "--IDLE--"

            'Log errors
            If ErrorList.Count > 0 Then
                Dim ErrorLogFile As String = System.IO.Path.Combine(DB.StoragePath, "DLL_errors.log")
                System.IO.File.WriteAllLines(ErrorLogFile, ErrorList.ToArray)
                If DB.AutoOpenDLLErrorLog Then Process.Start(ErrorLogFile)
            End If

        End If

    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text &= " running as " & CStr(IIf(IntPtr.Size = 4, "x86", "x64"))
        pgMain.SelectedObject = DB
    End Sub

    Private Sub tsmiForm_OpenEXELocation_Click(sender As Object, e As EventArgs) Handles tsmiForm_OpenEXELocation.Click
        Process.Start(DB.EXEPath)
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        StopNow = True
    End Sub

    Private Sub LogFunctionCall(ByRef ErrList As List(Of String), ByVal Message As String)
        If String.IsNullOrEmpty(Message) = False Then ErrList.Add(Message)
    End Sub

    Private Sub tsmiForm_End_Click(sender As Object, e As EventArgs) Handles tsmiForm_End.Click
        End
    End Sub

    '''<summary>Add an entry to the log.</summary>
    '''<param name="Text">Log text to add.</param>
    Public Sub AddLog(ByVal Text As String)
        If tbLog.Text.Length = 0 Then
            tbLog.Text = Text
        Else
            tbLog.Text &= System.Environment.NewLine & Text
        End If
    End Sub

    '''<summary>Set all embedded parameters to a common state, excluding FrameCount, ROIPosition and TimeStamp.</summary>
    Private Sub SetEmbedded(ByRef Cam As FlyCapture2Managed.ManagedCamera, ByVal CommonState As Boolean)
        Dim embeddedInfo As FlyCapture2Managed.EmbeddedImageInfo = Cam.GetEmbeddedImageInfo()
        With embeddedInfo
            If .brightness.available Then .brightness.onOff = CommonState
            If .exposure.available Then .exposure.onOff = CommonState
            If .frameCounter.available Then .frameCounter.onOff = True
            If .gain.available Then .gain.onOff = CommonState
            If .GPIOPinState.available Then .GPIOPinState.onOff = CommonState
            If .ROIPosition.available Then .ROIPosition.onOff = True
            If .shutter.available Then .shutter.onOff = CommonState
            If .strobePattern.available Then .strobePattern.onOff = CommonState
            If .timestamp.available Then .timestamp.onOff = True
            If .whiteBalance.available Then .whiteBalance.onOff = CommonState
        End With
        Cam.SetEmbeddedImageInfo(embeddedInfo)
    End Sub

    '''<summary>Set Format7 with full ROI and 16 bit RAW format.</summary>
    Private Sub Set16BitFullROI(ByRef Cam As FlyCapture2Managed.ManagedCamera)
        Dim camInfo As FlyCapture2Managed.CameraInfo = Cam.GetCameraInfo()
        Dim Sensor_Width As UInteger = CType(Split(camInfo.sensorResolution, "x")(0), UInt16)
        Dim Sensor_Height As UInteger = CType(Split(camInfo.sensorResolution, "x")(1), UInt16)
        Dim Settings As New FlyCapture2Managed.Format7ImageSettings
        With Settings
            .mode = FlyCapture2Managed.Mode.Mode0
            .width = Sensor_Width
            .height = Sensor_Height
            .offsetX = 0
            .offsetY = 0
            .pixelFormat = FlyCapture2Managed.PixelFormat.PixelFormatMono16
        End With
        Dim recommendedPacketSize As UInteger = 47340
        Cam.SetFormat7Configuration(Settings, recommendedPacketSize)
    End Sub

    '''<summary>Try all PixelFormat for conversion.</summary>
    Private Sub TryAllFormats(ByRef RawData As FlyCapture2Managed.ManagedImage)
        Dim Checks As New Dictionary(Of FlyCapture2Managed.PixelFormat, String)
        For Each FormToTry As FlyCapture2Managed.PixelFormat In [Enum].GetValues(GetType(FlyCapture2Managed.PixelFormat))
            Using SaveImage As New FlyCapture2Managed.ManagedImage
                Try
                    RawData.Convert(FlyCapture2Managed.PixelFormat.PixelFormatSignedMono16, SaveImage)
                    Checks.Add(FormToTry, "--OK--")
                Catch ex As Exception
                    Checks.Add(FormToTry, "!!!" & ex.Message)
                End Try
            End Using
        Next FormToTry
    End Sub

End Class
