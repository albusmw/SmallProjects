Option Explicit On
Option Strict On

Public Class Form1

    Private Sub pbTestImage_Click(sender As Object, e As EventArgs) Handles pbTestImage.Click
        TestImage_DotInCircle(1, 15, 100, 100, Color.Red)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        For Each CommandArg As String In My.Application.CommandLineArgs
            If CommandArg.StartsWith("[") And CommandArg.EndsWith("]") Then
                Dim Color As String() = CommandArg.Substring(1, CommandArg.Length - 2).Split(CType(",", Char()))
                If Color.Length = 3 Then
                    pbTestImage.BackColor = Drawing.Color.FromArgb(CInt(Color(0)), CInt(Color(1)), CInt(Color(2)))
                End If
            End If
        Next CommandArg
    End Sub

    Private Sub TestImage_DotInCircle(ByVal R As Integer, ByVal R2 As Integer, ByVal DistX As Integer, ByVal DistY As Integer, ByVal ColorToUse As Drawing.Color)
        Dim PenToUse As New Pen(ColorToUse)
        Dim g As Graphics = pbTestImage.CreateGraphics()
        For X As Integer = 0 To pbTestImage.Width Step DistX
            For Y As Integer = 0 To pbTestImage.Height Step DistY
                g.DrawEllipse(PenToUse, New Rectangle(X - R, Y - R, 2 * R, R))
                g.DrawEllipse(PenToUse, New Rectangle(X - R2, Y - R2, 2 * R2, 2 * R2))
            Next Y
        Next X
    End Sub

End Class
