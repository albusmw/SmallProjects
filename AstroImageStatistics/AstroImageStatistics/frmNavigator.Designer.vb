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
        Me.lbPixel = New System.Windows.Forms.ListBox()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pbMain = New System.Windows.Forms.ToolStripProgressBar()
        Me.cbColorModes = New System.Windows.Forms.ComboBox()
        Me.tbStatResult = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.btnSaveMosaik = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbSelected = New System.Windows.Forms.TextBox()
        Me.bntAddRange = New System.Windows.Forms.Button()
        Me.btnDeleteAll = New System.Windows.Forms.Button()
        Me.btnUncheckAll = New System.Windows.Forms.Button()
        Me.btnCheckAll = New System.Windows.Forms.Button()
        Me.clbFiles = New System.Windows.Forms.CheckedListBox()
        Me.sfdMain = New System.Windows.Forms.SaveFileDialog()
        Me.ssMain.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbRootFile
        '
        Me.tbRootFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRootFile.Location = New System.Drawing.Point(57, 4)
        Me.tbRootFile.Name = "tbRootFile"
        Me.tbRootFile.Size = New System.Drawing.Size(588, 20)
        Me.tbRootFile.TabIndex = 0
        '
        'tbFilterString
        '
        Me.tbFilterString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFilterString.Location = New System.Drawing.Point(57, 30)
        Me.tbFilterString.Name = "tbFilterString"
        Me.tbFilterString.Size = New System.Drawing.Size(588, 20)
        Me.tbFilterString.TabIndex = 1
        Me.tbFilterString.Text = "QHY600_L_*.fit*"
        '
        'tbOffsetX
        '
        Me.tbOffsetX.Location = New System.Drawing.Point(152, 8)
        Me.tbOffsetX.Name = "tbOffsetX"
        Me.tbOffsetX.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetX.TabIndex = 3
        Me.tbOffsetX.Text = "2000"
        Me.tbOffsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbOffsetY
        '
        Me.tbOffsetY.Location = New System.Drawing.Point(152, 34)
        Me.tbOffsetY.Name = "tbOffsetY"
        Me.tbOffsetY.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetY.TabIndex = 4
        Me.tbOffsetY.Text = "3000"
        Me.tbOffsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbTileSize
        '
        Me.tbTileSize.Location = New System.Drawing.Point(152, 60)
        Me.tbTileSize.Name = "tbTileSize"
        Me.tbTileSize.Size = New System.Drawing.Size(66, 20)
        Me.tbTileSize.TabIndex = 5
        Me.tbTileSize.Text = "100"
        Me.tbTileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Root file"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "File Filter"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "ROI Offset X"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "ROI Offset Y"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "ROI Tile size"
        '
        'lbPixel
        '
        Me.lbPixel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbPixel.FormattingEnabled = True
        Me.lbPixel.IntegralHeight = False
        Me.lbPixel.Location = New System.Drawing.Point(76, 721)
        Me.lbPixel.Name = "lbPixel"
        Me.lbPixel.ScrollAlwaysVisible = True
        Me.lbPixel.Size = New System.Drawing.Size(1120, 298)
        Me.lbPixel.TabIndex = 11
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslStatus, Me.pbMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 1039)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(1208, 22)
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
        Me.cbColorModes.Location = New System.Drawing.Point(15, 86)
        Me.cbColorModes.Name = "cbColorModes"
        Me.cbColorModes.Size = New System.Drawing.Size(203, 21)
        Me.cbColorModes.TabIndex = 14
        '
        'tbStatResult
        '
        Me.tbStatResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbStatResult.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbStatResult.Location = New System.Drawing.Point(76, 474)
        Me.tbStatResult.Multiline = True
        Me.tbStatResult.Name = "tbStatResult"
        Me.tbStatResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.tbStatResult.Size = New System.Drawing.Size(1120, 241)
        Me.tbStatResult.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 477)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Statistics"
        '
        'scMain
        '
        Me.scMain.Location = New System.Drawing.Point(15, 12)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.btnSaveMosaik)
        Me.scMain.Panel1.Controls.Add(Me.tbOffsetX)
        Me.scMain.Panel1.Controls.Add(Me.tbOffsetY)
        Me.scMain.Panel1.Controls.Add(Me.Label3)
        Me.scMain.Panel1.Controls.Add(Me.cbColorModes)
        Me.scMain.Panel1.Controls.Add(Me.Label4)
        Me.scMain.Panel1.Controls.Add(Me.tbTileSize)
        Me.scMain.Panel1.Controls.Add(Me.Label5)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.Label7)
        Me.scMain.Panel2.Controls.Add(Me.tbSelected)
        Me.scMain.Panel2.Controls.Add(Me.bntAddRange)
        Me.scMain.Panel2.Controls.Add(Me.btnDeleteAll)
        Me.scMain.Panel2.Controls.Add(Me.btnUncheckAll)
        Me.scMain.Panel2.Controls.Add(Me.btnCheckAll)
        Me.scMain.Panel2.Controls.Add(Me.clbFiles)
        Me.scMain.Panel2.Controls.Add(Me.Label1)
        Me.scMain.Panel2.Controls.Add(Me.tbRootFile)
        Me.scMain.Panel2.Controls.Add(Me.tbFilterString)
        Me.scMain.Panel2.Controls.Add(Me.Label2)
        Me.scMain.Size = New System.Drawing.Size(1181, 456)
        Me.scMain.SplitterDistance = 393
        Me.scMain.TabIndex = 17
        '
        'btnSaveMosaik
        '
        Me.btnSaveMosaik.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveMosaik.Location = New System.Drawing.Point(302, 418)
        Me.btnSaveMosaik.Name = "btnSaveMosaik"
        Me.btnSaveMosaik.Size = New System.Drawing.Size(88, 35)
        Me.btnSaveMosaik.TabIndex = 15
        Me.btnSaveMosaik.Text = "Save mosaik"
        Me.btnSaveMosaik.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(5, 407)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 13)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "Selected:"
        '
        'tbSelected
        '
        Me.tbSelected.Location = New System.Drawing.Point(90, 404)
        Me.tbSelected.Name = "tbSelected"
        Me.tbSelected.ReadOnly = True
        Me.tbSelected.Size = New System.Drawing.Size(691, 20)
        Me.tbSelected.TabIndex = 23
        '
        'bntAddRange
        '
        Me.bntAddRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bntAddRange.Location = New System.Drawing.Point(651, 4)
        Me.bntAddRange.Name = "bntAddRange"
        Me.bntAddRange.Size = New System.Drawing.Size(130, 46)
        Me.bntAddRange.TabIndex = 22
        Me.bntAddRange.Text = "Add range"
        Me.bntAddRange.UseVisualStyleBackColor = True
        '
        'btnDeleteAll
        '
        Me.btnDeleteAll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteAll.Location = New System.Drawing.Point(90, 430)
        Me.btnDeleteAll.Name = "btnDeleteAll"
        Me.btnDeleteAll.Size = New System.Drawing.Size(604, 23)
        Me.btnDeleteAll.TabIndex = 21
        Me.btnDeleteAll.Text = "Delete all"
        Me.btnDeleteAll.UseVisualStyleBackColor = True
        '
        'btnUncheckAll
        '
        Me.btnUncheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUncheckAll.Location = New System.Drawing.Point(700, 430)
        Me.btnUncheckAll.Name = "btnUncheckAll"
        Me.btnUncheckAll.Size = New System.Drawing.Size(81, 23)
        Me.btnUncheckAll.TabIndex = 20
        Me.btnUncheckAll.Text = "Uncheck all"
        Me.btnUncheckAll.UseVisualStyleBackColor = True
        '
        'btnCheckAll
        '
        Me.btnCheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCheckAll.Location = New System.Drawing.Point(3, 430)
        Me.btnCheckAll.Name = "btnCheckAll"
        Me.btnCheckAll.Size = New System.Drawing.Size(81, 23)
        Me.btnCheckAll.TabIndex = 19
        Me.btnCheckAll.Text = "Check all"
        Me.btnCheckAll.UseVisualStyleBackColor = True
        '
        'clbFiles
        '
        Me.clbFiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clbFiles.FormattingEnabled = True
        Me.clbFiles.IntegralHeight = False
        Me.clbFiles.Location = New System.Drawing.Point(3, 56)
        Me.clbFiles.Name = "clbFiles"
        Me.clbFiles.ScrollAlwaysVisible = True
        Me.clbFiles.Size = New System.Drawing.Size(778, 342)
        Me.clbFiles.TabIndex = 18
        '
        'frmNavigator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1208, 1061)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbStatResult)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.lbPixel)
        Me.KeyPreview = True
        Me.Name = "frmNavigator"
        Me.Text = "Navigator"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel1.PerformLayout()
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.Panel2.PerformLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbRootFile As TextBox
    Friend WithEvents tbFilterString As TextBox
    Friend WithEvents tbOffsetX As TextBox
    Friend WithEvents tbOffsetY As TextBox
    Friend WithEvents tbTileSize As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lbPixel As ListBox
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tsslStatus As ToolStripStatusLabel
    Friend WithEvents pbMain As ToolStripProgressBar
    Friend WithEvents cbColorModes As ComboBox
    Friend WithEvents tbStatResult As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents clbFiles As CheckedListBox
    Friend WithEvents btnDeleteAll As Button
    Friend WithEvents btnUncheckAll As Button
    Friend WithEvents btnCheckAll As Button
    Friend WithEvents bntAddRange As Button
    Friend WithEvents btnSaveMosaik As Button
    Friend WithEvents sfdMain As SaveFileDialog
    Friend WithEvents Label7 As Label
    Friend WithEvents tbSelected As TextBox
End Class
