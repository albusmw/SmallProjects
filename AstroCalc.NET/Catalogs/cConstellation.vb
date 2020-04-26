Option Explicit On
Option Strict On

'''<summary>A list of all available constellations (88 in sum).</summary>
Public Structure sConstellation

  Public IAU As String
  Public Latin As String
  Public Genitive As String
  Public English As String
  Public Area As Integer
  Public Hemisphere As String
  Public AlphaStar As String

  Public German As String

  '''<summary>Constellation boundaries.</summary>
  Public Boundary As List(Of sGeneric)

  Public Sub New(ByVal NewIAU As String, ByVal NewLatin As String, ByVal NewGenitive As String, ByVal NewEnglish As String, ByVal NewArea As Integer, ByVal NewHemisphere As String, ByVal NewAlphaStar As String, ByVal NewGerman As String)
    Me.IAU = NewIAU
    Me.Latin = NewLatin
    Me.Genitive = NewGenitive
    Me.English = NewEnglish
    Me.Area = NewArea
    Me.Hemisphere = NewHemisphere
    Me.AlphaStar = NewAlphaStar
    Me.German = NewGerman
    Me.Boundary = New List(Of sGeneric)
  End Sub

  Public Shared Function Init(ByVal NewIAU As String, ByVal NewLatin As String, ByVal NewGenitive As String, ByVal NewEnglish As String, ByVal NewArea As Integer, ByVal NewHemisphere As String, ByVal NewAlphaStar As String) As sConstellation
    Return Init(NewIAU, NewLatin, NewGenitive, NewEnglish, NewArea, NewHemisphere, NewAlphaStar, String.Empty)
  End Function

  Public Shared Function Init(ByVal NewIAU As String, ByVal NewLatin As String, ByVal NewGenitive As String, ByVal NewEnglish As String, ByVal NewArea As Integer, ByVal NewHemisphere As String, ByVal NewAlphaStar As String, ByVal NewGerman As String) As sConstellation

    Dim RetVal As sConstellation
    RetVal.IAU = NewIAU
    RetVal.Latin = NewLatin
    RetVal.Genitive = NewGenitive
    RetVal.English = NewEnglish
    RetVal.Area = NewArea
    RetVal.Hemisphere = NewHemisphere
    RetVal.AlphaStar = NewAlphaStar
    RetVal.German = NewGerman
    RetVal.Boundary = New List(Of sGeneric)

    Return RetVal

  End Function

End Structure

