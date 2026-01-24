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
Imports System.Text
Imports System.IO
Imports System.Threading
Imports System.Drawing.Drawing2D
Imports LDPatternCreator

Public Class MainForm

#Region "Main Form"
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If ColourForm.Visible Then
            LDSettings.Editor.colourWindow_x = ColourForm.Location.X
            LDSettings.Editor.colourWindow_y = ColourForm.Location.Y
            LDSettings.Editor.colourWindow_width = ColourForm.Size.Width
            LDSettings.Editor.colourWindow_height = ColourForm.Size.Height
        End If
        If ImageForm.Visible Then
            LDSettings.Editor.backgroundWindow_x = ImageForm.Location.X
            LDSettings.Editor.backgroundWindow_y = ImageForm.Location.Y
        End If
        If PreferencesForm.Visible Then
            LDSettings.Editor.prefsWindow_x = PreferencesForm.Location.X
            LDSettings.Editor.prefsWindow_y = PreferencesForm.Location.Y
        End If
        LDSettings.Editor.mainWindow_x = Me.Location.X
        LDSettings.Editor.mainWindow_y = Me.Location.Y
        LDSettings.Editor.mainWindow_width = Me.Size.Width
        LDSettings.Editor.mainWindow_height = Me.Size.Height
        saveConfig()
        If MainState.unsavedChanges AndAlso ShowAllWarningsToolStripMenuItem.Checked Then
            Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.UnsavedChanges), MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
            If result = MsgBoxResult.Yes Then SaveToolStripMenuItem.PerformClick()
            If result = MsgBoxResult.Cancel Then e.Cancel = True
        End If
        If e.CloseReason = CloseReason.None OrElse e.CloseReason = CloseReason.ApplicationExitCall Then
            SaveAs.FileName = System.IO.Path.GetFullPath(EnvironmentPaths.appPath & "errordump_file.lpc")
            SaveToolStripMenuItem.PerformClick()
            Using DateiOut As BinaryWriter = New BinaryWriter(File.Open(EnvironmentPaths.appPath & "errordump_data.txt", FileMode.Create))
                DateiOut.Write("Application Path: " & EnvironmentPaths.appPath & vbLf)
                DateiOut.Write("Stack Trace: " & My.Application.Info.StackTrace & vbLf)
                DateiOut.Write("Loaded Assemblies: ")
                For i = 0 To My.Application.Info.LoadedAssemblies.Count - 1
                    DateiOut.Write(My.Application.Info.LoadedAssemblies(i).FullName)
                Next
            End Using
            Timer2.Interval = 3000
            e.Cancel = True
            Timer2.Enabled = True
        End If
    End Sub

    Private Sub MainForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If SBZoom.Focused AndAlso Not MainState.isLoading Then
            If View.selectionRadius > 10 AndAlso (e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Down) Then
                View.selectionRadius -= 10
            ElseIf View.selectionRadius < 1000 AndAlso (e.KeyCode = Keys.Right OrElse e.KeyCode = Keys.Up) Then
                View.selectionRadius += 10
            End If
            If e.KeyCode = Keys.Oemplus OrElse e.KeyCode = Keys.Add Then
                SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll), False)
                Me.Refresh()
                Exit Sub
            End If
            If e.KeyCode = Keys.OemMinus OrElse e.KeyCode = Keys.Subtract Then
                SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll), False)
                Me.Refresh()
                Exit Sub
            End If
            If e.KeyCode = LDSettings.Keys.Abort AndAlso BtnAbort.Visible AndAlso BtnAbort.Enabled Then Me.BtnAbort.PerformClick()
            If MainState.primitiveMode > PrimitiveModes.Inactive Then Exit Sub
            If e.KeyData = LoadPatternToolStripMenuItem.ShortcutKeys Then LoadPatternToolStripMenuItem.PerformClick()
            If e.KeyCode = LDSettings.Keys.Preview AndAlso BtnColours.Checked Then
                BtnPreview.PerformClick()
            ElseIf e.KeyCode = LDSettings.Keys.Preview Then
                BtnColours.PerformClick() : BtnPreview.PerformClick()
            End If
            If LDSettings.Editor.useAlternativeKeys Then
                If e.KeyCode = LDSettings.Keys.Translate Then
                    If Not MainState.mousePressed Then
                        MouseHelper.pressMouseRight()
                        MainState.mousePressed = True
                    End If
                    Exit Sub
                ElseIf e.KeyCode = LDSettings.Keys.Zoom Then
                    MainState.klickY = MouseHelper.getCursorpositionY()
                    MainState.zoomScroll = True
                End If
            End If
            If BtnPreview.Checked Then
                If e.KeyCode = Keys.Left Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Right Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Up Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Down Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll), False)
                Exit Sub
            End If
            If e.KeyCode = LDSettings.Keys.ModeSelect Then Me.BtnSelect.PerformClick()
            If e.KeyCode = LDSettings.Keys.ModeMove Then Me.BtnMove.PerformClick()
            If e.KeyCode = LDSettings.Keys.ModeRotate Then Me.BtnRotate.PerformClick()
            If e.KeyCode = LDSettings.Keys.ModeScale Then Me.BtnScale.PerformClick()
            If e.KeyCode = LDSettings.Keys.AddVertex Then Me.BtnAddVertex.PerformClick()
            If e.KeyCode = LDSettings.Keys.AddTriangle Then Me.BtnAddTriangle.PerformClick()
            If e.KeyCode = LDSettings.Keys.Abort Then MainState.cstep = 100 : MainState.cnumber = 0
            If e.KeyCode = LDSettings.Keys.ShowColours Then BtnColours.PerformClick()
            If e.KeyCode = LDSettings.Keys.Pipette Then BtnPipette.PerformClick()
            If e.KeyCode = LDSettings.Keys.CSG AndAlso BtnCSG.Enabled Then BtnCSG.ShowDropDown() : CSGUnionToolStripMenuItem.Select()
            If e.KeyCode = LDSettings.Keys.MergeSplit AndAlso BtnMerge.Enabled Then BtnMerge.ShowDropDown() : ToAverageToolStripMenuItem.Select()
            If e.KeyCode = LDSettings.Keys.AddPrimitive Then BtnPrimitives.ShowDropDown() : LDrawPrimitivesToolStripMenuItem.Select()
            If MainState.adjustmode Then
                If e.KeyCode = Keys.Left Then ImageForm.NUDoffsetX.Value -= View.moveSnap : SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Right Then ImageForm.NUDoffsetX.Value += View.moveSnap : SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Up Then ImageForm.NUDoffsetY.Value -= View.moveSnap : SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Down Then ImageForm.NUDoffsetY.Value += View.moveSnap : SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll), False)
            ElseIf (e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Right OrElse e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down) Then
                If e.KeyCode = Keys.Left Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Right Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Up Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll), False)
                If e.KeyCode = Keys.Down Then SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll), False)
                Select Case MainState.editMode
                    Case EditModes.Translation
                        Dim vabsOffsetX As Integer = View.moveSnap
                        Dim vabsOffsetY As Integer = View.moveSnap
                        If e.KeyCode = Keys.Left Then
                            vabsOffsetX = -View.moveSnap
                            vabsOffsetY = 0
                        End If
                        If e.KeyCode = Keys.Right Then
                            vabsOffsetX = View.moveSnap
                            vabsOffsetY = 0
                        End If
                        If e.KeyCode = Keys.Up Then
                            vabsOffsetX = 0
                            vabsOffsetY = -View.moveSnap
                        End If
                        If e.KeyCode = Keys.Down Then
                            vabsOffsetX = 0
                            vabsOffsetY = View.moveSnap
                        End If
                        For Each vert As Vertex In View.SelectedVertices
                            vert.X -= vabsOffsetX
                            vert.Y += vabsOffsetY
                        Next
                        If View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify = Modified.Primitive Then
                            Dim groupindex As Integer = View.SelectedVertices(0).groupindex
                            LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).translate(-vabsOffsetX, -vabsOffsetY)
                            NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 0)
                            NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 1)
                            NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 3) / 1000D * View.unitFactor
                            NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 0)
                            NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 1)
                            NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 3) / 1000D * View.unitFactor
                        End If
                        MainState.movemode = False
                        UndoRedoHelper.addHistory()
                        Exit Select
                    Case EditModes.Rotation
                        Dim angleToRotate As Double = View.rotateSnap * Math.PI / 180
                        If e.KeyCode = Keys.Right Then
                            angleToRotate *= -1.0
                        End If
                        If e.KeyCode = Keys.Down Then
                            angleToRotate *= -1.0
                        End If
                        If View.SelectedVertices.Count > 0 Then
                            If MainState.objectToModify = Modified.Primitive Then
                                Dim tid As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).centerVertexID
                                For Each vert As Vertex In View.SelectedVertices
                                    If vert.vertexID = tid Then
                                        MainState.temp_center = New Vertex(vert.X, vert.Y, False, False)
                                        Exit For
                                    End If
                                Next
                            Else
                                Dim cursorPos As New Vertex(getXcoordinate(MouseHelper.getCursorpositionX()), getYcoordinate(MouseHelper.getCursorpositionY()), False, False)
                                Dim minDist As Double = Double.MaxValue
                                MainState.temp_center = New Vertex(0, 0, False, False)
                                For Each vert As Vertex In View.SelectedVertices
                                    Dim dist As Double = vert.dist(cursorPos)
                                    If dist < minDist Then
                                        minDist = dist
                                        MainState.temp_center.X = vert.X
                                        MainState.temp_center.Y = vert.Y
                                    End If
                                Next
                            End If
                            For Each vert As Vertex In View.SelectedVertices
                                vert.angleFrom = vert.angle(MainState.temp_center) + Math.PI
                                vert.distanceFrom = vert.dist(MainState.temp_center)
                                vert.oldX = vert.X
                                vert.oldY = vert.Y
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.X = MainState.temp_center.X + vert.distanceFrom * Math.Cos(vert.angleFrom + angleToRotate)
                                vert.Y = MainState.temp_center.Y + vert.distanceFrom * Math.Sin(vert.angleFrom + angleToRotate)
                                If Double.IsNaN(vert.X) OrElse Double.IsNaN(vert.Y) Then
                                    vert.X = vert.oldX
                                    vert.Y = vert.oldY
                                End If
                            Next
                            If View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify = Modified.Primitive Then
                                Dim groupindex As Integer = View.SelectedVertices(0).groupindex
                                LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).rotate(angleToRotate)
                                NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 0)
                                NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 1)
                                NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 3) / 1000D * View.unitFactor
                                NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 0)
                                NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 1)
                                NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 3) / 1000D * View.unitFactor
                            End If
                            UndoRedoHelper.addHistory()
                        End If
                        Exit Select
                    Case EditModes.Scale
                        If View.SelectedVertices.Count > 0 Then
                            If MainState.objectToModify = Modified.Primitive Then
                                Dim tid As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).centerVertexID
                                For Each vert As Vertex In View.SelectedVertices
                                    If vert.vertexID = tid Then
                                        MainState.temp_center = New Vertex(vert.X, vert.Y, False, False)
                                        Exit For
                                    End If
                                Next
                            Else
                                MainState.temp_center = New Vertex(0, 0, False, False)
                                For Each vert As Vertex In View.SelectedVertices
                                    MainState.temp_center += vert
                                Next
                                MainState.temp_center.X /= View.SelectedVertices.Count
                                MainState.temp_center.Y /= View.SelectedVertices.Count
                            End If
                            For Each vert As Vertex In View.SelectedVertices
                                vert.angleFrom = vert.angle(MainState.temp_center) + Math.PI
                                vert.distanceFrom = vert.dist(MainState.temp_center)
                                vert.oldX = vert.X
                                vert.oldY = vert.Y
                            Next
                            Dim factorToScale As Double = 1 + View.scaleSnap
                            If e.KeyCode = Keys.Left Then
                                factorToScale = 1 - View.scaleSnap
                            End If
                            If e.KeyCode = Keys.Down Then
                                factorToScale = 1 - View.scaleSnap
                            End If
                            If factorToScale < 0.1 Then factorToScale = 0.1
                            If factorToScale > 2 Then factorToScale = 2
                            For Each vert As Vertex In View.SelectedVertices
                                If Not Control.ModifierKeys = Keys.Control Then
                                    vert.X = MainState.temp_center.X + factorToScale * vert.distanceFrom * Math.Cos(vert.angleFrom)
                                Else
                                    vert.X = vert.oldX
                                End If
                                If Not Control.ModifierKeys = Keys.Shift Then
                                    vert.Y = MainState.temp_center.Y + factorToScale * vert.distanceFrom * Math.Sin(vert.angleFrom)
                                Else
                                    vert.Y = vert.oldY
                                End If
                                If Double.IsNaN(vert.X) OrElse Double.IsNaN(vert.Y) Then
                                    vert.X = vert.oldX
                                    vert.Y = vert.oldY
                                End If
                            Next
                            If View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify = Modified.Primitive Then
                                Dim factorToScaleX As Double = 1
                                Dim factorToScaleY As Double = 1
                                If Not Control.ModifierKeys = Keys.Control Then factorToScaleX = factorToScale
                                If Not Control.ModifierKeys = Keys.Shift Then factorToScaleY = factorToScale
                                Dim groupindex As Integer = View.SelectedVertices(0).groupindex
                                LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).scale(factorToScaleX, factorToScaleY)
                                NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 0)
                                NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 1)
                                NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 3) / 1000D * View.unitFactor
                                NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 0)
                                NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 1)
                                NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 3) / 1000D * View.unitFactor
                            End If
                            UndoRedoHelper.addHistory()
                        End If
                End Select
                If View.CollisionVertices.Count > 0 Then
                    detectCollisions()
                    UndoRedoHelper.addHistory()
                End If
            End If
            If MainState.cstep = 1 Then
                If e.KeyCode = Keys.NumPad1 Then MainState.cnumber += 1
                If e.KeyCode = Keys.NumPad2 Then MainState.cnumber += 2
                If e.KeyCode = Keys.NumPad3 Then MainState.cnumber += 3
                If e.KeyCode = Keys.NumPad4 Then MainState.cnumber += 4
                If e.KeyCode = Keys.NumPad5 Then MainState.cnumber += 5
                If e.KeyCode = Keys.NumPad6 Then MainState.cnumber += 6
                If e.KeyCode = Keys.NumPad7 Then MainState.cnumber += 7
                If e.KeyCode = Keys.NumPad8 Then MainState.cnumber += 8
                If e.KeyCode = Keys.NumPad9 Then MainState.cnumber += 9
                If LDConfig.colourHMap.ContainsKey(MainState.cnumber) Then
                    setColour(LDConfig.colourHMap(MainState.cnumber), MainState.cnumber)
                Else
                    setColour(LDConfig.colourHMap(16), MainState.cnumber)
                End If
                UndoRedoHelper.addHistory()
                MainState.cstep = 100 : MainState.cnumber = 0
            Else
                If e.KeyCode = Keys.NumPad0 Then MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad1 Then MainState.cnumber += 1 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad2 Then MainState.cnumber += 2 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad3 Then MainState.cnumber += 3 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad4 Then MainState.cnumber += 4 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad5 Then MainState.cnumber += 5 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad6 Then MainState.cnumber += 6 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad7 Then MainState.cnumber += 7 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad8 Then MainState.cnumber += 8 * MainState.cstep : MainState.cstep /= 10
                If e.KeyCode = Keys.NumPad9 Then MainState.cnumber += 9 * MainState.cstep : MainState.cstep /= 10
            End If
        End If
    End Sub

    Private Sub MainForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If MainState.mousePressed Then
            MouseHelper.releaseMouseRight()
            MainState.mousePressed = False
        End If
        MainState.zoomScroll = False
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EnvironmentPaths.recentFiles.Clear()
        Dim strl(9) As String
        EnvironmentPaths.recentFiles.AddRange(strl)
        View.showGrid = True
        UndoRedoHelper.addHistory()
        Me.TrianglesModeToolStripMenuItem.PerformClick()
        If Not My.Computer.FileSystem.DirectoryExists(EnvironmentPaths.appPath & "lang") Then
            My.Computer.FileSystem.CreateDirectory(EnvironmentPaths.appPath & "lang")
        End If
        If Not My.Computer.FileSystem.DirectoryExists(EnvironmentPaths.appPath & "template") Then
            My.Computer.FileSystem.CreateDirectory(EnvironmentPaths.appPath & "template")
        End If

        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "lang\lang_de_DE.csv") Then
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "lang\lang_de_DE.csv", False, New System.Text.UTF8Encoding(False))
                For Each b As Char In My.Resources.lang_de_DE
                    DateiOut.Write(b)
                Next
            End Using
        End If
        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "lang\lang_en_GB.csv") Then
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "lang\lang_en_GB.csv", False, New System.Text.UTF8Encoding(False))
                For Each b As Char In My.Resources.lang_en_GB
                    DateiOut.Write(b)
                Next
            End Using
        End If
        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "lang\lang_fr_FR.csv") Then
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "lang\lang_fr_FR.csv", False, New System.Text.UTF8Encoding(False))
                For Each b As Char In My.Resources.lang_fr_FR
                    DateiOut.Write(b)
                Next
            End Using
        End If

        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "Rectifier.exe") Then
            Using DateiOut As BinaryWriter = New BinaryWriter(File.Open(EnvironmentPaths.appPath & "Rectifier.exe", FileMode.Create))
                For Each b As Byte In My.Resources.Rectifier
                    DateiOut.Write(b)
                Next
            End Using
        End If
        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "Slicerpro.exe") Then
            Using DateiOut As BinaryWriter = New BinaryWriter(File.Open(EnvironmentPaths.appPath & "Slicerpro.exe", FileMode.Create))
                For Each b As Byte In My.Resources.Slicerpro
                    DateiOut.Write(b)
                Next
            End Using
        End If
        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "Unificator.exe") Then
            Using DateiOut As BinaryWriter = New BinaryWriter(File.Open(EnvironmentPaths.appPath & "Unificator.exe", FileMode.Create))
                For Each b As Byte In My.Resources.Unificator
                    DateiOut.Write(b)
                Next
            End Using
        End If

        LDSettings.Editor.myLanguage = LanguageHelper.loadLanguageFromConfig()
        If LDSettings.Editor.myLanguage <> "" Then
            loadLanguage(LDSettings.Editor.myLanguage)
        End If
        loadConfig()
        SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -15, ScrollOrientation.VerticalScroll))
        If EnvironmentPaths.ldrawPath = "" Then
            If Me.FBDLDrawDir.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                MsgBox(I18N.trl8(I18N.lk.LDrawDirInfo), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Info))
                Me.LDrawPrimitivesToolStripMenuItem.Visible = False
                Me.ImportToolStripMenuItem.Visible = False
                Me.LoadPatternToolStripMenuItem.Visible = False
                Me.TemplateToolStripMenuItem.Visible = False
            Else
                EnvironmentPaths.ldrawPath = Me.FBDLDrawDir.SelectedPath
            End If
            saveConfig()
            loadConfig()
        End If
        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "LDConfig-fallback.ldr") Then
            Using DateiOut As BinaryWriter = New BinaryWriter(File.Open(EnvironmentPaths.appPath & "LDConfig-fallback.ldr", FileMode.Create))
                For Each b As Byte In My.Resources.LDConfig
                    DateiOut.Write(b)
                Next
            End Using
            If EnvironmentPaths.appPath <> (EnvironmentPaths.ldrawPath & "\") Then
                If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "LDConfig.ldr") Then
                    My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "LDConfig.ldr")
                End If
            End If
        End If
        If Not My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "Colours.txt") Then
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "Colours.txt", False, New System.Text.UTF8Encoding(False))
                For i As Integer = 0 To 5
                    DateiOut.WriteLine(i)
                    DateiOut.WriteLine("DEC")
                Next i
                DateiOut.WriteLine(70)
                DateiOut.WriteLine("DEC")
                DateiOut.WriteLine(71)
                DateiOut.WriteLine("DEC")
                DateiOut.WriteLine(72)
                DateiOut.WriteLine("DEC")
                For i As Integer = 9 To 15
                    DateiOut.WriteLine(i)
                    DateiOut.WriteLine("DEC")
                Next i
            End Using
        End If
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.ldrawPath & "\LDConfig.ldr") Then
            Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.ldrawPath & "\LDConfig.ldr", New System.Text.UTF8Encoding(False))
                Do
                    Dim s As String = Replace(DateiIn.ReadLine, "CODE", "@")
                    If s Like "*!COLOUR*" Then
                        Dim s1() As String = s.Split("@")
                        If s1.Length = 2 Then
                            s1(0) = Replace(Replace(s1(0), "0 !COLOUR ", ""), "_", " ")
                            s1(1) = Replace(s1(1), "VALUE", "@")
                            s1(1) = Replace(s1(1), "EDGE", "@")
                            Dim s2() As String = s1(1).Split("@")
                            s2(0) = Trim(s2(0))
                            s2(1) = Mid(Trim(s2(1)), 2)
                            Dim id As Short = CType(s2(0), Short)
                            If LDConfig.maxColourNumber < id Then LDConfig.maxColourNumber = id
                            If id <> 16 Then LDConfig.colourHMap.Add(id, Color.FromArgb(Integer.Parse("FF" & s2(1), System.Globalization.NumberStyles.HexNumber))) Else LDConfig.colourHMap.Add(id, Color.FromArgb(0, 0, 0, 0))
                            LDConfig.colourHMapName.Add(id, s1(0))
                        End If
                    End If
                Loop Until DateiIn.EndOfStream
            End Using
        Else
            Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "LDConfig-fallback.ldr", New System.Text.UTF8Encoding(False))
                Do
                    Dim s As String = Replace(DateiIn.ReadLine, "CODE", "@")
                    If s Like "*!COLOUR*" Then
                        Dim s1() As String = s.Split("@")
                        If s1.Length = 2 Then
                            s1(0) = Replace(Replace(s1(0), "0 !COLOUR ", ""), "_", " ")
                            s1(1) = Replace(s1(1), "VALUE", "@")
                            s1(1) = Replace(s1(1), "EDGE", "@")
                            Dim s2() As String = s1(1).Split("@")
                            s2(0) = Trim(s2(0))
                            s2(1) = Mid(Trim(s2(1)), 2)
                            Dim id As Short = CType(s2(0), Short)
                            If LDConfig.maxColourNumber < id Then LDConfig.maxColourNumber = id
                            If id <> 16 Then LDConfig.colourHMap.Add(id, Color.FromArgb(Integer.Parse("FF" & s2(1), System.Globalization.NumberStyles.HexNumber))) Else LDConfig.colourHMap.Add(id, Color.FromArgb(0, 0, 0, 0))
                            LDConfig.colourHMapName.Add(id, s1(0))
                        End If
                    End If
                Loop Until DateiIn.EndOfStream
            End Using
        End If
        For c As Integer = 0 To 999
            If Not LDConfig.colourHMap.ContainsKey(c) Then LDConfig.colourHMap.Add(c, Nothing) : LDConfig.colourHMapName.Add(c, "?")
        Next
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "Colours.txt") Then
            Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "Colours.txt", New System.Text.UTF8Encoding(False))
                Dim tsb() As ToolStripButton = {C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15}
                Dim c As Integer
                For Each t As ToolStripButton In tsb
                    c = CInt(DateiIn.ReadLine())
                    If DateiIn.ReadLine() = "DEC" Then
                        t.BackColor = LDConfig.colourHMap(c)
                        t.Tag = c
                    Else
                        t.BackColor = Color.FromArgb(c)
                        t.Tag = -1
                    End If
                    toolStripTrl8(t)
                    AddHandler t.Click, AddressOf Me.colourBtnClick
                    AddHandler t.MouseUp, AddressOf Me.colourBtnMouseUp
                Next
            End Using
        End If
        Dim templates() As String = Directory.GetFiles(EnvironmentPaths.appPath & "template\")
        For i As Integer = 0 To templates.Length - 1
            If templates(i) Like "*.txt" Then
                Dim ti As New ToolStripMenuItem
                ti.Text = Replace(Mid(templates(i), templates(i).LastIndexOf("\") + 2), ".txt", "")
                ti.ToolTipText = templates(i)
                AddHandler ti.Click, AddressOf TemplateItemClick
                TemplateToolStripMenuItem.DropDownItems.Add(ti)
            End If
        Next
        Dim languages() As String = Directory.GetFiles(EnvironmentPaths.appPath & "lang\")
        For i As Integer = 0 To languages.Length - 1
            If languages(i) Like "*.csv" Then
                Try
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(languages(i), New System.Text.UTF8Encoding(False))
                        Dim ti As New ToolStripMenuItem
                        ti.Text = Replace(Replace(DateiIn.ReadLine(), ";", ""), """", "")
                        ti.ToolTipText = languages(i)
                        AddHandler ti.Click, AddressOf loadLanguage
                        LanguageToolStripMenuItem.DropDownItems.Add(ti)
                    End Using
                Catch
                End Try
            End If
        Next
        newPattern()
        If My.Application.CommandLineArgs.Count > 0 Then
            Dim v As Array
            Dim mypath As String
            Dim commandlineargs As String = Environment.CommandLine
            v = Split(commandlineargs, """ ")
            mypath = v(1)
            If My.Computer.FileSystem.FileExists(mypath) AndAlso (mypath Like "*.lpc" OrElse mypath Like "*.txt") Then
                LoadLPCFile(mypath)
            End If
        End If
        Me.Refresh()
    End Sub

    Private Sub recentFileNameClick(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs)
        If My.Computer.FileSystem.FileExists(sender.ToolTipText) Then
            sender.Image = Nothing
            If MainState.unsavedChanges AndAlso ShowAllWarningsToolStripMenuItem.Checked Then
                Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.UnsavedChanges), MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
                If result = MsgBoxResult.Yes Then SaveToolStripMenuItem.PerformClick()
                If result = MsgBoxResult.Cancel Then Exit Sub
            End If
            LoadLPCFile(sender.ToolTipText)
        Else
            sender.Image = My.Resources.collision
            Beep()
        End If
    End Sub

    Private Sub TemplateItemClick(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs)
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "template\" & sender.Text & ".txt") Then
            If MainState.unsavedChanges AndAlso ShowAllWarningsToolStripMenuItem.Checked Then
                Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.UnsavedChanges), MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
                If result = MsgBoxResult.Yes Then SaveToolStripMenuItem.PerformClick()
                If result = MsgBoxResult.Cancel Then Exit Sub
            End If

            MainState.isLoading = True
            Dim result2 As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.NewPattern), MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.DefaultButton1 + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
            If result2 = MsgBoxResult.No Then
                LPCFile.templateProjectionQuads.Clear()
                LPCFile.templateTexts.Clear()
                LPCFile.templateShape.Clear()
                scaleVertices(True)
            Else
                newPattern()
                MainState.isLoading = True
            End If

            For Each v As Vertex In LPCFile.Vertices
                If v.groupindex = Primitive.TEMPLATE_INDEX Then
                    v.groupindex = Primitive.NO_INDEX
                End If
            Next

            Dim lineNumber As Integer = 0
            Dim content As String = ""
            Try
                Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "template\" & sender.Text & ".txt", New System.Text.UTF8Encoding(False))
                    lineNumber += 1
                    LPCFile.myMetadata.recommendedMode = CType(DateiIn.ReadLine(), Byte)
                    lineNumber += 1
                    LPCFile.myMetadata.additionalData = DateiIn.ReadLine()
                    For r As Integer = 0 To 3
                        lineNumber += 1
                        Dim rm() As String = Replace(DateiIn.ReadLine(), ".", MathHelper.comma).Split(" ")
                        For c As Integer = 0 To 3
                            LPCFile.myMetadata.matrix(r, c) = CType(rm(c), Double)
                        Next
                    Next
                    Do
                        lineNumber += 1
                        content = DateiIn.ReadLine
                        If content <> "" Then
                            Dim oldlenght As Integer
                            Do
                                oldlenght = content.Length
                                content = Replace(content, "  ", " ")
                            Loop Until oldlenght = content.Length
                            Dim s() As String = Replace(content, ".", MathHelper.comma).Split(" ")
                            If s.Length = 1 Then
                                If lineNumber > 7 AndAlso LPCFile.templateShape.Count > 0 Then
                                    LPCFile.templateShape.Add(New PointF(Single.Epsilon, Single.Epsilon))
                                End If
                            ElseIf s.Length = 2 Then
                                LPCFile.templateShape.Add(New PointF(CType(s(0), Single) * 1000, CType(s(1), Single) * 1000))
                                LPCFile.Vertices.Add(New Vertex(CType(s(0), Double), CType(s(1), Double), False) With {.groupindex = Primitive.TEMPLATE_INDEX})
                            ElseIf Mid(content, 1, 5) = "0 // " AndAlso s.Length > 4 Then
                                Dim Text As String = s(4)
                                For i As Integer = 5 To s.Length - 1
                                    Text += " " & s(i)
                                Next
                                LPCFile.templateTexts.Add(New TemplateTextInfo(CType(s(2), Single) * 1000, CType(s(3), Single) * 1000, Text))
                            ElseIf s.Length = 15 Then
                                LPCFile.templateShape.Add(New PointF(CType(s(2), Single) * 1000, CType(s(4), Single) * 1000))
                                inlinePrimitive(1, s(1),
                                -CType(s(5), Double), -CType(s(7), Double), 0, -CType(s(2), Double),
                                -CType(s(11), Double), -CType(s(13), Double), 0, CType(s(4), Double),
                                                                                    0, 0, 1, 0,
                                                                                    -1, 0, 0, 0,
                                                                                    0, -1, 0, 0,
                                                                                    0, 0, 1, 0,
                                                                                    Replace(s(14), MathHelper.comma, "."), New List(Of String), False)
                                MainState.isLoading = True
                            ElseIf s.Length = 20 Then
                                Dim Double_array(s.Length - 1) As Double
                                For i As Integer = 0 To s.Length - 1
                                    s(i) = Replace(Replace(Replace(s(i), "{", ""), "}", ""), ";", "")
                                    Double_array(i) = CType(s(i), Double)
                                Next
                                LPCFile.templateProjectionQuads.Add(New ProjectionQuad(
                                Double_array(0), Double_array(1), Double_array(2), Double_array(3), Double_array(4),
                                Double_array(5), Double_array(6), Double_array(7), Double_array(8), Double_array(9),
                                Double_array(10), Double_array(11), Double_array(12), Double_array(13), Double_array(14),
                                Double_array(15), Double_array(16), Double_array(17), Double_array(18), Double_array(19)))
                            End If
                        End If
                    Loop Until DateiIn.EndOfStream
                    cleanupDATVertices()
                    scaleVertices(False)
                End Using
            Catch ex As Exception
                MainState.isLoading = True
                MsgBox(String.Format(I18N.trl8(I18N.lk.ParsingErrorDescription), ex.Message & vbCrLf, lineNumber) & ": " & content, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Critical, I18N.trl8(I18N.lk.ParsingError))
                newPattern()
            End Try
            UndoRedoHelper.addHistory()
            MainState.isLoading = False
            Me.Refresh()
        End If
    End Sub

    Public Sub New()
        ' Fill the language set with default values
        Dim wordCount As Short
        Do
            Dim tlist As New List(Of String)
            tlist.AddRange([Enum].GetNames(GetType(I18N.lk)))
            wordCount = tlist.Count
            Exit Do
        Loop
        For i As Short = 0 To wordCount - 1
            If Not I18N.trl8.ContainsKey(i) Then
                I18N.trl8.Add(i, "NOT TRANSLATED")
            End If
        Next
        ' This call is mandatory for the Windows Form-Designer!
        InitializeComponent()
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
    End Sub

    Private Sub MainForm_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        Me.Refresh()
    End Sub

    Private Sub MainForm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        Me.Refresh()
    End Sub
#End Region

#Region "Mouse"
    Private Sub MainForm_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If MainState.isLoading Then Exit Sub
        Me.ContextMenuStrip = Nothing
        If Not Me.Focused Then Me.Focus()
        If Not SBZoom.Focused Then SBZoom.Focus()
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If View.getCorrectionOffset Then
                View.correctionOffsetX = System.Windows.Forms.Cursor.Position.X - e.X
                View.correctionOffsetY = System.Windows.Forms.Cursor.Position.Y - e.Y
                MouseHelper.moveMouseAbsoluteLeftClick(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2, View.correctionOffsetY + Me.ClientSize.Height / 2)
                View.getCorrectionOffset = False
            End If
            If MainState.adjustmode Then
                MainState.klickX = MouseHelper.getCursorpositionX()
                MainState.klickY = MouseHelper.getCursorpositionY()
                MainState.doAdjust = True
                Exit Sub
            End If
            If MainState.primitiveMode = PrimitiveModes.Inactive Then
                Select Case MainState.editMode
                    Case EditModes.Selection
                        MainState.klickX = MouseHelper.getCursorpositionX()
                        MainState.klickY = MouseHelper.getCursorpositionY()
                        MainState.doSelection = True
                        Me.Refresh()
                        Exit Select
                    Case EditModes.Translation
                        MainState.movemode = True
                        MainState.klickX = MouseHelper.getCursorpositionX()
                        MainState.klickY = MouseHelper.getCursorpositionY()
                        Exit Select
                    Case EditModes.Rotation
                        If View.SelectedVertices.Count > 0 Then
                            MainState.rotatemode = True
                            MainState.klickX = MouseHelper.getCursorpositionX()
                            MainState.klickY = MouseHelper.getCursorpositionY()
                            If MainState.objectToModify = Modified.Primitive Then
                                Dim tid As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).centerVertexID
                                For Each vert As Vertex In View.SelectedVertices
                                    If vert.vertexID = tid Then
                                        MainState.temp_center = New Vertex(vert.X, vert.Y, False, False)
                                        Exit For
                                    End If
                                Next
                            Else
                                Dim cursorPos As New Vertex(getXcoordinate(MainState.klickX), getYcoordinate(MainState.klickY), False, False)
                                Dim minDist As Double = Double.MaxValue
                                MainState.temp_center = New Vertex(0, 0, False, False)
                                For Each vert As Vertex In View.SelectedVertices
                                    Dim dist As Double = vert.dist(cursorPos)
                                    If dist < minDist Then
                                        minDist = dist
                                        MainState.temp_center.X = vert.X
                                        MainState.temp_center.Y = vert.Y
                                    End If
                                Next
                            End If
                            For Each vert As Vertex In View.SelectedVertices
                                vert.angleFrom = vert.angle(MainState.temp_center) + Math.PI
                                vert.distanceFrom = vert.dist(MainState.temp_center)
                                vert.oldX = vert.X
                                vert.oldY = vert.Y
                            Next
                        End If
                        Exit Select
                    Case EditModes.Scale
                        If View.SelectedVertices.Count > 0 Then
                            MainState.scalemode = True
                            MainState.klickX = MouseHelper.getCursorpositionX()
                            MainState.klickY = MouseHelper.getCursorpositionY()
                            If MainState.objectToModify = Modified.Primitive Then
                                Dim tid As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(1).groupindex)).centerVertexID
                                For Each vert As Vertex In View.SelectedVertices
                                    If vert.vertexID = tid Then
                                        MainState.temp_center = New Vertex(vert.X, vert.Y, False, False)
                                        Exit For
                                    End If
                                Next
                            Else
                                MainState.temp_center = New Vertex(0, 0, False, False)
                                For Each vert As Vertex In View.SelectedVertices
                                    MainState.temp_center += vert
                                Next
                                MainState.temp_center.X /= View.SelectedVertices.Count
                                MainState.temp_center.Y /= View.SelectedVertices.Count
                            End If
                            For Each vert As Vertex In View.SelectedVertices
                                vert.angleFrom = vert.angle(MainState.temp_center) + Math.PI
                                vert.distanceFrom = vert.dist(MainState.temp_center)
                                vert.oldX = vert.X
                                vert.oldY = vert.Y
                            Next
                            Me.Refresh()
                        End If
                        Exit Select
                End Select
            End If
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            View.oldOffsetX = View.offsetX
            View.oldOffsetY = View.offsetY
            MainState.klickX = MouseHelper.getCursorpositionX()
            MainState.klickY = MouseHelper.getCursorpositionY()
            MainState.doCameraMove = True
        End If
    End Sub

    Private Sub MainForm_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If Not MainState.isLoading Then
            If View.SelectedTriangles.Count > 0 AndAlso View.SelectedTriangles(0).groupindex > -1 Then
                Try
                    Dim p As Primitive = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedTriangles(0).groupindex))
                    If p.primitiveName Like "subfile*" Then
                        LblCoords.Text = LPCFile.PrimitivesMetadataHMap(p.primitiveName).mData(1) & "   "
                    Else
                        LblCoords.Text = p.primitiveName & "   "
                    End If
                Catch
                End Try
            Else
                LblCoords.Text = ""
            End If
            LblCoords.Text = LblCoords.Text & Math.Round(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000, 3) & " | " & Math.Round(Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000, 3) & " " & translateUnit(View.unit)

            If BtnAddTriangle.Checked Then
                LblCoords.Text = LblCoords.Text & " " & I18N.trl8(I18N.lk.AddHint)
            End If
        End If
        Dim curX As Integer
        Dim curY As Integer
        curX = e.X
        curY = e.Y
        If curX + View.correctionOffsetX > Cursor.Position.X Then View.correctionOffsetX = Cursor.Position.X - curX
        If curX + View.correctionOffsetX < Cursor.Position.X Then View.correctionOffsetX = Cursor.Position.X - curX
        If curY + View.correctionOffsetY > Cursor.Position.Y Then View.correctionOffsetY = Cursor.Position.Y - curY
        If curY + View.correctionOffsetY < Cursor.Position.Y Then View.correctionOffsetY = Cursor.Position.Y - curY
        If MainState.doSelection OrElse MainState.doCameraMove OrElse MainState.movemode OrElse MainState.trianglemode > 0 OrElse MainState.referenceLineMode > 0 OrElse MainState.rotatemode OrElse MainState.scalemode OrElse MainState.adjustmode OrElse MainState.primitiveMode > 0 OrElse BtnAddVertex.Checked OrElse BtnAddTriangle.Checked OrElse BtnTriangleAutoCompletion.Enabled Then Me.Refresh()
        If MainState.adjustmode Then
            Dim tx As Integer = MouseHelper.getCursorpositionX()
            Dim ty As Integer = MouseHelper.getCursorpositionY()
            Dim absOffsetX As Integer = View.offsetX * View.zoomfactor + CType(Me.ClientSize.Width / 2, Integer)
            Dim absOffsetY As Integer = View.offsetY * View.zoomfactor + CType(Me.ClientSize.Height / 2, Integer)
            Dim absImgOffsetX As Integer = Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / View.zoomfactor)
            Dim absImgOffsetY As Integer = Fix((MouseHelper.getCursorpositionY() - MainState.klickY) / View.zoomfactor)
            If Not MainState.doAdjust Then
                absImgOffsetX = 0
                absImgOffsetY = 0
            End If
            Dim tempImgOffsetX As Integer = View.imgOffsetX
            Dim tempImgOffsetY As Integer = View.imgOffsetY
            Dim tScale As Double = View.imgScale
            If Me.Cursor = Cursors.SizeAll Then
                tempImgOffsetX += absImgOffsetX
                tempImgOffsetY += absImgOffsetY
            ElseIf Me.Cursor = Cursors.SizeNESW OrElse Me.Cursor = Cursors.SizeNWSE OrElse Me.Cursor = Cursors.SizeWE OrElse Me.Cursor = Cursors.SizeNS Then
                If MouseHelper.getCursorpositionX() < CType(absOffsetX + tempImgOffsetX * View.zoomfactor, Integer) Then absImgOffsetX *= -1
                If MouseHelper.getCursorpositionY() < CType(absOffsetY + tempImgOffsetY * View.zoomfactor, Integer) Then absImgOffsetY *= -1
                If Me.Cursor = Cursors.SizeWE Then
                    tScale += ((View.backgroundPicture.Width * tScale + absImgOffsetX) / View.backgroundPicture.Width - tScale) * 2
                Else
                    tScale += ((View.backgroundPicture.Height * tScale + absImgOffsetY) / View.backgroundPicture.Height - tScale) * 2
                End If
                If Control.ModifierKeys = Keys.Control Then MainState.adjustDirection = -1 Else MainState.adjustDirection = MainState.tempAdjustDirection
                Select Case MainState.adjustDirection
                    Case 0 : tempImgOffsetY -= absImgOffsetY
                    Case 1.5 : tempImgOffsetX += View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY -= absImgOffsetY
                    Case 3 : tempImgOffsetX += absImgOffsetX
                    Case 4.5 : tempImgOffsetX += View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY += absImgOffsetY
                    Case 6 : tempImgOffsetY += absImgOffsetY
                    Case 7.5 : tempImgOffsetX -= View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY += absImgOffsetY
                    Case 9 : tempImgOffsetX -= absImgOffsetX
                    Case 10.5 : tempImgOffsetX -= View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY -= absImgOffsetY
                End Select
            End If
            If Not MainState.doAdjust Then
                If CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) < tx AndAlso CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) < ty AndAlso
                    CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) + View.backgroundPicture.Width * View.zoomfactor * tScale > tx AndAlso
                    CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) + View.backgroundPicture.Height * View.zoomfactor * tScale > ty Then
                    Me.Cursor = Cursors.SizeAll
                ElseIf CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) <= tx + 6 AndAlso CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) <= ty + 6 AndAlso
                    CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) + View.backgroundPicture.Width * View.zoomfactor * tScale >= tx - 6 AndAlso
                    CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) + View.backgroundPicture.Height * View.zoomfactor * tScale >= ty - 6 Then
                    If Math.Abs(CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) - ty) < 6 OrElse Math.Abs(CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) + View.backgroundPicture.Height * View.zoomfactor * tScale - ty) < 6 Then
                        If Math.Abs(CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) - ty) < 6 Then
                            If Math.Abs(CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) - tx) < 6 Then
                                Me.Cursor = Cursors.SizeNWSE
                            ElseIf Math.Abs(CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) + View.backgroundPicture.Width * View.zoomfactor * tScale - tx) < 6 Then
                                Me.Cursor = Cursors.SizeNESW
                            Else
                                Me.Cursor = Cursors.SizeNS
                            End If
                        Else
                            If Math.Abs(CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) - tx) < 6 Then
                                Me.Cursor = Cursors.SizeNESW
                            ElseIf Math.Abs(CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) + View.backgroundPicture.Width * View.zoomfactor * tScale - tx) < 6 Then
                                Me.Cursor = Cursors.SizeNWSE
                            Else
                                Me.Cursor = Cursors.SizeNS
                            End If
                        End If
                        If Me.Cursor = Cursors.SizeNS Then
                            If Math.Abs(CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) - ty) < 6 Then
                                MainState.adjustDirection = 0
                            Else
                                MainState.adjustDirection = 6
                            End If
                        End If
                        If Me.Cursor = Cursors.SizeNESW Then
                            If Math.Abs(CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) - ty) < 6 Then
                                MainState.adjustDirection = 1.5
                            Else
                                MainState.adjustDirection = 7.5
                            End If
                        End If
                        If Me.Cursor = Cursors.SizeNWSE Then
                            If Math.Abs(CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) - ty) < 6 Then
                                MainState.adjustDirection = 10.5
                            Else
                                MainState.adjustDirection = 4.5
                            End If
                        End If
                    ElseIf Math.Abs(CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) - tx) < 6 OrElse Math.Abs(CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) + View.backgroundPicture.Width * View.zoomfactor * tScale - tx) < 6 Then
                        Me.Cursor = Cursors.SizeWE
                        If Math.Abs(CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) - tx) < 6 Then
                            MainState.adjustDirection = 9
                        Else
                            MainState.adjustDirection = 3
                        End If
                    Else
                        Me.Cursor = Cursors.Cross
                    End If
                Else
                    Me.Cursor = Cursors.Cross
                End If
                MainState.tempAdjustDirection = MainState.adjustDirection
            End If
        Else
            Me.Cursor = Cursors.Cross
        End If
        If MainState.zoomScroll Then
            If MainState.klickY - MouseHelper.getCursorpositionY() < -10 Then
                MainState.klickY = MouseHelper.getCursorpositionY()
                SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, 0, 1, ScrollOrientation.VerticalScroll))
            ElseIf MainState.klickY - MouseHelper.getCursorpositionY() > 10 Then
                MainState.klickY = MouseHelper.getCursorpositionY()
                SBZoom_Scroll(Me, New ScrollEventArgs(ScrollEventType.SmallDecrement, 0, -1, ScrollOrientation.VerticalScroll))
            End If
        End If
    End Sub

    Private Sub MainForm_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        MainState.doSelection = False
        MainState.readOnlyVertex = Nothing
        If Not MainState.isLoading Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If MainState.adjustmode Then
                    MainState.isLoading = True
                    MainState.doAdjust = False
                    Dim vabsOffsetX As Integer = Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / View.zoomfactor)
                    Dim vabsOffsetY As Integer = Fix((MouseHelper.getCursorpositionY() - MainState.klickY) / View.zoomfactor)
                    Dim absOffsetX As Integer = View.offsetX * View.zoomfactor + CType(Me.ClientSize.Width / 2, Integer)
                    Dim absOffsetY As Integer = View.offsetY * View.zoomfactor + CType(Me.ClientSize.Height / 2, Integer)
                    MainState.klickX = MouseHelper.getCursorpositionX()
                    MainState.klickY = MouseHelper.getCursorpositionY()
                    If Me.Cursor = Cursors.SizeAll Then
                        ImageForm.NUDoffsetX.Value = MathHelper.clipf(ImageForm.NUDoffsetX.Value + vabsOffsetX, ImageForm.NUDoffsetX.Minimum, ImageForm.NUDoffsetX.Maximum)
                        ImageForm.NUDoffsetY.Value = MathHelper.clipf(ImageForm.NUDoffsetY.Value + vabsOffsetY, ImageForm.NUDoffsetY.Minimum, ImageForm.NUDoffsetY.Maximum)
                    ElseIf Me.Cursor = Cursors.SizeNESW OrElse Me.Cursor = Cursors.SizeNWSE OrElse Me.Cursor = Cursors.SizeWE OrElse Me.Cursor = Cursors.SizeNS Then
                        If MouseHelper.getCursorpositionX() < CType(absOffsetX + View.imgOffsetX * View.zoomfactor, Integer) Then vabsOffsetX *= -1
                        If MouseHelper.getCursorpositionY() < CType(absOffsetY + View.imgOffsetY * View.zoomfactor, Integer) Then vabsOffsetY *= -1
                        If Me.Cursor = Cursors.SizeWE Then
                            ImageForm.NUDScale.Value = MathHelper.clipf(ImageForm.NUDScale.Value + ((View.backgroundPicture.Width * View.imgScale + vabsOffsetX) / View.backgroundPicture.Width - View.imgScale) * 2, ImageForm.NUDScale.Minimum, ImageForm.NUDScale.Maximum)
                        Else
                            ImageForm.NUDScale.Value = MathHelper.clipf(ImageForm.NUDScale.Value + ((View.backgroundPicture.Height * View.imgScale + vabsOffsetY) / View.backgroundPicture.Height - View.imgScale) * 2, ImageForm.NUDScale.Minimum, ImageForm.NUDScale.Maximum)
                        End If
                        If Control.ModifierKeys = Keys.Control Then MainState.adjustDirection = -1 Else MainState.adjustDirection = MainState.tempAdjustDirection
                        Select Case MainState.adjustDirection
                            Case 0 : ImageForm.NUDoffsetY.Value = MathHelper.clipf(ImageForm.NUDoffsetY.Value - vabsOffsetY, ImageForm.NUDoffsetY.Minimum, ImageForm.NUDoffsetY.Maximum)
                            Case 1.5 : ImageForm.NUDoffsetX.Value = MathHelper.clipf(ImageForm.NUDoffsetX.Value + View.backgroundPicture.Width / View.backgroundPicture.Height * vabsOffsetY, ImageForm.NUDoffsetX.Minimum, ImageForm.NUDoffsetX.Maximum) : ImageForm.NUDoffsetY.Value = MathHelper.clipf(ImageForm.NUDoffsetY.Value - vabsOffsetY, ImageForm.NUDoffsetY.Minimum, ImageForm.NUDoffsetY.Maximum)
                            Case 3 : ImageForm.NUDoffsetX.Value = MathHelper.clipf(ImageForm.NUDoffsetX.Value + vabsOffsetX, ImageForm.NUDoffsetX.Minimum, ImageForm.NUDoffsetX.Maximum)
                            Case 4.5 : ImageForm.NUDoffsetX.Value = MathHelper.clipf(ImageForm.NUDoffsetX.Value + View.backgroundPicture.Width / View.backgroundPicture.Height * vabsOffsetY, ImageForm.NUDoffsetX.Minimum, ImageForm.NUDoffsetX.Maximum) : ImageForm.NUDoffsetY.Value = MathHelper.clipf(ImageForm.NUDoffsetY.Value + vabsOffsetY, ImageForm.NUDoffsetY.Minimum, ImageForm.NUDoffsetY.Maximum)
                            Case 6 : ImageForm.NUDoffsetY.Value = MathHelper.clipf(ImageForm.NUDoffsetY.Value + vabsOffsetY, ImageForm.NUDoffsetY.Minimum, ImageForm.NUDoffsetY.Maximum)
                            Case 7.5 : ImageForm.NUDoffsetX.Value = MathHelper.clipf(ImageForm.NUDoffsetX.Value - View.backgroundPicture.Width / View.backgroundPicture.Height * vabsOffsetY, ImageForm.NUDoffsetX.Minimum, ImageForm.NUDoffsetX.Maximum) : ImageForm.NUDoffsetY.Value = MathHelper.clipf(ImageForm.NUDoffsetY.Value + vabsOffsetY, ImageForm.NUDoffsetY.Minimum, ImageForm.NUDoffsetY.Maximum)
                            Case 9 : ImageForm.NUDoffsetX.Value = MathHelper.clipf(ImageForm.NUDoffsetX.Value - vabsOffsetX, ImageForm.NUDoffsetX.Minimum, ImageForm.NUDoffsetX.Maximum)
                            Case 10.5 : ImageForm.NUDoffsetX.Value = MathHelper.clipf(ImageForm.NUDoffsetX.Value - View.backgroundPicture.Width / View.backgroundPicture.Height * vabsOffsetY, ImageForm.NUDoffsetX.Minimum, ImageForm.NUDoffsetX.Maximum) : ImageForm.NUDoffsetY.Value = MathHelper.clipf(ImageForm.NUDoffsetY.Value - vabsOffsetY, ImageForm.NUDoffsetY.Minimum, ImageForm.NUDoffsetY.Maximum)
                        End Select
                    End If
                    MainState.isLoading = False
                    Exit Sub
                End If
                If MainState.primitiveMode = PrimitiveModes.Inactive Then
                    If BtnAddVertex.Checked Then
                        LPCFile.Vertices.Add(New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False))
                    End If
                    If BtnAddReferenceLine.Checked Then
                        Select Case MainState.referenceLineMode
                            Case 1
                                MainState.referenceLineMode = 0
                                If LPCFile.templateShape.Count > 0 Then
                                    LPCFile.templateShape.Add(New PointF(Single.Epsilon, 0))
                                End If
                                LPCFile.templateShape.Add(New PointF(MainState.lastPointX, MainState.lastPointY))
                                MainState.lastPointX = Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap
                                MainState.lastPointY = Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap
                                LPCFile.templateShape.Add(New PointF(MainState.lastPointX, MainState.lastPointY))
                                Exit Select
                            Case 0
                                MainState.lastPointX = Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap
                                MainState.lastPointY = Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap
                                MainState.referenceLineMode = 1
                        End Select
                    End If
                    Select Case MainState.editMode
                        Case EditModes.Selection

                            ' CTRL?
                            If Not Control.ModifierKeys = Keys.Control Or MainState.objectToModify = Modified.Primitive Then
                                Helper_2D.clearSelection()
                            End If

                            Dim minX, maxX, minY, maxY, delta As Double
                            minX = getXcoordinate(Math.Max(MainState.klickX, MouseHelper.getCursorpositionX))
                            minY = getYcoordinate(Math.Min(MainState.klickY, MouseHelper.getCursorpositionY))
                            maxX = getXcoordinate(Math.Min(MainState.klickX, MouseHelper.getCursorpositionX))
                            maxY = getYcoordinate(Math.Max(MainState.klickY, MouseHelper.getCursorpositionY))

                            delta = Math.Sqrt((MainState.klickX - MouseHelper.getCursorpositionX()) ^ 2 + (MainState.klickY - MouseHelper.getCursorpositionY()) ^ 2)
                            If delta < 5 Then
                                ' Click on the screen..
                                ' .. to select a triangle.
                                If MainState.objectToModify = Modified.Triangle Then
                                    CSG.beamorig(0) = getXcoordinate(MouseHelper.getCursorpositionX())
                                    CSG.beamorig(1) = getYcoordinate(MouseHelper.getCursorpositionY())
                                    CSG.beamorig(2) = 0
                                    Dim vert0(2) As Decimal
                                    Dim vert1(2) As Decimal
                                    Dim vert2(2) As Decimal
                                    vert0(2) = 0 : vert1(2) = 0 : vert2(2) = 0
                                    For Each tri As Triangle In LPCFile.Triangles
                                        vert0(0) = tri.vertexA.X
                                        vert0(1) = tri.vertexA.Y
                                        vert1(0) = tri.vertexB.X
                                        vert1(1) = tri.vertexB.Y
                                        vert2(0) = tri.vertexC.X
                                        vert2(1) = tri.vertexC.Y
                                        If tri.groupindex < 0 AndAlso PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, CSG.beamdir, vert0, vert1, vert2) Then
                                            If Not View.SelectedTriangles.Contains(tri) Then
                                                tri.selected = True
                                                View.SelectedTriangles.Add(tri)
                                                If Not View.SelectedVertices.Contains(tri.vertexA) AndAlso tri.vertexA.groupindex < 0 Then View.SelectedVertices.Add(tri.vertexA)
                                                If Not View.SelectedVertices.Contains(tri.vertexB) AndAlso tri.vertexB.groupindex < 0 Then View.SelectedVertices.Add(tri.vertexB)
                                                If Not View.SelectedVertices.Contains(tri.vertexC) AndAlso tri.vertexC.groupindex < 0 Then View.SelectedVertices.Add(tri.vertexC)
                                                Exit For
                                            ElseIf Control.ModifierKeys = Keys.Control Then
                                                tri.selected = False
                                                Do
                                                    For Each t As Triangle In tri.vertexA.linkedTriangles
                                                        If t.selected Then Exit Do
                                                    Next
                                                    View.SelectedVertices.Remove(tri.vertexA)
                                                    Exit Do
                                                Loop
                                                Do
                                                    For Each t As Triangle In tri.vertexB.linkedTriangles
                                                        If t.selected Then Exit Do
                                                    Next
                                                    View.SelectedVertices.Remove(tri.vertexB)
                                                    Exit Do
                                                Loop
                                                Do
                                                    For Each t As Triangle In tri.vertexC.linkedTriangles
                                                        If t.selected Then Exit Do
                                                    Next
                                                    View.SelectedVertices.Remove(tri.vertexC)
                                                    Exit Do
                                                Loop
                                                View.SelectedTriangles.Remove(tri)
                                            End If
                                        End If
                                    Next
                                    ' .. to select a vertex.
                                ElseIf MainState.objectToModify = Modified.Vertex AndAlso Not BtnAddVertex.Checked Then
                                    minX -= 9 / View.zoomfactor : minY -= 9 / View.zoomfactor
                                    maxX += 9 / View.zoomfactor : maxY += 9 / View.zoomfactor
                                    Dim previous_vert As Vertex = Nothing
                                    Dim min_vert As Vertex = Nothing
                                    Dim minDelta As Double = Double.MaxValue
                                    Dim min_vertSelected As Vertex = Nothing
                                    For Each vert As Vertex In LPCFile.Vertices
                                        If Not Control.ModifierKeys = Keys.Control Then vert.selected = False
                                        If vert.X >= minX AndAlso vert.X <= maxX AndAlso
                                           vert.Y >= minY AndAlso vert.Y <= maxY Then
                                            delta = Math.Sqrt((vert.X - (minX + maxX) / 2) ^ 2 + (vert.Y - (minY + maxY) / 2) ^ 2)
                                            If delta < minDelta Then
                                                minDelta = delta
                                                If Not vert.selected Then
                                                    min_vert = vert
                                                    If Not previous_vert Is Nothing Then previous_vert.selected = False
                                                    vert.selected = True
                                                    previous_vert = vert
                                                Else
                                                    If Not Control.ModifierKeys = Keys.Control Then vert.selected = False
                                                    min_vertSelected = vert
                                                End If
                                            ElseIf Not Control.ModifierKeys = Keys.Control Then
                                                vert.selected = False
                                            End If
                                        ElseIf Not Control.ModifierKeys = Keys.Control Then
                                            vert.selected = False
                                        End If
                                    Next
                                    If Not min_vertSelected Is Nothing AndAlso Control.ModifierKeys = Keys.Control Then
                                        min_vertSelected.selected = False
                                        For Each t As Triangle In min_vertSelected.linkedTriangles
                                            If t.selected Then
                                                t.selected = False
                                                View.SelectedTriangles.Remove(t)
                                            End If
                                        Next
                                        View.SelectedVertices.Remove(min_vertSelected)
                                    End If
                                    If Not min_vert Is Nothing Then
                                        If (min_vert.groupindex < 0 OrElse BtnAddTriangle.Checked) Then
                                            View.SelectedVertices.Add(min_vert)
                                        Else
                                            MainState.readOnlyVertex = min_vert
                                        End If
                                    End If
                                    ' .. to select helper lines.
                                ElseIf MainState.objectToModify = Modified.HelperLine AndAlso LPCFile.templateShape.Count > 0 Then
                                    ' [H]
                                    If LPCFile.templateShape(0).X = Single.Epsilon Then
                                        ProjectionDataTemplateToolStripMenuItem.PerformClick()
                                    End If

                                    Dim minDist As Double = Double.MaxValue
                                    Dim dist As Double
                                    Dim startVertex As New Vertex(0, 0, False, False)
                                    Dim startPolyVertex As New Vertex(0, 0, False, False)
                                    Dim endVertex As Vertex = Nothing
                                    Dim distVertex As Vertex
                                    Dim finalIndex As Integer

                                    startVertex.X = LPCFile.templateShape(0).X
                                    startVertex.Y = LPCFile.templateShape(0).Y
                                    startPolyVertex.X = LPCFile.templateShape(0).X
                                    startPolyVertex.Y = LPCFile.templateShape(0).Y
                                    Dim start As Integer = 0
                                    Dim finish As Integer = LPCFile.templateShape.Count - 1
                                    For i As Integer = 1 To finish
                                        If LPCFile.templateShape(i).X = Single.Epsilon AndAlso Not endVertex Is Nothing Then
                                            distVertex = CSG.distanceVectorFromVertexToLine(New Vertex(minX, minY, False, False), startPolyVertex, endVertex)
                                            dist = New Vertex(minX, minY, False, False).dist(distVertex)
                                            If dist < minDist Then minDist = dist : finalIndex = i - 1
                                            start = i + 1
                                            If start <= finish Then
                                                startPolyVertex.X = LPCFile.templateShape(start).X
                                                startPolyVertex.Y = LPCFile.templateShape(start).Y
                                                startVertex.X = startPolyVertex.X
                                                startVertex.Y = startPolyVertex.Y
                                                endVertex = Nothing
                                            End If
                                        Else
                                            endVertex = New Vertex(0, 0, False, False)
                                            endVertex.X = LPCFile.templateShape(i).X
                                            endVertex.Y = LPCFile.templateShape(i).Y
                                            distVertex = CSG.distanceVectorFromVertexToLine(New Vertex(minX, minY, False, False), startVertex, endVertex)
                                            dist = New Vertex(minX, minY, False, False).dist(distVertex)
                                            If dist < minDist Then minDist = dist : finalIndex = i
                                            startVertex.X = LPCFile.templateShape(i).X
                                            startVertex.Y = LPCFile.templateShape(i).Y
                                        End If
                                    Next
                                    If Not LPCFile.templateShape(finish).X = Single.Epsilon Then
                                        endVertex.X = LPCFile.templateShape(finish).X
                                        endVertex.Y = LPCFile.templateShape(finish).Y
                                        distVertex = CSG.distanceVectorFromVertexToLine(New Vertex(minX, minY, False, False), startPolyVertex, endVertex)
                                        dist = New Vertex(minX, minY, False, False).dist(distVertex)
                                        If dist < minDist Then minDist = dist : finalIndex = finish
                                    End If

                                    If minDist < 10 / View.zoomfactor Then
                                        LPCFile.helperLineStartIndex = 0
                                        LPCFile.helperLineEndIndex = finish
                                        For start1 As Integer = finalIndex To 0 Step (-1)
                                            If LPCFile.templateShape(start1).X = Single.Epsilon Then LPCFile.helperLineStartIndex = start1 + 1 : Exit For
                                        Next
                                        For ende1 As Integer = finalIndex To finish
                                            If LPCFile.templateShape(ende1).X = Single.Epsilon Then LPCFile.helperLineEndIndex = ende1 - 1 : Exit For
                                        Next
                                    Else
                                        LPCFile.helperLineStartIndex = -1
                                        LPCFile.helperLineEndIndex = -1
                                    End If
                                End If
                                ' Draw a selection rectangle to select one or more..
                                ' .. vertices.
                            ElseIf MainState.objectToModify = Modified.Vertex Then
                                LPCFile.helperLineStartIndex = -1
                                LPCFile.helperLineEndIndex = -1
                                If BtnAddTriangle.Checked Then
                                    For Each vert As Vertex In LPCFile.Vertices
                                        If vert.X >= minX AndAlso vert.X <= maxX AndAlso
                                           vert.Y >= minY AndAlso vert.Y <= maxY Then
                                            If Not vert.selected Then
                                                View.SelectedVertices.Add(vert)
                                                vert.selected = True
                                            Else
                                                View.SelectedVertices.Remove(vert)
                                                vert.selected = False
                                            End If
                                        End If
                                    Next
                                Else
                                    For Each vert As Vertex In LPCFile.Vertices
                                        If Not Control.ModifierKeys = Keys.Control Then vert.selected = False
                                        If vert.X >= minX AndAlso vert.X <= maxX AndAlso
                                           vert.Y >= minY AndAlso vert.Y <= maxY Then
                                            If Not vert.selected Then
                                                If vert.groupindex < 0 Then
                                                    View.SelectedVertices.Add(vert)
                                                End If
                                                vert.selected = True
                                            End If
                                        ElseIf Control.ModifierKeys <> Keys.Control Then
                                            vert.selected = False
                                        End If
                                    Next
                                End If
                                ' ..triangles.
                            ElseIf MainState.objectToModify = Modified.Triangle Then
                                LPCFile.helperLineStartIndex = -1
                                LPCFile.helperLineEndIndex = -1
                                Dim vht As New Dictionary(Of Integer, Object)
                                View.SelectedVertices.Clear()
                                For Each tri As Triangle In LPCFile.Triangles
                                    If tri.groupindex = -1 Then
                                        If tri.vertexA.X >= minX AndAlso tri.vertexA.X <= maxX AndAlso
                                           tri.vertexA.Y >= minY AndAlso tri.vertexA.Y <= maxY AndAlso
                                           tri.vertexB.X >= minX AndAlso tri.vertexB.X <= maxX AndAlso
                                           tri.vertexB.Y >= minY AndAlso tri.vertexB.Y <= maxY AndAlso
                                           tri.vertexC.X >= minX AndAlso tri.vertexC.X <= maxX AndAlso
                                           tri.vertexC.Y >= minY AndAlso tri.vertexC.Y <= maxY Then
                                            If Not tri.selected Then
                                                View.SelectedTriangles.Add(tri)
                                                tri.selected = True
                                            End If
                                        ElseIf Control.ModifierKeys <> Keys.Control Then
                                            tri.selected = False
                                        End If
                                    End If
                                Next
                                For Each tri As Triangle In View.SelectedTriangles
                                    If Not vht.ContainsKey(tri.vertexA.vertexID) AndAlso tri.vertexA.groupindex = -1 Then vht.Add(tri.vertexA.vertexID, Nothing) : View.SelectedVertices.Add(tri.vertexA)
                                    If Not vht.ContainsKey(tri.vertexB.vertexID) AndAlso tri.vertexB.groupindex = -1 Then vht.Add(tri.vertexB.vertexID, Nothing) : View.SelectedVertices.Add(tri.vertexB)
                                    If Not vht.ContainsKey(tri.vertexC.vertexID) AndAlso tri.vertexC.groupindex = -1 Then vht.Add(tri.vertexC.vertexID, Nothing) : View.SelectedVertices.Add(tri.vertexC)
                                Next
                            End If

                            If MainState.objectToModify = Modified.Primitive Then
                                If Not Control.ModifierKeys = Keys.Control Then MainState.lastGroupLayer = 0
                                Dim tempGroupLayer As Integer
                                Dim primitiveNotSelected As Boolean = True
                                CSG.beamorig(0) = getXcoordinate(MouseHelper.getCursorpositionX())
                                CSG.beamorig(1) = getYcoordinate(MouseHelper.getCursorpositionY())
                                CSG.beamorig(2) = 0
                                Dim vert0(2) As Decimal
                                Dim vert1(2) As Decimal
                                Dim vert2(2) As Decimal
                                vert0(2) = 0 : vert1(2) = 0 : vert2(2) = 0
newTry:
                                For Each tri As Triangle In LPCFile.Triangles
                                    vert0(0) = tri.vertexA.X
                                    vert0(1) = tri.vertexA.Y
                                    vert1(0) = tri.vertexB.X
                                    vert1(1) = tri.vertexB.Y
                                    vert2(0) = tri.vertexC.X
                                    vert2(1) = tri.vertexC.Y

                                    If tri.groupindex >= 0 AndAlso PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, CSG.beamdir, vert0, vert1, vert2) AndAlso Not View.SelectedTriangles.Contains(tri) Then
                                        If Control.ModifierKeys = Keys.Control Then
                                            If tempGroupLayer = MainState.lastGroupLayer Then
                                                MainState.lastGroupLayer += 1
                                                selectPrimitive(tri)
                                                primitiveNotSelected = False
                                                Exit For
                                            End If
                                            tempGroupLayer += 1
                                        Else
                                            selectPrimitive(tri)
                                            primitiveNotSelected = False
                                            Exit For
                                        End If
                                    End If
                                Next

                                If primitiveNotSelected Then
                                    If tempGroupLayer > 0 Then MainState.lastGroupLayer = 0 : tempGroupLayer = 0 : GoTo newTry
                                    minX -= 9 / View.zoomfactor : minY -= 9 / View.zoomfactor
                                    maxX += 9 / View.zoomfactor : maxY += 9 / View.zoomfactor
                                    Dim previous_vert As Vertex = Nothing
                                    Dim min_vert As Vertex = Nothing
                                    Dim minDelta As Double = Double.MaxValue
                                    Dim min_vertSelected As Vertex = Nothing
                                    For Each vert As Vertex In LPCFile.Vertices
                                        If vert.groupindex >= 0 Then
                                            If vert.X >= minX AndAlso vert.X <= maxX AndAlso
                                               vert.Y >= minY AndAlso vert.Y <= maxY Then
                                                delta = Math.Sqrt((vert.X - (minX + maxX) / 2) ^ 2 + (vert.Y - (minY + maxY) / 2) ^ 2)
                                                If delta < minDelta Then
                                                    minDelta = delta
                                                    If Not vert.selected Then
                                                        min_vert = vert
                                                        If Not previous_vert Is Nothing Then previous_vert.selected = False
                                                        vert.selected = True
                                                        previous_vert = vert
                                                    Else
                                                        vert.selected = False
                                                        min_vertSelected = vert
                                                    End If
                                                Else
                                                    vert.selected = False
                                                End If
                                            Else
                                                vert.selected = False
                                            End If
                                        End If
                                    Next
                                    If Not min_vert Is Nothing Then
                                        For Each t As Triangle In min_vert.linkedTriangles
                                            If t.groupindex = min_vert.groupindex Then
                                                selectPrimitive(t)
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            ElseIf MainState.objectToModify = Modified.Vertex Then
                                For Each vert As Vertex In LPCFile.Vertices
                                    If vert.selected AndAlso vert.groupindex > -1 Then
                                        vert.selected = False
                                    End If
                                Next
                            End If

                            BtnPipette.Enabled = View.SelectedTriangles.Count = 1 OrElse (MainState.objectToModify = Modified.Primitive AndAlso View.SelectedTriangles.Count >= 1)
                            If BtnAddTriangle.Checked AndAlso MainState.objectToModify = Modified.Vertex Then
                                If delta < 5 AndAlso View.SelectedVertices.Count = 0 AndAlso FastTriangulationIIToolStripMenuItem.Checked Then
                                    LPCFile.Vertices.Add(New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False))
                                    If Not View.SelectedVertices.Contains(ListHelper.LLast(LPCFile.Vertices)) Then
                                        View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                                        ListHelper.LLast(View.SelectedVertices).selected = True
                                    End If
                                End If
                                If MainState.trianglemode > 0 AndAlso View.SelectedVertices.Count > 0 Then
                                    If MainState.trianglemode = 1 Then
                                        If Not View.SelectedVertices.Contains(MainState.temp_vertices(0)) Then View.SelectedVertices.Add(MainState.temp_vertices(0)) : MainState.temp_vertices(0).selected = True
                                    Else
                                        If Not View.SelectedVertices.Contains(MainState.temp_vertices(0)) Then View.SelectedVertices.Add(MainState.temp_vertices(0)) : MainState.temp_vertices(0).selected = True
                                        If Not View.SelectedVertices.Contains(MainState.temp_vertices(1)) Then View.SelectedVertices.Add(MainState.temp_vertices(1)) : MainState.temp_vertices(1).selected = True
                                    End If
                                End If
                                If View.SelectedVertices.Count = 3 Then
                                    If canBuiltTriangle(View.SelectedVertices(0), View.SelectedVertices(1), View.SelectedVertices(2)) Then
                                        LPCFile.Triangles.Add(New Triangle(View.SelectedVertices(0), View.SelectedVertices(1), View.SelectedVertices(2)))
                                        ListHelper.LLast(LPCFile.Triangles).myColour = MainState.lastColour
                                        ListHelper.LLast(LPCFile.Triangles).myColourNumber = MainState.lastColourNumber
                                        View.SelectedVertices(2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        View.SelectedVertices(1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        View.SelectedVertices(0).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        For i As Integer = 0 To 2
                                            View.SelectedVertices(i).selected = False
                                        Next
                                        View.SelectedVertices.Clear()
                                        MainState.intelligentFocusTriangle = ListHelper.LLast(LPCFile.Triangles)
                                    End If
                                ElseIf View.SelectedVertices.Count = 4 Then
                                    If canBuiltQuad(View.SelectedVertices(0), View.SelectedVertices(1), View.SelectedVertices(2), View.SelectedVertices(3)) Then
                                        Dim vcenter As New Vertex(0, 0, False, False)
                                        For i As Integer = 0 To 3
                                            vcenter += View.SelectedVertices(i)
                                        Next
                                        vcenter.X /= 4.0F
                                        vcenter.Y /= 4.0F
                                        For i As Integer = 0 To 3
                                            View.SelectedVertices(i).angleFrom = View.SelectedVertices(i).angle(vcenter)
                                        Next
                                        View.SelectedVertices.Sort()
                                        LPCFile.Triangles.Add(New Triangle(View.SelectedVertices(0), View.SelectedVertices(1), View.SelectedVertices(2)))
                                        ListHelper.LLast(LPCFile.Triangles).myColour = MainState.lastColour
                                        ListHelper.LLast(LPCFile.Triangles).myColourNumber = MainState.lastColourNumber
                                        View.SelectedVertices(0).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        View.SelectedVertices(1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        View.SelectedVertices(2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        LPCFile.Triangles.Add(New Triangle(View.SelectedVertices(2), View.SelectedVertices(3), View.SelectedVertices(0)))
                                        ListHelper.LLast(LPCFile.Triangles).myColour = MainState.lastColour
                                        ListHelper.LLast(LPCFile.Triangles).myColourNumber = MainState.lastColourNumber
                                        View.SelectedVertices(2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        View.SelectedVertices(3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        View.SelectedVertices(0).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                        Dim tri1 As Triangle = ListHelper.LLast(LPCFile.Triangles)
                                        Dim tri2 As Triangle = LPCFile.Triangles(LPCFile.Triangles.Count - 2)
                                        If tri1.maxAngle > Math.PI / 2.0 OrElse tri2.maxAngle > Math.PI / 2.0 Then
                                            View.SelectedTriangles.Add(tri1)
                                            View.SelectedTriangles.Add(tri2)
                                            Dim commonVertices As New List(Of Vertex) With {.Capacity = 5}
                                            Dim otherVertices As New List(Of Vertex) With {.Capacity = 5}
                                            Dim allVertices As New List(Of Vertex) With {.Capacity = 5}
                                            allVertices.Add(View.SelectedTriangles(0).vertexA)
                                            allVertices.Add(View.SelectedTriangles(0).vertexB)
                                            allVertices.Add(View.SelectedTriangles(0).vertexC)
                                            If Not allVertices.Contains(View.SelectedTriangles(1).vertexA) Then allVertices.Add(View.SelectedTriangles(1).vertexA) : otherVertices.Add(View.SelectedTriangles(1).vertexA) Else commonVertices.Add(View.SelectedTriangles(1).vertexA)
                                            If Not allVertices.Contains(View.SelectedTriangles(1).vertexB) Then allVertices.Add(View.SelectedTriangles(1).vertexB) : otherVertices.Add(View.SelectedTriangles(1).vertexB) Else commonVertices.Add(View.SelectedTriangles(1).vertexB)
                                            If Not allVertices.Contains(View.SelectedTriangles(1).vertexC) Then allVertices.Add(View.SelectedTriangles(1).vertexC) : otherVertices.Add(View.SelectedTriangles(1).vertexC) Else commonVertices.Add(View.SelectedTriangles(1).vertexC)
                                            If commonVertices.Count = 2 AndAlso otherVertices.Count = 1 Then
                                                allVertices.Remove(commonVertices(0))
                                                allVertices.Remove(commonVertices(1))
                                                allVertices.Remove(otherVertices(0))
                                                otherVertices.Add(allVertices(0))
                                                For t_i As Integer = 0 To 1
                                                    For t_j As Integer = 0 To 1
                                                        View.SelectedTriangles(t_i).vertexA.linkedTriangles.Remove(View.SelectedTriangles(t_j))
                                                        View.SelectedTriangles(t_i).vertexB.linkedTriangles.Remove(View.SelectedTriangles(t_j))
                                                        View.SelectedTriangles(t_i).vertexC.linkedTriangles.Remove(View.SelectedTriangles(t_j))
                                                    Next
                                                Next
                                                View.SelectedTriangles(0).vertexA = otherVertices(0)
                                                View.SelectedTriangles(0).vertexB = otherVertices(1)
                                                View.SelectedTriangles(0).vertexC = commonVertices(0)
                                                View.SelectedTriangles(1).vertexA = otherVertices(0)
                                                View.SelectedTriangles(1).vertexB = otherVertices(1)
                                                View.SelectedTriangles(1).vertexC = commonVertices(1)
                                                For t_i As Integer = 0 To 1
                                                    View.SelectedTriangles(t_i).vertexA.linkedTriangles.Add(View.SelectedTriangles(t_i))
                                                    View.SelectedTriangles(t_i).vertexB.linkedTriangles.Add(View.SelectedTriangles(t_i))
                                                    View.SelectedTriangles(t_i).vertexC.linkedTriangles.Add(View.SelectedTriangles(t_i))
                                                Next
                                                View.SelectedTriangles.Clear()
                                            End If
                                        End If
                                        For i As Integer = 0 To 3
                                            View.SelectedVertices(i).selected = False
                                        Next
                                        View.SelectedVertices.Clear()
                                        MainState.intelligentFocusTriangle = ListHelper.LLast(LPCFile.Triangles)
                                    End If
                                End If
                                If MainState.trianglemode > 0 AndAlso View.SelectedVertices.Count > 0 Then
                                    If MainState.trianglemode = 1 Then
                                        If View.SelectedVertices.Contains(MainState.temp_vertices(0)) Then View.SelectedVertices.Remove(MainState.temp_vertices(0)) : MainState.temp_vertices(0).selected = False
                                    Else
                                        If View.SelectedVertices.Contains(MainState.temp_vertices(0)) Then View.SelectedVertices.Remove(MainState.temp_vertices(0)) : MainState.temp_vertices(0).selected = False
                                        If View.SelectedVertices.Contains(MainState.temp_vertices(1)) Then View.SelectedVertices.Remove(MainState.temp_vertices(1)) : MainState.temp_vertices(1).selected = False
                                    End If
                                End If
                            End If

                            If View.SelectedVertices.Count = 1 Then
                                If BtnAddTriangle.Checked Then
                                    MainState.lastPointX = View.SelectedVertices(0).X
                                    MainState.lastPointY = View.SelectedVertices(0).Y
                                    If MainState.trianglemode = 2 AndAlso Not MainState.temp_vertices(0).Equals(View.SelectedVertices(0)) AndAlso Not MainState.temp_vertices(1).Equals(View.SelectedVertices(0)) Then
                                        If canBuiltTriangle(MainState.temp_vertices(0), MainState.temp_vertices(1), View.SelectedVertices(0)) Then
                                            LPCFile.Triangles.Add(New Triangle(MainState.temp_vertices(0), MainState.temp_vertices(1), View.SelectedVertices(0)))
                                            ListHelper.LLast(LPCFile.Triangles).myColour = MainState.lastColour
                                            ListHelper.LLast(LPCFile.Triangles).myColourNumber = MainState.lastColourNumber
                                            MainState.temp_vertices(0).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                            MainState.temp_vertices(1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                            View.SelectedVertices(0).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                            MainState.trianglemode = 0
                                        End If

                                        MainState.intelligentFocusTriangle = ListHelper.LLast(LPCFile.Triangles)
                                        MainState.trianglemode = 2
                                    Else
                                        If MainState.trianglemode = 1 AndAlso Not MainState.temp_vertices(0).Equals(View.SelectedVertices(0)) Then
                                            MainState.temp_vertices(1) = View.SelectedVertices(0)
                                            MainState.trianglemode = 2
                                        End If
                                        If MainState.trianglemode = 0 Then
                                            MainState.temp_vertices(0) = View.SelectedVertices(0)
                                            MainState.trianglemode = 1
                                        End If
                                    End If
                                    View.SelectedVertices(0).selected = False
                                    View.SelectedVertices.Clear()
                                End If
                            End If

                            If BtnAddVertex.Checked Then
                                If Not View.SelectedVertices.Contains(ListHelper.LLast(LPCFile.Vertices)) Then
                                    View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                                    ListHelper.LLast(View.SelectedVertices).selected = True
                                End If
                            End If

                            If MainState.objectToModify <> Modified.Vertex Then
                                BtnAddToGroup.Enabled = View.SelectedTriangles.Count > 0 AndAlso View.SelectedVertices.Count > 0 AndAlso Not MainState.objectToModify = Modified.Primitive
                                BtnUngroup.Enabled = View.SelectedTriangles.Count > 0 AndAlso View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify = Modified.Primitive
                            Else
                                BtnAddToGroup.Enabled = View.SelectedTriangles.Count > 0 AndAlso View.SelectedVertices.Count > 0 AndAlso Not MainState.objectToModify = Modified.Primitive
                            End If
                            Exit Select
                        Case EditModes.Translation
                            Dim vabsOffsetX As Integer = Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / View.zoomfactor / View.moveSnap) * View.moveSnap
                            Dim vabsOffsetY As Integer = Fix((MouseHelper.getCursorpositionY() - MainState.klickY) / View.zoomfactor / View.moveSnap) * View.moveSnap
                            If Control.ModifierKeys = Keys.Control Then vabsOffsetX = 0
                            If Control.ModifierKeys = Keys.Shift Then vabsOffsetY = 0
                            Dim newV As Vertex = Nothing
                            Dim mergeToIt As Boolean = False
                            If View.SelectedVertices.Count = 1 AndAlso Control.ModifierKeys = 196608 AndAlso LPCFile.templateProjectionQuads.Count > 0 Then
                                If View.SelectedVertices(0).linkedTriangles.Count > 0 Then
                                    Dim tv As Vertex = New Vertex(View.SelectedVertices(0).X, View.SelectedVertices(0).Y, False, False)
                                    For i = 0 To LPCFile.templateProjectionQuads.Count - 1
                                        If LPCFile.templateProjectionQuads(i).isInQuad(tv) Then
                                            Dim q As ProjectionQuad = LPCFile.templateProjectionQuads(i)
                                            Dim v As Vertex = New Vertex(View.SelectedVertices(0).X / 1000.0, View.SelectedVertices(0).Y / 1000.0, False, False)
                                            ' The type needs to be checked
                                            Dim tmpV As Vertex
                                            If q.isTriangle Then
                                                newV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False), New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False))
                                                Dim dist As Double
                                                Dim mindist As Double = v.dist(v)
                                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False), New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False))
                                                dist = v.dist(tmpV)
                                                If dist < mindist Then newV = tmpV : mindist = dist
                                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False), New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False))
                                                dist = v.dist(tmpV)
                                                If dist < mindist Then newV = tmpV : mindist = dist
                                            Else
                                                newV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False), New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False))
                                                Dim dist As Double
                                                Dim mindist As Double = v.dist(newV)
                                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False), New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False))
                                                dist = v.dist(tmpV)
                                                If dist < mindist Then newV = tmpV : mindist = dist
                                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False), New Vertex(q.inCoords(3, 0), q.inCoords(3, 1), False, False))
                                                dist = v.dist(tmpV)
                                                If dist < mindist Then newV = tmpV : mindist = dist
                                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(3, 0), q.inCoords(3, 1), False, False), New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False))
                                                dist = v.dist(tmpV)
                                                If dist < mindist Then newV = tmpV : mindist = dist
                                            End If
                                            mergeToIt = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                            For Each vert As Vertex In View.SelectedVertices
                                vert.X -= vabsOffsetX
                                vert.Y += vabsOffsetY
                            Next
                            If View.SelectedVertices.Count = 1 AndAlso mergeToIt Then
                                If Not newV Is Nothing Then
                                    View.SelectedVertices(0).X = newV.X * 1000.0
                                    View.SelectedVertices(0).Y = newV.Y * 1000.0
                                End If
                            End If
                            If View.SelectedVertices.Count = 1 AndAlso Control.ModifierKeys = 196608 Then
                                MergeToNearestTemplateLineToolStripMenuItem.PerformClick()
                            End If
                            If View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify = Modified.Primitive Then
                                Dim groupindex As Integer = View.SelectedVertices(0).groupindex
                                LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).translate(-vabsOffsetX, -vabsOffsetY)
                                NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 0)
                                NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 1)
                                NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 3) / 1000D * View.unitFactor
                                NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 0)
                                NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 1)
                                NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 3) / 1000D * View.unitFactor
                                GBMatrix.Visible = True
                            End If
                            MainState.movemode = False
                            If View.CollisionVertices.Count > 0 Then
                                detectCollisions()
                            End If
                            Exit Select
                        Case EditModes.Rotation
                            MainState.rotatemode = False
                            If View.SelectedVertices.Count > 0 Then
                                Dim absOffsetX As Integer = View.offsetX * View.zoomfactor + CType(Me.ClientSize.Width / 2, Integer)
                                Dim absOffsetY As Integer = View.offsetY * View.zoomfactor + CType(Me.ClientSize.Height / 2, Integer)
                                Dim v1 As New Vertex(MouseHelper.getCursorpositionX(), MouseHelper.getCursorpositionY(), False, False)
                                Dim v2 As New Vertex(MainState.klickX, MainState.klickY, False, False)
                                Dim vc As New Vertex(absOffsetX - MainState.temp_center.X * View.zoomfactor, absOffsetY + MainState.temp_center.Y * View.zoomfactor, False, False)
                                Dim angleToRotate As Double = -Fix((vc.angle(v1) - vc.angle(v2)) / (View.rotateSnap * Math.PI / 180)) * (View.rotateSnap * Math.PI / 180)
                                For Each vert As Vertex In View.SelectedVertices
                                    vert.X = MainState.temp_center.X + vert.distanceFrom * Math.Cos(vert.angleFrom + angleToRotate)
                                    vert.Y = MainState.temp_center.Y + vert.distanceFrom * Math.Sin(vert.angleFrom + angleToRotate)
                                    If Double.IsNaN(vert.X) OrElse Double.IsNaN(vert.Y) Then
                                        vert.X = vert.oldX
                                        vert.Y = vert.oldY
                                    End If
                                Next
                                If View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify = Modified.Primitive Then
                                    Dim groupindex As Integer = View.SelectedVertices(0).groupindex
                                    LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).rotate(angleToRotate)
                                    NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 0)
                                    NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 1)
                                    NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 3) / 1000D * View.unitFactor
                                    NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 0)
                                    NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 1)
                                    NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 3) / 1000D * View.unitFactor
                                    GBMatrix.Visible = True
                                End If
                                If View.CollisionVertices.Count > 0 Then
                                    detectCollisions()
                                End If
                            End If
                            Exit Select
                        Case EditModes.Scale
                            MainState.scalemode = False
                            Dim factorToScale As Double = 1 + Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / 100 / View.scaleSnap) * View.scaleSnap
                            If factorToScale < 0.1 Then factorToScale = 0.1
                            If factorToScale > 2 Then factorToScale = 2
                            For Each vert As Vertex In View.SelectedVertices
                                If Not Control.ModifierKeys = Keys.Control Then
                                    vert.X = MainState.temp_center.X + factorToScale * vert.distanceFrom * Math.Cos(vert.angleFrom)
                                Else
                                    vert.X = vert.oldX
                                End If
                                If Not Control.ModifierKeys = Keys.Shift Then
                                    vert.Y = MainState.temp_center.Y + factorToScale * vert.distanceFrom * Math.Sin(vert.angleFrom)
                                Else
                                    vert.Y = vert.oldY
                                End If
                                If Double.IsNaN(vert.X) OrElse Double.IsNaN(vert.Y) Then
                                    vert.X = vert.oldX
                                    vert.Y = vert.oldY
                                End If
                            Next
                            If View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify = Modified.Primitive Then
                                Dim factorToScaleX As Double = 1
                                Dim factorToScaleY As Double = 1
                                If Not Control.ModifierKeys = Keys.Control Then factorToScaleX = factorToScale
                                If Not Control.ModifierKeys = Keys.Shift Then factorToScaleY = factorToScale
                                Dim groupindex As Integer = View.SelectedVertices(0).groupindex
                                LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).scale(factorToScaleX, factorToScaleY)
                                NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 0)
                                NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 1)
                                NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(0, 3) / 1000D * View.unitFactor
                                NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 0)
                                NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 1)
                                NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(groupindex), Integer)).matrix(1, 3) / 1000D * View.unitFactor
                                GBMatrix.Visible = True
                            End If
                            If View.CollisionVertices.Count > 0 Then
                                detectCollisions()
                            End If
                            Exit Select
                    End Select
                Else

                    ' Spline Width & Segments
                    If MainState.primitiveMode = PrimitiveModes.SetSplineWidthNSegments Then
                        MainState.primitiveMode = PrimitiveModes.SetSplineNextPoint
                        ListHelper.LLast(MainState.Splines).persistGeometry()
                        Dim s As New Spline
                        s.startAt = ListHelper.LLast(MainState.Splines).stopAt
                        s.startDirection = ListHelper.LLast(MainState.Splines).stopDirection
                        s.segmentCount = Fix(NUDSplineSegs.Value) - 1
                        MainState.Splines.Add(s)
                    Else
                        ' Spline Next Direction
                        If MainState.primitiveMode = PrimitiveModes.SetSplineNextDirection Then
                            Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX())), Math.Round(getYcoordinate(MouseHelper.getCursorpositionY())), False, False)
                            Dim v2 As Vertex = ListHelper.LLast(MainState.Splines).stopAt
                            ListHelper.LLast(MainState.Splines).stopDirection = v1 - v2
                            MainState.primitiveMode = PrimitiveModes.SetSplineWidthNSegments
                        End If

                        ' Spline Next Point
                        If MainState.primitiveMode = PrimitiveModes.SetSplineNextPoint Then
                            ListHelper.LLast(MainState.Splines).stopAt = New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, True, False)
                            MainState.primitiveMode = PrimitiveModes.SetSplineNextDirection
                        End If
                    End If

                    ' Spline Starting Direction
                    If MainState.primitiveMode = PrimitiveModes.SetSplineStartingDirection Then
                        Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX())), Math.Round(getYcoordinate(MouseHelper.getCursorpositionY())), False, False)
                        Dim v2 As Vertex = ListHelper.LLast(MainState.Splines).startAt
                        ListHelper.LLast(MainState.Splines).startDirection = v1 - v2
                        MainState.primitiveMode = PrimitiveModes.SetSplineNextPoint
                    End If

                    ' Spline Starting Point
                    If MainState.primitiveMode = PrimitiveModes.SetSplineStartingPoint Then
                        Dim s As New Spline
                        s.startAt = New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, True, False)
                        s.segmentCount = Fix(NUDSplineSegs.Value) - 1
                        MainState.Splines.Add(s)
                        MainState.primitiveMode = PrimitiveModes.SetSplineStartingDirection
                    End If

                    If MainState.primitiveMode = PrimitiveModes.CreateTriangleChain Then
                        LPCFile.Vertices.Add(New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, True))
                        LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(LPCFile.Vertices.Count - 1), LPCFile.Vertices(LPCFile.Vertices.Count - 2), LPCFile.Vertices(LPCFile.Vertices.Count - 3)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                        LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                        LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                        LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                        View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                        View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                    End If

skipPrimitiveMode:

                    If MainState.primitiveMode = PrimitiveModes.SetTheFrameSize Then
                        If LDSettings.Editor.radiusInner_circle > 0 AndAlso MainState.primitiveHeight > LDSettings.Editor.radiusInner_circle AndAlso (MainState.primitiveObject = PrimitiveObjects.CircleWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.HollowCircle) Then
                            MainState.primitiveBordersize = LDSettings.Editor.radiusInner_circle / MainState.primitiveHeight
                        Else
                            MainState.primitiveBordersize = Fix(getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) / Math.Sqrt(MainState.primitiveWidth ^ 2 + MainState.primitiveHeight ^ 2)
                        End If
                        If MainState.primitiveBordersize < 0 Then MainState.primitiveBordersize *= -1
                        MathHelper.clip(MainState.primitiveBordersize, 0, 1)
                        If MainState.primitiveObject < 3 Then
                            For Each tri As Triangle In View.SelectedTriangles
                                tri.selected = False
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.selected = False
                            Next
                            View.SelectedTriangles.Clear()
                            View.SelectedVertices.Clear()
                            Dim v11, v21, v31 As Vertex
                            v11 = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y + MainState.primitiveHeight, False)
                            v21 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveHeight + MainState.primitiveWidth, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False)
                            v31 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveHeight - MainState.primitiveWidth, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False)
                            Dim v12, v22, v32 As Vertex
                            v12 = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y + MainState.primitiveHeight * MainState.primitiveBordersize, False)
                            v22 = New Vertex(MainState.primitiveCenter.X + (MainState.primitiveHeight + MainState.primitiveWidth) * MainState.primitiveBordersize, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2) * MainState.primitiveBordersize, False)
                            v32 = New Vertex(MainState.primitiveCenter.X - (MainState.primitiveHeight + MainState.primitiveWidth) * MainState.primitiveBordersize, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2) * MainState.primitiveBordersize, False)
                            LPCFile.Vertices.Add(v11) : LPCFile.Vertices.Add(v12) ' 6 5
                            LPCFile.Vertices.Add(v21) : LPCFile.Vertices.Add(v22) ' 4 3 
                            LPCFile.Vertices.Add(v31) : LPCFile.Vertices.Add(v32) ' 2 1
                            If MainState.primitiveObject = PrimitiveObjects.TriangleWithFrame Then
                                LPCFile.Triangles.Add(New Triangle(v12, v22, v32) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            End If
                            LPCFile.Triangles.Add(New Triangle(v11, v22, v21) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 6).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v11, v22, v12) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 6).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v21, v32, v31) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v21, v32, v22) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v31, v12, v11) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 6).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v31, v12, v32) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 1))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 2))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 3))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 4))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 5))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 6))
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.RectangleWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.HollowRectangle Then
                            For Each tri As Triangle In View.SelectedTriangles
                                tri.selected = False
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.selected = False
                            Next
                            View.SelectedTriangles.Clear()
                            View.SelectedVertices.Clear()
                            Dim v11, v21, v31, v41, v12, v22, v32, v42 As Vertex
                            v11 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False)
                            v21 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False)
                            v31 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False)
                            v41 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False)
                            v12 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y + MainState.primitiveHeight * MainState.primitiveBordersize, False)
                            v22 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y + MainState.primitiveHeight * MainState.primitiveBordersize, False)
                            v32 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y - MainState.primitiveHeight * MainState.primitiveBordersize, False)
                            v42 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y - MainState.primitiveHeight * MainState.primitiveBordersize, False)
                            LPCFile.Vertices.Add(v11) : LPCFile.Vertices.Add(v12) '8 7
                            LPCFile.Vertices.Add(v21) : LPCFile.Vertices.Add(v22) '6 5
                            LPCFile.Vertices.Add(v31) : LPCFile.Vertices.Add(v32) '4 3
                            LPCFile.Vertices.Add(v41) : LPCFile.Vertices.Add(v42) '2 1
                            If MainState.primitiveObject = PrimitiveObjects.RectangleWithFrame Then
                                LPCFile.Triangles.Add(New Triangle(v12, v22, v32) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(LPCFile.Vertices.Count - 7).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                                LPCFile.Triangles.Add(New Triangle(v32, v42, v12) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(LPCFile.Vertices.Count - 7).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            End If
                            LPCFile.Triangles.Add(New Triangle(v11, v22, v21) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 8).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 6).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v11, v22, v12) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 8).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 7).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v21, v32, v31) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 6).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v21, v32, v22) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 6).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 5).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v31, v42, v41) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v31, v42, v32) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v41, v12, v11) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 7).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 8).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v41, v12, v42) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 7).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 1))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 2))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 3))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 4))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 5))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 6))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 7))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 8))
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.CircleWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.HollowCircle OrElse MainState.primitiveObject = PrimitiveObjects.OvalWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.HollowOval Then
                            Dim segments_temp As Integer
                            If MainState.primitiveObject < 10 Then segments_temp = LDSettings.Editor.segments_circle Else segments_temp = LDSettings.Editor.segments_oval
                            For Each tri As Triangle In View.SelectedTriangles
                                tri.selected = False
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.selected = False
                            Next
                            View.SelectedTriangles.Clear()
                            View.SelectedVertices.Clear()
                            Dim vcenter As Vertex = Nothing
                            Dim vinner(segments_temp) As Vertex
                            Dim vouter(segments_temp) As Vertex
                            Dim counter As Integer = 0
                            Dim vinnerIndexFromID As New Hashtable
                            Dim vouterIndexFromID As New Hashtable
                            Dim vcenterIndexFromID As Integer
                            Dim vc As Integer = 0
                            If MainState.primitiveObject = PrimitiveObjects.CircleWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.OvalWithFrame Then
                                vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False)
                                LPCFile.Vertices.Add(vcenter)
                                vcenterIndexFromID = LPCFile.Vertices.Count - 1
                                View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                            End If
                            For s As Integer = 0 To segments_temp
                                Dim a As Double = (Math.PI * 2.0 / CDbl(segments_temp)) * CDbl(s)
                                vinner(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight * MainState.primitiveBordersize, False)
                                LPCFile.Vertices.Add(vinner(counter))
                                vc += 1
                                vinnerIndexFromID.Add(vinner(counter).vertexID, LPCFile.Vertices.Count - 1)
                                counter += 1
                            Next
                            If MainState.primitiveObject = PrimitiveObjects.CircleWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.OvalWithFrame Then
                                LPCFile.Triangles.Add(New Triangle(vcenter, vinner(segments_temp - 1), vinner(0)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(vcenterIndexFromID).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vinnerIndexFromID(vinner(segments_temp - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vinnerIndexFromID(vinner(0).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                                For counter = 1 To segments_temp - 1
                                    LPCFile.Triangles.Add(New Triangle(vcenter, vinner(counter), vinner(counter - 1)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                    LPCFile.Vertices(vcenterIndexFromID).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(vinnerIndexFromID(vinner(counter).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(vinnerIndexFromID(vinner(counter - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                                Next
                            End If
                            counter = 0
                            For s As Integer = 0 To segments_temp
                                Dim a As Double = (Math.PI * 2.0 / CDbl(segments_temp)) * CDbl(s)
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveWidth, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False)
                                LPCFile.Vertices.Add(vouter(counter))
                                vc += 1
                                vouterIndexFromID.Add(vouter(counter).vertexID, LPCFile.Vertices.Count - 1)
                                counter += 1
                            Next
                            LPCFile.Triangles.Add(New Triangle(vouter(segments_temp - 1), vinner(segments_temp - 1), vinner(0)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(vouterIndexFromID(vouter(segments_temp - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vinnerIndexFromID(vinner(segments_temp - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vinnerIndexFromID(vinner(0).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(vouter(0), vouter(segments_temp - 1), vinner(0)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(vouterIndexFromID(vouter(0).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vouterIndexFromID(vouter(segments_temp - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vinnerIndexFromID(vinner(0).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            For counter = 1 To segments_temp - 1
                                LPCFile.Triangles.Add(New Triangle(vouter(counter - 1), vinner(counter - 1), vinner(counter)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(vouterIndexFromID(vouter(counter - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vinnerIndexFromID(vinner(counter - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vinnerIndexFromID(vinner(counter).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                                LPCFile.Triangles.Add(New Triangle(vouter(counter), vouter(counter - 1), vinner(counter)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(vouterIndexFromID(vouter(counter).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vouterIndexFromID(vouter(counter - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vinnerIndexFromID(vinner(counter).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            Next
                            Dim vertsToDelete As New List(Of Vertex)
                            For counter = 1 To vc
                                If LPCFile.Vertices(LPCFile.Vertices.Count - counter).linkedTriangles.Count = 0 Then
                                    vertsToDelete.Add(LPCFile.Vertices(LPCFile.Vertices.Count - counter))
                                    Continue For
                                End If
                                View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - counter))
                            Next
                            For Each v As Vertex In vertsToDelete
                                LPCFile.Vertices.Remove(v)
                            Next
                        End If
                        abortPrimitiveMode()
                    End If
                    If MainState.primitiveMode = PrimitiveModes.SetTheWidth Then
                        If LDSettings.Editor.radius_oval_x > 0 AndAlso (MainState.primitiveObject = PrimitiveObjects.OvalWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.HollowOval) Then
                            MainState.primitiveWidth = LDSettings.Editor.radius_oval_x
                        Else
                            MainState.primitiveWidth = Fix(getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X)
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.SolidTriangle Then
                            For Each tri As Triangle In View.SelectedTriangles
                                tri.selected = False
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.selected = False
                            Next
                            View.SelectedTriangles.Clear()
                            View.SelectedVertices.Clear()
                            Dim v1, v2, v3 As Vertex
                            v1 = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y + MainState.primitiveHeight, True)
                            v2 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveHeight, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), True)
                            v3 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveHeight, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), True)
                            LPCFile.Vertices.Add(v1) : LPCFile.Vertices.Add(v2) : LPCFile.Vertices.Add(v3)
                            LPCFile.Triangles.Add(New Triangle(v1, v2, v3) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            ListHelper.LLast(LPCFile.Triangles).vertexB.X += MainState.primitiveWidth
                            ListHelper.LLast(LPCFile.Triangles).vertexC.X -= MainState.primitiveWidth
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 1))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 2))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 3))
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.SolidRectangle Then
                            For Each tri As Triangle In View.SelectedTriangles
                                tri.selected = False
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.selected = False
                            Next
                            View.SelectedTriangles.Clear()
                            View.SelectedVertices.Clear()
                            Dim v1, v2, v3, v4 As Vertex
                            v1 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False)
                            v2 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False)
                            v3 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False)
                            v4 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False)
                            LPCFile.Vertices.Add(v1) : LPCFile.Vertices.Add(v2) : LPCFile.Vertices.Add(v3) : LPCFile.Vertices.Add(v4)
                            LPCFile.Triangles.Add(New Triangle(v1, v2, v3) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            LPCFile.Triangles.Add(New Triangle(v3, v4, v1) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 1))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 2))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 3))
                            View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - 4))
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.SolidOval Then
                            For Each tri As Triangle In View.SelectedTriangles
                                tri.selected = False
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.selected = False
                            Next
                            View.SelectedTriangles.Clear()
                            View.SelectedVertices.Clear()
                            If LDSettings.Editor.radius_oval_x > 0 Then
                                MainState.primitiveWidth = LDSettings.Editor.radius_oval_x
                            Else
                                MainState.primitiveWidth = Fix(Math.Sqrt((getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y) ^ 2 + (getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) ^ 2))
                            End If
                            Dim vcenter As Vertex
                            Dim vouter(LDSettings.Editor.segments_oval) As Vertex
                            Dim counter As Integer = 0
                            Dim vouterIndexFromID As New Hashtable
                            Dim vcenterIndexFromID As Integer
                            Dim vc As Integer = 0
                            vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False)
                            LPCFile.Vertices.Add(vcenter)
                            vcenterIndexFromID = LPCFile.Vertices.Count - 1
                            View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                            For a As Double = 0 To Math.PI * 2 Step Math.PI * 2 / LDSettings.Editor.segments_oval
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveWidth, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False)
                                LPCFile.Vertices.Add(vouter(counter))
                                vc += 1
                                vouterIndexFromID.Add(vouter(counter).vertexID, LPCFile.Vertices.Count - 1)
                                counter += 1
                            Next
                            LPCFile.Triangles.Add(New Triangle(vcenter, vouter(LDSettings.Editor.segments_oval - 1), vouter(0)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(vcenterIndexFromID).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vouterIndexFromID(vouter(LDSettings.Editor.segments_oval - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vouterIndexFromID(vouter(0).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            For counter = 1 To LDSettings.Editor.segments_oval - 1
                                LPCFile.Triangles.Add(New Triangle(vcenter, vouter(counter), vouter(counter - 1)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(vcenterIndexFromID).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vouterIndexFromID(vouter(counter).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vouterIndexFromID(vouter(counter - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            Next
                            Dim vertsToDelete As New List(Of Vertex)
                            For counter = 1 To vc
                                If LPCFile.Vertices(LPCFile.Vertices.Count - counter).linkedTriangles.Count = 0 Then
                                    vertsToDelete.Add(LPCFile.Vertices(LPCFile.Vertices.Count - counter))
                                    Continue For
                                End If
                                View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - counter))
                            Next
                            For Each v As Vertex In vertsToDelete
                                LPCFile.Vertices.Remove(v)
                            Next
                        End If
                        If MainState.primitiveObject <> 0 AndAlso MainState.primitiveObject <> 3 AndAlso MainState.primitiveObject <> 9 Then
                            MainState.primitiveMode = PrimitiveModes.SetTheFrameSize
                        Else
                            abortPrimitiveMode()
                        End If
                    End If
                    If MainState.primitiveMode = PrimitiveModes.SetTheHeight Then
                        MainState.primitiveHeight = Fix(getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y)
                        If MainState.primitiveObject < 6 Then
                            MainState.primitiveMode = PrimitiveModes.SetTheWidth
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.SolidCircle Then
                            For Each tri As Triangle In View.SelectedTriangles
                                tri.selected = False
                            Next
                            For Each vert As Vertex In View.SelectedVertices
                                vert.selected = False
                            Next
                            View.SelectedTriangles.Clear()
                            View.SelectedVertices.Clear()
                            If LDSettings.Editor.radius_circle > 0 Then
                                MainState.primitiveHeight = LDSettings.Editor.radius_circle
                            Else
                                MainState.primitiveHeight = Fix(Math.Sqrt((getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y) ^ 2 + (getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) ^ 2))
                            End If
                            Dim vcenter As Vertex
                            Dim vouter(LDSettings.Editor.segments_circle) As Vertex
                            Dim counter As Integer = 0
                            Dim vouterIndexFromID As New Hashtable
                            Dim vcenterIndexFromID As Integer
                            Dim vc As Integer = 0
                            vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False)
                            LPCFile.Vertices.Add(vcenter)
                            vcenterIndexFromID = LPCFile.Vertices.Count - 1
                            View.SelectedVertices.Add(ListHelper.LLast(LPCFile.Vertices))
                            For a As Double = 0 To Math.PI * 2 Step Math.PI * 2 / LDSettings.Editor.segments_circle
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveHeight, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False)
                                LPCFile.Vertices.Add(vouter(counter))
                                vc += 1
                                vouterIndexFromID.Add(vouter(counter).vertexID, LPCFile.Vertices.Count - 1)
                                counter += 1
                            Next
                            LPCFile.Triangles.Add(New Triangle(vcenter, vouter(LDSettings.Editor.segments_circle - 1), vouter(0)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                            LPCFile.Vertices(vcenterIndexFromID).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vouterIndexFromID(vouter(LDSettings.Editor.segments_circle - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(vouterIndexFromID(vouter(0).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            For counter = 1 To LDSettings.Editor.segments_circle - 1
                                LPCFile.Triangles.Add(New Triangle(vcenter, vouter(counter), vouter(counter - 1)) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber})
                                LPCFile.Vertices(vcenterIndexFromID).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vouterIndexFromID(vouter(counter).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                LPCFile.Vertices(vouterIndexFromID(vouter(counter - 1).vertexID)).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                View.SelectedTriangles.Add(ListHelper.LLast(LPCFile.Triangles)) : ListHelper.LLast(LPCFile.Triangles).selected = True
                            Next
                            Dim vertsToDelete As New List(Of Vertex)
                            For counter = 1 To vc
                                If LPCFile.Vertices(LPCFile.Vertices.Count - counter).linkedTriangles.Count = 0 Then
                                    vertsToDelete.Add(LPCFile.Vertices(LPCFile.Vertices.Count - counter))
                                    Continue For
                                End If
                                View.SelectedVertices.Add(LPCFile.Vertices(LPCFile.Vertices.Count - counter))
                            Next
                            For Each v As Vertex In vertsToDelete
                                LPCFile.Vertices.Remove(v)
                            Next
                            abortPrimitiveMode()
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.CircleWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.HollowCircle Then
                            If LDSettings.Editor.radius_circle > 0 Then
                                MainState.primitiveHeight = LDSettings.Editor.radius_circle
                            Else
                                MainState.primitiveHeight = Fix(Math.Sqrt((getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y) ^ 2 + (getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) ^ 2))
                            End If
                            MainState.primitiveWidth = MainState.primitiveHeight
                            MainState.primitiveMode = PrimitiveModes.SetTheFrameSize
                        End If
                        If MainState.primitiveObject = PrimitiveObjects.SolidOval OrElse MainState.primitiveObject = PrimitiveObjects.OvalWithFrame OrElse MainState.primitiveObject = PrimitiveObjects.HollowOval Then
                            If LDSettings.Editor.radius_oval_y > 0 Then
                                MainState.primitiveHeight = LDSettings.Editor.radius_oval_y
                            Else
                                MainState.primitiveHeight = Fix(Math.Sqrt((getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y) ^ 2 + (getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) ^ 2))
                            End If
                            MainState.primitiveMode = PrimitiveModes.SetTheWidth
                        End If
                    End If
                    If MainState.primitiveMode = PrimitiveModes.SetTheOrigin Then
                        MainState.primitiveCenter = New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                        If MainState.primitiveNewName = "" Then
                            MainState.primitiveMode = PrimitiveModes.SetTheHeight
                        Else
                            abortPrimitiveMode()
                            inlinePrimitive(1, "16",
                                1, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 1, 0,
                                1, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 1, 0,
                                MainState.primitiveNewName, New List(Of String))
                            MainState.primitiveNewName = ""
                            PrimitiveModeToolStripMenuItem.PerformClick()
                        End If

                    End If
                End If
                ' Automatic primitive routines
                If MainState.primitiveMode = PrimitiveModes.SetTheHeight Then
                    If LDSettings.Editor.radius_circle > 0 AndAlso MainState.primitiveObject >= PrimitiveObjects.SolidCircle AndAlso MainState.primitiveObject <= PrimitiveObjects.HollowCircle Then
                        GoTo skipPrimitiveMode
                    End If
                    If LDSettings.Editor.radius_oval_y > 0 AndAlso MainState.primitiveObject >= PrimitiveObjects.SolidOval AndAlso MainState.primitiveObject <= PrimitiveObjects.HollowOval Then
                        GoTo skipPrimitiveMode
                    End If
                ElseIf MainState.primitiveMode = PrimitiveModes.SetTheWidth Then
                    If LDSettings.Editor.radius_oval_x > 0 AndAlso MainState.primitiveObject >= PrimitiveObjects.SolidOval AndAlso MainState.primitiveObject <= PrimitiveObjects.HollowOval Then
                        GoTo skipPrimitiveMode
                    End If
                ElseIf MainState.primitiveMode = PrimitiveModes.SetTheFrameSize Then
                    If LDSettings.Editor.radiusInner_circle > 0 AndAlso MainState.primitiveHeight > LDSettings.Editor.radiusInner_circle AndAlso MainState.primitiveObject >= PrimitiveObjects.SolidCircle AndAlso MainState.primitiveObject <= PrimitiveObjects.HollowCircle Then
                        GoTo skipPrimitiveMode
                    End If
                End If
                GBMatrix.Visible = MainState.objectToModify = Modified.Primitive AndAlso View.SelectedVertices.Count > 2 AndAlso View.SelectedVertices(0).groupindex >= 0 AndAlso Not BtnAddTriangle.Checked
                GBVertex.Visible = (View.SelectedVertices.Count = 1 AndAlso Not BtnAddTriangle.Checked) OrElse Not MainState.readOnlyVertex Is Nothing
                If GBVertex.Visible Then
                    If MainState.readOnlyVertex Is Nothing Then
                        NUDVertX.Value = View.SelectedVertices(0).X * View.unitFactor / 1000
                        NUDVertY.Value = View.SelectedVertices(0).Y * View.unitFactor / 1000
                        NUDVertX.ReadOnly = False
                        NUDVertY.ReadOnly = False
                    Else
                        NUDVertX.Value = MainState.readOnlyVertex.X * View.unitFactor / 1000
                        NUDVertY.Value = MainState.readOnlyVertex.Y * View.unitFactor / 1000
                        NUDVertX.ReadOnly = True
                        NUDVertY.ReadOnly = True
                    End If
                End If
            ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                If MainState.trianglemode = 0 AndAlso Math.Abs(MouseHelper.getCursorpositionX() - MainState.klickX) + Math.Abs(MouseHelper.getCursorpositionY() - MainState.klickY) < 2 Then
                    Me.ContextMenuStrip = CMS
                    Me.ContextMenuStrip.Show(Me, MouseHelper.getCursorpositionX(), MouseHelper.getCursorpositionY())
                End If
                MainState.doCameraMove = False
                If Not Control.ModifierKeys = Keys.Control Then
                    Helper_2D.stopTriangulation()
                End If
            End If
            UndoRedoHelper.addHistory()
        End If
        Me.Refresh()
    End Sub

    Private Function compareArray(ByVal a1 As Double(,), ByVal a2 As Double(,)) As Boolean
        For x As Integer = 0 To 3
            For y As Integer = 0 To 3
                If a1(x, y) <> a2(x, y) Then
                    Return False
                End If
            Next
        Next
        Return True
    End Function

    Private Sub selectPrimitive(ByRef tri As Triangle)
        selectGroupDirect(tri.groupindex)
        NUDM11.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).matrix(0, 0)
        NUDM12.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).matrix(0, 1)
        NUDM13.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).matrix(0, 3) / 1000D * View.unitFactor
        NUDM21.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).matrix(1, 0)
        NUDM22.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).matrix(1, 1)
        NUDM23.Value = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).matrix(1, 3) / 1000D * View.unitFactor
        Dim p As Primitive = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer))
        Dim m As New Matrix3D
        For y As Integer = 0 To 3
            For x As Integer = 0 To 3
                m.m(y, x) = p.matrix(y, x)
            Next
        Next
        Dim v As New Vertex3D(0, 0, 1)
        v = m * v
        If LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).primitiveName Like "subfile*" Then v.Z *= -1
        BtnInvert.Enabled = Not LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(tri.groupindex), Integer)).primitiveName Like "s\*"
        CBProject.Enabled = Not BtnInvert.Enabled
        If Not BtnInvert.Enabled Then
            BtnInvert.BackColor = Color.Transparent
            ' YZ mit -X (Right) [OK]
            Dim cmatrix1(,) As Double = {{0.0, 0.0, -1.0, 0.0},
                                         {0.0, 1.0, 0.0, 0.0},
                                         {1.0, 0.0, 0.0, 0.0},
                                         {0.0, 0.0, 0.0, 1.0}}
            If compareArray(p.matrixR, cmatrix1) Then CBProject.SelectedItem = I18N.trl8(I18N.lk.Right)
            ' XZ mit -Y (Top) [OK]
            Dim cmatrix2(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                        {0.0, 0.0, -1.0, 0.0},
                                        {0.0, 1.0, 0.0, 0.0},
                                        {0.0, 0.0, 0.0, 1.0}}
            If compareArray(p.matrixR, cmatrix2) Then CBProject.SelectedItem = I18N.trl8(I18N.lk.Top)
            ' XY mit -Z (Front) [OK] 
            Dim cmatrix3(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                        {0.0, 1.0, 0.0, 0.0},
                                        {0.0, 0.0, 1.0, 0.0},
                                        {0.0, 0.0, 0.0, 1.0}}
            If compareArray(p.matrixR, cmatrix3) Then CBProject.SelectedItem = I18N.trl8(I18N.lk.Front)
            ' YZ mit +X (Left) [OK]
            Dim cmatrix4(,) As Double = {{0.0, 0.0, 1.0, 0.0},
                                         {0.0, 1.0, 0.0, 0.0},
                                         {-1.0, 0.0, 0.0, 0.0},
                                         {0.0, 0.0, 0.0, 1.0}}
            If compareArray(p.matrixR, cmatrix4) Then CBProject.SelectedItem = I18N.trl8(I18N.lk.Left)
            ' XZ mit -Y (Bottom) [OK]
            Dim cmatrix5(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                        {0.0, 0.0, 1.0, 0.0},
                                        {0.0, -1.0, 0.0, 0.0},
                                        {0.0, 0.0, 0.0, 1.0}}
            If compareArray(p.matrixR, cmatrix5) Then CBProject.SelectedItem = I18N.trl8(I18N.lk.Bottom)
            ' XY mit +Z (Back) [OK]
            Dim cmatrix6(,) As Double = {{-1.0, 0.0, 0.0, 0.0},
                                        {0.0, 1.0, 0.0, 0.0},
                                        {0.0, 0.0, -1.0, 0.0},
                                        {0.0, 0.0, 0.0, 1.0}}
            If compareArray(p.matrixR, cmatrix6) Then CBProject.SelectedItem = I18N.trl8(I18N.lk.Back)

        ElseIf v.Z >= 0 Then
            CBProject.SelectedItem = Nothing
            BtnInvert.BackColor = Color.Green
        Else
            CBProject.SelectedItem = Nothing
            BtnInvert.BackColor = Color.Red
        End If
        GBMatrix.Visible = True
    End Sub

#End Region
#Region "Mathematical Functions"

    Public Function getXcoordinate(ByVal relX As Integer) As Double
        Return -((relX - Me.ClientSize.Width / 2) / View.zoomfactor - View.offsetX)
    End Function

    Public Function getYcoordinate(ByVal relY As Integer) As Double
        Return -((-relY + Me.ClientSize.Height / 2) / View.zoomfactor + View.offsetY)
    End Function


    Public Function getXcoordinateD(ByVal relX As Double) As Double
        Return -((relX - Me.ClientSize.Width / 2) / View.zoomfactor - View.offsetX)
    End Function

    Public Function getYcoordinateD(ByVal relY As Double) As Double
        Return -((-relY + Me.ClientSize.Height / 2) / View.zoomfactor + View.offsetY)
    End Function

#End Region
#Region "View Properties"
    Private Sub ShowGridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowGridToolStripMenuItem.Click
        View.showGrid = ShowGridToolStripMenuItem.Checked
        Me.Refresh()
    End Sub

    Private Sub ImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageToolStripMenuItem.Click
        If ImageToolStripMenuItem.Checked Then
            Me.AddOwnedForm(ImageForm)
            ImageForm.Show()
            Me.Focus()
            Me.SBZoom.Focus()
        Else
            ImageForm.Close()
        End If
        Me.Refresh()
    End Sub

    Private Sub ViewPrefsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewPrefsToolStripMenuItem.Click
        If ViewPrefsToolStripMenuItem.Checked Then
            Me.AddOwnedForm(PreferencesForm)
            PreferencesForm.Show()
            Me.Focus()
            Me.SBZoom.Focus()
        Else
            PreferencesForm.Close()
        End If
        Me.Refresh()
    End Sub

    Private Sub SBZoom_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs, Optional ByVal refresh As Boolean = True) Handles SBZoom.Scroll
        If Not MainState.isLoading Then
            View.zoomlevel += e.NewValue - e.OldValue
            MathHelper.clip(View.zoomlevel, -30, 90)
            If View.zoomlevel > -10 Then
                View.zoomfactor = 1 + View.zoomlevel * 0.1F
            ElseIf View.zoomlevel > -20 Then
                View.zoomfactor = 0.2F + View.zoomlevel * 0.01F
            Else
                View.zoomfactor = 0.03F + View.zoomlevel * 0.001F
            End If
            If View.zoomlevel = -30 Then View.zoomfactor = 0.001
            LblZoom.Text = I18N.trl8(I18N.lk.ZoomParam) & " " & CType(View.zoomfactor * 10000, Integer) / 100 & "%"
            LblCoords.Text = Math.Round(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000, 3) & " | " & Math.Round(Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000, 3) & " " & translateUnit(View.unit)
            e.NewValue = 0
            If refresh Then Me.Refresh()
        Else
            e.NewValue = e.OldValue
        End If
    End Sub
#End Region
#Region "AddFunctions"
    Private Sub BtnAddVertex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddVertex.Click
        If Not BtnAddVertex.Checked Then Helper_2D.stopTriangulation()
        MainState.keylock = True
        BtnAddTriangle.Checked = False
        CMSAddTriangle.Checked = False
        BtnAddReferenceLine.Checked = False
        CMSAddVertex.Checked = Not CMSAddVertex.Checked
        BtnMove.Enabled = Not BtnAddVertex.Checked
        BtnRotate.Enabled = Not BtnAddVertex.Checked
        BtnScale.Enabled = Not BtnAddVertex.Checked
        CMSMove.Enabled = Not BtnAddVertex.Checked
        CMSRotate.Enabled = Not BtnAddVertex.Checked
        CMSScale.Enabled = Not BtnAddVertex.Checked
        disableTextboxedit(Not BtnAddVertex.Checked)
        DeleteToolStripMenuItem.Enabled = True
        BtnMirror.Enabled = True
        BtnSelect.PerformClick()
        MainState.keylock = False
        If Not BtnAddVertex.Checked Then
            BtnMode.Enabled = True : VerticesModeToolStripMenuItem.Enabled = True : TrianglesModeToolStripMenuItem.Enabled = True : PrimitiveModeToolStripMenuItem.Enabled = True
            Select Case MainState.lastAction
                Case 0 : BtnSelect.PerformClick()
                Case 1 : BtnMove.PerformClick()
                Case 2 : BtnRotate.PerformClick()
                Case 3 : BtnScale.PerformClick()
            End Select
        ElseIf LDSettings.Editor.lockModeChange Then
            VerticesModeToolStripMenuItem.PerformClick()
            BtnMode.Enabled = False : VerticesModeToolStripMenuItem.Enabled = False : TrianglesModeToolStripMenuItem.Enabled = False : PrimitiveModeToolStripMenuItem.Enabled = False
        End If
        Me.Refresh()
    End Sub

    Private Sub BtnAddReferenceLline_Click(sender As Object, e As EventArgs) Handles BtnAddReferenceLine.Click
        If Not BtnAddReferenceLine.Checked Then Helper_2D.stopTriangulation()
        MainState.keylock = True
        BtnAddTriangle.Checked = False
        BtnAddVertex.Checked = False
        CMSAddTriangle.Checked = False
        CMSAddVertex.Checked = False
        BtnMove.Enabled = Not BtnAddReferenceLine.Checked
        BtnRotate.Enabled = Not BtnAddReferenceLine.Checked
        BtnScale.Enabled = Not BtnAddReferenceLine.Checked
        CMSMove.Enabled = Not BtnAddReferenceLine.Checked
        CMSRotate.Enabled = Not BtnAddReferenceLine.Checked
        CMSScale.Enabled = Not BtnAddReferenceLine.Checked
        disableTextboxedit(Not BtnAddReferenceLine.Checked)
        DeleteToolStripMenuItem.Enabled = True
        BtnMirror.Enabled = True
        BtnSelect.PerformClick()
        MainState.keylock = False
        If Not BtnAddReferenceLine.Checked Then
            BtnMode.Enabled = True : VerticesModeToolStripMenuItem.Enabled = True : TrianglesModeToolStripMenuItem.Enabled = True : PrimitiveModeToolStripMenuItem.Enabled = True
            Select Case MainState.lastAction
                Case 0 : BtnSelect.PerformClick()
                Case 1 : BtnMove.PerformClick()
                Case 2 : BtnRotate.PerformClick()
                Case 3 : BtnScale.PerformClick()
            End Select
        ElseIf LDSettings.Editor.lockModeChange Then
            VerticesModeToolStripMenuItem.PerformClick()
            BtnMode.Enabled = False : VerticesModeToolStripMenuItem.Enabled = False : TrianglesModeToolStripMenuItem.Enabled = False : PrimitiveModeToolStripMenuItem.Enabled = False
        End If
        Me.Refresh()
    End Sub

    Private Sub BtnAddTriangle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddTriangle.Click
        If Not BtnAddTriangle.Checked Then Helper_2D.stopTriangulation()
        Helper_2D.clearSelection()
        MainState.keylock = True
        BtnAddVertex.Checked = False
        CMSAddVertex.Checked = False
        BtnAddReferenceLine.Checked = False
        GBVertex.Visible = False
        CMSAddTriangle.Checked = Not CMSAddTriangle.Checked
        BtnMove.Enabled = Not BtnAddTriangle.Checked
        BtnRotate.Enabled = Not BtnAddTriangle.Checked
        BtnScale.Enabled = Not BtnAddTriangle.Checked
        CMSMove.Enabled = Not BtnAddTriangle.Checked
        CMSRotate.Enabled = Not BtnAddTriangle.Checked
        CMSScale.Enabled = Not BtnAddTriangle.Checked
        BtnMirror.Enabled = Not BtnAddTriangle.Checked
        disableTextboxedit(Not BtnAddTriangle.Checked)
        DeleteToolStripMenuItem.Enabled = True
        BtnSelect.PerformClick()
        VerticesModeToolStripMenuItem.PerformClick()
        MainState.keylock = False
        If Not BtnAddTriangle.Checked Then
            BtnMode.Enabled = True : VerticesModeToolStripMenuItem.Enabled = True : TrianglesModeToolStripMenuItem.Enabled = True : PrimitiveModeToolStripMenuItem.Enabled = True
            Select Case MainState.lastAction
                Case 0 : BtnSelect.PerformClick()
                Case 1 : BtnMove.PerformClick()
                Case 2 : BtnRotate.PerformClick()
                Case 3 : BtnScale.PerformClick()
            End Select
            Select Case MainState.lastMode
                Case 0 : VerticesModeToolStripMenuItem.PerformClick()
                Case 1 : TrianglesModeToolStripMenuItem.PerformClick()
                Case 2 : PrimitiveModeToolStripMenuItem.PerformClick()
                Case 3 : ReferenceLineModeToolStripMenuItem.PerformClick()
            End Select
        ElseIf LDSettings.Editor.lockModeChange Then
            BtnMode.Text = I18N.trl8(I18N.lk.TriangleMode)
            BtnMode.Enabled = False : VerticesModeToolStripMenuItem.Enabled = False : TrianglesModeToolStripMenuItem.Enabled = False : PrimitiveModeToolStripMenuItem.Enabled = False
        End If
        Me.Refresh()
    End Sub
#End Region
#Region "Modi"
    Private Sub BtnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelect.Click
        If Not MainState.keylock Then
            If BtnAddVertex.Checked Then BtnAddVertex.PerformClick()
            If BtnAddTriangle.Checked Then BtnAddTriangle.PerformClick()
            If BtnAddReferenceLine.Checked Then BtnAddReferenceLine.PerformClick()
            MainState.lastAction = 0
        End If
        BtnSelect.Checked = True
        BtnMove.Checked = False
        BtnRotate.Checked = False
        BtnScale.Checked = False
        MainState.editMode = EditModes.Selection
    End Sub

    Private Sub BtnMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMove.Click
        If Not MainState.keylock Then MainState.lastAction = 1
        BtnSelect.Checked = False
        BtnMove.Checked = True
        BtnRotate.Checked = False
        BtnScale.Checked = False
        MainState.editMode = EditModes.Translation
    End Sub

    Private Sub BtnRotate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRotate.Click
        If Not MainState.keylock Then MainState.lastAction = 2
        BtnSelect.Checked = False
        BtnMove.Checked = False
        BtnRotate.Checked = True
        BtnScale.Checked = False
        MainState.editMode = EditModes.Rotation
    End Sub

    Private Sub BtnScale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnScale.Click
        If Not MainState.keylock Then MainState.lastAction = 3
        BtnSelect.Checked = False
        BtnMove.Checked = False
        BtnRotate.Checked = False
        BtnScale.Checked = True
        MainState.editMode = EditModes.Scale
    End Sub

    Private Sub VerticesModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerticesModeToolStripMenuItem.Click
        LPCFile.helperLineStartIndex = -1
        LPCFile.helperLineEndIndex = -1
        GBMatrix.Visible = False
        MirrorXLeftToolStripMenuItem.Enabled = True
        MirrorXRightToolStripMenuItem.Enabled = True
        MirrorYTopToolStripMenuItem.Enabled = True
        MirrorYBottomToolStripMenuItem.Enabled = True
        BtnRound.Enabled = True
        If Not MainState.keylock Then MainState.lastMode = 0
        BtnAddToGroup.Enabled = False
        BtnUngroup.Enabled = False
        If BtnMode.Text = PrimitiveModeToolStripMenuItem.Text Then
            Helper_2D.clearSelection()
        End If
        BtnMode.Image = VerticesModeToolStripMenuItem.Image
        BtnMode.Text = VerticesModeToolStripMenuItem.Text
        BtnMode.ToolTipText = VerticesModeToolStripMenuItem.ToolTipText
        MainState.objectToModify = Modified.Vertex
        BtnCSG.Enabled = True
        ColourToolStrip.Visible = BtnColours.Checked
        BtnMerge.Enabled = True
        For Each tri As Triangle In View.SelectedTriangles
            tri.selected = False
        Next
        View.SelectedTriangles.Clear()
        For Each vert As Vertex In View.SelectedVertices
            vert.selected = True
        Next
        Me.Refresh()
        SelectAllToolStripMenuItem.Enabled = True
        SelectConnectedToolStripMenuItem.Enabled = True
        SelectTouchingToolStripMenuItem.Enabled = True
        SelectSameColourToolStripMenuItem.Enabled = True
        WithColourToolStripMenuItem.Enabled = True
    End Sub

    Private Sub TrianglesModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrianglesModeToolStripMenuItem.Click
        LPCFile.helperLineStartIndex = -1
        LPCFile.helperLineEndIndex = -1
        GBMatrix.Visible = False
        GBVertex.Visible = False
        MirrorXLeftToolStripMenuItem.Enabled = True
        MirrorXRightToolStripMenuItem.Enabled = True
        MirrorYTopToolStripMenuItem.Enabled = True
        MirrorYBottomToolStripMenuItem.Enabled = True
        BtnRound.Enabled = False
        If Not MainState.keylock Then MainState.lastMode = 1
        If BtnMode.Text = PrimitiveModeToolStripMenuItem.Text Then
            BtnAddToGroup.Enabled = False
            BtnUngroup.Enabled = False
            Helper_2D.clearSelection()
        End If
        BtnMode.Text = TrianglesModeToolStripMenuItem.Text
        BtnMode.Image = TrianglesModeToolStripMenuItem.Image
        BtnMode.ToolTipText = TrianglesModeToolStripMenuItem.ToolTipText
        MainState.objectToModify = Modified.Triangle
        BtnCSG.Enabled = True
        ColourToolStrip.Visible = BtnColours.Checked
        GBVertex.Visible = False
        BtnMerge.Enabled = False
        For Each vert As Vertex In View.SelectedVertices
            vert.selected = False
        Next
        View.SelectedVertices.Clear()
        Me.Refresh()
        SelectAllToolStripMenuItem.Enabled = True
        SelectConnectedToolStripMenuItem.Enabled = True
        SelectTouchingToolStripMenuItem.Enabled = True
        SelectSameColourToolStripMenuItem.Enabled = True
        WithColourToolStripMenuItem.Enabled = True
    End Sub

    Private Sub PrimitiveModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrimitiveModeToolStripMenuItem.Click
        LPCFile.helperLineStartIndex = -1
        LPCFile.helperLineEndIndex = -1
        BtnAddToGroup.Enabled = False
        BtnUngroup.Enabled = False
        MirrorXLeftToolStripMenuItem.Enabled = False
        MirrorXRightToolStripMenuItem.Enabled = False
        MirrorYTopToolStripMenuItem.Enabled = False
        MirrorYBottomToolStripMenuItem.Enabled = False
        BtnRound.Enabled = False
        If Not MainState.keylock AndAlso Not BtnPreview.Checked Then MainState.lastMode = 2
        SelectAllToolStripMenuItem.Enabled = False
        SelectConnectedToolStripMenuItem.Enabled = False
        SelectTouchingToolStripMenuItem.Enabled = False
        SelectSameColourToolStripMenuItem.Enabled = False
        WithColourToolStripMenuItem.Enabled = False
        BtnMode.Text = PrimitiveModeToolStripMenuItem.Text
        BtnMode.Image = PrimitiveModeToolStripMenuItem.Image
        BtnMode.ToolTipText = PrimitiveModeToolStripMenuItem.ToolTipText
        MainState.objectToModify = Modified.Primitive
        BtnCSG.Enabled = False
        BtnColours.Enabled = True
        ColourToolStrip.Visible = BtnColours.Checked
        GBVertex.Visible = False
        BtnMerge.Enabled = False
        For Each tri As Triangle In View.SelectedTriangles
            tri.selected = False
        Next
        View.SelectedTriangles.Clear()
        For Each vert As Vertex In View.SelectedVertices
            vert.selected = False
        Next
        View.SelectedVertices.Clear()
        Me.Refresh()
    End Sub

    Private Sub ReferenceLineModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReferenceLineModeToolStripMenuItem.Click
        BtnAddToGroup.Enabled = False
        BtnUngroup.Enabled = False
        MirrorXLeftToolStripMenuItem.Enabled = False
        MirrorXRightToolStripMenuItem.Enabled = False
        MirrorYTopToolStripMenuItem.Enabled = False
        MirrorYBottomToolStripMenuItem.Enabled = False
        BtnRound.Enabled = False
        If Not MainState.keylock AndAlso Not BtnPreview.Checked Then MainState.lastMode = 3
        SelectAllToolStripMenuItem.Enabled = False
        SelectConnectedToolStripMenuItem.Enabled = False
        SelectTouchingToolStripMenuItem.Enabled = False
        SelectSameColourToolStripMenuItem.Enabled = False
        WithColourToolStripMenuItem.Enabled = False
        BtnMode.Text = ReferenceLineModeToolStripMenuItem.Text
        BtnMode.Image = ReferenceLineModeToolStripMenuItem.Image
        BtnMode.ToolTipText = ReferenceLineModeToolStripMenuItem.ToolTipText
        MainState.objectToModify = Modified.HelperLine
        BtnCSG.Enabled = False
        BtnColours.Enabled = True
        ColourToolStrip.Visible = BtnColours.Checked
        GBVertex.Visible = False
        BtnMerge.Enabled = False
        For Each tri As Triangle In View.SelectedTriangles
            tri.selected = False
        Next
        View.SelectedTriangles.Clear()
        For Each vert As Vertex In View.SelectedVertices
            vert.selected = False
        Next
        View.SelectedVertices.Clear()
        Me.Refresh()
    End Sub

#End Region
#Region "Copy & Paste"
    Private Sub BtnCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCut.Click
        If View.SelectedVertices.Count > 0 OrElse View.SelectedTriangles.Count > 0 Then
            If View.SelectedVertices.Count = 1 Then GBVertex.Visible = False
            ClipboardHelper.copy()
            ClipboardHelper.delete()
            UndoRedoHelper.addHistory()
        End If
    End Sub

    Private Sub BtnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCopy.Click
        ClipboardHelper.copy()
    End Sub

    Private Sub BtnPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPaste.Click
        ClipboardHelper.paste()
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 OrElse View.SelectedTriangles.Count > 0 Then
            If View.SelectedVertices.Count = 1 Then GBVertex.Visible = False
            ClipboardHelper.copy()
            ClipboardHelper.delete()
            GBMatrix.Visible = False
            UndoRedoHelper.addHistory()
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        ClipboardHelper.copy()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        ClipboardHelper.paste()
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 OrElse View.SelectedTriangles.Count > 0 OrElse (LPCFile.helperLineStartIndex > -1 AndAlso LPCFile.helperLineEndIndex > -1 AndAlso LPCFile.helperLineStartIndex < LPCFile.helperLineEndIndex AndAlso LPCFile.helperLineEndIndex < LPCFile.templateShape.Count) Then
            If Not SBZoom.Focused Then
                Exit Sub
            End If
            If View.SelectedVertices.Count = 1 Then GBVertex.Visible = False
            GBMatrix.Visible = False
            ClipboardHelper.delete()
            UndoRedoHelper.addHistory()
        End If
    End Sub
#End Region

#Region "Select"
    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        SelectionHelper.selectAll()
        If View.SelectedTriangles.Count > 0 Then BtnAddToGroup.Enabled = True
        UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Private Sub SelectSameColourToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectSameColourToolStripMenuItem.Click
        SelectionHelper.selectSameColour()
        UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Private Sub SelectConnectedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectConnectedToolStripMenuItem.Click
        SelectionHelper.selectConnected()
        UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Private Sub SelectTouchingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectTouchingToolStripMenuItem.Click
        SelectionHelper.selectTouching()
        UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Private Sub WithColourToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WithColourToolStripMenuItem.Click
        WithColourToolStripMenuItem.GetCurrentParent.Show()
        Me.Refresh()
    End Sub
#End Region
#Region "Mirror"
    Private Sub MirrorXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MirrorXToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 Then
            GBMatrix.Visible = False
            MirrorHelper.mirrorX()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub MirrorYToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MirrorYToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 Then
            GBMatrix.Visible = False
            MirrorHelper.mirrorY()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub MirrorXLeftToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MirrorXLeftToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 Then
            ClipboardHelper.copy()
            ClipboardHelper.paste()
            MirrorHelper.mirrorX()
            For i As Integer = 0 To View.SelectedVertices.Count - 1
                View.SelectedVertices(i).X += MirrorHelper.deltaXmax * 2
                View.SelectedVertices(i).selected = False
            Next
            If MainState.objectToModify <> Modified.Primitive Then
                cleanupDATVertices()
            Else
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex)).translate(MirrorHelper.deltaXmax, 0)
            End If
            View.SelectedVertices.Clear()
            For i As Integer = 0 To View.SelectedTriangles.Count - 1
                View.SelectedTriangles(i).selected = False
            Next
            View.SelectedTriangles.Clear()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub MirrorXRightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MirrorXRightToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 Then
            ClipboardHelper.copy()
            ClipboardHelper.paste()
            MirrorHelper.mirrorX()
            For i As Integer = 0 To View.SelectedVertices.Count - 1
                View.SelectedVertices(i).X += MirrorHelper.deltaXmin * 2
                View.SelectedVertices(i).selected = False
            Next
            If MainState.objectToModify <> Modified.Primitive Then
                cleanupDATVertices()
            Else
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex)).translate(MirrorHelper.deltaXmin, 0)
            End If
            View.SelectedVertices.Clear()
            For i As Integer = 0 To View.SelectedTriangles.Count - 1
                View.SelectedTriangles(i).selected = False
            Next
            View.SelectedTriangles.Clear()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub MirrorYTopToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MirrorYTopToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 Then
            ClipboardHelper.copy()
            ClipboardHelper.paste()
            MirrorHelper.mirrorY()
            For i As Integer = 0 To View.SelectedVertices.Count - 1
                View.SelectedVertices(i).Y += MirrorHelper.deltaYmin * 2
                View.SelectedVertices(i).selected = False
            Next
            If MainState.objectToModify <> Modified.Primitive Then
                cleanupDATVertices()
            Else
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex)).translate(0, MirrorHelper.deltaYmax)
            End If
            View.SelectedVertices.Clear()
            For i As Integer = 0 To View.SelectedTriangles.Count - 1
                View.SelectedTriangles(i).selected = False
            Next
            View.SelectedTriangles.Clear()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub MirrorYBottomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MirrorYBottomToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 Then
            ClipboardHelper.copy()
            ClipboardHelper.paste()
            MirrorHelper.mirrorY()
            For i As Integer = 0 To View.SelectedVertices.Count - 1
                View.SelectedVertices(i).Y += MirrorHelper.deltaYmax * 2
                View.SelectedVertices(i).selected = False
            Next
            If MainState.objectToModify <> Modified.Primitive Then
                cleanupDATVertices()
            Else
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex)).translate(0, MirrorHelper.deltaYmin)
            End If
            View.SelectedVertices.Clear()
            For i As Integer = 0 To View.SelectedTriangles.Count - 1
                View.SelectedTriangles(i).selected = False
            Next
            View.SelectedTriangles.Clear()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub
#End Region
#Region "Colours"
    Private Sub colourBtnClick(ByVal sender As ToolStripButton, ByVal e As System.EventArgs)
        Dim cnum As Integer = sender.Tag
        If cnum > -1 Then
            setColour(LDConfig.colourHMap(cnum), cnum)
        Else
            setColour(sender.BackColor, -1)
        End If
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub colourBtnMouseUp(ByVal sender As ToolStripButton, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            ColourForm.isDialog = True
            ColourForm.Visible = False
            MainState.lastColour = sender.BackColor
            MainState.lastColourNumber = sender.Tag
            If ColourForm.ShowDialog = Windows.Forms.DialogResult.OK Then
            End If
            sender.BackColor = MainState.lastColour
            sender.Tag = MainState.lastColourNumber
            ColourForm.isDialog = False
            toolStripTrl8(sender, True)
        End If
    End Sub

    Private Sub C16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles C16.Click
        setColour(Nothing, 16)
        UndoRedoHelper.addHistory()
    End Sub

    Public Sub setColour(ByVal c As Color, ByVal cn As Short)
        If View.SelectedTriangles.Count > 0 Then
            If View.SelectedTriangles(0).groupindex <> -1 Then
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedTriangles(0).groupindex)).myColour = c
                LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedTriangles(0).groupindex)).myColourNumber = cn
            Else
                For Each tri As Triangle In View.SelectedTriangles
                    tri.myColour = c
                    tri.myColourNumber = cn
                Next
            End If
            Me.Refresh()
        End If
        MainState.lastColour = c
        MainState.lastColourNumber = cn
        ColourForm.NR.Value = MainState.lastColour.R
        ColourForm.NG.Value = MainState.lastColour.G
        ColourForm.NB.Value = MainState.lastColour.B
        CLast.BackColor = MainState.lastColour
        If cn = -1 Then
            CLast.Text = "0x2" & MainState.lastColour.R.ToString("X2") & MainState.lastColour.G.ToString("X2") & MainState.lastColour.B.ToString("X2")
        Else
            CLast.Text = MainState.lastColourNumber
        End If
        If ColourForm.isDialog Then ColourForm.isDialog = False : ColourForm.DialogResult = Windows.Forms.DialogResult.OK : ColourForm.Visible = False
    End Sub
#End Region
#Region "Undo & Redo"
    Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
        Undo()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoToolStripMenuItem.Click
        Redo()
    End Sub

    Private Sub BtnUndo_Click(sender As Object, e As EventArgs) Handles BtnUndo.Click
        Undo()
    End Sub

    Private Sub BtnRedo_Click(sender As Object, e As EventArgs) Handles BtnRedo.Click
        Redo()
    End Sub

    Private Sub Undo()
        MainState.isLoading = True
        UndoRedoHelper.undo()
        MainState.isLoading = False
        Me.Refresh()
    End Sub

    Private Sub Redo()
        MainState.isLoading = True
        UndoRedoHelper.redo()
        MainState.isLoading = False
        Me.Refresh()
    End Sub
#End Region
#Region "File Menue"
    Private Sub LoadPatternToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadPatternToolStripMenuItem.Click
        FileToolStripMenuItem.HideDropDown()
        If MainState.unsavedChanges AndAlso ShowAllWarningsToolStripMenuItem.Checked Then
            Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.UnsavedChanges), MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
            If result = MsgBoxResult.Yes Then SaveToolStripMenuItem.PerformClick()
            If result = MsgBoxResult.Cancel Then Exit Sub
        End If
        Dim result2 As DialogResult
        result2 = LoadFile.ShowDialog()
        If result2 = Windows.Forms.DialogResult.OK Then
            If LoadFile.FileName <> "" Then
                LoadLPCFile(LoadFile.FileName)
            End If
        End If
    End Sub

    Private Sub LoadLPCFile(ByVal myFilename As String)
        MainState.isLoading = True
        newPattern()
        BtnMove.PerformClick()
        MainState.isLoading = True
        Try
            Dim encoding As Object
            ' Detect Encoding
            Dim header As String
            Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(myFilename, New System.Text.UnicodeEncoding())
                header = DateiIn.ReadLine()
                If header = "0 LDraw - Pattern Creator 1.1 ASCII File" OrElse header = "0 LDraw - Pattern Creator 1.2 ASCII File" OrElse header = "0 LDraw - Pattern Creator 1.3 Unicode File" Then
                    encoding = New System.Text.UnicodeEncoding()
                Else
                    encoding = New System.Text.UTF8Encoding(False)
                End If
            End Using
            Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(myFilename, encoding)
                header = DateiIn.ReadLine()
            End Using
            header = Mid(header, header.IndexOf("0") + 1)

            If header = "0 LDraw - Pattern Creator 1.3 Unicode File" Then
                LoadLPCFileV13(myFilename, encoding)
            ElseIf header = "0 LDraw - Pattern Creator 1.2 ASCII File" Then
                LoadLPCFileV12(myFilename, encoding)
            ElseIf header = "0 LDraw - Pattern Creator 1.1 ASCII File" Then
                LoadLPCFileV11(myFilename, encoding)
            ElseIf header = "0 LDraw - Pattern Creator 1.0 ASCII File" Then
                LoadLPCFileV10(myFilename, encoding)
            End If

            Dim file As String = myFilename
            LoadFile.FileName = System.IO.Path.GetFullPath(file)
            SaveAs.FileName = System.IO.Path.GetFullPath(file)

            Dim shortFileName As String = CalculateShortFileNameAndUpdateTitle(file)

            Dim ti As New ToolStripMenuItem
            ti.Text = shortFileName
            ti.ToolTipText = file
            AddHandler ti.Click, AddressOf recentFileNameClick
            If EnvironmentPaths.recentFiles.Contains(file) Then
                EnvironmentPaths.recentFiles.Remove(file)
                EnvironmentPaths.recentFiles.Insert(0, file)
                For i As Integer = 0 To LoadPatternToolStripMenuItem.DropDownItems.Count - 1
                    If file = LoadPatternToolStripMenuItem.DropDownItems(i).ToolTipText Then
                        LoadPatternToolStripMenuItem.DropDownItems.RemoveAt(i)
                        Exit For
                    End If
                Next
                LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
            ElseIf EnvironmentPaths.recentFiles(9) = "" Then
                LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
                For i As Integer = 0 To 9
                    If EnvironmentPaths.recentFiles(i) = "" Then
                        EnvironmentPaths.recentFiles(i) = file
                        LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
                        Exit For
                    End If
                Next
            Else
                EnvironmentPaths.recentFiles.RemoveAt(9)
                EnvironmentPaths.recentFiles.Insert(0, file)
                LoadPatternToolStripMenuItem.DropDownItems.RemoveAt(9)
                LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
            End If
            saveConfig()
        Catch ex As Exception
            MsgBox(Replace(I18N.trl8(I18N.lk.InvalidDAT), "DAT", "LPC") & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton1, I18N.trl8(I18N.lk.Fatal))
            newPattern()
        End Try
        MainState.isLoading = False
        UndoRedoHelper.cleanupHistoryData()
        UndoRedoHelper.addHistory()
        MainState.unsavedChanges = False
        Me.Refresh()
    End Sub

    Private Sub LoadLPCFileV10(ByVal myFilename As String, ByVal encoding As Encoding)
        ' 0 LDraw - Pattern Creator 1.0 ASCII File
        Dim maxVertexID As Integer
        Dim maxTriangleID As Integer
        Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(myFilename, encoding)
            DateiIn.ReadLine()
            ' Load Triangles:
            Dim trianz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim myColour, vID1, vID2, vID3, triangleID As Integer
            Dim myColourNumber As Short
            For tri As Integer = 1 To trianz
                triangleID = CType(DateiIn.ReadLine(), Integer)
                If triangleID > maxTriangleID Then maxTriangleID = triangleID
                myColour = CType(DateiIn.ReadLine(), Integer)
                myColourNumber = CType(DateiIn.ReadLine(), Short)
                vID1 = CType(DateiIn.ReadLine(), Integer)
                vID2 = CType(DateiIn.ReadLine(), Integer)
                vID3 = CType(DateiIn.ReadLine(), Integer)
                LPCFile.Triangles.Add(New Triangle(New Vertex(0, 0, False, vID1) _
                                         , New Vertex(0, 0, False, vID2) _
                                         , New Vertex(0, 0, False, vID3), False) _
                                         With {.myColour = Color.FromArgb(myColour), .myColourNumber = myColourNumber, .triangleID = triangleID})
            Next
            ' Load Vertices:
            Dim vertanz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim X, Y As Double
            Dim vertexID, triangleID2 As Integer
            For vert As Integer = 1 To vertanz
                vertexID = CType(DateiIn.ReadLine(), Integer)
                If vertexID > maxVertexID Then maxVertexID = vertexID
                X = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Y = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                LPCFile.Vertices.Add(New Vertex(-X, -Y, False, vertexID))
                Dim linkedtrianz As Integer = CType(DateiIn.ReadLine(), Integer)
                For tri As Integer = 1 To linkedtrianz
                    triangleID2 = CType(DateiIn.ReadLine(), Integer)
                    ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(New Triangle(Nothing, Nothing, Nothing, False) With {.triangleID = triangleID2})
                Next
            Next
            GlobalIdSet.vertexIDglobal = CType(DateiIn.ReadLine(), Integer)
            GlobalIdSet.triangleIDglobal = CType(DateiIn.ReadLine(), Integer)
            If GlobalIdSet.vertexIDglobal < maxVertexID Then GlobalIdSet.vertexIDglobal = maxVertexID
            If GlobalIdSet.triangleIDglobal < maxTriangleID Then GlobalIdSet.triangleIDglobal = maxTriangleID
            View.imgPath = DateiIn.ReadLine()
            ImageForm.TBImage.Text = View.imgPath
            Try
                View.backgroundPicture.Dispose()
                If View.imgPath <> "" Then
                    View.backgroundPicture = New Bitmap(View.imgPath)
                Else
                    View.backgroundPicture = My.Resources.temp
                End If
            Catch
                View.backgroundPicture = My.Resources.temp
            End Try
            View.backgroundPictureBrush.Dispose()
            View.backgroundPictureBrush = New TextureBrush(View.backgroundPicture)
            ImageForm.NUDoffsetX.Maximum = Integer.MaxValue
            ImageForm.NUDoffsetY.Maximum = Integer.MaxValue
            ImageForm.NUDScale.Maximum = Decimal.MaxValue
            ImageForm.NUDoffsetX.Minimum = Integer.MinValue
            ImageForm.NUDoffsetY.Minimum = Integer.MinValue
            ImageForm.NUDScale.Minimum = Decimal.MinValue
            ImageForm.NUDoffsetX.Value = CType(DateiIn.ReadLine(), Integer)
            ImageForm.NUDoffsetY.Value = CType(DateiIn.ReadLine(), Integer)
            ImageForm.NUDScale.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.imgOffsetX = ImageForm.NUDoffsetX.Value
            View.imgOffsetY = ImageForm.NUDoffsetY.Value
            View.imgScale = ImageForm.NUDScale.Value
            ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
            cleanupOldData()
        End Using
    End Sub

    Private Sub LoadLPCFileV11(ByVal myFilename As String, ByVal encoding As Encoding)
        Dim duplicatePrimitiveIDs As Boolean
        Dim maxVertexID As Integer
        Dim maxTriangleID As Integer
        Dim maxPrimitiveID As Integer
        Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(myFilename, encoding)
            DateiIn.ReadLine()
            ' Load Triangles:
            Dim trianz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim myColour, vID1, vID2, vID3, triangleID, groupindex As Integer
            Dim myColourNumber As Short
            For tri As Integer = 1 To trianz
                triangleID = CType(DateiIn.ReadLine(), Integer)
                If triangleID > maxTriangleID Then maxTriangleID = triangleID
                myColour = CType(DateiIn.ReadLine(), Integer)
                myColourNumber = CType(DateiIn.ReadLine(), Short)
                If LDConfig.colourHMap.ContainsKey(myColourNumber) Then myColour = LDConfig.colourHMap(myColourNumber).ToArgb
                vID1 = CType(DateiIn.ReadLine(), Integer)
                vID2 = CType(DateiIn.ReadLine(), Integer)
                vID3 = CType(DateiIn.ReadLine(), Integer)
                groupindex = CType(DateiIn.ReadLine(), Integer)
                LPCFile.Triangles.Add(New Triangle(New Vertex(0, 0, False, vID1) _
                                         , New Vertex(0, 0, False, vID2) _
                                         , New Vertex(0, 0, False, vID3), False) _
                                         With {.myColour = Color.FromArgb(myColour), .myColourNumber = myColourNumber, .triangleID = triangleID, .groupindex = groupindex})
            Next
            ' Load Vertices:
            Dim vertanz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim X, Y As Double
            Dim vertexID, triangleID2 As Integer
            Dim VIDtoVI As New Dictionary(Of Integer, Integer)
            For vert As Integer = 1 To vertanz
                vertexID = CType(DateiIn.ReadLine(), Integer)
                If vertexID > maxVertexID Then maxVertexID = vertexID
                If Not VIDtoVI.ContainsKey(vertexID) Then
                    VIDtoVI.Add(vertexID, vert - 1)
                End If
                X = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Y = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                groupindex = CType(DateiIn.ReadLine(), Integer)
                LPCFile.Vertices.Add(New Vertex(X, Y, False, vertexID) With {.groupindex = groupindex})
                Dim linkedtrianz As Integer = CType(DateiIn.ReadLine(), Integer)
                For tri As Integer = 1 To linkedtrianz
                    triangleID2 = CType(DateiIn.ReadLine(), Integer)
                    ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(New Triangle(Nothing, Nothing, Nothing, False) With {.triangleID = triangleID2})
                Next
            Next
            ' Load Primitives:
            Dim primanz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim ox, oy As Double
            Dim primitiveNameRead As String
            Dim primitiveID As Integer
            Dim centerVertexID As Integer
            For prim As Integer = 1 To primanz
                Dim matrix(3, 3) As Double
                primitiveID = CType(DateiIn.ReadLine(), Integer)
                If primitiveID > maxPrimitiveID Then maxPrimitiveID = primitiveID
                centerVertexID = CType(DateiIn.ReadLine(), Integer)
                ox = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                oy = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                primitiveNameRead = DateiIn.ReadLine()
                If Not LPCFile.PrimitivesMetadataHMap.ContainsKey(primitiveNameRead) Then LPCFile.PrimitivesMetadataHMap.Add(primitiveNameRead, Nothing)
                myColourNumber = CType(DateiIn.ReadLine(), Short)
                If LDConfig.colourHMap.ContainsKey(myColourNumber) Then myColour = LDConfig.colourHMap(myColourNumber).ToArgb
                For z As Integer = 0 To 3
                    For s As Integer = 0 To 3
                        matrix(z, s) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                    Next
                Next
                LPCFile.Primitives.Add(New Primitive(0, 0, ox, oy, primitiveNameRead, centerVertexID, False) With {.matrix = matrix.Clone, .primitiveID = primitiveID, .myColourNumber = myColourNumber, .myColour = Color.FromArgb(myColour)})
                ' Validate primitive center vertex
                If Not duplicatePrimitiveIDs Then
                    If VIDtoVI.ContainsKey(centerVertexID) Then
                        Dim vert As Vertex = LPCFile.Vertices(VIDtoVI(centerVertexID))
                        If Fix(vert.X) <> Fix(matrix(0, 3)) Or Fix(vert.Y) <> Fix(matrix(1, 3)) Then
                            duplicatePrimitiveIDs = True
                        End If
                    Else
                        duplicatePrimitiveIDs = True
                    End If
                End If
            Next
            ' Load Metadata:
            DateiIn.ReadLine()
            LPCFile.myMetadata.recommendedMode = CType(DateiIn.ReadLine(), Byte)
            LPCFile.myMetadata.additionalData = DateiIn.ReadLine()
            For r As Integer = 0 To 3
                For c As Integer = 0 To 3
                    LPCFile.myMetadata.matrix(r, c) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Next
            Next
            For i As Integer = 0 To 11
                LPCFile.myMetadata.mData(i) = DateiIn.ReadLine()
            Next i
            Do While DateiIn.ReadLine() = "--Metadata--"
                Dim mData(11) As String
                Dim key As String = DateiIn.ReadLine()
                For i As Integer = 0 To 11
                    mData(i) = DateiIn.ReadLine()
                Next i
                If LPCFile.PrimitivesMetadataHMap.ContainsKey(key) Then
                    LPCFile.PrimitivesMetadataHMap.Remove(key)
                    LPCFile.PrimitivesMetadataHMap.Add(key, New Metadata(mData(0), mData(1), mData(2), mData(3), mData(4), mData(5), mData(6), mData(7), mData(8), mData(9), mData(10), mData(11)))
                End If
            Loop

            For Each key As String In LPCFile.PrimitivesMetadataHMap.Keys
                If key Like "subfile*" AndAlso LPCFile.PrimitivesMetadataHMap(key) Is Nothing Then
                    Dim start As Integer = 0
                    Do
                        Dim pc As Integer = LPCFile.Primitives.Count - 1
                        For i As Integer = start To pc
                            If LPCFile.Primitives(i).primitiveName = key Then
                                LPCFile.Primitives.RemoveAt(i)
                                Continue Do
                            End If
                        Next
                        Exit Do
                    Loop
                End If
            Next

            Dim pc2 As Integer = LPCFile.Primitives.Count - 1
            For prim As Integer = 0 To pc2
                Dim pID As Integer = LPCFile.Primitives(prim).primitiveID
                If LPCFile.PrimitivesHMap.ContainsKey(pID) Then
                    duplicatePrimitiveIDs = True
                Else
                    LPCFile.PrimitivesHMap.Add(pID, prim)
                End If
            Next

            Dim templateShapeCount As Integer = CType(DateiIn.ReadLine(), Integer)
            For i As Integer = 0 To templateShapeCount - 1
                Dim tx, ty As Single
                tx = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Single)
                ty = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Single)
                LPCFile.templateShape.Add(New PointF(tx, ty))
            Next

            DateiIn.ReadLine()
            LPCFile.includeMetadata = CType(DateiIn.ReadLine(), Boolean)
            GlobalIdSet.vertexIDglobal = CType(DateiIn.ReadLine(), Integer)
            GlobalIdSet.triangleIDglobal = CType(DateiIn.ReadLine(), Integer)
            GlobalIdSet.primitiveIDglobal = CType(DateiIn.ReadLine(), Integer)
            If GlobalIdSet.vertexIDglobal < maxVertexID Then GlobalIdSet.vertexIDglobal = maxVertexID
            If GlobalIdSet.triangleIDglobal < maxTriangleID Then GlobalIdSet.triangleIDglobal = maxTriangleID
            If GlobalIdSet.primitiveIDglobal < maxPrimitiveID Then GlobalIdSet.primitiveIDglobal = maxPrimitiveID
            View.imgPath = DateiIn.ReadLine()
            ImageForm.TBImage.Text = View.imgPath
            ImageForm.NUDoffsetX.Maximum = Integer.MaxValue
            ImageForm.NUDoffsetY.Maximum = Integer.MaxValue
            ImageForm.NUDScale.Maximum = Decimal.MaxValue
            ImageForm.NUDoffsetX.Minimum = Integer.MinValue
            ImageForm.NUDoffsetY.Minimum = Integer.MinValue
            ImageForm.NUDScale.Minimum = Decimal.MinValue
            ImageForm.NUDoffsetX.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Integer)
            ImageForm.NUDoffsetY.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Integer)
            ImageForm.NUDScale.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.imgOffsetX = ImageForm.NUDoffsetX.Value
            View.imgOffsetY = ImageForm.NUDoffsetY.Value
            View.imgScale = ImageForm.NUDScale.Value
            View.unitFactor = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
            View.unit = DateiIn.ReadLine()
            Select Case View.unit
                Case "LDU"
                    LDUToolStripMenuItem.PerformClick()
                Case "mm"
                    MillimeterToolStripMenuItem.PerformClick()
                Case "inch"
                    InchToolStripMenuItem.PerformClick()
            End Select

            PreferencesForm.NUDMoveSnap.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            PreferencesForm.NUDRotateSnap.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            PreferencesForm.NUDScaleSnap.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            PreferencesForm.NUDGrid.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.moveSnap = PreferencesForm.NUDMoveSnap.Value
            View.rotateSnap = PreferencesForm.NUDRotateSnap.Value
            View.scaleSnap = PreferencesForm.NUDScaleSnap.Value
            View.rasterSnap = PreferencesForm.NUDGrid.Value
        End Using

        If duplicatePrimitiveIDs Then

            Dim tc As Integer = LPCFile.Triangles.Count - 1
            Dim start As Integer = 0
newDelete:
            For i As Integer = start To tc
                Dim tri As Triangle = LPCFile.Triangles(i)
                tri.groupindex = -1
                If tri.vertexA.vertexID = tri.vertexB.vertexID OrElse
                tri.vertexB.vertexID = tri.vertexC.vertexID OrElse
                tri.vertexC.vertexID = tri.vertexA.vertexID Then
                    LPCFile.Triangles.RemoveAt(i)
                    start = i
                    tc -= 1
                    GoTo newDelete
                End If
            Next

            For Each vert As Vertex In LPCFile.Vertices
                vert.groupindex = -1
            Next
            LPCFile.Primitives.Clear()
            LPCFile.PrimitivesMetadataHMap.Clear()
            LPCFile.PrimitivesHMap.Clear()
        End If

        Try
            View.backgroundPicture.Dispose()
            If View.imgPath <> "" Then
                View.backgroundPicture = New Bitmap(View.imgPath)
            Else
                View.backgroundPicture = My.Resources.temp
            End If
        Catch
            View.backgroundPicture = My.Resources.temp
        End Try

        View.backgroundPictureBrush.Dispose()
        View.backgroundPictureBrush = New TextureBrush(View.backgroundPicture)

        ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
    End Sub

    Private Sub LoadLPCFileV12(ByVal myFilename As String, ByVal encoding As Encoding)
        Dim duplicatePrimitiveIDs As Boolean
        Dim maxVertexID As Integer
        Dim maxTriangleID As Integer
        Dim maxPrimitiveID As Integer
        Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(myFilename, encoding)
            DateiIn.ReadLine()
            ' Load Triangles:
            Dim trianz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim myColour, vID1, vID2, vID3, triangleID, groupindex As Integer
            Dim myColourNumber As Short
            For tri As Integer = 1 To trianz
                triangleID = CType(DateiIn.ReadLine(), Integer)
                If triangleID > maxTriangleID Then maxTriangleID = triangleID
                myColour = CType(DateiIn.ReadLine(), Integer)
                myColourNumber = CType(DateiIn.ReadLine(), Short)
                If LDConfig.colourHMap.ContainsKey(myColourNumber) Then myColour = LDConfig.colourHMap(myColourNumber).ToArgb
                vID1 = CType(DateiIn.ReadLine(), Integer)
                vID2 = CType(DateiIn.ReadLine(), Integer)
                vID3 = CType(DateiIn.ReadLine(), Integer)
                groupindex = CType(DateiIn.ReadLine(), Integer)
                LPCFile.Triangles.Add(New Triangle(New Vertex(0, 0, False, vID1) _
                                         , New Vertex(0, 0, False, vID2) _
                                         , New Vertex(0, 0, False, vID3), False) _
                                         With {.myColour = Color.FromArgb(myColour), .myColourNumber = myColourNumber, .triangleID = triangleID, .groupindex = groupindex})
            Next
            ' Load Vertices:
            Dim vertanz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim X, Y As Double
            Dim vertexID, triangleID2 As Integer
            Dim VIDtoVI As New Dictionary(Of Integer, Integer)
            For vert As Integer = 1 To vertanz
                vertexID = CType(DateiIn.ReadLine(), Integer)
                If vertexID > maxVertexID Then maxVertexID = vertexID
                If Not VIDtoVI.ContainsKey(vertexID) Then
                    VIDtoVI.Add(vertexID, vert - 1)
                End If
                X = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Y = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                groupindex = CType(DateiIn.ReadLine(), Integer)
                LPCFile.Vertices.Add(New Vertex(X, Y, False, vertexID) With {.groupindex = groupindex})
                Dim linkedtrianz As Integer = CType(DateiIn.ReadLine(), Integer)
                For tri As Integer = 1 To linkedtrianz
                    triangleID2 = CType(DateiIn.ReadLine(), Integer)
                    ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(New Triangle(Nothing, Nothing, Nothing, False) With {.triangleID = triangleID2})
                Next
            Next
            ' Load Primitives:
            Dim primanz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim ox, oy As Double
            Dim primitiveNameRead As String
            Dim primitiveID As Integer
            Dim centerVertexID As Integer
            For prim As Integer = 1 To primanz
                Dim matrix(3, 3) As Double
                primitiveID = CType(DateiIn.ReadLine(), Integer)
                If primitiveID > maxPrimitiveID Then maxPrimitiveID = primitiveID
                centerVertexID = CType(DateiIn.ReadLine(), Integer)
                ox = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                oy = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                primitiveNameRead = DateiIn.ReadLine()
                If Not LPCFile.PrimitivesMetadataHMap.ContainsKey(primitiveNameRead) Then LPCFile.PrimitivesMetadataHMap.Add(primitiveNameRead, Nothing)
                myColour = CType(DateiIn.ReadLine(), Integer)
                myColourNumber = CType(DateiIn.ReadLine(), Short)
                If LDConfig.colourHMap.ContainsKey(myColourNumber) Then myColour = LDConfig.colourHMap(myColourNumber).ToArgb
                For z As Integer = 0 To 3
                    For s As Integer = 0 To 3
                        matrix(z, s) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                    Next
                Next
                LPCFile.Primitives.Add(New Primitive(0, 0, ox, oy, primitiveNameRead, centerVertexID, False) With {.matrix = matrix.Clone, .primitiveID = primitiveID, .myColourNumber = myColourNumber, .myColour = Color.FromArgb(myColour)})
                ' Validate primitive center vertex
                If Not duplicatePrimitiveIDs Then
                    If VIDtoVI.ContainsKey(centerVertexID) Then
                        Dim vert As Vertex = LPCFile.Vertices(VIDtoVI(centerVertexID))
                        If Fix(vert.X) <> Fix(matrix(0, 3)) Or Fix(vert.Y) <> Fix(matrix(1, 3)) Then
                            duplicatePrimitiveIDs = True
                        End If
                    Else
                        duplicatePrimitiveIDs = True
                    End If
                End If
            Next
            ' Load Metadata:
            DateiIn.ReadLine()
            LPCFile.myMetadata.recommendedMode = CType(DateiIn.ReadLine(), Byte)
            LPCFile.myMetadata.additionalData = DateiIn.ReadLine()
            For r As Integer = 0 To 3
                For c As Integer = 0 To 3
                    LPCFile.myMetadata.matrix(r, c) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Next
            Next
            For i As Integer = 0 To 11
                LPCFile.myMetadata.mData(i) = DateiIn.ReadLine()
            Next i
            Do While DateiIn.ReadLine() = "--Metadata--"
                Dim mData(11) As String
                Dim key As String = DateiIn.ReadLine()
                For i As Integer = 0 To 11
                    mData(i) = DateiIn.ReadLine()
                Next i
                If LPCFile.PrimitivesMetadataHMap.ContainsKey(key) Then
                    LPCFile.PrimitivesMetadataHMap.Remove(key)
                    LPCFile.PrimitivesMetadataHMap.Add(key, New Metadata(mData(0), mData(1), mData(2), mData(3), mData(4), mData(5), mData(6), mData(7), mData(8), mData(9), mData(10), mData(11)))
                End If
            Loop

            For Each key As String In LPCFile.PrimitivesMetadataHMap.Keys
                If key Like "subfile*" AndAlso LPCFile.PrimitivesMetadataHMap(key) Is Nothing Then
                    Dim start As Integer = 0
                    Do
                        Dim pc As Integer = LPCFile.Primitives.Count - 1
                        For i As Integer = start To pc
                            If LPCFile.Primitives(i).primitiveName = key Then
                                LPCFile.Primitives.RemoveAt(i)
                                Continue Do
                            End If
                        Next
                        Exit Do
                    Loop
                End If
            Next

            Dim pc2 As Integer = LPCFile.Primitives.Count - 1
            For prim As Integer = 0 To pc2
                Dim pID As Integer = LPCFile.Primitives(prim).primitiveID
                If LPCFile.PrimitivesHMap.ContainsKey(pID) Then
                    duplicatePrimitiveIDs = True
                Else
                    LPCFile.PrimitivesHMap.Add(pID, prim)
                End If
            Next

            Dim templateShapeCount As Integer = CType(DateiIn.ReadLine(), Integer)
            For i As Integer = 0 To templateShapeCount - 1
                Dim tx, ty As Single
                tx = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Single)
                ty = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Single)
                LPCFile.templateShape.Add(New PointF(tx, ty))
            Next

            Dim templateQuadCount As Integer = CType(DateiIn.ReadLine(), Integer)
            DateiIn.ReadLine() ' old MainState.alreadyCalculatedQuads entry
            For i As Integer = 0 To templateQuadCount - 1
                Dim s() As String = Replace(DateiIn.ReadLine, ".", MathHelper.comma).Split(" ")
                Dim double_array(s.Length - 1) As Double
                For j As Integer = 0 To s.Length - 1
                    double_array(j) = CType(s(j), Double)
                Next
                LPCFile.templateProjectionQuads.Add(New ProjectionQuad(
                double_array(0), double_array(1), double_array(2), double_array(3), double_array(4),
                double_array(5), double_array(6), double_array(7), double_array(8), double_array(9),
                double_array(10), double_array(11), double_array(12), double_array(13), double_array(14),
                double_array(15), double_array(16), double_array(17), double_array(18), double_array(19)))
                For r As Integer = 0 To 3
                    For c As Integer = 0 To 3
                        LPCFile.templateProjectionQuads(i).matrix(r, c) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
                    Next
                Next
            Next
            Dim templateTextsCount As Integer = CType(DateiIn.ReadLine(), Integer)
            For i As Integer = 0 To templateTextsCount - 1
                Dim Text As String = Replace(DateiIn.ReadLine, ".", MathHelper.comma)
                X = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Y = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                LPCFile.templateTexts.Add(New TemplateTextInfo(X, Y, Text))
            Next
            DateiIn.ReadLine()
            LPCFile.includeMetadata = CType(DateiIn.ReadLine(), Boolean)
            GlobalIdSet.vertexIDglobal = CType(DateiIn.ReadLine(), Integer)
            GlobalIdSet.triangleIDglobal = CType(DateiIn.ReadLine(), Integer)
            GlobalIdSet.primitiveIDglobal = CType(DateiIn.ReadLine(), Integer)
            If GlobalIdSet.vertexIDglobal < maxVertexID Then GlobalIdSet.vertexIDglobal = maxVertexID
            If GlobalIdSet.triangleIDglobal < maxTriangleID Then GlobalIdSet.triangleIDglobal = maxTriangleID
            If GlobalIdSet.primitiveIDglobal < maxPrimitiveID Then GlobalIdSet.primitiveIDglobal = maxPrimitiveID
            View.imgPath = DateiIn.ReadLine()
            ImageForm.TBImage.Text = View.imgPath
            ImageForm.NUDoffsetX.Maximum = Integer.MaxValue
            ImageForm.NUDoffsetY.Maximum = Integer.MaxValue
            ImageForm.NUDScale.Maximum = Decimal.MaxValue
            ImageForm.NUDoffsetX.Minimum = Integer.MinValue
            ImageForm.NUDoffsetY.Minimum = Integer.MinValue
            ImageForm.NUDScale.Minimum = Decimal.MinValue
            ImageForm.NUDoffsetX.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Integer)
            ImageForm.NUDoffsetY.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Integer)
            ImageForm.NUDScale.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.imgOffsetX = ImageForm.NUDoffsetX.Value
            View.imgOffsetY = ImageForm.NUDoffsetY.Value
            View.imgScale = ImageForm.NUDScale.Value
            View.unitFactor = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
            View.unit = DateiIn.ReadLine()
            Select Case View.unit
                Case "LDU"
                    LDUToolStripMenuItem.PerformClick()
                Case "mm"
                    MillimeterToolStripMenuItem.PerformClick()
                Case "inch"
                    InchToolStripMenuItem.PerformClick()
            End Select

            PreferencesForm.NUDMoveSnap.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            PreferencesForm.NUDRotateSnap.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            PreferencesForm.NUDScaleSnap.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            PreferencesForm.NUDGrid.Value = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.moveSnap = PreferencesForm.NUDMoveSnap.Value
            View.rotateSnap = PreferencesForm.NUDRotateSnap.Value
            View.scaleSnap = PreferencesForm.NUDScaleSnap.Value
            View.rasterSnap = PreferencesForm.NUDGrid.Value
        End Using

        If duplicatePrimitiveIDs Then

            Dim tc As Integer = LPCFile.Triangles.Count - 1
            Dim start As Integer = 0
newDelete:
            For i As Integer = start To tc
                Dim tri As Triangle = LPCFile.Triangles(i)
                tri.groupindex = -1
                If tri.vertexA.vertexID = tri.vertexB.vertexID OrElse
                tri.vertexB.vertexID = tri.vertexC.vertexID OrElse
                tri.vertexC.vertexID = tri.vertexA.vertexID Then
                    LPCFile.Triangles.RemoveAt(i)
                    start = i
                    tc -= 1
                    GoTo newDelete
                End If
            Next

            For Each vert As Vertex In LPCFile.Vertices
                vert.groupindex = Primitive.NO_INDEX
            Next
            LPCFile.Primitives.Clear()
            LPCFile.PrimitivesMetadataHMap.Clear()
            LPCFile.PrimitivesHMap.Clear()
        End If

        Try
            View.backgroundPicture.Dispose()
            If View.imgPath <> "" Then
                View.backgroundPicture = New Bitmap(View.imgPath)
            Else
                View.backgroundPicture = My.Resources.temp
            End If
        Catch
            View.backgroundPicture = My.Resources.temp
        End Try

        View.backgroundPictureBrush.Dispose()
        View.backgroundPictureBrush = New TextureBrush(View.backgroundPicture)

        ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)

    End Sub

    Private Sub LoadLPCFileV13(ByVal myFilename As String, ByVal encoding As Encoding)
        Dim duplicatePrimitiveIDs As Boolean
        Dim maxVertexID As Integer
        Dim maxTriangleID As Integer
        Dim maxPrimitiveID As Integer
        Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(myFilename, encoding)
            DateiIn.ReadLine()
            ' Load Triangles:
            Dim trianz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim myColour, vID1, vID2, vID3, triangleID, groupindex As Integer
            Dim myColourNumber As Short
            For tri As Integer = 1 To trianz
                triangleID = CType(DateiIn.ReadLine(), Integer)
                If triangleID > maxTriangleID Then maxTriangleID = triangleID
                myColour = CType(DateiIn.ReadLine(), Integer)
                myColourNumber = CType(DateiIn.ReadLine(), Short)
                If LDConfig.colourHMap.ContainsKey(myColourNumber) Then myColour = LDConfig.colourHMap(myColourNumber).ToArgb
                vID1 = CType(DateiIn.ReadLine(), Integer)
                vID2 = CType(DateiIn.ReadLine(), Integer)
                vID3 = CType(DateiIn.ReadLine(), Integer)
                groupindex = CType(DateiIn.ReadLine(), Integer)
                LPCFile.Triangles.Add(New Triangle(New Vertex(0, 0, False, vID1) _
                                         , New Vertex(0, 0, False, vID2) _
                                         , New Vertex(0, 0, False, vID3), False) _
                                         With {.myColour = Color.FromArgb(myColour), .myColourNumber = myColourNumber, .triangleID = triangleID, .groupindex = groupindex})
            Next
            ' Load Vertices:
            Dim vertanz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim X, Y As Double
            Dim vertexID, triangleID2 As Integer
            Dim VIDtoVI As New Dictionary(Of Integer, Integer)
            For vert As Integer = 1 To vertanz
                vertexID = CType(DateiIn.ReadLine(), Integer)
                If vertexID > maxVertexID Then maxVertexID = vertexID
                If Not VIDtoVI.ContainsKey(vertexID) Then
                    VIDtoVI.Add(vertexID, vert - 1)
                End If
                X = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Y = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                groupindex = CType(DateiIn.ReadLine(), Integer)
                LPCFile.Vertices.Add(New Vertex(X, Y, False, vertexID) With {.groupindex = groupindex})
                Dim linkedtrianz As Integer = CType(DateiIn.ReadLine(), Integer)
                For tri As Integer = 1 To linkedtrianz
                    triangleID2 = CType(DateiIn.ReadLine(), Integer)
                    ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(New Triangle(Nothing, Nothing, Nothing, False) With {.triangleID = triangleID2})
                Next
            Next
            ' Load Primitives:
            Dim primanz As Integer = CType(DateiIn.ReadLine(), Integer)
            Dim ox, oy As Double
            Dim primitiveNameRead As String
            Dim primitiveID As Integer
            Dim centerVertexID As Integer
            For prim As Integer = 1 To primanz
                Dim matrix(3, 3) As Double
                primitiveID = CType(DateiIn.ReadLine(), Integer)
                If primitiveID > maxPrimitiveID Then maxPrimitiveID = primitiveID
                centerVertexID = CType(DateiIn.ReadLine(), Integer)
                ox = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                oy = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                primitiveNameRead = DateiIn.ReadLine()
                If Not LPCFile.PrimitivesMetadataHMap.ContainsKey(primitiveNameRead) Then LPCFile.PrimitivesMetadataHMap.Add(primitiveNameRead, Nothing)
                myColour = CType(DateiIn.ReadLine(), Integer)
                myColourNumber = CType(DateiIn.ReadLine(), Short)
                If LDConfig.colourHMap.ContainsKey(myColourNumber) Then myColour = LDConfig.colourHMap(myColourNumber).ToArgb
                For z As Integer = 0 To 3
                    For s As Integer = 0 To 3
                        matrix(z, s) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                    Next
                Next
                Dim p As New Primitive(0, 0, ox, oy, primitiveNameRead, centerVertexID, False) With {.matrix = matrix.Clone, .primitiveID = primitiveID, .myColourNumber = myColourNumber, .myColour = Color.FromArgb(myColour)}
                LPCFile.Primitives.Add(p)
                ' Validate primitive center vertex
                If Not duplicatePrimitiveIDs Then
                    If Not VIDtoVI.ContainsKey(centerVertexID) Then
                        Dim foundOldCenter As Boolean = False
                        Dim ov As New Vertex(matrix(0, 3), matrix(1, 3), False, False)
                        For Each v As Vertex In LPCFile.Vertices
                            If v.dist(ov) < 0.1 AndAlso p.primitiveID = v.groupindex Then
                                p.centerVertexID = v.vertexID
                                foundOldCenter = True
                                Exit For
                            End If
                        Next

                        If Not foundOldCenter Then
                            duplicatePrimitiveIDs = True
                        End If
                    End If
                End If
            Next
            ' Load Metadata:
            DateiIn.ReadLine()
            LPCFile.myMetadata.recommendedMode = CType(DateiIn.ReadLine(), Byte)
            LPCFile.myMetadata.additionalData = DateiIn.ReadLine()
            For r As Integer = 0 To 3
                For c As Integer = 0 To 3
                    LPCFile.myMetadata.matrix(r, c) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Next
            Next
            For i As Integer = 0 To 11
                LPCFile.myMetadata.mData(i) = DateiIn.ReadLine()
            Next i
            Do While DateiIn.ReadLine() = "--Metadata--"
                Dim mData(11) As String
                Dim key As String = DateiIn.ReadLine()
                For i As Integer = 0 To 11
                    mData(i) = DateiIn.ReadLine()
                Next i
                If LPCFile.PrimitivesMetadataHMap.ContainsKey(key) Then
                    LPCFile.PrimitivesMetadataHMap.Remove(key)
                    LPCFile.PrimitivesMetadataHMap.Add(key, New Metadata(mData(0), mData(1), mData(2), mData(3), mData(4), mData(5), mData(6), mData(7), mData(8), mData(9), mData(10), mData(11)))
                End If
            Loop

            For Each key As String In LPCFile.PrimitivesMetadataHMap.Keys
                If key Like "subfile*" AndAlso LPCFile.PrimitivesMetadataHMap(key) Is Nothing Then
                    Dim start As Integer = 0
                    Do
                        Dim pc As Integer = LPCFile.Primitives.Count - 1
                        For i As Integer = start To pc
                            If LPCFile.Primitives(i).primitiveName = key Then
                                LPCFile.Primitives.RemoveAt(i)
                                Continue Do
                            End If
                        Next
                        Exit Do
                    Loop
                End If
            Next

            Dim pc2 As Integer = LPCFile.Primitives.Count - 1
            For prim As Integer = 0 To pc2
                Dim pID As Integer = LPCFile.Primitives(prim).primitiveID
                If LPCFile.PrimitivesHMap.ContainsKey(pID) Then
                    duplicatePrimitiveIDs = True
                Else
                    LPCFile.PrimitivesHMap.Add(pID, prim)
                End If
            Next

            Dim templateShapeCount As Integer = CType(DateiIn.ReadLine(), Integer)
            For i As Integer = 0 To templateShapeCount - 1
                Dim tx, ty As Single
                tx = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Single)
                ty = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Single)
                LPCFile.templateShape.Add(New PointF(tx, ty))
            Next

            Dim templateQuadCount As Integer = CType(DateiIn.ReadLine(), Integer)
            DateiIn.ReadLine() ' old MainState.alreadyCalculatedQuads entry
            Dim transformedQuadCount As Integer = 0
            For i As Integer = 0 To templateQuadCount - 1
                Dim s() As String = Replace(DateiIn.ReadLine, ".", MathHelper.comma).Split(" ")
                Dim double_array(s.Length - 1) As Double
                For j As Integer = 0 To s.Length - 1
                    double_array(j) = CType(s(j), Double)
                Next
                LPCFile.templateProjectionQuads.Add(New ProjectionQuad(
                double_array(0), double_array(1), double_array(2), double_array(3), double_array(4),
                double_array(5), double_array(6), double_array(7), double_array(8), double_array(9),
                double_array(10), double_array(11), double_array(12), double_array(13), double_array(14),
                double_array(15), double_array(16), double_array(17), double_array(18), double_array(19)))
                For r As Integer = 0 To 3
                    For c As Integer = 0 To 3
                        LPCFile.templateProjectionQuads(i + transformedQuadCount).matrix(r, c) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
                    Next
                Next
            Next
            Dim templateTextsCount As Integer = CType(DateiIn.ReadLine(), Integer)
            For i As Integer = 0 To templateTextsCount - 1
                Dim Text As String = Replace(DateiIn.ReadLine, ".", MathHelper.comma)
                X = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                Y = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                LPCFile.templateTexts.Add(New TemplateTextInfo(X, Y, Text))
            Next
            DateiIn.ReadLine()
            LPCFile.includeMetadata = CType(DateiIn.ReadLine(), Boolean)
            GlobalIdSet.vertexIDglobal = CType(DateiIn.ReadLine(), Integer)
            GlobalIdSet.triangleIDglobal = CType(DateiIn.ReadLine(), Integer)
            GlobalIdSet.primitiveIDglobal = CType(DateiIn.ReadLine(), Integer)
            If GlobalIdSet.vertexIDglobal < maxVertexID Then GlobalIdSet.vertexIDglobal = maxVertexID
            If GlobalIdSet.triangleIDglobal < maxTriangleID Then GlobalIdSet.triangleIDglobal = maxTriangleID
            If GlobalIdSet.primitiveIDglobal < maxPrimitiveID Then GlobalIdSet.primitiveIDglobal = maxPrimitiveID
            View.imgPath = DateiIn.ReadLine()
            ImageForm.TBImage.Text = View.imgPath
            ImageForm.NUDoffsetX.Maximum = Integer.MaxValue
            ImageForm.NUDoffsetY.Maximum = Integer.MaxValue
            ImageForm.NUDScale.Maximum = Decimal.MaxValue
            ImageForm.NUDoffsetX.Minimum = Integer.MinValue
            ImageForm.NUDoffsetY.Minimum = Integer.MinValue
            ImageForm.NUDScale.Minimum = Decimal.MinValue
            View.imgOffsetX = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Integer)
            View.imgOffsetY = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Integer)
            View.imgScale = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            ImageForm.NUDoffsetX.Value = View.imgOffsetX
            ImageForm.NUDoffsetY.Value = View.imgOffsetY
            ImageForm.NUDScale.Value = View.imgScale
            View.unitFactor = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
            View.unit = DateiIn.ReadLine()
            Select Case View.unit
                Case "LDU"
                    LDUToolStripMenuItem.PerformClick()
                Case "mm"
                    MillimeterToolStripMenuItem.PerformClick()
                Case "inch"
                    InchToolStripMenuItem.PerformClick()
            End Select

            View.moveSnap = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.rotateSnap = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.scaleSnap = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            View.rasterSnap = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Decimal)
            PreferencesForm.NUDMoveSnap.Value = View.moveSnap
            PreferencesForm.NUDRotateSnap.Value = View.rotateSnap
            PreferencesForm.NUDScaleSnap.Value = View.scaleSnap
            PreferencesForm.NUDGrid.Value = View.rasterSnap

            DateiIn.ReadLine()
            Dim colourCount As Integer = CType(DateiIn.ReadLine(), Integer)
            MainState.colourReplacement = colourCount > 0
            LPCFile.colourReplacementMapBrush.Clear()
            LPCFile.oldColours.Clear()
            LPCFile.newColours.Clear()
            For i As Integer = 0 To colourCount - 1
                Dim c As Colour
                c.ldrawIndex = CType(DateiIn.ReadLine(), Integer)
                c.rgbValue = Color.FromArgb(CType(DateiIn.ReadLine(), Integer))
                LPCFile.oldColours.Add(c)
                c.ldrawIndex = CType(DateiIn.ReadLine(), Integer)
                c.rgbValue = Color.FromArgb(CType(DateiIn.ReadLine(), Integer))
                LPCFile.newColours.Add(c)
                If Not LPCFile.colourReplacementMapBrush.ContainsKey(LPCFile.oldColours(i).rgbValue.ToArgb) Then
                    If LPCFile.newColours(i).ldrawIndex = 16 Then
                        LPCFile.colourReplacementMapBrush.Add(LPCFile.oldColours(i).rgbValue.ToArgb, CType(New HatchBrush(HatchStyle.Percent05, LDSettings.Colours.linePen.Color, Color.Transparent), Brush))
                    Else
                        LPCFile.colourReplacementMapBrush.Add(LPCFile.oldColours(i).rgbValue.ToArgb, CType(New SolidBrush(LPCFile.newColours(i).rgbValue), Brush))
                    End If
                End If
            Next

            ' Since 1.3.7
            If Not DateiIn.EndOfStream Then
                DateiIn.ReadLine()
                ' Second test, because of the pre-release from 1.3.7
                If Not DateiIn.EndOfStream Then
                    LPCFile.replaceColour = CType(DateiIn.ReadLine(), Boolean)
                    LPCFile.project = CType(DateiIn.ReadLine(), Boolean)
                    LPCFile.rectify = CType(DateiIn.ReadLine(), Boolean)
                    LPCFile.unify = CType(DateiIn.ReadLine(), Boolean)
                    LPCFile.unifyLPC = CType(DateiIn.ReadLine(), Boolean)
                End If
            End If

            ' Since 1.3.8
            If Not DateiIn.EndOfStream Then
                DateiIn.ReadLine()
                For prim As Integer = 0 To LPCFile.Primitives.Count - 1
                    For z As Integer = 0 To 3
                        For s As Integer = 0 To 3
                            LPCFile.Primitives(prim).matrixR(z, s) = CType(Replace(DateiIn.ReadLine, ".", MathHelper.comma), Double)
                        Next
                    Next
                Next
            End If

            Dim backgroundPictureBegin As Boolean
            If DateiIn.ReadLine <> "--Background Image--" Then
                ' Read the opacity value (since 1.4.4)
                View.alpha = CType(DateiIn.ReadLine, Byte)
                Select Case View.alpha
                    Case 255
                        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 100%"
                    Case 127
                        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 50%"
                    Case 64
                        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 25%"
                    Case 26
                        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 10%"
                    Case Else
                        View.alpha = 255
                        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 100%"
                End Select
            Else
                View.alpha = 255
                OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 100%"
                backgroundPictureBegin = True
            End If

            If Not backgroundPictureBegin AndAlso DateiIn.ReadLine <> "--Background Image--" Then
                ' Read the SlicerPro flag (since 1.6.1)              
                LPCFile.slice = CType(DateiIn.ReadLine(), Boolean)
                MetadataDialog.CBSlicerPro.Checked = LPCFile.slice
                ' TODO Read new data here
            Else
                backgroundPictureBegin = True
            End If

            ' Read the Background Picture (since 1.3.9)
            If Not DateiIn.EndOfStream Then

                If Not backgroundPictureBegin Then
                    While DateiIn.ReadLine <> "--Background Image--"
                    End While
                End If

                Dim hashValue As String = DateiIn.ReadLine()
                Dim realValue As String

                Try
                    Dim fi As FileInfo = My.Computer.FileSystem.GetFileInfo(View.imgPath)
                    realValue = fi.LastAccessTime & fi.Length
                Catch
                    realValue = hashValue
                End Try

                ' Read it only, if necessary
                If realValue = hashValue AndAlso Not View.imgPath.Equals("") AndAlso Not My.Computer.FileSystem.FileExists(View.imgPath) Then

                    Dim count16 As Integer = CType(DateiIn.ReadLine(), Integer) - 1
                    Dim ar16(count16) As Byte
                    DateiIn.BaseStream.Position = DateiIn.BaseStream.Length - count16 - 1
                    For i As Integer = 0 To count16
                        ar16(i) = DateiIn.BaseStream.ReadByte()
                    Next i

                    Dim imgPath As String = Path.GetDirectoryName(myFilename) & "\" & Path.GetFileNameWithoutExtension(View.imgPath) & ".jpg"
                    Using imageStream As New IO.MemoryStream
                        Try
                            imageStream.Write(ar16, 0, ar16.Length)
                            imageStream.Flush()
                            Using b As New Bitmap(imageStream)
                                b.Save(imgPath, Drawing.Imaging.ImageFormat.Jpeg)
                            End Using
                            View.imgPath = imgPath
                        Catch
                            View.imgPath = ""
                        End Try
                    End Using
                    ImageForm.TBImage.Text = View.imgPath
                End If


                DateiIn.ReadLine()
            End If
        End Using

        If duplicatePrimitiveIDs Then

            Dim tc As Integer = LPCFile.Triangles.Count - 1
            Dim start As Integer = 0
newDelete:
            For i As Integer = start To tc
                Dim tri As Triangle = LPCFile.Triangles(i)
                tri.groupindex = -1
                If tri.vertexA.vertexID = tri.vertexB.vertexID OrElse
                tri.vertexB.vertexID = tri.vertexC.vertexID OrElse
                tri.vertexC.vertexID = tri.vertexA.vertexID Then
                    LPCFile.Triangles.RemoveAt(i)
                    start = i
                    tc -= 1
                    GoTo newDelete
                End If
            Next

            For Each vert As Vertex In LPCFile.Vertices
                vert.groupindex = -1
            Next
            LPCFile.Primitives.Clear()
            LPCFile.PrimitivesMetadataHMap.Clear()
            LPCFile.PrimitivesHMap.Clear()
        End If

        Try
            View.backgroundPicture.Dispose()
            If View.imgPath <> "" Then
                View.backgroundPicture = New Bitmap(View.imgPath)
            Else
                View.backgroundPicture = My.Resources.temp
            End If
        Catch
            View.backgroundPicture = My.Resources.temp
        End Try

        View.backgroundPictureBrush.Dispose()
        View.backgroundPictureBrush = New TextureBrush(View.backgroundPicture)

        ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
    End Sub

    Private Sub SaveToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveToolStripButton.Click
        SaveToolStripMenuItem.PerformClick()
    End Sub

    Private Function CalculateShortFileNameAndUpdateTitle(ByVal fileName As String)
        Dim shortFileName As String = Mid(SaveAs.FileName, SaveAs.FileName.LastIndexOf("\") + 2)
        Me.Text = "LD - Pattern Creator : " + shortFileName
        Return shortFileName
    End Function

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If SaveAs.FileName <> "" Then
            CalculateShortFileNameAndUpdateTitle(SaveAs.FileName)

            Dim uni As UnicodeEncoding = New System.Text.UnicodeEncoding()
            If SaveAs.FileName Like "*.lpc" Then
                Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(SaveAs.FileName, False, uni)
                    DateiOut.WriteLine("0 LDraw - Pattern Creator 1.3 Unicode File")
                    ' Dreiecke speichern:
                    DateiOut.WriteLine(LPCFile.Triangles.Count)
                    For Each tri As Triangle In LPCFile.Triangles
                        DateiOut.WriteLine(tri.triangleID)
                        DateiOut.WriteLine(tri.myColour.ToArgb)
                        DateiOut.WriteLine(tri.myColourNumber)
                        DateiOut.WriteLine(tri.vertexA.vertexID)
                        DateiOut.WriteLine(tri.vertexB.vertexID)
                        DateiOut.WriteLine(tri.vertexC.vertexID)
                        DateiOut.WriteLine(tri.groupindex)
                    Next
                    ' LPCFile.Vertices speichern:
                    DateiOut.WriteLine(LPCFile.Vertices.Count)
                    For Each vert As Vertex In LPCFile.Vertices
                        DateiOut.WriteLine(vert.vertexID)
                        DateiOut.WriteLine(I18N.globalize(vert.X))
                        DateiOut.WriteLine(I18N.globalize(vert.Y))
                        DateiOut.WriteLine(vert.groupindex)
                        DateiOut.WriteLine(vert.linkedTriangles.Count)
                        For Each tri As Triangle In vert.linkedTriangles
                            DateiOut.WriteLine(tri.triangleID)
                        Next
                    Next
                    ' Primitive speichern:
                    DateiOut.WriteLine(LPCFile.Primitives.Count)
                    For Each prim As Primitive In LPCFile.Primitives
                        DateiOut.WriteLine(prim.primitiveID)
                        DateiOut.WriteLine(prim.centerVertexID)
                        DateiOut.WriteLine(I18N.globalize(prim.ox))
                        DateiOut.WriteLine(I18N.globalize(prim.oy))
                        DateiOut.WriteLine(prim.primitiveName)
                        DateiOut.WriteLine(prim.myColour.ToArgb)
                        DateiOut.WriteLine(prim.myColourNumber)
                        For z As Integer = 0 To 3
                            For s As Integer = 0 To 3
                                DateiOut.WriteLine(I18N.globalize(prim.matrix(z, s)))
                            Next
                        Next
                    Next
                    ' Metadata speichern:
                    DateiOut.WriteLine("--Metadata--")
                    DateiOut.WriteLine(LPCFile.myMetadata.recommendedMode)
                    DateiOut.WriteLine(LPCFile.myMetadata.additionalData)
                    For r As Integer = 0 To 3
                        For c As Integer = 0 To 3
                            DateiOut.WriteLine(I18N.globalize(LPCFile.myMetadata.matrix(r, c)))
                        Next
                    Next
                    For i As Integer = 0 To 11
                        DateiOut.WriteLine(LPCFile.myMetadata.mData(i))
                    Next i

                    Dim prims As New Hashtable(LPCFile.Primitives.Count)
                    For Each i As Integer In LPCFile.PrimitivesHMap.Values
                        If Not prims.Contains(LPCFile.Primitives(i).primitiveName) Then
                            prims.Add(LPCFile.Primitives(i).primitiveName, Nothing)
                            If LPCFile.Primitives(i).primitiveName Like "subfile*" OrElse LPCFile.Primitives(i).primitiveName Like "s\*" Then
                                Dim m As Metadata = LPCFile.PrimitivesMetadataHMap.Item(LPCFile.Primitives(i).primitiveName)
                                DateiOut.WriteLine("--Metadata--")
                                DateiOut.WriteLine(LPCFile.Primitives(i).primitiveName)
                                For j As Integer = 0 To 11
                                    DateiOut.WriteLine(m.mData(j))
                                Next
                            End If
                        End If
                    Next
                    DateiOut.WriteLine("--Template--")
                    DateiOut.WriteLine(LPCFile.templateShape.Count)
                    For i As Integer = 0 To LPCFile.templateShape.Count - 1
                        DateiOut.WriteLine(I18N.globalize(LPCFile.templateShape(i).X))
                        DateiOut.WriteLine(I18N.globalize(LPCFile.templateShape(i).Y))
                    Next
                    DateiOut.WriteLine(LPCFile.templateProjectionQuads.Count)
                    DateiOut.WriteLine(0)
                    For i As Integer = 0 To LPCFile.templateProjectionQuads.Count - 1
                        DateiOut.WriteLine(I18N.globalize(LPCFile.templateProjectionQuads(i).toString()))
                        For r As Integer = 0 To 3
                            For c As Integer = 0 To 3
                                DateiOut.WriteLine(I18N.globalize(LPCFile.templateProjectionQuads(i).matrix(r, c)))
                            Next
                        Next
                    Next
                    DateiOut.WriteLine(LPCFile.templateTexts.Count)
                    For i = 0 To LPCFile.templateTexts.Count - 1
                        DateiOut.WriteLine(LPCFile.templateTexts(i).Text)
                        DateiOut.WriteLine(I18N.globalize(LPCFile.templateTexts(i).X))
                        DateiOut.WriteLine(I18N.globalize(LPCFile.templateTexts(i).Y))
                    Next
                    DateiOut.WriteLine("--Settings--")

                    DateiOut.WriteLine(LPCFile.includeMetadata.ToString)
                    DateiOut.WriteLine(GlobalIdSet.vertexIDglobal)
                    DateiOut.WriteLine(GlobalIdSet.triangleIDglobal)
                    DateiOut.WriteLine(GlobalIdSet.primitiveIDglobal)

                    DateiOut.WriteLine(View.imgPath)
                    DateiOut.WriteLine(I18N.globalize(Fix(View.imgOffsetX)))
                    DateiOut.WriteLine(I18N.globalize(Fix(View.imgOffsetY)))
                    DateiOut.WriteLine(I18N.globalize(View.imgScale))

                    DateiOut.WriteLine(I18N.globalize(View.unitFactor))
                    DateiOut.WriteLine(View.unit)
                    DateiOut.WriteLine(I18N.globalize(View.moveSnap))
                    DateiOut.WriteLine(I18N.globalize(View.rotateSnap))
                    DateiOut.WriteLine(I18N.globalize(View.scaleSnap))
                    DateiOut.WriteLine(I18N.globalize(View.rasterSnap))

                    DateiOut.WriteLine("--Colour Replacements--")
                    DateiOut.WriteLine(LPCFile.newColours.Count)
                    For i As Integer = 0 To LPCFile.newColours.Count - 1
                        DateiOut.WriteLine(LPCFile.oldColours(i).ldrawIndex)
                        DateiOut.WriteLine(LPCFile.oldColours(i).rgbValue.ToArgb)
                        DateiOut.WriteLine(LPCFile.newColours(i).ldrawIndex)
                        DateiOut.WriteLine(LPCFile.newColours(i).rgbValue.ToArgb)
                    Next

                    DateiOut.WriteLine("--Export Configuration--")

                    DateiOut.WriteLine(LPCFile.replaceColour.ToString)
                    DateiOut.WriteLine(LPCFile.project.ToString)
                    DateiOut.WriteLine(LPCFile.rectify.ToString)
                    DateiOut.WriteLine(LPCFile.unify.ToString)
                    DateiOut.WriteLine(LPCFile.unifyLPC.ToString)

                    DateiOut.WriteLine("--Additional Subfile Matrices--")
                    For Each prim As Primitive In LPCFile.Primitives
                        For r As Integer = 0 To 3
                            For c As Integer = 0 To 3
                                DateiOut.WriteLine(I18N.globalize(prim.matrixR(r, c)))
                            Next
                        Next
                    Next

                    DateiOut.WriteLine("--Opacity--")
                    DateiOut.WriteLine(View.alpha)

                    DateiOut.WriteLine("--Pattern Slicing--")
                    DateiOut.WriteLine(LPCFile.slice.ToString)

                    ' Write this always to the end
                    DateiOut.WriteLine("--Background Image--")
                    Try
                        Dim fi As FileInfo = My.Computer.FileSystem.GetFileInfo(View.imgPath)
                        DateiOut.WriteLine(fi.LastAccessTime & fi.Length)
                    Catch
                        DateiOut.WriteLine(0)
                    End Try
                    Using MS As New IO.MemoryStream
                        View.backgroundPicture.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg)
                        MS.Flush()
                        Dim ba() As Byte = MS.ToArray
                        DateiOut.WriteLine(ba.Length)
                        DateiOut.Flush()
                        For Each b As Byte In ba
                            DateiOut.BaseStream.WriteByte(b)
                        Next
                        DateiOut.BaseStream.Flush()
                    End Using
                End Using

            Else

                ' V1.0 Legacy Mode
                Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(SaveAs.FileName, False, System.Text.Encoding.ASCII)
                    DateiOut.WriteLine("0 LDraw - Pattern Creator 1.0 ASCII File")
                    ' Dreiecke speichern:
                    DateiOut.WriteLine(LPCFile.Triangles.Count)
                    For Each tri As Triangle In LPCFile.Triangles
                        DateiOut.WriteLine(tri.triangleID)
                        DateiOut.WriteLine(tri.myColour.ToArgb)
                        DateiOut.WriteLine(tri.myColourNumber)
                        DateiOut.WriteLine(tri.vertexA.vertexID)
                        DateiOut.WriteLine(tri.vertexB.vertexID)
                        DateiOut.WriteLine(tri.vertexC.vertexID)
                    Next
                    ' Vertices speichern:
                    DateiOut.WriteLine(LPCFile.Vertices.Count)
                    For Each vert As Vertex In LPCFile.Vertices
                        DateiOut.WriteLine(vert.vertexID)
                        DateiOut.WriteLine(I18N.globalize(-vert.X))
                        DateiOut.WriteLine(I18N.globalize(-vert.Y))
                        DateiOut.WriteLine(vert.linkedTriangles.Count)
                        For Each tri As Triangle In vert.linkedTriangles
                            DateiOut.WriteLine(tri.triangleID)
                        Next
                    Next
                    ' Einstellungen / IDs
                    DateiOut.WriteLine(GlobalIdSet.vertexIDglobal)
                    DateiOut.WriteLine(GlobalIdSet.triangleIDglobal)
                    DateiOut.WriteLine(View.imgPath)
                    DateiOut.WriteLine(I18N.globalize(Fix(View.imgOffsetX)))
                    DateiOut.WriteLine(I18N.globalize(Fix(View.imgOffsetY)))
                    DateiOut.WriteLine(I18N.globalize(View.imgScale))
                    DateiOut.WriteLine(I18N.globalize(View.unitFactor))
                    DateiOut.WriteLine(View.unit)
                    DateiOut.WriteLine(I18N.globalize(View.moveSnap))
                    DateiOut.WriteLine(I18N.globalize(View.rotateSnap))
                    DateiOut.WriteLine(I18N.globalize(View.scaleSnap))
                    DateiOut.WriteLine(I18N.globalize(View.rasterSnap))
                End Using
            End If
            Dim ti As New ToolStripMenuItem
            Dim file As String = SaveAs.FileName
            ti.Text = Mid(file, file.LastIndexOf("\") + 2)
            ti.ToolTipText = file
            AddHandler ti.Click, AddressOf recentFileNameClick
            If EnvironmentPaths.recentFiles.Contains(file) Then
                EnvironmentPaths.recentFiles.Remove(file)
                EnvironmentPaths.recentFiles.Insert(0, file)
                For i As Integer = 0 To LoadPatternToolStripMenuItem.DropDownItems.Count - 1
                    If file = LoadPatternToolStripMenuItem.DropDownItems(i).ToolTipText Then
                        LoadPatternToolStripMenuItem.DropDownItems.RemoveAt(i)
                        Exit For
                    End If
                Next
                LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
            ElseIf EnvironmentPaths.recentFiles(9) = "" Then
                LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
                For i As Integer = 0 To 9
                    If EnvironmentPaths.recentFiles(i) = "" Then
                        EnvironmentPaths.recentFiles(i) = file
                        LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
                        Exit For
                    End If
                Next
            Else
                EnvironmentPaths.recentFiles.RemoveAt(9)
                EnvironmentPaths.recentFiles.Insert(0, file)
                LoadPatternToolStripMenuItem.DropDownItems.RemoveAt(9)
                LoadPatternToolStripMenuItem.DropDownItems.Insert(0, ti)
            End If
            saveConfig()
            MainState.unsavedChanges = False
        Else
            SaveAsToolStripMenuItem.PerformClick()
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim result As DialogResult
        result = SaveAs.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            If SaveAs.FileName <> "" Then
                SaveToolStripMenuItem.PerformClick()
            End If
        End If
    End Sub

    Private Function useRecommendedProjection(ByVal projectMode As Byte) As Boolean
        If LPCFile.myMetadata.recommendedMode <> 6 AndAlso LPCFile.myMetadata.recommendedMode <> projectMode Then
            Dim shouldBe As String = ""
            Select Case LPCFile.myMetadata.recommendedMode
                Case 0 : shouldBe = I18N.trl8(I18N.lk.Plane1)
                Case 1 : shouldBe = I18N.trl8(I18N.lk.Plane2)
                Case 2 : shouldBe = I18N.trl8(I18N.lk.Plane3)
                Case 3 : shouldBe = I18N.trl8(I18N.lk.Plane4)
                Case 4 : shouldBe = I18N.trl8(I18N.lk.Plane5)
                Case 5 : shouldBe = I18N.trl8(I18N.lk.Plane6)
            End Select
            Dim isInstead As String = ""
            Select Case projectMode
                Case 0 : isInstead = I18N.trl8(I18N.lk.Plane1)
                Case 1 : isInstead = I18N.trl8(I18N.lk.Plane2)
                Case 2 : isInstead = I18N.trl8(I18N.lk.Plane3)
                Case 3 : isInstead = I18N.trl8(I18N.lk.Plane4)
                Case 4 : isInstead = I18N.trl8(I18N.lk.Plane5)
                Case 5 : isInstead = I18N.trl8(I18N.lk.Plane6)
            End Select
            If MsgBox(String.Format(I18N.trl8(I18N.lk.PlaneHint), shouldBe, isInstead), MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, I18N.trl8(I18N.lk.Question)) = MsgBoxResult.No Then Return False
        End If
        Return True
    End Function

    Private Function userWantsToExportDAT(ByVal projectMode As Byte) As Boolean
        Select Case projectMode
            Case 0 ' YZ mit -X (Right) [OK]
                SaveAsDAT.Title = I18N.trl8(I18N.lk.ExportDATRight)
            Case 3 ' YZ mit +X (Left) [OK]
                SaveAsDAT.Title = I18N.trl8(I18N.lk.ExportDATLeft)
            Case 1 ' XZ mit -Y (Top) [OK]
                SaveAsDAT.Title = I18N.trl8(I18N.lk.ExportDATTop)
            Case 4 ' XZ mit -Y (Bottom) [OK]
                SaveAsDAT.Title = I18N.trl8(I18N.lk.ExportDATBottom)
            Case 2 ' XY mit -Z (Front) [OK]
                SaveAsDAT.Title = I18N.trl8(I18N.lk.ExportDATFront)
            Case 5 ' XY mit +Z (Back) [OK]
                SaveAsDAT.Title = I18N.trl8(I18N.lk.ExportDATBack)
        End Select
        If (LPCFile.includeMetadata AndAlso LPCFile.myMetadata.mData(1) <> "") Then
            Dim dir As String = SaveAsDAT.InitialDirectory
            If Not dir.EndsWith("\") AndAlso dir.Length > 0 Then
                dir = dir & "\"
            End If
            If LPCFile.myMetadata.mData(4) Like "*Subpart" Then
                SaveAsDAT.FileName = dir & Mid(LPCFile.myMetadata.mData(1), 3)
            Else
                SaveAsDAT.FileName = dir & LPCFile.myMetadata.mData(1)
            End If
        End If

        Try
            Dim result As DialogResult
            result = SaveAsDAT.ShowDialog()
            If result = Windows.Forms.DialogResult.OK Then
                If SaveAsDAT.FileName <> "" Then
                    Return True
                End If
            End If
        Catch e As Exception
            ' Catches a very rare Stackoverflow Exception
            GC.Collect()
            userWantsToExportDAT(projectMode)
        End Try
        Return False
    End Function

    Public Sub performStep()
        LDSettings.StepCount += 1
        If LDSettings.StepCount = 20 Then
            LDSettings.StepCount = 0
            Try
                ExportProgressBar.PerformStep()
            Catch aoore As ArgumentOutOfRangeException

            End Try
            Me.Refresh()
        End If
    End Sub

    Public Sub exportDatThread(ByVal projectMode As Byte)
        ExportProgressBar.Visible = True
        ExportProgressBar.Value = 0
        ' Calculate Progress Maximum
        ExportProgressBar.Maximum = 0
        ExportProgressBar.Maximum += LPCFile.Triangles.Count
        If LPCFile.project Then
            ExportProgressBar.Maximum += LPCFile.templateProjectionQuads.Count
        End If
        ExportProgressBar.Step = 20
        MainState.isLoading = True
        MenuStrip1.Enabled = False
        Me.MainToolStrip.Enabled = False
        Me.ColourToolStrip.Enabled = False
        ImageForm.Enabled = False
        PreferencesForm.Enabled = False

        Dim batfile As String = Fix(Rnd() * 100000) & "sp.bat"
        Dim filePath As String = Mid(SaveAsDAT.FileName, 1, SaveAsDAT.FileName.LastIndexOf("\"))
        Dim fileType As String = Mid(SaveAsDAT.FileName, SaveAsDAT.FileName.LastIndexOf(".") + 1, 4)
        Dim fileName As String = Replace(Replace(SaveAsDAT.FileName, filePath & "\", ""), fileType, "")
        Dim partName As String
        Dim subfile As New Hashtable(LPCFile.Primitives.Count)
        Dim subfileToGroup As New Hashtable(LPCFile.Primitives.Count)
        Dim subfileToTriangle As New Hashtable(LPCFile.Primitives.Count)
        Dim vertexIDtoGroupNumber As New Dictionary(Of Integer, Integer)

        Dim counter As Integer = 1
        scaleVertices(True)

        If projectMode = LPCFile.myMetadata.recommendedMode Then

            If LPCFile.templateProjectionQuads.Count > 0 AndAlso LPCFile.project AndAlso LPCFile.slice Then
                cleanupDATVertices()
                cleanupDATTriangles()
                Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "form.dat", False, New System.Text.UTF8Encoding(False))
                    For Each q As ProjectionQuad In LPCFile.templateProjectionQuads
                        If q.isTriangle Then
                            DateiOut.WriteLine(Replace("3 10000 " &
                                               -q.inCoords(1, 0) & " 0 " & -q.inCoords(1, 1) & " " &
                                               -q.inCoords(2, 0) & " 0 " & -q.inCoords(2, 1) & " " &
                                               -q.inCoords(3, 0) & " 0 " & -q.inCoords(3, 1), MathHelper.comma, "."))
                        Else
                            DateiOut.WriteLine(Replace("4 10000 " &
                                               -q.inCoords(0, 0) & " 0 " & -q.inCoords(0, 1) & " " &
                                               -q.inCoords(1, 0) & " 0 " & -q.inCoords(1, 1) & " " &
                                               -q.inCoords(2, 0) & " 0 " & -q.inCoords(2, 1) & " " &
                                               -q.inCoords(3, 0) & " 0 " & -q.inCoords(3, 1), MathHelper.comma, "."))
                        End If
                    Next
                End Using


                Dim colorDict As New Dictionary(Of Integer, Color)

                Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "stamp.dat", False, New System.Text.UTF8Encoding(False))
                    Helper_2D.clearSelection()
                    Dim tmpColourNumber As Integer = 10001
                    For i As Integer = 0 To LPCFile.Triangles.Count - 1
                        Dim tri As Triangle = LPCFile.Triangles(i)
                        View.SelectedTriangles.Add(tri)
                        If tri.groupindex = -1 Then
                            tri.normalize()
                            tri.checkWinding()
                            If tri.myColourNumber = -1 Then
                                tri.myColourNumber = tmpColourNumber
                                colorDict.Add(tmpColourNumber, tri.myColour)
                                tmpColourNumber += 1
                            End If
                            DateiOut.WriteLine(Replace("3 " & tri.getColourString & " " & -tri.vertexA.X & " 0 " & -tri.vertexA.Y & " " _
                                                           & -tri.vertexB.X & " 0 " & -tri.vertexB.Y & " " _
                                                           & -tri.vertexC.X & " 0 " & -tri.vertexC.Y & " ", MathHelper.comma, "."))
                        End If
                    Next
                    Dim backup As Integer = MainState.objectToModify
                    MainState.objectToModify = Modified.Triangle
                    ClipboardHelper.delete()
                    MainState.objectToModify = backup
                End Using

                Dim oProcess As Process

                ' SlicerPro
                If EnvironmentPaths.ldrawPath <> "" Then

                    Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & batfile, False, New System.Text.UTF8Encoding(False))
                        DateiOut.WriteLine("cd " & EnvironmentPaths.appPath)
                        DateiOut.WriteLine("Slicerpro -y -uo -n -p 0.0000001 stamp.dat form.dat tmp.dat")
                    End Using

                    Dim psi As New ProcessStartInfo(EnvironmentPaths.appPath & batfile)
                    psi.WorkingDirectory = EnvironmentPaths.appPath
                    oProcess = System.Diagnostics.Process.Start(psi)
                    oProcess.EnableRaisingEvents = True
                    Dim ticks As Integer = 0
                    Do
                        Thread.Sleep(10)
                        ticks += 1
                        If ticks = 700 Then
                            If Not oProcess.HasExited Then oProcess.Kill()
                            My.Computer.FileSystem.CopyFile(EnvironmentPaths.appPath & "stamp.dat", EnvironmentPaths.appPath & "tmp.dat")
                            GoTo skipSlicing
                        End If
                    Loop Until My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "tmp.dat")
                    Do
                        Thread.Sleep(10)
                    Loop Until My.Computer.FileSystem.GetFileInfo(EnvironmentPaths.appPath & "tmp.dat").LastWriteTime.Ticks + 1000000 < Now.Ticks
                    Do
                        Thread.Sleep(10)
                        SendKeys.SendWait("{ENTER}")
                    Loop Until oProcess.HasExited
                    If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & batfile) Then My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & batfile)
skipSlicing:
                    ' Parse other data (triangles, quads, .. )
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "tmp.dat", New System.Text.UTF8Encoding(False))
                        Dim textline As String
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
                                If words(0) Like "*3*" AndAlso words.Length = 11 Then
                                    LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), CType(-words(4), Double), False))
                                    LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), CType(-words(7), Double), False))
                                    LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), CType(-words(10), Double), False))
                                    LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(LPCFile.Vertices.Count - 1), LPCFile.Vertices(LPCFile.Vertices.Count - 2), LPCFile.Vertices(LPCFile.Vertices.Count - 3)))
                                    ListHelper.LLast(LPCFile.Triangles).myColourNumber = CType(words(1), Short)
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                ElseIf words(0) Like "*4*" AndAlso words.Length = 14 Then

                                    LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), -CType(words(4), Double), False))
                                    LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), -CType(words(7), Double), False))
                                    LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), -CType(words(10), Double), False))

                                    LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(LPCFile.Vertices.Count - 1), LPCFile.Vertices(LPCFile.Vertices.Count - 2), LPCFile.Vertices(LPCFile.Vertices.Count - 3)))

                                    ListHelper.LLast(LPCFile.Triangles).myColourNumber = CType(words(1), Short)
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 3).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))

                                    LPCFile.Vertices.Add(New Vertex(-CType(words(11), Double), -CType(words(13), Double), False))

                                    LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(LPCFile.Vertices.Count - 1), LPCFile.Vertices(LPCFile.Vertices.Count - 2), LPCFile.Vertices(LPCFile.Vertices.Count - 4)))

                                    ListHelper.LLast(LPCFile.Triangles).myColourNumber = CType(words(1), Short)
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                    LPCFile.Vertices(LPCFile.Vertices.Count - 4).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                                End If
                            End If
                        Loop Until DateiIn.EndOfStream
                    End Using

                    For Each tri As Triangle In LPCFile.Triangles
                        If colorDict.ContainsKey(tri.myColourNumber) Then
                            tri.myColour = colorDict(tri.myColourNumber)
                            tri.myColourNumber = -1
                        End If
                    Next

                    paintTriangles()
                    cleanupDATVertices()
                    cleanupDATTriangles()

                    My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "stamp.dat")
                    My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "form.dat")
                    My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "tmp.dat")
                End If
            End If
        End If

        If filePath Like "*\s" Then filePath = Mid(filePath, 1, filePath.Length - 2)
        If LPCFile.includeMetadata Then
            If LPCFile.myMetadata.mData(2) = "" Then LPCFile.myMetadata.mData(2) = LDSettings.Editor.defaultName
            If LPCFile.myMetadata.mData(3) = "" Then LPCFile.myMetadata.mData(3) = LDSettings.Editor.defaultUser
            If LPCFile.myMetadata.mData(5) = "" Then LPCFile.myMetadata.mData(5) = LDSettings.Editor.defaultLicense
            If LPCFile.myMetadata.mData(4) Like "*Subpart" Then
                If fileName.Length < 2 OrElse fileName.LastIndexOf(CChar("s")) = -1 OrElse fileName.Chars(fileName.Length - 1) = CChar("s") Then
                    fileName = fileName + "s01"
                ElseIf Not Char.IsDigit(CChar(Mid(fileName, fileName.LastIndexOf(CChar("s")) + 2, 1))) Then
                    fileName = fileName + "s01"
                End If
                partName = Mid(fileName, 1, fileName.LastIndexOf(CChar("s")))
                LPCFile.myMetadata.mData(1) = "s\" & fileName & fileType
                If Not LPCFile.myMetadata.mData(0) Like "~*" Then
                    LPCFile.myMetadata.mData(0) = "~" & LPCFile.myMetadata.mData(0)
                End If
            Else
                If LPCFile.myMetadata.mData(4) = "" Then LPCFile.myMetadata.mData(4) = "Unofficial_Part"
                partName = fileName
                LPCFile.myMetadata.mData(1) = fileName & fileType
            End If
            Dim isSubpart As Boolean = LPCFile.myMetadata.mData(4) Like "*Subpart"
            For Each prim As Primitive In LPCFile.Primitives
                If prim.primitiveName Like "subfile*" Then
                    If Not subfile.Contains(prim.primitiveName) Then
                        If CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(2) = "" Then
                            CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(2) = LDSettings.Editor.defaultName
                        End If
                        If CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(3) = "" Then
                            CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(3) = LDSettings.Editor.defaultUser
                        End If
                        If CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(4) = "" Then
                            CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(4) = "Unofficial_Subpart"
                        End If
                        If CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(5) = "" Then
                            CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(5) = LDSettings.Editor.defaultLicense
                        End If
                        If isSubpart AndAlso LPCFile.myMetadata.mData(1) = ("s\" & partName & "s" & counter & ".dat") Then
                            counter += 1
                        End If
                        If CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(1) = "" OrElse CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(1) Like "*UNKNOWN*" Then CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(1) = "s\" & partName & "s" & counter & ".dat"
                        If Not CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(0) Like "~*" Then
                            CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(0) = "~" & CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(0)
                        End If
                        subfile.Add(prim.primitiveName, CType(LPCFile.PrimitivesMetadataHMap(prim.primitiveName), Metadata).mData(1))
                        counter += 1
                    End If
                End If
            Next
        Else
            For Each prim As Primitive In LPCFile.Primitives
                If prim.primitiveName Like "subfile*" AndAlso Not subfile.Contains(prim.primitiveName) Then
                    subfile.Add(prim.primitiveName, "s\" & fileName & "s" & counter & ".dat")
                    counter += 1
                End If
            Next
        End If

        If subfile.Count > 0 OrElse LPCFile.myMetadata.mData(4) Like "*Subpart" Then

            If Not My.Computer.FileSystem.DirectoryExists(filePath & "\s") Then
                My.Computer.FileSystem.CreateDirectory(filePath & "\s")
                Thread.Sleep(3000)
            End If

            ' Export Subfiles:
            For i As Integer = 0 To LPCFile.Triangles.Count - 1
                Dim tri As Triangle = LPCFile.Triangles(i)
                If tri.groupindex <> -1 Then
                    Dim value As String = subfile(LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).primitiveName)
                    If Not value Is Nothing AndAlso Not subfileToGroup.Contains(value) Then
                        subfileToGroup.Add(value, tri.groupindex)
                        subfileToTriangle.Add(value, i)
                    End If
                End If
            Next

            For Each key As String In subfile.Keys
                exportSubfile(filePath & "\" & subfile(key), subfileToGroup(subfile(key)), subfileToTriangle(subfile(key)))
            Next

        End If

        Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "tmp.dat", False, New System.Text.UTF8Encoding(False))
            If LPCFile.includeMetadata Then
                DateiOut.WriteLine("0 " & LPCFile.myMetadata.mData(0))
                DateiOut.WriteLine("0 Name: " & LPCFile.myMetadata.mData(1))
                DateiOut.WriteLine("0 Author: " & LPCFile.myMetadata.mData(2) & " [" & LPCFile.myMetadata.mData(3) & "]")
                DateiOut.WriteLine("0 !LDRAW_ORG " & LPCFile.myMetadata.mData(4))
                DateiOut.WriteLine("0 !LICENSE " & LPCFile.myMetadata.mData(5))
                DateiOut.WriteLine()
                If LPCFile.myMetadata.mData(6) <> "" Then
                    Dim alines() As String = Replace(LPCFile.myMetadata.mData(6), "<br>", "µ").Split("µ")
                    Dim helpWritten As Boolean
                    For i As Integer = 0 To alines.Length - 1
                        If alines(i).Trim <> "" Then DateiOut.WriteLine("0 !HELP " & alines(i)) : helpWritten = True
                    Next
                    If helpWritten Then DateiOut.WriteLine()
                End If
                If LPCFile.myMetadata.mData(7) = "BFC CERTIFY CCW" OrElse LPCFile.myMetadata.mData(7) = "BFC CERTIFY CW" OrElse LPCFile.myMetadata.mData(7) = "BFC NOCERTIFY" Then
                    DateiOut.WriteLine("0 " & LPCFile.myMetadata.mData(7))
                Else
                    DateiOut.WriteLine("0 BFC CERTIFY CCW")
                End If
                If LPCFile.myMetadata.mData(8) <> "" Then DateiOut.WriteLine("0 !CATEGORY " & LPCFile.myMetadata.mData(8))
                If LPCFile.myMetadata.mData(9) <> "" Then
                    Dim alines() As String = Replace(LPCFile.myMetadata.mData(9), "<br>", "µ").Split("µ")
                    Dim keywordsWritten As Boolean
                    For i As Integer = 0 To alines.Length - 1
                        If alines(i).Trim <> "" Then DateiOut.WriteLine("0 !KEYWORDS " & alines(i)) : keywordsWritten = True
                    Next
                    If keywordsWritten Then DateiOut.WriteLine()
                End If
                If LPCFile.myMetadata.mData(10) <> "" Then
                    Dim alines() As String = Replace(LPCFile.myMetadata.mData(10), "<br>", "µ").Split("µ")
                    Dim historyWritten As Boolean
                    For i As Integer = 0 To alines.Length - 1
                        If alines(i).Trim <> "" Then DateiOut.WriteLine("0 !HISTORY " & alines(i)) : historyWritten = True
                    Next
                    If historyWritten Then DateiOut.WriteLine()
                End If
                If LPCFile.myMetadata.mData(11) <> "" Then
                    Dim alines() As String = Replace(LPCFile.myMetadata.mData(11), "<br>", "µ").Split("µ")
                    Dim commentWritten As Boolean
                    For i As Integer = 0 To alines.Length - 1
                        If alines(i).Trim <> "" Then DateiOut.WriteLine("0 // " & alines(i)) : commentWritten = True
                    Next
                    If commentWritten Then DateiOut.WriteLine()
                End If
            Else
                DateiOut.WriteLine("0 BFC CERTIFY CCW")
            End If

            If projectMode = LPCFile.myMetadata.recommendedMode Then
                If LPCFile.myMetadata.additionalData <> "" Then
                    Dim alines() As String = Replace(LPCFile.myMetadata.additionalData & "<br>", "<br>", "µ").Split("µ")
                    For i As Integer = 0 To alines.Length - 1
                        DateiOut.WriteLine(alines(i))
                    Next
                End If
                If LPCFile.templateProjectionQuads.Count > 0 AndAlso LPCFile.project Then
                    For i As Integer = 0 To LPCFile.templateProjectionQuads.Count - 1
                        performStep()
                        LPCFile.templateProjectionQuads(i).CalculateMatrix()
                        If Not LPCFile.templateProjectionQuads(i).isTriangle Then
                            If Not LPCFile.templateProjectionQuads(i).isFlat Then
                                Dim q As ProjectionQuad = LPCFile.templateProjectionQuads(i).triangulate()
                                LPCFile.templateProjectionQuads(i).CalculateMatrix()
                                q.CalculateMatrix()
                                LPCFile.templateProjectionQuads.Add(q)
                                LDSettings.ExportAgain = True
                            End If
                        End If
                    Next
                    If LDSettings.ExportAgain Then
                        If LPCFile.templateShape(0).X = Single.Epsilon Then
                            LPCFile.templateShape.Clear()
                            LPCFile.templateShape.Add(New PointF(Single.Epsilon, Single.Epsilon))
                            For Each q As ProjectionQuad In LPCFile.templateProjectionQuads
                                For i As Integer = 0 To 3
                                    LPCFile.templateShape.Add(New PointF(CType(q.inCoords(i, 0) * 1000, Single), CType(q.inCoords(i, 1) * 1000, Single)))
                                Next
                            Next
                        Else
                            For Each q As ProjectionQuad In LPCFile.templateProjectionQuads
                                If q.isTriangle Then
                                    LPCFile.templateShape.Add(New PointF(Single.Epsilon, Single.Epsilon))
                                    For i As Integer = 1 To 3
                                        LPCFile.templateShape.Add(New PointF(CType(q.inCoords(i, 0) * 1000, Single), CType(q.inCoords(i, 1) * 1000, Single)))
                                    Next
                                End If
                            Next
                        End If
                    End If
                    For i As Integer = 0 To LPCFile.Vertices.Count - 1
                        vertexIDtoGroupNumber.Add(LPCFile.Vertices(i).vertexID, LPCFile.Vertices(i).groupindex)
                    Next
                End If
            End If

            For i As Integer = 0 To LPCFile.Primitives.Count - 1
                Dim prim As Primitive = LPCFile.Primitives(i)
                If Not prim.primitiveName Like "subfile*" AndAlso Not prim.primitiveName Like "s\*" Then
                    Dim matrix(3, 3) As Double
                    If LPCFile.templateProjectionQuads.Count > 0 AndAlso LPCFile.project AndAlso projectMode = LPCFile.myMetadata.recommendedMode Then
                        matrix = adjustPrimitiveMatrix(prim.matrix, projectMode, vertexIDtoGroupNumber(prim.centerVertexID))
                    Else
                        matrix = adjustPrimitiveMatrix(prim.matrix, projectMode)
                    End If
                    DateiOut.WriteLine(Replace("1 " & prim.getColourString & " " &
                                               matrix(0, 3) & " " & matrix(1, 3) & " " & matrix(2, 3) & " " &
                                               matrix(0, 0) & " " & matrix(0, 1) & " " & matrix(0, 2) & " " &
                                               matrix(1, 0) & " " & matrix(1, 1) & " " & matrix(1, 2) & " " &
                                               matrix(2, 0) & " " & matrix(2, 1) & " " & matrix(2, 2) &
                                               " " & prim.primitiveName, MathHelper.comma, "."))
                Else
                    Dim matrix(3, 3) As Double
                    If LPCFile.templateProjectionQuads.Count > 0 AndAlso LPCFile.project Then
                        matrix = adjustSubfileMatrix(prim.matrix, prim.matrixR, projectMode, prim.primitiveName Like "s\*", vertexIDtoGroupNumber(prim.centerVertexID))
                    Else
                        matrix = adjustSubfileMatrix(prim.matrix, prim.matrixR, projectMode, prim.primitiveName Like "s\*")
                    End If
                    If prim.primitiveName Like "s\*" Then
                        DateiOut.WriteLine(Replace("1 " & prim.getColourString & " " &
                                                      matrix(0, 3) & " " & matrix(1, 3) & " " & matrix(2, 3) & " " &
                                                      matrix(0, 0) & " " & matrix(0, 1) & " " & matrix(0, 2) & " " &
                                                      matrix(1, 0) & " " & matrix(1, 1) & " " & matrix(1, 2) & " " &
                                                      matrix(2, 0) & " " & matrix(2, 1) & " " & matrix(2, 2) &
                                                      " " & prim.primitiveName, MathHelper.comma, "."))
                    Else
                        DateiOut.WriteLine(Replace("1 " & prim.getColourString & " " &
                                                      matrix(0, 3) & " " & matrix(1, 3) & " " & matrix(2, 3) & " " &
                                                      matrix(0, 0) & " " & matrix(0, 1) & " " & matrix(0, 2) & " " &
                                                      matrix(1, 0) & " " & matrix(1, 1) & " " & matrix(1, 2) & " " &
                                                      matrix(2, 0) & " " & matrix(2, 1) & " " & matrix(2, 2) &
                                                      " " & subfile(prim.primitiveName), MathHelper.comma, "."))
                    End If
                End If
            Next
            If LPCFile.Triangles.Count > 0 Then
                If projectMode = LPCFile.myMetadata.recommendedMode Then
                    adjustAndSaveTriangles(projectMode, DateiOut)
                Else
                    For i As Integer = 0 To LPCFile.Triangles.Count - 1
                        Dim tri As Triangle = LPCFile.Triangles(i)
                        If tri.groupindex = -1 Then
                            tri.normalize()
                            tri.checkWinding()
                            Select Case projectMode
                                Case 0 : DateiOut.WriteLine(Replace("3 " & tri.getColourString & " 0 " & tri.vertexA.Y & " " & tri.vertexA.X & " 0 " _
                                                               & tri.vertexB.Y & " " & tri.vertexB.X & " 0 " _
                                                               & tri.vertexC.Y & " " & tri.vertexC.X & " ", MathHelper.comma, "."))
                                Case 1 : DateiOut.WriteLine(Replace("3 " & tri.getColourString & " " & -tri.vertexA.X & " 0 " & -tri.vertexA.Y & " " _
                                                           & -tri.vertexB.X & " 0 " & -tri.vertexB.Y & " " _
                                                           & -tri.vertexC.X & " 0 " & -tri.vertexC.Y & " ", MathHelper.comma, "."))
                                Case 2 : DateiOut.WriteLine(Replace("3 " & tri.getColourString & " " & -tri.vertexA.X & " " & tri.vertexA.Y & " 0 " _
                                                           & -tri.vertexB.X & " " & tri.vertexB.Y & " 0 " _
                                                           & -tri.vertexC.X & " " & tri.vertexC.Y & " 0", MathHelper.comma, "."))
                                Case 3 : DateiOut.WriteLine(Replace("3 " & tri.getColourString & " 0 " & tri.vertexA.Y & " " & -tri.vertexA.X & " 0 " _
                                                               & tri.vertexB.Y & " " & -tri.vertexB.X & " 0 " _
                                                               & tri.vertexC.Y & " " & -tri.vertexC.X & " ", MathHelper.comma, "."))
                                Case 4 : DateiOut.WriteLine(Replace("3 " & tri.getColourString & " " & -tri.vertexA.X & " 0 " & tri.vertexA.Y & " " _
                                                            & -tri.vertexB.X & " 0 " & tri.vertexB.Y & " " _
                                                            & -tri.vertexC.X & " 0 " & tri.vertexC.Y & " ", MathHelper.comma, "."))
                                Case 5 : DateiOut.WriteLine(Replace("3 " & tri.getColourString & " " & tri.vertexA.X & " " & tri.vertexA.Y & " 0 " _
                                                           & tri.vertexB.X & " " & tri.vertexB.Y & " 0 " _
                                                           & tri.vertexC.X & " " & tri.vertexC.Y & " 0", MathHelper.comma, "."))
                            End Select
                        End If
                    Next
                End If
            End If
        End Using

        If LPCFile.includeMetadata Then
            rectifyAndUnify(filePath & "\" & LPCFile.myMetadata.mData(1))
        Else
            rectifyAndUnify(SaveAsDAT.FileName)
        End If

        scaleVertices(False)
        If LPCFile.templateProjectionQuads.Count > 0 Then
            Helper_2D.clearSelection()
        End If
        MenuStrip1.Enabled = True
        Me.MainToolStrip.Enabled = True
        Me.ColourToolStrip.Enabled = True
        ImageForm.Enabled = True
        PreferencesForm.Enabled = True
        UndoRedoHelper.addHistory()
        MainState.isLoading = False
        ExportProgressBar.Visible = False
        Me.Refresh()
    End Sub

    Private Sub NewPatternToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewPatternToolStripMenuItem.Click
        If MainState.unsavedChanges AndAlso ShowAllWarningsToolStripMenuItem.Checked Then
            Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.UnsavedChanges), MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
            If result = MsgBoxResult.Yes Then SaveToolStripMenuItem.PerformClick()
            If result = MsgBoxResult.Cancel Then Exit Sub
        End If
        MainState.isLoading = True
        newPattern()
        UndoRedoHelper.addHistory()
        MainState.isLoading = False
        MainState.unsavedChanges = False
        Me.Refresh()
    End Sub

    Private Sub newPattern()
        View.alpha = 255
        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 100%"
        MainState.isLoading = True
        MainState.colourReplacement = False
        LPCFile.colourReplacementMapBrush.Clear()
        LPCFile.oldColours.Clear()
        LPCFile.newColours.Clear()
        DetectOverlapsToolStripMenuItem.Checked = False
        LPCFile.replaceColour = True
        LPCFile.project = True
        LPCFile.rectify = True
        LPCFile.unify = True
        LPCFile.unifyLPC = True
        LPCFile.slice = False
        BtnBFC.Checked = False
        LPCFile.includeMetadata = True
        View.backgroundPicture.Dispose()
        View.backgroundPicture = My.Resources.temp
        View.backgroundPictureBrush.Dispose()
        View.backgroundPictureBrush = New TextureBrush(View.backgroundPicture)
        View.SelectedTriangles.Clear()
        View.SelectedVertices.Clear()
        View.CollisionVertices.Clear()
        BtnNextOverlap.Visible = False
        BtnPrevOverlap.Visible = False
        LPCFile.Vertices.Clear()
        LPCFile.Triangles.Clear()
        LPCFile.PrimitivesHMap.Clear()
        LPCFile.Primitives.Clear()
        LPCFile.PrimitivesMetadataHMap.Clear()
        LPCFile.myMetadata = New Metadata() With {.isMainMetadata = True}
        For r As Integer = 0 To 3
            For c As Integer = 0 To 3
                LPCFile.myMetadata.matrix(r, c) = 0
                If c = r Then LPCFile.myMetadata.matrix(r, c) = 1
            Next
        Next
        LPCFile.templateShape.Clear()
        LPCFile.templateProjectionQuads.Clear()
        LPCFile.templateTexts.Clear()
        GlobalIdSet.vertexIDglobal = 0
        GlobalIdSet.triangleIDglobal = 0
        GlobalIdSet.primitiveIDglobal = 0
        View.imgPath = ""
        ImageForm.TBImage.Text = View.imgPath
        Me.GBVertex.Visible = False
        GBMatrix.Visible = False
        SaveAs.FileName = ""
        SaveAsDAT.FileName = ""
        View.moveSnap = 100 * View.unitFactor
        View.scaleSnap = 0.01
        View.rotateSnap = 15
        View.imgScale = 100
        View.imgOffsetX = 0
        View.imgOffsetY = 0
        View.rasterSnap = 1 * View.unitFactor
        UndoRedoHelper.clearHistory()
        MainState.isLoading = False
        If BtnPreview.Checked Then BtnPreview.PerformClick()
        Me.Refresh()
        GC.Collect()
    End Sub

#End Region

#Region "Primitives"

#Region "Basic Functions"
    Public Sub startPrimitiveMode()
        MainState.primitiveMode = PrimitiveModes.SetTheOrigin
        Me.MenuStrip1.Enabled = False
        For Each t As ToolStripMenuItem In Me.MenuStrip1.Items
            For Each e As Object In t.DropDownItems
                e.Tag = e.Enabled
                e.Enabled = False
            Next
        Next
        Me.MainToolStrip.Visible = False
        Me.ColourToolStrip.Tag = Me.ColourToolStrip.Visible
        Me.BtnAbort.Visible = True
        TrianglesModeToolStripMenuItem.PerformClick()
        Me.ColourToolStrip.Visible = False
        Helper_2D.clearSelection()
    End Sub

    Public Sub abortPrimitiveMode()
        If View.SelectedTriangles.Count > 0 AndAlso View.SelectedVertices.Count > 0 Then
            BtnAddToGroup.Enabled = True
            BtnUngroup.Enabled = False
        Else
            BtnAddToGroup.Enabled = False
        End If
        MainState.primitiveMode = PrimitiveModes.Inactive
        Me.MenuStrip1.Enabled = True
        For Each t As ToolStripMenuItem In Me.MenuStrip1.Items
            For Each e As Object In t.DropDownItems
                e.Enabled = e.Tag
            Next
        Next
        Me.MainToolStrip.Visible = True
        Me.ColourToolStrip.Visible = Me.ColourToolStrip.Tag
        Me.BtnAbort.Visible = False
        Me.MenuStrip1.Visible = True
        Me.BtnAbort.Text = I18N.trl8(I18N.lk.ABORT) & " []" : KeyToSet.setKey(BtnAbort, LDSettings.Keys.Abort)
        Me.BtnAbort.BackColor = Color.Red
    End Sub
#End Region

#Region "LDraw LPCFile.Primitives"
    Private Sub CircularRingSegmentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CircularRingSegmentToolStripMenuItem.Click
        RingDialog.hiRes = False
        Dim result As DialogResult
        result = RingDialog.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            MainState.primitiveObject = 255
            MainState.primitiveNewName = RingDialog.pname
            startPrimitiveMode()
        End If
    End Sub

    Private Sub CircularRingSegment48ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CircularRingSegment48ToolStripMenuItem.Click
        RingDialog.hiRes = True
        Dim result As DialogResult
        result = RingDialog.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            MainState.primitiveObject = 255
            MainState.primitiveNewName = RingDialog.pname
            startPrimitiveMode()
        End If
    End Sub
#End Region

#Region "Event Handler"
    Private Sub BtnAbort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbort.Click
        abortPrimitiveMode()
        MainState.adjustmode = False
        MainState.doAdjust = False
        GBSpline.Visible = False
        MainState.Splines.Clear()
        SBZoom.Focus()
        Me.Refresh()
    End Sub

    Private Sub SolidTriangleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SolidTriangleToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.SolidTriangle
        startPrimitiveMode()
    End Sub

    Private Sub TriangleWithFrameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TriangleWithFrameToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.TriangleWithFrame
        startPrimitiveMode()
    End Sub

    Private Sub HollowTriangleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HollowTriangleToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.HollowTriangle
        startPrimitiveMode()
    End Sub

    Private Sub SolidRectangleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SolidRectangleToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.SolidRectangle
        startPrimitiveMode()
    End Sub

    Private Sub RectangleWithFrameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangleWithFrameToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.RectangleWithFrame
        startPrimitiveMode()
    End Sub

    Private Sub HollowRectangleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HollowRectangleToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.HollowRectangle
        startPrimitiveMode()
    End Sub

    Private Sub SolidCircleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SolidCircleToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.SolidCircle
        startPrimitiveMode()
    End Sub

    Private Sub CircleWithFrameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CircleWithFrameToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.CircleWithFrame
        startPrimitiveMode()
    End Sub

    Private Sub HollowCircleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HollowCircleToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.HollowCircle
        startPrimitiveMode()
    End Sub

    Private Sub SolidOvalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SolidOvalToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.SolidOval
        startPrimitiveMode()
    End Sub

    Private Sub OvalWithFrameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OvalWithFrameToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.OvalWithFrame
        startPrimitiveMode()
    End Sub

    Private Sub HollowOvalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HollowOvalToolStripMenuItem.Click
        MainState.primitiveObject = PrimitiveObjects.HollowOval
        startPrimitiveMode()
    End Sub

    Private Sub ChainToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChainToolStripMenuItem.Click
        If LPCFile.Vertices.Count > 1 Then
            startPrimitiveMode()
            MainState.primitiveMode = PrimitiveModes.CreateTriangleChain
        End If
    End Sub

    Private Sub CircleSegments_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CircleSegments.TextChanged
        Try
            LDSettings.Editor.segments_circle = CType(CircleSegments.Text, Integer)
            If LDSettings.Editor.segments_circle < 3 Then LDSettings.Editor.segments_circle = 3
        Catch ex As Exception
            CircleSegments.Text = LDSettings.Editor.segments_circle
        End Try
    End Sub

    Private Sub OvalSegments_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OvalSegments.TextChanged
        Try
            LDSettings.Editor.segments_oval = CType(OvalSegments.Text, Integer)
            If LDSettings.Editor.segments_oval < 3 Then LDSettings.Editor.segments_oval = 3
        Catch ex As Exception
            OvalSegments.Text = LDSettings.Editor.segments_oval
        End Try
    End Sub

    Private Sub CircleRadius_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CircleRadius.TextChanged
        If CircleRadius.Text = "" Then LDSettings.Editor.radius_circle = 0 : Return
        Try
            LDSettings.Editor.radius_circle = CType(CircleRadius.Text, Double) * 1000 / View.unitFactor
            If LDSettings.Editor.radius_circle < 0 Then LDSettings.Editor.radius_circle *= -1
        Catch ex As Exception
            CircleRadius.Text = LDSettings.Editor.radius_circle / 1000
        End Try
    End Sub

    Private Sub CircleInnerRadius_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CircleInnerRadius.TextChanged
        If CircleInnerRadius.Text = "" Then LDSettings.Editor.radiusInner_circle = 0 : Return
        Try
            LDSettings.Editor.radiusInner_circle = CType(CircleInnerRadius.Text, Double) * 1000 / View.unitFactor
            If LDSettings.Editor.radiusInner_circle < 0 Then LDSettings.Editor.radiusInner_circle *= -1
        Catch ex As Exception
            CircleInnerRadius.Text = LDSettings.Editor.radiusInner_circle / 1000
        End Try
    End Sub

    Private Sub OvalRadiusX_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OvalRadiusX.TextChanged
        If OvalRadiusX.Text = "" Then LDSettings.Editor.radius_oval_x = 0 : Return
        Try
            LDSettings.Editor.radius_oval_x = CType(OvalRadiusX.Text, Double) * 1000 / View.unitFactor
            If LDSettings.Editor.radius_oval_x < 0 Then LDSettings.Editor.radius_oval_x *= -1
        Catch ex As Exception
            OvalRadiusX.Text = LDSettings.Editor.radius_oval_x / 1000
        End Try
    End Sub

    Private Sub OvalRadiusY_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OvalRadiusY.TextChanged
        If OvalRadiusY.Text = "" Then LDSettings.Editor.radius_oval_y = 0 : Return
        Try
            LDSettings.Editor.radius_oval_y = CType(OvalRadiusY.Text, Double) * 1000 / View.unitFactor
            If LDSettings.Editor.radius_oval_y < 0 Then LDSettings.Editor.radius_oval_y *= -1
        Catch ex As Exception
            OvalRadiusY.Text = LDSettings.Editor.radius_oval_y / 1000
        End Try
    End Sub
#End Region
#End Region

#Region "CMS"
    Private Sub CMSSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSSelect.Click
        BtnSelect.PerformClick()
    End Sub

    Private Sub CMSMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSMove.Click
        BtnMove.PerformClick()
    End Sub

    Private Sub CMSRotate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSRotate.Click
        BtnRotate.PerformClick()
    End Sub

    Private Sub CMSScale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSScale.Click
        BtnScale.PerformClick()
    End Sub

    Private Sub CMSVertex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSVertex.Click
        VerticesModeToolStripMenuItem.PerformClick()
    End Sub

    Private Sub CMSTriangle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSTriangle.Click
        TrianglesModeToolStripMenuItem.PerformClick()
    End Sub

    Private Sub CMSPrimitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSPrimitive.Click
        PrimitiveModeToolStripMenuItem.PerformClick()
    End Sub

    Private Sub CMSAddVertex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSAddVertex.Click
        BtnAddVertex.PerformClick()
        CMSAddTriangle.Checked = False
    End Sub

    Private Sub CMSAddTriangle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSAddTriangle.Click
        BtnAddTriangle.PerformClick()
        CMSAddVertex.Checked = False
    End Sub

    Private Sub CMSCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSCut.Click
        BtnCut.PerformClick()
    End Sub

    Private Sub CMSCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSCopy.Click
        BtnCopy.PerformClick()
    End Sub

    Private Sub CMSPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSPaste.Click
        BtnPaste.PerformClick()
    End Sub

    Private Sub CMSDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMSDelete.Click
        DeleteToolStripMenuItem.PerformClick()
    End Sub
#End Region

    Private Sub ToAverageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToAverageToolStripMenuItem.Click
        If View.SelectedVertices.Count > 1 AndAlso MainState.objectToModify <> Modified.Primitive Then
            mergeToAverage()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub ToAverageXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToAverageXToolStripMenuItem.Click
        If View.SelectedVertices.Count > 1 AndAlso MainState.objectToModify <> Modified.Primitive Then
            mergeToAverageX()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub ToAverageYToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToAverageYToolStripMenuItem.Click
        If View.SelectedVertices.Count > 1 AndAlso MainState.objectToModify <> Modified.Primitive Then
            mergeToAverageY()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub mergeToAverage()
        Helper_2D.stopTriangulation()
        Dim sumX As Integer = 0
        Dim sumY As Integer = 0
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            sumX += View.SelectedVertices(i).X
            sumY += View.SelectedVertices(i).Y
            View.SelectedVertices(i).selected = False
        Next
        sumX /= View.SelectedVertices.Count
        sumY /= View.SelectedVertices.Count
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            View.SelectedVertices(i).X = sumX
            View.SelectedVertices(i).Y = sumY
        Next
        View.SelectedVertices.Clear()
        View.SelectedTriangles.Clear()
        cleanupDATVertices()
        cleanupDATTriangles()
    End Sub

    Private Sub mergeToAverageX()
        Helper_2D.stopTriangulation()
        Dim sumX As Integer = 0
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            sumX += View.SelectedVertices(i).X
            View.SelectedVertices(i).selected = False
        Next
        sumX /= View.SelectedVertices.Count
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            View.SelectedVertices(i).X = (sumX \ 1000) * 1000
        Next
        View.SelectedVertices.Clear()
        View.SelectedTriangles.Clear()
        cleanupDATVertices()
        cleanupDATTriangles()
    End Sub

    Private Sub mergeToAverageY()
        Helper_2D.stopTriangulation()
        Dim sumY As Integer = 0
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            sumY += View.SelectedVertices(i).Y
            View.SelectedVertices(i).selected = False
        Next
        sumY /= View.SelectedVertices.Count
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            View.SelectedVertices(i).Y = (sumY \ 1000) * 1000
        Next
        View.SelectedVertices.Clear()
        View.SelectedTriangles.Clear()
        cleanupDATVertices()
        cleanupDATTriangles()
    End Sub

    Private Sub ToLastSelectedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToLastSelectedToolStripMenuItem.Click
        If View.SelectedVertices.Count > 1 AndAlso MainState.objectToModify <> Modified.Primitive Then
            mergeToLast()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub mergeToLast()
        Helper_2D.stopTriangulation()
        For i As Integer = 0 To View.SelectedVertices.Count - 1
            View.SelectedVertices(i).X = ListHelper.LLast(View.SelectedVertices).X
            View.SelectedVertices(i).Y = ListHelper.LLast(View.SelectedVertices).Y
            View.SelectedVertices(i).selected = False
        Next
        View.SelectedVertices.Clear()
        View.SelectedTriangles.Clear()
        cleanupDATVertices()
        cleanupDATTriangles()
    End Sub

#Region "CSG"

    Private Sub CSGAddToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CSGUnionToolStripMenuItem.Click
        If View.SelectedTriangles.Count > 1 Then
            CSG.unify2Triangles(View.SelectedTriangles(0), View.SelectedTriangles(1))
            cleanupDATVertices()
            Helper_2D.clearSelection()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub CSGIntersectionPointsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CSGIntersectionPointsToolStripMenuItem.Click
        Dim count As Integer = View.SelectedTriangles.Count - 1
        If View.SelectedTriangles.Count > 1 Then
            For trii1 As Integer = 0 To count
                Dim tri1 As Triangle = View.SelectedTriangles(trii1)
                For trii2 As Integer = trii1 + 1 To count
                    Dim tri2 As Triangle = View.SelectedTriangles(trii2)
                    CSG.trianglesIntersectionsOnly(tri1, tri2)
                Next
            Next
            cleanupDATVertices()
            Helper_2D.clearSelection()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub CSGSubdivideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CSGSubdivideToolStripMenuItem.Click
        If View.SelectedTriangles.Count = 1 Then
            CSG.subdivideTriangle(View.SelectedTriangles(0))
            cleanupDATVertices()
            Helper_2D.clearSelection()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub CSGSplitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CSGSplitToolStripMenuItem.Click
        If View.SelectedVertices.Count = 2 AndAlso MainState.objectToModify <> Modified.Primitive Then
            CSG.splitTriangle(View.SelectedVertices(0), View.SelectedVertices(1))
            Helper_2D.clearSelection()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub CSGRotateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CSGRotateToolStripMenuItem.Click
        If View.SelectedTriangles.Count = 2 Then
            CSG.rotateTriangles()
            Helper_2D.clearSelection()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

#End Region

    Private Sub MainForm_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        ' TODO: ArtificialTriangulator.iterate()
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        If MainState.doCameraMove Then
            View.offsetX = View.oldOffsetX + (MouseHelper.getCursorpositionX() - MainState.klickX) / View.zoomfactor
            View.offsetY = View.oldOffsetY + (MouseHelper.getCursorpositionY() - MainState.klickY) / View.zoomfactor
        End If
        If MainState.isLoading Then Exit Sub

        Dim absOffsetX As Integer = View.offsetX * View.zoomfactor + CType(Me.ClientSize.Width / 2, Integer)
        Dim absOffsetY As Integer = View.offsetY * View.zoomfactor + CType(Me.ClientSize.Height / 2, Integer)

        ' Picture
        If MainState.adjustmode Then
            Dim absImgOffsetX As Integer = Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / View.zoomfactor)
            Dim absImgOffsetY As Integer = Fix((MouseHelper.getCursorpositionY() - MainState.klickY) / View.zoomfactor)
            If Not MainState.doAdjust Then
                absImgOffsetX = 0
                absImgOffsetY = 0
            End If
            Dim tempImgOffsetX As Integer = View.imgOffsetX
            Dim tempImgOffsetY As Integer = View.imgOffsetY
            Dim tScale As Double = View.imgScale
            If Me.Cursor = Cursors.SizeAll Then
                tempImgOffsetX += absImgOffsetX
                tempImgOffsetY += absImgOffsetY
            ElseIf Me.Cursor = Cursors.SizeNESW OrElse Me.Cursor = Cursors.SizeNWSE OrElse Me.Cursor = Cursors.SizeWE OrElse Me.Cursor = Cursors.SizeNS Then
                If MouseHelper.getCursorpositionX() < CType(absOffsetX + tempImgOffsetX * View.zoomfactor, Integer) Then absImgOffsetX *= -1
                If MouseHelper.getCursorpositionY() < CType(absOffsetY + tempImgOffsetY * View.zoomfactor, Integer) Then absImgOffsetY *= -1
                If Me.Cursor = Cursors.SizeWE Then
                    tScale += ((View.backgroundPicture.Width * tScale + absImgOffsetX) / View.backgroundPicture.Width - tScale) * 2
                Else
                    tScale += ((View.backgroundPicture.Height * tScale + absImgOffsetY) / View.backgroundPicture.Height - tScale) * 2
                End If
                If Control.ModifierKeys = Keys.Control Then MainState.adjustDirection = -1 Else MainState.adjustDirection = MainState.tempAdjustDirection
                Select Case MainState.adjustDirection
                    Case 0 : tempImgOffsetY -= absImgOffsetY
                    Case 1.5 : tempImgOffsetX += View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY -= absImgOffsetY
                    Case 3 : tempImgOffsetX += absImgOffsetX
                    Case 4.5 : tempImgOffsetX += View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY += absImgOffsetY
                    Case 6 : tempImgOffsetY += absImgOffsetY
                    Case 7.5 : tempImgOffsetX -= View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY += absImgOffsetY
                    Case 9 : tempImgOffsetX -= absImgOffsetX
                    Case 10.5 : tempImgOffsetX -= View.backgroundPicture.Width / View.backgroundPicture.Height * absImgOffsetY : tempImgOffsetY -= absImgOffsetY
                End Select
            End If
            Try
                e.Graphics.DrawImage(View.backgroundPicture, CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer), CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer), CType(View.backgroundPicture.Width * View.zoomfactor * tScale, Integer), CType(View.backgroundPicture.Height * View.zoomfactor * tScale, Integer))
            Catch ex As Exception
            End Try
            e.Graphics.DrawRectangle(Pens.LimeGreen, CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer), CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer), CType(View.backgroundPicture.Width * View.zoomfactor * tScale, Integer), CType(View.backgroundPicture.Height * View.zoomfactor * tScale, Integer))
            e.Graphics.DrawRectangle(Pens.LimeGreen, CType(absOffsetX - (View.backgroundPicture.Width / 2 * tScale - tempImgOffsetX) * View.zoomfactor, Integer) - 1, CType(absOffsetY - (View.backgroundPicture.Height / 2 * tScale - tempImgOffsetY) * View.zoomfactor, Integer) - 1, CType(View.backgroundPicture.Width * View.zoomfactor * tScale + 2, Integer), CType(View.backgroundPicture.Height * View.zoomfactor * tScale + 2, Integer))
        ElseIf ShowBackgroundImageToolStripMenuItem.Checked Then

            Dim f As Double = View.zoomfactor * View.imgScale

            If f < 1.0 Then
                ' Fast drawing method when you zoom out
                Dim bw As Integer = View.backgroundPicture.Width
                Dim bh As Integer = View.backgroundPicture.Height
                e.Graphics.TranslateTransform(CType(absOffsetX - (bw / 2 * View.imgScale - View.imgOffsetX) * View.zoomfactor, Integer), CType(absOffsetY - (bh / 2 * View.imgScale - View.imgOffsetY) * View.zoomfactor, Integer))

                e.Graphics.ScaleTransform(f, f)
                e.Graphics.FillRectangle(View.backgroundPictureBrush, 0, 0, bw, bh)
                e.Graphics.ResetTransform()
            Else
                ' Slow, but more crisp drawing method when you zoom in very close
                Try
                    e.Graphics.DrawImage(View.backgroundPicture, CType(absOffsetX - (View.backgroundPicture.Width / 2 * View.imgScale - View.imgOffsetX) * View.zoomfactor, Integer), CType(absOffsetY - (View.backgroundPicture.Height / 2 * View.imgScale - View.imgOffsetY) * View.zoomfactor, Integer), CType(View.backgroundPicture.Width * View.zoomfactor * View.imgScale, Integer), CType(View.backgroundPicture.Height * View.zoomfactor * View.imgScale, Integer))
                Catch ex As Exception
                End Try
            End If

        End If

        ' When triangle auto-completion is enabled, draw a circle for selection
        If BtnTriangleAutoCompletion.Checked AndAlso Not Control.ModifierKeys = Keys.Control Then
            Dim radius As Double = getXcoordinate(View.selectionRadius) - getXcoordinate(0)
            Dim radiusSquared As Double = radius * radius
            Dim centerX As Double = getXcoordinate(MouseHelper.getCursorpositionX())
            Dim centerY As Double = getYcoordinate(MouseHelper.getCursorpositionY())
            Dim triangles As New Hashtable(LPCFile.Triangles.Count)
            View.TriangulationVertices.Clear()
            View.TriangulationVerticesInCircle.Clear()
            For Each vert As Vertex In LPCFile.Vertices
                Dim dX As Double = vert.X - centerX
                Dim dY As Double = vert.Y - centerY
                Dim dist As Double = dX * dX + dY * dY
                If dist < radiusSquared Then
                    View.TriangulationVerticesInCircle.Add(vert)
                    If View.TriangulationVertices.Count < 26 Then View.TriangulationVertices.Add(vert)
                    If View.TriangulationVerticesInCircle.Count > 40 Then Exit For
                    For Each tri As Triangle In vert.linkedTriangles
                        If Not triangles.ContainsKey(tri.triangleID) Then triangles.Add(tri.triangleID, tri)
                    Next
                End If
            Next

            ' Limit the feature only to a maximum of 40 vertices
            If View.TriangulationVerticesInCircle.Count > 40 Then
                View.TriangulationVertices.Clear()
                View.TriangulationVerticesInCircle.Clear()
            End If

            ' Now iterate over all triangle combinations for max. 25 vertices
            Dim tv As List(Of Vertex) = View.TriangulationVertices
            Dim tvc As Integer = tv.Count - 1
            Dim vert0(2) As Decimal
            Dim vert1(2) As Decimal
            Dim vert2(2) As Decimal
            Dim newTriangles As New List(Of Triangle)
            Dim alphaAngles As New Dictionary(Of Triangle, Double)
            Dim betaAngles As New Dictionary(Of Triangle, Double)
            Dim gammaAngles As New Dictionary(Of Triangle, Double)
            Dim maxAngleLimit As Double = Math.PI * 0.95
            Dim deg60 As Double = Math.PI / 3.0
            Dim angles As New List(Of Double)
            CSG.beamorig(2) = 0
            For ai As Integer = 0 To tvc
                For bi As Integer = ai + 1 To tvc
                    For ci As Integer = bi + 1 To tvc
                        Dim a As Vertex = tv(ai)
                        Dim b As Vertex = tv(bi)
                        Dim c As Vertex = tv(ci)
                        If CSG.pointsConnected(a, b, c) Then Continue For
                        vert0(0) = a.X
                        vert0(1) = a.Y
                        vert1(0) = b.X
                        vert1(1) = b.Y
                        vert2(0) = c.X
                        vert2(1) = c.Y
                        Dim containsVertex As Boolean = False
                        Dim maxX As Double = Math.Max(Math.Max(a.X, b.X), c.X)
                        Dim maxY As Double = Math.Max(Math.Max(a.Y, b.Y), c.Y)
                        Dim minX As Double = Math.Min(Math.Min(a.X, b.X), c.X)
                        Dim minY As Double = Math.Min(Math.Min(a.Y, b.Y), c.Y)
                        For Each vert As Vertex In View.TriangulationVerticesInCircle
                            If vert = a OrElse vert = b OrElse vert = c Then Continue For
                            CSG.beamorig(0) = vert.X
                            CSG.beamorig(1) = vert.Y
                            containsVertex = PowerRay.SCHNITTPKT_DREIECK(CSG.beamorig, CSG.beamdir, vert0, vert1, vert2)
                            If containsVertex Then Exit For
                        Next
                        If containsVertex Then Continue For

                        Dim newTri As New Triangle(a, b, c, False)
                        Dim intersectsWithTriangle As Boolean = False

                        ' Check if this triangle collides with another triangle
                        For Each id In triangles.Keys
                            Dim tri As Triangle = triangles(id)
                            If CSG.trianglesIntersectionsOnly(tri, newTri, False) Then
                                intersectsWithTriangle = True
                                Exit For
                            End If
                        Next

                        If intersectsWithTriangle Then Continue For

                        Dim alpha As Double = (b - a).directedAngle(c - a)
                        Dim beta As Double = (a - b).directedAngle(c - b)
                        Dim gamma As Double = Math.PI - alpha - beta

                        Dim maxAngle As Double = Math.Max(alpha, Math.Max(beta, gamma))
                        If maxAngle > maxAngleLimit Then Continue For

                        Dim triangleIntersectsVertex As Boolean = False
                        For Each vert As Vertex In View.TriangulationVerticesInCircle
                            If vert.vertexID <> a.vertexID AndAlso
                               vert.vertexID <> b.vertexID AndAlso
                               vert.vertexID <> c.vertexID Then
                                If vert.X <= maxX AndAlso
                                       vert.Y <= maxY AndAlso
                                       vert.X >= minX AndAlso
                                       vert.Y >= minY Then
                                    If CSG.distanceSquareFromVertexToLine(vert, a, b) < 10.0 OrElse
                                           CSG.distanceSquareFromVertexToLine(vert, a, c) < 10.0 OrElse
                                           CSG.distanceSquareFromVertexToLine(vert, b, c) < 10.0 Then
                                        triangleIntersectsVertex = True
                                        Exit For
                                    End If
                                End If
                            End If
                        Next

                        If triangleIntersectsVertex Then Continue For

                        ' Check if this triangle collides with a reference line
                        Dim collidesWithRefLine As Boolean = False
                        Dim shapeCount As Integer = LPCFile.templateShape.Count
                        If shapeCount > 1 AndAlso Not BtnPreview.Checked Then
                            If Not LPCFile.templateShape(0).X = Single.Epsilon Then
                                Dim start As Integer = 0
                                Dim finish As Integer = shapeCount - 1
                                Dim templateShapeArray(finish) As PointF
                                For i As Integer = 0 To finish
                                    templateShapeArray(i).X = LPCFile.templateShape(i).X
                                    templateShapeArray(i).Y = LPCFile.templateShape(i).Y
                                    If LPCFile.templateShape(i).X = Single.Epsilon Then
                                        Dim lenght As Integer = i - start - 1
                                        Dim templatePolyPart(lenght) As PointF
                                        Array.Copy(templateShapeArray, start, templatePolyPart, 0, lenght + 1)
                                        If PolygonCollidesWithReferenceLine(a, b, c, templatePolyPart) Then
                                            collidesWithRefLine = True
                                            Exit For
                                        End If
                                        start = i + 1
                                    End If
                                Next
                                Dim lenght2 As Integer = finish - start
                                Dim templatePolyPart2(lenght2) As PointF
                                Array.Copy(templateShapeArray, start, templatePolyPart2, 0, lenght2 + 1)
                                If PolygonCollidesWithReferenceLine(a, b, c, templatePolyPart2) Then
                                    collidesWithRefLine = True
                                    Exit For
                                End If
                            Else
                                For i As Integer = 1 To shapeCount - 1 Step 4
                                    Dim templateShapeArray(3) As PointF
                                    templateShapeArray(0).X = LPCFile.templateShape(i).X
                                    templateShapeArray(0).Y = LPCFile.templateShape(i).Y

                                    If (i + 1) < shapeCount Then
                                        templateShapeArray(1).X = LPCFile.templateShape(i + 1).X
                                        templateShapeArray(1).Y = LPCFile.templateShape(i + 1).Y
                                    Else
                                        templateShapeArray(1).X = templateShapeArray(0).X
                                        templateShapeArray(1).Y = templateShapeArray(0).Y
                                    End If

                                    If (i + 2) < shapeCount Then
                                        templateShapeArray(2).X = LPCFile.templateShape(i + 2).X
                                        templateShapeArray(2).Y = LPCFile.templateShape(i + 2).Y
                                    Else
                                        templateShapeArray(2).X = templateShapeArray(0).X
                                        templateShapeArray(2).Y = templateShapeArray(0).Y
                                    End If

                                    If (i + 3) < shapeCount Then
                                        templateShapeArray(3).X = LPCFile.templateShape(i + 3).X
                                        templateShapeArray(3).Y = LPCFile.templateShape(i + 3).Y
                                    Else
                                        templateShapeArray(3).X = templateShapeArray(0).X
                                        templateShapeArray(3).Y = templateShapeArray(0).Y
                                    End If
                                    If PolygonCollidesWithReferenceLine(a, b, c, templateShapeArray) Then
                                        collidesWithRefLine = True
                                        Exit For
                                    End If
                                Next
                            End If
                        End If

                        If collidesWithRefLine Then Continue For

                        angles.Add(Math.Abs(alpha - deg60))
                        angles.Add(Math.Abs(beta - deg60))
                        angles.Add(Math.Abs(gamma - deg60))
                        angles.Sort()

                        alphaAngles.Add(newTri, angles(0))
                        betaAngles.Add(newTri, angles(1))
                        gammaAngles.Add(newTri, angles(2))
                        newTriangles.Add(newTri)
                    Next
                Next
            Next

            ' Now sort the triangles by their angles, difference of abs(angle - 60 degree)
            newTriangles.Sort(Function(elementA As Triangle, elementB As Triangle)
                                  Dim result
                                  Dim alphaA As Double = alphaAngles(elementA)
                                  Dim alphaB As Double = alphaAngles(elementB)
                                  result = alphaA.CompareTo(alphaB)
                                  If result <> 0 Then Return result
                                  Dim betaA As Double = betaAngles(elementA)
                                  Dim betaB As Double = betaAngles(elementB)
                                  result = betaA.CompareTo(betaB)
                                  If result <> 0 Then Return result
                                  Dim gammaA As Double = gammaAngles(elementA)
                                  Dim gammaB As Double = gammaAngles(elementB)
                                  result = gammaA.CompareTo(gammaB)
                                  Return result
                              End Function)

            If newTriangles.Count > 0 Then
                Dim newTriangle As Triangle = newTriangles(0)
                Dim newTriangleToAdd As New Triangle(newTriangle.vertexA, newTriangle.vertexB, newTriangle.vertexC) With {.myColour = MainState.lastColour, .myColourNumber = MainState.lastColourNumber}
                LPCFile.Triangles.Add(newTriangleToAdd)
                newTriangle.vertexA.linkedTriangles.Add(newTriangleToAdd)
                newTriangle.vertexB.linkedTriangles.Add(newTriangleToAdd)
                newTriangle.vertexC.linkedTriangles.Add(newTriangleToAdd)
                UndoRedoHelper.addHistory()
            End If
        End If

        ' Grid, Origin
        If View.showGrid Then
            Dim scale As Double
            If View.zoomlevel > 80 Then scale = View.rasterSnap * View.zoomfactor : GoTo raster_zechnen
            If View.zoomlevel > 0 Then scale = View.rasterSnap * View.zoomfactor * 10 : GoTo raster_zechnen
            If View.zoomlevel > -10 Then scale = View.rasterSnap * View.zoomfactor * 100 : GoTo raster_zechnen
            If View.zoomlevel > -20 Then scale = View.rasterSnap * View.zoomfactor * 1000 : GoTo raster_zechnen
            If View.zoomlevel > -30 Then scale = View.rasterSnap * View.zoomfactor * 10000 : GoTo raster_zechnen
            scale = View.rasterSnap * View.zoomfactor * 100000
raster_zechnen:
            Dim relOffsetX As Integer
            Dim relOffsetY As Integer
            relOffsetX = absOffsetX Mod scale
            relOffsetY = absOffsetY Mod scale
            For x As Single = relOffsetX To Me.ClientSize.Width Step scale
                e.Graphics.DrawLine(LDSettings.Colours.gridPen, x, 0, x, Me.ClientSize.Height)
            Next
            For y As Single = relOffsetY To Me.ClientSize.Height Step scale
                e.Graphics.DrawLine(LDSettings.Colours.gridPen, 0, y, Me.ClientSize.Width, y)
            Next
            relOffsetX = absOffsetX Mod (scale * 10)
            relOffsetY = absOffsetY Mod (scale * 10)
            For x As Single = relOffsetX To Me.ClientSize.Width Step scale * 10
                e.Graphics.DrawLine(LDSettings.Colours.grid10Pen, x, 0, x, Me.ClientSize.Height)
            Next
            For y As Single = relOffsetY To Me.ClientSize.Height Step scale * 10
                e.Graphics.DrawLine(LDSettings.Colours.grid10Pen, 0, y, Me.ClientSize.Width, y)
            Next
            e.Graphics.DrawLine(LDSettings.Colours.originPen, absOffsetX, 0, absOffsetX, Me.ClientSize.Height)
            e.Graphics.DrawLine(LDSettings.Colours.originPen, 0, absOffsetY, Me.ClientSize.Width, absOffsetY)
        End If

        If MainState.movemode Then
            If Not Control.ModifierKeys = Keys.Control Or Control.ModifierKeys = Keys.Shift Then View.viewAbsOffsetX = Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / View.zoomfactor / View.moveSnap) * View.moveSnap Else View.viewAbsOffsetX = 0
            If Not Control.ModifierKeys = Keys.Shift Or Control.ModifierKeys = Keys.Control Then View.viewAbsOffsetY = Fix((MouseHelper.getCursorpositionY() - MainState.klickY) / View.zoomfactor / View.moveSnap) * View.moveSnap Else View.viewAbsOffsetY = 0
            For Each vert As Vertex In View.SelectedVertices
                vert.X -= View.viewAbsOffsetX
                vert.Y += View.viewAbsOffsetY
            Next
            If View.SelectedVertices.Count = 1 AndAlso Control.ModifierKeys = 196608 AndAlso LPCFile.templateProjectionQuads.Count > 0 Then
                If View.SelectedVertices(0).linkedTriangles.Count > 0 Then
                    Dim tv As Vertex = New Vertex(View.SelectedVertices(0).X, View.SelectedVertices(0).Y, False, False)
                    For i = 0 To LPCFile.templateProjectionQuads.Count - 1
                        If LPCFile.templateProjectionQuads(i).isInQuad(tv) Then
                            Dim q As ProjectionQuad = LPCFile.templateProjectionQuads(i)
                            Dim v As Vertex = New Vertex(View.SelectedVertices(0).X / 1000.0, View.SelectedVertices(0).Y / 1000.0, False, False)
                            ' The type needs to be checked
                            Dim newV As Vertex
                            Dim tmpV As Vertex
                            If q.isTriangle Then
                                newV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False), New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False))
                                Dim dist As Double
                                Dim mindist As Double = v.dist(v)
                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False), New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False))
                                dist = v.dist(tmpV)
                                If dist < mindist Then newV = tmpV : mindist = dist
                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False), New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False))
                                dist = v.dist(tmpV)
                                If dist < mindist Then newV = tmpV : mindist = dist
                            Else
                                newV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False), New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False))
                                Dim dist As Double
                                Dim mindist As Double = v.dist(newV)
                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(1, 0), q.inCoords(1, 1), False, False), New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False))
                                dist = v.dist(tmpV)
                                If dist < mindist Then newV = tmpV : mindist = dist
                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(2, 0), q.inCoords(2, 1), False, False), New Vertex(q.inCoords(3, 0), q.inCoords(3, 1), False, False))
                                dist = v.dist(tmpV)
                                If dist < mindist Then newV = tmpV : mindist = dist
                                tmpV = CSG.distanceVectorFromVertexToLine(v, New Vertex(q.inCoords(3, 0), q.inCoords(3, 1), False, False), New Vertex(q.inCoords(0, 0), q.inCoords(0, 1), False, False))
                                dist = v.dist(tmpV)
                                If dist < mindist Then newV = tmpV : mindist = dist
                            End If
                            View.viewAbsOffsetX -= View.SelectedVertices(0).X - newV.X * 1000.0
                            View.viewAbsOffsetY += View.SelectedVertices(0).Y - newV.Y * 1000.0
                            Exit For
                        End If
                    Next
                End If
            ElseIf View.SelectedVertices.Count = 1 AndAlso Control.ModifierKeys = 196608 Then
                If LPCFile.templateShape.Count > 1 AndAlso MainState.objectToModify <> Modified.Primitive Then

                    Dim minDist As Double = Double.MaxValue
                    Dim dist As Double
                    Dim startVertex As New Vertex(0, 0, False, False)
                    Dim startPolyVertex As New Vertex(0, 0, False, False)
                    Dim endVertex As Vertex = Nothing
                    Dim distVertex As Vertex
                    Dim finalVertex As Vertex = Nothing

                    ' Detect if it is nearer to a template polygon

                    ' Template polygon
                    If Not LPCFile.templateShape(0).X = Single.Epsilon Then
                        startVertex.X = LPCFile.templateShape(0).X
                        startVertex.Y = LPCFile.templateShape(0).Y
                        startPolyVertex.X = LPCFile.templateShape(0).X
                        startPolyVertex.Y = LPCFile.templateShape(0).Y
                        Dim start As Integer = 0
                        Dim finish As Integer = LPCFile.templateShape.Count - 1
                        For i As Integer = 1 To finish
                            If LPCFile.templateShape(i).X = Single.Epsilon AndAlso Not endVertex Is Nothing Then
                                distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startPolyVertex, endVertex)
                                dist = View.SelectedVertices(0).dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                                start = i + 1
                                If start <= finish Then
                                    startPolyVertex.X = LPCFile.templateShape(start).X
                                    startPolyVertex.Y = LPCFile.templateShape(start).Y
                                    startVertex.X = startPolyVertex.X
                                    startVertex.Y = startPolyVertex.Y
                                    endVertex = Nothing
                                End If
                            Else
                                endVertex = New Vertex(0, 0, False, False)
                                endVertex.X = LPCFile.templateShape(i).X
                                endVertex.Y = LPCFile.templateShape(i).Y
                                distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                                dist = View.SelectedVertices(0).dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                                startVertex.X = LPCFile.templateShape(i).X
                                startVertex.Y = LPCFile.templateShape(i).Y
                            End If
                        Next
                        endVertex.X = LPCFile.templateShape(finish).X
                        endVertex.Y = LPCFile.templateShape(finish).Y
                        distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startPolyVertex, endVertex)
                        dist = View.SelectedVertices(0).dist(distVertex)
                        If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                    Else
                        Dim finish As Integer = LPCFile.templateShape.Count - 1
                        For i As Integer = 1 To finish Step 4

                            If (i + 1) <= finish Then
                                startVertex = New Vertex(LPCFile.templateShape(i).X, LPCFile.templateShape(i).Y, False, False)
                                endVertex = New Vertex(LPCFile.templateShape(i + 1).X, LPCFile.templateShape(i + 1).Y, False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                                dist = View.SelectedVertices(0).dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                            End If

                            If (i + 2) <= finish Then
                                startVertex = New Vertex(LPCFile.templateShape(i + 1).X, LPCFile.templateShape(i + 1).Y, False, False)
                                endVertex = New Vertex(LPCFile.templateShape(i + 2).X, LPCFile.templateShape(i + 2).Y, False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                                dist = View.SelectedVertices(0).dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                            End If

                            If (i + 3) <= finish Then
                                startVertex = New Vertex(LPCFile.templateShape(i + 2).X, LPCFile.templateShape(i + 2).Y, False, False)
                                endVertex = New Vertex(LPCFile.templateShape(i + 3).X, LPCFile.templateShape(i + 3).Y, False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                                dist = View.SelectedVertices(0).dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)

                                startVertex = New Vertex(LPCFile.templateShape(i).X, LPCFile.templateShape(i).Y, False, False)
                                endVertex = New Vertex(LPCFile.templateShape(i + 3).X, LPCFile.templateShape(i + 3).Y, False, False)
                                distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                                dist = View.SelectedVertices(0).dist(distVertex)
                                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                            End If

                        Next
                    End If
                    If Not finalVertex Is Nothing Then
                        View.SelectedVertices(0).X = finalVertex.X
                        View.SelectedVertices(0).Y = finalVertex.Y
                    End If
                End If
            End If
        ElseIf MainState.rotatemode Then
            Dim v1 As New Vertex(MouseHelper.getCursorpositionX(), MouseHelper.getCursorpositionY(), False, False)
            Dim v2 As New Vertex(MainState.klickX, MainState.klickY, False, False)
            Dim vc As New Vertex(absOffsetX - MainState.temp_center.X * View.zoomfactor, absOffsetY + MainState.temp_center.Y * View.zoomfactor, False, False)
            Dim angleToRotate As Double = -Fix((vc.angle(v1) - vc.angle(v2)) / (View.rotateSnap * Math.PI / 180)) * (View.rotateSnap * Math.PI / 180)
            For Each vert As Vertex In View.SelectedVertices
                vert.X = MainState.temp_center.X + vert.distanceFrom * Math.Cos(vert.angleFrom + angleToRotate)
                vert.Y = MainState.temp_center.Y + vert.distanceFrom * Math.Sin(vert.angleFrom + angleToRotate)
                If Double.IsNaN(vert.X) OrElse Double.IsNaN(vert.Y) Then
                    vert.X = vert.oldX
                    vert.Y = vert.oldY
                End If
            Next
        ElseIf MainState.scalemode Then
            Dim factorToScale As Double = 1 + Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / 100 / View.scaleSnap) * View.scaleSnap
            If factorToScale < 0.1 Then factorToScale = 0.1
            If factorToScale > 2 Then factorToScale = 2
            For Each vert As Vertex In View.SelectedVertices
                If Not Control.ModifierKeys = Keys.Control Then
                    vert.X = MainState.temp_center.X + factorToScale * vert.distanceFrom * Math.Cos(vert.angleFrom)
                Else
                    vert.X = vert.oldX
                End If
                If Not Control.ModifierKeys = Keys.Shift Then
                    vert.Y = MainState.temp_center.Y + factorToScale * vert.distanceFrom * Math.Sin(vert.angleFrom)
                Else
                    vert.Y = vert.oldY
                End If
                If Double.IsNaN(vert.X) OrElse Double.IsNaN(vert.Y) Then
                    vert.X = vert.oldX
                    vert.Y = vert.oldY
                End If
            Next
        End If

        If Not LDSettings.Editor.showTemplateLinesOnTop Then
            ' Templates:
            Dim shapeCount As Integer = LPCFile.templateShape.Count
            If shapeCount > 1 AndAlso Not BtnPreview.Checked Then
                If Not LPCFile.templateShape(0).X = Single.Epsilon Then
                    Dim start As Integer = 0
                    Dim finish As Integer = shapeCount - 1
                    Dim templateShapeArray(finish) As PointF
                    For i As Integer = 0 To finish
                        templateShapeArray(i).X = CType(absOffsetX - LPCFile.templateShape(i).X * View.zoomfactor, Single)
                        templateShapeArray(i).Y = CType(absOffsetY + LPCFile.templateShape(i).Y * View.zoomfactor, Single)
                        If LPCFile.templateShape(i).X = Single.Epsilon Then
                            Dim lenght As Integer = i - start - 1
                            Dim templatePolyPart(lenght) As PointF
                            Array.Copy(templateShapeArray, start, templatePolyPart, 0, lenght + 1)
                            e.Graphics.DrawPolygon(LDSettings.Colours.templatePen, templatePolyPart)
                            start = i + 1
                        End If
                    Next
                    Dim lenght2 As Integer = finish - start
                    Dim templatePolyPart2(lenght2) As PointF
                    Array.Copy(templateShapeArray, start, templatePolyPart2, 0, lenght2 + 1)
                    e.Graphics.DrawPolygon(LDSettings.Colours.templatePen, templatePolyPart2)

                    For i = 0 To LPCFile.templateTexts.Count - 1
                        e.Graphics.DrawString(LPCFile.templateTexts(i).Text, New Font("Arial", (1000 * View.zoomfactor) + 8, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, absOffsetX - LPCFile.templateTexts(i).X * View.zoomfactor + 2, absOffsetY + LPCFile.templateTexts(i).Y * View.zoomfactor + 2)
                        e.Graphics.DrawString(LPCFile.templateTexts(i).Text, New Font("Arial", (1000 * View.zoomfactor) + 8, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, absOffsetX - LPCFile.templateTexts(i).X * View.zoomfactor, absOffsetY + LPCFile.templateTexts(i).Y * View.zoomfactor)
                    Next
                Else
                    For i As Integer = 1 To shapeCount - 1 Step 4
                        Dim templateShapeArray(3) As PointF
                        templateShapeArray(0).X = absOffsetX - LPCFile.templateShape(i).X * View.zoomfactor
                        templateShapeArray(0).Y = absOffsetY + LPCFile.templateShape(i).Y * View.zoomfactor

                        If (i + 1) < shapeCount Then
                            templateShapeArray(1).X = absOffsetX - LPCFile.templateShape(i + 1).X * View.zoomfactor
                            templateShapeArray(1).Y = absOffsetY + LPCFile.templateShape(i + 1).Y * View.zoomfactor
                        Else
                            templateShapeArray(1).X = templateShapeArray(0).X
                            templateShapeArray(1).Y = templateShapeArray(0).Y
                        End If

                        If (i + 2) < shapeCount Then
                            templateShapeArray(2).X = absOffsetX - LPCFile.templateShape(i + 2).X * View.zoomfactor
                            templateShapeArray(2).Y = absOffsetY + LPCFile.templateShape(i + 2).Y * View.zoomfactor
                        Else
                            templateShapeArray(2).X = templateShapeArray(0).X
                            templateShapeArray(2).Y = templateShapeArray(0).Y
                        End If

                        If (i + 3) < shapeCount Then
                            templateShapeArray(3).X = absOffsetX - LPCFile.templateShape(i + 3).X * View.zoomfactor
                            templateShapeArray(3).Y = absOffsetY + LPCFile.templateShape(i + 3).Y * View.zoomfactor
                        Else
                            templateShapeArray(3).X = templateShapeArray(0).X
                            templateShapeArray(3).Y = templateShapeArray(0).Y
                        End If
                        e.Graphics.DrawPolygon(LDSettings.Colours.projectionPen, templateShapeArray)
                    Next
                End If
            End If
        End If
        Dim r1 As New Rectangle
        Dim r2 As New Rectangle
        r1.X = -absOffsetX / View.zoomfactor
        r1.Y = -absOffsetY / View.zoomfactor
        r1.Width = Me.ClientSize.Width / View.zoomfactor
        r1.Height = Me.ClientSize.Height / View.zoomfactor

        e.Graphics.TranslateTransform(absOffsetX, absOffsetY)
        e.Graphics.ScaleTransform(View.zoomfactor, View.zoomfactor)
        ' Filled Triangles
        Dim transColour As Color = Color.FromArgb(20, 0, 0, 0)
        Dim lineColour As Color = LDSettings.Colours.linePen.Color
        If BtnColours.Checked Then
            Dim brushDict As New Dictionary(Of Integer, Brush)
            If BtnBFC.Checked AndAlso BtnPreview.Checked Then
                Dim pf(3) As PointF
                Dim green As Brush = New SolidBrush(Color.Green)
                Dim red As Brush = New SolidBrush(Color.Red)
                Dim blue As Brush = New SolidBrush(Color.Blue)
                Dim indecies As New Dictionary(Of Integer, Brush)
                For i As Integer = 0 To LPCFile.Triangles.Count - 1
                    Dim tri As Triangle = LPCFile.Triangles(i)
                    r2.X = Math.Min(-tri.vertexA.X, Math.Min(-tri.vertexB.X, -tri.vertexC.X))
                    r2.Y = Math.Min(tri.vertexA.Y, Math.Min(tri.vertexB.Y, tri.vertexC.Y))
                    r2.Width = Math.Abs(Math.Max(-tri.vertexA.X, Math.Max(-tri.vertexB.X, -tri.vertexC.X)) - r2.X)
                    r2.Height = Math.Abs(Math.Max(tri.vertexA.Y, Math.Max(tri.vertexB.Y, tri.vertexC.Y)) - r2.Y)
                    If Not (r2.IntersectsWith(r1) OrElse r1.Contains(r2)) Then Continue For
                    If tri.groupindex = -1 Then
                        pf(0).X = -tri.vertexA.X
                        pf(0).Y = tri.vertexA.Y
                        pf(1).X = -tri.vertexB.X
                        pf(1).Y = tri.vertexB.Y
                        pf(2).X = -tri.vertexC.X
                        pf(2).Y = tri.vertexC.Y
                        pf(3).X = pf(0).X
                        pf(3).Y = pf(0).Y
                        e.Graphics.FillPolygon(green, pf)
                    Else
                        If Not LPCFile.PrimitivesHMap.ContainsKey(tri.groupindex) Then Continue For
                        Dim p As Primitive = LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex))
                        If Not indecies.ContainsKey(tri.groupindex) Then
                            Dim m As New Matrix3D
                            For y As Integer = 0 To 3
                                For x As Integer = 0 To 3
                                    m.m(y, x) = p.matrix(y, x)
                                Next
                            Next
                            Dim v As New Vertex3D(0, 0, 1)
                            v = m * v
                            If p.primitiveName Like "subfile*" Then v.Z *= -1
                            If p.primitiveName Like "s\*" Then
                                indecies.Add(tri.groupindex, blue)
                            ElseIf v.Z < 0 Then
                                indecies.Add(tri.groupindex, red)
                            Else
                                indecies.Add(tri.groupindex, green)
                            End If
                        End If
                        pf(0).X = -tri.vertexA.X
                        pf(0).Y = tri.vertexA.Y
                        pf(1).X = -tri.vertexB.X
                        pf(1).Y = tri.vertexB.Y
                        pf(2).X = -tri.vertexC.X
                        pf(2).Y = tri.vertexC.Y
                        pf(3).X = pf(0).X
                        pf(3).Y = pf(0).Y
                        e.Graphics.FillPolygon(indecies(tri.groupindex), pf)
                    End If
                Next
            Else
                Dim hb As New HatchBrush(HatchStyle.Percent05, lineColour, transColour)
                If MainState.colourReplacement AndAlso BtnPreview.Checked Then
                    If View.alpha = 255 Then ' Farb-Ersetzter, ohne Alpha
                        Dim pf(3) As PointF
                        For i As Integer = 0 To LPCFile.Triangles.Count - 1
                            Dim tri As Triangle = LPCFile.Triangles(i)
                            r2.X = Math.Min(-tri.vertexA.X, Math.Min(-tri.vertexB.X, -tri.vertexC.X))
                            r2.Y = Math.Min(tri.vertexA.Y, Math.Min(tri.vertexB.Y, tri.vertexC.Y))
                            r2.Width = Math.Abs(Math.Max(-tri.vertexA.X, Math.Max(-tri.vertexB.X, -tri.vertexC.X)) - r2.X)
                            r2.Height = Math.Abs(Math.Max(tri.vertexA.Y, Math.Max(tri.vertexB.Y, tri.vertexC.Y)) - r2.Y)
                            If Not (r2.IntersectsWith(r1) OrElse r1.Contains(r2)) Then Continue For
                            If tri.myColourNumber <> 16 Then
                                Dim ci As Integer = tri.myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then
                                    If LPCFile.colourReplacementMapBrush.ContainsKey(ci) Then
                                        brushDict.Add(ci, LPCFile.colourReplacementMapBrush(ci))
                                    Else
                                        brushDict.Add(ci, New SolidBrush(tri.myColour))
                                    End If
                                End If
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            ElseIf tri.groupindex <> -1 AndAlso tri.myColourNumber = 16 AndAlso LPCFile.PrimitivesHMap.ContainsKey(tri.groupindex) AndAlso LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColourNumber <> 16 Then
                                Dim ci As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then
                                    If LPCFile.colourReplacementMapBrush.ContainsKey(ci) Then
                                        brushDict.Add(ci, LPCFile.colourReplacementMapBrush(ci))
                                    Else
                                        brushDict.Add(ci, New SolidBrush(LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColour))
                                    End If
                                End If
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            Else
                                Dim ci As Integer = tri.myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then
                                    If LPCFile.colourReplacementMapBrush.ContainsKey(ci) Then
                                        brushDict.Add(ci, LPCFile.colourReplacementMapBrush(ci))
                                    Else
                                        brushDict.Add(ci, hb)
                                    End If
                                End If
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            End If
                        Next
                    Else
                        Dim pf(3) As PointF
                        For i As Integer = 0 To LPCFile.Triangles.Count - 1
                            Dim tri As Triangle = LPCFile.Triangles(i)
                            r2.X = Math.Min(-tri.vertexA.X, Math.Min(-tri.vertexB.X, -tri.vertexC.X))
                            r2.Y = Math.Min(tri.vertexA.Y, Math.Min(tri.vertexB.Y, tri.vertexC.Y))
                            r2.Width = Math.Abs(Math.Max(-tri.vertexA.X, Math.Max(-tri.vertexB.X, -tri.vertexC.X)) - r2.X)
                            r2.Height = Math.Abs(Math.Max(tri.vertexA.Y, Math.Max(tri.vertexB.Y, tri.vertexC.Y)) - r2.Y)
                            If Not (r2.IntersectsWith(r1) OrElse r1.Contains(r2)) Then Continue For
                            If tri.myColourNumber <> 16 Then
                                Dim ci As Integer = tri.myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then
                                    If LPCFile.colourReplacementMapBrush.ContainsKey(ci) Then
                                        Dim col As Color
                                        If TypeOf LPCFile.colourReplacementMapBrush(ci) Is SolidBrush Then
                                            col = CType(LPCFile.colourReplacementMapBrush(ci), SolidBrush).Color
                                            brushDict.Add(ci, New SolidBrush(Color.FromArgb(View.alpha, col.R, col.G, col.B)))
                                        Else
                                            brushDict.Add(ci, LPCFile.colourReplacementMapBrush(ci))
                                        End If
                                    Else
                                        Dim col As Color = tri.myColour
                                        brushDict.Add(ci, New SolidBrush(Color.FromArgb(View.alpha, col.R, col.G, col.B)))
                                    End If
                                End If
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            ElseIf tri.groupindex <> -1 AndAlso tri.myColourNumber = 16 AndAlso LPCFile.PrimitivesHMap.ContainsKey(tri.groupindex) AndAlso LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColourNumber <> 16 Then
                                Dim ci As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then
                                    If LPCFile.colourReplacementMapBrush.ContainsKey(ci) Then
                                        brushDict.Add(ci, LPCFile.colourReplacementMapBrush(ci))
                                    Else
                                        Dim col As Color = LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColour
                                        brushDict.Add(ci, New SolidBrush(Color.FromArgb(View.alpha, col.R, col.G, col.B)))
                                    End If
                                End If
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            Else
                                Dim ci As Integer = tri.myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then
                                    If LPCFile.colourReplacementMapBrush.ContainsKey(ci) Then
                                        Dim col As Color
                                        If TypeOf LPCFile.colourReplacementMapBrush(ci) Is SolidBrush Then
                                            col = CType(LPCFile.colourReplacementMapBrush(ci), SolidBrush).Color
                                            brushDict.Add(ci, New SolidBrush(Color.FromArgb(View.alpha, col.R, col.G, col.B)))
                                        Else
                                            brushDict.Add(ci, LPCFile.colourReplacementMapBrush(ci))
                                        End If
                                    Else
                                        Dim col As Color = tri.myColour
                                        brushDict.Add(ci, hb)
                                    End If
                                End If
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            End If
                        Next
                    End If
                Else
                    If View.alpha = 255 Then ' Normal, ohne Alpha
                        Dim pf(3) As PointF
                        For i As Integer = 0 To LPCFile.Triangles.Count - 1
                            Dim tri As Triangle = LPCFile.Triangles(i)
                            r2.X = Math.Min(-tri.vertexA.X, Math.Min(-tri.vertexB.X, -tri.vertexC.X))
                            r2.Y = Math.Min(tri.vertexA.Y, Math.Min(tri.vertexB.Y, tri.vertexC.Y))
                            r2.Width = Math.Abs(Math.Max(-tri.vertexA.X, Math.Max(-tri.vertexB.X, -tri.vertexC.X)) - r2.X)
                            r2.Height = Math.Abs(Math.Max(tri.vertexA.Y, Math.Max(tri.vertexB.Y, tri.vertexC.Y)) - r2.Y)
                            If Not (r2.IntersectsWith(r1) OrElse r1.Contains(r2)) Then Continue For
                            If tri.myColourNumber <> 16 Then
                                Dim ci As Integer = tri.myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then brushDict.Add(ci, New SolidBrush(tri.myColour))
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            ElseIf tri.groupindex <> -1 AndAlso tri.myColourNumber = 16 AndAlso LPCFile.PrimitivesHMap.ContainsKey(tri.groupindex) AndAlso LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColourNumber <> 16 Then
                                Dim ci As Integer = LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColour.ToArgb
                                If Not brushDict.ContainsKey(ci) Then brushDict.Add(ci, New SolidBrush(LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColour))
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            Else
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(hb, pf)
                            End If
                        Next
                    Else
                        Dim pf(3) As PointF
                        For i As Integer = 0 To LPCFile.Triangles.Count - 1
                            Dim tri As Triangle = LPCFile.Triangles(i)
                            r2.X = Math.Min(-tri.vertexA.X, Math.Min(-tri.vertexB.X, -tri.vertexC.X))
                            r2.Y = Math.Min(tri.vertexA.Y, Math.Min(tri.vertexB.Y, tri.vertexC.Y))
                            r2.Width = Math.Abs(Math.Max(-tri.vertexA.X, Math.Max(-tri.vertexB.X, -tri.vertexC.X)) - r2.X)
                            r2.Height = Math.Abs(Math.Max(tri.vertexA.Y, Math.Max(tri.vertexB.Y, tri.vertexC.Y)) - r2.Y)
                            If Not (r2.IntersectsWith(r1) OrElse r1.Contains(r2)) Then Continue For
                            If tri.myColourNumber <> 16 Then
                                Dim col As Color = tri.myColour
                                Dim colt As Color = Color.FromArgb(View.alpha, col.R, col.G, col.B)
                                Dim ci As Integer = colt.ToArgb
                                If Not brushDict.ContainsKey(ci) Then brushDict.Add(ci, New SolidBrush(colt))
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            ElseIf tri.groupindex <> -1 AndAlso tri.myColourNumber = 16 AndAlso LPCFile.PrimitivesHMap.ContainsKey(tri.groupindex) AndAlso LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColourNumber <> 16 Then
                                Dim col As Color = LPCFile.Primitives(LPCFile.PrimitivesHMap(tri.groupindex)).myColour
                                Dim colt As Color = Color.FromArgb(View.alpha, col.R, col.G, col.B)
                                Dim ci As Integer = colt.ToArgb
                                If Not brushDict.ContainsKey(ci) Then brushDict.Add(ci, New SolidBrush(colt))
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(brushDict(ci), pf)
                            Else
                                pf(0).X = -tri.vertexA.X
                                pf(0).Y = tri.vertexA.Y
                                pf(1).X = -tri.vertexB.X
                                pf(1).Y = tri.vertexB.Y
                                pf(2).X = -tri.vertexC.X
                                pf(2).Y = tri.vertexC.Y
                                pf(3).X = pf(0).X
                                pf(3).Y = pf(0).Y
                                e.Graphics.FillPolygon(hb, pf)
                            End If
                        Next
                    End If
                End If
            End If
        End If

        If Not BtnPreview.Checked Then
            ' Triangles
            Dim pf2(3) As PointF
            For i As Integer = 0 To LPCFile.Triangles.Count - 1
                Dim tri As Triangle = LPCFile.Triangles(i)
                r2.X = Math.Min(-tri.vertexA.X, Math.Min(-tri.vertexB.X, -tri.vertexC.X))
                r2.Y = Math.Min(tri.vertexA.Y, Math.Min(tri.vertexB.Y, tri.vertexC.Y))
                r2.Width = Math.Abs(Math.Max(-tri.vertexA.X, Math.Max(-tri.vertexB.X, -tri.vertexC.X)) - r2.X)
                r2.Height = Math.Abs(Math.Max(tri.vertexA.Y, Math.Max(tri.vertexB.Y, tri.vertexC.Y)) - r2.Y)
                If Not (r2.IntersectsWith(r1) OrElse r1.Contains(r2)) Then Continue For
                If Not tri.selected Then
                    If tri.myColourNumber <> 15 OrElse Not BtnColours.Checked Then
                        pf2(0).X = -tri.vertexA.X
                        pf2(0).Y = tri.vertexA.Y
                        pf2(1).X = -tri.vertexB.X
                        pf2(1).Y = tri.vertexB.Y
                        pf2(2).X = -tri.vertexC.X
                        pf2(2).Y = tri.vertexC.Y
                        pf2(3).X = pf2(0).X
                        pf2(3).Y = pf2(0).Y
                        If tri.groupindex = -1 Then
                            e.Graphics.DrawPolygon(LDSettings.Colours.linePen, pf2)
                        Else
                            e.Graphics.DrawPolygon(LDSettings.Colours.originPen, pf2)
                        End If
                    Else
                        pf2(0).X = -tri.vertexA.X
                        pf2(0).Y = tri.vertexA.Y
                        pf2(1).X = -tri.vertexB.X
                        pf2(1).Y = tri.vertexB.Y
                        pf2(2).X = -tri.vertexC.X
                        pf2(2).Y = tri.vertexC.Y
                        pf2(3).X = pf2(0).X
                        pf2(3).Y = pf2(0).Y
                        If tri.groupindex = -1 Then
                            e.Graphics.DrawPolygon(LDSettings.Colours.inverseLinePen, pf2)
                        Else
                            e.Graphics.DrawPolygon(LDSettings.Colours.originPen, pf2)
                        End If
                    End If
                End If
            Next
            If MainState.objectToModify <> Modified.Vertex Then
                For i As Integer = 0 To View.SelectedTriangles.Count - 1
                    Dim tri As Triangle = View.SelectedTriangles(i)
                    pf2(0).X = -tri.vertexA.X
                    pf2(0).Y = tri.vertexA.Y
                    pf2(1).X = -tri.vertexB.X
                    pf2(1).Y = tri.vertexB.Y
                    pf2(2).X = -tri.vertexC.X
                    pf2(2).Y = tri.vertexC.Y
                    pf2(3).X = pf2(0).X
                    pf2(3).Y = pf2(0).Y
                    e.Graphics.DrawPolygon(LDSettings.Colours.selectedLinePen, pf2)
                Next
            Else
                For i As Integer = 0 To View.SelectedTriangles.Count - 1
                    Dim tri As Triangle = View.SelectedTriangles(i)
                    pf2(0).X = -tri.vertexA.X
                    pf2(0).Y = tri.vertexA.Y
                    pf2(1).X = -tri.vertexB.X
                    pf2(1).Y = tri.vertexB.Y
                    pf2(2).X = -tri.vertexC.X
                    pf2(2).Y = tri.vertexC.Y
                    pf2(3).X = pf2(0).X
                    pf2(3).Y = pf2(0).Y
                    e.Graphics.DrawPolygon(LDSettings.Colours.selectedLineInVertexModePen, pf2)
                Next
            End If
            If MainState.trianglemode > 0 Then
                e.Graphics.DrawLine(Pens.PowderBlue, CType(-MainState.lastPointX, Single), CType(MainState.lastPointY, Single), CType((MouseHelper.getCursorpositionX() - absOffsetX) / View.zoomfactor, Single), CType((MouseHelper.getCursorpositionY() - absOffsetY) / View.zoomfactor, Single))
                If MainState.trianglemode = 2 Then
                    e.Graphics.DrawLine(Pens.PowderBlue, CType(-MainState.temp_vertices(0).X, Single), CType(MainState.temp_vertices(0).Y, Single), CType((MouseHelper.getCursorpositionX() - absOffsetX) / View.zoomfactor, Single), CType((MouseHelper.getCursorpositionY() - absOffsetY) / View.zoomfactor, Single))
                    e.Graphics.DrawLine(Pens.PowderBlue, CType(-MainState.temp_vertices(1).X, Single), CType(MainState.temp_vertices(1).Y, Single), CType(-MainState.temp_vertices(0).X, Single), CType(MainState.temp_vertices(0).Y, Single))
                End If
            End If

            e.Graphics.ScaleTransform(1 / View.zoomfactor, 1 / View.zoomfactor)
            If MainState.referenceLineMode > 0 Then
                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePenFat, CType(-MainState.lastPointX * View.zoomfactor, Single), CType(MainState.lastPointY * View.zoomfactor, Single), CType((MouseHelper.getCursorpositionX() - absOffsetX), Single), CType((MouseHelper.getCursorpositionY() - absOffsetY), Single))
            End If
            e.Graphics.TranslateTransform(-absOffsetX, -absOffsetY)


            If LDSettings.Editor.showTemplateLinesOnTop Then
                ' Templates:
                Dim shapeCount As Integer = LPCFile.templateShape.Count
                If shapeCount > 1 AndAlso Not BtnPreview.Checked Then
                    If Not LPCFile.templateShape(0).X = Single.Epsilon Then
                        Dim start As Integer = 0
                        Dim finish As Integer = shapeCount - 1
                        Dim templateShapeArray(finish) As PointF
                        For i As Integer = 0 To finish
                            templateShapeArray(i).X = CType(absOffsetX - LPCFile.templateShape(i).X * View.zoomfactor, Single)
                            templateShapeArray(i).Y = CType(absOffsetY + LPCFile.templateShape(i).Y * View.zoomfactor, Single)
                            If LPCFile.templateShape(i).X = Single.Epsilon Then
                                Dim lenght As Integer = i - start - 1
                                Dim templatePolyPart(lenght) As PointF
                                Array.Copy(templateShapeArray, start, templatePolyPart, 0, lenght + 1)
                                e.Graphics.DrawPolygon(LDSettings.Colours.templatePen, templatePolyPart)
                                start = i + 1
                            End If
                        Next
                        Dim lenght2 As Integer = finish - start
                        Dim templatePolyPart2(lenght2) As PointF
                        Array.Copy(templateShapeArray, start, templatePolyPart2, 0, lenght2 + 1)
                        e.Graphics.DrawPolygon(LDSettings.Colours.templatePen, templatePolyPart2)

                        For i = 0 To LPCFile.templateTexts.Count - 1
                            e.Graphics.DrawString(LPCFile.templateTexts(i).Text, New Font("Arial", (1000 * View.zoomfactor) + 8, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, absOffsetX - LPCFile.templateTexts(i).X * View.zoomfactor + 2, absOffsetY + LPCFile.templateTexts(i).Y * View.zoomfactor + 2)
                            e.Graphics.DrawString(LPCFile.templateTexts(i).Text, New Font("Arial", (1000 * View.zoomfactor) + 8, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, absOffsetX - LPCFile.templateTexts(i).X * View.zoomfactor, absOffsetY + LPCFile.templateTexts(i).Y * View.zoomfactor)
                        Next
                    Else
                        For i As Integer = 1 To shapeCount - 1 Step 4
                            Dim templateShapeArray(3) As PointF
                            templateShapeArray(0).X = absOffsetX - LPCFile.templateShape(i).X * View.zoomfactor
                            templateShapeArray(0).Y = absOffsetY + LPCFile.templateShape(i).Y * View.zoomfactor

                            If (i + 1) < shapeCount Then
                                templateShapeArray(1).X = absOffsetX - LPCFile.templateShape(i + 1).X * View.zoomfactor
                                templateShapeArray(1).Y = absOffsetY + LPCFile.templateShape(i + 1).Y * View.zoomfactor
                            Else
                                templateShapeArray(1).X = templateShapeArray(0).X
                                templateShapeArray(1).Y = templateShapeArray(0).Y
                            End If

                            If (i + 2) < shapeCount Then
                                templateShapeArray(2).X = absOffsetX - LPCFile.templateShape(i + 2).X * View.zoomfactor
                                templateShapeArray(2).Y = absOffsetY + LPCFile.templateShape(i + 2).Y * View.zoomfactor
                            Else
                                templateShapeArray(2).X = templateShapeArray(0).X
                                templateShapeArray(2).Y = templateShapeArray(0).Y
                            End If

                            If (i + 3) < shapeCount Then
                                templateShapeArray(3).X = absOffsetX - LPCFile.templateShape(i + 3).X * View.zoomfactor
                                templateShapeArray(3).Y = absOffsetY + LPCFile.templateShape(i + 3).Y * View.zoomfactor
                            Else
                                templateShapeArray(3).X = templateShapeArray(0).X
                                templateShapeArray(3).Y = templateShapeArray(0).Y
                            End If

                            e.Graphics.DrawPolygon(LDSettings.Colours.projectionPen, templateShapeArray)
                        Next
                    End If
                End If
            End If

            If LPCFile.helperLineStartIndex > -1 AndAlso LPCFile.helperLineEndIndex > -1 AndAlso LPCFile.helperLineStartIndex < LPCFile.helperLineEndIndex AndAlso LPCFile.helperLineEndIndex < LPCFile.templateShape.Count Then
                Dim templateShapeArray(LPCFile.helperLineEndIndex - LPCFile.helperLineStartIndex) As PointF
                Dim counter As Integer = 0
                For i As Integer = LPCFile.helperLineStartIndex To LPCFile.helperLineEndIndex
                    templateShapeArray(counter).X = absOffsetX - LPCFile.templateShape(i).X * View.zoomfactor
                    templateShapeArray(counter).Y = absOffsetY + LPCFile.templateShape(i).Y * View.zoomfactor
                    counter += 1
                Next
                e.Graphics.DrawPolygon(LDSettings.Colours.selectedLinePenFat, templateShapeArray)
            End If

            ' When triangle auto-completion is enabled, draw a circle for selection
            If BtnTriangleAutoCompletion.Checked Then
                Dim tX, tY, selectionRadius As Integer
                selectionRadius = View.selectionRadius
                tX = MouseHelper.getCursorpositionX() - selectionRadius
                tY = MouseHelper.getCursorpositionY() - selectionRadius
                selectionRadius = 2 * selectionRadius
                e.Graphics.DrawEllipse(LDSettings.Colours.selectionRectPen, tX, tY, selectionRadius, selectionRadius)
            End If

            e.Graphics.TranslateTransform(absOffsetX, absOffsetY)
            ' TODO Uncomment for debug purposes
            '' Projection Quad Data
            'Dim pfq(4) As PointF
            'For Each q As ProjectionQuad In LPCFile.templateProjectionQuads
            '    pfq(0).X = -q.inCoords(0, 0) * View.zoomfactor * 1000
            '    pfq(0).Y = q.inCoords(0, 1) * View.zoomfactor * 1000
            '    pfq(1).X = -q.inCoords(1, 0) * View.zoomfactor * 1000
            '    pfq(1).Y = q.inCoords(1, 1) * View.zoomfactor * 1000
            '    pfq(2).X = -q.inCoords(2, 0) * View.zoomfactor * 1000
            '    pfq(2).Y = q.inCoords(2, 1) * View.zoomfactor * 1000
            '    pfq(3).X = -q.inCoords(3, 0) * View.zoomfactor * 1000
            '    pfq(3).Y = q.inCoords(3, 1) * View.zoomfactor * 1000
            '    pfq(4).X = -q.inCoords(0, 0) * View.zoomfactor * 1000
            '    pfq(4).Y = q.inCoords(0, 1) * View.zoomfactor * 1000
            '    e.Graphics.DrawPolygon(Settings.Colours.originPen, pfq)
            'Next
            'If LPCFile.templateProjectionQuads.Count > 0 Then
            '    Dim q2 As ProjectionQuad = LPCFile.templateProjectionQuads(sq)
            '    pfq(0).X = -q2.inCoords(0, 0) * View.zoomfactor * 1000
            '    pfq(0).Y = q2.inCoords(0, 1) * View.zoomfactor * 1000
            '    pfq(1).X = -q2.inCoords(1, 0) * View.zoomfactor * 1000
            '    pfq(1).Y = q2.inCoords(1, 1) * View.zoomfactor * 1000
            '    pfq(2).X = -q2.inCoords(2, 0) * View.zoomfactor * 1000
            '    pfq(2).Y = q2.inCoords(2, 1) * View.zoomfactor * 1000
            '    pfq(3).X = -q2.inCoords(3, 0) * View.zoomfactor * 1000
            '    pfq(3).Y = q2.inCoords(3, 1) * View.zoomfactor * 1000
            '    pfq(4).X = -q2.inCoords(0, 0) * View.zoomfactor * 1000
            '    pfq(4).Y = q2.inCoords(0, 1) * View.zoomfactor * 1000
            '    e.Graphics.DrawPolygon(Settings.Colours.selectedLinePen, pfq)
            'End If

            ' LPCFile.Vertices
            Dim vs As New List(Of RectangleF) With {.Capacity = View.SelectedVertices.Count}
            Dim v As New List(Of RectangleF) With {.Capacity = LPCFile.Vertices.Count}
            For i As Integer = 0 To LPCFile.Vertices.Count - 1
                Dim vert As Vertex = LPCFile.Vertices(i)
                If e.Graphics.IsVisible(CType(-vert.X * View.zoomfactor, Single), CType(vert.Y * View.zoomfactor, Single)) Then
                    If vert.selected AndAlso MainState.objectToModify = Modified.Vertex Then
                        vs.Add(New RectangleF(-vert.X * View.zoomfactor - View.pointsizeHalf, vert.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                    Else
                        v.Add(New RectangleF(-vert.X * View.zoomfactor - View.pointsizeHalf, vert.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                    End If
                End If
            Next
            For Each vert As Vertex In View.TriangulationVertices
                vs.Add(New RectangleF(-vert.X * View.zoomfactor - View.pointsizeHalf, vert.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
            Next
            If v.Count > 0 Then e.Graphics.FillRectangles(LDSettings.Colours.vertexBrush, v.ToArray)
            If vs.Count > 0 Then e.Graphics.FillRectangles(LDSettings.Colours.selectedVertexBrush, vs.ToArray)
            If MainState.trianglemode > 0 OrElse MainState.referenceLineMode > 0 Then
                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(-MainState.lastPointX * View.zoomfactor - View.pointsizeHalf, MainState.lastPointY * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
            End If
            e.Graphics.TranslateTransform(-absOffsetX, -absOffsetY)
            For i As Integer = 0 To View.CollisionVertices.Count - 1
                Dim vert As Vertex = View.CollisionVertices(i)
                e.Graphics.TranslateTransform(CType(absOffsetX - vert.X * View.zoomfactor - 16, Single), CType(absOffsetY + vert.Y * View.zoomfactor - 16, Single))
                e.Graphics.FillRectangle(View.collosionBrush, 0, 0, 32, 32)
                e.Graphics.ResetTransform()
            Next

            If MainState.movemode Then
                For Each vert As Vertex In View.SelectedVertices
                    vert.X += View.viewAbsOffsetX
                    vert.Y -= View.viewAbsOffsetY
                Next
            ElseIf MainState.rotatemode OrElse MainState.scalemode Then
                If MainState.scalemode Then
                    Dim factorToScale As Double = 1 + Fix((MouseHelper.getCursorpositionX() - MainState.klickX) / 100 / View.scaleSnap) * View.scaleSnap
                    If factorToScale < 0.1 Then factorToScale = 0.1
                    If factorToScale > 2 Then factorToScale = 2
                    Dim pf(4) As PointF
                    pf(0).X = MainState.klickX - 100
                    pf(0).Y = MainState.klickY - 5
                    pf(1).X = MainState.klickX + 100
                    pf(1).Y = MainState.klickY - 5
                    pf(2).X = MainState.klickX + 100
                    pf(2).Y = MainState.klickY + 13
                    pf(3).X = MainState.klickX - 100
                    pf(3).Y = MainState.klickY + 13
                    pf(4).X = MainState.klickX - 100
                    pf(4).Y = MainState.klickY - 5
                    e.Graphics.FillPolygon(Brushes.WhiteSmoke, pf)
                    e.Graphics.DrawString("0%                             100%                       200%", New Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, MainState.klickX - 100, MainState.klickY + 3)
                    For x As Integer = -100 To 100 Step 10
                        e.Graphics.DrawLine(Pens.DarkGray, MainState.klickX + x, MainState.klickY - 5, MainState.klickX + x, MainState.klickY + 5)
                    Next x
                    e.Graphics.DrawString(Fix(factorToScale * 100) & "%", New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Red, MainState.klickX + 5, MainState.klickY - 7)
                    e.Graphics.DrawLine(Pens.Black, MainState.klickX, MainState.klickY - 5, MainState.klickX, MainState.klickY + 5)
                Else
                    e.Graphics.FillRectangle(LDSettings.Colours.vertexBrush, New RectangleF(absOffsetX - MainState.temp_center.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + MainState.temp_center.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                End If
                For Each vert As Vertex In View.SelectedVertices
                    vert.X = vert.oldX
                    vert.Y = vert.oldY
                Next
            End If
        End If

        ' Selection Rectangle
        If MainState.doSelection AndAlso MainState.objectToModify <> Modified.Primitive AndAlso Not MainState.adjustmode Then
            Dim tX, tY, tW, tH As Integer
            tX = Math.Min(MainState.klickX, MouseHelper.getCursorpositionX)
            tY = Math.Min(MainState.klickY, MouseHelper.getCursorpositionY)
            tW = Math.Abs(MouseHelper.getCursorpositionX() - MainState.klickX)
            tH = Math.Abs(MouseHelper.getCursorpositionY() - MainState.klickY)
            e.Graphics.DrawRectangle(LDSettings.Colours.selectionRectPen, tX, tY, tW, tH)
        End If

        ' Add Vertex
        If BtnAddVertex.Checked Then
            Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
        End If

        ' Primitive Modes
        If MainState.primitiveMode > PrimitiveModes.Inactive Then
            Select Case MainState.primitiveMode
                Case PrimitiveModes.SetTheOrigin
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.POrigin), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.POrigin), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    MainState.primitiveCenter = New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 1, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 16, 3, 33))
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 16, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 1, 33, 3))
                    Exit Select
                Case PrimitiveModes.SetTheHeight
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.PHeight), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.PHeight), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 1, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 16, 2, 32))
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 16, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 1, 32, 2))
                    MainState.primitiveHeight = Fix(getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y)
                    Select Case MainState.primitiveObject
                        Case Is < 3
                            Dim v1, v2, v3 As Vertex
                            v1 = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v2 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveHeight, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False, False)
                            v3 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveHeight, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False, False)
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v2.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v2.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v3.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v3.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single), CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single), CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single))
                            Exit Select
                        Case Is < 6
                            Dim v1, v2, v3, v4 As Vertex
                            v1 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveHeight, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v2 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveHeight, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v3 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveHeight, MainState.primitiveCenter.Y - MainState.primitiveHeight, False, False)
                            v4 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveHeight, MainState.primitiveCenter.Y - MainState.primitiveHeight, False, False)
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v2.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v2.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v3.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v3.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v4.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v4.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single), CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single), CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single), CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single))
                            Exit Select
                        Case Is < 9
                            MainState.primitiveHeight = Fix(Math.Sqrt((getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y) ^ 2 + (getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) ^ 2))
                            Dim vcenter As Vertex
                            Dim vouter(LDSettings.Editor.segments_circle) As Vertex
                            Dim counter As Integer = 0
                            vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False, False)
                            For a As Double = 0 To Math.PI * 2 Step Math.PI * 2 / LDSettings.Editor.segments_circle
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveHeight, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False, False)
                                counter += 1
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(0).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(0).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_circle - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_circle - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                            For counter = 1 To LDSettings.Editor.segments_circle - 1
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(counter).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(counter).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vcenter.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vcenter.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Exit Select
                        Case Is < 12
                            MainState.primitiveHeight = Fix(Math.Sqrt((getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y) ^ 2 + (getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) ^ 2))
                            Dim vcenter As Vertex
                            Dim vouter(LDSettings.Editor.segments_oval) As Vertex
                            Dim counter As Integer = 0
                            vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False, False)
                            For a As Double = 0 To Math.PI * 2 Step Math.PI * 2 / LDSettings.Editor.segments_oval
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveHeight, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False, False)
                                counter += 1
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(0).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(0).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_oval - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_oval - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                            For counter = 1 To LDSettings.Editor.segments_oval - 1
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(counter).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(counter).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vcenter.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vcenter.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Exit Select
                    End Select
                    Exit Select
                Case PrimitiveModes.SetTheWidth
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.PWidth), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.PWidth), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 1, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 16, 2, 32))
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 16, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 1, 32, 2))
                    MainState.primitiveWidth = Fix(getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X)
                    Select Case MainState.primitiveObject
                        Case Is < 3
                            Dim v1, v2, v3 As Vertex
                            v1 = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v2 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveHeight + MainState.primitiveWidth, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False, False)
                            v3 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveHeight - MainState.primitiveWidth, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False, False)
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v2.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v2.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v3.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v3.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single), CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single), CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single))
                            Exit Select
                        Case Is < 6
                            Dim v1, v2, v3, v4 As Vertex
                            v1 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v2 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v3 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False, False)
                            v4 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False, False)
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v2.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v2.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v3.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v3.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v4.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v4.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single), CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single), CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single), CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single))
                            Exit Select
                        Case Is < 12
                            MainState.primitiveWidth = Fix(Math.Sqrt((getYcoordinate(MouseHelper.getCursorpositionY()) - MainState.primitiveCenter.Y) ^ 2 + (getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) ^ 2))
                            Dim vcenter As Vertex
                            Dim vouter(LDSettings.Editor.segments_oval) As Vertex
                            Dim counter As Integer = 0
                            vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False, False)
                            For a As Double = 0 To Math.PI * 2 Step Math.PI * 2 / LDSettings.Editor.segments_oval
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveWidth, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False, False)
                                counter += 1
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(0).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(0).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_oval - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_oval - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                            For counter = 1 To LDSettings.Editor.segments_oval - 1
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(counter).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(counter).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vcenter.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vcenter.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Exit Select
                    End Select
                    Exit Select
                Case PrimitiveModes.SetTheFrameSize
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.PFrameSize), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.PFrameSize), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 1, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 16, 2, 32))
                    e.Graphics.FillRectangle(LDSettings.Colours.selectionCrossBrush, New RectangleF(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor - 16, absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor - 1, 32, 2))
                    MainState.primitiveBordersize = Fix(getXcoordinate(MouseHelper.getCursorpositionX()) - MainState.primitiveCenter.X) / Math.Sqrt(MainState.primitiveWidth ^ 2 + MainState.primitiveHeight ^ 2)
                    If MainState.primitiveBordersize < 0 Then MainState.primitiveBordersize *= -1
                    MathHelper.clip(MainState.primitiveBordersize, 0, 1)
                    Select Case MainState.primitiveObject
                        Case Is < 3
                            Dim v11, v21, v31 As Vertex
                            v11 = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v21 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveHeight + MainState.primitiveWidth, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False, False)
                            v31 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveHeight - MainState.primitiveWidth, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2), False, False)
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v11.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v11.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v21.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v21.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v31.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v31.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v11.X * View.zoomfactor, Single), CType(absOffsetY + v11.Y * View.zoomfactor, Single), CType(absOffsetX - v21.X * View.zoomfactor, Single), CType(absOffsetY + v21.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v21.X * View.zoomfactor, Single), CType(absOffsetY + v21.Y * View.zoomfactor, Single), CType(absOffsetX - v31.X * View.zoomfactor, Single), CType(absOffsetY + v31.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v31.X * View.zoomfactor, Single), CType(absOffsetY + v31.Y * View.zoomfactor, Single), CType(absOffsetX - v11.X * View.zoomfactor, Single), CType(absOffsetY + v11.Y * View.zoomfactor, Single))
                            Dim v12, v22, v32 As Vertex
                            v12 = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y + MainState.primitiveHeight * MainState.primitiveBordersize, False, False)
                            v22 = New Vertex(MainState.primitiveCenter.X + (MainState.primitiveHeight + MainState.primitiveWidth) * MainState.primitiveBordersize, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2) * MainState.primitiveBordersize, False, False)
                            v32 = New Vertex(MainState.primitiveCenter.X - (MainState.primitiveHeight + MainState.primitiveWidth) * MainState.primitiveBordersize, MainState.primitiveCenter.Y - (MainState.primitiveHeight / 2) * MainState.primitiveBordersize, False, False)
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v12.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v12.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v22.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v22.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v32.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v32.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v12.X * View.zoomfactor, Single), CType(absOffsetY + v12.Y * View.zoomfactor, Single), CType(absOffsetX - v22.X * View.zoomfactor, Single), CType(absOffsetY + v22.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v22.X * View.zoomfactor, Single), CType(absOffsetY + v22.Y * View.zoomfactor, Single), CType(absOffsetX - v32.X * View.zoomfactor, Single), CType(absOffsetY + v32.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v32.X * View.zoomfactor, Single), CType(absOffsetY + v32.Y * View.zoomfactor, Single), CType(absOffsetX - v12.X * View.zoomfactor, Single), CType(absOffsetY + v12.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v11.X * View.zoomfactor, Single), CType(absOffsetY + v11.Y * View.zoomfactor, Single), CType(absOffsetX - v12.X * View.zoomfactor, Single), CType(absOffsetY + v12.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v21.X * View.zoomfactor, Single), CType(absOffsetY + v21.Y * View.zoomfactor, Single), CType(absOffsetX - v22.X * View.zoomfactor, Single), CType(absOffsetY + v22.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v31.X * View.zoomfactor, Single), CType(absOffsetY + v31.Y * View.zoomfactor, Single), CType(absOffsetX - v32.X * View.zoomfactor, Single), CType(absOffsetY + v32.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v11.X * View.zoomfactor, Single), CType(absOffsetY + v11.Y * View.zoomfactor, Single), CType(absOffsetX - v22.X * View.zoomfactor, Single), CType(absOffsetY + v22.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v21.X * View.zoomfactor, Single), CType(absOffsetY + v21.Y * View.zoomfactor, Single), CType(absOffsetX - v32.X * View.zoomfactor, Single), CType(absOffsetY + v32.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v31.X * View.zoomfactor, Single), CType(absOffsetY + v31.Y * View.zoomfactor, Single), CType(absOffsetX - v12.X * View.zoomfactor, Single), CType(absOffsetY + v12.Y * View.zoomfactor, Single))
                            Exit Select
                        Case Is < 6
                            Dim v1, v2, v3, v4, v5, v6, v7, v8 As Vertex
                            v1 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v2 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y + MainState.primitiveHeight, False, False)
                            v3 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False, False)
                            v4 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth, MainState.primitiveCenter.Y - MainState.primitiveHeight, False, False)
                            v5 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y + MainState.primitiveHeight * MainState.primitiveBordersize, False, False)
                            v6 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y + MainState.primitiveHeight * MainState.primitiveBordersize, False, False)
                            v7 = New Vertex(MainState.primitiveCenter.X - MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y - MainState.primitiveHeight * MainState.primitiveBordersize, False, False)
                            v8 = New Vertex(MainState.primitiveCenter.X + MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y - MainState.primitiveHeight * MainState.primitiveBordersize, False, False)
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v2.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v2.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v3.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v3.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v4.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v4.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v5.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v5.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v6.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v6.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v7.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v7.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v8.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v8.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            ' Kontur: Rechteck aussen
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single), CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single), CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single), CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single))
                            ' Kontur: Rechteck innen
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v5.X * View.zoomfactor, Single), CType(absOffsetY + v5.Y * View.zoomfactor, Single), CType(absOffsetX - v6.X * View.zoomfactor, Single), CType(absOffsetY + v6.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v6.X * View.zoomfactor, Single), CType(absOffsetY + v6.Y * View.zoomfactor, Single), CType(absOffsetX - v7.X * View.zoomfactor, Single), CType(absOffsetY + v7.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v7.X * View.zoomfactor, Single), CType(absOffsetY + v7.Y * View.zoomfactor, Single), CType(absOffsetX - v8.X * View.zoomfactor, Single), CType(absOffsetY + v8.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v8.X * View.zoomfactor, Single), CType(absOffsetY + v8.Y * View.zoomfactor, Single), CType(absOffsetX - v5.X * View.zoomfactor, Single), CType(absOffsetY + v5.Y * View.zoomfactor, Single))
                            ' Diagonale: Innen
                            If MainState.primitiveObject = PrimitiveObjects.RectangleWithFrame Then e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v5.X * View.zoomfactor, Single), CType(absOffsetY + v5.Y * View.zoomfactor, Single), CType(absOffsetX - v7.X * View.zoomfactor, Single), CType(absOffsetY + v7.Y * View.zoomfactor, Single))
                            ' Verbindungslinien: Aussen/Innen
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v5.X * View.zoomfactor, Single), CType(absOffsetY + v5.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single), CType(absOffsetX - v6.X * View.zoomfactor, Single), CType(absOffsetY + v6.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single), CType(absOffsetX - v7.X * View.zoomfactor, Single), CType(absOffsetY + v7.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single), CType(absOffsetX - v8.X * View.zoomfactor, Single), CType(absOffsetY + v8.Y * View.zoomfactor, Single))
                            ' Verbindungslinien: Diagonal
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v1.X * View.zoomfactor, Single), CType(absOffsetY + v1.Y * View.zoomfactor, Single), CType(absOffsetX - v6.X * View.zoomfactor, Single), CType(absOffsetY + v6.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v2.X * View.zoomfactor, Single), CType(absOffsetY + v2.Y * View.zoomfactor, Single), CType(absOffsetX - v7.X * View.zoomfactor, Single), CType(absOffsetY + v7.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v3.X * View.zoomfactor, Single), CType(absOffsetY + v3.Y * View.zoomfactor, Single), CType(absOffsetX - v8.X * View.zoomfactor, Single), CType(absOffsetY + v8.Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - v4.X * View.zoomfactor, Single), CType(absOffsetY + v4.Y * View.zoomfactor, Single), CType(absOffsetX - v5.X * View.zoomfactor, Single), CType(absOffsetY + v5.Y * View.zoomfactor, Single))
                            Exit Select
                        Case Is < 9
                            Dim vcenter As Vertex
                            Dim vinner(LDSettings.Editor.segments_circle) As Vertex
                            Dim vouter(LDSettings.Editor.segments_circle) As Vertex
                            Dim counter As Integer = 0
                            vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False, False)
                            For a As Double = 0 To Math.PI * 2 Step Math.PI * 2 / LDSettings.Editor.segments_circle
                                vinner(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveHeight * MainState.primitiveBordersize, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight * MainState.primitiveBordersize, False, False)
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveHeight, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False, False)
                                counter += 1
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(0).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(0).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vinner(0).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vinner(0).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_circle - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_circle - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(0).X * View.zoomfactor, Single), CType(absOffsetY + vinner(0).Y * View.zoomfactor, Single), CType(absOffsetX - vinner(LDSettings.Editor.segments_circle - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(LDSettings.Editor.segments_circle - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(0).X * View.zoomfactor, Single), CType(absOffsetY + vinner(0).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_circle - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_circle - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(LDSettings.Editor.segments_circle - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(LDSettings.Editor.segments_circle - 1).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_circle - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_circle - 1).Y * View.zoomfactor, Single))
                            If MainState.primitiveObject = PrimitiveObjects.CircleWithFrame Then e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(0).X * View.zoomfactor, Single), CType(absOffsetY + vinner(0).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                            For counter = 1 To LDSettings.Editor.segments_circle - 1
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vinner(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter - 1).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                If MainState.primitiveObject = PrimitiveObjects.CircleWithFrame Then e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(counter).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(counter).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vinner(counter).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vinner(counter).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vcenter.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vcenter.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Exit Select
                        Case Is < 12
                            Dim vcenter As Vertex
                            Dim vinner(LDSettings.Editor.segments_oval) As Vertex
                            Dim vouter(LDSettings.Editor.segments_oval) As Vertex
                            Dim counter As Integer = 0
                            vcenter = New Vertex(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, False, False)
                            For a As Double = 0 To Math.PI * 2 Step Math.PI * 2 / LDSettings.Editor.segments_oval
                                vinner(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveWidth * MainState.primitiveBordersize, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight * MainState.primitiveBordersize, False, False)
                                vouter(counter) = New Vertex(MainState.primitiveCenter.X + Math.Cos(a) * MainState.primitiveWidth, MainState.primitiveCenter.Y + Math.Sin(a) * MainState.primitiveHeight, False, False)
                                counter += 1
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(0).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(0).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vinner(0).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vinner(0).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(0).X * View.zoomfactor, Single), CType(absOffsetY + vouter(0).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_oval - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_oval - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(0).X * View.zoomfactor, Single), CType(absOffsetY + vinner(0).Y * View.zoomfactor, Single), CType(absOffsetX - vinner(LDSettings.Editor.segments_oval - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(LDSettings.Editor.segments_oval - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(0).X * View.zoomfactor, Single), CType(absOffsetY + vinner(0).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_oval - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_oval - 1).Y * View.zoomfactor, Single))
                            e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(LDSettings.Editor.segments_oval - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(LDSettings.Editor.segments_oval - 1).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(LDSettings.Editor.segments_oval - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(LDSettings.Editor.segments_oval - 1).Y * View.zoomfactor, Single))
                            If MainState.primitiveObject = PrimitiveObjects.OvalWithFrame Then e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(0).X * View.zoomfactor, Single), CType(absOffsetY + vinner(0).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                            For counter = 1 To LDSettings.Editor.segments_oval - 1
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vouter(counter).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vinner(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter - 1).Y * View.zoomfactor, Single), CType(absOffsetX - vouter(counter - 1).X * View.zoomfactor, Single), CType(absOffsetY + vouter(counter - 1).Y * View.zoomfactor, Single))
                                If MainState.primitiveObject = PrimitiveObjects.OvalWithFrame Then e.Graphics.DrawLine(LDSettings.Colours.selectedLinePen, CType(absOffsetX - vinner(counter).X * View.zoomfactor, Single), CType(absOffsetY + vinner(counter).Y * View.zoomfactor, Single), CType(absOffsetX - vcenter.X * View.zoomfactor, Single), CType(absOffsetY + vcenter.Y * View.zoomfactor, Single))
                                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vouter(counter).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vouter(counter).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                                e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vinner(counter).X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vinner(counter).Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Next
                            e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - vcenter.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + vcenter.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                            Exit Select
                    End Select
                Case PrimitiveModes.SetSplineStartingPoint
                    ' Spline Starting Point
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineStart), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineStart), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                    e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                Case PrimitiveModes.SetSplineStartingDirection
                    ' Spline Starting Direction
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineFirstDir), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineFirstDir), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    Dim cap1 As New AdjustableArrowCap(1, 1, False)
                    Dim cap2 As New AdjustableArrowCap(2, 1)
                    cap1.BaseCap = LineCap.Round
                    cap1.BaseInset = 5
                    cap1.StrokeJoin = LineJoin.Round
                    cap2.WidthScale = 3
                    cap2.BaseCap = LineCap.Triangle
                    cap2.Height = 10
                    Dim blackPen As New Pen(LDSettings.Colours.linePen.Color, 1)
                    blackPen.CustomStartCap = cap1
                    blackPen.CustomEndCap = cap2
                    Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                    Dim v2 As Vertex = ListHelper.LLast(MainState.Splines).startAt
                    e.Graphics.DrawLine(blackPen, CSng(absOffsetX - v2.X * View.zoomfactor), CSng(absOffsetY + v2.Y * View.zoomfactor), CSng(absOffsetX - v1.X * View.zoomfactor), CSng(absOffsetY + v1.Y * View.zoomfactor))
                Case PrimitiveModes.SetSplineNextPoint
                    ' Spline Next Point
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineNext), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineNext), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                    e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v1.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v1.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                Case PrimitiveModes.SetSplineNextDirection
                    ' Spline Next Direction
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineNextDir), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineNextDir), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    Dim cap1 As New AdjustableArrowCap(1, 1, False)
                    Dim cap2 As New AdjustableArrowCap(2, 1)
                    cap1.BaseCap = LineCap.Round
                    cap1.BaseInset = 5
                    cap1.StrokeJoin = LineJoin.Round
                    cap2.WidthScale = 3
                    cap2.BaseCap = LineCap.Triangle
                    cap2.Height = 10
                    Dim blackPen As New Pen(LDSettings.Colours.linePen.Color, 1)
                    blackPen.CustomStartCap = cap1
                    blackPen.CustomEndCap = cap2
                    Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                    Dim v2 As Vertex = ListHelper.LLast(MainState.Splines).stopAt
                    e.Graphics.DrawLine(blackPen, CSng(absOffsetX - v2.X * View.zoomfactor), CSng(absOffsetY + v2.Y * View.zoomfactor), CSng(absOffsetX - v1.X * View.zoomfactor), CSng(absOffsetY + v1.Y * View.zoomfactor))
                Case PrimitiveModes.SetSplineWidthNSegments
                    ' Spline Segments
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineSegCount), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.SplineSegCount), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                Case PrimitiveModes.CreateTriangleChain
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.TriangleChainAdd), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 12, 32)
                    e.Graphics.DrawString(I18N.trl8(I18N.lk.TriangleChainAdd), New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 10, 30)
                    MainState.primitiveCenter = New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                    e.Graphics.DrawLine(Pens.RoyalBlue, CType(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor, Single), CType(absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor, Single), CType(absOffsetX - ListHelper.LLast(LPCFile.Vertices).X * View.zoomfactor, Single), CType(absOffsetY + ListHelper.LLast(LPCFile.Vertices).Y * View.zoomfactor, Single))
                    e.Graphics.DrawLine(Pens.PowderBlue, CType(absOffsetX - MainState.primitiveCenter.X * View.zoomfactor, Single), CType(absOffsetY + MainState.primitiveCenter.Y * View.zoomfactor, Single), CType(absOffsetX - LPCFile.Vertices(LPCFile.Vertices.Count - 2).X * View.zoomfactor, Single), CType(absOffsetY + LPCFile.Vertices(LPCFile.Vertices.Count - 2).Y * View.zoomfactor, Single))
                    e.Graphics.DrawLine(Pens.PowderBlue, CType(absOffsetX - ListHelper.LLast(LPCFile.Vertices).X * View.zoomfactor, Single), CType(absOffsetY + ListHelper.LLast(LPCFile.Vertices).Y * View.zoomfactor, Single), CType(absOffsetX - LPCFile.Vertices(LPCFile.Vertices.Count - 2).X * View.zoomfactor, Single), CType(absOffsetY + LPCFile.Vertices(LPCFile.Vertices.Count - 2).Y * View.zoomfactor, Single))
                    Exit Select
            End Select
            If MainState.primitiveMode > PrimitiveModes.SetSplineStartingPoint Then
                For Each s As Spline In MainState.Splines
                    e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - s.startAt.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + s.startAt.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                    If Not s.stopAt Is Nothing Then
                        e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - s.stopAt.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + s.stopAt.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                    End If
                Next
                If MainState.primitiveMode > PrimitiveModes.SetSplineStartingDirection AndAlso MainState.primitiveMode <> PrimitiveModes.CreateTriangleChain Then
                    Dim v1 As New Vertex(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap) * View.moveSnap, Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap) * View.moveSnap, False, False)
                    ListHelper.LLast(MainState.Splines).calculateSimulationGeometry(v1.X, v1.Y)
                    For Each v As Vertex In ListHelper.LLast(MainState.Splines).Vertices
                        e.Graphics.FillRectangle(LDSettings.Colours.selectedVertexBrush, New RectangleF(absOffsetX - v.X * View.zoomfactor - View.pointsizeHalf, absOffsetY + v.Y * View.zoomfactor - View.pointsizeHalf, View.pointsize, View.pointsize))
                    Next
                End If
            End If
        End If

        ' Axis Label:
        If ShowAxisLabelToolStripMenuItem.Checked Then
            Dim scale As Double
            If View.zoomlevel > 80 Then scale = View.rasterSnap * View.zoomfactor : GoTo label_zeichnen
            If View.zoomlevel > 0 Then scale = View.rasterSnap * View.zoomfactor * 10.0 : GoTo label_zeichnen
            If View.zoomlevel > -10 Then scale = View.rasterSnap * View.zoomfactor * 100.0 : GoTo label_zeichnen
            If View.zoomlevel > -20 Then scale = View.rasterSnap * View.zoomfactor * 1000.0 : GoTo label_zeichnen
            If View.zoomlevel > -30 Then scale = View.rasterSnap * View.zoomfactor * 10000.0 : GoTo label_zeichnen
            scale = View.rasterSnap * View.zoomfactor * 100000.0
label_zeichnen:
            Dim relOffsetX As Double
            Dim relOffsetY As Double
            relOffsetX = (View.offsetX * View.zoomfactor + CType(Me.ClientSize.Width, Double) / 2.0) Mod (scale * 10.0)
            relOffsetY = (View.offsetY * View.zoomfactor + CType(Me.ClientSize.Height, Double) / 2.0) Mod (scale * 10.0)
            e.Graphics.FillRectangle(Brushes.LightGray, 0, Me.ClientSize.Height - 46, Me.ClientSize.Width, Me.ClientSize.Height)
            e.Graphics.FillRectangle(Brushes.LightGray, 0, 0, 46, Me.ClientSize.Height)
            If relOffsetX < 46.0 Then relOffsetX = relOffsetX + scale * 10.0
            For x As Double = relOffsetX To Me.ClientSize.Width Step scale * 10.0
                e.Graphics.DrawString(Math.Round(getXcoordinateD(x) / View.moveSnap * View.unitFactor * 10.0) * View.moveSnap / 10000.0, New Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, x + 2, Me.ClientSize.Height - 42)
                e.Graphics.DrawString(Math.Round(getXcoordinateD(x) / View.moveSnap * View.unitFactor * 10.0) * View.moveSnap / 10000.0, New Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, x, Me.ClientSize.Height - 44)
            Next
            For y As Double = relOffsetY To Me.ClientSize.Height Step scale * 10.0
                e.Graphics.DrawString(Math.Round(getYcoordinateD(y) / View.moveSnap * View.unitFactor * 10.0) * View.moveSnap / 10000.0, New Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Azure, 6, y + 2)
                e.Graphics.DrawString(Math.Round(getYcoordinateD(y) / View.moveSnap * View.unitFactor * 10.0) * View.moveSnap / 10000.0, New Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, 4, y)
            Next
        End If
        ' Detect Nearest Edge while adding new triangles
        selectNearestTriangleEdgeForNewObjects()
    End Sub

    Private Function PolygonCollidesWithReferenceLine(a As Vertex, b As Vertex, c As Vertex, polygon() As PointF) As Boolean
        If polygon.Length <= 1 Then Return False
        Dim p1 As Vertex
        Dim p2 As Vertex = Nothing

        For i As Integer = 0 To polygon.Length - 2
            p1 = New Vertex(polygon(i))
            p2 = New Vertex(polygon(i + 1))
            If CSG.intersectionBetweenTwoLines(a, b, p1, p2) IsNot Nothing OrElse
               CSG.intersectionBetweenTwoLines(b, c, p1, p2) IsNot Nothing OrElse
               CSG.intersectionBetweenTwoLines(c, a, p1, p2) IsNot Nothing Then
                Return True
            End If
        Next

        If p2 Is Nothing Then Return False
        p1 = New Vertex(polygon(0))

        If CSG.intersectionBetweenTwoLines(a, b, p1, p2) IsNot Nothing OrElse
           CSG.intersectionBetweenTwoLines(b, c, p1, p2) IsNot Nothing OrElse
           CSG.intersectionBetweenTwoLines(c, a, p1, p2) IsNot Nothing Then
            Return True
        End If
        Return False
    End Function

    Private Sub BtnTriangleAutoCompletion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnTriangleAutoCompletion.Click
        If BtnAddTriangle.Checked Then BtnAddTriangle.PerformClick()
        If BtnAddReferenceLine.Checked Then BtnAddReferenceLine.PerformClick()
        View.TriangulationVertices.Clear()
        View.TriangulationVerticesInCircle.Clear()
        BtnAddTriangle.Checked = False
        BtnAddReferenceLine.Checked = False
        BtnAddTriangle.Enabled = Not BtnTriangleAutoCompletion.Checked
        BtnAddReferenceLine.Enabled = Not BtnTriangleAutoCompletion.Checked
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub BtnColours_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnColours.Click
        ColourToolStrip.Visible = BtnColours.Checked
        BtnPreview.Checked = BtnPreview.Checked AndAlso BtnColours.Checked
        Me.Refresh()
    End Sub

    Private Sub BtnAutoRound_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAutoRound.Click
        For Each vert As Vertex In View.SelectedVertices
            If vert.groupindex = Primitive.NO_INDEX Then
                vert.X = Math.Round(vert.X)
                vert.Y = Math.Round(vert.Y)
            End If
        Next
        Me.Refresh()
    End Sub

    Private Sub ProjectOnYZPlaneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectOnYZPlaneToolStripMenuItem.Click
        importDat(0)
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub ProjectOnYZPlane2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectOnYZPlane2ToolStripMenuItem.Click
        importDat(3)
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub ProjectOnZXPlaneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectOnZXPlaneToolStripMenuItem.Click
        importDat(1)
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub ProjectOnZXPlane2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectOnZXPlane2ToolStripMenuItem.Click
        importDat(4)
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub ProjectOnXYPlaneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectOnXYPlaneToolStripMenuItem.Click
        importDat(2)
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub ProjectOnXYPlane2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectOnXYPlane2ToolStripMenuItem.Click
        importDat(5)
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub importDat(ByVal projectMode As Byte)
newTry:
        MainState.primitiveCenter.X = 0
        MainState.primitiveCenter.Y = 0
        Dim mySubfiles As New List(Of String)
        Dim startVertex As Integer = LPCFile.Vertices.Count
        Dim endVertex As Integer = 0
        Dim startTriangle As Integer = LPCFile.Triangles.Count
        Dim endTriangle As Integer = 0
        Dim iDialog As Form = ImportDialog
        Dim result3 As DialogResult
        result3 = iDialog.ShowDialog()
        If result3 = Windows.Forms.DialogResult.Cancel Then iDialog.Dispose() : Exit Sub
        Dim RBnew As RadioButton = CType(iDialog.Controls("GBMode").Controls("RBnew"), RadioButton)
        Dim RBtemplate As RadioButton = CType(iDialog.Controls("GBMode").Controls("RBtemplate"), RadioButton)
        If (RBnew.Checked OrElse RBtemplate.Checked) AndAlso MainState.unsavedChanges AndAlso ShowAllWarningsToolStripMenuItem.Checked Then
            Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.UnsavedChanges), MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
            If result = MsgBoxResult.Yes Then SaveToolStripMenuItem.PerformClick()
            If result = MsgBoxResult.Cancel Then iDialog.Dispose() : Exit Sub
        End If
        BtnMove.PerformClick()
        MainState.isLoading = True
        MenuStrip1.Enabled = False
        Me.MainToolStrip.Enabled = False
        Me.ColourToolStrip.Enabled = False
        ImageForm.Enabled = False
        PreferencesForm.Enabled = False
        Select Case projectMode
            Case 0 ' YZ mit -X (Right) [OK]
                OpenDAT.Title = I18N.trl8(I18N.lk.ImportDATRight)
            Case 3 ' YZ mit +X (Left) [OK]
                OpenDAT.Title = I18N.trl8(I18N.lk.ImportDATLeft)
            Case 1 ' XZ mit -Y (Top) [OK]
                OpenDAT.Title = I18N.trl8(I18N.lk.ImportDATTop)
            Case 4 ' XZ mit -Y (Bottom) [OK]
                OpenDAT.Title = I18N.trl8(I18N.lk.ImportDATBottom)
            Case 2 ' XY mit -Z (Front) [OK]
                OpenDAT.Title = I18N.trl8(I18N.lk.ImportDATFront)
            Case 5 ' XY mit +Z (Back) [OK]
                OpenDAT.Title = I18N.trl8(I18N.lk.ImportDATBack)
        End Select
        Dim result2 As DialogResult
        result2 = OpenDAT.ShowDialog()
        If result2 = Windows.Forms.DialogResult.OK Then
            If OpenDAT.FileName <> "" Then
                Try
                    If RBnew.Checked OrElse RBtemplate.Checked Then
                        If RBtemplate.Checked Then
                            MainState.isLoading = True
                            LPCFile.myMetadata = New Metadata() With {.isMainMetadata = True}
                            For r As Integer = 0 To 3
                                For c As Integer = 0 To 3
                                    LPCFile.myMetadata.matrix(r, c) = 0
                                    If c = r Then LPCFile.myMetadata.matrix(r, c) = 1
                                Next
                            Next
                            LPCFile.templateShape.Clear()
                            LPCFile.templateProjectionQuads.Clear()
                            LPCFile.templateTexts.Clear()
                            MainState.isLoading = False
                            LPCFile.templateShape.Add(New PointF(Single.Epsilon, Single.Epsilon))
                            scaleVertices(True)
                        Else
                            startVertex = 0
                            startTriangle = 0
                            newPattern()
                        End If
                    Else
                        Dim subfileMetadata As New Metadata()
                        subfileMetadata.additionalData = ""
                        EnvironmentPaths.folderPath = Path.GetDirectoryName(OpenDAT.FileName)
                        Dim m1 As New Matrix3D
                        Select Case projectMode
                            Case 0 ' YZ mit -X (Right) [OK]
                                m1.m(0, 0) = 0.0 : m1.m(0, 1) = 0.0 : m1.m(0, 2) = -1.0 : m1.m(0, 3) = 0.0
                                m1.m(1, 0) = 1.0 : m1.m(1, 1) = 0.0 : m1.m(1, 2) = 0.0 : m1.m(1, 3) = 0.0
                                m1.m(2, 0) = 0.0 : m1.m(2, 1) = -1.0 : m1.m(2, 2) = 0.0 : m1.m(2, 3) = 0.0
                                m1.m(3, 0) = 0.0 : m1.m(3, 1) = 0.0 : m1.m(3, 2) = 0.0 : m1.m(3, 3) = 1.0
                            Case 3 ' YZ mit +X (Left) [OK]
                                m1.m(0, 0) = 0.0 : m1.m(0, 1) = 0.0 : m1.m(0, 2) = 1.0 : m1.m(0, 3) = 0.0
                                m1.m(1, 0) = 1.0 : m1.m(1, 1) = 0.0 : m1.m(1, 2) = 0.0 : m1.m(1, 3) = 0.0
                                m1.m(2, 0) = 0.0 : m1.m(2, 1) = -1.0 : m1.m(2, 2) = 0.0 : m1.m(2, 3) = 0.0
                                m1.m(3, 0) = 0.0 : m1.m(3, 1) = 0.0 : m1.m(3, 2) = 0.0 : m1.m(3, 3) = 1.0
                            Case 1 ' XZ mit -Y (Top) [OK]
                                m1.m(0, 0) = 1.0 : m1.m(0, 1) = 0.0 : m1.m(0, 2) = 0.0 : m1.m(0, 3) = 0.0
                                m1.m(1, 0) = 0.0 : m1.m(1, 1) = 1.0 : m1.m(1, 2) = 0.0 : m1.m(1, 3) = 0.0
                                m1.m(2, 0) = 0.0 : m1.m(2, 1) = 0.0 : m1.m(2, 2) = 1.0 : m1.m(2, 3) = 0.0
                                m1.m(3, 0) = 0.0 : m1.m(3, 1) = 0.0 : m1.m(3, 2) = 0.0 : m1.m(3, 3) = 1.0
                            Case 4 ' XZ mit -Y (Bottom) [OK]
                                m1.m(0, 0) = 1.0 : m1.m(0, 1) = 0.0 : m1.m(0, 2) = 0.0 : m1.m(0, 3) = 0.0
                                m1.m(1, 0) = 0.0 : m1.m(1, 1) = 1.0 : m1.m(1, 2) = 0.0 : m1.m(1, 3) = 0.0
                                m1.m(2, 0) = 0.0 : m1.m(2, 1) = 0.0 : m1.m(2, 2) = -1.0 : m1.m(2, 3) = 0.0
                                m1.m(3, 0) = 0.0 : m1.m(3, 1) = 0.0 : m1.m(3, 2) = 0.0 : m1.m(3, 3) = 1.0
                            Case 2 ' XY mit -Z (Front) [OK]
                                m1.m(0, 0) = 1.0 : m1.m(0, 1) = 0.0 : m1.m(0, 2) = 0.0 : m1.m(0, 3) = 0.0
                                m1.m(1, 0) = 0.0 : m1.m(1, 1) = 0.0 : m1.m(1, 2) = 1.0 : m1.m(1, 3) = 0.0
                                m1.m(2, 0) = 0.0 : m1.m(2, 1) = -1.0 : m1.m(2, 2) = 0.0 : m1.m(2, 3) = 0.0
                                m1.m(3, 0) = 0.0 : m1.m(3, 1) = 0.0 : m1.m(3, 2) = 0.0 : m1.m(3, 3) = 1.0
                            Case 5 ' XY mit +Z (Back) [OK]
                                m1.m(0, 0) = -1.0 : m1.m(0, 1) = 0.0 : m1.m(0, 2) = 0.0 : m1.m(0, 3) = 0.0
                                m1.m(1, 0) = 0.0 : m1.m(1, 1) = 0.0 : m1.m(1, 2) = 1.0 : m1.m(1, 3) = 0.0
                                m1.m(2, 0) = 0.0 : m1.m(2, 1) = -1.0 : m1.m(2, 2) = 0.0 : m1.m(2, 3) = 0.0
                                m1.m(3, 0) = 0.0 : m1.m(3, 1) = 0.0 : m1.m(3, 2) = 0.0 : m1.m(3, 3) = 1.0
                        End Select
                        If LPCFile.PrimitivesMetadataHMap.ContainsKey("s\" & Path.GetFileName(OpenDAT.FileName)) Then Exit Try
                        appendSubpart(Path.GetFileName(OpenDAT.FileName), "16",
                                      m1,
                                      projectMode, subfileMetadata, New Dictionary(Of String, Byte))
                        endTriangle = LPCFile.Triangles.Count
                        endVertex = LPCFile.Vertices.Count
                        If startTriangle <> endTriangle Then

                            cleanupDATVertices(startVertex, startTriangle)
                            cleanupDATTriangles(startTriangle)
                            paintTriangles(startTriangle)

                            LPCFile.Vertices.Add(New Vertex(0, 0, False))
                            LPCFile.Triangles.Add(New Triangle(ListHelper.LLast(LPCFile.Vertices), LPCFile.Vertices(startVertex), LPCFile.Vertices(startVertex)))
                            ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))

                            LPCFile.Primitives.Add(New Primitive(0, 0, 0, 0, "s\" & Path.GetFileName(OpenDAT.FileName), ListHelper.LLast(LPCFile.Vertices).vertexID))

                            Select Case projectMode
                                Case 0 ' YZ mit -X (Right) [OK]
                                    Dim cmatrix(,) As Double = {{0.0, 0.0, -1.0, 0.0},
                                                                {0.0, 1.0, 0.0, 0.0},
                                                                {1.0, 0.0, 0.0, 0.0},
                                                                {0.0, 0.0, 0.0, 1.0}}
                                    ListHelper.LLast(LPCFile.Primitives).matrixR = cmatrix.Clone
                                Case 1 ' XZ mit -Y (Top) [OK]
                                    Dim cmatrix(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                                                {0.0, 0.0, -1.0, 0.0},
                                                                {0.0, 1.0, 0.0, 0.0},
                                                                {0.0, 0.0, 0.0, 1.0}}
                                    ListHelper.LLast(LPCFile.Primitives).matrixR = cmatrix.Clone
                                Case 2 ' XY mit -Z (Front) [OK] 
                                    Dim cmatrix(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                                                {0.0, 1.0, 0.0, 0.0},
                                                                {0.0, 0.0, 1.0, 0.0},
                                                                {0.0, 0.0, 0.0, 1.0}}
                                    ListHelper.LLast(LPCFile.Primitives).matrixR = cmatrix.Clone
                                Case 3 ' YZ mit +X (Left) [OK]
                                    Dim cmatrix(,) As Double = {{0.0, 0.0, 1.0, 0.0},
                                                                {0.0, 1.0, 0.0, 0.0},
                                                                {-1.0, 0.0, 0.0, 0.0},
                                                                {0.0, 0.0, 0.0, 1.0}}
                                    ListHelper.LLast(LPCFile.Primitives).matrixR = cmatrix.Clone
                                Case 4 ' XZ mit -Y (Bottom) [OK]
                                    Dim cmatrix(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                                                {0.0, 0.0, 1.0, 0.0},
                                                                {0.0, -1.0, 0.0, 0.0},
                                                                {0.0, 0.0, 0.0, 1.0}}
                                    ListHelper.LLast(LPCFile.Primitives).matrixR = cmatrix.Clone
                                Case 5 ' XY mit +Z (Back) [OK]
                                    Dim cmatrix(,) As Double = {{-1.0, 0.0, 0.0, 0.0},
                                                                {0.0, 1.0, 0.0, 0.0},
                                                                {0.0, 0.0, -1.0, 0.0},
                                                                {0.0, 0.0, 0.0, 1.0}}
                                    ListHelper.LLast(LPCFile.Primitives).matrixR = cmatrix.Clone
                            End Select

                            LPCFile.PrimitivesHMap.Add(GlobalIdSet.primitiveIDglobal, LPCFile.Primitives.Count - 1)
                            LPCFile.PrimitivesMetadataHMap.Add(ListHelper.LLast(LPCFile.Primitives).primitiveName, subfileMetadata.Clone)
                            LPCFile.PrimitivesMetadataHMap.Item(ListHelper.LLast(LPCFile.Primitives).primitiveName).mData(1) = ""
                            endTriangle = LPCFile.Triangles.Count
                            endVertex = LPCFile.Vertices.Count
                            For i As Integer = startVertex To endVertex - 1
                                LPCFile.Vertices(i).groupindex = GlobalIdSet.primitiveIDglobal
                            Next
                            For i As Integer = startTriangle To endTriangle - 1
                                LPCFile.Triangles(i).groupindex = GlobalIdSet.primitiveIDglobal
                            Next
                        End If
                        Exit Try
                    End If
                    Dim tempMetadata As New Metadata()
                    tempMetadata.additionalData = ""
                    EnvironmentPaths.folderPath = Path.GetDirectoryName(OpenDAT.FileName)

                    ' Parse the header..
                    ' 1.) Read the description
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String = DateiIn.ReadLine().Trim()
                        If textline Like "0 *" Then
                            tempMetadata.mData(0) = Mid(textline, 3)
                        End If
                    End Using

                    ' 2.) Read the name
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Do
                            textline = DateiIn.ReadLine().Trim()
                            If textline Like "0 Name: *" Then
                                tempMetadata.mData(1) = Mid(textline, 9)
                                Exit Do
                            End If
                        Loop Until DateiIn.EndOfStream OrElse textline Like "1 *" OrElse textline Like "2 *" OrElse textline Like "3 *" _
                         OrElse textline Like "4 *" OrElse textline Like "5 *"
                    End Using

                    ' 3.) Read the author
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Do
                            textline = DateiIn.ReadLine().Trim()
                            If textline Like "0 Author: *" Then
                                If Not (textline.Contains("[") OrElse textline.Contains("]")) Then
                                    textline = textline + " []"
                                End If
                                tempMetadata.mData(2) = Mid(textline, 11, textline.LastIndexOf("[") - 11)
                                tempMetadata.mData(3) = Mid(textline, textline.LastIndexOf("[") + 2, textline.LastIndexOf("]") - textline.LastIndexOf("[") - 1)
                                Exit Do
                            End If
                        Loop Until DateiIn.EndOfStream OrElse textline Like "1 *" OrElse textline Like "2 *" OrElse textline Like "3 *" _
                         OrElse textline Like "4 *" OrElse textline Like "5 *"
                    End Using

                    ' 4.) Read the type
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Do
                            textline = DateiIn.ReadLine().Trim()
                            If textline Like "0 !LDRAW_ORG *" Then
                                tempMetadata.mData(4) = Mid(textline, 14)
                                Exit Do
                            End If
                        Loop Until DateiIn.EndOfStream OrElse textline Like "1 *" OrElse textline Like "2 *" OrElse textline Like "3 *" _
                         OrElse textline Like "4 *" OrElse textline Like "5 *"
                    End Using

                    ' 5.) Read the license
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Do
                            textline = DateiIn.ReadLine().Trim()
                            If textline Like "0 !LICENSE *" Then
                                tempMetadata.mData(5) = Mid(textline, 12)
                                Exit Do
                            End If
                        Loop Until DateiIn.EndOfStream OrElse textline Like "1 *" OrElse textline Like "2 *" OrElse textline Like "3 *" _
                         OrElse textline Like "4 *" OrElse textline Like "5 *"
                    End Using

                    ' 6.) Read !HELP, !HISTORY, 0 // & !KEYWORDS lines
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Do
                            textline = DateiIn.ReadLine().Trim()
                            If textline Like "0 !HELP *" Then
                                tempMetadata.mData(6) += Mid(textline, 9) & "<br>"
                            ElseIf textline Like "0 !KEYWORDS*" Then
                                tempMetadata.mData(9) += Mid(textline, 13) & "<br>"
                            ElseIf textline Like "0 !HISTORY*" Then
                                tempMetadata.mData(10) += Mid(textline, 12) & "<br>"
                            ElseIf textline Like "0 // *" Then
                                tempMetadata.mData(11) += Mid(textline, 6) & "<br>"
                            End If
                        Loop Until DateiIn.EndOfStream OrElse textline Like "1 *" OrElse textline Like "2 *" OrElse textline Like "3 *" _
                         OrElse textline Like "4 *" OrElse textline Like "5 *"
                    End Using
                    If Not String.IsNullOrEmpty(tempMetadata.mData(6)) Then tempMetadata.mData(6) = Mid(tempMetadata.mData(6), 1, tempMetadata.mData(6).Length - 4)
                    If Not String.IsNullOrEmpty(tempMetadata.mData(9)) Then tempMetadata.mData(9) = Mid(tempMetadata.mData(9), 1, tempMetadata.mData(9).Length - 4)
                    If Not String.IsNullOrEmpty(tempMetadata.mData(10)) Then tempMetadata.mData(10) = Mid(tempMetadata.mData(10), 1, tempMetadata.mData(10).Length - 4)
                    If Not String.IsNullOrEmpty(tempMetadata.mData(11)) Then tempMetadata.mData(11) = Mid(tempMetadata.mData(11), 1, tempMetadata.mData(11).Length - 4)

                    ' 7.) Read BFC info
                    tempMetadata.mData(7) = "BFC CERTIFY CCW"
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Do
                            textline = DateiIn.ReadLine().Trim()
                            If textline Like "0 BFC *" Then
                                tempMetadata.mData(7) = Mid(textline, 3)
                                Exit Do
                            End If
                        Loop Until DateiIn.EndOfStream OrElse textline Like "1 *" OrElse textline Like "2 *" OrElse textline Like "3 *" _
                         OrElse textline Like "4 *" OrElse textline Like "5 *"
                    End Using

                    ' 8.) Read category
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Do
                            textline = DateiIn.ReadLine().Trim()
                            If textline Like "0 !CATEGORY*" Then
                                tempMetadata.mData(8) = Mid(textline, 13)
                                Exit Do
                            End If
                        Loop Until DateiIn.EndOfStream OrElse textline Like "1 *" OrElse textline Like "2 *" OrElse textline Like "3 *" _
                         OrElse textline Like "4 *" OrElse textline Like "5 *"
                    End Using

                    ' Parse other data (triangles, quads, .. )
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenDAT.FileName, New System.Text.UTF8Encoding(False))
                        Dim textline As String
                        Dim createPrimitives As Boolean = RBnew.Checked OrElse RBtemplate.Checked
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
                                If RBtemplate.Checked AndAlso words(0) Like "*0*" Then
                                    tempMetadata.additionalData += ttextline & "<br>"
                                End If
                                If words(0) Like "*1*" AndAlso words.Length > 14 Then
                                    If RBtemplate.Checked Then
                                        tempMetadata.additionalData += ttextline & "<br>"
                                    Else
                                        Select Case projectMode
                                            ' YZ mit -X (Right) []
                                            Case 0 : inlinePrimitive(projectMode, words(1),
                                                    -CType(words(11), Double), -CType(words(13), Double), 0, CType(words(4), Double),
                                                    -CType(words(8), Double), -CType(words(10), Double), 0, CType(words(3), Double),
                                                    0, 0, 1, 0,
                                                    1, 0, 0, 0,
                                                    0, 1, 0, 0,
                                                    0, 0, -1, 0,
                                                    getDATFilename(origline), mySubfiles, False, createPrimitives)
                                            ' YZ mit +X (Left) []
                                            Case 3 : inlinePrimitive(projectMode, words(1),
                                                    CType(words(11), Double), CType(words(13), Double), 0, -CType(words(4), Double),
                                                    -CType(words(8), Double), -CType(words(10), Double), 0, CType(words(3), Double),
                                                    0, 0, 1, 0,
                                                    1, 0, 0, 0,
                                                    0, 1, 0, 0,
                                                    0, 0, -1, 0,
                                                    getDATFilename(origline), mySubfiles, False, createPrimitives)
                                            ' XZ mit -Y (Top) [OK]
                                            Case 1 : inlinePrimitive(projectMode, words(1),
                                                    -CType(words(5), Double), -CType(words(7), Double), 0, -CType(words(2), Double),
                                                    -CType(words(11), Double), -CType(words(13), Double), 0, -CType(words(4), Double),
                                                    0, 0, 1, 0,
                                                    -1, 0, 0, 0,
                                                    0, -1, 0, 0,
                                                    0, 0, -1, 0,
                                                    getDATFilename(origline), mySubfiles, False, createPrimitives)
                                            ' XZ mit -Y (Bottom) [OK]
                                            Case 4 : inlinePrimitive(projectMode, words(1),
                                                CType(words(5), Double), CType(words(7), Double), 0, -CType(words(2), Double),
                                                -CType(words(11), Double), -CType(words(13), Double), 0, CType(words(4), Double),
                                                0, 0, 1, 0,
                                                1, 0, 0, 0,
                                                0, 1, 0, 0,
                                                0, 0, -1, 0,
                                                getDATFilename(origline), mySubfiles, False, createPrimitives)
                                            ' XY mit -Z (Front) [OK]
                                            Case 2 : inlinePrimitive(projectMode, words(1),
                                               CType(words(5), Double), CType(words(7), Double), 0, -CType(words(2), Double),
                                                -CType(words(8), Double), -CType(words(10), Double), 0, CType(words(3), Double),
                                                    0, 0, 1, 0,
                                                    1, 0, 0, 0,
                                                    0, 1, 0, 0,
                                                    0, 0, -1, 0,
                                                    getDATFilename(origline), mySubfiles, False, createPrimitives)
                                            ' XY mit +Z (Back) [OK]
                                            Case 5 : inlinePrimitive(projectMode, words(1),
                                                -CType(words(5), Double), -CType(words(7), Double), 0, CType(words(2), Double),
                                                -CType(words(8), Double), -CType(words(10), Double), 0, CType(words(3), Double),
                                                    0, 0, 1, 0,
                                                    1, 0, 0, 0,
                                                    0, 1, 0, 0,
                                                    0, 0, -1, 0,
                                                    getDATFilename(origline), mySubfiles, False, createPrimitives)
                                        End Select
                                    End If
                                ElseIf words(0) Like "*3*" AndAlso words.Length = 11 Then
                                    Select Case projectMode
                                        Case 0
                                            '3 4, 6 7, 9 10
                                            LPCFile.Vertices.Add(New Vertex(CType(words(4), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(7), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(10), Double), CType(words(9), Double), False))
                                        Case 1
                                            '4 2, 7 5, 10 8
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), CType(-words(4), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), CType(-words(7), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), CType(-words(10), Double), False))
                                        Case 2
                                            '2 3, 5 6, 8 9
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), CType(words(9), Double), False))
                                        Case 3
                                            '3 4, 6 7, 9 10
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(4), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(7), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(10), Double), CType(words(9), Double), False))
                                        Case 4
                                            '4 2, 7 5, 10 8
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), CType(words(4), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), CType(words(7), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), CType(words(10), Double), False))
                                        Case 5
                                            '2 3, 5 6, 8 9
                                            LPCFile.Vertices.Add(New Vertex(CType(words(2), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(5), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(8), Double), CType(words(9), Double), False))
                                    End Select
                                    If Not RBtemplate.Checked Then
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
                                    Else
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 3).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 3).Y * 1000, Single)))
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 3).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 3).Y * 1000, Single)))
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 2).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 2).Y * 1000, Single)))
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 1).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 1).Y * 1000, Single)))
                                        LPCFile.templateProjectionQuads.Add(New ProjectionQuad(LPCFile.Vertices(LPCFile.Vertices.Count - 3).X, LPCFile.Vertices(LPCFile.Vertices.Count - 3).Y,
                                                                                      LPCFile.Vertices(LPCFile.Vertices.Count - 3).X, LPCFile.Vertices(LPCFile.Vertices.Count - 3).Y,
                                                                                      LPCFile.Vertices(LPCFile.Vertices.Count - 2).X, LPCFile.Vertices(LPCFile.Vertices.Count - 2).Y,
                                                                                      LPCFile.Vertices(LPCFile.Vertices.Count - 1).X, LPCFile.Vertices(LPCFile.Vertices.Count - 1).Y,
                                                                                      CType(words(2), Double), CType(words(3), Double), CType(words(4), Double),
                                                                                      CType(words(2), Double), CType(words(3), Double), CType(words(4), Double),
                                                                                      CType(words(5), Double), CType(words(6), Double), CType(words(7), Double),
                                                                                      CType(words(8), Double), CType(words(9), Double), CType(words(10), Double)))
                                    End If
                                ElseIf words(0) Like "*4*" AndAlso words.Length = 14 Then
                                    Select Case projectMode
                                        Case 0
                                            '3 4, 6 7, 9 10, 12 13
                                            LPCFile.Vertices.Add(New Vertex(CType(words(4), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(7), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(10), Double), CType(words(9), Double), False))
                                            If Not RBtemplate.Checked Then
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
                                            End If
                                            LPCFile.Vertices.Add(New Vertex(CType(words(13), Double), CType(words(12), Double), False))
                                        Case 1
                                            '4 2, 7 5, 10 8, 13 11
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), -CType(words(4), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), -CType(words(7), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), -CType(words(10), Double), False))
                                            If Not RBtemplate.Checked Then
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
                                            End If
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(11), Double), -CType(words(13), Double), False))
                                        Case 2
                                            '2 3, 5 6, 8 9, 11 12
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), CType(words(9), Double), False))
                                            If Not RBtemplate.Checked Then
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
                                            End If
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(11), Double), CType(words(12), Double), False))
                                        Case 3
                                            '3 4, 6 7, 9 10, 12 13
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(4), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(7), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(10), Double), CType(words(9), Double), False))
                                            If Not RBtemplate.Checked Then
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
                                            End If
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(13), Double), CType(words(12), Double), False))
                                        Case 4
                                            '4 2, 7 5, 10 8, 13 11
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(2), Double), CType(words(4), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(5), Double), CType(words(7), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(8), Double), CType(words(10), Double), False))
                                            If Not RBtemplate.Checked Then
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
                                            End If
                                            LPCFile.Vertices.Add(New Vertex(-CType(words(11), Double), CType(words(13), Double), False))
                                        Case 5
                                            '2 3, 5 6, 8 9, 11 12
                                            LPCFile.Vertices.Add(New Vertex(CType(words(2), Double), CType(words(3), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(5), Double), CType(words(6), Double), False))
                                            LPCFile.Vertices.Add(New Vertex(CType(words(8), Double), CType(words(9), Double), False))
                                            If Not RBtemplate.Checked Then
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
                                            End If
                                            LPCFile.Vertices.Add(New Vertex(CType(words(11), Double), CType(words(12), Double), False))
                                    End Select
                                    If Not RBtemplate.Checked Then
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
                                    Else
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 4).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 4).Y * 1000, Single)))
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 3).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 3).Y * 1000, Single)))
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 2).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 2).Y * 1000, Single)))
                                        LPCFile.templateShape.Add(New PointF(CType(LPCFile.Vertices(LPCFile.Vertices.Count - 1).X * 1000, Single), CType(LPCFile.Vertices(LPCFile.Vertices.Count - 1).Y * 1000, Single)))
                                        LPCFile.templateProjectionQuads.Add(New ProjectionQuad(LPCFile.Vertices(LPCFile.Vertices.Count - 4).X, LPCFile.Vertices(LPCFile.Vertices.Count - 4).Y,
                                                                                      LPCFile.Vertices(LPCFile.Vertices.Count - 3).X, LPCFile.Vertices(LPCFile.Vertices.Count - 3).Y,
                                                                                      LPCFile.Vertices(LPCFile.Vertices.Count - 2).X, LPCFile.Vertices(LPCFile.Vertices.Count - 2).Y,
                                                                                      LPCFile.Vertices(LPCFile.Vertices.Count - 1).X, LPCFile.Vertices(LPCFile.Vertices.Count - 1).Y,
                                                                                      CType(words(2), Double), CType(words(3), Double), CType(words(4), Double),
                                                                                      CType(words(5), Double), CType(words(6), Double), CType(words(7), Double),
                                                                                      CType(words(8), Double), CType(words(9), Double), CType(words(10), Double),
                                                                                      CType(words(11), Double), CType(words(12), Double), CType(words(13), Double)))
                                    End If
                                ElseIf words(0) Like "*2*" Then
                                    If RBtemplate.Checked Then tempMetadata.additionalData += ttextline & "<br>"
                                ElseIf words(0) Like "*5*" Then
                                    If RBtemplate.Checked Then tempMetadata.additionalData += ttextline & "<br>"
                                End If
                            End If
                        Loop Until DateiIn.EndOfStream
                    End Using
                    scaleVertices(False)
                    endTriangle = LPCFile.Triangles.Count
                    endVertex = LPCFile.Vertices.Count
                    If (endVertex <> startVertex AndAlso endTriangle <> startTriangle) OrElse RBtemplate.Checked Then
                        cleanupDATVertices()
                        cleanupDATTriangles()
                        paintTriangles()
                        endTriangle = LPCFile.Triangles.Count
                        endVertex = LPCFile.Vertices.Count
                        If RBnew.Checked Then
                            LPCFile.myMetadata = tempMetadata.Clone
                            LPCFile.myMetadata.isMainMetadata = True
                            For r As Integer = 0 To 3
                                For c As Integer = 0 To 3
                                    LPCFile.myMetadata.matrix(r, c) = 0
                                    If c = r Then LPCFile.myMetadata.matrix(r, c) = 1
                                Next
                            Next
                        End If
                    End If
                    If RBtemplate.Checked Then
                        LPCFile.myMetadata.recommendedMode = projectMode
                        endVertex = LPCFile.Vertices.Count
                        For Each v As Vertex In LPCFile.Vertices
                            If v.groupindex = Primitive.TEMPLATE_INDEX Then
                                v.groupindex = Primitive.NO_INDEX
                            End If
                        Next
                        For i As Integer = startVertex To endVertex - 1
                            LPCFile.Vertices(i).groupindex = Primitive.TEMPLATE_INDEX
                        Next
                    End If
                    If LPCFile.Vertices.Count = 0 Then
                        If tempMetadata.mData(1) = "" Then
                            Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.NoInfoInDAT1), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Info))
                        Else
                            Dim result As MsgBoxResult = MsgBox(String.Format(I18N.trl8(I18N.lk.NoInfoInDAT2), tempMetadata.mData(1)), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Info))
                        End If
                    End If
                    ' Cleanup malformed primitives                    
                    Dim VIDtoVI As New Hashtable(LPCFile.Vertices.Count)
                    For Each v As Vertex In LPCFile.Vertices
                        VIDtoVI.Add(v.vertexID, Nothing)
                    Next
                    Dim noCenterVertex As Boolean
                    For Each p As Primitive In LPCFile.Primitives
                        If Not VIDtoVI.ContainsKey(p.centerVertexID) Then
                            noCenterVertex = True
                            Exit For
                        End If
                    Next
                    If noCenterVertex Then
                        Dim tc As Integer = LPCFile.Triangles.Count - 1
                        Dim start As Integer = 0
newDelete:
                        For i As Integer = start To tc
                            Dim tri As Triangle = LPCFile.Triangles(i)
                            tri.groupindex = -1
                            If tri.vertexA.vertexID = tri.vertexB.vertexID OrElse
                            tri.vertexB.vertexID = tri.vertexC.vertexID OrElse
                            tri.vertexC.vertexID = tri.vertexA.vertexID Then
                                LPCFile.Triangles.RemoveAt(i)
                                start = i
                                tc -= 1
                                GoTo newDelete
                            End If
                        Next
                        For Each vert As Vertex In LPCFile.Vertices
                            vert.groupindex = Primitive.NO_INDEX
                        Next
                        LPCFile.Primitives.Clear()
                        LPCFile.PrimitivesMetadataHMap.Clear()
                        LPCFile.PrimitivesHMap.Clear()
                    End If
                Catch ex As Exception
                    If MsgBox(I18N.trl8(I18N.lk.InvalidDAT), MsgBoxStyle.RetryCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Exclamation, I18N.trl8(I18N.lk.Fatal)) = MsgBoxResult.Retry Then
                        GoTo newTry
                    End If
                    View.backgroundPicture = My.Resources.temp
                    View.imgPath = ""
                    ImageForm.TBImage.Text = View.imgPath
                    newPattern()
                End Try
            End If
        End If
        iDialog.Dispose()
        MainState.unsavedChanges = False
        MainState.isLoading = False
        MenuStrip1.Enabled = True
        Me.MainToolStrip.Enabled = True
        Me.ColourToolStrip.Enabled = True
        ImageForm.Enabled = True
        PreferencesForm.Enabled = True
        Me.Refresh()
    End Sub

    Private Sub appendSubpart(ByVal filename As String,
                              ByVal colour As String,
                              ByVal m1 As Matrix3D,
                              ByVal projMode As Byte, ByRef mdata As Metadata, ByVal fileList As Dictionary(Of String, Byte))

        Dim filepath As String
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.folderPath & "\" & filename) Then
            filepath = EnvironmentPaths.folderPath & "\" & filename
        ElseIf EnvironmentPaths.folderPath Like "*\s" AndAlso My.Computer.FileSystem.FileExists(Mid(EnvironmentPaths.folderPath, 1, EnvironmentPaths.folderPath.Length - 2) & "\" & filename) Then
            filepath = Mid(EnvironmentPaths.folderPath, 1, EnvironmentPaths.folderPath.Length - 2) & "\" & filename
        ElseIf My.Computer.FileSystem.FileExists(EnvironmentPaths.ldrawPath & "\" & filename) Then
            filepath = EnvironmentPaths.ldrawPath & "\" & filename
        ElseIf My.Computer.FileSystem.FileExists(EnvironmentPaths.ldrawPath & "\P\" & filename) Then
            filepath = EnvironmentPaths.ldrawPath & "\P\" & filename
        Else
            Exit Sub
        End If

        If fileList.ContainsKey(filepath) Then
            Exit Sub
        End If

        fileList.Add(filepath, 0)

        Dim tempMetadata As New Metadata()
        tempMetadata.additionalData = ""
        EnvironmentPaths.folderPath = Path.GetDirectoryName(OpenDAT.FileName)
        Dim triangles3D As New List(Of Triangle3D)
        Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(filepath, New System.Text.UTF8Encoding(False))
            Dim textline As String
            Dim headerState As Integer = 0
            Do
                textline = DateiIn.ReadLine().Trim()
                Dim origline As String = textline
                If textline <> Nothing Then
                    ' Header parsen
                    If headerState <> -1 Then

                        Try
nextTry:
                            If textline Like "1 *" Then headerState = -1
                            If textline Like "2 *" Then headerState = -1
                            If textline Like "3 *" Then headerState = -1
                            If textline Like "4 *" Then headerState = -1
                            If textline Like "5 *" Then headerState = -1
                            If headerState = 0 AndAlso textline Like "0 *" Then
                                tempMetadata.mData(0) = Mid(textline, 3)
                                headerState = 1
                            ElseIf headerState = 1 AndAlso textline Like "0 Name: *" Then
                                tempMetadata.mData(1) = Mid(textline, 9)
                                headerState = 2
                            ElseIf headerState = 2 AndAlso textline Like "0 Author: *" Then
                                tempMetadata.mData(2) = Mid(textline, 11, textline.LastIndexOf("[") - 11)
                                tempMetadata.mData(3) = Mid(textline, textline.LastIndexOf("[") + 2, textline.LastIndexOf("]") - textline.LastIndexOf("[") - 1)
                                headerState = 3
                            ElseIf headerState = 3 AndAlso textline Like "0 !LDRAW_ORG *" Then
                                tempMetadata.mData(4) = Mid(textline, 14)
                                headerState = 4
                            ElseIf headerState = 4 AndAlso textline Like "0 !LICENSE *" Then
                                tempMetadata.mData(5) = Mid(textline, 12)
                                headerState = 5
                            ElseIf headerState = 5 Then
                                If textline Like "0 !HELP *" Then
                                    tempMetadata.mData(6) += Mid(textline, 9) & "<br>"
                                Else
                                    headerState = 6 : GoTo nextTry
                                End If
                            ElseIf headerState = 6 AndAlso textline Like "0 BFC *" Then
                                tempMetadata.mData(7) = Mid(textline, 3)
                                headerState = 7
                            ElseIf headerState = 7 Then
                                If textline Like "0 !CATEGORY*" Then
                                    tempMetadata.mData(8) = Mid(textline, 11)
                                Else
                                    headerState = 8 : GoTo nextTry
                                End If
                            ElseIf headerState = 8 Then
                                If textline Like "0 !KEYWORDS*" Then
                                    tempMetadata.mData(9) += Mid(textline, 12) & "<br>"
                                Else
                                    headerState = 9 : GoTo nextTry
                                End If
                            ElseIf headerState = 9 Then
                                If textline Like "0 !HISTORY*" Then
                                    tempMetadata.mData(10) += Mid(textline, 12) & "<br>"
                                ElseIf Not textline Like "0 !CMDLINE*" Then
                                    headerState = 10 : GoTo nextTry
                                End If
                            ElseIf headerState = 10 Then
                                If textline Like "0 //*" Then
                                    tempMetadata.mData(11) += Mid(textline, 5) & "<br>"
                                Else
                                    If tempMetadata.mData(6).Length > 3 Then tempMetadata.mData(6) = Mid(tempMetadata.mData(6), 1, tempMetadata.mData(6).Length - 4)
                                    If tempMetadata.mData(9).Length > 3 Then tempMetadata.mData(9) = Mid(tempMetadata.mData(9), 1, tempMetadata.mData(9).Length - 4)
                                    If tempMetadata.mData(10).Length > 3 Then tempMetadata.mData(10) = Mid(tempMetadata.mData(10), 1, tempMetadata.mData(10).Length - 4)
                                    If tempMetadata.mData(11).Length > 3 Then tempMetadata.mData(11) = Mid(tempMetadata.mData(11), 1, tempMetadata.mData(11).Length - 4)
                                    headerState = -1
                                End If
                            ElseIf textline Like "*!CMDLINE*" Then
                            Else
                                headerState = -1
                            End If
                            If headerState <> -1 Then Continue Do
                        Catch
                            headerState += 1
                        End Try
                    End If
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
                        Dim tm As New Matrix3D
                        tm.m(0, 0) = CType(words(5), Double) : tm.m(0, 1) = CType(words(6), Double) : tm.m(0, 2) = CType(words(7), Double) : tm.m(0, 3) = CType(words(2), Double)
                        tm.m(1, 0) = CType(words(8), Double) : tm.m(1, 1) = CType(words(9), Double) : tm.m(1, 2) = CType(words(10), Double) : tm.m(1, 3) = CType(words(3), Double)
                        tm.m(2, 0) = CType(words(11), Double) : tm.m(2, 1) = CType(words(12), Double) : tm.m(2, 2) = CType(words(13), Double) : tm.m(2, 3) = CType(words(4), Double)
                        tm.m(3, 0) = 0.0 : tm.m(3, 1) = 0.0 : tm.m(3, 2) = 0.0 : tm.m(3, 3) = 1.0
                        tm = tm * m1
                        Dim newFileList As New Dictionary(Of String, Byte)
                        For Each s As String In fileList.Keys
                            newFileList.Add(s, 0)
                        Next
                        appendSubpart(getDATFilename(origline), words(1), tm, projMode, Nothing, newFileList)
                    ElseIf words(0) Like "*3*" AndAlso words.Length = 11 Then
                        If words(1) = "16" Then words(1) = colour
                        triangles3D.Add(New Triangle3D(New Vertex3D(CType(words(2), Double), CType(words(3), Double), CType(words(4), Double)),
                                                       New Vertex3D(CType(words(5), Double), CType(words(6), Double), CType(words(7), Double)),
                                                       New Vertex3D(CType(words(8), Double), CType(words(9), Double), CType(words(10), Double))))
                        If words(1) Like "0x2*" Then
                            ListHelper.LLast(triangles3D).myColour = Color.FromArgb(255,
                                                                       Convert.ToInt32("0x" & Mid(words(1), 4, 2), 16),
                                                                       Convert.ToInt32("0x" & Mid(words(1), 6, 2), 16),
                                                                       Convert.ToInt32("0x" & Mid(words(1), 8, 2), 16))
                            ListHelper.LLast(triangles3D).myColourNumber = -1
                        ElseIf words(1) Like "0x*" Then
                            ListHelper.LLast(triangles3D).myColourNumber = 16
                        Else
                            ListHelper.LLast(triangles3D).myColourNumber = CType(words(1), Short)
                        End If
                    ElseIf words(0) Like "*4*" AndAlso words.Length = 14 Then
                        If words(1) = "16" Then words(1) = colour
                        triangles3D.Add(New Triangle3D(New Vertex3D(CType(words(2), Double), CType(words(3), Double), CType(words(4), Double)),
                                                       New Vertex3D(CType(words(5), Double), CType(words(6), Double), CType(words(7), Double)),
                                                       New Vertex3D(CType(words(8), Double), CType(words(9), Double), CType(words(10), Double))))
                        If words(1) Like "0x2*" Then
                            ListHelper.LLast(triangles3D).myColour = Color.FromArgb(255,
                                                                       Convert.ToInt32("0x" & Mid(words(1), 4, 2), 16),
                                                                       Convert.ToInt32("0x" & Mid(words(1), 6, 2), 16),
                                                                       Convert.ToInt32("0x" & Mid(words(1), 8, 2), 16))
                            ListHelper.LLast(triangles3D).myColourNumber = -1
                        ElseIf words(1) Like "0x*" Then
                            ListHelper.LLast(triangles3D).myColourNumber = 16
                        Else
                            ListHelper.LLast(triangles3D).myColourNumber = CType(words(1), Short)
                        End If

                        triangles3D.Add(New Triangle3D(New Vertex3D(CType(words(8), Double), CType(words(9), Double), CType(words(10), Double)),
                                                       New Vertex3D(CType(words(11), Double), CType(words(12), Double), CType(words(13), Double)),
                                                       New Vertex3D(CType(words(2), Double), CType(words(3), Double), CType(words(4), Double))))
                        If words(1) Like "0x2*" Then
                            ListHelper.LLast(triangles3D).myColour = Color.FromArgb(255,
                                                                       Convert.ToInt32("0x" & Mid(words(1), 4, 2), 16),
                                                                       Convert.ToInt32("0x" & Mid(words(1), 6, 2), 16),
                                                                       Convert.ToInt32("0x" & Mid(words(1), 8, 2), 16))
                            ListHelper.LLast(triangles3D).myColourNumber = -1
                        ElseIf words(1) Like "0x*" Then
                            ListHelper.LLast(triangles3D).myColourNumber = 16
                        Else
                            ListHelper.LLast(triangles3D).myColourNumber = CType(words(1), Short)
                        End If
                    End If
                End If
            Loop Until DateiIn.EndOfStream
            For Each tri As Triangle3D In triangles3D
                For i As Integer = 0 To 2
                    tri.vertices(i) = m1 * tri.vertices(i)
                    Dim v As Vertex3D = tri.vertices(i)
                    LPCFile.Vertices.Add(New Vertex(v.X * -1000.0, v.Z * -1000.0, False))
                Next
                Dim vc As Integer = LPCFile.Vertices.Count - 1
                LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(vc), LPCFile.Vertices(vc - 1), LPCFile.Vertices(vc - 2)))
                ListHelper.LLast(LPCFile.Triangles).myColour = tri.myColour
                ListHelper.LLast(LPCFile.Triangles).myColourNumber = tri.myColourNumber
                LPCFile.Vertices(vc).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                LPCFile.Vertices(vc - 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                LPCFile.Vertices(vc - 2).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            Next
        End Using
        If Not mdata Is Nothing Then mdata = tempMetadata.Clone
    End Sub

    Private Sub inlinePrimitive(ByVal projectMode As Byte, ByVal colour As String,
                                ByVal M11 As Double, ByVal M12 As Double, ByVal M13 As Double, ByVal M14 As Double,
                                ByVal M21 As Double, ByVal M22 As Double, ByVal M23 As Double, ByVal M24 As Double,
                                ByVal M31 As Double, ByVal M32 As Double, ByVal M33 As Double, ByVal M34 As Double, _
 _
                                ByVal N11 As Double, ByVal N12 As Double, ByVal N13 As Double, ByVal N14 As Double,
                                ByVal N21 As Double, ByVal N22 As Double, ByVal N23 As Double, ByVal N24 As Double,
                                ByVal N31 As Double, ByVal N32 As Double, ByVal N33 As Double, ByVal N34 As Double,
                                ByVal name As String, ByRef subFileList As List(Of String), Optional ByVal scale As Boolean = True, Optional ByVal createPrimitives As Boolean = True)

        Dim matrix(3, 3) As Double

        Dim matrix2(3, 3) As Double
        Dim matrix3(3, 3) As Double

        Dim subfiles As New List(Of String)

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

        Dim start As Integer = LPCFile.Vertices.Count
        Dim start2 As Integer = LPCFile.Triangles.Count

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

        If name Like "ring#.dat" OrElse name Like "ring##.dat" Then name = "4-4" & name
        If Not createPrimitives OrElse (name Like "48\*" AndAlso (name Like "48\#-#ring#.dat" OrElse name Like "48\#-#rin##.dat" OrElse name Like "48\#-##rin#.dat" OrElse name Like "48\#-##ri##.dat" OrElse name Like "48\##-##ri#.dat")) OrElse
            (name Like "48\*aring.dat") OrElse
            (name Like "48\*chrd.dat") OrElse
            (name Like "*chrd.dat") OrElse
            (name Like "48\*ndis.dat") OrElse
            (name Like "*ndis.dat") OrElse
            (name Like "48\*tang.dat") OrElse
            (name Like "*tang.dat") OrElse
            (name Like "#-#ring#.dat" OrElse name Like "#-#rin##.dat" OrElse name Like "#-##rin#.dat" OrElse name Like "#-##ri##.dat" OrElse name Like "##-##ri#.dat") OrElse
            (name Like "48\*disc.dat") OrElse
            (name Like "*disc.dat") Then
            If My.Computer.FileSystem.FileExists(filepath) Then
newTry:
                Dim startVertex As Integer = LPCFile.Vertices.Count
                Dim endVertex As Integer = 0
                Dim startTriangle As Integer = LPCFile.Triangles.Count
                Dim endTriangle As Integer = 0
                BtnMove.PerformClick()
                MainState.isLoading = True
                subfiles.Add(name)
                Try
                    Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(filepath, New System.Text.UTF8Encoding(False))
                        Dim textline As String

                        If createPrimitives Then LPCFile.Vertices.Add(New Vertex(0, 0, False))

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
                                    If Not createPrimitives Then words(1) = colour

                                    inlineSubfile(projectMode, words(1),
                                                    -CType(words(5), Double), -CType(words(7), Double), 0, -CType(words(2), Double),
                                                    -CType(words(11), Double), -CType(words(13), Double), 0, -CType(words(4), Double),
                                                    0, 0, 1, 0,
                                                    1, 0, 0, 0,
                                                    0, 1, 0, 0,
                                                    0, 0, 1, 0,
                                                    getDATFilename(origline), subfiles)

                                ElseIf words(0) Like "*3*" AndAlso words.Length = 11 Then
                                    If Not createPrimitives Then words(1) = colour
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
                                    If Not createPrimitives Then words(1) = colour
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
                    endTriangle = LPCFile.Triangles.Count
                    endVertex = LPCFile.Vertices.Count
                    If (endVertex <> startVertex AndAlso endTriangle <> startTriangle) Then

                        If Not createPrimitives Then
                            cleanupDATVertices(startVertex, startTriangle)
                            cleanupDATTriangles(startTriangle)
                        End If

                        paintTriangles(startTriangle)
                        endTriangle = LPCFile.Triangles.Count
                        endVertex = LPCFile.Vertices.Count

                        If createPrimitives Then
                            LPCFile.Triangles.Add(New Triangle(LPCFile.Vertices(startVertex), LPCFile.Vertices(startVertex), LPCFile.Vertices(startVertex + 1)))
                            LPCFile.Vertices(startVertex).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
                            LPCFile.Vertices(startVertex + 1).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))


                            LPCFile.Primitives.Add(New Primitive(MainState.primitiveCenter.X, MainState.primitiveCenter.Y, 0, 0, name, LPCFile.Vertices(startVertex).vertexID))
                            For i As Integer = startVertex To endVertex - 1
                                LPCFile.Vertices(i).groupindex = GlobalIdSet.primitiveIDglobal
                            Next
                            For i As Integer = startTriangle To endTriangle
                                LPCFile.Triangles(i).groupindex = GlobalIdSet.primitiveIDglobal
                            Next
                        End If
                    Else
                        MainState.isLoading = False
                        Me.Refresh()
                        Exit Sub
                    End If
                Catch ex As Exception
                    MainState.isLoading = False
                    Me.Refresh()
                    Exit Sub
                End Try
                MainState.isLoading = False
                Me.Refresh()
            Else
                MainState.isLoading = False
                Me.Refresh()
                Exit Sub
            End If
            GoTo doMatrix
        Else
            MainState.isLoading = False
            Me.Refresh()
            Exit Sub
        End If
        'If name Like "*s\*.dat" Then
        '    subFileList.Add(name)
        'End If
        'GoTo doNothing
doMatrix:
        If createPrimitives Then
            LPCFile.PrimitivesHMap.Add(GlobalIdSet.primitiveIDglobal, LPCFile.Primitives.Count - 1)
            Dim lastPrimitive As Primitive = ListHelper.LLast(LPCFile.Primitives)
            If scale Then

                lastPrimitive.myColourNumber = CType(MainState.lastColourNumber, Short)
                lastPrimitive.myColour = MainState.lastColour
            Else
                If colour Like "0x*" Then
                    lastPrimitive.myColour = Color.FromArgb(Convert.ToInt32(colour, 16))
                    lastPrimitive.myColourNumber = -1
                Else
                    lastPrimitive.myColourNumber = CType(colour, Short)
                    If LDConfig.colourHMap.ContainsKey(lastPrimitive.myColourNumber) Then lastPrimitive.myColour = LDConfig.colourHMap(lastPrimitive.myColourNumber) Else lastPrimitive.myColour = Nothing
                End If
            End If
            lastPrimitive.matrix = matrix3.Clone
            lastPrimitive.matrix(0, 3) = lastPrimitive.matrix(0, 3) * 1000.0 + MainState.primitiveCenter.X
            lastPrimitive.matrix(1, 3) = lastPrimitive.matrix(1, 3) * 1000.0 + MainState.primitiveCenter.Y
        End If
        Dim tx, ty As Double
        If scale Then
            For i = start To LPCFile.Vertices.Count - 1
                tx = matrix3(0, 0) * LPCFile.Vertices(i).X + matrix3(0, 1) * LPCFile.Vertices(i).Y + matrix3(0, 3) * 1000.0
                ty = matrix3(1, 0) * LPCFile.Vertices(i).X + matrix3(1, 1) * LPCFile.Vertices(i).Y + matrix3(1, 3) * 1000.0
                LPCFile.Vertices(i).X = tx * 1000.0 + MainState.primitiveCenter.X
                LPCFile.Vertices(i).Y = ty * 1000.0 + MainState.primitiveCenter.Y
            Next
        Else
            For i = start To LPCFile.Vertices.Count - 1
                tx = matrix3(0, 0) * LPCFile.Vertices(i).X + matrix3(0, 1) * LPCFile.Vertices(i).Y + matrix3(0, 3)
                ty = matrix3(1, 0) * LPCFile.Vertices(i).X + matrix3(1, 1) * LPCFile.Vertices(i).Y + matrix3(1, 3)
                LPCFile.Vertices(i).X = tx
                LPCFile.Vertices(i).Y = ty
            Next
        End If
    End Sub

    Private Sub ResetViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetViewToolStripMenuItem.Click
        View.oldOffsetX = 0 : View.oldOffsetY = 0 : View.offsetX = 0 : View.offsetY = 0
        View.zoomfactor = 0.05
        View.zoomlevel = -15
        LblZoom.Text = I18N.trl8(I18N.lk.ZoomParam) & " 5%"
        Me.Refresh()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox(I18N.parseVersion(I18N.trl8(I18N.lk.AboutInfo)), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, I18N.trl8(I18N.lk.AboutTitle))
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' TODO Set to true only for integrity testing!!
        If False Then
            ' Permanent integrity test..
            If Timer1.Interval <> 5 Then
                SBZoom.Focus()
                Timer1.Interval = 5
            Else
                Dim VIDtoVI As New Dictionary(Of Integer, Integer)(LPCFile.Vertices.Count)
                Dim TIDtoTI As New Dictionary(Of Integer, Integer)(LPCFile.Triangles.Count)
                Dim i As Integer = 0
                For Each t As Triangle In LPCFile.Triangles
                    TIDtoTI.Add(t.triangleID, i)
                    If t.triangleID > GlobalIdSet.triangleIDglobal Then
                        Timer1.Enabled = False
                        If MsgBox("LPCValidator: Dreieck-ID unzulässig!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                        End If
                    End If
                    i += 1
                Next
                i = 0
                For Each v As Vertex In LPCFile.Vertices
                    VIDtoVI.Add(v.vertexID, i)
                    If v.vertexID > GlobalIdSet.vertexIDglobal Then
                        Timer1.Enabled = False
                        If MsgBox("LPCValidator: Vertex-ID unzulässig!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                        End If
                    End If
                    For Each t As Triangle In v.linkedTriangles
                        If Not t.vertexA.Equals(v) AndAlso Not t.vertexB.Equals(v) AndAlso Not t.vertexC.Equals(v) Then
                            If TIDtoTI.ContainsKey(t.triangleID) Then
                                Timer1.Enabled = False
                                If MsgBox("LPCValidator: Vertex verweist auf ungebundenes Dreieck!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                                End If
                            Else
                                Timer1.Enabled = False
                                If MsgBox("LPCValidator: Vertex verweist auf ungebundenes Dreieck, das nicht mehr existiert!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                                End If
                            End If
                        End If
                    Next
                    i += 1
                Next
                For Each t As Triangle In LPCFile.Triangles
                    If Not VIDtoVI.ContainsKey(t.vertexA.vertexID) OrElse Not VIDtoVI.ContainsKey(t.vertexB.vertexID) OrElse Not VIDtoVI.ContainsKey(t.vertexC.vertexID) Then
                        Timer1.Enabled = False
                        If MsgBox("LPCValidator: Dreieck verweist auf ungebundenen Vertex, der nicht mehr existiert!!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                        End If
                    End If
                    If t.groupindex > Primitive.NO_INDEX AndAlso (t.vertexA.groupindex <> t.groupindex OrElse t.vertexB.groupindex <> t.groupindex OrElse t.vertexC.groupindex <> t.groupindex) Then
                        Timer1.Enabled = False
                        If MsgBox("LPCValidator: Dreieck darf nicht Teil einer Gruppe sein, weil nicht alle seine Vertices Teil der selben Gruppe sind!!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                        End If
                    End If
                    i += 1
                Next
                i = 0
                Dim PIDtoPI As New Dictionary(Of Integer, Integer)(LPCFile.Vertices.Count)
                Dim meta As New Hashtable(LPCFile.PrimitivesMetadataHMap.Count)
                For Each m As String In LPCFile.PrimitivesMetadataHMap.Keys
                    meta.Add(m, Nothing)
                Next

                For Each p As Primitive In LPCFile.Primitives
                    If PIDtoPI.ContainsKey(p.primitiveID) Then
                        Timer1.Enabled = False
                        If MsgBox("LPCValidator: Primitiv besitzt keine eindeutige ID!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                        End If
                    Else
                        PIDtoPI.Add(p.primitiveID, i)
                    End If
                    If Not VIDtoVI.ContainsKey(p.centerVertexID) Then
                        Timer1.Enabled = False
                        If MsgBox("LPCValidator: Primitiv ohne Zentrums-Vertex!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                        End If
                    Else
                        Dim vert As Vertex = LPCFile.Vertices(VIDtoVI(p.centerVertexID))
                        If Fix(vert.X) <> Fix(p.matrix(0, 3)) Or Fix(vert.Y) <> Fix(p.matrix(1, 3)) Then
                            Timer1.Enabled = False
                            If MsgBox("LPCValidator: Primitiv besitzt abweichendes Zentrum!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                            End If
                        End If
                    End If
                    If p.primitiveName Like "subfile*" AndAlso Not LPCFile.PrimitivesMetadataHMap.ContainsKey(p.primitiveName) Then
                        Timer1.Enabled = False
                        If MsgBox("LPCValidator: Subfile ohne Metadaten!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                        End If
                    End If
                    If meta.ContainsKey(p.primitiveName) Then meta.Remove(p.primitiveName)
                    i += 1
                Next
                If meta.Count > 0 Then
                    Timer1.Enabled = False
                    If MsgBox("LPCValidator: Metadaten ohne Subfile!", MsgBoxStyle.Critical) = MsgBoxResult.Ok Then
                    End If
                End If
            End If
        Else
            ' Standard behaviour..
            DebugToolStripButton.Visible = False
            SBZoom.Focus()
            MainState.unsavedChanges = False
            Timer1.Enabled = False
            Timer1.Dispose()
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Me.Close()
    End Sub

    Private Sub selectNearestTriangleEdgeForNewObjects()
        If Not MainState.isLoading AndAlso (BtnAddVertex.Checked OrElse BtnAddTriangle.Checked) Then
            If Not MainState.intelligentFocusTriangle Is Nothing AndAlso Not Control.ModifierKeys = Keys.Control AndAlso Not MainState.doSelection Then
                MainState.trianglemode = 2
                Dim cx As Double = getXcoordinate(MouseHelper.getCursorpositionX)
                Dim cy As Double = getYcoordinate(MouseHelper.getCursorpositionY)
                Dim cVert As New Vertex(cx, cy, False, False)

                Dim distA As Double = cVert.dist(MainState.intelligentFocusTriangle.vertexA)
                Dim distB As Double = cVert.dist(MainState.intelligentFocusTriangle.vertexB)
                Dim distC As Double = cVert.dist(MainState.intelligentFocusTriangle.vertexC)

                If distA <= distB AndAlso distB <= distC Then
                    MainState.temp_vertices(0) = MainState.intelligentFocusTriangle.vertexA
                    MainState.temp_vertices(1) = MainState.intelligentFocusTriangle.vertexB
                    MainState.temp_corner_vertex = MainState.intelligentFocusTriangle.vertexC
                ElseIf distA <= distC AndAlso distC <= distB Then
                    MainState.temp_vertices(0) = MainState.intelligentFocusTriangle.vertexA
                    MainState.temp_vertices(1) = MainState.intelligentFocusTriangle.vertexC
                    MainState.temp_corner_vertex = MainState.intelligentFocusTriangle.vertexB
                ElseIf distB <= distA AndAlso distA <= distC Then
                    MainState.temp_vertices(0) = MainState.intelligentFocusTriangle.vertexB
                    MainState.temp_vertices(1) = MainState.intelligentFocusTriangle.vertexA
                    MainState.temp_corner_vertex = MainState.intelligentFocusTriangle.vertexC
                ElseIf distB <= distC AndAlso distC <= distA Then
                    MainState.temp_vertices(0) = MainState.intelligentFocusTriangle.vertexB
                    MainState.temp_vertices(1) = MainState.intelligentFocusTriangle.vertexC
                    MainState.temp_corner_vertex = MainState.intelligentFocusTriangle.vertexA
                ElseIf distC <= distA AndAlso distA <= distB Then
                    MainState.temp_vertices(0) = MainState.intelligentFocusTriangle.vertexC
                    MainState.temp_vertices(1) = MainState.intelligentFocusTriangle.vertexA
                    MainState.temp_corner_vertex = MainState.intelligentFocusTriangle.vertexB
                ElseIf distC <= distB AndAlso distB <= distA Then
                    MainState.temp_vertices(0) = MainState.intelligentFocusTriangle.vertexC
                    MainState.temp_vertices(1) = MainState.intelligentFocusTriangle.vertexB
                    MainState.temp_corner_vertex = MainState.intelligentFocusTriangle.vertexA
                Else
                    Exit Sub
                End If

                checkIntersection(cVert)

                Dim newTri As Triangle = New Triangle(MainState.temp_vertices(0), MainState.temp_vertices(1), cVert, False)
                Dim linkedTris As New List(Of Triangle)
                linkedTris.Add(MainState.intelligentFocusTriangle)
                Dim isClosed As Boolean
                If Control.ModifierKeys = Keys.Shift Then
                    linkedTris.AddRange(MainState.intelligentFocusTriangle.vertexA.linkedTriangles)
                    linkedTris.AddRange(MainState.intelligentFocusTriangle.vertexB.linkedTriangles)
                    linkedTris.AddRange(MainState.intelligentFocusTriangle.vertexC.linkedTriangles)
                Else
                    isClosed = CSG.pointsConnected(MainState.temp_vertices(0), MainState.temp_vertices(1)) OrElse
                    CSG.isVertexInTriangle(MainState.intelligentFocusTriangle.vertexA, newTri) OrElse
                    CSG.isVertexInTriangle(MainState.intelligentFocusTriangle.vertexB, newTri) OrElse
                    CSG.isVertexInTriangle(MainState.intelligentFocusTriangle.vertexC, newTri)
                End If

                For Each tri As Triangle In linkedTris
                    If CSG.trianglesIntersectionsOnly(newTri, tri, False) OrElse isClosed Then
                        MainState.intelligentFocusTriangle = tri
                    End If
                Next
                If CSG.isVertexInTriangle(cVert, MainState.intelligentFocusTriangle) Then
                    MainState.trianglemode = 0
                End If
                MainState.lastPointX = MainState.temp_vertices(1).X
                MainState.lastPointY = MainState.temp_vertices(1).Y
            End If
        End If
    End Sub

    Private Sub checkIntersection(ByVal cVert As Vertex)
        Dim smallestDistance As Vertex = MainState.temp_vertices(0)
        Dim mediumDistance As Vertex = MainState.temp_vertices(1)
        Dim longestDistance As Vertex = MainState.temp_corner_vertex

        Dim intersectionA As Vertex = CSG.intersectionBetweenTwoLines(cVert, mediumDistance, smallestDistance, longestDistance)

        If Not intersectionA Is Nothing Then
            MainState.temp_vertices(1) = longestDistance
            MainState.temp_corner_vertex = mediumDistance
            mediumDistance = MainState.temp_vertices(1)
            longestDistance = MainState.temp_corner_vertex
        End If

        Dim intersectionB As Vertex = CSG.intersectionBetweenTwoLines(smallestDistance, cVert, mediumDistance, longestDistance)

        If Not intersectionB Is Nothing Then
            MainState.temp_vertices(0) = mediumDistance
            MainState.temp_vertices(1) = longestDistance
            MainState.temp_corner_vertex = smallestDistance
        End If

        Dim newTri As Triangle = New Triangle(MainState.temp_vertices(0), MainState.temp_vertices(1), cVert, False)

        If CSG.isVertexInTriangle(MainState.temp_corner_vertex, newTri) Then
            MainState.temp_vertices(1) = longestDistance
            MainState.temp_corner_vertex = mediumDistance
            mediumDistance = MainState.temp_vertices(1)
            longestDistance = MainState.temp_corner_vertex
        End If
    End Sub


    Private Sub MainForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ImageToolStripMenuItem.Checked = False
        If LDSettings.Editor.showImageViewAtStartup Then
            ImageToolStripMenuItem.PerformClick()
        End If
        ViewPrefsToolStripMenuItem.Checked = False
        If LDSettings.Editor.showPreferencesViewAtStartup Then
            ViewPrefsToolStripMenuItem.PerformClick()
        End If
        Me.BackColor = LDSettings.Colours.background
        Dim tmx, tmy As Integer
        tmx = System.Windows.Forms.Cursor.Position.X
        tmy = System.Windows.Forms.Cursor.Position.Y
        Me.Activate()
        If LDSettings.Editor.startWithFullscreen Then
            Me.WindowState = FormWindowState.Maximized
            Me.BringToFront()
            View.getCorrectionOffset = True
            MouseHelper.moveMouseAbsoluteLeftClick(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2, View.correctionOffsetY + Me.ClientSize.Height / 2)
        Else
            Me.WindowState = FormWindowState.Normal
            Me.BringToFront()
            View.getCorrectionOffset = True
            MouseHelper.moveMouseAbsoluteLeftClick(Me.Location.X + Me.Width / 2 + View.correctionOffsetX, Me.Location.Y + View.correctionOffsetY + Me.Height / 2)
        End If
        If Not LDSettings.Editor.startWithFullscreen Then
            Me.Location = New Point(LDSettings.Editor.mainWindow_x, LDSettings.Editor.mainWindow_y)
            Me.Size = New Point(LDSettings.Editor.mainWindow_width, LDSettings.Editor.mainWindow_height)
        End If
        Me.Activate()
        Me.BringToFront()
    End Sub

    Private Sub ToolStrip1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainToolStrip.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub ToolStrip2_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColourToolStrip.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub MenuStrip1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuStrip1.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub GBMatrix_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles GBMatrix.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub GBSpline_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles GBSpline.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub GBWidth_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub StatusStrip1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles StatusStrip1.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub BtnMode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnMode.Click
        If MainState.objectToModify = Modified.Primitive Then
            For Each tri As Triangle In View.SelectedTriangles
                tri.selected = False
            Next
            For Each vert As Vertex In View.SelectedVertices
                vert.selected = False
            Next
            View.SelectedTriangles.Clear()
            View.SelectedVertices.Clear()
            VerticesModeToolStripMenuItem.PerformClick()
        Else
            If MainState.objectToModify = Modified.Triangle Then
                PrimitiveModeToolStripMenuItem.PerformClick()
            Else
                TrianglesModeToolStripMenuItem.PerformClick()
            End If
        End If
    End Sub

    Private Sub LDUToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LDUToolStripMenuItem.Click
        CircleRadius.Text = ""
        CircleInnerRadius.Text = ""
        OvalRadiusX.Text = ""
        OvalRadiusY.Text = ""
        LDSettings.Editor.radius_circle = 0
        LDSettings.Editor.radiusInner_circle = 0
        LDSettings.Editor.radius_oval_x = 0
        LDSettings.Editor.radius_oval_y = 0
        GBMatrix.Visible = False
        MainState.conversionEnabled = True
        PreferencesForm.NUDMoveSnap.DecimalPlaces = 0
        PreferencesForm.NUDGrid.DecimalPlaces = 1
        If Not LDUToolStripMenuItem.Checked Then LDUToolStripMenuItem.Checked = True
        InchToolStripMenuItem.Checked = False
        MillimeterToolStripMenuItem.Checked = False
        View.unit = "LDU"
        View.unitFactor = 1
        ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
        PreferencesForm.LblMoveSnap.Text = String.Format(I18N.trl8(I18N.lk.MoveSnap), translateUnit(View.unit))
        PreferencesForm.LblGridSize.Text = String.Format(I18N.trl8(I18N.lk.GridSize), translateUnit(View.unit))
        LblUnit1.Text = translateUnit(View.unit)
        LblUnit2.Text = translateUnit(View.unit)
        LblUnit3.Text = translateUnit(View.unit)
        LblUnit4.Text = translateUnit(View.unit)
        Dim temp As Double
        temp = View.moveSnap * View.unitFactor
        PreferencesForm.NUDMoveSnap.Minimum = 1
        PreferencesForm.NUDMoveSnap.Maximum = 10000
        MathHelper.clip(temp, PreferencesForm.NUDMoveSnap.Minimum, PreferencesForm.NUDMoveSnap.Maximum)
        PreferencesForm.NUDMoveSnap.Value = temp
        temp = View.rasterSnap * View.unitFactor
        PreferencesForm.NUDGrid.Minimum = 0.1
        PreferencesForm.NUDGrid.Maximum = 10
        MathHelper.clip(temp, PreferencesForm.NUDGrid.Minimum, PreferencesForm.NUDGrid.Maximum)
        PreferencesForm.NUDGrid.Value = temp
        If GBVertex.Visible AndAlso View.SelectedVertices.Count = 1 Then
            NUDVertX.Value = View.SelectedVertices(0).X * View.unitFactor / 1000
            NUDVertY.Value = View.SelectedVertices(0).Y * View.unitFactor / 1000
        End If
        MainState.conversionEnabled = False
    End Sub

    Private Sub MillimeterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MillimeterToolStripMenuItem.Click
        CircleRadius.Text = ""
        CircleInnerRadius.Text = ""
        OvalRadiusX.Text = ""
        OvalRadiusY.Text = ""
        LDSettings.Editor.radius_circle = 0
        LDSettings.Editor.radiusInner_circle = 0
        LDSettings.Editor.radius_oval_x = 0
        LDSettings.Editor.radius_oval_y = 0
        GBMatrix.Visible = False
        MainState.conversionEnabled = True
        PreferencesForm.NUDMoveSnap.DecimalPlaces = 1
        PreferencesForm.NUDGrid.DecimalPlaces = 2
        If Not MillimeterToolStripMenuItem.Checked Then MillimeterToolStripMenuItem.Checked = True
        LDUToolStripMenuItem.Checked = False
        InchToolStripMenuItem.Checked = False
        View.unit = "mm"
        View.unitFactor = 0.4F
        ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
        PreferencesForm.LblMoveSnap.Text = String.Format(I18N.trl8(I18N.lk.MoveSnap), translateUnit(View.unit))
        PreferencesForm.LblGridSize.Text = String.Format(I18N.trl8(I18N.lk.GridSize), translateUnit(View.unit))
        LblUnit1.Text = translateUnit(View.unit)
        LblUnit2.Text = translateUnit(View.unit)
        LblUnit3.Text = translateUnit(View.unit)
        LblUnit4.Text = translateUnit(View.unit)
        Dim temp As Double
        temp = View.moveSnap * View.unitFactor
        PreferencesForm.NUDMoveSnap.Minimum = 0.4
        PreferencesForm.NUDMoveSnap.Maximum = 4000
        MathHelper.clip(temp, PreferencesForm.NUDMoveSnap.Minimum, PreferencesForm.NUDMoveSnap.Maximum)
        PreferencesForm.NUDMoveSnap.Value = temp
        temp = View.rasterSnap * View.unitFactor
        PreferencesForm.NUDGrid.Minimum = 0.4 * 0.1
        PreferencesForm.NUDGrid.Maximum = 0.4 * 10
        MathHelper.clip(temp, PreferencesForm.NUDGrid.Minimum, PreferencesForm.NUDGrid.Maximum)
        PreferencesForm.NUDGrid.Value = temp
        If GBVertex.Visible AndAlso View.SelectedVertices.Count = 1 Then
            NUDVertX.Value = View.SelectedVertices(0).X * View.unitFactor / 1000
            NUDVertY.Value = View.SelectedVertices(0).Y * View.unitFactor / 1000
        End If
        MainState.conversionEnabled = False
    End Sub

    Private Sub InchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InchToolStripMenuItem.Click
        CircleRadius.Text = ""
        CircleInnerRadius.Text = ""
        OvalRadiusX.Text = ""
        OvalRadiusY.Text = ""
        LDSettings.Editor.radius_circle = 0
        LDSettings.Editor.radiusInner_circle = 0
        LDSettings.Editor.radius_oval_x = 0
        LDSettings.Editor.radius_oval_y = 0
        GBMatrix.Visible = False
        MainState.conversionEnabled = True
        PreferencesForm.NUDMoveSnap.DecimalPlaces = 3
        PreferencesForm.NUDGrid.DecimalPlaces = 4
        If Not InchToolStripMenuItem.Checked Then InchToolStripMenuItem.Checked = True
        LDUToolStripMenuItem.Checked = False
        MillimeterToolStripMenuItem.Checked = False
        View.unit = "inch"
        View.unitFactor = 0.016F
        ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
        PreferencesForm.LblMoveSnap.Text = String.Format(I18N.trl8(I18N.lk.MoveSnap), translateUnit(View.unit))
        PreferencesForm.LblGridSize.Text = String.Format(I18N.trl8(I18N.lk.GridSize), translateUnit(View.unit))
        LblUnit1.Text = translateUnit(View.unit)
        LblUnit2.Text = translateUnit(View.unit)
        LblUnit3.Text = translateUnit(View.unit)
        LblUnit4.Text = translateUnit(View.unit)
        Dim temp As Double
        temp = View.moveSnap * View.unitFactor
        PreferencesForm.NUDMoveSnap.Minimum = 0.016
        PreferencesForm.NUDMoveSnap.Maximum = 160
        MathHelper.clip(temp, PreferencesForm.NUDMoveSnap.Minimum, PreferencesForm.NUDMoveSnap.Maximum)
        PreferencesForm.NUDMoveSnap.Value = temp
        temp = View.rasterSnap * View.unitFactor
        PreferencesForm.NUDGrid.Minimum = 0.016 * 0.1
        PreferencesForm.NUDGrid.Maximum = 0.016 * 10
        MathHelper.clip(temp, PreferencesForm.NUDGrid.Minimum, PreferencesForm.NUDGrid.Maximum)
        PreferencesForm.NUDGrid.Value = temp
        If GBVertex.Visible AndAlso View.SelectedVertices.Count = 1 Then
            NUDVertX.Value = View.SelectedVertices(0).X * View.unitFactor / 1000
            NUDVertY.Value = View.SelectedVertices(0).Y * View.unitFactor / 1000
        End If
        MainState.conversionEnabled = False
    End Sub

    Private Sub BtnAddToGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddToGroup.Click

        For Each tri As Triangle In View.SelectedTriangles
            For Each tri2 As Triangle In tri.vertexA.linkedTriangles
                If tri2.groupindex <> -1 Then Exit Sub
            Next
            For Each tri2 As Triangle In tri.vertexB.linkedTriangles
                If tri2.groupindex <> -1 Then Exit Sub
            Next
            For Each tri2 As Triangle In tri.vertexC.linkedTriangles
                If tri2.groupindex <> -1 Then Exit Sub
            Next
            If tri.vertexA.groupindex <> Primitive.NO_INDEX Then Exit Sub
            If tri.vertexB.groupindex <> Primitive.NO_INDEX Then Exit Sub
            If tri.vertexC.groupindex <> Primitive.NO_INDEX Then Exit Sub
            tri.selected = False
        Next

        MainState.temp_center = New Vertex(0, 0, False, False)
        For Each vert As Vertex In View.SelectedVertices
            MainState.temp_center += vert
        Next
        MainState.temp_center.X /= View.SelectedVertices.Count
        MainState.temp_center.Y /= View.SelectedVertices.Count

        Dim mindist As Double = Double.PositiveInfinity
        Dim minvert As Vertex = New Vertex(MainState.temp_center.X, MainState.temp_center.Y, False, False)
        For Each vert As Vertex In View.SelectedVertices
            Dim dist As Double
            dist = vert.dist(MainState.temp_center)
            If dist < mindist Then
                minvert = vert
                mindist = dist
            End If
        Next
        CenterDialog.NUDVertX.Value = MainState.temp_center.X
        CenterDialog.NUDVertY.Value = MainState.temp_center.Y
        Dim result As DialogResult
        result = CenterDialog.ShowDialog()
        If result = DialogResult.OK Then
            LPCFile.Vertices.Add(New Vertex(CenterDialog.NUDVertX.Value, CenterDialog.NUDVertY.Value, False))
            LPCFile.Triangles.Add(New Triangle(minvert, minvert, ListHelper.LLast(LPCFile.Vertices), True))
            ListHelper.LLast(LPCFile.Vertices).linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            minvert.linkedTriangles.Add(ListHelper.LLast(LPCFile.Triangles))
            LPCFile.Primitives.Add(New Primitive(CenterDialog.NUDVertX.Value, CenterDialog.NUDVertY.Value, 0, 0, "subfile" & Now.Ticks, ListHelper.LLast(LPCFile.Vertices).vertexID))
            LPCFile.PrimitivesHMap.Add(GlobalIdSet.primitiveIDglobal, LPCFile.Primitives.Count - 1)
            LPCFile.PrimitivesMetadataHMap.Add(ListHelper.LLast(LPCFile.Primitives).primitiveName, New Metadata())
            ListHelper.LLast(LPCFile.Vertices).groupindex = GlobalIdSet.primitiveIDglobal
            ListHelper.LLast(LPCFile.Triangles).groupindex = GlobalIdSet.primitiveIDglobal
            For Each vert As Vertex In View.SelectedVertices
                vert.groupindex = GlobalIdSet.primitiveIDglobal
                vert.selected = False
            Next
            For Each tri As Triangle In View.SelectedTriangles
                tri.groupindex = GlobalIdSet.primitiveIDglobal
                tri.selected = False
            Next
            View.SelectedVertices.Clear()
            View.SelectedTriangles.Clear()
            BtnAddToGroup.Enabled = False
            PrimitiveModeToolStripMenuItem.PerformClick()
            Me.Refresh()
        End If
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub BtnUngroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs, Optional ByVal history As Boolean = True) Handles BtnUngroup.Click
        Dim myindex As Integer = CType(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex), Integer)
        LPCFile.PrimitivesHMap.Remove(View.SelectedVertices(0).groupindex)
        If LPCFile.PrimitivesHMap.Keys.Count > 0 Then
            Dim klist(LPCFile.PrimitivesHMap.Keys.Count - 1) As Integer
            Dim counter As Integer = 0
            For Each key As Integer In LPCFile.PrimitivesHMap.Keys
                klist(counter) = key
                counter += 1
            Next
            For i As Integer = 0 To LPCFile.PrimitivesHMap.Keys.Count - 1
                If LPCFile.PrimitivesHMap.Item(klist(i)) > myindex Then
                    LPCFile.PrimitivesHMap.Item(klist(i)) -= 1
                End If
            Next i
        End If
        Dim tempMyColour As Color = LPCFile.Primitives(myindex).myColour
        Dim tempMyColourNumber As Short = LPCFile.Primitives(myindex).myColourNumber
        Dim tempVertexID As Integer = LPCFile.Primitives(myindex).centerVertexID
        Dim tempVertex As Vertex = Nothing
        Dim tempName As String = LPCFile.Primitives(myindex).primitiveName
        Dim deleteMetadata As Boolean = True
        LPCFile.Primitives.RemoveAt(myindex)
        For Each prim As Primitive In LPCFile.Primitives
            If prim.primitiveName = tempName Then
                deleteMetadata = False
                Exit For
            End If
        Next
        If deleteMetadata AndAlso LPCFile.PrimitivesMetadataHMap.ContainsKey(tempName) Then
            LPCFile.PrimitivesMetadataHMap.Remove(tempName)
        End If
        For Each vert As Vertex In View.SelectedVertices
            If vert.vertexID = tempVertexID Then tempVertex = vert
            vert.groupindex = Primitive.NO_INDEX
            vert.selected = False
        Next
        For Each tri As Triangle In View.SelectedTriangles
            If tri.myColourNumber = 16 Then
                tri.myColourNumber = tempMyColourNumber
                tri.myColour = tempMyColour
            End If

            tri.groupindex = -1
            tri.selected = False
        Next
        View.SelectedVertices.Clear()
        View.SelectedTriangles.Clear()
        If Not tempVertex Is Nothing AndAlso tempVertex.linkedTriangles.Count = 1 Then
            VerticesModeToolStripMenuItem.PerformClick()
            View.SelectedVertices.Add(tempVertex)
            ClipboardHelper.delete()
        End If
        BtnUngroup.Enabled = False
        GBMatrix.Visible = False
        If history Then UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Private Sub MetadataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetadataToolStripMenuItem.Click
        MetadataDialog.ShowDialog()
        UndoRedoHelper.addHistory()
    End Sub

    Private Sub EProjectOnYZPlaneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EProjectOnYZPlaneToolStripMenuItem.Click
        exportDat(0)
    End Sub

    Private Sub EProjectOnYZPlane2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EProjectOnYZPlane2ToolStripMenuItem.Click
        exportDat(3)
    End Sub

    Private Sub EProjectOnZXPlaneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EProjectOnZXPlaneToolStripMenuItem.Click
        exportDat(1)
    End Sub

    Private Sub EProjectOnZXPlane2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EProjectOnZXPlane2ToolStripMenuItem.Click
        exportDat(4)
    End Sub

    Private Sub EProjectOnXYPlaneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EProjectOnXYPlaneToolStripMenuItem.Click
        exportDat(2)
    End Sub

    Private Sub EProjectOnXYPlane2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EProjectOnXYPlane2ToolStripMenuItem.Click
        exportDat(5)
    End Sub

    Private Sub exportDat(ByVal projectMode As Byte)
        If Not MainState.isLoading AndAlso useRecommendedProjection(projectMode) AndAlso userWantsToExportDAT(projectMode) Then
            LDSettings.ExportAgain = True
            While LDSettings.ExportAgain
                LDSettings.ExportAgain = False
                Invoke(New LDSettings.exportDatD(AddressOf exportDatThread), projectMode)
            End While
        End If
    End Sub

    Private Sub BtnShowPalette_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnShowPalette.Click
        Me.AddOwnedForm(ColourForm)
        If ColourForm.Visible Then ColourForm.BringToFront()
        ColourForm.Show()
    End Sub

    Private Sub BtnPipette_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPipette.Click
        If View.SelectedTriangles.Count = 1 Then
            MainState.lastColour = View.SelectedTriangles(0).myColour
            MainState.lastColourNumber = View.SelectedTriangles(0).myColourNumber
            CLast.BackColor = MainState.lastColour
            If MainState.lastColourNumber = -1 Then
                CLast.Text = "0x2" & MainState.lastColour.R.ToString("X2") & MainState.lastColour.G.ToString("X2") & MainState.lastColour.B.ToString("X2")
            Else
                CLast.Text = MainState.lastColourNumber
            End If
        ElseIf MainState.objectToModify = Modified.Primitive AndAlso View.SelectedTriangles.Count > 0 AndAlso View.SelectedTriangles(0).groupindex <> -1 Then
            MainState.lastColourNumber = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedTriangles(0).groupindex)).myColourNumber
            MainState.lastColour = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedTriangles(0).groupindex)).myColour
            CLast.BackColor = MainState.lastColour
            If MainState.lastColourNumber = -1 Then
                CLast.Text = "0x2" & MainState.lastColour.R.ToString("X2") & MainState.lastColour.G.ToString("X2") & MainState.lastColour.B.ToString("X2")
            Else
                CLast.Text = MainState.lastColourNumber
            End If
        Else
            BtnPipette.Enabled = False
        End If
        ColourForm.NR.Value = MainState.lastColour.R
        ColourForm.NG.Value = MainState.lastColour.G
        ColourForm.NB.Value = MainState.lastColour.B
    End Sub

    Private Sub disableTextboxedit(ByVal b As Boolean)
        DeleteToolStripMenuItem.Enabled = b
        CutToolStripMenuItem.Enabled = b
        CopyToolStripMenuItem.Enabled = b
        PasteToolStripMenuItem.Enabled = b
        BtnCut.Enabled = b
        BtnCopy.Enabled = b
        BtnPaste.Enabled = b
        CMSCut.Enabled = False
        CMSCut.Enabled = b
        CMSCopy.Enabled = b
        CMSPaste.Enabled = b
    End Sub

    Private Sub NUDVertX_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDVertX.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDVertX_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDVertX.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub NUDVertX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDVertX.ValueChanged
        If View.SelectedVertices.Count = 1 Then
            View.SelectedVertices(0).X = NUDVertX.Value * 1000 / View.unitFactor
            Me.Refresh()
        ElseIf Not MainState.readOnlyVertex Is Nothing Then
            If NUDVertX.Value <> (MainState.readOnlyVertex.X / 1000 * View.unitFactor) Then NUDVertX.Value = MainState.readOnlyVertex.X / 1000 * View.unitFactor
        Else
            GBVertex.Visible = False
        End If
    End Sub

    Private Sub NUDVertY_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDVertY.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDVertY_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDVertY.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub NUDVertY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NUDVertY.ValueChanged
        If View.SelectedVertices.Count = 1 Then
            View.SelectedVertices(0).Y = NUDVertY.Value * 1000 / View.unitFactor
            Me.Refresh()
        ElseIf Not MainState.readOnlyVertex Is Nothing Then
            If NUDVertY.Value <> (MainState.readOnlyVertex.Y / 1000 * View.unitFactor) Then NUDVertY.Value = MainState.readOnlyVertex.Y / 1000 * View.unitFactor
        Else
            GBVertex.Visible = False
        End If
    End Sub

    Private Sub GBVertex_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles GBVertex.MouseEnter
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub GBVertex_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GBVertex.VisibleChanged
        If GBVertex.Visible AndAlso View.SelectedVertices.Count > 0 Then
            NUDVertX.Value = View.SelectedVertices(0).X * View.unitFactor / 1000
            NUDVertY.Value = View.SelectedVertices(0).Y * View.unitFactor / 1000
        End If
    End Sub

    Private Sub CLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CLast.Click
        setColour(MainState.lastColour, MainState.lastColourNumber)
    End Sub

    Public Sub loadConfig()
        If My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "Config.cfg") Then
            MainState.isLoading = True
            Try
                Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(EnvironmentPaths.appPath & "Config.cfg", New System.Text.UnicodeEncoding())
                    LDSettings.Editor.myLanguage = DateiIn.ReadLine()
                    Me.ShowAllWarningsToolStripMenuItem.Checked = CType(DateiIn.ReadLine, Boolean)
                    LDSettings.Editor.defaultName = DateiIn.ReadLine
                    LDSettings.Editor.defaultUser = DateiIn.ReadLine
                    LDSettings.Editor.defaultLicense = DateiIn.ReadLine
                    LDSettings.Editor.max_undo = CType(DateiIn.ReadLine, Byte)
                    LDSettings.Editor.performanceMode = CType(DateiIn.ReadLine, Boolean)

                    LDSettings.Editor.startWithFullscreen = CType(DateiIn.ReadLine, Boolean)
                    LDSettings.Editor.showImageViewAtStartup = CType(DateiIn.ReadLine, Boolean)
                    LDSettings.Editor.showPreferencesViewAtStartup = CType(DateiIn.ReadLine, Boolean)

                    LDSettings.Editor.useAlternativeKeys = CType(DateiIn.ReadLine, Boolean)

                    LDSettings.Keys.Abort = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnAbort, LDSettings.Keys.Abort)
                    LDSettings.Keys.AddTriangle = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnAddTriangle, LDSettings.Keys.AddTriangle)
                    LDSettings.Keys.AddVertex = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnAddVertex, LDSettings.Keys.AddVertex)
                    BtnAddReferenceLine.ToolTipText = I18N.trl8(I18N.lk.AddReferenceLine)
                    BtnTriangleAutoCompletion.ToolTipText = I18N.trl8(I18N.lk.TriangleAutoCompletion)

                    LDSettings.Keys.ModeMove = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnMove, LDSettings.Keys.ModeMove)
                    LDSettings.Keys.ModeRotate = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnRotate, LDSettings.Keys.ModeRotate)
                    LDSettings.Keys.ModeScale = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnScale, LDSettings.Keys.ModeScale)
                    LDSettings.Keys.ModeSelect = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnSelect, LDSettings.Keys.ModeSelect)

                    LDSettings.Keys.Pipette = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnPipette, LDSettings.Keys.Pipette)
                    LDSettings.Keys.Preview = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnPreview, LDSettings.Keys.Preview)
                    LDSettings.Keys.ShowColours = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnColours, LDSettings.Keys.ShowColours)
                    LDSettings.Keys.Translate = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnTranslate, LDSettings.Keys.Translate)
                    LDSettings.Keys.Zoom = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnZoom, LDSettings.Keys.Zoom)

                    NewPatternToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    NewPatternToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(NewPatternToolStripMenuItem.ShortcutKeys))
                    LoadPatternToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    LoadPatternToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(LoadPatternToolStripMenuItem.ShortcutKeys))
                    LoadPatternToolStripMenuItem.ToolTipText = LoadPatternToolStripMenuItem.ShortcutKeyDisplayString
                    SaveToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    SaveToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(SaveToolStripMenuItem.ShortcutKeys))
                    SelectAllToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    SelectAllToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(SelectAllToolStripMenuItem.ShortcutKeys))
                    SelectSameColourToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    SelectSameColourToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(SelectSameColourToolStripMenuItem.ShortcutKeys))

                    SelectConnectedToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    SelectConnectedToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(SelectConnectedToolStripMenuItem.ShortcutKeys))
                    SelectTouchingToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    SelectTouchingToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(SelectTouchingToolStripMenuItem.ShortcutKeys))
                    WithColourToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    WithColourToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(WithColourToolStripMenuItem.ShortcutKeys))
                    ResetViewToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    ResetViewToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ResetViewToolStripMenuItem.ShortcutKeys))
                    ImageToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    ImageToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ImageToolStripMenuItem.ShortcutKeys))
                    KeyToSet.setKey(ImageToolStripMenuItem, ImageToolStripMenuItem.ShortcutKeys)
                    ViewPrefsToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    ViewPrefsToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ViewPrefsToolStripMenuItem.ShortcutKeys))
                    KeyToSet.setKey(ViewPrefsToolStripMenuItem, ViewPrefsToolStripMenuItem.ShortcutKeys)
                    ToAverageToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    ToAverageToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ToAverageToolStripMenuItem.ShortcutKeys))
                    ToLastSelectedToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    ToLastSelectedToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ToLastSelectedToolStripMenuItem.ShortcutKeys))

                    LDSettings.Colours.background = Color.FromArgb(CType(DateiIn.ReadLine, Integer))
                    Me.BackColor = LDSettings.Colours.background
                    LDSettings.Colours.linePen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)
                    LDSettings.Colours.inverseLinePen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)
                    LDSettings.Colours.selectedLinePen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)
                    LDSettings.Colours.selectedLinePenFat = New Pen(LDSettings.Colours.selectedLinePen.Color, 2.0F)
                    LDSettings.Colours.selectedLineInVertexModePen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)
                    LDSettings.Colours.vertexBrush = New SolidBrush(Color.FromArgb(CType(DateiIn.ReadLine, Integer)))
                    LDSettings.Colours.selectedVertexBrush = New SolidBrush(Color.FromArgb(CType(DateiIn.ReadLine, Integer)))

                    LDSettings.Colours.originPen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)
                    LDSettings.Colours.gridPen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)
                    LDSettings.Colours.grid10Pen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)
                    LDSettings.Colours.selectionRectPen = New Pen(Color.FromArgb(CType(DateiIn.ReadLine, Integer)), 0.001F)

                    LDSettings.Colours.selectionCrossBrush = New SolidBrush(Color.FromArgb(CType(DateiIn.ReadLine, Integer)))
                    EnvironmentPaths.ldrawPath = DateiIn.ReadLine

                    VerticesModeToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    KeyToSet.setKey(VerticesModeToolStripMenuItem, VerticesModeToolStripMenuItem.ShortcutKeys)
                    TrianglesModeToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    KeyToSet.setKey(TrianglesModeToolStripMenuItem, TrianglesModeToolStripMenuItem.ShortcutKeys)
                    PrimitiveModeToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                    KeyToSet.setKey(PrimitiveModeToolStripMenuItem, PrimitiveModeToolStripMenuItem.ShortcutKeys)

                    ' Config Entries from version 1.3.3
                    If Not DateiIn.EndOfStream Then
                        LDSettings.Keys.CSG = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnCSG, LDSettings.Keys.CSG)
                        LDSettings.Keys.MergeSplit = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnMerge, LDSettings.Keys.MergeSplit)
                        LDSettings.Keys.AddPrimitive = CType(DateiIn.ReadLine, Integer) : KeyToSet.setKey(BtnPrimitives, LDSettings.Keys.AddPrimitive)
                    Else
                        LDSettings.Keys.CSG = Keys.F9 : KeyToSet.setKey(BtnCSG, LDSettings.Keys.CSG)
                        LDSettings.Keys.MergeSplit = Keys.F8 : KeyToSet.setKey(BtnMerge, LDSettings.Keys.MergeSplit)
                        LDSettings.Keys.AddPrimitive = Keys.F7 : KeyToSet.setKey(BtnPrimitives, LDSettings.Keys.AddPrimitive)
                    End If

                    ' Config Entries from version 1.3.5
                    If Not DateiIn.EndOfStream Then
                        LDSettings.Editor.showTemplateLinesOnTop = CType(DateiIn.ReadLine, Boolean)
                    End If

                    ' Config Entries from version 1.4.1
                    If Not DateiIn.EndOfStream Then
                        LDSettings.Editor.lockModeChange = CType(DateiIn.ReadLine, Boolean)
                    End If

                    ' Config Entries from version 1.4.2
                    If Not DateiIn.EndOfStream Then
                        ShowGridToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                        ShowGridToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ShowGridToolStripMenuItem.ShortcutKeys))
                        ShowAxisLabelToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                        ShowAxisLabelToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ShowAxisLabelToolStripMenuItem.ShortcutKeys))
                        ShowBackgroundImageToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                        ShowBackgroundImageToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(ShowBackgroundImageToolStripMenuItem.ShortcutKeys))

                        EnvironmentPaths.recentFiles.Clear()
                        LoadPatternToolStripMenuItem.DropDownItems.Clear()
                        For i As Integer = 0 To 9
                            Dim file As String = DateiIn.ReadLine
                            If file <> "" Then
                                EnvironmentPaths.recentFiles.Add(file)
                                Dim ti As New ToolStripMenuItem
                                ti.Text = Mid(file, file.LastIndexOf("\") + 2)
                                ti.ToolTipText = file
                                AddHandler ti.Click, AddressOf recentFileNameClick
                                LoadPatternToolStripMenuItem.DropDownItems.Add(ti)
                            End If
                        Next
                        If EnvironmentPaths.recentFiles.Count <> 10 Then
                            Dim ts(9 - EnvironmentPaths.recentFiles.Count) As String
                            EnvironmentPaths.recentFiles.AddRange(ts)
                        End If
                    End If

                    ' Config Entries from version 1.4.4
                    If Not DateiIn.EndOfStream Then
                        MergeToNearestTemplateLineToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                        MergeToNearestTemplateLineToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(MergeToNearestTemplateLineToolStripMenuItem.ShortcutKeys))
                        MergeToNearestTriangleLineToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                        MergeToNearestTriangleLineToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(MergeToNearestTriangleLineToolStripMenuItem.ShortcutKeys))
                        MergeToNearestPrimvertexToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                        MergeToNearestPrimvertexToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(MergeToNearestPrimvertexToolStripMenuItem.ShortcutKeys))
                        CSGSplitToolStripMenuItem.ShortcutKeys = CType(DateiIn.ReadLine, Integer)
                        CSGSplitToolStripMenuItem.ShortcutKeyDisplayString = KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(CSGSplitToolStripMenuItem.ShortcutKeys))
                    End If

                    If Not DateiIn.EndOfStream Then
                        ' Config Entries from version 1.4.7
                        LDSettings.Editor.mainWindow_x = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.mainWindow_y = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.mainWindow_width = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.mainWindow_height = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.backgroundWindow_x = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.backgroundWindow_y = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.colourWindow_x = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.colourWindow_y = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.prefsWindow_x = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.prefsWindow_y = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.colourWindow_width = CType(DateiIn.ReadLine, Integer)
                        LDSettings.Editor.colourWindow_height = CType(DateiIn.ReadLine, Integer)
                    End If

                    If Not DateiIn.EndOfStream Then
                        ' Config Entries from version 1.6.6
                        View.showGrid = CType(DateiIn.ReadLine, Boolean)
                        ShowGridToolStripMenuItem.Checked = View.showGrid
                        If CType(DateiIn.ReadLine, Boolean) Then
                            BtnColours.PerformClick()
                        End If
                    End If

                End Using
                BtnMode.ToolTipText = I18N.trl8(I18N.lk.TriangleMode) & " [" & KeyToSet.keyToString(New System.Windows.Forms.KeyEventArgs(TrianglesModeToolStripMenuItem.ShortcutKeys)) & "]"
                Me.MaxUndoToolStripTextBox.Text = LDSettings.Editor.max_undo
                If Not LDSettings.Editor.showImageViewAtStartup AndAlso ImageToolStripMenuItem.Checked Then Me.ImageToolStripMenuItem.PerformClick()
                If Not LDSettings.Editor.showPreferencesViewAtStartup AndAlso ViewPrefsToolStripMenuItem.Checked Then ViewPrefsToolStripMenuItem.PerformClick()
            Catch
                Try
                    My.Computer.FileSystem.DeleteFile(EnvironmentPaths.appPath & "Config.cfg")
                Catch
                End Try
            End Try
            MainState.isLoading = False
        End If
    End Sub

    Public Sub saveConfig()
        Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(EnvironmentPaths.appPath & "Config.cfg", False, New System.Text.UnicodeEncoding())
            DateiOut.WriteLine(LDSettings.Editor.myLanguage)

            DateiOut.WriteLine(ShowAllWarningsToolStripMenuItem.Checked)
            DateiOut.WriteLine(LDSettings.Editor.defaultName)
            DateiOut.WriteLine(LDSettings.Editor.defaultUser)
            DateiOut.WriteLine(LDSettings.Editor.defaultLicense)
            DateiOut.WriteLine(LDSettings.Editor.max_undo)
            DateiOut.WriteLine(LDSettings.Editor.performanceMode)
            DateiOut.WriteLine(LDSettings.Editor.startWithFullscreen)
            DateiOut.WriteLine(LDSettings.Editor.showImageViewAtStartup)
            DateiOut.WriteLine(LDSettings.Editor.showPreferencesViewAtStartup)
            DateiOut.WriteLine(LDSettings.Editor.useAlternativeKeys)

            DateiOut.WriteLine(LDSettings.Keys.Abort)
            DateiOut.WriteLine(LDSettings.Keys.AddTriangle)
            DateiOut.WriteLine(LDSettings.Keys.AddVertex)
            DateiOut.WriteLine(LDSettings.Keys.ModeMove)

            DateiOut.WriteLine(LDSettings.Keys.ModeRotate)
            DateiOut.WriteLine(LDSettings.Keys.ModeScale)
            DateiOut.WriteLine(LDSettings.Keys.ModeSelect)

            DateiOut.WriteLine(LDSettings.Keys.Pipette)
            DateiOut.WriteLine(LDSettings.Keys.Preview)
            DateiOut.WriteLine(LDSettings.Keys.ShowColours)
            DateiOut.WriteLine(LDSettings.Keys.Translate)
            DateiOut.WriteLine(LDSettings.Keys.Zoom)

            DateiOut.WriteLine(CType(NewPatternToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(LoadPatternToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(SaveToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(SelectAllToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(SelectSameColourToolStripMenuItem.ShortcutKeys, Integer))

            DateiOut.WriteLine(CType(SelectConnectedToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(SelectTouchingToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(WithColourToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(ResetViewToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(ImageToolStripMenuItem.ShortcutKeys, Integer))

            DateiOut.WriteLine(CType(ViewPrefsToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(ToAverageToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(ToLastSelectedToolStripMenuItem.ShortcutKeys, Integer))

            DateiOut.WriteLine(LDSettings.Colours.background.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.linePen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.inverseLinePen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.selectedLinePen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.selectedLineInVertexModePen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.vertexBrush.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.selectedVertexBrush.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.originPen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.gridPen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.grid10Pen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.selectionRectPen.Color.ToArgb)
            DateiOut.WriteLine(LDSettings.Colours.selectionCrossBrush.Color.ToArgb)

            DateiOut.WriteLine(EnvironmentPaths.ldrawPath)

            DateiOut.WriteLine(CType(VerticesModeToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(TrianglesModeToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(PrimitiveModeToolStripMenuItem.ShortcutKeys, Integer))

            ' Config Entries from version 1.3.3
            DateiOut.WriteLine(LDSettings.Keys.CSG)
            DateiOut.WriteLine(LDSettings.Keys.MergeSplit)
            DateiOut.WriteLine(LDSettings.Keys.AddPrimitive)

            ' Config Entries from version 1.3.5
            DateiOut.WriteLine(LDSettings.Editor.showTemplateLinesOnTop)

            ' Config Entries from version 1.4.1
            DateiOut.WriteLine(LDSettings.Editor.lockModeChange)

            ' Config Entries from version 1.4.2
            DateiOut.WriteLine(CType(ShowGridToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(ShowAxisLabelToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(ShowBackgroundImageToolStripMenuItem.ShortcutKeys, Integer))
            For i As Integer = 0 To 9
                DateiOut.WriteLine(EnvironmentPaths.recentFiles(i))
            Next

            ' Config Entries from version 1.4.4
            DateiOut.WriteLine(CType(MergeToNearestTemplateLineToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(MergeToNearestTriangleLineToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(MergeToNearestPrimvertexToolStripMenuItem.ShortcutKeys, Integer))
            DateiOut.WriteLine(CType(CSGSplitToolStripMenuItem.ShortcutKeys, Integer))

            ' Config Entries from version 1.4.7
            DateiOut.WriteLine(LDSettings.Editor.mainWindow_x)
            DateiOut.WriteLine(LDSettings.Editor.mainWindow_y)
            DateiOut.WriteLine(LDSettings.Editor.mainWindow_width)
            DateiOut.WriteLine(LDSettings.Editor.mainWindow_height)
            DateiOut.WriteLine(LDSettings.Editor.backgroundWindow_x)
            DateiOut.WriteLine(LDSettings.Editor.backgroundWindow_y)
            DateiOut.WriteLine(LDSettings.Editor.colourWindow_x)
            DateiOut.WriteLine(LDSettings.Editor.colourWindow_y)
            DateiOut.WriteLine(LDSettings.Editor.prefsWindow_x)
            DateiOut.WriteLine(LDSettings.Editor.prefsWindow_y)
            DateiOut.WriteLine(LDSettings.Editor.colourWindow_width)
            DateiOut.WriteLine(LDSettings.Editor.colourWindow_height)

            ' Config Entries from version 1.6.6
            DateiOut.WriteLine(View.showGrid)
            DateiOut.WriteLine(ColourToolStrip.Visible)
        End Using
    End Sub

    Private Sub ShowAllWarningsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowAllWarningsToolStripMenuItem.Click
        saveConfig()
    End Sub

    Private Sub BtnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPreview.Click
        Dim isEnabled As Boolean = Not BtnPreview.Checked
        If BtnAddTriangle.Checked Then BtnAddTriangle.PerformClick()
        If BtnAddVertex.Checked Then BtnAddVertex.PerformClick()
        If BtnTriangleAutoCompletion.Checked Then BtnTriangleAutoCompletion.PerformClick()
        BtnSelect.PerformClick()
        Helper_2D.clearSelection()
        Me.MainToolStrip.Enabled = isEnabled
        Me.MenuStrip1.Enabled = isEnabled
        Me.CMS.Enabled = isEnabled
        VerticesModeToolStripMenuItem.Enabled = isEnabled
        disableTextboxedit(isEnabled)
        If BtnPreview.Checked Then
            BtnPreview.Tag = MainState.objectToModify
            PrimitiveModeToolStripMenuItem.PerformClick()
        Else
            Select Case BtnPreview.Tag
                Case Modified.Triangle
                    TrianglesModeToolStripMenuItem.PerformClick()
                Case Modified.Vertex
                    VerticesModeToolStripMenuItem.PerformClick()
            End Select
        End If
        Me.Refresh()
    End Sub

    Private Sub CreateATemplateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateATemplateToolStripMenuItem.Click
        ChooseTemplateDialog.selectedText = ""
        Dim result As DialogResult
        result = TemplateEditor.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            Dim ti As New ToolStripMenuItem
            ti.Text = TemplateEditor.oldName
            ti.ToolTipText = EnvironmentPaths.appPath & TemplateEditor.oldName & ".txt"
            AddHandler ti.Click, AddressOf TemplateItemClick
            TemplateToolStripMenuItem.DropDownItems.Add(ti)
        End If
    End Sub

    Private Sub EditTemplateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditTemplateToolStripMenuItem.Click
        ChooseTemplateDialog.ShowDialog()
    End Sub

    Private Sub MaxUndoToolStripTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MaxUndoToolStripTextBox.TextChanged
        Try
            If LDSettings.Editor.max_undo <> CType(MaxUndoToolStripTextBox.Text, Byte) Then
                LDSettings.Editor.max_undo = CType(MaxUndoToolStripTextBox.Text, Byte)
                If LDSettings.Editor.max_undo < 3 Then LDSettings.Editor.max_undo = 3
                MaxUndoToolStripTextBox.Text = LDSettings.Editor.max_undo
                saveConfig()
            End If
        Catch ex As Exception
            MaxUndoToolStripTextBox.Text = LDSettings.Editor.max_undo
        End Try
    End Sub

    Private Sub O100ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles O100ToolStripMenuItem.Click
        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 100%"
        View.alpha = 255
        Me.Refresh()
    End Sub

    Private Sub O50ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles O50ToolStripMenuItem.Click
        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 50%"
        View.alpha = 127
        Me.Refresh()
    End Sub

    Private Sub O25ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles O25ToolStripMenuItem.Click
        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 25%"
        View.alpha = 64
        Me.Refresh()
    End Sub

    Private Sub O10ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles O10ToolStripMenuItem.Click
        OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & ": 10%"
        View.alpha = 26
        Me.Refresh()
    End Sub

    Private Sub LoadBackgroundImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadBackgroundImageToolStripMenuItem.Click
        If Not ImageForm.Visible Then
            ImageForm.Visible = True
            ImageForm.BtnImagePath.PerformClick()
            ImageForm.Visible = False
        Else
            ImageForm.BtnImagePath.PerformClick()
        End If
    End Sub

    Private Sub MoreOptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvancedOptionsToolStripMenuItem.Click
        If OptionsDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            saveConfig()
            Me.Refresh()
        End If
    End Sub

    Private Sub ShowAxisLabelToolStripMenuItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowAxisLabelToolStripMenuItem.CheckedChanged
        Me.Refresh()
    End Sub

    Private Sub ShowBackgroundImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowBackgroundImageToolStripMenuItem.Click
        Me.Refresh()
    End Sub

    Private Sub AdjustBackgroundImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdjustBackgroundImageToolStripMenuItem.Click
        If Not ImageForm.Visible Then
            ImageForm.Visible = True
            ImageForm.BtnAdjust.PerformClick()
            ImageForm.Visible = False
        Else
            ImageForm.BtnAdjust.PerformClick()
        End If
    End Sub

    Private Sub LblZoom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LblZoom.Click
        If ZoomDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            If View.zoomfactor = 0.001 Then
                View.zoomlevel = -30
            ElseIf View.zoomfactor <= 0.01F Then
                View.zoomlevel = (View.zoomfactor - 1.0F) * 10.0F
            ElseIf View.zoomfactor <= 0.1F Then
                View.zoomlevel = (View.zoomfactor - 0.2F) * 100.0F
            ElseIf View.zoomfactor <= 10.0F Then
                View.zoomlevel = (View.zoomfactor - 1.0F) * 10.0F
            End If
            MathHelper.clip(View.zoomlevel, -30, 90)
            LblZoom.Text = I18N.trl8(I18N.lk.ZoomParam) & " " & CType(View.zoomfactor * 10000, Integer) / 100 & "%"
            LblCoords.Text = Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000 & " | " & Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000 & " " & translateUnit(View.unit)
            Me.Refresh()
        End If
    End Sub

    Private Sub ZoomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomToolStripMenuItem.Click
        If ZoomDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            If View.zoomfactor = 0.001 Then
                View.zoomlevel = -30
            ElseIf View.zoomfactor <= 0.01F Then
                View.zoomlevel = (View.zoomfactor - 1.0F) * 10.0F
            ElseIf View.zoomfactor <= 0.1F Then
                View.zoomlevel = (View.zoomfactor - 0.2F) * 100.0F
            ElseIf View.zoomfactor <= 10.0F Then
                View.zoomlevel = (View.zoomfactor - 1.0F) * 10.0F
            End If
            MathHelper.clip(View.zoomlevel, -30, 90)
            LblZoom.Text = I18N.trl8(I18N.lk.ZoomParam) & " " & CType(View.zoomfactor * 10000, Integer) / 100 & "%"
            LblCoords.Text = Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000 & " | " & Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000 & " " & translateUnit(View.unit)
            Me.Refresh()
        End If
    End Sub

    Private Sub MergeToNearestPrimvertexToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MergeToNearestPrimvertexToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 AndAlso MainState.objectToModify <> Modified.Primitive Then
            If View.SelectedVertices.Count > 1 Then
                Dim sumX As Integer = 0
                Dim sumY As Integer = 0
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    sumX += View.SelectedVertices(i).X
                    sumY += View.SelectedVertices(i).Y
                Next
                sumX /= View.SelectedVertices.Count
                sumY /= View.SelectedVertices.Count
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    View.SelectedVertices(i).X = sumX
                    View.SelectedVertices(i).Y = sumY
                Next
                View.SelectedTriangles.Clear()
                cleanupDATVertices()
                cleanupDATTriangles()
                For i As Integer = 0 To LPCFile.Vertices.Count - 1
                    For j As Integer = 0 To View.SelectedVertices.Count - 1
                        If LPCFile.Vertices(i).vertexID = View.SelectedVertices(j).vertexID Then
                            View.SelectedVertices(j).selected = False
                        End If
                    Next
                Next
newDelete:
                For j As Integer = 0 To View.SelectedVertices.Count - 1
                    If View.SelectedVertices(j).selected Then
                        View.SelectedVertices.RemoveAt(j)
                        GoTo newDelete
                    End If
                Next
            End If
            View.SelectedVertices(0).selected = False
            Dim x As Double = 0
            Dim y As Double = 0
            Dim min_dist As Double = Double.MaxValue
            For i As Integer = 0 To LPCFile.Vertices.Count - 1
                If LPCFile.Vertices(i).vertexID <> View.SelectedVertices(0).vertexID Then
                    If LPCFile.Vertices(i).groupindex <> Primitive.NO_INDEX AndAlso min_dist > LPCFile.Vertices(i).dist(View.SelectedVertices(0)) Then
                        min_dist = LPCFile.Vertices(i).dist(View.SelectedVertices(0))
                        x = LPCFile.Vertices(i).X
                        y = LPCFile.Vertices(i).Y
                    End If
                End If
            Next
            View.SelectedVertices(0).X = x
            View.SelectedVertices(0).Y = y
            View.SelectedVertices.Clear()
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub MergeToNearestTemplateLineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MergeToNearestTemplateLineToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 AndAlso LPCFile.templateShape.Count > 1 AndAlso MainState.objectToModify <> Modified.Primitive Then
            If View.SelectedVertices.Count > 1 Then
                Dim sumX As Integer = 0
                Dim sumY As Integer = 0
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    sumX += View.SelectedVertices(i).X
                    sumY += View.SelectedVertices(i).Y
                Next
                sumX /= View.SelectedVertices.Count
                sumY /= View.SelectedVertices.Count
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    View.SelectedVertices(i).X = sumX
                    View.SelectedVertices(i).Y = sumY
                Next
                View.SelectedTriangles.Clear()
                cleanupDATVertices()
                cleanupDATTriangles()
                For i As Integer = 0 To LPCFile.Vertices.Count - 1
                    For j As Integer = 0 To View.SelectedVertices.Count - 1
                        If LPCFile.Vertices(i).vertexID = View.SelectedVertices(j).vertexID Then
                            View.SelectedVertices(j).selected = False
                        End If
                    Next
                Next
newDelete:
                For j As Integer = 0 To View.SelectedVertices.Count - 1
                    If View.SelectedVertices(j).selected Then
                        View.SelectedVertices.RemoveAt(j)
                        GoTo newDelete
                    End If
                Next
            End If
            View.SelectedVertices(0).selected = False
            Dim x As Double = View.SelectedVertices(0).X
            Dim y As Double = View.SelectedVertices(0).Y
            Dim minDist As Double = Double.MaxValue
            Dim dist As Double
            Dim startVertex As New Vertex(0, 0, False, False)
            Dim startPolyVertex As New Vertex(0, 0, False, False)
            Dim endVertex As Vertex = Nothing
            Dim distVertex As Vertex
            Dim finalVertex As Vertex = Nothing

            ' Detect if it is nearer to a template polygon

            ' Template polygon
            If Not LPCFile.templateShape(0).X = Single.Epsilon Then
                startVertex.X = LPCFile.templateShape(0).X
                startVertex.Y = LPCFile.templateShape(0).Y
                startPolyVertex.X = LPCFile.templateShape(0).X
                startPolyVertex.Y = LPCFile.templateShape(0).Y
                Dim start As Integer = 0
                Dim shapeCount As Integer = LPCFile.templateShape.Count - 1
                For i As Integer = 1 To shapeCount
                    If LPCFile.templateShape(i).X = Single.Epsilon AndAlso Not endVertex Is Nothing Then
                        distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startPolyVertex, endVertex)
                        dist = View.SelectedVertices(0).dist(distVertex)
                        If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                        start = i + 1
                        If start <= shapeCount Then
                            startPolyVertex.X = LPCFile.templateShape(start).X
                            startPolyVertex.Y = LPCFile.templateShape(start).Y
                            startVertex.X = startPolyVertex.X
                            startVertex.Y = startPolyVertex.Y
                            endVertex = Nothing
                        End If
                    Else
                        endVertex = New Vertex(0, 0, False, False)
                        endVertex.X = LPCFile.templateShape(i).X
                        endVertex.Y = LPCFile.templateShape(i).Y
                        distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                        dist = View.SelectedVertices(0).dist(distVertex)
                        If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                        startVertex.X = LPCFile.templateShape(i).X
                        startVertex.Y = LPCFile.templateShape(i).Y
                    End If
                Next
                endVertex.X = LPCFile.templateShape(shapeCount).X
                endVertex.Y = LPCFile.templateShape(shapeCount).Y
                distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startPolyVertex, endVertex)
                dist = View.SelectedVertices(0).dist(distVertex)
                If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
            Else
                Dim shapeCount As Integer = LPCFile.templateShape.Count - 1
                For i As Integer = 1 To shapeCount Step 4

                    If (i + 1) <= shapeCount Then
                        startVertex = New Vertex(LPCFile.templateShape(i).X, LPCFile.templateShape(i).Y, False, False)
                        endVertex = New Vertex(LPCFile.templateShape(i + 1).X, LPCFile.templateShape(i + 1).Y, False, False)
                        distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                        dist = View.SelectedVertices(0).dist(distVertex)
                        If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                    End If

                    If (i + 2) <= shapeCount Then
                        startVertex = New Vertex(LPCFile.templateShape(i + 1).X, LPCFile.templateShape(i + 1).Y, False, False)
                        endVertex = New Vertex(LPCFile.templateShape(i + 2).X, LPCFile.templateShape(i + 2).Y, False, False)
                        distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                        dist = View.SelectedVertices(0).dist(distVertex)
                        If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                    End If

                    If (i + 3) <= shapeCount Then
                        startVertex = New Vertex(LPCFile.templateShape(i + 2).X, LPCFile.templateShape(i + 2).Y, False, False)
                        endVertex = New Vertex(LPCFile.templateShape(i + 3).X, LPCFile.templateShape(i + 3).Y, False, False)
                        distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                        dist = View.SelectedVertices(0).dist(distVertex)
                        If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)

                        startVertex = New Vertex(LPCFile.templateShape(i).X, LPCFile.templateShape(i).Y, False, False)
                        endVertex = New Vertex(LPCFile.templateShape(i + 3).X, LPCFile.templateShape(i + 3).Y, False, False)
                        distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), startVertex, endVertex)
                        dist = View.SelectedVertices(0).dist(distVertex)
                        If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False)
                    End If

                Next
            End If
            If Not finalVertex Is Nothing Then
                View.SelectedVertices(0).X = finalVertex.X
                View.SelectedVertices(0).Y = finalVertex.Y
            End If
            View.SelectedVertices.Clear()
            GBVertex.Visible = False
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub MergeToNearestTriangleLineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MergeToNearestTriangleLineToolStripMenuItem.Click
        If View.SelectedVertices.Count > 0 AndAlso LPCFile.Triangles.Count > 0 AndAlso MainState.objectToModify <> Modified.Primitive Then
            If View.SelectedVertices.Count > 1 Then
                Dim sumX As Integer = 0
                Dim sumY As Integer = 0
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    sumX += View.SelectedVertices(i).X
                    sumY += View.SelectedVertices(i).Y
                Next
                sumX /= View.SelectedVertices.Count
                sumY /= View.SelectedVertices.Count
                For i As Integer = 0 To View.SelectedVertices.Count - 1
                    View.SelectedVertices(i).X = sumX
                    View.SelectedVertices(i).Y = sumY
                Next
                View.SelectedTriangles.Clear()
                cleanupDATVertices()
                cleanupDATTriangles()
                For i As Integer = 0 To LPCFile.Vertices.Count - 1
                    For j As Integer = 0 To View.SelectedVertices.Count - 1
                        If LPCFile.Vertices(i).vertexID = View.SelectedVertices(j).vertexID Then
                            View.SelectedVertices(j).selected = False
                        End If
                    Next
                Next
newDelete:
                For j As Integer = 0 To View.SelectedVertices.Count - 1
                    If View.SelectedVertices(j).selected Then
                        View.SelectedVertices.RemoveAt(j)
                        GoTo newDelete
                    End If
                Next
            End If
            View.SelectedVertices(0).selected = False
            Dim x As Double = View.SelectedVertices(0).X
            Dim y As Double = View.SelectedVertices(0).Y
            Dim minDist As Double = Double.MaxValue
            Dim dist As Double
            Dim distVertex As Vertex
            Dim finalVertex As Vertex = Nothing
            Dim splitVertex1 As Vertex = Nothing
            Dim splitVertex2 As Vertex = Nothing
            Dim splitTriangle As Triangle = Nothing
            ' Detect if it is nearer to a line
            Dim invalidTriangles As New Dictionary(Of Integer, Byte)
            For Each tri As Triangle In View.SelectedVertices(0).linkedTriangles
                invalidTriangles.Add(tri.triangleID, 0)
            Next
            ' Line
            For Each tri As Triangle In LPCFile.Triangles
                If Not invalidTriangles.ContainsKey(tri.triangleID) Then
                    distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), tri.vertexA, tri.vertexB)
                    dist = View.SelectedVertices(0).dist(distVertex)
                    If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : splitTriangle = tri : splitVertex1 = tri.vertexA : splitVertex2 = tri.vertexB

                    distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), tri.vertexB, tri.vertexC)
                    dist = View.SelectedVertices(0).dist(distVertex)
                    If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : splitTriangle = tri : splitVertex1 = tri.vertexB : splitVertex2 = tri.vertexC

                    distVertex = CSG.distanceVectorFromVertexToLine(View.SelectedVertices(0), tri.vertexC, tri.vertexA)
                    dist = View.SelectedVertices(0).dist(distVertex)
                    If dist < minDist Then minDist = dist : finalVertex = New Vertex(distVertex.X, distVertex.Y, False, False) : splitTriangle = tri : splitVertex1 = tri.vertexC : splitVertex2 = tri.vertexA
                End If
            Next
            If Not finalVertex Is Nothing Then
                View.SelectedVertices(0).X = finalVertex.X
                View.SelectedVertices(0).Y = finalVertex.Y
                If splitTriangle.groupindex = -1 Then
                    View.SelectedVertices.Clear()
                    View.SelectedVertices.Add(splitVertex1)
                    View.SelectedVertices.Add(splitVertex2)
                    CSG.splitTriangle(View.SelectedVertices(0), View.SelectedVertices(1))
                    ListHelper.LLast(LPCFile.Vertices).X = finalVertex.X
                    ListHelper.LLast(LPCFile.Vertices).Y = finalVertex.Y
                    cleanupDATVertices()
                    cleanupDATTriangles()
                End If
            End If
            View.SelectedVertices.Clear()
            GBVertex.Visible = False
            If View.CollisionVertices.Count > 0 Then detectCollisions()
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub SetPathToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetPathToolStripMenuItem.Click
        If Me.FBDLDrawDir.ShowDialog = Windows.Forms.DialogResult.Cancel Then
        Else
            EnvironmentPaths.ldrawPath = Me.FBDLDrawDir.SelectedPath
            saveConfig()
        End If
    End Sub

    Private Sub DetectOverlapsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DetectOverlapsToolStripMenuItem.Click
        If DetectOverlapsToolStripMenuItem.Checked Then
            View.CollisionVertices.Clear()
            DetectOverlapsToolStripMenuItem.Checked = False
            BtnNextOverlap.Visible = False
            BtnPrevOverlap.Visible = False
        Else
            MainState.collisionIndex = 0
            detectCollisions()
            If View.CollisionVertices.Count > 0 Then
                MsgBox(String.Format(I18N.trl8(I18N.lk.OverlapsFound), View.CollisionVertices.Count), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Exclamation, I18N.trl8(I18N.lk.Warning))
            Else
                MsgBox(I18N.trl8(I18N.lk.NoOverlaps), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Info))
            End If
        End If
        UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Public Sub detectCollisions()
        View.CollisionVertices.Clear()
        SelectionHelper.selectAll()
        cleanupDATTriangles3()
        Dim startVertexCount As Integer = LPCFile.Vertices.Count
        Dim stc As Integer = View.SelectedTriangles.Count - 1
        Dim stv As Integer = View.SelectedVertices.Count - 1
        For trii1 As Integer = 0 To stc
            Dim tri1 As Triangle = View.SelectedTriangles(trii1)
            For verti1 As Integer = 0 To stv
                Dim vert1 As Vertex = View.SelectedVertices(verti1)
                If vert1.linkedTriangles.Count > 0 Then
                    If CSG.isVertexInTriangle(vert1, tri1) Then
                        View.CollisionVertices.Add(vert1)
                    Else
                        If vert1.vertexID <> tri1.vertexA.vertexID AndAlso
                           vert1.vertexID <> tri1.vertexB.vertexID AndAlso
                           vert1.vertexID <> tri1.vertexC.vertexID Then
                            If vert1.X <= Math.Max(Math.Max(tri1.vertexA.X, tri1.vertexB.X), tri1.vertexC.X) AndAlso
                               vert1.Y <= Math.Max(Math.Max(tri1.vertexA.Y, tri1.vertexB.Y), tri1.vertexC.Y) AndAlso
                               vert1.X >= Math.Min(Math.Min(tri1.vertexA.X, tri1.vertexB.X), tri1.vertexC.X) AndAlso
                               vert1.Y >= Math.Min(Math.Min(tri1.vertexA.Y, tri1.vertexB.Y), tri1.vertexC.Y) Then
                                If CSG.distanceSquareFromVertexToLine(vert1, tri1.vertexA, tri1.vertexB) < 10.0 OrElse
                                   CSG.distanceSquareFromVertexToLine(vert1, tri1.vertexA, tri1.vertexC) < 10.0 OrElse
                                   CSG.distanceSquareFromVertexToLine(vert1, tri1.vertexB, tri1.vertexC) < 10.0 Then
                                    View.CollisionVertices.Add(vert1)
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        Next
        For trii1 As Integer = 0 To stc
            Dim tri1 As Triangle = View.SelectedTriangles(trii1)
            For trii2 As Integer = trii1 + 1 To stc
                Dim tri2 As Triangle = View.SelectedTriangles(trii2)
                CSG.trianglesIntersectionsOnly(tri1, tri2)
            Next
        Next
        Dim endVertexCount As Integer = LPCFile.Vertices.Count
        For i As Integer = startVertexCount To endVertexCount - 1
            View.CollisionVertices.Add(LPCFile.Vertices(startVertexCount))
            LPCFile.Vertices.RemoveAt(startVertexCount)
        Next

        BtnNextOverlap.Visible = View.CollisionVertices.Count > 0
        BtnPrevOverlap.Visible = BtnNextOverlap.Visible
        DetectOverlapsToolStripMenuItem.Checked = BtnNextOverlap.Visible
        Helper_2D.clearSelection()
    End Sub

    Private Sub BtnPrevOverlap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrevOverlap.Click
        MainState.collisionIndex -= 1
        If MainState.collisionIndex <= -1 Then MainState.collisionIndex = View.CollisionVertices.Count - 1
        Dim vert As Vertex = View.CollisionVertices(MainState.collisionIndex)
        View.offsetX = vert.X
        View.offsetY = -vert.Y
        Me.Refresh()
    End Sub

    Private Sub BtnNextOverlap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnNextOverlap.Click
        MainState.collisionIndex += 1
        If MainState.collisionIndex >= View.CollisionVertices.Count Then MainState.collisionIndex = 0
        Dim vert As Vertex = View.CollisionVertices(MainState.collisionIndex)
        View.offsetX = vert.X
        View.offsetY = -vert.Y
        Me.Refresh()
    End Sub

    Private Sub ReplaceColoursOnExportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReplaceColoursOnExportToolStripMenuItem.Click
        ReplacerDialog.ShowDialog()
    End Sub

    Private Sub loadLanguage(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs)
        LDSettings.Editor.myLanguage = sender.ToolTipText
        saveConfig()
        loadLanguage(LDSettings.Editor.myLanguage)
        loadConfig()
        Me.Refresh()
    End Sub

    Public Function translateUnit(ByVal unit As String) As String
        Select Case View.unit
            Case "LDU" : Return I18N.trl8(I18N.lk.LDU)
            Case "mm" : Return I18N.trl8(I18N.lk.mm)
        End Select
        Return I18N.trl8(I18N.lk.Inch)
    End Function

    Private Sub loadLanguage(ByVal lang As String, Optional ByVal alreadyFailedToLoad As Boolean = False)
        Try
            I18N.trl8.Clear()
            Dim f As Font
            Dim bf As Font
            Dim bi As Font
            Dim wordCount As Short
            Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(lang, New System.Text.UTF8Encoding(False))
                DateiIn.ReadLine()
                Dim fontLine() As String = DateiIn.ReadLine().Split(";")
                f = New Font(fontLine(0), Val(fontLine(1)), FontStyle.Regular, GraphicsUnit.Pixel)
                bf = New Font(fontLine(0), Val(fontLine(1)), FontStyle.Bold, GraphicsUnit.Pixel)
                bi = New Font(fontLine(0), Val(fontLine(1)), FontStyle.Italic, GraphicsUnit.Pixel)
                DateiIn.ReadLine()
                ' Setting the fonts of all dialogs and forms, except StatusDialog (because I instantiate the dialog)               
                ChooseTemplateDialog.Font = f
                ColourForm.Font = f
                ImportDialog.Font = f
                Me.Font = f
                MetadataDialog.Font = f
                OptionsDialog.Font = f
                ReplacerDialog.Font = f
                RingDialog.Font = f
                SetKeyDialog.Font = f
                TemplateEditor.Font = f
                ZoomDialog.Font = f
                Do
                    Dim line As String = DateiIn.ReadLine()
                    Dim transLine() As String = line.Split(";")
                    Dim key As Short = I18N.stringToShort(transLine(0))
                    If key > -1 Then
                        I18N.trl8.Add(key, I18N.parse(transLine(1)))
                    End If
                Loop Until DateiIn.EndOfStream
                Dim tlist As New List(Of String)
                tlist.AddRange([Enum].GetNames(GetType(I18N.lk)))
                wordCount = tlist.Count
            End Using

            For i As Short = 0 To wordCount - 1
                If Not I18N.trl8.ContainsKey(i) Then
                    I18N.trl8.Add(i, "NOT TRANSLATED")
                End If
            Next

            Me.CBProject.Items.Clear()
            Me.CBProject.Items.Add(I18N.trl8(I18N.lk.Right))
            Me.CBProject.Items.Add(I18N.trl8(I18N.lk.Top))
            Me.CBProject.Items.Add(I18N.trl8(I18N.lk.Front))
            Me.CBProject.Items.Add(I18N.trl8(I18N.lk.Left))
            Me.CBProject.Items.Add(I18N.trl8(I18N.lk.Bottom))
            Me.CBProject.Items.Add(I18N.trl8(I18N.lk.Back))

            ' Menu Captions
            Me.FileToolStripMenuItem.Text = I18N.trl8(I18N.lk.MenuFile)
            Me.FileToolStripMenuItem.Font = f
            Me.EditToolStripMenuItem.Text = I18N.trl8(I18N.lk.MenuEdit)
            Me.EditToolStripMenuItem.Font = f
            ViewToolStripMenuItem.Text = I18N.trl8(I18N.lk.MenuView)
            ViewToolStripMenuItem.Font = f
            Me.OptionsToolStripMenuItem.Text = I18N.trl8(I18N.lk.MenuOptions)
            Me.OptionsToolStripMenuItem.Font = f
            Me.LanguageToolStripMenuItem.Text = I18N.trl8(I18N.lk.MenuLanguage)
            Me.LanguageToolStripMenuItem.Font = f
            Me.HelpToolStripMenuItem.Text = I18N.trl8(I18N.lk.MenuHelp)
            Me.HelpToolStripMenuItem.Font = f
            ' File Menu
            Me.NewPatternToolStripMenuItem.Text = I18N.trl8(I18N.lk.NewFile)
            Me.NewPatternToolStripMenuItem.Font = f
            Me.LoadPatternToolStripMenuItem.Text = I18N.trl8(I18N.lk.Load)
            Me.LoadPatternToolStripMenuItem.Font = f
            Me.SaveToolStripMenuItem.Text = I18N.trl8(I18N.lk.Save)
            Me.SaveToolStripMenuItem.Font = f
            Me.SaveAsToolStripMenuItem.Text = I18N.trl8(I18N.lk.SaveAs)
            Me.SaveAsToolStripMenuItem.Font = f
            Me.TemplateToolStripMenuItem.Text = I18N.trl8(I18N.lk.LoadTemplate)
            Me.TemplateToolStripMenuItem.Font = f
            Me.UnloadTemplateDataToolStripMenuItem.Text = I18N.trl8(I18N.lk.DeleteTemplateData)
            Me.UnloadTemplateDataToolStripMenuItem.Font = f
            Me.ProjectionDataTemplateToolStripMenuItem.Text = I18N.trl8(I18N.lk.ConvertProjection)
            Me.ProjectionDataTemplateToolStripMenuItem.Font = f
            For i As Integer = 2 To Me.TemplateToolStripMenuItem.DropDownItems.Count - 1
                Me.TemplateToolStripMenuItem.DropDownItems(i).Font = f
            Next
            Me.CreateATemplateToolStripMenuItem.Text = I18N.trl8(I18N.lk.CreateTemplate)
            Me.CreateATemplateToolStripMenuItem.Font = f
            Me.EditTemplateToolStripMenuItem.Text = I18N.trl8(I18N.lk.EditTemplate)
            Me.EditTemplateToolStripMenuItem.Font = f
            Me.LoadBackgroundImageToolStripMenuItem.Text = I18N.trl8(I18N.lk.LoadBGImage)
            Me.LoadBackgroundImageToolStripMenuItem.Font = f
            Me.AdjustBackgroundImageToolStripMenuItem.Text = I18N.trl8(I18N.lk.AdjustBGImage)
            Me.AdjustBackgroundImageToolStripMenuItem.Font = f
            Me.ImportToolStripMenuItem.Text = I18N.trl8(I18N.lk.ImportDAT)
            Me.ImportToolStripMenuItem.Font = f
            Me.ExportToolStripMenuItem.Text = I18N.trl8(I18N.lk.ExportDAT)
            Me.ExportToolStripMenuItem.Font = f
            Me.ImportToolbarColoursToolStripMenuItem.Text = I18N.trl8(I18N.lk.ImportColours)
            Me.ImportToolbarColoursToolStripMenuItem.Font = f
            Me.ExportToolbarColoursToolStripMenuItem.Text = I18N.trl8(I18N.lk.ExportColours)
            Me.ExportToolbarColoursToolStripMenuItem.Font = f
            Me.CreateAStickerToolStripMenuItem.Text = I18N.trl8(I18N.lk.StickerGenerator)
            Me.CreateAStickerToolStripMenuItem.Font = f
            Me.ProjectOnXYPlane2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXYBack)
            Me.ProjectOnXYPlane2ToolStripMenuItem.Font = f
            Me.ProjectOnXYPlaneToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXYFront)
            Me.ProjectOnXYPlaneToolStripMenuItem.Font = f
            Me.ProjectOnYZPlane2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnYZLeft)
            Me.ProjectOnYZPlane2ToolStripMenuItem.Font = f
            Me.ProjectOnYZPlaneToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnYZRight)
            Me.ProjectOnYZPlaneToolStripMenuItem.Font = f
            Me.ProjectOnZXPlane2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXZBottom)
            Me.ProjectOnZXPlane2ToolStripMenuItem.Font = f
            Me.ProjectOnZXPlaneToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXZTop)
            Me.ProjectOnZXPlaneToolStripMenuItem.Font = f
            Me.EProjectOnXYPlane2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXYBack)
            Me.EProjectOnXYPlane2ToolStripMenuItem.Font = f
            Me.EProjectOnXYPlaneToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXYFront)
            Me.EProjectOnXYPlaneToolStripMenuItem.Font = f
            Me.EProjectOnYZPlane2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnYZLeft)
            Me.EProjectOnYZPlane2ToolStripMenuItem.Font = f
            Me.EProjectOnYZPlaneToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnYZRight)
            Me.EProjectOnYZPlaneToolStripMenuItem.Font = f
            Me.EProjectOnZXPlane2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXZBottom)
            Me.EProjectOnZXPlane2ToolStripMenuItem.Font = f
            Me.EProjectOnZXPlaneToolStripMenuItem.Text = I18N.trl8(I18N.lk.ProjectOnXZTop)
            Me.EProjectOnZXPlaneToolStripMenuItem.Font = f
            Me.ReplaceColoursOnExportToolStripMenuItem.Text = I18N.trl8(I18N.lk.ReplaceColours)
            Me.ReplaceColoursOnExportToolStripMenuItem.Font = f
            Me.MetadataToolStripMenuItem.Text = I18N.trl8(I18N.lk.Metadata)
            Me.MetadataToolStripMenuItem.Font = f
            Me.ExitToolStripMenuItem.Text = I18N.trl8(I18N.lk.ExitApp)
            Me.ExitToolStripMenuItem.Font = f
            ' Edit Menu
            Me.UndoToolStripMenuItem.Text = I18N.trl8(I18N.lk.Undo)
            Me.UndoToolStripMenuItem.Font = f
            Me.RedoToolStripMenuItem.Text = I18N.trl8(I18N.lk.Redo)
            Me.RedoToolStripMenuItem.Font = f
            Me.BtnUndo.Text = I18N.trl8(I18N.lk.Undo)
            Me.BtnRedo.Text = I18N.trl8(I18N.lk.Redo)
            Me.CutToolStripMenuItem.Text = I18N.trl8(I18N.lk.Cut)
            Me.CutToolStripMenuItem.Font = f
            Me.CopyToolStripMenuItem.Text = I18N.trl8(I18N.lk.Copy)
            Me.CopyToolStripMenuItem.Font = f
            Me.PasteToolStripMenuItem.Text = I18N.trl8(I18N.lk.Paste)
            Me.PasteToolStripMenuItem.Font = f
            Me.DeleteToolStripMenuItem.Text = I18N.trl8(I18N.lk.Delete)
            Me.DeleteToolStripMenuItem.Font = f
            Me.SelectAllToolStripMenuItem.Text = I18N.trl8(I18N.lk.SelectAll)
            Me.SelectAllToolStripMenuItem.Font = f
            Me.SelectSameColourToolStripMenuItem.Text = I18N.trl8(I18N.lk.SelectSame)
            Me.SelectSameColourToolStripMenuItem.Font = f
            Me.SelectConnectedToolStripMenuItem.Text = I18N.trl8(I18N.lk.SelectConnected)
            Me.SelectConnectedToolStripMenuItem.Font = f
            Me.SelectTouchingToolStripMenuItem.Text = I18N.trl8(I18N.lk.SelectTouching)
            Me.SelectTouchingToolStripMenuItem.Font = f
            Me.WithColourToolStripMenuItem.Text = I18N.trl8(I18N.lk.PlusSameColour)
            Me.WithColourToolStripMenuItem.Font = f
            Me.DetectOverlapsToolStripMenuItem.Text = I18N.trl8(I18N.lk.DetectOverlaps)
            Me.DetectOverlapsToolStripMenuItem.Font = f
            Me.FastTriangulationIIToolStripMenuItem.Text = I18N.trl8(I18N.lk.FastTriangulation)
            Me.FastTriangulationIIToolStripMenuItem.Font = f
            ' View Menu
            Me.ResetViewToolStripMenuItem.Text = I18N.trl8(I18N.lk.ResetView)
            Me.ResetViewToolStripMenuItem.Font = f
            Me.ShowGridToolStripMenuItem.Text = I18N.trl8(I18N.lk.ShowGrid)
            Me.ShowGridToolStripMenuItem.Font = f
            Me.ShowAxisLabelToolStripMenuItem.Text = I18N.trl8(I18N.lk.ShowAxis)
            Me.ShowAxisLabelToolStripMenuItem.Font = f
            Me.ShowBackgroundImageToolStripMenuItem.Text = I18N.trl8(I18N.lk.ShowImage)
            Me.ShowBackgroundImageToolStripMenuItem.Font = f
            Me.ImageToolStripMenuItem.Text = I18N.trl8(I18N.lk.Image)
            Me.ImageToolStripMenuItem.Font = f
            ViewPrefsToolStripMenuItem.Text = I18N.trl8(I18N.lk.ViewPreferences)
            ViewPrefsToolStripMenuItem.Font = f
            Me.ZoomToolStripMenuItem.Text = I18N.trl8(I18N.lk.Zoom)
            Me.ZoomToolStripMenuItem.Font = f
            Me.UnitToolStripMenuItem.Text = I18N.trl8(I18N.lk.Unit)
            Me.UnitToolStripMenuItem.Font = f
            ' Options Menu
            Me.ShowAllWarningsToolStripMenuItem.Text = I18N.trl8(I18N.lk.AllWarnings)
            Me.ShowAllWarningsToolStripMenuItem.Font = f
            Me.ShowAllWarningsToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.AllWarningsTip)
            Me.SetPathToolStripMenuItem.Text = I18N.trl8(I18N.lk.ResetLDraw)
            Me.SetPathToolStripMenuItem.Font = f
            Me.AdvancedOptionsToolStripMenuItem.Text = I18N.trl8(I18N.lk.AdvancedOpt)
            Me.AdvancedOptionsToolStripMenuItem.Font = f
            Me.AdvancedOptionsToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.AdvancedOptTip)
            ' Help Menu
            Me.AboutToolStripMenuItem.Text = I18N.trl8(I18N.lk.About)
            Me.AboutToolStripMenuItem.Font = f
            'Me.UpdateToolStripMenuItem.Text = I18N.trl8(I18N.lk.Update)
            'Me.UpdateToolStripMenuItem.Font = f
            ' Actions
            Me.BtnSelect.Text = I18N.trl8(I18N.lk.DoSelect)
            Me.BtnSelect.Font = f
            Me.BtnMove.Text = I18N.trl8(I18N.lk.Move)
            Me.BtnMove.Font = f
            Me.BtnRotate.Text = I18N.trl8(I18N.lk.Rotate)
            Me.BtnRotate.Font = f
            Me.BtnScale.Text = I18N.trl8(I18N.lk.Scale)
            Me.BtnScale.Font = f
            Me.BtnAddVertex.Text = I18N.trl8(I18N.lk.AddVertex)
            Me.BtnAddVertex.Font = f
            Me.BtnAddTriangle.Text = I18N.trl8(I18N.lk.AddTriangle)
            Me.BtnAddTriangle.Font = f
            Me.BtnAddReferenceLine.Text = I18N.trl8(I18N.lk.AddReferenceLine)
            Me.BtnAddReferenceLine.Font = f
            Me.BtnTriangleAutoCompletion.Text = I18N.trl8(I18N.lk.TriangleAutoCompletion)
            Me.BtnTriangleAutoCompletion.Font = f
            Me.BtnCut.Text = I18N.trl8(I18N.lk.Tcut)
            Me.BtnCut.Font = f
            Me.BtnCopy.Text = I18N.trl8(I18N.lk.Tcopy)
            Me.BtnCopy.Font = f
            Me.BtnPaste.Text = I18N.trl8(I18N.lk.Tpaste)
            Me.BtnPaste.Font = f
            Me.BtnAddToGroup.Text = I18N.trl8(I18N.lk.Group)
            Me.BtnAddToGroup.Font = f
            Me.BtnUngroup.Text = I18N.trl8(I18N.lk.Ungroup)
            Me.BtnUngroup.Font = f
            Me.BtnColours.Text = I18N.trl8(I18N.lk.ShowColours)
            Me.BtnColours.Font = f
            ' Modes
            Me.TrianglesModeToolStripMenuItem.Text = I18N.trl8(I18N.lk.TriangleMode)
            Me.TrianglesModeToolStripMenuItem.Font = f
            Me.VerticesModeToolStripMenuItem.Text = I18N.trl8(I18N.lk.VertexMode)
            Me.VerticesModeToolStripMenuItem.Font = f
            Me.PrimitiveModeToolStripMenuItem.Text = I18N.trl8(I18N.lk.PrimitiveMode)
            Me.PrimitiveModeToolStripMenuItem.Font = f
            Me.ReferenceLineModeToolStripMenuItem.Text = I18N.trl8(I18N.lk.ReferenceLineMode)
            Me.ReferenceLineModeToolStripMenuItem.Font = f
            Me.BtnMode.Font = f
            If MainState.objectToModify = Modified.Triangle Then Me.BtnMode.Text = I18N.trl8(I18N.lk.TriangleMode)
            If MainState.objectToModify = Modified.Vertex Then Me.BtnMode.Text = I18N.trl8(I18N.lk.VertexMode)
            If MainState.objectToModify = Modified.Primitive Then Me.BtnMode.Text = I18N.trl8(I18N.lk.PrimitiveMode)
            If MainState.objectToModify = Modified.HelperLine Then Me.BtnMode.Text = I18N.trl8(I18N.lk.ReferenceLineMode)
            ' Mirror
            Me.BtnMirror.Text = I18N.trl8(I18N.lk.Mirror)
            Me.BtnMirror.Font = f
            Me.MirrorXToolStripMenuItem.Text = I18N.trl8(I18N.lk.OnX)
            Me.MirrorXToolStripMenuItem.Font = f
            Me.MirrorYToolStripMenuItem.Text = I18N.trl8(I18N.lk.OnY)
            Me.MirrorYToolStripMenuItem.Font = f
            Me.MirrorXLeftToolStripMenuItem.Text = I18N.trl8(I18N.lk.OnXLeft)
            Me.MirrorXLeftToolStripMenuItem.Font = f
            Me.MirrorXRightToolStripMenuItem.Text = I18N.trl8(I18N.lk.OnXRight)
            Me.MirrorXRightToolStripMenuItem.Font = f
            Me.MirrorYTopToolStripMenuItem.Text = I18N.trl8(I18N.lk.OnYTop)
            Me.MirrorYTopToolStripMenuItem.Font = f
            Me.MirrorYBottomToolStripMenuItem.Text = I18N.trl8(I18N.lk.OnYBottom)
            Me.MirrorYBottomToolStripMenuItem.Font = f
            ' CSG
            Me.BtnCSG.Text = I18N.trl8(I18N.lk.CSG)
            Me.BtnCSG.Font = Font
            Me.BtnCSG.ToolTipText = I18N.trl8(I18N.lk.CSGTip)
            Me.CSGUnionToolStripMenuItem.Text = I18N.trl8(I18N.lk.CSGUnion)
            Me.CSGUnionToolStripMenuItem.Font = f
            Me.CSGUnionToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.CSGUnionTip)
            Me.CSGSubdivideToolStripMenuItem.Text = I18N.trl8(I18N.lk.CSGSubdivide)
            Me.CSGSubdivideToolStripMenuItem.Font = f
            Me.CSGRotateToolStripMenuItem.Text = I18N.trl8(I18N.lk.CSGRotate)
            Me.CSGRotateToolStripMenuItem.Font = f
            Me.CSGIntersectionPointsToolStripMenuItem.Text = I18N.trl8(I18N.lk.CSGIntersect)
            Me.CSGIntersectionPointsToolStripMenuItem.Font = f
            Me.CSGIntersectionPointsToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.CSGIntersectTip)
            ' Merge / Split
            Me.BtnMerge.Text = I18N.trl8(I18N.lk.MergeSplit)
            Me.BtnMerge.ToolTipText = Me.BtnMerge.Text
            Me.BtnMerge.Font = f
            Me.ToAverageToolStripMenuItem.Text = I18N.trl8(I18N.lk.ToAverage)
            Me.ToAverageToolStripMenuItem.Font = f
            Me.ToAverageXToolStripMenuItem.Text = I18N.trl8(I18N.lk.ToAverageX)
            Me.ToAverageXToolStripMenuItem.Font = f
            Me.ToAverageYToolStripMenuItem.Text = I18N.trl8(I18N.lk.ToAverageY)
            Me.ToAverageYToolStripMenuItem.Font = f
            Me.ToAverageToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.ToAverageTip)
            Me.ToLastSelectedToolStripMenuItem.Text = I18N.trl8(I18N.lk.ToLastSelected)
            Me.ToLastSelectedToolStripMenuItem.Font = f
            Me.ToLastSelectedToolStripMenuItem.ToolTipText = I18N.trl8(I18N.lk.ToLastSelectedTip)
            Me.MergeToNearestPrimvertexToolStripMenuItem.Text = I18N.trl8(I18N.lk.ToNearestPrim)
            Me.MergeToNearestPrimvertexToolStripMenuItem.Font = f
            Me.MergeToNearestTemplateLineToolStripMenuItem.Text = I18N.trl8(I18N.lk.ToNearestLineTemplate)
            Me.MergeToNearestTemplateLineToolStripMenuItem.Font = f
            Me.MergeToNearestTriangleLineToolStripMenuItem.Text = I18N.trl8(I18N.lk.ToNearestLineTriangle)
            Me.MergeToNearestTriangleLineToolStripMenuItem.Font = f
            Me.CSGSplitToolStripMenuItem.Text = I18N.trl8(I18N.lk.Split)
            Me.CSGSplitToolStripMenuItem.Font = f
            Me.ButtonRemoveIsolatedVertices.Text = I18N.trl8(I18N.lk.RemoveIsolated)
            ButtonRemoveIsolatedVertices.ToolTipText = I18N.trl8(I18N.lk.RemoveIsolated)
            Me.ButtonRemoveIsolatedVertices.Font = f
            ' LPCFile.Primitives
            Me.BtnPrimitives.Text = I18N.trl8(I18N.lk.AddPrimitive)
            Me.BtnPrimitives.ToolTipText = Me.BtnPrimitives.Text
            Me.BtnPrimitives.Font = f
            Me.HollowCircleToolStripMenuItem.Text = I18N.trl8(I18N.lk.Hollow)
            Me.HollowCircleToolStripMenuItem.Font = f
            Me.HollowOvalToolStripMenuItem.Text = I18N.trl8(I18N.lk.Hollow)
            Me.HollowOvalToolStripMenuItem.Font = f
            Me.HollowRectangleToolStripMenuItem.Text = I18N.trl8(I18N.lk.Hollow)
            Me.HollowRectangleToolStripMenuItem.Font = f
            Me.HollowTriangleToolStripMenuItem.Text = I18N.trl8(I18N.lk.Hollow)
            Me.HollowTriangleToolStripMenuItem.Font = f
            Me.SolidCircleToolStripMenuItem.Text = I18N.trl8(I18N.lk.Solid)
            Me.SolidCircleToolStripMenuItem.Font = f
            Me.SolidOvalToolStripMenuItem.Text = I18N.trl8(I18N.lk.Solid)
            Me.SolidOvalToolStripMenuItem.Font = f
            Me.SolidRectangleToolStripMenuItem.Text = I18N.trl8(I18N.lk.Solid)
            Me.SolidRectangleToolStripMenuItem.Font = f
            Me.SolidTriangleToolStripMenuItem.Text = I18N.trl8(I18N.lk.Solid)
            Me.SolidTriangleToolStripMenuItem.Font = f
            Me.CircleWithFrameToolStripMenuItem.Text = I18N.trl8(I18N.lk.Frame)
            Me.CircleWithFrameToolStripMenuItem.Font = f
            Me.OvalWithFrameToolStripMenuItem.Text = I18N.trl8(I18N.lk.Frame)
            Me.OvalWithFrameToolStripMenuItem.Font = f
            Me.RectangleWithFrameToolStripMenuItem.Text = I18N.trl8(I18N.lk.Frame)
            Me.RectangleWithFrameToolStripMenuItem.Font = f
            Me.TriangleWithFrameToolStripMenuItem.Text = I18N.trl8(I18N.lk.Frame)
            Me.TriangleWithFrameToolStripMenuItem.Font = f
            Me.SegmentsToolStripMenuItem.Text = I18N.trl8(I18N.lk.Segments)
            Me.SegmentsToolStripMenuItem.Font = f
            Me.Segments2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.Segments)
            Me.Segments2ToolStripMenuItem.Font = f
            Me.RadiusToolStripMenuItem.Text = I18N.trl8(I18N.lk.Radius)
            Me.RadiusToolStripMenuItem.Font = f
            Me.RadiusInnerToolStripMenuItem.Text = I18N.trl8(I18N.lk.InnerRadius)
            Me.RadiusInnerToolStripMenuItem.Font = f
            Me.Radius2ToolStripMenuItem.Text = I18N.trl8(I18N.lk.Width)
            Me.Radius2ToolStripMenuItem.Font = f
            Me.Radius3ToolStripMenuItem.Text = I18N.trl8(I18N.lk.Height)
            Me.Radius3ToolStripMenuItem.Font = f
            Me.TriangleToolStripMenuItem.Text = I18N.trl8(I18N.lk.LPCTriangle)
            Me.TriangleToolStripMenuItem.Font = f
            Me.RectangleToolStripMenuItem.Text = I18N.trl8(I18N.lk.LPCRectangle)
            Me.RectangleToolStripMenuItem.Font = f
            Me.CircleToolStripMenuItem.Text = I18N.trl8(I18N.lk.LPCCircle)
            Me.CircleToolStripMenuItem.Font = f
            Me.OvalToolStripMenuItem.Text = I18N.trl8(I18N.lk.LPCOval)
            Me.OvalToolStripMenuItem.Font = f
            Me.ChainToolStripMenuItem.Text = I18N.trl8(I18N.lk.LPCChain)
            Me.ChainToolStripMenuItem.Font = f
            Me.LDrawPrimitivesToolStripMenuItem.Text = I18N.trl8(I18N.lk.LDrawPrimitive)
            Me.LDrawPrimitivesToolStripMenuItem.Font = f
            Me.DiscToolStripMenuItem.Text = I18N.trl8(I18N.lk.CircDiscSector)
            Me.DiscToolStripMenuItem.Font = f
            Me.NDisToolStripMenuItem.Text = I18N.trl8(I18N.lk.InvCircDiscSector)
            Me.NDisToolStripMenuItem.Font = f
            Me.NDisTangToolStripMenuItem.Text = I18N.trl8(I18N.lk.InvTangCircDiscSector)
            Me.NDisTangToolStripMenuItem.Font = f
            Me.NDisTruncToolStripMenuItem.Text = I18N.trl8(I18N.lk.InvTruncCircDiscSector)
            Me.NDisTruncToolStripMenuItem.Font = f
            Me.CircularDiscSegmentToolStripMenuItem.Text = I18N.trl8(I18N.lk.CircDiscSegment)
            Me.CircularDiscSegmentToolStripMenuItem.Font = f
            Me.CircularRingSegmentToolStripMenuItem.Text = I18N.trl8(I18N.lk.CircRingSegment)
            Me.CircularRingSegmentToolStripMenuItem.Font = f
            Me.Disc48ToolStripMenuItem.Text = I18N.trl8(I18N.lk.CircDiscSector)
            Me.Disc48ToolStripMenuItem.Font = f
            Me.NDis48ToolStripMenuItem.Text = I18N.trl8(I18N.lk.InvCircDiscSector)
            Me.NDis48ToolStripMenuItem.Font = f
            Me.NDisTang48ToolStripMenuItem.Text = I18N.trl8(I18N.lk.InvTangCircDiscSector)
            Me.NDisTang48ToolStripMenuItem.Font = f
            Me.NDisTrunc48ToolStripMenuItem.Text = I18N.trl8(I18N.lk.InvTruncCircDiscSector)
            Me.NDisTrunc48ToolStripMenuItem.Font = f
            Me.CircularDiscSegment48ToolStripMenuItem.Text = I18N.trl8(I18N.lk.CircDiscSegment)
            Me.CircularDiscSegment48ToolStripMenuItem.Font = f
            Me.CircularRingSegment48ToolStripMenuItem.Text = I18N.trl8(I18N.lk.CircRingSegment)
            Me.CircularRingSegment48ToolStripMenuItem.Font = f
            Me.AdaptorRingToolStripMenuItem.Text = I18N.trl8(I18N.lk.AdaptorRing)
            Me.AdaptorRingToolStripMenuItem.Font = f
            Me.HighResToolStripMenuItem.Text = I18N.trl8(I18N.lk.HighRes)
            Me.HighResToolStripMenuItem.Font = f
            ' Units
            Me.LDUToolStripMenuItem.Text = I18N.trl8(I18N.lk.LDUnit)
            Me.LDUToolStripMenuItem.Font = f
            Me.MillimeterToolStripMenuItem.Text = I18N.trl8(I18N.lk.Millimetre)
            Me.MillimeterToolStripMenuItem.Font = f
            Me.InchToolStripMenuItem.Text = I18N.trl8(I18N.lk.InchUnit)
            Me.InchToolStripMenuItem.Font = f
            ' Colour Toolbar
            Dim tsb() As ToolStripButton = {C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15}
            For Each t As ToolStripButton In tsb
                toolStripTrl8(t)
            Next
            Me.C16.ToolTipText = I18N.trl8(I18N.lk.Colour) & " 16 [Num0 + Num1 + Num6]"
            Me.C16.Text = I18N.trl8(I18N.lk.Colour) & " 16"
            Me.C16.Font = f
            Me.BtnShowPalette.Text = I18N.trl8(I18N.lk.More)
            Me.BtnShowPalette.Font = f
            Me.BtnPipette.Text = I18N.trl8(I18N.lk.TakeColour)
            Me.BtnPipette.Font = f
            Me.BtnPreview.Text = I18N.trl8(I18N.lk.Preview)
            Me.OpacityToolStripDropDownButton.Text = I18N.trl8(I18N.lk.Opacity) & Mid(Me.OpacityToolStripDropDownButton.Text, Me.OpacityToolStripDropDownButton.Text.IndexOf(":") + 1)
            Me.OpacityToolStripDropDownButton.Font = f
            Me.O100ToolStripMenuItem.Font = f
            Me.O50ToolStripMenuItem.Font = f
            Me.O25ToolStripMenuItem.Font = f
            Me.O100ToolStripMenuItem.Font = f
            ' Context Menu
            Me.CMSAction.Text = I18N.trl8(I18N.lk.CMSActions)
            Me.CMSAction.Font = f
            Me.CMSMode.Text = I18N.trl8(I18N.lk.CMSMode)
            Me.CMSMode.Font = f
            Me.CMSCut.Text = I18N.trl8(I18N.lk.Tcut)
            Me.CMSCut.Font = f
            Me.CMSCopy.Text = I18N.trl8(I18N.lk.Tcopy)
            Me.CMSCopy.Font = f
            Me.CMSPaste.Text = I18N.trl8(I18N.lk.Tpaste)
            Me.CMSPaste.Font = f
            Me.CMSDelete.Text = I18N.trl8(I18N.lk.Tdelete)
            Me.CMSDelete.Font = f
            Me.CMSAddVertex.Text = I18N.trl8(I18N.lk.AddVertex)
            Me.CMSAddVertex.Font = f
            Me.CMSAddTriangle.Text = I18N.trl8(I18N.lk.AddTriangle)
            Me.CMSAddTriangle.Font = f
            Me.CMSSelect.Text = I18N.trl8(I18N.lk.DoSelect)
            Me.CMSSelect.Font = f
            Me.CMSMove.Text = I18N.trl8(I18N.lk.Move)
            Me.CMSMove.Font = f
            Me.CMSRotate.Text = I18N.trl8(I18N.lk.Rotate)
            Me.CMSRotate.Font = f
            Me.CMSScale.Text = I18N.trl8(I18N.lk.Scale)
            Me.CMSScale.Font = f
            Me.CMSVertex.Text = I18N.trl8(I18N.lk.VertexMode)
            Me.CMSVertex.Font = f
            Me.CMSTriangle.Text = I18N.trl8(I18N.lk.TriangleMode)
            Me.CMSTriangle.Font = f
            Me.CMSPrimitive.Text = I18N.trl8(I18N.lk.PrimitiveMode)
            Me.CMSPrimitive.Font = f
            ' Matrix Manipulator
            Me.GBMatrix.Text = I18N.trl8(I18N.lk.TemplateMatrix)
            Me.GBMatrix.Font = f
            Me.BtnMatrixApply.Font = f
            Me.BtnMatrixApply.Text = I18N.trl8(I18N.lk.Apply)
            Me.LblUnit3.Text = translateUnit(View.unit)
            Me.LblUnit3.Font = f
            Me.LblUnit4.Text = translateUnit(View.unit)
            Me.LblUnit4.Font = f
            ' Double Vertex Data Manipulator
            Me.GBVertex.Text = I18N.trl8(I18N.lk.VertexData)
            Me.GBVertex.Font = f
            Me.LblVertexX.Text = I18N.trl8(I18N.lk.VertexX)
            Me.LblVertexX.Font = f
            Me.LblVertexY.Text = I18N.trl8(I18N.lk.VertexY)
            Me.LblVertexY.Font = f
            Me.LblUnit1.Text = translateUnit(View.unit)
            Me.LblUnit1.Font = f
            Me.LblUnit2.Text = translateUnit(View.unit)
            Me.LblUnit2.Font = f
            ' Image
            ImageForm.Text = I18N.trl8(I18N.lk.ImageTitle)
            ImageForm.Font = f
            ImageForm.LblImageFile.Text = I18N.trl8(I18N.lk.ImageFile)
            ImageForm.LblImageFile.Font = f
            ImageForm.LblImageOffsetX.Text = I18N.trl8(I18N.lk.OffsetX)
            ImageForm.LblImageOffsetX.Font = f
            ImageForm.LblImageOffsetY.Text = I18N.trl8(I18N.lk.OffsetY)
            ImageForm.LblImageOffsetY.Font = f
            ImageForm.LblImageScale.Text = I18N.trl8(I18N.lk.ImageScale)
            ImageForm.LblImageScale.Font = f
            ImageForm.BtnAdjust.Text = I18N.trl8(I18N.lk.AdjustImageBtn)
            ImageForm.BtnAdjust.Font = bf
            ImageForm.LblImageSize.Text = Math.Round(View.imgScale * View.backgroundPicture.Width / 1000 * View.unitFactor, 3) & " x " & vbCrLf & Math.Round(View.imgScale * View.backgroundPicture.Height / 1000 * View.unitFactor, 3) & translateUnit(View.unit)
            ImageForm.LblImageSize.Font = f
            ' View-Preferences
            PreferencesForm.Text = Replace(I18N.trl8(I18N.lk.ViewPreferences), "&", "")
            PreferencesForm.Font = f
            PreferencesForm.LblMoveSnap.Text = String.Format(I18N.trl8(I18N.lk.MoveSnap), translateUnit(View.unit))
            PreferencesForm.LblMoveSnap.Font = f
            PreferencesForm.LblRotateSnap.Text = I18N.trl8(I18N.lk.RotateSnap)
            PreferencesForm.LblRotateSnap.Font = f
            PreferencesForm.LblScaleSnap.Text = I18N.trl8(I18N.lk.ScaleSnap)
            PreferencesForm.LblScaleSnap.Font = f
            PreferencesForm.LblGridSize.Text = String.Format(I18N.trl8(I18N.lk.GridSize), translateUnit(View.unit))
            PreferencesForm.LblGridSize.Font = f
            ' Zoom Dialog
            ZoomDialog.Text = I18N.trl8(I18N.lk.ZoomTitle)
            ZoomDialog.Font = f
            ZoomDialog.LblUpperLeft.Text = I18N.trl8(I18N.lk.UpperLeft)
            ZoomDialog.LblLowerRight.Text = I18N.trl8(I18N.lk.LowerRight)
            ZoomDialog.LblZoom.Text = I18N.trl8(I18N.lk.ZoomParam)
            ZoomDialog.GBZoom.Text = I18N.trl8(I18N.lk.ZoomParam)
            ZoomDialog.GBViewport.Text = I18N.trl8(I18N.lk.Viewport)
            LblZoom.Text = I18N.trl8(I18N.lk.ZoomParam) & " " & CType(View.zoomfactor * 10000, Integer) / 100 & "%"
            LblZoom.Font = f
            LblCoords.Text = Math.Round(Math.Round(getXcoordinate(MouseHelper.getCursorpositionX()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000, 3) & " | " & Math.Round(Math.Round(getYcoordinate(MouseHelper.getCursorpositionY()) / View.moveSnap * View.unitFactor) * View.moveSnap / 1000, 3) & " " & translateUnit(View.unit)
            ZoomDialog.Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
            ZoomDialog.OK_Button.Text = I18N.trl8(I18N.lk.OK)
            ' Template Editor
            TemplateEditor.Text = I18N.trl8(I18N.lk.EditorTitle)
            TemplateEditor.Font = f
            TemplateEditor.GBData.Text = I18N.trl8(I18N.lk.DataGroup)
            TemplateEditor.LblTitle.Text = I18N.trl8(I18N.lk.TemplateTitle)
            TemplateEditor.LblProjectOn.Text = I18N.trl8(I18N.lk.ProjectOn)
            TemplateEditor.CBProjectMode.Items.Clear()
            TemplateEditor.CBProjectMode.Items.Add(I18N.trl8(I18N.lk.Right))
            TemplateEditor.CBProjectMode.Items.Add(I18N.trl8(I18N.lk.Top))
            TemplateEditor.CBProjectMode.Items.Add(I18N.trl8(I18N.lk.Front))
            TemplateEditor.CBProjectMode.Items.Add(I18N.trl8(I18N.lk.Left))
            TemplateEditor.CBProjectMode.Items.Add(I18N.trl8(I18N.lk.Bottom))
            TemplateEditor.CBProjectMode.Items.Add(I18N.trl8(I18N.lk.Back))
            TemplateEditor.LblOffset.Text = I18N.trl8(I18N.lk.TemplateOffset)
            TemplateEditor.LblTransMatrix.Text = I18N.trl8(I18N.lk.TemplateMatrix)
            TemplateEditor.LblPolyData.Text = I18N.trl8(I18N.lk.PolygonData)
            TemplateEditor.LblAdditionalLines.Text = I18N.trl8(I18N.lk.AdditionalLines)
            TemplateEditor.BtnExample.Text = I18N.trl8(I18N.lk.ShowExample)
            TemplateEditor.Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
            TemplateEditor.OK_Button.Text = I18N.trl8(I18N.lk.OK)
            ' Template Manager
            ChooseTemplateDialog.Text = I18N.trl8(I18N.lk.ManagerTitle)
            ChooseTemplateDialog.Font = f
            ChooseTemplateDialog.Edit_Button.Text = I18N.trl8(I18N.lk.Edit)
            ChooseTemplateDialog.Delete_Button.Text = I18N.trl8(I18N.lk.Tdelete)
            ChooseTemplateDialog.Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
            ChooseTemplateDialog.OK_Button.Text = I18N.trl8(I18N.lk.OK)
            ' Colour Palette
            ColourForm.Text = I18N.trl8(I18N.lk.ColourTitle)
            ColourForm.Font = f
            ' ColourForm.LblTip.Text = >> Not needed job did loadConfig()
            ColourForm.GBDirectColour.Text = I18N.trl8(I18N.lk.DirectColour) & vbCrLf & "(0x2RRGGBB):"
            ColourForm.BtnDirectColour.Text = I18N.trl8(I18N.lk.SetColour)
            ' Misc Dialogs
            Me.FBDLDrawDir.Description = I18N.trl8(I18N.lk.LDrawDir)
            Me.OpenBitmap.Title = I18N.trl8(I18N.lk.BitmapImport)
            Me.OpenBitmap.Filter = I18N.trl8(I18N.lk.ImageFiles) & " (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|" & I18N.trl8(I18N.lk.AllFiles) & " (*.*)|*.*"
            Me.OpenImage.Title = I18N.trl8(I18N.lk.OpenImage)
            Me.OpenImage.Filter = I18N.trl8(I18N.lk.ImageFiles) & " (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|" & I18N.trl8(I18N.lk.AllFiles) & " (*.*)|*.*"
            Me.LoadFile.Title = I18N.trl8(I18N.lk.OpenLPC)
            Me.LoadFile.Filter = "Pattern Creator 1.1-1.3 (ASCII/Unicode,*.lpc)|*.lpc|Pattern Creator 1.0 (ASCII,*.txt)|*.txt|" & I18N.trl8(I18N.lk.AllFiles) & " (*.*)|*.*"
            Me.SaveAs.Title = Replace(I18N.trl8(I18N.lk.SaveAs), "&", "")
            Me.SaveAs.Filter = "Pattern Creator 1.3 (Unicode,*.lpc)|*.lpc|Pattern Creator 1.0 (ASCII,*.txt)|*.txt|" & I18N.trl8(I18N.lk.AllFiles) & " (*.*)|*.*"
            Me.OpenDAT.Filter = I18N.trl8(I18N.lk.DATFiles) & "|*.dat"
            Me.BtnAbort.Font = bf
            Me.BtnAbort.Text = I18N.trl8(I18N.lk.ABORT) & " []" : KeyToSet.setKey(BtnAbort, LDSettings.Keys.Abort)
            With CenterDialog
                .Text = I18N.trl8(I18N.lk.Center)
                .Font = f
                .LblUnit1.Text = translateUnit(View.unit)
                .LblUnit2.Text = translateUnit(View.unit)
                .LblVertexX.Text = I18N.trl8(I18N.lk.VertexX)
                .LblVertexY.Text = I18N.trl8(I18N.lk.VertexY)
                .Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
                .OK_Button.Text = I18N.trl8(I18N.lk.OK)
            End With
            ' Import Dialog
            ImportDialog.Text = I18N.trl8(I18N.lk.ImportTitle)
            ImportDialog.Font = f
            ImportDialog.GBMode.Text = I18N.trl8(I18N.lk.ImportMode)
            ImportDialog.RBnew.Text = I18N.trl8(I18N.lk.ImportOverwrite)
            ImportDialog.RBappend.Text = I18N.trl8(I18N.lk.ImportAppend)
            ImportDialog.RBtemplate.Text = I18N.trl8(I18N.lk.ImportProjection)
            ImportDialog.Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
            ImportDialog.OK_Button.Text = I18N.trl8(I18N.lk.OK)
            ' Colour Replacer
            ReplacerDialog.Text = I18N.trl8(I18N.lk.ReplacerTitle)
            ReplacerDialog.Font = f
            ReplacerDialog.Label1.Text = I18N.trl8(I18N.lk.ReplacerTip)
            ReplacerDialog.GroupBox1.Text = I18N.trl8(I18N.lk.ReplacerOldColour)
            ReplacerDialog.GroupBox2.Text = I18N.trl8(I18N.lk.ReplacerNewColour)
            ReplacerDialog.BtnInsertRule.Text = I18N.trl8(I18N.lk.ReplacerInsertRule)
            ReplacerDialog.o1h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.o2h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.o3h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.o4h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.o5h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.o6h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.o7h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.o8h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n1h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n2h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n3h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n4h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n5h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n6h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n7h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.n8h.Text = I18N.trl8(I18N.lk.ReplacerPick)
            ReplacerDialog.Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
            ReplacerDialog.OK_Button.Text = I18N.trl8(I18N.lk.OK)
            ' Overlap Detector
            BtnPrevOverlap.Text = I18N.trl8(I18N.lk.PrevOverlap)
            BtnPrevOverlap.Font = bf
            BtnNextOverlap.Text = I18N.trl8(I18N.lk.NextOverlap)
            BtnNextOverlap.Font = bf
            ' Metadata Dialog
            With MetadataDialog
                .Text = I18N.trl8(I18N.lk.MetadataTitle)
                .Font = f
                .GBDatExport.Text = ""
                .GBPartTree.Text = I18N.trl8(I18N.lk.MPartTree)
                .GBPreview.Text = I18N.trl8(I18N.lk.MPreview)
                .CBPreview.Text = I18N.trl8(I18N.lk.MEnablePreview)
                .CBColourReplacer.Text = Replace(I18N.trl8(I18N.lk.ReplaceColours), "..", "")
                .BtnDefault.Text = I18N.trl8(I18N.lk.MSetAsDefault)
                .LblPartDescription.Text = I18N.trl8(I18N.lk.MPart)
                .LblFilename.Text = I18N.trl8(I18N.lk.MFile)
                .LblAuthor.Text = I18N.trl8(I18N.lk.MAuthor)
                .LblRealname.Text = I18N.trl8(I18N.lk.MRealName)
                .LblUsername.Text = I18N.trl8(I18N.lk.MUser)
                .LblParttype.Text = I18N.trl8(I18N.lk.MType)
                .LblLicense.Text = I18N.trl8(I18N.lk.MLicense)
                .LblHelp.Text = I18N.trl8(I18N.lk.MHelp)
                .LblBFC.Text = I18N.trl8(I18N.lk.MBFC)
                .LblCategory.Text = I18N.trl8(I18N.lk.MCategory)
                .LblKeywords.Text = I18N.trl8(I18N.lk.MKeywords)
                .LblHistory.Text = I18N.trl8(I18N.lk.MHistory)
                .LblComments.Text = I18N.trl8(I18N.lk.MComments)
                .CBInclude.Text = I18N.trl8(I18N.lk.MExport)
                .Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
                .OK_Button.Text = I18N.trl8(I18N.lk.OK)
            End With
            SetKeyDialog.Text = I18N.trl8(I18N.lk.KeyMsg)
            SetKeyDialog.Font = f
            SetKeyDialog.Cancel_Button.Text = I18N.trl8(I18N.lk.OK)
            With OptionsDialog
                .Text = I18N.trl8(I18N.lk.Options)
                .Font = f
                .Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
                .OK_Button.Text = I18N.trl8(I18N.lk.OK)
                .BtnRestoreDefaults.Text = I18N.trl8(I18N.lk.SRestore)
                ' Options Settings
                .TPSettings.Text = I18N.trl8(I18N.lk.SettingsTitle)
                .GBMeta.Text = I18N.trl8(I18N.lk.SMetaDefaults)
                .LblAuthor.Text = I18N.trl8(I18N.lk.MAuthor)
                .LblLicense.Text = I18N.trl8(I18N.lk.MLicense)
                .LblRealName.Text = I18N.trl8(I18N.lk.MRealName)
                .LblUser.Text = I18N.trl8(I18N.lk.MUser)
                .GBUndo.Text = I18N.trl8(I18N.lk.SUndoRedo)
                .LblMaxUndo.Text = I18N.trl8(I18N.lk.SMaxUndo)
                .GBStartup.Text = I18N.trl8(I18N.lk.SStartup)
                .CBFullscreen.Text = I18N.trl8(I18N.lk.SStartFullscreen)
                .CBViewImage.Text = I18N.trl8(I18N.lk.SImage)
                .CBViewPreferences.Text = I18N.trl8(I18N.lk.SPrefs)
                .CBAlternativeZoomAndTrans.Text = I18N.trl8(I18N.lk.SAlternative)
                .CBTemplateLinesTop.Text = I18N.trl8(I18N.lk.STemplateTop)
                .CBAddModeLock.Text = I18N.trl8(I18N.lk.SAddLock)
                ' Options Hotkeys
                .TPKeys.Text = I18N.trl8(I18N.lk.Hotkeys)
                .Shortkeys.Columns(0).HeaderText = I18N.trl8(I18N.lk.Tfunction)
                .Shortkeys.Columns(1).HeaderText = I18N.trl8(I18N.lk.KeyCombo)
                ' Options Colours
                .TPColours.Text = I18N.trl8(I18N.lk.Colours)
                .GBObject.Text = I18N.trl8(I18N.lk.Tobject)
                .GBObjectcolour.Text = I18N.trl8(I18N.lk.ObjectColour)
                .LblOld.Text = I18N.trl8(I18N.lk.Old)
                .LblNew.Text = I18N.trl8(I18N.lk.Tnew)
                .BtnCopy.Text = I18N.trl8(I18N.lk.Copy)
                .BtnPaste.Text = I18N.trl8(I18N.lk.Paste)
                .BtnApply.Text = I18N.trl8(I18N.lk.Apply)
            End With
            ' Primitive Ring Dialog
            With RingDialog
                .Text = I18N.trl8(I18N.lk.PRingTitle)
                .Font = f
                .CB48.Text = "48 " & Replace(I18N.trl8(I18N.lk.Segments), ":", "")
                .LblRadius.Text = I18N.trl8(I18N.lk.PRadius)
                .LblResolution.Text = I18N.trl8(I18N.lk.PResolution)
                .LblName.Text = I18N.trl8(I18N.lk.PName)
                .LblFullName.Text = I18N.trl8(I18N.lk.PFilename)
                .Cancel_Button.Text = I18N.trl8(I18N.lk.Cancel)
                .OK_Button.Text = I18N.trl8(I18N.lk.OK)
            End With
            ' Spliner
            SplineToolStripMenuItem.Text = I18N.trl8(I18N.lk.SplineMenu)
            SplineToolStripMenuItem.Font = f
            LblSplineSegs.Text = I18N.trl8(I18N.lk.Segments)
            LblSplineSegs.Font = f
        Catch
            If Not alreadyFailedToLoad Then
                loadConfig()
                LDSettings.Editor.myLanguage = EnvironmentPaths.appPath & "lang\lang_en_GB.csv"
                loadLanguage(LDSettings.Editor.myLanguage, True)
                saveConfig()
            End If
        End Try
    End Sub

    Private Sub toolStripTrl8(ByRef b As ToolStripButton, Optional ByVal saveSetting As Boolean = False, Optional ByVal path As String = "")
        If path = "" Then path = EnvironmentPaths.appPath & "Colours.txt"
        If b.Tag > -1 Then
            Dim hundred As Integer = CInt(b.Tag) \ 100
            Dim ten As Integer = (CInt(b.Tag) - hundred * 100) \ 10
            Dim one As Integer = CInt(b.Tag) - hundred * 100 - ten * 10
            b.ToolTipText = I18N.trl8(I18N.lk.Colour) & " " & b.Tag & " [Num" & hundred & " + Num" & ten & " + Num" & one & "]"
        Else
            b.ToolTipText = "0x2" & b.BackColor.R.ToString("X2") & b.BackColor.G.ToString("X2") & b.BackColor.B.ToString("X2")
        End If
        If saveSetting Then
            Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(path, False, New System.Text.UTF8Encoding(False))
                Dim tsb() As ToolStripButton = {C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15}
                For Each ref As ToolStripButton In tsb
                    If ref.Tag = -1 Then
                        DateiOut.WriteLine(ref.BackColor.ToArgb)
                        DateiOut.WriteLine("HEX")
                    Else
                        DateiOut.WriteLine(ref.Tag)
                        DateiOut.WriteLine("DEC")
                    End If
                Next
            End Using
        End If
    End Sub

    Private Sub BtnMatrixApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnMatrixApply.Click
        If View.SelectedTriangles.Count > 0 Then
            Dim group As Integer = View.SelectedTriangles(0).groupindex
            Dim prim As Primitive = LPCFile.Primitives(LPCFile.PrimitivesHMap(group))
            Dim transmatrix(,) As Double = CType(prim.matrix.Clone, Double(,))
            Dim transmatrix2(,) As Double = CType(prim.matrix.Clone, Double(,))
            Try
                transmatrix = Inverse(transmatrix)
                transmatrix2(0, 0) = NUDM11.Value
                transmatrix2(0, 1) = NUDM12.Value
                transmatrix2(0, 3) = NUDM13.Value * 1000.0 / View.unitFactor
                transmatrix2(1, 0) = NUDM21.Value
                transmatrix2(1, 1) = NUDM22.Value
                transmatrix2(1, 3) = NUDM23.Value * 1000.0 / View.unitFactor
                transmatrix2 = Inverse(transmatrix2)
            Catch
                GBMatrix.Visible = False
                Me.Refresh()
                Exit Sub
            End Try
            Dim tx, ty As Double
            Dim nx, ny As Double
            Dim m11, m12, m21, m22 As Double
            m11 = NUDM11.Value
            m12 = NUDM12.Value
            m21 = NUDM21.Value
            m22 = NUDM22.Value
            nx = NUDM13.Value * 1000.0 / View.unitFactor
            ny = NUDM23.Value * 1000.0 / View.unitFactor
            For Each vert As Vertex In View.SelectedVertices
                tx = transmatrix(0, 0) * vert.X + transmatrix(0, 1) * vert.Y + transmatrix(0, 3)
                ty = transmatrix(1, 0) * vert.X + transmatrix(1, 1) * vert.Y + transmatrix(1, 3)
                vert.X = tx
                vert.Y = ty
                tx = m11 * vert.X + m12 * vert.Y + nx
                ty = m21 * vert.X + m22 * vert.Y + ny
                vert.X = tx
                vert.Y = ty
            Next
            prim.matrix(0, 0) = m11
            prim.matrix(0, 1) = m12
            prim.matrix(0, 3) = nx
            prim.matrix(1, 0) = m21
            prim.matrix(1, 1) = m22
            prim.matrix(1, 3) = ny
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub BtnRound1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRound1.Click
        round(0)
    End Sub

    Private Sub BtnRound10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRound10.Click
        round(1)
    End Sub

    Private Sub BtnRound100_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRound100.Click
        round(2)
    End Sub

    Private Sub BtnRound1000_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRound1000.Click
        round(3)
    End Sub

    Private Sub round(ByVal decimalPlaces As Integer)
        For Each vert As Vertex In View.SelectedVertices
            vert.X = Fix(Math.Round(vert.X / 1000.0, decimalPlaces, MidpointRounding.AwayFromZero) * 1000.0)
            vert.Y = Fix(Math.Round(vert.Y / 1000.0, decimalPlaces, MidpointRounding.AwayFromZero) * 1000.0)
        Next
        UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Private Sub NUDM11_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM11.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDM11_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM11.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub NUDM12_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM12.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDM12_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM12.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub NUDM13_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM13.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDM13_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM13.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub NUDM21_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM21.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDM21_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM21.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub NUDM22_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM22.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDM22_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM22.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub NUDM23_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM23.GotFocus
        disableTextboxedit(False)
    End Sub

    Private Sub NUDM23_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDM23.LostFocus
        If BtnAddVertex.Checked Then
            DeleteToolStripMenuItem.Enabled = True
        Else
            disableTextboxedit(True)
        End If
    End Sub

    Private Sub BtnInvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnInvert.Click
        If View.SelectedTriangles.Count > 0 Then
            Dim p As Primitive = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedTriangles(0).groupindex))
            Dim m As New Matrix3D
            Dim mt As New Matrix3D
            For y As Integer = 0 To 3
                For x As Integer = 0 To 3
                    m.m(y, x) = p.matrix(y, x)
                Next
            Next
            mt.m(0, 0) = 1.0
            mt.m(1, 1) = 1.0
            mt.m(2, 2) = -1.0
            mt.m(3, 3) = 1.0
            m = m * mt
            For y As Integer = 0 To 3
                For x As Integer = 0 To 3
                    p.matrix(y, x) = m.m(y, x)
                Next
            Next
            If BtnInvert.BackColor = Color.Green Then
                BtnInvert.BackColor = Color.Red
            Else
                BtnInvert.BackColor = Color.Green
            End If
        End If
        Me.Refresh()
    End Sub

    Private Sub BtnBFC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBFC.Click
        Me.Refresh()
    End Sub

    Private Sub ImportToolbarColoursToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportToolbarColoursToolStripMenuItem.Click
        OpenColours.Title = I18N.trl8(I18N.lk.ImportColours) & ":"
        Try
            If OpenColours.ShowDialog = Windows.Forms.DialogResult.OK AndAlso My.Computer.FileSystem.FileExists(OpenColours.FileName) Then
                Using DateiIn As StreamReader = My.Computer.FileSystem.OpenTextFileReader(OpenColours.FileName, New System.Text.UTF8Encoding(False))
                    Dim c As Integer
                    Dim tsb() As ToolStripButton = {C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15}
                    For Each t As ToolStripButton In tsb
                        c = CInt(DateiIn.ReadLine())
                        If DateiIn.ReadLine() = "DEC" Then
                            t.BackColor = LDConfig.colourHMap(c)
                            t.Tag = c
                        Else
                            t.BackColor = Color.FromArgb(c)
                            t.Tag = -1
                        End If
                        toolStripTrl8(t)
                    Next
                End Using
                toolStripTrl8(Me.C0, True)
            End If
        Catch
        End Try
    End Sub

    Private Sub ExportToolbarColoursToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToolbarColoursToolStripMenuItem.Click
        SaveColours.Title = I18N.trl8(I18N.lk.ExportColours) & ":"
        Try
            If SaveColours.ShowDialog = Windows.Forms.DialogResult.OK AndAlso SaveColours.FileName <> "" Then
                toolStripTrl8(Me.C0, True, SaveColours.FileName)
            End If
        Catch
        End Try
    End Sub

    Private Sub CreateAStickerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateAStickerToolStripMenuItem.Click
        If StickerDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If MainState.unsavedChanges AndAlso ShowAllWarningsToolStripMenuItem.Checked Then
                Dim result As MsgBoxResult = MsgBox(I18N.trl8(I18N.lk.UnsavedChanges), MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.Information, I18N.trl8(I18N.lk.Question))
                If result = MsgBoxResult.Yes Then SaveToolStripMenuItem.PerformClick()
                If result = MsgBoxResult.Cancel Then Exit Sub
            End If
            newPattern()
            LPCFile.myMetadata.mData(0) = StickerDialog.LblAutoDescription.Text & StickerDialog.TBDescription.Text
            LPCFile.myMetadata.recommendedMode = 1
            Dim x As Integer = Math.Abs(StickerDialog.NUDWidth.Value / View.unitFactor * 500)
            Dim y As Integer = Math.Abs(StickerDialog.NUDHeight.Value / View.unitFactor * 500)
            LPCFile.myMetadata.matrix(1, 3) = -0.25
            LPCFile.myMetadata.additionalData = "1 16 0 -.25 0 " & x / 1000 & " 0 0 0 .25 0 0 0 " & y / 1000 & " box5-12.dat"
            LPCFile.Vertices.Add(New Vertex(x, y, False) With {.groupindex = Primitive.TEMPLATE_INDEX})
            LPCFile.Vertices.Add(New Vertex(-x, y, False) With {.groupindex = Primitive.TEMPLATE_INDEX})
            LPCFile.Vertices.Add(New Vertex(-x, -y, False) With {.groupindex = Primitive.TEMPLATE_INDEX})
            LPCFile.Vertices.Add(New Vertex(x, -y, False) With {.groupindex = Primitive.TEMPLATE_INDEX})
            LPCFile.templateShape.Add(New PointF(x, y))
            LPCFile.templateShape.Add(New PointF(-x, y))
            LPCFile.templateShape.Add(New PointF(-x, -y))
            LPCFile.templateShape.Add(New PointF(x, -y))
        End If
        Me.Refresh()
    End Sub

    Private Sub SearchTemplate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchTemplate.TextChanged
        Dim s As String = "*" & SearchTemplate.Text & "*"
        Dim ic As Integer = TemplateToolStripMenuItem.DropDownItems.Count - 1
        For i As Integer = 4 To ic
            Dim t As ToolStripItem = TemplateToolStripMenuItem.DropDownItems(i)
            t.Visible = t.Text Like s
        Next
    End Sub

    Private Sub BtnPrimitives_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrimitives.DropDownOpening
        If Not MainState.primitivesLoaded Then
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
                                If s2(0) = "D" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    DiscToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "N" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    NDisToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "T" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    NDisTangToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "S" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    CircularDiscSegmentToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "U" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    NDisTruncToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "D48" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    Disc48ToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "N48" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    NDis48ToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "T48" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    NDisTang48ToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "S48" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    CircularDiscSegment48ToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "U48" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    NDisTrunc48ToolStripMenuItem.DropDownItems.Add(tmi)
                                ElseIf s2(0) = "A48" Then
                                    Dim tmi As New ToolStripMenuItem
                                    tmi.Text = s2(1)
                                    AddHandler tmi.Click, AddressOf primitive_Click
                                    AdaptorRingToolStripMenuItem.DropDownItems.Add(tmi)
                                End If
                            End If
                        End If
                    Loop Until DateiIn.EndOfStream
                End Using
            End If
            MainState.primitivesLoaded = True
        End If
    End Sub

    Private Sub primitive_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs)
        MainState.primitiveObject = 255
        MainState.primitiveNewName = sender.Text
        startPrimitiveMode()
    End Sub

    Private Sub CBProject_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBProject.SelectedValueChanged
        Dim p As Primitive = LPCFile.Primitives(CType(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex), Integer))
        Select Case CBProject.SelectedItem
            Case I18N.trl8(I18N.lk.Right) ' YZ mit -X (Right) [OK]
                Dim cmatrix(,) As Double = {{0.0, 0.0, -1.0, 0.0},
                                            {0.0, 1.0, 0.0, 0.0},
                                            {1.0, 0.0, 0.0, 0.0},
                                            {0.0, 0.0, 0.0, 1.0}}
                p.matrixR = cmatrix.Clone
            Case I18N.trl8(I18N.lk.Top) ' XZ mit -Y (Top) [OK]
                Dim cmatrix(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                            {0.0, 0.0, -1.0, 0.0},
                                            {0.0, 1.0, 0.0, 0.0},
                                            {0.0, 0.0, 0.0, 1.0}}
                p.matrixR = cmatrix.Clone
            Case I18N.trl8(I18N.lk.Front) ' XY mit -Z (Front) [OK] 
                Dim cmatrix(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                            {0.0, 1.0, 0.0, 0.0},
                                            {0.0, 0.0, 1.0, 0.0},
                                            {0.0, 0.0, 0.0, 1.0}}
                p.matrixR = cmatrix.Clone
            Case I18N.trl8(I18N.lk.Left) ' YZ mit +X (Left) [OK]
                Dim cmatrix(,) As Double = {{0.0, 0.0, 1.0, 0.0},
                                            {0.0, 1.0, 0.0, 0.0},
                                            {-1.0, 0.0, 0.0, 0.0},
                                            {0.0, 0.0, 0.0, 1.0}}
                p.matrixR = cmatrix.Clone
            Case I18N.trl8(I18N.lk.Bottom) ' XZ mit -Y (Bottom) [OK]
                Dim cmatrix(,) As Double = {{1.0, 0.0, 0.0, 0.0},
                                            {0.0, 0.0, 1.0, 0.0},
                                            {0.0, -1.0, 0.0, 0.0},
                                            {0.0, 0.0, 0.0, 1.0}}
                p.matrixR = cmatrix.Clone
            Case I18N.trl8(I18N.lk.Back) ' XY mit +Z (Back) [OK]
                Dim cmatrix(,) As Double = {{-1.0, 0.0, 0.0, 0.0},
                                            {0.0, 1.0, 0.0, 0.0},
                                            {0.0, 0.0, -1.0, 0.0},
                                            {0.0, 0.0, 0.0, 1.0}}
                p.matrixR = cmatrix.Clone
        End Select
        SBZoom.Focus()
    End Sub

    Private Sub SplineToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplineToolStripMenuItem.Click
        startPrimitiveMode()
        MainState.primitiveMode = PrimitiveModes.SetSplineStartingPoint
        GBSpline.Visible = True
        NUDSplineSegs.Focus()
    End Sub

    Private Sub NUDSplineSegs_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NUDSplineSegs.ValueChanged
        If MainState.Splines.Count > 0 Then
            ListHelper.LLast(MainState.Splines).segmentCount = Fix(NUDSplineSegs.Value) - 1
            Me.Refresh()
        End If
    End Sub

    Private Sub BtnMatrixCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnMatrixCopy.Click
        Try
            If View.SelectedVertices.Count > 0 Then
                ClipboardHelper.ClipboardMatrix = LPCFile.Primitives(LPCFile.PrimitivesHMap(View.SelectedVertices(0).groupindex)).matrix.Clone
                BtnMatrixPaste.Enabled = True
            End If
        Catch ex As Exception
            BtnMatrixPaste.Enabled = False
        End Try
    End Sub

    Private Sub BtnMatrixPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnMatrixPaste.Click
        Try
            NUDM11.Value = ClipboardHelper.ClipboardMatrix(0, 0)
            NUDM12.Value = ClipboardHelper.ClipboardMatrix(0, 1)
            NUDM13.Value = ClipboardHelper.ClipboardMatrix(0, 3) / 1000.0 * View.unitFactor
            NUDM21.Value = ClipboardHelper.ClipboardMatrix(1, 0)
            NUDM22.Value = ClipboardHelper.ClipboardMatrix(1, 1)
            NUDM23.Value = ClipboardHelper.ClipboardMatrix(1, 3) / 1000.0 * View.unitFactor
            BtnMatrixApply.PerformClick()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub BtnVertexCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVertexCopy.Click
        Try
            If MainState.readOnlyVertex Is Nothing AndAlso View.SelectedVertices.Count = 1 Then
                ClipboardHelper.ClipboardVertex(0) = View.SelectedVertices(0).X
                ClipboardHelper.ClipboardVertex(1) = View.SelectedVertices(0).Y
            Else
                ClipboardHelper.ClipboardVertex(0) = MainState.readOnlyVertex.X
                ClipboardHelper.ClipboardVertex(1) = MainState.readOnlyVertex.Y
            End If
            BtnVertexPaste.Enabled = True
        Catch ex As Exception
            BtnVertexPaste.Enabled = False
        End Try
    End Sub

    Private Sub BtnVertexPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVertexPaste.Click
        Try
            If Not NUDVertX.ReadOnly AndAlso View.SelectedVertices.Count = 1 Then
                View.SelectedVertices(0).X = ClipboardHelper.ClipboardVertex(0)
                View.SelectedVertices(0).Y = ClipboardHelper.ClipboardVertex(1)
                NUDVertX.Value = ClipboardHelper.ClipboardVertex(0) * View.unitFactor / 1000
                NUDVertY.Value = ClipboardHelper.ClipboardVertex(1) * View.unitFactor / 1000
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MainForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Select Case Me.WindowState
            Case FormWindowState.Normal
                ' Form was restored
                If ColourForm.Visible Then
                    ColourForm.WindowState = FormWindowState.Normal
                End If
                If ImageForm.Visible Then
                    ImageForm.WindowState = FormWindowState.Normal
                End If
                If PreferencesForm.Visible Then
                    PreferencesForm.WindowState = FormWindowState.Normal
                End If
            Case FormWindowState.Minimized
                ' Form was minimized
                If ColourForm.Visible Then
                    ColourForm.WindowState = FormWindowState.Minimized
                End If
                If ImageForm.Visible Then
                    ImageForm.WindowState = FormWindowState.Minimized
                End If
                If PreferencesForm.Visible Then
                    PreferencesForm.WindowState = FormWindowState.Minimized
                End If
            Case FormWindowState.Maximized
                ' Form was maximized
                If ColourForm.Visible Then
                    ColourForm.WindowState = FormWindowState.Normal
                End If
                If ImageForm.Visible Then
                    ImageForm.WindowState = FormWindowState.Normal
                End If
                If PreferencesForm.Visible Then
                    PreferencesForm.WindowState = FormWindowState.Normal
                End If
        End Select
    End Sub

    Private Sub UnloadTemplateDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnloadTemplateDataToolStripMenuItem.Click
        LPCFile.myMetadata.additionalData = ""
        For ix As Integer = 0 To 3
            For iy As Integer = 0 To 3
                LPCFile.myMetadata.matrix(ix, iy) = 0.0
            Next
        Next
        For id As Integer = 0 To 3
            LPCFile.myMetadata.matrix(id, id) = 1.0
        Next
        LPCFile.myMetadata.recommendedMode = 6
        LPCFile.templateProjectionQuads.Clear()
        LPCFile.templateTexts.Clear()
        LPCFile.templateShape.Clear()
        For Each v As Vertex In LPCFile.Vertices
            If v.groupindex = Primitive.TEMPLATE_INDEX Then
                v.groupindex = Primitive.NO_INDEX
            End If
        Next
        UndoRedoHelper.addHistory()
        Me.Refresh()
    End Sub

    Private Sub ProjectionDataTemplateToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProjectionDataTemplateToolStripMenuItem.Click
        Dim shapeCount = LPCFile.templateShape.Count
        If shapeCount > 0 Then

            If LPCFile.templateShape(0).X = Single.Epsilon Then ' we have a projection data templateShape
                LPCFile.templateShape.RemoveAt(0) ' Remove the first point (it is the indicator for projection data
                shapeCount -= 5
                For i As Integer = shapeCount To 3 Step -4
                    LPCFile.templateShape.Insert(i, New PointF(Single.Epsilon, 0))
                Next
            End If

            Try
                Dim templateName As String = "_New_Template_"
                Dim templateNumber As Integer = 1
                While My.Computer.FileSystem.FileExists(EnvironmentPaths.appPath & "template\" & templateName & templateNumber & ".txt")
                    templateNumber += 1
                End While
                templateName = EnvironmentPaths.appPath & "template\" & templateName & templateNumber & ".txt"
                Using DateiOut As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(templateName, False, New System.Text.UTF8Encoding(False))
                    DateiOut.WriteLine(LPCFile.myMetadata.recommendedMode)
                    DateiOut.WriteLine(LPCFile.myMetadata.additionalData)
                    For r As Integer = 0 To 3
                        For c As Integer = 0 To 2
                            DateiOut.Write(I18N.globalize(LPCFile.myMetadata.matrix(r, c)) & " ")
                        Next
                        DateiOut.Write(I18N.globalize(LPCFile.myMetadata.matrix(r, 3)))
                        DateiOut.WriteLine()
                    Next

                    For Each p As PointF In LPCFile.templateShape
                        If p.X = Single.Epsilon Then
                            DateiOut.WriteLine("CUT")
                        Else
                            DateiOut.WriteLine(I18N.globalize(p.X / 1000.0F) & " " & I18N.globalize(p.Y / 1000.0F))
                        End If
                    Next

                    For Each q As ProjectionQuad In LPCFile.templateProjectionQuads
                        DateiOut.WriteLine("{" & I18N.globalize(q.inCoords(0, 0)) & " " & I18N.globalize(q.inCoords(0, 1)) _
                                         & "; " & I18N.globalize(q.inCoords(1, 0)) & " " & I18N.globalize(q.inCoords(1, 1)) _
                                         & "; " & I18N.globalize(q.inCoords(2, 0)) & " " & I18N.globalize(q.inCoords(2, 1)) _
                                         & "; " & I18N.globalize(q.inCoords(3, 0)) & " " & I18N.globalize(q.inCoords(3, 1)) & "} " _
                                         & "{" & I18N.globalize(q.outCoords(0, 0)) & " " & I18N.globalize(q.outCoords(0, 1)) & " " & I18N.globalize(q.outCoords(0, 2)) _
                                         & "; " & I18N.globalize(q.outCoords(1, 0)) & " " & I18N.globalize(q.outCoords(1, 1)) & " " & I18N.globalize(q.outCoords(1, 2)) _
                                         & "; " & I18N.globalize(q.outCoords(2, 0)) & " " & I18N.globalize(q.outCoords(2, 1)) & " " & I18N.globalize(q.outCoords(2, 2)) _
                                         & "; " & I18N.globalize(q.outCoords(3, 0)) & " " & I18N.globalize(q.outCoords(3, 1)) & " " & I18N.globalize(q.outCoords(3, 2)) & "}")
                    Next
                End Using
                Dim ti As New ToolStripMenuItem
                ti.Text = Replace(Mid(templateName, templateName.LastIndexOf("\") + 2), ".txt", "")
                ti.ToolTipText = templateName
                AddHandler ti.Click, AddressOf TemplateItemClick
                TemplateToolStripMenuItem.DropDownItems.Add(ti)
            Catch ex As Exception
            End Try
            UndoRedoHelper.addHistory()
            Me.Refresh()
        End If
    End Sub

    Private Sub ExportToolStripMenuItem_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExportToolStripMenuItem.MouseEnter
        EProjectOnYZPlane2ToolStripMenuItem.Image = Nothing
        EProjectOnZXPlane2ToolStripMenuItem.Image = Nothing
        EProjectOnXYPlane2ToolStripMenuItem.Image = Nothing
        EProjectOnYZPlaneToolStripMenuItem.Image = Nothing
        EProjectOnZXPlaneToolStripMenuItem.Image = Nothing
        EProjectOnXYPlaneToolStripMenuItem.Image = Nothing
        Select Case LPCFile.myMetadata.recommendedMode
            Case 0 ' YZ mit -X (Right) [OK]
                EProjectOnYZPlaneToolStripMenuItem.Image = My.Resources.check
            Case 1 ' XZ mit -Y (Top) [OK]
                EProjectOnZXPlaneToolStripMenuItem.Image = My.Resources.check
            Case 2 ' XY mit -Z (Front) [OK] 
                EProjectOnXYPlaneToolStripMenuItem.Image = My.Resources.check
            Case 3 ' YZ mit +X (Left) [OK]
                EProjectOnYZPlane2ToolStripMenuItem.Image = My.Resources.check
            Case 4 ' XZ mit -Y (Bottom) [OK]
                EProjectOnZXPlane2ToolStripMenuItem.Image = My.Resources.check
            Case 5 ' XY mit +Z (Back) [OK]
                EProjectOnXYPlane2ToolStripMenuItem.Image = My.Resources.check
        End Select
    End Sub

    Private Sub MainForm_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        If e.Delta > 0 Then
            Dim s As New ScrollEventArgs(ScrollEventType.LargeIncrement, 0, 1)
            SBZoom_Scroll(sender, s, True)
        Else
            Dim s As New ScrollEventArgs(ScrollEventType.LargeDecrement, 0, -1)
            SBZoom_Scroll(sender, s, True)
        End If
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles DebugToolStripButton.Click
        DebugToolStripButton.Checked = Not DebugToolStripButton.Checked
        Timer1.Enabled = DebugToolStripButton.Checked
    End Sub

    Private Sub ButtonRemoveIsolatedVertices_Click(sender As Object, e As EventArgs) Handles ButtonRemoveIsolatedVertices.Click
        If View.SelectedVertices.Count = 1 Then GBVertex.Visible = False
        Helper_2D.clearSelection()
        For i As Integer = 0 To LPCFile.Vertices.Count - 1
            If LPCFile.Vertices(i).groupindex = Primitive.NO_INDEX AndAlso LPCFile.Vertices(i).linkedTriangles.Count = 0 Then
                View.SelectedVertices.Add(LPCFile.Vertices(i))
            End If
        Next
        Dim backup As Integer = MainState.objectToModify
        MainState.objectToModify = Modified.Vertex
        ClipboardHelper.delete()
        MainState.objectToModify = backup
        Me.Refresh()
        UndoRedoHelper.addHistory()
    End Sub
End Class
