Option Explicit On
Option Strict On

Public Class MainForm

    Dim DB As New cDB
    Dim Logger As New cLogging

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        CType(sender, System.Windows.Forms.Button).Enabled = False : DE

        'Define input file and open
        Dim FileIO As New System.IO.FileStream(DB.InputFile, IO.FileMode.Open, IO.FileAccess.Read)
        Dim BinaryIN As New System.IO.BinaryReader(FileIO)
        Dim InputPath As String = System.IO.Path.GetDirectoryName(DB.InputFile)

        'Read header elements
        Dim Header As New cSERFormat.cSERHeader(BinaryIN)
        tbLog.Text = Join(Header.PrintInfo.ToArray, System.Environment.NewLine)

        'Init SER writer
        Dim SEROut As New cSERFormat.cSerFormatWriter
        With SEROut
            .Header.FrameWidth = DB.CutOutWidth
            .Header.FrameHeight = DB.CutOutHeight
            .Header.PixelDepthPerPlane = Header.PixelDepthPerPlane
            .Header.FrameCount = 100
            .InitForWrite(DB.OutputFile)
        End With

        'Go on if stream position is ok
        If BinaryIN.BaseStream.Position = 178 Then

            'Check if file is currupt
            Dim TailLength As Long = FileIO.Length - 178 - Header.TotalImageBytes

            If TailLength > 0 Then

                'We assume RGGB and 16 bit for now ...
                If Header.BytePerPixel = 2 Then

                    'Init sum image
                    Dim SumImage(Header.FrameWidth - 1, Header.FrameHeight - 1) As Integer
                    For IdxY As Integer = 0 To Header.FrameWidth - 1
                        For IdxX As Integer = 0 To Header.FrameHeight - 1
                            SumImage(IdxY, IdxX) = Integer.MinValue
                        Next IdxX
                    Next IdxY

                    'Process all frames
                    tspbMain.Maximum = Header.FrameCount

                    For FrameCountIdx As Integer = 1 To Header.FrameCount

                        '1.) Read in 1 frame and convert to 2-byte data type
                        Dim FrameSize As Integer = CInt(Header.FrameWidth * Header.FrameHeight * Header.BytePerPixel)
                        Dim FrameVector As Byte() = BinaryIN.ReadBytes(FrameSize)

                        'The SER file has a different row-column order compared to the FITS file, so we have to swap ...
                        Dim Frame(Header.FrameHeight - 1, Header.FrameWidth - 1) As UInt16
                        Dim ReadPtr As Integer = 0
                        Dim PtrX As Integer = 0
                        Dim PtrY As Integer = 0
                        For Idx As Long = 1 To Frame.LongLength
                            Frame(PtrY, PtrX) = BitConverter.ToUInt16(New Byte() {FrameVector(ReadPtr + 1), FrameVector(ReadPtr)}, 0)
                            PtrX += 1
                            If PtrX > Frame.GetUpperBound(1) Then
                                PtrX = 0
                                PtrY += 1
                            End If
                            ReadPtr += 2
                        Next Idx

                        Logger.Tic("Convert to Int16")
                        System.Buffer.BlockCopy(FrameVector, 0, Frame, 0, FrameVector.Count)                            'Direct copy 1-to-1 byte in memory to generate 16-bit from 2 8-bit
                        Logger.Toc()

                        '2.) For the first image, detect mean X and Y summing statistics
                        If FrameCountIdx >= -1 Then
                            Dim CenterX As Integer = DB.CenterX
                            Dim CenterY As Integer = DB.CenterY
                            If DB.Tracking Then
                                Logger.Tic("Statistics")
                                Dim WidthStat(Header.FrameWidth - 1) As Long
                                Dim HeightStat(Header.FrameHeight - 1) As Long
                                Parallel.For(0, Header.FrameWidth - 1, Sub(IdxY)
                                                                           For IdxX As Integer = 0 To Header.FrameHeight - 1
                                                                               WidthStat(IdxY) += Frame(IdxY, IdxX)
                                                                               HeightStat(IdxX) += Frame(IdxY, IdxX)
                                                                           Next IdxX
                                                                       End Sub
                        )
                                'Get pixel peak of the image
                                ForCommonCode.SlidingAVG(WidthStat, DB.SlideWidth, CenterX) : CenterX = ((CenterX \ 2) * 2) - (DB.SlideWidth \ 2)
                                ForCommonCode.SlidingAVG(HeightStat, DB.SlideWidth, CenterY) : CenterY = ((CenterY \ 2) * 2) - (DB.SlideWidth \ 2)
                                Logger.Toc()
                            End If

                            'Cut out center and store
                            Dim CenterImage(DB.CutOutWidth - 1, DB.CutOutHeight - 1) As UInt16
                            Dim Ptr1 As Integer = 0
                            Dim Ptr2 As Integer = 0
                            For Idx_X As Integer = CenterX - (DB.CutOutWidth \ 2) To CenterX + (DB.CutOutWidth \ 2) - 1
                                For Idx_Y As Integer = CenterY - (DB.CutOutHeight \ 2) To CenterY + (DB.CutOutHeight \ 2) - 1
                                    CenterImage(Ptr1, Ptr2) = Frame(Idx_X, Idx_Y)
                                    Ptr2 += 1
                                Next Idx_Y
                                Ptr1 += 1 : Ptr2 = 0
                            Next Idx_X
                            SEROut.AppendFrame(CenterImage)
                            If String.IsNullOrEmpty(DB.FITSOutFileName) = False Then
                                Dim FITSFileName As String = System.IO.Path.Combine(InputPath, DB.FITSOutFileName.Replace("#####", Format(FrameCountIdx, "00000").Trim))
                                cFITSWriter.Write(FITSFileName, CenterImage, cFITSWriter.eBitPix.Int16)
                            End If
                        End If

                        '3.) Get global MAX image
                        If DB.CalcFITSSumFile Then
                            Logger.Tic("Global sum")
                            Dim DoParallel As Boolean = True
                            If DoParallel Then
                                Parallel.For(0, Header.FrameWidth - 1, Sub(IdxY)
                                                                           For IdxX As Integer = 0 To Header.FrameHeight - 1
                                                                               If Frame(IdxY, IdxX) > SumImage(IdxY, IdxX) Then
                                                                                   SumImage(IdxY, IdxX) = Frame(IdxY, IdxX)
                                                                               End If
                                                                           Next IdxX
                                                                       End Sub
                            )
                            Else
                                For IdxY As Integer = 0 To Header.FrameWidth - 1

                                Next IdxY
                            End If
                            Logger.Toc()
                        End If

                        'Debug - save 1st frame
                        If FrameCountIdx = 1 Then
                            Dim FITSFirstName As String = System.IO.Path.Combine(InputPath, "DEBUG_FITS_0001.fits")
                            cFITSWriter.Write(FITSFirstName, Frame, cFITSWriter.eBitPix.Int16)
                            Exit For
                        End If

                        tspbMain.Value = FrameCountIdx
                        tsslMain.Text = "Frame " & Format(FrameCountIdx, "0000").Trim & "/" & Format(tspbMain.Maximum, "0000").Trim
                        System.Windows.Forms.Application.DoEvents()

                        If FrameCountIdx = 200 Then Exit For

                    Next FrameCountIdx

                    'Finish SER file
                    SEROut.CloseSerFile()

                    'Store total peak image
                    If DB.CalcFITSSumFile Then
                        Dim FITSSumFileName As String = System.IO.Path.Combine(InputPath, DB.FITSSumFile)
                        cFITSWriter.Write(FITSSumFileName, SumImage, cFITSWriter.eBitPix.Int16)
                    End If

                End If

            Else

                'TODO: Calculate "really" stored number of frames and read these frames

            End If

        End If

        BinaryIN.Close()
        FileIO.Close()

        CType(sender, System.Windows.Forms.Button).Enabled = True : DE()

    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        pgMain.SelectedObject = DB
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub tsmiReadHeader_Click(sender As Object, e As EventArgs) Handles tsmiReadHeader.Click
        tbLog.Text = Join((New cSERFormat.cSERHeader(New System.IO.BinaryReader(New System.IO.FileStream(DB.InputFile, IO.FileMode.Open, IO.FileAccess.Read)))).PrintInfo.ToArray, System.Environment.NewLine)
    End Sub

    Private Sub OpenInputPathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenInputPathToolStripMenuItem.Click
        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(DB.InputFile)) Then Process.Start(System.IO.Path.GetDirectoryName(DB.InputFile))
    End Sub

    Private Sub ShowLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowLogToolStripMenuItem.Click
        Logger.ShowLog()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub


End Class
