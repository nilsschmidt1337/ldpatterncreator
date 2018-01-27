Imports System.Windows.Forms

Public Class CenterDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        NUDVertX.Value *= 1000D
        NUDVertY.Value *= 1000D
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub CenterDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        NUDVertX.Value /= 1000D
        NUDVertY.Value /= 1000D
    End Sub
End Class
