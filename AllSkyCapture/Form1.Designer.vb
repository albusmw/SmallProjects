<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.pbLastImage = New System.Windows.Forms.PictureBox()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scLeftPanel = New System.Windows.Forms.SplitContainer()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectCameraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenLastImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenStoragePathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.JoinToVideoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiTakeOnePicture = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFTPUpload = New System.Windows.Forms.ToolStripMenuItem()
        Me.TelnetClientToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MyCaptureDefaultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tCheckExpState = New System.Windows.Forms.Timer(Me.components)
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslCapture = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslSunPos = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslNoCapture = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsmiFile_ClearLog = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pbLastImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.scLeftPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scLeftPanel.Panel1.SuspendLayout()
        Me.scLeftPanel.Panel2.SuspendLayout()
        Me.scLeftPanel.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbLastImage
        '
        Me.pbLastImage.BackColor = System.Drawing.Color.Gray
        Me.pbLastImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbLastImage.Location = New System.Drawing.Point(0, 0)
        Me.pbLastImage.Name = "pbLastImage"
        Me.pbLastImage.Size = New System.Drawing.Size(439, 477)
        Me.pbLastImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbLastImage.TabIndex = 2
        Me.pbLastImage.TabStop = False
        '
        'tbLog
        '
        Me.tbLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbLog.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLog.Location = New System.Drawing.Point(0, 0)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ReadOnly = True
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(339, 236)
        Me.tbLog.TabIndex = 3
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(12, 27)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.scLeftPanel)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.pbLastImage)
        Me.scMain.Size = New System.Drawing.Size(787, 477)
        Me.scMain.SplitterDistance = 344
        Me.scMain.TabIndex = 7
        '
        'scLeftPanel
        '
        Me.scLeftPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scLeftPanel.Location = New System.Drawing.Point(3, 3)
        Me.scLeftPanel.Name = "scLeftPanel"
        Me.scLeftPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scLeftPanel.Panel1
        '
        Me.scLeftPanel.Panel1.Controls.Add(Me.pgMain)
        '
        'scLeftPanel.Panel2
        '
        Me.scLeftPanel.Panel2.Controls.Add(Me.tbLog)
        Me.scLeftPanel.Size = New System.Drawing.Size(339, 471)
        Me.scLeftPanel.SplitterDistance = 231
        Me.scLeftPanel.TabIndex = 4
        '
        'pgMain
        '
        Me.pgMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMain.Location = New System.Drawing.Point(0, 0)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(339, 231)
        Me.pgMain.TabIndex = 16
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.TestToolStripMenuItem, Me.SetToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(811, 24)
        Me.msMain.TabIndex = 16
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectCameraToolStripMenuItem, Me.ToolStripMenuItem2, Me.OpenLastImageToolStripMenuItem, Me.OpenStoragePathToolStripMenuItem, Me.JoinToVideoToolStripMenuItem, Me.ToolStripMenuItem1, Me.tsmiFile_ClearLog, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'SelectCameraToolStripMenuItem
        '
        Me.SelectCameraToolStripMenuItem.Name = "SelectCameraToolStripMenuItem"
        Me.SelectCameraToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SelectCameraToolStripMenuItem.Text = "Select camera"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(177, 6)
        '
        'OpenLastImageToolStripMenuItem
        '
        Me.OpenLastImageToolStripMenuItem.Name = "OpenLastImageToolStripMenuItem"
        Me.OpenLastImageToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OpenLastImageToolStripMenuItem.Text = "Open last image"
        '
        'OpenStoragePathToolStripMenuItem
        '
        Me.OpenStoragePathToolStripMenuItem.Name = "OpenStoragePathToolStripMenuItem"
        Me.OpenStoragePathToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OpenStoragePathToolStripMenuItem.Text = "Open storage path"
        '
        'JoinToVideoToolStripMenuItem
        '
        Me.JoinToVideoToolStripMenuItem.Name = "JoinToVideoToolStripMenuItem"
        Me.JoinToVideoToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.JoinToVideoToolStripMenuItem.Text = "Join to video"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiTakeOnePicture, Me.ToolStripMenuItem3, Me.tsmiFTPUpload, Me.TelnetClientToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.TestToolStripMenuItem.Text = "Test"
        '
        'tsmiTakeOnePicture
        '
        Me.tsmiTakeOnePicture.Name = "tsmiTakeOnePicture"
        Me.tsmiTakeOnePicture.Size = New System.Drawing.Size(180, 22)
        Me.tsmiTakeOnePicture.Text = "Take 1 picture"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(177, 6)
        '
        'tsmiFTPUpload
        '
        Me.tsmiFTPUpload.Name = "tsmiFTPUpload"
        Me.tsmiFTPUpload.Size = New System.Drawing.Size(180, 22)
        Me.tsmiFTPUpload.Text = "FTP upload"
        '
        'TelnetClientToolStripMenuItem
        '
        Me.TelnetClientToolStripMenuItem.Name = "TelnetClientToolStripMenuItem"
        Me.TelnetClientToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.TelnetClientToolStripMenuItem.Text = "Telnet client"
        '
        'SetToolStripMenuItem
        '
        Me.SetToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MyCaptureDefaultsToolStripMenuItem})
        Me.SetToolStripMenuItem.Name = "SetToolStripMenuItem"
        Me.SetToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.SetToolStripMenuItem.Text = "Set"
        '
        'MyCaptureDefaultsToolStripMenuItem
        '
        Me.MyCaptureDefaultsToolStripMenuItem.Name = "MyCaptureDefaultsToolStripMenuItem"
        Me.MyCaptureDefaultsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.MyCaptureDefaultsToolStripMenuItem.Text = "My capture defaults"
        '
        'tCheckExpState
        '
        Me.tCheckExpState.Enabled = True
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslCapture, Me.tsslSunPos, Me.tsslNoCapture})
        Me.ssMain.Location = New System.Drawing.Point(0, 517)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(811, 22)
        Me.ssMain.TabIndex = 17
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslCapture
        '
        Me.tsslCapture.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.tsslCapture.Name = "tsslCapture"
        Me.tsslCapture.Size = New System.Drawing.Size(83, 17)
        Me.tsslCapture.Text = "EXP RUNNING"
        '
        'tsslSunPos
        '
        Me.tsslSunPos.Name = "tsslSunPos"
        Me.tsslSunPos.Size = New System.Drawing.Size(22, 17)
        Me.tsslSunPos.Text = "---"
        '
        'tsslNoCapture
        '
        Me.tsslNoCapture.Name = "tsslNoCapture"
        Me.tsslNoCapture.Size = New System.Drawing.Size(119, 17)
        Me.tsslNoCapture.Text = "ToolStripStatusLabel1"
        '
        'tsmiFile_ClearLog
        '
        Me.tsmiFile_ClearLog.Name = "tsmiFile_ClearLog"
        Me.tsmiFile_ClearLog.Size = New System.Drawing.Size(180, 22)
        Me.tsmiFile_ClearLog.Text = "Clear log"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(811, 539)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.msMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "Form1"
        Me.Text = "AllSkyCapture Version 1.2"
        CType(Me.pbLastImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.scLeftPanel.Panel1.ResumeLayout(False)
        Me.scLeftPanel.Panel2.ResumeLayout(False)
        Me.scLeftPanel.Panel2.PerformLayout()
        CType(Me.scLeftPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scLeftPanel.ResumeLayout(False)
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbLastImage As System.Windows.Forms.PictureBox
    Friend WithEvents tbLog As System.Windows.Forms.TextBox
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents scLeftPanel As SplitContainer
    Friend WithEvents msMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectCameraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenLastImageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiFTPUpload As ToolStripMenuItem
    Friend WithEvents tCheckExpState As Timer
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents OpenStoragePathToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents JoinToVideoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SetToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MyCaptureDefaultsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tsslSunPos As ToolStripStatusLabel
    Friend WithEvents tsmiTakeOnePicture As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents tsslCapture As ToolStripStatusLabel
    Friend WithEvents tsslNoCapture As ToolStripStatusLabel
    Friend WithEvents TelnetClientToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiFile_ClearLog As ToolStripMenuItem
End Class
