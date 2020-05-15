﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.tsmiFPSIndicator = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslMemory = New System.Windows.Forms.ToolStripStatusLabel()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunXMLSequenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExplorerHereToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExploreCurrentCampaignToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TestWebInterfaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StoreStatisticsAsEXCELFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CaptureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllReadoutModesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExposureTimeSeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GainVariationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PresetsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSpeedTest = New System.Windows.Forms.ToolStripMenuItem()
        Me.FastLiveModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CenterROIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TESTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.USBTreeReaderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiASIZWO = New System.Windows.Forms.ToolStripMenuItem()
        Me.FITSWriterWithKeywordsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiActions = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiResetLoopStat = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiNewGUID = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiLoad10MicronData = New System.Windows.Forms.ToolStripMenuItem()
        Me.zgcMain = New ZedGraph.ZedGraphControl()
        Me.tSetTemp = New System.Windows.Forms.Timer(Me.components)
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbCapture = New System.Windows.Forms.ToolStripButton()
        Me.tsbStopCapture = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ilMain = New System.Windows.Forms.ImageList(Me.components)
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.pgMeta = New System.Windows.Forms.PropertyGrid()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.rtbStatistics = New System.Windows.Forms.RichTextBox()
        Me.sfdMain = New System.Windows.Forms.SaveFileDialog()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.tStatusUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.OpenLastStoredFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ssMain.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.tcMain.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.SuspendLayout()
        '
        'pgMain
        '
        Me.pgMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMain.Location = New System.Drawing.Point(3, 3)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(380, 692)
        Me.pgMain.TabIndex = 0
        Me.pgMain.ToolbarVisible = False
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslMain, Me.tspbProgress, Me.tsslProgress, Me.tsmiFPSIndicator, Me.tsslMemory})
        Me.ssMain.Location = New System.Drawing.Point(0, 984)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(1551, 22)
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
        'tsmiFPSIndicator
        '
        Me.tsmiFPSIndicator.Name = "tsmiFPSIndicator"
        Me.tsmiFPSIndicator.Size = New System.Drawing.Size(47, 17)
        Me.tsmiFPSIndicator.Text = "FPS: ???"
        '
        'tsslMemory
        '
        Me.tsslMemory.Name = "tsslMemory"
        Me.tsslMemory.Size = New System.Drawing.Size(73, 17)
        Me.tsslMemory.Text = "Memory: ???"
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.CaptureToolStripMenuItem, Me.PresetsToolStripMenuItem, Me.TESTToolStripMenuItem, Me.tsmiActions})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(1551, 24)
        Me.msMain.TabIndex = 2
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunXMLSequenceToolStripMenuItem, Me.ToolStripMenuItem6, Me.ExplorerHereToolStripMenuItem, Me.ExploreCurrentCampaignToolStripMenuItem, Me.OpenLastStoredFileToolStripMenuItem, Me.ToolStripMenuItem1, Me.TestWebInterfaceToolStripMenuItem, Me.StoreStatisticsAsEXCELFileToolStripMenuItem, Me.ToolStripMenuItem5, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'RunXMLSequenceToolStripMenuItem
        '
        Me.RunXMLSequenceToolStripMenuItem.Name = "RunXMLSequenceToolStripMenuItem"
        Me.RunXMLSequenceToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.RunXMLSequenceToolStripMenuItem.Text = "Run XML sequence"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(215, 6)
        '
        'ExplorerHereToolStripMenuItem
        '
        Me.ExplorerHereToolStripMenuItem.Name = "ExplorerHereToolStripMenuItem"
        Me.ExplorerHereToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.ExplorerHereToolStripMenuItem.Text = "Explorer here"
        '
        'ExploreCurrentCampaignToolStripMenuItem
        '
        Me.ExploreCurrentCampaignToolStripMenuItem.Name = "ExploreCurrentCampaignToolStripMenuItem"
        Me.ExploreCurrentCampaignToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.ExploreCurrentCampaignToolStripMenuItem.Text = "Explore current campaign"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(215, 6)
        '
        'TestWebInterfaceToolStripMenuItem
        '
        Me.TestWebInterfaceToolStripMenuItem.Name = "TestWebInterfaceToolStripMenuItem"
        Me.TestWebInterfaceToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.TestWebInterfaceToolStripMenuItem.Text = "Test web interface"
        '
        'StoreStatisticsAsEXCELFileToolStripMenuItem
        '
        Me.StoreStatisticsAsEXCELFileToolStripMenuItem.Name = "StoreStatisticsAsEXCELFileToolStripMenuItem"
        Me.StoreStatisticsAsEXCELFileToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.StoreStatisticsAsEXCELFileToolStripMenuItem.Text = "Store statistics as EXCEL file"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(215, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'CaptureToolStripMenuItem
        '
        Me.CaptureToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SeriesToolStripMenuItem})
        Me.CaptureToolStripMenuItem.Name = "CaptureToolStripMenuItem"
        Me.CaptureToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.CaptureToolStripMenuItem.Text = "Capture"
        '
        'SeriesToolStripMenuItem
        '
        Me.SeriesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllReadoutModesToolStripMenuItem, Me.ExposureTimeSeriesToolStripMenuItem, Me.GainVariationToolStripMenuItem})
        Me.SeriesToolStripMenuItem.Name = "SeriesToolStripMenuItem"
        Me.SeriesToolStripMenuItem.Size = New System.Drawing.Size(104, 22)
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
        'PresetsToolStripMenuItem
        '
        Me.PresetsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiSpeedTest, Me.FastLiveModeToolStripMenuItem, Me.CenterROIToolStripMenuItem})
        Me.PresetsToolStripMenuItem.Name = "PresetsToolStripMenuItem"
        Me.PresetsToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.PresetsToolStripMenuItem.Text = "Presets"
        '
        'tsmiSpeedTest
        '
        Me.tsmiSpeedTest.Name = "tsmiSpeedTest"
        Me.tsmiSpeedTest.Size = New System.Drawing.Size(150, 22)
        Me.tsmiSpeedTest.Text = "Speed test"
        '
        'FastLiveModeToolStripMenuItem
        '
        Me.FastLiveModeToolStripMenuItem.Name = "FastLiveModeToolStripMenuItem"
        Me.FastLiveModeToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.FastLiveModeToolStripMenuItem.Text = "Fast live mode"
        '
        'CenterROIToolStripMenuItem
        '
        Me.CenterROIToolStripMenuItem.Name = "CenterROIToolStripMenuItem"
        Me.CenterROIToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.CenterROIToolStripMenuItem.Text = "Center ROI"
        '
        'TESTToolStripMenuItem
        '
        Me.TESTToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.USBTreeReaderToolStripMenuItem, Me.ToolStripMenuItem2, Me.tsmiASIZWO, Me.FITSWriterWithKeywordsToolStripMenuItem})
        Me.TESTToolStripMenuItem.Name = "TESTToolStripMenuItem"
        Me.TESTToolStripMenuItem.Size = New System.Drawing.Size(73, 20)
        Me.TESTToolStripMenuItem.Text = "!!! TEST !!!!!"
        '
        'USBTreeReaderToolStripMenuItem
        '
        Me.USBTreeReaderToolStripMenuItem.Name = "USBTreeReaderToolStripMenuItem"
        Me.USBTreeReaderToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.USBTreeReaderToolStripMenuItem.Text = "USBTree reader"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(204, 6)
        '
        'tsmiASIZWO
        '
        Me.tsmiASIZWO.Name = "tsmiASIZWO"
        Me.tsmiASIZWO.Size = New System.Drawing.Size(207, 22)
        Me.tsmiASIZWO.Text = "ASI ZWO Reader"
        '
        'FITSWriterWithKeywordsToolStripMenuItem
        '
        Me.FITSWriterWithKeywordsToolStripMenuItem.Name = "FITSWriterWithKeywordsToolStripMenuItem"
        Me.FITSWriterWithKeywordsToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.FITSWriterWithKeywordsToolStripMenuItem.Text = "FITS writer with keywords"
        '
        'tsmiActions
        '
        Me.tsmiActions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiResetLoopStat, Me.tsmiNewGUID, Me.tsmiLoad10MicronData})
        Me.tsmiActions.Name = "tsmiActions"
        Me.tsmiActions.Size = New System.Drawing.Size(59, 20)
        Me.tsmiActions.Text = "Actions"
        '
        'tsmiResetLoopStat
        '
        Me.tsmiResetLoopStat.Name = "tsmiResetLoopStat"
        Me.tsmiResetLoopStat.Size = New System.Drawing.Size(179, 22)
        Me.tsmiResetLoopStat.Text = "Reset loop statistics"
        '
        'tsmiNewGUID
        '
        Me.tsmiNewGUID.Name = "tsmiNewGUID"
        Me.tsmiNewGUID.Size = New System.Drawing.Size(179, 22)
        Me.tsmiNewGUID.Text = "New GUID"
        '
        'tsmiLoad10MicronData
        '
        Me.tsmiLoad10MicronData.Name = "tsmiLoad10MicronData"
        Me.tsmiLoad10MicronData.Size = New System.Drawing.Size(179, 22)
        Me.tsmiLoad10MicronData.Text = "Load 10Micron data"
        '
        'zgcMain
        '
        Me.zgcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgcMain.Location = New System.Drawing.Point(0, 0)
        Me.zgcMain.Name = "zgcMain"
        Me.zgcMain.ScrollGrace = 0R
        Me.zgcMain.ScrollMaxX = 0R
        Me.zgcMain.ScrollMaxY = 0R
        Me.zgcMain.ScrollMaxY2 = 0R
        Me.zgcMain.ScrollMinX = 0R
        Me.zgcMain.ScrollMinY = 0R
        Me.zgcMain.ScrollMinY2 = 0R
        Me.zgcMain.Size = New System.Drawing.Size(1153, 450)
        Me.zgcMain.TabIndex = 0
        '
        'tSetTemp
        '
        Me.tSetTemp.Enabled = True
        Me.tSetTemp.Interval = 500
        '
        'tsMain
        '
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbCapture, Me.tsbStopCapture, Me.ToolStripSeparator1})
        Me.tsMain.Location = New System.Drawing.Point(0, 24)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(1551, 38)
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
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 38)
        '
        'ilMain
        '
        Me.ilMain.ImageStream = CType(resources.GetObject("ilMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilMain.TransparentColor = System.Drawing.Color.Transparent
        Me.ilMain.Images.SetKeyName(0, "Capture.png")
        Me.ilMain.Images.SetKeyName(1, "StopCapture.png")
        '
        'tbLogOutput
        '
        Me.tbLogOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbLogOutput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLogOutput.Location = New System.Drawing.Point(0, 0)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(394, 188)
        Me.tbLogOutput.TabIndex = 4
        Me.tbLogOutput.WordWrap = False
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.TabPage1)
        Me.tcMain.Controls.Add(Me.TabPage2)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(394, 724)
        Me.tcMain.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.pgMain)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(386, 698)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Exposure"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.pgMeta)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(386, 698)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Meta data"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'pgMeta
        '
        Me.pgMeta.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMeta.Location = New System.Drawing.Point(3, 3)
        Me.pgMeta.Name = "pgMeta"
        Me.pgMeta.Size = New System.Drawing.Size(380, 692)
        Me.pgMeta.TabIndex = 1
        Me.pgMeta.ToolbarVisible = False
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(0, 65)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.SplitContainer3)
        Me.scMain.Size = New System.Drawing.Size(1551, 916)
        Me.scMain.SplitterDistance = 394
        Me.scMain.TabIndex = 7
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.tcMain)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.tbLogOutput)
        Me.SplitContainer2.Size = New System.Drawing.Size(394, 916)
        Me.SplitContainer2.SplitterDistance = 724
        Me.SplitContainer2.TabIndex = 0
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.zgcMain)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.rtbStatistics)
        Me.SplitContainer3.Size = New System.Drawing.Size(1153, 916)
        Me.SplitContainer3.SplitterDistance = 450
        Me.SplitContainer3.TabIndex = 0
        '
        'rtbStatistics
        '
        Me.rtbStatistics.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbStatistics.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbStatistics.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbStatistics.Location = New System.Drawing.Point(0, 0)
        Me.rtbStatistics.Name = "rtbStatistics"
        Me.rtbStatistics.Size = New System.Drawing.Size(1153, 462)
        Me.rtbStatistics.TabIndex = 0
        Me.rtbStatistics.Text = ""
        Me.rtbStatistics.WordWrap = False
        '
        'ofdMain
        '
        Me.ofdMain.FileName = "OpenFileDialog1"
        '
        'tStatusUpdate
        '
        Me.tStatusUpdate.Enabled = True
        Me.tStatusUpdate.Interval = 250
        '
        'OpenLastStoredFileToolStripMenuItem
        '
        Me.OpenLastStoredFileToolStripMenuItem.Name = "OpenLastStoredFileToolStripMenuItem"
        Me.OpenLastStoredFileToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.OpenLastStoredFileToolStripMenuItem.Text = "Open last stored file"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1551, 1006)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.msMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "QHY Capture"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.tcMain.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
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
    Friend WithEvents tsslMain As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents zgcMain As ZedGraph.ZedGraphControl
    Friend WithEvents tspbProgress As Windows.Forms.ToolStripProgressBar
    Friend WithEvents tsslProgress As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tSetTemp As Windows.Forms.Timer
    Friend WithEvents tsMain As Windows.Forms.ToolStrip
    Friend WithEvents tsbCapture As Windows.Forms.ToolStripButton
    Friend WithEvents tsbStopCapture As Windows.Forms.ToolStripButton
    Friend WithEvents PresetsToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiSpeedTest As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ilMain As Windows.Forms.ImageList
    Friend WithEvents SeriesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllReadoutModesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExposureTimeSeriesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents GainVariationToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestWebInterfaceToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents FastLiveModeToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tbLogOutput As Windows.Forms.TextBox
    Friend WithEvents CenterROIToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tcMain As Windows.Forms.TabControl
    Friend WithEvents TabPage1 As Windows.Forms.TabPage
    Friend WithEvents TabPage2 As Windows.Forms.TabPage
    Friend WithEvents pgMeta As Windows.Forms.PropertyGrid
    Friend WithEvents scMain As Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As Windows.Forms.SplitContainer
    Friend WithEvents rtbStatistics As Windows.Forms.RichTextBox
    Friend WithEvents TESTToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents USBTreeReaderToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExploreCurrentCampaignToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiActions As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiResetLoopStat As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiFPSIndicator As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StoreStatisticsAsEXCELFileToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfdMain As Windows.Forms.SaveFileDialog
    Friend WithEvents tsmiNewGUID As Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunXMLSequenceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents tsmiLoad10MicronData As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents tsmiASIZWO As ToolStripMenuItem
    Friend WithEvents FITSWriterWithKeywordsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsslMemory As ToolStripStatusLabel
    Friend WithEvents tStatusUpdate As Timer
    Friend WithEvents OpenLastStoredFileToolStripMenuItem As ToolStripMenuItem
End Class
