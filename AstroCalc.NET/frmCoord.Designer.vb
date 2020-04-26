<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCoord
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbDegree = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbDegreeMin = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbDegreeSec = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbHours = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbHoursMin = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.tbHoursSec = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.tbFloat = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnFromClipboard = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Silver
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 72)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "RA"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbDegree
        '
        Me.tbDegree.Location = New System.Drawing.Point(40, 12)
        Me.tbDegree.Name = "tbDegree"
        Me.tbDegree.Size = New System.Drawing.Size(31, 20)
        Me.tbDegree.TabIndex = 1
        Me.tbDegree.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label2.Location = New System.Drawing.Point(77, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(11, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "°"
        '
        'tbDegreeMin
        '
        Me.tbDegreeMin.Location = New System.Drawing.Point(94, 12)
        Me.tbDegreeMin.Name = "tbDegreeMin"
        Me.tbDegreeMin.Size = New System.Drawing.Size(31, 20)
        Me.tbDegreeMin.TabIndex = 3
        Me.tbDegreeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label3.Location = New System.Drawing.Point(131, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(9, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "'"
        '
        'tbDegreeSec
        '
        Me.tbDegreeSec.Location = New System.Drawing.Point(146, 12)
        Me.tbDegreeSec.Name = "tbDegreeSec"
        Me.tbDegreeSec.Size = New System.Drawing.Size(109, 20)
        Me.tbDegreeSec.TabIndex = 5
        Me.tbDegreeSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label4.Location = New System.Drawing.Point(261, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(12, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = """"
        '
        'tbHours
        '
        Me.tbHours.Location = New System.Drawing.Point(40, 38)
        Me.tbHours.Name = "tbHours"
        Me.tbHours.Size = New System.Drawing.Size(31, 20)
        Me.tbHours.TabIndex = 7
        Me.tbHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label5.Location = New System.Drawing.Point(77, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(13, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "h"
        '
        'tbHoursMin
        '
        Me.tbHoursMin.Location = New System.Drawing.Point(94, 38)
        Me.tbHoursMin.Name = "tbHoursMin"
        Me.tbHoursMin.Size = New System.Drawing.Size(31, 20)
        Me.tbHoursMin.TabIndex = 9
        Me.tbHoursMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label6.Location = New System.Drawing.Point(131, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(9, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "'"
        '
        'tbHoursSec
        '
        Me.tbHoursSec.Location = New System.Drawing.Point(146, 38)
        Me.tbHoursSec.Name = "tbHoursSec"
        Me.tbHoursSec.Size = New System.Drawing.Size(109, 20)
        Me.tbHoursSec.TabIndex = 11
        Me.tbHoursSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label7.Location = New System.Drawing.Point(261, 41)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(12, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = """"
        '
        'tbFloat
        '
        Me.tbFloat.Location = New System.Drawing.Point(40, 64)
        Me.tbFloat.Name = "tbFloat"
        Me.tbFloat.Size = New System.Drawing.Size(215, 20)
        Me.tbFloat.TabIndex = 13
        Me.tbFloat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label8.Location = New System.Drawing.Point(262, 67)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(11, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "°"
        '
        'btnFromClipboard
        '
        Me.btnFromClipboard.Location = New System.Drawing.Point(279, 12)
        Me.btnFromClipboard.Name = "btnFromClipboard"
        Me.btnFromClipboard.Size = New System.Drawing.Size(46, 42)
        Me.btnFromClipboard.TabIndex = 15
        Me.btnFromClipboard.Text = "From clip"
        Me.btnFromClipboard.UseVisualStyleBackColor = True
        '
        'frmCoord
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(330, 90)
        Me.Controls.Add(Me.btnFromClipboard)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.tbFloat)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbHoursSec)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbHoursMin)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbHours)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbDegreeSec)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbDegreeMin)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbDegree)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmCoord"
        Me.Text = "Coordinates"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbDegree As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbDegreeMin As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbDegreeSec As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbHours As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbHoursMin As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbHoursSec As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbFloat As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnFromClipboard As System.Windows.Forms.Button
End Class
