Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class ReplacerDialog

    Dim backupNewColour As New List(Of Colour)
    Dim backupOldColour As New List(Of Colour)
    Dim c_ColourPanels(1, 7) As Panel
    Dim c_ColourTypes(1, 7) As ComboBox
    Dim c_CoulourButtons(1, 7) As Button
    Dim c_ColourLdraw(1, 7) As ComboBox
    Dim c_Delete(7) As Button
    Dim doUpdate As Boolean



    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        LPCFile.newColours.Clear()
        LPCFile.oldColours.Clear()
        LPCFile.newColours.AddRange(backupNewColour)
        LPCFile.oldColours.AddRange(backupOldColour)
        MainState.colourReplacement = backupNewColour.Count > 0
        LPCFile.colourReplacementMapBrush.Clear()
        For i As Integer = 0 To LPCFile.newColours.Count - 1
            If Not LPCFile.colourReplacementMapBrush.ContainsKey(LPCFile.oldColours(i).rgbValue.ToArgb) Then
                If LPCFile.newColours(i).ldrawIndex = 16 Then
                    LPCFile.colourReplacementMapBrush.Add(LPCFile.oldColours(i).rgbValue.ToArgb, CType(New HatchBrush(HatchStyle.Percent05, LDSettings.Colours.linePen.Color, Color.Transparent), Brush))
                Else
                    LPCFile.colourReplacementMapBrush.Add(LPCFile.oldColours(i).rgbValue.ToArgb, CType(New SolidBrush(LPCFile.newColours(i).rgbValue), Brush))
                End If
            End If
        Next
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BtnInsertRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnInsertRule.Click
        Dim cOld As New Colour
        Dim cNew As New Colour
        cOld.ldrawIndex = 0
        cOld.rgbValue = LDConfig.colourHMap(cOld.ldrawIndex)
        cNew.ldrawIndex = 0
        cNew.rgbValue = LDConfig.colourHMap(cNew.ldrawIndex)
        backupOldColour.Add(cOld)
        backupNewColour.Add(cNew)
        updateView()
    End Sub

    Private Sub VScrollBar1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles VScrollBar1.Scroll
        updateView()
    End Sub

    ' done
#Region "Type Old"
    Private Sub o1t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o1t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o1t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 0).SelectedIndex = 0
                    o1l.Visible = True
                    o1h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o1l.Visible = False
                    o1h.Visible = True
                End If
                o1c.BackColor = backupOldColour(VScrollBar1.Value).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o2t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o2t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o2t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 1).SelectedIndex = 0
                    o2l.Visible = True
                    o2h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o2l.Visible = False
                    o2h.Visible = True
                End If
                o2c.BackColor = backupOldColour(VScrollBar1.Value + 1).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o3t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o3t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o3t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 2).SelectedIndex = 0
                    o3l.Visible = True
                    o3h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o3l.Visible = False
                    o3h.Visible = True
                End If
                o3c.BackColor = backupOldColour(VScrollBar1.Value + 2).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o4t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o4t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o4t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 3).SelectedIndex = 0
                    o4l.Visible = True
                    o4h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o4l.Visible = False
                    o4h.Visible = True
                End If
                o4c.BackColor = backupOldColour(VScrollBar1.Value + 3).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o5t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o5t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o5t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 4).SelectedIndex = 0
                    o5l.Visible = True
                    o5h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o5l.Visible = False
                    o5h.Visible = True
                End If
                o5c.BackColor = backupOldColour(VScrollBar1.Value + 4).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o6t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o6t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o6t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 5).SelectedIndex = 0
                    o6l.Visible = True
                    o6h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o6l.Visible = False
                    o6h.Visible = True
                End If
                o6c.BackColor = backupOldColour(VScrollBar1.Value + 5).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o7t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o7t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o7t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 6).SelectedIndex = 0
                    o7l.Visible = True
                    o7h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o7l.Visible = False
                    o7h.Visible = True
                End If
                o7c.BackColor = backupOldColour(VScrollBar1.Value + 6).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o8t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o8t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If o8t.SelectedIndex = 0 Then
                    backupOldColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(0, 7).SelectedIndex = 0
                    o8l.Visible = True
                    o8h.Visible = False
                Else
                    backupOldColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    o8l.Visible = False
                    o8h.Visible = True
                End If
                o8c.BackColor = backupOldColour(VScrollBar1.Value + 7).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region
    ' done
