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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.pbMain = New System.Windows.Forms.PictureBox
        Me.ssMain = New System.Windows.Forms.StatusStrip
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsbStart = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton
        Me.tsbSave = New System.Windows.Forms.ToolStripButton
        Me.pgMain = New System.Windows.Forms.PropertyGrid
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ssMain.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbMain
        '
        Me.pbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMain.BackColor = System.Drawing.Color.Black
        Me.pbMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbMain.Location = New System.Drawing.Point(199, 28)
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(655, 293)
        Me.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbMain.TabIndex = 0
        Me.pbMain.TabStop = False
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 324)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(866, 22)
        Me.ssMain.TabIndex = 1
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(19, 17)
        Me.tsslMain.Text = "---"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbStart, Me.ToolStripButton1, Me.tsbSave})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(866, 25)
        Me.ToolStrip1.TabIndex = 0
        '
        'tsbStart
        '
        Me.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbStart.Image = CType(resources.GetObject("tsbStart.Image"), System.Drawing.Image)
        Me.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbStart.Name = "tsbStart"
        Me.tsbStart.Size = New System.Drawing.Size(35, 22)
        Me.tsbStart.Text = "Start"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(55, 22)
        Me.ToolStripButton1.Text = "Full Scale"
        '
        'tsbSave
        '
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbSave.Image = CType(resources.GetObject("tsbSave.Image"), System.Drawing.Image)
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(35, 22)
        Me.tsbSave.Text = "Save"
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(0, 28)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(193, 296)
        Me.pgMain.TabIndex = 4
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gray
        Me.ClientSize = New System.Drawing.Size(866, 346)
        Me.Controls.Add(Me.pbMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.pgMain)
        Me.Name = "MainForm"
        Me.Text = "Fractal.NET"
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbMain As System.Windows.Forms.PictureBox
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents tsslMain As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbStart As System.Windows.Forms.ToolStripButton
    Friend WithEvents pgMain As System.Windows.Forms.PropertyGrid
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton

End Class
