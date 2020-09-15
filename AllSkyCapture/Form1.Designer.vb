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
        Me.btnAscomCamera = New System.Windows.Forms.Button()
        Me.btnTakeImage = New System.Windows.Forms.Button()
        Me.pbLastImage = New System.Windows.Forms.PictureBox()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.tbSelectedCam = New System.Windows.Forms.TextBox()
        Me.tbExposeTime = New System.Windows.Forms.TextBox()
        Me.btnOpenImage = New System.Windows.Forms.Button()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.tbStorageRoot = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnUp2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnUp10 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        CType(Me.pbLastImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAscomCamera
        '
        Me.btnAscomCamera.Location = New System.Drawing.Point(12, 12)
        Me.btnAscomCamera.Name = "btnAscomCamera"
        Me.btnAscomCamera.Size = New System.Drawing.Size(114, 20)
        Me.btnAscomCamera.TabIndex = 0
        Me.btnAscomCamera.Text = "ASCOM Selector"
        Me.btnAscomCamera.UseVisualStyleBackColor = True
        '
        'btnTakeImage
        '
        Me.btnTakeImage.Location = New System.Drawing.Point(298, 12)
        Me.btnTakeImage.Name = "btnTakeImage"
        Me.btnTakeImage.Size = New System.Drawing.Size(94, 20)
        Me.btnTakeImage.TabIndex = 1
        Me.btnTakeImage.Text = "Take image"
        Me.btnTakeImage.UseVisualStyleBackColor = True
        '
        'pbLastImage
        '
        Me.pbLastImage.BackColor = System.Drawing.Color.Red
        Me.pbLastImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbLastImage.Location = New System.Drawing.Point(0, 0)
        Me.pbLastImage.Name = "pbLastImage"
        Me.pbLastImage.Size = New System.Drawing.Size(606, 627)
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
        Me.tbLog.Size = New System.Drawing.Size(475, 627)
        Me.tbLog.TabIndex = 3
        '
        'tbSelectedCam
        '
        Me.tbSelectedCam.Location = New System.Drawing.Point(132, 12)
        Me.tbSelectedCam.Name = "tbSelectedCam"
        Me.tbSelectedCam.Size = New System.Drawing.Size(160, 20)
        Me.tbSelectedCam.TabIndex = 4
        Me.tbSelectedCam.Text = "ASCOM.ASICamera2.Camera"
        '
        'tbExposeTime
        '
        Me.tbExposeTime.Location = New System.Drawing.Point(6, 19)
        Me.tbExposeTime.Name = "tbExposeTime"
        Me.tbExposeTime.Size = New System.Drawing.Size(63, 20)
        Me.tbExposeTime.TabIndex = 5
        Me.tbExposeTime.Text = "0.01"
        Me.tbExposeTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnOpenImage
        '
        Me.btnOpenImage.Location = New System.Drawing.Point(298, 38)
        Me.btnOpenImage.Name = "btnOpenImage"
        Me.btnOpenImage.Size = New System.Drawing.Size(94, 20)
        Me.btnOpenImage.TabIndex = 6
        Me.btnOpenImage.Text = "Open image"
        Me.btnOpenImage.UseVisualStyleBackColor = True
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(12, 94)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.tbLog)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.pbLastImage)
        Me.scMain.Size = New System.Drawing.Size(1085, 627)
        Me.scMain.SplitterDistance = 475
        Me.scMain.TabIndex = 7
        '
        'tbStorageRoot
        '
        Me.tbStorageRoot.Location = New System.Drawing.Point(132, 38)
        Me.tbStorageRoot.Name = "tbStorageRoot"
        Me.tbStorageRoot.Size = New System.Drawing.Size(160, 20)
        Me.tbStorageRoot.TabIndex = 10
        Me.tbStorageRoot.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Storage root"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.btnUp2)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.btnUp10)
        Me.GroupBox1.Controls.Add(Me.tbExposeTime)
        Me.GroupBox1.Location = New System.Drawing.Point(398, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(254, 46)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Exposure [s]"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(119, 17)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(38, 22)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "/ 2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnUp2
        '
        Me.btnUp2.Location = New System.Drawing.Point(161, 17)
        Me.btnUp2.Name = "btnUp2"
        Me.btnUp2.Size = New System.Drawing.Size(38, 22)
        Me.btnUp2.TabIndex = 8
        Me.btnUp2.Text = "x 2"
        Me.btnUp2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(75, 17)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(38, 22)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "/ 10"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnUp10
        '
        Me.btnUp10.Location = New System.Drawing.Point(205, 17)
        Me.btnUp10.Name = "btnUp10"
        Me.btnUp10.Size = New System.Drawing.Size(38, 22)
        Me.btnUp10.TabIndex = 6
        Me.btnUp10.Text = "x 10"
        Me.btnUp10.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(658, 11)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(69, 47)
        Me.Button3.TabIndex = 13
        Me.Button3.Text = "Gain MAX"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(733, 11)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(69, 47)
        Me.Button4.TabIndex = 14
        Me.Button4.Text = "Gain MIN"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(808, 12)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(94, 46)
        Me.Button5.TabIndex = 15
        Me.Button5.Text = "Test FTP"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1109, 733)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbStorageRoot)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.btnOpenImage)
        Me.Controls.Add(Me.tbSelectedCam)
        Me.Controls.Add(Me.btnTakeImage)
        Me.Controls.Add(Me.btnAscomCamera)
        Me.Name = "Form1"
        Me.Text = "AllSkyCapture Version 1.1 (21.11.2017)"
        CType(Me.pbLastImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel1.PerformLayout()
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAscomCamera As System.Windows.Forms.Button
    Friend WithEvents btnTakeImage As System.Windows.Forms.Button
    Friend WithEvents pbLastImage As System.Windows.Forms.PictureBox
    Friend WithEvents tbLog As System.Windows.Forms.TextBox
    Friend WithEvents tbSelectedCam As System.Windows.Forms.TextBox
    Friend WithEvents tbExposeTime As System.Windows.Forms.TextBox
    Friend WithEvents btnOpenImage As System.Windows.Forms.Button
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents tbStorageRoot As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnUp10 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents btnUp2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
End Class
