Imports System.Windows.Forms

Public Class StickerDialog

    Private wInt As Integer
    Private wFloat As Integer

    Private hInt As Integer
    Private hFloat As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NUDWidth_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDWidth.ValueChanged
        recalc(CDec(NUDWidth.Value * View.unitFactor), CDec(NUDHeight.Value * View.unitFactor))
    End Sub

    Private Sub NUDHeight_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDHeight.ValueChanged
        recalc(CDec(NUDWidth.Value * View.unitFactor), CDec(NUDHeight.Value * View.unitFactor))
    End Sub

    Private Sub StickerDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.OK_Button.Text = I18N.trl8(I18N.lk.OK)
        Me.Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
        Me.Text = I18N.trl8(I18N.lk.StickerGenerator)
        Me.GBDescription.Text = I18N.trl8(I18N.lk.MPart)

        Dim s As String = I18N.trl8(I18N.lk.BDimension)
        Me.GBSize.Text = Mid(s, 1, s.IndexOf(CChar("(")) - 1) & ":"
        Me.LblWidth.Text = I18N.trl8(I18N.lk.Width)
        Me.LblHeight.Text = I18N.trl8(I18N.lk.Height)
        LblUnit1.Text = MainForm.LblUnit1.Text
        LblUnit2.Text = LblUnit1.Text
        NUDWidth.Value = CDec(View.unitFactor * 10)
        NUDHeight.Value = CDec(View.unitFactor * 10)
        TBDescription.Text = ""
        LblAutoDescription.Text = "Sticker  0.5 x  0.5 with "
    End Sub

    Private Sub recalc(ByVal w As Decimal, ByVal h As Decimal)
        wInt = CInt(Fix(w / 20))
        hInt = CInt(Fix(h / 20))
        wFloat = CInt(Fix(w * 10 / 20)) Mod 10
        hFloat = CInt(Fix(h * 10 / 20)) Mod 10

        Dim sW As String = "" & wInt
        Dim sH As String = "" & hInt
        Dim lW As Integer = sW.Length
        Dim lH As Integer = sH.Length
        If lW = lH AndAlso lW = 1 Then
            sW = " " & sW
            sH = " " & sH
        End If
        While lW > lH
            lH += 1
            sH = " " & sH
        End While
        While lW < lH
            lW += 1
            sW = " " & sW
        End While

        sW = sW & "." & wFloat
        sH = sH & "." & hFloat

        LblAutoDescription.Text = "Sticker " & sH & " x " & sW & " with "
    End Sub

End Class
