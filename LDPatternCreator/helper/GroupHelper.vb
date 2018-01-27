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

Module GroupHelper
    Public Sub selectGroupDirect(ByVal group As Integer)
        Helper_2D.clearSelection()
        Dim vertDict As New Dictionary(Of Integer, Boolean)
        Dim tri As Triangle
        For trii As Integer = 0 To LPCFile.Triangles.Count - 1
            tri = LPCFile.Triangles(trii)
            If tri.groupindex = group Then
                View.SelectedTriangles.Add(tri) : tri.selected = True
                If Not vertDict.ContainsKey(tri.vertexA.vertexID) Then View.SelectedVertices.Add(tri.vertexA) : vertDict.Add(tri.vertexA.vertexID, False)
                If Not vertDict.ContainsKey(tri.vertexB.vertexID) Then View.SelectedVertices.Add(tri.vertexB) : vertDict.Add(tri.vertexB.vertexID, False)
                If Not vertDict.ContainsKey(tri.vertexC.vertexID) Then View.SelectedVertices.Add(tri.vertexC) : vertDict.Add(tri.vertexC.vertexID, False)
            End If
        Next
    End Sub
End Module
