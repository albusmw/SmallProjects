<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseProxyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExternalDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DSSRequestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadConstellationConfigurationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IAUConstellationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SkyViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CoordinateEntryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LiveImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CatalogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WIKIConstellationPagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HenryDraperCatalogueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConstallationBoundariesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslCurrently = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HorizonToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ExternalDataToolStripMenuItem, Me.NewToolStripMenuItem, Me.UpdateToolStripMenuItem, Me.TestToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(906, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UseProxyToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'UseProxyToolStripMenuItem
        '
        Me.UseProxyToolStripMenuItem.CheckOnClick = True
        Me.UseProxyToolStripMenuItem.Name = "UseProxyToolStripMenuItem"
        Me.UseProxyToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.UseProxyToolStripMenuItem.Text = "Use proxy"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ExternalDataToolStripMenuItem
        '
        Me.ExternalDataToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DSSRequestToolStripMenuItem, Me.LoadConstellationConfigurationsToolStripMenuItem, Me.IAUConstellationsToolStripMenuItem})
        Me.ExternalDataToolStripMenuItem.Name = "ExternalDataToolStripMenuItem"
        Me.ExternalDataToolStripMenuItem.Size = New System.Drawing.Size(88, 20)
        Me.ExternalDataToolStripMenuItem.Text = "External Data"
        '
        'DSSRequestToolStripMenuItem
        '
        Me.DSSRequestToolStripMenuItem.Name = "DSSRequestToolStripMenuItem"
        Me.DSSRequestToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.DSSRequestToolStripMenuItem.Text = "STScI Digitized Sky Survey"
        '
        'LoadConstellationConfigurationsToolStripMenuItem
        '
        Me.LoadConstellationConfigurationsToolStripMenuItem.Name = "LoadConstellationConfigurationsToolStripMenuItem"
        Me.LoadConstellationConfigurationsToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.LoadConstellationConfigurationsToolStripMenuItem.Text = "Load constellation configurations"
        '
        'IAUConstellationsToolStripMenuItem
        '
        Me.IAUConstellationsToolStripMenuItem.Name = "IAUConstellationsToolStripMenuItem"
        Me.IAUConstellationsToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.IAUConstellationsToolStripMenuItem.Text = "IAU - Constellations"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SkyViewToolStripMenuItem, Me.CoordinateEntryToolStripMenuItem, Me.LiveImageToolStripMenuItem})
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'SkyViewToolStripMenuItem
        '
        Me.SkyViewToolStripMenuItem.Name = "SkyViewToolStripMenuItem"
        Me.SkyViewToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SkyViewToolStripMenuItem.Text = "Sky view"
        '
        'CoordinateEntryToolStripMenuItem
        '
        Me.CoordinateEntryToolStripMenuItem.Name = "CoordinateEntryToolStripMenuItem"
        Me.CoordinateEntryToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CoordinateEntryToolStripMenuItem.Text = "Coordinate entry"
        '
        'LiveImageToolStripMenuItem
        '
        Me.LiveImageToolStripMenuItem.Name = "LiveImageToolStripMenuItem"
        Me.LiveImageToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.LiveImageToolStripMenuItem.Text = "Live image"
        '
        'UpdateToolStripMenuItem
        '
        Me.UpdateToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CatalogToolStripMenuItem})
        Me.UpdateToolStripMenuItem.Name = "UpdateToolStripMenuItem"
        Me.UpdateToolStripMenuItem.Size = New System.Drawing.Size(57, 20)
        Me.UpdateToolStripMenuItem.Text = "Update"
        '
        'CatalogToolStripMenuItem
        '
        Me.CatalogToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WIKIConstellationPagesToolStripMenuItem, Me.HenryDraperCatalogueToolStripMenuItem, Me.ConstallationBoundariesToolStripMenuItem})
        Me.CatalogToolStripMenuItem.Name = "CatalogToolStripMenuItem"
        Me.CatalogToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CatalogToolStripMenuItem.Text = "Catalog"
        '
        'WIKIConstellationPagesToolStripMenuItem
        '
        Me.WIKIConstellationPagesToolStripMenuItem.Name = "WIKIConstellationPagesToolStripMenuItem"
        Me.WIKIConstellationPagesToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.WIKIConstellationPagesToolStripMenuItem.Text = "WIKI constellation pages"
        '
        'HenryDraperCatalogueToolStripMenuItem
        '
        Me.HenryDraperCatalogueToolStripMenuItem.Name = "HenryDraperCatalogueToolStripMenuItem"
        Me.HenryDraperCatalogueToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.HenryDraperCatalogueToolStripMenuItem.Text = "Henry Draper Catalogue"
        '
        'ConstallationBoundariesToolStripMenuItem
        '
        Me.ConstallationBoundariesToolStripMenuItem.Name = "ConstallationBoundariesToolStripMenuItem"
        Me.ConstallationBoundariesToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.ConstallationBoundariesToolStripMenuItem.Text = "Constallation boundaries"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.tsslCurrently})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 473)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(906, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(59, 17)
        Me.ToolStripStatusLabel1.Text = "Currently:"
        '
        'tsslCurrently
        '
        Me.tsslCurrently.BackColor = System.Drawing.SystemColors.Control
        Me.tsslCurrently.Name = "tsslCurrently"
        Me.tsslCurrently.Size = New System.Drawing.Size(22, 17)
        Me.tsslCurrently.Text = "---"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HorizonToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.TestToolStripMenuItem.Text = "Test"
        '
        'HorizonToolStripMenuItem
        '
        Me.HorizonToolStripMenuItem.Name = "HorizonToolStripMenuItem"
        Me.HorizonToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.HorizonToolStripMenuItem.Text = "Horizon"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(906, 495)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainForm"
        Me.Text = "AstroCalc.NET"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ExternalDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DSSRequestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SkyViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadConstellationConfigurationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CoordinateEntryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LiveImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CatalogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WIKIConstellationPagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsslCurrently As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents UseProxyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents HenryDraperCatalogueToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ConstallationBoundariesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents IAUConstellationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HorizonToolStripMenuItem As ToolStripMenuItem
End Class
