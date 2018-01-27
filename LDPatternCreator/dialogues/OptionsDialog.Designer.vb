<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsDialog
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.TCOptions = New System.Windows.Forms.TabControl()
        Me.TPSettings = New System.Windows.Forms.TabPage()
        Me.GBMisc = New System.Windows.Forms.GroupBox()
        Me.CBAddModeLock = New System.Windows.Forms.CheckBox()
        Me.CBTemplateLinesTop = New System.Windows.Forms.CheckBox()
        Me.CBAlternativeZoomAndTrans = New System.Windows.Forms.CheckBox()
        Me.GBStartup = New System.Windows.Forms.GroupBox()
        Me.CBViewPreferences = New System.Windows.Forms.CheckBox()
        Me.CBViewImage = New System.Windows.Forms.CheckBox()
        Me.CBFullscreen = New System.Windows.Forms.CheckBox()
        Me.GBUndo = New System.Windows.Forms.GroupBox()
        Me.CBPerformaceMode = New System.Windows.Forms.CheckBox()
        Me.NUDMaxUndo = New System.Windows.Forms.NumericUpDown()
        Me.LblMaxUndo = New System.Windows.Forms.Label()
        Me.GBMeta = New System.Windows.Forms.GroupBox()
        Me.CBLicense = New System.Windows.Forms.ComboBox()
        Me.TBUserName = New System.Windows.Forms.TextBox()
        Me.TBRealName = New System.Windows.Forms.TextBox()
        Me.LblLicense = New System.Windows.Forms.Label()
        Me.LblUser = New System.Windows.Forms.Label()
        Me.LblRealName = New System.Windows.Forms.Label()
        Me.LblAuthor = New System.Windows.Forms.Label()
        Me.TPKeys = New System.Windows.Forms.TabPage()
        Me.Shortkeys = New System.Windows.Forms.DataGridView()
        Me.CName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CShortKey = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CSet = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.CData = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPColours = New System.Windows.Forms.TabPage()
        Me.GBObjectcolour = New System.Windows.Forms.GroupBox()
        Me.BtnCopy = New System.Windows.Forms.Button()
        Me.BtnPaste = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.HR = New System.Windows.Forms.HScrollBar()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.HG = New System.Windows.Forms.HScrollBar()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.HB = New System.Windows.Forms.HScrollBar()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.HA = New System.Windows.Forms.HScrollBar()
        Me.PNew = New System.Windows.Forms.Panel()
        Me.POld = New System.Windows.Forms.Panel()
        Me.LblNew = New System.Windows.Forms.Label()
        Me.LblOld = New System.Windows.Forms.Label()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.PGradient = New System.Windows.Forms.Panel()
        Me.PBack = New System.Windows.Forms.Panel()
        Me.GBObject = New System.Windows.Forms.GroupBox()
        Me.LColours = New System.Windows.Forms.ListBox()
        Me.LTags = New System.Windows.Forms.ListBox()
        Me.BtnRestoreDefaults = New System.Windows.Forms.Button()
        Me.CDChooseColour = New System.Windows.Forms.ColorDialog()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TCOptions.SuspendLayout()
        Me.TPSettings.SuspendLayout()
        Me.GBMisc.SuspendLayout()
        Me.GBStartup.SuspendLayout()
        Me.GBUndo.SuspendLayout()
        CType(Me.NUDMaxUndo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GBMeta.SuspendLayout()
        Me.TPKeys.SuspendLayout()
        CType(Me.Shortkeys, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TPColours.SuspendLayout()
        Me.GBObjectcolour.SuspendLayout()
        Me.GBObject.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(445, 422)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(178, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(93, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(80, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(80, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'TCOptions
        '
        Me.TCOptions.Controls.Add(Me.TPSettings)
        Me.TCOptions.Controls.Add(Me.TPKeys)
        Me.TCOptions.Controls.Add(Me.TPColours)
        Me.TCOptions.Location = New System.Drawing.Point(17, 12)
        Me.TCOptions.Name = "TCOptions"
        Me.TCOptions.SelectedIndex = 0
        Me.TCOptions.Size = New System.Drawing.Size(595, 398)
        Me.TCOptions.TabIndex = 1
        '
        'TPSettings
        '
        Me.TPSettings.Controls.Add(Me.GBMisc)
        Me.TPSettings.Controls.Add(Me.GBStartup)
        Me.TPSettings.Controls.Add(Me.GBUndo)
        Me.TPSettings.Controls.Add(Me.GBMeta)
        Me.TPSettings.Location = New System.Drawing.Point(4, 22)
        Me.TPSettings.Name = "TPSettings"
        Me.TPSettings.Size = New System.Drawing.Size(587, 372)
        Me.TPSettings.TabIndex = 2
        Me.TPSettings.Text = "Settings:"
        Me.TPSettings.UseVisualStyleBackColor = True
        '
        'GBMisc
        '
        Me.GBMisc.Controls.Add(Me.CBAddModeLock)
        Me.GBMisc.Controls.Add(Me.CBTemplateLinesTop)
        Me.GBMisc.Controls.Add(Me.CBAlternativeZoomAndTrans)
        Me.GBMisc.Location = New System.Drawing.Point(10, 288)
        Me.GBMisc.Name = "GBMisc"
        Me.GBMisc.Size = New System.Drawing.Size(563, 81)
        Me.GBMisc.TabIndex = 3
        Me.GBMisc.TabStop = False
        Me.GBMisc.Text = "Misc.:"
        '
        'CBAddModeLock
        '
        Me.CBAddModeLock.AutoSize = True
        Me.CBAddModeLock.Location = New System.Drawing.Point(6, 64)
        Me.CBAddModeLock.Name = "CBAddModeLock"
        Me.CBAddModeLock.Size = New System.Drawing.Size(361, 17)
        Me.CBAddModeLock.TabIndex = 2
        Me.CBAddModeLock.Text = "When 'Add...' is activated, the program locks the Vertex/Triangle Mode"
        Me.CBAddModeLock.UseVisualStyleBackColor = True
        '
        'CBTemplateLinesTop
        '
        Me.CBTemplateLinesTop.AutoSize = True
        Me.CBTemplateLinesTop.Location = New System.Drawing.Point(6, 41)
        Me.CBTemplateLinesTop.Name = "CBTemplateLinesTop"
        Me.CBTemplateLinesTop.Size = New System.Drawing.Size(151, 17)
        Me.CBTemplateLinesTop.TabIndex = 1
        Me.CBTemplateLinesTop.Text = "Draw template lines on top"
        Me.CBTemplateLinesTop.UseVisualStyleBackColor = True
        '
        'CBAlternativeZoomAndTrans
        '
        Me.CBAlternativeZoomAndTrans.AutoSize = True
        Me.CBAlternativeZoomAndTrans.Location = New System.Drawing.Point(6, 19)
        Me.CBAlternativeZoomAndTrans.Name = "CBAlternativeZoomAndTrans"
        Me.CBAlternativeZoomAndTrans.Size = New System.Drawing.Size(237, 17)
        Me.CBAlternativeZoomAndTrans.TabIndex = 0
        Me.CBAlternativeZoomAndTrans.Text = "Use alternative keys for zoom and translation"
        Me.CBAlternativeZoomAndTrans.UseVisualStyleBackColor = True
        '
        'GBStartup
        '
        Me.GBStartup.Controls.Add(Me.CBViewPreferences)
        Me.GBStartup.Controls.Add(Me.CBViewImage)
        Me.GBStartup.Controls.Add(Me.CBFullscreen)
        Me.GBStartup.Location = New System.Drawing.Point(10, 193)
        Me.GBStartup.Name = "GBStartup"
        Me.GBStartup.Size = New System.Drawing.Size(563, 89)
        Me.GBStartup.TabIndex = 2
        Me.GBStartup.TabStop = False
        Me.GBStartup.Text = "Startup:"
        '
        'CBViewPreferences
        '
        Me.CBViewPreferences.AutoSize = True
        Me.CBViewPreferences.Checked = True
        Me.CBViewPreferences.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CBViewPreferences.Location = New System.Drawing.Point(6, 65)
        Me.CBViewPreferences.Name = "CBViewPreferences"
        Me.CBViewPreferences.Size = New System.Drawing.Size(143, 17)
        Me.CBViewPreferences.TabIndex = 2
        Me.CBViewPreferences.Text = "Show 'Preferences'-View"
        Me.CBViewPreferences.UseVisualStyleBackColor = True
        '
        'CBViewImage
        '
        Me.CBViewImage.AutoSize = True
        Me.CBViewImage.Checked = True
        Me.CBViewImage.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CBViewImage.Location = New System.Drawing.Point(6, 42)
        Me.CBViewImage.Name = "CBViewImage"
        Me.CBViewImage.Size = New System.Drawing.Size(115, 17)
        Me.CBViewImage.TabIndex = 1
        Me.CBViewImage.Text = "Show 'Image'-View"
        Me.CBViewImage.UseVisualStyleBackColor = True
        '
        'CBFullscreen
        '
        Me.CBFullscreen.AutoSize = True
        Me.CBFullscreen.Checked = True
        Me.CBFullscreen.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CBFullscreen.Location = New System.Drawing.Point(6, 19)
        Me.CBFullscreen.Name = "CBFullscreen"
        Me.CBFullscreen.Size = New System.Drawing.Size(121, 17)
        Me.CBFullscreen.TabIndex = 0
        Me.CBFullscreen.Text = "Start with Fullscreen"
        Me.CBFullscreen.UseVisualStyleBackColor = True
        '
        'GBUndo
        '
        Me.GBUndo.Controls.Add(Me.CBPerformaceMode)
        Me.GBUndo.Controls.Add(Me.NUDMaxUndo)
        Me.GBUndo.Controls.Add(Me.LblMaxUndo)
        Me.GBUndo.Location = New System.Drawing.Point(10, 121)
        Me.GBUndo.Name = "GBUndo"
        Me.GBUndo.Size = New System.Drawing.Size(563, 66)
        Me.GBUndo.TabIndex = 1
        Me.GBUndo.TabStop = False
        Me.GBUndo.Text = "Undo/Redo:"
        '
        'CBPerformaceMode
        '
        Me.CBPerformaceMode.AutoSize = True
        Me.CBPerformaceMode.Location = New System.Drawing.Point(75, 37)
        Me.CBPerformaceMode.Name = "CBPerformaceMode"
        Me.CBPerformaceMode.Size = New System.Drawing.Size(220, 17)
        Me.CBPerformaceMode.TabIndex = 2
        Me.CBPerformaceMode.Text = "Disable Undo/Redo (Performance Mode)"
        Me.CBPerformaceMode.UseVisualStyleBackColor = True
        '
        'NUDMaxUndo
        '
        Me.NUDMaxUndo.Location = New System.Drawing.Point(9, 34)
        Me.NUDMaxUndo.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.NUDMaxUndo.Minimum = New Decimal(New Integer() {3, 0, 0, 0})
        Me.NUDMaxUndo.Name = "NUDMaxUndo"
        Me.NUDMaxUndo.Size = New System.Drawing.Size(60, 20)
        Me.NUDMaxUndo.TabIndex = 1
        Me.NUDMaxUndo.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'LblMaxUndo
        '
        Me.LblMaxUndo.AutoSize = True
        Me.LblMaxUndo.Location = New System.Drawing.Point(6, 18)
        Me.LblMaxUndo.Name = "LblMaxUndo"
        Me.LblMaxUndo.Size = New System.Drawing.Size(62, 13)
        Me.LblMaxUndo.TabIndex = 0
        Me.LblMaxUndo.Text = "Max. Undo:"
        '
        'GBMeta
        '
        Me.GBMeta.Controls.Add(Me.CBLicense)
        Me.GBMeta.Controls.Add(Me.TBUserName)
        Me.GBMeta.Controls.Add(Me.TBRealName)
        Me.GBMeta.Controls.Add(Me.LblLicense)
        Me.GBMeta.Controls.Add(Me.LblUser)
        Me.GBMeta.Controls.Add(Me.LblRealName)
        Me.GBMeta.Controls.Add(Me.LblAuthor)
        Me.GBMeta.Location = New System.Drawing.Point(10, 17)
        Me.GBMeta.Name = "GBMeta"
        Me.GBMeta.Size = New System.Drawing.Size(563, 98)
        Me.GBMeta.TabIndex = 0
        Me.GBMeta.TabStop = False
        Me.GBMeta.Text = "Metadata Defaults:"
        '
        'CBLicense
        '
        Me.CBLicense.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBLicense.FormattingEnabled = True
        Me.CBLicense.Items.AddRange(New Object() {"", "Redistributable under CCAL version 2.0 : see CAreadme.txt", "Not redistributable : see NonCAreadme.txt"})
        Me.CBLicense.Location = New System.Drawing.Point(6, 71)
        Me.CBLicense.Name = "CBLicense"
        Me.CBLicense.Size = New System.Drawing.Size(532, 21)
        Me.CBLicense.TabIndex = 14
        '
        'TBUserName
        '
        Me.TBUserName.Location = New System.Drawing.Point(357, 32)
        Me.TBUserName.Name = "TBUserName"
        Me.TBUserName.Size = New System.Drawing.Size(181, 20)
        Me.TBUserName.TabIndex = 10
        '
        'TBRealName
        '
        Me.TBRealName.Location = New System.Drawing.Point(124, 32)
        Me.TBRealName.Name = "TBRealName"
        Me.TBRealName.Size = New System.Drawing.Size(192, 20)
        Me.TBRealName.TabIndex = 9
        '
        'LblLicense
        '
        Me.LblLicense.AutoSize = True
        Me.LblLicense.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.LblLicense.Location = New System.Drawing.Point(6, 55)
        Me.LblLicense.Name = "LblLicense"
        Me.LblLicense.Size = New System.Drawing.Size(74, 13)
        Me.LblLicense.TabIndex = 17
        Me.LblLicense.Text = "License-Type:"
        '
        'LblUser
        '
        Me.LblUser.AutoSize = True
        Me.LblUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.LblUser.Location = New System.Drawing.Point(354, 16)
        Me.LblUser.Name = "LblUser"
        Me.LblUser.Size = New System.Drawing.Size(58, 13)
        Me.LblUser.TabIndex = 15
        Me.LblUser.Text = "Username:"
        '
        'LblRealName
        '
        Me.LblRealName.AutoSize = True
        Me.LblRealName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.LblRealName.Location = New System.Drawing.Point(121, 16)
        Me.LblRealName.Name = "LblRealName"
        Me.LblRealName.Size = New System.Drawing.Size(63, 13)
        Me.LblRealName.TabIndex = 13
        Me.LblRealName.Text = "Real Name:"
        '
        'LblAuthor
        '
        Me.LblAuthor.AutoSize = True
        Me.LblAuthor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.LblAuthor.Location = New System.Drawing.Point(6, 16)
        Me.LblAuthor.Name = "LblAuthor"
        Me.LblAuthor.Size = New System.Drawing.Size(41, 13)
        Me.LblAuthor.TabIndex = 11
        Me.LblAuthor.Text = "Author:"
        '
        'TPKeys
        '
        Me.TPKeys.Controls.Add(Me.Shortkeys)
        Me.TPKeys.Location = New System.Drawing.Point(4, 22)
        Me.TPKeys.Name = "TPKeys"
        Me.TPKeys.Padding = New System.Windows.Forms.Padding(3)
        Me.TPKeys.Size = New System.Drawing.Size(587, 372)
        Me.TPKeys.TabIndex = 1
        Me.TPKeys.Text = "Hotkeys:"
        Me.TPKeys.UseVisualStyleBackColor = True
        '
        'Shortkeys
        '
        Me.Shortkeys.AllowUserToAddRows = False
        Me.Shortkeys.AllowUserToDeleteRows = False
        Me.Shortkeys.AllowUserToResizeColumns = False
        Me.Shortkeys.AllowUserToResizeRows = False
        Me.Shortkeys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Shortkeys.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CName, Me.CShortKey, Me.CSet, Me.CData})
        Me.Shortkeys.Location = New System.Drawing.Point(7, 7)
        Me.Shortkeys.Name = "Shortkeys"
        Me.Shortkeys.ReadOnly = True
        Me.Shortkeys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Shortkeys.ShowCellErrors = False
        Me.Shortkeys.ShowRowErrors = False
        Me.Shortkeys.Size = New System.Drawing.Size(574, 336)
        Me.Shortkeys.TabIndex = 0
        '
        'CName
        '
        Me.CName.Frozen = True
        Me.CName.HeaderText = "Function"
        Me.CName.Name = "CName"
        Me.CName.ReadOnly = True
        Me.CName.Width = 250
        '
        'CShortKey
        '
        Me.CShortKey.Frozen = True
        Me.CShortKey.HeaderText = "Key-Combination"
        Me.CShortKey.Name = "CShortKey"
        Me.CShortKey.ReadOnly = True
        Me.CShortKey.Width = 180
        '
        'CSet
        '
        Me.CSet.Frozen = True
        Me.CSet.HeaderText = ""
        Me.CSet.Name = "CSet"
        Me.CSet.ReadOnly = True
        Me.CSet.Text = ""
        '
        'CData
        '
        Me.CData.Frozen = True
        Me.CData.HeaderText = "Data"
        Me.CData.Name = "CData"
        Me.CData.ReadOnly = True
        Me.CData.Visible = False
        '
        'TPColours
        '
        Me.TPColours.Controls.Add(Me.GBObjectcolour)
        Me.TPColours.Controls.Add(Me.GBObject)
        Me.TPColours.Location = New System.Drawing.Point(4, 22)
        Me.TPColours.Name = "TPColours"
        Me.TPColours.Padding = New System.Windows.Forms.Padding(3)
        Me.TPColours.Size = New System.Drawing.Size(587, 372)
        Me.TPColours.TabIndex = 0
        Me.TPColours.Text = "Colours:"
        Me.TPColours.UseVisualStyleBackColor = True
        '
        'GBObjectcolour
        '
        Me.GBObjectcolour.Controls.Add(Me.BtnCopy)
        Me.GBObjectcolour.Controls.Add(Me.BtnPaste)
        Me.GBObjectcolour.Controls.Add(Me.Label11)
        Me.GBObjectcolour.Controls.Add(Me.HR)
        Me.GBObjectcolour.Controls.Add(Me.Label10)
        Me.GBObjectcolour.Controls.Add(Me.HG)
        Me.GBObjectcolour.Controls.Add(Me.Label9)
        Me.GBObjectcolour.Controls.Add(Me.HB)
        Me.GBObjectcolour.Controls.Add(Me.Label8)
        Me.GBObjectcolour.Controls.Add(Me.HA)
        Me.GBObjectcolour.Controls.Add(Me.PNew)
        Me.GBObjectcolour.Controls.Add(Me.POld)
        Me.GBObjectcolour.Controls.Add(Me.LblNew)
        Me.GBObjectcolour.Controls.Add(Me.LblOld)
        Me.GBObjectcolour.Controls.Add(Me.BtnApply)
        Me.GBObjectcolour.Controls.Add(Me.PGradient)
        Me.GBObjectcolour.Controls.Add(Me.PBack)
        Me.GBObjectcolour.Location = New System.Drawing.Point(231, 7)
        Me.GBObjectcolour.Name = "GBObjectcolour"
        Me.GBObjectcolour.Size = New System.Drawing.Size(350, 359)
        Me.GBObjectcolour.TabIndex = 2
        Me.GBObjectcolour.TabStop = False
        Me.GBObjectcolour.Text = "Object-Colour:"
        '
        'BtnCopy
        '
        Me.BtnCopy.Location = New System.Drawing.Point(19, 325)
        Me.BtnCopy.Name = "BtnCopy"
        Me.BtnCopy.Size = New System.Drawing.Size(100, 23)
        Me.BtnCopy.TabIndex = 15
        Me.BtnCopy.Text = "Copy"
        Me.BtnCopy.UseVisualStyleBackColor = True
        '
        'BtnPaste
        '
        Me.BtnPaste.Location = New System.Drawing.Point(125, 325)
        Me.BtnPaste.Name = "BtnPaste"
        Me.BtnPaste.Size = New System.Drawing.Size(100, 23)
        Me.BtnPaste.TabIndex = 16
        Me.BtnPaste.Text = "Paste"
        Me.BtnPaste.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(33, 90)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(18, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "R:"
        '
        'HR
        '
        Me.HR.LargeChange = 25
        Me.HR.Location = New System.Drawing.Point(78, 83)
        Me.HR.Maximum = 280
        Me.HR.Name = "HR"
        Me.HR.Size = New System.Drawing.Size(213, 20)
        Me.HR.TabIndex = 11
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(33, 110)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(18, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "G:"
        '
        'HG
        '
        Me.HG.LargeChange = 25
        Me.HG.Location = New System.Drawing.Point(78, 103)
        Me.HG.Maximum = 280
        Me.HG.Name = "HG"
        Me.HG.Size = New System.Drawing.Size(213, 20)
        Me.HG.TabIndex = 9
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(33, 130)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(17, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "B:"
        '
        'HB
        '
        Me.HB.LargeChange = 25
        Me.HB.Location = New System.Drawing.Point(78, 123)
        Me.HB.Maximum = 280
        Me.HB.Name = "HB"
        Me.HB.Size = New System.Drawing.Size(213, 20)
        Me.HB.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(33, 150)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(17, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "A:"
        '
        'HA
        '
        Me.HA.LargeChange = 25
        Me.HA.Location = New System.Drawing.Point(78, 143)
        Me.HA.Maximum = 280
        Me.HA.Name = "HA"
        Me.HA.Size = New System.Drawing.Size(213, 20)
        Me.HA.TabIndex = 5
        '
        'PNew
        '
        Me.PNew.Location = New System.Drawing.Point(107, 220)
        Me.PNew.Name = "PNew"
        Me.PNew.Size = New System.Drawing.Size(60, 20)
        Me.PNew.TabIndex = 4
        '
        'POld
        '
        Me.POld.Location = New System.Drawing.Point(107, 201)
        Me.POld.Name = "POld"
        Me.POld.Size = New System.Drawing.Size(60, 20)
        Me.POld.TabIndex = 3
        '
        'LblNew
        '
        Me.LblNew.AutoSize = True
        Me.LblNew.Location = New System.Drawing.Point(33, 227)
        Me.LblNew.Name = "LblNew"
        Me.LblNew.Size = New System.Drawing.Size(32, 13)
        Me.LblNew.TabIndex = 2
        Me.LblNew.Text = "New:"
        '
        'LblOld
        '
        Me.LblOld.AutoSize = True
        Me.LblOld.Location = New System.Drawing.Point(33, 201)
        Me.LblOld.Name = "LblOld"
        Me.LblOld.Size = New System.Drawing.Size(26, 13)
        Me.LblOld.TabIndex = 1
        Me.LblOld.Text = "Old:"
        '
        'BtnApply
        '
        Me.BtnApply.Location = New System.Drawing.Point(231, 325)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(100, 23)
        Me.BtnApply.TabIndex = 0
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'PGradient
        '
        Me.PGradient.Location = New System.Drawing.Point(84, 63)
        Me.PGradient.Name = "PGradient"
        Me.PGradient.Size = New System.Drawing.Size(200, 20)
        Me.PGradient.TabIndex = 13
        '
        'PBack
        '
        Me.PBack.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(1, Byte), Integer), CType(CType(1, Byte), Integer))
        Me.PBack.Location = New System.Drawing.Point(100, 195)
        Me.PBack.Name = "PBack"
        Me.PBack.Size = New System.Drawing.Size(75, 53)
        Me.PBack.TabIndex = 14
        '
        'GBObject
        '
        Me.GBObject.Controls.Add(Me.LColours)
        Me.GBObject.Controls.Add(Me.LTags)
        Me.GBObject.Location = New System.Drawing.Point(7, 7)
        Me.GBObject.Name = "GBObject"
        Me.GBObject.Size = New System.Drawing.Size(216, 359)
        Me.GBObject.TabIndex = 1
        Me.GBObject.TabStop = False
        Me.GBObject.Text = "Object:"
        '
        'LColours
        '
        Me.LColours.FormattingEnabled = True
        Me.LColours.Location = New System.Drawing.Point(6, 19)
        Me.LColours.Name = "LColours"
        Me.LColours.Size = New System.Drawing.Size(204, 329)
        Me.LColours.TabIndex = 0
        '
        'LTags
        '
        Me.LTags.FormattingEnabled = True
        Me.LTags.Location = New System.Drawing.Point(6, 19)
        Me.LTags.Name = "LTags"
        Me.LTags.Size = New System.Drawing.Size(204, 186)
        Me.LTags.TabIndex = 13
        Me.LTags.Visible = False
        '
        'BtnRestoreDefaults
        '
        Me.BtnRestoreDefaults.Location = New System.Drawing.Point(17, 426)
        Me.BtnRestoreDefaults.Name = "BtnRestoreDefaults"
        Me.BtnRestoreDefaults.Size = New System.Drawing.Size(212, 23)
        Me.BtnRestoreDefaults.TabIndex = 2
        Me.BtnRestoreDefaults.Text = "Restore Defaults"
        Me.BtnRestoreDefaults.UseVisualStyleBackColor = True
        '
        'OptionsDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(635, 463)
        Me.Controls.Add(Me.BtnRestoreDefaults)
        Me.Controls.Add(Me.TCOptions)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OptionsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options: "
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TCOptions.ResumeLayout(False)
        Me.TPSettings.ResumeLayout(False)
        Me.GBMisc.ResumeLayout(False)
        Me.GBMisc.PerformLayout()
        Me.GBStartup.ResumeLayout(False)
        Me.GBStartup.PerformLayout()
        Me.GBUndo.ResumeLayout(False)
        Me.GBUndo.PerformLayout()
        CType(Me.NUDMaxUndo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GBMeta.ResumeLayout(False)
        Me.GBMeta.PerformLayout()
        Me.TPKeys.ResumeLayout(False)
        CType(Me.Shortkeys, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TPColours.ResumeLayout(False)
        Me.GBObjectcolour.ResumeLayout(False)
        Me.GBObjectcolour.PerformLayout()
        Me.GBObject.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TCOptions As System.Windows.Forms.TabControl
    Friend WithEvents TPColours As System.Windows.Forms.TabPage
    Friend WithEvents TPKeys As System.Windows.Forms.TabPage
    Friend WithEvents TPSettings As System.Windows.Forms.TabPage
    Friend WithEvents GBMeta As System.Windows.Forms.GroupBox
    Friend WithEvents CBLicense As System.Windows.Forms.ComboBox
    Friend WithEvents TBUserName As System.Windows.Forms.TextBox
    Friend WithEvents TBRealName As System.Windows.Forms.TextBox
    Friend WithEvents LblLicense As System.Windows.Forms.Label
    Friend WithEvents LblUser As System.Windows.Forms.Label
    Friend WithEvents LblRealName As System.Windows.Forms.Label
    Friend WithEvents LblAuthor As System.Windows.Forms.Label
    Friend WithEvents GBUndo As System.Windows.Forms.GroupBox
    Friend WithEvents NUDMaxUndo As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblMaxUndo As System.Windows.Forms.Label
    Friend WithEvents CBPerformaceMode As System.Windows.Forms.CheckBox
    Friend WithEvents GBStartup As System.Windows.Forms.GroupBox
    Friend WithEvents CBFullscreen As System.Windows.Forms.CheckBox
    Friend WithEvents CBViewPreferences As System.Windows.Forms.CheckBox
    Friend WithEvents CBViewImage As System.Windows.Forms.CheckBox
    Friend WithEvents Shortkeys As System.Windows.Forms.DataGridView
    Friend WithEvents BtnRestoreDefaults As System.Windows.Forms.Button
    Friend WithEvents GBMisc As System.Windows.Forms.GroupBox
    Friend WithEvents CBAlternativeZoomAndTrans As System.Windows.Forms.CheckBox
    Friend WithEvents CDChooseColour As System.Windows.Forms.ColorDialog
    Friend WithEvents GBObjectcolour As System.Windows.Forms.GroupBox
    Friend WithEvents PNew As System.Windows.Forms.Panel
    Friend WithEvents POld As System.Windows.Forms.Panel
    Friend WithEvents LblNew As System.Windows.Forms.Label
    Friend WithEvents LblOld As System.Windows.Forms.Label
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents GBObject As System.Windows.Forms.GroupBox
    Friend WithEvents LColours As System.Windows.Forms.ListBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents HR As System.Windows.Forms.HScrollBar
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents HG As System.Windows.Forms.HScrollBar
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents HB As System.Windows.Forms.HScrollBar
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents HA As System.Windows.Forms.HScrollBar
    Friend WithEvents LTags As System.Windows.Forms.ListBox
    Friend WithEvents PGradient As System.Windows.Forms.Panel
    Friend WithEvents PBack As System.Windows.Forms.Panel
    Friend WithEvents BtnPaste As System.Windows.Forms.Button
    Friend WithEvents BtnCopy As System.Windows.Forms.Button
    Friend WithEvents CName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CShortKey As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CSet As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents CData As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CBTemplateLinesTop As System.Windows.Forms.CheckBox
    Friend WithEvents CBAddModeLock As System.Windows.Forms.CheckBox

End Class
