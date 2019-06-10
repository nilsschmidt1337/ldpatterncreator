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

Public Class Helper_2D

    Public Shared Sub clearSelection()
        For i As Integer = 0 To View.SelectedTriangles.Count - 1
            View.SelectedTriangles(i).selected = False
        Next
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            View.SelectedVertices(i).selected = False
        Next
        View.SelectedTriangles.Clear()
        View.SelectedVertices.Clear()
        LPCFile.helperLineStartIndex = -1
        LPCFile.helperLineEndIndex = -1
    End Sub

    Public Shared Sub stopTriangulation()
        MainState.trianglemode = 0 : MainState.intelligentFocusTriangle = Nothing : MainState.referenceLineMode = 0
    End Sub

    Public Shared Function removeTriangle(ByVal tri As Triangle) As Boolean
        tri.vertexA.linkedTriangles.Remove(tri)
        tri.vertexB.linkedTriangles.Remove(tri)
        tri.vertexC.linkedTriangles.Remove(tri)
        Return LPCFile.Triangles.Remove(tri)
    End Function

    Public Shared Sub removeTriangle(ByVal tri As Triangle, ByVal index As Integer)
        tri.vertexA.linkedTriangles.Remove(tri)
        tri.vertexB.linkedTriangles.Remove(tri)
        tri.vertexC.linkedTriangles.Remove(tri)
        LPCFile.Triangles.RemoveAt(index)
    End Sub

End Class
