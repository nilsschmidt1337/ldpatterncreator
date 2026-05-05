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

Public Class Spline

    Public Shared vh1 As Vertex = Nothing
    Public Shared vh2 As Vertex = Nothing

    Public Vertices As New List(Of Vertex)

    Public width As Double = 0
    Public segmentCount As Integer = 16
    Public startAt As Vertex
    Public stopAt As Vertex

    Public startDirection As Vertex
    Public stopDirection As Vertex

    Public k(1) As Double
    Public l(1) As Double
    Public m(1) As Double

    Public Sub calculateSimulationGeometry(ByVal tx As Double, ByVal ty As Double)
        Vertices.Clear()
        Dim tStartAt As Vertex = New Vertex(startAt.X, startAt.Y, False, False)
        Dim tStartDirection As Vertex = New Vertex(startDirection.X, startDirection.Y, False, False)
        Dim tStopAt As Vertex
        Dim tStopDirection As Vertex
        If stopAt Is Nothing Then
            tStopAt = New Vertex(tx, ty, False, False)
            tStopDirection = tStopAt - tStartAt
        Else
            tStopAt = New Vertex(stopAt.X, stopAt.Y, False, False)
            If stopDirection Is Nothing Then
                tStopDirection = New Vertex(tx, ty, False, False) - tStopAt
                tStopDirection *= 10.0
            Else
                tStopDirection = New Vertex(stopDirection.X, stopDirection.Y, False, False)
                tStopDirection *= 10.0
            End If
        End If


        k(0) = tStartDirection.X
        k(1) = tStartDirection.Y

        m(0) = -2.0 * (tStopAt.X - tStopDirection.X / 4.0 - 3.0 * tStartDirection.X / 4.0 - tStartAt.X)
        m(1) = -2.0 * (tStopAt.Y - tStopDirection.Y / 4.0 - 3.0 * tStartDirection.Y / 4.0 - tStartAt.Y)

        l(0) = (tStopDirection.X - tStartDirection.X - 6.0 * m(0)) / 4.0
        l(1) = (tStopDirection.Y - tStartDirection.Y - 6.0 * m(1)) / 4.0


        If tStartAt.X = tStopAt.X AndAlso tStartAt.Y = tStopAt.Y Then
            Exit Sub
        End If
        If tStartDirection.X = 0 AndAlso tStartDirection.Y = 0 Then
            Exit Sub
        End If
        If tStopDirection.X = 0 AndAlso tStopDirection.Y = 0 Then
            Exit Sub
        End If

        Dim st As Double = 1.0 / segmentCount
        Dim oldT As Double
        Dim t As Double = 0
        For i As Integer = 0 To segmentCount
            Vertices.Add(New Vertex(
            tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
            ,
            tStartAt.Y + k(1) * t + l(1) * t ^ 2 + m(1) * t ^ 3 _
            , False, False))
            t += st
            oldT = t
        Next
        Dim vc As Integer = Vertices.Count
        If segmentCount = vc Then
            t = oldT + (1.0 - oldT) / 2.0
            Vertices.Add(New Vertex(
            tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
            ,
            tStartAt.Y + k(1) * t + l(1) * t ^ 2 + m(1) * t ^ 3 _
            , False, False))
            vc += 1
        ElseIf (segmentCount + 2) = vc Then
            Vertices.RemoveAt(vc - 1)
            vc -= 1
        End If
        vc -= 1
        Dim dist As Double
        For i As Integer = 1 To vc
            dist += Vertices(i).dist(Vertices(i - 1))
        Next
        dist /= vc

        Vertices.Clear()
        vc = 0
        t = 0
        For i As Integer = 0 To segmentCount
            vc += 1
            If vc = 1 Then
                Vertices.Add(New Vertex(
                tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
                ,
                tStartAt.Y + k(1) * t + l(1) * t ^ 2 + m(1) * t ^ 3 _
                , False, False))
            ElseIf vc >= segmentCount + 1 Then
            Else
                Dim tv As Vertex = Nothing
                For iteration As Integer = 1 To 100
                    tv = New Vertex(
                    tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
                    ,
                    tStartAt.Y + k(1) * t + l(1) * t ^ 2 + m(1) * t ^ 3 _
                    , False, False)
                    Dim td As Double = ListHelper.LLast(Vertices).dist(tv)
                    If td > dist Then
                        t -= 0.001
                    Else
                        t += 0.001
                    End If
                Next iteration
                Vertices.Add(tv)
                If segmentCount > vc Then
                    st = (1.0 - t) / (segmentCount - vc)
                End If
            End If
            t += st
            oldT = t
        Next

        vc = Vertices.Count
        If segmentCount = vc Then
            t = oldT + (1.0 - oldT) / 2.0
            Dim tv As Vertex = Nothing
            For iteration As Integer = 1 To 100
                tv = New Vertex(
                tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
                ,
                tStartAt.Y + k(1) * t + l(1) * t ^ 2 + m(1) * t ^ 3 _
                , False, False)
                Dim td As Double = ListHelper.LLast(Vertices).dist(tv)
                If td > dist Then
                    t -= 0.001
                Else
                    t += 0.001
                End If
            Next iteration
            'Vertices.Add(tv)
        ElseIf (segmentCount + 2) = vc Then
            'Vertices.RemoveAt(vc - 1)
        End If

        If width > 0 Then
            Dim bandVertices As New List(Of Vertex)

            If vh1 IsNot Nothing AndAlso vh2 IsNot Nothing Then
                bandVertices.Add(vh2)
                bandVertices.Add(vh1)
            End If

            Dim zero As New Vertex(0, 0, False, False)

            For i As Integer = 1 To Vertices.Count - 1
                Dim b As Vertex = Vertices(i - 1)
                Dim v As Vertex = Vertices(i)

                ' Swap and negate
                Dim d As Vertex = v - b

                Dim length As Double = d.dist(zero)
                If length > 0.00001 Then
                    d = New Vertex(d.X / length, d.Y / length, False, False)

                    Dim n As New Vertex(-d.Y, d.X, False, False)

                    n *= width / 2.0

                    If i = 1 Then
                        bandVertices.Add(Vertices(0) + n)
                        bandVertices.Add(Vertices(0) - n)
                    End If

                    bandVertices.Add(v + n)
                    bandVertices.Add(v - n)
                End If

            Next

            Vertices.Clear()
            Vertices.AddRange(bandVertices)
        End If
    End Sub

    Public Sub persistGeometry()
        Dim triVerts As New List(Of Vertex)
        For Each v As Vertex In Vertices
            Dim nv As New Vertex(v.X, v.Y, False)
            LPCFile.Vertices.Add(nv)
            triVerts.Add(nv)
        Next

        If width > 0 AndAlso triVerts.Count > 1 Then
            vh1 = triVerts(Vertices.Count - 1)
            vh2 = triVerts(Vertices.Count - 2)

            Dim a As Vertex = Nothing, b As Vertex = Nothing, c As Vertex = Nothing

            For offset As Integer = 0 To -2 Step -1
                Dim count As Integer = offset
                For Each v As Vertex In triVerts
                    count += 1
                    If count = 1 Then a = v
                    If count = 2 Then b = v
                    If count = 3 Then
                        c = v

                        Dim t As Triangle = New Triangle(a, b, c) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber}
                        LPCFile.Triangles.Add(t)

                        a.linkedTriangles.Add(t)
                        b.linkedTriangles.Add(t)
                        c.linkedTriangles.Add(t)

                        count = 0
                    End If
                Next
            Next
        End If
    End Sub

End Class
