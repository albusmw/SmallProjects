<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
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
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.gbFiles = New System.Windows.Forms.GroupBox()
        Me.lbInputFiles = New System.Windows.Forms.ListBox()
        Me.scTop = New System.Windows.Forms.SplitContainer()
        Me.gbHeader = New System.Windows.Forms.GroupBox()
        Me.cbSort = New System.Windows.Forms.CheckBox()
        Me.lbHeader = New System.Windows.Forms.ListBox()
        Me.gbLog = New System.Windows.Forms.GroupBox()
        Me.dgvMain = New System.Windows.Forms.DataGridView()
        Me.Element = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Prop = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Edit = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Source = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.scBottom = New System.Windows.Forms.SplitContainer()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiAddDirectory = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ClearListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadListOfFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveListOfFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.sfdMain = New System.Windows.Forms.SaveFileDialog()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.fbdMain = New System.Windows.Forms.FolderBrowserDialog()
        Me.gbFiles.SuspendLayout()
        Me.scTop.Panel1.SuspendLayout()
        Me.scTop.Panel2.SuspendLayout()
        Me.scTop.SuspendLayout()
        Me.gbHeader.SuspendLayout()
        Me.gbLog.SuspendLayout()
        CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scBottom.Panel1.SuspendLayout()
        Me.scBottom.Panel2.SuspendLayout()
        Me.scBottom.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Location = New System.Drawing.Point(4, 3)
        Me.btnStart.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(1119, 51)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLog.Location = New System.Drawing.Point(9, 29)
        Me.tbLog.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(1094, 301)
        Me.tbLog.TabIndex = 1
        Me.tbLog.WordWrap = False
        '
        'gbFiles
        '
        Me.gbFiles.Controls.Add(Me.lbInputFiles)
        Me.gbFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbFiles.Location = New System.Drawing.Point(0, 0)
        Me.gbFiles.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbFiles.Name = "gbFiles"
        Me.gbFiles.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbFiles.Size = New System.Drawing.Size(912, 715)
        Me.gbFiles.TabIndex = 2
        Me.gbFiles.TabStop = False
        Me.gbFiles.Text = "Files to process (drop here ...)"
        '
        'lbInputFiles
        '
        Me.lbInputFiles.AllowDrop = True
        Me.lbInputFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbInputFiles.FormattingEnabled = True
        Me.lbInputFiles.IntegralHeight = False
        Me.lbInputFiles.ItemHeight = 20
        Me.lbInputFiles.Location = New System.Drawing.Point(9, 29)
        Me.lbInputFiles.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lbInputFiles.Name = "lbInputFiles"
        Me.lbInputFiles.ScrollAlwaysVisible = True
        Me.lbInputFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lbInputFiles.Size = New System.Drawing.Size(892, 670)
        Me.lbInputFiles.TabIndex = 0
        '
        'scTop
        '
        Me.scTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scTop.Location = New System.Drawing.Point(18, 42)
        Me.scTop.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.scTop.Name = "scTop"
        '
        'scTop.Panel1
        '
        Me.scTop.Panel1.Controls.Add(Me.gbFiles)
        '
        'scTop.Panel2
        '
        Me.scTop.Panel2.Controls.Add(Me.gbHeader)
        Me.scTop.Size = New System.Drawing.Size(1698, 715)
        Me.scTop.SplitterDistance = 912
        Me.scTop.SplitterWidth = 6
        Me.scTop.TabIndex = 3
        '
        'gbHeader
        '
        Me.gbHeader.Controls.Add(Me.cbSort)
        Me.gbHeader.Controls.Add(Me.lbHeader)
        Me.gbHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbHeader.Location = New System.Drawing.Point(0, 0)
        Me.gbHeader.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbHeader.Name = "gbHeader"
        Me.gbHeader.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbHeader.Size = New System.Drawing.Size(780, 715)
        Me.gbHeader.TabIndex = 0
        Me.gbHeader.TabStop = False
        Me.gbHeader.Text = "Header of file"
        '
        'cbSort
        '
        Me.cbSort.AutoSize = True
        Me.cbSort.Location = New System.Drawing.Point(9, 29)
        Me.cbSort.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbSort.Name = "cbSort"
        Me.cbSort.Size = New System.Drawing.Size(65, 24)
        Me.cbSort.TabIndex = 2
        Me.cbSort.Text = "Sort"
        Me.cbSort.UseVisualStyleBackColor = True
        '
        'lbHeader
        '
        Me.lbHeader.AllowDrop = True
        Me.lbHeader.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbHeader.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbHeader.FormattingEnabled = True
        Me.lbHeader.IntegralHeight = False
        Me.lbHeader.ItemHeight = 20
        Me.lbHeader.Location = New System.Drawing.Point(9, 60)
        Me.lbHeader.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lbHeader.Name = "lbHeader"
        Me.lbHeader.ScrollAlwaysVisible = True
        Me.lbHeader.Size = New System.Drawing.Size(760, 639)
        Me.lbHeader.TabIndex = 1
        '
        'gbLog
        '
        Me.gbLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbLog.Controls.Add(Me.tbLog)
        Me.gbLog.Location = New System.Drawing.Point(4, 63)
        Me.gbLog.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbLog.Name = "gbLog"
        Me.gbLog.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbLog.Size = New System.Drawing.Size(1114, 342)
        Me.gbLog.TabIndex = 4
        Me.gbLog.TabStop = False
        Me.gbLog.Text = "Log output"
        '
        'dgvMain
        '
        Me.dgvMain.AllowUserToAddRows = False
        Me.dgvMain.AllowUserToDeleteRows = False
        Me.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMain.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Element, Me.Prop, Me.Edit, Me.Source})
        Me.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMain.EnableHeadersVisualStyles = False
        Me.dgvMain.Location = New System.Drawing.Point(0, 0)
        Me.dgvMain.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvMain.Name = "dgvMain"
        Me.dgvMain.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dgvMain.RowHeadersVisible = False
        Me.dgvMain.Size = New System.Drawing.Size(565, 409)
        Me.dgvMain.TabIndex = 5
        '
        'Element
        '
        Me.Element.HeaderText = "Header element"
        Me.Element.Name = "Element"
        Me.Element.ReadOnly = True
        '
        'Prop
        '
        Me.Prop.HeaderText = "Property"
        Me.Prop.Name = "Prop"
        Me.Prop.ReadOnly = True
        '
        'Edit
        '
        Me.Edit.HeaderText = "Edit mode"
        Me.Edit.Items.AddRange(New Object() {"Don't touch", "Make all ...", "List from ...", "Dictionary from ..."})
        Me.Edit.Name = "Edit"
        '
        'Source
        '
        Me.Source.HeaderText = "Edit source"
        Me.Source.Name = "Source"
        '
        'scBottom
        '
        Me.scBottom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scBottom.Location = New System.Drawing.Point(18, 768)
        Me.scBottom.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.scBottom.Name = "scBottom"
        '
        'scBottom.Panel1
        '
        Me.scBottom.Panel1.Controls.Add(Me.dgvMain)
        '
        'scBottom.Panel2
        '
        Me.scBottom.Panel2.Controls.Add(Me.btnStart)
        Me.scBottom.Panel2.Controls.Add(Me.gbLog)
        Me.scBottom.Size = New System.Drawing.Size(1698, 409)
        Me.scBottom.SplitterDistance = 565
        Me.scBottom.SplitterWidth = 6
        Me.scBottom.TabIndex = 6
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(9, 3, 0, 3)
        Me.MenuStrip1.Size = New System.Drawing.Size(1734, 35)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiAddDirectory, Me.ToolStripMenuItem2, Me.ClearListToolStripMenuItem, Me.LoadListOfFilesToolStripMenuItem, Me.SaveListOfFilesToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(50, 29)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'tsmiAddDirectory
        '
        Me.tsmiAddDirectory.Name = "tsmiAddDirectory"
        Me.tsmiAddDirectory.Size = New System.Drawing.Size(252, 30)
        Me.tsmiAddDirectory.Text = "Add directory"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(249, 6)
        '
        'ClearListToolStripMenuItem
        '
        Me.ClearListToolStripMenuItem.Name = "ClearListToolStripMenuItem"
        Me.ClearListToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.ClearListToolStripMenuItem.Text = "Clear list"
        '
        'LoadListOfFilesToolStripMenuItem
        '
        Me.LoadListOfFilesToolStripMenuItem.Name = "LoadListOfFilesToolStripMenuItem"
        Me.LoadListOfFilesToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.LoadListOfFilesToolStripMenuItem.Text = "Load list of files"
        '
        'SaveListOfFilesToolStripMenuItem
        '
        Me.SaveListOfFilesToolStripMenuItem.Name = "SaveListOfFilesToolStripMenuItem"
        Me.SaveListOfFilesToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.SaveListOfFilesToolStripMenuItem.Text = "Save list of files"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(249, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ofdMain
        '
        Me.ofdMain.FileName = "OpenFileDialog1"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1734, 1195)
        Me.Controls.Add(Me.scBottom)
        Me.Controls.Add(Me.scTop)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "MainForm"
        Me.Text = "FITS Header Changer"
        Me.gbFiles.ResumeLayout(False)
        Me.scTop.Panel1.ResumeLayout(False)
        Me.scTop.Panel2.ResumeLayout(False)
        Me.scTop.ResumeLayout(False)
        Me.gbHeader.ResumeLayout(False)
        Me.gbHeader.PerformLayout()
        Me.gbLog.ResumeLayout(False)
        Me.gbLog.PerformLayout()
        CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scBottom.Panel1.ResumeLayout(False)
        Me.scBottom.Panel2.ResumeLayout(False)
        Me.scBottom.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents tbLog As System.Windows.Forms.TextBox
    Friend WithEvents gbFiles As System.Windows.Forms.GroupBox
    Friend WithEvents lbInputFiles As System.Windows.Forms.ListBox
    Friend WithEvents scTop As System.Windows.Forms.SplitContainer
    Friend WithEvents gbHeader As System.Windows.Forms.GroupBox
    Friend WithEvents lbHeader As System.Windows.Forms.ListBox
    Friend WithEvents cbSort As System.Windows.Forms.CheckBox
    Friend WithEvents gbLog As System.Windows.Forms.GroupBox
    Friend WithEvents dgvMain As System.Windows.Forms.DataGridView
    Friend WithEvents Element As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Prop As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Edit As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Source As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents scBottom As System.Windows.Forms.SplitContainer
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoadListOfFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveListOfFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents sfdMain As SaveFileDialog
    Friend WithEvents ClearListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents tsmiAddDirectory As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents fbdMain As FolderBrowserDialog
End Class
