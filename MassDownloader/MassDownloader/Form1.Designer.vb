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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SpecialToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StrassennamenÜberVIVOToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'wbMain
        '
        Me.wbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.wbMain.Location = New System.Drawing.Point(12, 44)
        Me.wbMain.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbMain.Name = "wbMain"
        Me.wbMain.Size = New System.Drawing.Size(652, 646)
        Me.wbMain.TabIndex = 0
        '
        'tbAdr
        '
        Me.tbAdr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdr.Location = New System.Drawing.Point(670, 44)
        Me.tbAdr.Name = "tbAdr"
        Me.tbAdr.Size = New System.Drawing.Size(520, 26)
        Me.tbAdr.TabIndex = 1
        Me.tbAdr.Text = "https://ris.komuna.net/holzkirchen/Meeting.mvc/List"
        '
        'btnLinks
        '
        Me.btnLinks.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLinks.Location = New System.Drawing.Point(670, 108)
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
        Me.tbLinks.Location = New System.Drawing.Point(670, 192)
        Me.tbLinks.Multiline = True
        Me.tbLinks.Name = "tbLinks"
        Me.tbLinks.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLinks.Size = New System.Drawing.Size(520, 498)
        Me.tbLinks.TabIndex = 3
        Me.tbLinks.WordWrap = False
        '
        'tbProgress
        '
        Me.tbProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbProgress.Location = New System.Drawing.Point(670, 160)
        Me.tbProgress.Name = "tbProgress"
        Me.tbProgress.ReadOnly = True
        Me.tbProgress.Size = New System.Drawing.Size(520, 26)
        Me.tbProgress.TabIndex = 4
        '
        'MenuStrip1
        '
        Me.MenuStrip1.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SpecialToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1202, 33)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SpecialToolStripMenuItem
        '
        Me.SpecialToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StrassennamenÜberVIVOToolStripMenuItem})
        Me.SpecialToolStripMenuItem.Name = "SpecialToolStripMenuItem"
        Me.SpecialToolStripMenuItem.Size = New System.Drawing.Size(83, 29)
        Me.SpecialToolStripMenuItem.Text = "Special"
        '
        'StrassennamenÜberVIVOToolStripMenuItem
        '
        Me.StrassennamenÜberVIVOToolStripMenuItem.Name = "StrassennamenÜberVIVOToolStripMenuItem"
        Me.StrassennamenÜberVIVOToolStripMenuItem.Size = New System.Drawing.Size(320, 34)
        Me.StrassennamenÜberVIVOToolStripMenuItem.Text = "Strassennamen über VIVO"
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
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainForm"
        Me.Text = "Mass downloader"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents wbMain As WebBrowser
    Friend WithEvents tbAdr As TextBox
    Friend WithEvents btnLinks As Button
    Friend WithEvents tbLinks As TextBox
    Friend WithEvents tbProgress As TextBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents SpecialToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StrassennamenÜberVIVOToolStripMenuItem As ToolStripMenuItem
End Class
