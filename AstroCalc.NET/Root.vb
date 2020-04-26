Option Explicit On
Option Strict On

Public Class Root

  '''<summary>Location the EXE is running from.</summary>
  Public Shared MyPath As String = IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location)
    '''<summary>INI file content.</summary>
    Public Shared INI_IO As New Ato.cINI_IO

    Public Shared CrossIndexKostjuk As New AstroCalc.NET.Databases.cCrossIndexKostjuk

  Public Shared WIKI_Constellations As New AstroCalc.NET.Databases.cWIKI_Constellations
  Public Shared ConstLines As New AstroCalc.NET.Databases.cConstLines

  Public Shared HenryDraper As New AstroCalc.NET.Databases.cHenryDraper
  Public Shared Hipparcos As New AstroCalc.NET.Databases.cHipparcos

  '========================================================================================================================
  'Reflections of the INI content

  '''<summary>Latitude [°].</summary>
  Public Shared ReadOnly Property Latitude() As Double
    Get
            Return INI_IO.Get("Observatory", "Latitude", Double.NaN)
        End Get
  End Property

  '''<summary>Longitude [°].</summary>
  Public Shared ReadOnly Property Longitude() As Double
    Get
            Return INI_IO.Get("Observatory", "Longitude", Double.NaN)
        End Get
  End Property

    '''<summary>Observatory position.</summary>
    Public Shared ReadOnly Property Observatory() As Ato.AstroCalc.sLatLong
        Get
            Return New Ato.AstroCalc.sLatLong(Latitude, Longitude)
        End Get
    End Property

    '''<summary>Height [m].</summary>
    Public Shared ReadOnly Property Height() As Double
    Get
            Return INI_IO.Get("Observatory", "Height", Double.NaN)
        End Get
  End Property

  '''<summary>Catalog root directory.</summary>
  Public Shared ReadOnly Property CatRoot() As String
    Get
            Return INI_IO.Get("Catalog", "Root", MyPath & "\catalog")
        End Get
  End Property

  '''<summary>Wiki constellations root directory.</summary>
  Public Shared ReadOnly Property CatRoot_WIKIConstellations() As String
    Get
            Return CatRoot & "\" & INI_IO.Get("Catalog", "WIKIConstellations", "WIKIConstellations")
        End Get
  End Property

  '''<summary>Henry Draper root directory.</summary>
  Public Shared ReadOnly Property CatRoot_HenryDraper() As String
    Get
            Return CatRoot & "\" & INI_IO.Get("Catalog", "HenryDraper", "HDE")
        End Get
  End Property

  '''<summary>Hipparcos root directory.</summary>
  Public Shared ReadOnly Property CatRoot_Hipparcos() As String
    Get
            Return CatRoot & "\" & INI_IO.Get("Catalog", "Hipparcos", "HIP")
        End Get
  End Property

  '''<summary>Kostjuk cross index root directory.</summary>
  Public Shared ReadOnly Property CatRoot_CrossIndexKostjuk() As String
    Get
            Return CatRoot & "\" & INI_IO.Get("Catalog", "Kostjuk", "Kostjuk")
        End Get
  End Property

  '''<summary>Load the parameters from the INI file to it's components.</summary>
  Public Shared Sub INI_to_Components()

        'Internet download component
        AstroCalc.NET.Common.cDownloader.ConfigureProxy(
      INI_IO.Get("Internet-Proxy", "URL", String.Empty),
      INI_IO.Get("Internet-Proxy", "Port", 80),
      INI_IO.Get("Internet-Authenticate", "Username", "weis_m"),
      INI_IO.Get("Internet-Authenticate", "Password", "havolo37"))

    End Sub

End Class
