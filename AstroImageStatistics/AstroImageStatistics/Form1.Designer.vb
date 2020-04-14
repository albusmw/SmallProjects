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
        Me.FITSGrepToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileToAnalyseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOpenLastFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.StoreStatisticsEXCELFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.StoreStackingResultToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSaveMeanFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.SumImageDoubleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StdDevImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MaxMinInt32ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ResetStackingToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSaveImageData = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.BasicProcessingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdjustRGBChannelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StretcherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPlateSolve = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiCorrectVignette = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestCodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveOverscanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WriteTestDataToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.AfiineTranslateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BitGrayscaleFileGenerationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ASCOMDynamicallyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.FocusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslRunning = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.sfdMain = New System.Windows.Forms.SaveFileDialog()
        Me.HotPixelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.msMain.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'msMain
        '
        Me.msMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.SpecialAnalysisToolStripMenuItem, Me.BasicProcessingToolStripMenuItem, Me.TestCodeToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.msMain.Size = New System.Drawing.Size(1498, 24)
        Me.msMain.TabIndex = 0
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FITSGrepToolStripMenuItem, Me.OpenFileToAnalyseToolStripMenuItem, Me.tsmiOpenLastFile, Me.ToolStripMenuItem2, Me.StoreStatisticsEXCELFileToolStripMenuItem, Me.ToolStripMenuItem4, Me.StoreStackingResultToolStripMenuItem, Me.tsmiSaveImageData, Me.ClearStatisticsMemoryToolStripMenuItem, Me.ToolStripMenuItem1, Me.OpenEXELocationToolStripMenuItem, Me.ToolStripMenuItem5, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 22)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'FITSGrepToolStripMenuItem
        '
        Me.FITSGrepToolStripMenuItem.Name = "FITSGrepToolStripMenuItem"
        Me.FITSGrepToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
        Me.FITSGrepToolStripMenuItem.Text = "FITS Grep"
        '
        'OpenFileToAnalyseToolStripMenuItem
        '
        Me.OpenFileToAnalyseToolStripMenuItem.Name = "OpenFileToAnalyseToolStripMenuItem"
        Me.OpenFileToAnalyseToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.OpenFileToAnalyseToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
        Me.OpenFileToAnalyseToolStripMenuItem.Text = "Open file(s) to analyse"
        '
        'tsmiOpenLastFile
        '
        Me.tsmiOpenLastFile.Name = "tsmiOpenLastFile"
        Me.tsmiOpenLastFile.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.tsmiOpenLastFile.Size = New System.Drawing.Size(246, 22)
        Me.tsmiOpenLastFile.Text = "Open last file processed"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(243, 6)
        '
        'StoreStatisticsEXCELFileToolStripMenuItem
        '
        Me.StoreStatisticsEXCELFileToolStripMenuItem.Name = "StoreStatisticsEXCELFileToolStripMenuItem"
        Me.StoreStatisticsEXCELFileToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.StoreStatisticsEXCELFileToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
        Me.StoreStatisticsEXCELFileToolStripMenuItem.Text = "Store statistics EXCEL file"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(243, 6)
        '
        'StoreStackingResultToolStripMenuItem
        '
        Me.StoreStackingResultToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiSaveMeanFile, Me.SumImageDoubleToolStripMenuItem, Me.StdDevImageToolStripMenuItem, Me.MaxMinInt32ToolStripMenuItem, Me.ToolStripMenuItem3, Me.ResetStackingToolStripMenuItem1})
        Me.StoreStackingResultToolStripMenuItem.Name = "StoreStackingResultToolStripMenuItem"
        Me.StoreStackingResultToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
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
        'tsmiSaveImageData
        '
        Me.tsmiSaveImageData.Name = "tsmiSaveImageData"
        Me.tsmiSaveImageData.Size = New System.Drawing.Size(246, 22)
        Me.tsmiSaveImageData.Text = "Save current image"
        '
        'ClearStatisticsMemoryToolStripMenuItem
        '
        Me.ClearStatisticsMemoryToolStripMenuItem.Name = "ClearStatisticsMemoryToolStripMenuItem"
        Me.ClearStatisticsMemoryToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
        Me.ClearStatisticsMemoryToolStripMenuItem.Text = "Clear statistics memory"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(243, 6)
        '
        'OpenEXELocationToolStripMenuItem
        '
        Me.OpenEXELocationToolStripMenuItem.Name = "OpenEXELocationToolStripMenuItem"
        Me.OpenEXELocationToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
        Me.OpenEXELocationToolStripMenuItem.Text = "Open EXE location"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(243, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'SpecialAnalysisToolStripMenuItem
        '
        Me.SpecialAnalysisToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RowAndColumnStatisticsToolStripMenuItem, Me.PlotStatisticsVsGainToolStripMenuItem, Me.ReplotStatisticsToolStripMenuItem, Me.ADUQuantizationToolStripMenuItem, Me.tsmiCalcVignette, Me.HotPixelToolStripMenuItem})
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
        'BasicProcessingToolStripMenuItem
        '
        Me.BasicProcessingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdjustRGBChannelsToolStripMenuItem, Me.StretcherToolStripMenuItem, Me.tsmiPlateSolve, Me.tsmiCorrectVignette})
        Me.BasicProcessingToolStripMenuItem.Name = "BasicProcessingToolStripMenuItem"
        Me.BasicProcessingToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.BasicProcessingToolStripMenuItem.Text = "Basic processing"
        '
        'AdjustRGBChannelsToolStripMenuItem
        '
        Me.AdjustRGBChannelsToolStripMenuItem.Name = "AdjustRGBChannelsToolStripMenuItem"
        Me.AdjustRGBChannelsToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.AdjustRGBChannelsToolStripMenuItem.Text = "Adjust RGB channels"
        '
        'StretcherToolStripMenuItem
        '
        Me.StretcherToolStripMenuItem.Name = "StretcherToolStripMenuItem"
        Me.StretcherToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.StretcherToolStripMenuItem.Text = "Stretcher"
        '
        'tsmiPlateSolve
        '
        Me.tsmiPlateSolve.Name = "tsmiPlateSolve"
        Me.tsmiPlateSolve.Size = New System.Drawing.Size(183, 22)
        Me.tsmiPlateSolve.Text = "Plate solve image"
        '
        'tsmiCorrectVignette
        '
        Me.tsmiCorrectVignette.Name = "tsmiCorrectVignette"
        Me.tsmiCorrectVignette.Size = New System.Drawing.Size(183, 22)
        Me.tsmiCorrectVignette.Text = "Correct vignette"
        '
        'TestCodeToolStripMenuItem
        '
        Me.TestCodeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveOverscanToolStripMenuItem, Me.WriteTestDataToolStripMenuItem1, Me.AfiineTranslateToolStripMenuItem, Me.BitGrayscaleFileGenerationToolStripMenuItem, Me.ASCOMDynamicallyToolStripMenuItem, Me.ToolStripMenuItem6, Me.FocusToolStripMenuItem})
        Me.TestCodeToolStripMenuItem.Name = "TestCodeToolStripMenuItem"
        Me.TestCodeToolStripMenuItem.Size = New System.Drawing.Size(68, 22)
        Me.TestCodeToolStripMenuItem.Text = "Test code"
        '
        'RemoveOverscanToolStripMenuItem
        '
        Me.RemoveOverscanToolStripMenuItem.Name = "RemoveOverscanToolStripMenuItem"
        Me.RemoveOverscanToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.RemoveOverscanToolStripMenuItem.Text = "Remove overscan"
        '
        'WriteTestDataToolStripMenuItem1
        '
        Me.WriteTestDataToolStripMenuItem1.Name = "WriteTestDataToolStripMenuItem1"
        Me.WriteTestDataToolStripMenuItem1.Size = New System.Drawing.Size(231, 22)
        Me.WriteTestDataToolStripMenuItem1.Text = "Write test data"
        '
        'AfiineTranslateToolStripMenuItem
        '
        Me.AfiineTranslateToolStripMenuItem.Name = "AfiineTranslateToolStripMenuItem"
        Me.AfiineTranslateToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.AfiineTranslateToolStripMenuItem.Text = "Afiine translate"
        '
        'BitGrayscaleFileGenerationToolStripMenuItem
        '
        Me.BitGrayscaleFileGenerationToolStripMenuItem.Name = "BitGrayscaleFileGenerationToolStripMenuItem"
        Me.BitGrayscaleFileGenerationToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.BitGrayscaleFileGenerationToolStripMenuItem.Text = "16bit grayscale file generation"
        '
        'ASCOMDynamicallyToolStripMenuItem
        '
        Me.ASCOMDynamicallyToolStripMenuItem.Name = "ASCOMDynamicallyToolStripMenuItem"
        Me.ASCOMDynamicallyToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.ASCOMDynamicallyToolStripMenuItem.Text = "ASCOM dynamically"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(228, 6)
        '
        'FocusToolStripMenuItem
        '
        Me.FocusToolStripMenuItem.Name = "FocusToolStripMenuItem"
        Me.FocusToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.FocusToolStripMenuItem.Text = "Focus"
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
        Me.tbLogOutput.Location = New System.Drawing.Point(266, 25)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(1225, 1021)
        Me.tbLogOutput.TabIndex = 3
        Me.tbLogOutput.WordWrap = False
        '
        'ssMain
        '
        Me.ssMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslRunning, Me.tsslMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 1049)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Padding = New System.Windows.Forms.Padding(1, 0, 9, 0)
        Me.ssMain.Size = New System.Drawing.Size(1498, 22)
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
        'pgMain
        '
        Me.pgMain.Location = New System.Drawing.Point(12, 27)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(248, 641)
        Me.pgMain.TabIndex = 5
        '
        'HotPixelToolStripMenuItem
        '
        Me.HotPixelToolStripMenuItem.Name = "HotPixelToolStripMenuItem"
        Me.HotPixelToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.HotPixelToolStripMenuItem.Text = "Hot pixel"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1498, 1071)
        Me.Controls.Add(Me.pgMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.tbLogOutput)
        Me.Controls.Add(Me.msMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.msMain
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Astro Image Statistics Version 0.2"
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents msMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenFileToAnalyseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents tbLogOutput As TextBox
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents tsslMain As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents tsmiOpenLastFile As ToolStripMenuItem
    Friend WithEvents TestCodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveOverscanToolStripMenuItem As ToolStripMenuItem
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
    Friend WithEvents StoreStatisticsEXCELFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents ResetStackingToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents BasicProcessingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AdjustRGBChannelsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiSaveImageData As ToolStripMenuItem
    Friend WithEvents sfdMain As SaveFileDialog
    Friend WithEvents StretcherToolStripMenuItem As ToolStripMenuItem
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
End Class
