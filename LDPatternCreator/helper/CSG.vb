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
Public Class CSG

    Public Shared beamdir() As Decimal = {0, 0, -1}
    Public Shared beamorig(2) As Decimal
    Public Shared schnittpunkt As New Vertex(0, 0, False, False)

    Public Shared Sub subdivideTriangle(ByRef tri As Triangle)
        tri.normalize()
        tri.checkWinding()
        Dim tvA As Vertex = tri.vertexA
        Dim tvB As Vertex = tri.vertexB
        Dim tvC As Vertex = tri.vertexC

        View.SelectedTriangles.Remove(tri)
        Helper_2D.removeTriangle(tri)

        Dim center1 As New Vertex((tvA.X + tvB.X) / 2, (tvA.Y + tvB.Y) / 2, False)
        LPCFile.Vertices.Add(center1)
        Dim center2 As New Vertex((tvB.X + tvC.X) / 2, (tvB.Y + tvC.Y) / 2, False)
        LPCFile.Vertices.Add(center2)
        Dim center3 As New Vertex((tvC.X + tvA.X) / 2, (tvC.Y + tvA.Y) / 2, False)
        LPCFile.Vertices.Add(center3)

        LPCFile.Triangles.Add(New Triangle(center1, center2, center3) With {.myColour = tri.myColour, .myColourNumber = tri.myColourNumber})
        center1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        center2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        center3.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))

        View.SelectedVertices.Clear()
        View.SelectedVertices.Add(tvA)
        View.SelectedVertices.Add(tvB)
        splitTriangle(tvA, tvB)
        View.SelectedVertices.Clear()
        View.SelectedVertices.Add(tvB)
        View.SelectedVertices.Add(tvC)
        splitTriangle(tvB, tvC)
        View.SelectedVertices.Clear()
        View.SelectedVertices.Add(tvC)
        View.SelectedVertices.Add(tvA)
        splitTriangle(tvC, tvA)

        LPCFile.Triangles.Add(New Triangle(center1, center2, tvB) With {.myColour = tri.myColour, .myColourNumber = tri.myColourNumber})
        center1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        center2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        tvB.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))

        LPCFile.Triangles.Add(New Triangle(center2, center3, tvC) With {.myColour = tri.myColour, .myColourNumber = tri.myColourNumber})
        center2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        center3.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        tvC.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))

        LPCFile.Triangles.Add(New Triangle(center3, center1, tvA) With {.myColour = tri.myColour, .myColourNumber = tri.myColourNumber})
        center3.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        center1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
        tvA.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
    End Sub

    Public Shared Sub splitTriangle(ByRef v1 As Vertex, ByRef v2 As Vertex)
        Dim tri1 As Triangle = Nothing
        Dim tri2 As Triangle = Nothing
        Do
            For i As Integer = 0 To View.SelectedVertices(0).linkedTriangles.Count - 1
                For j As Integer = 0 To View.SelectedVertices(1).linkedTriangles.Count - 1
                    If View.SelectedVertices(0).linkedTriangles(i).triangleID = View.SelectedVertices(1).linkedTriangles(j).triangleID Then
                        If tri1 Is Nothing Then
                            tri1 = View.SelectedVertices(0).linkedTriangles(i)
                        ElseIf tri2 Is Nothing Then
                            tri2 = View.SelectedVertices(0).linkedTriangles(i)
                        End If
                        If Not tri1 Is Nothing AndAlso Not tri2 Is Nothing Then Exit Do
                    End If
                Next
            Next
            Exit Do
        Loop
        If Not tri1 Is Nothing AndAlso Not tri2 Is Nothing Then
            Dim center As Vertex = New Vertex((v1.X + v2.X) / 2, (v1.Y + v2.Y) / 2, False)
            LPCFile.Vertices.Add(center)
            Dim tv1 As Vertex = Nothing
            If v1.vertexID = tri1.vertexA.vertexID Then
                If v2.vertexID = tri1.vertexB.vertexID Then
                    tv1 = tri1.vertexC
                ElseIf v2.vertexID = tri1.vertexC.vertexID Then
                    tv1 = tri1.vertexB
                End If
            ElseIf v1.vertexID = tri1.vertexB.vertexID Then
                If v2.vertexID = tri1.vertexA.vertexID Then
                    tv1 = tri1.vertexC
                ElseIf v2.vertexID = tri1.vertexC.vertexID Then
                    tv1 = tri1.vertexA
                End If
            ElseIf v1.vertexID = tri1.vertexC.vertexID Then
                If v2.vertexID = tri1.vertexB.vertexID Then
                    tv1 = tri1.vertexA
                ElseIf v2.vertexID = tri1.vertexA.vertexID Then
                    tv1 = tri1.vertexB
                End If
            End If
            Dim tv2 As Vertex = Nothing
            If v1.vertexID = tri2.vertexA.vertexID Then
                If v2.vertexID = tri2.vertexB.vertexID Then
                    tv2 = tri2.vertexC
                ElseIf v2.vertexID = tri2.vertexC.vertexID Then
                    tv2 = tri2.vertexB
                End If
            ElseIf v1.vertexID = tri2.vertexB.vertexID Then
                If v2.vertexID = tri2.vertexA.vertexID Then
                    tv2 = tri2.vertexC
                ElseIf v2.vertexID = tri2.vertexC.vertexID Then
                    tv2 = tri2.vertexA
                End If
            ElseIf v1.vertexID = tri2.vertexC.vertexID Then
                If v2.vertexID = tri2.vertexB.vertexID Then
                    tv2 = tri2.vertexA
                ElseIf v2.vertexID = tri2.vertexA.vertexID Then
                    tv2 = tri2.vertexB
                End If
            End If
            If Not tv1 Is Nothing Then
                LPCFile.Triangles.Add(New Triangle(v1, tv1, center) With {.myColour = tri1.myColour, .myColourNumber = tri1.myColourNumber})
                ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                v1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                tv1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                LPCFile.Triangles.Add(New Triangle(v2, tv1, center) With {.myColour = tri1.myColour, .myColourNumber = tri1.myColourNumber})
                ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                v2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                tv1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            End If
            If Not tv2 Is Nothing Then
                LPCFile.Triangles.Add(New Triangle(v1, tv2, center) With {.myColour = tri2.myColour, .myColourNumber = tri2.myColourNumber})
                ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                v1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                tv2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                LPCFile.Triangles.Add(New Triangle(v2, tv2, center) With {.myColour = tri2.myColour, .myColourNumber = tri2.myColourNumber})
                ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                v2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                tv2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            End If
            Helper_2D.removeTriangle(tri1)
            Helper_2D.removeTriangle(tri2)
        ElseIf Not tri1 Is Nothing Then
            Dim center As Vertex = New Vertex((v1.X + v2.X) / 2, (v1.Y + v2.Y) / 2, False)
            LPCFile.Vertices.Add(center)
            Dim tv1 As Vertex = Nothing
            If v1.vertexID = tri1.vertexA.vertexID Then
                If v2.vertexID = tri1.vertexB.vertexID Then
                    tv1 = tri1.vertexC
                ElseIf v2.vertexID = tri1.vertexC.vertexID Then
                    tv1 = tri1.vertexB
                End If
            ElseIf v1.vertexID = tri1.vertexB.vertexID Then
                If v2.vertexID = tri1.vertexA.vertexID Then
                    tv1 = tri1.vertexC
                ElseIf v2.vertexID = tri1.vertexC.vertexID Then
                    tv1 = tri1.vertexA
                End If
            ElseIf v1.vertexID = tri1.vertexC.vertexID Then
                If v2.vertexID = tri1.vertexB.vertexID Then
                    tv1 = tri1.vertexA
                ElseIf v2.vertexID = tri1.vertexA.vertexID Then
                    tv1 = tri1.vertexB
                End If
            End If
            LPCFile.Triangles.Add(New Triangle(v1, tv1, center) With {.myColour = tri1.myColour, .myColourNumber = tri1.myColourNumber})
            ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            v1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            tv1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            LPCFile.Triangles.Add(New Triangle(v2, tv1, center) With {.myColour = tri1.myColour, .myColourNumber = tri1.myColourNumber})
            ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            v2.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            tv1.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            Helper_2D.removeTriangle(tri1)
        End If
    End Sub

    Public Shared Function intersectionBetweenTwoLines(ByRef v11 As Vertex, ByRef v12 As Vertex, ByRef v21 As Vertex, ByRef v22 As Vertex) As Vertex
        If (v11.X = v21.X AndAlso v11.Y = v21.Y) OrElse (v12.X = v22.X AndAlso v12.Y = v22.Y) Then Return Nothing
        If (v11.X = v22.X AndAlso v11.Y = v22.Y) OrElse (v12.X = v21.X AndAlso v12.Y = v21.Y) Then Return Nothing

        Dim s1_x, s1_y, s2_x, s2_y As Double
        s1_x = v12.X - v11.X
        s1_y = v12.Y - v11.Y

        s2_x = v22.X - v21.X
        s2_y = v22.Y - v21.Y

        Dim s, t As Double
        s = (-s1_y * (v11.X - v21.X) + s1_x * (v11.Y - v21.Y)) / (-s2_x * s1_y + s1_x * s2_y)
        t = (s2_x * (v11.Y - v21.Y) - s2_y * (v11.X - v21.X)) / (-s2_x * s1_y + s1_x * s2_y)

        If s > 0 AndAlso s < 1 AndAlso t > 0 AndAlso t < 1 Then
            Return New Vertex(v11.X + (t * s1_x), v11.Y + (t * s1_y), False, False)
        End If

        Return Nothing
    End Function

    Public Shared Function intersectionBetweenTwoLinesStrict(ByRef v11 As Vertex, ByRef v12 As Vertex, ByRef v21 As Vertex, ByRef v22 As Vertex) As Vertex
        If (v11.X = v21.X AndAlso v11.Y = v21.Y) OrElse (v12.X = v22.X AndAlso v12.Y = v22.Y) Then Return Nothing
        If (v11.X = v22.X AndAlso v11.Y = v22.Y) OrElse (v12.X = v21.X AndAlso v12.Y = v21.Y) Then Return Nothing

        Dim s1_x, s1_y, s2_x, s2_y As Double
        s1_x = v12.X - v11.X
        s1_y = v12.Y - v11.Y

        s2_x = v22.X - v21.X
        s2_y = v22.Y - v21.Y

        Dim s, t As Double
        s = (-s1_y * (v11.X - v21.X) + s1_x * (v11.Y - v21.Y)) / (-s2_x * s1_y + s1_x * s2_y)
        t = (s2_x * (v11.Y - v21.Y) - s2_y * (v11.X - v21.X)) / (-s2_x * s1_y + s1_x * s2_y)

        If s >= 0 AndAlso s <= 1 AndAlso t >= 0 AndAlso t <= 1 Then
            Return New Vertex(v11.X + (t * s1_x), v11.Y + (t * s1_y), False, False)
        End If

        Return Nothing
    End Function

    Public Shared Function intersectionLineBetweenTriangleAndLine(ByRef tri As Triangle, ByRef v1 As Vertex, ByRef v2 As Vertex) As Line
        Dim v1inT As Boolean = CSG.isVertexInTriangle(v1, tri)
        Dim v2inT As Boolean = CSG.isVertexInTriangle(v2, tri)
        Dim innerVertex As Vertex = Nothing
        Dim outerVertex As Vertex = Nothing
        If v1inT AndAlso v2inT Then Return Nothing
        Dim intersectVertex As Vertex = Nothing
        If v1inT = v2inT Then
            Dim ic As Integer = -1
            Dim intersections(1) As Vertex
            intersectVertex = intersectionBetweenTwoLinesStrict(tri.vertexA, tri.vertexB, v1, v2)
            If Not intersectVertex Is Nothing Then ic += 1 : intersections(ic) = intersectVertex
            intersectVertex = intersectionBetweenTwoLinesStrict(tri.vertexA, tri.vertexC, v1, v2)
            If Not intersectVertex Is Nothing Then ic += 1 : intersections(ic) = intersectVertex
            intersectVertex = intersectionBetweenTwoLinesStrict(tri.vertexC, tri.vertexB, v1, v2)
            If Not intersectVertex Is Nothing AndAlso ic < 1 Then ic += 1 : intersections(ic) = intersectVertex
            If ic = 1 Then
                Return New Line(intersections(0), intersections(1))
            End If
            Return Nothing
        End If
        If v1inT Then innerVertex = v1 : outerVertex = v2
        If v2inT Then innerVertex = v2 : outerVertex = v1
        intersectVertex = intersectionBetweenTwoLinesStrict(tri.vertexA, tri.vertexB, innerVertex, outerVertex)
        If Not intersectVertex Is Nothing Then Return New Line(innerVertex, intersectVertex)
        intersectVertex = intersectionBetweenTwoLinesStrict(tri.vertexA, tri.vertexC, innerVertex, outerVertex)
        If Not intersectVertex Is Nothing Then Return New Line(innerVertex, intersectVertex)
        intersectVertex = intersectionBetweenTwoLinesStrict(tri.vertexC, tri.vertexB, innerVertex, outerVertex)
        If Not intersectVertex Is Nothing Then Return New Line(innerVertex, intersectVertex)
        Return Nothing
    End Function

    Public Shared Function intersectionLineBetweenTriangleAndLine(ByRef tri As Triangle, ByRef line As Line) As Line
        Return intersectionLineBetweenTriangleAndLine(tri, line.v(0), line.v(1))
    End Function

    Public Shared Function pointsConnected(ByRef v1 As Vertex, ByRef v2 As Vertex) As Boolean
        Dim ht As New Dictionary(Of Triangle, Byte)
        Dim c As Integer
        For Each tri As Triangle In v1.linkedTriangles
            If Not ht.ContainsKey(tri) Then
                ht.Add(tri, 0)
            End If
        Next
        For Each tri As Triangle In v2.linkedTriangles
            If ht.ContainsKey(tri) Then c += 1
            If c > 1 Then Return True
        Next
        Return False
    End Function

    Public Shared Function pointsConnected(ByRef v1 As Vertex, ByRef v2 As Vertex, ByRef v3 As Vertex) As Boolean
        Dim ht As New Dictionary(Of Triangle, Byte)
        Dim ht2 As New Dictionary(Of Triangle, Byte)
        Dim c As Integer
        For Each tri As Triangle In v1.linkedTriangles
            If Not ht.ContainsKey(tri) Then
                ht.Add(tri, 0)
            End If
        Next
        For Each tri As Triangle In v2.linkedTriangles
            If ht.ContainsKey(tri) Then
                If Not ht2.ContainsKey(tri) Then ht2.Add(tri, 0)
                c = 1
            End If
        Next
        If c = 0 Then Return False
        For Each tri As Triangle In v3.linkedTriangles
            If ht.ContainsKey(tri) AndAlso ht2.ContainsKey(tri) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Shared Sub rotateTriangles()
        Dim commonVertices As New List(Of Vertex) With {.Capacity = 5}
        Dim otherVertices As New List(Of Vertex) With {.Capacity = 5}
        Dim allVertices As New List(Of Vertex) With {.Capacity = 5}
        allVertices.Add(View.SelectedTriangles(0).vertexA)
        allVertices.Add(View.SelectedTriangles(0).vertexB)
        allVertices.Add(View.SelectedTriangles(0).vertexC)
        If Not allVertices.Contains(View.SelectedTriangles(1).vertexA) Then allVertices.Add(View.SelectedTriangles(1).vertexA) : otherVertices.Add(View.SelectedTriangles(1).vertexA) Else commonVertices.Add(View.SelectedTriangles(1).vertexA)
        If Not allVertices.Contains(View.SelectedTriangles(1).vertexB) Then allVertices.Add(View.SelectedTriangles(1).vertexB) : otherVertices.Add(View.SelectedTriangles(1).vertexB) Else commonVertices.Add(View.SelectedTriangles(1).vertexB)
        If Not allVertices.Contains(View.SelectedTriangles(1).vertexC) Then allVertices.Add(View.SelectedTriangles(1).vertexC) : otherVertices.Add(View.SelectedTriangles(1).vertexC) Else commonVertices.Add(View.SelectedTriangles(1).vertexC)
        If commonVertices.Count = 2 AndAlso otherVertices.Count = 1 Then
            allVertices.Remove(commonVertices(0))
            allVertices.Remove(commonVertices(1))
            allVertices.Remove(otherVertices(0))
            otherVertices.Add(allVertices(0))
            For t_i As Integer = 0 To 1
                For t_j As Integer = 0 To 1
                    View.SelectedTriangles(t_i).vertexA.linkedTriangles.Remove(View.SelectedTriangles(t_j))
                    View.SelectedTriangles(t_i).vertexB.linkedTriangles.Remove(View.SelectedTriangles(t_j))
                    View.SelectedTriangles(t_i).vertexC.linkedTriangles.Remove(View.SelectedTriangles(t_j))
                Next
            Next
            View.SelectedTriangles(0).vertexA = otherVertices(0)
            View.SelectedTriangles(0).vertexB = otherVertices(1)
            View.SelectedTriangles(0).vertexC = commonVertices(0)
            View.SelectedTriangles(1).vertexA = otherVertices(0)
            View.SelectedTriangles(1).vertexB = otherVertices(1)
            View.SelectedTriangles(1).vertexC = commonVertices(1)
            For t_i As Integer = 0 To 1
                View.SelectedTriangles(t_i).vertexA.linkedTriangles.Add(View.SelectedTriangles(t_i))
                View.SelectedTriangles(t_i).vertexB.linkedTriangles.Add(View.SelectedTriangles(t_i))
                View.SelectedTriangles(t_i).vertexC.linkedTriangles.Add(View.SelectedTriangles(t_i))
            Next
        End If
    End Sub

    Public Shared Function unify2Triangles(ByRef tri1 As Triangle, ByRef tri2 As Triangle) As Boolean

        Dim verticesIntersections As New List(Of Vertex)
        Dim verticesInner As New List(Of Vertex)
        Dim verticesOuter As New List(Of Vertex)
        Dim verticesComplete As New List(Of Vertex)


        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexA, tri1.vertexB, tri2.vertexA, tri2.vertexB)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))
        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexA, tri1.vertexB, tri2.vertexB, tri2.vertexC)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))
        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexA, tri1.vertexB, tri2.vertexC, tri2.vertexA)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))

        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexB, tri1.vertexC, tri2.vertexA, tri2.vertexB)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))
        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexB, tri1.vertexC, tri2.vertexB, tri2.vertexC)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))
        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexB, tri1.vertexC, tri2.vertexC, tri2.vertexA)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))

        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexC, tri1.vertexA, tri2.vertexA, tri2.vertexB)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))
        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexC, tri1.vertexA, tri2.vertexB, tri2.vertexC)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))
        schnittpunkt = intersectionBetweenTwoLines(tri1.vertexC, tri1.vertexA, tri2.vertexC, tri2.vertexA)
        If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False))

        ' Bestimme innere Punkte:

        Dim vert0(2) As Decimal
        Dim vert1(2) As Decimal
        Dim vert2(2) As Decimal
        vert0(2) = 0 : vert1(2) = 0 : vert2(2) = 0 : CSG.beamorig(2) = 1
        ' -> Test auf Dreieck 1
        vert0(0) = tri1.vertexA.X
        vert0(1) = tri1.vertexA.Y
        vert1(0) = tri1.vertexB.X
        vert1(1) = tri1.vertexB.Y
        vert2(0) = tri1.vertexC.X
        vert2(1) = tri1.vertexC.Y
        If tri2.vertexA <> tri1.vertexA AndAlso tri2.vertexA <> tri1.vertexB AndAlso tri2.vertexA <> tri1.vertexC Then
            CSG.beamorig(0) = tri2.vertexA.X : CSG.beamorig(1) = tri2.vertexA.Y
            If PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, beamdir, vert0, vert1, vert2) Then
                verticesInner.Add(tri2.vertexA)
                tri2.vertexA.selected = True
            End If
        End If
        If tri2.vertexB <> tri1.vertexA AndAlso tri2.vertexB <> tri1.vertexB AndAlso tri2.vertexB <> tri1.vertexC Then
            CSG.beamorig(0) = tri2.vertexB.X : CSG.beamorig(1) = tri2.vertexB.Y
            If PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, beamdir, vert0, vert1, vert2) Then
                verticesInner.Add(tri2.vertexB)
                tri2.vertexB.selected = True
            End If
        End If
        If tri2.vertexC <> tri1.vertexA AndAlso tri2.vertexC <> tri1.vertexB AndAlso tri2.vertexC <> tri1.vertexC Then
            CSG.beamorig(0) = tri2.vertexC.X : CSG.beamorig(1) = tri2.vertexC.Y
            If PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, beamdir, vert0, vert1, vert2) Then
                verticesInner.Add(tri2.vertexC)
                tri2.vertexC.selected = True
            End If
        End If

        ' -> Test auf Dreieck 2
        vert0(0) = tri2.vertexA.X
        vert0(1) = tri2.vertexA.Y
        vert1(0) = tri2.vertexB.X
        vert1(1) = tri2.vertexB.Y
        vert2(0) = tri2.vertexC.X
        vert2(1) = tri2.vertexC.Y
        If tri1.vertexA <> tri2.vertexA AndAlso tri1.vertexA <> tri2.vertexB AndAlso tri1.vertexA <> tri2.vertexC Then
            CSG.beamorig(0) = tri1.vertexA.X : CSG.beamorig(1) = tri1.vertexA.Y
            If PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, beamdir, vert0, vert1, vert2) Then
                verticesInner.Add(tri1.vertexA)
                tri1.vertexA.selected = True
            End If
        End If
        If tri1.vertexB <> tri2.vertexA AndAlso tri1.vertexB <> tri2.vertexB AndAlso tri1.vertexB <> tri2.vertexC Then
            CSG.beamorig(0) = tri1.vertexB.X : CSG.beamorig(1) = tri1.vertexB.Y
            If PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, beamdir, vert0, vert1, vert2) Then
                verticesInner.Add(tri1.vertexB)
                tri1.vertexB.selected = True
            End If
        End If
        If tri1.vertexC <> tri2.vertexA AndAlso tri1.vertexC <> tri2.vertexB AndAlso tri1.vertexC <> tri2.vertexC Then
            CSG.beamorig(0) = tri1.vertexC.X : CSG.beamorig(1) = tri1.vertexC.Y
            If PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, beamdir, vert0, vert1, vert2) Then
                verticesInner.Add(tri1.vertexC)
                tri1.vertexC.selected = True
            End If
        End If

        If verticesInner.Count > 0 OrElse verticesIntersections.Count > 0 Then

            ' Bestimme äußere Punkte:
            If Not verticesInner.Contains(tri1.vertexA) Then verticesOuter.Add(tri1.vertexA)
            If Not verticesInner.Contains(tri1.vertexB) Then verticesOuter.Add(tri1.vertexB)
            If Not verticesInner.Contains(tri1.vertexC) Then verticesOuter.Add(tri1.vertexC)

            If Not verticesInner.Contains(tri2.vertexA) Then verticesOuter.Add(tri2.vertexA)
            If Not verticesInner.Contains(tri2.vertexB) Then verticesOuter.Add(tri2.vertexB)
            If Not verticesInner.Contains(tri2.vertexC) Then verticesOuter.Add(tri2.vertexC)

            LPCFile.Vertices.AddRange(verticesIntersections)
            verticesInner.AddRange(verticesIntersections)
            verticesComplete.AddRange(verticesInner)
            verticesComplete.AddRange(verticesOuter)

            Dim iTriangles As New List(Of Triangle)
            For i1 As Integer = 0 To verticesComplete.Count - 1
                For i2 As Integer = i1 + 1 To verticesComplete.Count - 1
                    For i3 As Integer = i2 + 1 To verticesComplete.Count - 1
                        Dim ttri As New Triangle(verticesComplete(i1), verticesComplete(i2), verticesComplete(i3), False)
                        If Not isTriangleInTriangle(ttri, tri1) Then Continue For
                        If Not isTriangleInTriangle(ttri, tri2) Then Continue For
                        Dim i4 As Integer = 0
                        While i4 < iTriangles.Count
                            Dim tri As Triangle = iTriangles(i4)
                            If isPartlyTriangleInTriangle(ttri, tri) Then Continue For
                            i4 += 1
                        End While
                        If Not (ttri.vertexA = ttri.vertexB OrElse ttri.vertexA = ttri.vertexC OrElse ttri.vertexB = ttri.vertexC) Then
                            If tri1.triangleID > tri2.triangleID Then
                                LPCFile.Triangles.Add(New Triangle(verticesComplete(i3), verticesComplete(i1), verticesComplete(i2)) With {.myColour = tri1.myColour, .myColourNumber = tri1.myColourNumber})
                            Else
                                LPCFile.Triangles.Add(New Triangle(verticesComplete(i3), verticesComplete(i1), verticesComplete(i2)) With {.myColour = tri2.myColour, .myColourNumber = tri2.myColourNumber})
                            End If
                            verticesComplete(i3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            verticesComplete(i1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            verticesComplete(i2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            iTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                        Else
                            Continue For
                        End If
                    Next
                Next
            Next

            For i1 As Integer = 0 To verticesComplete.Count - 1
                For i2 As Integer = i1 + 1 To verticesComplete.Count - 1
                    For i3 As Integer = i2 + 1 To verticesComplete.Count - 1
                        Dim ttri As New Triangle(verticesComplete(i1), verticesComplete(i2), verticesComplete(i3), False)
                        If Not isTriangleInTriangle(ttri, tri1) AndAlso Not isTriangleInTriangle(ttri, tri2) Then Continue For
                        Dim i4 As Integer = 0
                        While i4 < iTriangles.Count
                            Dim tri As Triangle = iTriangles(i4)
                            If isPartlyTriangleInTriangle(ttri, tri) Then Continue For
                            i4 += 1
                        End While
                        If Not (ttri.vertexA = ttri.vertexB OrElse ttri.vertexA = ttri.vertexC OrElse ttri.vertexB = ttri.vertexC) Then
                            If isTriangleInTriangle(ttri, tri1) Then
                                LPCFile.Triangles.Add(New Triangle(verticesComplete(i3), verticesComplete(i1), verticesComplete(i2)) With {.myColour = tri1.myColour, .myColourNumber = tri1.myColourNumber})
                            Else
                                LPCFile.Triangles.Add(New Triangle(verticesComplete(i3), verticesComplete(i1), verticesComplete(i2)) With {.myColour = tri2.myColour, .myColourNumber = tri2.myColourNumber})
                            End If
                            verticesComplete(i3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            verticesComplete(i1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            verticesComplete(i2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            iTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                        Else
                            Continue For
                        End If
                    Next
                Next
            Next
            If iTriangles.Count > 0 Then
                View.SelectedTriangles.Remove(tri1)
                View.SelectedTriangles.Remove(tri2)
                Helper_2D.removeTriangle(tri1)
                Helper_2D.removeTriangle(tri2)
                Return True
            Else
                Return False
            End If
        End If
        Return False
    End Function

    Public Shared Function distanceVectorFromVertexToLine(ByRef v1 As Vertex, ByVal a As Vertex, ByVal b As Vertex) As Vertex
        Dim iter As Integer = 0
        Dim vt1 As Vertex = a
        Dim vt2 As Vertex = b
        Do
            If v1.dist(vt1) < v1.dist(vt2) Then
                vt2 = New Vertex((vt1.X + vt2.X) / 2.0, (vt1.Y + vt2.Y) / 2.0, False, False)
            Else
                vt1 = New Vertex((vt1.X + vt2.X) / 2.0, (vt1.Y + vt2.Y) / 2.0, False, False)
            End If
            iter += 1
        Loop Until iter = 160
        If v1.dist(vt1) < v1.dist(vt2) Then
            Return vt1
        Else
            Return vt2
        End If
    End Function

    Public Shared Function distanceSquareFromVertexToLine(ByRef v1 As Vertex, ByVal a As Vertex, ByVal b As Vertex) As Double        
        Return v1.dist(distanceVectorFromVertexToLine(v1, a, b))
    End Function

    Public Shared Function projectionQuadIntersectionVertex(ByRef v1 As Vertex, ByRef t1 As Triangle, ByRef q1 As ProjectionQuad) As Vertex
        Dim v As Vertex = Nothing
        If t1.vertexA.vertexID = v1.vertexID Then v = t1.vertexB
        If t1.vertexB.vertexID = v1.vertexID Then v = t1.vertexC
        If t1.vertexC.vertexID = v1.vertexID Then v = t1.vertexA
        Do
            schnittpunkt = intersectionBetweenTwoLines(v1, v, New Vertex(1000.0 * q1.inCoords(3, 0), 1000.0 * q1.inCoords(3, 1), False, False), New Vertex(1000.0 * q1.inCoords(0, 0), 1000.0 * q1.inCoords(0, 1), False, False))
            If Not schnittpunkt Is Nothing Then Exit Do
            For i = 0 To 2
                schnittpunkt = intersectionBetweenTwoLines(v1, v, New Vertex(1000.0 * q1.inCoords(i, 0), 1000.0 * q1.inCoords(i, 1), False, False), New Vertex(1000.0 * q1.inCoords(i + 1, 0), 1000.0 * q1.inCoords(i + 1, 1), False, False))
                If Not schnittpunkt Is Nothing Then Exit Do
            Next
            Return Nothing
        Loop
        Return schnittpunkt
    End Function

    Public Shared Function trianglesIntersectionsOnly(ByRef tri1 As Triangle, ByRef tri2 As Triangle, Optional ByVal output_vertices As Boolean = True) As Boolean

        Dim verticesIntersections As New List(Of Vertex)

        Dim ul1_x As Double = Math.Min(tri1.vertexA.X, Math.Min(tri1.vertexB.X, tri1.vertexC.X))
        Dim ul1_y As Double = Math.Min(tri1.vertexA.Y, Math.Min(tri1.vertexB.Y, tri1.vertexC.Y))
        Dim ul2_x As Double = Math.Min(tri2.vertexA.X, Math.Min(tri2.vertexB.X, tri2.vertexC.X))
        Dim ul2_y As Double = Math.Min(tri2.vertexA.Y, Math.Min(tri2.vertexB.Y, tri2.vertexC.Y))

        Dim w1 As Double = Math.Max(tri1.vertexA.X, Math.Max(tri1.vertexB.X, tri1.vertexC.X)) - ul1_x
        Dim h1 As Double = Math.Max(tri1.vertexA.Y, Math.Max(tri1.vertexB.Y, tri1.vertexC.Y)) - ul1_y
        Dim w2 As Double = Math.Max(tri2.vertexA.X, Math.Max(tri2.vertexB.X, tri2.vertexC.X)) - ul2_x
        Dim h2 As Double = Math.Max(tri2.vertexA.Y, Math.Max(tri2.vertexB.Y, tri2.vertexC.Y)) - ul2_y

        Dim r1 As New Rectangle(ul1_x, ul1_y, w1, h1)
        Dim r2 As New Rectangle(ul2_x, ul2_y, w2, h2)

        If r1.IntersectsWith(r2) OrElse r1.Contains(r2) OrElse r2.Contains(r1) Then
            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexA, tri1.vertexB, tri2.vertexA, tri2.vertexB)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))
            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexA, tri1.vertexB, tri2.vertexB, tri2.vertexC)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))
            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexA, tri1.vertexB, tri2.vertexC, tri2.vertexA)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))

            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexB, tri1.vertexC, tri2.vertexA, tri2.vertexB)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))
            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexB, tri1.vertexC, tri2.vertexB, tri2.vertexC)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))
            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexB, tri1.vertexC, tri2.vertexC, tri2.vertexA)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))

            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexC, tri1.vertexA, tri2.vertexA, tri2.vertexB)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))
            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexC, tri1.vertexA, tri2.vertexB, tri2.vertexC)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))
            schnittpunkt = intersectionBetweenTwoLines(tri1.vertexC, tri1.vertexA, tri2.vertexC, tri2.vertexA)
            If Not schnittpunkt Is Nothing Then verticesIntersections.Add(New Vertex(schnittpunkt.X, schnittpunkt.Y, False, output_vertices))


            If output_vertices Then
                LPCFile.Vertices.AddRange(verticesIntersections)
            Else
                Dim start As Integer = 0
