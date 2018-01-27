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

Module CanBuildHelper
    Public Function canBuiltTriangle(ByVal v1 As Vertex, ByVal v2 As Vertex, ByVal v3 As Vertex) As Boolean
        For Each tri1 As Triangle In v1.linkedTriangles
            For Each tri2 As Triangle In v2.linkedTriangles
                If tri1.Equals(tri2) Then
                    For Each tri3 As Triangle In v3.linkedTriangles
                        If tri3.Equals(tri2) Then
                            Return False
                        End If
                    Next
                End If
            Next
        Next
        Return True
    End Function

    Public Function canBuiltQuad(ByVal v1 As Vertex, ByVal v2 As Vertex, ByVal v3 As Vertex, ByVal v4 As Vertex) As Boolean
        Dim delta1 As Double = v1.Y - v2.Y
        Dim delta2 As Double = v2.X - v1.X
        Dim prod1 As Double = v1.X * v2.Y - v2.X * v1.Y
        Dim triangle_ABC As Double = delta1 * v3.X + delta2 * v3.Y + prod1
        Dim triangle_ABD As Double = delta1 * v4.X + delta2 * v4.Y + prod1
        Dim triangle_BCD As Double = (v2.Y - v3.Y) * v4.X + (v3.X - v2.X) * v4.Y + (v2.X * v3.Y - v3.X * v2.Y)
        Dim triangle_CAD As Double = (v3.Y - v1.Y) * v4.X + (v1.X - v3.X) * v4.Y + (v3.X * v1.Y - v1.X * v3.Y)
        If Math.Abs(triangle_ABC) < 0.01 OrElse Math.Abs(triangle_ABD) < 0.01 OrElse Math.Abs(triangle_BCD) < 0.01 OrElse Math.Abs(triangle_CAD) < 0.01 Then Return False
        Dim c As Integer
        If triangle_ABC < 0 Then c += 1
        If triangle_ABD < 0 Then c += 1
        If triangle_BCD < 0 Then c += 1
        If triangle_CAD < 0 Then c += 1
        Return c <> 2 AndAlso c <> 4 AndAlso canBuiltTriangle(v1, v2, v3) AndAlso canBuiltTriangle(v2, v3, v4) AndAlso canBuiltTriangle(v3, v1, v4) AndAlso canBuiltTriangle(v4, v1, v2)
    End Function
End Module
