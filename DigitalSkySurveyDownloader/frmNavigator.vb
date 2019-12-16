Option Explicit On
Option Strict On

Public Class frmNavigator

    Dim StorageRoot As String = "\\DS001\astro\catalogs\DSS\DSS1\"
    Dim RA_Init As Double = 0
    Dim DEC_Init As Double = 0

    Private Sub frmNavigator_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim ImageData(,) As Double = {}
        Dim Reader As New cFITSReader()

        Dim FileName As String = StorageRoot & "..."
        Reader.ReadIn(FileName, ImageData)

    End Sub

End Class