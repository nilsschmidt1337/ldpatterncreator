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

Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports LDPatternCreator

Public Class CloneHelper

    Public Shared Function DeepClone(ByVal source As Object) As Object
        Using m As New MemoryStream() With {.Capacity = 102400}
            Dim b As New BinaryFormatter()
            b.Serialize(m, source)
            m.Position = 0
            Return b.UnsafeDeserialize(m, Nothing)
        End Using
    End Function

    Public Shared Function DeepCloneVertices(ByRef vl As List(Of Vertex)) As List(Of Vertex)
        Dim rl As New List(Of Vertex) With {.Capacity = vl.Count + 1}
        For Each v As Vertex In vl
            Dim tv As New Vertex() With {.vertexID = v.vertexID, .groupindex = v.groupindex, .X = v.X, .Y = v.Y, .selected = v.selected}
            rl.Add(tv)
        Next
        Return rl
    End Function

    Public Shared Function DeepCloneTriangles(ByRef vl As List(Of Triangle)) As List(Of Triangle)
        Dim rl As New List(Of Triangle) With {.Capacity = vl.Count + 1}
        For Each t As Triangle In vl
            Dim tt As New Triangle() With {.triangleID = t.triangleID, .groupindex = t.groupindex, .selected = t.selected, .myColour = t.myColour, .myColourNumber = t.myColourNumber,
                                           .vertexA = New Vertex() With {.vertexID = t.vertexA.vertexID},
                                           .vertexB = New Vertex() With {.vertexID = t.vertexB.vertexID},
                                           .vertexC = New Vertex() With {.vertexID = t.vertexC.vertexID}}
            rl.Add(tt)
        Next
        Return rl
    End Function

    Public Shared Function DeepClonePrimitives(ByRef vl As List(Of Primitive)) As List(Of Primitive)
        Dim rl As New List(Of Primitive) With {.Capacity = vl.Count + 1}
        For Each p As Primitive In vl
            Dim matrix(3, 3) As Double
            Dim matrixR(3, 3) As Double
            For x As Integer = 0 To 3
                For y As Integer = 0 To 3
                    matrix(x, y) = p.matrix(x, y)
                    matrixR(x, y) = p.matrixR(x, y)
                Next
            Next
            Dim pp As New Primitive() With {.centerVertexID = p.centerVertexID, .ox = p.ox, .oy = p.oy, .myColour = p.myColour, .myColourNumber = p.myColourNumber,
                                           .primitiveID = p.primitiveID,
                                           .primitiveName = p.primitiveName,
                                           .matrix = matrix,
                                           .matrixR = matrixR}
            rl.Add(pp)
        Next
        Return rl
    End Function

    Public Shared Function DeepClonePrimitivesHMap(vd As Dictionary(Of Integer, Integer)) As Dictionary(Of Integer, Integer)
        Dim rd As New Dictionary(Of Integer, Integer)(vd.Count + 1)
        For Each k As Integer In vd.Keys
            rd.Add(k, vd(k))
        Next
        Return rd
    End Function

    Public Shared Function DeepClonePrimitivesMetadataHMap(vd As Dictionary(Of String, Metadata)) As Dictionary(Of String, Metadata)
        Dim rd As New Dictionary(Of String, Metadata)
        Dim keys As New List(Of String)
        keys.AddRange(vd.Keys)
        For Each k As String In keys
            rd.Add(k, DeepCloneMetadata(vd(k)))
        Next
        Return rd
    End Function

    Public Shared Function DeepCloneMetadata(ByRef vm As Metadata) As Metadata
        If vm Is Nothing Then
            Return Nothing
        End If
        Return vm.Clone
    End Function

    Public Shared Function DeepCloneTemplateShape(vts As List(Of PointF)) As List(Of PointF)
        Dim rts As New List(Of PointF)
        For Each p As PointF In vts
            rts.Add(New PointF(p.X, p.Y))
        Next
        Return rts
    End Function

    Friend Shared Function DeepCloneTemplateTexts(vt As List(Of TemplateTextInfo)) As List(Of TemplateTextInfo)
        Dim rt As New List(Of TemplateTextInfo)
        For Each t As TemplateTextInfo In vt
            rt.Add(New TemplateTextInfo(t.X, t.Y, t.Text))
        Next
        Return rt
    End Function

    Public Shared Function DeepCloneProjectionQuads(vp As List(Of ProjectionQuad)) As List(Of ProjectionQuad)
        Dim rp As New List(Of ProjectionQuad)
        For Each q As ProjectionQuad In vp
            rp.Add(New ProjectionQuad() With {.isTriangle = q.isTriangle, .inCoords = q.inCoords.Clone, .outCoords = q.outCoords.Clone, .matrix = q.matrix.Clone})
        Next
        Return rp
    End Function
End Class
