<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RingDialog
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
        Me.LblResolution = New System.Windows.Forms.Label()
        Me.CB48 = New System.Windows.Forms.CheckBox()
        Me.CBResolution = New System.Windows.Forms.ComboBox()
        Me.LblRadius = New System.Windows.Forms.Label()
        Me.CBRadius = New System.Windows.Forms.ComboBox()
        Me.LblName = New System.Windows.Forms.Label()
        Me.CBName = New System.Windows.Forms.ComboBox()
        Me.LblFullName = New System.Windows.Forms.Label()
        Me.TBName = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel1.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(26, 228)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(171, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(79, 23)
        Me.OK_Button.TabIndex = 4
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(88, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(80, 23)
        Me.Cancel_Button.TabIndex = 5
        Me.Cancel_Button.Text = "Cancel"
        '
        'LblResolution
        '
        Me.LblResolution.AutoSize = True
        Me.LblResolution.Location = New System.Drawing.Point(13, 84)
        Me.LblResolution.Name = "LblResolution"
        Me.LblResolution.Size = New System.Drawing.Size(60, 13)
        Me.LblResolution.TabIndex = 1
        Me.LblResolution.Text = "Resolution:"
        '
        'CB48
        '
        Me.CB48.AutoSize = True
        Me.CB48.Location = New System.Drawing.Point(12, 12)
        Me.CB48.Name = "CB48"
        Me.CB48.Size = New System.Drawing.Size(38, 17)
        Me.CB48.TabIndex = 1
        Me.CB48.Text = "48"
        Me.CB48.UseVisualStyleBackColor = True
        '
        'CBResolution
        '
        Me.CBResolution.FormattingEnabled = True
        Me.CBResolution.Location = New System.Drawing.Point(12, 100)
        Me.CBResolution.Name = "CBResolution"
        Me.CBResolution.Size = New System.Drawing.Size(194, 21)
        Me.CBResolution.Sorted = True
        Me.CBResolution.TabIndex = 3
        Me.CBResolution.Text = "4-4"
        '
        'LblRadius
        '
        Me.LblRadius.AutoSize = True
        Me.LblRadius.Location = New System.Drawing.Point(13, 38)
        Me.LblRadius.Name = "LblRadius"
        Me.LblRadius.Size = New System.Drawing.Size(43, 13)
        Me.LblRadius.TabIndex = 4
        Me.LblRadius.Text = "Radius:"
        '
        'CBRadius
        '
        Me.CBRadius.FormattingEnabled = True
        Me.CBRadius.Location = New System.Drawing.Point(12, 54)
        Me.CBRadius.Name = "CBRadius"
        Me.CBRadius.Size = New System.Drawing.Size(194, 21)
        Me.CBRadius.Sorted = True
        Me.CBRadius.TabIndex = 2
        Me.CBRadius.Text = "1"
        '
        'LblName
        '
        Me.LblName.AutoSize = True
        Me.LblName.Location = New System.Drawing.Point(13, 133)
        Me.LblName.Name = "LblName"
        Me.LblName.Size = New System.Drawing.Size(38, 13)
        Me.LblName.TabIndex = 6
        Me.LblName.Text = "Name:"
        '
        'CBName
        '
        Me.CBName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CBName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CBName.FormattingEnabled = True
        Me.CBName.Location = New System.Drawing.Point(12, 149)
        Me.CBName.Name = "CBName"
        Me.CBName.Size = New System.Drawing.Size(194, 21)
        Me.CBName.TabIndex = 0
        Me.CBName.Text = "4-4ring1"
        '
        'LblFullName
        '
        Me.LblFullName.AutoSize = True
        Me.LblFullName.Location = New System.Drawing.Point(13, 184)
        Me.LblFullName.Name = "LblFullName"
        Me.LblFullName.Size = New System.Drawing.Size(57, 13)
        Me.LblFullName.TabIndex = 8
        Me.LblFullName.Text = "Full Name:"
        '
        'TBName
        '
        Me.TBName.Location = New System.Drawing.Point(12, 200)
        Me.TBName.Name = "TBName"
        Me.TBName.ReadOnly = True
        Me.TBName.Size = New System.Drawing.Size(194, 20)
        Me.TBName.TabIndex = 9
        Me.TBName.TabStop = False
        Me.TBName.Text = "4-4ring1.dat"
        '
        'RingDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(218, 269)
        Me.Controls.Add(Me.TBName)
        Me.Controls.Add(Me.LblFullName)
        Me.Controls.Add(Me.CBName)
        Me.Controls.Add(Me.LblName)
        Me.Controls.Add(Me.CBRadius)
        Me.Controls.Add(Me.LblRadius)
        Me.Controls.Add(Me.CBResolution)
        Me.Controls.Add(Me.CB48)
        Me.Controls.Add(Me.LblResolution)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RingDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add a Circular Ring Segment: "
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents LblResolution As System.Windows.Forms.Label
    Friend WithEvents CB48 As System.Windows.Forms.CheckBox
    Friend WithEvents CBResolution As System.Windows.Forms.ComboBox
    Friend WithEvents LblRadius As System.Windows.Forms.Label
    Friend WithEvents CBRadius As System.Windows.Forms.ComboBox
    Friend WithEvents LblName As System.Windows.Forms.Label
    Friend WithEvents CBName As System.Windows.Forms.ComboBox
    Friend WithEvents LblFullName As System.Windows.Forms.Label
    Friend WithEvents TBName As System.Windows.Forms.TextBox

End Class
