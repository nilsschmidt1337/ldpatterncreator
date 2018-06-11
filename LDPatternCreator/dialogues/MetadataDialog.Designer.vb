<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MetadataDialog
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
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("(No Subfiles)")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Main File", New System.Windows.Forms.TreeNode() {TreeNode1})
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.GBPartTree = New System.Windows.Forms.GroupBox()
        Me.TVTree = New System.Windows.Forms.TreeView()
        Me.LblPartDescription = New System.Windows.Forms.Label()
        Me.LblFilename = New System.Windows.Forms.Label()
        Me.LblAuthor = New System.Windows.Forms.Label()
        Me.LblRealname = New System.Windows.Forms.Label()
        Me.LblUsername = New System.Windows.Forms.Label()
        Me.LblParttype = New System.Windows.Forms.Label()
        Me.LblLicense = New System.Windows.Forms.Label()
        Me.LblBFC = New System.Windows.Forms.Label()
        Me.LblCategory = New System.Windows.Forms.Label()
        Me.LblKeywords = New System.Windows.Forms.Label()
        Me.LblHistory = New System.Windows.Forms.Label()
        Me.LblComments = New System.Windows.Forms.Label()
        Me.TBDescription = New System.Windows.Forms.TextBox()
        Me.TBFilename = New System.Windows.Forms.TextBox()
        Me.TBRealName = New System.Windows.Forms.TextBox()
        Me.TBUserName = New System.Windows.Forms.TextBox()
        Me.CBPartType = New System.Windows.Forms.ComboBox()
        Me.CBLicense = New System.Windows.Forms.ComboBox()
        Me.CBBFC = New System.Windows.Forms.ComboBox()
        Me.RTBHelp = New System.Windows.Forms.RichTextBox()
        Me.RTBKeywords = New System.Windows.Forms.RichTextBox()
        Me.RTBHistory = New System.Windows.Forms.RichTextBox()
        Me.TBCategory = New System.Windows.Forms.TextBox()
        Me.BtnDefault = New System.Windows.Forms.Button()
        Me.RTBComments = New System.Windows.Forms.RichTextBox()
        Me.LblHelp = New System.Windows.Forms.Label()
        Me.CBInclude = New System.Windows.Forms.CheckBox()
        Me.GBPreview = New System.Windows.Forms.GroupBox()
        Me.PPreview = New System.Windows.Forms.Panel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.CBPreview = New System.Windows.Forms.CheckBox()
        Me.GBDatExport = New System.Windows.Forms.GroupBox()
        Me.CBSlicerPro = New System.Windows.Forms.CheckBox()
        Me.CBColourReplacer = New System.Windows.Forms.CheckBox()
        Me.CBRectifier = New System.Windows.Forms.CheckBox()
        Me.CBUnificator = New System.Windows.Forms.CheckBox()
        Me.CBUnificatorLPC = New System.Windows.Forms.CheckBox()
        Me.CBProjector = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GBPartTree.SuspendLayout()
        Me.GBPreview.SuspendLayout()
        Me.GBDatExport.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(525, 569)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(175, 29)
        Me.TableLayoutPanel1.TabIndex = 12
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(80, 23)
        Me.OK_Button.TabIndex = 12
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(91, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(80, 23)
        Me.Cancel_Button.TabIndex = 13
        Me.Cancel_Button.Text = "Cancel"
        '
        'GBPartTree
        '
        Me.GBPartTree.Controls.Add(Me.TVTree)
        Me.GBPartTree.Location = New System.Drawing.Point(13, 13)
        Me.GBPartTree.Name = "GBPartTree"
        Me.GBPartTree.Size = New System.Drawing.Size(195, 211)
        Me.GBPartTree.TabIndex = 15
        Me.GBPartTree.TabStop = False
        Me.GBPartTree.Text = "Part-Tree:"
        '
        'TVTree
        '
        Me.TVTree.FullRowSelect = True
        Me.TVTree.HideSelection = False
        Me.TVTree.Location = New System.Drawing.Point(7, 20)
        Me.TVTree.Name = "TVTree"
        TreeNode1.Name = "KnotSub"
        TreeNode1.Text = "(No Subfiles)"
        TreeNode2.Checked = True
        TreeNode2.Name = "KnotMain"
        TreeNode2.Text = "Main File"
        Me.TVTree.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode2})
        Me.TVTree.Size = New System.Drawing.Size(182, 185)
        Me.TVTree.TabIndex = 15
        '
        'LblPartDescription
        '
        Me.LblPartDescription.AutoSize = True
        Me.LblPartDescription.Location = New System.Drawing.Point(214, 12)
        Me.LblPartDescription.Name = "LblPartDescription"
        Me.LblPartDescription.Size = New System.Drawing.Size(85, 13)
        Me.LblPartDescription.TabIndex = 2
        Me.LblPartDescription.Text = "Part-Description:"
        '
        'LblFilename
        '
        Me.LblFilename.AutoSize = True
        Me.LblFilename.Location = New System.Drawing.Point(214, 51)
        Me.LblFilename.Name = "LblFilename"
        Me.LblFilename.Size = New System.Drawing.Size(52, 13)
        Me.LblFilename.TabIndex = 3
        Me.LblFilename.Text = "Filename:"
        '
        'LblAuthor
        '
        Me.LblAuthor.AutoSize = True
        Me.LblAuthor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic)
        Me.LblAuthor.Location = New System.Drawing.Point(214, 90)
        Me.LblAuthor.Name = "LblAuthor"
        Me.LblAuthor.Size = New System.Drawing.Size(41, 13)
        Me.LblAuthor.TabIndex = 4
        Me.LblAuthor.Text = "Author:"
        '
        'LblRealname
        '
        Me.LblRealname.AutoSize = True
        Me.LblRealname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic)
        Me.LblRealname.Location = New System.Drawing.Point(289, 90)
        Me.LblRealname.Name = "LblRealname"
        Me.LblRealname.Size = New System.Drawing.Size(63, 13)
        Me.LblRealname.TabIndex = 5
        Me.LblRealname.Text = "Real Name:"
        '
        'LblUsername
        '
        Me.LblUsername.AutoSize = True
        Me.LblUsername.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic)
        Me.LblUsername.Location = New System.Drawing.Point(496, 90)
        Me.LblUsername.Name = "LblUsername"
        Me.LblUsername.Size = New System.Drawing.Size(58, 13)
        Me.LblUsername.TabIndex = 6
        Me.LblUsername.Text = "Username:"
        '
        'LblParttype
        '
        Me.LblParttype.AutoSize = True
        Me.LblParttype.Location = New System.Drawing.Point(214, 131)
        Me.LblParttype.Name = "LblParttype"
        Me.LblParttype.Size = New System.Drawing.Size(56, 13)
        Me.LblParttype.TabIndex = 7
        Me.LblParttype.Text = "Part-Type:"
        '
        'LblLicense
        '
        Me.LblLicense.AutoSize = True
        Me.LblLicense.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic)
        Me.LblLicense.Location = New System.Drawing.Point(214, 171)
        Me.LblLicense.Name = "LblLicense"
        Me.LblLicense.Size = New System.Drawing.Size(74, 13)
        Me.LblLicense.TabIndex = 8
        Me.LblLicense.Text = "License-Type:"
        '
        'LblBFC
        '
        Me.LblBFC.AutoSize = True
        Me.LblBFC.Location = New System.Drawing.Point(214, 275)
        Me.LblBFC.Name = "LblBFC"
        Me.LblBFC.Size = New System.Drawing.Size(81, 13)
        Me.LblBFC.TabIndex = 9
        Me.LblBFC.Text = "BFC-Statement:"
        '
        'LblCategory
        '
        Me.LblCategory.AutoSize = True
        Me.LblCategory.Location = New System.Drawing.Point(214, 315)
        Me.LblCategory.Name = "LblCategory"
        Me.LblCategory.Size = New System.Drawing.Size(52, 13)
        Me.LblCategory.TabIndex = 10
        Me.LblCategory.Text = "Category:"
        '
        'LblKeywords
        '
        Me.LblKeywords.AutoSize = True
        Me.LblKeywords.Location = New System.Drawing.Point(214, 354)
        Me.LblKeywords.Name = "LblKeywords"
        Me.LblKeywords.Size = New System.Drawing.Size(56, 13)
        Me.LblKeywords.TabIndex = 11
        Me.LblKeywords.Text = "Keywords:"
        '
        'LblHistory
        '
        Me.LblHistory.AutoSize = True
        Me.LblHistory.Location = New System.Drawing.Point(214, 418)
        Me.LblHistory.Name = "LblHistory"
        Me.LblHistory.Size = New System.Drawing.Size(42, 13)
        Me.LblHistory.TabIndex = 12
        Me.LblHistory.Text = "History:"
        '
        'LblComments
        '
        Me.LblComments.AutoSize = True
        Me.LblComments.Location = New System.Drawing.Point(214, 482)
        Me.LblComments.Name = "LblComments"
        Me.LblComments.Size = New System.Drawing.Size(59, 13)
        Me.LblComments.TabIndex = 13
        Me.LblComments.Text = "Comments:"
        '
        'TBDescription
        '
        Me.TBDescription.Location = New System.Drawing.Point(217, 28)
        Me.TBDescription.MaxLength = 1024
        Me.TBDescription.Name = "TBDescription"
        Me.TBDescription.Size = New System.Drawing.Size(479, 20)
        Me.TBDescription.TabIndex = 0
        '
        'TBFilename
        '
        Me.TBFilename.Location = New System.Drawing.Point(217, 67)
        Me.TBFilename.Name = "TBFilename"
        Me.TBFilename.Size = New System.Drawing.Size(479, 20)
        Me.TBFilename.TabIndex = 1
        '
        'TBRealName
        '
        Me.TBRealName.Location = New System.Drawing.Point(292, 108)
        Me.TBRealName.Name = "TBRealName"
        Me.TBRealName.Size = New System.Drawing.Size(197, 20)
        Me.TBRealName.TabIndex = 2
        '
        'TBUserName
        '
        Me.TBUserName.Location = New System.Drawing.Point(499, 108)
        Me.TBUserName.Name = "TBUserName"
        Me.TBUserName.Size = New System.Drawing.Size(197, 20)
        Me.TBUserName.TabIndex = 3
        '
        'CBPartType
        '
        Me.CBPartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBPartType.FormattingEnabled = True
        Me.CBPartType.Items.AddRange(New Object() {"", "Part", "Subpart", "Unofficial_Part", "Unofficial_Subpart"})
        Me.CBPartType.Location = New System.Drawing.Point(217, 147)
        Me.CBPartType.Name = "CBPartType"
        Me.CBPartType.Size = New System.Drawing.Size(479, 21)
        Me.CBPartType.TabIndex = 4
        '
        'CBLicense
        '
        Me.CBLicense.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBLicense.FormattingEnabled = True
        Me.CBLicense.Items.AddRange(New Object() {"", "Redistributable under CCAL version 2.0 : see CAreadme.txt", "Not redistributable : see NonCAreadme.txt"})
        Me.CBLicense.Location = New System.Drawing.Point(217, 187)
        Me.CBLicense.Name = "CBLicense"
        Me.CBLicense.Size = New System.Drawing.Size(479, 21)
        Me.CBLicense.TabIndex = 5
        '
        'CBBFC
        '
        Me.CBBFC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBBFC.FormattingEnabled = True
        Me.CBBFC.Items.AddRange(New Object() {"", "BFC CERTIFY CCW", "BFC CERTIFY CW", "BFC NOCERTIFY"})
        Me.CBBFC.Location = New System.Drawing.Point(217, 291)
        Me.CBBFC.Name = "CBBFC"
        Me.CBBFC.Size = New System.Drawing.Size(479, 21)
        Me.CBBFC.TabIndex = 7
        '
        'RTBHelp
        '
        Me.RTBHelp.Location = New System.Drawing.Point(217, 227)
        Me.RTBHelp.Name = "RTBHelp"
        Me.RTBHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RTBHelp.Size = New System.Drawing.Size(479, 45)
        Me.RTBHelp.TabIndex = 6
        Me.RTBHelp.Text = ""
        '
        'RTBKeywords
        '
        Me.RTBKeywords.Location = New System.Drawing.Point(217, 370)
        Me.RTBKeywords.Name = "RTBKeywords"
        Me.RTBKeywords.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RTBKeywords.Size = New System.Drawing.Size(479, 45)
        Me.RTBKeywords.TabIndex = 9
        Me.RTBKeywords.Text = ""
        '
        'RTBHistory
        '
        Me.RTBHistory.Location = New System.Drawing.Point(217, 434)
        Me.RTBHistory.Name = "RTBHistory"
        Me.RTBHistory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RTBHistory.Size = New System.Drawing.Size(479, 45)
        Me.RTBHistory.TabIndex = 10
        Me.RTBHistory.Text = ""
        '
        'TBCategory
        '
        Me.TBCategory.Location = New System.Drawing.Point(217, 331)
        Me.TBCategory.Name = "TBCategory"
        Me.TBCategory.Size = New System.Drawing.Size(479, 20)
        Me.TBCategory.TabIndex = 8
        '
        'BtnDefault
        '
        Me.BtnDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic)
        Me.BtnDefault.Location = New System.Drawing.Point(11, 578)
        Me.BtnDefault.Name = "BtnDefault"
        Me.BtnDefault.Size = New System.Drawing.Size(152, 23)
        Me.BtnDefault.TabIndex = 14
        Me.BtnDefault.Text = "Set as Default"
        Me.BtnDefault.UseVisualStyleBackColor = True
        '
        'RTBComments
        '
        Me.RTBComments.Location = New System.Drawing.Point(217, 498)
        Me.RTBComments.Name = "RTBComments"
        Me.RTBComments.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RTBComments.Size = New System.Drawing.Size(479, 45)
        Me.RTBComments.TabIndex = 11
        Me.RTBComments.Text = ""
        '
        'LblHelp
        '
        Me.LblHelp.AutoSize = True
        Me.LblHelp.Location = New System.Drawing.Point(214, 211)
        Me.LblHelp.Name = "LblHelp"
        Me.LblHelp.Size = New System.Drawing.Size(32, 13)
        Me.LblHelp.TabIndex = 28
        Me.LblHelp.Text = "Help:"
        '
        'CBInclude
        '
        Me.CBInclude.AutoSize = True
        Me.CBInclude.Location = New System.Drawing.Point(6, 19)
        Me.CBInclude.Name = "CBInclude"
        Me.CBInclude.Size = New System.Drawing.Size(109, 17)
        Me.CBInclude.TabIndex = 29
        Me.CBInclude.Text = "Include Metadata"
        Me.CBInclude.UseVisualStyleBackColor = True
        '
        'GBPreview
        '
        Me.GBPreview.Controls.Add(Me.PPreview)
        Me.GBPreview.Location = New System.Drawing.Point(19, 418)
        Me.GBPreview.Name = "GBPreview"
        Me.GBPreview.Size = New System.Drawing.Size(134, 125)
        Me.GBPreview.TabIndex = 30
        Me.GBPreview.TabStop = False
        Me.GBPreview.Text = "Preview:"
        '
        'PPreview
        '
        Me.PPreview.BackColor = System.Drawing.Color.Black
        Me.PPreview.Location = New System.Drawing.Point(7, 20)
        Me.PPreview.Name = "PPreview"
        Me.PPreview.Size = New System.Drawing.Size(120, 100)
        Me.PPreview.TabIndex = 0
        '
        'Timer1
        '
        '
        'CBPreview
        '
        Me.CBPreview.AutoSize = True
        Me.CBPreview.Checked = True
        Me.CBPreview.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CBPreview.Location = New System.Drawing.Point(18, 549)
        Me.CBPreview.Name = "CBPreview"
        Me.CBPreview.Size = New System.Drawing.Size(100, 17)
        Me.CBPreview.TabIndex = 31
        Me.CBPreview.Text = "Enable Preview"
        Me.CBPreview.UseVisualStyleBackColor = True
        '
        'GBDatExport
        '
        Me.GBDatExport.Controls.Add(Me.CBSlicerPro)
        Me.GBDatExport.Controls.Add(Me.CBColourReplacer)
        Me.GBDatExport.Controls.Add(Me.CBRectifier)
        Me.GBDatExport.Controls.Add(Me.CBUnificator)
        Me.GBDatExport.Controls.Add(Me.CBUnificatorLPC)
        Me.GBDatExport.Controls.Add(Me.CBProjector)
        Me.GBDatExport.Controls.Add(Me.CBInclude)
        Me.GBDatExport.Location = New System.Drawing.Point(13, 231)
        Me.GBDatExport.Name = "GBDatExport"
        Me.GBDatExport.Size = New System.Drawing.Size(195, 184)
        Me.GBDatExport.TabIndex = 32
        Me.GBDatExport.TabStop = False
        Me.GBDatExport.Text = "DAT-Export:"
        '
        'CBSlicerPro
        '
        Me.CBSlicerPro.AutoSize = True
        Me.CBSlicerPro.Location = New System.Drawing.Point(6, 134)
        Me.CBSlicerPro.Name = "CBSlicerPro"
        Me.CBSlicerPro.Size = New System.Drawing.Size(88, 17)
        Me.CBSlicerPro.TabIndex = 35
        Me.CBSlicerPro.Text = "SlicerPro.exe"
        Me.CBSlicerPro.UseVisualStyleBackColor = True
        '
        'CBColourReplacer
        '
        Me.CBColourReplacer.AutoSize = True
        Me.CBColourReplacer.Location = New System.Drawing.Point(6, 157)
        Me.CBColourReplacer.Name = "CBColourReplacer"
        Me.CBColourReplacer.Size = New System.Drawing.Size(125, 17)
        Me.CBColourReplacer.TabIndex = 34
        Me.CBColourReplacer.Text = "LPC-Colour-Replacer"
        Me.CBColourReplacer.UseVisualStyleBackColor = True
        '
        'CBRectifier
        '
        Me.CBRectifier.AutoSize = True
        Me.CBRectifier.Location = New System.Drawing.Point(6, 111)
        Me.CBRectifier.Name = "CBRectifier"
        Me.CBRectifier.Size = New System.Drawing.Size(85, 17)
        Me.CBRectifier.TabIndex = 33
        Me.CBRectifier.Text = "Rectifier.exe"
        Me.CBRectifier.UseVisualStyleBackColor = True
        '
        'CBUnificator
        '
        Me.CBUnificator.AutoSize = True
        Me.CBUnificator.Location = New System.Drawing.Point(6, 88)
        Me.CBUnificator.Name = "CBUnificator"
        Me.CBUnificator.Size = New System.Drawing.Size(91, 17)
        Me.CBUnificator.TabIndex = 32
        Me.CBUnificator.Text = "Unificator.exe"
        Me.CBUnificator.UseVisualStyleBackColor = True
        '
        'CBUnificatorLPC
        '
        Me.CBUnificatorLPC.AutoSize = True
        Me.CBUnificatorLPC.Location = New System.Drawing.Point(6, 65)
        Me.CBUnificatorLPC.Name = "CBUnificatorLPC"
        Me.CBUnificatorLPC.Size = New System.Drawing.Size(94, 17)
        Me.CBUnificatorLPC.TabIndex = 31
        Me.CBUnificatorLPC.Text = "LPC-Unificator"
        Me.CBUnificatorLPC.UseVisualStyleBackColor = True
        '
        'CBProjector
        '
        Me.CBProjector.AutoSize = True
        Me.CBProjector.Location = New System.Drawing.Point(6, 42)
        Me.CBProjector.Name = "CBProjector"
        Me.CBProjector.Size = New System.Drawing.Size(91, 17)
        Me.CBProjector.TabIndex = 30
        Me.CBProjector.Text = "LPC-Projector"
        Me.CBProjector.UseVisualStyleBackColor = True
        '
        'MetadataDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(712, 610)
        Me.Controls.Add(Me.GBDatExport)
        Me.Controls.Add(Me.CBPreview)
        Me.Controls.Add(Me.GBPreview)
        Me.Controls.Add(Me.LblHelp)
        Me.Controls.Add(Me.RTBComments)
        Me.Controls.Add(Me.BtnDefault)
        Me.Controls.Add(Me.TBCategory)
        Me.Controls.Add(Me.RTBHistory)
        Me.Controls.Add(Me.RTBKeywords)
        Me.Controls.Add(Me.RTBHelp)
        Me.Controls.Add(Me.CBBFC)
        Me.Controls.Add(Me.CBLicense)
        Me.Controls.Add(Me.CBPartType)
        Me.Controls.Add(Me.TBUserName)
        Me.Controls.Add(Me.TBRealName)
        Me.Controls.Add(Me.TBFilename)
        Me.Controls.Add(Me.TBDescription)
        Me.Controls.Add(Me.LblComments)
        Me.Controls.Add(Me.LblHistory)
        Me.Controls.Add(Me.LblKeywords)
        Me.Controls.Add(Me.LblCategory)
        Me.Controls.Add(Me.LblBFC)
        Me.Controls.Add(Me.LblLicense)
        Me.Controls.Add(Me.LblParttype)
        Me.Controls.Add(Me.LblUsername)
        Me.Controls.Add(Me.LblRealname)
        Me.Controls.Add(Me.LblAuthor)
        Me.Controls.Add(Me.LblFilename)
        Me.Controls.Add(Me.LblPartDescription)
        Me.Controls.Add(Me.GBPartTree)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MetadataDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Metadata: "
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GBPartTree.ResumeLayout(False)
        Me.GBPreview.ResumeLayout(False)
        Me.GBDatExport.ResumeLayout(False)
        Me.GBDatExport.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GBPartTree As System.Windows.Forms.GroupBox
    Friend WithEvents TVTree As System.Windows.Forms.TreeView
    Friend WithEvents LblPartDescription As System.Windows.Forms.Label
    Friend WithEvents LblFilename As System.Windows.Forms.Label
    Friend WithEvents LblAuthor As System.Windows.Forms.Label
    Friend WithEvents LblRealname As System.Windows.Forms.Label
    Friend WithEvents LblUsername As System.Windows.Forms.Label
    Friend WithEvents LblParttype As System.Windows.Forms.Label
    Friend WithEvents LblLicense As System.Windows.Forms.Label
    Friend WithEvents LblBFC As System.Windows.Forms.Label
    Friend WithEvents LblCategory As System.Windows.Forms.Label
    Friend WithEvents LblKeywords As System.Windows.Forms.Label
    Friend WithEvents LblHistory As System.Windows.Forms.Label
    Friend WithEvents LblComments As System.Windows.Forms.Label
    Friend WithEvents TBDescription As System.Windows.Forms.TextBox
    Friend WithEvents TBFilename As System.Windows.Forms.TextBox
    Friend WithEvents TBRealName As System.Windows.Forms.TextBox
    Friend WithEvents TBUserName As System.Windows.Forms.TextBox
    Friend WithEvents CBPartType As System.Windows.Forms.ComboBox
    Friend WithEvents CBLicense As System.Windows.Forms.ComboBox
    Friend WithEvents CBBFC As System.Windows.Forms.ComboBox
    Friend WithEvents RTBHelp As System.Windows.Forms.RichTextBox
    Friend WithEvents RTBKeywords As System.Windows.Forms.RichTextBox
    Friend WithEvents RTBHistory As System.Windows.Forms.RichTextBox
    Friend WithEvents TBCategory As System.Windows.Forms.TextBox
    Friend WithEvents BtnDefault As System.Windows.Forms.Button
    Friend WithEvents RTBComments As System.Windows.Forms.RichTextBox
    Friend WithEvents LblHelp As System.Windows.Forms.Label
    Public WithEvents CBInclude As System.Windows.Forms.CheckBox
    Friend WithEvents GBPreview As System.Windows.Forms.GroupBox
    Friend WithEvents PPreview As System.Windows.Forms.Panel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents CBPreview As System.Windows.Forms.CheckBox
    Friend WithEvents GBDatExport As System.Windows.Forms.GroupBox
    Friend WithEvents CBUnificator As System.Windows.Forms.CheckBox
    Friend WithEvents CBUnificatorLPC As System.Windows.Forms.CheckBox
    Friend WithEvents CBProjector As System.Windows.Forms.CheckBox
    Friend WithEvents CBColourReplacer As System.Windows.Forms.CheckBox
    Friend WithEvents CBRectifier As System.Windows.Forms.CheckBox
    Friend WithEvents CBSlicerPro As CheckBox
End Class
