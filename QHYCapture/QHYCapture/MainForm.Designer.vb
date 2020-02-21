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
        Me.components = New System.ComponentModel.Container()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExplorerHereToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CaptureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunCaptureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.zgcMain = New ZedGraph.ZedGraphControl()
        Me.ssMain.SuspendLayout()
        Me.msMain.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(12, 27)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(281, 468)
        Me.pgMain.TabIndex = 0
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 507)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(790, 22)
        Me.ssMain.TabIndex = 1
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(66, 17)
        Me.tsslMain.Text = "--- IDLE ---"
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.CaptureToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(790, 24)
        Me.msMain.TabIndex = 2
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExplorerHereToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ExplorerHereToolStripMenuItem
        '
        Me.ExplorerHereToolStripMenuItem.Name = "ExplorerHereToolStripMenuItem"
        Me.ExplorerHereToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.ExplorerHereToolStripMenuItem.Text = "Explorer here"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(140, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'CaptureToolStripMenuItem
        '
        Me.CaptureToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunCaptureToolStripMenuItem})
        Me.CaptureToolStripMenuItem.Name = "CaptureToolStripMenuItem"
        Me.CaptureToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.CaptureToolStripMenuItem.Text = "Capture"
        '
        'RunCaptureToolStripMenuItem
        '
        Me.RunCaptureToolStripMenuItem.Name = "RunCaptureToolStripMenuItem"
        Me.RunCaptureToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.RunCaptureToolStripMenuItem.Text = "Run capture"
        '
        'tbLogOutput
        '
        Me.tbLogOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLogOutput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLogOutput.Location = New System.Drawing.Point(3, 3)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(473, 229)
        Me.tbLogOutput.TabIndex = 3
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(299, 27)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.zgcMain)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.tbLogOutput)
        Me.scMain.Size = New System.Drawing.Size(479, 468)
        Me.scMain.SplitterDistance = 229
        Me.scMain.TabIndex = 4
        '
        'zgcMain
        '
        Me.zgcMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.zgcMain.Location = New System.Drawing.Point(3, 3)
        Me.zgcMain.Name = "zgcMain"
        Me.zgcMain.ScrollGrace = 0R
        Me.zgcMain.ScrollMaxX = 0R
        Me.zgcMain.ScrollMaxY = 0R
        Me.zgcMain.ScrollMaxY2 = 0R
        Me.zgcMain.ScrollMinX = 0R
        Me.zgcMain.ScrollMinY = 0R
        Me.zgcMain.ScrollMinY2 = 0R
        Me.zgcMain.Size = New System.Drawing.Size(473, 223)
        Me.zgcMain.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(790, 529)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.msMain)
        Me.Controls.Add(Me.pgMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.Panel2.PerformLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pgMain As Windows.Forms.PropertyGrid
    Friend WithEvents ssMain As Windows.Forms.StatusStrip
    Friend WithEvents msMain As Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExplorerHereToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaptureToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunCaptureToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsslMain As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tbLogOutput As Windows.Forms.TextBox
    Friend WithEvents scMain As Windows.Forms.SplitContainer
    Friend WithEvents zgcMain As ZedGraph.ZedGraphControl
End Class
