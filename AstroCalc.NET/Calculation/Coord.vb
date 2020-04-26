Option Explicit On
Option Strict On

Public Class Coord

    '''<summary>Convert the given hours (decimals allowed) to degree.</summary>
    Public Shared Function DegFromHMS(ByVal Hours As String) As Double
        Return Val(Hours.Replace(",", ".")) * 15
    End Function

    Public Shared Function DegFromHMS(ByVal Hours As String, ByVal Minutes As String) As Double
        Return (CInt(Hours) + (Val(Minutes) / 60)) * 15
    End Function

    Public Shared Function DegFromHMS(ByVal Hours As Integer, ByVal Minutes As Integer, ByVal Seconds As Double) As Double
        Return 15 * (Hours + (Minutes / 60) + (Seconds / 3600))
    End Function

    Public Shared Function DegFromDMS(ByVal Degree As Integer, ByVal Minutes As Integer, ByVal Seconds As Double) As Double
        Return Degree + (Minutes / 60) + (Seconds / 3600)
    End Function

End Class
