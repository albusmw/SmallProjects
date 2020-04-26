Option Explicit On
Option Strict On

Public Class MainForm

  Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'TEST CODE ...
        'Test.Go()
        'End

        Me.Show()
    DB.LogForm = New frmLog
    DB.LogForm.MdiParent = Me
    DB.LogForm.Show()
    System.Windows.Forms.Application.DoEvents()

    'Load environment
    DB.LogForm.PartLine("Init directory structure ...")
    Dim RetVal As String = DB.InitDirectoryStructure()
    If String.IsNullOrEmpty(RetVal) = False Then
      DB.LogForm.FullLine("FAILED:")
      DB.LogForm.PartLine("  " & RetVal)
    Else
      DB.LogForm.FullLine("OK.")
    End If

    'Load PyEphem environment
    DB.LogForm.PartLine("Loading python...")
        Dim PyEphemCheck As String = PyEphem.CheckIfInstalled("C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python37_64\python.exe")
        If String.IsNullOrEmpty(PyEphemCheck) = False Then
      DB.LogForm.FullLine("FAILED:")
      DB.LogForm.PartLine("  Error during start-up:" & System.Environment.NewLine & "<" & PyEphemCheck & ">")
    Else
      DB.LogForm.FullLine("OK.")
    End If

    'Load build-in constellations list
    DB.LogForm.PartLine("Loading constellations ...")
    DB.Constellation = New cConstellation
    DB.LogForm.FullLine("OK, " & cConstellation.Catalog.Count.ToString.Trim & " constellations loaded.")

    'Loading boundaries
    DB.LogForm.PartLine("Loading constellation boundaries ...")
    DB.Constellation.InjectBoundaries(DB.File_ConstellationBoundary)
    DB.LogForm.FullLine("OK")

    'Load Henry Draper catalogue
    DB.LogForm.PartLine("Loading catalog: Henry Draper ...")
    DB.HenryDraper = New AstroCalc.NET.Databases.cHenryDraper(DB.File_HD)
    DB.LogForm.FullLine("OK, " & DB.HenryDraper.Catalog.Count.ToString.Trim & " elements loaded.")

    'Load WIKI data
    DB.LogForm.PartLine("Loading WIKI data for constellations ...")
    DB.WIKI_Constellations = New AstroCalc.NET.Databases.cWIKI_Constellations(DB.Path_WIKI_Constellations)
    DB.LogForm.FullLine("OK, " & DB.WIKI_Constellations.CatalogSize.ToString.Trim & " elements loaded.")

    'Load NGC catalog
    DB.LogForm.PartLine("Loading catalog: NGC...")
    DB.NGC = New cNGC(DB.Path_NGC)
    DB.LogForm.FullLine("OK, " & DB.NGC.Catalog.Count.ToString.Trim & " elements loaded.")

    'Show tracking form
    Dim InfoForm As New frmInfo
    InfoForm.Show(Me)

  End Sub

  Private Sub DSSRequestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DSSRequestToolStripMenuItem.Click
        Dim DSS As New frmDSS
        DSS.Show(Me)
    End Sub

    Private Sub SkyViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkyViewToolStripMenuItem.Click
        Dim SkyView As New frmSky
        SkyView.Show()
    End Sub

    Private Sub CoordinateEntryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CoordinateEntryToolStripMenuItem.Click
        Dim Coord As New frmCoord
        Coord.Show()
    End Sub

    Private Sub LiveImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LiveImageToolStripMenuItem.Click
        Dim LiveImage As New frmLiveImage
        LiveImage.Show()
    End Sub

  Private Sub WIKIConstellationPagesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WIKIConstellationPagesToolStripMenuItem.Click
        DB.WIKI_Constellations.Download()
        DB.WIKI_Constellations = New AstroCalc.NET.Databases.cWIKI_Constellations(DB.Path_WIKI_Constellations)
  End Sub


  Private Sub HenryDraperCatalogueToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles HenryDraperCatalogueToolStripMenuItem.Click
        DB.HenryDraper.Download()
  End Sub

  Private Sub ConstallationBoundariesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ConstallationBoundariesToolStripMenuItem.Click
    'Load constellation boundaries
        DB.WIKI_Constellations.DownloadBoundaries(DB.File_ConstellationBoundary)
  End Sub

  Private Sub IAUConstellationsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles IAUConstellationsToolStripMenuItem.Click
    Dim NewForm As New frmExternalCharts
    NewForm.Show()
  End Sub

    Private Sub HorizonToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HorizonToolStripMenuItem.Click
        Horizons_Loader.Go()
    End Sub

End Class