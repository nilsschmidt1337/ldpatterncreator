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
Imports System.Windows.Forms

Public Class OptionsDialog
    Private keyDict As New Dictionary(Of String, Integer)

    Private dummyToolStripMenuItem As New ToolStripMenuItem
    Private dummyKeyEventArgs As System.Windows.Forms.KeyEventArgs

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        saveKeys()
        saveSettings()
        saveColours()
        loadSettings()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OptionsDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        loadKeys()
        loadSettings()
        loadColours()
    End Sub

    Private Sub loadColours()
        LColours.Items.Clear()
        LTags.Items.Clear()
        addColour(I18N.trl8(I18N.lk.C00), LDSettings.Colours.background)
        PBack.BackColor = LDSettings.Colours.background
        addColour(I18N.trl8(I18N.lk.C01), LDSettings.Colours.linePen)
        addColour(I18N.trl8(I18N.lk.C02), LDSettings.Colours.inverseLinePen)
        addColour(I18N.trl8(I18N.lk.C03), LDSettings.Colours.selectedLinePen)
        addColour(I18N.trl8(I18N.lk.C04), LDSettings.Colours.selectedLineInVertexModePen)
        addColour(I18N.trl8(I18N.lk.C05), LDSettings.Colours.vertexBrush)
        addColour(I18N.trl8(I18N.lk.C06), LDSettings.Colours.selectedVertexBrush)
        addColour(I18N.trl8(I18N.lk.C07), LDSettings.Colours.originPen)
        addColour(I18N.trl8(I18N.lk.C08), LDSettings.Colours.gridPen)
        addColour(I18N.trl8(I18N.lk.C09), LDSettings.Colours.grid10Pen)
        addColour(I18N.trl8(I18N.lk.C10), LDSettings.Colours.selectionRectPen)
        addColour(I18N.trl8(I18N.lk.C11), LDSettings.Colours.selectionCrossBrush)
    End Sub

    Private Sub saveColours()
        LDSettings.Colours.background = LTags.Items(0)
        Try
            MainForm.BackColor = CType(LTags.Items(0), Color)
            MainForm.Opacity = 1D
        Catch
            MainForm.BackColor = Color.FromArgb(CType(LTags.Items(0), Color).R, CType(LTags.Items(0), Color).G, CType(LTags.Items(0), Color).B)
            MainForm.Opacity = CType(LTags.Items(0), Color).A / 255
        End Try
        LDSettings.Colours.linePen = LTags.Items(1)
        LDSettings.Colours.inverseLinePen = LTags.Items(2)
        LDSettings.Colours.selectedLinePen = LTags.Items(3)
        LDSettings.Colours.selectedLineInVertexModePen = LTags.Items(4)
        LDSettings.Colours.vertexBrush = LTags.Items(5)
        LDSettings.Colours.selectedVertexBrush = LTags.Items(6)
        LDSettings.Colours.originPen = LTags.Items(7)
        LDSettings.Colours.gridPen = LTags.Items(8)
        LDSettings.Colours.grid10Pen = LTags.Items(9)
        LDSettings.Colours.selectionRectPen = LTags.Items(10)
        LDSettings.Colours.selectionCrossBrush = LTags.Items(11)
    End Sub

    Private Sub loadSettings()
        CBLicense.Text = LDSettings.Editor.defaultLicense
        TBRealName.Text = LDSettings.Editor.defaultName
        TBUserName.Text = LDSettings.Editor.defaultUser
        NUDMaxUndo.Value = LDSettings.Editor.max_undo
        CBPerformaceMode.Checked = LDSettings.Editor.performanceMode
        CBViewImage.Checked = LDSettings.Editor.showImageViewAtStartup
        CBViewPreferences.Checked = LDSettings.Editor.showPreferencesViewAtStartup
        CBFullscreen.Checked = LDSettings.Editor.startWithFullscreen
        CBAlternativeZoomAndTrans.Checked = LDSettings.Editor.useAlternativeKeys
        CBTemplateLinesTop.Checked = LDSettings.Editor.showTemplateLinesOnTop
        CBAddModeLock.Checked = LDSettings.Editor.lockModeChange
    End Sub

    Private Sub loadKeys()
        keyDict.Clear()
        keyDict.Add("Add", -1)
        keyDict.Add("Subtract", -1)
        keyDict.Add("Oemplus", -1)
        keyDict.Add("OemMinus", -1)
        keyDict.Add("NumPad0", -1)
        keyDict.Add("NumPad1", -1)
        keyDict.Add("NumPad2", -1)
        keyDict.Add("NumPad3", -1)
        keyDict.Add("NumPad4", -1)
        keyDict.Add("NumPad5", -1)
        keyDict.Add("NumPad6", -1)
        keyDict.Add("NumPad7", -1)
        keyDict.Add("NumPad8", -1)
        keyDict.Add("NumPad9", -1)
        keyDict.Add("Del", -1)
        keyDict.Add("Ctrl+X", -1)
        keyDict.Add("Ctrl+C", -1)
        keyDict.Add("Ctrl+V", -1)
        keyDict.Add("Ctrl+Z", -1)
        keyDict.Add("Ctrl+Y", -1)
        keyDict.Add("Left", -1)
        keyDict.Add("Right", -1)
        keyDict.Add("Up", -1)
        keyDict.Add("Down", -1)
        keyDict.Add("Ctrl+Alt", -2)
        keyDict.Add("Ctrl+Shift", -2)
        keyDict.Add("Shift+Alt", -2)
        keyDict.Add("Ctrl+Shift+Alt", -2)
        Shortkeys.Rows.Clear()
        If CBAlternativeZoomAndTrans.Checked Then
            If My.Computer.Mouse.ButtonsSwapped Then
                Shortkeys.Rows.Add(I18N.trl8(I18N.lk.Context1), I18N.trl8(I18N.lk.LMB), "---", -1)
            Else
                Shortkeys.Rows.Add(I18N.trl8(I18N.lk.Context1), I18N.trl8(I18N.lk.RMB), "---", -1)
            End If
        Else
            If My.Computer.Mouse.ButtonsSwapped Then
                Shortkeys.Rows.Add(I18N.trl8(I18N.lk.Context2), I18N.trl8(I18N.lk.LMB), "---", -1)
            Else
                Shortkeys.Rows.Add(I18N.trl8(I18N.lk.Context2), I18N.trl8(I18N.lk.RMB), "---", -1)
            End If
        End If
        Shortkeys.Rows(Shortkeys.Rows.Count - 1).Tag = 0
        addKey(I18N.trl8(I18N.lk.Key00), MainForm.NewPatternToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key01), MainForm.LoadPatternToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key02), MainForm.SaveToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key03), MainForm.BtnSelect)
        addKey(I18N.trl8(I18N.lk.Key04), MainForm.BtnMove)
        addKey(I18N.trl8(I18N.lk.Key05), MainForm.BtnRotate)
        addKey(I18N.trl8(I18N.lk.Key06), MainForm.BtnScale)
        addKey(I18N.trl8(I18N.lk.Key07), MainForm.BtnAddVertex)
        addKey(I18N.trl8(I18N.lk.Key08), MainForm.BtnAddTriangle)
        addKey(I18N.trl8(I18N.lk.Key09), MainForm.VerticesModeToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key10), MainForm.TrianglesModeToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key11), MainForm.PrimitiveModeToolStripMenuItem)

        addKey(I18N.trl8(I18N.lk.Key12), MainForm.ImageToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key13), MainForm.ViewPrefsToolStripMenuItem)

        addKey(I18N.trl8(I18N.lk.AddPrimitive), MainForm.BtnPrimitives)
        addKey(I18N.trl8(I18N.lk.MergeSplit), MainForm.BtnMerge)
        addKey(I18N.trl8(I18N.lk.CSG), MainForm.BtnCSG)

        addKey(I18N.trl8(I18N.lk.Key14), MainForm.BtnPreview)
        addKey(I18N.trl8(I18N.lk.Key15), MainForm.BtnColours)
        addKey(I18N.trl8(I18N.lk.Key16), MainForm.BtnPipette)
        addKey(I18N.trl8(I18N.lk.Key17), MainForm.BtnAbort)

        addKey(I18N.trl8(I18N.lk.Key18), MainForm.BtnZoom)
        addKey(I18N.trl8(I18N.lk.Key19), MainForm.BtnTranslate)
        addKey(I18N.trl8(I18N.lk.Key20), MainForm.ResetViewToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key21), MainForm.SelectAllToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key22), MainForm.SelectSameColourToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key23), MainForm.SelectConnectedToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key24), MainForm.SelectTouchingToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key25), MainForm.WithColourToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key26), MainForm.ToAverageToolStripMenuItem)
        addKey(I18N.trl8(I18N.lk.Key27), MainForm.ToLastSelectedToolStripMenuItem)

        addKey(Replace(I18N.trl8(I18N.lk.ShowGrid), "&", ""), MainForm.ShowGridToolStripMenuItem)
        addKey(Replace(I18N.trl8(I18N.lk.ShowAxis), "&", ""), MainForm.ShowAxisLabelToolStripMenuItem)
        addKey(Replace(I18N.trl8(I18N.lk.ShowImage), "&", ""), MainForm.ShowBackgroundImageToolStripMenuItem)

        addKey(StrConv(Replace(I18N.trl8(I18N.lk.ToNearestLineTemplate), "..", ""), VbStrConv.ProperCase), MainForm.MergeToNearestTemplateLineToolStripMenuItem)
        addKey(StrConv(Replace(I18N.trl8(I18N.lk.ToNearestLineTriangle), "..", ""), VbStrConv.ProperCase), MainForm.MergeToNearestTriangleLineToolStripMenuItem)
        addKey(StrConv(Replace(I18N.trl8(I18N.lk.ToNearestPrim), "..", ""), VbStrConv.ProperCase), MainForm.MergeToNearestPrimvertexToolStripMenuItem)
        addKey(StrConv(Replace(I18N.trl8(I18N.lk.Split), "..", ""), VbStrConv.ProperCase), MainForm.CSGSplitToolStripMenuItem)
    End Sub

    Private Sub saveSettings()
        LDSettings.Editor.defaultLicense = CBLicense.Text
        LDSettings.Editor.defaultName = TBRealName.Text
        LDSettings.Editor.defaultUser = TBUserName.Text
        Dim mu As Byte = Fix(NUDMaxUndo.Value)
        If LDSettings.Editor.max_undo <> mu Then
            UndoRedoHelper.clearHistory()
        End If
        LDSettings.Editor.max_undo = mu
        LDSettings.Editor.performanceMode = CBPerformaceMode.Checked
        LDSettings.Editor.showImageViewAtStartup = CBViewImage.Checked
        LDSettings.Editor.showPreferencesViewAtStartup = CBViewPreferences.Checked
        LDSettings.Editor.startWithFullscreen = CBFullscreen.Checked
        LDSettings.Editor.useAlternativeKeys = CBAlternativeZoomAndTrans.Checked
        LDSettings.Editor.showTemplateLinesOnTop = CBTemplateLinesTop.Checked
        LDSettings.Editor.lockModeChange = CBAddModeLock.Checked
        If MainForm.PerformanceEnabledToolStripMenuItem.Checked <> CBPerformaceMode.Checked Then MainForm.PerformanceEnabledToolStripMenuItem.PerformClick()
    End Sub

    Private Sub saveKeys()
        For i As Integer = 0 To Shortkeys.Rows.Count - 1
            If TypeOf Shortkeys.Rows(i).Tag Is ToolStripMenuItem Then
                Select Case CType(Shortkeys.Rows(i).Tag, ToolStripMenuItem).Name
                    Case "ImageToolStripMenuItem"
                        ImageForm.Text = I18N.trl8(I18N.lk.ImageTitle) & " [" & Shortkeys.Rows(i).Cells(1).Value & "]:"
                    Case "SnappingToolStripMenuItem"
                        PreferencesForm.Text = Replace(I18N.trl8(I18N.lk.ViewPreferences), "&", "") & " [" & Shortkeys.Rows(i).Cells(1).Value & "]:"
                    Case "VerticesModeToolStripMenuItem"
                        MainForm.CMSVertex.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        MainForm.VerticesModeToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.VertexMode) & " [" & Shortkeys.Rows(i).Cells(1).Value & "]"
                    Case "TrianglesModeToolStripMenuItem"
                        MainForm.CMSTriangle.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        MainForm.TrianglesModeToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.TriangleMode) & " [" & Shortkeys.Rows(i).Cells(1).Value & "]"
                    Case "PrimitiveModeToolStripMenuItem"
                        MainForm.CMSPrimitive.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        MainForm.PrimitiveModeToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.PrimitiveMode) & " [" & Shortkeys.Rows(i).Cells(1).Value & "]"
                    Case "ReferenceLineModeToolStripMenuItem"
                        MainForm.PrimitiveModeToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.ReferenceLineMode)
                End Select
                CType(Shortkeys.Rows(i).Tag, ToolStripMenuItem).ShortcutKeys = CType(Shortkeys.Rows(i).Cells(3).Value, Integer) : CType(Shortkeys.Rows(i).Tag, ToolStripMenuItem).ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
            ElseIf TypeOf Shortkeys.Rows(i).Tag Is ToolStripButton Then
                CType(Shortkeys.Rows(i).Tag, ToolStripButton).ToolTipText = Shortkeys.Rows(i).Tag.Text & " [" & Shortkeys.Rows(i).Cells(1).Value & "]"
                Select Case CType(Shortkeys.Rows(i).Tag, ToolStripButton).Name
                    Case "BtnSelect"
                        MainForm.CMSSelect.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        LDSettings.Keys.ModeSelect = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnMove"
                        MainForm.CMSMove.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        LDSettings.Keys.ModeMove = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnRotate"
                        MainForm.CMSRotate.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        LDSettings.Keys.ModeRotate = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnScale"
                        MainForm.CMSScale.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        LDSettings.Keys.ModeScale = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnAddVertex"
                        MainForm.CMSAddVertex.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        LDSettings.Keys.AddVertex = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnAddTriangle"
                        MainForm.CMSAddTriangle.ShortcutKeyDisplayString = Shortkeys.Rows(i).Cells(1).Value
                        LDSettings.Keys.AddTriangle = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnPreview"
                        LDSettings.Keys.Preview = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnColours"
                        LDSettings.Keys.ShowColours = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnPipette"
                        LDSettings.Keys.Pipette = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                End Select
            ElseIf TypeOf Shortkeys.Rows(i).Tag Is Button Then
                CType(Shortkeys.Rows(i).Tag, Button).Text = Mid(Shortkeys.Rows(i).Tag.Text, 1, Shortkeys.Rows(i).Tag.Text.IndexOf("[") - 1) & " [" & Shortkeys.Rows(i).Cells(1).Value & "]"
                Select Case CType(Shortkeys.Rows(i).Tag, Button).Name
                    Case "BtnAbort"
                        LDSettings.Keys.Abort = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                        ColourForm.LblTip.Text = String.Format(I18N.trl8(I18N.lk.ColourTip), KeyToSet.keyToString(New KeyEventArgs(LDSettings.Keys.Abort)))
                        ColourForm.Refresh()
                    Case "BtnZoom"
                        LDSettings.Keys.Zoom = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnTranslate"
                        LDSettings.Keys.Translate = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                End Select
            ElseIf TypeOf Shortkeys.Rows(i).Tag Is ToolStripDropDownButton Then
                CType(Shortkeys.Rows(i).Tag, ToolStripDropDownButton).ToolTipText = Mid(Shortkeys.Rows(i).Tag.ToolTipText, 1, Shortkeys.Rows(i).Tag.ToolTipText.IndexOf("[") - 1) & " [" & Shortkeys.Rows(i).Cells(1).Value & "]"
                Select Case CType(Shortkeys.Rows(i).Tag, ToolStripDropDownButton).Name
                    Case "BtnCSG"
                        LDSettings.Keys.CSG = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnMerge"
                        LDSettings.Keys.MergeSplit = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                    Case "BtnPrimitives"
                        LDSettings.Keys.AddPrimitive = CType(Shortkeys.Rows(i).Cells(3).Value, Integer)
                End Select
            End If
        Next
        If MainState.objectToModify = Modified.HelperLine Then MainForm.BtnMode.ToolTipText = MainForm.ReferenceLineModeToolStripMenuItem.ToolTipText
        If MainState.objectToModify = Modified.Primitive Then MainForm.BtnMode.ToolTipText = MainForm.PrimitiveModeToolStripMenuItem.ToolTipText
        If MainState.objectToModify = Modified.Triangle Then MainForm.BtnMode.ToolTipText = MainForm.TrianglesModeToolStripMenuItem.ToolTipText
        If MainState.objectToModify = Modified.Vertex Then MainForm.BtnMode.ToolTipText = MainForm.VerticesModeToolStripMenuItem.ToolTipText
    End Sub

    Private Sub addKey(ByVal description As String, ByRef target As Object)
        If TypeOf target Is ToolStripMenuItem Then
            Dim keystring As String = CType(target, ToolStripMenuItem).ShortcutKeyDisplayString
            If keyDict.ContainsKey(keystring) Then
                Dim temp As String = keystring
                Dim i As Integer = 1
                While keyDict.ContainsKey(temp & " (" & i & ")")
                    i += 1
                End While
                keystring = temp & " (" & i & ")"
            End If
            Shortkeys.Rows.Add(description, keystring, I18N.trl8(I18N.lk.SetKey), CType(target, ToolStripMenuItem).ShortcutKeys)
            keyDict.Add(keystring, Shortkeys.Rows.Count - 1)
            Shortkeys.Rows(Shortkeys.Rows.Count - 1).Tag = target
        ElseIf TypeOf target Is ToolStripButton Then
            Dim keyindex As Integer
            Dim keystring As String = Replace(Replace(target.ToolTipText, target.Text & " [", ""), "]", "")
            Select Case CType(target, ToolStripButton).Name
                Case "BtnSelect"
                    keyindex = LDSettings.Keys.ModeSelect
                Case "BtnMove"
                    keyindex = LDSettings.Keys.ModeMove
                Case "BtnRotate"
                    keyindex = LDSettings.Keys.ModeRotate
                Case "BtnScale"
                    keyindex = LDSettings.Keys.ModeScale
                Case "BtnAddVertex"
                    keyindex = LDSettings.Keys.AddVertex
                Case "BtnAddTriangle"
                    keyindex = LDSettings.Keys.AddTriangle
                Case "BtnPreview"
                    keyindex = LDSettings.Keys.Preview
                Case "BtnColours"
                    keyindex = LDSettings.Keys.ShowColours
                Case "BtnPipette"
                    keyindex = LDSettings.Keys.Pipette
            End Select
            If keyDict.ContainsKey(keystring) Then
                Dim temp As String = keystring
                Dim i As Integer = 1
                While keyDict.ContainsKey(temp & " (" & i & ")")
                    i += 1
                End While
                keystring = temp & " (" & i & ")"
            End If
            Shortkeys.Rows.Add(description, keystring, I18N.trl8(I18N.lk.SetKey), keyindex)
            keyDict.Add(keystring, Shortkeys.Rows.Count - 1)
            Shortkeys.Rows(Shortkeys.Rows.Count - 1).Tag = target
        ElseIf TypeOf target Is Button Then
            Dim keyindex As Integer
            Dim keystring As String = Replace(Replace(Mid(target.Text, target.Text.lastIndexOf("[") + 1), "[", ""), "]", "")
            Select Case CType(target, Button).Name
                Case "BtnAbort"
                    keyindex = LDSettings.Keys.Abort
                Case "BtnZoom"
                    keyindex = LDSettings.Keys.Zoom
                Case "BtnTranslate"
                    keyindex = LDSettings.Keys.Translate
            End Select
            If keyDict.ContainsKey(keystring) Then
                Dim temp As String = keystring
                Dim i As Integer = 1
                While keyDict.ContainsKey(temp & " (" & i & ")")
                    i += 1
                End While
                keystring = temp & " (" & i & ")"
            End If
            Shortkeys.Rows.Add(description, keystring, I18N.trl8(I18N.lk.SetKey), keyindex)
            keyDict.Add(keystring, Shortkeys.Rows.Count - 1)
            Shortkeys.Rows(Shortkeys.Rows.Count - 1).Tag = target
        ElseIf TypeOf target Is ToolStripDropDownButton Then
            Dim keyindex As Integer
            Dim keystring As String = Replace(Replace(Mid(target.ToolTipText, target.ToolTipText.lastIndexOf("[") + 1), "[", ""), "]", "")
            Select Case CType(target, ToolStripDropDownButton).Name
                Case "BtnCSG"
                    keyindex = LDSettings.Keys.CSG
                Case "BtnMerge"
                    keyindex = LDSettings.Keys.MergeSplit
                Case "BtnPrimitives"
                    keyindex = LDSettings.Keys.AddPrimitive
            End Select
            If keyDict.ContainsKey(keystring) Then
                Dim temp As String = keystring
                Dim i As Integer = 1
                While keyDict.ContainsKey(temp & " (" & i & ")")
                    i += 1
                End While
                keystring = temp & " (" & i & ")"
            End If
            Shortkeys.Rows.Add(description, keystring, I18N.trl8(I18N.lk.SetKey), keyindex)
            keyDict.Add(keystring, Shortkeys.Rows.Count - 1)
            Shortkeys.Rows(Shortkeys.Rows.Count - 1).Tag = target
        End If
    End Sub

    Private Sub addColour(ByVal description As String, ByRef reference As Object)
        LColours.Items.Add(description)
        LTags.Items.Add(reference)
    End Sub

    Private Sub Shortkeys_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Shortkeys.CellContentClick
        If e.ColumnIndex = 2 AndAlso Shortkeys.Rows(e.RowIndex).Cells(2).Value <> "---" Then
