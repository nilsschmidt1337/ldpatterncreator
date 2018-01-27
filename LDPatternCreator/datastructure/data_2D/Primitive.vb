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

<Serializable()> _
Public Class Primitive
    Implements System.Runtime.Serialization.ISerializable

    Public Const TEMPLATE_INDEX As Integer = Integer.MaxValue
    Public Const NO_INDEX As Integer = -1

    Public primitiveID As Integer
    Public centerVertexID As Integer
    Public matrix(3, 3) As Double
    Public matrixR(3, 3) As Double
    Public primitiveName As String
    Public myColour As Color
    Public myColourNumber As Short = 16
    Public ox As Double
    Public oy As Double

    Sub New()

    End Sub

    Sub New(ByVal vx As Double, ByVal vy As Double, ByVal offset_x As Double, ByVal offset_y As Double, ByVal name As String, ByVal centerVertexID As Integer, Optional ByVal needID As Boolean = True)
        Me.matrix(0, 3) = vx
        Me.matrix(1, 3) = vy
        Me.matrix(0, 0) = 1
        Me.matrix(1, 1) = 1
        Me.matrix(2, 2) = -1
        Me.matrix(3, 3) = 1
        For i As Integer = 0 To 3
            Me.matrixR(i, i) = 1.0
        Next
        Me.ox = offset_x
        Me.oy = offset_y
        If needID Then
            GlobalIdSet.primitiveIDglobal += 1
            Me.primitiveID = GlobalIdSet.primitiveIDglobal
        End If
        Me.primitiveName = name
        Me.centerVertexID = centerVertexID
    End Sub

    Public Sub mirror(ByVal on_x As Boolean, ByVal on_Y As Boolean)
        Dim vert As Vertex = New Vertex(ox, oy, False, False)
        Dim fx As Double = 1
        Dim fy As Double = 1
        Dim tox, toy As Double
        If on_x Then fx = -1
        If on_Y Then fy = -1
        tox = matrix(0, 3)
        toy = matrix(1, 3)
        translate(-matrix(0, 3), matrix(1, 3))
        translate(-ox, oy)

        ' Spiegeln..
        Dim matrix2(,) As Double = {{fx, 0, 0, 0}, _
                                    {0, fy, 0, 0}, _
                                    {0, 0, 1, 0}, _
                                    {0, 0, 0, 1}}
        Dim matrix3(3, 3) As Double

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

        matrix3(3, 0) = 0
        matrix3(3, 1) = 0
        matrix3(3, 2) = 0
        matrix3(3, 3) = 1

        matrix = matrix3

        translate(ox, -oy)
        translate(tox, -toy)
        If on_x Then ox *= -1
        If on_Y Then oy *= -1
    End Sub

    Public Sub translate(ByVal dx As Double, ByVal dy As Double)
        Dim matrix2(,) As Double = {{1, 0, 0, dx}, _
                                    {0, 1, 0, -dy}, _
                                    {0, 0, 1, 0}, _
                                    {0, 0, 0, 1}}
        Dim matrix3(3, 3) As Double

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

        matrix3(3, 0) = 0
        matrix3(3, 1) = 0
        matrix3(3, 2) = 0
        matrix3(3, 3) = 1

        matrix = matrix3
    End Sub

    Public Sub rotate(ByVal r As Double)
        Dim vert As Vertex = New Vertex(ox, oy, False, False)
        Dim angle As Double = vert.MYangle
        Dim dist As Double = Math.Sqrt(vert.X ^ 2 + vert.Y ^ 2)
        Dim tox, toy As Double
        tox = matrix(0, 3)
        toy = matrix(1, 3)
        translate(-matrix(0, 3), matrix(1, 3))
        translate(-ox, oy)

        ' Rotieren..
        Dim matrix2(,) As Double = {{Math.Cos(r), -Math.Sin(r), 0, 0}, _
                                    {Math.Sin(r), Math.Cos(r), 0, 0}, _
                                    {0, 0, 1, 0}, _
                                    {0, 0, 0, 1}}
        Dim matrix3(3, 3) As Double

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

        matrix3(3, 0) = 0
        matrix3(3, 1) = 0
        matrix3(3, 2) = 0
        matrix3(3, 3) = 1

        matrix = matrix3

        translate(ox, -oy)
        translate(tox, -toy)
        ox = Math.Cos(angle + r) * dist
        oy = Math.Sin(angle + r) * dist
    End Sub

    Public Sub scale(ByVal sx As Double, ByVal sy As Double)
        Dim vert As Vertex = New Vertex(ox, oy, False, False)
        Dim angle As Double = vert.MYangle
        Dim dist As Double = Math.Sqrt(vert.X ^ 2 + vert.Y ^ 2)
        Dim tox, toy As Double
        tox = matrix(0, 3)
        toy = matrix(1, 3)
        translate(-matrix(0, 3), matrix(1, 3))
        translate(-ox, oy)

        ' Scalieren..
        Dim matrix2(,) As Double = {{sx, 0, 0, 0}, _
                                    {0, sy, 0, 0}, _
                                    {0, 0, 1, 0}, _
                                    {0, 0, 0, 1}}
        Dim matrix3(3, 3) As Double

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

        matrix3(3, 0) = 0
        matrix3(3, 1) = 0
        matrix3(3, 2) = 0
        matrix3(3, 3) = 1

        matrix = matrix3

        translate(ox, -oy)
        translate(tox, -toy)
        ox *= sx
        oy *= sy
    End Sub

    Private Sub ISerializable_GetObjectData(ByVal oInfo As SerializationInfo, ByVal oContext As StreamingContext) Implements ISerializable.GetObjectData
        Dim tempinteger As Integer
        Dim tempmatrix(3, 3) As Double
        Dim tempstring As String = ""
        Dim tempdouble As Double
        Dim tempshort As Short
        Dim tempcolour As Color = Color.Black
        oInfo.AddValue("primitiveID", Me.primitiveID, tempinteger.GetType())
        oInfo.AddValue("centerVertexID", Me.centerVertexID, tempinteger.GetType())
        oInfo.AddValue("matrix", Me.matrix, tempmatrix.GetType())
        oInfo.AddValue("matrixR", Me.matrixR, tempmatrix.GetType())
        oInfo.AddValue("primitiveName", Me.primitiveName, tempstring.GetType())
        oInfo.AddValue("myColour", Me.myColour, MainState.tempcolour.GetType())
        oInfo.AddValue("myColourNumber", Me.myColourNumber, tempshort.GetType())
        oInfo.AddValue("ox", Me.ox, tempdouble.GetType())
        oInfo.AddValue("oy", Me.oy, tempdouble.GetType())
    End Sub

    Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        Dim tempinteger As Integer
        Dim tempmatrix(3, 3) As Double
        Dim tempstring As String = ""
        Dim tempdouble As Double
        Dim tempcolour As Color = Color.Black
        Me.primitiveID = info.GetValue("primitiveID", tempinteger.GetType())
        Me.centerVertexID = info.GetValue("centerVertexID", tempinteger.GetType())
        Me.matrix = info.GetValue("matrix", tempmatrix.GetType())
        Me.matrixR = info.GetValue("matrixR", tempmatrix.GetType())
        Me.primitiveName = info.GetValue("primitiveName", tempstring.GetType())
        Me.myColour = info.GetValue("myColour", MainState.tempcolour.GetType)
        Me.myColourNumber = info.GetInt16("myColourNumber")
        Me.ox = info.GetValue("ox", tempdouble.GetType())
        Me.oy = info.GetValue("oy", tempdouble.GetType())
    End Sub

    Public Function getColourString() As String
        If myColourNumber = -1 Then
            Return "0x2" & myColour.R.ToString("X2") & myColour.G.ToString("X2") & myColour.B.ToString("X2")
        End If
        Return myColourNumber
    End Function

End Class
