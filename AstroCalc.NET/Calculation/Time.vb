Option Explicit On
Option Strict On

Public Class TimeCalc

  Public Shared ReadOnly J2000_Zero As Double = Ato.AstroCalc.JulianDateTime(New DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc))

  '''<summary>Describe different between UTC and local time as string.</summary>
  '''<returns>Difference value.</returns>
  Public Shared Function DiffToUTC() As String
    Dim Delta As Double = (Now - DateTime.UtcNow).TotalHours
    If Delta > 0 Then
      Return "+" & Delta.ToString.Trim
    Else
      Return Delta.ToString.Trim
    End If
  End Function

  '''<summary>Convert the passed UTC time to the local time.</summary>
  '''<param name="Local">Local time to convert.</param>
  '''<returns>Local time.</returns>
  Public Shared Function LocalToUTC(ByVal Local As DateTime) As DateTime
    Return Local.AddHours(-(Now - DateTime.UtcNow).TotalHours)
  End Function

  '''<summary>Convert the passed UTC time to the local time.</summary>
  '''<param name="UTC">UTC time to convert.</param>
  '''<returns>Local time.</returns>
  Public Shared Function UTCToLocal(ByVal UTC As DateTime) As DateTime
    Return UTC.AddHours((Now - DateTime.UtcNow).TotalHours)
  End Function

  '''<summary>Calculate the Besselian date from the given JD.</summary>
  '''<param name="JD">Julian date.</param>
  '''<returns>Besselian date.</returns>
  '''<remarks>Taken from http://maia.usno.navy.mil.</remarks>
  Public Shared Function BesselianDate(ByVal JD As Double) As Double
    Return 2000.0 + (MJD(JD) - 51544.03) / 365.2422
  End Function

  '''<summary>Return the Modified Julian Date.</summary>
  '''<param name="JD">Julian Date to convert.</param>
  '''<returns>Modified Julian Date.</returns>
  '''<remarks>Zero is returned for 17th of november 1858 0:00.</remarks>
  Public Shared Function MJD(ByVal JD As Double) As Double
    Return JD - 2400000.5
  End Function


  '''<summary>Return UT2 - UT1.</summary>
  '''<param name="T">Besselian Date.</param>
  Public Shared Function UT2_UT1(ByVal T As Double) As Double
    Return 0.022 * Math.Sin(2 * Math.PI * T) - 0.012 * Math.Cos(2 * Math.PI * T) - 0.006 * Math.Sin(4 * Math.PI * T) + 0.007 * Math.Cos(4 * Math.PI * T)
  End Function

End Class
