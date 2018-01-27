Imports System.Windows.Forms

Public Class SetKeyDialog

    Private lastKey As System.Windows.Forms.KeyEventArgs

    Private Sub Cancel_Button_MouseClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.MouseClick
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetKeyDialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.None Then
            KeyToSet.keyToSet = New KeyEventArgs(Keys.Escape)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub SetKeyDialog_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        KeyToSet.keyToSet = e
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub


End Class
