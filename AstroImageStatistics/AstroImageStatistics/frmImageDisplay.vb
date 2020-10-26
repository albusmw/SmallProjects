Option Explicit On
Option Strict On

Public Class frmImageDisplay

    Public DataContainer As AstroNET.Statistics
    Public StatToUsed As AstroNET.Statistics.sStatistics

    Private ImageDisplayProp As New cImageDisplayProp

    'This elements are self-coded and will not work in 64-bit from the toolbox ...
    Private WithEvents pbMain As PictureBoxEx

    '''<summary>File name of the display to show.</summary>
    Public FileToDisplay As String = String.Empty

    Private Sub frmImageDisplay_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Text = "Image <" & FileToDisplay & ">"
    End Sub

    Private Sub frmImageDisplay_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Load custom controls - main image (must be done due to 64-bit IDE limitation)
        pbMain = New PictureBoxEx
        scMain.Panel2.Controls.Add(pbMain)
        pbMain.Dock = DockStyle.Fill
        pbMain.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        pbMain.SizeMode = PictureBoxSizeMode.Zoom
        pbMain.BackColor = Color.Purple
        scMain.SplitterDistance = 200

        pgMain.SelectedObject = ImageDisplayProp

    End Sub

    '''<summary>Calculate the image to be displayed.</summary>
    Public Sub GenerateDisplayImage()

        Dim Stopper As New cStopper
        Dim NAXIS3 As Integer = 0

        'Calculate data range and scaling
        Dim Data_Min As Long = StatToUsed.MonoStatistics_Int.Min.Key
        Dim Data_Max As Long = StatToUsed.MonoStatistics_Int.Max.Key
        Dim LinOffset As Double = -Data_Min
        Dim LinScale As Double = 255 / (Data_Max - Data_Min)

        'Build a LUT for all colors present in the picture - the LUT is build as 3 vectors with R/G/B values due to speed reason (dictionary is slow ...)
        Stopper.Tic()
        Dim LUT(UShort.MaxValue) As Int32
        For Each Entry As Long In StatToUsed.MonochromHistogram_Int.Keys
            Dim ColorToUse As Color
            Select Case Entry
                Case Is < StatToUsed.MonoStatistics_Int.Percentile(ImageDisplayProp.MinPct)
                    ColorToUse = ImageDisplayProp.MinBelowColor
                Case Is > StatToUsed.MonoStatistics_Int.Percentile(ImageDisplayProp.MaxPct)
                    ColorToUse = ImageDisplayProp.MaxAboveColor
                Case Else
                    Dim Scaled As Double = Math.Floor((Entry + LinOffset) * LinScale)
                    ColorToUse = cColorMaps.Bone(Scaled)
            End Select
            Dim C1 As Int32 = CType(ColorToUse.A, Int32) << 24
            Dim C2 As Int32 = CType(ColorToUse.R, Int32) << 16
            Dim C3 As Int32 = CType(ColorToUse.G, Int32) << 8
            Dim C4 As Int32 = CType(ColorToUse.B, Int32)
            LUT(CInt(Entry)) = C1 + C2 + C3 + C4
        Next Entry
        Stopper.Toc("Generating LUT for each pixel value in the image")

        'Generate the output picture
        Stopper.Tic()
        Dim OutputImage As New cLockBitmap32Bit(DataContainer.DataProcessor_UInt16.ImageData(NAXIS3).Data.GetUpperBound(0), DataContainer.DataProcessor_UInt16.ImageData(NAXIS3).Data.GetUpperBound(1))
        OutputImage.LockBits()
        Dim Stride As Integer = OutputImage.BitmapData.Stride
        Stopper.Toc("Prepare image")

        'Calculate output image data
        Stopper.Tic()
        Dim YOffset As Integer = 0
        For Y As Integer = 0 To OutputImage.Height - 1
            Dim BaseOffset As Integer = YOffset
            For X As Integer = 0 To OutputImage.Width - 1
                Dim PixelValue As Integer = DataContainer.DataProcessor_UInt16.ImageData(NAXIS3).Data(X, Y)
                OutputImage.Pixels(BaseOffset) = LUT(PixelValue)
                BaseOffset += 1
            Next X
            YOffset += OutputImage.Width
        Next Y
        Stopper.Toc("Calculating image (" & DataContainer.DataProcessor_UInt16.ImageData(NAXIS3).Data.LongLength.ToString.Trim & " pixel)")

        Stopper.Tic()
        OutputImage.UnlockBits()
        pbMain.Image = OutputImage.BitmapToProcess
        Stopper.Toc("Display image")

        Clipboard.SetText(Join(Stopper.GetLog.ToArray, System.Environment.NewLine))

    End Sub

    Private Sub pgMain_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles pgMain.PropertyValueChanged
        GenerateDisplayImage()
    End Sub

    Private Sub pgMain_MouseWheel(sender As Object, e As MouseEventArgs) Handles pgMain.MouseWheel
        Select Case pgMain.SelectedGridItem.PropertyDescriptor.Name
            Case "MaxPct"
                CType(pgMain.SelectedObject, cImageDisplayProp).MaxPct += Math.Sign(e.Delta)
            Case "MinPct"
                CType(pgMain.SelectedObject, cImageDisplayProp).MinPct += Math.Sign(e.Delta)
        End Select
        pgMain.SelectedObject = ImageDisplayProp
        GenerateDisplayImage()
    End Sub

End Class

Public Class cImageDisplayProp

    '''<summary>Start display color conversion at given percentage value.</summary>
    Public Property MinPct As Integer = 0
    '''<summary>Color to use if value is below display range.</summary>
    Public Property MinBelowColor As Color = Color.Blue

    '''<summary>Stop display color conversion at given percentage value.</summary>
    Public Property MaxPct As Integer = 100
    '''<summary>Color to use if value is above display range.</summary>
    Public Property MaxAboveColor As Color = Color.Red

End Class