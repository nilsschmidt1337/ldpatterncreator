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
Public Class Vertex
    Implements IComparable(Of Vertex)
    Implements IEquatable(Of Vertex)

    Public X As Double
    Public Y As Double
    Public oldX As Double
    Public oldY As Double
    Public selected As Boolean

    Public angleFrom As Double
    Public distanceFrom As Double

    Public linkedTriangles As New List(Of Triangle)

    Public groupindex As Integer

    Public vertexID As Integer

    Overloads Function Equals(ByVal other As Vertex) As Boolean Implements IEquatable(Of Vertex).Equals
        Return Me.vertexID = other.vertexID
    End Function

    Sub New()

    End Sub

    Sub New(ByVal vx As Double, ByVal vy As Double, ByVal sel As Boolean, Optional ByVal needID As Boolean = True)
        Me.X = vx
        Me.Y = vy
        Me.selected = sel
        Me.groupindex = Primitive.NO_INDEX
        If needID Then
            GlobalIdSet.vertexIDglobal += 1
            Me.vertexID = GlobalIdSet.vertexIDglobal
        End If
    End Sub

    Public Shared Operator <>(ByVal v1 As Vertex, ByVal v2 As Vertex) As Boolean
        Return v1.X <> v2.X OrElse v1.Y <> v2.Y
    End Operator

    Public Shared Operator =(ByVal v1 As Vertex, ByVal v2 As Vertex) As Boolean
        Return v1.X = v2.X AndAlso v1.Y = v2.Y
    End Operator

    Public Shared Operator +(ByVal v1 As Vertex, ByVal v2 As Vertex) As Vertex
        Return New Vertex(v1.X + v2.X, v1.Y + v2.Y, False, False)
    End Operator

    Public Shared Operator -(ByVal v1 As Vertex, ByVal v2 As Vertex) As Vertex
        Return New Vertex(v1.X - v2.X, v1.Y - v2.Y, False, False)
    End Operator

    Public Shared Operator *(ByVal v1 As Vertex, ByVal v2 As Vertex) As Double
        Return v1.X * v2.X + v1.Y * v2.Y
    End Operator

    Public Shared Operator *(ByVal v1 As Vertex, ByVal v2 As Double) As Vertex
        Return New Vertex(v1.X * v2, v1.Y * v2, False, False)
    End Operator

    Public Shared Operator >(ByVal v1 As Vertex, ByVal v2 As Vertex) As Boolean
        Return v1.angleFrom > v2.angleFrom
    End Operator

    Public Shared Operator >=(ByVal v1 As Vertex, ByVal v2 As Vertex) As Boolean
        Return v1.angleFrom >= v2.angleFrom
    End Operator

    Public Shared Operator <(ByVal v1 As Vertex, ByVal v2 As Vertex) As Boolean
        Return v1.angleFrom < v2.angleFrom
    End Operator

    Public Shared Operator <=(ByVal v1 As Vertex, ByVal v2 As Vertex) As Boolean
        Return v1.angleFrom <= v2.angleFrom
    End Operator

    Public Overloads Function CompareTo(ByVal other As Vertex) As Integer Implements IComparable(Of Vertex).CompareTo
        If Me > other Then
            Return 1
        ElseIf Me < other Then
            Return -1
        Else
            Return 0
        End If
    End Function

    Public Function angle(ByVal v2 As Vertex) As Double
        Dim angle2, tx, ty As Double
        tx = v2.X - Me.X
        ty = v2.Y - Me.Y
        angle2 = Math.Atan2(ty, tx)
        If angle2 < 0 Then angle2 += MathHelper.twoPI
        Return angle2
    End Function

    Public Function MYangle() As Double
        Dim angle2 As Double
        angle2 = Math.Atan2(Me.Y, Me.X)
        If angle2 < 0 Then angle2 += MathHelper.twoPI
        Return angle2
    End Function

    Public Function dist(ByVal v2 As Vertex) As Double
        Return Math.Sqrt((Me.X - v2.X) ^ 2 + (Me.Y - v2.Y) ^ 2)
    End Function

End Class
