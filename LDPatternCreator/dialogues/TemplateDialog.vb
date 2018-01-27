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
Imports System.IO
Public Class TemplateEditor

    Dim errors As Integer
    Dim m(3, 3) As Double

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not RTBPoly.Focused AndAlso Not RTBData.Focused Then
            Dim lineNumber As Integer = 0
            Try
                If oldName = "" Then
                    oldName = TBTitle.Text
                ElseIf oldName <> TBTitle.Text Then
                    My.Computer.FileSystem.RenameFile(EnvironmentPaths.templatePath & oldName & ".txt", TBTitle.Text & ".txt")
                    ChooseTemplateDialog.selectedText = TBTitle.Text
                    For Each item As Object In MainForm.TemplateToolStripMenuItem.DropDownItems
                        Dim menuItem As ToolStripMenuItem = TryCast(item, ToolStripMenuItem)
                        If Not menuItem Is Nothing AndAlso item.Text = oldName Then
                            item.Text = TBTitle.Text
                        End If
                    Next
                End If
                Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.templatePath & TBTitle.Text & ".txt", False, New System.Text.UTF8Encoding(False))
                    lineNumber += 1
                    DateiOut.WriteLine(Mid(CBProjectMode.Text, 1, 1))
                    lineNumber += 1
                    DateiOut.WriteLine(Replace(RTBData.Text, vbLf, "<br>"))
                    For r As Integer = 0 To 3
                        lineNumber += 1
                        If r = 0 Then DateiOut.WriteLine(Replace(M00.Text, MathHelper.comma, ".") & " " & Replace(M01.Text, MathHelper.comma, ".") & " " & Replace(M02.Text, MathHelper.comma, ".") & " " & Replace(M03.Text, MathHelper.comma, "."))
                        If r = 1 Then DateiOut.WriteLine(Replace(M10.Text, MathHelper.comma, ".") & " " & Replace(M11.Text, MathHelper.comma, ".") & " " & Replace(M12.Text, MathHelper.comma, ".") & " " & Replace(M13.Text, MathHelper.comma, "."))
                        If r = 2 Then DateiOut.WriteLine(Replace(M20.Text, MathHelper.comma, ".") & " " & Replace(M21.Text, MathHelper.comma, ".") & " " & Replace(M22.Text, MathHelper.comma, ".") & " " & Replace(M23.Text, MathHelper.comma, "."))
                    Next
                    lineNumber += 1
                    DateiOut.WriteLine("0 0 0 1")
                    For Each line As String In RTBPoly.Lines
                        lineNumber += 1
                        If line <> "" Then
                            DateiOut.WriteLine(line)
                        End If
                    Next
                End Using
            Catch ex As Exception
                MsgBox(String.Format(I18N.trl8(I18N.lk.ParsingErrorDescription), ex.Message & vbCrLf, lineNumber), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Critical, I18N.trl8(I18N.lk.ParsingError))
            End Try
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            ToolTip1.Show(I18N.trl8(I18N.lk.CtrlEnterHint), Me.Controls("GBData").Controls("RTBData"))
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

#Region "Matrix"
    Private Sub M00_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M00.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(0, 0) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M01_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M01.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(0, 1) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M02_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M02.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(0, 2) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M03_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M03.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(0, 3) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M10_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M10.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(1, 0) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M11.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(1, 1) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M12.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(1, 2) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M13.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(1, 3) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M20_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M20.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(2, 0) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M21_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M21.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(2, 1) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M22_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M22.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(2, 2) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub

    Private Sub M23_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M23.TextChanged
        Try
            If sender.Text = "" Then Throw New Exception
            m(2, 3) = CType(Replace(sender.Text, ".", ","), Double)
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.ControlText) Then
                If errors > 0 Then errors -= 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        Catch
            If sender.ForeColor <> Color.FromKnownColor(KnownColor.HotTrack) Then
                errors += 1
            End If
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            OK_Button.Enabled = False
        End Try
    End Sub
