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

Module DatFileNameHelper
    Public Function getDATFilename(ByVal line As String) As String
        Dim lenght As Integer = line.Length
        Dim spacesCount As Integer = 0
        Dim lastWasNotSpace As Boolean = True
        For i As Integer = 0 To lenght - 1
            If spacesCount < 14 Then
                If line.Chars(i).Equals(CChar(" ")) AndAlso lastWasNotSpace Then
                    spacesCount += 1
                    lastWasNotSpace = False
                Else
                    lastWasNotSpace = True
                End If
            Else
                line = Mid(line, i + 1)
                line = Mid(line, 1, line.Length - 4) & ".dat"
                Exit For
            End If
        Next
        Return line
    End Function
End Module
