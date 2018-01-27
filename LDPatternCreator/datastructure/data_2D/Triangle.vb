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
Imports System.Runtime.Serialization
Imports System.Text
Public Class Triangle
    Implements IEquatable(Of Triangle)

    Public vertexA As Vertex
    Public vertexB As Vertex
    Public vertexC As Vertex
    Public selected As Boolean
    Public myColour As Color
    Public myColourNumber As Short
    Public groupindex As Integer
    Public triangleID As Integer

    Overloads Function Equals(ByVal other As Triangle) As Boolean Implements IEquatable(Of Triangle).Equals
        Return Me.triangleID = other.triangleID
    End Function

    Sub New()

    End Sub

    Sub New(ByVal v1 As Vertex, ByVal v2 As Vertex, ByVal v3 As Vertex, Optional ByVal needID As Boolean = True)
        Me.vertexA = v1
        Me.vertexB = v2
        Me.vertexC = v3
        Me.groupindex = -1
        Me.myColourNumber = 16
        If needID Then
            GlobalIdSet.triangleIDglobal += 1
            Me.triangleID = GlobalIdSet.triangleIDglobal
        End If
    End Sub

    Public Sub normalize()
        Dim tmpVertex As Vertex
        tmpVertex = vertexA : If vertexA.X > vertexB.X Then vertexA = vertexB : vertexB = tmpVertex
        tmpVertex = vertexA : If vertexA.X > vertexC.X Then vertexA = vertexC : vertexC = tmpVertex
        tmpVertex = vertexB : If vertexB.X > vertexC.X Then vertexB = vertexC : vertexC = tmpVertex

        tmpVertex = vertexA : If vertexA.Y > vertexB.Y Then vertexA = vertexB : vertexB = tmpVertex
        tmpVertex = vertexA : If vertexA.Y > vertexC.Y Then vertexA = vertexC : vertexC = tmpVertex
        tmpVertex = vertexB : If vertexB.Y > vertexC.Y Then vertexB = vertexC : vertexC = tmpVertex
    End Sub

    Public Function getCenter() As Vertex
        Return New Vertex((vertexA.X + vertexB.X + vertexC.X) / 3, (vertexA.Y + vertexB.Y + vertexC.Y) / 3, False, False)
    End Function

    Public Function getFingerprint() As String
        Dim sb As New StringBuilder()
        sb.Append(vertexA.X)
        sb.Append(vertexA.Y)
        sb.Append(vertexB.X)
        sb.Append(vertexB.Y)
        sb.Append(vertexC.X)
        sb.Append(vertexC.Y)
        Return sb.ToString
    End Function

    Public Sub checkWinding()
        Dim v1 As New Vertex(0, 0, False, False)
        Dim v2 As New Vertex(0, 0, False, False)
        v1 = Me.vertexB - Me.vertexA
        v2 = Me.vertexC - Me.vertexA
        If LPCFile.myMetadata.mData(7) = "" OrElse Not LPCFile.includeMetadata OrElse (LPCFile.includeMetadata AndAlso (LPCFile.myMetadata.mData(7) = "BFC CERTIFY CCW" OrElse LPCFile.myMetadata.mData(7) = "BFC NOCERTIFY")) Then
            If (v1.X * v2.Y - v1.Y * v2.X) < 0 Then
                Dim tv As Vertex
                tv = Me.vertexB
                Me.vertexB = Me.vertexC
                Me.vertexC = tv
            End If
        Else
            If (v1.X * v2.Y - v1.Y * v2.X) >= 0 Then
                Dim tv As Vertex
                tv = Me.vertexB
                Me.vertexB = Me.vertexC
                Me.vertexC = tv
            End If
        End If
    End Sub

    Public Function maxAngle() As Double
        Dim a1 As Double = Math.Abs(Me.vertexA.angle(vertexB) - Me.vertexA.angle(Me.vertexC))
        If a1 > Math.PI Then a1 = Math.PI * 2.0 - a1
        Dim a2 As Double = Math.Abs(Me.vertexB.angle(vertexA) - Me.vertexB.angle(Me.vertexC))
        If a2 > Math.PI Then a2 = Math.PI * 2.0 - a2
        Dim a3 As Double = Math.Abs(Me.vertexC.angle(vertexB) - Me.vertexC.angle(Me.vertexA))
        If a3 > Math.PI Then a3 = Math.PI * 2.0 - a3
        Return Math.Max(a1, Math.Max(a2, a3))
    End Function

    Public Function getColourString()
        If myColourNumber = -1 Then
            Return "0x2" & myColour.R.ToString("X2") & myColour.G.ToString("X2") & myColour.B.ToString("X2")
        End If
        Return myColourNumber
    End Function

End Class
