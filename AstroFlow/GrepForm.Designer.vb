<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GrepForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.tbSearchIn = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbResults = New System.Windows.Forms.ListBox()
        Me.lbLinesFound = New System.Windows.Forms.ListBox()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslAction = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslFoundFiles = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tspbMain = New System.Windows.Forms.ToolStripProgressBar()
        Me.lbDisplayedFile = New System.Windows.Forms.Label()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.ttMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbFileFilter = New System.Windows.Forms.TextBox()
        Me.tbViewer = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.fbdMain = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnFolderSelect = New System.Windows.Forms.Button()
        Me.cmsFileAction = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RowAndColumsStatisticsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenWithFITSWorkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ssMain.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.cmsFileAction.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.Location = New System.Drawing.Point(1466, 18)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(105, 71)
        Me.btnSearch.TabIndex = 0
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'tbSearchIn
        '
        Me.tbSearchIn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSearchIn.Location = New System.Drawing.Point(105, 18)
        Me.tbSearchIn.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbSearchIn.Name = "tbSearchIn"
        Me.tbSearchIn.Size = New System.Drawing.Size(1293, 26)
        Me.tbSearchIn.TabIndex = 1
        Me.tbSearchIn.Text = "D:\ClearCase_snapshots\weis_m_K18"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Search in"
        '
        'lbResults
        '
        Me.lbResults.ContextMenuStrip = Me.cmsFileAction
        Me.lbResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbResults.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbResults.FormattingEnabled = True
        Me.lbResults.IntegralHeight = False
        Me.lbResults.ItemHeight = 20
        Me.lbResults.Location = New System.Drawing.Point(0, 0)
        Me.lbResults.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lbResults.Name = "lbResults"
        Me.lbResults.ScrollAlwaysVisible = True
        Me.lbResults.Size = New System.Drawing.Size(1557, 505)
        Me.lbResults.TabIndex = 7
        Me.ttMain.SetToolTip(Me.lbResults, "Click the file to display the found items in the lower text box")
        '
        'lbLinesFound
        '
        Me.lbLinesFound.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbLinesFound.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbLinesFound.FormattingEnabled = True
        Me.lbLinesFound.IntegralHeight = False
        Me.lbLinesFound.ItemHeight = 20
        Me.lbLinesFound.Location = New System.Drawing.Point(0, 0)
        Me.lbLinesFound.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lbLinesFound.Name = "lbLinesFound"
        Me.lbLinesFound.ScrollAlwaysVisible = True
        Me.lbLinesFound.Size = New System.Drawing.Size(1557, 446)
        Me.lbLinesFound.TabIndex = 8
        Me.ttMain.SetToolTip(Me.lbLinesFound, "Double-click the entry to open UltraEdit on the selected position")
        '
        'ssMain
        '
        Me.ssMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.tsslAction, Me.ToolStripStatusLabel2, Me.tsslFoundFiles, Me.tspbMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 1104)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Padding = New System.Windows.Forms.Padding(2, 0, 21, 0)
        Me.ssMain.Size = New System.Drawing.Size(1588, 31)
        Me.ssMain.TabIndex = 9
        Me.ssMain.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(67, 26)
        Me.ToolStripStatusLabel1.Text = "Action:"
        '
        'tsslAction
        '
        Me.tsslAction.Name = "tsslAction"
        Me.tsslAction.Size = New System.Drawing.Size(1221, 26)
        Me.tsslAction.Spring = True
        Me.tsslAction.Text = "-- IDLE --"
        Me.tsslAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(103, 26)
        Me.ToolStripStatusLabel2.Text = "Found files:"
        '
        'tsslFoundFiles
        '
        Me.tsslFoundFiles.Name = "tsslFoundFiles"
        Me.tsslFoundFiles.Size = New System.Drawing.Size(22, 26)
        Me.tsslFoundFiles.Text = "0"
        '
        'tspbMain
        '
        Me.tspbMain.Name = "tspbMain"
        Me.tspbMain.Size = New System.Drawing.Size(150, 25)
        Me.tspbMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'lbDisplayedFile
        '
        Me.lbDisplayedFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbDisplayedFile.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDisplayedFile.Location = New System.Drawing.Point(4, 0)
        Me.lbDisplayedFile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbDisplayedFile.Name = "lbDisplayedFile"
        Me.lbDisplayedFile.Size = New System.Drawing.Size(1549, 23)
        Me.lbDisplayedFile.TabIndex = 10
        Me.lbDisplayedFile.Text = "---"
        Me.lbDisplayedFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(18, 126)
        Me.scMain.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.lbResults)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.lbDisplayedFile)
        Me.scMain.Panel2.Controls.Add(Me.lbLinesFound)
        Me.scMain.Size = New System.Drawing.Size(1557, 957)
        Me.scMain.SplitterDistance = 505
        Me.scMain.SplitterWidth = 6
        Me.scMain.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 63)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "File filter"
        '
        'tbFileFilter
        '
        Me.tbFileFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFileFilter.Location = New System.Drawing.Point(108, 54)
        Me.tbFileFilter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbFileFilter.Name = "tbFileFilter"
        Me.tbFileFilter.Size = New System.Drawing.Size(1347, 26)
        Me.tbFileFilter.TabIndex = 3
        Me.tbFileFilter.Text = "*"
        '
        'tbViewer
        '
        Me.tbViewer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbViewer.Location = New System.Drawing.Point(105, 90)
        Me.tbViewer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbViewer.Name = "tbViewer"
        Me.tbViewer.Size = New System.Drawing.Size(1350, 26)
        Me.tbViewer.TabIndex = 12
        Me.tbViewer.Text = "C:\Program Files (x86)\FITSWork\Fitswork4.exe"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 93)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 20)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Viewer"
        '
        'btnFolderSelect
        '
        Me.btnFolderSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFolderSelect.Location = New System.Drawing.Point(1406, 18)
        Me.btnFolderSelect.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFolderSelect.Name = "btnFolderSelect"
        Me.btnFolderSelect.Size = New System.Drawing.Size(49, 27)
        Me.btnFolderSelect.TabIndex = 14
        Me.btnFolderSelect.Text = "..."
        Me.btnFolderSelect.UseVisualStyleBackColor = True
        '
        'cmsFileAction
        '
        Me.cmsFileAction.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cmsFileAction.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenWithFITSWorkToolStripMenuItem, Me.ToolStripMenuItem1, Me.RowAndColumsStatisticsToolStripMenuItem})
        Me.cmsFileAction.Name = "cmsFileAction"
        Me.cmsFileAction.Size = New System.Drawing.Size(347, 103)
        '
        'RowAndColumsStatisticsToolStripMenuItem
        '
        Me.RowAndColumsStatisticsToolStripMenuItem.Name = "RowAndColumsStatisticsToolStripMenuItem"
        Me.RowAndColumsStatisticsToolStripMenuItem.Size = New System.Drawing.Size(346, 30)
        Me.RowAndColumsStatisticsToolStripMenuItem.Text = "Row and colums statistics to CSV"
        '
        'OpenWithFITSWorkToolStripMenuItem
        '
        Me.OpenWithFITSWorkToolStripMenuItem.Name = "OpenWithFITSWorkToolStripMenuItem"
        Me.OpenWithFITSWorkToolStripMenuItem.Size = New System.Drawing.Size(346, 30)
        Me.OpenWithFITSWorkToolStripMenuItem.Text = "Open with FITSWork"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(343, 6)
        '
        'GrepForm
        '
        Me.AcceptButton = Me.btnSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1588, 1135)
        Me.Controls.Add(Me.btnFolderSelect)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbViewer)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbFileFilter)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbSearchIn)
        Me.Controls.Add(Me.btnSearch)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "GrepForm"
        Me.Text = "FITS Search"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.cmsFileAction.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents tbSearchIn As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbResults As System.Windows.Forms.ListBox
    Friend WithEvents lbLinesFound As System.Windows.Forms.ListBox
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsslAction As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsslFoundFiles As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lbDisplayedFile As System.Windows.Forms.Label
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents ttMain As System.Windows.Forms.ToolTip
    Friend WithEvents tspbMain As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents Label2 As Label
    Friend WithEvents tbFileFilter As TextBox
    Friend WithEvents tbViewer As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents fbdMain As FolderBrowserDialog
    Friend WithEvents btnFolderSelect As Button
    Friend WithEvents cmsFileAction As ContextMenuStrip
    Friend WithEvents RowAndColumsStatisticsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenWithFITSWorkToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
End Class
