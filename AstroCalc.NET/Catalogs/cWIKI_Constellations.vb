Option Explicit On
Option Strict On

Namespace AstroCalc.NET

  Namespace Databases

    '''<summary>Constellation start information taken from the wikipedia.</summary>
    Public Class cWIKI_Constellations

      '================================================================================
      ' Events
      '--------------------------------------------------------------------------------

      '================================================================================
      ' Properties
      '--------------------------------------------------------------------------------

      '''<summary>Root URL where to load the content from.</summary>
      Public Property RootURL() As String
        Get
          Return MyRootURL
        End Get
        Set(value As String)
          MyRootURL = value
        End Set
      End Property
      Private MyRootURL As String = "http://en.wikipedia.org/w/index.php"

      Public ReadOnly Property CatalogSize() As Long
        Get
          If IsNothing(LoadedCatalog) = True Then Return 0 Else Return LoadedCatalog.Count
        End Get
      End Property

      '================================================================================
      ' Privates
      '--------------------------------------------------------------------------------

      '''<summary>Downloader for content.</summary>
      Private MyDownloader As New AstroCalc.NET.Common.cDownloader
      '''<summary>Local directory to use for load and store.</summary>
      Private LocalCatalogDirectory As String = String.Empty

      '''<summary>Event to see what is going on.</summary>
      Public Event Currently(ByVal Info As String)

      Public LoadedCatalog As List(Of sGeneric)

      '================================================================================
      ' Constructor
      '--------------------------------------------------------------------------------

      Public Sub New()
        Me.New(String.Empty)
      End Sub

      Public Sub New(ByRef LocalCatalogDirectoryToUse As String)
        LocalCatalogDirectory = LocalCatalogDirectoryToUse
        LoadedCatalog = New List(Of sGeneric)
        For Each Entry As sConstellation In cConstellation.Catalog
          Load(LocalCatalogDirectory, Entry)
        Next Entry
      End Sub

      '================================================================================
      ' Functions
      '--------------------------------------------------------------------------------

      Private Function WIKIName(ByVal Entry As sConstellation) As String
        Return Entry.Latin.Replace(" ", "_")
      End Function

      Public Sub DownloadBoundaries(ByRef LocalCatalogFile As String)
        MyDownloader.InitWebClient()
        RaiseEvent Currently("Loading constellation boundaries")
        Dim Webpage As String = "ftp://cdsarc.u-strasbg.fr/pub/cats/VI/49/bound_20.dat"
        Dim PageContent As String = MyDownloader.DownloadString(Webpage)
        System.IO.File.WriteAllText(LocalCatalogFile, PageContent)
        RaiseEvent Currently("Boundaries loaded")
      End Sub

      '''<summary>Load the content from Wikipedia to the specified local folder.</summary>
      Public Sub Download()
        MyDownloader.InitWebClient()
        For Each Entry As sConstellation In cConstellation.Catalog
          RaiseEvent Currently("Loading " & Entry.English)
          Dim WebPage As String = RootURL & "?title=List_of_stars_in_" & WIKIName(Entry) & "&printable=yes"
          Dim PageContent As String = MyDownloader.DownloadString(WebPage)
          System.IO.File.WriteAllText(LocalCatalogDirectory & "\" & WIKIName(Entry) & ".html", PageContent, System.Text.Encoding.UTF8)
        Next Entry
        RaiseEvent Currently("All constellations loaded")
      End Sub

      '''<summary>Load the specified constellation from disc.</summary>
      Private Sub Load(ByVal LocalCatalogDirectory As String, ByVal Constellation As sConstellation)

        'Load file if present
        Dim FileName As String = LocalCatalogDirectory & "\" & WIKIName(Constellation) & ".html"
        If System.IO.File.Exists(FileName) = False Then Exit Sub
        Dim PC As String = System.IO.File.ReadAllText(FileName, System.Text.Encoding.UTF8)

        'Get table element
        Dim Start As Integer = PC.IndexOf("<table")
        Dim StopIdx As Integer = PC.IndexOf("</table")
        PC = PC.Substring(Start, StopIdx - Start + 1)

        'Simplify
        PC = PC.Replace("&#160;", " ")
        PC = PC.Replace("<sup>h</sup>", "h")
        PC = PC.Replace("<sup>m</sup>", "'")
        PC = PC.Replace("<sup>s</sup>", "''")
        PC = PC.Replace("Â°", "°")
        PC = PC.Replace("â€²", "'")
        PC = PC.Replace("â€³", "''")
        PC = PC.Replace("âˆ’", "-")
        PC = PC.Replace(Chr(10), String.Empty)
        PC = PC.Replace(Chr(13), String.Empty)

        'Get all table lines
        Dim AllStars As String() = Split(PC, "</tr><tr>")

        'Parse the header line to get the meaning of each column
        Dim ColHD As Integer = -1
        Dim ColRA As Integer = -1
        Dim ColDEC As Integer = -1
        Dim ColNotes As Integer = -1
        Dim ColVisMag As Integer = -1

        Dim ColHeaders As String() = Split(AllStars(0), "</th>")
        For Idx As Integer = 0 To ColHeaders.GetUpperBound(0) - 1
          Dim DC As String = ColHeaders(Idx)
          If DC.ToUpper.Contains(">HD<") Then ColHD = Idx
          If DC.ToUpper.Contains(">RA<") Then ColRA = Idx
          If DC.ToUpper.Contains(">DEC<") Then ColDEC = Idx
          If DC.ToUpper.Contains(">VIS.<") Then ColVisMag = Idx
          If DC.ToUpper.Contains(">NOTES") Then ColNotes = Idx
        Next

        'Parse data columns
        For Idx As Integer = 1 To AllStars.GetUpperBound(0) - 1

          Dim NewEntry As sGeneric

          'Get the line to process
          Dim ToSplit As String = AllStars(Idx).Trim      'get line that represents 1 star
          RemoveAttributes(ToSplit, "td")                          'clean up the td tag
          ToSplit = ToSplit.Replace("</td><td>", "|")     'use td tag so split

          'Remove initial td and split with td
          If ToSplit.StartsWith("<td>") Then ToSplit = ToSplit.Substring(4)
          If ToSplit.EndsWith("</td>") Then ToSplit = ToSplit.Substring(0, ToSplit.Length - 5)

          Dim Splitted As String() = Split(ToSplit, "|")

                    NewEntry.RightAscension = AstroParser.ParseRA(Splitted(ColRA))
                    NewEntry.Declination = AstroParser.ParseDeclination(Splitted(ColDEC))
                    NewEntry.Magnitude = New sGeneric.sMagnitude(Val(Splitted(ColVisMag)), sGeneric.eMagnitudeType.Visual)
          NewEntry.Cat = New sGeneric.sCat(Splitted(ColHD), sGeneric.eCatType.HD)

          'Process the notes line
          Dim Notes As String = String.Empty
          If Splitted.GetUpperBound(0) >= ColNotes Then

            Notes = RemoveHTMLTags(Splitted(ColNotes)).Trim.Replace(", ", "|")

            'Extract common comments - stars
            Detect(Notes, "spectroscopic binary")
            Detect(Notes, "binary star")
            Detect(Notes, "double star")
            Detect(Notes, "triple star")
            Detect(Notes, "quadruple star")
            Detect(Notes, "multiple star")

            'Extract common comments - planets
            Detect(Notes, "has a planet")
            Detect(Notes, "has a transiting planet")
            Detect(Notes, "has two transiting planets")

            'Extract common comments - pulsars
            Detect(Notes, "millisecond pulsar")
            Detect(Notes, "pulsar")
            Detect(Notes, "semiregular variable")
            Detect(Notes, "variable star")
            Detect(Notes, "suspected variable")
            Detect(Notes, "classical Cepheid")

            'Parse remaining part
            Dim Splitter As String() = Split(Notes, "|")
            For SpIdx As Integer = 0 To Splitter.GetUpperBound(0)
              'Detect variables type
              Splitter(SpIdx) = Splitter(SpIdx).Replace("variables", "variable")
              If Splitter(SpIdx).Contains("variable") Then
                Splitter(SpIdx) = Splitter(SpIdx).Replace("variable", String.Empty)
                Dim VarType As String = Splitter(SpIdx)
                Splitter(SpIdx) = String.Empty
              End If
              'Detect Vmax =
              If Splitter(SpIdx).StartsWith("Vmax =") Then
                Dim V_max As String = Splitter(SpIdx).Replace("Vmax =", String.Empty).Trim
                Splitter(SpIdx) = String.Empty
              End If
              'Detect Vmin =
              If Splitter(SpIdx).StartsWith("Vmin =") Then
                Dim V_min As String = Splitter(SpIdx).Replace("Vmin =", String.Empty).Trim
                Splitter(SpIdx) = String.Empty
              End If
              'Detect ΔV =
              If Splitter(SpIdx).StartsWith("ΔV =") Then
                Dim DeltaV As String = Splitter(SpIdx).Replace("ΔV =", String.Empty).Trim
                Splitter(SpIdx) = String.Empty
              End If
              'Detect P = 
              If Splitter(SpIdx).StartsWith("P = ") Then
                Dim P As String = Splitter(SpIdx).Replace("P = ", String.Empty).Trim
                Splitter(SpIdx) = String.Empty
              End If
            Next SpIdx
            Notes = Join(Splitter, "|")

            Notes = Notes.Replace("(b)", String.Empty).Replace(";", String.Empty).Trim

          End If

          'Get link to HD catalog
          If String.IsNullOrEmpty(Splitted(ColHD)) = False Then
            Try
              Dim HDText As String = Splitted(ColHD)
              'Remove "<..."
              If HDText.StartsWith("<") Then
                HDText = HDText.Substring(HDText.IndexOf(">") + 1)
                HDText = HDText.Substring(0, HDText.IndexOf("<") - 1)
              End If
              'Remove A and B
              HDText = HDText.Replace(" ", String.Empty)
              Dim HenryDraperText As String = HDText.Replace("A", String.Empty).Replace("B", String.Empty).Replace("C", String.Empty)
              Dim HenryDraper As Integer = CInt(Split(HenryDraperText, "<br/>")(0).Trim)




              'If IsNothing(DB.HenryDraper) = False Then
              '  If DB.HenryDraper.Catalog.ContainsKey(HenryDraper) = False Then
              '    Debug.Print("ID <" & HenryDraper.ToString.Trim & "> missing in " & Constellation.Latin)
              '  End If
              'End If

            Catch ex As Exception
              Debug.Print(ex.Message)
            End Try
          End If

          LoadedCatalog.Add(NewEntry)

        Next Idx

      End Sub

      Private Function Detect(ByRef NoteText As String, ByVal TextToDetect As String) As Boolean
        If NoteText.Contains(TextToDetect) Then
          NoteText = NoteText.Replace(TextToDetect, String.Empty).Trim
          Return True
        Else
          Return False
        End If
      End Function

      '''<summary>Remove all HTML attributes from the specified tag.</summary>
      Private Sub RemoveAttributes(ByRef Text As String, ByVal TagToSearch As String)
        Dim StartIdx As Integer = 0
        Do
          StartIdx = Text.IndexOf("<" & TagToSearch, StartIdx)
          If StartIdx > -1 Then
            Dim StopIdx As Integer = Text.IndexOf(">", StartIdx)
            If StopIdx > -1 Then
              Dim CutLength As Integer = StopIdx - (StartIdx + 1 + TagToSearch.Length)
              Dim NewText As String = Text.Substring(0, StartIdx + 1 + TagToSearch.Length) & Text.Substring(StopIdx)
              StartIdx += 1
              Text = NewText
            End If
          End If
        Loop Until StartIdx = -1
      End Sub

      '''<summary>Remove all HTML tags.</summary>
      Private Function RemoveHTMLTags(ByVal Text As String) As String
        Return System.Text.RegularExpressions.Regex.Replace(Text, "<.*?>", "")
      End Function

    End Class

  End Namespace

End Namespace

