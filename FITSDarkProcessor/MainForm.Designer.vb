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
        Me.btnAnalysis = New System.Windows.Forms.Button()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tspbProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.tsslCurrentMean = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnConvert = New System.Windows.Forms.Button()
        Me.tbLog = New FITSDarkProcessor.LogList()
        Me.ssMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAnalysis
        '
        Me.btnAnalysis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAnalysis.Location = New System.Drawing.Point(12, 12)
        Me.btnAnalysis.Name = "btnAnalysis"
        Me.btnAnalysis.Size = New System.Drawing.Size(327, 31)
        Me.btnAnalysis.TabIndex = 1
        Me.btnAnalysis.Text = "Analysis"
        Me.btnAnalysis.UseVisualStyleBackColor = True
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tspbProgress, Me.tsslCurrentMean})
        Me.ssMain.Location = New System.Drawing.Point(0, 449)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(527, 22)
        Me.ssMain.TabIndex = 2
        Me.ssMain.Text = "StatusStrip1"
        '
        'tspbProgress
        '
        Me.tspbProgress.Name = "tspbProgress"
        Me.tspbProgress.Size = New System.Drawing.Size(300, 16)
        Me.tspbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'tsslCurrentMean
        '
        Me.tsslCurrentMean.Name = "tsslCurrentMean"
        Me.tsslCurrentMean.Size = New System.Drawing.Size(95, 17)
        Me.tsslCurrentMean.Text = "Current Mean: ..."
        '
        'btnConvert
        '
        Me.btnConvert.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConvert.Location = New System.Drawing.Point(345, 12)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(82, 31)
        Me.btnConvert.TabIndex = 3
        Me.btnConvert.Text = "Convert"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'tbLog
        '
        Me.tbLog.AddTime = True
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Location = New System.Drawing.Point(0, 49)
        Me.tbLog.Name = "tbLog"
        Me.tbLog.Size = New System.Drawing.Size(527, 397)
        Me.tbLog.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(527, 471)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.btnAnalysis)
        Me.Controls.Add(Me.tbLog)
        Me.Name = "MainForm"
        Me.Text = "FITS Dark Processor V1.0"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbLog As FITSDarkProcessor.LogList
  Friend WithEvents btnAnalysis As System.Windows.Forms.Button
  Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
  Friend WithEvents tspbProgress As System.Windows.Forms.ToolStripProgressBar
  Friend WithEvents tsslCurrentMean As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents btnConvert As System.Windows.Forms.Button

End Class