'''<summary>Information related to constellations.</summary>
Public Class cConstellation

  '''<summary>Shared catalog containing all constellations (hard-coded).</summary>
  Public Shared Catalog As New List(Of sConstellation)(New sConstellation() { _
    New sConstellation("And", "Andromeda", "Andromedae", "Andromeda", 722, "NH", "Alpheratz", "Andromeda"), _
  New sConstellation("Ant", "Antlia", "Antliae", "Air Pump", 239, "SH", "", "Luftpumpe"), _
    New sConstellation("Aps", "Apus", "Apodis", "Bird of Paradise", 206, "SH", "", "Paradiesvogel"), _
    New sConstellation("Aqr", "Aquarius", "Aquarii", "Water Carrier", 980, "SH", "Sadalmelik", "Adler"), _
    New sConstellation("Aql", "Aquila", "Aquilae", "Eagle", 652, "NH-SH", "Altair", "Wassermann"), _
    New sConstellation("Ara", "Ara", "Arae", "Altar", 237, "SH", "", "Altar"), _
    New sConstellation("Ari", "Aries", "Arietis", "Ram", 441, "NH", "Hamal", "Widder"), _
    New sConstellation("Aur", "Auriga", "Aurigae", "Charioteer", 657, "NH", "Capella", "Fuhrmann"), _
    New sConstellation("Boo", "Bootes", "Bootis", "Herdsman", 907, "NH", "Arcturus", "Bärenhüter / Bootes"), _
    New sConstellation("Cae", "Caelum", "Caeli", "Chisel", 125, "SH", "", "Grabstichel"), _
    New sConstellation("Cam", "Camelopardalis", "Camelopardalis", "Giraffe", 757, "NH", "", "Giraffe"), _
    New sConstellation("Cnc", "Cancer", "Cancri", "Crab", 506, "NH", "Acubens", "Krebs"), _
    New sConstellation("CVn", "Canes Venatici", "Canun Venaticorum", "Hunting Dogs", 465, "NH", "Cor Caroli", "Jagdhunde"), _
    New sConstellation("CMa", "Canis Major", "Canis Majoris", "Big Dog", 380, "SH", "Sirius", "Großer Hund"), _
    New sConstellation("CMi", "Canis Minor", "Canis Minoris", "Little Dog", 183, "NH", "Procyon", "Kleiner Hund"), _
    New sConstellation("Cap", "Capricornus", "Capricorni", "Goat ( Capricorn )", 414, "SH", "Algedi", "Steinbock"), _
    New sConstellation("Car", "Carina", "Carinae", "Keel", 494, "SH", "Canopus", "Kiel des Schiffs"), _
    New sConstellation("Cas", "Cassiopeia", "Cassiopeiae", "Cassiopeia", 598, "NH", "Schedar", "Kassiopeia"), _
    New sConstellation("Cen", "Centaurus", "Centauri", "Centaur", 1060, "SH", "Rigil Kentaurus", "Zentaur"), _
    New sConstellation("Cep", "Cepheus", "Cephei", "Cepheus", 588, "SH", "Alderamin", "Kepheus"), _
    New sConstellation("Cet", "Cetus", "Ceti", "Whale", 1231, "SH", "Menkar", "Walfisch"), _
    New sConstellation("Cha", "Chamaeleon", "Chamaleontis", "Chameleon", 132, "SH", "", "Chamäleon"), _
    New sConstellation("Cir", "Circinus", "Circini", "Compasses", 93, "SH", "", "Zirkel"), _
    New sConstellation("Col", "Columba", "Columbae", "Dove", 270, "SH", "Phact", "Taube"), _
    New sConstellation("Com", "Coma Berenices", "Comae Berenices", "Berenice's Hair", 386, "NH", "Diadem", "Haar der Berenike"), _
    New sConstellation("CrA", "Corona Australis", "Coronae Australis", "Southern Crown", 128, "SH", "", "Südliche Krone"), _
    New sConstellation("CrB", "Corona Borealis", "Coronae Borealis", "Northern Crown", 179, "NH", "Alphecca", "Nördliche Krone"), _
    New sConstellation("Crv", "Corvus", "Corvi", "Crow", 184, "SH", "Alchiba", "Rabe"), _
    New sConstellation("Crt", "Crater", "Crateris", "Cup", 282, "SH", "Alkes", "Becher"), _
    New sConstellation("Cru", "Crux", "Crucis", "Southern Cross", 68, "SH", "Acrux", "Kreuz des Südens"), _
    New sConstellation("Cyg", "Cygnus", "Cygni", "Swan", 804, "NH", "Deneb", "Schwan"), _
    New sConstellation("Del", "Delphinus", "Delphini", "Dolphin", 189, "NH", "Sualocin", "???"), _
    New sConstellation("Dor", "Dorado", "Doradus", "Goldfish", 179, "SH", "", "???"), _
    New sConstellation("Dra", "Draco", "Draconis", "Dragon", 1083, "NH", "Thuban", "???"), _
    New sConstellation("Equ", "Equuleus", "Equulei", "Little Horse", 72, "NH", "Kitalpha", "???"), _
    New sConstellation("Eri", "Eridanus", "Eridani", "River", 1138, "SH", "Achernar", "???"), _
    New sConstellation("For", "Fornax", "Fornacis", "Furnace", 398, "SH", "", "???"), _
    New sConstellation("Gem", "Gemini", "Geminorum", "Twins", 514, "NH", "Castor", "???"), _
    New sConstellation("Gru", "Grus", "Gruis", "Crane", 366, "SH", "Al Na'ir", "???"), _
    New sConstellation("Her", "Hercules", "Herculis", "Hercules", 1225, "NH", "Rasalgethi", "???"), _
    New sConstellation("Hor", "Horologium", "Horologii", "Clock", 249, "SH", "", "???"), _
    New sConstellation("Hya", "Hydra", "Hydrae", "Hydra ( Sea Serpent )", 1303, "SH", "Alphard", "???"), _
    New sConstellation("Hyi", "Hydrus", "Hydri", "Water Serpen ( male )", 243, "SH", "", "???"), _
    New sConstellation("Ind", "Indus", "Indi", "Indian", 294, "SH", "", "???"), _
    New sConstellation("Lac", "Lacerta", "Lacertae", "Lizard", 201, "NH", "", "???"), _
    New sConstellation("Leo", "Leo", "Leonis", "Lion", 947, "NH", "Regulus", "???"), _
    New sConstellation("LMi", "Leo Minor", "Leonis Minoris", "Smaller Lion", 232, "NH", "", "???"), _
    New sConstellation("Lep", "Lepus", "Leporis", "Hare", 290, "SH", "Arneb", "???"), _
    New sConstellation("Lib", "Libra", "Librae", "Balance", 538, "SH", "Zubenelgenubi", "???"), _
    New sConstellation("Lup", "Lupus", "Lupi", "Wolf", 334, "SH", "Men", "???"), _
    New sConstellation("Lyn", "Lynx", "Lyncis", "Lynx", 545, "NH", "", "???"), _
    New sConstellation("Lyr", "Lyra", "Lyrae", "Lyre", 286, "NH", "Vega", "???"), _
    New sConstellation("Men", "Mensa", "Mensae", "Table", 153, "SH", "", "???"), _
    New sConstellation("Mic", "Microscopium", "Microscopii", "Microscope", 210, "SH", "", "???"), _
    New sConstellation("Mon", "Monoceros", "Monocerotis", "Unicorn", 482, "SH", "", "???"), _
    New sConstellation("Mus", "Musca", "Muscae", "Fly", 138, "SH", "", "???"), _
    New sConstellation("Nor", "Norma", "Normae", "Square", 165, "SH", "", "???"), _
    New sConstellation("Oct", "Octans", "Octantis", "Octant", 291, "SH", "", "???"), _
    New sConstellation("Oph", "Ophiuchus", "Ophiuchi", "Serpent Holder", 948, "NH-SH", "Rasalhague", "???"), _
    New sConstellation("Ori", "Orion", "Orionis", "Orion", 594, "NH-SH", "Betelgeuse", "???"), _
    New sConstellation("Pav", "Pavo", "Pavonis", "Peacock", 378, "SH", "Peacock", "???"), _
    New sConstellation("Peg", "Pegasus", "Pegasi", "Winged Horse", 1121, "NH", "Markab", "???"), _
    New sConstellation("Per", "Perseus", "Persei", "Perseus", 615, "NH", "Mirfak", "???"), _
    New sConstellation("Phe", "Phoenix", "Phoenicis", "Phoenix", 469, "SH", "Ankaa", "???"), _
    New sConstellation("Pic", "Pictor", "Pictoris", "Easel", 247, "SH", "", "???"), _
    New sConstellation("Psc", "Pisces", "Piscium", "Fishes", 889, "NH", "Alrischa", "???"), _
    New sConstellation("PsA", "Piscis Austrinus", "Piscis Austrini", "Southern Fish", 245, "SH", "Fomalhaut", "???"), _
    New sConstellation("Pup", "Puppis", "Puppis", "Stern", 673, "SH", "", "???"), _
    New sConstellation("Pyx", "Pyxis", "Pyxidis", "Compass", 221, "SH", "", "???"), _
    New sConstellation("Ret", "Reticulum", "Reticuli", "Reticle", 114, "SH", "", "???"), _
    New sConstellation("Sge", "Sagitta", "Sagittae", "Arrow", 80, "NH", "", "???"), _
    New sConstellation("Sgr", "Sagittarius", "Sagittarii", "Archer", 867, "SH", "Rukbat", "???"), _
    New sConstellation("Sco", "Scorpius", "Scorpii", "Scorpion", 497, "SH", "Antares", "???"), _
    New sConstellation("Scl", "Sculptor", "Sculptoris", "Sculptor", 475, "SH", "", "???"), _
    New sConstellation("Sct", "Scutum", "Scuti", "Shield", 109, "SH", "", "???"), _
    New sConstellation("Ser", "Serpens", "Serpentis", "Serpent", 637, "NH-SH", "Unuck al Hai", "???"), _
    New sConstellation("Sex", "Sextans", "Sextantis", "Sextant", 314, "SH", "", "???"), _
    New sConstellation("Tau", "Taurus", "Tauri", "Bull", 797, "NH", "Aldebaran", "???"), _
    New sConstellation("Tel", "Telescopium", "Telescopii", "Telescope", 252, "SH", "", "???"), _
    New sConstellation("Tri", "Triangulum", "Trianguli", "Triangle", 132, "NH", "Ras al Mothallah", "???"), _
    New sConstellation("TrA", "Triangulum Australe", "Trianguli Australis", "Southern Triangle", 110, "SH", "Atria", "???"), _
    New sConstellation("Tuc", "Tucana", "Tucanae", "Toucan", 295, "SH", "", "???"), _
    New sConstellation("UMa", "Ursa Major", "Ursae Majoris", "Great Bear", 1280, "NH", "Dubhe", "???"), _
    New sConstellation("UMi", "Ursa Minor", "Ursae Minoris", "Little Bear", 256, "NH", "Polaris", "???"), _
    New sConstellation("Vel", "Vela", "Velorum", "Sails", 500, "SH", "", "???"), _
    New sConstellation("Vir", "Virgo", "Virginis", "Virgin", 1294, "NH-SH", "Spica", "???"), _
    New sConstellation("Vol", "Volans", "Volantis", "Flying Fish", 141, "SH", "", "???"), _
    New sConstellation("Vul", "Vulpecula", "Vulpeculae", "Fox", 268, "NH", "", "???") _
  })

    Public Function Verbose(ByVal Abbreviation As String) As String
        Abbreviation = Abbreviation.ToUpper
        For Each Entry As sConstellation In Catalog
            If Entry.IAU.ToUpper = Abbreviation Then Return Entry.Latin
        Next Entry
        Return Abbreviation
  End Function

  Public Function InjectBoundaries(ByVal BoundaryFile As String) As Boolean

    If System.IO.File.Exists(BoundaryFile) = False Then Return False

    For Each Line As String In System.IO.File.ReadAllLines(BoundaryFile)

      Dim RAhr As Double = Coord.DegFromHMS(Line.Substring(0, 10))
      Dim DEdeg As Double = Val(Line.Substring(11, 11).Replace("+", String.Empty))
      Dim cst As String = Line.Substring(23, 4).Trim
      Dim type As String = Line.Substring(28, 1)

      For Each Entry As sConstellation In Catalog
        If Entry.IAU.ToUpper = cst Then
          Entry.Boundary.Add(New sGeneric(RAhr, DEdeg))
        End If
      Next Entry

    Next Line

    DumpBoundaries(0)

    Return True

  End Function

  Private Sub DumpBoundaries(ByVal Idx As Integer)
    Dim DumpLine As New List(Of String)
    For Each Entry As sGeneric In Catalog.Item(Idx).Boundary
      DumpLine.Add(Entry.RightAscension.ToString.Trim & ";" & Entry.Declination.ToString.Trim)
    Next Entry
    System.IO.File.WriteAllLines("D:\Privat\Astro_TEMP\" & Catalog.Item(Idx).IAU & ".csv", DumpLine.ToArray)
  End Sub

  Public Sub New()

    'Translations from here: http://de.wikipedia.org/wiki/Liste_der_Sternbilder_in_verschiedenen_Sprachen

    Catalog = New List(Of sConstellation)
    
  End Sub

End Class
