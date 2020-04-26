<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLiveImage
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SunSOHOToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SDOHMIContinuumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pbMain = New System.Windows.Forms.PictureBox
        Me.SeeingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TimesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ImageToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(292, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ImageToolStripMenuItem
        '
        Me.ImageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SunSOHOToolStripMenuItem, Me.SeeingToolStripMenuItem})
        Me.ImageToolStripMenuItem.Name = "ImageToolStripMenuItem"
        Me.ImageToolStripMenuItem.Size = New System.Drawing.Size(49, 20)
        Me.ImageToolStripMenuItem.Text = "Image"
        '
        'SunSOHOToolStripMenuItem
        '
        Me.SunSOHOToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SDOHMIContinuumToolStripMenuItem})
        Me.SunSOHOToolStripMenuItem.Name = "SunSOHOToolStripMenuItem"
        Me.SunSOHOToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SunSOHOToolStripMenuItem.Text = "Sun - SOHO"
        '
        'SDOHMIContinuumToolStripMenuItem
        '
        Me.SDOHMIContinuumToolStripMenuItem.Name = "SDOHMIContinuumToolStripMenuItem"
        Me.SDOHMIContinuumToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.SDOHMIContinuumToolStripMenuItem.Text = "SDO/HMI Continuum"
        '
        'pbMain
        '
        Me.pbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMain.Location = New System.Drawing.Point(0, 27)
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(292, 245)
        Me.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbMain.TabIndex = 1
        Me.pbMain.TabStop = False
        '
        'SeeingToolStripMenuItem
        '
        Me.SeeingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TimesToolStripMenuItem})
        Me.SeeingToolStripMenuItem.Name = "SeeingToolStripMenuItem"
        Me.SeeingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SeeingToolStripMenuItem.Text = "Seeing"
        '
        'TimesToolStripMenuItem
        '
        Me.TimesToolStripMenuItem.Name = "TimesToolStripMenuItem"
        Me.TimesToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.TimesToolStripMenuItem.Text = "7Times"
        '
        'frmLiveImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Controls.Add(Me.pbMain)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmLiveImage"
        Me.Text = "Live image"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.pbMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SunSOHOToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SDOHMIContinuumToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pbMain As System.Windows.Forms.PictureBox
    Friend WithEvents SeeingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
