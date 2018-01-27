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
Imports System.Text

Module AdjustHelper
    Public Sub adjustAndSaveTriangles(ByVal projectMode As Byte, ByRef DateiOut As StreamWriter)
        Dim v(2, 2) As Decimal
        Dim matrix(3, 3) As Decimal
        Dim matrix2(3, 3) As Decimal
        Dim matrix3(3, 3) As Decimal

        For r As Integer = 0 To 3
            For c As Integer = 0 To 3
                matrix2(r, c) = LPCFile.myMetadata.matrix(r, c)
            Next
        Next

        Select Case projectMode
            Case 0
                Dim cmatrix(,) As Decimal = {{0D, 0D, 1D, 0D},
                                             {0D, 1D, 0D, 0D},
                                             {1D, 0D, 0D, 0D},
                                             {0D, 0D, 0D, 1D}}
                matrix = cmatrix.Clone
            Case 1
                Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                             {0D, 0D, 1D, 0D},
                                             {0D, -1D, 0D, 0D},
                                             {0D, 0D, 0D, 1D}}
                matrix = cmatrix.Clone
            Case 2
                Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                             {0D, 1D, 0D, 0D},
                                             {0D, 0D, -1D, 0D},
                                             {0D, 0D, 0D, 1D}}
                matrix = cmatrix.Clone
            Case 3
                Dim cmatrix(,) As Decimal = {{0D, 0D, -1D, 0D},
                                             {0D, 1D, 0D, 0D},
                                             {-1D, 0D, 0D, 0D},
                                             {0D, 0D, 0D, 1D}}
                matrix = cmatrix.Clone
            Case 4
                Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                             {0D, 0D, -1D, 0D},
                                             {0D, 1D, 0D, 0D},
                                             {0D, 0D, 0D, 1D}}
                matrix = cmatrix.Clone
            Case 5
                Dim cmatrix(,) As Decimal = {{1D, 0D, 0D, 0D},
                                             {0D, 1D, 0D, 0D},
                                             {0D, 0D, -1D, 0D},
                                             {0D, 0D, 0D, 1D}}
                matrix = cmatrix.Clone
        End Select

        matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
        matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
        matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
        matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

        matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
        matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
        matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
        matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

        matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
        matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
        matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
        matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

        matrix3(3, 0) = 0D
        matrix3(3, 1) = 0D
        matrix3(3, 2) = 0D
        matrix3(3, 3) = 1D

        matrix = matrix3.Clone

        Dim pointMap As New Dictionary(Of String, Decimal())
        Dim lineMap As New Dictionary(Of String, Decimal())
        Dim lineList As New List(Of Vertex3D2())

        For Each tri As Triangle In LPCFile.Triangles
            tri.normalize()
            tri.checkWinding()
        Next

        ' Unificator
        Const DELTA As Decimal = 0.01D
        Dim pqc As Integer = LPCFile.templateProjectionQuads.Count - 1
        If LPCFile.unifyLPC Then
            ' Add line and condline coordinates from additional data too!
            Dim additionalLines As String() = LPCFile.myMetadata.additionalData.Split("<br>")
            For Each line As String In additionalLines
                If line Is Nothing OrElse line = "" Then Continue For
                line = Replace(line, "br>", "").Trim()
                If line <> Nothing AndAlso line <> "" Then
                    Dim oldlenght As Integer
                    Do
                        oldlenght = line.Length
                        line = Replace(line, "  ", " ")
                    Loop Until oldlenght = line.Length
                    Dim t(2) As Decimal
                    Dim words() As String = line.Split(CType(" ", Char))
                    If words(0).Equals("2") OrElse words(0).Equals("5") Then
                        ' Save all lines for later unification
                        Dim vlist(1) As Vertex3D2
                        Try
                            vlist(0) = New Vertex3D2(CType(Replace(words(2), ".", MathHelper.comma), Decimal), CType(Replace(words(3), ".", MathHelper.comma), Decimal), CType(Replace(words(4), ".", MathHelper.comma), Decimal))
                            vlist(1) = New Vertex3D2(CType(Replace(words(5), ".", MathHelper.comma), Decimal), CType(Replace(words(6), ".", MathHelper.comma), Decimal), CType(Replace(words(7), ".", MathHelper.comma), Decimal))
                        Catch ex As Exception
                            Continue For
                        End Try
                        lineList.Add(vlist)
                        For i As Integer = 0 To 3 Step 3
                            t(0) = CType(Replace(words(2 + i), ".", MathHelper.comma), Decimal)
                            t(1) = CType(Replace(words(3 + i), ".", MathHelper.comma), Decimal)
                            t(2) = CType(Replace(words(4 + i), ".", MathHelper.comma), Decimal)
                            Dim dx As Decimal = -DELTA
                            While dx <= DELTA
                                Dim dy As Decimal = -DELTA
                                While dy <= DELTA
                                    Dim dz As Decimal = -DELTA
                                    While dz <= DELTA
                                        Dim sb As New StringBuilder
                                        sb.Append(Math.Round(t(0) + dx, 2))
                                        sb.Append(";")
                                        sb.Append(Math.Round(t(1) + dy, 2))
                                        sb.Append(";")
                                        sb.Append(Math.Round(t(2) + dz, 2))
                                        Dim key As String = sb.ToString
                                        If Not lineMap.ContainsKey(key) Then
                                            lineMap.Add(key, t.Clone)
                                        End If
                                        dz = dz + DELTA
                                    End While
                                    dy = dy + DELTA
                                End While
                                dx = dx + DELTA
                            End While
                        Next i
                    Else
                        Continue For
                    End If
                End If
            Next

            For Each proQ As ProjectionQuad In LPCFile.templateProjectionQuads
                For i As Integer = 0 To 3
                    Dim t(2) As Decimal
                    t(0) = proQ.outCoords(i, 0)
                    t(1) = proQ.outCoords(i, 1)
                    t(2) = proQ.outCoords(i, 2)
                    Dim dx As Decimal = -DELTA
                    While dx <= DELTA
                        Dim dy As Decimal = -DELTA
                        While dy <= DELTA
                            Dim dz As Decimal = -DELTA
                            While dz <= DELTA
                                Dim sb As New StringBuilder
                                sb.Append(Math.Round(t(0) + dx, 1))
                                sb.Append(";")
                                sb.Append(Math.Round(t(1) + dy, 1))
                                sb.Append(";")
                                sb.Append(Math.Round(t(2) + dz, 1))
                                Dim key As String = sb.ToString
                                If Not pointMap.ContainsKey(key) Then
                                    pointMap.Add(key, t.Clone)
                                End If
                                dz = dz + DELTA
                            End While
                            dy = dy + DELTA
                        End While
                        dx = dx + DELTA
                    End While
                Next i
            Next
            For Each tri As Triangle In LPCFile.Triangles

                v(0, 0) = tri.vertexA.X
                v(0, 1) = tri.vertexA.Y
                v(0, 2) = 0D
                v(1, 0) = tri.vertexB.X
                v(1, 1) = tri.vertexB.Y
                v(1, 2) = 0D
                v(2, 0) = tri.vertexC.X
                v(2, 1) = tri.vertexC.Y
                v(2, 2) = 0D

                Dim no_match As Boolean = True
                Dim targetQuad As ProjectionQuad = Nothing

                If pqc > -1 AndAlso LPCFile.project Then
                    For j As Integer = 0 To pqc
                        If LPCFile.templateProjectionQuads(j).isInQuad((v(0, 0) + v(1, 0) + v(2, 0)) / 3D, (v(0, 1) + v(1, 1) + v(2, 1)) / 3D) Then
                            no_match = False
                            targetQuad = LPCFile.templateProjectionQuads(j)
                            For row As Integer = 0 To 3
                                For col As Integer = 0 To 3
                                    matrix(row, col) = targetQuad.matrix(row, col)
                                Next
                            Next
                            Exit For
                        End If
                    Next
                End If

                For i As Integer = 0 To 2
                    If pqc > -1 AndAlso LPCFile.project Then
                        If no_match Then
                            ' "Merge" this vertex to the nearest ProjectionQuad line
                            ' and use the matrix of this projection quad (this is "very" complex, but there is no elegant method!!)
                            Dim minDist As Double = Double.MaxValue
                            Dim dist As Double
                            Dim currentVertex As New Vertex(v(i, 0), v(i, 1), False, False)
                            Dim distVertex As Vertex
                            Dim finalVertex As Vertex = Nothing
                            targetQuad = Nothing
                            ' Detect if it is nearer to a line
                            ' Line
                            For Each quad As ProjectionQuad In LPCFile.templateProjectionQuads
                                Dim targetVertex1 As Vertex
                                Dim targetVertex2 As Vertex
                                If Not quad.isTriangle Then
                                    targetVertex1 = New Vertex(quad.inCoords(0, 0), quad.inCoords(0, 1), False, False)
                                    targetVertex2 = New Vertex(quad.inCoords(1, 0), quad.inCoords(1, 1), False, False)
                                    distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                    dist = currentVertex.dist(distVertex)
                                    If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad
                                End If

                                targetVertex1 = New Vertex(quad.inCoords(1, 0), quad.inCoords(1, 1), False, False)
                                targetVertex2 = New Vertex(quad.inCoords(2, 0), quad.inCoords(2, 1), False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                dist = currentVertex.dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad

                                targetVertex1 = New Vertex(quad.inCoords(2, 0), quad.inCoords(2, 1), False, False)
                                targetVertex2 = New Vertex(quad.inCoords(3, 0), quad.inCoords(3, 1), False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                dist = currentVertex.dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad

                                targetVertex1 = New Vertex(quad.inCoords(3, 0), quad.inCoords(3, 1), False, False)
                                If Not quad.isTriangle Then
                                    targetVertex2 = New Vertex(quad.inCoords(0, 0), quad.inCoords(0, 1), False, False)
                                Else
                                    targetVertex2 = New Vertex(quad.inCoords(1, 0), quad.inCoords(1, 1), False, False)
                                End If
                                distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                dist = currentVertex.dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad
                            Next
                            If Not targetQuad Is Nothing Then
                                For row As Integer = 0 To 3
                                    For col As Integer = 0 To 3
                                        matrix(row, col) = targetQuad.matrix(row, col)
                                    Next
                                Next
                            End If
                        End If
                    End If
                    Dim t(2) As Decimal
                    If no_match Then
                        t(0) = Math.Round(matrix(0, 0) * v(i, 0) + matrix(0, 1) * v(i, 1) + matrix(0, 2) * v(i, 2) + matrix(0, 3), 5)
                        t(1) = Math.Round(matrix(1, 0) * v(i, 0) + matrix(1, 1) * v(i, 1) + matrix(1, 2) * v(i, 2) + matrix(1, 3), 5)
                        t(2) = Math.Round(matrix(2, 0) * v(i, 0) + matrix(2, 1) * v(i, 1) + matrix(2, 2) * v(i, 2) + matrix(2, 3), 5)
                    Else
                        t = targetQuad.uvProjection(v(i, 0), v(i, 1))
                    End If
                    v(i, 0) = t(0)
                    v(i, 1) = t(1)
                    v(i, 2) = t(2)
                    Dim dx As Decimal = -DELTA
                    While dx <= DELTA
                        Dim dy As Decimal = -DELTA
                        While dy <= DELTA
                            Dim dz As Decimal = -DELTA
                            While dz <= DELTA
                                Dim sb As New StringBuilder
                                sb.Append(Math.Round(v(i, 0) + dx, 2))
                                sb.Append(";")
                                sb.Append(Math.Round(v(i, 1) + dy, 2))
                                sb.Append(";")
                                sb.Append(Math.Round(v(i, 2) + dz, 2))
                                Dim key As String = sb.ToString
                                If Not pointMap.ContainsKey(key) Then
                                    pointMap.Add(key, t.Clone)
                                End If
                                dz = dz + DELTA
                            End While
                            dy = dy + DELTA
                        End While
                        dx = dx + DELTA
                    End While
                Next
            Next
        End If

        For Each tri As Triangle In LPCFile.Triangles

            If tri.groupindex = -1 Then

                v(0, 0) = tri.vertexA.X
                v(0, 1) = tri.vertexA.Y
                v(0, 2) = 0D
                v(1, 0) = tri.vertexB.X
                v(1, 1) = tri.vertexB.Y
                v(1, 2) = 0D
                v(2, 0) = tri.vertexC.X
                v(2, 1) = tri.vertexC.Y
                v(2, 2) = 0D

                Dim no_match As Boolean = True
                Dim targetQuad As ProjectionQuad = Nothing

                If pqc > -1 AndAlso LPCFile.project Then
                    For j As Integer = 0 To pqc
                        If LPCFile.templateProjectionQuads(j).isInQuad((v(0, 0) + v(1, 0) + v(2, 0)) / 3D, (v(0, 1) + v(1, 1) + v(2, 1)) / 3D) Then
                            no_match = False
                            targetQuad = LPCFile.templateProjectionQuads(j)
                            For row As Integer = 0 To 3
                                For col As Integer = 0 To 3
                                    matrix(row, col) = targetQuad.matrix(row, col)
                                Next
                            Next
                            Exit For
                        End If
                    Next
                End If

                For i As Integer = 0 To 2
                    If pqc > -1 AndAlso LPCFile.project Then
                        If no_match Then
                            ' "Merge" this vertex to the nearest ProjectionQuad line
                            ' and use the matrix of this projection quad (this is "very" complex, but there is no elegant method!!)
                            Dim minDist As Double = Double.MaxValue
                            Dim dist As Double
                            Dim currentVertex As New Vertex(v(i, 0), v(i, 1), False, False)
                            Dim distVertex As Vertex
                            Dim finalVertex As Vertex = Nothing
                            targetQuad = Nothing
                            ' Detect if it is nearer to a line
                            ' Line
                            For Each quad As ProjectionQuad In LPCFile.templateProjectionQuads
                                Dim targetVertex1 As Vertex
                                Dim targetVertex2 As Vertex
                                If Not quad.isTriangle Then
                                    targetVertex1 = New Vertex(quad.inCoords(0, 0), quad.inCoords(0, 1), False, False)
                                    targetVertex2 = New Vertex(quad.inCoords(1, 0), quad.inCoords(1, 1), False, False)
                                    distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                    dist = currentVertex.dist(distVertex)
                                    If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad
                                End If

                                targetVertex1 = New Vertex(quad.inCoords(1, 0), quad.inCoords(1, 1), False, False)
                                targetVertex2 = New Vertex(quad.inCoords(2, 0), quad.inCoords(2, 1), False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                dist = currentVertex.dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad

                                targetVertex1 = New Vertex(quad.inCoords(2, 0), quad.inCoords(2, 1), False, False)
                                targetVertex2 = New Vertex(quad.inCoords(3, 0), quad.inCoords(3, 1), False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                dist = currentVertex.dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad

                                targetVertex1 = New Vertex(quad.inCoords(3, 0), quad.inCoords(3, 1), False, False)
                                If Not quad.isTriangle Then
                                    targetVertex2 = New Vertex(quad.inCoords(0, 0), quad.inCoords(0, 1), False, False)
                                Else
                                    targetVertex2 = New Vertex(quad.inCoords(1, 0), quad.inCoords(1, 1), False, False)
                                End If
                                distVertex = CSG.distanceVectorFromVertexToLine(currentVertex, targetVertex1, targetVertex2)
                                dist = currentVertex.dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : targetQuad = quad
                            Next
                            If Not targetQuad Is Nothing Then
                                For row As Integer = 0 To 3
                                    For col As Integer = 0 To 3
                                        matrix(row, col) = targetQuad.matrix(row, col)
                                    Next
                                Next
                            End If
                        End If
                    End If
                    Dim t(2) As Decimal
                    If no_match Then
                        t(0) = Math.Round(matrix(0, 0) * v(i, 0) + matrix(0, 1) * v(i, 1) + matrix(0, 2) * v(i, 2) + matrix(0, 3), 5)
                        t(1) = Math.Round(matrix(1, 0) * v(i, 0) + matrix(1, 1) * v(i, 1) + matrix(1, 2) * v(i, 2) + matrix(1, 3), 5)
                        t(2) = Math.Round(matrix(2, 0) * v(i, 0) + matrix(2, 1) * v(i, 1) + matrix(2, 2) * v(i, 2) + matrix(2, 3), 5)
                    Else
                        t = targetQuad.uvProjection(v(i, 0), v(i, 1))
                    End If
                    v(i, 0) = t(0)
                    v(i, 1) = t(1)
                    v(i, 2) = t(2)
                    Dim dx As Decimal = -DELTA
                    While dx <= DELTA
                        Dim dy As Decimal = -DELTA
                        While dy <= DELTA
                            Dim dz As Decimal = -DELTA
                            While dz <= DELTA
                                Dim sb As New StringBuilder
                                sb.Append(Math.Round(v(i, 0) + dx, 1))
                                sb.Append(";")
                                sb.Append(Math.Round(v(i, 1) + dy, 1))
                                sb.Append(";")
                                sb.Append(Math.Round(v(i, 2) + dz, 1))
                                Dim key As String = sb.ToString
                                If pointMap.ContainsKey(key) Then
                                    Dim h() As Decimal = pointMap(key)
                                    v(i, 0) = h(0)
                                    v(i, 1) = h(1)
                                    v(i, 2) = h(2)
                                End If
                                Dim sb2 As New StringBuilder
                                sb2.Append(Math.Round(v(i, 0) + dx, 2))
                                sb2.Append(";")
                                sb2.Append(Math.Round(v(i, 1) + dy, 2))
                                sb2.Append(";")
                                sb2.Append(Math.Round(v(i, 2) + dz, 2))
                                Dim key2 As String = sb2.ToString
                                If pointMap.ContainsKey(key2) Then
                                    Dim h() As Decimal = pointMap(key2)
                                    v(i, 0) = h(0)
                                    v(i, 1) = h(1)
                                    v(i, 2) = h(2)
                                End If
                                If lineMap.ContainsKey(key2) Then
                                    Dim h() As Decimal = lineMap(key2)
                                    v(i, 0) = h(0)
                                    v(i, 1) = h(1)
                                    v(i, 2) = h(2)
                                End If
                                dz = dz + DELTA
                            End While
                            dy = dy + DELTA
                        End While
                        dx = dx + DELTA
                    End While

                    ' Unification to all lines
                    For Each line As Vertex3D2() In lineList
                        Dim vert As Vertex3D2 = New Vertex3D2(v(i, 0), v(i, 1), v(i, 2))
                        Dim nvert As Vertex3D2 = Vertex3D2.distanceVectorFromVertexToLine(vert, line(0), line(1))
                        Dim dist As Double = vert.dist(nvert)
                        If dist < 0.05 Then
                            v(i, 0) = nvert.X
                            v(i, 1) = nvert.Y
                            v(i, 2) = nvert.Z
                        End If
                    Next
                Next
                If Math.Sqrt((v(0, 0) - v(1, 0)) ^ 2 + (v(0, 1) - v(1, 1)) ^ 2 + (v(0, 2) - v(1, 2)) ^ 2) > 0.001 AndAlso
                   Math.Sqrt((v(2, 0) - v(1, 0)) ^ 2 + (v(2, 1) - v(1, 1)) ^ 2 + (v(2, 2) - v(1, 2)) ^ 2) > 0.001 AndAlso
                   Math.Sqrt((v(0, 0) - v(2, 0)) ^ 2 + (v(0, 1) - v(2, 1)) ^ 2 + (v(0, 2) - v(2, 2)) ^ 2) > 0.001 Then
                    DateiOut.WriteLine(Replace("3 " & tri.getColourString & " " & v(0, 0) & " " & v(0, 1) & " " & v(0, 2) & " " _
                                                       & v(1, 0) & " " & v(1, 1) & " " & v(1, 2) & " " _
                                                       & v(2, 0) & " " & v(2, 1) & " " & v(2, 2), MathHelper.comma, "."))
                    MainForm.performStep()
                End If
            End If
        Next
    End Sub

    Public Function adjustPrimitiveMatrix(ByVal pmatrix(,) As Double, ByVal projectMode As Byte, Optional ByVal group As Integer = -1) As Double(,)

        Dim matrix2(3, 3) As Decimal
        Dim matrix3(3, 3) As Decimal

        Dim matrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                   {0D, 0D, -1D, 0D},
                                   {0D, 1D, 0D, 0D},
                                   {0D, 0D, 0D, 1D}}

        For r As Integer = 0 To 3
            For c As Integer = 0 To 3
                matrix2(r, c) = pmatrix(r, c)
            Next
        Next

        matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
        matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
        matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
        matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

        matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
        matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
        matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
        matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

        matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
        matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
        matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
        matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

        matrix3(3, 0) = 0D
        matrix3(3, 1) = 0D
        matrix3(3, 2) = 0D
        matrix3(3, 3) = 1D

        matrix = matrix3.Clone

        If LPCFile.templateProjectionQuads.Count = 0 Or Not LPCFile.project Then

            Select Case projectMode
                Case 0 ' YZ mit -X (Right) [OK]
                    Dim cmatrix(,) As Decimal = {{0D, 0D, 1D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {1D, 0D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 1 ' XZ mit -Y (Top) [OK]
                    Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                                 {0D, 0D, 1D, 0D},
                                                 {0D, -1D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 2 ' XY mit -Z (Front) [OK] 
                    Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {0D, 0D, 1D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 3 ' YZ mit +X (Left) [OK]
                    Dim cmatrix(,) As Decimal = {{0D, 0D, -1D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {-1D, 0D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 4 ' XZ mit -Y (Bottom) [OK]
                    Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                                 {0D, 0D, -1D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 5 ' XY mit +Z (Back) [OK]
                    Dim cmatrix(,) As Decimal = {{1D, 0D, 0D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {0D, 0D, -1D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
            End Select

            matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
            matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
            matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
            matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

            matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
            matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
            matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
            matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

            matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
            matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
            matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
            matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

            matrix3(3, 0) = 0D
            matrix3(3, 1) = 0D
            matrix3(3, 2) = 0D
            matrix3(3, 3) = 1D
            matrix = matrix3.Clone
        End If

        If projectMode = LPCFile.myMetadata.recommendedMode Then
            Do
                If LPCFile.templateProjectionQuads.Count > 0 AndAlso LPCFile.project Then
                    selectGroupDirect(group)
                    For i = 0 To LPCFile.templateProjectionQuads.Count - 1
                        If LPCFile.templateProjectionQuads(i).isInQuad(View.SelectedTriangles) Then
                            For r As Integer = 0 To 3
                                For c As Integer = 0 To 3
                                    matrix2(r, c) = LPCFile.templateProjectionQuads(i).matrix(r, c)
                                Next
                            Next
                            matrix2(0, 3) = matrix2(0, 3) * 1000D
                            matrix2(1, 3) = matrix2(1, 3) * 1000D
                            matrix2(2, 3) = matrix2(2, 3) * 1000D
                            Exit Do
                        End If
                    Next
                End If
                For r As Integer = 0 To 3
                    For c As Integer = 0 To 3
                        matrix2(r, c) = LPCFile.myMetadata.matrix(r, c)
                    Next
                Next
                matrix2(0, 3) = matrix2(0, 3) * 1000D
                matrix2(1, 3) = matrix2(1, 3) * 1000D
                matrix2(2, 3) = matrix2(2, 3) * 1000D
                Exit Do
            Loop

            matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
            matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
            matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
            matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

            matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
            matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
            matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
            matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

            matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
            matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
            matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
            matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

            matrix3(3, 0) = 0D
            matrix3(3, 1) = 0D
            matrix3(3, 2) = 0D
            matrix3(3, 3) = 1D

            matrix = matrix3.Clone

        End If
        matrix = repairMatrix(matrix)
        matrix(0, 3) = matrix(0, 3) / 1000D
        matrix(1, 3) = matrix(1, 3) / 1000D
        matrix(2, 3) = matrix(2, 3) / 1000D
        Dim result(,) As Double = {{matrix(0, 0), matrix(0, 1), matrix(0, 2), matrix(0, 3)},
                                   {matrix(1, 0), matrix(1, 1), matrix(1, 2), matrix(1, 3)},
                                   {matrix(2, 0), matrix(2, 1), matrix(2, 2), matrix(2, 3)},
                                   {0.0, 0.0, 0.0, 1.0}}
        Return result
    End Function

    Public Function adjustSubfileMatrix(ByVal pmatrix(,) As Double, ByVal pmatrix2(,) As Double, ByVal projectMode As Byte, ByVal isSubfile As Boolean, Optional ByVal group As Integer = -1) As Double(,)


        Dim matrix(3, 3) As Decimal
        Dim matrix2(3, 3) As Decimal
        Dim matrix3(3, 3) As Decimal


        Dim smatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                     {0D, 1D, 0D, 0D},
                                     {0D, 0D, -1D, 0D},
                                     {0D, 0D, 0D, 1D}}
        matrix = smatrix.Clone

        If isSubfile Then
            Dim kmatrix(3, 3) As Decimal
            For r As Integer = 0 To 3
                For c As Integer = 0 To 3
                    kmatrix(r, c) = pmatrix2(r, c)
                Next
            Next
            matrix2 = matrix.Clone
            matrix = kmatrix.Clone

            matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
            matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
            matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
            matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

            matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
            matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
            matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
            matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

            matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
            matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
            matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
            matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

            matrix3(3, 0) = 0D
            matrix3(3, 1) = 0D
            matrix3(3, 2) = 0D
            matrix3(3, 3) = 1D

            matrix = matrix3.Clone
        End If

        For r As Integer = 0 To 3
            For c As Integer = 0 To 3
                matrix2(r, c) = pmatrix(r, c)
            Next
        Next

        matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
        matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
        matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
        matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

        matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
        matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
        matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
        matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

        matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
        matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
        matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
        matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

        matrix3(3, 0) = 0D
        matrix3(3, 1) = 0D
        matrix3(3, 2) = 0D
        matrix3(3, 3) = 1D

        matrix = matrix3.Clone

        If LPCFile.templateProjectionQuads.Count = 0 Or Not LPCFile.project Then
            Select Case projectMode
                Case 0 ' YZ mit -X (Right) [OK]
                    Dim cmatrix(,) As Decimal = {{0D, 0D, 1D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {1D, 0D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 1 ' XZ mit -Y (Top) [OK]
                    Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                                 {0D, 0D, 1D, 0D},
                                                 {0D, -1D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 2 ' XY mit -Z (Front) [OK] 
                    Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {0D, 0D, 1D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 3 ' YZ mit +X (Left) [OK]
                    Dim cmatrix(,) As Decimal = {{0D, 0D, -1D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {-1D, 0D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 4 ' XZ mit -Y (Bottom) [OK]
                    Dim cmatrix(,) As Decimal = {{-1D, 0D, 0D, 0D},
                                                 {0D, 0D, -1D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
                Case 5 ' XY mit +Z (Back) [OK] 
                    Dim cmatrix(,) As Decimal = {{1D, 0D, 0D, 0D},
                                                 {0D, 1D, 0D, 0D},
                                                 {0D, 0D, -1D, 0D},
                                                 {0D, 0D, 0D, 1D}}
                    matrix2 = cmatrix.Clone
            End Select
        Else
            ' identisch mit back
            Dim cmatrix(,) As Decimal = {{1D, 0D, 0D, 0D},
                                         {0D, 1D, 0D, 0D},
                                         {0D, 0D, -1D, 0D},
                                         {0D, 0D, 0D, 1D}}
            matrix2 = cmatrix.Clone
        End If

        matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
        matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
        matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
        matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

        matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
        matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
        matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
        matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

        matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
        matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
        matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
        matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

        matrix3(3, 0) = 0D
        matrix3(3, 1) = 0D
        matrix3(3, 2) = 0D
        matrix3(3, 3) = 1D

        matrix = matrix3.Clone

        matrix(0, 3) = matrix(0, 3) / 1000D
        matrix(1, 3) = matrix(1, 3) / 1000D
        matrix(2, 3) = matrix(2, 3) / 1000D

        If projectMode = LPCFile.myMetadata.recommendedMode Then

            Do
                If LPCFile.templateProjectionQuads.Count > 0 AndAlso LPCFile.project Then
                    selectGroupDirect(group)
                    For i = 0 To LPCFile.templateProjectionQuads.Count - 1
                        If LPCFile.templateProjectionQuads(i).isInQuad(View.SelectedTriangles) Then
                            For r As Integer = 0 To 3
                                For c As Integer = 0 To 3
                                    matrix2(r, c) = LPCFile.templateProjectionQuads(i).matrix(r, c)
                                Next
                            Next
                            matrix2 = repairMatrix(matrix2)
                            Dim det As Double = matrix2(0, 0) * (matrix2(1, 1) * matrix2(2, 2) - matrix2(1, 2) * matrix2(2, 1)) + matrix2(0, 1) * (matrix2(1, 2) * matrix2(2, 0) - matrix2(1, 0) * matrix2(2, 2)) + matrix2(0, 2) * (matrix2(1, 0) * matrix2(2, 1) - matrix2(1, 1) * matrix2(2, 0))
                            If CType(LPCFile.PrimitivesMetadataHMap(LPCFile.Primitives(LPCFile.PrimitivesHMap(group)).primitiveName), Metadata).mData(6) = "BFC CERTIFY CW" Xor det < 0 Then matrix(2, 2) = -matrix(2, 2)
                            Exit Do
                        End If
                    Next
                End If
                For r As Integer = 0 To 3
                    For c As Integer = 0 To 3
                        matrix2(r, c) = LPCFile.myMetadata.matrix(r, c)
                    Next
                Next
                Exit Do
            Loop

            matrix3(0, 0) = matrix2(0, 0) * matrix(0, 0) + matrix2(0, 1) * matrix(1, 0) + matrix2(0, 2) * matrix(2, 0)
            matrix3(0, 1) = matrix2(0, 0) * matrix(0, 1) + matrix2(0, 1) * matrix(1, 1) + matrix2(0, 2) * matrix(2, 1)
            matrix3(0, 2) = matrix2(0, 0) * matrix(0, 2) + matrix2(0, 1) * matrix(1, 2) + matrix2(0, 2) * matrix(2, 2)
            matrix3(0, 3) = matrix2(0, 0) * matrix(0, 3) + matrix2(0, 1) * matrix(1, 3) + matrix2(0, 2) * matrix(2, 3) + matrix2(0, 3)

            matrix3(1, 0) = matrix2(1, 0) * matrix(0, 0) + matrix2(1, 1) * matrix(1, 0) + matrix2(1, 2) * matrix(2, 0)
            matrix3(1, 1) = matrix2(1, 0) * matrix(0, 1) + matrix2(1, 1) * matrix(1, 1) + matrix2(1, 2) * matrix(2, 1)
            matrix3(1, 2) = matrix2(1, 0) * matrix(0, 2) + matrix2(1, 1) * matrix(1, 2) + matrix2(1, 2) * matrix(2, 2)
            matrix3(1, 3) = matrix2(1, 0) * matrix(0, 3) + matrix2(1, 1) * matrix(1, 3) + matrix2(1, 2) * matrix(2, 3) + matrix2(1, 3)

            matrix3(2, 0) = matrix2(2, 0) * matrix(0, 0) + matrix2(2, 1) * matrix(1, 0) + matrix2(2, 2) * matrix(2, 0)
            matrix3(2, 1) = matrix2(2, 0) * matrix(0, 1) + matrix2(2, 1) * matrix(1, 1) + matrix2(2, 2) * matrix(2, 1)
            matrix3(2, 2) = matrix2(2, 0) * matrix(0, 2) + matrix2(2, 1) * matrix(1, 2) + matrix2(2, 2) * matrix(2, 2)
            matrix3(2, 3) = matrix2(2, 0) * matrix(0, 3) + matrix2(2, 1) * matrix(1, 3) + matrix2(2, 2) * matrix(2, 3) + matrix2(2, 3)

            matrix3(3, 0) = 0D
            matrix3(3, 1) = 0D
            matrix3(3, 2) = 0D
            matrix3(3, 3) = 1D

            matrix = matrix3.Clone

        End If
        matrix = repairMatrix(matrix)
        Dim result(,) As Double = {{matrix(0, 0), matrix(0, 1), matrix(0, 2), matrix(0, 3)},
                                   {matrix(1, 0), matrix(1, 1), matrix(1, 2), matrix(1, 3)},
                                   {matrix(2, 0), matrix(2, 1), matrix(2, 2), matrix(2, 3)},
                                   {0.0, 0.0, 0.0, 1.0}}
        Return result
    End Function

    Private Function repairMatrix(ByVal matrix(,) As Decimal) As Decimal(,)
        Dim rmatrix(,) As Decimal = matrix.Clone
        ' All-Row/All Colum Zero:
        For r As Integer = 0 To 2
            Dim rsum As Decimal
            For c As Integer = 0 To 2
                rsum += Math.Abs(rmatrix(r, c))
            Next
            If Math.Abs(rsum) < 0.00001D Then
                For c As Integer = 0 To 2
                    Dim csum As Decimal = 0
                    For r2 As Integer = 0 To 2
                        csum += Math.Abs(rmatrix(r2, c))
                    Next
                    If Math.Abs(csum) < 0.00001D Then
                        rmatrix(r, c) = 1D
                    End If
                Next
            End If
        Next
        For c As Integer = 0 To 2
            Dim csum As Decimal = 0
            For r2 As Integer = 0 To 2
                csum += Math.Abs(rmatrix(r2, c))
            Next
            If Math.Abs(csum) < 0.00001D Then
                rmatrix(c, c) = 1.0
            End If
        Next
        ' Delete singularities:
        Dim det As Decimal = rmatrix(0, 0) * (rmatrix(1, 1) * rmatrix(2, 2) - rmatrix(1, 2) * rmatrix(2, 1)) + rmatrix(0, 1) * (rmatrix(1, 2) * rmatrix(2, 0) - rmatrix(1, 0) * rmatrix(2, 2)) + rmatrix(0, 2) * (rmatrix(1, 0) * rmatrix(2, 1) - rmatrix(1, 1) * rmatrix(2, 0))
        If Math.Abs(det) < 0.0001D Then
            For c As Integer = 0 To 2
                If rmatrix(c, c) = 0D Then
                    rmatrix(c, c) = 1D
                End If
            Next
        End If
        Return rmatrix.Clone
    End Function

End Module
