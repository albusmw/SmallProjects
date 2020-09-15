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
        Me.tbOffsetX = New System.Windows.Forms.TextBox()
        Me.tbOffsetY = New System.Windows.Forms.TextBox()
        Me.tbTileSize = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lbPixel = New System.Windows.Forms.ListBox()
        Me.cbSaveFITS = New System.Windows.Forms.CheckBox()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pbMain = New System.Windows.Forms.ToolStripProgressBar()
        Me.cbColorModes = New System.Windows.Forms.ComboBox()
        Me.tbStatResult = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ssMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbRootFile
        '
        Me.tbRootFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRootFile.Location = New System.Drawing.Point(76, 12)
        Me.tbRootFile.Name = "tbRootFile"
        Me.tbRootFile.Size = New System.Drawing.Size(1120, 20)
        Me.tbRootFile.TabIndex = 0
        '
        'tbFilterString
        '
        Me.tbFilterString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFilterString.Location = New System.Drawing.Point(76, 38)
        Me.tbFilterString.Name = "tbFilterString"
        Me.tbFilterString.Size = New System.Drawing.Size(1120, 20)
        Me.tbFilterString.TabIndex = 1
        Me.tbFilterString.Text = "QHY600_L_*.*"
        '
        'tbOffsetX
        '
        Me.tbOffsetX.Location = New System.Drawing.Point(76, 64)
        Me.tbOffsetX.Name = "tbOffsetX"
        Me.tbOffsetX.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetX.TabIndex = 3
        Me.tbOffsetX.Text = "2000"
        Me.tbOffsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbOffsetY
        '
        Me.tbOffsetY.Location = New System.Drawing.Point(76, 90)
        Me.tbOffsetY.Name = "tbOffsetY"
        Me.tbOffsetY.Size = New System.Drawing.Size(66, 20)
        Me.tbOffsetY.TabIndex = 4
        Me.tbOffsetY.Text = "3000"
        Me.tbOffsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbTileSize
        '
        Me.tbTileSize.Location = New System.Drawing.Point(76, 116)
        Me.tbTileSize.Name = "tbTileSize"
        Me.tbTileSize.Size = New System.Drawing.Size(66, 20)
        Me.tbTileSize.TabIndex = 5
        Me.tbTileSize.Text = "100"
        Me.tbTileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Root file"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "File Filter"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "ROI Offset X"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "ROI Offset Y"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "ROI Tile size"
        '
        'lbPixel
        '
        Me.lbPixel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbPixel.FormattingEnabled = True
        Me.lbPixel.IntegralHeight = False
        Me.lbPixel.Location = New System.Drawing.Point(76, 900)
        Me.lbPixel.Name = "lbPixel"
        Me.lbPixel.ScrollAlwaysVisible = True
        Me.lbPixel.Size = New System.Drawing.Size(1120, 298)
        Me.lbPixel.TabIndex = 11
        '
        'cbSaveFITS
        '
        Me.cbSaveFITS.AutoSize = True
        Me.cbSaveFITS.Location = New System.Drawing.Point(76, 142)
        Me.cbSaveFITS.Name = "cbSaveFITS"
        Me.cbSaveFITS.Size = New System.Drawing.Size(144, 17)
        Me.cbSaveFITS.TabIndex = 12
        Me.cbSaveFITS.Text = "Save Mosaik as FITS file"
        Me.cbSaveFITS.UseVisualStyleBackColor = True
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslStatus, Me.pbMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 1218)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(1208, 22)
        Me.ssMain.TabIndex = 13
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslStatus
        '
        Me.tsslStatus.Name = "tsslStatus"
        Me.tsslStatus.Size = New System.Drawing.Size(22, 17)
        Me.tsslStatus.Text = "---"
        Me.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pbMain
        '
        Me.pbMain.Name = "pbMain"
        Me.pbMain.Size = New System.Drawing.Size(100, 16)
        '
        'cbColorModes
        '
        Me.cbColorModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColorModes.FormattingEnabled = True
        Me.cbColorModes.Items.AddRange(New Object() {"Gray", "Hot", "Jet", "Bone", "False-Color", "False-Color HSL"})
        Me.cbColorModes.Location = New System.Drawing.Point(148, 116)
        Me.cbColorModes.Name = "cbColorModes"
        Me.cbColorModes.Size = New System.Drawing.Size(134, 21)
        Me.cbColorModes.TabIndex = 14
        '
        'tbStatResult
        '
        Me.tbStatResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbStatResult.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbStatResult.Location = New System.Drawing.Point(76, 165)
        Me.tbStatResult.Multiline = True
        Me.tbStatResult.Name = "tbStatResult"
        Me.tbStatResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.tbStatResult.Size = New System.Drawing.Size(1120, 729)
        Me.tbStatResult.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 168)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Statistics"
        '
        'frmNavigator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1208, 1240)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbStatResult)
        Me.Controls.Add(Me.cbColorModes)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.cbSaveFITS)
        Me.Controls.Add(Me.lbPixel)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbTileSize)
        Me.Controls.Add(Me.tbOffsetY)
        Me.Controls.Add(Me.tbOffsetX)
        Me.Controls.Add(Me.tbFilterString)
        Me.Controls.Add(Me.tbRootFile)
        Me.KeyPreview = True
        Me.Name = "frmNavigator"
        Me.Text = "Navigator"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbRootFile As TextBox
    Friend WithEvents tbFilterString As TextBox
    Friend WithEvents tbOffsetX As TextBox
    Friend WithEvents tbOffsetY As TextBox
    Friend WithEvents tbTileSize As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lbPixel As ListBox
    Friend WithEvents cbSaveFITS As CheckBox
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tsslStatus As ToolStripStatusLabel
    Friend WithEvents pbMain As ToolStripProgressBar
    Friend WithEvents cbColorModes As ComboBox
    Friend WithEvents tbStatResult As TextBox
    Friend WithEvents Label6 As Label
End Class
