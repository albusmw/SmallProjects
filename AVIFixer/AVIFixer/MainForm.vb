Option Explicit On
Option Strict On

Public Class MainForm

    Dim DB As New cDB
    Dim StopFlag As Boolean = False

    Const GByte As Long = 1024 * 1024 * 1024

    '======================================================================================

    ReadOnly Property WorkFolder As String
        Get
            If IsNothing(tbWorkFolder) Then Return String.Empty Else Return tbWorkFolder.Text
        End Get
    End Property

    ReadOnly Property VideoFile As String
        Get
            Return IO.Path.Combine(WorkFolder, tbInputAVIFile.Text)
        End Get
    End Property

    ReadOnly Property AVIWidth As Integer
        Get
            Try
                Return CInt(tbWidth.Text)
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property

    ReadOnly Property AVIHeight As Integer
        Get
            Try
                Return CInt(tbHeight.Text)
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property

    '======================================================================================

    Dim ImageToCreate As String = WorkFolder & "\Recover\Image"
    Dim VideoFileName As String = WorkFolder & "\Recover\Recover.avi"
    Dim FFMPEG_AllFiles As String = WorkFolder & "\Recover\ffmpeglist.txt"

    Dim GByteChuckStep As Integer = 488                     'guess value

    '''<summary>Update the content of the focus window.</summary>
    '''<param name="Data">Data to display.</param>
    Public Function GetBitmapData(ByRef Data(,) As Byte) As Bitmap
        Dim OutputImage As New cLockBitmap(Data.GetUpperBound(0), Data.GetUpperBound(1))
        OutputImage.LockBits()
        Dim Stride As Integer = OutputImage.BitmapData.Stride
        Dim BytePerPixel As Integer = OutputImage.ColorBytesPerPixel
        Dim YOffset As Integer = 0
        For Y As Integer = 0 To OutputImage.Height - 1
            Dim BaseOffset As Integer = YOffset
            For X As Integer = 0 To OutputImage.Width - 1
                OutputImage.Pixels(BaseOffset) = Data(X, Y)
                OutputImage.Pixels(BaseOffset + 1) = Data(X, Y)
                OutputImage.Pixels(BaseOffset + 2) = Data(X, Y)
                BaseOffset += BytePerPixel
            Next X
            YOffset += Stride
        Next Y
        OutputImage.UnlockBits()
        Return OutputImage.BitmapToProcess
    End Function

    Private Sub De()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub tsmiFile_Start_Click(sender As Object, e As EventArgs) Handles tsmiFile_Start.Click

        Dim FileName As String = String.Empty
        Dim LastChunckDelta As Integer = 0

        Using InStream As New System.IO.FileStream(VideoFile, IO.FileMode.Open)

            Dim FileLen As Long = InStream.Length
            Dim InitialOffset As Long = 66056
            Dim ChunkCorrection As Long = 0
            Dim FirstFrame As Long = 0
            Dim ChunkSize As Long = 6433792                 'taken from RIFF information of a valid capture

            Dim FFMPEGList As New List(Of String)

            InitialOffset += FirstFrame * ChunkSize

            Using BinIn As New System.IO.BinaryReader(InStream)

                For ImageIdx As Integer = 0 To 3000

                    With tspbMain
                        .Minimum = 0
                        .Maximum = 3000
                        .Value = ImageIdx
                        De()
                    End With

                    'Calculate chunck offset
                    Dim TotalOffset As Long = InitialOffset + (ImageIdx * ChunkSize)

                    'Currect by GByte
                    Dim GByteOffset As Integer = CInt(TotalOffset \ GByte)
                    TotalOffset += GByteOffset * GByteChuckStep

                    'Correct by manual file
                    If DB.ChunkMoves.ContainsKey(ImageIdx) Then
                        ChunkCorrection += (DB.ChunkMoves(ImageIdx) - LastChunckDelta)
                        LastChunckDelta = DB.ChunkMoves(ImageIdx)
                    End If
                    TotalOffset += ChunkCorrection

                    InStream.Seek(TotalOffset, IO.SeekOrigin.Begin)

                    FileName = ImageToCreate & "_" & Format(ImageIdx, "0000") & ".png"
                    Dim CalculateFile As Boolean = True
                    If System.IO.File.Exists(FileName) Then
                        If DB.OverwriteFiles Then
                            System.IO.File.Delete(FileName)
                        Else
                            CalculateFile = False
                        End If
                    End If

                    If CalculateFile = True Then
                        Dim LockBitmap(CInt(AVIWidth), CInt(AVIHeight)) As Byte
                        Dim OutImagePtr_X As Integer = 0
                        Dim OutImagePtr_Y As Integer = 0

                        Dim ImageBytes As Byte() = BinIn.ReadBytes(AVIHeight * AVIWidth)
                        Dim ImageBytesPtr As Integer = 0

                        For X As Long = 0 To AVIHeight - 1
                            For Y As Long = 0 To AVIWidth - 1
                                Dim Data As Byte = ImageBytes(ImageBytesPtr) : ImageBytesPtr += 1
                                LockBitmap(OutImagePtr_X, OutImagePtr_Y) = Data
                                OutImagePtr_X += 1
                                If OutImagePtr_X = AVIWidth Then
                                    OutImagePtr_X = 0
                                    OutImagePtr_Y += 1
                                End If
                            Next Y
                        Next X
                        GetBitmapData(LockBitmap).Save(FileName, Imaging.ImageFormat.Png)
                    End If

                    FFMPEGList.Add("file '" & FileName & "'")

                    If StopFlag = True Then Exit For

                Next ImageIdx

                System.IO.File.WriteAllLines(FFMPEG_AllFiles, FFMPEGList.ToArray)

            End Using

        End Using

        tspbMain.Value = 0

    End Sub

    '''<summary>Get 1 single frame starting at the initial offset.</summary>
    '''<param name="InitialOffset"></param>
    '''<returns>Decoded image.</returns>
    Private Function GetSingleFrame(ByVal InitialOffset As Integer) As Bitmap

        Try

            Dim LastChunckDelta As Integer = 0

            Using InStream As New System.IO.FileStream(VideoFile, IO.FileMode.Open)

                Dim FileLen As Long = InStream.Length

                Using BinIn As New System.IO.BinaryReader(InStream)

                    InStream.Seek(InitialOffset, IO.SeekOrigin.Begin)

                    Dim LockBitmap(CInt(AVIWidth), CInt(AVIHeight)) As Byte
                    Dim OutImagePtr_X As Integer = 0
                    Dim OutImagePtr_Y As Integer = 0

                    Dim ImageBytes As Byte() = BinIn.ReadBytes(AVIHeight * AVIWidth)
                    Dim ImageBytesPtr As Integer = 0

                    For X As Long = 0 To AVIHeight - 1
                        For Y As Long = 0 To AVIWidth - 1
                            Dim Data As Byte = ImageBytes(ImageBytesPtr) : ImageBytesPtr += 1
                            LockBitmap(OutImagePtr_X, OutImagePtr_Y) = Data
                            OutImagePtr_X += 1
                            If OutImagePtr_X > LockBitmap.GetUpperBound(0) Then
                                OutImagePtr_X = 0
                                OutImagePtr_Y += 1
                            End If
                        Next Y
                    Next X
                    Return GetBitmapData(LockBitmap)

                End Using

            End Using

        Catch ex As Exception

            tsslErrorMessage.Text = ex.Message
            Return Nothing

        End Try

    End Function

    Private Sub tsmiFile_Stop_Click(sender As Object, e As EventArgs) Handles tsmiFile_Stop.Click
        StopFlag = True
    End Sub

    Private Sub tsmiFile_LoadChunks_Click(sender As Object, e As EventArgs) Handles tsmiFile_LoadChunks.Click
        Dim Split() As Char = {" "c, Chr(9)}
        DB.ChunkMoves = New Dictionary(Of Integer, Integer)
        For Each Line As String In System.IO.File.ReadAllLines("\\192.168.100.10\astro_misc\!Support und Probleme\Mehmet (AVI Sonne)\Chunck_Moves.txt")
            Dim Values As String() = Line.Split(Split)
            DB.ChunkMoves.Add(CInt(Values(0)), CInt(Values(Values.Length - 1)))
        Next Line
        MsgBox(DB.ChunkMoves.Count.ToString.Trim & " chunks added")
    End Sub

    Private Sub tsmiFile_RunFFMPEG_Click(sender As Object, e As EventArgs) Handles tsmiFile_RunFFMPEG.Click
        Dim FFMPEG As New ProcessStartInfo
        With FFMPEG
            .FileName = DB.FFMPEGExe
            .WorkingDirectory = System.IO.Path.GetDirectoryName(.FileName)
            .Arguments = "-y -r 25 -f concat -safe 0 -i """ & FFMPEG_AllFiles & """ -c:v libx264 -vf ""fps=25,format=yuv420p"" -report " & VideoFileName
            .UseShellExecute = True
            .RedirectStandardError = False
        End With
        tbCommand.Text = FFMPEG.Arguments
        Dim Runner As Process = Process.Start(FFMPEG)
        Runner.WaitForExit()
        'MsgBox(Runner.StandardError.ReadToEnd)
    End Sub

    Private Sub tbWidth_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbWidth.MouseWheel, tbHeight.MouseWheel, tbInitialOffset.MouseWheel
        Dim StepSize As Integer = 1
        Try
            Dim OldVal As Integer = CInt(CType(sender, TextBox).Text)
            If e.Delta > 0 Then
                OldVal += StepSize
            Else
                OldVal -= StepSize : If OldVal < 1 Then OldVal = 1
            End If
            CType(sender, TextBox).Text = OldVal.ToString.Trim
            pbImage.Image = GetSingleFrame(CInt(tbInitialOffset.Text))
        Catch ex As Exception
            'Do nothing ....
        End Try
    End Sub

End Class
