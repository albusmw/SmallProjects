Option Explicit On
Option Strict On

Namespace AstroCalc.NET

    Public Class DateAndTime

        '''<summary>Convert UT to local time.</summary>
        Public Shared Function UTtoLocal(ByVal Value As DateTime) As DateTime
            Dim Delta As TimeSpan = DateTime.Now - DateTime.UtcNow
            Return Value.AddHours(Delta.Hours)
        End Function

        '''<summary>Calculate the Julian Date from the given date and time in Universal Time.</summary>
        '''<param name="Value">Date value in Universal Time.</param>
        '''<returns>Julian Date.</returns>
        '''<remarks>See http://de.wikipedia.org/wiki/Julianisches_Datum#Astronomisches_Julianisches_Datum for details.</remarks>
        Public Shared Function JDUT(ByVal Value As DateTime) As Double
            Return JDUT(Value.Year, Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second)
        End Function

        '''<summary>Calculate the Julian Date from the given date and time in Universal Time.</summary>
        '''<param name="Year">Year.</param>
        '''<param name="Month">Month.</param>
        '''<param name="Day">Day.</param>
        '''<param name="Hour">Hour.</param>
        '''<param name="Minute">Minute.</param>
        '''<param name="Second">Second.</param>
        '''<returns>Julian Date.</returns>
        '''<remarks>See http://de.wikipedia.org/wiki/Julianisches_Datum#Astronomisches_Julianisches_Datum for details.</remarks>
        Public Shared Function JDUT(ByVal Year As Integer, ByVal Month As Integer, ByVal Day As Integer, ByVal Hour As Integer, ByVal Minute As Integer, ByVal Second As Integer) As Double

            Dim Y As Integer = Year
            Dim M As Integer = Month
            Dim D As Integer = Day

            Dim A As Integer = 0
            Dim B As Integer = 0

            Dim H As Double = (Hour / 24) + (Minute / 1440) + (Second / 86400)

            If M <= 2 Then
                Y -= 1 : M += 12
            End If

            If IsGregorian(Y, M, D) Then
                A = CInt(System.Math.Floor(Y / 100))
                B = CInt(2 - A + System.Math.Floor(A / 4))
            End If

            Return System.Math.Floor(365.25 * (Y + 4716)) + System.Math.Floor(30.6001 * (M + 1)) + D + H + B - 1524.5


        End Function

        '''<summary>Returns the Modified Julian Date.</summary>
        '''<param name="Value">Date to convert.</param>
        '''<returns>Modified Julian Date.</returns>
        Public Function MJD(ByVal Value As DateTime) As Double
            Return JDUT(Value) - 2400000.5
        End Function

        '''<summary>Determine if the date is in the gregorian calender.</summary>
        '''<returns>TRUE for gregorian, FALSE for julian. If neighter nor, an exception is thrown.</returns>
        Private Shared Function IsGregorian(ByVal Year As Integer, ByVal Month As Integer, ByVal Day As Integer) As Boolean
            Select Case Year
                Case Is > 1582 : Return True
                Case Is < 1582 : Return False
                Case Else
                    Select Case Month
                        Case Is > 10 : Return True
                        Case Is < 10 : Return False
                        Case Else
                            Select Case Day
                                Case Is >= 15 : Return True
                                Case Is <= 4 : Return False
                                Case Else : Throw New Exception("Date is not a valid date in gregorian or julian calender")
                            End Select
                    End Select
            End Select

        End Function

    End Class

    Public Class DateAndTime_Test

        '''<summary>Test function..</summary>
        Public Function Test() As Boolean

            If DateAndTime.JDUT(333, 1, 27, 12, 0, 0) <> 1842713 Then Return False
            If DateAndTime.JDUT(New Date(1990, 1, 1, 12, 0, 0)) <> 2447893 Then Return False
            If DateAndTime.JDUT(New Date(2010, 3, 25, 16, 30, 0)) <> 2455281.1875 Then Return False

            Return True

        End Function

    End Class

End Namespace
