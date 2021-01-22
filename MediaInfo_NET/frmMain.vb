Imports System.Windows.Forms

Public Class MainForm

    Inherits Form
    ' Methods
    <DebuggerNonUserCode()> _
    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopy.Click
        Dim AllEntries As String = String.Empty
        For Each Entry As String In lbRenaming.Items
            AllEntries = AllEntries & Entry & System.Environment.NewLine
        Next Entry
        Clipboard.Clear()
        Clipboard.SetText(AllEntries)
        Interaction.MsgBox("Content copied to clipboard", MsgBoxStyle.OkOnly, Nothing)
    End Sub

    Private Sub btnRootDir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRootDir.Click
        Dim fbdMain As New FolderBrowserDialog
        If fbdMain.ShowDialog = DialogResult.OK Then tbRootDir.Text = fbdMain.SelectedPath
    End Sub

    Private Sub btnStart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStart.Click

        Dim DirectoryCount As Integer = 0
        Dim ItemToScan As New List(Of sItemToScan)

        lbRenaming.Items.Clear()

        'Scan all files
        For Each File As String In IO.Directory.GetFiles(Me.tbRootDir.Text)
            Dim SingleItem As sItemToScan = New sItemToScan(File, False)
            ItemToScan.Add(SingleItem)
        Next

        'Scan all directories
        For Each Directory As String In IO.Directory.GetDirectories(Me.tbRootDir.Text)
            Dim SingleItem As sItemToScan = New sItemToScan(Directory, True)
            ItemToScan.Add(SingleItem)
        Next

        'Add header
        MediaInfo_services.DescribeHeader(lbRenaming, True)

        'Now run the scan
        For Each scan As sItemToScan In ItemToScan
            If scan.IsDirectory Then
                Dim FolderName As String = scan.Path.Replace(IO.Path.GetDirectoryName(scan.Path), String.Empty)
                If FolderName.StartsWith("\") Then FolderName = FolderName.Substring(1)
                If FolderName.EndsWith(")") Then
                    Dim num2 As Integer = FolderName.LastIndexOf("(")
                    If num2 > 1 Then FolderName = FolderName.Substring(0, (num2 - 1)).Trim
                End If
            End If
            Dim files As New List(Of String)
            GetExtension(scan, files, True)
            DirectoryCount += 1
            Application.DoEvents()
        Next
        MsgBox((DirectoryCount.ToString.Trim & " directories found."), MsgBoxStyle.OkOnly, Nothing)
    End Sub

    Private Sub GetExtension(ByVal ItemToScan As sItemToScan, ByRef Files As List(Of String), ByVal LiveUpdate As Boolean)

        If ItemToScan.IsDirectory Then
            Dim AllFiles As String() = IO.Directory.GetFiles(ItemToScan.Path)
            If (AllFiles.Length <> 0) Then
                Dim scan As sItemToScan
                For Each SingleFile As String In AllFiles
                    scan = New sItemToScan(SingleFile, False)
                    GetExtension(scan, Files, LiveUpdate)
                Next
                For Each SingleDirectory As String In IO.Directory.GetDirectories(ItemToScan.Path)
                    scan = New sItemToScan(SingleDirectory, True)
                    GetExtension(scan, Files, LiveUpdate)
                Next
            End If
        Else

            'Check if this is a movie file
            Dim Extension As String = System.IO.Path.GetExtension(ItemToScan.Path).ToUpper.Replace(".", String.Empty)
            Select Case Extension
                Case "MKV", "TS", "T2S", "MPG", "MPEG", "AVI", "MP4", "FLV", "MOV", "DIVX", "VOB", "EVO", "WMV", "PS", "RMVB", "RM", "OGG", "OGV", "OGM"
                    Dim item As String = Strings.Join(MediaInfo_services.DescribeFile(lbRenaming, ItemToScan.Path, LiveUpdate), ", ")
                    Files.Add(item)
            End Select

        End If

    End Sub

    ' Nested Types
    Private Structure sItemToScan
        Public Path As String
        Public IsDirectory As Boolean
        Public Sub New(ByVal NewPath As String, ByVal NewIsDirectory As Boolean)
            Me.Path = NewPath
            Me.IsDirectory = NewIsDirectory
        End Sub
    End Structure

End Class
