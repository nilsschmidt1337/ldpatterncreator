
Module Delaunay

    Public Structure dVertex 'Points (Vertices)
        Dim x As Double
        Dim y As Double
    End Structure

    Public Structure dTriangle 'Created Triangles, vv# are the vertex pointers
        Dim vv0 As Integer
        Dim vv1 As Integer
        Dim vv2 As Integer
    End Structure

    Private verts As New Dictionary(Of dVertex, Boolean)

    Public Const MaxVertices As Integer = 10000 'Set these as applicable
    Public Const MaxTriangles As Integer = MaxVertices * 3

    Public Vertex(MaxVertices + 2) As dVertex 'Our points, the extra 2 are for the 3 points of the supertriangle
    Public VertexCount As Integer = -1
    Public Triangle(MaxTriangles - 1) As dTriangle 'Our Created Triangles, -1 because it is zero based

    Private xmin As Double = Double.MaxValue
    Private xmax As Double = Double.MinValue
    Private ymin As Double = Double.MaxValue
    Private ymax As Double = Double.MinValue

    Public Function isVisible(ByRef tri As dTriangle) As Boolean
        Return tri.vv0 < VertexCount AndAlso tri.vv1 < VertexCount AndAlso tri.vv2 < VertexCount
    End Function


    Public Sub AddVertexOutsideTriangles(ByVal x As Double, ByVal y As Double)
        Dim vert As dVertex
        vert.x = x
        vert.y = y
        If verts.ContainsKey(vert) OrElse (x = 0 AndAlso y = 0) Then
            Exit Sub
        End If

        CSG.beamorig(0) = x
        CSG.beamorig(1) = y
        CSG.beamorig(2) = 0
        Dim vert0(2) As Decimal
        Dim vert1(2) As Decimal
        Dim vert2(2) As Decimal
        vert0(2) = 0 : vert1(2) = 0 : vert2(2) = 0
        For Each tri As Triangle In LPCFile.Triangles
            vert0(0) = tri.vertexA.X
            vert0(1) = tri.vertexA.Y
            vert1(0) = tri.vertexB.X
            vert1(1) = tri.vertexB.Y
            vert2(0) = tri.vertexC.X
            vert2(1) = tri.vertexC.Y
            If PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, CSG.beamdir, vert0, vert1, vert2) Then
                Exit Sub
            End If
        Next

        verts.Add(vert, True)
        If VertexCount + 1 < MaxVertices Then
            VertexCount += 1
            Vertex(VertexCount) = vert
            If x < xmin Then xmin = x
            If x > xmax Then xmax = x
            If y < ymin Then ymin = y
            If y > ymax Then ymax = y
        End If
    End Sub

    Public Sub AddVertex(ByVal x As Double, ByVal y As Double)
        Dim vert As dVertex
        vert.x = x
        vert.y = y
        If verts.ContainsKey(vert) OrElse (x = 0 AndAlso y = 0) Then
            Exit Sub
        End If
        verts.Add(vert, True)
        If VertexCount + 1 < MaxVertices Then
            VertexCount += 1
            Vertex(VertexCount) = vert
            If x < xmin Then xmin = x
            If x > xmax Then xmax = x
            If y < ymin Then ymin = y
            If y > ymax Then ymax = y
        End If
    End Sub

    Public Sub EmptyVertexList()
        verts.Clear()
        VertexCount = -1
        ReDim Vertex(MaxVertices + 2) 'clears all the data out, although really not necessary
        xmin = Double.MaxValue
        xmax = Double.MinValue
        ymin = Double.MaxValue
        ymax = Double.MinValue
    End Sub


    Public Sub CalculateTriangles() 'These triangles are arranged in clockwise order.
        Dim Edges(1, MaxTriangles * 3)
        Dim Nedge As Integer
        Dim j As Integer
        Dim k As Integer
        Dim inc As Boolean

        Dim dx As Double = xmax - xmin
        Dim dy As Double = ymax - ymin
        Dim dmax As Double = dy
        If dx > dy Then dmax = dx
        Dim xmid As Double = (xmax + xmin) / 2
        Dim ymid As Double = (ymax + ymin) / 2

        'Set up the supertriangle. This is a triangle which encompasses all the sample points.
        'The supertriangle coordinates are added to the end of the vertex list. The supertriangle is the first triangle in the triangle list.
        Vertex(VertexCount + 1).x = xmid - 2 * dmax
        Vertex(VertexCount + 1).y = ymid - dmax
        Vertex(VertexCount + 2).x = xmid
        Vertex(VertexCount + 2).y = ymid + 2 * dmax
        Vertex(VertexCount + 3).x = xmid + 2 * dmax
        Vertex(VertexCount + 3).y = ymid - dmax
        Triangle(0).vv0 = VertexCount + 1
        Triangle(0).vv1 = VertexCount + 2
        Triangle(0).vv2 = VertexCount + 3
        Dim ntri As Integer = 0

        'Include each point one at a time into the existing mesh
        For i = 0 To VertexCount
            Nedge = 0
            'Set up the edge buffer.
            'If the point (Vertex(i).x,Vertex(i).y) lies inside the circumcircle then the three edges of that triangle are added to the edge buffer.
            j = -1
            Do
                j += 1
                inc = InCircle(Vertex(i).x, Vertex(i).y, Vertex(Triangle(j).vv0).x, Vertex(Triangle(j).vv0).y, Vertex(Triangle(j).vv1).x, Vertex(Triangle(j).vv1).y, Vertex(Triangle(j).vv2).x, Vertex(Triangle(j).vv2).y)
                If inc Then
                    Edges(0, Nedge) = Triangle(j).vv0
                    Edges(1, Nedge) = Triangle(j).vv1
                    Edges(0, Nedge + 1) = Triangle(j).vv1
                    Edges(1, Nedge + 1) = Triangle(j).vv2
                    Edges(0, Nedge + 2) = Triangle(j).vv2
                    Edges(1, Nedge + 2) = Triangle(j).vv0
                    Nedge += 3
                    Triangle(j).vv0 = Triangle(ntri).vv0
                    Triangle(j).vv1 = Triangle(ntri).vv1
                    Triangle(j).vv2 = Triangle(ntri).vv2
                    j -= 1
                    ntri -= 1
                End If
            Loop While j < ntri

            'Tag multiple edges
            'Note: if all triangles are specified anticlockwise then all interior edges are opposite pointing in direction.            
            For j = 0 To Nedge - 2
                If Not Edges(0, j) = -1 And Not Edges(1, j) = -1 Then
                    For k = j + 1 To Nedge - 1
                        If Not Edges(0, k) = -1 And Not Edges(1, k) = -1 Then
                            If Edges(0, j) = Edges(1, k) Then
                                If Edges(1, j) = Edges(0, k) Then
                                    Edges(0, j) = -1
                                    Edges(1, j) = -1
                                    Edges(0, k) = -1
                                    Edges(1, k) = -1
                                End If
                            End If
                        End If
                    Next k
                End If
            Next j

            'Form new triangles for the current point, Skipping over any tagged edges.
            'All edges are arranged in clockwise order.
            For j = 0 To Nedge - 1
                If Not Edges(0, j) = -1 And Not Edges(1, j) = -1 Then
                    ntri += 1
                    Triangle(ntri).vv0 = Edges(0, j)
                    Triangle(ntri).vv1 = Edges(1, j)
                    Triangle(ntri).vv2 = i
                End If
            Next j
        Next
    End Sub
    Private Function InCircle(ByRef xp As Integer, ByRef yp As Integer, ByRef x1 As Integer, ByRef y1 As Integer, ByRef x2 As Integer, ByRef y2 As Integer, ByRef x3 As Integer, ByRef y3 As Integer) As Boolean
        'Return TRUE if the point (xp,yp) lies inside the circumcircle
        'made up by points (x1,y1) (x2,y2) (x3,y3)
        'The circumcircle centre is returned in (xc,yc) and the radius r
        'NOTE: A point on the edge is inside the circumcircle

        Dim m1 As Double
        Dim m2 As Double
        Dim mx1 As Double
        Dim mx2 As Double
        Dim my1 As Double
        Dim my2 As Double
        Dim dx As Double
        Dim dy As Double
        Dim rsqr As Double
        Dim drsqr As Double
        Dim xc As Double
        Dim yc As Double

        If y1 = y2 And y2 = y3 Then
            Return False
        ElseIf y2 = y1 Then
            m2 = -(x3 - x2) / (y3 - y2)
            mx2 = (x2 + x3) / 2
            my2 = (y2 + y3) / 2
            xc = (x2 + x1) / 2
            yc = m2 * (xc - mx2) + my2
        ElseIf y3 = y2 Then
            m1 = -(x2 - x1) / (y2 - y1)
            mx1 = (x1 + x2) / 2
            my1 = (y1 + y2) / 2
            xc = (x3 + x2) / 2
            yc = m1 * (xc - mx1) + my1
        Else
            m1 = -(x2 - x1) / (y2 - y1)
            m2 = -(x3 - x2) / (y3 - y2)
            mx1 = (x1 + x2) / 2
            mx2 = (x2 + x3) / 2
            my1 = (y1 + y2) / 2
            my2 = (y2 + y3) / 2
            xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2)
            yc = m1 * (xc - mx1) + my1
        End If

        dx = x2 - xc
        dy = y2 - yc
        rsqr = dx * dx + dy * dy
        dx = xp - xc
        dy = yp - yc
        drsqr = dx * dx + dy * dy

        Return drsqr <= rsqr
    End Function

    Private Function WhichSide(ByRef xp As Integer, ByRef yp As Integer, ByRef x1 As Integer, ByRef y1 As Integer, ByRef x2 As Integer, ByRef y2 As Integer) As Short
        'Determines which side of a line the point (xp,yp) lies.
        'The line goes from (x1,y1) to (x2,y2)
        'Returns -1 for a point to the left
        '         0 for a point on the line
        '        +1 for a point to the right

        Dim equation As Double = ((yp - y1) * (x2 - x1)) - ((y2 - y1) * (xp - x1))

        If equation > 0 Then
            WhichSide = -1
        ElseIf equation = 0 Then
            WhichSide = 0
        Else
            WhichSide = 1
        End If

    End Function

    Private Function InTriangle(ByVal x As Integer, ByVal y As Integer, ByVal v0 As Integer, ByVal v1 As Integer, ByVal v2 As Integer) As Boolean
        InTriangle = False
        Dim v0x As Single = Vertex(v0).x
        Dim v0y As Single = Vertex(v0).y
        Dim v1x As Single = Vertex(v1).x
        Dim v1y As Single = Vertex(v1).y
        Dim v2x As Single = Vertex(v2).x
        Dim v2y As Single = Vertex(v2).y
        Dim side1 As Single = WhichSide(x, y, v0x, v0y, v1x, v1y)
        Dim side2 As Single = WhichSide(x, y, v1x, v1y, v2x, v2y)
        Dim side3 As Single = WhichSide(x, y, v2x, v2y, v0x, v0y)

        If (side1 = 0) And (side2 = 0) Then InTriangle = True
        If (side1 = 0) And (side3 = 0) Then InTriangle = True
        If (side2 = 0) And (side3 = 0) Then InTriangle = True
        If (side1 = 0) And (side2 = side3) Then InTriangle = True
        If (side2 = 0) And (side1 = side3) Then InTriangle = True
        If (side3 = 0) And (side1 = side2) Then InTriangle = True
        If (side1 = side2) And (side2 = side3) Then InTriangle = True
    End Function
End Module
