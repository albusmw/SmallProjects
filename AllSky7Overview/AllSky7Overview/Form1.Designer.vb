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
        Me.pbMain = New System.Windows.Forms.PictureBox()
        Me.cbAllCams = New System.Windows.Forms.ComboBox()
        Me.ltbMain = New AllSky7Overview.cLogTextBox()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OnlineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOnline_AllCameras = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbMain
        '
        Me.pbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMain.BackColor = System.Drawing.Color.Silver
        Me.pbMain.Location = New System.Drawing.Point(3, 3)
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(1111, 359)
        Me.pbMain.TabIndex = 0
        Me.pbMain.TabStop = False
        '
        'cbAllCams
        '
        Me.cbAllCams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAllCams.FormattingEnabled = True
        Me.cbAllCams.Location = New System.Drawing.Point(12, 27)
        Me.cbAllCams.Name = "cbAllCams"
        Me.cbAllCams.Size = New System.Drawing.Size(421, 21)
        Me.cbAllCams.TabIndex = 1
        '
        'ltbMain
        '
        Me.ltbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ltbMain.Font = New System.Drawing.Font("Courier New", 8.0!)
        Me.ltbMain.Location = New System.Drawing.Point(3, 3)
        Me.ltbMain.Multiline = True
        Me.ltbMain.Name = "ltbMain"
        Me.ltbMain.ReadOnly = True
        Me.ltbMain.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.ltbMain.Size = New System.Drawing.Size(1111, 355)
        Me.ltbMain.TabIndex = 3
        Me.ltbMain.TimeStampFormat = "yyyy-MM-dd hh:nn:ss"
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(12, 54)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.pbMain)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.ltbMain)
        Me.scMain.Size = New System.Drawing.Size(1117, 730)
        Me.scMain.SplitterDistance = 365
        Me.scMain.TabIndex = 4
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OnlineToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(1141, 24)
        Me.msMain.TabIndex = 5
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OnlineToolStripMenuItem
        '
        Me.OnlineToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiOnline_AllCameras})
        Me.OnlineToolStripMenuItem.Name = "OnlineToolStripMenuItem"
        Me.OnlineToolStripMenuItem.Size = New System.Drawing.Size(54, 20)
        Me.OnlineToolStripMenuItem.Text = "Online"
        '
        'tsmiOnline_AllCameras
        '
        Me.tsmiOnline_AllCameras.Name = "tsmiOnline_AllCameras"
        Me.tsmiOnline_AllCameras.Size = New System.Drawing.Size(188, 22)
        Me.tsmiOnline_AllCameras.Text = "Get all single cameras"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1141, 796)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.cbAllCams)
        Me.Controls.Add(Me.msMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.Panel2.PerformLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pbMain As PictureBox
    Friend WithEvents cbAllCams As ComboBox
    Friend WithEvents ltbMain As cLogTextBox
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents msMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OnlineToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiOnline_AllCameras As ToolStripMenuItem
End Class
