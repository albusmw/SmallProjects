<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDIVIQuery
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
        Me.wbMain = New System.Windows.Forms.WebBrowser()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnProcessCSV = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'wbMain
        '
        Me.wbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.wbMain.Location = New System.Drawing.Point(11, 11)
        Me.wbMain.Margin = New System.Windows.Forms.Padding(2)
        Me.wbMain.MinimumSize = New System.Drawing.Size(13, 13)
        Me.wbMain.Name = "wbMain"
        Me.wbMain.Size = New System.Drawing.Size(924, 524)
        Me.wbMain.TabIndex = 1
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Location = New System.Drawing.Point(12, 540)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(795, 23)
        Me.btnStart.TabIndex = 2
        Me.btnStart.Text = "Run query"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnProcessCSV
        '
        Me.btnProcessCSV.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProcessCSV.Location = New System.Drawing.Point(813, 540)
        Me.btnProcessCSV.Name = "btnProcessCSV"
        Me.btnProcessCSV.Size = New System.Drawing.Size(121, 23)
        Me.btnProcessCSV.TabIndex = 3
        Me.btnProcessCSV.Text = "Process CSV"
        Me.btnProcessCSV.UseVisualStyleBackColor = True
        '
        'frmDIVIQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(946, 575)
        Me.Controls.Add(Me.btnProcessCSV)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.wbMain)
        Me.Name = "frmDIVIQuery"
        Me.Text = "frmDIVIQuery"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents wbMain As WebBrowser
    Friend WithEvents btnStart As Button
    Friend WithEvents btnProcessCSV As Button
End Class
