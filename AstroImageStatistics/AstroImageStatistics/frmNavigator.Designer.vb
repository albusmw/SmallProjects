<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNavigator
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
        Me.tbRootFile = New System.Windows.Forms.TextBox()
        Me.tbFilterString = New System.Windows.Forms.TextBox()
        Me.pbMain = New System.Windows.Forms.ProgressBar()
        Me.tbOffsetX = New System.Windows.Forms.TextBox()
        Me.tbOffsetY = New System.Windows.Forms.TextBox()
        Me.tbTileSize = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lbPixel = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'tbRootFile
        '
        Me.tbRootFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRootFile.Location = New System.Drawing.Point(55, 12)
        Me.tbRootFile.Name = "tbRootFile"
        Me.tbRootFile.Size = New System.Drawing.Size(411, 20)
        Me.tbRootFile.TabIndex = 0
        '
        'tbFilterString
        '
        Me.tbFilterString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFilterString.Location = New System.Drawing.Point(55, 38)
        Me.tbFilterString.Name = "tbFilterString"
        Me.tbFilterString.Size = New System.Drawing.Size(411, 20)
        Me.tbFilterString.TabIndex = 1
        Me.tbFilterString.Text = "QHY600_L_*.*"
        '
        'pbMain
        '
        Me.pbMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMain.Location = New System.Drawing.Point(55, 121)
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(411, 21)
        Me.pbMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbMain.TabIndex = 2
        '
        'tbOffsetX
        '
        Me.tbOffsetX.Location = New System.Drawing.Point(55, 69)
        Me.tbOffsetX.Name = "tbOffsetX"
        Me.tbOffsetX.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetX.TabIndex = 3
        Me.tbOffsetX.Text = "2000"
        '
        'tbOffsetY
        '
        Me.tbOffsetY.Location = New System.Drawing.Point(55, 95)
        Me.tbOffsetY.Name = "tbOffsetY"
        Me.tbOffsetY.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetY.TabIndex = 4
        Me.tbOffsetY.Text = "3000"
        '
        'tbTileSize
        '
        Me.tbTileSize.Location = New System.Drawing.Point(178, 81)
        Me.tbTileSize.Name = "tbTileSize"
        Me.tbTileSize.Size = New System.Drawing.Size(44, 20)
        Me.tbTileSize.TabIndex = 5
        Me.tbTileSize.Text = "100"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Root file"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Filter"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Offset X"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 98)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Offset Y"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(127, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Tile size"
        '
        'lbPixel
        '
        Me.lbPixel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbPixel.FormattingEnabled = True
        Me.lbPixel.IntegralHeight = False
        Me.lbPixel.Location = New System.Drawing.Point(55, 148)
        Me.lbPixel.Name = "lbPixel"
        Me.lbPixel.ScrollAlwaysVisible = True
        Me.lbPixel.Size = New System.Drawing.Size(411, 576)
        Me.lbPixel.TabIndex = 11
        '
        'frmNavigator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 734)
        Me.Controls.Add(Me.lbPixel)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbTileSize)
        Me.Controls.Add(Me.tbOffsetY)
        Me.Controls.Add(Me.tbOffsetX)
        Me.Controls.Add(Me.pbMain)
        Me.Controls.Add(Me.tbFilterString)
        Me.Controls.Add(Me.tbRootFile)
        Me.KeyPreview = True
        Me.Name = "frmNavigator"
        Me.Text = "Navigator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbRootFile As TextBox
    Friend WithEvents tbFilterString As TextBox
    Friend WithEvents pbMain As ProgressBar
    Friend WithEvents tbOffsetX As TextBox
    Friend WithEvents tbOffsetY As TextBox
    Friend WithEvents tbTileSize As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lbPixel As ListBox
End Class
