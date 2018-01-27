<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TemplateEditor
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.BtnExample = New System.Windows.Forms.Button()
        Me.LblTitle = New System.Windows.Forms.Label()
        Me.TBTitle = New System.Windows.Forms.TextBox()
        Me.GBData = New System.Windows.Forms.GroupBox()
        Me.RTBData = New System.Windows.Forms.RichTextBox()
        Me.LblAdditionalLines = New System.Windows.Forms.Label()
        Me.RTBPoly = New System.Windows.Forms.RichTextBox()
        Me.CBProjectMode = New System.Windows.Forms.ComboBox()
        Me.LblProjectOn = New System.Windows.Forms.Label()
        Me.LblPolyData = New System.Windows.Forms.Label()
        Me.M22 = New System.Windows.Forms.TextBox()
        Me.M21 = New System.Windows.Forms.TextBox()
        Me.M20 = New System.Windows.Forms.TextBox()
        Me.M12 = New System.Windows.Forms.TextBox()
        Me.M11 = New System.Windows.Forms.TextBox()
        Me.M10 = New System.Windows.Forms.TextBox()
        Me.M02 = New System.Windows.Forms.TextBox()
        Me.M01 = New System.Windows.Forms.TextBox()
        Me.M00 = New System.Windows.Forms.TextBox()
        Me.LblTransMatrix = New System.Windows.Forms.Label()
        Me.M23 = New System.Windows.Forms.TextBox()
        Me.M13 = New System.Windows.Forms.TextBox()
        Me.LblOffset = New System.Windows.Forms.Label()
        Me.M03 = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GBData.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(630, 527)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Save"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'BtnExample
        '
        Me.BtnExample.Location = New System.Drawing.Point(17, 533)
        Me.BtnExample.Name = "BtnExample"
        Me.BtnExample.Size = New System.Drawing.Size(104, 23)
        Me.BtnExample.TabIndex = 1
        Me.BtnExample.Text = "Show Example"
        Me.BtnExample.UseVisualStyleBackColor = True
        '
        'LblTitle
        '
        Me.LblTitle.AutoSize = True
        Me.LblTitle.Location = New System.Drawing.Point(14, 16)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(30, 13)
        Me.LblTitle.TabIndex = 2
        Me.LblTitle.Text = "Title:"
        '
        'TBTitle
        '
        Me.TBTitle.Location = New System.Drawing.Point(59, 13)
        Me.TBTitle.Name = "TBTitle"
        Me.TBTitle.Size = New System.Drawing.Size(287, 20)
        Me.TBTitle.TabIndex = 3
        '
        'GBData
        '
        Me.GBData.Controls.Add(Me.RTBData)
        Me.GBData.Controls.Add(Me.LblAdditionalLines)
        Me.GBData.Controls.Add(Me.RTBPoly)
        Me.GBData.Controls.Add(Me.CBProjectMode)
        Me.GBData.Controls.Add(Me.LblProjectOn)
        Me.GBData.Controls.Add(Me.LblPolyData)
        Me.GBData.Controls.Add(Me.M22)
        Me.GBData.Controls.Add(Me.M21)
        Me.GBData.Controls.Add(Me.M20)
        Me.GBData.Controls.Add(Me.M12)
        Me.GBData.Controls.Add(Me.M11)
        Me.GBData.Controls.Add(Me.M10)
        Me.GBData.Controls.Add(Me.M02)
        Me.GBData.Controls.Add(Me.M01)
        Me.GBData.Controls.Add(Me.M00)
        Me.GBData.Controls.Add(Me.LblTransMatrix)
        Me.GBData.Controls.Add(Me.M23)
        Me.GBData.Controls.Add(Me.M13)
        Me.GBData.Controls.Add(Me.LblOffset)
        Me.GBData.Controls.Add(Me.M03)
        Me.GBData.Location = New System.Drawing.Point(17, 39)
        Me.GBData.Name = "GBData"
        Me.GBData.Size = New System.Drawing.Size(765, 485)
        Me.GBData.TabIndex = 4
        Me.GBData.TabStop = False
        Me.GBData.Text = "Data:"
        '
        'RTBData
        '
        Me.RTBData.Location = New System.Drawing.Point(6, 204)
        Me.RTBData.Name = "RTBData"
        Me.RTBData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.RTBData.Size = New System.Drawing.Size(753, 275)
        Me.RTBData.TabIndex = 17
        Me.RTBData.Text = ""
        '
        'LblAdditionalLines
        '
        Me.LblAdditionalLines.AutoSize = True
        Me.LblAdditionalLines.Location = New System.Drawing.Point(6, 186)
        Me.LblAdditionalLines.Name = "LblAdditionalLines"
        Me.LblAdditionalLines.Size = New System.Drawing.Size(84, 13)
        Me.LblAdditionalLines.TabIndex = 16
        Me.LblAdditionalLines.Text = "Additional Lines:"
        '
        'RTBPoly
        '
        Me.RTBPoly.Location = New System.Drawing.Point(334, 32)
        Me.RTBPoly.Name = "RTBPoly"
        Me.RTBPoly.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.RTBPoly.Size = New System.Drawing.Size(425, 166)
        Me.RTBPoly.TabIndex = 15
        Me.RTBPoly.Text = ""
        '
        'CBProjectMode
        '
        Me.CBProjectMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBProjectMode.FormattingEnabled = True
        Me.CBProjectMode.Items.AddRange(New Object() {"0 = Right", "1 = Top", "2 = Front", "3 = Left", "4 = Bottom", "5 = Back"})
        Me.CBProjectMode.Location = New System.Drawing.Point(6, 32)
        Me.CBProjectMode.Name = "CBProjectMode"
        Me.CBProjectMode.Size = New System.Drawing.Size(313, 21)
        Me.CBProjectMode.TabIndex = 0
        '
        'LblProjectOn
        '
        Me.LblProjectOn.AutoSize = True
        Me.LblProjectOn.Location = New System.Drawing.Point(6, 16)
        Me.LblProjectOn.Name = "LblProjectOn"
        Me.LblProjectOn.Size = New System.Drawing.Size(61, 13)
        Me.LblProjectOn.TabIndex = 15
        Me.LblProjectOn.Text = "Project on.."
        '
        'LblPolyData
        '
        Me.LblPolyData.AutoSize = True
        Me.LblPolyData.Location = New System.Drawing.Point(331, 16)
        Me.LblPolyData.Name = "LblPolyData"
        Me.LblPolyData.Size = New System.Drawing.Size(74, 13)
        Me.LblPolyData.TabIndex = 14
        Me.LblPolyData.Text = "Polygon-Data:"
        '
        'M22
        '
        Me.M22.Location = New System.Drawing.Point(218, 163)
        Me.M22.Name = "M22"
        Me.M22.Size = New System.Drawing.Size(101, 20)
        Me.M22.TabIndex = 13
        '
        'M21
        '
        Me.M21.Location = New System.Drawing.Point(112, 163)
        Me.M21.Name = "M21"
        Me.M21.Size = New System.Drawing.Size(100, 20)
        Me.M21.TabIndex = 12
        '
        'M20
        '
        Me.M20.Location = New System.Drawing.Point(6, 163)
        Me.M20.Name = "M20"
        Me.M20.Size = New System.Drawing.Size(100, 20)
        Me.M20.TabIndex = 11
        '
        'M12
        '
        Me.M12.Location = New System.Drawing.Point(218, 137)
        Me.M12.Name = "M12"
        Me.M12.Size = New System.Drawing.Size(101, 20)
        Me.M12.TabIndex = 10
        '
        'M11
        '
        Me.M11.Location = New System.Drawing.Point(112, 137)
        Me.M11.Name = "M11"
        Me.M11.Size = New System.Drawing.Size(100, 20)
        Me.M11.TabIndex = 9
        '
        'M10
        '
        Me.M10.Location = New System.Drawing.Point(6, 137)
        Me.M10.Name = "M10"
        Me.M10.Size = New System.Drawing.Size(100, 20)
        Me.M10.TabIndex = 8
        '
        'M02
        '
        Me.M02.Location = New System.Drawing.Point(219, 111)
        Me.M02.Name = "M02"
        Me.M02.Size = New System.Drawing.Size(100, 20)
        Me.M02.TabIndex = 7
        '
        'M01
        '
        Me.M01.Location = New System.Drawing.Point(112, 111)
        Me.M01.Name = "M01"
        Me.M01.Size = New System.Drawing.Size(100, 20)
        Me.M01.TabIndex = 6
        '
        'M00
        '
        Me.M00.Location = New System.Drawing.Point(6, 111)
        Me.M00.Name = "M00"
        Me.M00.Size = New System.Drawing.Size(100, 20)
        Me.M00.TabIndex = 5
        '
        'LblTransMatrix
        '
        Me.LblTransMatrix.AutoSize = True
        Me.LblTransMatrix.Location = New System.Drawing.Point(6, 95)
        Me.LblTransMatrix.Name = "LblTransMatrix"
        Me.LblTransMatrix.Size = New System.Drawing.Size(111, 13)
        Me.LblTransMatrix.TabIndex = 4
        Me.LblTransMatrix.Text = "Transformation-Matrix:"
        '
        'M23
        '
        Me.M23.Location = New System.Drawing.Point(219, 72)
        Me.M23.Name = "M23"
        Me.M23.Size = New System.Drawing.Size(100, 20)
        Me.M23.TabIndex = 3
        '
        'M13
        '
        Me.M13.Location = New System.Drawing.Point(112, 72)
        Me.M13.Name = "M13"
        Me.M13.Size = New System.Drawing.Size(100, 20)
        Me.M13.TabIndex = 2
        '
        'LblOffset
        '
        Me.LblOffset.AutoSize = True
        Me.LblOffset.Location = New System.Drawing.Point(6, 56)
        Me.LblOffset.Name = "LblOffset"
        Me.LblOffset.Size = New System.Drawing.Size(38, 13)
        Me.LblOffset.TabIndex = 1
        Me.LblOffset.Text = "Offset:"
        '
        'M03
        '
        Me.M03.Location = New System.Drawing.Point(6, 72)
        Me.M03.Name = "M03"
        Me.M03.Size = New System.Drawing.Size(100, 20)
        Me.M03.TabIndex = 1
        '
        'TemplateEditor
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(794, 568)
        Me.Controls.Add(Me.GBData)
        Me.Controls.Add(Me.TBTitle)
        Me.Controls.Add(Me.LblTitle)
        Me.Controls.Add(Me.BtnExample)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TemplateEditor"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Template-Editor: "
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GBData.ResumeLayout(False)
        Me.GBData.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents BtnExample As System.Windows.Forms.Button
    Friend WithEvents LblTitle As System.Windows.Forms.Label
    Friend WithEvents TBTitle As System.Windows.Forms.TextBox
    Friend WithEvents GBData As System.Windows.Forms.GroupBox
    Friend WithEvents RTBPoly As System.Windows.Forms.RichTextBox
    Friend WithEvents CBProjectMode As System.Windows.Forms.ComboBox
    Friend WithEvents LblProjectOn As System.Windows.Forms.Label
    Friend WithEvents LblPolyData As System.Windows.Forms.Label
    Friend WithEvents M22 As System.Windows.Forms.TextBox
    Friend WithEvents M21 As System.Windows.Forms.TextBox
    Friend WithEvents M20 As System.Windows.Forms.TextBox
    Friend WithEvents M12 As System.Windows.Forms.TextBox
    Friend WithEvents M11 As System.Windows.Forms.TextBox
    Friend WithEvents M10 As System.Windows.Forms.TextBox
    Friend WithEvents M02 As System.Windows.Forms.TextBox
    Friend WithEvents M01 As System.Windows.Forms.TextBox
    Friend WithEvents M00 As System.Windows.Forms.TextBox
    Friend WithEvents LblTransMatrix As System.Windows.Forms.Label
    Friend WithEvents M23 As System.Windows.Forms.TextBox
    Friend WithEvents M13 As System.Windows.Forms.TextBox
    Friend WithEvents LblOffset As System.Windows.Forms.Label
    Friend WithEvents M03 As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents RTBData As System.Windows.Forms.RichTextBox
    Friend WithEvents LblAdditionalLines As System.Windows.Forms.Label

End Class
