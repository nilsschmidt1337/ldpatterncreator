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

Imports System.Windows.Forms

Public Class ChooseTemplateDialog

    Public selectedText As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        For Each line As String In LBDelete.Items
            If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "template\" & line & ".txt") Then
                My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "template\" & line & ".txt", FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                For d As Integer = 0 To MainForm.TemplateToolStripMenuItem.DropDownItems.Count - 1
                    Dim item As ToolStripItem = MainForm.TemplateToolStripMenuItem.DropDownItems.Item(d)
                    If item.Text = line Then
                        MainForm.TemplateToolStripMenuItem.DropDownItems.RemoveAt(d)
                        Exit For
                    End If
                Next
            End If
        Next
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ChooseTemplateDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LBTemplates.Items.Clear()
        LBDelete.Items.Clear()
        For i = 4 To MainForm.TemplateToolStripMenuItem.DropDownItems.Count - 1
            LBTemplates.Items.Add(MainForm.TemplateToolStripMenuItem.DropDownItems(i).Text)
        Next i
    End Sub

    Private Sub Delete_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete_Button.Click
        If Not LBTemplates.SelectedItem Is Nothing Then
            LBDelete.Items.Add(LBTemplates.SelectedItem)
            LBTemplates.Items.RemoveAt(LBTemplates.SelectedIndex)
        End If
    End Sub

    Private Sub Edit_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Edit_Button.Click
        If Not LBTemplates.SelectedItem Is Nothing Then
            selectedText = LBTemplates.SelectedItem.ToString
            If TemplateEditor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                LBTemplates.Items(LBTemplates.SelectedIndex) = selectedText
            End If
        End If
    End Sub
End Class
