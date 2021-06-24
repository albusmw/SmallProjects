Option Explicit On
Option Strict On

Public Class cImgProcess

    Private BitmapData As Drawing.Imaging.BitmapData = Nothing

    Dim Pixels As Byte() = {}
    Dim Stride As Integer
    Dim ColorBytesPerPixel As Integer

    Public Sub BitmapToMatrix(ByRef BitmapToProcess As Bitmap, ByRef Matrix(,) As UInt16)

        ColorBytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(BitmapToProcess.PixelFormat) \ 8

        'Lock bitmap and return bitmap data
        BitmapData = BitmapToProcess.LockBits(New Drawing.Rectangle(0, 0, BitmapToProcess.Width, BitmapToProcess.Height), Drawing.Imaging.ImageLockMode.ReadWrite, BitmapToProcess.PixelFormat)

        'Copy data from pointer to array
        Dim RequiredSize As Integer = BitmapData.Stride * BitmapToProcess.Height
        If Pixels.Length <> RequiredSize Then Pixels = New Byte(RequiredSize - 1) {}
        Runtime.InteropServices.Marshal.Copy(BitmapData.Scan0, Pixels, 0, Pixels.Length)

        ReDim Matrix(BitmapToProcess.Width - 1, BitmapToProcess.Height - 1)
        For Idx1 As Integer = 0 To Matrix.GetUpperBound(0)
            For Idx2 As Integer = 0 To Matrix.GetUpperBound(1)
                Matrix(Idx1, Idx2) = GetPixel(Idx1, Idx2).R
            Next Idx2
        Next Idx1

        BitmapToProcess.UnlockBits(BitmapData)

    End Sub

    '''<summary>Get the color of the specified pixel.</summary>
    '''<param name="X">X coordinate - from left to right, X=0 is top-left.</param>
    '''<param name="Y">Y coordinate - from top to bottom, Y=0 is top.</param>
    '''<returns>Color value.</returns>
    Private Function GetPixel(x As Integer, y As Integer) As Drawing.Color

        Dim RetVal As Drawing.Color = Drawing.Color.Empty

        ' Get start index of the specified pixel
        Dim i As Integer = (y * BitmapData.Stride) + (x * ColorBytesPerPixel)

        If i > Pixels.Length - ColorBytesPerPixel Then
            Throw New IndexOutOfRangeException()
        End If

        Select Case ColorBytesPerPixel
            Case 1
                'For 8 bpp get color value (Red, Green and Blue values are the same)
                Dim c As Byte = Pixels(i)
                RetVal = Drawing.Color.FromArgb(c, c, c)
            Case 3
                'For 24 bpp get Red, Green and Blue
                Dim b As Byte = Pixels(i)
                Dim g As Byte = Pixels(i + 1)
                Dim r As Byte = Pixels(i + 2)
                RetVal = Drawing.Color.FromArgb(r, g, b)
            Case 4
                'For 32 bpp get Red, Green, Blue and Alpha
                Dim b As Byte = Pixels(i)
                Dim g As Byte = Pixels(i + 1)
                Dim r As Byte = Pixels(i + 2)
                Dim a As Byte = Pixels(i + 3)
                ' a
                RetVal = Drawing.Color.FromArgb(a, r, g, b)
        End Select

        Return RetVal

    End Function

End Class
