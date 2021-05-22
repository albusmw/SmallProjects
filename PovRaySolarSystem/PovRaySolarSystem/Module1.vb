Option Explicit On
Option Strict On

Module Module1

    Dim Loader As New cHorizons_Loader
    Dim NL As String = System.Environment.NewLine

    Sub Main()

        Dim BodyFiles As String = "C:\GIT\SmallProjects\PovRaySolarSystem\PovRayInput"



        Dim MoonData As List(Of String) = Loader.LoadPlanetaryCoordinated(Loader.GetBody(cHorizons_Loader.eBodies.Moon), 1)
        Dim EarthData As List(Of String) = Loader.LoadPlanetaryCoordinated(Loader.GetBody(cHorizons_Loader.eBodies.Earth_GeoCenter), 1)

        System.IO.File.WriteAllText(System.IO.Path.Combine(BodyFiles, "Moon.inc"), GetBodyFile("Moon", MoonData))
        System.IO.File.WriteAllText(System.IO.Path.Combine(BodyFiles, "Earth.inc"), GetBodyFile("Earth", EarthData))

    End Sub

    Private Function GetBodyFile(ByVal VariableName As String, ByRef Data As List(Of String)) As String
        Dim Vec_X As New List(Of String)
        Dim Vec_Y As New List(Of String)
        Dim Vec_Z As New List(Of String)

        Vec_X.Add("#declare " & VariableName & "_X =array[" & Data.Count.ToString.Trim & "]{")
        Vec_Y.Add("#declare " & VariableName & "_Y =array[" & Data.Count.ToString.Trim & "]{")
        Vec_Z.Add("#declare " & VariableName & "_Z =array[" & Data.Count.ToString.Trim & "]{")

        For Idx As Integer = 0 To Data.Count - 1
            Dim Content As String() = Split(Data(Idx), Loader.OutputSplitter)
            Dim Com As String = CStr(IIf(Idx = Data.Count - 1, String.Empty, ","))
            Vec_X.Add(GetWithNoE(Content(1)) & Com)
            Vec_Y.Add(GetWithNoE(Content(2)) & Com)
            Vec_Z.Add(GetWithNoE(Content(3)) & Com)
        Next Idx

        Vec_X.Add("}")
        Vec_Y.Add("}")
        Vec_Z.Add("}")

        Return Join(Vec_X.ToArray, String.Empty) & NL & Join(Vec_Y.ToArray, String.Empty) & NL & Join(Vec_Z.ToArray, String.Empty)


    End Function

    Private Function GetWithNoE(ByVal Text As String) As String
        Return Val(Text.Replace(",", ".")).ToString.Trim.Replace(",", ".")
    End Function

End Module
