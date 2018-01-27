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

    Public Vertices As New List(Of Vertex)

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
        For t As Double = 0.0 To 1.0 Step st
            Vertices.Add(New Vertex( _
            tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
            , _
            tStartAt.Y + k(1) * t + l(1) * t ^ 2 + m(1) * t ^ 3 _
            , False, False))
            oldT = t
        Next
        Dim vc As Integer = Vertices.Count
        If vc <> (segmentCount + 1) Then
            vc = vc
        End If
        If segmentCount = vc Then
            Dim t As Double = oldT + (1.0 - oldT) / 2.0
            Vertices.Add(New Vertex( _
            tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
            , _
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
        dist /= (vc + 1.0)

        Vertices.Clear()
        vc = 0
        For t As Double = 0.0 To 1.0 Step st
            vc += 1
            If vc = 1 Then
                Vertices.Add(New Vertex( _
                tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
                , _
                tStartAt.Y + k(1) * t + l(1) * t ^ 2 + m(1) * t ^ 3 _
                , False, False))
            Else
                Dim tv As Vertex = Nothing
                For iteration As Integer = 1 To 100
                    tv = New Vertex( _
                    tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
                    , _
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
            oldT = t
        Next

        vc = Vertices.Count
        If segmentCount = vc Then
            Dim t As Double = oldT + (1.0 - oldT) / 2.0
            Dim tv As Vertex = Nothing
            For iteration As Integer = 1 To 100
                tv = New Vertex( _
                tStartAt.X + k(0) * t + l(0) * t ^ 2 + m(0) * t ^ 3 _
                , _
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
        ElseIf (segmentCount + 2) = vc Then
            Vertices.RemoveAt(vc - 1)
        End If
    End Sub

    Public Sub persistGeometry()
        For Each v As Vertex In Vertices
            LPCFile.Vertices.Add(New Vertex(v.X, v.Y, False))
        Next
    End Sub

End Class
