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
Public Class SelectionHelper

    Public Shared Sub selectAll()
        View.SelectedTriangles.Clear()
        View.SelectedVertices.Clear()
        Dim vertDict As New Dictionary(Of Integer, Boolean)
        For Each tri As Triangle In LPCFile.Triangles
            If tri.groupindex = -1 Then
                View.SelectedTriangles.Add(tri)
                tri.selected = True
                If Not vertDict.ContainsKey(tri.vertexA.vertexID) AndAlso tri.vertexA.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexA.vertexID, False)
                If Not vertDict.ContainsKey(tri.vertexB.vertexID) AndAlso tri.vertexB.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexB.vertexID, False)
                If Not vertDict.ContainsKey(tri.vertexC.vertexID) AndAlso tri.vertexC.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexC.vertexID, False)
            End If
        Next
        For Each vert As Vertex In LPCFile.Vertices
            If Not vertDict.ContainsKey(vert.vertexID) AndAlso vert.groupindex = Primitive.NO_INDEX Then
                vert.selected = True
                View.SelectedVertices.Add(vert)
                vertDict.Add(vert.vertexID, False)
            End If
        Next
    End Sub

    Public Shared Sub selectSameColour()
        If View.SelectedTriangles.Count > 0 Then
            Dim tc As Short = View.SelectedTriangles(0).myColourNumber
            Dim tci As Integer = View.SelectedTriangles(0).myColour.ToArgb
            Dim vertDict As New Dictionary(Of Integer, Boolean)
            View.SelectedTriangles.Clear()
            View.SelectedVertices.Clear()
            If tc <> -1 Then
                For Each tri As Triangle In LPCFile.Triangles
                    If tri.myColourNumber = tc AndAlso tri.groupindex < 0 Then
                        View.SelectedTriangles.Add(tri)
                        tri.selected = True
                        If Not vertDict.ContainsKey(tri.vertexA.vertexID) AndAlso tri.vertexA.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexA.vertexID, False)
                        If Not vertDict.ContainsKey(tri.vertexB.vertexID) AndAlso tri.vertexB.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexB.vertexID, False)
                        If Not vertDict.ContainsKey(tri.vertexC.vertexID) AndAlso tri.vertexC.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexC.vertexID, False)
                    End If
                Next
            Else
                For Each tri As Triangle In LPCFile.Triangles
                    If tri.myColour.ToArgb = tci AndAlso tri.groupindex < 0 Then
                        View.SelectedTriangles.Add(tri)
                        tri.selected = True
                        If Not vertDict.ContainsKey(tri.vertexA.vertexID) AndAlso tri.vertexA.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexA.vertexID, False)
                        If Not vertDict.ContainsKey(tri.vertexB.vertexID) AndAlso tri.vertexB.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexB.vertexID, False)
                        If Not vertDict.ContainsKey(tri.vertexC.vertexID) AndAlso tri.vertexC.groupindex = Primitive.NO_INDEX Then View.SelectedVertices.Add(tri.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexC.vertexID, False)
                    End If
                Next
            End If
        End If
    End Sub

    Public Shared Sub selectConnected(Optional ByVal sameColourTest As Boolean = True)
        Dim startcount As Integer = 0
        Dim tc As Short
        Dim tci As Integer
        If MainForm.WithColourToolStripMenuItem.Checked AndAlso sameColourTest AndAlso View.SelectedTriangles.Count > 0 Then tc = View.SelectedTriangles(View.SelectedTriangles.Count - 1).myColourNumber : tci = View.SelectedTriangles(View.SelectedTriangles.Count - 1).myColour.ToArgb
