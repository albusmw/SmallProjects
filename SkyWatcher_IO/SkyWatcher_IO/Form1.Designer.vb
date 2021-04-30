<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.tbCOMPort = New System.Windows.Forms.TextBox()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.SuspendLayout()
        '
        'tbCOMPort
        '
        Me.tbCOMPort.Location = New System.Drawing.Point(12, 12)
        Me.tbCOMPort.Name = "tbCOMPort"
        Me.tbCOMPort.Size = New System.Drawing.Size(65, 20)
        Me.tbCOMPort.TabIndex = 3
        Me.tbCOMPort.Text = "COM3"
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(83, 12)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(150, 20)
        Me.btnTest.TabIndex = 4
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'pgMain
        '
        Me.pgMain.Location = New System.Drawing.Point(12, 38)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(221, 400)
        Me.pgMain.TabIndex = 5
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.pgMain)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.tbCOMPort)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbCOMPort As TextBox
    Friend WithEvents btnTest As Button
    Friend WithEvents pgMain As PropertyGrid
End Class
