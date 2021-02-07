Option Explicit On
Option Strict On

Public Class MainForm

    Dim WithEvents Mandel As New cMandel

    Dim StopNow As Boolean = False
    Dim ZoomRunning As Boolean

    Dim CombinedBitmapAndZoombox As Bitmap
    Dim m_ZoomingGraphics As Graphics

    Dim Zoom_start_X As Double
    Dim Zoom_start_Y As Double
    Dim Zoom_end_X As Double
    Dim Zoom_end_Y As Double

    Private Sub tsbStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbStart.Click

        'Dim A As New Emil.GMP.BigInt("1234567890123456789012345678901234567890123456789012345678901234567890")
        'Dim B As New Emil.GMP.BigInt("1234567890123456789012345678901234567890123456789012345678901234567890")
        'Dim C As Emil.GMP.BigInt = A * B
        'MsgBox(C.ToString)

        If tsbStart.Text = "Start" Then
            tsbStart.Text = "Stop"
            StopNow = False
            Calculate()
        Else
            StopNow = True
        End If

    End Sub

    Private Sub Calculate()

        For Idx As Integer = 1 To Mandel.Steps
            tsslMain.Text = Idx.ToString.Trim & " / " & Mandel.Steps.ToString.Trim
            If Mandel.ZoomFactor <> 1 Then Mandel.ZoomTo(pbMain)
            Mandel.DrawMandelbrot(pbMain)
            pbMain.Image = Mandel.Fractal
            pbMain.Invalidate()
            pbMain.Refresh()
            pgMain.SelectedObject = Mandel
            System.Windows.Forms.Application.DoEvents()
            If StopNow = True Then Exit For
        Next Idx

        tsbStart.Text = "Start"

    End Sub

    Private Sub BlackWhiteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Mandel.ColorProfile = cMandel.eColorProfile.BlackAndWhite
    End Sub

    Private Sub RedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Mandel.ColorProfile = cMandel.eColorProfile.Red
    End Sub

    Private Sub RainbowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Mandel.ColorProfile = cMandel.eColorProfile.Rainbow
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pgMain.SelectedObject = Mandel
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        With Mandel
            .X_min = -2.2
            .X_max = 2.2
            .Y_min = -1.2
            .Y_max = 1.2
        End With
        UpdateGraphics()
    End Sub

    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Dim ImageFormat As System.Drawing.Imaging.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp
        Dim Path As String = Microsoft.VisualBasic.FileIO.SpecialDirectories.MyPictures & "\Fractal.bmp"
        Mandel.Fractal.Save(Path, ImageFormat)
        MsgBox("Saved to <" & Path & ">")
    End Sub

#Region "Rubberbanding"

    'Start rubberband drawing.
    Private Sub picCanvas_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbMain.MouseDown

        'Starting zoom action -> activate and store init components
        ZoomRunning = True
        Zoom_start_X = e.X
        Zoom_start_Y = e.Y
        Zoom_end_X = Zoom_start_X
        Zoom_end_Y = Zoom_start_Y

        ' Make a copy of the current bitmap.
        CombinedBitmapAndZoombox = Mandel.Fractal
        m_ZoomingGraphics = Graphics.FromImage(CombinedBitmapAndZoombox)

    End Sub

    ' Continue rubberband drawing.
    Private Sub picCanvas_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbMain.MouseMove

        'Exit if not zooming
        If Not ZoomRunning Then Exit Sub

        ' Save the new corner.
        Zoom_end_X = e.X
        Zoom_end_Y = e.Y

        ' Draw the new box.
        Dim gr As Graphics = pbMain.CreateGraphics()
        Dim rect As New Rectangle
        rect.X = CInt(Min(Zoom_start_X, Zoom_end_X))
        rect.Y = CInt(Min(Zoom_start_Y, Zoom_end_Y))
        rect.Width = CInt(System.Math.Abs(Zoom_end_X - Zoom_start_X))
        rect.Height = CInt(System.Math.Abs(Zoom_end_Y - Zoom_start_Y))

        m_ZoomingGraphics.DrawImage(Mandel.Fractal, 0, 0)
        m_ZoomingGraphics.DrawRectangle(Pens.White, rect)

        ' Display the result.
        pbMain.Image = CombinedBitmapAndZoombox

    End Sub

    ' Finish rubberband drawing.
    Private Sub pbMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbMain.MouseUp

        'If zoom is not running, exit, else switch off zooming
        If Not ZoomRunning Then Exit Sub
        ZoomRunning = False

        'Save the new corner.
        Zoom_end_X = e.X
        Zoom_end_Y = e.Y

        'Dispose of the zooming Bitmap and Graphics.
        pbMain.Image = Nothing
        CombinedBitmapAndZoombox.Dispose()
        m_ZoomingGraphics.Dispose()
        CombinedBitmapAndZoombox = Nothing
        m_ZoomingGraphics = Nothing

        'Put the selected coordinates in order.
        Dim x1, x2, y1, y2 As Double
        x1 = Min(Zoom_start_X, Zoom_end_X)
        x2 = Max(Zoom_start_X, Zoom_end_X)
        y1 = Min(Zoom_start_Y, Zoom_end_Y)
        y2 = Max(Zoom_start_Y, Zoom_end_Y)

        'Set area to focus on

        'Convert screen coords into drawing coords - X
        Dim Factor_X As Double = Mandel.X_span / pbMain.ClientSize.Width
        Zoom_start_X = Mandel.X_min + (x1 * Factor_X)
        Zoom_end_X = Mandel.X_min + (x2 * Factor_X)
        Mandel.X_min = Zoom_start_X
        Mandel.X_max = Zoom_end_X

        'Convert screen coords into drawing coords - Y
        Dim Factor_Y As Double = Mandel.Y_span / pbMain.ClientSize.Height
        Zoom_start_Y = Mandel.Y_min + (y1 * Factor_Y)
        Zoom_end_Y = Mandel.Y_min + (y2 * Factor_Y)
        Mandel.Y_min = Zoom_start_Y
        Mandel.Y_max = Zoom_end_Y

        'Calculate graphics
        UpdateGraphics()

    End Sub

    Public Sub UpdateGraphics()

        'Draw the new Mandelbrot set.
        Mandel.DrawMandelbrot(pbMain)
        pgMain.SelectedObject = Mandel
        pbMain.Image = Mandel.Fractal
        pbMain.Invalidate()
        pbMain.Refresh()

    End Sub

    Private Function Min(ByVal X1 As Double, ByVal X2 As Double) As Double
        If X1 < X2 Then Return X1 Else Return X2
    End Function

    Private Function Max(ByVal X1 As Double, ByVal X2 As Double) As Double
        If X1 > X2 Then Return X1 Else Return X2
    End Function

#End Region ' Rubberbanding

    Private Sub Mandel_PropertyChanged() Handles Mandel.PropertyChanged
        UpdateGraphics()
    End Sub
End Class
