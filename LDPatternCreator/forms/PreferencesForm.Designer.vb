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
        Me.BtnOne = New System.Windows.Forms.Button()
        Me.BtnTenth = New System.Windows.Forms.Button()
        Me.BtnHundredth = New System.Windows.Forms.Button()
        Me.BtnThousandth = New System.Windows.Forms.Button()
        Me.LblOne = New System.Windows.Forms.Label()
        Me.LblTenth = New System.Windows.Forms.Label()
        Me.LblHundredth = New System.Windows.Forms.Label()
        Me.LblThousandth = New System.Windows.Forms.Label()
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
        Me.NUDGrid.Location = New System.Drawing.Point(15, 216)
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
        Me.LblGridSize.Location = New System.Drawing.Point(12, 199)
        Me.LblGridSize.Name = "LblGridSize"
        Me.LblGridSize.Size = New System.Drawing.Size(121, 13)
        Me.LblGridSize.TabIndex = 20
        Me.LblGridSize.Text = "Grid Size (1/1000 LDU):"
        '
        'LblMoveSnap
        '
        Me.LblMoveSnap.AutoSize = True
        Me.LblMoveSnap.Location = New System.Drawing.Point(12, 82)
        Me.LblMoveSnap.Name = "LblMoveSnap"
        Me.LblMoveSnap.Size = New System.Drawing.Size(134, 13)
        Me.LblMoveSnap.TabIndex = 15
        Me.LblMoveSnap.Text = "Move Snap (1/1000 LDU):"
        '
        'NUDScaleSnap
        '
        Me.NUDScaleSnap.DecimalPlaces = 4
        Me.NUDScaleSnap.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.NUDScaleSnap.Location = New System.Drawing.Point(15, 176)
        Me.NUDScaleSnap.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NUDScaleSnap.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.NUDScaleSnap.Name = "NUDScaleSnap"
        Me.NUDScaleSnap.Size = New System.Drawing.Size(198, 20)
        Me.NUDScaleSnap.TabIndex = 21
        Me.NUDScaleSnap.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'NUDMoveSnap
        '
        Me.NUDMoveSnap.Location = New System.Drawing.Point(15, 98)
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
        Me.LblScaleSnap.Location = New System.Drawing.Point(12, 160)
        Me.LblScaleSnap.Name = "LblScaleSnap"
        Me.LblScaleSnap.Size = New System.Drawing.Size(65, 13)
        Me.LblScaleSnap.TabIndex = 17
        Me.LblScaleSnap.Text = "Scale Snap:"
        '
        'LblRotateSnap
        '
        Me.LblRotateSnap.AutoSize = True
        Me.LblRotateSnap.Location = New System.Drawing.Point(12, 121)
        Me.LblRotateSnap.Name = "LblRotateSnap"
        Me.LblRotateSnap.Size = New System.Drawing.Size(114, 13)
        Me.LblRotateSnap.TabIndex = 16
        Me.LblRotateSnap.Text = "Rotate Snap (Degree):"
        '
        'NUDRotateSnap
        '
        Me.NUDRotateSnap.DecimalPlaces = 4
        Me.NUDRotateSnap.Location = New System.Drawing.Point(15, 137)
        Me.NUDRotateSnap.Maximum = New Decimal(New Integer() {3599999, 0, 0, 262144})
        Me.NUDRotateSnap.Minimum = New Decimal(New Integer() {1, 0, 0, 262144})
        Me.NUDRotateSnap.Name = "NUDRotateSnap"
        Me.NUDRotateSnap.Size = New System.Drawing.Size(198, 20)
        Me.NUDRotateSnap.TabIndex = 19
        Me.NUDRotateSnap.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'BtnOne
        '
        Me.BtnOne.BackgroundImage = Global.LDPatternCreator.My.Resources.Resources.snap4_stud
        Me.BtnOne.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnOne.Location = New System.Drawing.Point(15, 12)
        Me.BtnOne.Name = "BtnOne"
        Me.BtnOne.Size = New System.Drawing.Size(32, 32)
        Me.BtnOne.TabIndex = 23
        Me.BtnOne.UseVisualStyleBackColor = True
        '
        'BtnTenth
        '
        Me.BtnTenth.BackgroundImage = Global.LDPatternCreator.My.Resources.Resources.snap3_coarse
        Me.BtnTenth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnTenth.Location = New System.Drawing.Point(70, 12)
        Me.BtnTenth.Name = "BtnTenth"
        Me.BtnTenth.Size = New System.Drawing.Size(32, 32)
        Me.BtnTenth.TabIndex = 24
        Me.BtnTenth.UseVisualStyleBackColor = True
        '
        'BtnHundredth
        '
        Me.BtnHundredth.BackgroundImage = Global.LDPatternCreator.My.Resources.Resources.snap2_medium
        Me.BtnHundredth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnHundredth.Location = New System.Drawing.Point(125, 12)
        Me.BtnHundredth.Name = "BtnHundredth"
        Me.BtnHundredth.Size = New System.Drawing.Size(32, 32)
        Me.BtnHundredth.TabIndex = 25
        Me.BtnHundredth.UseVisualStyleBackColor = True
        '
        'BtnThousandth
        '
        Me.BtnThousandth.BackgroundImage = Global.LDPatternCreator.My.Resources.Resources.snap1_fine
        Me.BtnThousandth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnThousandth.Location = New System.Drawing.Point(180, 12)
        Me.BtnThousandth.Name = "BtnThousandth"
        Me.BtnThousandth.Size = New System.Drawing.Size(32, 32)
        Me.BtnThousandth.TabIndex = 26
        Me.BtnThousandth.UseVisualStyleBackColor = True
        '
        'LblOne
        '
        Me.LblOne.AutoSize = True
        Me.LblOne.Location = New System.Drawing.Point(12, 47)
        Me.LblOne.Name = "LblOne"
        Me.LblOne.Size = New System.Drawing.Size(38, 13)
        Me.LblOne.TabIndex = 27
        Me.LblOne.Text = "1 LDU"
        '
        'LblTenth
        '
        Me.LblTenth.AutoSize = True
        Me.LblTenth.Location = New System.Drawing.Point(80, 47)
        Me.LblTenth.Name = "LblTenth"
        Me.LblTenth.Size = New System.Drawing.Size(22, 13)
        Me.LblTenth.TabIndex = 28
        Me.LblTenth.Text = "0.1"
        '
        'LblHundredth
        '
        Me.LblHundredth.AutoSize = True
        Me.LblHundredth.Location = New System.Drawing.Point(129, 47)
        Me.LblHundredth.Name = "LblHundredth"
        Me.LblHundredth.Size = New System.Drawing.Size(28, 13)
        Me.LblHundredth.TabIndex = 29
        Me.LblHundredth.Text = "0.01"
        '
        'LblThousandth
        '
        Me.LblThousandth.AutoSize = True
        Me.LblThousandth.Location = New System.Drawing.Point(184, 47)
        Me.LblThousandth.Name = "LblThousandth"
        Me.LblThousandth.Size = New System.Drawing.Size(34, 13)
        Me.LblThousandth.TabIndex = 30
        Me.LblThousandth.Text = "0.001"
        '
        'PreferencesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(227, 260)
        Me.Controls.Add(Me.LblThousandth)
        Me.Controls.Add(Me.LblHundredth)
        Me.Controls.Add(Me.LblTenth)
        Me.Controls.Add(Me.LblOne)
        Me.Controls.Add(Me.BtnThousandth)
        Me.Controls.Add(Me.BtnHundredth)
        Me.Controls.Add(Me.BtnTenth)
        Me.Controls.Add(Me.BtnOne)
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
    Friend WithEvents BtnOne As Button
    Friend WithEvents BtnTenth As Button
    Friend WithEvents BtnHundredth As Button
    Friend WithEvents BtnThousandth As Button
    Friend WithEvents LblOne As Label
    Friend WithEvents LblTenth As Label
    Friend WithEvents LblHundredth As Label
    Friend WithEvents LblThousandth As Label
End Class
