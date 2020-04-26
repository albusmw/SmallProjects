<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDSS
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
        Me.tbSearch = New System.Windows.Forms.TextBox
        Me.tbShowing = New System.Windows.Forms.TextBox
        Me.pbMain = New System.Windows.Forms.PictureBox
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbSearch
        '
        Me.tbSearch.Location = New System.Drawing.Point(12, 9)
        Me.tbSearch.Name = "tbSearch"
        Me.tbSearch.Size = New System.Drawing.Size(83, 20)
        Me.tbSearch.TabIndex = 1
        '
        'tbShowing
        '
        Me.tbShowing.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbShowing.Location = New System.Drawing.Point(101, 9)
        Me.tbShowing.Name = "tbShowing"
        Me.tbShowing.ReadOnly = True
        Me.tbShowing.Size = New System.Drawing.Size(500, 20)
        Me.tbShowing.TabIndex = 2
        '
        'pbMain
        '
        Me.pbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMain.Location = New System.Drawing.Point(12, 35)
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(589, 305)
        Me.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbMain.TabIndex = 3
        Me.pbMain.TabStop = False
        '
        'frmDSS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(613, 352)
        Me.Controls.Add(Me.pbMain)
        Me.Controls.Add(Me.tbShowing)
        Me.Controls.Add(Me.tbSearch)
        Me.Name = "frmDSS"
        Me.Text = "STScI Digitized Sky Survey"
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbSearch As System.Windows.Forms.TextBox
    Friend WithEvents tbShowing As System.Windows.Forms.TextBox
    Friend WithEvents pbMain As System.Windows.Forms.PictureBox
End Class