#Region "Type New"
    Private Sub n1t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n1t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n1t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 0).SelectedIndex = 0
                    n1l.Visible = True
                    n1h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n1l.Visible = False
                    n1h.Visible = True
                End If
                n1c.BackColor = backupNewColour(VScrollBar1.Value).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n2t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n2t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n2t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 1).SelectedIndex = 0
                    n2l.Visible = True
                    n2h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n2l.Visible = False
                    n2h.Visible = True
                End If
                n2c.BackColor = backupNewColour(VScrollBar1.Value + 1).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n3t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n3t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n3t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 2).SelectedIndex = 0
                    n3l.Visible = True
                    n3h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n3l.Visible = False
                    n3h.Visible = True
                End If
                n3c.BackColor = backupNewColour(VScrollBar1.Value + 2).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n4t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n4t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n4t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 3).SelectedIndex = 0
                    n4l.Visible = True
                    n4h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n4l.Visible = False
                    n4h.Visible = True
                End If
                n4c.BackColor = backupNewColour(VScrollBar1.Value + 3).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n5t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n5t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n5t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 4).SelectedIndex = 0
                    n5l.Visible = True
                    n5h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n5l.Visible = False
                    n5h.Visible = True
                End If
                n5c.BackColor = backupNewColour(VScrollBar1.Value + 4).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n6t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n6t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n6t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 5).SelectedIndex = 0
                    n6l.Visible = True
                    n6h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n6l.Visible = False
                    n6h.Visible = True
                End If
                n6c.BackColor = backupNewColour(VScrollBar1.Value + 5).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n7t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n7t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n7t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 6).SelectedIndex = 0
                    n7l.Visible = True
                    n7h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n7l.Visible = False
                    n7h.Visible = True
                End If
                n7c.BackColor = backupNewColour(VScrollBar1.Value + 6).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n8t_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n8t.SelectedIndexChanged
        If Not doUpdate Then
            Try
                If n8t.SelectedIndex = 0 Then
                    backupNewColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = 0, .rgbValue = LDConfig.colourHMap(0)}
                    c_ColourLdraw(1, 7).SelectedIndex = 0
                    n8l.Visible = True
                    n8h.Visible = False
                Else
                    backupNewColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = -1, .rgbValue = LDConfig.colourHMap(0)}
                    n8l.Visible = False
                    n8h.Visible = True
                End If
                n8c.BackColor = backupNewColour(VScrollBar1.Value + 7).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region
    ' done
#Region "Colour Dialog Old"
    Private Sub o1h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o1h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o1c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub


    Private Sub o2h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o2h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value + 1).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o2c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub o3h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o3h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value + 2).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o3c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub o4h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o4h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value + 3).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o4c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub o5h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o5h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value + 4).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o5c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub o6h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o6h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value + 5).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o6c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub o7h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o7h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value + 6).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o7c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub o8h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles o8h.Click
        Me.ColorDialog1.Color = backupOldColour(VScrollBar1.Value + 7).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupOldColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            o8c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub
#End Region
    ' done
#Region "Colour Dialog New"
    Private Sub n1h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n1h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n1c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub n2h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n2h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value + 1).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n2c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub n3h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n3h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value + 2).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n3c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub n4h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n4h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value + 3).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n4c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub n5h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n5h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value + 4).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n5c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub n6h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n6h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value + 5).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n6c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub n7h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n7h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value + 6).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n7c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub n8h_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles n8h.Click
        Me.ColorDialog1.Color = backupNewColour(VScrollBar1.Value + 7).rgbValue
        If Me.ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            backupNewColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = -1, .rgbValue = Me.ColorDialog1.Color}
            n8c.BackColor = Me.ColorDialog1.Color
        End If
    End Sub
#End Region
    'done
