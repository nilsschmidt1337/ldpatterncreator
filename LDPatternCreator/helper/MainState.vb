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

Public Class MainState

    Public Shared cstep As Byte = 100
    Public Shared cnumber As Short = 0
    Public Shared mousePressed As Boolean
    Public Shared zoomScroll As Boolean

    Public Shared colourReplacement As Boolean
    Public Shared conversionEnabled As Boolean = False
    Public Shared primitivesLoaded As Boolean
    Public Shared doSelection As Boolean
    Public Shared doCameraMove As Boolean
    Public Shared movemode As Boolean
    Public Shared rotatemode As Boolean
    Public Shared scalemode As Boolean
    Public Shared isLoading As Boolean
    Public Shared isSolving As Boolean
    Public Shared doAdjust As Boolean
    Public Shared adjustmode As Boolean
    Public Shared keylock As Boolean

    Public Shared trianglemode As Byte = 0
    Public Shared referenceLineMode As Byte = 0

    Public Shared editMode As Byte = EditModes.Selection
    Public Shared objectToModify As Integer = Modified.Vertex

    Public Shared unsavedChanges As Boolean = False
    Public Shared doIntelligentSelection As Boolean = False
    Public Shared intelligentFocusTriangle As Triangle

    Public Shared adjustDirection As Double = 0
    Public Shared tempAdjustDirection As Double = 0

    Public Shared lastAction As Byte = 0
    Public Shared lastMode As Byte = 0
    Public Shared lastPointX As Double
    Public Shared lastPointY As Double
    Public Shared lastColour As Color = Nothing
    Public Shared lastColourNumber As Double = 16
    Public Shared lastGroupLayer As Integer

    Public Shared collisionIndex As Integer = 0

    Public Shared primitiveMode As Byte
    Public Shared primitiveObject As Byte
    Public Shared primitiveNewName As String

    Public Shared primitiveCenter As New Vertex(0, 0, False, False)
    Public Shared primitiveHeight As Double
    Public Shared primitiveWidth As Double
    Public Shared primitiveBordersize As Double

    Public Shared klickX As Integer
    Public Shared klickY As Integer

    Public Shared temp_center As Vertex
    Public Shared temp_vertices(1) As Vertex

    Public Shared tempvertex As New Vertex(0, 0, False, False)
    Public Shared tempcolour As Color
    Public Shared readOnlyVertex As Vertex

    Public Shared Splines As New List(Of Spline)

End Class
