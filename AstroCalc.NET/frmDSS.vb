Option Explicit On
Option Strict On

Public Class frmDSS

    Dim Magnification As Double = 0.5

    '''<summary></summary>
    '''<param name="RightAscension"></param>
    '''<param name="Declination"></param>
    '''<param name="Height">Height [arcminutes].</param>
    '''<param name="Width">Width [arcminutes].</param>
    '''<returns>URL to run the selected request.</returns>
    Private Function FormRequest(ByVal RightAscension As Double, ByVal Declination As Double, ByVal Height As Double, ByVal Width As Double) As String

        'See http://archive.stsci.edu/dss/script_usage.html for details on how to receive an image

        Dim RetVal As New List(Of String)

        RetVal.Add("v=poss2ukstu_red")                  'which version of the survey to use
        RetVal.Add("r=" & Str(RightAscension).Trim)     'right ascension
        RetVal.Add("d=" & Str(Declination).Trim)        'declination
        RetVal.Add("e=J2000")                           'equinox (B1950 or J2000; default: J2000)
        RetVal.Add("h=" & Str(Height).Trim)             'height of image (arcminutes; default: 15.0)
        RetVal.Add("w=" & Str(Width).Trim)               'width of image (arcminutes; default: 15.0)
        RetVal.Add("f=gif")                             'image format (FITS or GIF; default: FITS)
        RetVal.Add("c=none")                            'compression (UNIX, GZIP, or NONE; default: NONE; compression applies to FITS only)
        RetVal.Add("s=on")                              'save the file to disk instead of trying to display. (ON (or anything) or not defined; default: not defined.)
        RetVal.Add("fov=NONE")                          'which FOV to overlay (NONE, LAUNCH, SM93, SM97; default: NONE)
        RetVal.Add("v3=0")                  '           V3 roll angle (default: 0.0)

        tbShowing.Text = "Showing RA " & Util.RightAscension(RightAscension) & " / DE " & Util.Declination(Declination)

        Dim Query As String = "http://archive.stsci.edu/cgi-bin/dss_search?" & Join(RetVal.ToArray, "&")

        Return Query

    End Function

    Private Sub tbSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSearch.TextChanged

        Dim NGCObject As cNGC.sNGCEntry = DB.NGC.GetEntry(tbSearch.Text.Replace(" ", String.Empty))

        If NGCObject.IsNothing = False Then
            tbSearch.BackColor = Color.LimeGreen
            Dim URL As String = FormRequest(NGCObject.Star.RightAscension, NGCObject.Star.Declination, NGCObject.Dimension / Magnification, NGCObject.Dimension / Magnification)
            LoadPictureBox(URL)
        Else
            tbSearch.BackColor = Color.Red
        End If

    End Sub

    Private Sub LoadPictureBox(ByVal URL As String)
        Dim Loader As New System.Net.WebClient
        Dim ImageDate As Byte() = Loader.DownloadData(URL)
        pbMain.Image = Image.FromStream(New IO.MemoryStream(ImageDate))
    End Sub

End Class