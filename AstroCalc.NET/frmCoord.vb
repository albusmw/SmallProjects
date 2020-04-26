Option Explicit On
Option Strict On

Public Class frmCoord

    Private Sub DegreeEntry(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDegree.TextChanged, tbDegreeMin.TextChanged, tbDegreeSec.TextChanged
        If tbDegree.Focused = True Or tbDegreeMin.Focused = True Or tbDegreeSec.Focused = True Then
            Dim RaFloat As Double
            'Update float field
            RaFloat = ValEx(tbDegree) + (ValEx(tbDegreeMin) / 60) + (ValEx(tbDegreeSec) / 3600)
            tbFloat.Text = StrEx(RaFloat)
            'Update hour field
            RaFloat = ValEx(tbFloat)
            Dim Hours As Integer = CInt(Fix(RaFloat / 15)) : tbHours.Text = StrEx(Hours)
            RaFloat = (RaFloat - (Hours * 15)) * (60 / 15)
            Dim Minutes As Integer = CInt(Fix(RaFloat)) : tbHoursMin.Text = StrEx(Minutes)
            RaFloat = (RaFloat - Minutes) * 60 : tbHoursSec.Text = StrEx(RaFloat)
        End If
    End Sub

    Private Sub HourEntry(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHours.TextChanged, tbHoursMin.TextChanged, tbHoursSec.TextChanged
        If tbHours.Focused = True Or tbHoursMin.Focused = True Or tbHoursSec.Focused = True Then
            Dim RaFloat As Double
            'Update float field
            RaFloat = (ValEx(tbHours) + (ValEx(tbHoursMin) / 60) + (ValEx(tbHoursSec) / 3600)) * 15
            tbFloat.Text = StrEx(RaFloat)
            'Update degree field
            Dim MoveUp As Double
            Dim SecDegree As Double = ValEx(tbHoursSec) * 15
            If SecDegree >= 60 Then
                MoveUp = CInt(Fix(SecDegree / 60))
                SecDegree = SecDegree - (MoveUp * 60)
            Else
                MoveUp = 0
            End If
            tbDegreeSec.Text = StrEx(SecDegree)
            Dim MinDegree As Double = (ValEx(tbHoursMin) * 15 + MoveUp)
            If MinDegree >= 60 Then
                MoveUp = CInt(Fix(MinDegree / 60))
                MinDegree = MinDegree - (MoveUp * 60)
            Else
                MoveUp = 0
            End If
            tbDegreeMin.Text = StrEx(MinDegree)
            tbDegree.Text = StrEx(ValEx(tbHours) * 15 + MoveUp)
        End If
    End Sub

    Private Sub FloatEntry(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbFloat.TextChanged
        If tbFloat.Focused = True Then
            Dim RaFloat As Double
            'Update hour field
            RaFloat = ValEx(tbFloat)
            Dim Hours As Integer = CInt(Fix(RaFloat / 15)) : tbHours.Text = StrEx(Hours)
            RaFloat = (RaFloat - (Hours * 15)) * (60 / 15)
            Dim Minutes As Integer = CInt(Fix(RaFloat)) : tbHoursMin.Text = StrEx(Minutes)
            RaFloat = (RaFloat - Minutes) * 60 : tbHoursSec.Text = StrEx(RaFloat)
            'Update degree field
            RaFloat = ValEx(tbFloat)
            Dim Degree As Integer = CInt(Fix(RaFloat)) : tbDegree.Text = StrEx(Degree)
            RaFloat = (RaFloat - Degree) * 60
            Minutes = CInt(Fix(RaFloat)) : tbDegreeMin.Text = StrEx(Minutes)
            RaFloat = (RaFloat - Minutes) * 60 : tbDegreeSec.Text = StrEx(RaFloat)
        End If
    End Sub

    Private Function ValEx(ByRef Box As TextBox) As Double
        Return ValEx(Box.Text)
    End Function

    Private Function ValEx(ByRef Box As String) As Double
        Return Val(Box.Replace(",", "."))
    End Function

    Private Function StrEx(ByRef Value As Double) As String
        Return Str(Value).Replace(",", ".").Trim
    End Function

    Private Function StrEx(ByRef Value As Integer) As String
        Return Str(Value).Replace(",", ".").Trim
    End Function

    Private Sub btnFromClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFromClipboard.Click
        Dim ClipText As String = Clipboard.GetText
        Dim UnitValues As String() = {"M", "S", "'", """", "′′", "′"}
        Const Sep As String = "|"
        If ClipText.ToUpper.Contains("H") Then
            ClipText = ClipText.ToUpper.Replace(" ", String.Empty)
            ClipText = ReplaceChars(ClipText, UnitValues, Sep)
            ClipText = ClipText.Replace("H", Sep).Trim
            If ClipText.EndsWith(Sep) Then ClipText = ClipText.Substring(0, ClipText.Length - 1)
            Dim Splitted As String() = Split(ClipText, Sep)
            If Splitted.Length = 3 Then
                tbHours.Focus()
                tbHours.Text = StrEx(ValEx(Splitted(0)))
                tbHoursMin.Text = StrEx(ValEx(Splitted(1)))
                tbHoursSec.Text = StrEx(ValEx(Splitted(2)))
            End If
        End If
        If ClipText.Contains("°") Or ClipText.ToUpper.Contains("DEG") Then
            ClipText = ClipText.ToUpper.Replace(" ", String.Empty)
            ClipText = ReplaceChars(ClipText, UnitValues, Sep)
            ClipText = ClipText.Replace("°", Sep).Replace("DEG", Sep).Trim
            If ClipText.EndsWith(Sep) Then ClipText = ClipText.Substring(0, ClipText.Length - 1)
            Dim Splitted As String() = Split(ClipText, Sep)
            If Splitted.Length = 3 Then
                tbDegree.Focus()
                tbDegree.Text = StrEx(ValEx(Splitted(0)))
                tbDegreeMin.Text = StrEx(ValEx(Splitted(1)))
                tbDegreeSec.Text = StrEx(ValEx(Splitted(2)))
            End If
        End If
    End Sub

    Private Function ReplaceChars(ByVal Original As String, ByVal ReplaceThis As IEnumerable(Of String), ByVal ReplaceBy As String) As String
        Dim RetVal As String = Original
        For Each Entry As String In ReplaceThis
            RetVal = RetVal.Replace(Entry, ReplaceBy)
        Next
        Return RetVal
    End Function

End Class