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
        Me.btnTest = New System.Windows.Forms.Button()
        Me.pbCapture = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.tsmiForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiForm_OpenEXELocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiForm_End = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslLoopStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslCaptureProgress = New System.Windows.Forms.ToolStripStatusLabel()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scRight = New System.Windows.Forms.SplitContainer()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.tsslCaptureDetails = New System.Windows.Forms.ToolStripStatusLabel()
        CType(Me.pbCapture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.ssMain.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.scRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scRight.Panel1.SuspendLayout()
        Me.scRight.Panel2.SuspendLayout()
        Me.scRight.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnTest
        '
        Me.btnTest.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTest.Location = New System.Drawing.Point(12, 27)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(927, 41)
        Me.btnTest.TabIndex = 0
        Me.btnTest.Text = "Capture"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'pbCapture
        '
        Me.pbCapture.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbCapture.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.pbCapture.Location = New System.Drawing.Point(3, 3)
        Me.pbCapture.Name = "pbCapture"
        Me.pbCapture.Size = New System.Drawing.Size(760, 217)
        Me.pbCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCapture.TabIndex = 1
        Me.pbCapture.TabStop = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiForm, Me.TestToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1182, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'tsmiForm
        '
        Me.tsmiForm.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiForm_OpenEXELocation, Me.ToolStripMenuItem1, Me.tsmiForm_End})
        Me.tsmiForm.Name = "tsmiForm"
        Me.tsmiForm.Size = New System.Drawing.Size(37, 20)
        Me.tsmiForm.Text = "File"
        '
        'tsmiForm_OpenEXELocation
        '
        Me.tsmiForm_OpenEXELocation.Name = "tsmiForm_OpenEXELocation"
        Me.tsmiForm_OpenEXELocation.Size = New System.Drawing.Size(171, 22)
        Me.tsmiForm_OpenEXELocation.Text = "Open EXE location"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(168, 6)
        '
        'tsmiForm_End
        '
        Me.tsmiForm_End.Name = "tsmiForm_End"
        Me.tsmiForm_End.Size = New System.Drawing.Size(171, 22)
        Me.tsmiForm_End.Text = "End"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BitToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.TestToolStripMenuItem.Text = "Test"
        '
        'BitToolStripMenuItem
        '
        Me.BitToolStripMenuItem.Name = "BitToolStripMenuItem"
        Me.BitToolStripMenuItem.Size = New System.Drawing.Size(105, 22)
        Me.BitToolStripMenuItem.Text = "16-bit"
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(3, 3)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(382, 825)
        Me.pgMain.TabIndex = 3
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Location = New System.Drawing.Point(945, 27)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(225, 41)
        Me.btnStop.TabIndex = 4
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslLoopStatus, Me.tsslCaptureProgress, Me.tsslCaptureDetails})
        Me.ssMain.Location = New System.Drawing.Point(0, 908)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(1182, 22)
        Me.ssMain.TabIndex = 5
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslLoopStatus
        '
        Me.tsslLoopStatus.Name = "tsslLoopStatus"
        Me.tsslLoopStatus.Size = New System.Drawing.Size(56, 17)
        Me.tsslLoopStatus.Text = "-- IDLE --"
        '
        'tsslCaptureProgress
        '
        Me.tsslCaptureProgress.Name = "tsslCaptureProgress"
        Me.tsslCaptureProgress.Size = New System.Drawing.Size(56, 17)
        Me.tsslCaptureProgress.Text = "-- IDLE --"
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(12, 74)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.pgMain)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.scRight)
        Me.scMain.Size = New System.Drawing.Size(1158, 831)
        Me.scMain.SplitterDistance = 388
        Me.scMain.TabIndex = 6
        '
        'scRight
        '
        Me.scRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scRight.Location = New System.Drawing.Point(0, 0)
        Me.scRight.Name = "scRight"
        Me.scRight.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scRight.Panel1
        '
        Me.scRight.Panel1.Controls.Add(Me.pbCapture)
        '
        'scRight.Panel2
        '
        Me.scRight.Panel2.Controls.Add(Me.tbLog)
        Me.scRight.Size = New System.Drawing.Size(766, 831)
        Me.scRight.SplitterDistance = 223
        Me.scRight.TabIndex = 0
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLog.Location = New System.Drawing.Point(3, 3)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(760, 598)
        Me.tbLog.TabIndex = 0
        '
        'tsslCaptureDetails
        '
        Me.tsslCaptureDetails.Name = "tsslCaptureDetails"
        Me.tsslCaptureDetails.Size = New System.Drawing.Size(56, 17)
        Me.tsslCaptureDetails.Text = "-- IDLE --"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1182, 930)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainForm"
        Me.Text = "PointGreySolar"
        CType(Me.pbCapture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.scRight.Panel1.ResumeLayout(False)
        Me.scRight.Panel2.ResumeLayout(False)
        Me.scRight.Panel2.PerformLayout()
        CType(Me.scRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scRight.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnTest As Button
    Friend WithEvents pbCapture As PictureBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents tsmiForm As ToolStripMenuItem
    Friend WithEvents tsmiForm_OpenEXELocation As ToolStripMenuItem
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents btnStop As Button
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tsslLoopStatus As ToolStripStatusLabel
    Friend WithEvents tsslCaptureProgress As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents tsmiForm_End As ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents scRight As SplitContainer
    Friend WithEvents tbLog As TextBox
    Friend WithEvents tsslCaptureDetails As ToolStripStatusLabel
End Class
