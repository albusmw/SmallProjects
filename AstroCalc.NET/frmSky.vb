Option Explicit On
Option Strict On

Public Class frmSky

    Private Sub frmSky_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PlotSky()
    End Sub

    Public Sub PlotSky()

        'Make the Bitmap and Graphics objects. 
        Dim wid As Integer = pbMain.ClientSize.Width
        Dim hgt As Integer = pbMain.ClientSize.Height
        Dim bm As New Bitmap(wid, hgt)
        Dim gr As Graphics = Graphics.FromImage(bm)

        'Plot
        For Each Entry As cNGC.sNGCEntry In DB.NGC.Catalog
            Dim X As Double = Entry.Star.RightAscension * (wid / 360)
            Dim Y As Double = (Entry.Star.Declination + 90) * (hgt / 180)
            gr.DrawEllipse(Pens.Red, New RectangleF(CSng(X - 0.5), CSng(Y - 0.5), 1, 1))
        Next Entry

        'Display the result. 
        pbMain.Image = bm
        pbMain.Refresh()

        gr.Dispose()       

    End Sub


    Private Sub frmSky_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        PlotSky()
    End Sub

End Class