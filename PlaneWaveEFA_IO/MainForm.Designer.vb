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
        Me.components = New System.ComponentModel.Container()
        Me.lbLog = New System.Windows.Forms.ListBox()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.scLeft = New System.Windows.Forms.SplitContainer()
        Me.pgValues = New System.Windows.Forms.PropertyGrid()
        Me.tUpdater = New System.Windows.Forms.Timer(Me.components)
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.tsmiFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_OpenEXE = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFile_Exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_WCF = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRun = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRun_SingleQuery = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_CopyLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.FocuserMoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.scLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scLeft.Panel1.SuspendLayout()
        Me.scLeft.Panel2.SuspendLayout()
        Me.scLeft.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbLog
        '
        Me.lbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbLog.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbLog.FormattingEnabled = True
        Me.lbLog.IntegralHeight = False
        Me.lbLog.ItemHeight = 14
        Me.lbLog.Location = New System.Drawing.Point(271, 27)
        Me.lbLog.Name = "lbLog"
        Me.lbLog.ScrollAlwaysVisible = True
        Me.lbLog.Size = New System.Drawing.Size(433, 501)
        Me.lbLog.TabIndex = 1
        '
        'pgMain
        '
        Me.pgMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMain.Location = New System.Drawing.Point(0, 0)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(253, 246)
        Me.pgMain.TabIndex = 3
        '
        'scLeft
        '
        Me.scLeft.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.scLeft.Location = New System.Drawing.Point(12, 27)
        Me.scLeft.Name = "scLeft"
        Me.scLeft.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scLeft.Panel1
        '
        Me.scLeft.Panel1.Controls.Add(Me.pgMain)
        '
        'scLeft.Panel2
        '
        Me.scLeft.Panel2.Controls.Add(Me.pgValues)
        Me.scLeft.Size = New System.Drawing.Size(253, 501)
        Me.scLeft.SplitterDistance = 246
        Me.scLeft.TabIndex = 4
        '
        'pgValues
        '
        Me.pgValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgValues.Location = New System.Drawing.Point(0, 0)
        Me.pgValues.Name = "pgValues"
        Me.pgValues.Size = New System.Drawing.Size(253, 251)
        Me.pgValues.TabIndex = 4
        '
        'tUpdater
        '
        Me.tUpdater.Enabled = True
        Me.tUpdater.Interval = 1000
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile, Me.tsmiRun})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(716, 24)
        Me.msMain.TabIndex = 5
        Me.msMain.Text = "MenuStrip1"
        '
        'tsmiFile
        '
        Me.tsmiFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile_OpenEXE, Me.ToolStripMenuItem1, Me.tsmiFile_Exit, Me.tsmiFile_WCF, Me.tsmiFile_CopyLog})
        Me.tsmiFile.Name = "tsmiFile"
        Me.tsmiFile.Size = New System.Drawing.Size(37, 20)
        Me.tsmiFile.Text = "File"
        '
        'tsmiFile_OpenEXE
        '
        Me.tsmiFile_OpenEXE.Name = "tsmiFile_OpenEXE"
        Me.tsmiFile_OpenEXE.Size = New System.Drawing.Size(189, 22)
        Me.tsmiFile_OpenEXE.Text = "Open EXE path"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(186, 6)
        '
        'tsmiFile_Exit
        '
        Me.tsmiFile_Exit.Name = "tsmiFile_Exit"
        Me.tsmiFile_Exit.Size = New System.Drawing.Size(189, 22)
        Me.tsmiFile_Exit.Text = "Exit"
        '
        'tsmiFile_WCF
        '
        Me.tsmiFile_WCF.Name = "tsmiFile_WCF"
        Me.tsmiFile_WCF.Size = New System.Drawing.Size(189, 22)
        Me.tsmiFile_WCF.Text = "Test WCF interface"
        '
        'tsmiRun
        '
        Me.tsmiRun.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiRun_SingleQuery, Me.FocuserMoveToolStripMenuItem})
        Me.tsmiRun.Name = "tsmiRun"
        Me.tsmiRun.Size = New System.Drawing.Size(40, 20)
        Me.tsmiRun.Text = "Run"
        '
        'tsmiRun_SingleQuery
        '
        Me.tsmiRun_SingleQuery.Name = "tsmiRun_SingleQuery"
        Me.tsmiRun_SingleQuery.Size = New System.Drawing.Size(180, 22)
        Me.tsmiRun_SingleQuery.Text = "Single status query"
        '
        'tsmiFile_CopyLog
        '
        Me.tsmiFile_CopyLog.Name = "tsmiFile_CopyLog"
        Me.tsmiFile_CopyLog.Size = New System.Drawing.Size(189, 22)
        Me.tsmiFile_CopyLog.Text = "Copy log to clipboard"
        '
        'FocuserMoveToolStripMenuItem
        '
        Me.FocuserMoveToolStripMenuItem.Name = "FocuserMoveToolStripMenuItem"
        Me.FocuserMoveToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.FocuserMoveToolStripMenuItem.Text = "Focuser move"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(716, 540)
        Me.Controls.Add(Me.scLeft)
        Me.Controls.Add(Me.lbLog)
        Me.Controls.Add(Me.msMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "PW EFA COM Test Version 1.1"
        Me.scLeft.Panel1.ResumeLayout(False)
        Me.scLeft.Panel2.ResumeLayout(False)
        CType(Me.scLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scLeft.ResumeLayout(False)
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents scLeft As SplitContainer
    Friend WithEvents pgValues As PropertyGrid
    Friend WithEvents tUpdater As Timer
    Friend WithEvents msMain As MenuStrip
    Friend WithEvents tsmiFile As ToolStripMenuItem
    Friend WithEvents tsmiFile_OpenEXE As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents tsmiFile_Exit As ToolStripMenuItem
    Friend WithEvents tsmiFile_WCF As ToolStripMenuItem
    Friend WithEvents tsmiRun As ToolStripMenuItem
    Friend WithEvents tsmiRun_SingleQuery As ToolStripMenuItem
    Friend WithEvents tsmiFile_CopyLog As ToolStripMenuItem
    Friend WithEvents FocuserMoveToolStripMenuItem As ToolStripMenuItem
End Class
