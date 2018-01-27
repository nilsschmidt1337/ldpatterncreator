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

Imports System.Threading
Public Class MathHelper
    Public Const twoPI As Double = Math.PI * 2
    Public Shared comma As String = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator

    Public Shared Sub clip(ByRef value As Double, ByVal min As Double, ByVal max As Double)
        value = Math.Max(value, min)
        value = Math.Min(value, max)
    End Sub

    Public Shared Function clip(ByRef value As Integer, ByVal min As Integer, ByVal max As Integer) As Integer
        value = Math.Max(value, min)
        value = Math.Min(value, max)
        Return value
    End Function

    Public Shared Function clipf(ByVal value As Double, ByVal min As Double, ByVal max As Double) As Double
        value = Math.Max(value, min)
        value = Math.Min(value, max)
        Return value
    End Function
End Class
