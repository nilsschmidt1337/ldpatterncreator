Imports Accord.Imaging

Module ArtificialTriangulator

    Private temp As Integer = 0
    Private threshold As Single
    Private oldSize As Integer = -1

    Dim isolatedVerts As New List(Of Vertex)

    Public Sub iterate()

        If temp < 10 Then
            temp += 1
            Exit Sub
        End If
        temp = 0

        Dim backupTriangles As New List(Of Triangle)
        Dim selectedTriangles As New List(Of Triangle)


        For i As Integer = 0 To LPCFile.Vertices.Count - 1
            If LPCFile.Vertices(i).groupindex = Primitive.NO_INDEX AndAlso LPCFile.Vertices(i).linkedTriangles.Count = 0 Then
                isolatedVerts.Add(LPCFile.Vertices(i))
            End If
        Next

        For Each tri As Triangle In LPCFile.Triangles
            If tri.myColourNumber <> 16 Then
                backupTriangles.Add(tri)
            End If
        Next
        selectedTriangles.AddRange(View.SelectedTriangles)
        resetPattern()

        Dim bw As Integer = View.backgroundPicture.Width
        Dim bh As Integer = View.backgroundPicture.Height
        Dim ox = bw / 2 * View.imgScale - View.imgOffsetX
        Dim oy = bh / 2 * View.imgScale - View.imgOffsetY

        Dim sigma As Double = 1.2
        Dim k As Single = 0.04

        If selectedTriangles.Count = oldSize Then
            threshold -= 1000
            threshold = Math.Max(5000, threshold)
        Else
            threshold += 5000
        End If
        oldSize = selectedTriangles.Count

        ' Create a New Harris Corners Detector using the given parameters
        Dim harris As New HarrisCornersDetector(k) With {
                        .Measure = HarrisCornerMeasure.Harris,
                        .Threshold = threshold,
                        .Sigma = sigma
                    }

        Delaunay.EmptyVertexList()

        ' Corner vertices
        Delaunay.AddVertexOutsideTriangles(Math.Round(ox), Math.Round(-oy))
        Delaunay.AddVertexOutsideTriangles(Math.Round(ox), Math.Round(View.imgScale * bh - oy))
        Delaunay.AddVertexOutsideTriangles(Math.Round(ox - View.imgScale * bw), Math.Round(-oy))
        Delaunay.AddVertexOutsideTriangles(Math.Round(ox - View.imgScale * bw), Math.Round(View.imgScale * bh - oy))

        For Each tri As Triangle In selectedTriangles
            If tri.myColourNumber <> 16 Then Continue For
            Dim a As New Vertex(tri.vertexA.X, tri.vertexA.Y, False)
            Dim b As New Vertex(tri.vertexB.X, tri.vertexB.Y, False)
            Dim c As New Vertex(tri.vertexC.X, tri.vertexC.Y, False)
            Dim newTri As New Triangle(a, b, c)
            Delaunay.AddVertex(a.X, a.Y)
            Delaunay.AddVertex(b.X, b.Y)
            Delaunay.AddVertex(c.X, c.Y)
            newTri.myColour = tri.myColour
            newTri.myColourNumber = tri.myColourNumber
            View.SelectedTriangles.Add(newTri)
            LPCFile.Triangles.Add(newTri)
        Next

        For Each tri As Triangle In backupTriangles
            Dim a As New Vertex(tri.vertexA.X, tri.vertexA.Y, False)
            Dim b As New Vertex(tri.vertexB.X, tri.vertexB.Y, False)
            Dim c As New Vertex(tri.vertexC.X, tri.vertexC.Y, False)
            Dim newTri As New Triangle(a, b, c)
            Delaunay.AddVertex(a.X, a.Y)
            Delaunay.AddVertex(b.X, b.Y)
            Delaunay.AddVertex(c.X, c.Y)
            newTri.myColour = tri.myColour
            newTri.myColourNumber = tri.myColourNumber
            LPCFile.Triangles.Add(newTri)
        Next

        For Each vert As Vertex In isolatedVerts
            Dim v As New Vertex(vert.X, vert.Y, False)
            Delaunay.AddVertexOutsideTriangles(vert.X, vert.Y)
        Next

        Dim cornerPs As List(Of Accord.IntPoint) = Nothing
        Try
            cornerPs = harris.ProcessImage(View.backgroundPicture)
            For Each p In cornerPs
                Delaunay.AddVertexOutsideTriangles(Math.Round(ox - View.imgScale * p.X), Math.Round(View.imgScale * p.Y - oy))
            Next
        Catch ex As Exception
        End Try

        If Not cornerPs Is Nothing AndAlso cornerPs.Count > 10 Then
            Delaunay.CalculateTriangles()

            For Each t As Delaunay.dTriangle In Delaunay.Triangle
                If Not Delaunay.isVisible(t) Then Continue For
                Dim a As Delaunay.dVertex = Delaunay.Vertex(t.vv0)
                Dim b As Delaunay.dVertex = Delaunay.Vertex(t.vv1)
                Dim c As Delaunay.dVertex = Delaunay.Vertex(t.vv2)
                If a.x = 0 AndAlso a.y = 0 Then Continue For
                If b.x = 0 AndAlso b.y = 0 Then Continue For
                If c.x = 0 AndAlso c.y = 0 Then Continue For
                LPCFile.Triangles.Add(New Triangle(New Vertex(a.x, a.y, False), New Vertex(b.x, b.y, False), New Vertex(c.x, c.y, False)))
            Next

        End If

    End Sub

    Private Sub resetPattern()
        ' View.SelectedVertices.Clear()
        View.SelectedTriangles.Clear()
        LPCFile.Vertices.Clear()
        LPCFile.Triangles.Clear()
        LPCFile.PrimitivesHMap.Clear()
        LPCFile.Primitives.Clear()
        LPCFile.PrimitivesMetadataHMap.Clear()
        LPCFile.templateShape.Clear()
        LPCFile.templateProjectionQuads.Clear()
        LPCFile.templateTexts.Clear()
        GlobalIdSet.vertexIDglobal = 0
        GlobalIdSet.triangleIDglobal = 0
        GlobalIdSet.primitiveIDglobal = 0
        UndoRedoHelper.clearHistory()
    End Sub

End Module
