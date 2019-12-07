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
        Me.btnRun = New System.Windows.Forms.Button()
        Me.tbPattern = New System.Windows.Forms.TextBox()
        Me.tbFolder = New System.Windows.Forms.TextBox()
        Me.tbStatus = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(748, 122)
        Me.btnRun.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(118, 62)
        Me.btnRun.TabIndex = 0
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'tbPattern
        '
        Me.tbPattern.Location = New System.Drawing.Point(641, 44)
        Me.tbPattern.Name = "tbPattern"
        Me.tbPattern.Size = New System.Drawing.Size(231, 26)
        Me.tbPattern.TabIndex = 1
        Me.tbPattern.Text = "*.dll"
        '
        'tbFolder
        '
        Me.tbFolder.Location = New System.Drawing.Point(641, 12)
        Me.tbFolder.Name = "tbFolder"
        Me.tbFolder.Size = New System.Drawing.Size(231, 26)
        Me.tbFolder.TabIndex = 2
        Me.tbFolder.Text = "F:\"
        '
        'tbStatus
        '
        Me.tbStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbStatus.Location = New System.Drawing.Point(12, 83)
        Me.tbStatus.Name = "tbStatus"
        Me.tbStatus.ReadOnly = True
        Me.tbStatus.Size = New System.Drawing.Size(854, 26)
        Me.tbStatus.TabIndex = 3
        Me.tbStatus.Text = "---"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 202)
        Me.Controls.Add(Me.tbStatus)
        Me.Controls.Add(Me.tbFolder)
        Me.Controls.Add(Me.tbPattern)
        Me.Controls.Add(Me.btnRun)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "Form1"
        Me.Text = "DeletePatternFiles v0.2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents tbPattern As TextBox
    Friend WithEvents tbFolder As TextBox
    Friend WithEvents tbStatus As TextBox
End Class
