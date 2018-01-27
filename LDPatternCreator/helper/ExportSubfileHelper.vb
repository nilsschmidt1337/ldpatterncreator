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

Module ExportSubfileHelper
    Public Sub exportSubfile(ByVal fileName As String, ByVal group As Integer, ByRef tri As Integer)

        Dim subfileMetadata As Metadata = LPCFile.PrimitivesMetadataHMap(LPCFile.Primitives(LPCFile.PrimitivesHMap(group)).primitiveName)

        Helper_2D.clearSelection()

        View.SelectedTriangles.Add(LPCFile.Triangles(tri)) : LPCFile.Triangles(tri).selected = True
        View.SelectedVertices.Add(LPCFile.Triangles(tri).vertexA)
        View.SelectedVertices.Add(LPCFile.Triangles(tri).vertexB)
        View.SelectedVertices.Add(LPCFile.Triangles(tri).vertexC)

        SelectionHelper.selectPrimitiveGroup(group)

        Dim tempTris As New List(Of Triangle)

        For i As Integer = 0 To View.SelectedTriangles.Count - 1
            Dim v(2) As Vertex
            v(0) = View.SelectedTriangles(i).vertexA
            v(1) = View.SelectedTriangles(i).vertexB
            v(2) = View.SelectedTriangles(i).vertexC
            If v(0).vertexID <> v(1).vertexID AndAlso v(0).vertexID <> v(2).vertexID AndAlso v(1).vertexID <> v(2).vertexID Then tempTris.Add(New Triangle(New Vertex(v(0).X, v(0).Y, False, False), New Vertex(v(1).X, v(1).Y, False, False), New Vertex(v(2).X, v(2).Y, False, False), False) With {.myColourNumber = View.SelectedTriangles(i).myColourNumber})
            View.SelectedTriangles(i).selected = False
        Next
        Dim prim As Primitive = LPCFile.Primitives(LPCFile.PrimitivesHMap(group))
        Dim transmatrix(,) As Double = CType(prim.matrix.Clone, Double(,))

        transmatrix(0, 3) /= 1000
        transmatrix(1, 3) /= 1000

        transmatrix = Inverse(transmatrix)

        Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "tmp.dat", False, New System.Text.UTF8Encoding(False))
            If LPCFile.includeMetadata Then
                DateiOut.WriteLine("0 " & subfileMetadata.mData(0))
                DateiOut.WriteLine("0 Name: " & subfileMetadata.mData(1))
                DateiOut.WriteLine("0 Author: " & subfileMetadata.mData(2) & " [" & subfileMetadata.mData(3) & "]")
                DateiOut.WriteLine("0 !LDRAW_ORG " & subfileMetadata.mData(4))
                DateiOut.WriteLine("0 !LICENSE " & subfileMetadata.mData(5))
                DateiOut.WriteLine()
                If subfileMetadata.mData(6) <> "" Then
                    Dim alines() As String = Replace(subfileMetadata.mData(6), "<br>", "µ").Split("µ")
                    For i As Integer = 0 To alines.Length - 1
                        DateiOut.WriteLine("0 !HELP " & alines(i))
                    Next
                    If alines.Length > 0 Then DateiOut.WriteLine()
                End If
                If subfileMetadata.mData(7) = "BFC CERTIFY CCW" OrElse subfileMetadata.mData(7) = "BFC CERTIFY CW" OrElse subfileMetadata.mData(7) = "BFC NOCERTIFY" Then
                    DateiOut.WriteLine("0 " & subfileMetadata.mData(7))
                Else
                    DateiOut.WriteLine("0 BFC CERTIFY CCW")
                End If
                If subfileMetadata.mData(8) <> "" Then DateiOut.WriteLine("0 !CATEGORY " & subfileMetadata.mData(8))
                If subfileMetadata.mData(9) <> "" Then
                    Dim alines() As String = Replace(subfileMetadata.mData(9), "<br>", "µ").Split("µ")
                    For i As Integer = 0 To alines.Length - 1
                        DateiOut.WriteLine("0 !KEYWORDS " & alines(i))
                    Next
                    If alines.Length > 0 Then DateiOut.WriteLine()
                End If
                If subfileMetadata.mData(10) <> "" Then
                    Dim alines() As String = Replace(subfileMetadata.mData(10), "<br>", "µ").Split("µ")
                    For i As Integer = 0 To alines.Length - 1
                        DateiOut.WriteLine("0 !HISTORY " & alines(i))
                    Next
                    If alines.Length > 0 Then DateiOut.WriteLine()
                End If
                If subfileMetadata.mData(11) <> "" Then
                    Dim alines() As String = Replace(subfileMetadata.mData(11), "<br>", "µ").Split("µ")
                    For i As Integer = 0 To alines.Length - 1
                        DateiOut.WriteLine("0 // " & alines(i))
                    Next
                    If alines.Length > 0 Then DateiOut.WriteLine()
                End If
            Else
                DateiOut.WriteLine("0 BFC CERTIFY CCW")
            End If

            For i As Integer = 0 To tempTris.Count - 1
                Dim tri2 As Triangle = tempTris(i)
                Dim tx, ty As Double
                tx = transmatrix(0, 0) * tri2.vertexA.X + transmatrix(0, 1) * tri2.vertexA.Y + transmatrix(0, 3)
                ty = transmatrix(1, 0) * tri2.vertexA.X + transmatrix(1, 1) * tri2.vertexA.Y + transmatrix(1, 3)
                tri2.vertexA.X = tx
                tri2.vertexA.Y = ty
                tx = transmatrix(0, 0) * tri2.vertexB.X + transmatrix(0, 1) * tri2.vertexB.Y + transmatrix(0, 3)
                ty = transmatrix(1, 0) * tri2.vertexB.X + transmatrix(1, 1) * tri2.vertexB.Y + transmatrix(1, 3)
                tri2.vertexB.X = tx
                tri2.vertexB.Y = ty
                tx = transmatrix(0, 0) * tri2.vertexC.X + transmatrix(0, 1) * tri2.vertexC.Y + transmatrix(0, 3)
                ty = transmatrix(1, 0) * tri2.vertexC.X + transmatrix(1, 1) * tri2.vertexC.Y + transmatrix(1, 3)
                tri2.vertexC.X = tx
                tri2.vertexC.Y = ty
                tri2.normalize()
                tri2.checkWinding()
                DateiOut.WriteLine(Replace("3 " & tri2.getColourString & " " & -tri2.vertexA.X & " " & tri2.vertexA.Y & " 0 " _
                                                   & -tri2.vertexB.X & " " & tri2.vertexB.Y & " 0 " _
                                                   & -tri2.vertexC.X & " " & tri2.vertexC.Y & " 0", MathHelper.comma, "."))
            Next i
        End Using

        PostProcessing.rectifyAndUnify(fileName)

        tempTris.Clear()
        View.SelectedTriangles.Clear()
        View.SelectedVertices.Clear()
    End Sub
End Module
