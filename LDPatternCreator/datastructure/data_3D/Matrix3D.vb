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

Public Class Matrix3D
    Public m(3, 3) As Double

    Sub New()
        For c As Integer = 0 To 3 : m(c, c) = 1.0 : Next
    End Sub

    Shared Operator *(ByVal m1 As Matrix3D, ByVal m2 As Matrix3D) As Matrix3D
        Dim returnMatrix As New Matrix3D

        returnMatrix.m(0, 0) = m2.m(0, 0) * m1.m(0, 0) + m2.m(0, 1) * m1.m(1, 0) + m2.m(0, 2) * m1.m(2, 0)
        returnMatrix.m(0, 1) = m2.m(0, 0) * m1.m(0, 1) + m2.m(0, 1) * m1.m(1, 1) + m2.m(0, 2) * m1.m(2, 1)
        returnMatrix.m(0, 2) = m2.m(0, 0) * m1.m(0, 2) + m2.m(0, 1) * m1.m(1, 2) + m2.m(0, 2) * m1.m(2, 2)
        returnMatrix.m(0, 3) = m2.m(0, 0) * m1.m(0, 3) + m2.m(0, 1) * m1.m(1, 3) + m2.m(0, 2) * m1.m(2, 3) + m2.m(0, 3)

        returnMatrix.m(1, 0) = m2.m(1, 0) * m1.m(0, 0) + m2.m(1, 1) * m1.m(1, 0) + m2.m(1, 2) * m1.m(2, 0)
        returnMatrix.m(1, 1) = m2.m(1, 0) * m1.m(0, 1) + m2.m(1, 1) * m1.m(1, 1) + m2.m(1, 2) * m1.m(2, 1)
        returnMatrix.m(1, 2) = m2.m(1, 0) * m1.m(0, 2) + m2.m(1, 1) * m1.m(1, 2) + m2.m(1, 2) * m1.m(2, 2)
        returnMatrix.m(1, 3) = m2.m(1, 0) * m1.m(0, 3) + m2.m(1, 1) * m1.m(1, 3) + m2.m(1, 2) * m1.m(2, 3) + m2.m(1, 3)

        returnMatrix.m(2, 0) = m2.m(2, 0) * m1.m(0, 0) + m2.m(2, 1) * m1.m(1, 0) + m2.m(2, 2) * m1.m(2, 0)
        returnMatrix.m(2, 1) = m2.m(2, 0) * m1.m(0, 1) + m2.m(2, 1) * m1.m(1, 1) + m2.m(2, 2) * m1.m(2, 1)
        returnMatrix.m(2, 2) = m2.m(2, 0) * m1.m(0, 2) + m2.m(2, 1) * m1.m(1, 2) + m2.m(2, 2) * m1.m(2, 2)
        returnMatrix.m(2, 3) = m2.m(2, 0) * m1.m(0, 3) + m2.m(2, 1) * m1.m(1, 3) + m2.m(2, 2) * m1.m(2, 3) + m2.m(2, 3)

        returnMatrix.m(3, 0) = 0.0
        returnMatrix.m(3, 1) = 0.0
        returnMatrix.m(3, 2) = 0.0
        returnMatrix.m(3, 3) = 1.0

        Return returnMatrix
    End Operator

    Shared Operator *(ByVal m1 As Matrix3D, ByVal v1 As Vertex3D) As Vertex3D
        Dim returnVertex As New Vertex3D
        returnVertex.X = m1.m(0, 0) * v1.X + m1.m(0, 1) * v1.Y + m1.m(0, 2) * v1.Z + m1.m(0, 3)
        returnVertex.Y = m1.m(1, 0) * v1.X + m1.m(1, 1) * v1.Y + m1.m(1, 2) * v1.Z + m1.m(1, 3)
        returnVertex.Z = m1.m(2, 0) * v1.X + m1.m(2, 1) * v1.Y + m1.m(2, 2) * v1.Z + m1.m(2, 3)
        Return returnVertex
    End Operator
End Class
