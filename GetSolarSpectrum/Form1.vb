Option Explicit On
Option Strict On

Public Class Form1

  Dim LeftWavelength As Double = 380
  Dim RightWavelength As Double = 780
  Dim ImageHeight As Integer = 4000
  Dim nmPerStep As Double = 0.025000000000000033

  Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    Dim MyBitmap As System.Drawing.Bitmap
    Dim MyGraphPane As Drawing.Graphics

    Dim Pen As New System.Drawing.Pen(Brushes.Black)

    'Get size by stepping through
    Dim Idx As Integer = 0
    For Wavelength As Double = LeftWavelength To RightWavelength Step nmPerStep
      Idx += 1
    Next Wavelength
    Dim ImageWidth As Integer = Idx

    'Get bitmap and graph
    MyBitmap = New System.Drawing.Bitmap(Idx, ImageHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
    MyGraphPane = System.Drawing.Graphics.FromImage(MyBitmap)

    'Draw spectrum
    Idx = 0
    For Wavelength As Double = LeftWavelength To RightWavelength Step nmPerStep
      Pen.Color = GetColorValue(Wavelength)
      MyGraphPane.DrawLine(Pen, New System.Drawing.Point(Idx, 0), New System.Drawing.Point(Idx, ImageHeight))
      Idx += 1
    Next Wavelength

    'Draw lines
    Pen.Color = Color.Black
    For Each SingleLine As Double In FraunhoferLines.Keys
      Idx = CInt(((SingleLine - LeftWavelength) / (RightWavelength - LeftWavelength)) * ImageWidth)
      Pen.Width = CSng(FraunhoferLines(SingleLine) / nmPerStep)
      MyGraphPane.DrawLine(Pen, New System.Drawing.Point(Idx, 0), New System.Drawing.Point(Idx, ImageHeight))
    Next SingleLine

    Dim Format As Imaging.ImageFormat = System.Drawing.Imaging.ImageFormat.Png
    MyBitmap.Save("C:\Frauenhofer.png", Format)

    MsgBox("OK")

  End Sub

  Private Function GetColorValue(ByVal w As Double) As Color

    Dim R As Double
    Dim G As Double
    Dim B As Double

    Dim Gamma As Double = 0.80000000000000004
    Dim IntensityMax As Double = 255

    If w >= 380 And w < 440 Then
      R = -(w - 440) / (440 - 380)
      G = 0.0
      B = 1.0
    ElseIf (w >= 440 And w < 490) Then
      R = 0.0
      G = (w - 440) / (490 - 440)
      B = 1.0
    ElseIf (w >= 490 And w < 510) Then
      R = 0.0
      G = 1.0
      B = -(w - 510) / (510 - 490)
    ElseIf (w >= 510 And w < 580) Then
      R = (w - 510) / (580 - 510)
      G = 1.0
      B = 0.0
    ElseIf (w >= 580 And w < 645) Then
      R = 1.0
      G = -(w - 645) / (645 - 580)
      B = 0.0
    ElseIf (w >= 645 And w <= 780) Then
      R = 1.0
      G = 0.0
      B = 0.0
    Else
      R = 0.0
      G = 0.0
      B = 0.0
    End If

    'Let the intensity fall off near the vision limits

    Dim factor As Double
    If ((w >= 380) And (w < 420)) Then
      factor = 0.29999999999999999 + 0.69999999999999996 * (w - 380) / (420 - 380)
    ElseIf ((w >= 420) And (w < 701)) Then
      factor = 1.0
    ElseIf ((w >= 701) And (w < 781)) Then
      factor = 0.29999999999999999 + 0.69999999999999996 * (780 - w) / (780 - 700)
    Else
      factor = 0.0
    End If

    Return Color.FromArgb(CInt(Math.Round(IntensityMax * Math.Pow(R * factor, Gamma))), CInt(Math.Round(IntensityMax * Math.Pow(G * factor, Gamma))), CInt(Math.Round(IntensityMax * Math.Pow(B * factor, Gamma))))

  End Function

  Private Function FraunhoferLines() As Dictionary(Of Double, Double)

    Dim RetVal As New Dictionary(Of Double, Double)

    'http://www.columbia.edu/~vjd1/Solar%20Spectrum%20Ex.html
    '(Wide lines seem to be also in other parts ...)

    TryAdd(RetVal, 393.3682, 2.0253000000000001)
    TryAdd(RetVal, 394.40159999999997, 0.048800000000000003)
    TryAdd(RetVal, 396.15350000000001, 0.062100000000000002)
    TryAdd(RetVal, 396.8492, 1.5467)
    TryAdd(RetVal, 404.58249999999998, 0.1174)
    TryAdd(RetVal, 406.3605, 0.078700000000000006)
    TryAdd(RetVal, 407.17489999999998, 0.072300000000000003)
    TryAdd(RetVal, 407.7724, 0.042799999999999998)
    TryAdd(RetVal, 410.1748, 0.31330000000000002)
    TryAdd(RetVal, 413.20670000000001, 0.040399999999999998)
    TryAdd(RetVal, 414.38780000000003, 0.046600000000000003)
    TryAdd(RetVal, 416.72770000000003, 0.02)
    TryAdd(RetVal, 420.20400000000001, 0.032599999999999997)
    TryAdd(RetVal, 422.67399999999998, 0.14760000000000001)
    TryAdd(RetVal, 423.5949, 0.0385)
    TryAdd(RetVal, 425.01299999999998, 0.034200000000000001)
    TryAdd(RetVal, 425.0797, 0.040000000000000001)
    TryAdd(RetVal, 425.43459999999999, 0.039300000000000002)
    TryAdd(RetVal, 426.04860000000002, 0.059499999999999997)
    TryAdd(RetVal, 427.17739999999998, 0.075600000000000001)
    TryAdd(RetVal, 432.57749999999999, 0.079299999999999995)
    TryAdd(RetVal, 434.04750000000001, 0.28549999999999998)
    TryAdd(RetVal, 438.35570000000001, 0.1008)
    TryAdd(RetVal, 440.47609999999997, 0.089800000000000005)
    TryAdd(RetVal, 441.51350000000002, 0.041700000000000001)
    TryAdd(RetVal, 452.86270000000002, 0.0275)
    TryAdd(RetVal, 455.40359999999998, 0.015900000000000001)
    TryAdd(RetVal, 470.30029999999999, 0.032599999999999997)
    TryAdd(RetVal, 486.13420000000002, 0.36799999999999999)
    TryAdd(RetVal, 489.15019999999998, 0.031199999999999999)
    TryAdd(RetVal, 492.0514, 0.047100000000000003)
    TryAdd(RetVal, 495.76130000000001, 0.069599999999999995)
    TryAdd(RetVal, 516.73270000000002, 0.0935)
    TryAdd(RetVal, 517.26980000000003, 0.12590000000000001)
    TryAdd(RetVal, 518.36189999999999, 0.15840000000000001)
    TryAdd(RetVal, 525.02160000000003, 0.0061999999999999998)
    TryAdd(RetVal, 526.95500000000004, 0.047800000000000002)
    TryAdd(RetVal, 532.80510000000004, 0.037499999999999999)
    TryAdd(RetVal, 552.84180000000003, 0.0293)
    TryAdd(RetVal, 588.9973, 0.075200000000000003)
    TryAdd(RetVal, 589.59400000000005, 0.056399999999999999)
    TryAdd(RetVal, 610.27269999999999, 0.0135)
    TryAdd(RetVal, 612.22260000000006, 0.022200000000000001)
    TryAdd(RetVal, 616.21799999999996, 0.022200000000000001)
    TryAdd(RetVal, 630.24990000000003, 0.0083000000000000001)
    TryAdd(RetVal, 656.2808, 0.10199999999999999)

    'http://www.coseti.org/9006-025.htm
    TryAdd(RetVal, 333.66890000000001, 0.041599999999999998)
    TryAdd(RetVal, 341.47789999999998, 0.081600000000000006)
    TryAdd(RetVal, 343.35789999999997, 0.049200000000000001)
    TryAdd(RetVal, 344.06259999999997, 0.12429999999999999)
    TryAdd(RetVal, 344.1019, 0.063399999999999998)
    TryAdd(RetVal, 344.38839999999999, 0.065500000000000003)
    TryAdd(RetVal, 344.62709999999998, 0.047)
    TryAdd(RetVal, 345.8467, 0.065600000000000006)
    TryAdd(RetVal, 346.16669999999999, 0.075800000000000006)
    TryAdd(RetVal, 347.54570000000001, 0.062199999999999998)
    TryAdd(RetVal, 347.6712, 0.0465)
    TryAdd(RetVal, 349.05939999999998, 0.083000000000000004)
    TryAdd(RetVal, 349.29750000000001, 0.082600000000000007)
    TryAdd(RetVal, 349.78429999999997, 0.072599999999999998)
    TryAdd(RetVal, 351.03269999999998, 0.048899999999999999)
    TryAdd(RetVal, 351.50659999999999, 0.071800000000000003)
    TryAdd(RetVal, 352.12700000000001, 0.038100000000000002)
    TryAdd(RetVal, 352.45359999999999, 0.12709999999999999)
    TryAdd(RetVal, 355.49369999999999, 0.040399999999999998)
    TryAdd(RetVal, 355.85320000000002, 0.048500000000000001)
    TryAdd(RetVal, 356.53960000000001, 0.099000000000000005)
    TryAdd(RetVal, 356.63830000000002, 0.0458)
    TryAdd(RetVal, 357.01339999999999, 0.13800000000000001)
    TryAdd(RetVal, 357.86930000000001, 0.048800000000000003)
    TryAdd(RetVal, 358.12090000000001, 0.21440000000000001)
    TryAdd(RetVal, 358.69900000000001, 0.053199999999999997)
    TryAdd(RetVal, 359.34949999999998, 0.0436)
    TryAdd(RetVal, 360.88690000000003, 0.1046)
    TryAdd(RetVal, 361.8777, 0.14099999999999999)
    TryAdd(RetVal, 361.94, 0.056800000000000003)
    TryAdd(RetVal, 363.14749999999998, 0.13639999999999999)
    TryAdd(RetVal, 364.7851, 0.097000000000000003)
    TryAdd(RetVal, 367.9923, 0.0448)
    TryAdd(RetVal, 368.51960000000003, 0.0275)
    TryAdd(RetVal, 370.55770000000001, 0.0562)
    TryAdd(RetVal, 370.92559999999997, 0.057299999999999997)
    TryAdd(RetVal, 371.99470000000002, 0.16639999999999999)
    TryAdd(RetVal, 373.48739999999998, 0.30270000000000002)
    TryAdd(RetVal, 373.71409999999997, 0.1071)
    TryAdd(RetVal, 374.55739999999997, 0.1202)
    TryAdd(RetVal, 374.82709999999997, 0.049700000000000001)
    TryAdd(RetVal, 374.9495, 0.19070000000000001)
    TryAdd(RetVal, 375.8245, 0.16470000000000001)
    TryAdd(RetVal, 375.92989999999998, 0.033399999999999999)
    TryAdd(RetVal, 376.38029999999998, 0.082900000000000001)
    TryAdd(RetVal, 376.72039999999998, 0.082000000000000003)
    TryAdd(RetVal, 378.78910000000002, 0.051200000000000002)
    TryAdd(RetVal, 379.50119999999998, 0.054699999999999999)
    TryAdd(RetVal, 380.67180000000002, 0.020899999999999998)
    TryAdd(RetVal, 381.58510000000001, 0.12720000000000001)
    TryAdd(RetVal, 382.04360000000003, 0.17119999999999999)
    TryAdd(RetVal, 382.58909999999997, 0.15190000000000001)
    TryAdd(RetVal, 382.78320000000002, 0.089700000000000002)
    TryAdd(RetVal, 382.93650000000002, 0.087400000000000005)
    TryAdd(RetVal, 383.23099999999999, 0.16850000000000001)
    TryAdd(RetVal, 383.42329999999998, 0.062399999999999997)
    TryAdd(RetVal, 383.83019999999999, 0.192)
    TryAdd(RetVal, 384.04469999999998, 0.0567)
    TryAdd(RetVal, 384.10579999999999, 0.051700000000000003)
    TryAdd(RetVal, 384.99770000000001, 0.0608)
    TryAdd(RetVal, 385.63810000000001, 0.064799999999999996)
    TryAdd(RetVal, 385.99220000000003, 0.15540000000000001)
    TryAdd(RetVal, 387.80270000000002, 0.055500000000000001)
    TryAdd(RetVal, 388.62939999999998, 0.091999999999999998)
    TryAdd(RetVal, 389.97190000000001, 0.0436)
    TryAdd(RetVal, 390.29559999999998, 0.052999999999999999)
    TryAdd(RetVal, 390.5532, 0.081600000000000006)
    TryAdd(RetVal, 392.02690000000001, 0.034099999999999998)
    TryAdd(RetVal, 392.29230000000001, 0.041399999999999999)
    TryAdd(RetVal, 392.79329999999999, 0.018700000000000001)
    TryAdd(RetVal, 393.0308, 0.010800000000000001)
    TryAdd(RetVal, 393.3682, 2.0253000000000001)
    TryAdd(RetVal, 394.40159999999997, 0.048800000000000003)
    TryAdd(RetVal, 396.15350000000001, 0.062100000000000002)
    TryAdd(RetVal, 396.8492, 1.5467)
    TryAdd(RetVal, 404.58249999999998, 0.1174)
    TryAdd(RetVal, 406.3605, 0.078700000000000006)
    TryAdd(RetVal, 407.17489999999998, 0.072300000000000003)
    TryAdd(RetVal, 407.7724, 0.042799999999999998)
    TryAdd(RetVal, 410.1748, 0.31330000000000002)
    TryAdd(RetVal, 413.20670000000001, 0.040399999999999998)
    TryAdd(RetVal, 414.38780000000003, 0.046600000000000003)
    TryAdd(RetVal, 416.72770000000003, 0.02)
    TryAdd(RetVal, 420.20400000000001, 0.032599999999999997)
    TryAdd(RetVal, 422.67399999999998, 0.14760000000000001)
    TryAdd(RetVal, 423.5949, 0.0385)
    TryAdd(RetVal, 425.01299999999998, 0.034200000000000001)
    TryAdd(RetVal, 425.0797, 0.040000000000000001)
    TryAdd(RetVal, 425.43459999999999, 0.039300000000000002)
    TryAdd(RetVal, 426.04860000000002, 0.059499999999999997)
    TryAdd(RetVal, 427.17739999999998, 0.075600000000000001)
    TryAdd(RetVal, 432.57749999999999, 0.079299999999999995)
    TryAdd(RetVal, 434.04750000000001, 0.28549999999999998)
    TryAdd(RetVal, 438.35570000000001, 0.1008)
    TryAdd(RetVal, 440.47609999999997, 0.089800000000000005)
    TryAdd(RetVal, 441.51350000000002, 0.041700000000000001)
    TryAdd(RetVal, 452.86270000000002, 0.0275)
    TryAdd(RetVal, 455.40359999999998, 0.015900000000000001)
    TryAdd(RetVal, 470.30029999999999, 0.032599999999999997)
    TryAdd(RetVal, 486.13420000000002, 0.36799999999999999)
    TryAdd(RetVal, 489.15019999999998, 0.031199999999999999)
    TryAdd(RetVal, 492.0514, 0.047100000000000003)
    TryAdd(RetVal, 495.76130000000001, 0.069599999999999995)
    TryAdd(RetVal, 516.73270000000002, 0.0935)
    TryAdd(RetVal, 517.26980000000003, 0.12590000000000001)
    TryAdd(RetVal, 518.36189999999999, 0.15840000000000001)
    TryAdd(RetVal, 525.02160000000003, 0.0061999999999999998)
    TryAdd(RetVal, 526.95500000000004, 0.047800000000000002)
    TryAdd(RetVal, 532.80510000000004, 0.037499999999999999)
    TryAdd(RetVal, 552.84180000000003, 0.0293)
    TryAdd(RetVal, 588.9973, 0.075200000000000003)
    TryAdd(RetVal, 589.59400000000005, 0.056399999999999999)
    TryAdd(RetVal, 610.27269999999999, 0.0135)
    TryAdd(RetVal, 612.22260000000006, 0.022200000000000001)
    TryAdd(RetVal, 616.21799999999996, 0.022200000000000001)
    TryAdd(RetVal, 630.24990000000003, 0.0083000000000000001)
    TryAdd(RetVal, 656.2808, 0.40200000000000002)
    TryAdd(RetVal, 849.80619999999999, 0.14699999999999999)
    TryAdd(RetVal, 854.21439999999996, 0.36699999999999999)
    TryAdd(RetVal, 866.21699999999998, 0.26000000000000001)

    Return RetVal

  End Function

  Private Sub TryAdd(ByRef Dict As Dictionary(Of Double, Double), ByVal NewA As Double, ByVal NewB As Double)
    If Dict.ContainsKey(NewA) = False Then
      Dict.Add(NewA, NewB)
    End If
  End Sub

End Class
