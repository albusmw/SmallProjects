<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManualAdjust
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
        Me.cbSelectedHisto = New System.Windows.Forms.ComboBox()
        Me.tbCurve_1_Scale = New System.Windows.Forms.TextBox()
        Me.tbCurve_1_Name = New System.Windows.Forms.TextBox()
        Me.tbCurve_1_Offset = New System.Windows.Forms.TextBox()
        Me.tbCurve_2_Name = New System.Windows.Forms.TextBox()
        Me.tbCurve_3_Name = New System.Windows.Forms.TextBox()
        Me.tbCurve_4_Name = New System.Windows.Forms.TextBox()
        Me.tbCurve_2_Scale = New System.Windows.Forms.TextBox()
        Me.tbCurve_3_Scale = New System.Windows.Forms.TextBox()
        Me.tbCurve_4_Scale = New System.Windows.Forms.TextBox()
        Me.tbCurve_2_Offset = New System.Windows.Forms.TextBox()
        Me.tbCurve_3_Offset = New System.Windows.Forms.TextBox()
        Me.tbCurve_4_Offset = New System.Windows.Forms.TextBox()
        Me.tbCurve_1_YMul = New System.Windows.Forms.TextBox()
        Me.tbCurve_2_YMul = New System.Windows.Forms.TextBox()
        Me.tbCurve_3_YMul = New System.Windows.Forms.TextBox()
        Me.tbCurve_4_YMul = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'cbSelectedHisto
        '
        Me.cbSelectedHisto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbSelectedHisto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSelectedHisto.FormattingEnabled = True
        Me.cbSelectedHisto.Location = New System.Drawing.Point(12, 12)
        Me.cbSelectedHisto.Name = "cbSelectedHisto"
        Me.cbSelectedHisto.Size = New System.Drawing.Size(422, 21)
        Me.cbSelectedHisto.TabIndex = 0
        '
        'tbCurve_1_Scale
        '
        Me.tbCurve_1_Scale.Location = New System.Drawing.Point(119, 39)
        Me.tbCurve_1_Scale.Name = "tbCurve_1_Scale"
        Me.tbCurve_1_Scale.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_1_Scale.TabIndex = 1
        Me.tbCurve_1_Scale.Text = "1,00"
        Me.tbCurve_1_Scale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_1_Name
        '
        Me.tbCurve_1_Name.Location = New System.Drawing.Point(12, 39)
        Me.tbCurve_1_Name.Name = "tbCurve_1_Name"
        Me.tbCurve_1_Name.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_1_Name.TabIndex = 2
        Me.tbCurve_1_Name.Text = "R[0,0]"
        '
        'tbCurve_1_Offset
        '
        Me.tbCurve_1_Offset.Location = New System.Drawing.Point(226, 39)
        Me.tbCurve_1_Offset.Name = "tbCurve_1_Offset"
        Me.tbCurve_1_Offset.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_1_Offset.TabIndex = 3
        Me.tbCurve_1_Offset.Text = "0,00"
        Me.tbCurve_1_Offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_2_Name
        '
        Me.tbCurve_2_Name.Location = New System.Drawing.Point(12, 65)
        Me.tbCurve_2_Name.Name = "tbCurve_2_Name"
        Me.tbCurve_2_Name.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_2_Name.TabIndex = 4
        Me.tbCurve_2_Name.Text = "G[0,1]"
        '
        'tbCurve_3_Name
        '
        Me.tbCurve_3_Name.Location = New System.Drawing.Point(12, 91)
        Me.tbCurve_3_Name.Name = "tbCurve_3_Name"
        Me.tbCurve_3_Name.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_3_Name.TabIndex = 5
        Me.tbCurve_3_Name.Text = "G1[1,0]"
        '
        'tbCurve_4_Name
        '
        Me.tbCurve_4_Name.Location = New System.Drawing.Point(12, 117)
        Me.tbCurve_4_Name.Name = "tbCurve_4_Name"
        Me.tbCurve_4_Name.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_4_Name.TabIndex = 6
        Me.tbCurve_4_Name.Text = "B[1,1]"
        '
        'tbCurve_2_Scale
        '
        Me.tbCurve_2_Scale.Location = New System.Drawing.Point(119, 65)
        Me.tbCurve_2_Scale.Name = "tbCurve_2_Scale"
        Me.tbCurve_2_Scale.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_2_Scale.TabIndex = 7
        Me.tbCurve_2_Scale.Text = "1,00"
        Me.tbCurve_2_Scale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_3_Scale
        '
        Me.tbCurve_3_Scale.Location = New System.Drawing.Point(119, 91)
        Me.tbCurve_3_Scale.Name = "tbCurve_3_Scale"
        Me.tbCurve_3_Scale.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_3_Scale.TabIndex = 8
        Me.tbCurve_3_Scale.Text = "1,00"
        Me.tbCurve_3_Scale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_4_Scale
        '
        Me.tbCurve_4_Scale.Location = New System.Drawing.Point(119, 117)
        Me.tbCurve_4_Scale.Name = "tbCurve_4_Scale"
        Me.tbCurve_4_Scale.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_4_Scale.TabIndex = 9
        Me.tbCurve_4_Scale.Text = "1,00"
        Me.tbCurve_4_Scale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_2_Offset
        '
        Me.tbCurve_2_Offset.Location = New System.Drawing.Point(226, 65)
        Me.tbCurve_2_Offset.Name = "tbCurve_2_Offset"
        Me.tbCurve_2_Offset.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_2_Offset.TabIndex = 10
        Me.tbCurve_2_Offset.Text = "0,00"
        Me.tbCurve_2_Offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_3_Offset
        '
        Me.tbCurve_3_Offset.Location = New System.Drawing.Point(226, 91)
        Me.tbCurve_3_Offset.Name = "tbCurve_3_Offset"
        Me.tbCurve_3_Offset.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_3_Offset.TabIndex = 11
        Me.tbCurve_3_Offset.Text = "0,00"
        Me.tbCurve_3_Offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_4_Offset
        '
        Me.tbCurve_4_Offset.Location = New System.Drawing.Point(226, 117)
        Me.tbCurve_4_Offset.Name = "tbCurve_4_Offset"
        Me.tbCurve_4_Offset.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_4_Offset.TabIndex = 12
        Me.tbCurve_4_Offset.Text = "0,00"
        Me.tbCurve_4_Offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_1_YMul
        '
        Me.tbCurve_1_YMul.Location = New System.Drawing.Point(333, 39)
        Me.tbCurve_1_YMul.Name = "tbCurve_1_YMul"
        Me.tbCurve_1_YMul.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_1_YMul.TabIndex = 13
        Me.tbCurve_1_YMul.Text = "1,00"
        Me.tbCurve_1_YMul.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_2_YMul
        '
        Me.tbCurve_2_YMul.Location = New System.Drawing.Point(333, 65)
        Me.tbCurve_2_YMul.Name = "tbCurve_2_YMul"
        Me.tbCurve_2_YMul.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_2_YMul.TabIndex = 14
        Me.tbCurve_2_YMul.Text = "1,00"
        Me.tbCurve_2_YMul.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_3_YMul
        '
        Me.tbCurve_3_YMul.Location = New System.Drawing.Point(333, 91)
        Me.tbCurve_3_YMul.Name = "tbCurve_3_YMul"
        Me.tbCurve_3_YMul.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_3_YMul.TabIndex = 15
        Me.tbCurve_3_YMul.Text = "1,00"
        Me.tbCurve_3_YMul.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tbCurve_4_YMul
        '
        Me.tbCurve_4_YMul.Location = New System.Drawing.Point(333, 117)
        Me.tbCurve_4_YMul.Name = "tbCurve_4_YMul"
        Me.tbCurve_4_YMul.Size = New System.Drawing.Size(101, 20)
        Me.tbCurve_4_YMul.TabIndex = 16
        Me.tbCurve_4_YMul.Text = "1,00"
        Me.tbCurve_4_YMul.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmManualAdjust
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(454, 151)
        Me.Controls.Add(Me.tbCurve_4_YMul)
        Me.Controls.Add(Me.tbCurve_3_YMul)
        Me.Controls.Add(Me.tbCurve_2_YMul)
        Me.Controls.Add(Me.tbCurve_1_YMul)
        Me.Controls.Add(Me.tbCurve_4_Offset)
        Me.Controls.Add(Me.tbCurve_3_Offset)
        Me.Controls.Add(Me.tbCurve_2_Offset)
        Me.Controls.Add(Me.tbCurve_4_Scale)
        Me.Controls.Add(Me.tbCurve_3_Scale)
        Me.Controls.Add(Me.tbCurve_2_Scale)
        Me.Controls.Add(Me.tbCurve_4_Name)
        Me.Controls.Add(Me.tbCurve_3_Name)
        Me.Controls.Add(Me.tbCurve_2_Name)
        Me.Controls.Add(Me.tbCurve_1_Offset)
        Me.Controls.Add(Me.tbCurve_1_Name)
        Me.Controls.Add(Me.tbCurve_1_Scale)
        Me.Controls.Add(Me.cbSelectedHisto)
        Me.Name = "frmManualAdjust"
        Me.Text = "Manual balance adjustment"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cbSelectedHisto As ComboBox
    Friend WithEvents tbCurve_1_Scale As TextBox
    Friend WithEvents tbCurve_1_Name As TextBox
    Friend WithEvents tbCurve_1_Offset As TextBox
    Friend WithEvents tbCurve_2_Name As TextBox
    Friend WithEvents tbCurve_3_Name As TextBox
    Friend WithEvents tbCurve_4_Name As TextBox
    Friend WithEvents tbCurve_2_Scale As TextBox
    Friend WithEvents tbCurve_3_Scale As TextBox
    Friend WithEvents tbCurve_4_Scale As TextBox
    Friend WithEvents tbCurve_2_Offset As TextBox
    Friend WithEvents tbCurve_3_Offset As TextBox
    Friend WithEvents tbCurve_4_Offset As TextBox
    Friend WithEvents tbCurve_1_YMul As TextBox
    Friend WithEvents tbCurve_2_YMul As TextBox
    Friend WithEvents tbCurve_3_YMul As TextBox
    Friend WithEvents tbCurve_4_YMul As TextBox
End Class
