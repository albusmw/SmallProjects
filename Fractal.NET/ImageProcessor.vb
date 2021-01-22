Option Explicit On
Option Strict On

Public Class ImageProcessor

    '''<summary>One native pixel.</summary>
    Public Structure NativePixel

        Public R As Byte
        Public G As Byte
        Public B As Byte
        Public Sub New(ByVal R As Byte, ByVal G As Byte, ByVal B As Byte)
            Me.R = R : Me.G = G : Me.B = B
        End Sub
        Public Sub New(ByVal Value As Drawing.Color)
            Me.R = Value.R : Me.G = Value.G : Me.B = Value.B
        End Sub

    End Structure

    '''<summary>Create a new bitmap from the passed binary pixel array.</summary>
    '''<param name="NativArray">The binary array to get the bitmap from.</param>
    '''<returns>Bitmap containing the native pixel elements.</returns>
    Public Shared Function CreateBMP(ByRef NativArray(,) As NativePixel) As Drawing.Bitmap

        Const Padding As Integer = 0                            'unknown parameter ...

        Dim X As Integer                                        'X position
        Dim Y As Integer                                        'Y position
        Dim LockArea As Drawing.Rectangle                       'area to get data from
        Dim LockData As System.Drawing.Imaging.BitmapData       'locked area
        Dim FirstPixel As IntPtr                                'pointer to first pixel
        Dim Bytes As Integer                                    'number of total pixel
        Dim RGBArray() As Byte                                  'raw array to get data from
        Dim RGBPointer As Integer                               'pointer within the RGBArray

        Dim NewBMP As Drawing.Bitmap                            'the new bitmap

        'Create a new bitmap
        NewBMP = New Drawing.Bitmap(NativArray.GetUpperBound(0) + 1, NativArray.GetUpperBound(1) + 1 + Padding)

        'Lock the bitmap's bits.  
        Bytes = NewBMP.Width * NewBMP.Height * 4
        LockArea = New Drawing.Rectangle(0, 0, NewBMP.Width, NewBMP.Height)
        LockData = NewBMP.LockBits(LockArea, Drawing.Imaging.ImageLockMode.WriteOnly, Drawing.Imaging.PixelFormat.Format32bppRgb)

        'Get the address of the first line.
        FirstPixel = LockData.Scan0

        ' Declare an array to hold the bytes of the bitmap.
        ' This code is specific to a bitmap with 24 bits per pixels.
        ReDim RGBArray(0 To Bytes - 1)

        'Create the new bitmap - memory order is blue-green-red
        RGBPointer = 0
        For Y = 0 To NewBMP.Height - 1 - Padding
            For X = 0 To NewBMP.Width - 1
                RGBArray(RGBPointer) = NativArray(X, Y).B
                RGBArray(RGBPointer + 1) = NativArray(X, Y).G
                RGBArray(RGBPointer + 2) = NativArray(X, Y).R
                RGBPointer += 4
            Next X
        Next Y

        'Copy the RGB values back to the bitmap
        System.Runtime.InteropServices.Marshal.Copy(RGBArray, 0, FirstPixel, Bytes)

        'Unlock the bits.
        NewBMP.UnlockBits(LockData)

        Return NewBMP

    End Function

End Class
