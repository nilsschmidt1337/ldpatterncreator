Option Strict Off
Public Class PreferencesForm

    Dim isLoading As Boolean = True

    Private Sub PreferencesForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainForm.mySettings.prefsWindow_x = Me.Location.X
        MainForm.mySettings.prefsWindow_y = Me.Location.Y
        MainForm.ViewPrefsToolStripMenuItem.Checked = False
    End Sub

    Private Sub NUDGrid_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDGrid.ValueChanged
        If isLoading Then Exit Sub
        If MainState.conversionEnabled Then
            View.rasterSnap = NUDGrid.Value / View.unitFactor
        Else
            View.rasterSnap = Fix(NUDGrid.Value * 10 ^ NUDGrid.DecimalPlaces) / 10 ^ NUDGrid.DecimalPlaces / View.unitFactor
        End If
        MainForm.Refresh()
    End Sub

    Private Sub PreferencesForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = MainForm.ViewPrefsToolStripMenuItem.ShortcutKeys Then Me.Close()
        If e.KeyCode = MainForm.ImageToolStripMenuItem.ShortcutKeys Then ImageForm.Close()
    End Sub

    Private Sub NUDMoveSnap_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDMoveSnap.ValueChanged
        If isLoading Then Exit Sub
        If MainState.conversionEnabled Then
            View.moveSnap = NUDMoveSnap.Value / View.unitFactor
        Else
            View.moveSnap = Fix(NUDMoveSnap.Value * 10 ^ NUDMoveSnap.DecimalPlaces) / 10 ^ NUDMoveSnap.DecimalPlaces / View.unitFactor
        End If
        MainForm.NUDVertX.Increment = View.moveSnap * View.unitFactor / 1000
        MainForm.NUDVertY.Increment = View.moveSnap * View.unitFactor / 1000
        MainForm.Refresh()
    End Sub

    Private Sub NUDRotateSnap_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDRotateSnap.ValueChanged
        If isLoading Then Exit Sub
        View.rotateSnap = CDbl(NUDRotateSnap.Value)
    End Sub

    Private Sub NUDScaleSnap_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDScaleSnap.ValueChanged
        If isLoading Then Exit Sub
        View.scaleSnap = CDbl(NUDScaleSnap.Value)
    End Sub

    Private Sub PreferencesForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' View-Preferences
        Me.Text = Replace(I18N.trl8(I18N.lk.ViewPreferences), "&", "")
        Me.LblMoveSnap.Text = String.Format(I18N.trl8(I18N.lk.MoveSnap), translateUnit(View.unit))
        Me.LblRotateSnap.Text = I18N.trl8(I18N.lk.RotateSnap)
        Me.LblScaleSnap.Text = I18N.trl8(I18N.lk.ScaleSnap)
        Me.LblGridSize.Text = String.Format(I18N.trl8(I18N.lk.GridSize), translateUnit(View.unit))
        Me.TopMost = True
        Me.Focus()
    End Sub

    Private Function translateUnit(ByVal unit As String) As String
        Select Case View.unit
            Case "LDU" : Return I18N.trl8(I18N.lk.LDU)
            Case "mm" : Return I18N.trl8(I18N.lk.mm)
        End Select
        Return I18N.trl8(I18N.lk.Inch)
    End Function

    Private Sub PreferencesForm_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        Dim lr_x As Integer = Me.Location.X + Me.Size.Width
        Dim lr_y As Integer = Me.Location.Y + Me.Size.Height
        Dim ul_x As Integer = Me.Location.X
        Dim ul_y As Integer = Me.Location.Y
        If (lr_x > Screen.PrimaryScreen.Bounds.Width) Then Me.Location = New Point(Screen.PrimaryScreen.Bounds.Width - Me.Size.Width, Me.Location.Y)
        If (lr_y > Screen.PrimaryScreen.Bounds.Height) Then Me.Location = New Point(Me.Location.X, Screen.PrimaryScreen.Bounds.Height - Me.Size.Height)
        If (ul_x < 0) Then Me.Location = New Point(0, Me.Location.Y)
        If (ul_y < 0) Then Me.Location = New Point(Me.Location.X, 0)
    End Sub

    Private Sub PreferencesForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.BringToFront()
        Me.TopMost = False
        isLoading = True
        Me.NUDMoveSnap.Value = View.moveSnap
        Me.NUDRotateSnap.Value = View.rotateSnap
        Me.NUDScaleSnap.Value = View.scaleSnap
        Me.NUDGrid.Value = View.rasterSnap
        Me.Refresh()
        isLoading = False
        Me.Location = New Point(MainForm.mySettings.prefsWindow_x, MainForm.mySettings.prefsWindow_y)
    End Sub

    Private Sub PreferencesForm_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        Me.BringToFront()
        Me.TopMost = False
    End Sub
End Class