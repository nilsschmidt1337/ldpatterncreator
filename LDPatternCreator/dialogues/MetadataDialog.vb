Option Strict Off
Imports System.Windows.Forms

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

Public Class MetadataDialog
    Dim max_x, max_y, min_x, min_y As Double
    Dim factor_x, factor_y As Double
    Dim drawingTriangles As New List(Of Triangle)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not RTBComments.Focused AndAlso Not RTBHelp.Focused AndAlso Not RTBHistory.Focused AndAlso Not RTBKeywords.Focused Then
            With Me
                LPCFile.includeMetadata = .CBInclude.Checked
                LPCFile.replaceColour = .CBColourReplacer.Checked
                LPCFile.project = .CBProjector.Checked
                LPCFile.rectify = .CBRectifier.Checked
                LPCFile.unify = .CBUnificator.Checked
                LPCFile.unifyLPC = .CBUnificatorLPC.Checked
                LPCFile.slice = .CBSlicerPro.Checked
            End With

            TVTree.SelectedNode.Tag = New Metadata(TBDescription.Text, TBFilename.Text, TBRealName.Text, TBUserName.Text, CBPartType.Text, CBLicense.Text, Replace(RTBHelp.Text, vbLf, "<br>"), CBBFC.Text, TBCategory.Text, Replace(RTBKeywords.Text, vbLf, "<br>"), Replace(RTBHistory.Text, vbLf, "<br>"), Replace(RTBComments.Text, vbLf, "<br>")) With {.mAlias = TVTree.SelectedNode.Tag.mAlias, .additionalData = TVTree.SelectedNode.Tag.additionalData, .matrix = TVTree.SelectedNode.Tag.matrix.Clone, .recommendedMode = TVTree.SelectedNode.Tag.recommendedMode, .isMainMetadata = TVTree.SelectedNode.Tag.isMainMetadata}
            LPCFile.myMetadata = TVTree.Nodes(0).Tag.Clone
            Dim nText As String = LPCFile.myMetadata.mData(1)
            If nText Like "*.dat" Then
                If Mid(nText, 1, 2) = "s\" Then
                    nText = Mid(Mid(nText, 1, nText.LastIndexOf("s")), 3)
                Else
                    nText = Mid(nText, 1, nText.Length - 4)
                End If
            Else
                nText = "UNKNOWN"
            End If
            Dim scounter As Integer = 1
            For i As Integer = 0 To TVTree.Nodes(0).Nodes.Count - 1
                If TVTree.Nodes(0).Nodes(i).ForeColor <> Color.LightGray Then
                    LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias) = TVTree.Nodes(0).Nodes(i).Tag.Clone
                    Dim text As String
                    text = "s\" & nText
                    If (text & "s" & scounter & ".dat") = LPCFile.myMetadata.mData(1) Then
                        scounter += 1
                    End If
                    LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(1) = text & "s" & scounter & ".dat"
                    scounter += 1
                    Select Case LPCFile.myMetadata.mData(4)
                        Case "Part"
                            LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(4) = "Subpart"
                        Case "Subpart"
                            LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(4) = "Subpart"
                        Case "Unofficial_Part"
                            LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(4) = "Unofficial_Subpart"
                        Case "Unofficial_Subpart"
                            LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(4) = "Unofficial_Subpart"
                    End Select
                    LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(5) = LPCFile.myMetadata.mData(5)
                    LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(7) = LPCFile.myMetadata.mData(7)
                End If
            Next
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            If RTBComments.Focused Then ToolTip1.Show(I18N.trl8(I18N.lk.CtrlEnterHint), Me.Controls("RTBComments"))
            If RTBHelp.Focused Then ToolTip1.Show(I18N.trl8(I18N.lk.CtrlEnterHint), Me.Controls("RTBHelp"))
            If RTBHistory.Focused Then ToolTip1.Show(I18N.trl8(I18N.lk.CtrlEnterHint), Me.Controls("RTBHistory"))
            If RTBKeywords.Focused Then ToolTip1.Show(I18N.trl8(I18N.lk.CtrlEnterHint), Me.Controls("RTBKeywords"))
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        LPCFile.myMetadata.mData(1) = oldName
        LPCFile.myMetadata.mData(7) = oldBFC
        LPCFile.myMetadata.mData(5) = oldLicense
        LPCFile.myMetadata.mData(4) = oldType
        Dim nText As String = oldName
        If nText Like "*.dat" Then
            If Mid(nText, 1, 2) = "s\" Then
                nText = Mid(Mid(nText, 1, nText.LastIndexOf("s")), 3)
            Else
                nText = Mid(nText, 1, nText.Length - 4)
            End If
        Else
            nText = "UNKNOWN"
        End If
        Dim scounter As Integer = 1

        For i As Integer = 0 To TVTree.Nodes(0).Nodes.Count - 1
            If TVTree.Nodes(0).Nodes(i).ForeColor <> Color.LightGray Then
                If LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(1) Like "*.dat" Then
                    Dim text As String
                    text = "s\" & nText
                    If (text & "s" & scounter & ".dat") = oldName Then
                        scounter += 1
                    End If
                    LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(1) = text & "s" & scounter & ".dat"
                    scounter += 1
                End If
                LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(4) = oldType
                LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(5) = oldLicense
                LPCFile.PrimitivesMetadataHMap.Item(TVTree.Nodes(0).Nodes(i).Tag.mAlias).mData(7) = oldBFC
            End If
        Next
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BtnDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDefault.Click
        LDSettings.Editor.defaultLicense = CBLicense.Text
        LDSettings.Editor.defaultName = TBRealName.Text
        LDSettings.Editor.defaultUser = TBUserName.Text
        MainForm.saveConfig()
    End Sub

    Private Sub TVTree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TVTree.AfterSelect
        Dim state As Boolean = Not TVTree.SelectedNode.ForeColor = Color.LightGray
        Me.TBCategory.Enabled = state
        Me.TBDescription.Enabled = state
        Me.TBFilename.Enabled = state AndAlso TVTree.SelectedNode.Level = 0
        Me.TBRealName.Enabled = state
        Me.TBUserName.Enabled = state
        Me.RTBComments.Enabled = state
        Me.RTBHelp.Enabled = state
        Me.RTBHistory.Enabled = state
        Me.RTBKeywords.Enabled = state
        Me.CBBFC.Enabled = state AndAlso TVTree.SelectedNode.Level = 0
        Me.CBLicense.Enabled = state AndAlso TVTree.SelectedNode.Level = 0
        Me.CBPartType.Enabled = state AndAlso TVTree.SelectedNode.Level = 0
        Me.GBPreview.Enabled = TVTree.SelectedNode.Level = 1
        If state Then
            If TVTree.SelectedNode.Tag Is Nothing Then
                TVTree.SelectedNode.Tag = New Metadata()
            End If
            TVTree.SelectedNode.Tag.show()
        Else
            TVTree.SelectedNode.Tag = New Metadata()
            TVTree.SelectedNode.Tag.show()
        End If
        If TVTree.SelectedNode.Level = 1 AndAlso Not TVTree.SelectedNode.ForeColor = Color.LightGray Then
            max_x = Double.MinValue
            max_y = Double.MinValue
            min_x = Double.MaxValue
            min_y = Double.MaxValue
            drawingTriangles.Clear()
            For i As Integer = 0 To LPCFile.Triangles.Count - 1
                Dim tri As Triangle = LPCFile.Triangles(i)
                If tri.groupindex <> -1 AndAlso TVTree.SelectedNode.Tag.mAlias = LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).primitiveName Then
                    drawingTriangles.Add(tri)
                    If tri.vertexA.X > max_x Then max_x = tri.vertexA.X
                    If tri.vertexA.Y > max_y Then max_y = tri.vertexA.Y
                    If tri.vertexB.X > max_x Then max_x = tri.vertexB.X
                    If tri.vertexB.Y > max_y Then max_y = tri.vertexB.Y
                    If tri.vertexC.X > max_x Then max_x = tri.vertexC.X
                    If tri.vertexC.Y > max_y Then max_y = tri.vertexC.Y
                    If tri.vertexA.X < min_x Then min_x = tri.vertexA.X
                    If tri.vertexA.Y < min_y Then min_y = tri.vertexA.Y
                    If tri.vertexB.X < min_x Then min_x = tri.vertexB.X
                    If tri.vertexB.Y < min_y Then min_y = tri.vertexB.Y
                    If tri.vertexC.X < min_x Then min_x = tri.vertexC.X
                    If tri.vertexC.Y < min_y Then min_y = tri.vertexC.Y
                End If
            Next
            factor_x = Me.PPreview.Width / (max_x - min_x)
            factor_y = Me.PPreview.Height / (max_y - min_y)
        Else
            drawingTriangles.Clear()
        End If
        PPreview.Refresh()
    End Sub

    Private Sub TVTree_BeforeSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles TVTree.BeforeSelect
        If Not TVTree.SelectedNode Is Nothing AndAlso Not TVTree.SelectedNode.ForeColor = Color.LightGray Then
            TVTree.SelectedNode.Tag = New Metadata(TBDescription.Text, TBFilename.Text, TBRealName.Text, TBUserName.Text, CBPartType.Text, CBLicense.Text, Replace(RTBHelp.Text, vbLf, "<br>"), CBBFC.Text, TBCategory.Text, Replace(RTBKeywords.Text, vbLf, "<br>"), Replace(RTBHistory.Text, vbLf, "<br>"), Replace(RTBComments.Text, vbLf, "<br>")) With {.mAlias = TVTree.SelectedNode.Tag.mAlias, .additionalData = TVTree.SelectedNode.Tag.additionalData, .matrix = TVTree.SelectedNode.Tag.matrix.Clone, .recommendedMode = TVTree.SelectedNode.Tag.recommendedMode, .isMainMetadata = TVTree.SelectedNode.Tag.isMainMetadata}
        End If
    End Sub

    Dim oldBFC As String = ""
    Dim oldName As String = ""
    Dim oldLicense As String = ""
    Dim oldType As String = ""
    Dim oText As String = ""
    Private Sub MetadataDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        TVTree.Nodes(0).Tag = LPCFile.myMetadata
        oldBFC = LPCFile.myMetadata.mData(7)
        oldName = LPCFile.myMetadata.mData(1)
        oldLicense = LPCFile.myMetadata.mData(5)
        oldType = LPCFile.myMetadata.mData(4)
        With Me
            .CBInclude.Checked = LPCFile.includeMetadata
            .CBColourReplacer.Checked = LPCFile.replaceColour
            .CBProjector.Checked = LPCFile.project
            .CBRectifier.Checked = LPCFile.rectify
            .CBUnificator.Checked = LPCFile.unify
            .CBUnificatorLPC.Checked = LPCFile.unifyLPC
            .CBSlicerPro.Checked = LPCFile.slice
        End With
        Try
            Do
                TVTree.Nodes(0).Nodes(0).Remove()
            Loop
        Catch
        End Try
        If LPCFile.PrimitivesHMap.Values.Count > 0 Then
            Dim prims As New Hashtable(LPCFile.Primitives.Count)
            Dim scounter As Integer = 1
            For Each i As Integer In LPCFile.PrimitivesHMap.Values
                If Not prims.Contains(LPCFile.Primitives(i).primitiveName) Then
                    prims.Add(LPCFile.Primitives(i).primitiveName, Nothing)
                    TVTree.Nodes(0).Nodes.Add(LPCFile.Primitives(i).primitiveName)
                    If Not LPCFile.Primitives(i).primitiveName Like "subfile*" Then
                        TVTree.Nodes(0).Nodes(TVTree.Nodes(0).Nodes.Count - 1).ForeColor = Color.LightGray
                    Else
                        TVTree.Nodes(0).Nodes(TVTree.Nodes(0).Nodes.Count - 1).Tag = LPCFile.PrimitivesMetadataHMap.Item(LPCFile.Primitives(i).primitiveName)
                        TVTree.Nodes(0).Nodes(TVTree.Nodes(0).Nodes.Count - 1).Tag.mAlias = LPCFile.Primitives(i).primitiveName
                        oText = LPCFile.myMetadata.mData(1)
                        If oText Like "*.dat" Then
                            If Mid(oText, 1, 2) = "s\" Then
                                oText = Mid(Mid(oText, 1, oText.LastIndexOf("s")), 3)
                            Else
                                oText = Mid(oText, 1, oText.Length - 4)
                            End If
                            TVTree.Nodes(0).Nodes(TVTree.Nodes(0).Nodes.Count - 1).Tag.mData(1) = "s\" & oText & "s" & scounter & ".dat"
                        Else
                            oText = "UNKNOWN"
                        End If
                        TVTree.Nodes(0).Nodes(TVTree.Nodes(0).Nodes.Count - 1).Text = "s\" & oText & "s" & scounter & ".dat"
                        scounter += 1
                    End If
                End If
            Next
        Else
            TVTree.Nodes(0).Nodes.Add(I18N.trl8(I18N.lk.MNoSubFiles))
            TVTree.Nodes(0).Nodes(0).ForeColor = Color.LightGray
        End If
        TVTree.SelectedNode = TVTree.Nodes(0)
        TVTree.Nodes(0).Text = I18N.trl8(I18N.lk.MMainFile)
        TVTree.Nodes(0).Tag.show()
    End Sub

    Private Sub PPreview_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PPreview.Paint
        If drawingTriangles.Count > 0 AndAlso CBPreview.Checked Then
            ' Triangles
            Dim pf2(3) As PointF
            For i As Integer = 0 To drawingTriangles.Count - 1
                Dim tri As Triangle = drawingTriangles(i)
                pf2(0).X = PPreview.Width - (tri.vertexA.X - min_x) * factor_x
                pf2(0).Y = (tri.vertexA.Y - min_y) * factor_y
                pf2(1).X = PPreview.Width - (tri.vertexB.X - min_x) * factor_x
                pf2(1).Y = (tri.vertexB.Y - min_y) * factor_y
                pf2(2).X = PPreview.Width - (tri.vertexC.X - min_x) * factor_x
                pf2(2).Y = (tri.vertexC.Y - min_y) * factor_y
                pf2(3).X = PPreview.Width - (tri.vertexA.X - min_x) * factor_x
                pf2(3).Y = (tri.vertexA.Y - min_y) * factor_y
                e.Graphics.DrawPolygon(Pens.WhiteSmoke, pf2)
            Next
        End If
    End Sub

    Private Sub TVTree_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TVTree.MouseUp
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        TBDescription.Focus()

        If TBDescription.Text.Length > 0 Then
            Dim oldText As String = TBDescription.Text
            TBDescription.SelectionStart = TBDescription.Text.Length - 1
            TBDescription.SelectedText = ""
            TBDescription.Text = oldText
            TBDescription.SelectionStart = TBDescription.Text.Length
        End If

        Timer1.Enabled = False
    End Sub

    Private Sub TBFilename_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TBFilename.TextChanged
        If TVTree.SelectedNode.Level = 0 Then
            OK_Button.Enabled = TBFilename.Text = "" OrElse (TBFilename.Text Like "*.dat" AndAlso checkFilename(TBFilename.Text))
            TVTree.Enabled = OK_Button.Enabled
            If OK_Button.Enabled Then
                TBFilename.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
                LPCFile.myMetadata.mData(1) = TBFilename.Text
                If TBFilename.Text <> "" Then
                    Dim nText As String = TBFilename.Text
                    If Mid(nText, 1, 2) = "s\" Then
                        nText = Mid(Mid(nText, 1, nText.LastIndexOf("s")), 3)
                    Else
                        nText = Mid(nText, 1, nText.Length - 4)
                    End If
                    Dim scounter As Integer = 1
                    For i = 0 To TVTree.SelectedNode.Nodes.Count - 1
                        If TVTree.SelectedNode.Nodes(i).ForeColor <> Color.LightGray Then
                            Dim text As String
                            text = Mid(TVTree.SelectedNode.Nodes(i).Text, 3, TVTree.SelectedNode.Nodes(i).Text.LastIndexOf("s") - 2)
                            text = "s\" & Replace(text, oText, nText)
                            If (text & "s" & scounter & ".dat") = TBFilename.Text Then
                                scounter += 1
                            End If
                            TVTree.SelectedNode.Nodes(i).Text = text & "s" & scounter & ".dat"
                            scounter += 1
                        End If
                    Next
                    oText = nText
                End If
            Else
                TBFilename.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
            End If
        End If
    End Sub

    Private Function checkFilename(ByVal filename As String) As Boolean
        If filename Like ".*" Then Return False
        If Mid(filename, 1, 2) = "s\" Then
            If CBPartType.Text = "Part" OrElse CBPartType.Text = "Unofficial_Part" Then Return False
            filename = Mid(filename, 3)
            If Not (filename Like "*s#.dat" OrElse filename Like "*s##.dat" OrElse filename Like "*s###.dat") Then Return False
        Else
            If CBPartType.Text = "Subpart" OrElse CBPartType.Text = "Unofficial_Subpart" Then Return False
        End If
        Dim ca() As Char = System.IO.Path.GetInvalidFileNameChars
        For i As Integer = 0 To filename.Length - 1
            Dim c As Char = filename(i)
            For j = 0 To ca.Length - 1
                If c = ca(j) Then
                    Return False
                End If
            Next
        Next
        Return True
    End Function

    Private Sub CBLicense_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBLicense.SelectedValueChanged
        If TVTree.SelectedNode.Level = 0 Then
            LPCFile.myMetadata.mData(5) = CBLicense.Text
        End If
    End Sub

    Private Sub CBPartType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBPartType.SelectedValueChanged
        If TVTree.SelectedNode.Level = 0 Then
            LPCFile.myMetadata.mData(4) = CBPartType.Text
            If Mid(TBFilename.Text, 1, 2) = "s\" Then
                If CBPartType.Text = "Part" OrElse CBPartType.Text = "Unofficial_Part" Then
                    OK_Button.Enabled = False
                    TVTree.Enabled = False
                    TBFilename.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
                Else
                    OK_Button.Enabled = True
                    TVTree.Enabled = True
                    TBFilename.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
                End If
            Else
                If CBPartType.Text = "Subpart" OrElse CBPartType.Text = "Unofficial_Subpart" Then
                    OK_Button.Enabled = False
                    TVTree.Enabled = False
                    TBFilename.ForeColor = Color.FromKnownColor(KnownColor.HotTrack)
                Else
                    OK_Button.Enabled = True
                    TVTree.Enabled = True
                    TBFilename.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
                End If
            End If
        End If
    End Sub

    Private Sub CBBFC_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBBFC.SelectedValueChanged
        If TVTree.SelectedNode.Level = 0 Then
            LPCFile.myMetadata.mData(7) = CBBFC.Text
        End If
    End Sub

    Private Sub CBPreview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBPreview.CheckedChanged
        Me.PPreview.Refresh()
    End Sub
End Class
