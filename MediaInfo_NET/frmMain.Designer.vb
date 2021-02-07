Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

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

    <STAThread()> _
    Public Shared Sub Main()
        Application.Run(MainForm)
    End Sub

    ' Fields
    Friend WithEvents btnCopy As Button
    Friend WithEvents btnRootDir As Button
    Friend WithEvents btnStart As Button
    Friend WithEvents fbdMain As FolderBrowserDialog
    Friend WithEvents lbRenaming As ListBox
    Friend WithEvents tbRootDir As TextBox

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.tbRootDir = New System.Windows.Forms.TextBox()
        Me.btnRootDir = New System.Windows.Forms.Button()
        Me.fbdMain = New System.Windows.Forms.FolderBrowserDialog()
        Me.lbRenaming = New System.Windows.Forms.ListBox()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Location = New System.Drawing.Point(683, 268)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(86, 35)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'tbRootDir
        '
        Me.tbRootDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRootDir.Location = New System.Drawing.Point(12, 12)
        Me.tbRootDir.Name = "tbRootDir"
        Me.tbRootDir.Size = New System.Drawing.Size(713, 20)
        Me.tbRootDir.TabIndex = 1
        Me.tbRootDir.Text = "G:\HD"
        '
        'btnRootDir
        '
        Me.btnRootDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRootDir.Location = New System.Drawing.Point(731, 11)
        Me.btnRootDir.Name = "btnRootDir"
        Me.btnRootDir.Size = New System.Drawing.Size(38, 21)
        Me.btnRootDir.TabIndex = 2
        Me.btnRootDir.Text = "..."
        Me.btnRootDir.UseVisualStyleBackColor = True
        '
        'lbRenaming
        '
        Me.lbRenaming.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbRenaming.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbRenaming.FormattingEnabled = True
        Me.lbRenaming.IntegralHeight = False
        Me.lbRenaming.ItemHeight = 14
        Me.lbRenaming.Location = New System.Drawing.Point(12, 38)
        Me.lbRenaming.Name = "lbRenaming"
        Me.lbRenaming.Size = New System.Drawing.Size(665, 265)
        Me.lbRenaming.TabIndex = 3
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.Location = New System.Drawing.Point(683, 201)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(86, 61)
        Me.btnCopy.TabIndex = 5
        Me.btnCopy.Text = "Copy list" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "to clipboard"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.ClientSize = New System.Drawing.Size(781, 315)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.lbRenaming)
        Me.Controls.Add(Me.btnRootDir)
        Me.Controls.Add(Me.tbRootDir)
        Me.Controls.Add(Me.btnStart)
        Me.Name = "MainForm"
        Me.Text = "MediaInfo File Scanner 1.0"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class
