<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOpenRecentFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOpenLastFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripSeparator()
        Me.FITSGrepToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiSaveLastStatXLS = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSaveAllFilesStat = New System.Windows.Forms.ToolStripMenuItem()
        Me.StoreStackingResultToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSaveMeanFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.SumImageDoubleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StdDevImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MaxMinInt32ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ResetStackingToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MaxImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSaveImageData = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ClearStatisticsMemoryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenEXELocationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpecialAnalysisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RowAndColumnStatisticsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PlotStatisticsVsGainToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReplotStatisticsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ADUQuantizationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiCalcVignette = New System.Windows.Forms.ToolStripMenuItem()
        Me.HotPixelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.MultifileAreaCompareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiManualColorBalancer = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiProcessing = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiAdjustRGB = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiStretch = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPlateSolve = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiCorrectVignette = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSetPixelToValue = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestCodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WriteTestDataToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.AfiineTranslateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BitGrayscaleFileGenerationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ASCOMDynamicallyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.FocusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FITSTestFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NEFReadingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetPixelAboveCertainValueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiUseOpenCV = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenCVMedianFilterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisplayImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CoordsForALADINCallToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslRunning = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tspbMain = New System.Windows.Forms.ToolStripProgressBar()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.sfdMain = New System.Windows.Forms.SaveFileDialog()
        Me.gbDetails = New System.Windows.Forms.GroupBox()
        Me.tbDetails = New System.Windows.Forms.TextBox()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scLeft = New System.Windows.Forms.SplitContainer()
        Me.tsmiSaveFITSAndStats = New System.Windows.Forms.ToolStripMenuItem()
        Me.msMain.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.gbDetails.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.scLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scLeft.Panel1.SuspendLayout()
        Me.scLeft.Panel2.SuspendLayout()
        Me.scLeft.SuspendLayout()
        Me.SuspendLayout()
        '
        'msMain
        '
        Me.msMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.SpecialAnalysisToolStripMenuItem, Me.tsmiProcessing, Me.TestCodeToolStripMenuItem, Me.DisplayImageToolStripMenuItem, Me.ToolsToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.msMain.Size = New System.Drawing.Size(1692, 24)
        Me.msMain.TabIndex = 0
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiOpenFile, Me.tsmiOpenRecentFiles, Me.tsmiOpenLastFile, Me.ToolStripMenuItem8, Me.FITSGrepToolStripMenuItem, Me.ToolStripMenuItem2, Me.tsmiSaveLastStatXLS, Me.tsmiSaveAllFilesStat, Me.tsmiSaveFITSAndStats, Me.StoreStackingResultToolStripMenuItem, Me.tsmiSaveImageData, Me.ToolStripMenuItem4, Me.ClearStatisticsMemoryToolStripMenuItem, Me.ToolStripMenuItem1, Me.OpenEXELocationToolStripMenuItem, Me.ToolStripMenuItem5, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 22)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'tsmiOpenFile
        '
        Me.tsmiOpenFile.Name = "tsmiOpenFile"
        Me.tsmiOpenFile.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.tsmiOpenFile.Size = New System.Drawing.Size(300, 22)
        Me.tsmiOpenFile.Text = "Open file(s) to analyse"
        '
        'tsmiOpenRecentFiles
        '
        Me.tsmiOpenRecentFiles.Name = "tsmiOpenRecentFiles"
        Me.tsmiOpenRecentFiles.Size = New System.Drawing.Size(300, 22)
        Me.tsmiOpenRecentFiles.Text = "Open recent files ..."
        '
        'tsmiOpenLastFile
        '
        Me.tsmiOpenLastFile.Name = "tsmiOpenLastFile"
        Me.tsmiOpenLastFile.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.tsmiOpenLastFile.Size = New System.Drawing.Size(300, 22)
        Me.tsmiOpenLastFile.Text = "Open last file processed"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(297, 6)
        '
        'FITSGrepToolStripMenuItem
        '
        Me.FITSGrepToolStripMenuItem.Name = "FITSGrepToolStripMenuItem"
        Me.FITSGrepToolStripMenuItem.Size = New System.Drawing.Size(300, 22)
        Me.FITSGrepToolStripMenuItem.Text = "FITS Grep"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(297, 6)
        '
        'tsmiSaveLastStatXLS
        '
        Me.tsmiSaveLastStatXLS.Name = "tsmiSaveLastStatXLS"
        Me.tsmiSaveLastStatXLS.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.tsmiSaveLastStatXLS.Size = New System.Drawing.Size(300, 22)
        Me.tsmiSaveLastStatXLS.Text = "Save last image statistics EXCEL file"
        '
        'tsmiSaveAllFilesStat
        '
        Me.tsmiSaveAllFilesStat.Name = "tsmiSaveAllFilesStat"
        Me.tsmiSaveAllFilesStat.Size = New System.Drawing.Size(300, 22)
        Me.tsmiSaveAllFilesStat.Text = "Save all-files statistics"
        '
        'StoreStackingResultToolStripMenuItem
        '
        Me.StoreStackingResultToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiSaveMeanFile, Me.SumImageDoubleToolStripMenuItem, Me.StdDevImageToolStripMenuItem, Me.MaxMinInt32ToolStripMenuItem, Me.ToolStripMenuItem3, Me.ResetStackingToolStripMenuItem1, Me.MaxImageToolStripMenuItem})
        Me.StoreStackingResultToolStripMenuItem.Name = "StoreStackingResultToolStripMenuItem"
        Me.StoreStackingResultToolStripMenuItem.Size = New System.Drawing.Size(300, 22)
        Me.StoreStackingResultToolStripMenuItem.Text = "Save stacking image"
        '
        'tsmiSaveMeanFile
        '
        Me.tsmiSaveMeanFile.Name = "tsmiSaveMeanFile"
        Me.tsmiSaveMeanFile.Size = New System.Drawing.Size(183, 22)
        Me.tsmiSaveMeanFile.Text = "Mean image [Int32]"
        '
        'SumImageDoubleToolStripMenuItem
        '
        Me.SumImageDoubleToolStripMenuItem.Name = "SumImageDoubleToolStripMenuItem"
        Me.SumImageDoubleToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.SumImageDoubleToolStripMenuItem.Text = "Sum image [Double]"
        '
        'StdDevImageToolStripMenuItem
        '
        Me.StdDevImageToolStripMenuItem.Name = "StdDevImageToolStripMenuItem"
        Me.StdDevImageToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.StdDevImageToolStripMenuItem.Text = "StdDev image"
        '
        'MaxMinInt32ToolStripMenuItem
        '
        Me.MaxMinInt32ToolStripMenuItem.Name = "MaxMinInt32ToolStripMenuItem"
        Me.MaxMinInt32ToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.MaxMinInt32ToolStripMenuItem.Text = "Max-Min [Int32]"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(180, 6)
        '
        'ResetStackingToolStripMenuItem1
        '
        Me.ResetStackingToolStripMenuItem1.Name = "ResetStackingToolStripMenuItem1"
        Me.ResetStackingToolStripMenuItem1.Size = New System.Drawing.Size(183, 22)
        Me.ResetStackingToolStripMenuItem1.Text = "Reset stacking"
        '
        'MaxImageToolStripMenuItem
        '
        Me.MaxImageToolStripMenuItem.Name = "MaxImageToolStripMenuItem"
        Me.MaxImageToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.MaxImageToolStripMenuItem.Text = "Max image"
        '
        'tsmiSaveImageData
        '
        Me.tsmiSaveImageData.Name = "tsmiSaveImageData"
        Me.tsmiSaveImageData.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.tsmiSaveImageData.Size = New System.Drawing.Size(300, 22)
        Me.tsmiSaveImageData.Text = "Save current image"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(297, 6)
        '
        'ClearStatisticsMemoryToolStripMenuItem
        '
        Me.ClearStatisticsMemoryToolStripMenuItem.Name = "ClearStatisticsMemoryToolStripMenuItem"
        Me.ClearStatisticsMemoryToolStripMenuItem.Size = New System.Drawing.Size(300, 22)
        Me.ClearStatisticsMemoryToolStripMenuItem.Text = "Clear statistics memory"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(297, 6)
        '
        'OpenEXELocationToolStripMenuItem
        '
        Me.OpenEXELocationToolStripMenuItem.Name = "OpenEXELocationToolStripMenuItem"
        Me.OpenEXELocationToolStripMenuItem.Size = New System.Drawing.Size(300, 22)
        Me.OpenEXELocationToolStripMenuItem.Text = "Open EXE location"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(297, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(300, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'SpecialAnalysisToolStripMenuItem
        '
        Me.SpecialAnalysisToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RowAndColumnStatisticsToolStripMenuItem, Me.PlotStatisticsVsGainToolStripMenuItem, Me.ReplotStatisticsToolStripMenuItem, Me.ADUQuantizationToolStripMenuItem, Me.tsmiCalcVignette, Me.HotPixelToolStripMenuItem, Me.ToolStripMenuItem7, Me.MultifileAreaCompareToolStripMenuItem, Me.tsmiManualColorBalancer})
        Me.SpecialAnalysisToolStripMenuItem.Name = "SpecialAnalysisToolStripMenuItem"
        Me.SpecialAnalysisToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.SpecialAnalysisToolStripMenuItem.Text = "Special analysis"
        '
        'RowAndColumnStatisticsToolStripMenuItem
        '
        Me.RowAndColumnStatisticsToolStripMenuItem.Name = "RowAndColumnStatisticsToolStripMenuItem"
        Me.RowAndColumnStatisticsToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.RowAndColumnStatisticsToolStripMenuItem.Text = "Row and column statistics"
        '
        'PlotStatisticsVsGainToolStripMenuItem
        '
        Me.PlotStatisticsVsGainToolStripMenuItem.Name = "PlotStatisticsVsGainToolStripMenuItem"
        Me.PlotStatisticsVsGainToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.PlotStatisticsVsGainToolStripMenuItem.Text = "Plot statistics vs gain"
        '
        'ReplotStatisticsToolStripMenuItem
        '
        Me.ReplotStatisticsToolStripMenuItem.Name = "ReplotStatisticsToolStripMenuItem"
        Me.ReplotStatisticsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.ReplotStatisticsToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.ReplotStatisticsToolStripMenuItem.Text = "Re-plot statistics"
        '
        'ADUQuantizationToolStripMenuItem
        '
        Me.ADUQuantizationToolStripMenuItem.Name = "ADUQuantizationToolStripMenuItem"
        Me.ADUQuantizationToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
        Me.ADUQuantizationToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.ADUQuantizationToolStripMenuItem.Text = "ADU quantization"
        '
        'tsmiCalcVignette
        '
        Me.tsmiCalcVignette.Name = "tsmiCalcVignette"
        Me.tsmiCalcVignette.Size = New System.Drawing.Size(212, 22)
        Me.tsmiCalcVignette.Text = "Vignette"
        '
        'HotPixelToolStripMenuItem
        '
        Me.HotPixelToolStripMenuItem.Name = "HotPixelToolStripMenuItem"
        Me.HotPixelToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.HotPixelToolStripMenuItem.Text = "Hot pixel"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(209, 6)
        '
        'MultifileAreaCompareToolStripMenuItem
        '
        Me.MultifileAreaCompareToolStripMenuItem.Name = "MultifileAreaCompareToolStripMenuItem"
        Me.MultifileAreaCompareToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.MultifileAreaCompareToolStripMenuItem.Text = "Multi-file area compare"
        '
        'tsmiManualColorBalancer
        '
        Me.tsmiManualColorBalancer.Name = "tsmiManualColorBalancer"
        Me.tsmiManualColorBalancer.Size = New System.Drawing.Size(212, 22)
        Me.tsmiManualColorBalancer.Text = "Manual color balancer"
        '
        'tsmiProcessing
        '
        Me.tsmiProcessing.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiAdjustRGB, Me.tsmiStretch, Me.tsmiPlateSolve, Me.tsmiCorrectVignette, Me.tsmiSetPixelToValue})
        Me.tsmiProcessing.Name = "tsmiProcessing"
        Me.tsmiProcessing.Size = New System.Drawing.Size(76, 22)
        Me.tsmiProcessing.Text = "Processing"
        '
        'tsmiAdjustRGB
        '
        Me.tsmiAdjustRGB.Name = "tsmiAdjustRGB"
        Me.tsmiAdjustRGB.Size = New System.Drawing.Size(301, 22)
        Me.tsmiAdjustRGB.Text = "Adjust RGB channels (using modus)"
        '
        'tsmiStretch
        '
        Me.tsmiStretch.Name = "tsmiStretch"
        Me.tsmiStretch.Size = New System.Drawing.Size(301, 22)
        Me.tsmiStretch.Text = "Stretcher histogramm over complete range"
        '
        'tsmiPlateSolve
        '
        Me.tsmiPlateSolve.Name = "tsmiPlateSolve"
        Me.tsmiPlateSolve.Size = New System.Drawing.Size(301, 22)
        Me.tsmiPlateSolve.Text = "Plate solve image"
        '
        'tsmiCorrectVignette
        '
        Me.tsmiCorrectVignette.Name = "tsmiCorrectVignette"
        Me.tsmiCorrectVignette.Size = New System.Drawing.Size(301, 22)
        Me.tsmiCorrectVignette.Text = "Correct vignette"
        '
        'tsmiSetPixelToValue
        '
        Me.tsmiSetPixelToValue.Name = "tsmiSetPixelToValue"
        Me.tsmiSetPixelToValue.Size = New System.Drawing.Size(301, 22)
        Me.tsmiSetPixelToValue.Text = "Set pixel above to certain value"
        '
        'TestCodeToolStripMenuItem
        '
        Me.TestCodeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WriteTestDataToolStripMenuItem1, Me.AfiineTranslateToolStripMenuItem, Me.BitGrayscaleFileGenerationToolStripMenuItem, Me.ASCOMDynamicallyToolStripMenuItem, Me.ToolStripMenuItem6, Me.FocusToolStripMenuItem, Me.FITSTestFilesToolStripMenuItem, Me.NEFReadingToolStripMenuItem, Me.SetPixelAboveCertainValueToolStripMenuItem, Me.ToolStripMenuItem9, Me.tsmiUseOpenCV, Me.OpenCVMedianFilterToolStripMenuItem})
        Me.TestCodeToolStripMenuItem.Name = "TestCodeToolStripMenuItem"
        Me.TestCodeToolStripMenuItem.Size = New System.Drawing.Size(68, 22)
        Me.TestCodeToolStripMenuItem.Text = "Test code"
        '
        'WriteTestDataToolStripMenuItem1
        '
        Me.WriteTestDataToolStripMenuItem1.Name = "WriteTestDataToolStripMenuItem1"
        Me.WriteTestDataToolStripMenuItem1.Size = New System.Drawing.Size(336, 22)
        Me.WriteTestDataToolStripMenuItem1.Text = "Write test data"
        '
        'AfiineTranslateToolStripMenuItem
        '
        Me.AfiineTranslateToolStripMenuItem.Name = "AfiineTranslateToolStripMenuItem"
        Me.AfiineTranslateToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.AfiineTranslateToolStripMenuItem.Text = "Afiine translate"
        '
        'BitGrayscaleFileGenerationToolStripMenuItem
        '
        Me.BitGrayscaleFileGenerationToolStripMenuItem.Name = "BitGrayscaleFileGenerationToolStripMenuItem"
        Me.BitGrayscaleFileGenerationToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.BitGrayscaleFileGenerationToolStripMenuItem.Text = "16bit grayscale file generation"
        '
        'ASCOMDynamicallyToolStripMenuItem
        '
        Me.ASCOMDynamicallyToolStripMenuItem.Name = "ASCOMDynamicallyToolStripMenuItem"
        Me.ASCOMDynamicallyToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.ASCOMDynamicallyToolStripMenuItem.Text = "ASCOM dynamically"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(333, 6)
        '
        'FocusToolStripMenuItem
        '
        Me.FocusToolStripMenuItem.Name = "FocusToolStripMenuItem"
        Me.FocusToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.FocusToolStripMenuItem.Text = "Focus"
        '
        'FITSTestFilesToolStripMenuItem
        '
        Me.FITSTestFilesToolStripMenuItem.Name = "FITSTestFilesToolStripMenuItem"
        Me.FITSTestFilesToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.FITSTestFilesToolStripMenuItem.Text = "FITS Test Files"
        '
        'NEFReadingToolStripMenuItem
        '
        Me.NEFReadingToolStripMenuItem.Name = "NEFReadingToolStripMenuItem"
        Me.NEFReadingToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.NEFReadingToolStripMenuItem.Text = "NEF reading"
        '
        'SetPixelAboveCertainValueToolStripMenuItem
        '
        Me.SetPixelAboveCertainValueToolStripMenuItem.Name = "SetPixelAboveCertainValueToolStripMenuItem"
        Me.SetPixelAboveCertainValueToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.SetPixelAboveCertainValueToolStripMenuItem.Text = "Set pixel above certain value"
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(333, 6)
        '
        'tsmiUseOpenCV
        '
        Me.tsmiUseOpenCV.Name = "tsmiUseOpenCV"
        Me.tsmiUseOpenCV.Size = New System.Drawing.Size(336, 22)
        Me.tsmiUseOpenCV.Text = "Use OpenCV"
        '
        'OpenCVMedianFilterToolStripMenuItem
        '
        Me.OpenCVMedianFilterToolStripMenuItem.Name = "OpenCVMedianFilterToolStripMenuItem"
        Me.OpenCVMedianFilterToolStripMenuItem.Size = New System.Drawing.Size(336, 22)
        Me.OpenCVMedianFilterToolStripMenuItem.Text = "OpenCV Median Filter (ERROR: Row-Colum issue)"
        '
        'DisplayImageToolStripMenuItem
        '
        Me.DisplayImageToolStripMenuItem.Name = "DisplayImageToolStripMenuItem"
        Me.DisplayImageToolStripMenuItem.Size = New System.Drawing.Size(93, 22)
        Me.DisplayImageToolStripMenuItem.Text = "Display image"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CoordsForALADINCallToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(46, 22)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'CoordsForALADINCallToolStripMenuItem
        '
        Me.CoordsForALADINCallToolStripMenuItem.Name = "CoordsForALADINCallToolStripMenuItem"
        Me.CoordsForALADINCallToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.CoordsForALADINCallToolStripMenuItem.Text = "Coords for ALADIN call"
        '
        'ofdMain
        '
        Me.ofdMain.Multiselect = True
        '
        'tbLogOutput
        '
        Me.tbLogOutput.AllowDrop = True
        Me.tbLogOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLogOutput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLogOutput.Location = New System.Drawing.Point(3, 3)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(1289, 1125)
        Me.tbLogOutput.TabIndex = 3
        Me.tbLogOutput.WordWrap = False
        '
        'ssMain
        '
        Me.ssMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslRunning, Me.tsslMain, Me.tspbMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 1161)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Padding = New System.Windows.Forms.Padding(1, 0, 9, 0)
        Me.ssMain.Size = New System.Drawing.Size(1692, 22)
        Me.ssMain.TabIndex = 4
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslRunning
        '
        Me.tsslRunning.Font = New System.Drawing.Font("Wingdings", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.tsslRunning.ForeColor = System.Drawing.Color.Silver
        Me.tsslRunning.Name = "tsslRunning"
        Me.tsslRunning.Size = New System.Drawing.Size(17, 17)
        Me.tsslRunning.Text = "l"
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(22, 17)
        Me.tsslMain.Text = "---"
        '
        'tspbMain
        '
        Me.tspbMain.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.tspbMain.Name = "tspbMain"
        Me.tspbMain.Size = New System.Drawing.Size(100, 16)
        Me.tspbMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'pgMain
        '
        Me.pgMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgMain.Location = New System.Drawing.Point(3, 3)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(363, 564)
        Me.pgMain.TabIndex = 5
        Me.pgMain.ToolbarVisible = False
        '
        'gbDetails
        '
        Me.gbDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDetails.Controls.Add(Me.tbDetails)
        Me.gbDetails.Location = New System.Drawing.Point(3, 3)
        Me.gbDetails.Name = "gbDetails"
        Me.gbDetails.Size = New System.Drawing.Size(363, 551)
        Me.gbDetails.TabIndex = 6
        Me.gbDetails.TabStop = False
        Me.gbDetails.Text = "Details"
        '
        'tbDetails
        '
        Me.tbDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDetails.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbDetails.Location = New System.Drawing.Point(6, 19)
        Me.tbDetails.Multiline = True
        Me.tbDetails.Name = "tbDetails"
        Me.tbDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbDetails.Size = New System.Drawing.Size(351, 526)
        Me.tbDetails.TabIndex = 0
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(12, 27)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.scLeft)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.tbLogOutput)
        Me.scMain.Size = New System.Drawing.Size(1668, 1131)
        Me.scMain.SplitterDistance = 369
        Me.scMain.TabIndex = 7
        '
        'scLeft
        '
        Me.scLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scLeft.Location = New System.Drawing.Point(0, 0)
        Me.scLeft.Name = "scLeft"
        Me.scLeft.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scLeft.Panel1
        '
        Me.scLeft.Panel1.Controls.Add(Me.pgMain)
        '
        'scLeft.Panel2
        '
        Me.scLeft.Panel2.Controls.Add(Me.gbDetails)
        Me.scLeft.Size = New System.Drawing.Size(369, 1131)
        Me.scLeft.SplitterDistance = 570
        Me.scLeft.TabIndex = 0
        '
        'tsmiSaveFITSAndStats
        '
        Me.tsmiSaveFITSAndStats.Name = "tsmiSaveFITSAndStats"
        Me.tsmiSaveFITSAndStats.Size = New System.Drawing.Size(300, 22)
        Me.tsmiSaveFITSAndStats.Text = "Save FITS and statistics summary"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1692, 1183)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.msMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.msMain
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Astro Image Statistics Version 0.3"
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.gbDetails.ResumeLayout(False)
        Me.gbDetails.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.Panel2.PerformLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.scLeft.Panel1.ResumeLayout(False)
        Me.scLeft.Panel2.ResumeLayout(False)
        CType(Me.scLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scLeft.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents msMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiOpenFile As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents tbLogOutput As TextBox
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tsslMain As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents tsmiOpenLastFile As ToolStripMenuItem
    Friend WithEvents TestCodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenEXELocationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WriteTestDataToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents StoreStackingResultToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiSaveMeanFile As ToolStripMenuItem
    Friend WithEvents StdDevImageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents SumImageDoubleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MaxMinInt32ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SpecialAnalysisToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RowAndColumnStatisticsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PlotStatisticsVsGainToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pgMain As PropertyGrid
    Friend WithEvents ReplotStatisticsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AfiineTranslateToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiSaveLastStatXLS As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents ResetStackingToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents tsmiProcessing As ToolStripMenuItem
    Friend WithEvents tsmiAdjustRGB As ToolStripMenuItem
    Friend WithEvents tsmiSaveImageData As ToolStripMenuItem
    Friend WithEvents sfdMain As SaveFileDialog
    Friend WithEvents tsmiStretch As ToolStripMenuItem
    Friend WithEvents tsslRunning As ToolStripStatusLabel
    Friend WithEvents BitGrayscaleFileGenerationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripSeparator
    Friend WithEvents ADUQuantizationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiPlateSolve As ToolStripMenuItem
    Friend WithEvents tsmiCalcVignette As ToolStripMenuItem
    Friend WithEvents tsmiCorrectVignette As ToolStripMenuItem
    Friend WithEvents FITSGrepToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ASCOMDynamicallyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents ClearStatisticsMemoryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripSeparator
    Friend WithEvents FocusToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HotPixelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FITSTestFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As ToolStripSeparator
    Friend WithEvents MultifileAreaCompareToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tspbMain As ToolStripProgressBar
    Friend WithEvents tsmiOpenRecentFiles As ToolStripMenuItem
    Friend WithEvents gbDetails As GroupBox
    Friend WithEvents tbDetails As TextBox
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents scLeft As SplitContainer
    Friend WithEvents MaxImageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As ToolStripSeparator
    Friend WithEvents tsmiSaveAllFilesStat As ToolStripMenuItem
    Friend WithEvents NEFReadingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SetPixelAboveCertainValueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiSetPixelToValue As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem9 As ToolStripSeparator
    Friend WithEvents tsmiUseOpenCV As ToolStripMenuItem
    Friend WithEvents OpenCVMedianFilterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DisplayImageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CoordsForALADINCallToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiManualColorBalancer As ToolStripMenuItem
    Friend WithEvents tsmiSaveFITSAndStats As ToolStripMenuItem
End Class
