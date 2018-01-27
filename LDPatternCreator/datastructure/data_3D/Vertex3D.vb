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

Public Structure Vertex3D
    Public X As Double
    Public Y As Double
    Public Z As Double
    Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        Me.X = x
        Me.Y = y
        Me.Z = z
    End Sub

    Public Function cross(ByVal target As Vertex3D) As Vertex3D
        Return New Vertex3D(Me.Y * target.Z - Me.Z * target.Y, Me.Z * target.X - Me.X * target.Z, Me.X * target.Y - Me.Y * target.X)
    End Function

    Public Function angleBetween(ByVal target As Vertex3D) As Double
        Return Math.Acos((Me * target) / (Me.abs * target.abs))
    End Function

    Public Function linearCoeff2(ByVal target As Vertex3D) As Single
        Dim px As Double = 0.0
        Me.normalize() : target.normalize()
        px = Me * target
        If px < 0 Then
            Return -1
        Else
            Return 1
        End If
    End Function

    Public Shared Operator *(ByVal v1 As Vertex3D, ByVal v2 As Vertex3D) As Double
        Return ((v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z))
    End Operator

    Public Shared Operator +(ByVal v1 As Vertex3D, ByVal v2 As Vertex3D) As Vertex3D
        Return New Vertex3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z)
    End Operator

    Public Shared Operator -(ByVal v1 As Vertex3D, ByVal v2 As Vertex3D) As Vertex3D
        Return New Vertex3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z)
    End Operator

    Public Function abs() As Double
        Return Math.Sqrt(Me.X ^ 2 + Me.Y ^ 2 + Me.Z ^ 2)
    End Function

    Public Sub normalize()
        Dim a As Double = Me.abs()
        If a > 0.00001 Then
            Me.X = Fix(Me.X / a * 10000) / 10000
            Me.Y = Fix(Me.Y / a * 10000) / 10000
            Me.Z = Fix(Me.Z / a * 10000) / 10000
        End If
    End Sub
End Structure
