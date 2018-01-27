<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ColourForm
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
        Me.Colours = New System.Windows.Forms.FlowLayoutPanel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.LblTip = New System.Windows.Forms.Label()
        Me.LblR = New System.Windows.Forms.Label()
        Me.GBDirectColour = New System.Windows.Forms.GroupBox()
        Me.BtnDirectColour = New System.Windows.Forms.Button()
        Me.NB = New System.Windows.Forms.NumericUpDown()
        Me.NG = New System.Windows.Forms.NumericUpDown()
        Me.NR = New System.Windows.Forms.NumericUpDown()
        Me.LblB = New System.Windows.Forms.Label()
        Me.LblG = New System.Windows.Forms.Label()
        Me.BtnExtend = New System.Windows.Forms.Button()
        Me.GBDirectColour.SuspendLayout()
        CType(Me.NB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Colours
        '
        Me.Colours.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Colours.AutoScroll = True
        Me.Colours.Location = New System.Drawing.Point(21, 38)
        Me.Colours.Name = "Colours"
        Me.Colours.Size = New System.Drawing.Size(267, 241)
        Me.Colours.TabIndex = 0
        '
        'LblTip
        '
        Me.LblTip.AutoSize = True
        Me.LblTip.Location = New System.Drawing.Point(18, 9)
        Me.LblTip.Name = "LblTip"
        Me.LblTip.Size = New System.Drawing.Size(266, 26)
        Me.LblTip.TabIndex = 1
        Me.LblTip.Text = "Tip: Use the numpad to type in colour numbers directly." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "E.g. ""015"" for white. Pr" & _
    "ess ESC to cancel your input."
        '
        'LblR
        '
        Me.LblR.AutoSize = True
        Me.LblR.Location = New System.Drawing.Point(6, 37)
        Me.LblR.Name = "LblR"
        Me.LblR.Size = New System.Drawing.Size(15, 13)
        Me.LblR.TabIndex = 2
        Me.LblR.Text = "R"
        '
        'GBDirectColour
        '
        Me.GBDirectColour.Controls.Add(Me.BtnDirectColour)
        Me.GBDirectColour.Controls.Add(Me.NB)
        Me.GBDirectColour.Controls.Add(Me.NG)
        Me.GBDirectColour.Controls.Add(Me.NR)
        Me.GBDirectColour.Controls.Add(Me.LblB)
        Me.GBDirectColour.Controls.Add(Me.LblG)
        Me.GBDirectColour.Controls.Add(Me.LblR)
        Me.GBDirectColour.Location = New System.Drawing.Point(294, 71)
        Me.GBDirectColour.Name = "GBDirectColour"
        Me.GBDirectColour.Size = New System.Drawing.Size(112, 136)
        Me.GBDirectColour.TabIndex = 3
        Me.GBDirectColour.TabStop = False
        Me.GBDirectColour.Text = "Direct Colour (0x2RRGGBB):"
        '
        'BtnDirectColour
        '
        Me.BtnDirectColour.Location = New System.Drawing.Point(9, 107)
        Me.BtnDirectColour.Name = "BtnDirectColour"
        Me.BtnDirectColour.Size = New System.Drawing.Size(97, 23)
        Me.BtnDirectColour.TabIndex = 8
        Me.BtnDirectColour.Text = "Set"
        Me.BtnDirectColour.UseVisualStyleBackColor = True
        '
        'NB
        '
        Me.NB.Location = New System.Drawing.Point(27, 84)
        Me.NB.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.NB.Name = "NB"
        Me.NB.Size = New System.Drawing.Size(48, 20)
        Me.NB.TabIndex = 7
        '
        'NG
        '
        Me.NG.Location = New System.Drawing.Point(27, 59)
        Me.NG.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.NG.Name = "NG"
        Me.NG.Size = New System.Drawing.Size(48, 20)
        Me.NG.TabIndex = 6
        '
        'NR
        '
        Me.NR.Location = New System.Drawing.Point(27, 35)
        Me.NR.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.NR.Name = "NR"
        Me.NR.Size = New System.Drawing.Size(48, 20)
        Me.NR.TabIndex = 5
        '
        'LblB
        '
        Me.LblB.AutoSize = True
        Me.LblB.Location = New System.Drawing.Point(6, 86)
        Me.LblB.Name = "LblB"
        Me.LblB.Size = New System.Drawing.Size(14, 13)
        Me.LblB.TabIndex = 4
        Me.LblB.Text = "B"
        '
        'LblG
        '
        Me.LblG.AutoSize = True
        Me.LblG.Location = New System.Drawing.Point(6, 61)
        Me.LblG.Name = "LblG"
        Me.LblG.Size = New System.Drawing.Size(15, 13)
        Me.LblG.TabIndex = 3
        Me.LblG.Text = "G"
        '
        'BtnExtend
        '
        Me.BtnExtend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExtend.BackgroundImage = Global.LDPatternCreator.My.Resources.Resources.arrow
        Me.BtnExtend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnExtend.Location = New System.Drawing.Point(294, 249)
        Me.BtnExtend.Name = "BtnExtend"
        Me.BtnExtend.Size = New System.Drawing.Size(30, 30)
        Me.BtnExtend.TabIndex = 4
        Me.BtnExtend.UseVisualStyleBackColor = True
        '
        'ColourForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(418, 291)
        Me.Controls.Add(Me.BtnExtend)
        Me.Controls.Add(Me.GBDirectColour)
        Me.Controls.Add(Me.LblTip)
        Me.Controls.Add(Me.Colours)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "ColourForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Colour-Palette "
        Me.GBDirectColour.ResumeLayout(False)
        Me.GBDirectColour.PerformLayout()
        CType(Me.NB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents Colours As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents LblTip As System.Windows.Forms.Label
    Friend WithEvents LblR As System.Windows.Forms.Label
    Friend WithEvents GBDirectColour As System.Windows.Forms.GroupBox
    Friend WithEvents LblB As System.Windows.Forms.Label
    Friend WithEvents LblG As System.Windows.Forms.Label
    Friend WithEvents BtnDirectColour As System.Windows.Forms.Button
    Friend WithEvents NB As System.Windows.Forms.NumericUpDown
    Friend WithEvents NG As System.Windows.Forms.NumericUpDown
    Friend WithEvents NR As System.Windows.Forms.NumericUpDown
    Friend WithEvents BtnExtend As System.Windows.Forms.Button
End Class
