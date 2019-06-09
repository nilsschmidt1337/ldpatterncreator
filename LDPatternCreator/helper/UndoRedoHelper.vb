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
Public Class UndoRedoHelper

    Public Shared pointer As Byte = 0

    Public Shared historyVertices As New List(Of List(Of Vertex))
    Public Shared historyTriangles As New List(Of List(Of Triangle))
    Public Shared historySelectedVertices As New List(Of List(Of Vertex))
    Public Shared historySelectedTriangles As New List(Of List(Of Triangle))
    Public Shared historyPrimitives As New List(Of List(Of Primitive))
    Public Shared historyPrimitivesHash As New List(Of Dictionary(Of Integer, Integer))
    Public Shared historyPrimitivesMetadataHash As New List(Of Dictionary(Of String, Metadata))
    Public Shared historyTemplateShape As New List(Of List(Of PointF))
    Public Shared historyMyMetadata As New List(Of Metadata)
    Public Shared historyProjectionQuads As New List(Of List(Of ProjectionQuad))
    Public Shared historyTemplateText As New List(Of List(Of TemplateTextInfo))


    Public Shared Sub addHistory()
        If Not MainForm.PerformanceEnabledToolStripMenuItem.Checked Then
            Dim vc As Integer = historyVertices.Count
            If pointer < vc Then
                historyVertices.RemoveRange(pointer + 1, historyVertices.Count - pointer - 1)
                historyTriangles.RemoveRange(pointer + 1, historyTriangles.Count - pointer - 1)
                historySelectedVertices.RemoveRange(pointer + 1, historySelectedVertices.Count - pointer - 1)
                historySelectedTriangles.RemoveRange(pointer + 1, historySelectedTriangles.Count - pointer - 1)
                historyPrimitives.RemoveRange(pointer + 1, historyPrimitives.Count - pointer - 1)
                historyPrimitivesHash.RemoveRange(pointer + 1, historyPrimitivesHash.Count - pointer - 1)
                historyTemplateShape.RemoveRange(pointer + 1, historyTemplateShape.Count - pointer - 1)
                historyPrimitivesMetadataHash.RemoveRange(pointer + 1, historyPrimitivesMetadataHash.Count - pointer - 1)
                historyMyMetadata.RemoveRange(pointer + 1, historyMyMetadata.Count - pointer - 1)
                historyProjectionQuads.RemoveRange(pointer + 1, historyProjectionQuads.Count - pointer - 1)
                historyTemplateText.RemoveRange(pointer + 1, historyTemplateText.Count - pointer - 1)
            End If

            historyVertices.Add(CloneHelper.DeepCloneVertices(LPCFile.Vertices))
            historyTriangles.Add(CloneHelper.DeepCloneTriangles(LPCFile.Triangles))
            historySelectedVertices.Add(CloneHelper.DeepCloneVertices(View.SelectedVertices))
            historySelectedTriangles.Add(CloneHelper.DeepCloneTriangles(View.SelectedTriangles))
            historyMyMetadata.Add(CloneHelper.DeepCloneMetadata(LPCFile.myMetadata))
            historyPrimitives.Add(CloneHelper.DeepClonePrimitives(LPCFile.Primitives))
            historyPrimitivesHash.Add(CloneHelper.DeepClonePrimitivesHMap(LPCFile.PrimitivesHMap))
            historyPrimitivesMetadataHash.Add(CloneHelper.DeepClonePrimitivesMetadataHMap(LPCFile.PrimitivesMetadataHMap))
            historyTemplateShape.Add(CloneHelper.DeepCloneTemplateShape(LPCFile.templateShape))
            historyProjectionQuads.Add(CloneHelper.DeepCloneProjectionQuads(LPCFile.templateProjectionQuads))
            historyTemplateText.Add(CloneHelper.DeepCloneTemplateTexts(LPCFile.templateTexts))

            If pointer < LDSettings.Editor.max_undo Then
                pointer += 1
            Else
                UndoRedoHelper.historyVertices.RemoveAt(0)
                UndoRedoHelper.historyTriangles.RemoveAt(0)
                UndoRedoHelper.historySelectedVertices.RemoveAt(0)
                UndoRedoHelper.historySelectedTriangles.RemoveAt(0)
                UndoRedoHelper.historyPrimitives.RemoveAt(0)
                UndoRedoHelper.historyPrimitivesHash.RemoveAt(0)
                UndoRedoHelper.historyTemplateShape.RemoveAt(0)
                UndoRedoHelper.historyPrimitivesMetadataHash.RemoveAt(0)
                UndoRedoHelper.historyMyMetadata.RemoveAt(0)
                UndoRedoHelper.historyProjectionQuads.RemoveAt(0)
                UndoRedoHelper.historyTemplateText.RemoveAt(0)
            End If
            MainForm.UndoToolStripMenuItem.Enabled = vc > 1
            MainForm.RedoToolStripMenuItem.Enabled = False
        End If
        MainState.unsavedChanges = True
    End Sub

    Public Shared Sub undo()
        If pointer = historyVertices.Count Then pointer -= 1
        If pointer > 0 Then
            pointer -= 1
            If Not UndoRedoHelper.historyVertices(pointer) Is Nothing Then LPCFile.Vertices.Clear() : LPCFile.Vertices.AddRange(CloneHelper.DeepCloneVertices(historyVertices(pointer)))
            If Not UndoRedoHelper.historySelectedVertices(pointer) Is Nothing Then
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    View.SelectedVertices(i).selected = False
                Next
                View.SelectedVertices.Clear() : View.SelectedVertices.AddRange(CloneHelper.DeepCloneVertices(historySelectedVertices(pointer)))
            End If
            If Not UndoRedoHelper.historyTriangles(pointer) Is Nothing Then LPCFile.Triangles.Clear() : LPCFile.Triangles.AddRange(CloneHelper.DeepCloneTriangles(historyTriangles(pointer)))
            If Not UndoRedoHelper.historySelectedTriangles(pointer) Is Nothing Then
                For i As Integer = 0 To View.SelectedTriangles.Count - 1
                    View.SelectedTriangles(i).selected = False
                Next
                View.SelectedTriangles.Clear() : View.SelectedTriangles.AddRange(CloneHelper.DeepCloneTriangles(historySelectedTriangles(pointer)))
            End If
            If Not UndoRedoHelper.historyPrimitives(pointer) Is Nothing Then LPCFile.Primitives.Clear() : LPCFile.Primitives.AddRange(CloneHelper.DeepClonePrimitives(historyPrimitives(pointer)))
            If Not UndoRedoHelper.historyPrimitivesHash(pointer) Is Nothing Then LPCFile.PrimitivesHMap = CloneHelper.DeepClonePrimitivesHMap(historyPrimitivesHash(pointer))
            If Not UndoRedoHelper.historyMyMetadata(pointer) Is Nothing Then LPCFile.myMetadata = CloneHelper.DeepCloneMetadata(historyMyMetadata(pointer))
            If Not UndoRedoHelper.historyPrimitivesMetadataHash(pointer) Is Nothing Then LPCFile.PrimitivesMetadataHMap = CloneHelper.DeepClonePrimitivesMetadataHMap(historyPrimitivesMetadataHash(pointer))
            If Not UndoRedoHelper.historyTemplateShape(pointer) Is Nothing Then LPCFile.templateShape.Clear() : LPCFile.templateShape.AddRange(CloneHelper.DeepCloneTemplateShape(historyTemplateShape(pointer)))
            If Not UndoRedoHelper.historyProjectionQuads(pointer) Is Nothing Then LPCFile.templateProjectionQuads.Clear() : LPCFile.templateProjectionQuads.AddRange(CloneHelper.DeepCloneProjectionQuads(historyProjectionQuads(pointer)))
            If Not UndoRedoHelper.historyTemplateText(pointer) Is Nothing Then LPCFile.templateTexts.Clear() : LPCFile.templateTexts.AddRange(CloneHelper.DeepCloneTemplateTexts(historyTemplateText(pointer)))
            MainForm.RedoToolStripMenuItem.Enabled = True
            UndoRedoHelper.cleanupHistoryData()
            UndoRedoHelper.cleanupUndoRedo()
        End If
        MainForm.UndoToolStripMenuItem.Enabled = pointer > 0
    End Sub

    Public Shared Sub redo()
        If pointer < historyVertices.Count - 1 Then
            pointer += 1
            If Not UndoRedoHelper.historyVertices(pointer) Is Nothing Then LPCFile.Vertices.Clear() : LPCFile.Vertices.AddRange(CloneHelper.DeepCloneVertices(historyVertices(pointer)))
            If Not UndoRedoHelper.historySelectedVertices(pointer) Is Nothing Then
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    View.SelectedVertices(i).selected = False
                Next
                View.SelectedVertices.Clear() : View.SelectedVertices.AddRange(CloneHelper.DeepCloneVertices(historySelectedVertices(pointer)))
            End If
            If Not UndoRedoHelper.historyTriangles(pointer) Is Nothing Then LPCFile.Triangles.Clear() : LPCFile.Triangles.AddRange(CloneHelper.DeepCloneTriangles(historyTriangles(pointer)))
            If Not UndoRedoHelper.historySelectedTriangles(pointer) Is Nothing Then
                For i As Integer = 0 To View.SelectedTriangles.Count - 1
                    View.SelectedTriangles(i).selected = False
                Next
                View.SelectedTriangles.Clear() : View.SelectedTriangles.AddRange(CloneHelper.DeepCloneTriangles(historySelectedTriangles(pointer)))
            End If
            If Not UndoRedoHelper.historyPrimitives(pointer) Is Nothing Then LPCFile.Primitives.Clear() : LPCFile.Primitives.AddRange(CloneHelper.DeepClonePrimitives(historyPrimitives(pointer)))
            If Not UndoRedoHelper.historyPrimitivesHash(pointer) Is Nothing Then LPCFile.PrimitivesHMap = CloneHelper.DeepClonePrimitivesHMap(historyPrimitivesHash(pointer))
            If Not UndoRedoHelper.historyMyMetadata(pointer) Is Nothing Then LPCFile.myMetadata = CloneHelper.DeepCloneMetadata(historyMyMetadata(pointer))
            If Not UndoRedoHelper.historyPrimitivesMetadataHash(pointer) Is Nothing Then LPCFile.PrimitivesMetadataHMap = CloneHelper.DeepClonePrimitivesMetadataHMap(historyPrimitivesMetadataHash(pointer))
            If Not UndoRedoHelper.historyTemplateShape(pointer) Is Nothing Then LPCFile.templateShape.Clear() : LPCFile.templateShape.AddRange(CloneHelper.DeepCloneTemplateShape(historyTemplateShape(pointer)))
            If Not UndoRedoHelper.historyProjectionQuads(pointer) Is Nothing Then LPCFile.templateProjectionQuads.Clear() : LPCFile.templateProjectionQuads.AddRange(CloneHelper.DeepCloneProjectionQuads(historyProjectionQuads(pointer)))
            If Not UndoRedoHelper.historyTemplateText(pointer) Is Nothing Then LPCFile.templateTexts.Clear() : LPCFile.templateTexts.AddRange(CloneHelper.DeepCloneTemplateTexts(historyTemplateText(pointer)))
            MainForm.UndoToolStripMenuItem.Enabled = True
            UndoRedoHelper.cleanupHistoryData()
            UndoRedoHelper.cleanupUndoRedo()
        End If
        MainForm.RedoToolStripMenuItem.Enabled = pointer < (historyVertices.Count - 1)
    End Sub

    Public Shared Sub cleanupUndoRedo()

        If MainState.objectToModify = Modified.Primitive AndAlso View.SelectedTriangles.Count > 0 AndAlso View.SelectedTriangles(0).groupindex = -1 Then
            Helper_2D.clearSelection()
        End If

        If (MainState.objectToModify = Modified.Triangle Or MainState.objectToModify = Modified.Vertex) AndAlso View.SelectedTriangles.Count > 0 AndAlso View.SelectedTriangles(0).groupindex > -1 Then
            Helper_2D.clearSelection()
        End If

        MainForm.BtnAddToGroup.Enabled = (MainState.objectToModify = Modified.Triangle Or MainState.objectToModify = Modified.Vertex) AndAlso View.SelectedTriangles.Count > 0
        MainForm.BtnUngroup.Enabled = MainState.objectToModify = Modified.Primitive AndAlso View.SelectedTriangles.Count > 0 AndAlso View.SelectedTriangles(0).groupindex > -1
    End Sub

    Public Shared Sub cleanupHistoryData()

        Dim VIDtoVI As New Dictionary(Of Integer, Integer)(LPCFile.Vertices.Count)
        Dim TIDtoTI As New Dictionary(Of Integer, Integer)(LPCFile.Triangles.Count)

        Dim vcount As Integer = LPCFile.Vertices.Count - 1
        For i As Integer = 0 To vcount
            Dim oldVertexId As Integer = LPCFile.Vertices(i).vertexID
            If VIDtoVI.ContainsKey(oldVertexId) Then
                GlobalIdSet.vertexIDglobal += 1
                LPCFile.Vertices(i).vertexID = GlobalIdSet.vertexIDglobal
                VIDtoVI.Add(LPCFile.Vertices(i).vertexID, i)
                If LPCFile.PrimitivesHMap.ContainsKey(LPCFile.Vertices(i).groupindex) Then
                    Dim p As Primitive = LPCFile.Primitives(LPCFile.PrimitivesHMap(LPCFile.Vertices(i).groupindex))
                    If p.centerVertexID = oldVertexId Then
                        p.centerVertexID = LPCFile.Vertices(i).vertexID
                    End If
                End If
            Else
                VIDtoVI.Add(LPCFile.Vertices(i).vertexID, i)
            End If
            LPCFile.Vertices(i).linkedTriangles.Clear()
        Next

        Dim tcount As Integer = LPCFile.Triangles.Count - 1
        For i As Integer = 0 To tcount
            If TIDtoTI.ContainsKey(LPCFile.Triangles(i).triangleID) Then
                GlobalIdSet.triangleIDglobal += 1
                LPCFile.Triangles(i).triangleID = GlobalIdSet.triangleIDglobal
                TIDtoTI.Add(LPCFile.Triangles(i).triangleID, i)
            Else
                TIDtoTI.Add(LPCFile.Triangles(i).triangleID, i)
            End If
        Next

        Dim start As Integer = 0
