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

Public Structure SystemColours
    Public background As Color
    Public linePen As Pen
    Public inverseLinePen As Pen
    Public selectedLinePen As Pen
    Public selectedLinePenFat As Pen
    Public selectedLineInVertexModePen As Pen
    Public vertexBrush As SolidBrush
    Public selectedVertexBrush As SolidBrush
    Public originPen As Pen
    Public gridPen As Pen
    Public grid10Pen As Pen
    Public selectionRectPen As Pen
    Public selectionCrossBrush As SolidBrush
    Public templatePen As Pen
    Public projectionPen As Pen
    Public Sub New(ByVal newSetting As Boolean)
        background = Color.FromArgb(0, 0, 0)
        linePen = New Pen(Color.FromArgb(234, 244, 234), 0.001F)
        inverseLinePen = New Pen(Color.FromArgb(12, 12, 12), 0.001F)
        selectedLinePen = New Pen(Color.FromArgb(245, 12, 12), 0.001F)
        selectedLinePenFat = New Pen(Color.FromArgb(245, 12, 12), 2.0F)
        selectedLineInVertexModePen = New Pen(Color.FromArgb(245, 180, 180), 0.001F)
        vertexBrush = New SolidBrush(Color.FromArgb(245, 245, 12))
        selectedVertexBrush = New SolidBrush(Color.FromArgb(245, 12, 12))
        originPen = New Pen(Color.FromArgb(140, 215, 140), 0.001F)
        gridPen = New Pen(Color.FromArgb(0, 60, 0), 0.001F)
        grid10Pen = New Pen(Color.FromArgb(60, 60, 105), 0.001F)
        selectionRectPen = New Pen(Color.FromArgb(245, 245, 220), 0.001F)
        selectionCrossBrush = New SolidBrush(Color.FromArgb(120, 0, 0))
        templatePen = New Pen(Color.FromArgb(Color.Orange.ToArgb), 2.0F)
        projectionPen = New Pen(Color.FromArgb(Color.LimeGreen.ToArgb), 2.0F)
    End Sub
End Structure
