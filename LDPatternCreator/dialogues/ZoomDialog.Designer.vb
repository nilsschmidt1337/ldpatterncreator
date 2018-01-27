<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ZoomDialog
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
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.GBZoom = New System.Windows.Forms.GroupBox()
        Me.LblPercent = New System.Windows.Forms.Label()
        Me.NUDZoom = New System.Windows.Forms.NumericUpDown()
        Me.LblZoom = New System.Windows.Forms.Label()
        Me.GBViewport = New System.Windows.Forms.GroupBox()
        Me.NUDLRy = New System.Windows.Forms.NumericUpDown()
        Me.NUDLRx = New System.Windows.Forms.NumericUpDown()
        Me.NUDULy = New System.Windows.Forms.NumericUpDown()
        Me.NUDULx = New System.Windows.Forms.NumericUpDown()
        Me.LblLowerRight = New System.Windows.Forms.Label()
        Me.LblUpperLeft = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GBZoom.SuspendLayout()
        CType(Me.NUDZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GBViewport.SuspendLayout()
        CType(Me.NUDLRy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDLRx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDULy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDULx, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(213, 155)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(183, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(5, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(80, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(97, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(80, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'GBZoom
        '
        Me.GBZoom.Controls.Add(Me.LblPercent)
        Me.GBZoom.Controls.Add(Me.NUDZoom)
        Me.GBZoom.Controls.Add(Me.LblZoom)
        Me.GBZoom.Location = New System.Drawing.Point(13, 13)
        Me.GBZoom.Name = "GBZoom"
        Me.GBZoom.Size = New System.Drawing.Size(380, 49)
        Me.GBZoom.TabIndex = 1
        Me.GBZoom.TabStop = False
        Me.GBZoom.Text = "Zoom:"
        '
        'LblPercent
        '
        Me.LblPercent.AutoSize = True
        Me.LblPercent.Location = New System.Drawing.Point(276, 20)
        Me.LblPercent.Name = "LblPercent"
        Me.LblPercent.Size = New System.Drawing.Size(15, 13)
        Me.LblPercent.TabIndex = 2
        Me.LblPercent.Text = "%"
        '
        'NUDZoom
        '
        Me.NUDZoom.DecimalPlaces = 1
        Me.NUDZoom.Location = New System.Drawing.Point(178, 18)
        Me.NUDZoom.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NUDZoom.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NUDZoom.Name = "NUDZoom"
        Me.NUDZoom.Size = New System.Drawing.Size(92, 20)
        Me.NUDZoom.TabIndex = 1
        Me.NUDZoom.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'LblZoom
        '
        Me.LblZoom.AutoSize = True
        Me.LblZoom.Location = New System.Drawing.Point(10, 20)
        Me.LblZoom.Name = "LblZoom"
        Me.LblZoom.Size = New System.Drawing.Size(37, 13)
        Me.LblZoom.TabIndex = 0
        Me.LblZoom.Text = "Zoom:"
        '
        'GBViewport
        '
        Me.GBViewport.Controls.Add(Me.NUDLRy)
        Me.GBViewport.Controls.Add(Me.NUDLRx)
        Me.GBViewport.Controls.Add(Me.NUDULy)
        Me.GBViewport.Controls.Add(Me.NUDULx)
        Me.GBViewport.Controls.Add(Me.LblLowerRight)
        Me.GBViewport.Controls.Add(Me.LblUpperLeft)
        Me.GBViewport.Location = New System.Drawing.Point(12, 68)
        Me.GBViewport.Name = "GBViewport"
        Me.GBViewport.Size = New System.Drawing.Size(381, 79)
        Me.GBViewport.TabIndex = 2
        Me.GBViewport.TabStop = False
        Me.GBViewport.Text = "Viewport:"
        '
        'NUDLRy
        '
        Me.NUDLRy.DecimalPlaces = 4
        Me.NUDLRy.Location = New System.Drawing.Point(277, 47)
        Me.NUDLRy.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
        Me.NUDLRy.Minimum = New Decimal(New Integer() {100000000, 0, 0, -2147483648})
        Me.NUDLRy.Name = "NUDLRy"
        Me.NUDLRy.Size = New System.Drawing.Size(92, 20)
        Me.NUDLRy.TabIndex = 8
        '
        'NUDLRx
        '
        Me.NUDLRx.DecimalPlaces = 4
        Me.NUDLRx.Location = New System.Drawing.Point(179, 47)
        Me.NUDLRx.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
        Me.NUDLRx.Minimum = New Decimal(New Integer() {100000000, 0, 0, -2147483648})
        Me.NUDLRx.Name = "NUDLRx"
        Me.NUDLRx.Size = New System.Drawing.Size(92, 20)
        Me.NUDLRx.TabIndex = 7
        '
        'NUDULy
        '
        Me.NUDULy.DecimalPlaces = 4
        Me.NUDULy.Location = New System.Drawing.Point(277, 21)
        Me.NUDULy.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
        Me.NUDULy.Minimum = New Decimal(New Integer() {100000000, 0, 0, -2147483648})
        Me.NUDULy.Name = "NUDULy"
        Me.NUDULy.Size = New System.Drawing.Size(92, 20)
        Me.NUDULy.TabIndex = 7
        '
        'NUDULx
        '
        Me.NUDULx.DecimalPlaces = 4
        Me.NUDULx.Location = New System.Drawing.Point(179, 21)
        Me.NUDULx.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
        Me.NUDULx.Minimum = New Decimal(New Integer() {100000000, 0, 0, -2147483648})
        Me.NUDULx.Name = "NUDULx"
        Me.NUDULx.Size = New System.Drawing.Size(92, 20)
        Me.NUDULx.TabIndex = 6
        '
        'LblLowerRight
        '
        Me.LblLowerRight.AutoSize = True
        Me.LblLowerRight.Location = New System.Drawing.Point(11, 49)
        Me.LblLowerRight.Name = "LblLowerRight"
        Me.LblLowerRight.Size = New System.Drawing.Size(101, 13)
        Me.LblLowerRight.TabIndex = 5
        Me.LblLowerRight.Text = "Lower Right Corner:"
        '
        'LblUpperLeft
        '
        Me.LblUpperLeft.AutoSize = True
        Me.LblUpperLeft.Location = New System.Drawing.Point(11, 23)
        Me.LblUpperLeft.Name = "LblUpperLeft"
        Me.LblUpperLeft.Size = New System.Drawing.Size(94, 13)
        Me.LblUpperLeft.TabIndex = 0
        Me.LblUpperLeft.Text = "Upper Left Corner:"
        '
        'ZoomDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(405, 196)
        Me.Controls.Add(Me.GBViewport)
        Me.Controls.Add(Me.GBZoom)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ZoomDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Zoom / Viewport: "
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GBZoom.ResumeLayout(False)
        Me.GBZoom.PerformLayout()
        CType(Me.NUDZoom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GBViewport.ResumeLayout(False)
        Me.GBViewport.PerformLayout()
        CType(Me.NUDLRy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDLRx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDULy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDULx, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GBZoom As System.Windows.Forms.GroupBox
    Friend WithEvents LblPercent As System.Windows.Forms.Label
    Friend WithEvents NUDZoom As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblZoom As System.Windows.Forms.Label
    Friend WithEvents GBViewport As System.Windows.Forms.GroupBox
    Friend WithEvents LblUpperLeft As System.Windows.Forms.Label
    Friend WithEvents LblLowerRight As System.Windows.Forms.Label
    Friend WithEvents NUDULy As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUDULx As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUDLRy As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUDLRx As System.Windows.Forms.NumericUpDown

End Class
