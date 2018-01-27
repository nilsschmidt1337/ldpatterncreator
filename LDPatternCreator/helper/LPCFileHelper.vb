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
Imports System.IO

Public Class LPCFileHelper

    Public Shared Sub beautifyHeader()
        Dim doHistory As Boolean = True
        Dim doRest As Boolean = False
        Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "tmpr.dat", New System.Text.UTF8Encoding(False))
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "tmp.dat", False, New System.Text.UTF8Encoding(False))
                Do Until DateiIn.EndOfStream
                    Dim line As String = DateiIn.ReadLine
                    If doRest AndAlso line <> "" AndAlso Not line Like "*0*!HISTORY*" Then
                        DateiOut.WriteLine()
                        doRest = False
                    End If
                    If doHistory AndAlso line Like "*0*!HISTORY*" Then
                        DateiOut.WriteLine()
                        doHistory = False
                        doRest = True
                    End If
                    DateiOut.WriteLine(line)
                    If line Like "*0*BFC*CERTIFY*" Then DateiOut.WriteLine()
                    If line Like "*0*!LICENSE*" Then DateiOut.WriteLine()
                Loop
            End Using
        End Using
        My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "tmp.dat", EnvironmentPaths.appPath & "tmpr.dat", True)
    End Sub

    Public Shared Sub replaceColours()
        Dim lType As New Hashtable
        Dim cSub As New Dictionary(Of String, String)
        For i As Integer = 0 To LPCFile.oldColours.Count - 1
            Try
                If LPCFile.oldColours(i).ldrawIndex = -1 Then
                    If LPCFile.newColours(i).ldrawIndex = -1 Then
                        cSub.Add("0x2" & LPCFile.oldColours(i).rgbValue.R.ToString("X2") & LPCFile.oldColours(i).rgbValue.G.ToString("X2") & LPCFile.oldColours(i).rgbValue.B.ToString("X2"), "0x2" & LPCFile.newColours(i).rgbValue.R.ToString("X2") & LPCFile.newColours(i).rgbValue.G.ToString("X2") & LPCFile.newColours(i).rgbValue.B.ToString("X2"))
                    Else
                        cSub.Add("0x2" & LPCFile.oldColours(i).rgbValue.R.ToString("X2") & LPCFile.oldColours(i).rgbValue.G.ToString("X2") & LPCFile.oldColours(i).rgbValue.B.ToString("X2"), LPCFile.newColours(i).ldrawIndex)
                    End If
                Else
                    If LPCFile.newColours(i).ldrawIndex = -1 Then
                        cSub.Add(LPCFile.oldColours(i).ldrawIndex, "0x2" & LPCFile.newColours(i).rgbValue.R.ToString("X2") & LPCFile.newColours(i).rgbValue.G.ToString("X2") & LPCFile.newColours(i).rgbValue.B.ToString("X2"))
                    Else
                        cSub.Add(LPCFile.oldColours(i).ldrawIndex, LPCFile.newColours(i).ldrawIndex)
                    End If
                End If
            Catch
            End Try
        Next
        lType.Add("1", Nothing)
        lType.Add("2", Nothing)
        lType.Add("3", Nothing)
        lType.Add("4", Nothing)
        lType.Add("5", Nothing)
        Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "tmpr.dat", New System.Text.UTF8Encoding(False))
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "tmp.dat", False, New System.Text.UTF8Encoding(False))
                Do Until DateiIn.EndOfStream
                    Dim line As String = DateiIn.ReadLine
                    Dim type As String = Mid(line, 1, 1)
                    If lType.ContainsKey(type) Then
                        Dim c As String = Mid(line, 3, line.IndexOf(CType(" ", Char), 3) - 2)
                        Dim rest As String = Mid(line, line.IndexOf(CType(" ", Char), 3) + 2)
                        If cSub.ContainsKey(c) Then
                            DateiOut.WriteLine(type & " " & cSub(c) & " " & rest)
                        Else
                            DateiOut.WriteLine(line)
                        End If
                    Else
                        DateiOut.WriteLine(line)
                    End If
                Loop
            End Using
        End Using
        My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "tmp.dat", EnvironmentPaths.appPath & "tmpr.dat", True)
    End Sub

End Class
