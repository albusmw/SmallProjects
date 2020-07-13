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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbSourceRoot = New System.Windows.Forms.TextBox()
        Me.tbDestinationRoot = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.tbDropHere = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Source Root"
        '
        'tbSourceRoot
        '
        Me.tbSourceRoot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceRoot.Location = New System.Drawing.Point(147, 12)
        Me.tbSourceRoot.Name = "tbSourceRoot"
        Me.tbSourceRoot.Size = New System.Drawing.Size(548, 26)
        Me.tbSourceRoot.TabIndex = 1
        Me.tbSourceRoot.Text = "C:\Users\albusmw\foxdox"
        '
        'tbDestinationRoot
        '
        Me.tbDestinationRoot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDestinationRoot.Location = New System.Drawing.Point(147, 44)
        Me.tbDestinationRoot.Name = "tbDestinationRoot"
        Me.tbDestinationRoot.Size = New System.Drawing.Size(548, 26)
        Me.tbDestinationRoot.TabIndex = 2
        Me.tbDestinationRoot.Text = "C:\Users\albusmw\Dropbox\Verwaltung\Finanzamt\Share Steuerberater"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(129, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Destination Root"
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Location = New System.Drawing.Point(16, 221)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(679, 239)
        Me.tbLog.TabIndex = 4
        '
        'tbDropHere
        '
        Me.tbDropHere.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDropHere.Location = New System.Drawing.Point(16, 76)
        Me.tbDropHere.Multiline = True
        Me.tbDropHere.Name = "tbDropHere"
        Me.tbDropHere.Size = New System.Drawing.Size(679, 139)
        Me.tbDropHere.TabIndex = 5
        Me.tbDropHere.Text = "Drop here ..."
        Me.tbDropHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(707, 472)
        Me.Controls.Add(Me.tbDropHere)
        Me.Controls.Add(Me.tbLog)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbDestinationRoot)
        Me.Controls.Add(Me.tbSourceRoot)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "DragDropCopy"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents tbSourceRoot As TextBox
    Friend WithEvents tbDestinationRoot As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tbLog As TextBox
    Friend WithEvents tbDropHere As TextBox
End Class
