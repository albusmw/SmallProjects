<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileToAnalyseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.WriteTestDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestCodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveOverscanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenEXELocationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.TestCodeToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip1.Size = New System.Drawing.Size(533, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenFileToAnalyseToolStripMenuItem, Me.WriteTestDataToolStripMenuItem, Me.OpenEXELocationToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 22)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenFileToAnalyseToolStripMenuItem
        '
        Me.OpenFileToAnalyseToolStripMenuItem.Name = "OpenFileToAnalyseToolStripMenuItem"
        Me.OpenFileToAnalyseToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OpenFileToAnalyseToolStripMenuItem.Text = "Open file to analyse"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'WriteTestDataToolStripMenuItem
        '
        Me.WriteTestDataToolStripMenuItem.Name = "WriteTestDataToolStripMenuItem"
        Me.WriteTestDataToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.WriteTestDataToolStripMenuItem.Text = "Write test data"
        '
        'TestCodeToolStripMenuItem
        '
        Me.TestCodeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveOverscanToolStripMenuItem})
        Me.TestCodeToolStripMenuItem.Name = "TestCodeToolStripMenuItem"
        Me.TestCodeToolStripMenuItem.Size = New System.Drawing.Size(68, 22)
        Me.TestCodeToolStripMenuItem.Text = "Test code"
        '
        'RemoveOverscanToolStripMenuItem
        '
        Me.RemoveOverscanToolStripMenuItem.Name = "RemoveOverscanToolStripMenuItem"
        Me.RemoveOverscanToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.RemoveOverscanToolStripMenuItem.Text = "Remove overscan"
        '
        'tbLogOutput
        '
        Me.tbLogOutput.AllowDrop = True
        Me.tbLogOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLogOutput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLogOutput.Location = New System.Drawing.Point(9, 25)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(517, 245)
        Me.tbLogOutput.TabIndex = 3
        Me.tbLogOutput.WordWrap = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslMain})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 270)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 9, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(533, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(22, 17)
        Me.tsslMain.Text = "---"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'OpenEXELocationToolStripMenuItem
        '
        Me.OpenEXELocationToolStripMenuItem.Name = "OpenEXELocationToolStripMenuItem"
        Me.OpenEXELocationToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OpenEXELocationToolStripMenuItem.Text = "Open EXE location"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(533, 292)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.tbLogOutput)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Astro Image Statistics"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenFileToAnalyseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents tbLogOutput As TextBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents tsslMain As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents WriteTestDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TestCodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveOverscanToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenEXELocationToolStripMenuItem As ToolStripMenuItem
End Class
