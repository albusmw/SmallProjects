<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCooling
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
        Me.components = New System.ComponentModel.Container()
        Me.tQuery = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbT_set = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbT_measured = New System.Windows.Forms.TextBox()
        Me.btnSetTemp = New System.Windows.Forms.Button()
        Me.tbCoolerPWM = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pbDelta = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'tQuery
        '
        Me.tQuery.Enabled = True
        Me.tQuery.Interval = 250
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "T requested"
        '
        'tbT_set
        '
        Me.tbT_set.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbT_set.Location = New System.Drawing.Point(82, 17)
        Me.tbT_set.Name = "tbT_set"
        Me.tbT_set.ReadOnly = True
        Me.tbT_set.Size = New System.Drawing.Size(69, 20)
        Me.tbT_set.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "T measured"
        '
        'tbT_measured
        '
        Me.tbT_measured.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbT_measured.Location = New System.Drawing.Point(82, 43)
        Me.tbT_measured.Name = "tbT_measured"
        Me.tbT_measured.ReadOnly = True
        Me.tbT_measured.Size = New System.Drawing.Size(69, 20)
        Me.tbT_measured.TabIndex = 3
        '
        'btnSetTemp
        '
        Me.btnSetTemp.Location = New System.Drawing.Point(157, 12)
        Me.btnSetTemp.Name = "btnSetTemp"
        Me.btnSetTemp.Size = New System.Drawing.Size(57, 29)
        Me.btnSetTemp.TabIndex = 4
        Me.btnSetTemp.Text = "Change"
        Me.btnSetTemp.UseVisualStyleBackColor = True
        '
        'tbCoolerPWM
        '
        Me.tbCoolerPWM.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbCoolerPWM.Location = New System.Drawing.Point(82, 69)
        Me.tbCoolerPWM.Name = "tbCoolerPWM"
        Me.tbCoolerPWM.ReadOnly = True
        Me.tbCoolerPWM.Size = New System.Drawing.Size(69, 20)
        Me.tbCoolerPWM.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cooler @"
        '
        'pbDelta
        '
        Me.pbDelta.ForeColor = System.Drawing.Color.Red
        Me.pbDelta.Location = New System.Drawing.Point(15, 95)
        Me.pbDelta.Name = "pbDelta"
        Me.pbDelta.Size = New System.Drawing.Size(140, 20)
        Me.pbDelta.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbDelta.TabIndex = 8
        '
        'frmCooling
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(589, 160)
        Me.Controls.Add(Me.pbDelta)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbCoolerPWM)
        Me.Controls.Add(Me.btnSetTemp)
        Me.Controls.Add(Me.tbT_measured)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbT_set)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmCooling"
        Me.Text = "Cooling"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tQuery As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents tbT_set As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tbT_measured As TextBox
    Friend WithEvents btnSetTemp As Button
    Friend WithEvents tbCoolerPWM As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents pbDelta As ProgressBar
End Class
