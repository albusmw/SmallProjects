<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tspbProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.tsslProgress = New System.Windows.Forms.ToolStripStatusLabel()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExplorerHereToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestWebInterfaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CaptureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunCaptureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllReadoutModesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExposureTimeSeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GainVariationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PresetsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoRealObjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExperimentalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DMatrixCutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PresetsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BiasCaptureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FastLiveModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tbStatistics = New System.Windows.Forms.TextBox()
        Me.zgcMain = New ZedGraph.ZedGraphControl()
        Me.tSetTemp = New System.Windows.Forms.Timer(Me.components)
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbCapture = New System.Windows.Forms.ToolStripButton()
        Me.tsbStopCapture = New System.Windows.Forms.ToolStripButton()
        Me.ilMain = New System.Windows.Forms.ImageList(Me.components)
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scTextBox = New System.Windows.Forms.SplitContainer()
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.CenterROIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ssMain.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.tsMain.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.scTextBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scTextBox.Panel1.SuspendLayout()
        Me.scTextBox.Panel2.SuspendLayout()
        Me.scTextBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(12, 65)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(362, 761)
        Me.pgMain.TabIndex = 0
        Me.pgMain.ToolbarVisible = False
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslMain, Me.tspbProgress, Me.tsslProgress})
        Me.ssMain.Location = New System.Drawing.Point(0, 838)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(1406, 22)
        Me.ssMain.TabIndex = 1
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(50, 17)
        Me.tsslMain.Text = "--IDLE--"
        '
        'tspbProgress
        '
        Me.tspbProgress.ForeColor = System.Drawing.Color.Lime
        Me.tspbProgress.Name = "tspbProgress"
        Me.tspbProgress.Size = New System.Drawing.Size(300, 16)
        Me.tspbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'tsslProgress
        '
        Me.tsslProgress.Name = "tsslProgress"
        Me.tsslProgress.Size = New System.Drawing.Size(22, 17)
        Me.tsslProgress.Text = "---"
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.CaptureToolStripMenuItem, Me.ExperimentalToolStripMenuItem, Me.PresetsToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(1406, 24)
        Me.msMain.TabIndex = 2
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExplorerHereToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem, Me.TestWebInterfaceToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ExplorerHereToolStripMenuItem
        '
        Me.ExplorerHereToolStripMenuItem.Name = "ExplorerHereToolStripMenuItem"
        Me.ExplorerHereToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.ExplorerHereToolStripMenuItem.Text = "Explorer here"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(165, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'TestWebInterfaceToolStripMenuItem
        '
        Me.TestWebInterfaceToolStripMenuItem.Name = "TestWebInterfaceToolStripMenuItem"
        Me.TestWebInterfaceToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.TestWebInterfaceToolStripMenuItem.Text = "Test web interface"
        '
        'CaptureToolStripMenuItem
        '
        Me.CaptureToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunCaptureToolStripMenuItem, Me.SeriesToolStripMenuItem, Me.PresetsToolStripMenuItem1})
        Me.CaptureToolStripMenuItem.Name = "CaptureToolStripMenuItem"
        Me.CaptureToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.CaptureToolStripMenuItem.Text = "Capture"
        '
        'RunCaptureToolStripMenuItem
        '
        Me.RunCaptureToolStripMenuItem.Name = "RunCaptureToolStripMenuItem"
        Me.RunCaptureToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.RunCaptureToolStripMenuItem.Text = "Run capture"
        '
        'SeriesToolStripMenuItem
        '
        Me.SeriesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllReadoutModesToolStripMenuItem, Me.ExposureTimeSeriesToolStripMenuItem, Me.GainVariationToolStripMenuItem})
        Me.SeriesToolStripMenuItem.Name = "SeriesToolStripMenuItem"
        Me.SeriesToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.SeriesToolStripMenuItem.Text = "Series"
        '
        'AllReadoutModesToolStripMenuItem
        '
        Me.AllReadoutModesToolStripMenuItem.Name = "AllReadoutModesToolStripMenuItem"
        Me.AllReadoutModesToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.AllReadoutModesToolStripMenuItem.Text = "All read-out modes"
        '
        'ExposureTimeSeriesToolStripMenuItem
        '
        Me.ExposureTimeSeriesToolStripMenuItem.Name = "ExposureTimeSeriesToolStripMenuItem"
        Me.ExposureTimeSeriesToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.ExposureTimeSeriesToolStripMenuItem.Text = "Exposure time series"
        '
        'GainVariationToolStripMenuItem
        '
        Me.GainVariationToolStripMenuItem.Name = "GainVariationToolStripMenuItem"
        Me.GainVariationToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.GainVariationToolStripMenuItem.Text = "Gain variation"
        '
        'PresetsToolStripMenuItem1
        '
        Me.PresetsToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NoRealObjectToolStripMenuItem})
        Me.PresetsToolStripMenuItem1.Name = "PresetsToolStripMenuItem1"
        Me.PresetsToolStripMenuItem1.Size = New System.Drawing.Size(138, 22)
        Me.PresetsToolStripMenuItem1.Text = "Presets"
        '
        'NoRealObjectToolStripMenuItem
        '
        Me.NoRealObjectToolStripMenuItem.Name = "NoRealObjectToolStripMenuItem"
        Me.NoRealObjectToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.NoRealObjectToolStripMenuItem.Text = "Camera-only test"
        '
        'ExperimentalToolStripMenuItem
        '
        Me.ExperimentalToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DMatrixCutToolStripMenuItem})
        Me.ExperimentalToolStripMenuItem.Name = "ExperimentalToolStripMenuItem"
        Me.ExperimentalToolStripMenuItem.Size = New System.Drawing.Size(88, 20)
        Me.ExperimentalToolStripMenuItem.Text = "Experimental"
        '
        'DMatrixCutToolStripMenuItem
        '
        Me.DMatrixCutToolStripMenuItem.Name = "DMatrixCutToolStripMenuItem"
        Me.DMatrixCutToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DMatrixCutToolStripMenuItem.Text = "2D matrix cut"
        '
        'PresetsToolStripMenuItem
        '
        Me.PresetsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BiasCaptureToolStripMenuItem, Me.FastLiveModeToolStripMenuItem, Me.CenterROIToolStripMenuItem})
        Me.PresetsToolStripMenuItem.Name = "PresetsToolStripMenuItem"
        Me.PresetsToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.PresetsToolStripMenuItem.Text = "Presets"
        '
        'BiasCaptureToolStripMenuItem
        '
        Me.BiasCaptureToolStripMenuItem.Name = "BiasCaptureToolStripMenuItem"
        Me.BiasCaptureToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.BiasCaptureToolStripMenuItem.Text = "Bias capture"
        '
        'FastLiveModeToolStripMenuItem
        '
        Me.FastLiveModeToolStripMenuItem.Name = "FastLiveModeToolStripMenuItem"
        Me.FastLiveModeToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.FastLiveModeToolStripMenuItem.Text = "Fast live mode"
        '
        'tbStatistics
        '
        Me.tbStatistics.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbStatistics.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbStatistics.Location = New System.Drawing.Point(0, 0)
        Me.tbStatistics.Multiline = True
        Me.tbStatistics.Name = "tbStatistics"
        Me.tbStatistics.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbStatistics.Size = New System.Drawing.Size(1008, 241)
        Me.tbStatistics.TabIndex = 3
        '
        'zgcMain
        '
        Me.zgcMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.zgcMain.Location = New System.Drawing.Point(3, 3)
        Me.zgcMain.Name = "zgcMain"
        Me.zgcMain.ScrollGrace = 0R
        Me.zgcMain.ScrollMaxX = 0R
        Me.zgcMain.ScrollMaxY = 0R
        Me.zgcMain.ScrollMaxY2 = 0R
        Me.zgcMain.ScrollMinX = 0R
        Me.zgcMain.ScrollMinY = 0R
        Me.zgcMain.ScrollMinY2 = 0R
        Me.zgcMain.Size = New System.Drawing.Size(1008, 365)
        Me.zgcMain.TabIndex = 0
        '
        'tSetTemp
        '
        Me.tSetTemp.Enabled = True
        Me.tSetTemp.Interval = 500
        '
        'tsMain
        '
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbCapture, Me.tsbStopCapture})
        Me.tsMain.Location = New System.Drawing.Point(0, 24)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(1406, 38)
        Me.tsMain.TabIndex = 5
        Me.tsMain.Text = "ToolStrip1"
        '
        'tsbCapture
        '
        Me.tsbCapture.Image = CType(resources.GetObject("tsbCapture.Image"), System.Drawing.Image)
        Me.tsbCapture.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCapture.Name = "tsbCapture"
        Me.tsbCapture.Size = New System.Drawing.Size(53, 35)
        Me.tsbCapture.Text = "Capture"
        Me.tsbCapture.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbStopCapture
        '
        Me.tsbStopCapture.Image = CType(resources.GetObject("tsbStopCapture.Image"), System.Drawing.Image)
        Me.tsbStopCapture.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbStopCapture.Name = "tsbStopCapture"
        Me.tsbStopCapture.Size = New System.Drawing.Size(35, 35)
        Me.tsbStopCapture.Text = "Stop"
        Me.tsbStopCapture.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ilMain
        '
        Me.ilMain.ImageStream = CType(resources.GetObject("ilMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilMain.TransparentColor = System.Drawing.Color.Transparent
        Me.ilMain.Images.SetKeyName(0, "Capture.png")
        Me.ilMain.Images.SetKeyName(1, "StopCapture.png")
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(380, 65)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.zgcMain)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.scTextBox)
        Me.scMain.Size = New System.Drawing.Size(1014, 761)
        Me.scMain.SplitterDistance = 371
        Me.scMain.TabIndex = 4
        '
        'scTextBox
        '
        Me.scTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scTextBox.Location = New System.Drawing.Point(3, 3)
        Me.scTextBox.Name = "scTextBox"
        Me.scTextBox.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scTextBox.Panel1
        '
        Me.scTextBox.Panel1.Controls.Add(Me.tbStatistics)
        '
        'scTextBox.Panel2
        '
        Me.scTextBox.Panel2.Controls.Add(Me.tbLogOutput)
        Me.scTextBox.Size = New System.Drawing.Size(1008, 380)
        Me.scTextBox.SplitterDistance = 241
        Me.scTextBox.TabIndex = 4
        '
        'tbLogOutput
        '
        Me.tbLogOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbLogOutput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLogOutput.Location = New System.Drawing.Point(0, 0)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(1008, 135)
        Me.tbLogOutput.TabIndex = 4
        '
        'CenterROIToolStripMenuItem
        '
        Me.CenterROIToolStripMenuItem.Name = "CenterROIToolStripMenuItem"
        Me.CenterROIToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CenterROIToolStripMenuItem.Text = "Center ROI"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1406, 860)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.msMain)
        Me.Controls.Add(Me.pgMain)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "QHY Capture"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.scTextBox.Panel1.ResumeLayout(False)
        Me.scTextBox.Panel1.PerformLayout()
        Me.scTextBox.Panel2.ResumeLayout(False)
        Me.scTextBox.Panel2.PerformLayout()
        CType(Me.scTextBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scTextBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pgMain As Windows.Forms.PropertyGrid
    Friend WithEvents ssMain As Windows.Forms.StatusStrip
    Friend WithEvents msMain As Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExplorerHereToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaptureToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunCaptureToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsslMain As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tbStatistics As Windows.Forms.TextBox
    Friend WithEvents zgcMain As ZedGraph.ZedGraphControl
    Friend WithEvents ExperimentalToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DMatrixCutToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tspbProgress As Windows.Forms.ToolStripProgressBar
    Friend WithEvents tsslProgress As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tSetTemp As Windows.Forms.Timer
    Friend WithEvents tsMain As Windows.Forms.ToolStrip
    Friend WithEvents tsbCapture As Windows.Forms.ToolStripButton
    Friend WithEvents tsbStopCapture As Windows.Forms.ToolStripButton
    Friend WithEvents PresetsToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents BiasCaptureToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ilMain As Windows.Forms.ImageList
    Friend WithEvents SeriesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllReadoutModesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExposureTimeSeriesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents GainVariationToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents PresetsToolStripMenuItem1 As Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoRealObjectToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestWebInterfaceToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents scMain As Windows.Forms.SplitContainer
    Friend WithEvents FastLiveModeToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents scTextBox As Windows.Forms.SplitContainer
    Friend WithEvents tbLogOutput As Windows.Forms.TextBox
    Friend WithEvents CenterROIToolStripMenuItem As Windows.Forms.ToolStripMenuItem
End Class
