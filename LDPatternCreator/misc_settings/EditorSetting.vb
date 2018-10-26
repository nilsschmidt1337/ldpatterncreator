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

Public Structure EditorSetting
    Public myLanguage As String
    Public segments_circle As Integer
    Public segments_oval As Integer
    Public radius_circle As Integer
    Public radiusInner_circle As Integer
    Public radius_oval_x As Integer
    Public radius_oval_y As Integer
    Public defaultName As String
    Public defaultUser As String
    Public defaultLicense As String
    Public max_undo As Byte
    Public performanceMode As Boolean
    Public startWithFullscreen As Boolean
    Public mainWindow_x As Integer
    Public mainWindow_y As Integer
    Public mainWindow_width As Integer
    Public mainWindow_height As Integer
    Public backgroundWindow_x As Integer
    Public backgroundWindow_y As Integer
    Public prefsWindow_x As Integer
    Public prefsWindow_y As Integer
    Public colourWindow_x As Integer
    Public colourWindow_y As Integer
    Public colourWindow_width As Integer
    Public colourWindow_height As Integer
    Public showImageViewAtStartup As Boolean
    Public showPreferencesViewAtStartup As Boolean
    Public useAlternativeKeys As Boolean
    Public showTemplateLinesOnTop As Boolean
    Public lockModeChange As Boolean
    Public Sub New(ByVal newSetting As Boolean)
        defaultName = ""
        defaultUser = ""
        defaultLicense = ""
        max_undo = 30
        performanceMode = False
        startWithFullscreen = True
        showImageViewAtStartup = True : showPreferencesViewAtStartup = True : useAlternativeKeys = False : lockModeChange = False
        segments_circle = 16
        segments_oval = 16
        radius_circle = 0
        radius_oval_x = 0
        radius_oval_y = 0
        mainWindow_x = 100
        mainWindow_y = 100
        colourWindow_x = 100
        colourWindow_y = 100
        prefsWindow_x = 100
        prefsWindow_y = 100
        backgroundWindow_x = 100
        backgroundWindow_y = 100
        mainWindow_width = 640
        mainWindow_y = 480
        colourWindow_width = 424
        colourWindow_height = 315
    End Sub
End Structure
