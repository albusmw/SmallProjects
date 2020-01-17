<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.btnStart = New System.Windows.Forms.Button()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectFocuserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectCameraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenEXEPathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.msMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Location = New System.Drawing.Point(647, 383)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(141, 55)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'msMain
        '
        Me.msMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(800, 33)
        Me.msMain.TabIndex = 1
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectFocuserToolStripMenuItem, Me.SelectCameraToolStripMenuItem, Me.ToolStripMenuItem1, Me.OpenEXEPathToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(50, 29)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'SelectFocuserToolStripMenuItem
        '
        Me.SelectFocuserToolStripMenuItem.Name = "SelectFocuserToolStripMenuItem"
        Me.SelectFocuserToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.SelectFocuserToolStripMenuItem.Text = "Select focuser"
        '
        'SelectCameraToolStripMenuItem
        '
        Me.SelectCameraToolStripMenuItem.Name = "SelectCameraToolStripMenuItem"
        Me.SelectCameraToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.SelectCameraToolStripMenuItem.Text = "Select camera"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(249, 6)
        '
        'OpenEXEPathToolStripMenuItem
        '
        Me.OpenEXEPathToolStripMenuItem.Name = "OpenEXEPathToolStripMenuItem"
        Me.OpenEXEPathToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.OpenEXEPathToolStripMenuItem.Text = "Open EXE path"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(252, 30)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'tbLogOutput
        '
        Me.tbLogOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLogOutput.Font = New System.Drawing.Font("Courier New", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLogOutput.Location = New System.Drawing.Point(324, 36)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ReadOnly = True
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(464, 341)
        Me.tbLogOutput.TabIndex = 2
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(12, 36)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(306, 405)
        Me.pgMain.TabIndex = 3
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Enabled = False
        Me.btnStop.Location = New System.Drawing.Point(500, 383)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(141, 55)
        Me.btnStop.TabIndex = 4
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.pgMain)
        Me.Controls.Add(Me.tbLogOutput)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.msMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "frmMain"
        Me.Text = "AstroFocus"
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnStart As Button
    Friend WithEvents msMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectFocuserToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectCameraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tbLogOutput As TextBox
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents OpenEXEPathToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents btnStop As Button
End Class
