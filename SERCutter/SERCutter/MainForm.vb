Option Explicit On
Option Strict On

Public Class MainForm

    '''<summary>Handle to Intel IPP functions.</summary>
    Private IntelIPP As cIntelIPP

    '''<summary>Statistics processor (for the last file).</summary>
    Private SingleStatCalc As AstroNET.Statistics

    '''<summary>Statistics of the last frame.</summary>
    Private LastStat As AstroNET.Statistics.sStatistics

    '''<summary>Database holding parameters.</summary>
    Dim DB As New cDB

    Dim Logger As New cLogging

    Dim FrameData As List(Of cFrameData)

    '''<summary>Drag-and-drop handler.</summary>
    Private WithEvents DD As Ato.DragDrop

    '''<summary>Data for 1 single SER frame.</summary>
    Private Class cFrameData
        Public Property FrameIdx As Integer = 0
        Public Property TimeStamp As DateTime = Nothing
        Public Property Center_X As Double = Double.NaN
        Public Property Center_Y As Double = Double.NaN
        Public Sub New(ByVal NewFrameIdx As Integer, ByVal NewTimeStamp As DateTime, ByVal NewCenter_X As Double, ByVal NewCenter_Y As Double)
            FrameIdx = NewFrameIdx
            TimeStamp = NewTimeStamp
            Center_X = NewCenter_X
            Center_Y = NewCenter_Y
        End Sub
    End Class

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        pgMain.SelectedObject = DB
        'IPP laden
        'Load IPP
        Dim IPPLoadError As String = String.Empty
        DB.IPPPath = cIntelIPP.SearchDLLToUse(cIntelIPP.PossiblePaths(DB.MyPath).ToArray, IPPLoadError)
        If String.IsNullOrEmpty(IPPLoadError) = True Then
            IntelIPP = New cIntelIPP(DB.IPPPath)
            cFITSWriter.UseIPPForWriting = True
        Else
            cFITSWriter.UseIPPForWriting = False
        End If
        cFITSWriter.IPPPath = IntelIPP.IPPPath
        SingleStatCalc = New AstroNET.Statistics(IntelIPP)
        DD = New Ato.DragDrop(tbLog, False)
    End Sub

    Private Sub DD_DropOccured(Files() As String) Handles DD.DropOccured
        'Handle drag-and-drop for all dropped FIT(s) files
        For Each File As String In Files
            DB.InputFile = File
        Next File
        pgMain.SelectedObject = DB
    End Sub

    '''<summary>Take an IPP path if there is not yet one set.</summary>
    Private Sub TestIPPPath(ByRef CurrentPath As String, ByVal Path As String)
        If System.IO.Directory.Exists(Path) Then
            If String.IsNullOrEmpty(CurrentPath) = True Then CurrentPath = Path
        End If
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        CType(sender, System.Windows.Forms.Button).Enabled = False : DE()

        SingleStatCalc.ResetAllProcessors()

        'Define input file and open
        Using FileIO As New System.IO.FileStream(DB.InputFile, IO.FileMode.Open, IO.FileAccess.Read)
            Using BinaryIN As New System.IO.BinaryReader(FileIO)
                Dim InputPath As String = System.IO.Path.GetDirectoryName(DB.InputFile)
                Dim FileNameOnly As String = System.IO.Path.GetFileNameWithoutExtension(DB.InputFile)
                Dim OutputPath As String = System.IO.Path.GetDirectoryName(DB.OutputFile.Replace(DB.FileNamePlaceHolder, FileNameOnly))

                'Tracking results
                FrameData = New List(Of cFrameData)

                'Read header elements
                Dim Header As New cSERFormat.cSERHeader(BinaryIN)
                Dim TimeStamps As DateTime() = Header.ReadTrailer(BinaryIN)
                Dim Delta As Double = (TimeStamps(1) - TimeStamps(0)).TotalSeconds
                tbLog.Text = Join(Header.PrintInfo.ToArray, System.Environment.NewLine)

                'Init SER writer
                Dim SEROut As New cSERFormat.cSerFormatWriter
                With SEROut
                    .Header.FrameWidth = DB.CutOutWidth
                    .Header.FrameHeight = DB.CutOutHeight
                    .Header.PixelDepthPerPlane = Header.PixelDepthPerPlane
                    .Header.FrameCount = 100
                    .InitForWrite(DB.OutputFile.Replace(DB.FileNamePlaceHolder, FileNameOnly))
                End With

                'Go on if stream position is ok
                If BinaryIN.BaseStream.Position = cSERFormat.cSERHeader.SERHeaderLength Then

                    'Check if file is currupt
                    Dim TailLength As Long = FileIO.Length - cSERFormat.cSERHeader.SERHeaderLength - Header.TotalImageBytes

                    If TailLength > 0 Then

                        'We assume RGGB and 16 bit for now ...
                        If Header.BytePerPixel = 2 Then

                            'Init center sum image
                            Dim CenterSumImage(DB.CutOutWidth - 1, DB.CutOutHeight - 1) As UInt32
                            For IdxY As Integer = 0 To CenterSumImage.GetUpperBound(0)
                                For IdxX As Integer = 0 To CenterSumImage.GetUpperBound(1)
                                    CenterSumImage(IdxY, IdxX) = 0
                                Next IdxX
                            Next IdxY

                            'Process all frames
                            tspbMain.Maximum = Header.FrameCount
                            Dim InitCenter_X As Integer = 0
                            Dim InitCenter_Y As Integer = 0

                            For FrameCountIdx As Integer = 1 To Header.FrameCount

                                '1.) Read in 1 frame and convert to 2-byte data type
                                Dim FullFrameSize As Integer = CInt(Header.FrameWidth * Header.FrameHeight * Header.BytePerPixel)
                                ReDim SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(Header.FrameWidth - 1, Header.FrameHeight - 1)
                                IntelIPP.Transpose(BinaryIN.ReadBytes(FullFrameSize), SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data)

                                'Statistics
                                LastStat = SingleStatCalc.ImageStatistics(LastStat.DataMode)

                                Select Case DB.Tracking

                                    Case cDB.eTracker.Off
                                'Do nothing

                                    Case cDB.eTracker.FullBody
                                        'Calculate center-of-mass for complete image
                                        Dim UsedSample As Integer = 0       'total number of used samples
                                        Dim UsedSampleSum As Double = 0     'sum of all used samples
                                        Dim Mid_X As Double = 0
                                        Dim Mid_Y As Double = 0
                                        With SingleStatCalc.DataProcessor_UInt16.ImageData(0)
                                            If DB.FullBody_weighted = True Then
                                                'Weight each used sample with its ADU value
                                                For Idx_X As Integer = 0 To .Data.GetUpperBound(0)
                                                    For Idx_Y As Integer = 0 To .Data.GetUpperBound(1)
                                                        If .Data(Idx_X, Idx_Y) >= DB.FullBody_threshold Then
                                                            UsedSample += 1
                                                            UsedSampleSum += .Data(Idx_X, Idx_Y)
                                                            Mid_X += Idx_X * .Data(Idx_X, Idx_Y)
                                                            Mid_Y += Idx_Y * .Data(Idx_X, Idx_Y)
                                                        End If
                                                    Next Idx_Y
                                                Next Idx_X
                                            Else
                                                'Weight each used sample with 1
                                                For Idx_X As Integer = 0 To .Data.GetUpperBound(0)
                                                    For Idx_Y As Integer = 0 To .Data.GetUpperBound(1)
                                                        If .Data(Idx_X, Idx_Y) >= DB.FullBody_threshold Then
                                                            UsedSample += 1
                                                            UsedSampleSum += 1
                                                            Mid_X += Idx_X
                                                            Mid_Y += Idx_Y
                                                        End If
                                                    Next Idx_Y
                                                Next Idx_X
                                            End If
                                        End With
                                        Dim Mid_X_float As Double = Mid_X / UsedSampleSum
                                        Dim Mid_Y_float As Double = Mid_Y / UsedSampleSum
                                        DB.MyTrackCenterX = CInt(Mid_X_float)
                                        DB.MyTrackCenterY = CInt(Mid_Y_float)

                                    Case cDB.eTracker.SmallObject
                                        Dim Ptr As Long = Logger.Tic("Statistics")
                                        Dim WidthStat(Header.FrameWidth - 1) As Long
                                        Dim HeightStat(Header.FrameHeight - 1) As Long
                                        Parallel.For(0, Header.FrameWidth - 1, Sub(IdxY)
                                                                                   For IdxX As Integer = 0 To Header.FrameHeight - 1
                                                                                       WidthStat(IdxY) += SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(IdxY, IdxX)
                                                                                       HeightStat(IdxX) += SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(IdxY, IdxX)
                                                                                   Next IdxX
                                                                               End Sub
                                )
                                        'Get pixel peak of the image
                                        ForCommonCode.SlidingAVG(WidthStat, DB.SlideWidth, DB.MyTrackCenterX) : DB.MyTrackCenterX = ((DB.MyTrackCenterX \ 2) * 2) - (DB.SlideWidth \ 2)
                                        ForCommonCode.SlidingAVG(HeightStat, DB.SlideWidth, DB.MyTrackCenterY) : DB.MyTrackCenterY = ((DB.MyTrackCenterX \ 2) * 2) - (DB.SlideWidth \ 2)
                                        Logger.Toc(Ptr)
                                End Select
                                FrameData.Add(New cFrameData(FrameCountIdx, DateTime.MinValue, DB.MyTrackCenterX, DB.MyTrackCenterY))
                                pgMain.Refresh()

                                'Calculate move of frame
                                Dim FrameShift_X As Integer = 0
                                Dim FrameShift_Y As Integer = 0
                                If FrameCountIdx = 1 Then
                                    If DB.TrackingSearchCenter = True Then
                                        DB.CutOutCenterX = DB.MyTrackCenterX
                                        DB.CutOutCenterY = DB.MyTrackCenterY
                                    End If
                                    InitCenter_X = DB.MyTrackCenterX
                                    InitCenter_Y = DB.MyTrackCenterY
                                Else
                                    FrameShift_X = DB.MyTrackCenterX - InitCenter_X
                                    FrameShift_Y = DB.MyTrackCenterY - InitCenter_Y
                                End If
                                Dim NewCenter_X As Integer = DB.CutOutCenterX + FrameShift_X
                                Dim NewCenter_Y As Integer = DB.CutOutCenterY + FrameShift_Y

                                'Cut out center and store
                                Dim CenterImage(DB.CutOutWidth - 1, DB.CutOutHeight - 1) As UInt16
                                Dim Ptr1 As Integer = 0
                                Dim Ptr2 As Integer = 0
                                Dim X_Start As Integer = NewCenter_X - (DB.CutOutWidth \ 2)
                                Dim X_Stop As Integer = NewCenter_X + (DB.CutOutWidth \ 2) - 1
                                Dim Y_Start As Integer = NewCenter_Y - (DB.CutOutHeight \ 2)
                                Dim Y_Stop As Integer = NewCenter_Y + (DB.CutOutHeight \ 2) - 1
                                FixIndizes(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(0), SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data.GetUpperBound(1), X_Start, X_Stop, Y_Start, Y_Stop)
                                Logger.Log("Frame #" & FrameCountIdx.ValRegIndep & ": X=" & X_Start.ValRegIndep & "..." & X_Stop.ValRegIndep & " / Y=" & Y_Start.ValRegIndep & "..." & Y_Stop.ValRegIndep)
                                For Idx_X As Integer = X_Start To X_Stop
                                    For Idx_Y As Integer = Y_Start To Y_Stop
                                        CenterImage(Ptr1, Ptr2) = SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(Idx_X, Idx_Y)
                                        CenterSumImage(Ptr1, Ptr2) += SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data(Idx_X, Idx_Y)
                                        Ptr2 += 1
                                    Next Idx_Y
                                    Ptr1 += 1 : Ptr2 = 0
                                Next Idx_X
                                SEROut.AppendFrame(CenterImage)
                                If String.IsNullOrEmpty(DB.FITSOutFileName) = False Then
                                    Dim FITSFileName As String = System.IO.Path.Combine(InputPath, DB.FITSOutFileName.Replace("#####", Format(FrameCountIdx, "00000").Trim))
                                    cFITSWriter.Write(FITSFileName, CenterImage, cFITSWriter.eBitPix.Int16)
                                End If

                                'Debug - save 1st frame
                                If FrameCountIdx = 1 And String.IsNullOrEmpty(DB.FirstFITSFile) = False Then
                                    Dim FITSFirstName As String = System.IO.Path.Combine(OutputPath, DB.FirstFITSFile.Replace(DB.FileNamePlaceHolder, FileNameOnly))
                                    cFITSWriter.Write(FITSFirstName, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, cFITSWriter.eBitPix.Int16)
                                End If

                                tspbMain.Value = FrameCountIdx
                                tsslMain.Text = "Frame " & Format(FrameCountIdx, "0000").Trim & "/" & Format(tspbMain.Maximum, "0000").Trim
                                System.Windows.Forms.Application.DoEvents()

                                If FrameCountIdx > DB.CutOut_LastFrame Then Exit For

                            Next FrameCountIdx

                            'Finish SER file
                            SEROut.CloseSerFile()

                            'Get CenterSumImage as UInt16
                            Dim MaxData As UInt32 = UInt32.MinValue
                            Dim MinData As UInt32 = UInt32.MaxValue
                            IntelIPP.MinMax(CenterSumImage, MinData, MaxData)
                            If MinData <> MaxData Then
                                Dim CenterSumImage_UInt16(CenterSumImage.GetUpperBound(0), CenterSumImage.GetUpperBound(1)) As UInt16
                                For Idx1 As Integer = 0 To CenterSumImage.GetUpperBound(0)
                                    For Idx2 As Integer = 0 To CenterSumImage.GetUpperBound(1)
                                        CenterSumImage_UInt16(Idx1, Idx2) = CType(((CenterSumImage(Idx1, Idx2) - MinData) / (MaxData - MinData)) * UInt16.MaxValue, UInt16)
                                    Next Idx2
                                Next Idx1
                                'Store total peak image
                                cFITSWriter.Write(System.IO.Path.Combine(OutputPath, DB.FITSCenterSumFile.Replace(DB.FileNamePlaceHolder, FileNameOnly)), CenterSumImage_UInt16, cFITSWriter.eBitPix.Int16)
                            Else
                                'Image is complete the same level ...
                            End If


                        End If

                    Else

                        'TODO: Calculate "really" stored number of frames and read these frames

                    End If

                End If

                'Close all streams
                BinaryIN.Close()
                FileIO.Close()

            End Using
        End Using

        tspbMain.Value = 0
        CType(sender, System.Windows.Forms.Button).Enabled = True : DE()

    End Sub

    '''<summary>Fix the indizes to match the required size and index ranges.</summary>
    Private Sub FixIndizes(ByVal UBound0 As Integer, ByVal UBound1 As Integer, ByRef XStart As Integer, ByRef XStop As Integer, ByRef YStart As Integer, ByRef YStop As Integer)
        '1. Remember original size
        Dim OrigWidth As Integer = XStop - XStart + 1
        Dim OrigHeight As Integer = YStop - YStart + 1
        '2. Correct wrong parameters
        If XStart < 0 Then XStart = 0
        If YStart < 0 Then YStart = 0
        If XStop > UBound0 Then XStop = UBound0
        If YStop > UBound1 Then YStop = UBound1
        '3. Fix
        If XStop - XStart + 1 <> OrigWidth Then
            'todo
        End If
        If YStop - YStart + 1 <> OrigHeight Then
            'todo
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub tsmiReadHeader_Click(sender As Object, e As EventArgs) Handles tsmiReadHeader.Click
        Dim NL As String = System.Environment.NewLine
        Using X As New System.IO.FileStream(DB.InputFile, IO.FileMode.Open, IO.FileAccess.Read)
            Dim SEReader As New cSERFormat.cSERHeader(New System.IO.BinaryReader(X))
            tbLog.Text = Join(SEReader.PrintInfo.ToArray, System.Environment.NewLine)
            Dim FileLength As Long = (New System.IO.FileInfo(DB.InputFile)).Length
            Dim DataContentLength As Long = CLng(SEReader.FrameCount) * CLng(SEReader.FrameWidth) * CLng(SEReader.FrameHeight) * CLng(SEReader.BytePerPixel)
            tbLog.Text &= NL & "File length    : " & FileLength.ValRegIndep & " byte"
            tbLog.Text &= NL & "Content length : " & DataContentLength.ValRegIndep & " byte"
            tbLog.Text &= NL & "Trailer length : " & ((FileLength - DataContentLength - cSERFormat.cSERHeader.SERHeaderLength) / SEReader.FrameCount).ValRegIndep & " byte per frame"
        End Using
    End Sub

    Private Sub OpenInputPathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenInputPathToolStripMenuItem.Click
        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(DB.InputFile)) Then Process.Start(System.IO.Path.GetDirectoryName(DB.InputFile))
    End Sub

    Private Sub ShowLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowLogToolStripMenuItem.Click
        'Logger.ShowLog()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub tsmiFile_SaveXLS_Click(sender As Object, e As EventArgs) Handles tsmiFile_SaveXLS.Click

        Using workbook As New ClosedXML.Excel.XLWorkbook

            ''1.) Histogram
            'If AddHisto = True Then
            '    Dim XY As New List(Of Object())
            '    For Each Key As Long In LoopStat.MonochromHistogram_Int.Keys
            '        Dim Values As New List(Of Object)
            '        Values.Add(Key)
            '        Values.Add(LoopStat.MonochromHistogram_Int(Key))
            '        If LoopStat.BayerHistograms_Int(0, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(0, 0)(Key)) Else Values.Add(String.Empty)
            '        If LoopStat.BayerHistograms_Int(0, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(0, 1)(Key)) Else Values.Add(String.Empty)
            '        If LoopStat.BayerHistograms_Int(1, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(1, 0)(Key)) Else Values.Add(String.Empty)
            '        If LoopStat.BayerHistograms_Int(1, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(1, 1)(Key)) Else Values.Add(String.Empty)
            '        XY.Add(Values.ToArray)
            '    Next Key
            '    Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram")
            '    worksheet.Cell(1, 1).InsertData(New List(Of String)({"Pixel value", "Count Mono", "Count Bayer_0_0", "Count Bayer_0_1", "Count Bayer_1_0", "Count Bayer_1_1"}), True)
            '    worksheet.Cell(2, 1).InsertData(XY)
            '    For Each col As ClosedXML.Excel.IXLColumn In worksheet.ColumnsUsed
            '        col.AdjustToContents()
            '    Next col
            'End If

            ''2.) Histo density
            'Dim HistDens As New List(Of Object())
            'For Each Key As UInteger In LoopStat.MonoStatistics_Int.HistXDist.Keys
            '    HistDens.Add(New Object() {Key, LoopStat.MonoStatistics_Int.HistXDist(Key)})
            'Next Key
            'Dim worksheet2 As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram Density")
            'worksheet2.Cell(1, 1).InsertData(New List(Of String)({"Step size", "Count"}), True)
            'worksheet2.Cell(2, 1).InsertData(HistDens)
            'For Each col As ClosedXML.Excel.IXLColumn In worksheet2.ColumnsUsed
            '    col.AdjustToContents()
            'Next col

            ''4) Save and open
            'Dim FileToGenerate As String = IO.Path.Combine(M.DB.StoragePath, sfdMain.FileName)
            'workbook.SaveAs(FileToGenerate)

        End Using

    End Sub

    Private Sub tsmiDebug_PlotTracking_Click(sender As Object, e As EventArgs) Handles tsmiDebug_PlotTracking.Click

        Dim Center_X As New List(Of Double)
        Dim Center_Y As New List(Of Double)
        For Each Item As cFrameData In FrameData
            Center_X.Add(Item.Center_X)
            Center_Y.Add(Item.Center_Y)
        Next Item

        Dim Disp As New cZEDGraphForm : Disp.PlotData("Center", Center_X.ToArray, Center_Y.ToArray, Drawing.Color.Red)

    End Sub

End Class
