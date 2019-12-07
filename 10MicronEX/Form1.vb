Option Explicit On
Option Strict On

'TODO: Star name decoding still does not work correct (VIZIER has not all Ref Stars)

Public Class Form1

    '''<summary>RefStar position that can be hold from the Vizier data.</summary>
    Public Structure sRefStarPos
        '''<summary>Right accension [h].</summary>
        Public RA As Double
        '''<summary>Declination [°].</summary>
        Public Dec As Double
        Public Sub New(ByVal NewRA As Double, ByVal NewDec As Double)
            Me.RA = NewRA
            Me.Dec = NewDec
        End Sub
    End Structure

    Dim MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
    Dim RefStars As New Dictionary(Of String, sRefStarPos)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim port As Int32 = 3490
        Dim client As New Net.Sockets.TcpClient(tbMountIP.Text, port)
        Dim stream As Net.Sockets.NetworkStream = client.GetStream()

        Dim CommunicationLog As New List(Of String)
        Dim CSVOut As New List(Of String)

        CommunicationLog.Add("Firmware date               : <" & GetAnswer(stream, ":GVD#") & ">")
        CommunicationLog.Add("Firmware time               : <" & GetAnswer(stream, ":GVT#") & ">")
        CommunicationLog.Add("Firmware number             : <" & GetAnswer(stream, ":GVN#") & ">")
        CommunicationLog.Add("Product name                : <" & GetAnswer(stream, ":GVP#") & ">")
        CommunicationLog.Add("Control box hardware version: <" & GetAnswer(stream, ":GVZ#") & ">")

        Dim StarCount As String = GetAnswer(stream, ":getalst#")
        CommunicationLog.Add("Alignment loaded with " & StarCount & " stars")
        CommunicationLog.Add("         HH:MM:SS.SS,+dd*mm:ss.s,eeee.e,ppp")
        For Idx As Integer = 1 To CInt(StarCount)
            Dim StarString As String = GetAnswer(stream, ":getalp" & Idx.ToString.Trim & "#").Replace("*", "°")
            Dim Splitted As String() = StarString.Split(CType(",", Char()))
            Dim RA As String() = Splitted(0).Split(CType(":", Char()))
            Dim RAFloat As Double = Val(RA(0)) + (Val(RA(1)) / 60) + (Val(RA(2)) / 3600)
            Dim DE As String() = Splitted(1).Replace("°", ":").Split(CType(":", Char()))
            Dim DEFloat As Double = Val(DE(0)) + (Val(DE(1)) / 60) + (Val(DE(2)) / 3600)
            Dim StarName As String = " (" & DecodeStar(StarString) & ")"
            CommunicationLog.Add("Star " & Format(Idx, "00").Trim & ": " & StarString & StarName.PadRight(30) & "|" & Format(RAFloat, "000.000000") & "|" & Format(DEFloat, "000.000000"))
            CSVOut.Add(Format(RAFloat, "000.000000") & ";" & Format(DEFloat, "000.000000") & ";" & Splitted(2).Trim.Replace(".", ","))
        Next Idx

        tbOutput.Text = Join(CommunicationLog.ToArray, System.Environment.NewLine)
        Clipboard.Clear()
        Clipboard.SetText(Join(CSVOut.ToArray, System.Environment.NewLine))

    End Sub

    Private Function DecodeStar(ByVal StarString As String) As String
        Dim MinErr As Double = Double.MaxValue
        Dim BestStar As String = String.Empty
        Dim StarStringRA As Double = AstroParser.ParseRA(StarString.Split(CType(",", Char()))(0))
        Dim StarStringDec As Double = AstroParser.ParsePosition(StarString.Split(CType(",", Char()))(1))
        For Each RefStar As String In RefStars.Keys
            Dim ErrorRMS As Double = (RefStars(RefStar).RA - StarStringRA) ^ 2 + (RefStars(RefStar).Dec - StarStringDec) ^ 2
            If ErrorRMS < MinErr Then
                MinErr = ErrorRMS
                BestStar = RefStar
            End If
        Next RefStar
        Return BestStar & " (" & MinErr.ToString.Trim & ")"
    End Function

    Private Function GetAnswer(ByRef Stream As Net.Sockets.NetworkStream, ByVal Query As String) As String
        SendQuery(Stream, Query)
        Return ReceiveAnswer(Stream)
    End Function

    Private Sub SendQuery(ByRef Stream As Net.Sockets.NetworkStream, ByVal Query As String)
        Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes(Query)
        Stream.Write(data, 0, data.Length)
    End Sub

    Private Function ReceiveAnswer(ByRef Stream As Net.Sockets.NetworkStream) As String
        Dim Answer As String = String.Empty
        Do
            Dim Received As Integer = Stream.ReadByte
            If Chr(Received) = "#" Then
                Exit Do
            Else
                Answer &= Chr(Received)
            End If
        Loop Until 1 = 0
        Return Answer
    End Function

    Private Sub LoadRefStarDatabase()

        'Generate a list of all alignment stars used by the 10Micron mount
        Dim AllAlignStars As New List(Of String)
        With AllAlignStars
            .Add("Albireo".ToUpper)
            .Add("Aldebaran".ToUpper)
            .Add("Alderamin".ToUpper)
            .Add("Algenib".ToUpper)
            .Add("Alkaid".ToUpper)
            .Add("Alpha Cam".ToUpper)
            .Add("Alpha Fornacis".ToUpper)
            .Add("Alpha Lyncis".ToUpper)
            .Add("Alphard".ToUpper)
            .Add("Alpheratz".ToUpper)
            .Add("Altair".ToUpper)
            .Add("Alula Borealis".ToUpper)
            .Add("Antares".ToUpper)
            .Add("Arcturus".ToUpper)
            .Add("Beta Aqr".ToUpper)
            .Add("Betelgeuse".ToUpper)
            .Add("Capella".ToUpper)
            .Add("Caph".ToUpper)
            .Add("Castor".ToUpper)
            .Add("Cor Caroli".ToUpper)
            .Add("Deneb".ToUpper)
            .Add("Denebola".ToUpper)
            .Add("Diphda".ToUpper)
            .Add("Dubhe".ToUpper)
            .Add("Eltanin".ToUpper)
            .Add("Enif".ToUpper)
            .Add("Gamma Cas".ToUpper)
            .Add("Gemma".ToUpper)
            .Add("Gienah Ghurab".ToUpper)
            .Add("Hamal".ToUpper)
            .Add("Kochab".ToUpper)
            .Add("Lambda Aqr".ToUpper)
            .Add("Menkar".ToUpper)
            .Add("Menkent".ToUpper)
            .Add("Mirach".ToUpper)
            .Add("Mirfak".ToUpper)
            .Add("Muscida".ToUpper)
            .Add("Nu Ophiuchi".ToUpper)
            .Add("Omega Cap".ToUpper)
            .Add("Pi Herculis".ToUpper)
            .Add("Polaris".ToUpper)
            .Add("Pollux".ToUpper)
            .Add("Procyon".ToUpper)
            .Add("Ras Alhague".ToUpper)
            .Add("Regulus".ToUpper)
            .Add("Rho Puppis".ToUpper)
            .Add("Rigel".ToUpper)
            .Add("Scheat".ToUpper)
            .Add("Sirius".ToUpper)
            .Add("Spica".ToUpper)
            .Add("Unukalhai".ToUpper)
            .Add("Vega".ToUpper)
            .Add("Vindemiatrix".ToUpper)
            .Add("Zaurak".ToUpper)
            .Add("Zeta Herculis".ToUpper)
            .Add("Zeta Persei".ToUpper)
            .Add("Zuben el Genubi".ToUpper)
        End With

        RefStars.Clear()

        Dim InData As Boolean = False
        For Each Entry As String In System.IO.File.ReadAllLines(System.IO.Path.Combine(MyPath, "Star Names.txt"))
            If Entry.StartsWith("---") Then
                InData = True
            Else
                If InData = True Then
                    Dim Line As String() = Split(Entry, "|")
                    If Line.Length >= 4 Then
                        Dim Name As String = Line(3).ToUpper
                        If Name.Length > 0 Then
                            If AllAlignStars.Contains(Name) Then
                                RefStars.Add(Name, New sRefStarPos(AstroParser.ParseRA(Line(1)), AstroParser.ParsePosition(Line(2))))
                            End If
                        End If
                    End If
                End If
            End If
        Next Entry

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadRefStarDatabase()
        Catch ex As Exception
            'No nothing ...
        End Try
    End Sub

End Class
