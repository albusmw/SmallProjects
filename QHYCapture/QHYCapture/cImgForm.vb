Option Explicit On
Option Strict On

'''<summary>Display a simple picturebox window (form and graph on it).</summary>
Public Class cImgForm

    '''<summary>The form that shall be displayed.</summary>
    Public Hoster As System.Windows.Forms.Form = Nothing
    '''<summary>The ZED graph control inside the form.</summary>
    Public Image As PictureBoxEx = Nothing

    '''<summary>Prepare.</summary>
    Public Sub New()
        If IsNothing(Hoster) = True Then Hoster = New System.Windows.Forms.Form
        If IsNothing(Image) = True Then
            Image = New PictureBoxEx
            Hoster.Controls.Add(Image)
            With Image
                .Dock = Windows.Forms.DockStyle.Fill
                .InterpolationMode = Drawing.Drawing2D.InterpolationMode.NearestNeighbor
                .SizeMode = Windows.Forms.PictureBoxSizeMode.Zoom
                .BackColor = Drawing.Color.Black
            End With
        End If
    End Sub

    Public Function Show(ByVal NewTitle As String) As System.Windows.Forms.PictureBox
        Hoster.Text = NewTitle
        Hoster.Show()
        Return Image
    End Function

End Class