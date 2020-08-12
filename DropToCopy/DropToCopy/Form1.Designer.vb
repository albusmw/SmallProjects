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
        Me.btnSource = New System.Windows.Forms.Button()
        Me.btnDest = New System.Windows.Forms.Button()
        Me.btnEXELocation = New System.Windows.Forms.Button()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(96, 11)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Source Root"
        '
        'tbSourceRoot
        '
        Me.tbSourceRoot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceRoot.Location = New System.Drawing.Point(186, 8)
        Me.tbSourceRoot.Margin = New System.Windows.Forms.Padding(2)
        Me.tbSourceRoot.Name = "tbSourceRoot"
        Me.tbSourceRoot.Size = New System.Drawing.Size(484, 20)
        Me.tbSourceRoot.TabIndex = 1
        Me.tbSourceRoot.Text = "C:\Users\albusmw\foxdox"
        '
        'tbDestinationRoot
        '
        Me.tbDestinationRoot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDestinationRoot.Location = New System.Drawing.Point(186, 32)
        Me.tbDestinationRoot.Margin = New System.Windows.Forms.Padding(2)
        Me.tbDestinationRoot.Name = "tbDestinationRoot"
        Me.tbDestinationRoot.Size = New System.Drawing.Size(484, 20)
        Me.tbDestinationRoot.TabIndex = 2
        Me.tbDestinationRoot.Text = "C:\Users\albusmw\Dropbox\Verwaltung\Finanzamt\Share Steuerberater"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(96, 35)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Destination Root"
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Location = New System.Drawing.Point(11, 152)
        Me.tbLog.Margin = New System.Windows.Forms.Padding(2)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(698, 352)
        Me.tbLog.TabIndex = 4
        '
        'tbDropHere
        '
        Me.tbDropHere.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDropHere.Location = New System.Drawing.Point(11, 56)
        Me.tbDropHere.Margin = New System.Windows.Forms.Padding(2)
        Me.tbDropHere.Multiline = True
        Me.tbDropHere.Name = "tbDropHere"
        Me.tbDropHere.Size = New System.Drawing.Size(659, 92)
        Me.tbDropHere.TabIndex = 5
        Me.tbDropHere.Text = "Drop here ..."
        Me.tbDropHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnSource
        '
        Me.btnSource.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSource.Location = New System.Drawing.Point(675, 8)
        Me.btnSource.Name = "btnSource"
        Me.btnSource.Size = New System.Drawing.Size(34, 20)
        Me.btnSource.TabIndex = 6
        Me.btnSource.Text = "..."
        Me.btnSource.UseVisualStyleBackColor = True
        '
        'btnDest
        '
        Me.btnDest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDest.Location = New System.Drawing.Point(675, 32)
        Me.btnDest.Name = "btnDest"
        Me.btnDest.Size = New System.Drawing.Size(34, 20)
        Me.btnDest.TabIndex = 7
        Me.btnDest.Text = "..."
        Me.btnDest.UseVisualStyleBackColor = True
        '
        'btnEXELocation
        '
        Me.btnEXELocation.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEXELocation.Location = New System.Drawing.Point(675, 58)
        Me.btnEXELocation.Name = "btnEXELocation"
        Me.btnEXELocation.Size = New System.Drawing.Size(34, 89)
        Me.btnEXELocation.TabIndex = 8
        Me.btnEXELocation.Text = "..."
        Me.btnEXELocation.UseVisualStyleBackColor = True
        '
        'btnSync
        '
        Me.btnSync.Location = New System.Drawing.Point(11, 8)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(80, 44)
        Me.btnSync.TabIndex = 9
        Me.btnSync.Text = "SYNC"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(715, 510)
        Me.Controls.Add(Me.btnSync)
        Me.Controls.Add(Me.btnEXELocation)
        Me.Controls.Add(Me.btnDest)
        Me.Controls.Add(Me.btnSource)
        Me.Controls.Add(Me.tbDropHere)
        Me.Controls.Add(Me.tbLog)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbDestinationRoot)
        Me.Controls.Add(Me.tbSourceRoot)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "DragDropCopy Version 1.2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents tbSourceRoot As TextBox
    Friend WithEvents tbDestinationRoot As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tbLog As TextBox
    Friend WithEvents tbDropHere As TextBox
    Friend WithEvents btnSource As Button
    Friend WithEvents btnDest As Button
    Friend WithEvents btnEXELocation As Button
    Friend WithEvents btnSync As Button
End Class
