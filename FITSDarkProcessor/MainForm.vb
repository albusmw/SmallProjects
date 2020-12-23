Option Explicit On
Option Strict On

Public Class MainForm

  Dim PeaksToSearch As Integer = 100000
  Dim SuspectSigmaLimit As Double = 0.5

  Private Structure sPixelValues
    Public Pixel As Point
    Public Value As Double
    Public Sub New(ByVal NewPixel As Point, ByVal NewValue As Double)
      Me.Pixel = NewPixel
      Me.Value = NewValue
    End Sub
    Public Shared Function CompareTo(ByVal X1 As sPixelValues, ByVal X2 As sPixelValues) As Integer
      Return X1.Value.CompareTo(X2.Value)
    End Function
  End Structure

  Private Sub btnAnalysis_Click(sender As Object, e As EventArgs) Handles btnAnalysis.Click

    Dim MaxFilesToUse As Integer = 10

    CType(sender, Button).Enabled = False

    Dim Root As String = "C:\Users\weis_m\Dropbox\Astro\Bilder\FITS\"
    Dim AllFiles As New List(Of String)
    For Each File As String In System.IO.Directory.GetFiles(Root, "lasttest_Ha_1x1_600s_m20C_001.fit")
      AllFiles.Add(File)
    Next File

    '================================================================================
    tbLog.AddEntry("Reading 1st file <" & AllFiles(0) & "> ...")
    Dim Reader As New cFITSReader
    Dim Data(,) As Double = Nothing
    Reader.ReadIn(AllFiles(0), Data)
        Dim BSCALE As Double = Reader.BSCALE
        Dim BZERO As Double = Reader.BZERO
    '================================================================================

    '================================================================================
    tbLog.AddEntry("Processing basic statistics ...")
    Dim Sum As Double = 0
    Dim SumSum As Double = 0
    For Idx1 As Integer = 0 To Data.GetUpperBound(0)
      For Idx2 As Integer = 0 To Data.GetUpperBound(1)
        Sum += Data(Idx1, Idx2)
        SumSum += Data(Idx1, Idx2) * Data(Idx1, Idx2)
      Next Idx2
    Next Idx1
    '================================================================================

    '================================================================================
    Dim FirstImageMean As Double = Sum / Data.Length
    Dim FirstImageVariance As Double = (SumSum / Data.Length) - (FirstImageMean * FirstImageMean)
    Dim FirstImageSigma As Double = Math.Sqrt(FirstImageVariance)
    tbLog.AddEntry(" -> Mean     : " & FirstImageMean.ToString.Trim)
    tbLog.AddEntry(" -> Variance : " & FirstImageVariance.ToString.Trim)
    tbLog.AddEntry(" -> Sigma    : " & FirstImageSigma.ToString.Trim)
    '================================================================================

    '================================================================================

    'Find peaks - suspect hot pixel that are above mean + x * sigma
    tbLog.AddEntry("Getting and sorting all pixel ...")
    Dim AllElements As New List(Of sPixelValues)
    For Idx1 As Integer = 0 To Data.GetUpperBound(0)
      For Idx2 As Integer = 0 To Data.GetUpperBound(1)
        If Data(Idx1, Idx2) > FirstImageMean + (SuspectSigmaLimit * FirstImageSigma) Then
          AllElements.Add(New sPixelValues(New Point(Idx1, Idx2), Data(Idx1, Idx2)))
        End If
      Next Idx2
    Next Idx1
    AllElements.Sort(AddressOf sPixelValues.CompareTo)
    tbLog.AddEntry("  -> found " & AllElements.Count.ToString.Trim & " suspects ...")

    'Find top N elements of the suspects (can be limited)
    Dim ElementsToCorrect As Integer = CInt(IIf(AllElements.Count > PeaksToSearch, PeaksToSearch, AllElements.Count))
    tbLog.AddEntry("Getting top " & ElementsToCorrect.ToString.Trim & " pixel")
    Dim PixelPtr As Integer = AllElements.Count - 1
    Dim SuspectHotPixel As New List(Of sPixelValues)
    Dim SuspectHotPixelPositions As New List(Of Point)
    For Idx As Integer = 0 To ElementsToCorrect - 1
      SuspectHotPixel.Add(AllElements(PixelPtr))
      SuspectHotPixelPositions.Add(AllElements(PixelPtr).Pixel)
      PixelPtr -= 1
    Next Idx

    'Read the pixel values for all suspect pixel over all files
    Dim HotPixelsOverFiles As New List(Of Double())
    For FileIdx As Integer = 0 To CInt(IIf(AllFiles.Count > MaxFilesToUse, MaxFilesToUse, AllFiles.Count - 1))
      HotPixelsOverFiles.Add(PeakSample(AllFiles(FileIdx), SuspectHotPixelPositions))
    Next FileIdx

    'Calculate statistics for found top N elements over all files
    Dim RepairValue As New List(Of Int16)
    tbLog.AddEntry("Calculating statistics ...")
    Dim PeakLogs As New List(Of String)
    tspbProgress.Maximum = SuspectHotPixel.Count
    tspbProgress.Value = 0
    Dim RepairCount As Integer = 0
    For Idx As Integer = 0 To HotPixelsOverFiles(0).GetUpperBound(0)
      'Build the statistics over all files
      Dim PixelStatistics As New Ato.cSingleValueStatistics(Ato.cSingleValueStatistics.eValueType.Linear) : PixelStatistics.StoreRawValues = True
      For FileIdx As Integer = 0 To HotPixelsOverFiles.Count - 1
        PixelStatistics.AddValue(HotPixelsOverFiles(FileIdx)(Idx))
      Next FileIdx
      'Calculate if repair
      Dim Repair As Boolean = True
      If PixelStatistics.Mean + (2 * PixelStatistics.Sigma) > FirstImageMean + (2 * FirstImageSigma) Then
        Repair = True
        RepairCount += 1
      End If
      'Set repair value
      If Repair Then
        Dim FloatValue As Double = NormalDistribution(FirstImageMean, FirstImageSigma)
        Dim Int16Value As Double = (FloatValue / BSCALE) - BZERO
        If Int16Value < Int16.MinValue Then Int16Value = Int16.MinValue Else If Int16Value > Int16.MaxValue Then Int16Value = Int16.MaxValue
        RepairValue.Add(CType(Int16Value, Int16))
      Else
        RepairValue.Add(CType((Data(SuspectHotPixel(Idx).Pixel.X, SuspectHotPixel(Idx).Pixel.Y) / BSCALE) - BZERO, Int16))
      End If
      'Indicate progress
      tsslCurrentMean.Text = "Current peak: " & SuspectHotPixel(Idx).Value.ToString.Trim
      tspbProgress.Value += 1
      System.Windows.Forms.Application.DoEvents()
    Next Idx
    tspbProgress.Value = 0
    tbLog.AddEntry("  DONE - repaired " & RepairCount.ToString.Trim & " pixel")

    '================================================================================

    'Repair
    tbLog.AddEntry("Reparing ...")
    Dim FixedFile As String = Root & "\" & "Dark_02_repaired.FIT"
    If System.IO.File.Exists(FixedFile) Then System.IO.File.Delete(FixedFile)
    System.IO.File.Copy(AllFiles(0), FixedFile)
        Dim PointReader As New cFITSReader
        Dim ImageData(,) As Double = Nothing
        Dim DataStartPosition As Integer = PointReader.ReadIn(FixedFile, ImageData)
        PointReader.FixSample(FixedFile, DataStartPosition, SuspectHotPixelPositions, RepairValue.ToArray)

        '================================================================================
        'Finish

        tbLog.AddEntry("==========================================================")
    CType(sender, Button).Enabled = True

  End Sub

  Private Function FindMax(ByRef Data(,) As Double) As Double
    Dim Max As Double = Double.MinValue
    For Idx1 As Integer = 0 To Data.GetUpperBound(0)
      For Idx2 As Integer = 0 To Data.GetUpperBound(1)
        If Data(Idx1, Idx2) > Max Then Max = Data(Idx1, Idx2)
      Next Idx2
    Next Idx1
    Return Max
  End Function

  Private Function PeakSample(ByVal File As String, ByVal PointsToRead As List(Of Point)) As Double()
    Dim PointReader As New cFITSReader
    Dim ImageData(,) As Double = Nothing
    PointReader.ReadIn(File, True, ImageData, PointsToRead.ToArray)
    GC.Collect()
    Dim RetVal As New List(Of Double)
    For Idx As Integer = 0 To ImageData.GetUpperBound(0)
      RetVal.Add(ImageData(Idx, 0))
    Next Idx
    Return RetVal.ToArray
  End Function

  '''<summary>Create a normal-distributed random number.</summary>
  '''<returns>Random number.</returns>
  '''<remarks></remarks>
  Public Shared Function NormalDistribution(ByVal Mean As Double, ByVal Sigma As Double) As Double
    Return Mean + (NormalDistribution() * Sigma)
  End Function

  '''<summary>Create a normal-distributed random number.</summary>
  '''<returns>Random number.</returns>
  '''<remarks></remarks>
  Public Shared Function NormalDistribution() As Double
    Dim U1 As Double, U2 As Double
    Dim V As Double, W As Double
    Do
      U1 = Rnd() : U2 = Rnd()
      V = (((2 * U1) - 1) * ((2 * U1) - 1)) + (((2 * U2) - 1) * ((2 * U2) - 1))
    Loop Until V <= 1
    W = System.Math.Sqrt(-2 * System.Math.Log(V) / V)
    Return ((2 * U1) - 1) * W
  End Function

    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click

        Dim Root As String = "C:\Users\weis_m\Dropbox\Astro\!Bilder\FITS\"
        Dim AllFiles As New List(Of String)
        For Each File As String In System.IO.Directory.GetFiles(Root, "lasttest_Ha_1x1_600s_m20C_001.fit")
            AllFiles.Add(File)
        Next File

        '================================================================================
        tbLog.AddEntry("Reading file <" & AllFiles(0) & "> ...")
        Dim Reader As New cFITSReader
        Dim Data(,) As Double = Nothing
        Reader.ReadIn(AllFiles(0), Data)
        Dim BSCALE As Double = Reader.BSCALE
        Dim BZERO As Double = Reader.BZERO
        '================================================================================

        '================================================================================
        tbLog.AddEntry("Processing basic statistics ...")
        Dim Max As Double = Double.MinValue
        Dim Min As Double = Double.MaxValue
        Dim Sum As Double = 0
        Dim SumSum As Double = 0
        For Idx1 As Integer = 0 To Data.GetUpperBound(0)
            For Idx2 As Integer = 0 To Data.GetUpperBound(1)
                Sum += Data(Idx1, Idx2)
                SumSum += Data(Idx1, Idx2) * Data(Idx1, Idx2)
                If Data(Idx1, Idx2) > Max Then Max = Data(Idx1, Idx2)
                If Data(Idx1, Idx2) < Min Then Min = Data(Idx1, Idx2)
            Next Idx2
        Next Idx1
        Dim Scale As Double = 255 / Max
        '================================================================================

        'Create a transparent PNG
        Dim StarBaseColor As Color = System.Drawing.Color.FromArgb(255, 0, 0)
        Dim bmp As New Bitmap(Data.GetUpperBound(0) + 1, Data.GetUpperBound(1) + 1)
        Dim g As Graphics = Graphics.FromImage(bmp)
        g.Clear(Color.Transparent)
        For Idx1 As Integer = 0 To Data.GetUpperBound(0)
            For Idx2 As Integer = 0 To Data.GetUpperBound(1)
                Dim Alpha As Integer = CInt(Data(Idx1, Idx2) * Scale)
                bmp.SetPixel(Idx1, Idx2, System.Drawing.Color.FromArgb(Alpha, StarBaseColor))
            Next Idx2
        Next Idx1

        'Save
        bmp.Save("C:\Users\weis_m\Desktop\Stars.png", System.Drawing.Imaging.ImageFormat.Png)

        MsgBox("OK")

    End Sub

End Class
