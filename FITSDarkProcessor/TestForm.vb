Public Class TestForm

  Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles Me.Load
    With pbMain
      .Load("C:\Users\weis_m\Desktop\Stars.png")
      .BackColor = Color.Green
    End With
  End Sub

End Class