nextStep:
        If MainState.objectToModify = Modified.Vertex Then
            Dim tmpcount As Integer = View.SelectedVertices.Count - 1
            For verti = startcount To tmpcount
                Dim vert As Vertex = View.SelectedVertices(verti)
                For Each tri As Triangle In vert.linkedTriangles
                    If Not View.SelectedVertices.Contains(tri.vertexA) AndAlso tri.vertexA.groupindex < 0 Then tri.vertexA.selected = True : View.SelectedVertices.Add(tri.vertexA)
                    If Not View.SelectedVertices.Contains(tri.vertexB) AndAlso tri.vertexB.groupindex < 0 Then tri.vertexB.selected = True : View.SelectedVertices.Add(tri.vertexB)
                    If Not View.SelectedVertices.Contains(tri.vertexC) AndAlso tri.vertexC.groupindex < 0 Then tri.vertexC.selected = True : View.SelectedVertices.Add(tri.vertexC)
                Next
            Next
            startcount = tmpcount + 1
            If View.SelectedVertices.Count > startcount Then GoTo nextstep
        ElseIf MainState.objectToModify = Modified.Triangle Then
            Dim tmpcount As Integer = View.SelectedTriangles.Count - 1
            If MainForm.WithColourToolStripMenuItem.Checked AndAlso sameColourTest Then
                For trii = startcount To tmpcount
                    Dim tri As Triangle = View.SelectedTriangles(trii)
                    If Not tri.vertexA Is Nothing Then
                        For Each tri2 As Triangle In tri.vertexA.linkedTriangles
                            If ((tri2.myColourNumber = tc AndAlso tc <> -1) OrElse (tc = -1 AndAlso tri2.myColour.ToArgb = tci)) AndAlso Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                                If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex < 0 Then tri2.vertexA.selected = True : View.SelectedVertices.Add(tri2.vertexA)
                                If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex < 0 Then tri2.vertexB.selected = True : View.SelectedVertices.Add(tri2.vertexB)
                                If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex < 0 Then tri2.vertexC.selected = True : View.SelectedVertices.Add(tri2.vertexC)
                                tri2.selected = True : View.SelectedTriangles.Add(tri2)
                            End If
                        Next
                    End If
                    If Not tri.vertexB Is Nothing Then
                        For Each tri2 As Triangle In tri.vertexB.linkedTriangles
                            If ((tri2.myColourNumber = tc AndAlso tc <> -1) OrElse (tc = -1 AndAlso tri2.myColour.ToArgb = tci)) AndAlso Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                                If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex < 0 Then tri2.vertexA.selected = True : View.SelectedVertices.Add(tri2.vertexA)
                                If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex < 0 Then tri2.vertexB.selected = True : View.SelectedVertices.Add(tri2.vertexB)
                                If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex < 0 Then tri2.vertexC.selected = True : View.SelectedVertices.Add(tri2.vertexC)
                                tri2.selected = True : View.SelectedTriangles.Add(tri2)
                            End If
                        Next
                    End If
                    If Not tri.vertexC Is Nothing Then
                        For Each tri2 As Triangle In tri.vertexC.linkedTriangles
                            If ((tri2.myColourNumber = tc AndAlso tc <> -1) OrElse (tc = -1 AndAlso tri2.myColour.ToArgb = tci)) AndAlso Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                                If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex < 0 Then tri2.vertexA.selected = True : View.SelectedVertices.Add(tri2.vertexA)
                                If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex < 0 Then tri2.vertexB.selected = True : View.SelectedVertices.Add(tri2.vertexB)
                                If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex < 0 Then tri2.vertexC.selected = True : View.SelectedVertices.Add(tri2.vertexC)
                                tri2.selected = True : View.SelectedTriangles.Add(tri2)
                            End If
                        Next
                    End If
                Next
            Else
                For trii = startcount To tmpcount
                    Dim tri As Triangle = View.SelectedTriangles(trii)
                    For Each tri2 As Triangle In tri.vertexA.linkedTriangles
                        If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex < 0 Then tri2.vertexA.selected = True : View.SelectedVertices.Add(tri2.vertexA)
                            If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex < 0 Then tri2.vertexB.selected = True : View.SelectedVertices.Add(tri2.vertexB)
                            If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex < 0 Then tri2.vertexC.selected = True : View.SelectedVertices.Add(tri2.vertexC)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                    For Each tri2 As Triangle In tri.vertexB.linkedTriangles
                        If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex < 0 Then tri2.vertexA.selected = True : View.SelectedVertices.Add(tri2.vertexA)
                            If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex < 0 Then tri2.vertexB.selected = True : View.SelectedVertices.Add(tri2.vertexB)
                            If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex < 0 Then tri2.vertexC.selected = True : View.SelectedVertices.Add(tri2.vertexC)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                    For Each tri2 As Triangle In tri.vertexC.linkedTriangles
                        If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex < 0 Then tri2.vertexA.selected = True : View.SelectedVertices.Add(tri2.vertexA)
                            If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex < 0 Then tri2.vertexB.selected = True : View.SelectedVertices.Add(tri2.vertexB)
                            If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex < 0 Then tri2.vertexC.selected = True : View.SelectedVertices.Add(tri2.vertexC)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                Next
            End If
            startcount = tmpcount + 1
            If View.SelectedTriangles.Count > startcount Then GoTo nextstep
        End If
    End Sub

    Public Shared Sub selectPrimitiveGroup(ByVal group As Integer)
        Dim startcount As Integer = 0
