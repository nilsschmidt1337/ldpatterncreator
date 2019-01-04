<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageForm
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
        Me.BtnAdjust = New System.Windows.Forms.Button()
        Me.LblImageSize = New System.Windows.Forms.Label()
        Me.NUDScale = New System.Windows.Forms.NumericUpDown()
        Me.LblImageScale = New System.Windows.Forms.Label()
        Me.NUDoffsetY = New System.Windows.Forms.NumericUpDown()
        Me.LblImageOffsetY = New System.Windows.Forms.Label()
        Me.NUDoffsetX = New System.Windows.Forms.NumericUpDown()
        Me.LblImageOffsetX = New System.Windows.Forms.Label()
        Me.BtnImagePath = New System.Windows.Forms.Button()
        Me.TBImage = New System.Windows.Forms.TextBox()
        Me.LblImageFile = New System.Windows.Forms.Label()
        CType(Me.NUDScale, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDoffsetY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDoffsetX, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnAdjust
        '
        Me.BtnAdjust.Location = New System.Drawing.Point(15, 158)
        Me.BtnAdjust.Name = "BtnAdjust"
        Me.BtnAdjust.Size = New System.Drawing.Size(198, 23)
        Me.BtnAdjust.TabIndex = 30
        Me.BtnAdjust.Text = "Adjust BG Image"
        Me.BtnAdjust.UseVisualStyleBackColor = True
        '
        'LblImageSize
        '
        Me.LblImageSize.AutoSize = True
        Me.LblImageSize.Location = New System.Drawing.Point(12, 184)
        Me.LblImageSize.Name = "LblImageSize"
        Me.LblImageSize.Size = New System.Drawing.Size(52, 13)
        Me.LblImageSize.TabIndex = 29
        Me.LblImageSize.Text = "0 x 0LDU"
        '
        'NUDScale
        '
        Me.NUDScale.DecimalPlaces = 4
        Me.NUDScale.Location = New System.Drawing.Point(143, 132)
        Me.NUDScale.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.NUDScale.Name = "NUDScale"
        Me.NUDScale.Size = New System.Drawing.Size(70, 20)
        Me.NUDScale.TabIndex = 28
        Me.NUDScale.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'LblImageScale
        '
        Me.LblImageScale.AutoSize = True
        Me.LblImageScale.Location = New System.Drawing.Point(12, 134)
        Me.LblImageScale.Name = "LblImageScale"
        Me.LblImageScale.Size = New System.Drawing.Size(34, 13)
        Me.LblImageScale.TabIndex = 27
        Me.LblImageScale.Text = "Scale"
        '
        'NUDoffsetY
        '
        Me.NUDoffsetY.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NUDoffsetY.Location = New System.Drawing.Point(15, 106)
        Me.NUDoffsetY.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.NUDoffsetY.Minimum = New Decimal(New Integer() {1000000, 0, 0, -2147483648})
        Me.NUDoffsetY.Name = "NUDoffsetY"
        Me.NUDoffsetY.Size = New System.Drawing.Size(198, 20)
        Me.NUDoffsetY.TabIndex = 26
        '
        'LblImageOffsetY
        '
        Me.LblImageOffsetY.AutoSize = True
        Me.LblImageOffsetY.Location = New System.Drawing.Point(12, 89)
        Me.LblImageOffsetY.Name = "LblImageOffsetY"
        Me.LblImageOffsetY.Size = New System.Drawing.Size(114, 13)
        Me.LblImageOffsetY.TabIndex = 21
        Me.LblImageOffsetY.Text = "Offset Y (1/1000 LDU)"
        '
        'NUDoffsetX
        '
        Me.NUDoffsetX.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NUDoffsetX.Location = New System.Drawing.Point(15, 66)
        Me.NUDoffsetX.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.NUDoffsetX.Minimum = New Decimal(New Integer() {1000000, 0, 0, -2147483648})
        Me.NUDoffsetX.Name = "NUDoffsetX"
        Me.NUDoffsetX.Size = New System.Drawing.Size(198, 20)
        Me.NUDoffsetX.TabIndex = 25
        '
        'LblImageOffsetX
        '
        Me.LblImageOffsetX.AutoSize = True
        Me.LblImageOffsetX.Location = New System.Drawing.Point(12, 49)
        Me.LblImageOffsetX.Name = "LblImageOffsetX"
        Me.LblImageOffsetX.Size = New System.Drawing.Size(114, 13)
        Me.LblImageOffsetX.TabIndex = 24
        Me.LblImageOffsetX.Text = "Offset X (1/1000 LDU)"
        '
        'BtnImagePath
        '
        Me.BtnImagePath.Location = New System.Drawing.Point(193, 26)
        Me.BtnImagePath.Name = "BtnImagePath"
        Me.BtnImagePath.Size = New System.Drawing.Size(20, 20)
        Me.BtnImagePath.TabIndex = 23
        Me.BtnImagePath.Text = "..."
        Me.BtnImagePath.UseVisualStyleBackColor = True
        '
        'TBImage
        '
        Me.TBImage.Location = New System.Drawing.Point(15, 26)
        Me.TBImage.Name = "TBImage"
        Me.TBImage.ReadOnly = True
        Me.TBImage.Size = New System.Drawing.Size(172, 20)
        Me.TBImage.TabIndex = 22
        '
        'LblImageFile
        '
        Me.LblImageFile.AutoSize = True
        Me.LblImageFile.Location = New System.Drawing.Point(12, 9)
        Me.LblImageFile.Name = "LblImageFile"
        Me.LblImageFile.Size = New System.Drawing.Size(23, 13)
        Me.LblImageFile.TabIndex = 20
        Me.LblImageFile.Text = "File"
        '
        'ImageForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(223, 228)
        Me.Controls.Add(Me.BtnAdjust)
        Me.Controls.Add(Me.LblImageSize)
        Me.Controls.Add(Me.NUDScale)
        Me.Controls.Add(Me.LblImageScale)
        Me.Controls.Add(Me.NUDoffsetY)
        Me.Controls.Add(Me.LblImageOffsetY)
        Me.Controls.Add(Me.NUDoffsetX)
        Me.Controls.Add(Me.LblImageOffsetX)
        Me.Controls.Add(Me.BtnImagePath)
        Me.Controls.Add(Me.TBImage)
        Me.Controls.Add(Me.LblImageFile)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "ImageForm"
        Me.Text = "Background-Image [F5]: "
        CType(Me.NUDScale, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDoffsetY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDoffsetX, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAdjust As System.Windows.Forms.Button
    Friend WithEvents LblImageSize As System.Windows.Forms.Label
    Friend WithEvents NUDScale As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblImageScale As System.Windows.Forms.Label
    Friend WithEvents NUDoffsetY As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblImageOffsetY As System.Windows.Forms.Label
    Friend WithEvents NUDoffsetX As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblImageOffsetX As System.Windows.Forms.Label
    Friend WithEvents BtnImagePath As System.Windows.Forms.Button
    Friend WithEvents TBImage As System.Windows.Forms.TextBox
    Friend WithEvents LblImageFile As System.Windows.Forms.Label
End Class
