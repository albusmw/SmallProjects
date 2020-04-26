Option Explicit On
Option Strict On

Namespace AstroCalc.NET

    Namespace Databases

        Public Class cHenryDraper

            '''<summary>Root URL where to load the content from.</summary>
            Public Property RootURL() As String
                Get
                    Return MyRootURL
                End Get
                Set(value As String)
                    MyRootURL = value
                End Set
            End Property
            Private MyRootURL As String = "http://cdsarc.u-strasbg.fr/vizier/ftp/cats/III/135A/catalog.dat.gz"

            '''<summary>Downloader for content.</summary>
            Private MyDownloader As New AstroCalc.NET.Common.cDownloader

            Public Event Currently(ByVal Info As String)

      Public Catalog As New Dictionary(Of Integer, sHDEEntry)

            'Henry Draper Catalogue
            ' Download: http://cdsarc.u-strasbg.fr/cgi-bin/qcat?III/135A

            'Byte-per-byte Description of file: catalog.dat
            '--------------------------------------------------------------------------------
            '   Bytes Format  Units   Label  Explanations
            '--------------------------------------------------------------------------------
            '   1-  6  I6     ---     HD     [1/272150]+ Henry Draper Catalog (HD) number
            '   7- 18  A12    ---     DM     Durchmusterung identification (1)
            '  19- 20  I2     h       RAh    Hours RA, equinox B1900, epoch 1900.0
            '  21- 23  I3     0.1min  RAdm   Deci-Minutes RA, equinox B1900, epoch 1900.0
            '      24  A1     ---     DE-    Sign Dec, equinox B1900, epoch 1900.0
            '  25- 26  I2     deg     DEd    Degrees Dec, equinox B1900, epoch 1900.0
            '  27- 28  I2     arcmin  DEm    Minutes Dec, equinox B1900, epoch 1900.0
            '      29  I1     ---   q_Ptm    [0/1]? Code for Ptm: 0 = measured, 1 = value
            '                                       inferred from Ptg and spectral type
            '  30- 34  F5.2   mag     Ptm    ? Photovisual magnitude (2)
            '      35  A1     ---   n_Ptm    [C] 'C' if Ptm is combined value with Ptg
            '      36  I1     ---   q_Ptg    [0/1]? Code for Ptg: 0 = measured, 1 = value
            '                                       inferred from Ptm and spectral type
            '  37- 41  F5.2   mag     Ptg    ? Photographic magnitude (2)
            '      42  A1     ---   n_Ptg    [C] 'C' if Ptg is combined value for this
            '                                  entry and the following or preceding entry
            '  43- 45  A3     ---     SpT    Spectral type
            '  46- 47  A2     ---     Int    [ 0-9B] Photographic intensity of spectrum (3)
            '      48  A1     ---     Rem    [DEGMR*] Remarks, see note (4)
            '--------------------------------------------------------------------------------

            Public Structure sHDEEntry : Implements IGenerics
                '''<summary>Generic information.</summary>
                Public Property Star As sGeneric Implements IGenerics.Star
                '''<summary>Henry Draper Catalog (HD) number.</summary>
                Public HD As Integer
                '''<summary>Durchmusterung identification.</summary>
                Public DM As String
                '''<summary>Photovisual magnitude.</summary>
                Public MagnitudePhotovisual As Single
                '''<summary>Photographic magnitude.</summary>
                Public MagnitudePhotographic As Single
            End Structure

      '''<summary>Original file name (as present on server).</summary>
      Public ReadOnly Property OrigFileName() As String
        Get
          Return "catalog.dat.gz"
        End Get
      End Property

      '''<summary>Extracted file name (as present on local disc after extract).</summary>
      Public ReadOnly Property ExtractedFile() As String
        Get
          Return "catalog.dat"
        End Get
      End Property

      '''<summary>Local folder where to find the catalog file.</summary>
      Public ReadOnly Property LocalFolder() As String
        Get
          Return MyLocalFolder
        End Get
      End Property
      Private MyLocalFolder As String = String.Empty

            Public Sub Download()
        MyDownloader.InitWebClient()
        RaiseEvent Currently("Loading Henry Draper Catalogue data from VIZIER FTP ...")
        MyDownloader.DownloadFile(RootURL, LocalFolder & "\" & OrigFileName)
        GZIP.DecompressTo(LocalFolder & "\" & OrigFileName, LocalFolder & "\" & ExtractedFile)
        System.IO.File.Delete(LocalFolder & "\" & OrigFileName)
        RaiseEvent Currently("   DONE.")
            End Sub


      Public Sub New()
        Me.New(String.Empty)
      End Sub

      Public Sub New(ByVal CatalogFolder As String)

        Catalog = New Dictionary(Of Integer, sHDEEntry)
        Dim ErrorCount As Integer = 0

        MyLocalFolder = CatalogFolder
        If System.IO.File.Exists(MyLocalFolder & "\" & ExtractedFile) = False Then Exit Sub

        For Each BlockContent As String In System.IO.File.ReadAllLines(MyLocalFolder & "\" & ExtractedFile, System.Text.Encoding.ASCII)
          'Add some spaces in the back to avoid problems during parsing
          BlockContent &= "                           "
          Try
            Dim NewEntry As New sHDEEntry
            NewEntry.HD = CInt(BlockContent.Substring(0, 6).Trim)
            NewEntry.DM = BlockContent.Substring(6, 12)
            Dim RA As Double = (CInt(BlockContent.Substring(18, 2)) + (CInt(BlockContent.Substring(20, 3)) / 600)) * 15
            Dim DE As Double = (CInt(BlockContent.Substring(24, 2)) + (CInt(BlockContent.Substring(26, 2)) / 60)) * CDbl(IIf(BlockContent.Substring(23, 1) = "-", -1, 1))
            NewEntry.Star = New sGeneric(RA, DE)
            NewEntry.MagnitudePhotovisual = CSng(Val(BlockContent.Substring(29, 5)))
            NewEntry.MagnitudePhotographic = CSng(Val(BlockContent.Substring(36, 5)))
            Catalog.Add(NewEntry.HD, NewEntry)
          Catch ex As Exception
            ErrorCount += 1
          End Try
        Next BlockContent

      End Sub

        End Class

    End Namespace

End Namespace