newTry:
            If SetKeyDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim keyString As String
                keyString = keyToString(KeyToSet.keyToSet)
                If keyDict.ContainsKey(keyString) AndAlso (keyDict(keyString) <> e.RowIndex OrElse keyDict(keyString) < 0) Then
                    If keyDict(keyString) = -2 Then
                        If MsgBox(I18N.trl8(I18N.lk.InvalidKey), MsgBoxStyle.RetryCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton2, I18N.trl8(I18N.lk.Info)) = MsgBoxResult.Retry Then GoTo newTry
                    Else
                        If MsgBox(I18N.trl8(I18N.lk.KeyInUse), MsgBoxStyle.RetryCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information + MsgBoxStyle.DefaultButton2, I18N.trl8(I18N.lk.Info)) = MsgBoxResult.Retry Then GoTo newTry
                    End If
                Else
                    If TypeOf Shortkeys.Rows(e.RowIndex).Tag Is ToolStripMenuItem Then
                        Try
                            dummyToolStripMenuItem.ShortcutKeys = KeyToSet.keyToSet.KeyData
                        Catch ex As Exception
                            If MsgBox(I18N.trl8(I18N.lk.InvalidKey), MsgBoxStyle.RetryCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton2, I18N.trl8(I18N.lk.Info)) = MsgBoxResult.Retry Then GoTo newTry
                            Exit Sub
                        End Try
                    End If
                    keyDict.Remove(Shortkeys.Rows(e.RowIndex).Cells(1).Value & "")
                    keyDict.Add(keyString, e.RowIndex)
                    Shortkeys.Rows(e.RowIndex).Cells(1).Value = keyString
                    If keyString <> "Ctrl" AndAlso keyString <> "Alt" AndAlso keyString <> "Shift" Then
                        Shortkeys.Rows(e.RowIndex).Cells(3).Value = KeyToSet.keyToSet.KeyData
                    Else
                        Select Case keyString
                            Case "Ctrl" : Shortkeys.Rows(e.RowIndex).Cells(3).Value = Keys.ControlKey
                            Case "Alt" : Shortkeys.Rows(e.RowIndex).Cells(3).Value = Keys.Menu
                            Case "Shift" : Shortkeys.Rows(e.RowIndex).Cells(3).Value = Keys.ShiftKey
                        End Select
                    End If
                End If
                refreshBlacklist()
                Me.Refresh()
            End If
        End If
    End Sub

    Private Function keyToString(ByVal key As System.Windows.Forms.KeyEventArgs) As String
        Dim keyString As String = ""
        If key.Control Then
            keyString += "Ctrl+"
        End If
        If key.Shift Then
            keyString += "Shift+"
        End If
        If key.Alt Then
            keyString += "Alt+"
        End If
        keyString += key.KeyCode.ToString
        keyString = Replace(keyString, "D0", "0")
        keyString = Replace(keyString, "D1", "1")
        keyString = Replace(keyString, "D2", "2")
        keyString = Replace(keyString, "D3", "3")
        keyString = Replace(keyString, "D4", "4")
        keyString = Replace(keyString, "D5", "5")
        keyString = Replace(keyString, "D6", "6")
        keyString = Replace(keyString, "D7", "7")
        keyString = Replace(keyString, "D8", "8")
        keyString = Replace(keyString, "D9", "9")
        keyString = Replace(keyString, "Delete", "Del")
        keyString = Replace(keyString, "+ControlKey", "")
        keyString = Replace(keyString, "+ShiftKey", "")
        keyString = Replace(keyString, "+Menu", "")
        keyString = Replace(keyString, "Escape", "Esc")
        Return keyString
    End Function

    Private Sub BtnRestoreDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRestoreDefaults.Click
        Select Case TCOptions.SelectedIndex
            Case 0
                TBRealName.Text = ""
                TBUserName.Text = ""
                CBLicense.Text = ""
                NUDMaxUndo.Value = 30
                CBFullscreen.Checked = True
                CBPerformaceMode.Checked = False
                CBViewImage.Checked = True
                CBViewPreferences.Checked = True
                CBAlternativeZoomAndTrans.Checked = False
                CBTemplateLinesTop.Checked = False
            Case 1
                Dim tRow As Byte = 1
                loadKeys()
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+N"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.N : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+O"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.O : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+S"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.S : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "1"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.D1 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "2"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.D2 : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "3"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.D3 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "4"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.D4 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "5"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.D5 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "6"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.D6 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "F2"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F2 : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "F3"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F3 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "F4"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F4 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "F5"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F5 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "F6"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F6 : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "F7"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F7 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "F8"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F8 : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "F9"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.F9 : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "P"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.P : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "Space"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Space : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "O"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.O : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Esc"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Escape : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "K"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.K : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "L"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.L : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+R"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.R : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+A"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.A : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+Alt+C"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.Alt Or Keys.C : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Alt+C"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Alt Or Keys.C : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Alt+T"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Alt Or Keys.T : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "Alt+S"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Alt Or Keys.S : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+W"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.W : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+E"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.E : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+G"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.G : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+L"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.L : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+B"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.B : tRow += 1

                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+T"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.T : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+Q"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.Q : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Ctrl+P"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Control Or Keys.P : tRow += 1
                Shortkeys.Rows(tRow).Cells(1).Value = "Alt+V"
                Shortkeys.Rows(tRow).Cells(3).Value = Keys.Alt Or Keys.V : tRow += 1

                refreshBlacklist()
            Case 2
                LTags.Items(0) = Color.FromArgb(0, 0, 0)
                PBack.BackColor = Color.FromArgb(0, 0, 0)
                LTags.Items(1) = New Pen(Color.FromArgb(234, 244, 234), 0.001F)
                LTags.Items(2) = New Pen(Color.FromArgb(12, 12, 12), 0.001F)
                LTags.Items(3) = New Pen(Color.FromArgb(245, 12, 12), 0.001F)
                LTags.Items(4) = New Pen(Color.FromArgb(245, 180, 180), 0.001F)
                LTags.Items(5) = New SolidBrush(Color.FromArgb(245, 245, 12))
                LTags.Items(6) = New SolidBrush(Color.FromArgb(245, 12, 12))
                LTags.Items(7) = New Pen(Color.FromArgb(140, 215, 140), 0.001F)
                LTags.Items(8) = New Pen(Color.FromArgb(0, 60, 0), 0.001F)
                LTags.Items(9) = New Pen(Color.FromArgb(60, 60, 105), 0.001F)
                LTags.Items(10) = New Pen(Color.FromArgb(245, 245, 220), 0.001F)
                LTags.Items(11) = New SolidBrush(Color.FromArgb(120, 0, 0))
                If LColours.SelectedIndex = 0 Then LColours.SelectedIndex = 1
                LColours.SelectedIndex = 0
        End Select
    End Sub

    Private Sub refreshBlacklist()
        keyDict.Clear()
        keyDict.Add("Add", -1)
        keyDict.Add("Subtract", -1)
        keyDict.Add("Oemplus", -1)
        keyDict.Add("OemMinus", -1)
        keyDict.Add("NumPad0", -1)
        keyDict.Add("NumPad1", -1)
        keyDict.Add("NumPad2", -1)
        keyDict.Add("NumPad3", -1)
        keyDict.Add("NumPad4", -1)
        keyDict.Add("NumPad5", -1)
        keyDict.Add("NumPad6", -1)
        keyDict.Add("NumPad7", -1)
        keyDict.Add("NumPad8", -1)
        keyDict.Add("NumPad9", -1)
        keyDict.Add("Del", -1)
        keyDict.Add("Ctrl+X", -1)
        keyDict.Add("Ctrl+C", -1)
        keyDict.Add("Ctrl+V", -1)
        keyDict.Add("Ctrl+Z", -1)
        keyDict.Add("Ctrl+Y", -1)
        keyDict.Add("Ctrl+Q", -1)
        keyDict.Add("Left", -1)
        keyDict.Add("Right", -1)
        keyDict.Add("Up", -1)
        keyDict.Add("Down", -1)
        keyDict.Add("Ctrl+Alt", -2)
        keyDict.Add("Ctrl+Shift", -2)
        keyDict.Add("Shift+Alt", -2)
        keyDict.Add("Ctrl+Shift+Alt", -2)
        For i As Integer = 1 To Shortkeys.Rows.Count - 1
            Dim keystring As String = keyToString(New KeyEventArgs(Shortkeys.Rows(i).Cells(3).Value))
            If keyDict.ContainsKey(keystring) Then
                Dim temp As String = keystring
                Dim j As Integer = 1
                While keyDict.ContainsKey(temp & " (" & j & ")")
                    j += 1
                End While
                keystring = temp & " (" & j & ")"
            End If
            keyDict.Add(keystring, i)
            Shortkeys.Rows(i).Cells(1).Value = keystring
        Next i
    End Sub

    Private Sub CBAlternativeZoomAndTrans_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBAlternativeZoomAndTrans.CheckedChanged
        If Shortkeys.Rows.Count > 0 Then
            If CBAlternativeZoomAndTrans.Checked Then
                Shortkeys.Rows(0).Cells(0).Value = I18N.trl8(I18N.lk.Context1)
            Else
                Shortkeys.Rows(0).Cells(0).Value = I18N.trl8(I18N.lk.Context2)
            End If
        End If
    End Sub

    Dim active_channel As Byte

    Private Sub HR_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles HR.MouseEnter
        active_channel = 0
        PGradient.Refresh()
    End Sub

    Private Sub HR_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HR.Scroll
        Try
            PNew.BackColor = Color.FromArgb(HA.Value, HR.Value, HG.Value, HB.Value)
        Catch
            HR.Value = MathHelper.clip(HR.Value, 0, 255)
            HG.Value = MathHelper.clip(HG.Value, 0, 255)
            HB.Value = MathHelper.clip(HB.Value, 0, 255)
            HA.Value = MathHelper.clip(HA.Value, 0, 255)
        End Try
    End Sub

    Private Sub HG_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles HG.MouseEnter
        active_channel = 1
        PGradient.Refresh()
    End Sub

    Private Sub HG_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HG.Scroll
        Try
            PNew.BackColor = Color.FromArgb(HA.Value, HR.Value, HG.Value, HB.Value)
        Catch
            HR.Value = MathHelper.clip(HR.Value, 0, 255)
            HG.Value = MathHelper.clip(HG.Value, 0, 255)
            HB.Value = MathHelper.clip(HB.Value, 0, 255)
            HA.Value = MathHelper.clip(HA.Value, 0, 255)
        End Try
    End Sub

    Private Sub HB_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles HB.MouseEnter
        active_channel = 2
        PGradient.Refresh()
    End Sub

    Private Sub HB_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HB.Scroll
        Try
            PNew.BackColor = Color.FromArgb(HA.Value, HR.Value, HG.Value, HB.Value)
        Catch
            HR.Value = MathHelper.clip(HR.Value, 0, 255)
            HG.Value = MathHelper.clip(HG.Value, 0, 255)
            HB.Value = MathHelper.clip(HB.Value, 0, 255)
            HA.Value = MathHelper.clip(HA.Value, 0, 255)
        End Try
    End Sub

    Private Sub HA_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles HA.MouseEnter
        active_channel = 3
        PGradient.Refresh()
    End Sub

    Private Sub HA_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HA.Scroll
        Try
            PNew.BackColor = Color.FromArgb(HA.Value, HR.Value, HG.Value, HB.Value)
        Catch
            HR.Value = MathHelper.clip(HR.Value, 0, 255)
            HG.Value = MathHelper.clip(HG.Value, 0, 255)
            HB.Value = MathHelper.clip(HB.Value, 0, 255)
            HA.Value = MathHelper.clip(HA.Value, 0, 255)
        End Try
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        If LColours.SelectedIndex > -1 Then
            If TypeOf LTags.Items(LColours.SelectedIndex) Is Pen Then
                LTags.Items(LColours.SelectedIndex) = New Pen(Color.FromArgb(MathHelper.clip(HA.Value, 0, 255), MathHelper.clip(HR.Value, 0, 255), MathHelper.clip(HG.Value, 0, 255), MathHelper.clip(HB.Value, 0, 255)), 0.001F)
            ElseIf TypeOf LTags.Items(LColours.SelectedIndex) Is SolidBrush Then
                LTags.Items(LColours.SelectedIndex) = New SolidBrush(Color.FromArgb(MathHelper.clip(HA.Value, 0, 255), MathHelper.clip(HR.Value, 0, 255), MathHelper.clip(HG.Value, 0, 255), MathHelper.clip(HB.Value, 0, 255)))
            Else
                LTags.Items(LColours.SelectedIndex) = Color.FromArgb(MathHelper.clip(HA.Value, 0, 255), MathHelper.clip(HR.Value, 0, 255), MathHelper.clip(HG.Value, 0, 255), MathHelper.clip(HB.Value, 0, 255))
                PBack.BackColor = LTags.Items(LColours.SelectedIndex)
            End If
            POld.BackColor = Color.FromArgb(MathHelper.clip(HA.Value, 0, 255), MathHelper.clip(HR.Value, 0, 255), MathHelper.clip(HG.Value, 0, 255), MathHelper.clip(HB.Value, 0, 255))
        End If
    End Sub

    Private Sub LColours_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LColours.SelectedIndexChanged
        If LColours.SelectedIndex > -1 Then
            If TypeOf LTags.Items(LColours.SelectedIndex) Is Pen Then
                POld.BackColor = CType(LTags.Items(LColours.SelectedIndex), Pen).Color
            ElseIf TypeOf LTags.Items(LColours.SelectedIndex) Is SolidBrush Then
                POld.BackColor = CType(LTags.Items(LColours.SelectedIndex), SolidBrush).Color
            Else
                POld.BackColor = CType(LTags.Items(LColours.SelectedIndex), Color)
            End If
            PNew.BackColor = POld.BackColor
            HR.Value = POld.BackColor.R
            HG.Value = POld.BackColor.G
            HB.Value = POld.BackColor.B
            HA.Value = POld.BackColor.A
            PGradient.Refresh()
            If LColours.SelectedIndex = 0 Then HA.Minimum = 200 Else HA.Minimum = 0
        End If
    End Sub

    Private Sub PGradient_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PGradient.Paint
        Select Case active_channel
            Case 0
                For x As Single = 0 To 200 Step 1.275
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(MathHelper.clip(HA.Value, 0, 255), x * 1.275, MathHelper.clip(HG.Value, 0, 255), MathHelper.clip(HB.Value, 0, 255))), x, 0.0F, 1.275F, 20.0F)
                Next
            Case 1
                For x As Single = 0 To 200 Step 1.275
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(MathHelper.clip(HA.Value, 0, 255), MathHelper.clip(HR.Value, 0, 255), x * 1.275, MathHelper.clip(HB.Value, 0, 255))), x, 0.0F, 1.275F, 20.0F)
                Next
            Case 2
                For x As Single = 0 To 200 Step 1.275
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(MathHelper.clip(HA.Value, 0, 255), MathHelper.clip(HR.Value, 0, 255), MathHelper.clip(HG.Value, 0, 255), x * 1.275)), x, 0.0F, 1.275F, 20.0F)
                Next
            Case 3
                For x As Single = 0 To 200 Step 1.275
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(x * 1.275, MathHelper.clip(HR.Value, 0, 255), MathHelper.clip(HG.Value, 0, 255), MathHelper.clip(HB.Value, 0, 255))), x, 0.0F, 1.275F, 20.0F)
                Next
        End Select
    End Sub

    Private Sub TCOptions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TCOptions.SelectedIndexChanged
        If TCOptions.SelectedIndex = 2 Then
            If LColours.SelectedIndex = -1 Then
                LColours.SelectedIndex = 0
            End If
        End If
    End Sub

    Dim cr As Integer
    Dim cg As Integer
    Dim cb As Integer
    Dim ca As Integer
    Private Sub BtnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCopy.Click
        cr = MathHelper.clip(HR.Value, 0, 255)
        cg = MathHelper.clip(HG.Value, 0, 255)
        cb = MathHelper.clip(HB.Value, 0, 255)
        ca = MathHelper.clip(HA.Value, 0, 255)
    End Sub


    Private Sub BtnPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPaste.Click
        HR.Value = cr
        HG.Value = cg
        HB.Value = cb
        HA.Value = ca
        PNew.BackColor = Color.FromArgb(ca, cr, cg, cb)
        PGradient.Refresh()
    End Sub

End Class