newDelete:
        For i As Integer = start To tcount
            Dim vi1 As Integer
            Dim vi2 As Integer
            Dim vi3 As Integer
            If VIDtoVI.TryGetValue(LPCFile.Triangles(i).vertexA.vertexID, vi1) AndAlso
               VIDtoVI.TryGetValue(LPCFile.Triangles(i).vertexB.vertexID, vi2) AndAlso
               VIDtoVI.TryGetValue(LPCFile.Triangles(i).vertexC.vertexID, vi3) Then
                LPCFile.Triangles(i).vertexA = LPCFile.Vertices(vi1)
                LPCFile.Triangles(i).vertexB = LPCFile.Vertices(vi2)
                LPCFile.Triangles(i).vertexC = LPCFile.Vertices(vi3)
                LPCFile.Triangles(i).vertexA.linkedTriangles.Add(LPCFile.Triangles(i))
                LPCFile.Triangles(i).vertexB.linkedTriangles.Add(LPCFile.Triangles(i))
                LPCFile.Triangles(i).vertexC.linkedTriangles.Add(LPCFile.Triangles(i))
            Else
                start = i
                LPCFile.Triangles.RemoveAt(i)
                tcount -= 1
                GoTo newDelete
            End If
        Next

        Dim svcount As Integer = View.SelectedVertices.Count - 1
        start = 0
