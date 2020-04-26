Option Explicit On
Option Strict On

Public Class Util

    Public Shared Function MyFormat() As String
        Return MyFormat(Now)
    End Function

    'Get the current data in PyEphem format
    Public Shared Function MyFormat(ByVal DateAndTime As DateTime) As String
        Return Format(DateAndTime, "dd.MM.yyyy HH:mm:ss")
    End Function

    'Get the current data in PyEphem format
    Public Shared Function MyTimeFormat(ByVal DateAndTime As DateTime) As String
        Return Format(DateAndTime, "HH:mm:ss")
    End Function

    'Get the current data in PyEphem format
    Public Shared Function PyEphemDate() As String
        Return PyEphemDate(Now)
    End Function

    'Get the current data in PyEphem format
    Public Shared Function PyEphemDate(ByVal DateAndTime As DateTime) As String
        Return Format(DateAndTime, "yyyy.MM.dd").Replace(".", "/") & " " & Format(DateAndTime, "HH:mm:ss")
    End Function

    'Get the current data in PyEphem format
    Public Shared Function PyEphemDate(ByVal DateAndTime As String) As DateTime
        If String.IsNullOrEmpty(DateAndTime) = False Then
            Dim Parts As String() = Split(DateAndTime.Replace("/", ":").Replace(" ", ":"), ":")
            Return New DateTime(CInt(Parts(0)), CInt(Parts(1)), CInt(Parts(2)), CInt(Parts(3)), CInt(Parts(4)), CInt(Parts(5)))
        Else
            Return New DateTime(0)
        End If
    End Function

    Public Shared Function LatitudeAsString(ByVal Latitude As Double, ByVal Orientation As Boolean) As String

        Dim FullDegree As Integer
        Dim FullMinutes As Integer
        Dim Seconds As Double
        SplitToDMS(Latitude, FullDegree, FullMinutes, Seconds)

        Dim RetVal As String = FormatDMS(FullDegree, FullMinutes, Seconds)
        If Orientation = True Then RetVal &= " " & LatitudeOrientation(Latitude)
        Return RetVal

    End Function

    Public Shared Function LatitudeOrientation(ByVal Latitude As Double) As String
        Return CStr(IIf(Latitude > 0, "(N)", "(S)"))
    End Function

    Public Shared Function LongitudeAsString(ByVal Longitude As Double, ByVal Orientation As Boolean) As String

        Dim FullDegree As Integer
        Dim FullMinutes As Integer
        Dim Seconds As Double
        SplitToDMS(Longitude, FullDegree, FullMinutes, Seconds)

        Dim RetVal As String = FormatDMS(FullDegree, FullMinutes, Seconds)
        If Orientation = True Then RetVal &= " " & LongitudeOrientation(Longitude)
        Return RetVal

    End Function

    Public Shared Function FormatDMS(ByVal FullDegree As Integer, ByVal FullMinutes As Integer, ByVal Seconds As Double) As String
        Return FullDegree.ToString.Trim.PadLeft(3) & "° " & FullMinutes.ToString.Trim.PadLeft(2) & "' " & Format(Seconds, "00.000").Replace(",", ".") & """"
    End Function

    Public Shared Function LongitudeOrientation(ByVal Longitude As Double) As String
        Return CStr(IIf(Longitude > 0, "(E)", "(W)"))
    End Function

    Private Shared Sub SplitToDMS(ByVal Degree As Double, ByRef FullDegree As Integer, ByRef FullMinutes As Integer, ByRef Seconds As Double)
        FullDegree = CInt(Fix(Degree))
        Dim Minutes As Double = (Math.Abs(Degree) - Math.Abs(FullDegree)) * 60
        FullMinutes = CInt(Fix(Minutes))
        Seconds = (Minutes - FullMinutes) * 60
    End Sub

    Public Shared Function FormatTimeSpan(ByVal Span As TimeSpan) As String
        Return Span.Hours.ToString.Trim.PadLeft(2) & "h " & Span.Minutes.ToString.Trim.PadLeft(2) & "m " & Span.Seconds.ToString.Trim.PadLeft(2) & "s"
    End Function

    Public Shared Function RightAscension(ByVal RA As String) As Double
        If RA.ToUpper.Contains("H") Then
            'Hour format
            RA = RA.Replace(" ", String.Empty).Replace("''", String.Empty).Replace("""", String.Empty)
            RA = RA.Replace("h", "|").Replace("'", "|").Replace("+", String.Empty).Trim
            RA = RA.Replace(",", ".")
            Dim Parts As String() = Split(RA, "|")
            If Parts.Length = 3 Then
                Return (Val(Parts(0)) + (Val(Parts(1)) / 60) + (Val(Parts(2)) / 3600)) * 15
            End If
        End If
    End Function

    Public Shared Function RightAscension(ByVal RA As Double) As String
        RA = RA / 15
        Dim FullHours As Integer = CInt(Fix(RA))
        RA = (RA - FullHours) * 60
        Dim FullMinutes As Integer = CInt(Fix(RA))
        Dim Seconds As Double = (RA - FullMinutes) * 60
        Return FullHours.ToString.Trim & "h " & FullMinutes.ToString.ToUpper & "m " & Format(Seconds, "##.000").Replace(",", ".") & "s"
    End Function

    Public Shared Function Declination(ByVal RA As String) As Double
        RA = RA.Replace(" ", String.Empty).Replace("''", String.Empty).Replace("""", String.Empty)
        RA = RA.Replace("°", "|").Replace("'", "|").Replace("+", String.Empty).Trim
        RA = RA.Replace(",", ".")
        Dim Parts As String() = Split(RA, "|")
        If Parts.Length = 3 Then
            Return (Val(Parts(0)) + (Val(Parts(1)) / 60) + (Val(Parts(2)) / 3600))
        End If
    End Function

    Public Shared Function Declination(ByVal DE As Double) As String

        Dim FullDegree As Integer
        Dim FullMinutes As Integer
        Dim Seconds As Double
        SplitToDMS(DE, FullDegree, FullMinutes, Seconds)
        Return FormatDMS(FullDegree, FullMinutes, Seconds)

    End Function


End Class
