Option Explicit On
Option Strict On

Public Class Form1

    Const GainNotSet As Short = Short.MinValue + 1

    Dim SelectedCamera As String = "ASCOM.ASICamera2.Camera"
    Dim FileName As String
    Dim LastLoggedDate As New DateTime(0)
    Dim SelectedGain As Short = GainNotSet
    Dim CurrentImageName As String = "AllSkyImage"

    Private Sub btnAscomCamera_Click(sender As Object, e As EventArgs) Handles btnAscomCamera.Click
        tbSelectedCam.Text = Chooser_Camera()
        Log("Selected: " & tbSelectedCam.Text)
    End Sub

    Private Function Chooser_Camera() As String
        Dim Chooser As New ASCOM.Utilities.Chooser
        Chooser.DeviceType = "Camera"
        SelectedCamera = Chooser.Choose(SelectedCamera)
        Return SelectedCamera
    End Function

    Private Sub btnTakeImage_Click(sender As Object, e As EventArgs) Handles btnTakeImage.Click

        Log("Connecting camera")
        Dim Camera As New ASCOM.DriverAccess.Camera(SelectedCamera)
        Camera.Connected = True
        Log("    DONE (" & Camera.Description & ")")
        Log("    Gain range    : " & Camera.GainMin.ToString.Trim & " ... " & Camera.GainMax.ToString.Trim)
        Log("    Exposure range: " & Camera.ExposureMin.ToString.Trim & " ... " & Camera.ExposureMax.ToString.Trim)

        Camera.BinX = 1
        Camera.BinY = 1

        'Set gain
        If SelectedGain <> GainNotSet Then
            If SelectedGain > Camera.GainMax Then SelectedGain = Camera.GainMax
            If SelectedGain < Camera.GainMin Then SelectedGain = Camera.GainMin
            Camera.Gain = SelectedGain
        End If
        Log("    Gain selected: " & Camera.Gain.ToString.Trim)

        StartLog("Starting exposure with " & tbExposeTime.Text.ToString.Trim & " seconds")
        btnTakeImage.Enabled = False : System.Windows.Forms.Application.DoEvents()
        Camera.StartExposure(Val(tbExposeTime.Text.Replace(",", ".")), False)
        Do
            If Camera.ImageReady Then Exit Do
        Loop Until 1 = 0
        Dim CCDSensorData As Integer(,) = CType(Camera.ImageArray, Integer(,))
        btnTakeImage.Enabled = True : System.Windows.Forms.Application.DoEvents()
        FinishLog("    DONE.")

        Dim PixelX As Integer = CCDSensorData.GetUpperBound(0) + 1
        Dim PixelY As Integer = CCDSensorData.GetUpperBound(1) + 1

        Log(PixelX.ToString.Trim & " x " & PixelY.ToString.Trim & " = " & (PixelX * PixelY).ToString.Trim & ", " & Camera.SensorType.ToString)

        Dim BitmapToCreate As Bitmap
        If Camera.SensorType <> ASCOM.DeviceInterface.SensorType.Monochrome Then
            BitmapToCreate = New Bitmap((CCDSensorData.GetUpperBound(0) + 1) \ 2, (CCDSensorData.GetUpperBound(1) + 1) \ 2, System.Drawing.Imaging.PixelFormat.Format24bppRgb)      'color mode
        Else
            BitmapToCreate = New Bitmap((CCDSensorData.GetUpperBound(0) + 1), (CCDSensorData.GetUpperBound(1) + 1), System.Drawing.Imaging.PixelFormat.Format8bppIndexed)              'grayscale mode (we use 24bppRgb as well here - to be improved ...)
        End If

        'Prepare underlaying bitmap byte data and copy 
        Dim BitmapByteData As System.Drawing.Imaging.BitmapData = BitmapToCreate.LockBits(New Rectangle(0, 0, BitmapToCreate.Width, BitmapToCreate.Height), Drawing.Imaging.ImageLockMode.ReadWrite, BitmapToCreate.PixelFormat)
        Dim FirstLineAdr As IntPtr = BitmapByteData.Scan0
        Dim TotalByteCount As Integer = Math.Abs(BitmapByteData.Stride) * BitmapToCreate.Height
        Log(TotalByteCount.ToString.Trim & " byte for bitmap")

        Dim RGBValues(TotalByteCount - 1) As Byte
        System.Runtime.InteropServices.Marshal.Copy(FirstLineAdr, RGBValues, 0, TotalByteCount)

        'Start manipulation of the image - copy CCD data data from the camera to the image structure
        Dim CCDSensorBayerStep As Integer = 0
        Dim BytePerOutputPixel As Integer = 0      'RGB, each 1 byte
        Dim Scaler As Integer = 256
        Dim Min As Integer = Integer.MaxValue
        Dim Max As Integer = Integer.MinValue
        Dim Y As Integer = 0
        Dim X As Integer = 0
        If Camera.SensorType <> ASCOM.DeviceInterface.SensorType.Monochrome Then
            'Color sensor
            CCDSensorBayerStep = 2
            BytePerOutputPixel = 3       'RGB, each 1 byte
            For Counter As Integer = 0 To RGBValues.Length - 1 Step BytePerOutputPixel
                'Read the bayer matrix data (only use 3 of 4 pixel)
                Dim R As Byte = CByte(Math.Floor(CCDSensorData(Y + 1, X) / Scaler))
                Dim G As Byte = CByte(Math.Floor(CCDSensorData(Y, X) / Scaler))
                Dim B As Byte = CByte(Math.Floor(CCDSensorData(Y, X + 1) / Scaler))
                'Transfer the data to the byte array
                RGBValues(Counter) = B
                RGBValues(Counter + 1) = R
                RGBValues(Counter + 2) = G
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
            For Counter As Integer = 0 To RGBValues.Length - 1 Step BytePerOutputPixel
                'Read the grayscale data
                Dim Value As Byte = CByte(Math.Floor(CCDSensorData(Y, X) / Scaler))
                'Transfer the data to the byte array
                RGBValues(Counter) = Value
                'RGBValues(Counter + 1) = Value
                'RGBValues(Counter + 2) = Value
                Y += CCDSensorBayerStep
                If Y > CCDSensorData.GetUpperBound(0) Then
                    Y = 0
                    X += CCDSensorBayerStep
                End If
            Next Counter
        End If

        System.Runtime.InteropServices.Marshal.Copy(RGBValues, 0, FirstLineAdr, TotalByteCount)
        BitmapToCreate.UnlockBits(BitmapByteData)

        'Create a grayscale palette
        If Camera.SensorType = ASCOM.DeviceInterface.SensorType.Monochrome Then
            Dim GrayScale As Imaging.ColorPalette = BitmapToCreate.Palette
            For Idx As Integer = 0 To 255
                GrayScale.Entries(Idx) = Color.FromArgb(Idx, Idx, Idx)
            Next Idx
            BitmapToCreate.Palette = GrayScale
        End If

        'Set the image to be displayed
        pbLastImage.Image = BitmapToCreate

        'Save image
        Dim BaseFileName As String = CurrentImageName
        FileName = tbStorageRoot.Text & "\" & BaseFileName
        StartLog("Saving image to <" & BaseFileName & ".png" & ">")
        BitmapToCreate.Save(FileName & ".png", System.Drawing.Imaging.ImageFormat.Png)
        StartLog("Saving image to <" & BaseFileName & ".jpg" & ">")
        BitmapToCreate.Save(FileName & ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
        FinishLog("    DONE.")

        StartLog("Disconnecting camera")
        Camera.Connected = False
        FinishLog("    DONE.")
        Log("==============================================")

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

    Private Sub btnOpenImage_Click(sender As Object, e As EventArgs) Handles btnOpenImage.Click
        If MsgBox("YES for JPEG, NO for PNG", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
            Process.Start(FileName & ".jpg")
        Else
            Process.Start(FileName & ".png")
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tbStorageRoot.Text = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    End Sub

    Private Sub btnUp10_Click(sender As Object, e As EventArgs) Handles btnUp10.Click
        tbExposeTime.Text = CStr(Val(tbExposeTime.Text) * 10).Trim.Replace(",", ".")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        tbExposeTime.Text = CStr(Val(tbExposeTime.Text) / 10).Trim.Replace(",", ".")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        tbExposeTime.Text = CStr(Val(tbExposeTime.Text) / 2).Trim.Replace(",", ".")
    End Sub

    Private Sub btnUp2_Click(sender As Object, e As EventArgs) Handles btnUp2.Click
        tbExposeTime.Text = CStr(Val(tbExposeTime.Text) * 2).Trim.Replace(",", ".")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SelectedGain = Short.MaxValue
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SelectedGain = Short.MinValue
    End Sub

    Private Sub UploadFile(ByVal File As String)

        'File generated: http://www.albusmw.de/transfer/AllSkyImage.jpg

        Dim FTP_Server As String = "ftp.strato.de"
        Dim FTP_User As String = "ftp_AllSky@albusmw.de"
        Dim FTP_Pwd As String = "qbZHD57224"

        'Get the object used to communicate with the server.  
        Dim RemoteFilename As String = "ftp://www.albusmw.de/" & CurrentImageName & ".jpg"
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

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        UploadFile(CurrentImageName & ".jpg")
    End Sub
End Class
