Option Explicit On
Option Strict On

Public Class DB

  Private Shared SubPath_WIKI_Constellations As String = "WIKI Constellations"
    Public Shared Path_WIKI_Constellations As String = Root.CatRoot & "\" & SubPath_WIKI_Constellations

  Private Shared SubPath_ConstellationBoundary As String = "ConstellationBoundary"
    Public Shared Path_ConstellationBoundary As String = Root.CatRoot & "\" & SubPath_ConstellationBoundary
  Public Shared File_ConstellationBoundary As String = Path_ConstellationBoundary & "\bound_20.dat"

  Private Shared SubPath_HD As String = "HenryDraper"
    Public Shared Path_HD As String = Root.CatRoot & "\" & SubPath_HD
  Public Shared File_HD As String = Path_HD & "\catalog.dat"

    Public Shared Path_NGC As String = Root.CatRoot & "\NGC2000.0\ngc2000.dat"

  '''<summary>The main log form.</summary>
  Public Shared LogForm As frmLog

    Public Shared WithEvents HenryDraper As AstroCalc.NET.Databases.cHenryDraper

  '''<summary>NGC catalog.</summary>
  Public Shared WithEvents NGC As cNGC

  '''<summary>Constellation catalog.</summary>
  Public Shared WithEvents Constellation As cConstellation

  '''<summary>Constellation catalog from WIKI.</summary>
    Public Shared WithEvents WIKI_Constellations As AstroCalc.NET.Databases.cWIKI_Constellations

  Public Shared UpdateGUI As Boolean = True

  '''<summary>Web client to be used.</summary>
  Public Shared Downloader As System.Net.WebClient

  '''<summary>Init the directory structure.</summary>
  '''<returns>Empty string if everything worked file, error string else.</returns>
  Public Shared Function InitDirectoryStructure() As String
        If System.IO.Directory.Exists(Root.CatRoot) = False Then Return "Root directory <" & Root.CatRoot & "> does not exist."
    If System.IO.Directory.Exists(Path_WIKI_Constellations) = False Then System.IO.Directory.CreateDirectory(Path_WIKI_Constellations)
    If System.IO.Directory.Exists(Path_ConstellationBoundary) = False Then System.IO.Directory.CreateDirectory(Path_ConstellationBoundary)
    If System.IO.Directory.Exists(Path_HD) = False Then System.IO.Directory.CreateDirectory(Path_HD)
    Return String.Empty
  End Function

  Public Shared Sub InitWebClient(ByVal UseProxy As Boolean)
    If IsNothing(Downloader) Then Downloader = New System.Net.WebClient()
    If UseProxy Then ConfigForRSProxy(Downloader)
  End Sub

  'Configure the connection to work with the R&S firewall
  Public Shared Sub ConfigForRSProxy(ByRef Connection As Net.WebClient)
    Dim cr As New System.Net.NetworkCredential("weis_m", "havolo37")
    Dim pr As New System.Net.WebProxy("proxy-emea.rsint.net", 80)
    Connection.Proxy = pr
    Connection.Proxy.Credentials = cr
  End Sub

  Private Shared Sub WIKI_Constellations_Currently(ByVal Info As String) Handles WIKI_Constellations.Currently
    LogForm.FullLine("  WIKI constellation: " & Info)
  End Sub

End Class
