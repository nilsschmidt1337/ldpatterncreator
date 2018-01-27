<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StickerDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.GBDescription = New System.Windows.Forms.GroupBox()
        Me.TBDescription = New System.Windows.Forms.TextBox()
        Me.LblAutoDescription = New System.Windows.Forms.Label()
        Me.NUDWidth = New System.Windows.Forms.NumericUpDown()
        Me.NUDHeight = New System.Windows.Forms.NumericUpDown()
        Me.GBSize = New System.Windows.Forms.GroupBox()
        Me.LblUnit2 = New System.Windows.Forms.Label()
        Me.LblUnit1 = New System.Windows.Forms.Label()
        Me.LblHeight = New System.Windows.Forms.Label()
        Me.LblWidth = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GBDescription.SuspendLayout()
        CType(Me.NUDWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GBSize.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(277, 192)
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
        Me.OK_Button.Text = "OK"
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
        'GBDescription
        '
        Me.GBDescription.Controls.Add(Me.TBDescription)
        Me.GBDescription.Controls.Add(Me.LblAutoDescription)
        Me.GBDescription.Location = New System.Drawing.Point(12, 12)
        Me.GBDescription.Name = "GBDescription"
        Me.GBDescription.Size = New System.Drawing.Size(411, 81)
        Me.GBDescription.TabIndex = 1
        Me.GBDescription.TabStop = False
        Me.GBDescription.Text = "Description:"
        '
        'TBDescription
        '
        Me.TBDescription.Location = New System.Drawing.Point(9, 43)
        Me.TBDescription.Name = "TBDescription"
        Me.TBDescription.Size = New System.Drawing.Size(396, 20)
        Me.TBDescription.TabIndex = 1
        '
        'LblAutoDescription
        '
        Me.LblAutoDescription.AutoSize = True
        Me.LblAutoDescription.Location = New System.Drawing.Point(6, 27)
        Me.LblAutoDescription.Name = "LblAutoDescription"
        Me.LblAutoDescription.Size = New System.Drawing.Size(106, 13)
        Me.LblAutoDescription.TabIndex = 0
        Me.LblAutoDescription.Text = "Sticker  1.1 x  8 with "
        '
        'NUDWidth
        '
        Me.NUDWidth.DecimalPlaces = 3
        Me.NUDWidth.Location = New System.Drawing.Point(130, 19)
        Me.NUDWidth.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.NUDWidth.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.NUDWidth.Name = "NUDWidth"
        Me.NUDWidth.Size = New System.Drawing.Size(129, 20)
        Me.NUDWidth.TabIndex = 2
        Me.NUDWidth.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'NUDHeight
        '
        Me.NUDHeight.DecimalPlaces = 3
        Me.NUDHeight.Location = New System.Drawing.Point(130, 45)
        Me.NUDHeight.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.NUDHeight.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.NUDHeight.Name = "NUDHeight"
        Me.NUDHeight.Size = New System.Drawing.Size(129, 20)
        Me.NUDHeight.TabIndex = 3
        Me.NUDHeight.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'GBSize
        '
        Me.GBSize.Controls.Add(Me.LblUnit2)
        Me.GBSize.Controls.Add(Me.NUDHeight)
        Me.GBSize.Controls.Add(Me.LblUnit1)
        Me.GBSize.Controls.Add(Me.NUDWidth)
        Me.GBSize.Controls.Add(Me.LblHeight)
        Me.GBSize.Controls.Add(Me.LblWidth)
        Me.GBSize.Location = New System.Drawing.Point(12, 99)
        Me.GBSize.Name = "GBSize"
        Me.GBSize.Size = New System.Drawing.Size(411, 82)
        Me.GBSize.TabIndex = 4
        Me.GBSize.TabStop = False
        Me.GBSize.Text = "Size:"
        '
        'LblUnit2
        '
        Me.LblUnit2.AutoSize = True
        Me.LblUnit2.Location = New System.Drawing.Point(265, 47)
        Me.LblUnit2.Name = "LblUnit2"
        Me.LblUnit2.Size = New System.Drawing.Size(29, 13)
        Me.LblUnit2.TabIndex = 3
        Me.LblUnit2.Text = "LDU"
        '
        'LblUnit1
        '
        Me.LblUnit1.AutoSize = True
        Me.LblUnit1.Location = New System.Drawing.Point(265, 21)
        Me.LblUnit1.Name = "LblUnit1"
        Me.LblUnit1.Size = New System.Drawing.Size(29, 13)
        Me.LblUnit1.TabIndex = 2
        Me.LblUnit1.Text = "LDU"
        '
        'LblHeight
        '
        Me.LblHeight.AutoSize = True
        Me.LblHeight.Location = New System.Drawing.Point(49, 47)
        Me.LblHeight.Name = "LblHeight"
        Me.LblHeight.Size = New System.Drawing.Size(41, 13)
        Me.LblHeight.TabIndex = 1
        Me.LblHeight.Text = "Height:"
        '
        'LblWidth
        '
        Me.LblWidth.AutoSize = True
        Me.LblWidth.Location = New System.Drawing.Point(49, 21)
        Me.LblWidth.Name = "LblWidth"
        Me.LblWidth.Size = New System.Drawing.Size(38, 13)
        Me.LblWidth.TabIndex = 0
        Me.LblWidth.Text = "Width:"
        '
        'StickerDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(435, 233)
        Me.Controls.Add(Me.GBSize)
        Me.Controls.Add(Me.GBDescription)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StickerDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create a Sticker: "
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GBDescription.ResumeLayout(False)
        Me.GBDescription.PerformLayout()
        CType(Me.NUDWidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDHeight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GBSize.ResumeLayout(False)
        Me.GBSize.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GBDescription As System.Windows.Forms.GroupBox
    Friend WithEvents TBDescription As System.Windows.Forms.TextBox
    Friend WithEvents LblAutoDescription As System.Windows.Forms.Label
    Friend WithEvents NUDWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUDHeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents GBSize As System.Windows.Forms.GroupBox
    Friend WithEvents LblUnit2 As System.Windows.Forms.Label
    Friend WithEvents LblUnit1 As System.Windows.Forms.Label
    Friend WithEvents LblHeight As System.Windows.Forms.Label
    Friend WithEvents LblWidth As System.Windows.Forms.Label

End Class
