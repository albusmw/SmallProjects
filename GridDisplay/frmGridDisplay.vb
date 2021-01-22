Option Explicit On
Option Strict On

Public Class frmGridDisplay

    Private Sub AddKey(ByVal Row As Integer, ByVal Col As Integer, ByVal Text As String)
        With dgvMain.Rows(Row).Cells(Col)
            .Value = Text.Trim : .Style.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Style.Font = New Font(dgvMain.DefaultCellStyle.Font, FontStyle.Bold)
        End With
    End Sub

    Private Sub AddSub(ByVal Row As Integer, ByVal Col As Integer, ByVal Text As String)
        With dgvMain.Rows(Row).Cells(Col)
            .Value = Text.Trim : .Style.Alignment = DataGridViewContentAlignment.MiddleRight
            .Style.Font = New Font(dgvMain.DefaultCellStyle.Font, FontStyle.Regular)
        End With
    End Sub

    Private Sub AddValue(ByRef Row As Integer, ByVal Col As Integer, ByVal Text As String, ByVal BackColor As Color)
        With dgvMain.Rows(Row).Cells(Col)
            .Value = Text : .Style.Alignment = DataGridViewContentAlignment.MiddleRight
            .Style.Font = New Font("Courier New", 8, FontStyle.Regular)
            .Style.BackColor = BackColor
            .Style.SelectionBackColor = BackColor
        End With
        Row += 1
    End Sub

    Public Sub UpdateDisplay()

        With dgvMain

            .Columns.Clear()

            .ScrollBars = ScrollBars.None
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
            .DefaultCellStyle.SelectionBackColor = .DefaultCellStyle.BackColor
            .DefaultCellStyle.SelectionForeColor = .DefaultCellStyle.ForeColor
            .Columns.Add("C1", String.Empty)
            .Columns.Add("C2", String.Empty)
            .Columns.Add("C3", String.Empty)
            .Columns.Add("C4", String.Empty)
            .Columns.Add("C5", String.Empty)
            .Columns.Add("C6", String.Empty)
            .Columns.Add("C7", String.Empty)
            .Columns.Add("C8", String.Empty)
            .RowHeadersVisible = False
            .ColumnHeadersVisible = False
            .Rows.Add(15)
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None

            Dim RowHeight As Integer = CInt(Math.Floor(dgvMain.Height / .Rows.Count))
            For Idx As Integer = 0 To .Rows.Count - 1
                .Rows(Idx).Height = RowHeight
            Next Idx

            .Columns(2).Width = 20 : .Columns(2).DefaultCellStyle.BackColor = Color.LightGray
            .Columns(5).Width = 20 : .Columns(5).DefaultCellStyle.BackColor = Color.LightGray

            Dim AvailableWidth As Integer = dgvMain.Width - 40
            Dim ColWidth As Integer = CInt(Math.Floor(AvailableWidth / ((.Columns.Count - 2))))
            For Idx As Integer = 0 To .Columns.Count - 1
                If Idx <> 2 And Idx <> 5 Then .Columns(Idx).Width = ColWidth
            Next Idx

            Dim UTC As DateTime = Now.ToUniversalTime

            Dim MoonPos As Ato.AstroCalc.sAzAlt = Ato.AstroCalc.MoonPosition(UTC)
            Dim Moon As String = "ALT: " & Ato.AstroCalc.Format360Degree(MoonPos.ALT) & System.Environment.NewLine & "AZ:  " & Ato.AstroCalc.Format360Degree(MoonPos.AZ)

            Dim PolarisPos As Ato.AstroCalc.sAzAlt = GetPosition(Ato.AstroCalc.KnownStars.Polaris)
            Dim Polaris As String = "ALT: " & Ato.AstroCalc.Format360Degree(PolarisPos.ALT) & System.Environment.NewLine & "AZ:  " & Ato.AstroCalc.Format360Degree(PolarisPos.AZ)

            Dim AltairPos As Ato.AstroCalc.sAzAlt = GetPosition(Ato.AstroCalc.KnownStars.Altair)
            Dim Altair As String = "ALT: " & Ato.AstroCalc.Format360Degree(AltairPos.ALT) & System.Environment.NewLine & "AZ:  " & Ato.AstroCalc.Format360Degree(AltairPos.AZ)

            Dim R0 As Integer
            Dim C0 As Integer

            R0 = 0
            C0 = 0
            AddKey(C0, R0, "Mount        ") : AddValue(C0, RightOf(R0), "Parked", Color.Green)
            AddSub(C0, R0, "RA           ") : AddValue(C0, RightOf(R0), "12° 13' 45.3"" N", Color.White)
            AddSub(C0, R0, "DEC          ") : AddValue(C0, RightOf(R0), "+22° 43' 45.3""", Color.White)

            C0 = 4
            AddKey(C0, R0, "CDK          ") : AddValue(C0, RightOf(R0), "Cooling", Color.Orange)
            AddSub(C0, R0, "Fans         ") : AddValue(C0, RightOf(R0), "OFF", Color.Red)

            C0 = 11
            AddKey(C0, R0, "Dome         ") : AddValue(C0, RightOf(R0), "CLOSED", Color.Green)
            AddSub(C0, R0, "Temperature  ") : AddValue(C0, RightOf(R0), "0.0 °C", Color.White)
            AddSub(C0, R0, "Fans         ") : AddValue(C0, RightOf(R0), Database.DomeFans, Database.ToColor(Database.DomeFans))
            AddSub(C0, R0, "A/C          ") : AddValue(C0, RightOf(R0), Database.AirCondition, Database.ToColor(Database.AirCondition))
            AddSub(C0, R0, "Lights       ") : AddValue(C0, RightOf(R0), Database.DomeLights, Database.ToColor(Database.DomeLights))

            R0 = 3
            C0 = 0
            AddKey(C0, R0, "SBIG STX     ") : AddValue(C0, RightOf(R0), Database.SBIG_Power, Database.ToColor(Database.SBIG_Power))
            AddSub(C0, R0, "Communication") : AddValue(C0, RightOf(R0), "Idle", Color.Green)
            AddSub(C0, R0, "Temperature  ") : AddValue(C0, RightOf(R0), "-12.2 °C", Color.Green)
            AddSub(C0, R0, "Setpoint     ") : AddValue(C0, RightOf(R0), "-12.2 °C", Color.White)

            R0 = 6
            C0 = 0
            AddKey(C0, R0, "Current Time ") : AddValue(C0, RightOf(R0), Ato.AstroCalc.TimeForDisplay(Now), Color.White)
            AddSub(C0, R0, "UTC          ") : AddValue(C0, RightOf(R0), Ato.AstroCalc.TimeForDisplay(UTC), Color.White)
            AddSub(C0, R0, "LST          ") : AddValue(C0, RightOf(R0), Ato.AstroCalc.LSTFormated(UTC, DB.Location_Long), Color.White)
            AddSub(C0, R0, "JD           ") : AddValue(C0, RightOf(R0), Format(Ato.AstroCalc.JulianDate(Now), "0.000000"), Color.White)
            AddSub(C0, R0, "Latitude     ") : AddValue(C0, RightOf(R0), Ato.AstroCalc.Format360Degree(DB.Location_Lat), Color.White)
            AddSub(C0, R0, "Longitude    ") : AddValue(C0, RightOf(R0), Ato.AstroCalc.Format360Degree(DB.Location_Long), Color.White)

            C0 = 8
            AddKey(C0, R0, "Objects      ") : AddValue(C0, RightOf(R0), "...", Color.White)
            AddSub(C0, R0, "Polaris      ") : AddValue(C0, RightOf(R0), Polaris, Color.White)
            AddSub(C0, R0, "Altair       ") : AddValue(C0, RightOf(R0), Altair, Color.White)
            'AddSub(C0, R0, "Moon        ") : AddValue(C0, RightOf(R0), Moon, Color.White)

        End With

    End Sub

    Private Function GetPosition(ByVal AstroObject As Ato.AstroCalc.sRADec) As Ato.AstroCalc.sAzAlt
        Return Ato.AstroCalc.GetHorizontalPosition(Now.ToUniversalTime, New Ato.AstroCalc.sLatLong(DB.Location_Lat, DB.Location_Long), AstroObject)
    End Function

    Private Function RightOf(ByVal R As Integer) As Integer
        If R + 1 <= dgvMain.Columns.Count - 1 Then Return R + 1 Else Return dgvMain.Columns.Count - 1
    End Function

    Private Sub frmGridDisplay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateDisplay()
    End Sub

End Class
