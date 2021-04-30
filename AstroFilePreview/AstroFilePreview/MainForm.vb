Option Explicit On
Option Strict On

Public Class MainForm

    Dim PreviewGen As New frmImageDisplay

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim root = New TreeNode("C:\") : root.Tag = "C:\" : root.ImageKey = "drive"
        tvDirTree.Nodes.Add(root)
        PreviewGen.GenerateDisplayImage()
    End Sub

    '''<summary>Append all subdirectories to the selected node.</summary>
    Private Function AppendSubDirectories(ByRef Node As TreeNode) As Boolean
        'Get a list of all subdirectories
        Dim AllDirectories As New List(Of String)
        Try
            AllDirectories = New List(Of String)(System.IO.Directory.GetDirectories(CStr(Node.Tag)))
        Catch ex As Exception
            'Directory list is not available -> indicate as red and exit
            Node.ForeColor = Color.Red
            Return False
        End Try
        'Add all directories
        For Each subDir As String In AllDirectories
            Dim info As New IO.DirectoryInfo(subDir)
            If info.Exists Then
                Dim NodeToAdd As New TreeNode(info.Name)
                NodeToAdd.Tag = subDir
                NodeToAdd.ImageKey = "folder"
                Node.Nodes.Add(NodeToAdd)
            End If
        Next subDir
        Return True
    End Function

    '''<summary>List all files in the selected directory.</summary>
    Private Sub ListAllFiles(ByRef Node As TreeNode)
        lvFiles.Items.Clear()
        Dim nodeDirInfo As New IO.DirectoryInfo(CStr(Node.Tag))
        For Each file As IO.FileInfo In nodeDirInfo.GetFiles
            Dim item As New ListViewItem(file.Name, "file")
            Dim subItems As ListViewItem.ListViewSubItem() = {New ListViewItem.ListViewSubItem(item, "File"), New ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())}
            item.SubItems.AddRange(subItems)
            lvFiles.Items.Add(item)
        Next file
        lvFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub

    Private Sub tvDirTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvDirTree.AfterSelect
        If AppendSubDirectories(tvDirTree.SelectedNode) = True Then
            ListAllFiles(tvDirTree.SelectedNode)
        End If
        tvDirTree.SelectedNode.Expand()
    End Sub

    '======================================================================================================

    Private Sub PreviewImage(ByVal File As String)
        With PreviewGen
            .FileToDisplay = File
            .Show()
            .SingleStatCalc = CurrentData
            .StatToUsed = CurrentStatistics
            .MyIPP = DB.IPP
            .Props.MinCutOff_ADU = CurrentStatistics.MonoStatistics_Int.Min.Key
            .Props.MaxCutOff_ADU = CurrentStatistics.MonoStatistics_Int.Max.Key
            .GenerateDisplayImage()
        End With

    End Sub

End Class


