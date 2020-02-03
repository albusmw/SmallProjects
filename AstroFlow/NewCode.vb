Option Explicit On
Option Strict On

'Code that is new implemented and does not know where to go goes here ...
Public Class NewCode

    Public Shared Sub ScaleData(ByRef FITSHeader As List(Of cFITSHeaderChanger.sHeaderElement), ByRef ImageData(,) As Int32)

        'Apply scaling if indicated
        If DB.Settings.FITS_UseBZERO Then
            Dim BZERO As String = cFITSHeaderChanger.GetHeaderValue(FITSHeader, eFITSKeywords.BZERO)
            If String.IsNullOrEmpty(BZERO) = False Then
                Dim BZeroValue As Int32 = CInt(BZERO)                                                       'Does not handle floating point values ...
                For Idx0 As Integer = 0 To ImageData.GetUpperBound(0)
                    For Idx1 As Integer = 0 To ImageData.GetUpperBound(1)
                        ImageData(Idx0, Idx1) += BZeroValue
                    Next Idx1
                Next Idx0
            End If
        End If

    End Sub


End Class
