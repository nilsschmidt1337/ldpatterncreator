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

    ' Define a regular expression to match the significant digits and exponent
    Private Shared sciRegexWithOptionalSign As New Text.RegularExpressions.Regex("(\+|-)?\d+\.?\d*e-?\d+")

    Public Shared Function ScientificNotationToDecimal(ByVal value As String) As String

        Dim matchWithSign As Text.RegularExpressions.Match = sciRegexWithOptionalSign.Match(value)

        ' Check if the string matches the scientific notation pattern
        If Not matchWithSign.Success Then
            ' Return the original value
            Return value
        End If

        ' Extract the significant digits and exponent from the match
        Dim significantDigits As String
        Dim significantDigitsWithSign As String = matchWithSign.Groups(0).Value

        Dim sign As String

        If significantDigitsWithSign.StartsWith("-") Then
            significantDigits = significantDigitsWithSign.Substring(1)
            sign = "-"
        ElseIf significantDigitsWithSign.StartsWith("+") Then
            significantDigits = significantDigitsWithSign.Substring(1)
            sign = "+"
        Else
            significantDigits = significantDigitsWithSign
            sign = ""
        End If

        ' Remove the 'e' and extract the exponent
        Dim expIndex As Integer = significantDigits.IndexOf("e")
        If expIndex <> -1 Then
            Dim exponent As String = significantDigits.Substring(expIndex + 1)
            significantDigits = significantDigits.Substring(0, expIndex)

            ' Convert the significant digits to a decimal by moving the decimal point according to the exponent
            Dim dotIndex As Integer = significantDigits.IndexOf(".")
            Dim moveBy As Integer = CInt(exponent)

            ' Remove the decimal point
            significantDigits = significantDigits.Replace(".", "")

            Dim digitCount As Integer = significantDigits.Length

            ' Append or prepend zeros
            If moveBy < 0 Then
                ' Prepend
                For i As Integer = 0 To Math.Abs(moveBy) - 1
                    significantDigits = "0" & significantDigits
                Next
            ElseIf moveBy > 0 Then
                ' Append
                For i As Integer = 0 To moveBy - 1
                    significantDigits &= "0"
                Next
            End If


            If dotIndex <> -1 Then

                If moveBy < 0 Then

                    ' Close-to-zero values like 1.4e-8 (moveBy = -8, dotIndex = 1)
                    If (-moveBy - dotIndex) > 6 Then
                        Return "0"
                    End If

                    significantDigits = significantDigits.Substring(0, dotIndex) & "." & significantDigits.Substring(dotIndex)

                    ' Remove leading zeros
                    While significantDigits.StartsWith("0")
                        significantDigits = significantDigits.Substring(1)
                    End While
                ElseIf moveBy > 0 Then
                    significantDigits = significantDigits.Substring(0, dotIndex + moveBy) & "." & significantDigits.Substring(dotIndex + moveBy)

                    ' Trailing leading zeros
                    While significantDigits.EndsWith("0")
                        significantDigits = significantDigits.Substring(0, significantDigits.Length - 1)
                    End While

                    If significantDigits.EndsWith(".") Then
                        significantDigits = significantDigits.Substring(0, significantDigits.Length - 1)
                    End If
                End If


            Else
                ' No decimal point, just add one at the appropriate position for the exponent value
                If moveBy < 0 Then
                    significantDigits = significantDigits.Substring(0, digitCount - moveBy - 1) & "." & significantDigits.Substring(digitCount - moveBy - 1)

                    ' Remove leading zeros
                    While significantDigits.StartsWith("0")
                        significantDigits = significantDigits.Substring(1)
                    End While
                End If
            End If
        End If

        ' Return the formatted number, preserving any sign from the original string
        Dim finalNumber As String = sign & significantDigits
        Return finalNumber
    End Function
End Class