#Region "Delete Replacement"
    Private Sub d1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d1.Click
        backupOldColour.RemoveAt(VScrollBar1.Value)
        backupNewColour.RemoveAt(VScrollBar1.Value)
        updateView()
    End Sub

    Private Sub d2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d2.Click
        backupOldColour.RemoveAt(VScrollBar1.Value + 1)
        backupNewColour.RemoveAt(VScrollBar1.Value + 1)
        updateView()
    End Sub

    Private Sub d3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d3.Click
        backupOldColour.RemoveAt(VScrollBar1.Value + 2)
        backupNewColour.RemoveAt(VScrollBar1.Value + 2)
        updateView()
    End Sub

    Private Sub d4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d4.Click
        backupOldColour.RemoveAt(VScrollBar1.Value + 3)
        backupNewColour.RemoveAt(VScrollBar1.Value + 3)
        updateView()
    End Sub

    Private Sub d5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d5.Click
        backupOldColour.RemoveAt(VScrollBar1.Value + 4)
        backupNewColour.RemoveAt(VScrollBar1.Value + 4)
        updateView()
    End Sub

    Private Sub d6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d6.Click
        backupOldColour.RemoveAt(VScrollBar1.Value + 5)
        backupNewColour.RemoveAt(VScrollBar1.Value + 5)
        updateView()
    End Sub

    Private Sub d7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d7.Click
        backupOldColour.RemoveAt(VScrollBar1.Value + 6)
        backupNewColour.RemoveAt(VScrollBar1.Value + 6)
        updateView()
    End Sub

    Private Sub d8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles d8.Click
        backupOldColour.RemoveAt(VScrollBar1.Value + 7)
        backupNewColour.RemoveAt(VScrollBar1.Value + 7)
        updateView()
    End Sub
