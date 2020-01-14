<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.wbMain = New System.Windows.Forms.WebBrowser()
        Me.tbAdr = New System.Windows.Forms.TextBox()
        Me.btnLinks = New System.Windows.Forms.Button()
        Me.tbLinks = New System.Windows.Forms.TextBox()
        Me.tbProgress = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'wbMain
        '
        Me.wbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.wbMain.Location = New System.Drawing.Point(12, 12)
        Me.wbMain.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbMain.Name = "wbMain"
        Me.wbMain.Size = New System.Drawing.Size(652, 678)
        Me.wbMain.TabIndex = 0
        '
        'tbAdr
        '
        Me.tbAdr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdr.Location = New System.Drawing.Point(670, 12)
        Me.tbAdr.Name = "tbAdr"
        Me.tbAdr.Size = New System.Drawing.Size(520, 26)
        Me.tbAdr.TabIndex = 1
        Me.tbAdr.Text = "https://ris.komuna.net/holzkirchen/Meeting.mvc/List"
        '
        'btnLinks
        '
        Me.btnLinks.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLinks.Location = New System.Drawing.Point(670, 44)
        Me.btnLinks.Name = "btnLinks"
        Me.btnLinks.Size = New System.Drawing.Size(520, 46)
        Me.btnLinks.TabIndex = 2
        Me.btnLinks.Text = "Get all links"
        Me.btnLinks.UseVisualStyleBackColor = True
        '
        'tbLinks
        '
        Me.tbLinks.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLinks.Font = New System.Drawing.Font("Courier New", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLinks.Location = New System.Drawing.Point(670, 132)
        Me.tbLinks.Multiline = True
        Me.tbLinks.Name = "tbLinks"
        Me.tbLinks.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLinks.Size = New System.Drawing.Size(520, 558)
        Me.tbLinks.TabIndex = 3
        Me.tbLinks.WordWrap = False
        '
        'tbProgress
        '
        Me.tbProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbProgress.Location = New System.Drawing.Point(670, 96)
        Me.tbProgress.Name = "tbProgress"
        Me.tbProgress.ReadOnly = True
        Me.tbProgress.Size = New System.Drawing.Size(520, 26)
        Me.tbProgress.TabIndex = 4
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1202, 702)
        Me.Controls.Add(Me.tbProgress)
        Me.Controls.Add(Me.tbLinks)
        Me.Controls.Add(Me.btnLinks)
        Me.Controls.Add(Me.tbAdr)
        Me.Controls.Add(Me.wbMain)
        Me.Name = "MainForm"
        Me.Text = "Mass downloader"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents wbMain As WebBrowser
    Friend WithEvents tbAdr As TextBox
    Friend WithEvents btnLinks As Button
    Friend WithEvents tbLinks As TextBox
    Friend WithEvents tbProgress As TextBox
End Class