#End Region

    Private Function g(ByVal s As String) As String
        Return Replace(s, ".", MathHelper.comma)
    End Function


    Public oldName As String
    Private Sub TemplateEditor_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        oldName = ChooseTemplateDialog.selectedText
        If ChooseTemplateDialog.selectedText = "" Then
            M00.Text = "1" : M01.Text = "0" : M02.Text = "0" : M03.Text = "0"
            M10.Text = "0" : M11.Text = "1" : M12.Text = "0" : M13.Text = "0"
            M20.Text = "0" : M21.Text = "0" : M22.Text = "1" : M23.Text = "0"
            CBProjectMode.Text = I18N.trl8(I18N.lk.Right)
            TBTitle.Text = ""
            RTBData.Text = ""
            RTBPoly.Text = ""
            errors = 0
            BtnExample.Visible = True
            OK_Button.Enabled = False
        Else
            Dim lineNumber As Integer = 0
            errors = 0
            TBTitle.Text = oldName
            Try
                Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.templatePath & ChooseTemplateDialog.selectedText & ".txt", New System.Text.UTF8Encoding(False))
                    lineNumber += 1
                    Select Case CType(DateiIn.ReadLine(), Byte)
                        Case "0" : CBProjectMode.Text = I18N.trl8(I18N.lk.Right)
                        Case "1" : CBProjectMode.Text = I18N.trl8(I18N.lk.Top)
                        Case "2" : CBProjectMode.Text = I18N.trl8(I18N.lk.Front)
                        Case "3" : CBProjectMode.Text = I18N.trl8(I18N.lk.Left)
                        Case "4" : CBProjectMode.Text = I18N.trl8(I18N.lk.Bottom)
                        Case "5" : CBProjectMode.Text = I18N.trl8(I18N.lk.Back)
                    End Select
                    lineNumber += 1
                    RTBData.Text = Replace(DateiIn.ReadLine(), "<br>", vbLf)
                    For r As Integer = 0 To 3
                        Dim rm() As String = DateiIn.ReadLine().Split(" ")
                        If r = 0 Then M00.Text = g(rm(0)) : M01.Text = g(rm(1)) : M02.Text = g(rm(2)) : M03.Text = g(rm(3))
                        If r = 1 Then M10.Text = g(rm(0)) : M11.Text = g(rm(1)) : M12.Text = g(rm(2)) : M13.Text = g(rm(3))
                        If r = 2 Then M20.Text = g(rm(0)) : M21.Text = g(rm(1)) : M22.Text = g(rm(2)) : M23.Text = g(rm(3))
                    Next
                    RTBPoly.Text = ""
                    Do
                        lineNumber += 1
                        RTBPoly.Text += DateiIn.ReadLine & vbLf
                    Loop Until DateiIn.EndOfStream
                    RTBPoly.Text = Mid(RTBPoly.Text, 1, RTBPoly.Text.LastIndexOf(vbLf))
                End Using
            Catch ex As Exception
                MsgBox(String.Format(I18N.trl8(I18N.lk.ParsingErrorDescription), ex.Message & vbCrLf, lineNumber), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Critical, I18N.trl8(I18N.lk.ParsingError))
                OK_Button.Enabled = False
            End Try
            BtnExample.Visible = False
        End If
    End Sub

    Private Function checkFilename(ByVal filename As String) As Boolean
        For d As Integer = 0 To MainForm.TemplateToolStripMenuItem.DropDownItems.Count - 1
            Dim item As ToolStripItem = MainForm.TemplateToolStripMenuItem.DropDownItems.Item(d)
            If item.Text.ToLower = filename.ToLower AndAlso oldName.ToLower <> filename.ToLower Then
                Return False
            End If
        Next
        Dim ca() As Char = System.IO.Path.GetInvalidFileNameChars
        For i As Integer = 0 To filename.Length - 1
            Dim c As Char = filename(i)
            For j = 0 To ca.Length - 1
                If c = ca(j) OrElse c = "&" Then
                    Return False
                End If
            Next
        Next
        Return filename <> ""
    End Function

    Private Sub BtnExample_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExample.Click
        TBTitle.Text = ""
        CBProjectMode.Text = I18N.trl8(I18N.lk.Left)
        RTBData.Text = "0 // Subfile" & vbCrLf & "1 16 0 0 0 1 0 0 0 1 0 0 0 1 s\2525s01.dat"
        M00.Text = "1"
        M01.Text = "0"
        M02.Text = "0"
        M03.Text = "0"

        M10.Text = "0"
        M11.Text = "1"
        M12.Text = "0"
        M13.Text = "0"

        M20.Text = "0"
        M21.Text = "0"
        M22.Text = "1"
        M23.Text = "0"
        RTBPoly.Text = "-60 - 48" & vbCrLf & _
                       "60 -48" & vbCrLf & _
                       "60 48" & vbCrLf & _
                       "-60 48" & vbCrLf & _
                       "CUT" & vbCrLf & _
                       "-60 -48" & vbCrLf & _
                       "-60 -144" & vbCrLf & _
                       "60 -144" & vbCrLf & _
                       "60 -48" & vbCrLf & _
                       "{60 48; 60 -48; -60 -48; -60 48} {2 96 10; 2 0 10; 2 0 130; 2 96 130}" & vbCrLf & _
                       "{60 -48; 60 -144; -60 -144; -60 -48} {-2 96 130; -2 0 130; -2 0 10; -2 96 10}" & vbCrLf & _
                       "0 // -70 0 Front of 2525" & vbCrLf & _
                       "0 // -70 -96 Back of 2525"
    End Sub

    Private Sub TBTitle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBTitle.TextChanged
        OK_Button.Enabled = errors = 0 AndAlso checkFilename(TBTitle.Text)
        If checkFilename(TBTitle.Text) Then
            sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
        Else
            sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
        End If
    End Sub
End Class
