<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogList
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
    Me.tbLog = New System.Windows.Forms.TextBox()
    Me.btClear = New System.Windows.Forms.Button()
    Me.SuspendLayout()
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
    Me.tbLog.Size = New System.Drawing.Size(504, 465)
    Me.tbLog.TabIndex = 0
    '
    'btClear
    '
    Me.btClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btClear.Location = New System.Drawing.Point(3, 474)
    Me.btClear.Name = "btClear"
    Me.btClear.Size = New System.Drawing.Size(52, 28)
    Me.btClear.TabIndex = 1
    Me.btClear.Text = "Clear"
    Me.btClear.UseVisualStyleBackColor = True
    '
    'LogList
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.btClear)
    Me.Controls.Add(Me.tbLog)
    Me.Name = "LogList"
    Me.Size = New System.Drawing.Size(510, 505)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents tbLog As System.Windows.Forms.TextBox
  Friend WithEvents btClear As System.Windows.Forms.Button

End Class
