<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSinglePixelStat
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
        Me.tbRootFolder = New System.Windows.Forms.TextBox()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.tbOffsetX = New System.Windows.Forms.TextBox()
        Me.tbOffsetY = New System.Windows.Forms.TextBox()
        Me.tbValues = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'tbRootFolder
        '
        Me.tbRootFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRootFolder.Location = New System.Drawing.Point(106, 12)
        Me.tbRootFolder.Name = "tbRootFolder"
        Me.tbRootFolder.Size = New System.Drawing.Size(913, 20)
        Me.tbRootFolder.TabIndex = 0
        Me.tbRootFolder.Text = "\\192.168.100.10\astro\2021_03_06 (NGC2174)"
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(939, 38)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(80, 46)
        Me.btnRun.TabIndex = 1
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'tbOffsetX
        '
        Me.tbOffsetX.Location = New System.Drawing.Point(106, 38)
        Me.tbOffsetX.Name = "tbOffsetX"
        Me.tbOffsetX.Size = New System.Drawing.Size(172, 20)
        Me.tbOffsetX.TabIndex = 2
        Me.tbOffsetX.Text = "272"
        '
        'tbOffsetY
        '
        Me.tbOffsetY.Location = New System.Drawing.Point(106, 64)
        Me.tbOffsetY.Name = "tbOffsetY"
        Me.tbOffsetY.Size = New System.Drawing.Size(172, 20)
        Me.tbOffsetY.TabIndex = 3
        Me.tbOffsetY.Text = "115"
        '
        'tbValues
        '
        Me.tbValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbValues.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbValues.Location = New System.Drawing.Point(106, 90)
        Me.tbValues.Multiline = True
        Me.tbValues.Name = "tbValues"
        Me.tbValues.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbValues.Size = New System.Drawing.Size(913, 865)
        Me.tbValues.TabIndex = 4
        Me.tbValues.Text = "1000"
        '
        'frmSinglePixelStat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1031, 967)
        Me.Controls.Add(Me.tbValues)
        Me.Controls.Add(Me.tbOffsetY)
        Me.Controls.Add(Me.tbOffsetX)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.tbRootFolder)
        Me.Name = "frmSinglePixelStat"
        Me.Text = "Pixel statistics over multiple files"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbRootFolder As TextBox
    Friend WithEvents btnRun As Button
    Friend WithEvents tbOffsetX As TextBox
    Friend WithEvents tbOffsetY As TextBox
    Friend WithEvents tbValues As TextBox
End Class
