Option Explicit On
Option Strict On

Public Class MainForm

    Dim AllHeaders As Dictionary(Of eFITSKeywords, List(Of Object))

    Dim WithEvents Changer As New cFITSHeaderChanger
    Dim OriginalFile As String = "D:\ORIG.FIT"
    Dim NewFile As String = "D:\CHANGED.FIT"

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

    tbLog.Enabled = False

        tbLog.Text = String.Empty

        'Move over all elements
        For Each FileName As String In lbInputFiles.Items
            tbLog.Text = "Processing <" & FileName & ">"
            Dim HeaderElements As List(Of cFITSHeaderParser.sHeaderElement) = cFITSHeaderChanger.ReadHeader(FileName)
            For RowIdx As Integer = 0 To dgvMain.RowCount - 1

            Next RowIdx
        Next FileName

        Exit Sub



        'TEST CODE BELOW ....

        'Prepare custom elements
        Dim SiteLong As Double = 11 + (41 / 60) + (30.7 / 3600)
    Dim SiteLat As Double = 47 + (52 / 60) + (41.8 / 3600)
    Dim FOV As Double = (180 / Math.PI) * 2 * Math.Atan(36 / (2 * 2540))

    'List of elements that need to be changed or inserted if missing
    Dim ValuesToChange As New Dictionary(Of String, Object)

    AddVal(ValuesToChange, FITSKeywords.OBSERVER(0), cFITSHeaderChanger.Format("Martin Weiss"), FITSKeywords.OBSERVER(1))

    AddVal(ValuesToChange, FITSKeywords.OBSERVAT(0), cFITSHeaderChanger.Format("AllSky Holzkirchen"), FITSKeywords.OBSERVAT(1))
    AddVal(ValuesToChange, FITSKeywords.SITELAT(0), cFITSHeaderChanger.Format(SiteLat), FITSKeywords.SITELAT(1))
    AddVal(ValuesToChange, FITSKeywords.SITELONG(0), cFITSHeaderChanger.Format(SiteLong), FITSKeywords.SITELONG(1))
    AddVal(ValuesToChange, FITSKeywords.SITEELEV(0), cFITSHeaderChanger.Format(691.0), FITSKeywords.SITEELEV(1))

    AddVal(ValuesToChange, FITSKeywords.TELESCOP(0), cFITSHeaderChanger.Format("Planewave CDK 12.5"), FITSKeywords.TELESCOP(1))
    AddVal(ValuesToChange, FITSKeywords.TELAPER(0), cFITSHeaderChanger.Format(0.317), FITSKeywords.TELAPER(1))
    AddVal(ValuesToChange, FITSKeywords.TELFOC(0), cFITSHeaderChanger.Format(2.54), FITSKeywords.TELFOC(1))
    AddVal(ValuesToChange, FITSKeywords.TELSCALE(0), cFITSHeaderChanger.Format((FOV * 3600) / 36), FITSKeywords.TELSCALE(1))

    AddVal(ValuesToChange, FITSKeywords.FOV1(0), cFITSHeaderChanger.Format(FOV), FITSKeywords.FOV1(1))
    AddVal(ValuesToChange, FITSKeywords.FOV2(0), cFITSHeaderChanger.Format(FOV), FITSKeywords.FOV2(1))

    AddVal(ValuesToChange, FITSKeywords.CCD_TEMP(0), cFITSHeaderChanger.Format(-20.0), FITSKeywords.CCD_TEMP(1))
    AddVal(ValuesToChange, FITSKeywords.TEMPERAT(0), cFITSHeaderChanger.Format(20.0), FITSKeywords.TEMPERAT(1))

    AddVal(ValuesToChange, FITSKeywords.EXPTIME(0), cFITSHeaderChanger.Format(600), FITSKeywords.EXPTIME(1))

    AddVal(ValuesToChange, FITSKeywords.DATEORIG(0), cFITSHeaderChanger.Format("1910-08-02"), FITSKeywords.DATEORIG(1))

    AddVal(ValuesToChange, FITSKeywords.DATE_OBS(0), cFITSHeaderChanger.Format(Now), FITSKeywords.DATE_OBS(1))
    AddVal(ValuesToChange, FITSKeywords.RA_ORIG(0), cFITSHeaderChanger.Format("19:11:42"), FITSKeywords.RA_ORIG(1))
    AddVal(ValuesToChange, FITSKeywords.DEC_ORIG(0), cFITSHeaderChanger.Format("15:04:00"), FITSKeywords.DEC_ORIG(1))

    AddVal(ValuesToChange, FITSKeywords.FILTER(0), cFITSHeaderChanger.Format("NONE"), FITSKeywords.FILTER(1))



    ValuesToChange.Add("FOCUSSZ", New String() {cFITSHeaderChanger.Format(2300), "Focuser step size in microns"})


    'Custom elements
    ValuesToChange.Add("SET-TEMP", New String() {cFITSHeaderChanger.Format(-20.0), "CCD temperature setpoint in degrees C"})
    ValuesToChange.Add("COLORTYP", cFITSHeaderChanger.Format(0))
    ValuesToChange.Add("IMAGETYP", New String() {"'Dark Frame'", "Light Frame, Bias Frame, Dark Frame, Flat Frame, or Tricolor Image"})

    Changer.ChangeHeader(OriginalFile, NewFile, ValuesToChange)

    tbLog.Enabled = True

  End Sub

  Private Sub AddVal(ByRef Elements As Dictionary(Of String, Object), ByVal Key As String, ByVal Value As String)
    Elements.Add(Key, Value)
  End Sub

  Private Sub AddVal(ByRef Elements As Dictionary(Of String, Object), ByVal Key As String, ByVal Value As String, ByVal Comment As String)
    Elements.Add(Key, New String() {Value, Comment})
  End Sub

  Private Sub Changer_Log(Text As String) Handles Changer.Log
    tbLog.Text &= Text & System.Environment.NewLine
  End Sub

  Private Sub tbFilesToProcess_DragDrop(sender As Object, e As DragEventArgs)
    e.Effect = DragDropEffects.All
  End Sub

    Private Sub lbInputFiles_DragDrop(sender As Object, e As DragEventArgs) Handles lbInputFiles.DragDrop

        Try

            'Read paths of all files contained in the drop
            Dim FilePaths() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

            'Add all new elements
            For Each DroppedItem As String In FilePaths
                For Each File As String In GetAllObjects(DroppedItem)
                    If lbInputFiles.Items.Contains(File) = False Then lbInputFiles.Items.Add(File)
                Next File
            Next DroppedItem

            'Set caption
            gbFiles.Text = "Files to process (drop here ...) - TOTAL: " & lbInputFiles.Items.Count.ToString.Trim & " files"

        Catch ex As Exception

            'Do nothing special here

        End Try

    End Sub

    Private Sub lbInputFiles_DragEnter(sender As Object, e As DragEventArgs) Handles lbInputFiles.DragEnter

        ' Checks if the detected Drop-Event is a File-Drop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then

            ' Read paths of all files contained in the drop
            Dim FilePaths() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

            ' Check number of files contained in the drop

            ' -------------- Multi File Drop ----------------
            For Each FilePath As String In FilePaths
                ' Check if the dropped filetype is allowed
                If FilePath.ToUpper.EndsWith("FIT") Or FilePath.ToUpper.EndsWith("FITS") Or System.IO.File.GetAttributes(FilePath) = IO.FileAttributes.Directory Then
                    ' Config File or Settings File --> Allow filedrop
                    e.Effect = DragDropEffects.Copy
                Else
                    e.Effect = DragDropEffects.None
                    Exit For
                End If
            Next FilePath

        End If

    End Sub

    Private Sub lbInputFiles_KeyUp(sender As Object, e As KeyEventArgs) Handles lbInputFiles.KeyUp
    If e.KeyCode = Keys.Delete Then
      lbInputFiles.Items.Remove(lbInputFiles.SelectedItem)
    End If
  End Sub

  Private Sub lbInputFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbInputFiles.SelectedIndexChanged

        'Get selected file and read all headers
        Dim SelectedFile As String = CStr(lbInputFiles.SelectedItem)
        Dim HeaderElements As List(Of cFITSHeaderParser.sHeaderElement) = cFITSHeaderChanger.ReadHeader(SelectedFile)

        'Display number of elements and store if required
        gbHeader.Text = HeaderElements.Count.ToString.Trim & " elements in file <" & SelectedFile & "> found"
        If cbSort.Checked = True Then HeaderElements.Sort(AddressOf cFITSHeaderParser.sHeaderElement.Sorter)
        lbHeader.Items.Clear()

        'Display a list of all elements
        For Each Entry As cFITSHeaderParser.sHeaderElement In HeaderElements
            Dim NewEntry As String = FITSKeyword.KeywordString(Entry.Keyword).PadRight(8) & "= " & Entry.Value.ToString.Trim
            lbHeader.Items.Add(NewEntry)
        Next Entry

        'Fill the summary dara grid
        FillDateGridView()

  End Sub

  Private Sub cbSort_CheckedChanged(sender As Object, e As EventArgs) Handles cbSort.CheckedChanged
    lbInputFiles_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Private Sub FillDateGridView()

        dgvMain.Rows.Clear()

        'We should do a matrix here as files that miss an element are not processed correct ...

        'Read all headers from all files
        AllHeaders = New Dictionary(Of eFITSKeywords, List(Of Object))
        Dim FileCount As Integer = 0
        For Each File As Object In lbInputFiles.SelectedItems
            Dim FileName As String = CStr(File)
            Dim HeaderElements As List(Of cFITSHeaderParser.sHeaderElement) = cFITSHeaderChanger.ReadHeader(FileName)
            For Each Entry As cFITSHeaderParser.sHeaderElement In HeaderElements
                If AllHeaders.ContainsKey(Entry.Keyword) = False Then AllHeaders.Add(Entry.Keyword, New List(Of Object))
            Next Entry
            FileCount += 1
        Next File

        'Read all entries
        For Each File As Object In lbInputFiles.SelectedItems
            Dim FileName As String = CStr(File)
            Dim HeaderElements As List(Of cFITSHeaderParser.sHeaderElement) = cFITSHeaderChanger.ReadHeader(FileName)
            For Each Entry As cFITSHeaderParser.sHeaderElement In HeaderElements
                If AllHeaders(Entry.Keyword).Contains(Entry.Value) = False Then AllHeaders(Entry.Keyword).Add(Entry.Value)
            Next Entry
            FileCount += 1
        Next File

        'Display all information for all files
        For Each HeaderElement As eFITSKeywords In AllHeaders.Keys
            Dim BackColor As Color = Color.White
            Select Case AllHeaders(HeaderElement).Count
                Case 1
                    dgvMain.Rows.Add(New Object() {HeaderElement, "All the same", "Don't touch", "<" & AllHeaders(HeaderElement)(0).ToString & ">"}) : BackColor = Color.Gray
                Case FileCount
                    dgvMain.Rows.Add(New Object() {HeaderElement, "All individual", "Don't touch", FileCount.ToString.Trim & " elements"}) : BackColor = Color.Yellow
                Case Else
                    dgvMain.Rows.Add(New Object() {HeaderElement, "Mixed", "Don't touch", AllHeaders(HeaderElement).Count.ToString.Trim & " elements"}) : BackColor = Color.Orange
            End Select
            With dgvMain.Rows(dgvMain.Rows.Count - 1).Cells(3)
                .Style.Font = New Font("Courier New", 8)
                .Style.BackColor = BackColor
            End With
            dgvMain.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        Next HeaderElement

    End Sub

    Private Sub dgvMain_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMain.CellClick
        Try
            'Display all individual elements
            If e.ColumnIndex = 3 Then
                Dim HeaderEntry As eFITSKeywords = cFITSHeaderParser.GetKeywordEnum(dgvMain.Rows(e.RowIndex).Cells(0).Value.ToString)
                MsgBox(Join(AllHeaders(HeaderEntry).ToArray, System.Environment.NewLine))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        end
    End Sub

    Private Sub SaveListOfFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveListOfFilesToolStripMenuItem.Click
        With sfdMain
            .FileName = String.Empty
            .Title = "Store list of files"
            .Filter = "File list (*.txt)|*.txt"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With
        Dim AllFiles As New List(Of String)
        For Each Item As String In lbInputFiles.Items
            AllFiles.Add(Item)
        Next Item
        System.IO.File.WriteAllLines(sfdMain.FileName, AllFiles.ToArray)
    End Sub

    Private Sub ClearListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearListToolStripMenuItem.Click
        lbInputFiles.Items.Clear()
    End Sub

    Private Sub LoadListOfFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadListOfFilesToolStripMenuItem.Click
        With ofdMain
            .FileName = String.Empty
            .Title = "Load list of files"
            .Filter = "File list (*.txt)|*.txt"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With
        lbInputFiles.Items.AddRange(System.IO.File.ReadAllLines(ofdMain.FileName))
    End Sub

    Private Sub tsmiAddDirectory_Click(sender As Object, e As EventArgs) Handles tsmiAddDirectory.Click

        'Select a folder or try to use a manual location
        With fbdMain
            If .ShowDialog <> DialogResult.OK Then
                Dim ManualSelect As String = InputBox("Manual entry:", "Manual entry", String.Empty)
                If System.IO.Directory.Exists(ManualSelect) = False Then Exit Sub
                fbdMain.SelectedPath = ManualSelect
            End If
        End With

        Try

            'Add all new elements
            For Each File As String In GetAllObjects(fbdMain.SelectedPath)
                If lbInputFiles.Items.Contains(File) = False Then lbInputFiles.Items.Add(File)
            Next File

            'Set caption
            gbFiles.Text = "Files to process (drop here ...) - TOTAL: " & lbInputFiles.Items.Count.ToString.Trim & " files"

        Catch ex As Exception

            'Nothing special here right now

        End Try


    End Sub

    '=======================================================================================================================
    'Sub functions
    '=======================================================================================================================

    '''<summary>Recusrively add all FIT and FITS files in the passed folder.</summary>
    '''<param name="StartPath">Folder to start from.</param>
    '''<param name="AllFiles">All FIT and FITS files.</param>
    Private Sub DirScan(ByVal StartPath As String, ByRef AllFiles As List(Of String))
        For Each Directory As String In System.IO.Directory.GetDirectories(StartPath)
            DirScan(Directory, AllFiles)
        Next Directory
        For Each File As String In System.IO.Directory.GetFiles(StartPath)
            Select Case System.IO.Path.GetExtension(File).ToUpper
                Case ".FIT", ".FITS" : AllFiles.Add(File)
            End Select
        Next File
    End Sub

    Private Function GetAllObjects(ByVal Path As String) As List(Of String)
        Dim RetVal As New List(Of String)
        If (System.IO.File.GetAttributes(Path) And IO.FileAttributes.Directory) = IO.FileAttributes.Directory Then
            'Directory was dropped -> get all files
            Dim DirFiles As New List(Of String) : DirScan(Path, DirFiles)
            For Each DirFile As String In DirFiles
                If RetVal.Contains(DirFile) = False Then RetVal.Add(DirFile)
            Next DirFile
        Else
            'File was dropped -> add file
            If RetVal.Contains(Path) = False Then RetVal.Add(Path)
        End If
        Return RetVal
    End Function

End Class