nextStep:
        Dim tmpcount As Integer = View.SelectedTriangles.Count - 1
        For trii = startcount To tmpcount
            Dim tri As Triangle = View.SelectedTriangles(trii)
            For Each tri2 As Triangle In tri.vertexA.linkedTriangles
                If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex = group Then
                    If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex = group Then View.SelectedVertices.Add(tri2.vertexA)
                    If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex = group Then View.SelectedVertices.Add(tri2.vertexB)
                    If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex = group Then View.SelectedVertices.Add(tri2.vertexC)
                    tri2.selected = True : View.SelectedTriangles.Add(tri2)
                End If
            Next
            For Each tri2 As Triangle In tri.vertexB.linkedTriangles
                If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex = group Then
                    If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex = group Then View.SelectedVertices.Add(tri2.vertexA)
                    If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex = group Then View.SelectedVertices.Add(tri2.vertexB)
                    If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex = group Then View.SelectedVertices.Add(tri2.vertexC)
                    tri2.selected = True : View.SelectedTriangles.Add(tri2)
                End If
            Next
            For Each tri2 As Triangle In tri.vertexC.linkedTriangles
                If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex = group Then
                    If Not View.SelectedVertices.Contains(tri2.vertexA) AndAlso tri2.vertexA.groupindex = group Then View.SelectedVertices.Add(tri2.vertexA)
                    If Not View.SelectedVertices.Contains(tri2.vertexB) AndAlso tri2.vertexB.groupindex = group Then View.SelectedVertices.Add(tri2.vertexB)
                    If Not View.SelectedVertices.Contains(tri2.vertexC) AndAlso tri2.vertexC.groupindex = group Then View.SelectedVertices.Add(tri2.vertexC)
                    tri2.selected = True : View.SelectedTriangles.Add(tri2)
                End If
            Next
        Next
        startcount = tmpcount + 1
        If View.SelectedTriangles.Count > startcount Then GoTo nextstep
    End Sub

    Public Shared Sub selectTouching()
        Dim vertDict As New Dictionary(Of Integer, Boolean)
        Dim tmpcount2 As Integer = View.SelectedVertices.Count - 1
        For verti = 0 To tmpcount2
            vertDict.Add(View.SelectedVertices(verti).vertexID, False)
        Next
        If MainState.objectToModify = Modified.Vertex Then
            Dim tmpcount As Integer = View.SelectedVertices.Count - 1
            For verti = 0 To tmpcount
                Dim vert As Vertex = View.SelectedVertices(verti)
                For Each tri As Triangle In vert.linkedTriangles
                    If Not vertDict.ContainsKey(tri.vertexA.vertexID) AndAlso tri.vertexA.groupindex = -1 Then View.SelectedVertices.Add(tri.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexA.vertexID, False)
                    If Not vertDict.ContainsKey(tri.vertexB.vertexID) AndAlso tri.vertexB.groupindex = -1 Then View.SelectedVertices.Add(tri.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexB.vertexID, False)
                    If Not vertDict.ContainsKey(tri.vertexC.vertexID) AndAlso tri.vertexC.groupindex = -1 Then View.SelectedVertices.Add(tri.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri.vertexC.vertexID, False)
                Next
            Next
        ElseIf MainState.objectToModify = Modified.Triangle Then
            Dim tc As Short
            Dim tci As Integer
            If MainForm.WithColourToolStripMenuItem.Checked AndAlso View.SelectedTriangles.Count > 0 Then tc = View.SelectedTriangles(View.SelectedTriangles.Count - 1).myColourNumber : tci = View.SelectedTriangles(View.SelectedTriangles.Count - 1).myColour.ToArgb
            Dim tmpcount As Integer = View.SelectedTriangles.Count - 1
            If MainForm.WithColourToolStripMenuItem.Checked Then
                For trii = 0 To tmpcount
                    Dim tri As Triangle = View.SelectedTriangles(trii)
                    For Each tri2 As Triangle In tri.vertexA.linkedTriangles
                        If ((tri2.myColourNumber = tc AndAlso tc <> -1) OrElse (tc = -1 AndAlso tri2.myColour.ToArgb = tci)) AndAlso Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not vertDict.ContainsKey(tri2.vertexA.vertexID) AndAlso tri2.vertexA.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexA.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexB.vertexID) AndAlso tri2.vertexB.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexB.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexC.vertexID) AndAlso tri2.vertexC.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexC.vertexID, False)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                    For Each tri2 As Triangle In tri.vertexB.linkedTriangles
                        If ((tri2.myColourNumber = tc AndAlso tc <> -1) OrElse (tc = -1 AndAlso tri2.myColour.ToArgb = tci)) AndAlso Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not vertDict.ContainsKey(tri2.vertexA.vertexID) AndAlso tri2.vertexA.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexA.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexB.vertexID) AndAlso tri2.vertexB.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexB.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexC.vertexID) AndAlso tri2.vertexC.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexC.vertexID, False)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                    For Each tri2 As Triangle In tri.vertexC.linkedTriangles
                        If ((tri2.myColourNumber = tc AndAlso tc <> -1) OrElse (tc = -1 AndAlso tri2.myColour.ToArgb = tci)) AndAlso Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not vertDict.ContainsKey(tri2.vertexA.vertexID) AndAlso tri2.vertexA.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexA.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexB.vertexID) AndAlso tri2.vertexB.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexB.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexC.vertexID) AndAlso tri2.vertexC.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexC.vertexID, False)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                Next
            Else
                For trii = 0 To tmpcount
                    Dim tri As Triangle = View.SelectedTriangles(trii)
                    For Each tri2 As Triangle In tri.vertexA.linkedTriangles
                        If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not vertDict.ContainsKey(tri2.vertexA.vertexID) AndAlso tri2.vertexA.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexA.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexB.vertexID) AndAlso tri2.vertexB.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexB.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexC.vertexID) AndAlso tri2.vertexC.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexC.vertexID, False)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                    For Each tri2 As Triangle In tri.vertexB.linkedTriangles
                        If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not vertDict.ContainsKey(tri2.vertexA.vertexID) AndAlso tri2.vertexA.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexA.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexB.vertexID) AndAlso tri2.vertexB.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexB.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexC.vertexID) AndAlso tri2.vertexC.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexC.vertexID, False)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                    For Each tri2 As Triangle In tri.vertexC.linkedTriangles
                        If Not View.SelectedTriangles.Contains(tri2) AndAlso tri2.groupindex < 0 Then
                            If Not vertDict.ContainsKey(tri2.vertexA.vertexID) AndAlso tri2.vertexA.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexA) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexA.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexB.vertexID) AndAlso tri2.vertexB.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexB) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexB.vertexID, False)
                            If Not vertDict.ContainsKey(tri2.vertexC.vertexID) AndAlso tri2.vertexC.groupindex = -1 Then View.SelectedVertices.Add(tri2.vertexC) : ListHelper.LLast(View.SelectedVertices).selected = True : vertDict.Add(tri2.vertexC.vertexID, False)
                            tri2.selected = True : View.SelectedTriangles.Add(tri2)
                        End If
                    Next
                Next
            End If
        End If
    End Sub

End Class