newRemove:
                For i As Integer = start To verticesIntersections.Count - 1
                    Dim v As Vertex = verticesIntersections(i)
                    If tri1.vertexA.X = v.X AndAlso tri1.vertexA.Y = v.Y Then verticesIntersections.RemoveAt(i) : start = i : GoTo newRemove
                    If tri1.vertexB.X = v.X AndAlso tri1.vertexB.Y = v.Y Then verticesIntersections.RemoveAt(i) : start = i : GoTo newRemove
                    If tri1.vertexC.X = v.X AndAlso tri1.vertexC.Y = v.Y Then verticesIntersections.RemoveAt(i) : start = i : GoTo newRemove

                    If tri2.vertexA.X = v.X AndAlso tri2.vertexA.Y = v.Y Then verticesIntersections.RemoveAt(i) : start = i : GoTo newRemove
                    If tri2.vertexB.X = v.X AndAlso tri2.vertexB.Y = v.Y Then verticesIntersections.RemoveAt(i) : start = i : GoTo newRemove
                    If tri2.vertexC.X = v.X AndAlso tri2.vertexC.Y = v.Y Then verticesIntersections.RemoveAt(i) : start = i : GoTo newRemove
                Next
            End If
        End If

        Return verticesIntersections.Count > 0

    End Function

    Public Shared Function isVertexInTriangle(ByVal vert1 As Vertex, ByVal tri1 As Triangle) As Boolean
        Dim ul1_x As Double = Math.Min(tri1.vertexA.X, Math.Min(tri1.vertexB.X, tri1.vertexC.X))
        Dim ul1_y As Double = Math.Min(tri1.vertexA.Y, Math.Min(tri1.vertexB.Y, tri1.vertexC.Y))
        Dim w1 As Double = Math.Max(tri1.vertexA.X, Math.Max(tri1.vertexB.X, tri1.vertexC.X)) - ul1_x
        Dim h1 As Double = Math.Max(tri1.vertexA.Y, Math.Max(tri1.vertexB.Y, tri1.vertexC.Y)) - ul1_y
        Dim r1 As New Rectangle(ul1_x, ul1_y, w1, h1)
        If r1.Contains(vert1.X, vert1.Y) Then
            If Not tri1.vertexA.Equals(vert1) AndAlso Not tri1.vertexB.Equals(vert1) AndAlso Not tri1.vertexC.Equals(vert1) _
            AndAlso tri1.vertexA.dist(vert1) > 100 AndAlso tri1.vertexB.dist(vert1) > 100 AndAlso tri1.vertexC.dist(vert1) > 100 Then
                Dim tri(,) As Double = {{tri1.vertexA.X, tri1.vertexA.Y}, {tri1.vertexB.X, tri1.vertexB.Y}, {tri1.vertexC.X, tri1.vertexC.Y}}
                Dim beamdir() As Decimal = {0, 0, -1}
                Dim center() As Double = {(tri1.vertexA.X + tri1.vertexB.X + tri1.vertexC.X) / 3, (tri1.vertexA.Y + tri1.vertexB.Y + tri1.vertexC.Y) / 3}
                Dim dA As Double = Math.Sqrt((tri1.vertexA.X - center(0)) ^ 2 + (tri1.vertexA.Y - center(1)) ^ 2)
                If dA = 0 Then Return False
                Dim beamorig1() As Decimal = {vert1.X, vert1.Y, 0}
                Dim vert00() As Decimal = {tri(0, 0), tri(0, 1), 0}
                Dim vert01() As Decimal = {tri(1, 0), tri(1, 1), 0}
                Dim vert02() As Decimal = {tri(2, 0), tri(2, 1), 0}
                Return PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02)
            End If
        End If
        Return False
    End Function

    Private Shared Function isTriangleInTriangle(ByVal tri1 As Triangle, ByVal tri2 As Triangle) As Boolean
        Dim tri2c(,) As Double = {{tri2.vertexA.X, tri2.vertexA.Y}, {tri2.vertexB.X, tri2.vertexB.Y}, {tri2.vertexC.X, tri2.vertexC.Y}}
        Dim beamdir() As Decimal = {0, 0, -1}
        Dim center() As Double = {(tri1.vertexA.X + tri1.vertexB.X + tri1.vertexC.X) / 3, (tri1.vertexA.Y + tri1.vertexB.Y + tri1.vertexC.Y) / 3}
        Dim dA As Double = Math.Sqrt((tri1.vertexA.X - center(0)) ^ 2 + (tri1.vertexA.Y - center(1)) ^ 2)
        Dim dB As Double = Math.Sqrt((tri1.vertexB.X - center(0)) ^ 2 + (tri1.vertexB.Y - center(1)) ^ 2)
        Dim dC As Double = Math.Sqrt((tri1.vertexC.X - center(0)) ^ 2 + (tri1.vertexC.Y - center(1)) ^ 2)
        If dA = 0 OrElse dB = 0 OrElse dC = 0 Then Return False
        Dim beamorig1() As Decimal = {tri1.vertexA.X + 0.5 * (center(0) - tri1.vertexA.X) / dA, tri1.vertexA.Y + 0.5 * (center(1) - tri1.vertexA.Y) / dA, 0}
        Dim beamorig2() As Decimal = {tri1.vertexB.X + 0.5 * (center(0) - tri1.vertexB.X) / dB, tri1.vertexB.Y + 0.5 * (center(1) - tri1.vertexB.Y) / dB, 0}
        Dim beamorig3() As Decimal = {tri1.vertexC.X + 0.5 * (center(0) - tri1.vertexC.X) / dC, tri1.vertexC.Y + 0.5 * (center(1) - tri1.vertexC.Y) / dC, 0}
        Dim vert00() As Decimal = {tri2c(0, 0), tri2c(0, 1), 0}
        Dim vert01() As Decimal = {tri2c(1, 0), tri2c(1, 1), 0}
        Dim vert02() As Decimal = {tri2c(2, 0), tri2c(2, 1), 0}
        Return PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02) AndAlso _
               PowerRay.SCHNITTPKT_DREIECK(beamorig2, beamdir, vert00, vert01, vert02) AndAlso _
               PowerRay.SCHNITTPKT_DREIECK(beamorig3, beamdir, vert00, vert01, vert02)
    End Function

    Private Shared Function isPartlyTriangleInTriangle(ByVal tri1 As Triangle, ByVal tri2 As Triangle)
        Dim tri2c(,) As Double = {{tri2.vertexA.X, tri2.vertexA.Y}, {tri2.vertexB.X, tri2.vertexB.Y}, {tri2.vertexC.X, tri2.vertexC.Y}}
        Dim beamdir() As Decimal = {0, 0, -1}
        Dim center() As Double = {(tri1.vertexA.X + tri1.vertexB.X + tri1.vertexC.X) / 3, (tri1.vertexA.Y + tri1.vertexB.Y + tri1.vertexC.Y) / 3}
        Dim dA As Double = Math.Sqrt((tri1.vertexA.X - center(0)) ^ 2 + (tri1.vertexA.Y - center(1)) ^ 2)
        Dim dB As Double = Math.Sqrt((tri1.vertexB.X - center(0)) ^ 2 + (tri1.vertexB.Y - center(1)) ^ 2)
        Dim dC As Double = Math.Sqrt((tri1.vertexC.X - center(0)) ^ 2 + (tri1.vertexC.Y - center(1)) ^ 2)
        If dA = 0 OrElse dB = 0 OrElse dC = 0 Then Return True
        Dim beamorig1() As Decimal = {tri1.vertexA.X + 0.5 * (center(0) - tri1.vertexA.X) / dA, tri1.vertexA.Y + 0.5 * (center(1) - tri1.vertexA.Y) / dA, 0}
        Dim beamorig2() As Decimal = {tri1.vertexB.X + 0.5 * (center(0) - tri1.vertexB.X) / dB, tri1.vertexB.Y + 0.5 * (center(1) - tri1.vertexB.Y) / dB, 0}
        Dim beamorig3() As Decimal = {tri1.vertexC.X + 0.5 * (center(0) - tri1.vertexC.X) / dC, tri1.vertexC.Y + 0.5 * (center(1) - tri1.vertexC.Y) / dC, 0}
        Dim vert00() As Decimal = {tri2c(0, 0), tri2c(0, 1), 0}
        Dim vert01() As Decimal = {tri2c(1, 0), tri2c(1, 1), 0}
        Dim vert02() As Decimal = {tri2c(2, 0), tri2c(2, 1), 0}
        Return PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02) OrElse _
               PowerRay.SCHNITTPKT_DREIECK(beamorig2, beamdir, vert00, vert01, vert02) OrElse _
               PowerRay.SCHNITTPKT_DREIECK(beamorig3, beamdir, vert00, vert01, vert02)
    End Function

End Class
