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
Public Class ClipboardHelper

    Public Shared ClipboardVertices As New List(Of Vertex)
    Public Shared ClipboardTriangles As New List(Of Triangle)
    Public Shared ClipboardPrimitives As New List(Of Primitive)
    Public Shared ClipboardMetadata As New List(Of Metadata)

    Public Shared ClipboardVertex(1) As Double
    Public Shared ClipboardMatrix(3, 3) As Double

    Public Shared Sub copy()
        If View.SelectedVertices.Count > 0 OrElse View.SelectedTriangles.Count > 0 Then
            ClipboardVertices.Clear()
            ClipboardTriangles.Clear()
            ClipboardPrimitives.Clear()
            ClipboardMetadata.Clear()
            Dim vertexIDdict As New Dictionary(Of Integer, Boolean)
            For Each vert As Vertex In View.SelectedVertices
                ClipboardVertices.Add(New Vertex(vert.X, vert.Y, True, False) With {.vertexID = vert.vertexID, .groupindex = vert.groupindex})
                vertexIDdict.Add(vert.vertexID, False)
            Next
            If MainState.objectToModify = Modified.Triangle Then
                For Each tri As Triangle In View.SelectedTriangles
                    If Not vertexIDdict.ContainsKey(tri.vertexA.vertexID) Then ClipboardVertices.Add(New Vertex(tri.vertexA.X, tri.vertexA.Y, False, False) With {.vertexID = tri.vertexA.vertexID}) : vertexIDdict.Add(tri.vertexA.vertexID, False)
                    If Not vertexIDdict.ContainsKey(tri.vertexB.vertexID) Then ClipboardVertices.Add(New Vertex(tri.vertexB.X, tri.vertexB.Y, False, False) With {.vertexID = tri.vertexB.vertexID}) : vertexIDdict.Add(tri.vertexB.vertexID, False)
                    If Not vertexIDdict.ContainsKey(tri.vertexC.vertexID) Then ClipboardVertices.Add(New Vertex(tri.vertexC.X, tri.vertexC.Y, False, False) With {.vertexID = tri.vertexC.vertexID}) : vertexIDdict.Add(tri.vertexC.vertexID, False)
                    ClipboardTriangles.Add(New Triangle(New Vertex(tri.vertexA.X, tri.vertexA.Y, True, False) With {.vertexID = tri.vertexA.vertexID, .groupindex = tri.vertexA.groupindex}, New Vertex(tri.vertexB.X, tri.vertexB.Y, True, False) With {.vertexID = tri.vertexB.vertexID, .groupindex = tri.vertexB.groupindex}, New Vertex(tri.vertexC.X, tri.vertexC.Y, True, False) With {.vertexID = tri.vertexC.vertexID, .groupindex = tri.vertexC.groupindex}, False) With {.selected = True, .myColourNumber = tri.myColourNumber, .myColour = tri.myColour, .groupindex = tri.groupindex, .triangleID = tri.triangleID})
                Next
            Else
                If MainState.objectToModify = Modified.Vertex Then
                    For Each tri As Triangle In LPCFile.Triangles
                        If vertexIDdict.ContainsKey(tri.vertexA.vertexID) AndAlso vertexIDdict.ContainsKey(tri.vertexB.vertexID) AndAlso vertexIDdict.ContainsKey(tri.vertexC.vertexID) Then
                            ClipboardTriangles.Add(New Triangle(New Vertex(tri.vertexA.X, tri.vertexA.Y, True, False) With {.vertexID = tri.vertexA.vertexID, .groupindex = tri.vertexA.groupindex}, New Vertex(tri.vertexB.X, tri.vertexB.Y, True, False) With {.vertexID = tri.vertexB.vertexID, .groupindex = tri.vertexB.groupindex}, New Vertex(tri.vertexC.X, tri.vertexC.Y, True, False) With {.vertexID = tri.vertexC.vertexID, .groupindex = tri.vertexC.groupindex}, False) With {.selected = True, .myColourNumber = tri.myColourNumber, .myColour = tri.myColour, .groupindex = tri.groupindex, .triangleID = tri.triangleID})
                        End If
                    Next
                Else
                    For Each tri As Triangle In LPCFile.Triangles
                        If vertexIDdict.ContainsKey(tri.vertexA.vertexID) AndAlso vertexIDdict.ContainsKey(tri.vertexB.vertexID) AndAlso vertexIDdict.ContainsKey(tri.vertexC.vertexID) AndAlso tri.groupindex = tri.vertexA.groupindex AndAlso tri.groupindex = tri.vertexB.groupindex AndAlso tri.groupindex = tri.vertexC.groupindex Then
                            ClipboardTriangles.Add(New Triangle(New Vertex(tri.vertexA.X, tri.vertexA.Y, True, False) With {.vertexID = tri.vertexA.vertexID, .groupindex = tri.vertexA.groupindex}, New Vertex(tri.vertexB.X, tri.vertexB.Y, True, False) With {.vertexID = tri.vertexB.vertexID, .groupindex = tri.vertexB.groupindex}, New Vertex(tri.vertexC.X, tri.vertexC.Y, True, False) With {.vertexID = tri.vertexC.vertexID, .groupindex = tri.vertexC.groupindex}, False) With {.selected = True, .myColourNumber = tri.myColourNumber, .myColour = tri.myColour, .groupindex = tri.groupindex, .triangleID = tri.triangleID})
                        End If
                    Next
                End If
            End If
            If View.SelectedVertices.Count > 0 AndAlso View.SelectedVertices(0).groupindex > Primitive.NO_INDEX Then
                Dim prim As Primitive = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex))
                ClipboardPrimitives.Add(New Primitive(0, 0, prim.ox, prim.oy, prim.primitiveName, prim.centerVertexID, False) With {.matrix = prim.matrix, .myColourNumber = prim.myColourNumber, .myColour = prim.myColour})
                If ListHelper.LLast(ClipboardPrimitives).primitiveName Like "subfile*" Then
                    Dim m As Metadata = CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).Clone()
                    ClipboardMetadata.Add(m)
                End If
            End If
            MainForm.Refresh()
        End If
    End Sub

    Public Shared Sub paste()
        If ClipboardVertices.Count > 0 Then
            For Each vert As Vertex In View.SelectedVertices
                vert.selected = False
            Next

            For Each tri As Triangle In View.SelectedTriangles
                tri.selected = False
            Next


            View.SelectedVertices.Clear()
            View.SelectedTriangles.Clear()

            Dim VIDtoVI As New Hashtable(ClipboardVertices.Count)

            Dim startVertices As Integer = LPCFile.Vertices.Count
            Dim startTriangles As Integer = LPCFile.Triangles.Count


            If ClipboardVertices(0).groupindex > Primitive.NO_INDEX Then
                ' Neues Primitive einfügen:
                MainForm.PrimitiveModeToolStripMenuItem.PerformClick()
                LPCFile.Primitives.Add(New Primitive(0, 0, ClipboardPrimitives(0).ox, ClipboardPrimitives(0).oy, ClipboardPrimitives(0).primitiveName, ClipboardPrimitives(0).centerVertexID) With {.matrix = ClipboardPrimitives(0).matrix, .myColourNumber = ClipboardPrimitives(0).myColourNumber, .myColour = ClipboardPrimitives(0).myColour})
                LPCFile.PrimitivesHMap.Add(GlobalIdSet.primitiveIDglobal, LPCFile.Primitives.Count - 1)
                For Each vert As Vertex In ClipboardVertices
                    LPCFile.Vertices.Add(New Vertex(vert.X, vert.Y, False) With {.angleFrom = vert.vertexID, .groupindex = GlobalIdSet.primitiveIDglobal})
                    View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                Next

                If ListHelper.LLast(LPCFile.Primitives).primitiveName Like "subfile*" AndAlso Not LPCFile.PrimitivesMetadataHMap.ContainsKey(ListHelper.LLast(LPCFile.Primitives).primitiveName) Then
                    LPCFile.PrimitivesMetadataHMap.Add(ListHelper.LLast(LPCFile.Primitives).primitiveName, ClipboardMetadata(0).Clone)
                End If

                For Each tri As Triangle In ClipboardTriangles
                    LPCFile.Triangles.Add(New Triangle(tri.vertexA, tri.vertexB, tri.vertexC) With {.selected = True, .myColourNumber = tri.myColourNumber, .myColour = tri.myColour, .groupindex = GlobalIdSet.primitiveIDglobal})
                    View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                Next

                MainForm.NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(GlobalIdSet.primitiveIDglobal), Integer)).matrix(0, 0)
                MainForm.NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(GlobalIdSet.primitiveIDglobal), Integer)).matrix(0, 1)
                MainForm.NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(GlobalIdSet.primitiveIDglobal), Integer)).matrix(0, 3) / 1000D * View.unitFactor
                MainForm.NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(GlobalIdSet.primitiveIDglobal), Integer)).matrix(1, 0)
                MainForm.NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(GlobalIdSet.primitiveIDglobal), Integer)).matrix(1, 1)
                MainForm.NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(GlobalIdSet.primitiveIDglobal), Integer)).matrix(1, 3) / 1000D * View.unitFactor
                MainForm.GBMatrix.Visible = True
            Else
                ' Nur LPCFile.Vertices und LPCFile.Triangles einfügen
                For Each vert As Vertex In ClipboardVertices
                    LPCFile.Vertices.Add(New Vertex(vert.X, vert.Y, True) With {.angleFrom = vert.vertexID})
                    View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                Next

                For Each tri As Triangle In ClipboardTriangles
                    LPCFile.Triangles.Add(New Triangle(tri.vertexA, tri.vertexB, tri.vertexC) With {.selected = True, .myColourNumber = tri.myColourNumber, .myColour = tri.myColour})
                    View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                Next
            End If

            ' Hashmap erstellen:
            For i As Integer = startVertices To LPCFile.Vertices.Count - 1
                VIDtoVI.Add(CType(LPCFile.Vertices(i).angleFrom, Integer), i)
                LPCFile.Vertices(i).angleFrom = 0
            Next

            ' Linken:
            If ClipboardVertices(0).groupindex > Primitive.NO_INDEX Then
                ListHelper.LLast(LPCFile.Primitives).centerVertexID = LPCFile.Vertices(VIDtoVI(ListHelper.LLast(LPCFile.Primitives).centerVertexID)).vertexID
            End If
            For i As Integer = startTriangles To LPCFile.Triangles.Count - 1
                LPCFile.Vertices(VIDtoVI(LPCFile.Triangles(i).vertexA.vertexID)).linkedTriangles.Add(LPCFile.Triangles(i))
                LPCFile.Vertices(VIDtoVI(LPCFile.Triangles(i).vertexB.vertexID)).linkedTriangles.Add(LPCFile.Triangles(i))
                LPCFile.Vertices(VIDtoVI(LPCFile.Triangles(i).vertexC.vertexID)).linkedTriangles.Add(LPCFile.Triangles(i))
                LPCFile.Triangles(i).vertexA = LPCFile.Vertices(VIDtoVI(LPCFile.Triangles(i).vertexA.vertexID))
                LPCFile.Triangles(i).vertexB = LPCFile.Vertices(VIDtoVI(LPCFile.Triangles(i).vertexB.vertexID))
                LPCFile.Triangles(i).vertexC = LPCFile.Vertices(VIDtoVI(LPCFile.Triangles(i).vertexC.vertexID))
            Next
            MainForm.Refresh()
        End If
    End Sub

    Public Shared Sub delete()
        If View.SelectedVertices.Count > 0 OrElse View.SelectedTriangles.Count > 0 Then
            MainForm.BtnAddToGroup.Enabled = False
            Helper_2D.stopTriangulation()
            MainState.isLoading = True
            If MainState.objectToModify <> Modified.Triangle Then
                If MainState.objectToModify = Modified.Primitive AndAlso MainState.trianglemode = 0 Then
                    Dim mygroupindex As Integer = View.SelectedVertices(0).groupindex
                    Dim myindex As Integer = CType(LPCFile.PrimitivesHMap(mygroupindex), Integer)
                    LPCFile.PrimitivesHMap.Remove(mygroupindex)
                    If LPCFile.PrimitivesHMap.Keys.Count > 0 Then
                        Dim klist(LPCFile.PrimitivesHMap.Keys.Count - 1) As Integer
                        Dim counter As Integer = 0
                        For Each key As Integer In LPCFile.PrimitivesHMap.Keys
                            klist(counter) = key
                            counter += 1
                        Next
                        For i As Integer = 0 To LPCFile.PrimitivesHMap.Keys.Count - 1
                            If LPCFile.PrimitivesHMap.Item(klist(i)) > myindex Then
                                LPCFile.PrimitivesHMap.Item(klist(i)) -= 1
                            End If
                        Next i
                    End If
                    Dim deleteMetadata As Boolean = True
                    Dim tempName As String = LPCFile.Primitives(myindex).primitiveName
                    LPCFile.Primitives.RemoveAt(myindex)
                    MainForm.BtnUngroup.Enabled = False
                    For Each prim As Primitive In LPCFile.Primitives
                        If prim.primitiveName = tempName Then
                            deleteMetadata = False
                            Exit For
                        End If
                    Next
                    If deleteMetadata AndAlso LPCFile.PrimitivesMetadataHMap.ContainsKey(tempName) Then
                        LPCFile.PrimitivesMetadataHMap.Remove(tempName)
                    End If
                    For Each vert As Vertex In View.SelectedVertices
