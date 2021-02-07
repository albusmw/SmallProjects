Option Explicit On
Option Strict On

Public Class Form1

    Private DB As New cDB

    Private NeverStarted As New DateTime(0)
    Private Never As New DateTime(DateTime.MaxValue.Ticks)

    Private LastTaken As DateTime = NeverStarted

    Private CaptureIdx As Integer = -1

    '''<summary>Name of the last saved JPG file.</summary>
    Private LastJPG As String = String.Empty
    '''<summary>Name of the last saved PNG file.</summary>
    Private LastPNG As String = String.Empty

    Dim LastLoggedDate As New DateTime(0)

    Private Function Chooser_Camera() As String
        Dim Chooser As New ASCOM.Utilities.Chooser
        Chooser.DeviceType = "Camera"
        DB.ASCOMCam = Chooser.Choose(DB.ASCOMCam)
        Return DB.ASCOMCam
    End Function

    Private Sub TakeImage(ByVal TimeStamp As DateTime)

        'Check pre condition
        If DB.SunHeight > DB.SunHeight_Day And DB.CaptureDuringDay = False Then
            tsslNoCapture.Text = "SUN TOO HIGH!"
            tsslNoCapture.BackColor = Color.Red
            Exit Sub
        End If

        tsslNoCapture.Text = "Preconditions OK"
        tsslNoCapture.BackColor = Color.Green

        Log("Connecting camera")
        If IsNothing(DB.Camera) Then
            DB.Camera = New ASCOM.DriverAccess.Camera(DB.ASCOMCam)
        End If
        DB.Camera.Connected = True
        Log("    DONE (" & DB.Camera.Description & ")")
        Log("    Gain range    : " & DB.Camera.GainMin.ToString.Trim & " ... " & DB.Camera.GainMax.ToString.Trim)
        Log("    Exposure range: " & DB.Camera.ExposureMin.ToString.Trim & " ... " & DB.Camera.ExposureMax.ToString.Trim)

        DB.Camera.BinX = 1
        DB.Camera.BinY = 1

        'Calculate settings
        Dim ExpToUse As Double = Double.NaN
        Dim GainToUse As Short = Short.MinValue
        If DB.SunHeight < DB.SunHeight_Night Then
            'Night
            ExpToUse = DB.Exp_Night
            GainToUse = DB.Gain_Night
        Else
            If DB.SunHeight > DB.SunHeight_Day Then
                'Day
                ExpToUse = DB.Exp_Day
                GainToUse = DB.Gain_Day
            Else
                'Dusk / Dawn
                Dim SunHeightRel As Double = (DB.SunHeight - DB.SunHeight_Night) / (DB.SunHeight_Day - DB.SunHeight_Night)  '0=night, 1=day
                ExpToUse = (DB.Exp_Night) + SunHeightRel * (DB.Exp_Day - DB.Exp_Night)
                GainToUse = CShort((DB.Gain_Night) + SunHeightRel * (DB.Gain_Day - DB.Gain_Night))
            End If
        End If

        'Set gain
        If GainToUse <> DB.GainNotSet Then
            If GainToUse > DB.Camera.GainMax Then GainToUse = DB.Camera.GainMax
            If GainToUse < DB.Camera.GainMin Then GainToUse = DB.Camera.GainMin
            DB.Camera.Gain = GainToUse
        End If
        Log("    Gain selected: " & DB.Camera.Gain.ToString.Trim)

        'Start exposing
        StartLog("Starting exposure with " & ExpToUse.ToString.Trim & " seconds")
        tsmiTakeOnePicture.Enabled = False : tsslCapture.BackColor = Color.Red : System.Windows.Forms.Application.DoEvents()
        DB.Camera.StartExposure(ExpToUse, False)
        Do
            If DB.Camera.ImageReady Then Exit Do
            System.Threading.Thread.Sleep(100)
            System.Windows.Forms.Application.DoEvents()
        Loop Until 1 = 0
        Dim CCDSensorData As Integer(,) = CType(DB.Camera.ImageArray, Integer(,))
        tsmiTakeOnePicture.Enabled = True : tsslCapture.BackColor = Color.Gray : System.Windows.Forms.Application.DoEvents()
        FinishLog("    DONE.")

        Dim PixelX As Integer = CCDSensorData.GetUpperBound(0) + 1
        Dim PixelY As Integer = CCDSensorData.GetUpperBound(1) + 1

        Log(PixelX.ToString.Trim & " x " & PixelY.ToString.Trim & " = " & (PixelX * PixelY).ToString.Trim & ", " & DB.Camera.SensorType.ToString)

        Dim BitmapToCreate As Bitmap
        Dim ColorPixelFormat As System.Drawing.Imaging.PixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb
        Dim MonoPixelFormat As System.Drawing.Imaging.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed
        If DB.Camera.SensorType <> ASCOM.DeviceInterface.SensorType.Monochrome Then
            BitmapToCreate = New Bitmap((CCDSensorData.GetUpperBound(0) + 1) \ 2, (CCDSensorData.GetUpperBound(1) + 1) \ 2, ColorPixelFormat)   'color mode
        Else
            BitmapToCreate = New Bitmap((CCDSensorData.GetUpperBound(0) + 1), (CCDSensorData.GetUpperBound(1) + 1), MonoPixelFormat)            'grayscale mode (we use 24bppRgb as well here - to be improved ...)
        End If

        'Prepare underlaying bitmap byte data and copy 
        Dim BitmapByteData As System.Drawing.Imaging.BitmapData = BitmapToCreate.LockBits(New Rectangle(0, 0, BitmapToCreate.Width, BitmapToCreate.Height), Drawing.Imaging.ImageLockMode.ReadWrite, BitmapToCreate.PixelFormat)
        Dim FirstLineAdr As IntPtr = BitmapByteData.Scan0
        Dim TotalByteCount As Integer = Math.Abs(BitmapByteData.Stride) * BitmapToCreate.Height
        Log(TotalByteCount.ToString.Trim & " byte for bitmap")

        Dim BitmapValues(TotalByteCount - 1) As Byte
        System.Runtime.InteropServices.Marshal.Copy(FirstLineAdr, BitmapValues, 0, TotalByteCount)

        'Start manipulation of the image - copy CCD data data from the camera to the image structure
        Dim CCDSensorBayerStep As Integer = 0
        Dim BytePerOutputPixel As Integer = 0       'RGB, each 1 byte
        Dim Scaler As Integer = 256                 'convert 16-bit colors to 8-bit
        Dim Min As Integer = Integer.MaxValue
        Dim Max As Integer = Integer.MinValue
        Dim Y As Integer = 0
        Dim X As Integer = 0
        If DB.Camera.SensorType <> ASCOM.DeviceInterface.SensorType.Monochrome Then
            'Color sensor
            CCDSensorBayerStep = 2
            BytePerOutputPixel = 3       'RGB, each 1 byte
            For Counter As Integer = 0 To BitmapValues.Length - 1 Step BytePerOutputPixel
                'Read the bayer matrix data (only use 3 of 4 pixel)
                Dim R As Byte = CByte(Math.Floor(CCDSensorData(Y + 1, X) / Scaler)) : SetMinMax(CCDSensorData(Y + 1, X), Min, Max)
                Dim G As Byte = CByte(Math.Floor(CCDSensorData(Y, X) / Scaler)) : SetMinMax(CCDSensorData(Y, X), Min, Max)
                Dim B As Byte = CByte(Math.Floor(CCDSensorData(Y, X + 1) / Scaler)) : SetMinMax(CCDSensorData(Y, X + 1), Min, Max)
                'Transfer the data to the byte array
                BitmapValues(Counter) = B
                BitmapValues(Counter + 1) = R
                BitmapValues(Counter + 2) = G
                Y += CCDSensorBayerStep
                If Y > CCDSensorData.GetUpperBound(0) Then
                    Y = 0
                    X += CCDSensorBayerStep
                End If
            Next Counter
        Else
            'Monochromatic sensor
            CCDSensorBayerStep = 1
            BytePerOutputPixel = 1
            For Counter As Integer = 0 To BitmapValues.Length - 1 Step BytePerOutputPixel
                'Read the grayscale data
                Dim Value As Byte = CByte(Math.Floor(CCDSensorData(Y, X) / Scaler)) : SetMinMax(CCDSensorData(Y, X), Min, Max)
                'Transfer the data to the byte array
                BitmapValues(Counter) = Value
                Y += CCDSensorBayerStep
                If Y > CCDSensorData.GetUpperBound(0) Then
                    Y = 0
                    X += CCDSensorBayerStep
                End If
            Next Counter
        End If
        Log("    MIN value: " & Min.ToString.Trim)
        Log("    MAX value: " & Max.ToString.Trim)

        'Add text by setting pixel to max
        Dim Inprint As New ImageInprint.sInprintParams
        With Inprint
            Dim AllLines As New List(Of String)
            AllLines.Add(DB.Inprint_station)
            AllLines.Add("Exp [" & ExpToUse.ValRegIndep("0.0000") & "] / Gain [" & GainToUse & "]")
            .TextToPrint = Join(AllLines.ToArray, System.Environment.NewLine)
            .TextSize = DB.Inprint_FontSize
            .PixelValue = CByte(Math.Floor(Max / Scaler))
            .PrintRight = False
        End With
        ImageInprint.Mono8BitText(BitmapToCreate, BitmapValues, Inprint)
        With Inprint
            Dim AllLines As New List(Of String)
            AllLines.Add(Format(Now, "dd.MM.yyyy_HH.mm.ss"))
            AllLines.Add("Sun height: " & DB.SunHeight.ValRegIndep("0.00"))
            .TextToPrint = Join(AllLines.ToArray, System.Environment.NewLine)
            .PrintRight = True
        End With
        ImageInprint.Mono8BitText(BitmapToCreate, BitmapValues, Inprint)

        'Get the image data from the RGB values
        System.Runtime.InteropServices.Marshal.Copy(BitmapValues, 0, FirstLineAdr, TotalByteCount)
        BitmapToCreate.UnlockBits(BitmapByteData)

        'Create a grayscale palette
        If DB.SaveIndexedGrayscale = True Then
            If DB.Camera.SensorType = ASCOM.DeviceInterface.SensorType.Monochrome Then
                Dim GrayScale As Imaging.ColorPalette = BitmapToCreate.Palette
                For Idx As Integer = 0 To GrayScale.Entries.Length - 1
                    GrayScale.Entries(Idx) = Color.FromArgb(Idx, Idx, Idx)
                Next Idx
                BitmapToCreate.Palette = GrayScale
            End If
        End If

        'Set the image to be displayed
        pbLastImage.Image = BitmapToCreate

        'Save image
        Dim BaseFileName As String = DB.CurrentImageName
        Dim FullPathName = IO.Path.Combine(DB.StorageRoot, BaseFileName)
        If DB.SaveAsJPG = True Then
            LastJPG = FullPathName & ".jpg"
            StartLog("Saving image to <" & LastJPG & ">")
            BitmapToCreate.Save(LastJPG, System.Drawing.Imaging.ImageFormat.Jpeg)
        End If
        If DB.SaveAsPNG = True Then
            LastPNG = FullPathName & ".png"
            StartLog("Saving image to <" & LastPNG & ">")
            BitmapToCreate.Save(LastPNG, System.Drawing.Imaging.ImageFormat.Png)
        End If

        'Copy to timeline
        If TimeStamp <> Never Then
            Dim TimeFormat As String = String.Empty
            Try
                TimeFormat = Format(TimeStamp, DB.FileNameFormat)
            Catch ex As Exception
                'Do nothing
            End Try
            If DB.FileNameFormat.Contains("%") Then TimeFormat = Format(CaptureIdx, DB.FileNameFormat.Replace("%", "0"))
            If DB.SaveAsJPG = True Then
                System.IO.File.Copy(LastJPG, IO.Path.Combine(DB.StorageRoot, BaseFileName) & TimeFormat & ".jpg")
            End If
            If DB.SaveAsPNG = True Then
                System.IO.File.Copy(LastPNG, IO.Path.Combine(DB.StorageRoot, BaseFileName) & TimeFormat & ".png")
            End If
        End If

        FinishLog("    DONE.")

        StartLog("Disconnecting camera")
        DB.Camera.Connected = False
        FinishLog("    DONE.")
        Log("==============================================")

    End Sub

    Private Sub SetMinMax(ByVal Value As Integer, ByRef Min As Integer, ByRef Max As Integer)
        If Value > Max Then Max = Value
        If Value < Min Then Min = Value
    End Sub

    Private Sub Log(ByVal NewText As String)
        StartLog(NewText)
        FinishLog(String.Empty)
    End Sub

    Private Sub StartLog(ByVal NewText As String)
        ShowNewDate()
        'Log message
        With tbLog
            .Text &= Format(Now, "HH:mm:ss") & "|" & NewText
            .SelectionStart = .Text.Length
            .ScrollToCaret()
        End With
    End Sub

    Private Sub FinishLog(ByVal NewText As String)
        'Log message
        With tbLog
            .Text &= NewText & System.Environment.NewLine
            .SelectionStart = .Text.Length
            .ScrollToCaret()
        End With
    End Sub

    Private Function ShowNewDate() As Boolean
        'Log date if date is changed
        Dim CurrentDate As DateTime = Now
        If LastLoggedDate.Year <> CurrentDate.Year Or LastLoggedDate.Month <> CurrentDate.Month Or LastLoggedDate.Day <> CurrentDate.Day Then
            tbLog.Text &= "##########################" & System.Environment.NewLine & Format(CurrentDate, "yyyy-MM-dd") & System.Environment.NewLine & "##########################" & System.Environment.NewLine
            LastLoggedDate = CurrentDate
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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

        pgMain.SelectedObject = DB

    End Sub

    Private Sub UploadFile(ByVal File As String)

        'File generated: http://www.albusmw.de/transfer/AllSkyImage.jpg

        Dim FTP_Server As String = "ftp.strato.de"
        Dim FTP_User As String = "ftp_AllSky@albusmw.de"
        Dim FTP_Pwd As String = "qbZHD57224"

        'Get the object used to communicate with the server.  
        Dim RemoteFilename As String = "ftp://www.albusmw.de/" & DB.CurrentImageName & ".jpg"
        Dim request As Net.FtpWebRequest = CType(Net.WebRequest.Create(RemoteFilename), Net.FtpWebRequest)
        request.Method = Net.WebRequestMethods.Ftp.UploadFile

        'This example assumes the FTP site uses anonymous logon.  
        request.Credentials = New Net.NetworkCredential(FTP_User, FTP_Pwd)

        'Copy the contents of the file to the request stream.  
        Dim sourceStream As New IO.StreamReader(File)
        Dim fileContents As Byte() = System.Text.Encoding.UTF8.GetBytes(sourceStream.ReadToEnd())
        sourceStream.Close()
        request.ContentLength = fileContents.Length

        Dim requestStream As IO.Stream = request.GetRequestStream()
        requestStream.Write(fileContents, 0, fileContents.Length)
        requestStream.Close()

        Dim response As Net.FtpWebResponse = CType(request.GetResponse(), Net.FtpWebResponse)

        Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription)

        response.Close()

    End Sub

    Private Function ValRegIndep(ByVal Text As String) As Double
        Return Val(Text.Replace(",", "."))
    End Function

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub SelectCameraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectCameraToolStripMenuItem.Click
        DB.ASCOMCam = Chooser_Camera()
        Log("Selected: " & DB.ASCOMCam)
    End Sub

    Private Sub OpenLastImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLastImageToolStripMenuItem.Click
        If System.IO.File.Exists(LastJPG) = True And System.IO.File.Exists(LastPNG) = False Then Process.Start(LastJPG)
        If System.IO.File.Exists(LastJPG) = False And System.IO.File.Exists(LastPNG) = True Then Process.Start(LastPNG)
        If System.IO.File.Exists(LastJPG) = True And System.IO.File.Exists(LastPNG) = True Then
            If MsgBox("YES for JPG, NO for PNG", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                Process.Start(LastJPG)
            Else
                Process.Start(LastPNG)
            End If
        End If
    End Sub

    Private Sub FTPUploadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiFTPUpload.Click
        UploadFile(IO.Path.Combine(DB.StorageRoot, DB.CurrentImageName & ".jpg"))
    End Sub

    Private Sub tCheckExpState_Tick(sender As Object, e As EventArgs) Handles tCheckExpState.Tick

        'Initial start
        If LastTaken = NeverStarted And DB.CaptureInterval > 0.0 Then
            CaptureIdx = 0
            LastTaken = Now
            TakeImage(LastTaken)
        End If

        'Next Shot
        If DB.CaptureInterval > 0.0 Then
            If Now >= LastTaken.AddSeconds(DB.CaptureInterval) Then
                LastTaken = LastTaken.AddSeconds(DB.CaptureInterval)
                CaptureIdx += 1
                TakeImage(Now)
            End If
        End If

        'Stop
        If DB.CaptureInterval = 0 Then
            LastTaken = NeverStarted
        End If

        'Init sun parameters
        If DB.SunSet = DateTime.MinValue Then
            DB.CalcSunParam()
            pgMain.SelectedObject = DB
        End If

        DB.CalcSunPos()
        tsslSunPos.Text = "Sun height: " & DB.SunHeight.ValRegIndep("0.0")

    End Sub

    Private Sub OpenStoragePathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenStoragePathToolStripMenuItem.Click
        If System.IO.Directory.Exists(DB.StorageRoot) Then Process.Start(DB.StorageRoot)
    End Sub

    Private Sub JoinToVideoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JoinToVideoToolStripMenuItem.Click
        Dim FFMPEG As New ProcessStartInfo
        With FFMPEG
            .FileName = DB.FFMPEGEXE
            .WorkingDirectory = System.IO.Path.GetDirectoryName(DB.FFMPEGEXE)
            .Arguments = "-start_number 0 -f image2 -i " & DB.StorageRoot & "\AllSkyImage15.09.2020_%04d.jpg -codec:v libx264 C:\temp\output.mp4"
            .UseShellExecute = True
            .RedirectStandardError = False
        End With
        Dim Runner As Process = Process.Start(FFMPEG)
        Runner.WaitForExit()
        'MsgBox(Runner.StandardError.ReadToEnd)
    End Sub

    Private Sub MyCaptureDefaultsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MyCaptureDefaultsToolStripMenuItem.Click
        With DB
            .CaptureInterval = 30
            .SaveAsPNG = True
            .SaveAsJPG = False
            .FileNameFormat = "%%%%"
        End With
    End Sub

    Private Sub tsmiTakeOnePicture_Click(sender As Object, e As EventArgs) Handles tsmiTakeOnePicture.Click
        TakeImage(Never)
    End Sub

    Private Sub tsmiFile_ClearLog_Click(sender As Object, e As EventArgs) Handles tsmiFile_ClearLog.Click
        tbLog.Text = String.Empty
    End Sub

End Class
