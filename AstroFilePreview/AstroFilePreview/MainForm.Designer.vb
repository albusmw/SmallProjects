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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.ilIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.lvFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.scFiles = New System.Windows.Forms.SplitContainer()
        Me.tvDirTree = New System.Windows.Forms.TreeView()
        Me.pbPreview = New System.Windows.Forms.PictureBox()
        CType(Me.scFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scFiles.Panel1.SuspendLayout()
        Me.scFiles.Panel2.SuspendLayout()
        Me.scFiles.SuspendLayout()
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ilIcons
        '
        Me.ilIcons.ImageStream = CType(resources.GetObject("ilIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.ilIcons.Images.SetKeyName(0, "drive")
        Me.ilIcons.Images.SetKeyName(1, "folder")
        Me.ilIcons.Images.SetKeyName(2, "file")
        '
        'lvFiles
        '
        Me.lvFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvFiles.GridLines = True
        Me.lvFiles.HideSelection = False
        Me.lvFiles.Location = New System.Drawing.Point(0, 0)
        Me.lvFiles.Name = "lvFiles"
        Me.lvFiles.Size = New System.Drawing.Size(439, 596)
        Me.lvFiles.SmallImageList = Me.ilIcons
        Me.lvFiles.TabIndex = 1
        Me.lvFiles.UseCompatibleStateImageBehavior = False
        Me.lvFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Type"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Last modified"
        '
        'scFiles
        '
        Me.scFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scFiles.Location = New System.Drawing.Point(12, 12)
        Me.scFiles.Name = "scFiles"
        '
        'scFiles.Panel1
        '
        Me.scFiles.Panel1.Controls.Add(Me.tvDirTree)
        '
        'scFiles.Panel2
        '
        Me.scFiles.Panel2.Controls.Add(Me.lvFiles)
        Me.scFiles.Size = New System.Drawing.Size(663, 596)
        Me.scFiles.SplitterDistance = 220
        Me.scFiles.TabIndex = 2
        '
        'tvDirTree
        '
        Me.tvDirTree.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvDirTree.ImageIndex = 1
        Me.tvDirTree.ImageList = Me.ilIcons
        Me.tvDirTree.Location = New System.Drawing.Point(3, 3)
        Me.tvDirTree.Name = "tvDirTree"
        Me.tvDirTree.SelectedImageIndex = 0
        Me.tvDirTree.Size = New System.Drawing.Size(214, 590)
        Me.tvDirTree.TabIndex = 0
        '
        'pbPreview
        '
        Me.pbPreview.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPreview.Location = New System.Drawing.Point(681, 15)
        Me.pbPreview.Name = "pbPreview"
        Me.pbPreview.Size = New System.Drawing.Size(506, 593)
        Me.pbPreview.TabIndex = 3
        Me.pbPreview.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1199, 620)
        Me.Controls.Add(Me.pbPreview)
        Me.Controls.Add(Me.scFiles)
        Me.Name = "MainForm"
        Me.Text = "AstroImagePreview"
        Me.scFiles.Panel1.ResumeLayout(False)
        Me.scFiles.Panel2.ResumeLayout(False)
        CType(Me.scFiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scFiles.ResumeLayout(False)
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvFiles As ListView
    Friend WithEvents scFiles As SplitContainer
    Friend WithEvents ilIcons As ImageList
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents tvDirTree As TreeView
    Friend WithEvents pbPreview As PictureBox
End Class
