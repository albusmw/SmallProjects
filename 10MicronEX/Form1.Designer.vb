<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tbOutput = New System.Windows.Forms.TextBox()
        Me.tbMountIP = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(414, 17)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(387, 42)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Load alignment information"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'tbOutput
        '
        Me.tbOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbOutput.Font = New System.Drawing.Font("Courier New", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbOutput.Location = New System.Drawing.Point(22, 65)
        Me.tbOutput.Multiline = True
        Me.tbOutput.Name = "tbOutput"
        Me.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbOutput.Size = New System.Drawing.Size(780, 735)
        Me.tbOutput.TabIndex = 1
        '
        'tbMountIP
        '
        Me.tbMountIP.Location = New System.Drawing.Point(100, 25)
        Me.tbMountIP.Name = "tbMountIP"
        Me.tbMountIP.Size = New System.Drawing.Size(158, 26)
        Me.tbMountIP.TabIndex = 2
        Me.tbMountIP.Text = "192.168.10.119"
        Me.tbMountIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Mount IP:"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(813, 822)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbMountIP)
        Me.Controls.Add(Me.tbOutput)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.Text = "10Micron alignment information Version 1.0"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents tbOutput As TextBox
    Friend WithEvents tbMountIP As TextBox
    Friend WithEvents Label1 As Label
End Class