newDelete2:
        For i As Integer = start To svcount
            Dim vi1 As Integer
            If VIDtoVI.TryGetValue(View.SelectedVertices(i).vertexID, vi1) Then
                View.SelectedVertices(i) = LPCFile.Vertices(vi1)
                View.SelectedVertices(i).selected = True
            Else
                svcount -= 1
                start = i
                View.SelectedVertices.RemoveAt(i)
                GoTo newDelete2
            End If
        Next

        Dim stcount As Integer = View.SelectedTriangles.Count - 1
        start = 0
newDelete3:
        For i As Integer = start To stcount
            Dim ti1 As Integer
            If TIDtoTI.TryGetValue(View.SelectedTriangles(i).triangleID, ti1) Then
                View.SelectedTriangles(i) = LPCFile.Triangles(ti1)
                View.SelectedTriangles(i).selected = True
            Else
                stcount -= 1
                start = i
                View.SelectedTriangles.RemoveAt(i)
                GoTo newDelete3
            End If
        Next

    End Sub

    Public Shared Sub clearHistory()
        MainForm.UndoToolStripMenuItem.Enabled = False
        MainForm.RedoToolStripMenuItem.Enabled = False
        UndoRedoHelper.historyVertices.Clear()
        UndoRedoHelper.historyTriangles.Clear()
        UndoRedoHelper.historySelectedVertices.Clear()
        UndoRedoHelper.historySelectedTriangles.Clear()
        UndoRedoHelper.historyPrimitives.Clear()
        UndoRedoHelper.historyPrimitivesHash.Clear()
        UndoRedoHelper.historyPrimitivesMetadataHash.Clear()
        UndoRedoHelper.historyTemplateShape.Clear()
        UndoRedoHelper.historyMyMetadata.Clear()
        UndoRedoHelper.historyProjectionQuads.Clear()
        UndoRedoHelper.historyTemplateText.Clear()
        UndoRedoHelper.pointer = 0
    End Sub

End Class