removeVertex1:
                        For iv As Integer = 0 To LPCFile.Vertices.Count - 1
                            Dim ref As Vertex = LPCFile.Vertices(iv)
                            If ref.Equals(vert) Then
nextTriangle1:
                                Dim vertexremove As Boolean = True
                                For Each tri As Triangle In vert.linkedTriangles
                                    If tri.groupindex = mygroupindex Then
                                        If LPCFile.Triangles.Contains(tri) Then
                                            Helper_2D.removeTriangle(tri)
                                        Else
                                            vert.linkedTriangles.Remove(tri)
                                        End If
                                        GoTo nextTriangle1
                                    Else
                                        vertexremove = False
                                    End If
                                Next
                                If vertexremove Then
                                    LPCFile.Vertices.RemoveAt(iv)
                                    GoTo removeVertex1
                                Else
                                    LPCFile.Vertices(iv).groupindex = -1
                                End If
                            End If
                        Next
                    Next
                Else
                    For Each vert As Vertex In View.SelectedVertices
removeVertex2:
                        For iv As Integer = 0 To LPCFile.Vertices.Count - 1
                            Dim ref As Vertex = LPCFile.Vertices(iv)
                            If ref.Equals(vert) AndAlso vert.groupindex = Primitive.NO_INDEX Then
