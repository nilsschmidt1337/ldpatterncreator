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
Imports System.Threading


Module PostProcessing

    Public Sub rectifyAndUnify(ByVal filename As String)

        Dim batfile1 As String = Fix(Rnd() * 100000) & "un.bat"
        Dim batfile2 As String = Fix(Rnd() * 100000) & "re.bat"
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & batfile1) Then My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & batfile1)
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & batfile2) Then My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & batfile2)

        Dim oProcess As Process

        Dim ticks As Integer

        If Not LPCFile.unify Then
            GoTo skipUnify
        End If
        ' Unificator
        If EnvironmentPaths.ldrawPath <> "" Then
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & batfile1, False, New System.Text.UTF8Encoding(False))
                DateiOut.WriteLine("cd " & EnvironmentPaths.appPath)
                DateiOut.WriteLine("Unificator -l " & EnvironmentPaths.ldrawPath & " -s tmp.dat tmpr.dat")
            End Using

            Dim psi As New ProcessStartInfo(EnvironmentPaths.appPath & batfile1)
            psi.WorkingDirectory = EnvironmentPaths.appPath
            oProcess = System.Diagnostics.Process.Start(psi)
            oProcess.EnableRaisingEvents = True
            ticks = 0
            Do
                Thread.Sleep(10)
                ticks += 1
                If ticks = 700 Then
                    If Not oProcess.HasExited Then oProcess.Kill()
                    My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "tmp.dat", EnvironmentPaths.appPath & "tmpr.dat")
                    GoTo skipUnify
                End If
            Loop Until My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "tmpr.dat")
            Do
                Thread.Sleep(10)
            Loop Until My.Computer.FileSystem.GetFileInfo(EnvironmentPaths.appPath & "tmpr.dat").LastWriteTime.Ticks + 1000000 < Now.Ticks
            Do
                Thread.Sleep(10)
                SendKeys.SendWait("{ENTER}")
            Loop Until oProcess.HasExited

            My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "tmp.dat")
            My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "tmpr.dat", EnvironmentPaths.appPath & "tmp.dat", True)
            My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "tmpr.dat")
        End If
skipUnify:

        If Not LPCFile.rectify Then
            My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "tmp.dat", EnvironmentPaths.appPath & "tmpr.dat")
            GoTo skipRectify
        End If
        ' Rectifier
        Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & batfile2, False, New System.Text.UTF8Encoding(False))
            DateiOut.WriteLine("cd " & EnvironmentPaths.appPath)
            DateiOut.WriteLine("Rectifier tmp.dat tmpr.dat")
        End Using

        Dim psi2 As New ProcessStartInfo(EnvironmentPaths.appPath & batfile2)
        psi2.WorkingDirectory = EnvironmentPaths.appPath
        oProcess = System.Diagnostics.Process.Start(psi2)
        oProcess.EnableRaisingEvents = True
        ticks = 0
        Do
            Thread.Sleep(10)
            ticks += 1
            If ticks = 700 Then
                If Not oProcess.HasExited Then oProcess.Kill()
                My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "tmp.dat", EnvironmentPaths.appPath & "tmpr.dat")
                GoTo skipUnify
            End If
        Loop Until My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "tmpr.dat")
        Do
            Thread.Sleep(10)
        Loop Until My.Computer.FileSystem.GetFileInfo(EnvironmentPaths.appPath & "tmpr.dat").LastWriteTime.Ticks + 1000000 < Now.Ticks
        Do
            Thread.Sleep(10)
            SendKeys.SendWait("{ENTER}")
        Loop Until oProcess.HasExited
skipRectify:
        If MainState.colourReplacement AndAlso LPCFile.replaceColour Then LPCFileHelper.replaceColours()
        If LPCFile.unify Then LPCFileHelper.beautifyHeader()
        My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "tmpr.dat", filename, True)
        My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "tmp.dat")
        My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "tmpr.dat")
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & batfile1) Then My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & batfile1)
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & batfile2) Then My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & batfile2)
    End Sub

End Module
