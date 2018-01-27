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

Public Structure KeySetting
    Public ModeSelect As Integer
    Public ModeMove As Integer
    Public ModeRotate As Integer
    Public ModeScale As Integer
    Public AddVertex As Integer
    Public AddTriangle As Integer
    Public Abort As Integer
    Public Pipette As Integer
    Public Preview As Integer
    Public ShowColours As Integer
    Public Zoom As Integer
    Public Translate As Integer
    Public AddPrimitive As Integer
    Public MergeSplit As Integer
    Public CSG As Integer
    Public Sub New(ByVal newSetting As Boolean)
        ModeSelect = Keys.D1
        ModeMove = Keys.D2
        ModeRotate = Keys.D3
        ModeScale = Keys.D4
        AddVertex = Keys.D5
        AddTriangle = Keys.D6
        Abort = Keys.Escape
        Pipette = Keys.O
        Preview = Keys.P
        ShowColours = Keys.Space
        Zoom = Keys.K
        Translate = Keys.L
        AddPrimitive = Keys.F7
        MergeSplit = Keys.F8
        CSG = Keys.F9
    End Sub
End Structure
