Option Explicit On
Option Strict On

Public Class MainForm

    Dim Logger As New cLogging

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'Processing properties
        Dim SlideWidth As Integer = 32
        Dim CutOut As Integer = 100

        'Define input file and open
        Dim SERFile As String = "C:\TEMP\ISS_001.ser"
        Dim FileIO As New System.IO.FileStream(SERFile, IO.FileMode.Open, IO.FileAccess.Read)
        Dim BinaryIN As New System.IO.BinaryReader(FileIO)

        'Read header elements
        Dim Header As String = System.Text.Encoding.ASCII.GetString(BinaryIN.ReadBytes(14))
        Dim LuID As Int32 = BitConverter.ToInt32(BinaryIN.ReadBytes(4), 0)
        Dim ColorID As Int32 = BitConverter.ToInt32(BinaryIN.ReadBytes(4), 0)
        Dim LittleEndian As Int32 = BitConverter.ToInt32(BinaryIN.ReadBytes(4), 0)
        Dim ImageWidth As Int32 = BitConverter.ToInt32(BinaryIN.ReadBytes(4), 0)
        Dim ImageHeight As Int32 = BitConverter.ToInt32(BinaryIN.ReadBytes(4), 0)
        Dim PixelDepthPerPlane As Int32 = BitConverter.ToInt32(BinaryIN.ReadBytes(4), 0)
        Dim FrameCount As Int32 = BitConverter.ToInt32(BinaryIN.ReadBytes(4), 0)
        Dim Observer As String = System.Text.Encoding.ASCII.GetString(BinaryIN.ReadBytes(40))
        Dim Instrument As String = System.Text.Encoding.ASCII.GetString(BinaryIN.ReadBytes(40))
        Dim Telescope As String = System.Text.Encoding.ASCII.GetString(BinaryIN.ReadBytes(40))
        Dim DateTimeLocalRaw As Int64 = BitConverter.ToInt64(BinaryIN.ReadBytes(8), 0)
        Dim DateTimeUTCRaw As Int64 = BitConverter.ToInt64(BinaryIN.ReadBytes(8), 0)

        'Derive time info
        Dim DateTimeSubSec As Int64 = DateTimeLocalRaw - ((DateTimeLocalRaw \ 10000000) * 10000000)
        Dim DateTimeLocal As DateTime = New DateTime(1, 1, 1, 0, 0, 0).AddSeconds(DateTimeLocalRaw \ 10000000)
        Dim DateTimeUTC As DateTime = New DateTime(1, 1, 1, 0, 0, 0).AddSeconds(DateTimeUTCRaw \ 10000000)

        'Init SER writer
        Dim SEROut As New cSerFormatWriter
        With SEROut
            .ImageWidth = CutOut
            .ImageHeight = CutOut
            .PixelDepthPerPlane = PixelDepthPerPlane
            .FrameCount = 100
            .InitForWrite("C:\TEMP\ISS_001_CUTOUT.ser")
        End With

        'Derive parameters
        Dim BytePerPixel As Integer = PixelDepthPerPlane \ 8

        'Go on if stream position is ok
        If BinaryIN.BaseStream.Position = 178 Then

            'Check if file is currupt
            Dim TotalImageBytes As Long = CLng(FrameCount) * CLng(ImageWidth) * CLng(ImageHeight) * CLng(BytePerPixel)
            Dim TailLength As Long = FileIO.Length - 178 - TotalImageBytes

            If TailLength > 0 Then

                'We assume RGGB and 16 bit for now ...
                If BytePerPixel = 2 Then

                    'Init sum image
                    Dim SumImage(ImageWidth - 1, ImageHeight - 1) As Integer
                    For IdxY As Integer = 0 To ImageWidth - 1
                        For IdxX As Integer = 0 To ImageHeight - 1
                            SumImage(IdxY, IdxX) = Integer.MinValue
                        Next IdxX
                    Next IdxY

                    'Process all frames
                    tspbMain.Maximum = FrameCount

                    For FrameCountIdx As Integer = 1 To FrameCount

                        '1.) Read in frame data and convert to 2-byte data type
                        Dim FrameSize As Integer = CInt(ImageWidth * ImageHeight * BytePerPixel)
                        Dim FileContent As Byte() = BinaryIN.ReadBytes(FrameSize)
                        Dim Frame(ImageWidth - 1, ImageHeight - 1) As Int16
                        Dim HeightStat(ImageHeight - 1) As Long
                        Logger.Tic("Convert to Int16")
                        System.Buffer.BlockCopy(FileContent, 0, Frame, 0, FileContent.Count)                            'Direct copy 1-to-1 byte in memory to generate 16-bit from 2 8-bit
                        Logger.Toc()

                        '2.) For the first image, detect mean X and Y summing statistics
                        If FrameCountIdx >= -1 Then
                            Logger.Tic("Statistics")
                            Dim WidthStat(ImageWidth - 1) As Long
                            Parallel.For(0, ImageWidth - 1, Sub(IdxY)
                                                                For IdxX As Integer = 0 To ImageHeight - 1
                                                                    WidthStat(IdxY) += Frame(IdxY, IdxX)
                                                                    HeightStat(IdxX) += Frame(IdxY, IdxX)
                                                                Next IdxX
                                                            End Sub
                            )
                            'Get pixel peak of the image
                            Dim CenterX As Integer : ForCommonCode.SlidingAVG(WidthStat, SlideWidth, CenterX) : CenterX = ((CenterX \ 2) * 2) - (SlideWidth \ 2)
                            Dim CenterY As Integer : ForCommonCode.SlidingAVG(HeightStat, SlideWidth, CenterY) : CenterY = ((CenterY \ 2) * 2) - (SlideWidth \ 2)
                            Logger.Toc()

                            'Cut out center and store
                            Dim CenterImage(CutOut - 1, CutOut - 1) As Short
                            Dim Ptr1 As Integer = 0
                            Dim Ptr2 As Integer = 0
                            For Idx1 As Integer = CenterX - (CutOut \ 2) To CenterX + (CutOut \ 2) - 1
                                For Idx2 As Integer = CenterY - (CutOut \ 2) To CenterY + (CutOut \ 2) - 1
                                    CenterImage(Ptr1, Ptr2) = Frame(Idx1, Idx2)
                                    Ptr2 += 1
                                Next Idx2
                                Ptr1 += 1 : Ptr2 = 0
                            Next Idx1
                            SEROut.AppendFrame(CenterImage)
                            cFITSWriter.Write("C:\TEMP\ISS\ISS_SingleFrame_" & Format(FrameCountIdx, "00000").Trim & ".fits", CenterImage, cFITSWriter.eBitPix.Int16)
                        End If

                        '3.) Get global MAX image
                        Logger.Tic("Global sum")
                        Dim DoParallel As Boolean = True
                        If DoParallel Then
                            Parallel.For(0, ImageWidth - 1, Sub(IdxY)
                                                                For IdxX As Integer = 0 To ImageHeight - 1
                                                                    If Frame(IdxY, IdxX) > SumImage(IdxY, IdxX) Then
                                                                        SumImage(IdxY, IdxX) = Frame(IdxY, IdxX)
                                                                    End If
                                                                Next IdxX
                                                            End Sub
                            )
                        Else
                            For IdxY As Integer = 0 To ImageWidth - 1

                            Next IdxY
                        End If
                        Logger.Toc()

                        tspbMain.Value = FrameCountIdx
                        tsslMain.Text = "Frame " & Format(FrameCountIdx, "0000").Trim & "/" & Format(tspbMain.Maximum, "0000").Trim
                        System.Windows.Forms.Application.DoEvents()

                        If FrameCountIdx = 200 Then Exit For

                    Next FrameCountIdx

                    Logger.ShowLog()

                    'Finish SER file
                    SEROut.CloseSerFile()

                    'Store total peak image
                    cFITSWriter.Write("C:\TEMP\ISS\SUM.fits", SumImage, cFITSWriter.eBitPix.Int16)

                End If

            Else

                'TODO: Calculate "really" stored number of frames and read these frames

            End If

        End If

        BinaryIN.Close()
        FileIO.Close()

        MsgBox("OK!")

    End Sub

End Class