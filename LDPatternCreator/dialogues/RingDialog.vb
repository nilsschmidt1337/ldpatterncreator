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

Imports System.Windows.Forms
Imports System.IO

Public Class RingDialog

    Public hiRes As Boolean = False

    Public n As Double
    Public m As Double
    Public segments As Integer
    Public radius As Integer
    Public pname As String = ""

    Private ResFromRad As New Hashtable
    Private RadFromRes As New Hashtable
    Private HResFromRad As New Hashtable
    Private RadFromHRes As New Hashtable

    Dim oldName As String
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If CB48.Checked Then segments = 48 Else segments = 16

        Dim strs(1) As String
        Dim strs2(1) As String
        Dim temp As String = Replace(TBName.Text, ".dat", "")
        pname = TBName.Text
        temp = Replace(temp, "48\", "")
        If temp.Contains("ring") Then
            temp = Replace(temp, "ring", "%")
            strs = temp.Split(CType("%", Char))
        End If
        If temp.Contains("rin") Then
            temp = Replace(temp, "rin", "%")
            strs = temp.Split(CType("%", Char))
        End If
        If temp.Contains("ri") Then
            temp = Replace(temp, "ri", "%")
            strs = temp.Split(CType("%", Char))
        End If
        strs2 = strs(0).Split(CType("-", Char))
        n = CType(strs2(0), Double)
        m = CType(strs2(1), Double)
        radius = CType(strs(1), Integer)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RingDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        CBRadius.Items.Clear()
        CBResolution.Items.Clear()

        If CBName.Items.Count = 0 Then


            If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "LDrawPrimitives.cfg") Then
                Using DateiOut As BinaryWriter = New BinaryWriter(File.Open(EnvironmentPaths.appPath & "LDrawPrimitives.cfg", FileMode.Create))
                    For Each b As Byte In My.Resources.Primitives1
                        DateiOut.Write(b)
                    Next
                End Using
            End If

            If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "LDrawPrimitives.cfg") Then
                Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "LDrawPrimitives.cfg", New System.Text.UTF8Encoding(False))
                    Do
                        Dim s As String = DateiIn.ReadLine
                        If s <> "" Then
                            Dim s2() As String = s.Split(";")
                            If s2.Length = 2 Then
                                If s2(0) = "R" Then
                                    CBName.Items.Add(s2(1))
                                End If
                            End If
                        End If
                    Loop Until DateiIn.EndOfStream
                End Using
            End If
        End If

        fillLists()

    End Sub

    Private Sub fillLists()
        If RadFromRes.Count = 0 Then
            For Each temp As String In CBName.Items
                Dim strs(1) As String
                Dim p48 As Boolean
                p48 = temp.Contains("48\")
                If p48 Then
                    temp = Replace(temp, "48\", "")
                End If
                If temp.Contains("ring") Then
                    temp = Replace(temp, "ring", "%")
                    strs = temp.Split(CType("%", Char))
                End If
                If temp.Contains("rin") Then
                    temp = Replace(temp, "rin", "%")
                    strs = temp.Split(CType("%", Char))
                End If
                If temp.Contains("ri") Then
                    temp = Replace(temp, "ri", "%")
                    strs = temp.Split(CType("%", Char))
                End If
                If strs(1).Length = 1 Then strs(1) = " " & strs(1)
                If strs(0).Length = 4 Then strs(0) = " " & strs(0)
                If strs(0).Length = 3 Then strs(0) = "  " & strs(0)
                If p48 Then
                    If HResFromRad.Contains(strs(1)) Then HResFromRad(strs(1)).Add(strs(0)) Else HResFromRad.Add(strs(1), New List(Of String)) : HResFromRad(strs(1)).Add(strs(0))
                    If RadFromHRes.Contains(strs(0)) Then RadFromHRes(strs(0)).Add(strs(1)) Else RadFromHRes.Add(strs(0), New List(Of String)) : RadFromHRes(strs(0)).Add(strs(1))
                Else
                    If ResFromRad.Contains(strs(1)) Then ResFromRad(strs(1)).Add(strs(0)) Else ResFromRad.Add(strs(1), New List(Of String)) : ResFromRad(strs(1)).Add(strs(0))
                    If RadFromRes.Contains(strs(0)) Then RadFromRes(strs(0)).Add(strs(1)) Else RadFromRes.Add(strs(0), New List(Of String)) : RadFromRes(strs(0)).Add(strs(1))
                End If
                CBRadius.Text = strs(1)
                CBResolution.Text = strs(0)
            Next

            CBRadius.Text = " 1"
        End If

        CBRadius.Items.Clear()
        CBResolution.Items.Clear()

        CB48.Checked = hiRes

        If CBRadius.Items.Count = 0 Then
            If CB48.Checked Then
                For Each key As String In HResFromRad.Keys
                    CBRadius.Items.Add(key)
                    For Each res As String In HResFromRad(key)
                        If Not CBResolution.Items.Contains(res) Then CBResolution.Items.Add(res)
                    Next
                Next
            Else
                For Each key As String In ResFromRad.Keys
                    CBRadius.Items.Add(key)
                    For Each res As String In ResFromRad(key)
                        If Not CBResolution.Items.Contains(res) Then CBResolution.Items.Add(res)
                    Next
                Next
            End If
        End If
    End Sub


    Private Sub getDataFromName()
        If CBName.Items.Contains(CBName.Text) Then
            Dim strs(1) As String
            Dim temp As String = CBName.Text
            CB48.Checked = temp.Contains("48\")
            If CB48.Checked Then
                temp = Replace(temp, "48\", "")
            End If
            If temp.Contains("ring") Then
                temp = Replace(temp, "ring", "%")
                strs = temp.Split(CType("%", Char))
            End If
            If temp.Contains("rin") Then
                temp = Replace(temp, "rin", "%")
                strs = temp.Split(CType("%", Char))
            End If
            If temp.Contains("ri") Then
                temp = Replace(temp, "ri", "%")
                strs = temp.Split(CType("%", Char))
            End If
            If strs(1).Length = 1 Then strs(1) = " " & strs(1)
            If strs(0).Length = 4 Then strs(0) = " " & strs(0)
            If strs(0).Length = 3 Then strs(0) = "  " & strs(0)
            CBRadius.Text = strs(1)
            CBResolution.Text = strs(0)
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Black
            CBResolution.BackColor = Color.White
            CBResolution.ForeColor = Color.Black
            CBRadius.BackColor = Color.White
            CBRadius.ForeColor = Color.Black
        Else
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Red
        End If
    End Sub


    Private Sub CBName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBName.SelectedIndexChanged
        getDataFromName()
    End Sub

    Private Sub CB48_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CB48.CheckedChanged
        CBRadius.Items.Clear()
        CBResolution.Items.Clear()

        Dim sep As String = "ring"
        sep = Trim(CBResolution.Text) & Mid(sep, 1, 12 - (Trim(CBResolution.Text) & sep & Trim(CBRadius.Text)).Length) & Trim(CBRadius.Text)

        If CB48.Checked Then
            CBName.Text = "48\" & sep
            For Each key As String In HResFromRad.Keys
                CBRadius.Items.Add(key)
                For Each res As String In HResFromRad(key)
                    If Not CBResolution.Items.Contains(res) Then CBResolution.Items.Add(res)
                Next
            Next
        Else
            CBName.Text = sep
            For Each key As String In ResFromRad.Keys
                CBRadius.Items.Add(key)
                For Each res As String In ResFromRad(key)
                    If Not CBResolution.Items.Contains(res) Then CBResolution.Items.Add(res)
                Next
            Next
        End If

        If CBName.Items.Contains(CBName.Text) Then
            TBName.Text = CBName.Text & ".dat"
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Black
        Else
            TBName.Text = ""
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Red
        End If
    End Sub

    Private Sub CBRadius_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBRadius.SelectedIndexChanged

        CBResolution.Items.Clear()
        Dim sep As String = "ring"
        sep = Trim(CBResolution.Text) & Mid(sep, 1, 12 - (Trim(CBResolution.Text) & sep & Trim(CBRadius.Text)).Length) & Trim(CBRadius.Text)


        If CB48.Checked Then
            CBName.Text = "48\" & sep
            For Each res As String In HResFromRad(CBRadius.Text)
                If Not CBResolution.Items.Contains(res) Then CBResolution.Items.Add(res)
            Next
        Else
            CBName.Text = sep
            For Each res As String In ResFromRad(CBRadius.Text)
                If Not CBResolution.Items.Contains(res) Then CBResolution.Items.Add(res)
            Next
        End If
        If CBResolution.Items.Contains(CBResolution.Text) Then
            CBResolution.BackColor = Color.White
            CBResolution.ForeColor = Color.Black
        Else
            CBResolution.BackColor = Color.White
            CBResolution.ForeColor = Color.Red
        End If
        If CBName.Items.Contains(CBName.Text) Then
            TBName.Text = CBName.Text & ".dat"
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Black
        Else
            TBName.Text = ""
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Red
        End If

    End Sub

    Private Sub CBResolution_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBResolution.SelectedIndexChanged
        CBRadius.Items.Clear()
        Dim sep As String = "ring"
        sep = Trim(CBResolution.Text) & Mid(sep, 1, 12 - (Trim(CBResolution.Text) & sep & Trim(CBRadius.Text)).Length) & Trim(CBRadius.Text)


        If CB48.Checked Then
            CBName.Text = "48\" & sep
            For Each res As String In RadFromHRes(CBResolution.Text)
                If Not CBRadius.Items.Contains(res) Then CBRadius.Items.Add(res)
            Next
        Else
            CBName.Text = sep
            For Each res As String In RadFromRes(CBResolution.Text)
                If Not CBRadius.Items.Contains(res) Then CBRadius.Items.Add(res)
            Next
        End If
        If CBRadius.Items.Contains(CBRadius.Text) Then
            CBRadius.BackColor = Color.White
            CBRadius.ForeColor = Color.Black
        Else
            CBRadius.BackColor = Color.White
            CBRadius.ForeColor = Color.Red
        End If
        If CBName.Items.Contains(CBName.Text) Then
            TBName.Text = CBName.Text & ".dat"
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Black
        Else
            TBName.Text = ""
            CBName.BackColor = Color.White
            CBName.ForeColor = Color.Red
        End If
    End Sub

    Private Sub TBName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TBName.TextChanged
        OK_Button.Enabled = Not TBName.Text = ""
    End Sub
End Class
