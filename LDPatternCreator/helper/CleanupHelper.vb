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

Imports System.Text

Module CleanupHelper

    Public Sub cleanupOldData()
        Dim VIDtoVI As New Hashtable(LPCFile.Vertices.Count)
        Dim TIDtoTI As New Hashtable(LPCFile.Triangles.Count)

        For i As Integer = 0 To LPCFile.Vertices.Count - 1
            VIDtoVI.Add(LPCFile.Vertices(i).vertexID, i)
        Next

        For i As Integer = 0 To LPCFile.Triangles.Count - 1
            TIDtoTI.Add(LPCFile.Triangles(i).triangleID, i)
        Next

        Dim start As Integer
        Dim start2 As Integer
nextVert:
        For i As Integer = start To LPCFile.Vertices.Count - 1
            start2 = 0
nextTri:
            For j As Integer = start2 To LPCFile.Vertices(i).linkedTriangles.Count - 1
                If Not TIDtoTI.Contains(LPCFile.Vertices(i).linkedTriangles(j).triangleID) Then
                    LPCFile.Vertices(i).linkedTriangles.RemoveAt(j)
                    GoTo nextTri
                End If
            Next
        Next

    End Sub

    Public Sub cleanupDATTriangles(Optional ByVal s As Integer = 0)
        Dim start As Integer = s
        For i As Integer = start To LPCFile.Triangles.Count - 1
            LPCFile.Triangles(i).normalize()
        Next
        Dim triHM As New Dictionary(Of String, Byte)
        Const epsilon As Double = 0.00001
nextTri:
        For i1 As Integer = start To LPCFile.Triangles.Count - 1
            Dim tri As Triangle = LPCFile.Triangles(i1)
            If tri.groupindex = -1 Then
                Dim fingerprint As String = tri.getFingerprint
                If triHM.ContainsKey(fingerprint) Then
                    Helper_2D.removeTriangle(tri, i1)
                    start = i1
                    GoTo nextTri
                Else
                    If Math.Abs(tri.vertexA.X - tri.vertexB.X) < epsilon AndAlso Math.Abs(tri.vertexA.Y - tri.vertexB.Y) < epsilon Then Helper_2D.removeTriangle(tri, i1) : start = i1 : GoTo nextTri
                    If Math.Abs(tri.vertexA.X - tri.vertexC.X) < epsilon AndAlso Math.Abs(tri.vertexA.Y - tri.vertexC.Y) < epsilon Then Helper_2D.removeTriangle(tri, i1) : start = i1 : GoTo nextTri
                    If Math.Abs(tri.vertexB.X - tri.vertexC.X) < epsilon AndAlso Math.Abs(tri.vertexB.Y - tri.vertexC.Y) < epsilon Then Helper_2D.removeTriangle(tri, i1) : start = i1 : GoTo nextTri
                    triHM.Add(fingerprint, 0)
                End If
            End If
        Next
    End Sub

    Public Sub cleanupDATTriangles3()
        For i As Integer = 0 To View.SelectedTriangles.Count - 1
            View.SelectedTriangles(i).normalize()
        Next
        Dim triHM As New Dictionary(Of String, Byte)
        For i1 As Integer = 0 To View.SelectedTriangles.Count - 1
            Dim tri As Triangle = View.SelectedTriangles(i1)
            If tri.groupindex = -1 Then
                Dim fingerprint As String = tri.getFingerprint
                If triHM.ContainsKey(fingerprint) Then
                    View.CollisionVertices.Add(tri.vertexA)
                    View.CollisionVertices.Add(tri.vertexB)
                    View.CollisionVertices.Add(tri.vertexC)
                Else
                    triHM.Add(fingerprint, 0)
                End If
            End If
        Next
    End Sub

    Public Sub cleanupDATVertices(Optional ByVal s As Integer = 0, Optional ByVal s2 As Integer = 0)
        Dim start As Integer = s
        Dim tc As Char = Chr(156)
        Dim sb As New StringBuilder
        Dim vertDict As New Dictionary(Of String, Vertex)
        Dim TIDtoTI As New Dictionary(Of Integer, Integer)

        For i As Integer = s2 To LPCFile.Triangles.Count - 1
            TIDtoTI.Add(LPCFile.Triangles(i).triangleID, i)
        Next
nextVert:
        For i1 As Integer = start To LPCFile.Vertices.Count - 1

            Dim vert As Vertex = LPCFile.Vertices(i1)
            sb.Remove(0, sb.Length)
            sb.Append(Math.Round(vert.X, 1))
            sb.Append(tc)
            sb.Append(Math.Round(vert.Y, 1))

            If vertDict.ContainsKey(sb.ToString) Then
                Dim vert2 As Vertex = vertDict(sb.ToString)
                ' Beide dürfen keine Primitive sein
                ' Beide dürfen fix sein
                If Not (vert.groupindex <> Primitive.NO_INDEX AndAlso vert2.groupindex <> Primitive.NO_INDEX) _
                    OrElse (vert.groupindex = Primitive.TEMPLATE_INDEX AndAlso vert2.groupindex = Primitive.TEMPLATE_INDEX) Then

                    ' Wenn einer Primitive Vertex / fix ist, dann merge den normalen Vertex an das Primitive / den Fixpunkt
                    If vert.groupindex <> -1 Then vert2.groupindex = vert.groupindex

                    For i As Integer = 0 To vert.linkedTriangles.Count - 1
                        Dim tri As Triangle = vert.linkedTriangles(i)
                        If Not vert2.linkedTriangles.Contains(LPCFile.Triangles(TIDtoTI(tri.triangleID))) Then
                            vert2.linkedTriangles.Add(LPCFile.Triangles(TIDtoTI(tri.triangleID)))
                        End If
                        If tri.vertexA.Equals(vert) Then LPCFile.Triangles(TIDtoTI(tri.triangleID)).vertexA = vert2
                        If tri.vertexB.Equals(vert) Then LPCFile.Triangles(TIDtoTI(tri.triangleID)).vertexB = vert2
                        If tri.vertexC.Equals(vert) Then LPCFile.Triangles(TIDtoTI(tri.triangleID)).vertexC = vert2
                    Next

                    LPCFile.Vertices.RemoveAt(i1)
                    start = i1
                    GoTo nextVert
                End If
            Else
                vertDict.Add(sb.ToString, vert)
            End If
        Next

    End Sub

End Module
