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
Public Class MouseHelper

    Declare Auto Sub mouse_event Lib "user32" (ByVal dwFlags As Int32, ByVal dx As Int32, ByVal dy As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As IntPtr)

    Const MOUSEEVENTF_MOVE As Int32 = &H1 '  mouse move
    Const MOUSEEVENTF_LEFTDOWN As Int32 = &H2 '  left button down
    Const MOUSEEVENTF_LEFTUP As Int32 = &H4 '  left button up
    Const MOUSEEVENTF_RIGHTDOWN As Int32 = &H8 '  right button down
    Const MOUSEEVENTF_RIGHTUP As Int32 = &H10 '  right button up
    Const MOUSEEVENTF_MIDDLEDOWN As Int32 = &H20 '  middle button down
    Const MOUSEEVENTF_MIDDLEUP As Int32 = &H40 '  middle button up
    Const MOUSEEVENTF_ABSOLUTE As Int32 = &H8000 '  absolute move

    Public Shared cur_x As Double
    Public Shared cur_y As Double
    Public Shared dest_x As Double
    Public Shared dest_y As Double

    Public Shared Function getCursorpositionX() As Integer
        Return Cursor.Position.X - View.correctionOffsetX
    End Function

    Public Shared Function getCursorpositionY() As Integer
        Return Cursor.Position.Y - View.correctionOffsetY
    End Function

    Public Shared Sub getMouseCoords()
        cur_x = System.Windows.Forms.Cursor.Position.X * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
        cur_y = System.Windows.Forms.Cursor.Position.Y * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
    End Sub

    Public Shared Sub pressMouseRight()
        getMouseCoords()
        If My.Computer.Mouse.ButtonsSwapped Then
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_LEFTDOWN, cur_x, cur_y, 0, 0)
        Else
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_RIGHTDOWN, cur_x, cur_y, 0, 0)
        End If
    End Sub

    Public Shared Sub pressMouseLeft()
        getMouseCoords()
        If My.Computer.Mouse.ButtonsSwapped Then
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_RIGHTDOWN, cur_x, cur_y, 0, 0)
        Else
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_LEFTDOWN, cur_x, cur_y, 0, 0)
        End If
    End Sub

    Public Shared Sub releaseMouseRight()
        getMouseCoords()
        If My.Computer.Mouse.ButtonsSwapped Then
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_LEFTUP, cur_x, cur_y, 0, 0)
        Else
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_RIGHTUP, cur_x, cur_y, 0, 0)
        End If
    End Sub

    Public Shared Sub moveMouseAbsoluteLeftClick(ByVal dx As Double, ByVal dy As Double)
        dest_x = dx * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
        dest_y = dy * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
        If My.Computer.Mouse.ButtonsSwapped Then
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE + MOUSEEVENTF_RIGHTDOWN + MOUSEEVENTF_RIGHTUP, dest_x, dest_y, 0, 0)
        Else
            mouse_event(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE + MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP, dest_x, dest_y, 0, 0)
        End If
    End Sub
End Class