#End Region

    Private Sub ReplacerDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.ColorDialog1.SolidColorOnly = False
        Me.ColorDialog1.FullOpen = True
        VScrollBar1.Value = 0
        backupNewColour.Clear()
        backupOldColour.Clear()
        backupNewColour.AddRange(LPCFile.newColours)
        backupOldColour.AddRange(LPCFile.oldColours)
        c_ColourLdraw(0, 0) = Me.o1l : c_ColourLdraw(1, 0) = Me.n1l : c_CoulourButtons(0, 0) = Me.o1h : c_CoulourButtons(1, 0) = Me.n1h
        c_ColourLdraw(0, 1) = Me.o2l : c_ColourLdraw(1, 1) = Me.n2l : c_CoulourButtons(0, 1) = Me.o2h : c_CoulourButtons(1, 1) = Me.n2h
        c_ColourLdraw(0, 2) = Me.o3l : c_ColourLdraw(1, 2) = Me.n3l : c_CoulourButtons(0, 2) = Me.o3h : c_CoulourButtons(1, 2) = Me.n3h
        c_ColourLdraw(0, 3) = Me.o4l : c_ColourLdraw(1, 3) = Me.n4l : c_CoulourButtons(0, 3) = Me.o4h : c_CoulourButtons(1, 3) = Me.n4h
        c_ColourLdraw(0, 4) = Me.o5l : c_ColourLdraw(1, 4) = Me.n5l : c_CoulourButtons(0, 4) = Me.o5h : c_CoulourButtons(1, 4) = Me.n5h
        c_ColourLdraw(0, 5) = Me.o6l : c_ColourLdraw(1, 5) = Me.n6l : c_CoulourButtons(0, 5) = Me.o6h : c_CoulourButtons(1, 5) = Me.n6h
        c_ColourLdraw(0, 6) = Me.o7l : c_ColourLdraw(1, 6) = Me.n7l : c_CoulourButtons(0, 6) = Me.o7h : c_CoulourButtons(1, 6) = Me.n7h
        c_ColourLdraw(0, 7) = Me.o8l : c_ColourLdraw(1, 7) = Me.n8l : c_CoulourButtons(0, 7) = Me.o8h : c_CoulourButtons(1, 7) = Me.n8h
        c_ColourPanels(0, 0) = Me.o1c : c_ColourPanels(1, 0) = Me.n1c : c_ColourTypes(0, 0) = Me.o1t : c_ColourTypes(1, 0) = Me.n1t
        c_ColourPanels(0, 1) = Me.o2c : c_ColourPanels(1, 1) = Me.n2c : c_ColourTypes(0, 1) = Me.o2t : c_ColourTypes(1, 1) = Me.n2t
        c_ColourPanels(0, 2) = Me.o3c : c_ColourPanels(1, 2) = Me.n3c : c_ColourTypes(0, 2) = Me.o3t : c_ColourTypes(1, 2) = Me.n3t
        c_ColourPanels(0, 3) = Me.o4c : c_ColourPanels(1, 3) = Me.n4c : c_ColourTypes(0, 3) = Me.o4t : c_ColourTypes(1, 3) = Me.n4t
        c_ColourPanels(0, 4) = Me.o5c : c_ColourPanels(1, 4) = Me.n5c : c_ColourTypes(0, 4) = Me.o5t : c_ColourTypes(1, 4) = Me.n5t
        c_ColourPanels(0, 5) = Me.o6c : c_ColourPanels(1, 5) = Me.n6c : c_ColourTypes(0, 5) = Me.o6t : c_ColourTypes(1, 5) = Me.n6t
        c_ColourPanels(0, 6) = Me.o7c : c_ColourPanels(1, 6) = Me.n7c : c_ColourTypes(0, 6) = Me.o7t : c_ColourTypes(1, 6) = Me.n7t
        c_ColourPanels(0, 7) = Me.o8c : c_ColourPanels(1, 7) = Me.n8c : c_ColourTypes(0, 7) = Me.o8t : c_ColourTypes(1, 7) = Me.n8t
        c_Delete(0) = Me.d1 : c_Delete(1) = Me.d2 : c_Delete(2) = Me.d3 : c_Delete(3) = Me.d4
        c_Delete(4) = Me.d5 : c_Delete(5) = Me.d6 : c_Delete(6) = Me.d7 : c_Delete(7) = Me.d8
        If c_ColourLdraw(0, 0).Items.Count = 0 Then
            Dim colourStrings(LDConfig.colourHMapName.Keys.Count - 1) As String
            Dim colourNumbers(LDConfig.colourHMapName.Keys.Count - 1) As Short
            Dim index As Integer = 0
            For Each s As Short In LDConfig.colourHMapName.Keys
                colourNumbers(index) = s
                index += 1
            Next
            Array.Sort(colourNumbers)
            index = 0
            For Each s As Short In colourNumbers
                colourStrings(index) = s & ": " & LDConfig.colourHMapName(s)
                index += 1
            Next
            For i As Integer = 0 To 7
                For j As Integer = 0 To 1
                    If c_ColourLdraw(j, i).Items.Count = 0 Then
                        c_ColourLdraw(j, i).Items.AddRange(colourStrings)
                        c_ColourLdraw(j, i).DropDownStyle = ComboBoxStyle.DropDownList
                        c_ColourTypes(j, i).DropDownStyle = ComboBoxStyle.DropDownList
                        c_ColourLdraw(j, i).DropDownWidth = 170
                    End If
                Next
            Next
        End If
        updateView()
    End Sub

    Private Sub updateView()
        doUpdate = True
        Dim colourCount As Integer = backupNewColour.Count
        If colourCount > 8 Then
            VScrollBar1.Maximum = colourCount - 8
        Else
            VScrollBar1.Maximum = 0
        End If

        Dim howManyEntriesToDisplay As Integer = colourCount - VScrollBar1.Value
        If howManyEntriesToDisplay > 8 Then howManyEntriesToDisplay = 8

        For i As Integer = 0 To howManyEntriesToDisplay - 1
            c_ColourPanels(0, i).BackColor = backupOldColour(VScrollBar1.Value + i).rgbValue
            c_ColourPanels(1, i).BackColor = backupNewColour(VScrollBar1.Value + i).rgbValue
            If backupOldColour(VScrollBar1.Value + i).ldrawIndex = -1 Then
                c_ColourTypes(0, i).SelectedIndex = 1
                c_CoulourButtons(0, i).Visible = True
                c_ColourLdraw(0, i).Visible = False
            Else
                c_ColourTypes(0, i).SelectedIndex = 0
                c_CoulourButtons(0, i).Visible = False
                c_ColourLdraw(0, i).Visible = True
                c_ColourLdraw(0, i).SelectedItem = backupOldColour(VScrollBar1.Value + i).ldrawIndex & ": " & LDConfig.colourHMapName.Item(backupOldColour(VScrollBar1.Value + i).ldrawIndex)
            End If
            If backupNewColour(VScrollBar1.Value + i).ldrawIndex = -1 Then
                c_ColourTypes(1, i).SelectedIndex = 1
                c_CoulourButtons(1, i).Visible = True
                c_ColourLdraw(1, i).Visible = False
            Else
                c_ColourTypes(1, i).SelectedIndex = 0
                c_CoulourButtons(1, i).Visible = False
                c_ColourLdraw(1, i).Visible = True
                c_ColourLdraw(1, i).SelectedItem = backupNewColour(VScrollBar1.Value + i).ldrawIndex & ": " & LDConfig.colourHMapName.Item(backupNewColour(VScrollBar1.Value + i).ldrawIndex)
            End If
            For j As Integer = 0 To 1
                c_ColourPanels(j, i).Visible = True
                c_ColourTypes(j, i).Visible = True
            Next
            c_Delete(i).Visible = True
        Next
        For i As Integer = howManyEntriesToDisplay To 7
            For j As Integer = 0 To 1
                c_ColourLdraw(j, i).Visible = False
                c_ColourPanels(j, i).Visible = False
                c_ColourTypes(j, i).Visible = False
                c_CoulourButtons(j, i).Visible = False
            Next
            c_Delete(i).Visible = False
        Next
        Me.Refresh()
        doUpdate = False
    End Sub

    ' done
