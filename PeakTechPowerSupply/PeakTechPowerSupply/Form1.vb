Option Explicit On
Option Strict On

Public Class Form1

    Dim COM_IO As New cPeakTech_COM

    Private Sub Start()
        COM_IO.Init(tbCOMPort.Text)
        COM_IO.SetU(12)
        COM_IO.SetI(10)
        COM_IO.SetState(True)
        tUpdate.Enabled = True
    End Sub

    Private Sub tUpdate_Tick(sender As Object, e As EventArgs) Handles tUpdate.Tick
        tbRxTx.BackColor = Color.Red : DE()
        COM_IO.Update()
        pgMain.SelectedObject = COM_IO
        tbRxTx.BackColor = Color.Silver : DE()
    End Sub

    Private Sub btnOff_Click(sender As Object, e As EventArgs) Handles btnOff.Click
        COM_IO.SetState(False)
    End Sub

    Private Sub btnOn_Click(sender As Object, e As EventArgs) Handles btnOn.Click
        COM_IO.SetState(True)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pgMain.SelectedObject = COM_IO
    End Sub

    Private Sub tbnStart_Click(sender As Object, e As EventArgs) Handles tbnStart.Click
        Start()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class
