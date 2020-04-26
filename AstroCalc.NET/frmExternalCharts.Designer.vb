<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExternalCharts
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
    Me.cbSelection = New System.Windows.Forms.ComboBox()
    Me.pbMain = New System.Windows.Forms.PictureBox()
    CType(Me.pbMain, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'cbSelection
    '
    Me.cbSelection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cbSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cbSelection.FormattingEnabled = True
    Me.cbSelection.Location = New System.Drawing.Point(1, 12)
    Me.cbSelection.Name = "cbSelection"
    Me.cbSelection.Size = New System.Drawing.Size(446, 21)
    Me.cbSelection.TabIndex = 0
    '
    'pbMain
    '
    Me.pbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pbMain.Location = New System.Drawing.Point(1, 39)
    Me.pbMain.Name = "pbMain"
    Me.pbMain.Size = New System.Drawing.Size(446, 402)
    Me.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
    Me.pbMain.TabIndex = 2
    Me.pbMain.TabStop = False
    '
    'frmExternalCharts
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(448, 442)
    Me.Controls.Add(Me.pbMain)
    Me.Controls.Add(Me.cbSelection)
    Me.Name = "frmExternalCharts"
    Me.Text = "External charts"
    CType(Me.pbMain, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents cbSelection As System.Windows.Forms.ComboBox
  Friend WithEvents pbMain As System.Windows.Forms.PictureBox
End Class
