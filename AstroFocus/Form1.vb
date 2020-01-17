Option Explicit On
Option Strict On

Public Class frmMain

    Dim DB As New cDB

    Private Sub SelectFocuserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectFocuserToolStripMenuItem.Click

        DB.ASCOM_Focuser_Chooser.DeviceType = "Focuser"
        Dim Selected As String = DB.ASCOM_Focuser_Chooser.Choose()
        DB.ASCOM_Focuser = New ASCOM.DriverAccess.Focuser(Selected)
        Log("Selected Focuser: <" & DB.ASCOM_Focuser.Name & ">")
        Log("                  <" & Selected & ">")
        DB.ASCOM_Focuser.Connected = True

    End Sub

    Private Sub SelectCameraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectCameraToolStripMenuItem.Click

        DB.ASCOM_Camera_Chooser.DeviceType = "Camera"
        Dim Selected As String = DB.ASCOM_Camera_Chooser.Choose()
        DB.ASCOM_Camera = New ASCOM.DriverAccess.Camera(Selected)
        Log("Selected Camera: <" & DB.ASCOM_Camera.Name & ">")
        Log("                 <" & Selected & ">")
        DB.ASCOM_Camera.Connected = True

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        Dim FocusString As String = String.Empty
        Dim Filename As String = String.Empty
        Dim FITSHeaderElements As New List(Of String())

        DB.LogFolder = System.IO.Path.Combine(DB.MyPath, Format(Now, "yyyy-MM-dd_HH-mm-ss"))
        If System.IO.Directory.Exists(DB.LogFolder) = False Then System.IO.Directory.CreateDirectory(DB.LogFolder)
        Log("Log folder: " & DB.LogFolder)

        'Exit on missing devices
        If IsNothing(DB.ASCOM_Focuser) = True Then
            Log("!!! NO FOCUSER SELECTED!")
            Exit Sub
        End If
        If IsNothing(DB.ASCOM_Camera) = True Then
            Log("!!! NO CAMERA SELECTED!")
            Exit Sub
        End If

        'Connect if not connected
        If DB.ASCOM_Focuser.Connected = False Then
            Log("Connecting to focuser ....")
            DB.ASCOM_Focuser.Connected = True
            LogOK()
        End If
        If DB.ASCOM_Camera.Connected = False Then
            Log("Connecting to camera ....")
            DB.ASCOM_Camera.Connected = True
            LogOK()
        End If

        DB.StopFlag = False
        btnStart.Enabled = False
        btnStop.Enabled = True
        DE()

        'Correct hardware parameters
        If DB.Gain > DB.ASCOM_Camera.GainMax Then DB.Gain = DB.ASCOM_Camera.GainMax
        If DB.Gain < DB.ASCOM_Camera.GainMin Then DB.Gain = DB.ASCOM_Camera.GainMin

        'Add some information
        Log("Focus process started")
        Log("Focuser   : " & DB.ASCOM_Focuser.Name)
        Log("  Position: " & CStr(DB.ASCOM_Focuser.Position.ToString.Trim))
        Log("Camera    : " & DB.ASCOM_Camera.Name)
        Log("  FullWell: " & DB.ASCOM_Camera.FullWellCapacity.ToString.Trim)

        'Move to start position
        Log("Moving focuser to start position")
        FocusString = FocusTo(DB.StartPosition)
        Log("  -> " & FocusString)

        'Prepare camera
        Log("Preparing camera for initial shot")
        With DB.ASCOM_Camera
            'ROI all
            .StartX = 0
            .StartY = 0
            .NumX = .CameraXSize - 1
            .NumY = .CameraYSize - 1
            .Gain = DB.Gain
        End With
        Log("  Gain: " & DB.ASCOM_Camera.Gain.ToString.Trim)
        LogOK()

        'Expose
        Log("Run initial exposure")
        Dim ImageDataInt32(,) As Int32 = Expose()

        'Write initial FITS image
        Log("Writing full image")
        FITSHeaderElements.Clear()
        FITSHeaderElements.AddRange(GetStandardFITSHeader)
        FITSHeaderElements.Add(New String() {eFITSKeywords.EXPTIME, CStr(DB.ExpTime.ToString.Trim.Replace(",", "."))})                            'exposure time
        FITSHeaderElements.Add(New String() {eFITSKeywords.GAIN, CStr(DB.ASCOM_Camera.Gain.ToString.Trim.Replace(",", "."))})                     'GAIN is not a standard FITS header keyword ...
        FITSHeaderElements.Add(New String() {eFITSKeywords.FOCUS, FocusString})                                                                   'focuser position
        FITSHeaderElements.Add(New String() {eFITSKeywords.SETTEMP, CStr(DB.ASCOM_Camera.CCDTemperature)})                                        'CCD temperature current
        'FITSHeaderElements.Add(New String() {eFITSKeywords.BAYERPAT, "1"})                                                                        'the camera has a bayer pattern
        'FITSHeaderElements.Add(New String() {eFITSKeywords.COLORTYP, "RGGB"})                                                                     'bayer pattern
        Filename = DB.LogFolder & "\FOCUS_FULL_" & FocusString & ".FITS"
        cFITSWriter.Write(Filename, ImageDataInt32, cFITSWriter.eBitPix.Int32, FITSHeaderElements)
        Log("  -> " & Filename)

        '======================================================================================================
        Log("Calculating ROI for focus shots")

        'Find peak position
        Dim PeakVal As Int32
        Dim PeakPosX As Integer
        Dim PeakPosY As Integer
        FindPeak(ImageDataInt32, PeakPosX, PeakPosY, PeakVal)
        Log("  -> Peak value: " & PeakVal.ToString.Trim & " @ <" & PeakPosX.ToString.Trim & ":" & PeakPosY.ToString.Trim & ">")

        'Confirm ROI if selected
        If DB.ROIConfirm = True Then
            Process.Start(Filename)
            DB.ROICenterX = CInt(InputBox("Please select ROICenterX (auto-detect was: <" & PeakPosX.ToString.Trim & ":" & PeakPosY.ToString.Trim & ">", PeakPosX.ToString.Trim))
            DB.ROICenterY = CInt(InputBox("Please select ROICenterX (auto-detect was: <" & PeakPosX.ToString.Trim & ":" & PeakPosY.ToString.Trim & ">", PeakPosX.ToString.Trim))
        End If

        'Calculate and set ROI
        If DB.ROICenterX <> -1 Then PeakPosX = DB.ROICenterX
        If DB.ROICenterY <> -1 Then PeakPosY = DB.ROICenterY
        ConfigureROI(ImageDataInt32, PeakPosX, PeakPosY, DB.ROIDelta)
        Log("  -> ROI center: " & ImageDataInt32(PeakPosX, PeakPosY).ToString.Trim & " @ <" & PeakPosX.ToString.Trim & ":" & PeakPosY.ToString.Trim & ">")
        LogOK()
        '======================================================================================================

        'Running focus sequence
        For FocusIdx As Integer = DB.StartPosition To DB.EndPosition Step DB.StepSize

            Log("Moving focuser to position <" & FocusIdx & ">")
            FocusString = FocusTo(FocusIdx)
            Log("  -> " & FocusString)

            With DB.ASCOM_Camera
                .AbortExposure()
                .StopExposure()
            End With
            ImageDataInt32 = Expose()

            'Write initial FITS image
            Log("Writing focus image <" & FocusString & ">")

            FITSHeaderElements.AddRange(GetStandardFITSHeader)
            FITSHeaderElements.Add(New String() {eFITSKeywords.EXPTIME, CStr(DB.ExpTime.ToString.Trim.Replace(",", "."))})                            'exposure time
            FITSHeaderElements.Add(New String() {eFITSKeywords.GAIN, CStr(DB.ASCOM_Camera.Gain.ToString.Trim.Replace(",", "."))})                     'GAIN is not a standard FITS header keyword ...
            FITSHeaderElements.Add(New String() {eFITSKeywords.FOCUS, FocusString})                                                                   'focuser position
            FITSHeaderElements.Add(New String() {eFITSKeywords.SETTEMP, CStr(DB.ASCOM_Camera.CCDTemperature)})                                        'CCD temperature current
            'FITSHeaderElements.Add(New String() {eFITSKeywords.BAYERPAT, "1"})                                                                        'the camera has a bayer pattern
            'FITSHeaderElements.Add(New String() {eFITSKeywords.COLORTYP, "RGGB"})                                                                     'bayer pattern
            Filename = DB.LogFolder & "\FOCUS_ROI_" & FocusString & ".FITS"
            cFITSWriter.Write(Filename, ImageDataInt32, cFITSWriter.eBitPix.Int32, FITSHeaderElements)
            Log("  -> " & Filename)

            'Write as PNG
            If DB.SavePNG = True Then cImageDisplay.CalculateImageFromData(ImageDataInt32).BitmapToProcess.Save(PNGFileName(Filename))

            If DB.StopFlag = True Then
                DB.StopFlag = False
                btnStart.Enabled = True
                btnStop.Enabled = False
                DE()
                Exit For
            End If

        Next FocusIdx

        System.IO.File.WriteAllText(DB.LogFolder & "\FOCUS.LOG", tbLogOutput.Text)
        DisconnectAll()

    End Sub

    Private Function PNGFileName(ByVal FileName As String) As String
        Return System.IO.Path.GetFileNameWithoutExtension(FileName) & ".PNG"
    End Function

    Private Sub FindPeak(ByRef ImageDataInt32(,) As Int32, ByRef PeakPosX As Integer, ByRef PeakPosY As Integer, ByRef PeakVal As Int32)
        PeakVal = Int32.MinValue
        PeakPosX = Integer.MinValue
        PeakPosY = Integer.MinValue
        For IdxX As Integer = 0 To ImageDataInt32.GetUpperBound(0)
            For IdxY As Integer = 0 To ImageDataInt32.GetUpperBound(1)
                If ImageDataInt32(IdxX, IdxY) > PeakVal Then
                    PeakVal = ImageDataInt32(IdxX, IdxY)
                    PeakPosX = IdxX
                    PeakPosY = IdxY
                End If
            Next IdxY
        Next IdxX
    End Sub

    Private Sub ConfigureROI(ByRef ImageDataInt32(,) As Int32, ByRef PeakPosX As Integer, ByRef PeakPosY As Integer, ByRef ROIDelta As Integer)
        Dim ROISize As Integer = ROIDelta + ROIDelta + 1
        Dim ROI_X_Start As Integer = PeakPosX - ROIDelta : If ROI_X_Start < 0 Then ROI_X_Start = 0
        Dim ROI_Y_Start As Integer = PeakPosY - ROIDelta : If ROI_Y_Start < 0 Then ROI_Y_Start = 0
        Dim ROI_X_Stop As Integer = ROI_X_Start + ROISize
        Dim ROI_Y_Stop As Integer = ROI_Y_Start + ROISize
        If ROI_X_Stop > ImageDataInt32.GetUpperBound(0) Then
            ROI_X_Stop = ImageDataInt32.GetUpperBound(0) : ROI_X_Start = ROI_X_Stop - ROISize
        End If
        If ROI_Y_Stop > ImageDataInt32.GetUpperBound(1) Then
            ROI_Y_Stop = ImageDataInt32.GetUpperBound(1) : ROI_Y_Start = ROI_Y_Stop - ROISize
        End If
        'Set camera ROI
        With DB.ASCOM_Camera
            .StartX = ROI_X_Start
            .StartY = ROI_Y_Start
            .NumX = ROISize
            .NumY = ROISize
        End With
    End Sub

    Private Function Expose() As Int32(,)
        DB.ASCOM_Camera.AbortExposure()
        DB.ASCOM_Camera.StopExposure()
        DB.ExpStopWatch.Reset() : DB.ExpStopWatch.Start()
        DB.ASCOM_Camera.StartExposure(DB.ExpTime, True)
        WaitForExpStop()
        DB.ExpStopWatch.Stop()
        Log("  -> Done in " & DB.ExpStopWatch.ElapsedMilliseconds.ToString.Trim & " ms")

        'Read data from the cam
        Log("Reading camera data")
        Dim ImageDataInt32(,) As Int32 = CType(DB.ASCOM_Camera.ImageArray, Int32(,))
        BayerFunctions.EqualizeChannels(ImageDataInt32)
        LogOK()
        Return ImageDataInt32
    End Function

    Private Function GetStandardFITSHeader() As List(Of String())
        Dim CustomHeaderElements As New List(Of String())
        CustomHeaderElements.Add(New String() {eFITSKeywords.OBJECT, "AUTO_FOCUS"})                                                                 'verbose object name (e.g. "M45")
        CustomHeaderElements.Add(New String() {eFITSKeywords.OBSERVER, "AstroFocus"})                                                               'observer
        CustomHeaderElements.Add(New String() {eFITSKeywords.TELESCOP, "PlaneWave CDK 12.5"})                                                       'observing instrument
        CustomHeaderElements.Add(New String() {eFITSKeywords.INSTRUME, "ZWO ASI094MC Pro"})                                                         'observing camera
        CustomHeaderElements.Add(New String() {eFITSKeywords.TELFOC, "2541"})
        CustomHeaderElements.Add(New String() {eFITSKeywords.TELAPER, "0.318"})
        CustomHeaderElements.Add(New String() {eFITSKeywords.DATE_OBS, cFITSKeywords.GetDateWithTime(Now)})                                         'observation start time
        CustomHeaderElements.Add(New String() {eFITSKeywords.RA, DB.RecJ2000})
        CustomHeaderElements.Add(New String() {eFITSKeywords.DEC, DB.DecJ2000.ToString.Trim.Replace(",", ".")})
        Return CustomHeaderElements
    End Function

    Private Function FocusTo(ByVal Position As Integer) As String
        DB.ASCOM_Focuser.Move(Position) : WaitForFocusStop()
        Return CStr(DB.ASCOM_Focuser.Position.ToString.Trim)
    End Function

    Private Function FocusStep() As String
        DB.ASCOM_Focuser.Action("", "") : WaitForFocusStop()
        Return CStr(DB.ASCOM_Focuser.Position.ToString.Trim)
    End Function

    Private Function WaitForFocusStop() As Boolean
        Dim SleepDuration As Integer = 10
        Dim FinalWait As Integer = 100
        Do
            System.Threading.Thread.Sleep(SleepDuration)
            System.Windows.Forms.Application.DoEvents()
        Loop Until DB.ASCOM_Focuser.IsMoving = False
        System.Threading.Thread.Sleep(FinalWait)
        Return True
    End Function

    Private Function WaitForExpStop() As Boolean
        Dim SleepDuration As Integer = 10
        Do
            System.Threading.Thread.Sleep(SleepDuration)
            DE()
        Loop Until DB.ASCOM_Camera.ImageReady = True
        Return True
    End Function

    Private Sub LogOK()
        Log("  -> OK")
    End Sub

    Private Sub Log(ByVal Text As String)
        Text = Format(Now, "HH.mm.ss:fff") & "|" & Text
        DB.Log.AppendLine(Text)
        With tbLogOutput
            .Text = DB.Log.ToString
            .SelectionStart = .Text.Length - 1
            .SelectionLength = 0
            .ScrollToCaret()
        End With
        DE()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub DisconnectAll()
        If IsNothing(DB.ASCOM_Focuser) = False Then
            If DB.ASCOM_Focuser.Connected = True Then
                Log("Disconnecting focuser ....")
                DB.ASCOM_Focuser.Connected = False
                LogOK()
            End If
        End If
        If IsNothing(DB.ASCOM_Camera) = False Then
            If DB.ASCOM_Camera.Connected = True Then
                Log("Disconnecting camera ....")
                DB.ASCOM_Camera.Connected = False
                LogOK()
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        DisconnectAll()
        End
    End Sub

    Private Sub OpenEXEPathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenEXEPathToolStripMenuItem.Click
        Process.Start(DB.MyPath)
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pgMain.SelectedObject = DB
        DB.ASCOM_Focuser = New ASCOM.DriverAccess.Focuser("ASCOM.FocusLynx.Focuser")
        DB.ASCOM_Camera = New ASCOM.DriverAccess.Camera("ASCOM.ASICamera2_2.Camera")
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        DB.StopFlag = True
    End Sub

End Class