nextTriangle2:
                                For Each tri As Triangle In vert.linkedTriangles
                                    If LPCFile.Triangles.Contains(tri) Then
                                        Helper_2D.removeTriangle(tri)
                                    Else
                                        vert.linkedTriangles.Remove(tri)
                                    End If
                                    GoTo nextTriangle2
                                Next
                                LPCFile.Vertices.RemoveAt(iv)
                                GoTo removeVertex2
                            End If
                        Next
                    Next
                End If
            Else
                For Each tri As Triangle In View.SelectedTriangles
                    Helper_2D.removeTriangle(tri)
                Next
            End If
            View.SelectedVertices.Clear()
            View.SelectedTriangles.Clear()
            If View.CollisionVertices.Count > 0 Then MainForm.detectCollisions()
            MainState.isLoading = False
            MainForm.Refresh()
        ElseIf LPCFile.helperLineStartIndex > -1 AndAlso LPCFile.helperLineEndIndex > -1 AndAlso LPCFile.helperLineStartIndex < LPCFile.helperLineEndIndex AndAlso LPCFile.helperLineEndIndex < LPCFile.templateShape.Count Then
            LPCFile.templateShape.RemoveRange(LPCFile.helperLineStartIndex, LPCFile.helperLineEndIndex - LPCFile.helperLineStartIndex + 1)
            If LPCFile.templateShape.Count > 0 Then
                If LPCFile.helperLineStartIndex < LPCFile.templateShape.Count AndAlso LPCFile.templateShape(LPCFile.helperLineStartIndex).X = Single.Epsilon Then
                    LPCFile.templateShape.RemoveAt(LPCFile.helperLineStartIndex)
                ElseIf LPCFile.helperLineStartIndex > 0 Then
                    LPCFile.templateShape.RemoveAt(LPCFile.helperLineStartIndex - 1)
                End If
            End If
            LPCFile.helperLineStartIndex = -1
            LPCFile.helperLineStartIndex = -1
            MainState.isLoading = False
            MainForm.Refresh()
        End If
    End Sub

End Class
