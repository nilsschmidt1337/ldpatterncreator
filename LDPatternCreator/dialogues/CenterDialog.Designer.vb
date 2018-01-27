<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CenterDialog
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.GBVertex = New System.Windows.Forms.GroupBox
        Me.LblUnit2 = New System.Windows.Forms.Label
        Me.LblUnit1 = New System.Windows.Forms.Label
        Me.NUDVertY = New System.Windows.Forms.NumericUpDown
        Me.NUDVertX = New System.Windows.Forms.NumericUpDown
        Me.LblVertexY = New System.Windows.Forms.Label
        Me.LblVertexX = New System.Windows.Forms.Label
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GBVertex.SuspendLayout()
        CType(Me.NUDVertY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDVertX, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(69, 101)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(189, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(7, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(80, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(101, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(80, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'GBVertex
        '
        Me.GBVertex.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBVertex.BackColor = System.Drawing.SystemColors.Control
        Me.GBVertex.Controls.Add(Me.LblUnit2)
        Me.GBVertex.Controls.Add(Me.LblUnit1)
        Me.GBVertex.Controls.Add(Me.NUDVertY)
        Me.GBVertex.Controls.Add(Me.NUDVertX)
        Me.GBVertex.Controls.Add(Me.LblVertexY)
        Me.GBVertex.Controls.Add(Me.LblVertexX)
        Me.GBVertex.Location = New System.Drawing.Point(12, 14)
        Me.GBVertex.Name = "GBVertex"
        Me.GBVertex.Size = New System.Drawing.Size(246, 81)
        Me.GBVertex.TabIndex = 19
        Me.GBVertex.TabStop = False
        '
        'LblUnit2
        '
        Me.LblUnit2.AutoSize = True
        Me.LblUnit2.Location = New System.Drawing.Point(193, 51)
        Me.LblUnit2.Name = "LblUnit2"
        Me.LblUnit2.Size = New System.Drawing.Size(29, 13)
        Me.LblUnit2.TabIndex = 5
        Me.LblUnit2.Text = "LDU"
        '
        'LblUnit1
        '
        Me.LblUnit1.AutoSize = True
        Me.LblUnit1.Location = New System.Drawing.Point(193, 29)
        Me.LblUnit1.Name = "LblUnit1"
        Me.LblUnit1.Size = New System.Drawing.Size(29, 13)
        Me.LblUnit1.TabIndex = 4
        Me.LblUnit1.Text = "LDU"
        '
        'NUDVertY
        '
        Me.NUDVertY.DecimalPlaces = 4
        Me.NUDVertY.Location = New System.Drawing.Point(33, 49)
        Me.NUDVertY.Maximum = New Decimal(New Integer() {1241513984, 370409800, 542101, 0})
        Me.NUDVertY.Minimum = New Decimal(New Integer() {1241513984, 370409800, 542101, -2147483648})
        Me.NUDVertY.Name = "NUDVertY"
        Me.NUDVertY.Size = New System.Drawing.Size(154, 20)
        Me.NUDVertY.TabIndex = 3
        '
        'NUDVertX
        '
        Me.NUDVertX.DecimalPlaces = 4
        Me.NUDVertX.Location = New System.Drawing.Point(33, 23)
        Me.NUDVertX.Maximum = New Decimal(New Integer() {1241513984, 370409800, 542101, 0})
        Me.NUDVertX.Minimum = New Decimal(New Integer() {1241513984, 370409800, 542101, -2147483648})
        Me.NUDVertX.Name = "NUDVertX"
        Me.NUDVertX.Size = New System.Drawing.Size(154, 20)
        Me.NUDVertX.TabIndex = 2
        '
        'LblVertexY
        '
        Me.LblVertexY.AutoSize = True
        Me.LblVertexY.Location = New System.Drawing.Point(10, 51)
        Me.LblVertexY.Name = "LblVertexY"
        Me.LblVertexY.Size = New System.Drawing.Size(17, 13)
        Me.LblVertexY.TabIndex = 1
        Me.LblVertexY.Text = "Y:"
        '
        'LblVertexX
        '
        Me.LblVertexX.AutoSize = True
        Me.LblVertexX.Location = New System.Drawing.Point(10, 25)
        Me.LblVertexX.Name = "LblVertexX"
        Me.LblVertexX.Size = New System.Drawing.Size(17, 13)
        Me.LblVertexX.TabIndex = 0
        Me.LblVertexX.Text = "X:"
        '
        'CenterDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(270, 139)
        Me.Controls.Add(Me.GBVertex)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CenterDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Set the center of the group:"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GBVertex.ResumeLayout(False)
        Me.GBVertex.PerformLayout()
        CType(Me.NUDVertY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDVertX, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GBVertex As System.Windows.Forms.GroupBox
    Friend WithEvents LblUnit2 As System.Windows.Forms.Label
    Friend WithEvents LblUnit1 As System.Windows.Forms.Label
    Friend WithEvents NUDVertY As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUDVertX As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblVertexY As System.Windows.Forms.Label
    Friend WithEvents LblVertexX As System.Windows.Forms.Label

End Class
