Option Explicit On
Option Strict On

Public Class frmNavigator

    Public IPP As cIntelIPP
    Dim SingleStatCalc As New AstroNET.Statistics(IPP)

    Dim MosaikForm As New cImgForm
    Dim UseIPP As Boolean = True

    Public Sub ShowMosaik()

        'Read the same segment from all files and compose a new combined image
        Dim TileSize As Integer = 0             'size for 1 tile
        Dim OffsetX As Integer = 0              'X offset start position
        Dim OffsetY As Integer = 0              'Y offset start position

        'Plaubility check
        Try
            TileSize = CInt(tbTileSize.Text) : If TileSize < 1 Then Exit Sub
            OffsetX = CInt(tbOffsetX.Text) - (TileSize \ 2) : If OffsetX < TileSize \ 2 Then Exit Sub
            OffsetY = CInt(tbOffsetY.Text) - (TileSize \ 2) : If OffsetY < TileSize \ 2 Then Exit Sub
        Catch ex As Exception
            Exit Sub
        End Try
        If System.IO.File.Exists(tbRootFile.Text) = False Then
            tbRootFile.BackColor = Color.Red
            Exit Sub
        Else
            tbRootFile.BackColor = Color.White
        End If

        Dim BaseDirectory As String = System.IO.Path.GetDirectoryName(tbRootFile.Text)
        Dim FITSReader As New cFITSReader

        Dim AllFiles As New List(Of String)(System.IO.Directory.GetFiles(BaseDirectory, tbFilterString.Text))
        If AllFiles.Count = 0 Then Exit Sub

        Dim MosaikWidth As Integer = CInt(Math.Ceiling(Math.Sqrt(AllFiles.Count)))              'Number of tiles in X direction
        Dim MosaikHeight As Integer = CInt(Math.Ceiling(AllFiles.Count / MosaikWidth))          'Number of tiles in Y direction
        ReDim SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(MosaikWidth * TileSize + (MosaikWidth - 1) - 1, MosaikHeight * TileSize + (MosaikHeight - 1) - 1)

        'Compose the mosaik
        Dim WidthPtr As Integer = 0 : Dim WidthIdx As Integer = 0
        Dim HeightPtr As Integer = 0
        pbMain.Maximum = AllFiles.Count
        For FileIdx As Integer = 0 To AllFiles.Count - 1
            Dim File As String = AllFiles(FileIdx)
            pbMain.Value = FileIdx : DE()
            Dim Data(,) As UInt16 = FITSReader.ReadInUInt16(File, UseIPP, OffsetX, TileSize, OffsetY, TileSize)
            For X As Integer = 0 To TileSize - 1
                For Y As Integer = 0 To TileSize - 1
                    SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(WidthPtr + X, HeightPtr + Y) = Data(X, Y)
                Next Y
            Next X
            WidthPtr += TileSize + 1 : WidthIdx += 1
            If WidthIdx >= MosaikWidth Then
                HeightPtr += TileSize + 1
                WidthPtr = 0
                WidthIdx = 0
            End If
        Next FileIdx

        'Run mosaik statistics
        Dim Stat As AstroNET.Statistics.sStatistics = SingleStatCalc.ImageStatistics(AstroNET.Statistics.sStatistics.eDataMode.Fixed)

        ShowDataForm(MosaikForm, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, Stat.MonoStatistics_Int.Min.Key, Stat.MonoStatistics_Int.Max.Key)
        pbMain.Value = 0

        'Dim FileToGenerate As String = System.IO.Path.Combine(MyPath, "Mosaik.fits")
        'cFITSWriter.Write(FileToGenerate, Mosaik, cFITSWriter.eBitPix.Int16)
        'Process.Start(FileToGenerate)

    End Sub

    Private Sub ShowDataForm(ByRef FormToShow As cImgForm, ByRef Data(,) As UInt16, ByVal Min As Long, ByVal Max As Long)

        Dim NewWindowRequired As Boolean = False
        If IsNothing(FormToShow) = True Then
            NewWindowRequired = True
        Else
            If FormToShow.Hoster.IsDisposed = True Then NewWindowRequired = True
        End If
        If NewWindowRequired = True Then
            FormToShow = New cImgForm
        End If
        FormToShow.Show("MosaikForm <" & tbRootFile.Text & ">")
        FormToShow.ShowData(Data, Min, Max)

    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub frmNavigator_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.Up
                tbOffsetY.Text = CStr(CInt(tbOffsetY.Text) + (CInt(tbTileSize.Text) \ 2)).Trim
            Case Keys.Down
                tbOffsetY.Text = CStr(CInt(tbOffsetY.Text) - (CInt(tbTileSize.Text) \ 2)).Trim
            Case Keys.Right
                tbOffsetX.Text = CStr(CInt(tbOffsetX.Text) + (CInt(tbTileSize.Text) \ 2)).Trim
            Case Keys.Left
                tbOffsetX.Text = CStr(CInt(tbOffsetX.Text) - (CInt(tbTileSize.Text) \ 2)).Trim
        End Select
    End Sub

    Private Sub tbOffsetX_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbOffsetX.MouseWheel
        Select Case e.Delta
            Case Is > 0
                tbOffsetX.Text = CStr(CInt(tbOffsetX.Text) + (CInt(tbTileSize.Text) \ 10)).Trim
            Case Is < 0
                tbOffsetX.Text = CStr(CInt(tbOffsetX.Text) - (CInt(tbTileSize.Text) \ 10)).Trim
        End Select
    End Sub

    Private Sub tbOffsetY_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbOffsetY.MouseWheel
        Select Case e.Delta
            Case Is > 0
                tbOffsetY.Text = CStr(CInt(tbOffsetY.Text) + (CInt(tbTileSize.Text) \ 10)).Trim
            Case Is < 0
                tbOffsetY.Text = CStr(CInt(tbOffsetY.Text) - (CInt(tbTileSize.Text) \ 10)).Trim
        End Select
    End Sub

    Private Sub tbTileSize_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbTileSize.MouseWheel
        Select Case e.Delta
            Case Is > 0
                tbTileSize.Text = CStr(CInt(tbTileSize.Text) - 10).Trim
            Case Is < 0
                tbTileSize.Text = CStr(CInt(tbTileSize.Text) + 10).Trim
        End Select
    End Sub

    Private Sub tbOffsetX_TextChanged(sender As Object, e As EventArgs) Handles tbOffsetX.TextChanged, tbOffsetY.TextChanged, tbTileSize.TextChanged
        ShowMosaik()
    End Sub

    Private Sub lbPixel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbPixel.SelectedIndexChanged
        Dim Splitted As String() = Split(CStr(lbPixel.SelectedItem), ":")
        tbOffsetX.Text = Splitted(0)
        tbOffsetY.Text = Splitted(1)
    End Sub

End Class