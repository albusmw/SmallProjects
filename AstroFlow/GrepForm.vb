Option Explicit On
Option Strict On

Public Class GrepForm

    Dim INIFile As String = "Config.INI"
    Dim UltraEditPath As String = String.Empty
    Dim Stopper As New Stopwatch
    Dim FitsExtensions As String = ""

    Private Sub Search()

        'Create the "real" FITS search string
        Dim FitsSearch As New List(Of String)
        FitsSearch.Add("nocase:" & tbFileFilter.Text & ".fit")
        FitsSearch.Add("nocase:" & tbFileFilter.Text & ".fits")
        Dim SearchArgument As String = String.Empty
        If String.IsNullOrEmpty(tbSearchIn.Text) = True Then
            SearchArgument = Join(FitsSearch.ToArray, "|")
        Else
            SearchArgument = """" & tbSearchIn.Text & """ " & Join(FitsSearch.ToArray, "|")
        End If

        'Create the query and run
        UpdateAction("Searching ...")
        Stopper.Reset() : Stopper.Start()
        Everything.Everything_SetSearchW(SearchArgument)
        Everything.Everything_QueryW(True)
        UpdateCount(Everything.Everything_GetNumResults)
        Stopper.Stop()
        Debug.Print("Search took " & Stopper.ElapsedMilliseconds.ToString.Trim & " ms")

        'Get all found files from the Everything DLL
        Stopper.Reset() : Stopper.Start()
        Dim bufsize As Integer = 260
        Dim buf As New System.Text.StringBuilder(bufsize)
        Dim Results As New List(Of String)
        For Idx As Integer = 0 To Everything.Everything_GetNumResults() - 1
            Everything.Everything_GetResultFullPathNameW(Idx, buf, bufsize)
            Results.Add(buf.ToString)
        Next Idx
        Debug.Print("Search query took " & Stopper.ElapsedMilliseconds.ToString.Trim & " ms")

        'Post-ccan all found files for special FITS propertoes (currently not used ...)
        Stopper.Reset() : Stopper.Start()
        Dim FilteredList As New List(Of String)
        Dim TotalCount As Long = 0
        Dim UseWindowsFind As Boolean = False
        tspbMain.Maximum = Results.Count
        tspbMain.Value = 0

        For Each Result As String In Results
            tspbMain.Value += 1
            FilteredList.Add(Result)
        Next Result
        Debug.Print("Grep took " & Stopper.ElapsedMilliseconds.ToString.Trim & " ms")

        'Display (sorted) list
        FilteredList.Sort()
        lbResults.Items.Clear()
        UpdateCount(FilteredList.Count)
        For Each FilteredResult As String In FilteredList
            lbResults.Items.Add(FilteredResult)
        Next FilteredResult

        tspbMain.Value = 0
        UpdateAction("")

    End Sub

    Private Sub btnSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnSearch.Click
        Search()
    End Sub

    Private Sub lbResults_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lbResults.SelectedIndexChanged

        'Get all file header elements
        Dim SelectedItem As String = CStr(lbResults.SelectedItem)
        lbDisplayedFile.Text = SelectedItem
        Dim HeaderElements As List(Of cFITSHeaderChanger.sHeaderElement) = cFITSHeaderChanger.ReadHeader(SelectedItem)

        'Display all file header elements - the "TRIM" is optional for a better display
        lbLinesFound.Items.Clear()
        For Each HeaderElement As cFITSHeaderChanger.sHeaderElement In HeaderElements
            lbLinesFound.Items.Add(HeaderElement.Keyword.PadRight(20) & HeaderElement.Value.Trim.PadLeft(40))
        Next HeaderElement

    End Sub

    Private Sub UpdateAction(ByVal Text As String)
        If String.IsNullOrEmpty(Text) = False Then
            tsslAction.Text = Text
        Else
            tsslAction.Text = "-- IDLE --"
        End If
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub UpdateCount(ByVal Count As Long)
        tsslFoundFiles.Text = Count.ToString.Trim
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub lbLinesFound_DoubleClick(sender As Object, e As System.EventArgs) Handles lbLinesFound.DoubleClick
        Dim SelectedLine As String = lbLinesFound.SelectedItem.ToString
        Dim LineToGo As Long = CLng(SelectedLine.Substring(0, 9))
        SelectedLine = SelectedLine.Substring(10)
        If System.IO.File.Exists(UltraEditPath) = True Then
            Process.Start(UltraEditPath, """" & lbDisplayedFile.Text & """")
        Else
            Process.Start("notepad.exe", """" & lbDisplayedFile.Text & """")
        End If
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim INIFileContent As New List(Of String)
        INIFileContent.Add("[General]")
        INIFileContent.Add("SearchIn=" & tbSearchIn.Text)
        INIFileContent.Add("FileFilter=" & tbFileFilter.Text)
        'If System.IO.File.Exists(INIFile) Then System.IO.File.Delete(INIFile)
        'System.IO.File.WriteAllLines(INIFile, INIFileContent.ToArray)
    End Sub

    Private Sub MainForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        UltraEditPath = CStr(Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Uedit32.exe", "Path", String.Empty))
        UltraEditPath = UltraEditPath & "\Uedit32.exe"
        tbSearchIn.Text = Ato.INIRead.GetINIFromFile(INIFile, "General", "SearchIn", "<Enter directory to search in>")
        tbFileFilter.Text = Ato.INIRead.GetINIFromFile(INIFile, "General", "FileFilter", "<Enter file pattern to match without fit(s) extension, e.g. *bias*>")
    End Sub

    Private Sub tbContains_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles tbFileFilter.KeyUp
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
            Search()
        End If
    End Sub

    Private Sub lbDisplayedFile_DoubleClick(sender As Object, e As System.EventArgs) Handles lbDisplayedFile.DoubleClick
        System.Diagnostics.Process.Start(lbDisplayedFile.Text)
    End Sub

    Private Sub lbResults_DoubleClick(sender As Object, e As EventArgs) Handles lbResults.DoubleClick
        If System.IO.File.Exists(tbViewer.Text) And System.IO.File.Exists(CStr(lbResults.SelectedItem)) Then
            Process.Start(tbViewer.Text, Chr(34) & CStr(lbResults.SelectedItem) & Chr(34))
        End If
    End Sub

    Private Sub btnFolderSelect_Click(sender As Object, e As EventArgs) Handles btnFolderSelect.Click
        With fbdMain
            .SelectedPath = tbSearchIn.Text
            If .ShowDialog = DialogResult.OK Then
                tbSearchIn.Text = .SelectedPath
            End If
        End With
    End Sub

    Private Sub RowAndColumsStatisticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RowAndColumsStatisticsToolStripMenuItem.Click

        Dim X As New cFITSReader
        Dim ImageData(,) As Double = {}
        Dim FileName As String = CStr(lbResults.SelectedItem)

        'Read the complete FITS file
        X.ReadIn(FileName, ImageData)

        'Calculate sum for all rows and columns
        Dim XSum(ImageData.GetUpperBound(0)) As Double
        Dim YSum(ImageData.GetUpperBound(1)) As Double
        For Idx1 As Integer = 0 To ImageData.GetUpperBound(0)
            For Idx2 As Integer = 0 To ImageData.GetUpperBound(1)
                XSum(Idx1) += ImageData(Idx1, Idx2)
                YSum(Idx2) += ImageData(Idx1, Idx2)
            Next Idx2
        Next Idx1

        'Normalize to the count -> results in the mean value
        For Idx As Integer = 0 To XSum.GetUpperBound(0)
            XSum(Idx) /= (ImageData.GetUpperBound(1) + 1)
        Next Idx
        For Idx As Integer = 0 To YSum.GetUpperBound(0)
            YSum(Idx) /= (ImageData.GetUpperBound(0) + 1)
        Next Idx

        'Generate histogram data for row and columns
        Dim Histo As New List(Of String)
        For Idx1 As Integer = 0 To Math.Max(XSum.GetUpperBound(0), YSum.GetUpperBound(0))
            Dim CSVLine As New List(Of String)
            CSVLine.Add(Idx1.ToString.Trim)                                                     'Index
            If Idx1 <= XSum.GetUpperBound(0) Then
                CSVLine.Add((Idx1 / XSum.GetUpperBound(0)).ToString.Trim.Replace(",", "."))     'normalized X coordinate
                CSVLine.Add(XSum(Idx1).ToString.Trim)                                           'X value
            End If
            If Idx1 <= YSum.GetUpperBound(0) Then
                CSVLine.Add((Idx1 / YSum.GetUpperBound(0)).ToString.Trim.Replace(",", "."))     'normalized Y coordinate
                CSVLine.Add(YSum(Idx1).ToString.Trim)                                           'Y value
            End If
            Histo.Add(Join(CSVLine.ToArray, ";"))
        Next Idx1

        'Store as CSV file and start it
        Try
            Dim CSVFileName As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(FileName), System.IO.Path.GetFileNameWithoutExtension(FileName) & "_HISTO.csv")
            If System.IO.File.Exists(CSVFileName) = True Then System.IO.File.Delete(CSVFileName)
            System.IO.File.WriteAllLines(CSVFileName, Histo.ToArray)
            Process.Start(CSVFileName)
        Catch ex As Exception
            MsgBox("Problems with creating the CSV file: <" & ex.Message & ">")
        End Try


    End Sub

    Private Sub OpenWithFITSWorkToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenWithFITSWorkToolStripMenuItem.Click
        lbResults_DoubleClick(Nothing, Nothing)
    End Sub

End Class