#Region "DEC New"
    Private Sub n1l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n1l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n1l.SelectedItem, String), 1, CType(n1l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n1c.BackColor = backupNewColour(VScrollBar1.Value).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n2l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n2l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n2l.SelectedItem, String), 1, CType(n2l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n2c.BackColor = backupNewColour(VScrollBar1.Value + 1).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n3l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n3l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n3l.SelectedItem, String), 1, CType(n3l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n3c.BackColor = backupNewColour(VScrollBar1.Value + 2).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n4l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n4l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n4l.SelectedItem, String), 1, CType(n4l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n4c.BackColor = backupNewColour(VScrollBar1.Value + 3).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub


    Private Sub n5l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n5l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n5l.SelectedItem, String), 1, CType(n5l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n5c.BackColor = backupNewColour(VScrollBar1.Value + 4).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n6l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n6l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n6l.SelectedItem, String), 1, CType(n6l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n6c.BackColor = backupNewColour(VScrollBar1.Value + 5).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n7l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n7l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n7l.SelectedItem, String), 1, CType(n7l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n7c.BackColor = backupNewColour(VScrollBar1.Value + 6).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub n8l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles n8l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(n8l.SelectedItem, String), 1, CType(n8l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupNewColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                n8c.BackColor = backupNewColour(VScrollBar1.Value + 7).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region
    ' done
#Region "DEC Old"
    Private Sub o1l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o1l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o1l.SelectedItem, String), 1, CType(o1l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o1c.BackColor = backupOldColour(VScrollBar1.Value).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o2l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o2l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o2l.SelectedItem, String), 1, CType(o2l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value + 1) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o2c.BackColor = backupOldColour(VScrollBar1.Value + 1).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o3l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o3l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o3l.SelectedItem, String), 1, CType(o3l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value + 2) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o3c.BackColor = backupOldColour(VScrollBar1.Value + 2).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o4l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o4l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o4l.SelectedItem, String), 1, CType(o4l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value + 3) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o4c.BackColor = backupOldColour(VScrollBar1.Value + 3).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub


    Private Sub o5l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o5l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o5l.SelectedItem, String), 1, CType(o5l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value + 4) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o5c.BackColor = backupOldColour(VScrollBar1.Value + 4).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o6l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o6l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o6l.SelectedItem, String), 1, CType(o6l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value + 5) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o6c.BackColor = backupOldColour(VScrollBar1.Value + 5).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o7l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o7l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o7l.SelectedItem, String), 1, CType(o7l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value + 6) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o7c.BackColor = backupOldColour(VScrollBar1.Value + 6).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub o8l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles o8l.SelectedIndexChanged
        If Not doUpdate Then
            Try
                Dim value As Short = CType(Val(Mid(CType(o8l.SelectedItem, String), 1, CType(o8l.SelectedItem, String).IndexOf(CType(":", Char)))), Short)
                backupOldColour(VScrollBar1.Value + 7) = New Colour With {.ldrawIndex = value, .rgbValue = LDConfig.colourHMap(value)}
                o8c.BackColor = backupOldColour(VScrollBar1.Value + 7).rgbValue
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region

End Class
