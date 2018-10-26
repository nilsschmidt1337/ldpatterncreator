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
Public Class KeyToSet
    Public Shared keyToSet As System.Windows.Forms.KeyEventArgs
    Public Shared Function keyToString(ByVal key As System.Windows.Forms.KeyEventArgs) As String
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
        keyString = Replace(keyString, "+ControlKey", "")
        keyString = Replace(keyString, "+ShiftKey", "")
        keyString = Replace(keyString, "+Menu", "")
        keyString = Replace(keyString, "Delete", "Del")
        keyString = Replace(keyString, "Escape", "Esc")
        Return keyString
    End Function

    Public Shared Sub setKey(ByRef target As Object, ByVal keyindex As Integer)
        Dim keystring As String = keyToString(New System.Windows.Forms.KeyEventArgs(keyindex))
        If TypeOf target Is ToolStripMenuItem Then
            Try
                Select Case CType(target, ToolStripMenuItem).Name
                    Case "ImageToolStripMenuItem"
                        ImageForm.Text = I18N.trl8(I18N.lk.ImageTitle) & " [" & keystring & "]:"
                    Case "ViewPrefsToolStripMenuItem"
                        PreferencesForm.Text = Replace(I18N.trl8(I18N.lk.ViewPreferences), "&", "") & " [" & keystring & "]:"
                    Case "VerticesModeToolStripMenuItem"
                        MainForm.CMSVertex.ShortcutKeyDisplayString = keystring
                        MainForm.VerticesModeToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.VertexMode) & " [" & keystring & "]"
                    Case "TrianglesModeToolStripMenuItem"
                        MainForm.CMSTriangle.ShortcutKeyDisplayString = keystring
                        MainForm.TrianglesModeToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.TriangleMode) & " [" & keystring & "]"
                    Case "PrimitiveModeToolStripMenuItem"
                        MainForm.CMSPrimitive.ShortcutKeyDisplayString = keystring
                        MainForm.PrimitiveModeToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.PrimitiveMode) & " [" & keystring & "]"
                End Select
            Catch
                Select Case CType(target, ToolStripMenuItem).Name
                    Case "ImageToolStripMenuItem"
                        ImageForm.Text = I18N.trl8(I18N.lk.ImageTitle) & " [" & keystring & "]:"
                    Case "SnappingToolStripMenuItem"
                        PreferencesForm.Text = Replace(I18N.trl8(I18N.lk.ViewPreferences), "&", "") & " [" & keystring & "]:"
                    Case "VerticesModeToolStripMenuItem"
                        MainForm.CMSVertex.ShortcutKeyDisplayString = keystring
                    Case "TrianglesModeToolStripMenuItem"
                        MainForm.CMSTriangle.ShortcutKeyDisplayString = keystring
                    Case "PrimitiveModeToolStripMenuItem"
                        MainForm.CMSPrimitive.ShortcutKeyDisplayString = keystring
                End Select
            End Try
            CType(target, ToolStripMenuItem).ShortcutKeys = keyindex : CType(target, ToolStripMenuItem).ShortcutKeyDisplayString = keystring
        ElseIf TypeOf target Is ToolStripButton Then
            CType(target, ToolStripButton).ToolTipText = target.Text & " [" & keystring & "]"
            Select Case CType(target, ToolStripButton).Name
                Case "BtnSelect"
                    MainForm.CMSSelect.ShortcutKeyDisplayString = keystring
                    LDSettings.Keys.ModeSelect = keyindex
                Case "BtnMove"
                    MainForm.CMSMove.ShortcutKeyDisplayString = keystring
                    LDSettings.Keys.ModeMove = keyindex
                Case "BtnRotate"
                    MainForm.CMSRotate.ShortcutKeyDisplayString = keystring
                    LDSettings.Keys.ModeRotate = keyindex
                Case "BtnScale"
                    MainForm.CMSScale.ShortcutKeyDisplayString = keystring
                    LDSettings.Keys.ModeScale = keyindex
                Case "BtnAddVertex"
                    MainForm.CMSAddVertex.ShortcutKeyDisplayString = keystring
                    LDSettings.Keys.AddVertex = keyindex
                Case "BtnAddTriangle"
                    MainForm.CMSAddTriangle.ShortcutKeyDisplayString = keystring
                    LDSettings.Keys.AddTriangle = keyindex
                Case "BtnPreview"
                    LDSettings.Keys.Preview = keyindex
                Case "BtnColours"
                    LDSettings.Keys.ShowColours = keyindex
                Case "BtnPipette"
                    LDSettings.Keys.Pipette = keyindex
            End Select
        ElseIf TypeOf target Is Button Then
            CType(target, Button).Text = Mid(target.Text, 1, target.Text.IndexOf("[") - 1) & " [" & keystring & "]"
            Select Case CType(target, Button).Name
                Case "BtnAbort"
                    LDSettings.Keys.Abort = keyindex
                    ColourForm.LblTip.Text = String.Format(I18N.trl8(I18N.lk.ColourTip), keyToString(New KeyEventArgs(LDSettings.Keys.Abort)))
                    ColourForm.Refresh()
                Case "BtnZoom"
                    LDSettings.Keys.Zoom = keyindex
                Case "BtnTranslate"
                    LDSettings.Keys.Translate = keyindex
            End Select
        ElseIf TypeOf target Is ToolStripDropDownButton Then
            If Not CType(target.ToolTipText, String).Contains("[") Then target.ToolTipText = target.ToolTipText & " []"
            CType(target, ToolStripDropDownButton).ToolTipText = Mid(target.ToolTipText, 1, target.ToolTipText.IndexOf("[") - 1) & " [" & keystring & "]"
            Select Case CType(target, ToolStripDropDownButton).Name
                Case "BtnCSG"
                    LDSettings.Keys.CSG = keyindex
                Case "BtnPrimitives"
                    LDSettings.Keys.AddPrimitive = keyindex
                Case "BtnMerge"
                    LDSettings.Keys.MergeSplit = keyindex
            End Select
        End If
    End Sub
End Class
