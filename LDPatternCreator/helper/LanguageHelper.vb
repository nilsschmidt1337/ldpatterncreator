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
Imports System.Text

Module LanguageHelper
    Public Function loadLanguageFromConfig() As String
        Dim lang As String
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "Config.cfg") Then
            Try
                Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "Config.cfg", New UnicodeEncoding())
                    lang = DateiIn.ReadLine()
                End Using
                lang = Mid(lang, lang.LastIndexOf("\") + 2)
                lang = EnvironmentPaths.appPath & "lang\" & lang
            Catch
                lang = EnvironmentPaths.appPath & "lang\lang_en_GB.csv"
            End Try
        Else
            lang = EnvironmentPaths.appPath & "lang\lang_en_GB.csv"
        End If
        Return lang
    End Function
End Module
