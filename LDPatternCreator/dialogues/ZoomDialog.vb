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

Public Class ZoomDialog

    Public Shared zoomfactor As Double
    Public Shared ul_corner(1) As Double
    Public Shared lr_corner(1) As Double
    Public Shared newOffset(1) As Double
    Private oldZF As Double = 1
    Private oldul_corner(1) As Double
    Private oldlr_corner(1) As Double
    Private oldOffset(1) As Double
    Private lockUL As Boolean = False
    Private lockLR As Boolean = True

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        View.zoomfactor = zoomfactor
        View.offsetX = (ul_corner(0) + lr_corner(0)) / 2
        View.offsetY = -(ul_corner(1) + lr_corner(1)) / 2
        If Not (NUDZoom.Focused OrElse NUDLRx.Focused OrElse NUDLRy.Focused OrElse NUDULx.Focused OrElse NUDULy.Focused) Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NUDZoom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDZoom.ValueChanged
        zoomfactor = NUDZoom.Value / 100
        lockLR = True
        lockUL = True
        NUDULx.Value = ul_corner(0) / 1000 * (oldZF / zoomfactor) * View.unitFactor
        NUDULy.Value = ul_corner(1) / 1000 * (oldZF / zoomfactor) * View.unitFactor

        NUDLRx.Value = lr_corner(0) / 1000 * (oldZF / zoomfactor) * View.unitFactor
        NUDLRy.Value = lr_corner(1) / 1000 * (oldZF / zoomfactor) * View.unitFactor
        lockLR = False
        lockUL = False
    End Sub

    Private Sub ZoomDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ZoomDialog.ul_corner(0) = MainForm.getXcoordinate(0)
        ZoomDialog.ul_corner(1) = MainForm.getYcoordinate(0)
        ZoomDialog.lr_corner(0) = MainForm.getXcoordinate(MainForm.ClientSize.Width)
        ZoomDialog.lr_corner(1) = MainForm.getYcoordinate(MainForm.ClientSize.Height)
        lockLR = True
        lockUL = True
        NUDULx.Value = Math.Round(ul_corner(0) / 1000 * View.unitFactor, 4)
        NUDULy.Value = Math.Round(ul_corner(1) / 1000 * View.unitFactor, 4)

        NUDLRx.Value = Math.Round(lr_corner(0) / 1000 * View.unitFactor, 4)
        NUDLRy.Value = Math.Round(lr_corner(1) / 1000 * View.unitFactor, 4)
        lockLR = False
        lockUL = False
        zoomfactor = View.zoomfactor
        oldZF = zoomfactor
        newOffset(0) = View.offsetX
        newOffset(1) = View.offsetY
        oldlr_corner = lr_corner.Clone
        oldul_corner = ul_corner.Clone
        oldOffset = newOffset.Clone
        Me.NUDZoom.Value = zoomfactor * 100
        Me.NUDZoom.Focus()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.NUDZoom.Value = zoomfactor * 100
    End Sub

    Private Sub NUDULx_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDULx.ValueChanged
        If Not lockUL Then
            lockLR = True
            Try
                ul_corner(0) = CType(NUDULx.Value, Double) * 1000 / View.unitFactor
                NUDLRx.Value = ul_corner(0) / 1000 - MainForm.ClientSize.Width / 1000 * View.unitFactor / zoomfactor
                lr_corner(0) = ul_corner(0) - MainForm.ClientSize.Width * View.unitFactor / zoomfactor
                sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            Catch ex As Exception
                sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            End Try
            lockLR = False
        End If
    End Sub

    Private Sub NUDULy_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDULy.ValueChanged
        If Not lockUL Then
            lockLR = True
            Try
                ul_corner(1) = CType(NUDULy.Value, Double) * 1000 / View.unitFactor
                NUDLRy.Value = ul_corner(1) / 1000 + MainForm.ClientSize.Height / 1000 * View.unitFactor / zoomfactor
                lr_corner(1) = ul_corner(1) + MainForm.ClientSize.Height * View.unitFactor / zoomfactor
                sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            Catch ex As Exception
                sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            End Try
            lockLR = False
        End If
    End Sub

    Private Sub NUDLRx_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDLRx.ValueChanged
        If Not lockLR Then
            lockUL = True
            Try
                lr_corner(0) = CType(NUDLRx.Value, Double) * 1000 / View.unitFactor
                NUDULx.Value = lr_corner(0) / 1000 + MainForm.ClientSize.Width / 1000 * View.unitFactor / zoomfactor
                ul_corner(0) = lr_corner(0) + MainForm.ClientSize.Width * View.unitFactor / zoomfactor
                sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            Catch ex As Exception
                sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            End Try
            lockUL = False
        End If
    End Sub

    Private Sub NUDLRy_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDLRy.ValueChanged
        If Not lockLR Then
            lockUL = True
            Try
                lr_corner(1) = CType(NUDLRy.Value, Double) * 1000 / View.unitFactor
                NUDULy.Value = lr_corner(1) / 1000 - MainForm.ClientSize.Height / 1000 * View.unitFactor / zoomfactor
                ul_corner(1) = lr_corner(1) - MainForm.ClientSize.Height * View.unitFactor / zoomfactor
                sender.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
            Catch ex As Exception
                sender.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            End Try
            lockUL = False
        End If
    End Sub
End Class
