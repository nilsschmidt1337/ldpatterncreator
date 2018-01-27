<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportDialog
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
        Me.GBMode = New System.Windows.Forms.GroupBox()
        Me.RBtemplate = New System.Windows.Forms.RadioButton()
        Me.RBappend = New System.Windows.Forms.RadioButton()
        Me.RBnew = New System.Windows.Forms.RadioButton()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GBMode.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(86, 165)
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
        Me.OK_Button.Text = "Import"
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
        'GBMode
        '
        Me.GBMode.Controls.Add(Me.RBtemplate)
        Me.GBMode.Controls.Add(Me.RBappend)
        Me.GBMode.Controls.Add(Me.RBnew)
        Me.GBMode.Location = New System.Drawing.Point(12, 12)
        Me.GBMode.Name = "GBMode"
        Me.GBMode.Size = New System.Drawing.Size(307, 147)
        Me.GBMode.TabIndex = 1
        Me.GBMode.TabStop = False
        Me.GBMode.Text = "Import-Options:"
        '
        'RBtemplate
        '
        Me.RBtemplate.AutoSize = True
        Me.RBtemplate.Location = New System.Drawing.Point(6, 109)
        Me.RBtemplate.Name = "RBtemplate"
        Me.RBtemplate.Size = New System.Drawing.Size(195, 17)
        Me.RBtemplate.TabIndex = 2
        Me.RBtemplate.TabStop = True
        Me.RBtemplate.Text = "Overwrite and import projection data"
        Me.RBtemplate.UseVisualStyleBackColor = True
        '
        'RBappend
        '
        Me.RBappend.AutoSize = True
        Me.RBappend.Location = New System.Drawing.Point(6, 66)
        Me.RBappend.Name = "RBappend"
        Me.RBappend.Size = New System.Drawing.Size(135, 17)
        Me.RBappend.TabIndex = 1
        Me.RBappend.Text = "Append part as subpart"
        Me.RBappend.UseVisualStyleBackColor = True
        '
        'RBnew
        '
        Me.RBnew.AutoSize = True
        Me.RBnew.Checked = True
        Me.RBnew.Location = New System.Drawing.Point(6, 21)
        Me.RBnew.Name = "RBnew"
        Me.RBnew.Size = New System.Drawing.Size(129, 17)
        Me.RBnew.TabIndex = 0
        Me.RBnew.TabStop = True
        Me.RBnew.Text = "Overwrite existing part"
        Me.RBnew.UseVisualStyleBackColor = True
        '
        'ImportDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(331, 206)
        Me.Controls.Add(Me.GBMode)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ImportDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Import a DAT File: "
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GBMode.ResumeLayout(False)
        Me.GBMode.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GBMode As System.Windows.Forms.GroupBox
    Public WithEvents RBappend As System.Windows.Forms.RadioButton
    Public WithEvents RBnew As System.Windows.Forms.RadioButton
    Public WithEvents RBtemplate As System.Windows.Forms.RadioButton

End Class
