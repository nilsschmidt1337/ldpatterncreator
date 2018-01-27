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

Public Structure Vertex3D2
    Public X As Decimal
    Public Y As Decimal
    Public Z As Decimal
    Sub New(ByVal x As Decimal, ByVal y As Decimal, ByVal z As Decimal)
        Me.X = x
        Me.Y = y
        Me.Z = z
    End Sub

    Public Function dist(ByVal v2 As Vertex3D2) As Double
        Return Math.Sqrt((Me.X - v2.X) ^ 2D + (Me.Y - v2.Y) ^ 2D + (Me.Z - v2.Z) ^ 2D)
    End Function

    Public Shared Function distanceVectorFromVertexToLine(ByRef v1 As Vertex3D2, ByVal a As Vertex3D2, ByVal b As Vertex3D2) As Vertex3D2
        Dim iter As Integer = 0
        Dim vt1 As Vertex3D2 = a
        Dim vt2 As Vertex3D2 = b
        Do
            If v1.dist(vt1) < v1.dist(vt2) Then
                vt2 = New Vertex3D2((vt1.X + vt2.X) / 2D, (vt1.Y + vt2.Y) / 2D, (vt1.Z + vt2.Z) / 2D)
            Else
                vt1 = New Vertex3D2((vt1.X + vt2.X) / 2D, (vt1.Y + vt2.Y) / 2D, (vt1.Z + vt2.Z) / 2D)
            End If
            iter += 1
        Loop Until iter = 32
        If v1.dist(vt1) < v1.dist(vt2) Then
            Return vt1
        Else
            Return vt2
        End If
    End Function
End Structure
