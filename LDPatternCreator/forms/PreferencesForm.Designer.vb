<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PreferencesForm
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
        Me.NUDGrid = New System.Windows.Forms.NumericUpDown()
        Me.LblGridSize = New System.Windows.Forms.Label()
        Me.LblMoveSnap = New System.Windows.Forms.Label()
        Me.NUDScaleSnap = New System.Windows.Forms.NumericUpDown()
        Me.NUDMoveSnap = New System.Windows.Forms.NumericUpDown()
        Me.LblScaleSnap = New System.Windows.Forms.Label()
        Me.LblRotateSnap = New System.Windows.Forms.Label()
        Me.NUDRotateSnap = New System.Windows.Forms.NumericUpDown()
        CType(Me.NUDGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDScaleSnap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDMoveSnap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDRotateSnap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NUDGrid
        '
        Me.NUDGrid.DecimalPlaces = 1
        Me.NUDGrid.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NUDGrid.Location = New System.Drawing.Point(15, 143)
        Me.NUDGrid.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NUDGrid.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NUDGrid.Name = "NUDGrid"
        Me.NUDGrid.Size = New System.Drawing.Size(198, 20)
        Me.NUDGrid.TabIndex = 22
        Me.NUDGrid.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'LblGridSize
        '
        Me.LblGridSize.AutoSize = True
        Me.LblGridSize.Location = New System.Drawing.Point(12, 126)
        Me.LblGridSize.Name = "LblGridSize"
        Me.LblGridSize.Size = New System.Drawing.Size(121, 13)
        Me.LblGridSize.TabIndex = 20
        Me.LblGridSize.Text = "Grid Size (1/1000 LDU):"
        '
        'LblMoveSnap
        '
        Me.LblMoveSnap.AutoSize = True
        Me.LblMoveSnap.Location = New System.Drawing.Point(12, 9)
        Me.LblMoveSnap.Name = "LblMoveSnap"
        Me.LblMoveSnap.Size = New System.Drawing.Size(134, 13)
        Me.LblMoveSnap.TabIndex = 15
        Me.LblMoveSnap.Text = "Move Snap (1/1000 LDU):"
        '
        'NUDScaleSnap
        '
        Me.NUDScaleSnap.DecimalPlaces = 4
        Me.NUDScaleSnap.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.NUDScaleSnap.Location = New System.Drawing.Point(15, 103)
        Me.NUDScaleSnap.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NUDScaleSnap.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.NUDScaleSnap.Name = "NUDScaleSnap"
        Me.NUDScaleSnap.Size = New System.Drawing.Size(198, 20)
        Me.NUDScaleSnap.TabIndex = 21
        Me.NUDScaleSnap.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'NUDMoveSnap
        '
        Me.NUDMoveSnap.Location = New System.Drawing.Point(15, 25)
        Me.NUDMoveSnap.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.NUDMoveSnap.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NUDMoveSnap.Name = "NUDMoveSnap"
        Me.NUDMoveSnap.Size = New System.Drawing.Size(198, 20)
        Me.NUDMoveSnap.TabIndex = 18
        Me.NUDMoveSnap.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'LblScaleSnap
        '
        Me.LblScaleSnap.AutoSize = True
        Me.LblScaleSnap.Location = New System.Drawing.Point(12, 87)
        Me.LblScaleSnap.Name = "LblScaleSnap"
        Me.LblScaleSnap.Size = New System.Drawing.Size(65, 13)
        Me.LblScaleSnap.TabIndex = 17
        Me.LblScaleSnap.Text = "Scale Snap:"
        '
        'LblRotateSnap
        '
        Me.LblRotateSnap.AutoSize = True
        Me.LblRotateSnap.Location = New System.Drawing.Point(12, 48)
        Me.LblRotateSnap.Name = "LblRotateSnap"
        Me.LblRotateSnap.Size = New System.Drawing.Size(114, 13)
        Me.LblRotateSnap.TabIndex = 16
        Me.LblRotateSnap.Text = "Rotate Snap (Degree):"
        '
        'NUDRotateSnap
        '
        Me.NUDRotateSnap.DecimalPlaces = 4
        Me.NUDRotateSnap.Location = New System.Drawing.Point(15, 64)
        Me.NUDRotateSnap.Maximum = New Decimal(New Integer() {3599999, 0, 0, 262144})
        Me.NUDRotateSnap.Minimum = New Decimal(New Integer() {1, 0, 0, 262144})
        Me.NUDRotateSnap.Name = "NUDRotateSnap"
        Me.NUDRotateSnap.Size = New System.Drawing.Size(198, 20)
        Me.NUDRotateSnap.TabIndex = 19
        Me.NUDRotateSnap.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'PreferencesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(227, 175)
        Me.Controls.Add(Me.NUDGrid)
        Me.Controls.Add(Me.LblGridSize)
        Me.Controls.Add(Me.LblMoveSnap)
        Me.Controls.Add(Me.NUDScaleSnap)
        Me.Controls.Add(Me.NUDMoveSnap)
        Me.Controls.Add(Me.LblScaleSnap)
        Me.Controls.Add(Me.LblRotateSnap)
        Me.Controls.Add(Me.NUDRotateSnap)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "PreferencesForm"
        Me.Text = "View-Preferences [F6]: "
        CType(Me.NUDGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDScaleSnap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDMoveSnap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDRotateSnap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NUDGrid As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblGridSize As System.Windows.Forms.Label
    Friend WithEvents LblMoveSnap As System.Windows.Forms.Label
    Friend WithEvents NUDScaleSnap As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUDMoveSnap As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblScaleSnap As System.Windows.Forms.Label
    Friend WithEvents LblRotateSnap As System.Windows.Forms.Label
    Friend WithEvents NUDRotateSnap As System.Windows.Forms.NumericUpDown
End Class
