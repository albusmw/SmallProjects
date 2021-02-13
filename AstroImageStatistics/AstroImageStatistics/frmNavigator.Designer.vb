<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNavigator
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tbRootFile = New System.Windows.Forms.TextBox()
        Me.tbFilterString = New System.Windows.Forms.TextBox()
        Me.tbOffsetX = New System.Windows.Forms.TextBox()
        Me.tbOffsetY = New System.Windows.Forms.TextBox()
        Me.tbTileSize = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pbMain = New System.Windows.Forms.ToolStripProgressBar()
        Me.cbColorModes = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbSelected = New System.Windows.Forms.TextBox()
        Me.bntAddRange = New System.Windows.Forms.Button()
        Me.sfdMain = New System.Windows.Forms.SaveFileDialog()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.tsmiFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_SaveMosaik = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFile_Exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSel_CheckAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSel_DeleteAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmi_CheckAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSel_UncheckAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.gbROI = New System.Windows.Forms.GroupBox()
        Me.clbFiles = New System.Windows.Forms.CheckedListBox()
        Me.scLow = New System.Windows.Forms.SplitContainer()
        Me.tbStatResult = New System.Windows.Forms.TextBox()
        Me.lbPixel = New System.Windows.Forms.ListBox()
        Me.ssMain.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.gbROI.SuspendLayout()
        CType(Me.scLow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scLow.Panel1.SuspendLayout()
        Me.scLow.Panel2.SuspendLayout()
        Me.scLow.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbRootFile
        '
        Me.tbRootFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRootFile.Location = New System.Drawing.Point(73, 83)
        Me.tbRootFile.Name = "tbRootFile"
        Me.tbRootFile.Size = New System.Drawing.Size(866, 20)
        Me.tbRootFile.TabIndex = 0
        '
        'tbFilterString
        '
        Me.tbFilterString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFilterString.Location = New System.Drawing.Point(73, 109)
        Me.tbFilterString.Name = "tbFilterString"
        Me.tbFilterString.Size = New System.Drawing.Size(866, 20)
        Me.tbFilterString.TabIndex = 1
        Me.tbFilterString.Text = "QHY600_L_*.fit*"
        '
        'tbOffsetX
        '
        Me.tbOffsetX.Location = New System.Drawing.Point(98, 19)
        Me.tbOffsetX.Name = "tbOffsetX"
        Me.tbOffsetX.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetX.TabIndex = 3
        Me.tbOffsetX.Text = "2000"
        Me.tbOffsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbOffsetY
        '
        Me.tbOffsetY.Location = New System.Drawing.Point(243, 19)
        Me.tbOffsetY.Name = "tbOffsetY"
        Me.tbOffsetY.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetY.TabIndex = 4
        Me.tbOffsetY.Text = "3000"
        Me.tbOffsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbTileSize
        '
        Me.tbTileSize.Location = New System.Drawing.Point(388, 19)
        Me.tbTileSize.Name = "tbTileSize"
        Me.tbTileSize.Size = New System.Drawing.Size(66, 20)
        Me.tbTileSize.TabIndex = 5
        Me.tbTileSize.Text = "100"
        Me.tbTileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Root file"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 112)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "File Filter"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "ROI Offset X"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(170, 22)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "ROI Offset Y"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(315, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "ROI Tile size"
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslStatus, Me.pbMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 591)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(1087, 22)
        Me.ssMain.TabIndex = 13
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslStatus
        '
        Me.tsslStatus.Name = "tsslStatus"
        Me.tsslStatus.Size = New System.Drawing.Size(22, 17)
        Me.tsslStatus.Text = "---"
        Me.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pbMain
        '
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(100, 16)
        '
        'cbColorModes
        '
        Me.cbColorModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColorModes.FormattingEnabled = True
        Me.cbColorModes.Items.AddRange(New Object() {"Gray", "Hot", "Jet", "Bone", "False-Color", "False-Color HSL"})
        Me.cbColorModes.Location = New System.Drawing.Point(460, 18)
        Me.cbColorModes.Name = "cbColorModes"
        Me.cbColorModes.Size = New System.Drawing.Size(144, 21)
        Me.cbColorModes.TabIndex = 14
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 138)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 13)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "Selected:"
        '
        'tbSelected
        '
        Me.tbSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSelected.Location = New System.Drawing.Point(73, 135)
        Me.tbSelected.Name = "tbSelected"
        Me.tbSelected.ReadOnly = True
        Me.tbSelected.Size = New System.Drawing.Size(866, 20)
        Me.tbSelected.TabIndex = 23
        '
        'bntAddRange
        '
        Me.bntAddRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bntAddRange.Location = New System.Drawing.Point(945, 83)
        Me.bntAddRange.Name = "bntAddRange"
        Me.bntAddRange.Size = New System.Drawing.Size(130, 46)
        Me.bntAddRange.TabIndex = 22
        Me.bntAddRange.Text = "Add range"
        Me.bntAddRange.UseVisualStyleBackColor = True
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile, Me.tsmiSel_CheckAll})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(1087, 24)
        Me.msMain.TabIndex = 18
        Me.msMain.Text = "MenuStrip1"
        '
        'tsmiFile
        '
        Me.tsmiFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile_SaveMosaik, Me.ToolStripMenuItem1, Me.tsmiFile_Exit})
        Me.tsmiFile.Name = "tsmiFile"
        Me.tsmiFile.Size = New System.Drawing.Size(37, 20)
        Me.tsmiFile.Text = "File"
        '
        'tsmiFile_SaveMosaik
        '
        Me.tsmiFile_SaveMosaik.Name = "tsmiFile_SaveMosaik"
        Me.tsmiFile_SaveMosaik.Size = New System.Drawing.Size(180, 22)
        Me.tsmiFile_SaveMosaik.Text = "Save mosaik"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'tsmiFile_Exit
        '
        Me.tsmiFile_Exit.Name = "tsmiFile_Exit"
        Me.tsmiFile_Exit.Size = New System.Drawing.Size(180, 22)
        Me.tsmiFile_Exit.Text = "Exit"
        '
        'tsmiSel_CheckAll
        '
        Me.tsmiSel_CheckAll.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiSel_DeleteAll, Me.tsmi_CheckAll, Me.tsmiSel_UncheckAll})
        Me.tsmiSel_CheckAll.Name = "tsmiSel_CheckAll"
        Me.tsmiSel_CheckAll.Size = New System.Drawing.Size(67, 20)
        Me.tsmiSel_CheckAll.Text = "Selection"
        '
        'tsmiSel_DeleteAll
        '
        Me.tsmiSel_DeleteAll.Name = "tsmiSel_DeleteAll"
        Me.tsmiSel_DeleteAll.Size = New System.Drawing.Size(180, 22)
        Me.tsmiSel_DeleteAll.Text = "Delete all files"
        '
        'tsmi_CheckAll
        '
        Me.tsmi_CheckAll.Name = "tsmi_CheckAll"
        Me.tsmi_CheckAll.Size = New System.Drawing.Size(180, 22)
        Me.tsmi_CheckAll.Text = "Check all"
        '
        'tsmiSel_UncheckAll
        '
        Me.tsmiSel_UncheckAll.Name = "tsmiSel_UncheckAll"
        Me.tsmiSel_UncheckAll.Size = New System.Drawing.Size(180, 22)
        Me.tsmiSel_UncheckAll.Text = "Uncheck all"
        '
        'gbROI
        '
        Me.gbROI.Controls.Add(Me.Label3)
        Me.gbROI.Controls.Add(Me.cbColorModes)
        Me.gbROI.Controls.Add(Me.tbOffsetY)
        Me.gbROI.Controls.Add(Me.Label5)
        Me.gbROI.Controls.Add(Me.tbTileSize)
        Me.gbROI.Controls.Add(Me.tbOffsetX)
        Me.gbROI.Controls.Add(Me.Label4)
        Me.gbROI.Location = New System.Drawing.Point(12, 27)
        Me.gbROI.Name = "gbROI"
        Me.gbROI.Size = New System.Drawing.Size(1123, 50)
        Me.gbROI.TabIndex = 19
        Me.gbROI.TabStop = False
        Me.gbROI.Text = "ROI"
        '
        'clbFiles
        '
        Me.clbFiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clbFiles.FormattingEnabled = True
        Me.clbFiles.IntegralHeight = False
        Me.clbFiles.Location = New System.Drawing.Point(73, 161)
        Me.clbFiles.Name = "clbFiles"
        Me.clbFiles.ScrollAlwaysVisible = True
        Me.clbFiles.Size = New System.Drawing.Size(1004, 125)
        Me.clbFiles.TabIndex = 26
        '
        'scLow
        '
        Me.scLow.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scLow.BackColor = System.Drawing.SystemColors.Control
        Me.scLow.Location = New System.Drawing.Point(73, 292)
        Me.scLow.Name = "scLow"
        Me.scLow.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scLow.Panel1
        '
        Me.scLow.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.scLow.Panel1.Controls.Add(Me.tbStatResult)
        '
        'scLow.Panel2
        '
        Me.scLow.Panel2.Controls.Add(Me.lbPixel)
        Me.scLow.Size = New System.Drawing.Size(1004, 279)
        Me.scLow.SplitterDistance = 196
        Me.scLow.TabIndex = 27
        '
        'tbStatResult
        '
        Me.tbStatResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbStatResult.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbStatResult.Location = New System.Drawing.Point(0, 0)
        Me.tbStatResult.Multiline = True
        Me.tbStatResult.Name = "tbStatResult"
        Me.tbStatResult.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbStatResult.Size = New System.Drawing.Size(1004, 196)
        Me.tbStatResult.TabIndex = 15
        '
        'lbPixel
        '
        Me.lbPixel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbPixel.FormattingEnabled = True
        Me.lbPixel.IntegralHeight = False
        Me.lbPixel.Location = New System.Drawing.Point(0, 0)
        Me.lbPixel.Name = "lbPixel"
        Me.lbPixel.ScrollAlwaysVisible = True
        Me.lbPixel.Size = New System.Drawing.Size(1004, 79)
        Me.lbPixel.TabIndex = 11
        '
        'frmNavigator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1087, 613)
        Me.Controls.Add(Me.scLow)
        Me.Controls.Add(Me.clbFiles)
        Me.Controls.Add(Me.gbROI)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbSelected)
        Me.Controls.Add(Me.bntAddRange)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.msMain)
        Me.Controls.Add(Me.tbRootFile)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbFilterString)
        Me.Controls.Add(Me.Label2)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.msMain
        Me.Name = "frmNavigator"
        Me.Text = "Navigator"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.gbROI.ResumeLayout(False)
        Me.gbROI.PerformLayout()
        Me.scLow.Panel1.ResumeLayout(False)
        Me.scLow.Panel1.PerformLayout()
        Me.scLow.Panel2.ResumeLayout(False)
        CType(Me.scLow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scLow.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbRootFile As TextBox
    Friend WithEvents tbFilterString As TextBox
    Friend WithEvents tbTileSize As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tsslStatus As ToolStripStatusLabel
    Friend WithEvents pbMain As ToolStripProgressBar
    Friend WithEvents cbColorModes As ComboBox
    Friend WithEvents bntAddRange As Button
    Friend WithEvents sfdMain As SaveFileDialog
    Friend WithEvents Label7 As Label
    Friend WithEvents tbSelected As TextBox
    Public WithEvents tbOffsetX As TextBox
    Public WithEvents tbOffsetY As TextBox
    Friend WithEvents msMain As MenuStrip
    Friend WithEvents tsmiFile As ToolStripMenuItem
    Friend WithEvents tsmiFile_SaveMosaik As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents tsmiFile_Exit As ToolStripMenuItem
    Friend WithEvents tsmiSel_CheckAll As ToolStripMenuItem
    Friend WithEvents tsmiSel_DeleteAll As ToolStripMenuItem
    Friend WithEvents gbROI As GroupBox
    Friend WithEvents tsmi_CheckAll As ToolStripMenuItem
    Friend WithEvents tsmiSel_UncheckAll As ToolStripMenuItem
    Friend WithEvents clbFiles As CheckedListBox
    Friend WithEvents scLow As SplitContainer
    Friend WithEvents tbStatResult As TextBox
    Friend WithEvents lbPixel As ListBox
End Class
