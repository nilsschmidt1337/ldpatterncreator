' MIT - License
'
' Copyright (c) 2010 - 2017 Nils Schmidt
' This program uses Rectifier.exe/Unificator.exe by permission of the author and copyright holder Philippe E. Hurbain - (C) 2012

' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
' to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
' and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

' The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
' INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
' PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
' FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
' ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Option Strict Off
Public Class ColourForm

    Public isDialog As Boolean

    Private Sub ColourForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainForm.mySettings.colourWindow_x = Me.Location.X
        MainForm.mySettings.colourWindow_y = Me.Location.Y
        MainForm.mySettings.colourWindow_width = Me.Size.Width
        MainForm.mySettings.colourWindow_height = Me.Size.Height
    End Sub

    Private Sub ColourForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If MainState.cstep = 1 Then
            If e.KeyCode = Keys.NumPad1 Then MainState.cnumber += 1
            If e.KeyCode = Keys.NumPad2 Then MainState.cnumber += 2
            If e.KeyCode = Keys.NumPad3 Then MainState.cnumber += 3
            If e.KeyCode = Keys.NumPad4 Then MainState.cnumber += 4
            If e.KeyCode = Keys.NumPad5 Then MainState.cnumber += 5
            If e.KeyCode = Keys.NumPad6 Then MainState.cnumber += 6
            If e.KeyCode = Keys.NumPad7 Then MainState.cnumber += 7
            If e.KeyCode = Keys.NumPad8 Then MainState.cnumber += 8
            If e.KeyCode = Keys.NumPad9 Then MainState.cnumber += 9
            If LDConfig.colourHMap.ContainsKey(MainState.cnumber) Then
                MainForm.setColour(LDConfig.colourHMap(MainState.cnumber), MainState.cnumber)
            Else
                MainForm.setColour(LDConfig.colourHMap(16), MainState.cnumber)
            End If
            UndoRedoHelper.addHistory()
            MainState.cstep = 100 : MainState.cnumber = 0
        Else
            If e.KeyCode = Keys.Escape Then MainState.cstep = 100 : MainState.cnumber = 0
            If e.KeyCode = Keys.NumPad0 Then MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad1 Then MainState.cnumber += 1 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad2 Then MainState.cnumber += 2 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad3 Then MainState.cnumber += 3 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad4 Then MainState.cnumber += 4 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad5 Then MainState.cnumber += 5 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad6 Then MainState.cnumber += 6 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad7 Then MainState.cnumber += 7 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad8 Then MainState.cnumber += 8 * MainState.cstep : MainState.cstep /= 10
            If e.KeyCode = Keys.NumPad9 Then MainState.cnumber += 9 * MainState.cstep : MainState.cstep /= 10
        End If
    End Sub

    Private Sub ColourForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = I18N.trl8(I18N.lk.ColourTitle)
        Me.GBDirectColour.Text = I18N.trl8(I18N.lk.DirectColour) & vbCrLf & "(0x2RRGGBB):"
        Me.BtnDirectColour.Text = I18N.trl8(I18N.lk.SetColour)
        Me.Controls("Colours").Controls.Clear()
        Me.TopMost = True
        ' Buttons anlegen
        For i As Integer = 0 To LDConfig.maxColourNumber
            If LDConfig.colourHMap.ContainsKey(i) AndAlso LDConfig.colourHMapName(i) <> "?" Then
                Dim btn As New Button
                btn.Height = 25
                btn.Width = 25
                ToolTip1.SetToolTip(btn, I18N.trl8(I18N.lk.Colour) & " " & i & ": " & LDConfig.colourHMapName(i))
                btn.Name = "btn" & i
                btn.BackColor = LDConfig.colourHMap(i)
                btn.Tag = i
                btn.FlatStyle = FlatStyle.Flat
                Me.Controls("Colours").Controls.Add(btn)
                AddHandler btn.Click, AddressOf btn_click
            End If
        Next
        Me.LblTip.Text = String.Format(I18N.trl8(I18N.lk.ColourTip), KeyToSet.keyToString(New KeyEventArgs(MainForm.myKeys.Abort)))
        Me.BtnDirectColour.Focus()
    End Sub

    Private Sub btn_click(ByVal sender As Object, ByVal e As EventArgs)
        MainForm.setColour(LDConfig.colourHMap(CType(sender.Tag, Short)), CType(sender.Tag, Short))
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub Colours_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Colours.MouseMove
        If Not Me.Focused Then
            Dim sbv As Decimal
            sbv = Colours.VerticalScroll.Value
            Me.Activate()
            Colours.VerticalScroll.Value = sbv
        End If
    End Sub

    Private Sub Colours_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles Colours.Scroll
        If Not Me.Focused Then
            Dim sbv As Decimal
            sbv = Colours.VerticalScroll.Value
            Me.Activate()
            Colours.VerticalScroll.Value = sbv
        End If
    End Sub

    Private Sub BrnDirectColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDirectColour.Click
        MainForm.setColour(Color.FromArgb(255, Fix(NR.Value), Fix(NG.Value), Fix(NB.Value)), -1)
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub BtnExtend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExtend.Click
        If Me.Height = 315 Then
            Me.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
            Me.Location = New Point(Me.Location.X, 0)
        Else
            Me.Height = 315
        End If
    End Sub

    Private Sub ColourForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.BtnDirectColour.Focus()
        Me.Location = New Point(MainForm.mySettings.colourWindow_x, MainForm.mySettings.colourWindow_y)
        Me.Size = New Point(MainForm.mySettings.colourWindow_width, MainForm.mySettings.colourWindow_height)
        Me.TopMost = False
    End Sub
End Class
