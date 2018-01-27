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
Public Class ProjectionQuad
    Implements System.Runtime.Serialization.ISerializable

    Public inCoords(3, 2) As Decimal ' Input Koordinaten des Quads, XY als Basis, Z ist immer 0 und wird im Template nicht angegeben
    Public outCoords(3, 2) As Decimal ' Ziel Koordinaten (XYZ), auf diese Koordinaten wird der Quad projeziert
    Public matrix(3, 3) As Decimal

    Public isTriangle As Boolean

    Private beamdir() As Decimal = {0, 0, -1}

    Sub New()
    End Sub

    Sub New(ByVal x1 As Double, ByVal y1 As Double, _
             ByVal x2 As Double, ByVal y2 As Double, _
             ByVal x3 As Double, ByVal y3 As Double, _
             ByVal x4 As Double, ByVal y4 As Double, _
             ByVal px1 As Double, ByVal py1 As Double, ByVal pz1 As Double, _
             ByVal px2 As Double, ByVal py2 As Double, ByVal pz2 As Double, _
             ByVal px3 As Double, ByVal py3 As Double, ByVal pz3 As Double, _
             ByVal px4 As Double, ByVal py4 As Double, ByVal pz4 As Double)
        Const precision As Integer = 10
        Const precision2 As Integer = 10
        inCoords(0, 0) = Math.Round(x1, precision) : inCoords(0, 1) = Math.Round(y1, precision) : inCoords(0, 2) = 0
        inCoords(1, 0) = Math.Round(x2, precision) : inCoords(1, 1) = Math.Round(y2, precision) : inCoords(1, 2) = 0
        inCoords(2, 0) = Math.Round(x3, precision) : inCoords(2, 1) = Math.Round(y3, precision) : inCoords(2, 2) = 0
        inCoords(3, 0) = Math.Round(x4, precision) : inCoords(3, 1) = Math.Round(y4, precision) : inCoords(3, 2) = 0
        isTriangle = inCoords(0, 0) = inCoords(1, 0) AndAlso inCoords(0, 1) = inCoords(1, 1)
        outCoords(0, 0) = Math.Round(px1, precision2) : outCoords(0, 1) = Math.Round(py1, precision2) : outCoords(0, 2) = Math.Round(pz1, precision2)
        outCoords(1, 0) = Math.Round(px2, precision2) : outCoords(1, 1) = Math.Round(py2, precision2) : outCoords(1, 2) = Math.Round(pz2, precision2)
        outCoords(2, 0) = Math.Round(px3, precision2) : outCoords(2, 1) = Math.Round(py3, precision2) : outCoords(2, 2) = Math.Round(pz3, precision2)
        outCoords(3, 0) = Math.Round(px4, precision2) : outCoords(3, 1) = Math.Round(py4, precision2) : outCoords(3, 2) = Math.Round(pz4, precision2)
    End Sub

    Overrides Function toString() As String
        If isTriangle Then
            Return inCoords(1, 0) & " " & inCoords(1, 1) _
            & " " & inCoords(1, 0) & " " & inCoords(1, 1) _
            & " " & inCoords(2, 0) & " " & inCoords(2, 1) _
            & " " & inCoords(3, 0) & " " & inCoords(3, 1) _
            & " " & outCoords(1, 0) & " " & outCoords(1, 1) & " " & outCoords(1, 2) _
            & " " & outCoords(1, 0) & " " & outCoords(1, 1) & " " & outCoords(1, 2) _
            & " " & outCoords(2, 0) & " " & outCoords(2, 1) & " " & outCoords(2, 2) _
            & " " & outCoords(3, 0) & " " & outCoords(3, 1) & " " & outCoords(3, 2)
        Else
            Return inCoords(0, 0) & " " & inCoords(0, 1) _
            & " " & inCoords(1, 0) & " " & inCoords(1, 1) _
            & " " & inCoords(2, 0) & " " & inCoords(2, 1) _
            & " " & inCoords(3, 0) & " " & inCoords(3, 1) _
            & " " & outCoords(0, 0) & " " & outCoords(0, 1) & " " & outCoords(0, 2) _
            & " " & outCoords(1, 0) & " " & outCoords(1, 1) & " " & outCoords(1, 2) _
            & " " & outCoords(2, 0) & " " & outCoords(2, 1) & " " & outCoords(2, 2) _
            & " " & outCoords(3, 0) & " " & outCoords(3, 1) & " " & outCoords(3, 2)
        End If
    End Function

    Shadows Function Clone() As ProjectionQuad
        Return New ProjectionQuad() With {.inCoords = Me.inCoords.Clone, .outCoords = Me.outCoords.Clone, .matrix = Me.matrix.Clone, .isTriangle = Me.isTriangle}
    End Function

    Private Sub ISerializable_GetObjectData(ByVal oInfo As SerializationInfo, ByVal oContext As StreamingContext) Implements ISerializable.GetObjectData
        oInfo.AddValue("matrix", Me.matrix, matrix.GetType())
        oInfo.AddValue("inCoords", Me.inCoords, inCoords.GetType())
        oInfo.AddValue("outCoords", Me.outCoords, outCoords.GetType())
        oInfo.AddValue("isTriangle", Me.isTriangle, isTriangle.GetType())
    End Sub

    Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        Dim tempmatrix(3, 3) As Decimal
        Dim tempcoords(3, 2) As Decimal
        Dim tempboolean As Boolean
        Me.matrix = info.GetValue("matrix", tempmatrix.GetType())
        Me.inCoords = info.GetValue("inCoords", tempcoords.GetType())
        Me.outCoords = info.GetValue("outCoords", tempcoords.GetType())
        Me.isTriangle = info.GetValue("isTriangle", tempboolean.GetType())
    End Sub

    Public Sub CalculateMatrix()
        ' Create a 4 x 5 - Matrix for each coordinate..
        ' dest_x1 = unknown_matrix(0, 0)  * source_x1 + unknown_matrix(0, 1)  * source_y1 + unknown_matrix(0, 2)  * source_z1 + unknown_matrix(0, 3) * 1

        't(0) = matrix(0, 0) * v(i, 0) + matrix(0, 1) * v(i, 1) + matrix(0, 2) * v(i, 2) + matrix(0, 3)
        't(1) = matrix(1, 0) * v(i, 0) + matrix(1, 1) * v(i, 1) + matrix(1, 2) * v(i, 2) + matrix(1, 3)
        't(2) = matrix(2, 0) * v(i, 0) + matrix(2, 1) * v(i, 1) + matrix(2, 2) * v(i, 2) + matrix(2, 3)
        Dim i_backup(3, 2) As Decimal
        Dim o_backup(3, 2) As Decimal
        i_backup = inCoords.Clone
        o_backup = outCoords.Clone
        If isTriangle Then
            For k As Integer = 0 To 2
                inCoords(0, k) = inCoords(2, k) + inCoords(3, k) - inCoords(1, k)
            Next
            For k As Integer = 0 To 2
                outCoords(0, k) = outCoords(2, k) + outCoords(3, k) - outCoords(1, k)
            Next
        End If

        Do
            Dim matrix_X(,) As Decimal = {{inCoords(0, 0), inCoords(0, 1), 0, 1, outCoords(0, 0)}, _
                                          {inCoords(1, 0), inCoords(1, 1), 0, 1, outCoords(1, 0)}, _
                                          {inCoords(2, 0), inCoords(2, 1), 0, 1, outCoords(2, 0)}, _
                                          {inCoords(3, 0), inCoords(3, 1), 0, 1, outCoords(3, 0)}}
            Dim solution_X() As Decimal = GaussianElimination(matrix_X)
            If solution_X Is Nothing Then Exit Do
            Dim matrix_Y(,) As Decimal = {{inCoords(0, 0), inCoords(0, 1), 0, 1, outCoords(0, 1)}, _
                                          {inCoords(1, 0), inCoords(1, 1), 0, 1, outCoords(1, 1)}, _
                                          {inCoords(2, 0), inCoords(2, 1), 0, 1, outCoords(2, 1)}, _
                                          {inCoords(3, 0), inCoords(3, 1), 0, 1, outCoords(3, 1)}}
            Dim solution_Y() As Decimal = GaussianElimination(matrix_Y)
            If solution_Y Is Nothing Then Exit Do
            Dim matrix_Z(,) As Decimal = {{inCoords(0, 0), inCoords(0, 1), 0, 1, outCoords(0, 2)}, _
                                          {inCoords(1, 0), inCoords(1, 1), 0, 1, outCoords(1, 2)}, _
                                          {inCoords(2, 0), inCoords(2, 1), 0, 1, outCoords(2, 2)}, _
                                          {inCoords(3, 0), inCoords(3, 1), 0, 1, outCoords(3, 2)}}
            Dim solution_Z() As Decimal = GaussianElimination(matrix_Z)
            If solution_Z Is Nothing Then Exit Do
            For i As Integer = 0 To 3
                matrix(0, i) = solution_X(i)
                matrix(1, i) = solution_Y(i)
                matrix(2, i) = solution_Z(i)
            Next
            matrix(3, 0) = 0 : matrix(3, 1) = 0 : matrix(3, 2) = 0 : matrix(3, 3) = 1
            ' All-Row/All Colum Zero entfernen:
            For r As Integer = 0 To 2
                Dim rsum As Decimal
                For c As Integer = 0 To 2
                    rsum += Math.Abs(matrix(r, c))
                Next
                If Math.Abs(rsum) < 0.00001D Then
                    For c As Integer = 0 To 2
                        Dim csum As Decimal = 0
                        For r2 As Integer = 0 To 2
                            csum += Math.Abs(matrix(r2, c))
                        Next
                        If Math.Abs(csum) < 0.00001D Then
                            matrix(r, c) = 1D
                        End If
                    Next
                End If
            Next
            For c As Integer = 0 To 2
                Dim csum As Decimal = 0
                For r2 As Integer = 0 To 2
                    csum += Math.Abs(matrix(r2, c))
                Next
                If Math.Abs(csum) < 0.00001D Then
                    matrix(c, c) = 1D
                End If
            Next


            inCoords = i_backup.Clone
            outCoords = o_backup.Clone

            'For i As Integer = 0 To 3
            '    Dim t(2) As Decimal
            '    t(0) = Math.Round(matrix(0, 0) * inCoords(i, 0) + matrix(0, 1) * inCoords(i, 1) + matrix(0, 3), 5)
            '    t(1) = Math.Round(matrix(1, 0) * inCoords(i, 0) + matrix(1, 1) * inCoords(i, 1) + matrix(1, 3), 5)
            '    t(2) = Math.Round(matrix(2, 0) * inCoords(i, 0) + matrix(2, 1) * inCoords(i, 1) + matrix(2, 3), 5)
            '    If Math.Abs(outCoords(i, 0) - t(0)) > 0.01D OrElse _
            '       Math.Abs(outCoords(i, 1) - t(1)) > 0.01D OrElse _
            '       Math.Abs(outCoords(i, 2) - t(2)) > 0.01D Then
            '        t(0) = t(0)
            '        'Exit Do
            '    End If
            'Next i

            Exit Sub
        Loop
        ' Projektions-Reparatur
        Dim idmatrix(,) As Decimal = {{1D, 0D, 0D, 0D}, {0D, 1D, 0D, 0D}, {0D, 0D, 1D, 0D}, {0D, 0D, 0D, 1D}}
        matrix = idmatrix.Clone
        inCoords = i_backup.Clone
        outCoords = o_backup.Clone        
    End Sub

    Public Function isInQuad(ByVal x As Decimal, ByVal y As Decimal) As Boolean
        Dim beamorig1() As Decimal = {x, y, 1D}
        If isTriangle Then
            Dim tri1(,) As Decimal = {{inCoords(1, 0), inCoords(1, 1)}, {inCoords(2, 0), inCoords(2, 1)}, {inCoords(3, 0), inCoords(3, 1)}}
            Dim vert00() As Decimal = {tri1(0, 0), tri1(0, 1), 0}
            Dim vert01() As Decimal = {tri1(1, 0), tri1(1, 1), 0}
            Dim vert02() As Decimal = {tri1(2, 0), tri1(2, 1), 0}
            Return (PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02))
        Else
            Dim tri1(,) As Decimal = {{inCoords(0, 0), inCoords(0, 1)}, {inCoords(1, 0), inCoords(1, 1)}, {inCoords(2, 0), inCoords(2, 1)}}
            Dim tri2(,) As Decimal = {{inCoords(2, 0), inCoords(2, 1)}, {inCoords(3, 0), inCoords(3, 1)}, {inCoords(0, 0), inCoords(0, 1)}}
            Dim vert00() As Decimal = {tri1(0, 0), tri1(0, 1), 0}
            Dim vert01() As Decimal = {tri1(1, 0), tri1(1, 1), 0}
            Dim vert02() As Decimal = {tri1(2, 0), tri1(2, 1), 0}
            Dim vert10() As Decimal = {tri2(0, 0), tri2(0, 1), 0}
            Dim vert11() As Decimal = {tri2(1, 0), tri2(1, 1), 0}
            Dim vert12() As Decimal = {tri2(2, 0), tri2(2, 1), 0}
            Return (PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02) OrElse _
                    PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert10, vert11, vert12))
        End If
    End Function

    ' Old:
    Public Function isInQuad(ByVal tri As Triangle) As Boolean
        If tri.vertexA.X = tri.vertexB.X AndAlso tri.vertexA.Y = tri.vertexB.Y Then Return True
        If tri.vertexA.X = tri.vertexC.X AndAlso tri.vertexA.Y = tri.vertexC.Y Then Return True
        If tri.vertexB.X = tri.vertexC.X AndAlso tri.vertexB.Y = tri.vertexC.Y Then Return True
        Dim beamorig1() As Decimal = {(tri.vertexA.X + tri.vertexB.X + tri.vertexC.X) / 3, (tri.vertexA.Y + tri.vertexB.Y + tri.vertexC.Y) / 3, 1}
        If isTriangle Then
            Dim tri1(,) As Decimal = {{inCoords(1, 0), inCoords(1, 1)}, {inCoords(2, 0), inCoords(2, 1)}, {inCoords(3, 0), inCoords(3, 1)}}
            Dim vert00() As Decimal = {tri1(0, 0), tri1(0, 1), 0}
            Dim vert01() As Decimal = {tri1(1, 0), tri1(1, 1), 0}
            Dim vert02() As Decimal = {tri1(2, 0), tri1(2, 1), 0}
            Return (PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02))
        Else
            Dim tri1(,) As Decimal = {{inCoords(0, 0), inCoords(0, 1)}, {inCoords(1, 0), inCoords(1, 1)}, {inCoords(2, 0), inCoords(2, 1)}}
            Dim tri2(,) As Decimal = {{inCoords(2, 0), inCoords(2, 1)}, {inCoords(3, 0), inCoords(3, 1)}, {inCoords(0, 0), inCoords(0, 1)}}
            Dim vert00() As Decimal = {tri1(0, 0), tri1(0, 1), 0}
            Dim vert01() As Decimal = {tri1(1, 0), tri1(1, 1), 0}
            Dim vert02() As Decimal = {tri1(2, 0), tri1(2, 1), 0}
            Dim vert10() As Decimal = {tri2(0, 0), tri2(0, 1), 0}
            Dim vert11() As Decimal = {tri2(1, 0), tri2(1, 1), 0}
            Dim vert12() As Decimal = {tri2(2, 0), tri2(2, 1), 0}
            Return (PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02) OrElse _
                    PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert10, vert11, vert12))
        End If
    End Function

    Public Function isInQuad(ByVal trilist As List(Of Triangle)) As Boolean
        For i = 0 To trilist.Count - 1
            If Not isInQuad(trilist(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function isInQuad(ByVal vert As Vertex)
        Dim tri1(,) As Double = {{inCoords(0, 0), inCoords(0, 1)}, {inCoords(1, 0), inCoords(1, 1)}, {inCoords(2, 0), inCoords(2, 1)}}
        Dim tri2(,) As Double = {{inCoords(2, 0), inCoords(2, 1)}, {inCoords(3, 0), inCoords(3, 1)}, {inCoords(0, 0), inCoords(0, 1)}}
        Dim beamorig1() As Decimal = {vert.X / 1000D, vert.Y / 1000D, 0}
        Dim vert00() As Decimal = {tri1(0, 0), tri1(0, 1), 0}
        Dim vert01() As Decimal = {tri1(1, 0), tri1(1, 1), 0}
        Dim vert02() As Decimal = {tri1(2, 0), tri1(2, 1), 0}
        Dim vert10() As Decimal = {tri2(0, 0), tri2(0, 1), 0}
        Dim vert11() As Decimal = {tri2(1, 0), tri2(1, 1), 0}
        Dim vert12() As Decimal = {tri2(2, 0), tri2(2, 1), 0}
        Return (PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert00, vert01, vert02) OrElse _
                PowerRay.SCHNITTPKT_DREIECK(beamorig1, beamdir, vert10, vert11, vert12))
    End Function

    Public Function isFlat() As Boolean
        If isTriangle Then
            Return True
        Else

            Dim n(3) As Vertex3D
            Dim c(3) As Double
            Dim v(3) As Vertex3D
            Dim cnc As Byte = 0
            v(0) = New Vertex3D(outCoords(1, 0) - outCoords(0, 0), _
                                outCoords(1, 1) - outCoords(0, 1), _
                                outCoords(1, 2) - outCoords(0, 2))
            v(1) = New Vertex3D(outCoords(2, 0) - outCoords(1, 0), _
                                outCoords(2, 1) - outCoords(1, 1), _
                                outCoords(2, 2) - outCoords(1, 2))
            v(2) = New Vertex3D(outCoords(3, 0) - outCoords(2, 0), _
                                outCoords(3, 1) - outCoords(2, 1), _
                                outCoords(3, 2) - outCoords(2, 2))
            v(3) = New Vertex3D(outCoords(0, 0) - outCoords(3, 0), _
                                outCoords(0, 1) - outCoords(3, 1), _
                                outCoords(0, 2) - outCoords(3, 2))  
            n(0) = v(0).cross(v(1))
            n(1) = v(1).cross(v(2))
            n(2) = v(2).cross(v(3))
            n(3) = v(3).cross(v(0))
            For i As Byte = 0 To 3
                c(i) = n(0).linearCoeff2(n(i))
                If c(i) < 0 Then cnc += 1
            Next i
            If cnc = 2 Then
                For i As Byte = 1 To 3
                    If c(i) < 0 Then
                        Return False ' HOURGLASS
                    End If
                Next i
            ElseIf cnc = 3 Then
                Return False ' CONCAVE
            End If
            If n(0).angleBetween(n(2)) > (Math.PI / 6000.0) Then
                Return False ' NON FLAT
            End If
            Return True
        End If
    End Function

    Public Function triangulate() As ProjectionQuad
        If isTriangle Then
            Throw New TypeAccessException("Cannot triangulate a triangle projection surface!")
        Else
            Dim pq As New ProjectionQuad()
            isTriangle = True
            pq.isTriangle = True
            For i As Integer = 0 To 1
                pq.inCoords(i, 0) = inCoords(1, 0)
                pq.inCoords(i, 1) = inCoords(1, 1)
                pq.outCoords(i, 0) = outCoords(1, 0)
                pq.outCoords(i, 1) = outCoords(1, 1)
                pq.outCoords(i, 2) = outCoords(1, 2)
            Next
            pq.inCoords(2, 0) = inCoords(2, 0)
            pq.inCoords(2, 1) = inCoords(2, 1)
            pq.outCoords(2, 0) = outCoords(2, 0)
            pq.outCoords(2, 1) = outCoords(2, 1)
            pq.outCoords(2, 2) = outCoords(2, 2)

            pq.inCoords(3, 0) = inCoords(0, 0)
            pq.inCoords(3, 1) = inCoords(0, 1)
            pq.outCoords(3, 0) = outCoords(0, 0)
            pq.outCoords(3, 1) = outCoords(0, 1)
            pq.outCoords(3, 2) = outCoords(0, 2)

            inCoords(1, 0) = inCoords(0, 0)
            inCoords(1, 1) = inCoords(0, 1)
            outCoords(1, 0) = outCoords(0, 0)
            outCoords(1, 1) = outCoords(0, 1)
            outCoords(1, 2) = outCoords(0, 2)
            Return pq
        End If
    End Function

    Friend Function uvProjection(ByVal x As Decimal, ByVal y As Decimal) As Decimal()
        Dim tri1(,) As Double = {{inCoords(0, 0), inCoords(0, 1)}, {inCoords(1, 0), inCoords(1, 1)}, {inCoords(2, 0), inCoords(2, 1)}}
        Dim tri2(,) As Double = {{inCoords(2, 0), inCoords(2, 1)}, {inCoords(3, 0), inCoords(3, 1)}, {inCoords(0, 0), inCoords(0, 1)}}
        Dim beamorig1() As Decimal = {x, y, 0}
        Dim vert00() As Decimal = {tri1(0, 0), tri1(0, 1), 0}
        Dim vert01() As Decimal = {tri1(1, 0), tri1(1, 1), 0}
        Dim vert02() As Decimal = {tri1(2, 0), tri1(2, 1), 0}
        Dim vert10() As Decimal = {tri2(0, 0), tri2(0, 1), 0}
        Dim vert11() As Decimal = {tri2(1, 0), tri2(1, 1), 0}
        Dim vert12() As Decimal = {tri2(2, 0), tri2(2, 1), 0}
        Dim u1 As Decimal
        Dim u2 As Decimal
        Dim v1 As Decimal
        Dim v2 As Decimal
        Dim w1 As Decimal
        Dim w2 As Decimal

        Dim uv1invalid As Boolean
        Dim uv2invalid As Boolean

        PowerRay.UV_KOORDINATEN(beamorig1, beamdir, vert00, vert01, vert02)
        u1 = PowerRay.u
        v1 = PowerRay.v
        w1 = 1D - PowerRay.u - PowerRay.v
        uv1invalid = PowerRay.t < 0D
        PowerRay.UV_KOORDINATEN(beamorig1, beamdir, vert10, vert11, vert12)
        u2 = PowerRay.u
        v2 = PowerRay.v
        w2 = 1D - PowerRay.u - PowerRay.v
        uv2invalid = PowerRay.t < 0D

        If uv1invalid AndAlso uv2invalid Then
            Return New Decimal() {x / 1000D, y / 1000D, 0D}
        ElseIf uv1invalid Then
            Return New Decimal() {
                Math.Round(outCoords(2, 0) * w2 + outCoords(3, 0) * u2 + outCoords(0, 0) * v2, 5),
                Math.Round(outCoords(2, 1) * w2 + outCoords(3, 1) * u2 + outCoords(0, 1) * v2, 5),
                Math.Round(outCoords(2, 2) * w2 + outCoords(3, 2) * u2 + outCoords(0, 2) * v2, 5)}
        ElseIf uv2invalid Then
            Return New Decimal() {
                Math.Round(outCoords(0, 0) * w1 + outCoords(1, 0) * u1 + outCoords(2, 0) * v1, 5),
                Math.Round(outCoords(0, 1) * w1 + outCoords(1, 1) * u1 + outCoords(2, 1) * v1, 5),
                Math.Round(outCoords(0, 2) * w1 + outCoords(1, 2) * u1 + outCoords(2, 2) * v1, 5)}
        Else
            Dim d1 As Decimal = (u1 - 0.5D) * (u1 - 0.5D) + (v1 - 0.5D) * (v1 - 0.5D) + (w1 - 0.5D) * (w1 - 0.5D)
            Dim d2 As Decimal = (u2 - 0.5D) * (u2 - 0.5D) + (v2 - 0.5D) * (v2 - 0.5D) + (w2 - 0.5D) * (w2 - 0.5D)
            If d1 < d2 Then
                Return New Decimal() {
                Math.Round(outCoords(0, 0) * w1 + outCoords(1, 0) * u1 + outCoords(2, 0) * v1, 5),
                Math.Round(outCoords(0, 1) * w1 + outCoords(1, 1) * u1 + outCoords(2, 1) * v1, 5),
                Math.Round(outCoords(0, 2) * w1 + outCoords(1, 2) * u1 + outCoords(2, 2) * v1, 5)}
            Else
                Return New Decimal() {
                Math.Round(outCoords(2, 0) * w2 + outCoords(3, 0) * u2 + outCoords(0, 0) * v2, 5),
                Math.Round(outCoords(2, 1) * w2 + outCoords(3, 1) * u2 + outCoords(0, 1) * v2, 5),
                Math.Round(outCoords(2, 2) * w2 + outCoords(3, 2) * u2 + outCoords(0, 2) * v2, 5)}
            End If
        End If
    End Function
End Class
