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
Imports System.Runtime.Serialization

<Serializable()> _
   Public Class TemplateTextInfo
    Implements System.Runtime.Serialization.ISerializable

    Public Text As String
    Public X As Double
    Public Y As Double

    Sub New(ByVal x As Double, ByVal y As Double, ByVal text As String)
        Me.X = x
        Me.Y = y
        Me.Text = text
    End Sub

    Shadows Function Clone() As TemplateTextInfo
        Return New TemplateTextInfo(X, Y, Text)
    End Function

    Private Sub ISerializable_GetObjectData(ByVal oInfo As SerializationInfo, ByVal oContext As StreamingContext) Implements ISerializable.GetObjectData
        oInfo.AddValue("X", Me.X, X.GetType())
        oInfo.AddValue("Y", Me.Y, Y.GetType())
        oInfo.AddValue("Text", Me.Text, Text.GetType())
    End Sub

    Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        Dim tempDouble As Double
        Dim tempstring As String = ""
        Me.X = info.GetValue("X", tempDouble.GetType())
        Me.Y = info.GetValue("Y", tempDouble.GetType())
        Me.Text = info.GetValue("Text", tempstring.GetType())
    End Sub

End Class
