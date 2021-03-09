<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.tUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.scOnOff = New System.Windows.Forms.SplitContainer()
        Me.btnOff = New System.Windows.Forms.Button()
        Me.btnOn = New System.Windows.Forms.Button()
        Me.tbCOMPort = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbnStart = New System.Windows.Forms.Button()
        CType(Me.scOnOff, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scOnOff.Panel1.SuspendLayout()
        Me.scOnOff.Panel2.SuspendLayout()
        Me.scOnOff.SuspendLayout()
        Me.SuspendLayout()
        '
        'tUpdate
        '
        Me.tUpdate.Interval = 500
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgMain.HelpVisible = False
        Me.pgMain.Location = New System.Drawing.Point(12, 38)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(283, 411)
        Me.pgMain.TabIndex = 0
        Me.pgMain.ToolbarVisible = False
        '
        'scOnOff
        '
        Me.scOnOff.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scOnOff.Location = New System.Drawing.Point(12, 455)
        Me.scOnOff.Name = "scOnOff"
        '
        'scOnOff.Panel1
        '
        Me.scOnOff.Panel1.Controls.Add(Me.btnOff)
        '
        'scOnOff.Panel2
        '
        Me.scOnOff.Panel2.Controls.Add(Me.btnOn)
        Me.scOnOff.Size = New System.Drawing.Size(283, 46)
        Me.scOnOff.SplitterDistance = 141
        Me.scOnOff.SplitterWidth = 1
        Me.scOnOff.TabIndex = 1
        '
        'btnOff
        '
        Me.btnOff.BackColor = System.Drawing.Color.Red
        Me.btnOff.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnOff.Location = New System.Drawing.Point(0, 0)
        Me.btnOff.Name = "btnOff"
        Me.btnOff.Size = New System.Drawing.Size(141, 46)
        Me.btnOff.TabIndex = 0
        Me.btnOff.Text = "Off"
        Me.btnOff.UseVisualStyleBackColor = False
        '
        'btnOn
        '
        Me.btnOn.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnOn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnOn.ForeColor = System.Drawing.Color.Black
        Me.btnOn.Location = New System.Drawing.Point(0, 0)
        Me.btnOn.Name = "btnOn"
        Me.btnOn.Size = New System.Drawing.Size(141, 46)
        Me.btnOn.TabIndex = 0
        Me.btnOn.Text = "On"
        Me.btnOn.UseVisualStyleBackColor = False
        '
        'tbCOMPort
        '
        Me.tbCOMPort.Location = New System.Drawing.Point(71, 12)
        Me.tbCOMPort.Name = "tbCOMPort"
        Me.tbCOMPort.Size = New System.Drawing.Size(65, 20)
        Me.tbCOMPort.TabIndex = 2
        Me.tbCOMPort.Text = "COM5"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "COM Port"
        '
        'tbnStart
        '
        Me.tbnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbnStart.Location = New System.Drawing.Point(233, 12)
        Me.tbnStart.Name = "tbnStart"
        Me.tbnStart.Size = New System.Drawing.Size(62, 20)
        Me.tbnStart.TabIndex = 4
        Me.tbnStart.Text = "Start"
        Me.tbnStart.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(307, 513)
        Me.Controls.Add(Me.tbnStart)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbCOMPort)
        Me.Controls.Add(Me.scOnOff)
        Me.Controls.Add(Me.pgMain)
        Me.Name = "Form1"
        Me.Text = "PeakTech Minimum UI"
        Me.scOnOff.Panel1.ResumeLayout(False)
        Me.scOnOff.Panel2.ResumeLayout(False)
        CType(Me.scOnOff, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scOnOff.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tUpdate As Timer
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents scOnOff As SplitContainer
    Friend WithEvents btnOff As Button
    Friend WithEvents btnOn As Button
    Friend WithEvents tbCOMPort As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents tbnStart As Button
End Class
