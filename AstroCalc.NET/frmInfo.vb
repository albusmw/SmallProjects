Option Explicit On
Option Strict On

Public Class frmInfo

    Public WithEvents UpdateTimer As New cSyncSecondTick

    Private Sub frmInfo_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then e.Cancel = True
    End Sub

    Private Sub frmInfo_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        UpdateTimer.Enabled = True
    End Sub

    Private Sub UpdateTimer_Tick(sender As Object, e As System.EventArgs) Handles UpdateTimer.Tick
        If DB.UpdateGUI Then wbMain.DocumentText = GenerateCode()
    End Sub

    Private Function GenerateCode() As String

        Dim RetVal As New List(Of String)

        Dim CT As DateTime = Now        'capture time to have 1 common base for calculation
        Dim JulianDate As Double = Ato.AstroCalc.JulianDate(CT)
        Dim BesselianDate As Double = TimeCalc.BesselianDate(JulianDate)

        RetVal.Add("<html>")
        RetVal.Add("<basefont size=""1"" color=""#CC0000"" face=""Modern"">")
        RetVal.Add("<body bgcolor=""#000000"">")
        RetVal.Add("<table border=""0"" cellspacing = ""0"" cellpadding =""0"" width=""100%"">")

        RetVal.AddRange(NewRow("Global", "Julian date", MyFormat(PyEphem.GetJulianDate(CT.ToUniversalTime), "0000000.00000"), String.Empty))
        RetVal.AddRange(NewRow(String.Empty, "  (own calc)", Format(Ato.AstroCalc.JulianDateTime(CT.ToUniversalTime), "0000000.00000").Replace(",", "."), String.Empty))
        RetVal.AddRange(NewRow(String.Empty, "Besselian date", MyFormat(BesselianDate, "0000.00000000"), String.Empty))
        RetVal.AddRange(NewRow(String.Empty, "UTC Time", Util.MyFormat(CT.ToUniversalTime), String.Empty))
        RetVal.AddRange(NewRow(String.Empty, "UT2 - UT1", MyFormat(TimeCalc.UT2_UT1(BesselianDate), "00.000"), "s"))
        RetVal.AddRange(NewRow(String.Empty, "GMST(0h UT)", Util.MyTimeFormat(Ato.AstroCalc.GMST(CT))))

        RetVal.AddRange(NewRow("Local", "Time", Util.MyFormat(CT), "(UTC" & TimeCalc.DiffToUTC & ")"))
        RetVal.AddRange(NewRow(String.Empty, "Latitude", Util.LatitudeAsString(Globals.Latitude, False), Util.LatitudeOrientation(Globals.Latitude)))
        RetVal.AddRange(NewRow(String.Empty, "Longitude", Util.LongitudeAsString(Globals.Longitude, False), Util.LongitudeOrientation(Globals.Longitude)))
        RetVal.AddRange(NewRow(String.Empty, "Height", Globals.Elevation.ToString.Trim, "m"))

        Dim SolarInfo As Dictionary(Of String, String) = PyEphem.GetSolarParams
        Dim FirstEntry As Boolean = True

        For Each Entry As String In SolarInfo.Keys
            RetVal.Add("<tr>")
            If FirstEntry = True Then
                RetVal.AddRange(NewRow("Local - Sun", Entry, SolarInfo(Entry)))
            Else
                RetVal.AddRange(NewRow(String.Empty, Entry, SolarInfo(Entry)))
            End If
            FirstEntry = False
        Next Entry



        RetVal.Add("</table>")
        RetVal.Add("</body>")
        RetVal.Add("</html>")

        Return Join(RetVal.ToArray, System.Environment.NewLine)

    End Function

    Private Function NewRow(ByVal C1 As String, ByVal C2 As String, ByVal C3 As String) As List(Of String)
        Return NewRow(C1, C2, C3, String.Empty)
    End Function

    Private Function NewRow(ByVal C1 As String, ByVal C2 As String, ByVal C3 As String, ByVal C4 As String) As List(Of String)

        Dim RetVal As New List(Of String)

        RetVal.Add("<tr>")
        RetVal.Add("<td nowrap>" & C1.Replace(" ", "&nbsp;") & "</td>")
        RetVal.Add("<td nowrap>" & C2.Replace(" ", "&nbsp;") & "</td>")
        RetVal.Add("<td nowrap align=""right"">" & C3.Replace(" ", "&nbsp;") & "</td>")
        RetVal.Add("<td nowrap>&nbsp;" & C4.Replace(" ", "&nbsp;") & "</td>")
        RetVal.Add("</tr>")

        Return RetVal

    End Function

    Private Function MyFormat(ByVal Value As Double, ByVal FormatToUse As String) As String
        Return Format(Value, FormatToUse).Replace(",", ".")
    End Function

End Class