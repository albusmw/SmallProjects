<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateVideo
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
        Me.btnSearchFiles = New System.Windows.Forms.Button()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.btnGenerateFileList = New System.Windows.Forms.Button()
        Me.lbSequences = New System.Windows.Forms.ListBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.tsmiFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_DeleteSequenceFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSearchFiles
        '
        Me.btnSearchFiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearchFiles.Location = New System.Drawing.Point(388, 27)
        Me.btnSearchFiles.Name = "btnSearchFiles"
        Me.btnSearchFiles.Size = New System.Drawing.Size(748, 36)
        Me.btnSearchFiles.TabIndex = 0
        Me.btnSearchFiles.Text = "Search files"
        Me.btnSearchFiles.UseVisualStyleBackColor = True
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(12, 27)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(370, 833)
        Me.pgMain.TabIndex = 1
        '
        'btnGenerate
        '
        Me.btnGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerate.Location = New System.Drawing.Point(1042, 814)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(94, 46)
        Me.btnGenerate.TabIndex = 2
        Me.btnGenerate.Text = "Generate video"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'btnGenerateFileList
        '
        Me.btnGenerateFileList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerateFileList.Enabled = False
        Me.btnGenerateFileList.Location = New System.Drawing.Point(388, 814)
        Me.btnGenerateFileList.Name = "btnGenerateFileList"
        Me.btnGenerateFileList.Size = New System.Drawing.Size(648, 46)
        Me.btnGenerateFileList.TabIndex = 3
        Me.btnGenerateFileList.Text = "Generate file list"
        Me.btnGenerateFileList.UseVisualStyleBackColor = True
        '
        'lbSequences
        '
        Me.lbSequences.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbSequences.FormattingEnabled = True
        Me.lbSequences.IntegralHeight = False
        Me.lbSequences.Location = New System.Drawing.Point(388, 69)
        Me.lbSequences.Name = "lbSequences"
        Me.lbSequences.ScrollAlwaysVisible = True
        Me.lbSequences.Size = New System.Drawing.Size(748, 734)
        Me.lbSequences.TabIndex = 4
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1148, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'tsmiFile
        '
        Me.tsmiFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile_DeleteSequenceFiles})
        Me.tsmiFile.Name = "tsmiFile"
        Me.tsmiFile.Size = New System.Drawing.Size(37, 20)
        Me.tsmiFile.Text = "File"
        '
        'tsmiFile_DeleteSequenceFiles
        '
        Me.tsmiFile_DeleteSequenceFiles.Name = "tsmiFile_DeleteSequenceFiles"
        Me.tsmiFile_DeleteSequenceFiles.Size = New System.Drawing.Size(230, 22)
        Me.tsmiFile_DeleteSequenceFiles.Text = "Delete selected sequence files"
        '
        'frmCreateVideo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1148, 872)
        Me.Controls.Add(Me.lbSequences)
        Me.Controls.Add(Me.btnGenerateFileList)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.pgMain)
        Me.Controls.Add(Me.btnSearchFiles)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmCreateVideo"
        Me.Text = "Create video"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnSearchFiles As Button
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents btnGenerate As Button
    Friend WithEvents btnGenerateFileList As Button
    Friend WithEvents lbSequences As ListBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents tsmiFile As ToolStripMenuItem
    Friend WithEvents tsmiFile_DeleteSequenceFiles As ToolStripMenuItem
End Class
