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
Public Class MirrorHelper

    Public Shared deltaXmin As Double
    Public Shared deltaXmax As Double
    Public Shared deltaYmin As Double
    Public Shared deltaYmax As Double

    Public Shared Sub mirrorX()
        MirrorHelper.deltaXmin = Double.PositiveInfinity
        MirrorHelper.deltaXmax = Double.NegativeInfinity
        If View.SelectedVertices.Count > 0 Then
            If MainState.objectToModify = Modified.Primitive Then
                Dim tid As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).centerVertexID
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).mirror(True, False)
                For Each vert As Vertex In View.SelectedVertices
                    If vert.vertexID = tid Then
                        MainState.temp_center = New Vertex(vert.X, vert.Y, False, False)
                        Exit For
                    End If
                Next
            Else               
                MainState.temp_center = New Vertex(0, 0, False, False)
                For Each vert As Vertex In View.SelectedVertices
                    If vert.groupindex <> Primitive.NO_INDEX Then Exit Sub
                    MainState.temp_center += vert
                Next
                MainState.temp_center.X /= View.SelectedVertices.Count
                MainState.temp_center.Y /= View.SelectedVertices.Count
            End If
            For Each vert As Vertex In View.SelectedVertices
                vert.X = MainState.temp_center.X - (vert.X - MainState.temp_center.X)
                If (MainState.temp_center.X - vert.X) < MirrorHelper.deltaXmin Then MirrorHelper.deltaXmin = MainState.temp_center.X - vert.X
                If (MainState.temp_center.X - vert.X) > MirrorHelper.deltaXmax Then MirrorHelper.deltaXmax = MainState.temp_center.X - vert.X
            Next
        End If
    End Sub

    Public Shared Sub mirrorY()
        MirrorHelper.deltaYmin = Double.PositiveInfinity
        MirrorHelper.deltaYmax = Double.NegativeInfinity
        If View.SelectedVertices.Count > 0 Then
            If MainState.objectToModify = Modified.Primitive Then
                Dim tid As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).centerVertexID
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).mirror(False, True)
                For Each vert As Vertex In View.SelectedVertices
                    If vert.vertexID = tid Then
                        MainState.temp_center = New Vertex(vert.X, vert.Y, False, False)
                        Exit For
                    End If
                Next
            Else
                MainState.temp_center = New Vertex(0, 0, False, False)
                For Each vert As Vertex In View.SelectedVertices
                    If vert.groupindex <> Primitive.NO_INDEX Then Exit Sub
                    MainState.temp_center += vert
                Next
                MainState.temp_center.X /= View.SelectedVertices.Count
                MainState.temp_center.Y /= View.SelectedVertices.Count
            End If
            For Each vert As Vertex In View.SelectedVertices
                vert.Y = MainState.temp_center.Y - (vert.Y - MainState.temp_center.Y)
                If (MainState.temp_center.Y - vert.Y) < MirrorHelper.deltaYmin Then MirrorHelper.deltaYmin = MainState.temp_center.Y - vert.Y
                If (MainState.temp_center.Y - vert.Y) > MirrorHelper.deltaYmax Then MirrorHelper.deltaYmax = MainState.temp_center.Y - vert.Y
            Next
        End If
    End Sub
End Class
