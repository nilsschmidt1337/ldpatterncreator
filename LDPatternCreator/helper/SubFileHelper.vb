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

Imports System.IO

Module SubFileHelper
    Public Sub inlineSubfile(ByVal projectMode As Byte, ByVal colour As String,
                            ByVal M11 As Double, ByVal M12 As Double, ByVal M13 As Double, ByVal M14 As Double,
                            ByVal M21 As Double, ByVal M22 As Double, ByVal M23 As Double, ByVal M24 As Double,
                            ByVal M31 As Double, ByVal M32 As Double, ByVal M33 As Double, ByVal M34 As Double, _
 _
                            ByVal N11 As Double, ByVal N12 As Double, ByVal N13 As Double, ByVal N14 As Double,
                            ByVal N21 As Double, ByVal N22 As Double, ByVal N23 As Double, ByVal N24 As Double,
                            ByVal N31 As Double, ByVal N32 As Double, ByVal N33 As Double, ByVal N34 As Double,
                            ByVal name As String, ByVal subFileList As List(Of String))
        If Not subFileList.Contains(name) Then

            Dim matrix(3, 3) As Double

            Dim matrix2(3, 3) As Double
            Dim matrix3(3, 3) As Double

            matrix2(0, 0) = M11
            matrix2(0, 1) = M12
            matrix2(0, 2) = M13
            matrix2(0, 3) = M14
            matrix2(1, 0) = M21
            matrix2(1, 1) = M22
            matrix2(1, 2) = M23
            matrix2(1, 3) = M24
            matrix2(2, 0) = M31
            matrix(2, 1) = M32
            matrix2(2, 2) = M33
            matrix2(2, 3) = M34
            matrix2(3, 0) = 0
            matrix2(3, 1) = 0
            matrix2(3, 2) = 0
            matrix2(3, 3) = 1
            matrix(0, 0) = N11
            matrix(0, 1) = N12
            matrix(0, 2) = N13
            matrix(0, 3) = N14
            matrix(1, 0) = N21
            matrix(1, 1) = N22
            matrix(1, 2) = N23
            matrix(1, 3) = N24
            matrix(2, 0) = N31
            matrix(2, 1) = N32
            matrix(2, 2) = N33
            matrix(2, 3) = N34
            matrix(3, 0) = 0
            matrix(3, 1) = 0
            matrix(3, 2) = 0
            matrix(3, 3) = 1

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

            Dim startVertex As Integer = LPCFile.Vertices.Count
            Dim endVertex As Integer

            Dim filepath As String
            If My.Computer.FileSystem.FileExists(EnvironmentPaths.folderPath & "\" & name) Then
                filepath = EnvironmentPaths.folderPath & "\" & name
            ElseIf EnvironmentPaths.folderPath Like "*\s" AndAlso My.Computer.FileSystem.FileExists(Mid(EnvironmentPaths.folderPath, 1, EnvironmentPaths.folderPath.Length - 2) & "\" & name) Then
                filepath = Mid(EnvironmentPaths.folderPath, 1, EnvironmentPaths.folderPath.Length - 2) & "\" & name
            ElseIf My.Computer.FileSystem.FileExists(EnvironmentPaths.ldrawPath & "\" & name) Then
                filepath = EnvironmentPaths.ldrawPath & "\" & name
            ElseIf My.Computer.FileSystem.FileExists(EnvironmentPaths.ldrawPath & "\P\" & name) Then
                filepath = EnvironmentPaths.ldrawPath & "\P\" & name
            Else
                Exit Sub
            End If

            Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(filepath, New System.Text.UTF8Encoding(False))
                Dim textline As String
                LPCFile.Vertices.Add(New Vertex(0, 0, False))
                Do
                    textline = DateiIn.ReadLine().Trim()
                    Dim origline As String = textline
                    If textline <> Nothing Then

                        Dim ttextline As String = textline
                        textline = Replace(Replace(textline, ".", MathHelper.comma).ToLowerInvariant, "  ", " ")
                        Dim oldlenght As Integer
                        Do
                            oldlenght = textline.Length
                            textline = Replace(textline, "  ", " ")
                        Loop Until oldlenght = textline.Length
                        Dim words() As String = textline.Split(CType(" ", Char))

                        If words(0) Like "*1*" AndAlso words.Length > 14 Then
                            If words(1) = "16" Then words(1) = colour
                            inlineSubfile(projectMode, words(1),
                                            -CType(words(5), Double), -CType(words(7), Double), 0, -CType(words(2), Double),
                                            -CType(words(11), Double), -CType(words(13), Double), 0, -CType(words(4), Double),
                                            0, 0, 1, 0,
                                            matrix3(0, 0), matrix3(0, 1), matrix3(0, 2), matrix3(0, 3),
                                            matrix3(1, 0), matrix3(1, 1), matrix3(1, 2), matrix3(1, 3),
                                            matrix3(2, 0), matrix3(2, 1), matrix3(2, 2), matrix3(2, 3),
                                            getDATFilename(origline), subFileList)

                        ElseIf words(0) Like "*3*" AndAlso words.Length = 11 Then
                            If words(1) = "16" Then words(1) = colour

                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), CType(-words(4), Double), False))
                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), CType(-words(7), Double), False))
                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), CType(-words(10), Double), False))

                            LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(LPCFile.Vertices.Count - 1), LPCFile.Vertices(LPCFile.Vertices.Count - 2), LPCFile.Vertices(LPCFile.Vertices.Count - 3)))
                            If words(1) Like "0x2*" Then
                                ListHelper.LLast(LPCFile.Triangles).myColour = Color.FromArgb(255,
                                                                           Convert.ToInt32("0x" & Mid(words(1), 4, 2), 16),
                                                                           Convert.ToInt32("0x" & Mid(words(1), 6, 2), 16),
                                                                           Convert.ToInt32("0x" & Mid(words(1), 8, 2), 16))
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = -1
                            ElseIf words(1) Like "0x*" Then
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = 16
                            Else
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = CType(words(1), Short)
                            End If
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))

                        ElseIf words(0) Like "*4*" AndAlso words.Length = 14 Then

                            If words(1) = "16" Then words(1) = colour

                            '4 2, 7 5, 10 8, 13 11
                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), -CType(words(4), Double), False))
                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), -CType(words(7), Double), False))
                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), -CType(words(10), Double), False))
                            LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(LPCFile.Vertices.Count - 1), LPCFile.Vertices(LPCFile.Vertices.Count - 2), LPCFile.Vertices(LPCFile.Vertices.Count - 3)))
                            If words(1) Like "0x2*" Then
                                ListHelper.LLast(LPCFile.Triangles).myColour = Color.FromArgb(255,
                                                                           Convert.ToInt32("0x" & Mid(words(1), 4, 2), 16),
                                                                           Convert.ToInt32("0x" & Mid(words(1), 6, 2), 16),
                                                                           Convert.ToInt32("0x" & Mid(words(1), 8, 2), 16))
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = -1
                            ElseIf words(1) Like "0x*" Then
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = 16
                            Else
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = CType(words(1), Short)
                            End If
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices.Add(New Vertex(-CType(words(11), Double), -CType(words(13), Double), False))
                            LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(LPCFile.Vertices.Count - 1), LPCFile.Vertices(LPCFile.Vertices.Count - 2), LPCFile.Vertices(LPCFile.Vertices.Count - 4)))
                            If words(1) Like "0x2*" Then
                                ListHelper.LLast(LPCFile.Triangles).myColour = Color.FromArgb(255,
                                                                           Convert.ToInt32("0x" & Mid(words(1), 4, 2), 16),
                                                                           Convert.ToInt32("0x" & Mid(words(1), 6, 2), 16),
                                                                           Convert.ToInt32("0x" & Mid(words(1), 8, 2), 16))
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = -1
                            ElseIf words(1) Like "0x*" Then
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = 16
                            Else
                                ListHelper.LLast(LPCFile.Triangles).myColourNumber = CType(words(1), Short)
                            End If
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                        End If
                    End If
                Loop Until DateiIn.EndOfStream
            End Using
            endVertex = LPCFile.Vertices.Count - 1
            Dim tx, ty As Double
            For i As Integer = startVertex To endVertex
                tx = matrix3(0, 0) * LPCFile.Vertices(i).X + matrix3(0, 1) * LPCFile.Vertices(i).Y + matrix3(0, 3)
                ty = matrix3(1, 0) * LPCFile.Vertices(i).X + matrix3(1, 1) * LPCFile.Vertices(i).Y + matrix3(1, 3)
                LPCFile.Vertices(i).X = tx
                LPCFile.Vertices(i).Y = ty
            Next
        End If
    End Sub
End Module
