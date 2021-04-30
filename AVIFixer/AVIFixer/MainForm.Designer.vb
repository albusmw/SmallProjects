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
        Me.gbBasics = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbInitialOffset = New System.Windows.Forms.TextBox()
        Me.tbWorkFolder = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbInputAVIFile = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbHeight = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbWidth = New System.Windows.Forms.TextBox()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_LoadChunks = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFile_Start = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_Stop = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFile_RunFFMPEG = New System.Windows.Forms.ToolStripMenuItem()
        Me.gbPicture = New System.Windows.Forms.GroupBox()
        Me.pbImage = New System.Windows.Forms.PictureBox()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tbCommand = New System.Windows.Forms.TextBox()
        Me.tspbMain = New System.Windows.Forms.ToolStripProgressBar()
        Me.tsslErrorMessage = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gbBasics.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.gbPicture.SuspendLayout()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ssMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbBasics
        '
        Me.gbBasics.Controls.Add(Me.Label5)
        Me.gbBasics.Controls.Add(Me.tbInitialOffset)
        Me.gbBasics.Controls.Add(Me.tbWorkFolder)
        Me.gbBasics.Controls.Add(Me.Label4)
        Me.gbBasics.Controls.Add(Me.Label3)
        Me.gbBasics.Controls.Add(Me.tbInputAVIFile)
        Me.gbBasics.Controls.Add(Me.Label2)
        Me.gbBasics.Controls.Add(Me.tbHeight)
        Me.gbBasics.Controls.Add(Me.Label1)
        Me.gbBasics.Controls.Add(Me.tbWidth)
        Me.gbBasics.Location = New System.Drawing.Point(12, 40)
        Me.gbBasics.Name = "gbBasics"
        Me.gbBasics.Size = New System.Drawing.Size(505, 161)
        Me.gbBasics.TabIndex = 6
        Me.gbBasics.TabStop = False
        Me.gbBasics.Text = "Basic parameters"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 126)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "1st frame offset"
        '
        'tbInitialOffset
        '
        Me.tbInitialOffset.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInitialOffset.Location = New System.Drawing.Point(383, 123)
        Me.tbInitialOffset.Name = "tbInitialOffset"
        Me.tbInitialOffset.Size = New System.Drawing.Size(116, 20)
        Me.tbInitialOffset.TabIndex = 8
        Me.tbInitialOffset.Text = "66056"
        '
        'tbWorkFolder
        '
        Me.tbWorkFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbWorkFolder.Location = New System.Drawing.Point(102, 19)
        Me.tbWorkFolder.Name = "tbWorkFolder"
        Me.tbWorkFolder.Size = New System.Drawing.Size(397, 20)
        Me.tbWorkFolder.TabIndex = 7
        Me.tbWorkFolder.Text = "C:\Users\albusmw\Downloads\Mehmet"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 22)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Work folder"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Image file"
        '
        'tbInputAVIFile
        '
        Me.tbInputAVIFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInputAVIFile.Location = New System.Drawing.Point(383, 45)
        Me.tbInputAVIFile.Name = "tbInputAVIFile"
        Me.tbInputAVIFile.Size = New System.Drawing.Size(116, 20)
        Me.tbInputAVIFile.TabIndex = 4
        Me.tbInputAVIFile.Text = "16_44_22.avi"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Image height"
        '
        'tbHeight
        '
        Me.tbHeight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbHeight.Location = New System.Drawing.Point(383, 97)
        Me.tbHeight.Name = "tbHeight"
        Me.tbHeight.Size = New System.Drawing.Size(116, 20)
        Me.tbHeight.TabIndex = 2
        Me.tbHeight.Text = "2078"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 74)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Image width"
        '
        'tbWidth
        '
        Me.tbWidth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbWidth.Location = New System.Drawing.Point(383, 71)
        Me.tbWidth.Name = "tbWidth"
        Me.tbWidth.Size = New System.Drawing.Size(116, 20)
        Me.tbWidth.TabIndex = 0
        Me.tbWidth.Text = "3096"
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(870, 24)
        Me.msMain.TabIndex = 7
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile_LoadChunks, Me.ToolStripMenuItem1, Me.tsmiFile_Start, Me.tsmiFile_Stop, Me.ToolStripMenuItem2, Me.tsmiFile_RunFFMPEG})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'tsmiFile_LoadChunks
        '
        Me.tsmiFile_LoadChunks.Name = "tsmiFile_LoadChunks"
        Me.tsmiFile_LoadChunks.Size = New System.Drawing.Size(187, 22)
        Me.tsmiFile_LoadChunks.Text = "Load chunk index file"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(184, 6)
        '
        'tsmiFile_Start
        '
        Me.tsmiFile_Start.Name = "tsmiFile_Start"
        Me.tsmiFile_Start.Size = New System.Drawing.Size(187, 22)
        Me.tsmiFile_Start.Text = "Start recovery"
        '
        'tsmiFile_Stop
        '
        Me.tsmiFile_Stop.Name = "tsmiFile_Stop"
        Me.tsmiFile_Stop.Size = New System.Drawing.Size(187, 22)
        Me.tsmiFile_Stop.Text = "Stop"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(184, 6)
        '
        'tsmiFile_RunFFMPEG
        '
        Me.tsmiFile_RunFFMPEG.Name = "tsmiFile_RunFFMPEG"
        Me.tsmiFile_RunFFMPEG.Size = New System.Drawing.Size(187, 22)
        Me.tsmiFile_RunFFMPEG.Text = "Run ffmpeg"
        '
        'gbPicture
        '
        Me.gbPicture.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPicture.Controls.Add(Me.pbImage)
        Me.gbPicture.Location = New System.Drawing.Point(12, 207)
        Me.gbPicture.Name = "gbPicture"
        Me.gbPicture.Size = New System.Drawing.Size(846, 485)
        Me.gbPicture.TabIndex = 8
        Me.gbPicture.TabStop = False
        Me.gbPicture.Text = "Recovered image"
        '
        'pbImage
        '
        Me.pbImage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbImage.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pbImage.Location = New System.Drawing.Point(6, 19)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(834, 451)
        Me.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbImage.TabIndex = 0
        Me.pbImage.TabStop = False
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tspbMain, Me.tsslErrorMessage})
        Me.ssMain.Location = New System.Drawing.Point(0, 782)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(870, 22)
        Me.ssMain.TabIndex = 9
        Me.ssMain.Text = "StatusStrip1"
        '
        'tbCommand
        '
        Me.tbCommand.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCommand.Location = New System.Drawing.Point(12, 710)
        Me.tbCommand.Name = "tbCommand"
        Me.tbCommand.Size = New System.Drawing.Size(846, 20)
        Me.tbCommand.TabIndex = 10
        '
        'tspbMain
        '
        Me.tspbMain.Name = "tspbMain"
        Me.tspbMain.Size = New System.Drawing.Size(100, 16)
        '
        'tsslErrorMessage
        '
        Me.tsslErrorMessage.ForeColor = System.Drawing.Color.Red
        Me.tsslErrorMessage.Name = "tsslErrorMessage"
        Me.tsslErrorMessage.Size = New System.Drawing.Size(22, 17)
        Me.tsslErrorMessage.Text = "---"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(870, 804)
        Me.Controls.Add(Me.tbCommand)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.gbPicture)
        Me.Controls.Add(Me.gbBasics)
        Me.Controls.Add(Me.msMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "AVIFixer"
        Me.gbBasics.ResumeLayout(False)
        Me.gbBasics.PerformLayout()
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.gbPicture.ResumeLayout(False)
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbBasics As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents tbInputAVIFile As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tbHeight As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents tbWidth As TextBox
    Friend WithEvents tbWorkFolder As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents msMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiFile_Start As ToolStripMenuItem
    Friend WithEvents tsmiFile_Stop As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents tsmiFile_LoadChunks As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents tsmiFile_RunFFMPEG As ToolStripMenuItem
    Friend WithEvents gbPicture As GroupBox
    Friend WithEvents pbImage As PictureBox
    Friend WithEvents tbInitialOffset As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tspbMain As ToolStripProgressBar
    Friend WithEvents tbCommand As TextBox
    Friend WithEvents tsslErrorMessage As ToolStripStatusLabel
End Class
