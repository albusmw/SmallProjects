<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FITSGrepToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DebugToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CompressionTestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.cMain = New System.Windows.Forms.SplitContainer()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.ByerPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.msMain.SuspendLayout()
        CType(Me.cMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cMain.Panel1.SuspendLayout()
        Me.cMain.Panel2.SuspendLayout()
        Me.cMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'msMain
        '
        Me.msMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.DebugToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(1343, 33)
        Me.msMain.TabIndex = 0
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.FITSGrepToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(50, 29)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'FITSGrepToolStripMenuItem
        '
        Me.FITSGrepToolStripMenuItem.Name = "FITSGrepToolStripMenuItem"
        Me.FITSGrepToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FITSGrepToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.FITSGrepToolStripMenuItem.Text = "FITS Grep"
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
        'DebugToolStripMenuItem
        '
        Me.DebugToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CompressionTestToolStripMenuItem, Me.ByerPreviewToolStripMenuItem})
        Me.DebugToolStripMenuItem.Name = "DebugToolStripMenuItem"
        Me.DebugToolStripMenuItem.Size = New System.Drawing.Size(78, 29)
        Me.DebugToolStripMenuItem.Text = "Debug"
        '
        'CompressionTestToolStripMenuItem
        '
        Me.CompressionTestToolStripMenuItem.Name = "CompressionTestToolStripMenuItem"
        Me.CompressionTestToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.CompressionTestToolStripMenuItem.Text = "Compression test"
        '
        'cMain
        '
        Me.cMain.BackColor = System.Drawing.Color.Gray
        Me.cMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cMain.Location = New System.Drawing.Point(0, 33)
        Me.cMain.Name = "cMain"
        '
        'cMain.Panel1
        '
        Me.cMain.Panel1.Controls.Add(Me.pgMain)
        '
        'cMain.Panel2
        '
        Me.cMain.Panel2.Controls.Add(Me.tbLog)
        Me.cMain.Size = New System.Drawing.Size(1343, 871)
        Me.cMain.SplitterDistance = 463
        Me.cMain.TabIndex = 1
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(3, 3)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(457, 865)
        Me.pgMain.TabIndex = 0
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Font = New System.Drawing.Font("Courier New", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLog.Location = New System.Drawing.Point(0, 3)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(873, 865)
        Me.tbLog.TabIndex = 0
        Me.tbLog.WordWrap = False
        '
        'ByerPreviewToolStripMenuItem
        '
        Me.ByerPreviewToolStripMenuItem.Name = "ByerPreviewToolStripMenuItem"
        Me.ByerPreviewToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.ByerPreviewToolStripMenuItem.Text = "Bayer preview"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1343, 904)
        Me.Controls.Add(Me.cMain)
        Me.Controls.Add(Me.msMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "AstroFlow"
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.cMain.Panel1.ResumeLayout(False)
        Me.cMain.Panel2.ResumeLayout(False)
        Me.cMain.Panel2.PerformLayout()
        CType(Me.cMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents msMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents cMain As SplitContainer
    Friend WithEvents tbLog As TextBox
    Friend WithEvents FITSGrepToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents DebugToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CompressionTestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ByerPreviewToolStripMenuItem As ToolStripMenuItem
End Class
