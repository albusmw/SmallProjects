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
    Me.btnGo = New System.Windows.Forms.Button()
    Me.lbLog = New System.Windows.Forms.ListBox()
    Me.Button1 = New System.Windows.Forms.Button()
    Me.SuspendLayout()
    '
    'btnGo
    '
    Me.btnGo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnGo.Location = New System.Drawing.Point(12, 12)
    Me.btnGo.Name = "btnGo"
    Me.btnGo.Size = New System.Drawing.Size(319, 36)
    Me.btnGo.TabIndex = 0
    Me.btnGo.Text = "Go"
    Me.btnGo.UseVisualStyleBackColor = True
    '
    'lbLog
    '
    Me.lbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lbLog.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbLog.FormattingEnabled = True
    Me.lbLog.IntegralHeight = False
    Me.lbLog.ItemHeight = 14
    Me.lbLog.Location = New System.Drawing.Point(12, 54)
    Me.lbLog.Name = "lbLog"
    Me.lbLog.ScrollAlwaysVisible = True
    Me.lbLog.Size = New System.Drawing.Size(413, 196)
    Me.lbLog.TabIndex = 1
    '
    'Button1
    '
    Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.Button1.Location = New System.Drawing.Point(337, 12)
    Me.Button1.Name = "Button1"
    Me.Button1.Size = New System.Drawing.Size(88, 36)
    Me.Button1.TabIndex = 2
    Me.Button1.Text = "Copy"
    Me.Button1.UseVisualStyleBackColor = True
    '
    'MainForm
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(437, 262)
    Me.Controls.Add(Me.Button1)
    Me.Controls.Add(Me.lbLog)
    Me.Controls.Add(Me.btnGo)
    Me.Name = "MainForm"
    Me.Text = "PW EFA COM Test Version 1.1"
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents btnGo As System.Windows.Forms.Button
  Friend WithEvents lbLog As System.Windows.Forms.ListBox
  Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
