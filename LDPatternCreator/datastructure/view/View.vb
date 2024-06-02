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

Public Class View

    Public Const pointsize As Integer = 4
    Public Const pointsizeHalf As Integer = 2

    Public Shared zoomlevel As Integer
    Public Shared zoomfactor As Double = 2

    Public Shared SelectedVertices As New List(Of Vertex)
    Public Shared SelectedTriangles As New List(Of Triangle)
    Public Shared CollisionVertices As New List(Of Vertex)
    Public Shared TriangulationVertices As New List(Of Vertex)
    Public Shared TriangulationVerticesInCircle As New List(Of Vertex)

    Public Shared unit As String = "LDU"
    Public Shared unitFactor As Double = 1

    Public Shared moveSnap As Double = 1
    Public Shared rasterSnap As Double = 1
    Public Shared rotateSnap As Double = 1
    Public Shared scaleSnap As Double = 0.01F

    Public Shared alpha As Byte = 255

    Public Shared imgPath As String
    Public Shared imgOffsetX As Integer = 0
    Public Shared imgOffsetY As Integer = 0
    Public Shared imgScale As Double = 100

    Public Shared backgroundPicture As Bitmap = My.Resources.temp
    Public Shared backgroundPictureBrush As New TextureBrush(My.Resources.temp)
    Public Shared collosionBrush As New TextureBrush(My.Resources.collision)

    Public Shared offsetX As Double
    Public Shared offsetY As Double
    Public Shared oldOffsetX As Double
    Public Shared oldOffsetY As Double

    Public Shared viewAbsOffsetX As Integer
    Public Shared viewAbsOffsetY As Integer

    Public Shared showGrid As Boolean

    Public Shared correctionOffsetX As Integer = 0
    Public Shared correctionOffsetY As Integer = 30
    Public Shared getCorrectionOffset As Boolean = False

    Public Shared selectionRadius As Integer = 100
End Class
