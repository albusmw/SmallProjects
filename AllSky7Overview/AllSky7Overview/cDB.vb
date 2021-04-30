Option Explicit On
Option Strict On

Public Class cDB

    Public Cams As List(Of sCam)

    Private Const DefaultPWD As String = "meteors"
    Public Const InitialPage As String = "stacks"

    Public Structure sCam
        Public Property URL As String
        Public Property UserName As String
        Public Property Password As String
        Public Property Cameras As Integer()
        Public Sub New(ByVal NewURL As String, ByVal NewUserName As String, ByVal NewPassword As String)
            URL = NewURL
            UserName = NewUserName
            Password = NewPassword
        End Sub
    End Structure

    Public Sub New()
        Cams = New List(Of sCam)
        Cams.Add(New sCam("https://ketzuer.allsky7.net/", "AMS16", DefaultPWD))
        'Cams.Add(New sCam("https://budapest.allsky7.net/", "AMS18", DefaultPWD))
        Cams.Add(New sCam("https://herford.allsky7.net/", "AMS21", DefaultPWD))
        Cams.Add(New sCam("https://lindenberg.allsky7.net/", "AMS22", DefaultPWD))
        'Cams.Add(New sCam("https://bernitt.allsky7.net/", "AMS30", DefaultPWD))
        Cams.Add(New sCam("https://haidmuehle.allsky7.net/", "AMS31", DefaultPWD))
        Cams.Add(New sCam("https://noordwijkerhout.allsky7.net/", "AMS32", DefaultPWD))
        Cams.Add(New sCam("https://seysdorf.allsky7.net/", "AMS33", DefaultPWD))
        Cams.Add(New sCam("https://gettorf.allsky7.net/", "AMS34", DefaultPWD))
        Cams.Add(New sCam("https://conow.allsky7.net/", "AMS35", DefaultPWD))
        Cams.Add(New sCam("https://kirchheim.allsky7.net/", "AMS36", DefaultPWD))
        Cams.Add(New sCam("https://dwingeloo.allsky7.net/", "AMS37", DefaultPWD))
        Cams.Add(New sCam("https://lagrand.allsky7.net/", "AMS41", DefaultPWD))
        Cams.Add(New sCam("https://elyiowa.allsky7.net/", "AMS42", DefaultPWD))
        Cams.Add(New sCam("https://karlsruhe.allsky7.net/", "AMS50", DefaultPWD))
        Cams.Add(New sCam("https://salzburg.allsky7.net/", "AMS51", "geminiden"))
        Cams.Add(New sCam("https://sonneberg.allsky7.net/", "AMS52", DefaultPWD))
        Cams.Add(New sCam("https://hofheim.allsky7.net/", "AMS53", DefaultPWD))
        Cams.Add(New sCam("https://benediktbeuern.allsky7.net/", "AMS54", DefaultPWD))
        Cams.Add(New sCam("https://drebach.allsky7.net/", "AMS55", DefaultPWD))
        Cams.Add(New sCam("https://bischbrunn.allsky7.net/", "AMS56", DefaultPWD))
        Cams.Add(New sCam("https://weilderstadt.allsky7.net/", "AMS57", DefaultPWD))
        Cams.Add(New sCam("https://holzkirchen.allsky7.net/", "AMS58", DefaultPWD))
        Cams.Add(New sCam("https://rhodes.allsky7.net/", "AMS59", DefaultPWD))
        Cams.Add(New sCam("https://goettingen.allsky7.net/", "AMS60", DefaultPWD))
        Cams.Add(New sCam("https://amrum.allsky7.net/", "AMS61", DefaultPWD))
        Cams.Add(New sCam("https://wangen.allsky7.net/", "AMS64", DefaultPWD))
        Cams.Add(New sCam("https://radebeul.allsky7.net/", "AMS65", DefaultPWD))
        Cams.Add(New sCam("https://oostkapelle.allsky7.net/", "AMS66", DefaultPWD))
        Cams.Add(New sCam("https://antwerp.allsky7.net/", "AMS67", DefaultPWD))
    End Sub

    Public Function GetAMSNumber(ByVal URL As String) As sCam
        For Each Entry As sCam In Cams
            If Entry.URL = URL Then Return Entry
        Next Entry
        Return Nothing
    End Function

End Class
