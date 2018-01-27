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
Public Class LPCFile

    Public Shared Vertices As New List(Of Vertex)
    Public Shared Triangles As New List(Of Triangle)
    Public Shared Primitives As New List(Of Primitive)
    Public Shared PrimitivesHMap As New Dictionary(Of Integer, Integer)(1000)
    Public Shared PrimitivesMetadataHMap As New Dictionary(Of String, Metadata)(100)
    Public Shared myMetadata As New Metadata() With {.isMainMetadata = True}

    Public Shared templateShape As New List(Of PointF)
    Public Shared templateProjectionQuads As New List(Of ProjectionQuad)
    Public Shared templateTexts As New List(Of TemplateTextInfo)

    Public Shared includeMetadata As Boolean = False
    Public Shared rectify As Boolean = True
    Public Shared slice As Boolean = False
    Public Shared unify As Boolean = True
    Public Shared unifyLPC As Boolean = True
    Public Shared replaceColour As Boolean = True
    Public Shared project As Boolean = True

    Public Shared colourReplacementMapBrush As New Dictionary(Of Integer, Brush)
    Public Shared oldColours As New List(Of Colour)
    Public Shared newColours As New List(Of Colour)

    Public Shared helperLineStartIndex As Integer = -1
    Public Shared helperLineEndIndex As Integer = -1

End Class
