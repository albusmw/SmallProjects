Option Explicit On
Option Strict On

Public Class Form1

    Private Sub pbTestImage_Click(sender As Object, e As EventArgs) Handles pbTestImage.Click

        Dim R As Integer = 1
        Dim R2 As Integer = 15
        Dim PenToUse As New Pen(Color.White)

        Dim g As Graphics = pbTestImage.CreateGraphics()
        For X As Integer = 0 To pbTestImage.Width Step 50
            For Y As Integer = 0 To pbTestImage.Height Step 50
                g.DrawEllipse(PenToUse, New Rectangle(X - R, Y - R, 2 * R, R))
                g.DrawEllipse(PenToUse, New Rectangle(X - R2, Y - R2, 2 * R2, 2 * R2))
            Next Y
        Next X

    End Sub

End Class
