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
        Me.btnTakeImage = New System.Windows.Forms.Button()
        Me.pbLastImage = New System.Windows.Forms.PictureBox()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scLeftPanel = New System.Windows.Forms.SplitContainer()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnUp2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnUp10 = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectCameraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenLastImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenStoragePathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.JoinToVideoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FTPUploadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HardwareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GAINToMAXToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GAINToMINToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tCheckExpState = New System.Windows.Forms.Timer(Me.components)
        Me.SetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MyCaptureDefaultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pbLastImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.scLeftPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scLeftPanel.Panel1.SuspendLayout()
        Me.scLeftPanel.Panel2.SuspendLayout()
        Me.scLeftPanel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnTakeImage
        '
        Me.btnTakeImage.Location = New System.Drawing.Point(206, 27)
        Me.btnTakeImage.Name = "btnTakeImage"
        Me.btnTakeImage.Size = New System.Drawing.Size(94, 46)
        Me.btnTakeImage.TabIndex = 1
        Me.btnTakeImage.Text = "Take test image"
        Me.btnTakeImage.UseVisualStyleBackColor = True
        '
        'pbLastImage
        '
        Me.pbLastImage.BackColor = System.Drawing.Color.Red
        Me.pbLastImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbLastImage.Location = New System.Drawing.Point(0, 0)
        Me.pbLastImage.Name = "pbLastImage"
        Me.pbLastImage.Size = New System.Drawing.Size(606, 642)
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
        Me.tbLog.Size = New System.Drawing.Size(470, 318)
        Me.tbLog.TabIndex = 3
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(12, 79)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.scLeftPanel)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.pbLastImage)
        Me.scMain.Size = New System.Drawing.Size(1085, 642)
        Me.scMain.SplitterDistance = 475
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
        Me.scLeftPanel.Size = New System.Drawing.Size(470, 636)
        Me.scLeftPanel.SplitterDistance = 314
        Me.scLeftPanel.TabIndex = 4
        '
        'pgMain
        '
        Me.pgMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMain.Location = New System.Drawing.Point(0, 0)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(470, 314)
        Me.pgMain.TabIndex = 16
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.btnUp2)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.btnUp10)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(188, 46)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Exposure [s]"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(50, 15)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(38, 22)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "/ 2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnUp2
        '
        Me.btnUp2.Location = New System.Drawing.Point(94, 15)
        Me.btnUp2.Name = "btnUp2"
        Me.btnUp2.Size = New System.Drawing.Size(38, 22)
        Me.btnUp2.TabIndex = 8
        Me.btnUp2.Text = "x 2"
        Me.btnUp2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(6, 15)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(38, 22)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "/ 10"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnUp10
        '
        Me.btnUp10.Location = New System.Drawing.Point(138, 15)
        Me.btnUp10.Name = "btnUp10"
        Me.btnUp10.Size = New System.Drawing.Size(38, 22)
        Me.btnUp10.TabIndex = 6
        Me.btnUp10.Text = "x 10"
        Me.btnUp10.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.TestToolStripMenuItem, Me.HardwareToolStripMenuItem, Me.SetToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1109, 24)
        Me.MenuStrip1.TabIndex = 16
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectCameraToolStripMenuItem, Me.ToolStripMenuItem2, Me.OpenLastImageToolStripMenuItem, Me.OpenStoragePathToolStripMenuItem, Me.JoinToVideoToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'SelectCameraToolStripMenuItem
        '
        Me.SelectCameraToolStripMenuItem.Name = "SelectCameraToolStripMenuItem"
        Me.SelectCameraToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.SelectCameraToolStripMenuItem.Text = "Select camera"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(169, 6)
        '
        'OpenLastImageToolStripMenuItem
        '
        Me.OpenLastImageToolStripMenuItem.Name = "OpenLastImageToolStripMenuItem"
        Me.OpenLastImageToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.OpenLastImageToolStripMenuItem.Text = "Open last image"
        '
        'OpenStoragePathToolStripMenuItem
        '
        Me.OpenStoragePathToolStripMenuItem.Name = "OpenStoragePathToolStripMenuItem"
        Me.OpenStoragePathToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.OpenStoragePathToolStripMenuItem.Text = "Open storage path"
        '
        'JoinToVideoToolStripMenuItem
        '
        Me.JoinToVideoToolStripMenuItem.Name = "JoinToVideoToolStripMenuItem"
        Me.JoinToVideoToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.JoinToVideoToolStripMenuItem.Text = "Join to video"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(169, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FTPUploadToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.TestToolStripMenuItem.Text = "Test"
        '
        'FTPUploadToolStripMenuItem
        '
        Me.FTPUploadToolStripMenuItem.Name = "FTPUploadToolStripMenuItem"
        Me.FTPUploadToolStripMenuItem.Size = New System.Drawing.Size(133, 22)
        Me.FTPUploadToolStripMenuItem.Text = "FTP upload"
        '
        'HardwareToolStripMenuItem
        '
        Me.HardwareToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GAINToMAXToolStripMenuItem, Me.GAINToMINToolStripMenuItem})
        Me.HardwareToolStripMenuItem.Name = "HardwareToolStripMenuItem"
        Me.HardwareToolStripMenuItem.Size = New System.Drawing.Size(70, 20)
        Me.HardwareToolStripMenuItem.Text = "Hardware"
        '
        'GAINToMAXToolStripMenuItem
        '
        Me.GAINToMAXToolStripMenuItem.Name = "GAINToMAXToolStripMenuItem"
        Me.GAINToMAXToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.GAINToMAXToolStripMenuItem.Text = "GAIN to MAX"
        '
        'GAINToMINToolStripMenuItem
        '
        Me.GAINToMINToolStripMenuItem.Name = "GAINToMINToolStripMenuItem"
        Me.GAINToMINToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.GAINToMINToolStripMenuItem.Text = "GAIN to MIN"
        '
        'tCheckExpState
        '
        Me.tCheckExpState.Enabled = True
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1109, 733)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.btnTakeImage)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "AllSkyCapture Version 1.1 (21.11.2017)"
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
        Me.GroupBox1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnTakeImage As System.Windows.Forms.Button
    Friend WithEvents pbLastImage As System.Windows.Forms.PictureBox
    Friend WithEvents tbLog As System.Windows.Forms.TextBox
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnUp10 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents btnUp2 As Button
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents scLeftPanel As SplitContainer
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectCameraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenLastImageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FTPUploadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HardwareToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GAINToMAXToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GAINToMINToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tCheckExpState As Timer
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents OpenStoragePathToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents JoinToVideoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SetToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MyCaptureDefaultsToolStripMenuItem As ToolStripMenuItem
End Class
