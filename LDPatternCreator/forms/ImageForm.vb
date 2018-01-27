Option Strict Off

Public Class ImageForm

    Dim isLoading As Boolean = True

    Private Sub ImageForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainForm.mySettings.backgroundWindow_x = Me.Location.X
        MainForm.mySettings.backgroundWindow_y = Me.Location.Y
        MainForm.ImageToolStripMenuItem.Checked = False
    End Sub

    Private Sub NUDoffsetX_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDoffsetX.ValueChanged
        If isLoading Then Exit Sub
        View.imgOffsetX = CInt(NUDoffsetX.Value)
        MainForm.Refresh()
    End Sub

    Private Sub NUDoffsetY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDoffsetY.ValueChanged
        If isLoading Then Exit Sub
        View.imgOffsetY = CInt(NUDoffsetY.Value)
        MainForm.Refresh()
    End Sub

    Private Sub NUDScale_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDScale.ValueChanged
        If isLoading Then Exit Sub
        If NUDScale.Value < 0.0001D Then NUDScale.Value = 0.0002D
        View.imgScale = NUDScale.Value
        LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & MainForm.translateUnit(View.unit)
        MainForm.Refresh()
    End Sub

    Private Sub BtnImagePath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImagePath.Click
        Dim result As DialogResult = MainForm.OpenImage.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            If MainForm.OpenImage.FileName <> "" Then
                Try
                    View.backgroundPicture.Dispose()
                    View.backgroundPicture = New Bitmap(MainForm.OpenImage.FileName)
                    View.imgPath = MainForm.OpenImage.FileName
                    LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & MainForm.translateUnit(View.unit)
                Catch ex As Exception
                    MsgBox(I18N.trl8(I18N.lk.InvalidImage), MsgBoxStyle.Exclamation, I18N.trl8(I18N.lk.Fatal))
                    View.backgroundPicture = My.Resources.temp
                    View.imgPath = ""
                End Try
                Me.TBImage.Text = View.imgPath
                View.backgroundPictureBrush.Dispose()
                View.backgroundPictureBrush = New TextureBrush(View.backgroundPicture)
            End If
            MainForm.Refresh()
        End If
    End Sub

    Private Sub BtnAdjust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdjust.Click
        If MainForm.MainToolStrip.Visible Then MainForm.startPrimitiveMode()
        MainState.primitiveMode = PrimitiveModes.Inactive
        MainForm.BtnAbort.Text = I18N.trl8(I18N.lk.OK) & " []" : KeyToSet.setKey(MainForm.BtnAbort, MainForm.myKeys.Abort)
        MainForm.BtnAbort.BackColor = Color.Green
        MainForm.BtnAbort.Visible = True
        MainForm.MainToolStrip.Visible = False
        MainForm.MenuStrip1.Visible = False
        MainForm.BtnAbort.Visible = True
        MainState.adjustmode = True
        If MainForm.BtnAddVertex.Checked Then MainForm.BtnAddVertex.PerformClick()
        If MainForm.BtnAddTriangle.Checked Then MainForm.BtnAddTriangle.PerformClick()
        MainForm.BtnSelect.PerformClick()
        MainForm.Refresh()
    End Sub

    Private Sub ImageForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = MainForm.ImageToolStripMenuItem.ShortcutKeys Then Me.Close()
        If e.KeyCode = MainForm.ViewPrefsToolStripMenuItem.ShortcutKeys Then PreferencesForm.Close()
    End Sub

    Private Sub ImageForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Image       
        Me.Text = I18N.trl8(I18N.lk.ImageTitle)
        Me.LblImageFile.Text = I18N.trl8(I18N.lk.ImageFile)
        Me.LblImageOffsetX.Text = I18N.trl8(I18N.lk.OffsetX)
        Me.LblImageOffsetY.Text = I18N.trl8(I18N.lk.OffsetY)
        Me.LblImageScale.Text = I18N.trl8(I18N.lk.ImageScale)
        Me.BtnAdjust.Text = I18N.trl8(I18N.lk.AdjustImageBtn)
        Me.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
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

    Private Sub ImageForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.TopMost = False
        isLoading = True
        Me.NUDoffsetX.Value = View.imgOffsetX
        Me.NUDoffsetY.Value = View.imgOffsetY
        Me.NUDScale.Value = View.imgScale
        Me.TBImage.Text = View.imgPath
        Me.Refresh()
        isLoading = False
        Me.Location = New Point(MainForm.mySettings.backgroundWindow_x, MainForm.mySettings.backgroundWindow_y)
    End Sub
End Class