Option Explicit On
Option Strict On

Public Class cNGC

  Public Catalog As List(Of sNGCEntry)

  'NGC Catalogue
  ' Download: ftp://cdsarc.u-strasbg.fr/pub/cats/VII/118

  'Byte-per-byte Description of file: ngc2000.dat
  '--------------------------------------------------------------------------------
  '   Bytes Format  Units   Label    Explanations
  '--------------------------------------------------------------------------------
  '   1-  5  A5     ---     Name     NGC or IC designation (preceded by I)
  '   7-  9  A3     ---     Type     Object classification (1)
  '  11- 12  I2     h       RAh      Right Ascension B2000 (hours)
  '  14- 17  F4.1   min     RAm      Right Ascension B2000 (minutes)
  '      20  A1     ---     DE-      Declination B2000 (sign)
  '  21- 22  I2     deg     DEd      Declination B2000 (degrees)
  '  24- 25  I2     arcmin  DEm      Declination B2000 (minutes)
  '      27  A1     ---     Source   Source of entry (2)
  '  30- 32  A3     ---     Const    Constellation
  '      33  A1     ---     l_size   [<] Limit on Size
  '  34- 38  F5.1   arcmin  size     ? Largest dimension
  '  41- 44  F4.1   mag     mag      ? Integrated magnitude, visual or photographic
  '                                      (see n_mag)
  '      45  A1     ---     n_mag    [p] 'p' if mag is photographic (blue)
  '  47- 96  A50    ---     Desc     Description of the object (3)
  '--------------------------------------------------------------------------------

  Public Structure sNGCEntry : Implements IGenerics
    '''<summary>NGC or IC designation (preceded by I).</summary>
    Public Name As String
    '''<summary>Object classification.</summary>
    Public Classification As String
    '''<summary>Generic information.</summary>
    Public Property Star As sGeneric Implements IGenerics.Star
    '''<summary>Constellation name.</summary>
    Public Constellation As String
    '''<summary>Largest dimension [arcmin].</summary>
    Public Dimension As Single
    '''<summary>Photovisual magnitude.</summary>
    Public Magnitude As Single
    '''<summary>Description of the object.</summary>
    Public Description As String
    '''<summary>Decide if the element is empty.</summary>
    Public Function IsNothing() As Boolean
      Return Star.IsNothing
    End Function
    Public Shared Function Empty() As sNGCEntry
      Dim RetVal As New sNGCEntry
      With RetVal
        .Name = String.Empty
        .Classification = String.Empty
        .Star.Invalidate()
        .Constellation = String.Empty
        .Dimension = Single.NaN
        .Magnitude = Single.NaN
        .Description = String.Empty
      End With
      Return RetVal
    End Function
  End Structure

  Public ReadOnly Property FileName() As String
    Get
      Return MyFileName
    End Get
  End Property
  Private MyFileName As String = String.Empty

  Public Function GetEntry(ByVal NameToSearch As String) As sNGCEntry
    For Each Entry As sNGCEntry In Catalog
      If Entry.Name = NameToSearch Then Return Entry
    Next Entry
    Return sNGCEntry.Empty
  End Function

  Public Sub New(ByVal FileToLoad As String)

    Catalog = New List(Of sNGCEntry)
    Dim ErrorCount As Integer = 0

    If System.IO.File.Exists(FileToLoad) = False Then Exit Sub
    MyFileName = FileToLoad

    For Each BlockContent As String In System.IO.File.ReadAllLines(FileName, System.Text.Encoding.ASCII)
      Try
        Dim NewEntry As New sNGCEntry
        NewEntry.Name = BlockContent.Substring(0, 5).Trim.Replace("I", "IC") : If NewEntry.Name.StartsWith("I") = False Then NewEntry.Name = "NGC" & NewEntry.Name
        NewEntry.Classification = TranslateClassification(BlockContent.Substring(6, 3))
        Dim RA As Double = Coord.DegFromHMS(BlockContent.Substring(10, 2), BlockContent.Substring(13, 4))
        Dim DE As Double = (CInt(BlockContent.Substring(20, 2)) + (CInt(BlockContent.Substring(23, 2)) / 60)) * CDbl(IIf(BlockContent.Substring(19, 1) = "-", -1, 1))
        NewEntry.Star = New sGeneric(RA, DE)
        NewEntry.Constellation = BlockContent.Substring(29, 3)
        NewEntry.Dimension = CSng(Val(BlockContent.Substring(33, 5)))
        NewEntry.Magnitude = CSng(Val(BlockContent.Substring(40, 4)))
        NewEntry.Description = BlockContent.Substring(46).Replace(", ", ",").Trim
        Catalog.Add(NewEntry)
      Catch ex As Exception
        ErrorCount += 1
      End Try
    Next BlockContent

  End Sub

  Private Function TranslateClassification(ByVal Code As String) As String
    Select Case Code.Trim
      Case "Gx" : Return "Galaxy"
      Case "OC" : Return "Open star cluster"
      Case "Gb" : Return "Globular star cluster, usually in the Milky Way Galaxy"
      Case "Nb" : Return "Bright emission or reflection nebula"
      Case "Pl" : Return "Planetary nebula"
      Case "C+N" : Return "Cluster associated with nebulosity"
      Case "Ast" : Return "Asterism or group of a few stars"
      Case "Kt" : Return "Knot or nebulous region in an external galaxy"
      Case "***" : Return "Triple star"
      Case "D*" : Return "Double star"
      Case "*" : Return "Single star"
      Case "?" : Return "Uncertain type or may not exist"
      Case "-" : Return "Object called nonexistent in the RNGC (Sulentic and Tifft 1973)"
      Case "PD" : Return "Photographic plate defect"
      Case Else : Return "Unidentified at the place given, or type unknown"
    End Select
  End Function

End Class
