Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Public Class cMandel

    Public Event PropertyChanged()

    'Available color profiles
    Public Enum eColorProfile
        BlackAndWhite
        Red
        Green
        Blue
        Rainbow
    End Enum

    'Native pixel calculation
    Private FractalBitmap_FAST(,) As ImageProcessor.NativePixel

    'Rendered image
    Public Fractal As Bitmap

    'Color coding list
    Dim m_Colors As New List(Of Color)

    'Possible background workers
    Private WithEvents BGWorker_1 As New System.ComponentModel.BackgroundWorker
    Private WithEvents BGWorker_2 As New System.ComponentModel.BackgroundWorker
    Private WithEvents BGWorker_3 As New System.ComponentModel.BackgroundWorker
    Private WithEvents BGWorker_4 As New System.ComponentModel.BackgroundWorker
    Private WithEvents BGWorker_5 As New System.ComponentModel.BackgroundWorker
    Private WithEvents BGWorker_6 As New System.ComponentModel.BackgroundWorker
    Private WithEvents BGWorker_7 As New System.ComponentModel.BackgroundWorker
    Private WithEvents BGWorker_8 As New System.ComponentModel.BackgroundWorker

    Public Sub New()
        Me.ColorProfile = eColorProfile.Rainbow
    End Sub

    '''<summary>Maximum magnitude square.</summary>
    <Category("1. Calculation")> _
    <DisplayName("a) Break at mag")> _
    Public Property MaxMagSquare() As Double
        Get
            Return MyMaxMagSquare
        End Get
        Set(ByVal value As Double)
            MyMaxMagSquare = value
            RaiseEvent PropertyChanged()
        End Set
    End Property
    Private MyMaxMagSquare As Double = 4

    '''<summary>Maximum number of iterations.</summary>
    <Category("1. Calculation")> _
    <DisplayName("b) Maximum iterations")> _
    Public Property MaxIterations() As Integer
        Get
            Return MyMaxIterations
        End Get
        Set(ByVal value As Integer)
            MyMaxIterations = value
            RaiseEvent PropertyChanged()
        End Set
    End Property
    Private MyMaxIterations As Integer = 64

    <Category("1. Calculation")> _
    <DisplayName("c) Workers to be used")> _
    Public Property Workers() As Integer
        Get
            Return MyWorkers
        End Get
        Set(ByVal value As Integer)
            If (value >= 1) And (value <= 8) Then
                MyWorkers = value
            End If
        End Set
    End Property
    Private MyWorkers As Integer = 4

    '''<summary>Left boundary.</summary>
    <Category("2. Displayed area")> _
    <DisplayName("a) X - min")> _
    Public Property X_min() As Double
        Get
            Return MyX_min
        End Get
        Set(ByVal value As Double)
            MyX_min = value
        End Set
    End Property
    Private MyX_min As Double = -2.2

    '''<summary>Right boundary.</summary>
    <Category("2. Displayed area")> _
    <DisplayName("b) X - max")> _
    Public Property X_max() As Double
        Get
            Return MyX_max
        End Get
        Set(ByVal value As Double)
            MyX_max = value
        End Set
    End Property
    Private MyX_max As Double = 2.2

    <Category("2. Displayed area")> _
    <DisplayName("c) X - span")> _
    Public ReadOnly Property X_span() As Double
        Get
            Return Me.X_max - Me.X_min
        End Get
    End Property


    '''<summary>Top boundary.</summary>
    <Category("2. Displayed area")> _
    <DisplayName("d) Y - min")> _
    Public Property Y_min() As Double
        Get
            Return MyY_min
        End Get
        Set(ByVal value As Double)
            MyY_min = value
        End Set
    End Property
    Private MyY_min As Double = -1.2

    '''<summary>Top boundary.</summary>
    <Category("2. Displayed area")> _
    <DisplayName("e) Y - max")> _
    Public Property Y_max() As Double
        Get
            Return MyY_max
        End Get
        Set(ByVal value As Double)
            MyY_max = value
        End Set
    End Property
    Private MyY_max As Double = 1.2

    <Category("2. Displayed area")> _
    <DisplayName("f) Y - span")> _
    Public ReadOnly Property Y_span() As Double
        Get
            Return Me.Y_max - Me.Y_min
        End Get
    End Property

    <Category("3. Image size")> _
    <DisplayName("a) Lock to picture size")> _
    Public Property LockToPicture() As Boolean
        Get
            Return MyLockToPicture
        End Get
        Set(ByVal value As Boolean)
            MyLockToPicture = value
        End Set
    End Property
    Private MyLockToPicture As Boolean = True

    <Category("3. Image size")> _
    <DisplayName("b) Manual width")> _
    Public Property ManWidth() As Integer
        Get
            Return MyManWidth
        End Get
        Set(ByVal value As Integer)
            MyManWidth = value
        End Set
    End Property
    Private MyManWidth As Integer = 10000

    <Category("3. Image size")> _
    <DisplayName("c) Manual height")> _
    Public Property ManHeight() As Integer
        Get
            Return MyManHeight
        End Get
        Set(ByVal value As Integer)
            MyManHeight = value
        End Set
    End Property
    Private MyManHeight As Integer = 6000

    '''<summary>X position of a flight.</summary>
    <Category("4. Flight options")> _
    <DisplayName("a) X coordinate to fly to")> _
    Public Property ZoomX() As Double
        Get
            Return MyZoomX
        End Get
        Set(ByVal value As Double)
            MyZoomX = value
        End Set
    End Property
    Private MyZoomX As Double = -0.13856524454488

    <Category("4. Flight options")> _
    <DisplayName("b) Y coordinate to fly to")> _
    Public Property ZoomY() As Double
        Get
            Return MyZoomY
        End Get
        Set(ByVal value As Double)
            MyZoomY = value
        End Set
    End Property
    Private MyZoomY As Double = -0.6493599074819

    <Category("4. Flight options")> _
    <DisplayName("c) Zoom per step")> _
    Public Property ZoomFactor() As Double
        Get
            Return MyZoomFactor
        End Get
        Set(ByVal value As Double)
            MyZoomFactor = value
        End Set
    End Property
    Private MyZoomFactor As Double = 1.2

    <Category("4. Flight options")> _
    <DisplayName("d) Steps to calculate")> _
    Public Property Steps() As Integer
        Get
            Return MySteps
        End Get
        Set(ByVal value As Integer)
            MySteps = value
        End Set
    End Property
    Private MySteps As Integer = 10

    <Category("5. Misc")> _
    <DisplayName("a) Update every ... ms")> _
    Public Property RefreshInterval() As Integer
        Get
            Return MyRefreshInterval
        End Get
        Set(ByVal value As Integer)
            MyRefreshInterval = value
        End Set
    End Property
    Private MyRefreshInterval As Integer = 10000

    <Category("5. Misc")> _
    <DisplayName("c) Color steps")> _
    Public Property ColorSteps() As Integer
        Get
            Return MyColorSteps
        End Get
        Set(ByVal value As Integer)
            MyColorSteps = value
            Me.ColorProfile = Me.ColorProfile
        End Set
    End Property
    Private MyColorSteps As Integer = 128

    Public Property ColorProfile() As eColorProfile
        Get
            Return MyColorProfile
        End Get
        Set(ByVal value As eColorProfile)
            MyColorProfile = value
            m_Colors.Clear()
            Select Case value
                Case eColorProfile.BlackAndWhite
                    Dim Stepping As Integer = CInt((255 / (Me.ColorSteps / 2)))
                    For Idx As Integer = 0 To 255 Step Stepping
                        AddColor(Color.FromArgb(Idx, Idx, Idx))
                    Next Idx
                    For Idx As Integer = 255 - Stepping To 0 Step -Stepping
                        AddColor(Color.FromArgb(Idx, Idx, Idx))
                    Next Idx
                Case eColorProfile.Red
                    Dim Stepping As Integer = CInt((255 / (Me.ColorSteps / 2)))
                    For Idx As Integer = 0 To 255 Step Stepping
                        AddColor(Color.FromArgb(Idx, 0, 0))
                    Next Idx
                    For Idx As Integer = 255 - Stepping To 0 Step -Stepping
                        AddColor(Color.FromArgb(Idx, 0, 0))
                    Next Idx
                Case eColorProfile.Rainbow
                    Dim Stepping As Integer = CInt((360 / (Me.ColorSteps)))
                    For Idx As Integer = Stepping To 360 Step Stepping
                        Dim NewColor As Color = cColorMaps.HLS_to_RGB(Idx, 0.5, 1)
                        AddColor(NewColor)
                    Next Idx
            End Select
        End Set
    End Property
    Private MyColorProfile As eColorProfile = eColorProfile.BlackAndWhite

    '''<summary>Add a new color to the list of available colors.</summary>
    '''<param name="NewColor">New color to be added.</param>
    Public Sub AddColor(ByVal NewColor As Color)
        m_Colors.Add(NewColor)
    End Sub

    Public Sub ZoomTo(ByRef picCanvas As PictureBox)
        ZoomTo(picCanvas, Me.ZoomX, Me.ZoomY)
    End Sub

    Public Sub ZoomTo(ByRef picCanvas As PictureBox, ByVal X As Double, ByVal Y As Double)
        'Calculate middle and range
        Dim X_range As Double = Me.X_max - Me.X_min
        Dim Y_range As Double = Me.Y_max - Me.Y_min
        Dim RangeRatio As Double = (Y_range / X_range)
        Dim AspectRatio As Double = (picCanvas.ClientSize.Height / picCanvas.ClientSize.Width)
        'Reduce range
        X_range /= Me.ZoomFactor
        Y_range /= Me.ZoomFactor
        'Set new calculation target
        Me.X_min = X - (X_range / 2)
        Me.X_max = X + (X_range / 2)
        Me.Y_min = Y - (Y_range / 2)
        Me.Y_max = Y + (Y_range / 2)
    End Sub

    ' Draw the Mandelbrot set.
    Public Sub DrawMandelbrot(ByRef picCanvas As PictureBox)

        Dim Pixels_width As Integer
        Dim Pixels_height As Integer
        Dim DeltaC_real As Double
        Dim DeltaC_imag As Double

        'Configure update
        Dim NextUpdate As DateTime = Now
        NextUpdate = Now.AddMilliseconds(Me.RefreshInterval)

        If Me.LockToPicture = True Then
            ReDim FractalBitmap_FAST(picCanvas.ClientSize.Width, picCanvas.ClientSize.Height)
            AdjustAspect(picCanvas)
        Else
            ReDim FractalBitmap_FAST(Me.ManWidth, Me.ManHeight)
        End If

        '- DeltaC_real is the change in the real part (X value) for C.
        '- DeltaC_imag is the change in the imaginary part (Y value).
        Pixels_width = FractalBitmap_FAST.GetUpperBound(0)
        Pixels_height = FractalBitmap_FAST.GetUpperBound(1)
        DeltaC_real = (Me.X_max - Me.X_min) / (Pixels_width - 1)
        DeltaC_imag = (Me.Y_max - Me.Y_min) / (Pixels_height - 1)

        Dim WorkerPartSize As Integer = Pixels_width \ Me.Workers

        'Build a list of start indizes
        Dim StartIndexList As New Stack(Of Integer)
        For Idx As Integer = 0 To Pixels_width - 1 Step WorkerPartSize
            StartIndexList.Push(Idx)
        Next Idx

        Dim ListOfWorker As New List(Of System.ComponentModel.BackgroundWorker)
        If Me.Workers >= 1 Then ListOfWorker.Add(BGWorker_1)
        If Me.Workers >= 2 Then ListOfWorker.Add(BGWorker_2)
        If Me.Workers >= 3 Then ListOfWorker.Add(BGWorker_3)
        If Me.Workers >= 4 Then ListOfWorker.Add(BGWorker_4)
        If Me.Workers >= 5 Then ListOfWorker.Add(BGWorker_5)
        If Me.Workers >= 6 Then ListOfWorker.Add(BGWorker_6)
        If Me.Workers >= 7 Then ListOfWorker.Add(BGWorker_7)
        If Me.Workers >= 8 Then ListOfWorker.Add(BGWorker_8)

        Do

            For Each BGWorker As System.ComponentModel.BackgroundWorker In ListOfWorker

                With BGWorker
                    If .IsBusy = False Then
                        Dim StartIndex As Integer = StartIndexList.Pop
                        Dim StopIndex As Integer = StartIndex + WorkerPartSize - 1
                        If StopIndex > Pixels_width - 1 Then StopIndex = Pixels_width - 1
                        .RunWorkerAsync(New Double() {StartIndex, StopIndex, 0, Pixels_height - 1, DeltaC_real, DeltaC_imag})
                    End If
                    If StartIndexList.Count = 0 Then Exit Do
                End With

            Next BGWorker

            If Now >= NextUpdate Then
                picCanvas.Image = ImageProcessor.CreateBMP(FractalBitmap_FAST)
                NextUpdate = Now.AddMilliseconds(Me.RefreshInterval)
            End If
            System.Windows.Forms.Application.DoEvents()

        Loop Until StartIndexList.Count = 0

        Dim IsDone As Boolean
        Do
            System.Windows.Forms.Application.DoEvents()
            IsDone = True
            For Each BGWorker As System.ComponentModel.BackgroundWorker In ListOfWorker
                If BGWorker.IsBusy = True Then IsDone = False
            Next BGWorker
        Loop Until IsDone = True

        'Configure the calculated image to the GUI
        Fractal = ImageProcessor.CreateBMP(FractalBitmap_FAST)

    End Sub

    Private Sub CalculateArea(ByVal X_start As Integer, ByVal X_end As Integer, ByVal Y_start As Integer, ByVal Y_end As Integer, ByVal DeltaC_real As Double, ByVal DeltaC_imag As Double)

        Dim Zr As Double = 0
        Dim Zim As Double = 0
        Dim Z2r As Double = 0
        Dim Z2im As Double = 0

        Dim clr As Integer
        Dim ReaC As Double
        Dim ImaC As Double

        Dim Z_real As Double
        Dim Z_imag As Double
        Dim Z2_real As Double
        Dim Z2_imag As Double

        For X As Integer = X_start To X_end

            ReaC = Me.X_min + X * DeltaC_real

            For Y As Integer = Y_start To Y_end

                ImaC = Me.Y_min + Y * DeltaC_imag

                Z_real = Zr
                Z_imag = Zim
                Z2_real = Z2r
                Z2_imag = Z2im
                clr = 1

                For clr = 1 To Me.MaxIterations - 1

                    'Calculate Z(clr).
                    Z2_real = Z_real * Z_real
                    Z2_imag = Z_imag * Z_imag
                    Z_imag = (2 * Z_imag * Z_real) + ImaC
                    Z_real = Z2_real - Z2_imag + ReaC

                    'Break if maximum magnitude is reached
                    If Z2_real + Z2_imag >= Me.MaxMagSquare Then Exit For

                Next clr

                ' Set the pixel's value.
                Dim ColorIdx As Integer = clr Mod (m_Colors.Count - 1)
                FractalBitmap_FAST(X, Y) = New ImageProcessor.NativePixel(m_Colors(ColorIdx))

            Next Y

        Next X

    End Sub

    Private Sub AdjustAspect(ByRef picCanvas As PictureBox)

        Dim want_aspect As Double
        Dim picCanvas_aspect As Double
        Dim hgt As Double
        Dim wid As Double
        Dim mid As Double

        'Calculate desired and present aspect ratio
        want_aspect = (Me.Y_max - Me.Y_min) / (Me.X_max - Me.X_min)
        picCanvas_aspect = picCanvas.ClientSize.Height / picCanvas.ClientSize.Width

        If want_aspect > picCanvas_aspect Then
            'The selected area is too tall and thin. -> Make it wider.
            wid = (Me.Y_max - Me.Y_min) / picCanvas_aspect
            mid = (Me.X_min + Me.X_max) / 2
            Me.X_min = mid - (wid / 2)
            Me.X_max = mid + (wid / 2)
        Else
            'The selected area is too short and wide. -> Make it taller.
            hgt = (Me.X_max - Me.X_min) * picCanvas_aspect
            mid = (Me.Y_min + Me.Y_max) / 2
            Me.Y_min = mid - (hgt / 2)
            Me.Y_max = mid + (hgt / 2)
        End If

    End Sub

    Private Sub BGWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGWorker_1.DoWork, BGWorker_2.DoWork, BGWorker_3.DoWork, BGWorker_4.DoWork, BGWorker_5.DoWork, BGWorker_6.DoWork, BGWorker_7.DoWork, BGWorker_8.DoWork
        Dim WhatToDo As Double() = CType(e.Argument, Double())
        CalculateArea(CInt(WhatToDo(0)), CInt(WhatToDo(1)), CInt(WhatToDo(2)), CInt(WhatToDo(3)), WhatToDo(4), WhatToDo(5))
    End Sub

End Class
