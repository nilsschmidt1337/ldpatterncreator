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

Imports System.Text
Imports System.Numerics

Module MatrixCalculations ' It's partly stolen from somewhere , but I did a lot of re- and new-writing, e.g. the Gaussian elimination / Big Integer Class.. ;-)

    Public Function Inverse(ByVal sourceMatrix(,) As Double) As Double(,)
        Dim eachCol As Integer
        Dim eachRow As Integer
        Dim rowsAndCols As Integer

        If (UBound(sourceMatrix, 1) <> UBound(sourceMatrix, 2)) Then
            Throw New Exception("Matrix must be square.")
        End If
        Dim rank As Integer = UBound(sourceMatrix, 1)

        Dim workMatrix(,) As Double = _
            CType(sourceMatrix.Clone, Double(,))

        Dim destMatrix(rank, rank) As Double
        Dim rightHandSide(rank) As Double
        Dim solutions(rank) As Double
        Dim rowPivots(rank) As Integer
        Dim colPivots(rank) As Integer

        workMatrix = FormLU(workMatrix, rowPivots, colPivots, rowsAndCols)

        For eachCol = 0 To rank
            rightHandSide(eachCol) = 1
            BackSolve(workMatrix, rightHandSide, solutions, rowPivots, colPivots)
            For eachRow = 0 To rank
                destMatrix(eachRow, eachCol) = solutions(eachRow)
                rightHandSide(eachRow) = 0
            Next eachRow
        Next eachCol

        Return destMatrix
    End Function

    Private Function FormLU(ByVal sourceMatrix(,) As Double, ByRef rowPivots() As Integer, ByRef colPivots() As Integer, ByRef rowsAndCols As Integer) As Double(,)
        Dim eachRow As Integer
        Dim eachCol As Integer
        Dim pivot As Integer
        Dim rowIndex As Integer
        Dim colIndex As Integer
        Dim bestRow As Integer
        Dim bestCol As Integer
        Dim rowToPivot As Integer
        Dim colToPivot As Integer
        Dim maxValue As Double
        Dim testValue As Double
        Dim oldMax As Double
        Const Deps As Double = 0.0000000000000001

        Dim rank As Integer = UBound(sourceMatrix, 1)
        Dim destMatrix(rank, rank) As Double
        Dim rowNorm(rank) As Double
        ReDim rowPivots(rank)
        ReDim colPivots(rank)

        Array.Copy(sourceMatrix, destMatrix, sourceMatrix.Length)

        For eachRow = 0 To rank
            rowPivots(eachRow) = eachRow
            colPivots(eachRow) = eachRow
            For eachCol = 0 To rank
                rowNorm(eachRow) += Math.Abs(destMatrix(eachRow, eachCol))
            Next eachCol
            If (rowNorm(eachRow) = 0) Then
                Throw New Exception("Cannot invert a singular matrix.")
            End If
        Next eachRow

        For pivot = 0 To rank - 1
            maxValue = 0
            For eachRow = pivot To rank
                rowIndex = rowPivots(eachRow)
                For eachCol = pivot To rank
                    colIndex = colPivots(eachCol)
                    testValue = Math.Abs(destMatrix(rowIndex, colIndex)) _
                        / rowNorm(rowIndex)
                    If (testValue > maxValue) Then
                        maxValue = testValue
                        bestRow = eachRow
                        bestCol = eachCol
                    End If
                Next eachCol
            Next eachRow

            If (maxValue = 0) Then
                Throw New Exception("Singular matrix used for LU.")
            ElseIf (pivot > 1) Then
                If (maxValue < (Deps * oldMax)) Then
                    Throw New Exception("Non-invertible matrix used for LU.")
                End If
            End If
            oldMax = maxValue

            If (rowPivots(pivot) <> rowPivots(bestRow)) Then
                rowsAndCols += 1
                Swap(rowPivots(pivot), rowPivots(bestRow))
            End If

            If (colPivots(pivot) <> colPivots(bestCol)) Then
                rowsAndCols += 1
                Swap(colPivots(pivot), colPivots(bestCol))
            End If

            rowToPivot = rowPivots(pivot)
            colToPivot = colPivots(pivot)

            For eachRow = (pivot + 1) To rank
                rowIndex = rowPivots(eachRow)
                destMatrix(rowIndex, colToPivot) = _
                    -destMatrix(rowIndex, colToPivot) / _
                    destMatrix(rowToPivot, colToPivot)
                For eachCol = (pivot + 1) To rank
                    colIndex = colPivots(eachCol)
                    destMatrix(rowIndex, colIndex) += _
                        destMatrix(rowIndex, colToPivot) * _
                        destMatrix(rowToPivot, colIndex)
                Next eachCol
            Next eachRow
        Next pivot

        If (destMatrix(rowPivots(rank), colPivots(rank)) = 0) Then
            Throw New Exception("Non-invertible matrix used for LU.")
        ElseIf (Math.Abs(destMatrix(rowPivots(rank), colPivots(rank))) / _
                rowNorm(rowPivots(rank))) < (Deps * oldMax) Then
            Throw New Exception("Non-invertible matrix used for LU.")
        End If

        Return destMatrix
    End Function

    Private Sub Swap(ByRef firstValue As Integer, ByRef secondValue As Integer)
        Dim holdValue As Integer
        holdValue = firstValue
        firstValue = secondValue
        secondValue = holdValue
    End Sub

    Private Sub BackSolve(ByVal sourceMatrix(,) As Double, _
            ByVal rightHandSide() As Double, ByVal solutions() As Double, _
            ByRef rowPivots() As Integer, ByRef colPivots() As Integer)
        Dim pivot As Integer
        Dim rowToPivot As Integer
        Dim colToPivot As Integer
        Dim eachRow As Integer
        Dim eachCol As Integer
        Dim rank As Integer = UBound(sourceMatrix, 1)

        For pivot = 0 To (rank - 1)
            colToPivot = colPivots(pivot)
            For eachRow = (pivot + 1) To rank
                rowToPivot = rowPivots(eachRow)
                rightHandSide(rowToPivot) += _
                    sourceMatrix(rowToPivot, colToPivot) _
                    * rightHandSide(rowPivots(pivot))
            Next eachRow
        Next pivot

        For eachRow = rank To 0 Step -1
            colToPivot = colPivots(eachRow)
            rowToPivot = rowPivots(eachRow)
            solutions(colToPivot) = rightHandSide(rowToPivot)
            For eachCol = (eachRow + 1) To rank
                solutions(colToPivot) -= _
                    sourceMatrix(rowToPivot, colPivots(eachCol)) _
                    * solutions(colPivots(eachCol))
            Next eachCol
            solutions(colToPivot) /= sourceMatrix(rowToPivot, colToPivot)
        Next eachRow
    End Sub

    Public Function GaussianElimination(ByVal sourceMatrix(,) As Decimal) As Decimal()
        ' Spaltenweises lösen des Gleichungssystems
        Dim numberOfEquations As Integer = UBound(sourceMatrix, 1) + 1
        Dim numberOfCoefficients As Integer = UBound(sourceMatrix, 2)
        If numberOfCoefficients > numberOfEquations Then Return Nothing Else numberOfCoefficients = numberOfEquations
        Dim destMatrix(numberOfEquations - 1, numberOfCoefficients) As Fraction
        Dim solution(numberOfEquations - 1) As Decimal

        For r = 0 To numberOfEquations - 1
            For c = 0 To numberOfCoefficients
                destMatrix(r, c) = New Fraction(sourceMatrix(r, c))
            Next
        Next

        For activeColumn = 0 To numberOfCoefficients - 1
            Dim activeRow As Integer = activeColumn
            ' 1. Testen ob alle Spalten-Einträge Null sind, wenn ja, gehe zur nächsten Zeile
            Dim allRowsZero As Boolean = True
            Dim rowWithNotZero As Integer
            Do
                For row = activeRow To numberOfEquations - 1
                    If Not destMatrix(row, activeColumn).isZero Then allRowsZero = False : rowWithNotZero = row : Exit Do
                Next
                Continue For
            Loop
            ' 2. Forme um..
            ' 2.1 Wenn M(c, c) Null ist, dann addiere zu M(c) die Zeile M(x) * f, mit M(x,c) <> Null, so dass M(c, c) = 1
            If destMatrix(activeRow, activeColumn).isZero Then
                ' Teile M(rowWithNotZero) durch M(rowWithNotZero,c)
                Dim divisor As Fraction = destMatrix(rowWithNotZero, activeColumn).Clone
                For column = activeColumn To numberOfCoefficients
                    destMatrix(rowWithNotZero, column) /= divisor
                Next
                ' Bringe M(rowWithNotZero,activeColumn) auf +1
                If destMatrix(rowWithNotZero, activeColumn).isNegative Then
                    For c2 = activeColumn To numberOfCoefficients
                        destMatrix(rowWithNotZero, c2).isNegative = Not destMatrix(rowWithNotZero, c2).isNegative
                    Next
                End If
                ' Addiere M(rowWithNotZero) zu M(activeRow)
                For column = 0 To numberOfCoefficients
                    destMatrix(activeRow, column) = destMatrix(activeRow, column).Clone + destMatrix(rowWithNotZero, column).Clone
                    destMatrix(activeRow, column).reduce()
                Next
            Else
                ' Sonst teile M(c) durch M(c,c)
                Dim divisor As Fraction = destMatrix(activeRow, activeColumn).Clone
                destMatrix(activeRow, activeColumn).isNegative = False
                BigInteger.TryParse("1", destMatrix(activeRow, activeColumn).upper)
                BigInteger.TryParse("1", destMatrix(activeRow, activeColumn).lower)
                For column = activeColumn + 1 To numberOfCoefficients
                    destMatrix(activeRow, column) /= divisor
                    destMatrix(activeRow, column).reduce()
                Next
            End If
            ' 2.2 Bringe nun alle anderen Zeilen M(x,activeRow) mit c <> x auf 0
            For row = 0 To numberOfEquations - 1
                If Not (row = activeRow OrElse destMatrix(row, activeColumn).isZero) Then
                    Dim factor As Fraction = destMatrix(row, activeColumn).Clone
                    ' Subtrahiere M(activeRow) von M(row)
                    destMatrix(row, activeColumn).isNegative = False
                    BigInteger.TryParse("0", destMatrix(row, activeColumn).upper)
                    BigInteger.TryParse("1", destMatrix(row, activeColumn).lower)
                    For column = activeColumn + 1 To numberOfCoefficients
                        destMatrix(row, column) = destMatrix(row, column).Clone - destMatrix(activeRow, column).Clone * factor
                        destMatrix(row, column).reduce()
                    Next
                End If
            Next
        Next

        ' Berechne die Lösung:
        ' Wenn es mehrere gibt, gebe nur eine davon zurück, wenn es keine gibt, gebe Nothing zurück.
        Dim columnsWithZero As Integer = 0
        For c = 0 To numberOfCoefficients - 1
            If destMatrix(numberOfEquations - 1, c).isZero Then
                columnsWithZero += 1
            End If
        Next
        If columnsWithZero = numberOfCoefficients AndAlso Not destMatrix(numberOfEquations - 1, numberOfCoefficients).isZero Then
            Return Nothing
        Else
            For r = 0 To numberOfEquations - 1
                solution(r) = destMatrix(r, numberOfCoefficients).ToDecimal
            Next
            Return solution
        End If
    End Function


    Structure Fraction
        Public upper As BigInteger
        Public lower As BigInteger
        Public isNegative As Boolean

        Shadows Function Clone() As Fraction
            Return New Fraction() With {.upper = Me.upper, .lower = Me.lower, .isNegative = Me.isNegative}
        End Function

        Sub New(ByVal value As Decimal)
            value = Math.Round(value, 10)
            If value < 0 Then
                isNegative = True
            End If
            Dim sV As String = value.ToString.ToUpperInvariant
            Dim sL As String = "1"
            If sV(0) = CType("-", Char) Then sV = Mid(sV, 2)
            Dim indexOfE As Integer = sV.IndexOf(CType("E", Char))
            Dim indexOfComma As Integer = sV.IndexOf(CType(MathHelper.comma, Char))
            Dim rvalue As Decimal = 0
            If indexOfE <> -1 Then
                Dim exponent As Integer
                exponent = CType(Mid(sV, indexOfE + 2), Integer)
                sV = Mid(sV, 1, indexOfE)
                If indexOfComma = -1 Then indexOfComma = sV.Length
                If exponent < 0 Then
                    For i = 2 To sV.Length - indexOfComma
                        sL += "0"
                    Next
                    For i = exponent To -1
                        sL += "0"
                    Next
                Else
                    For i = 2 To sV.Length - indexOfComma
                        sL += "0"
                    Next
                    For i = 1 To exponent
                        sV += "0"
                    Next
                End If
            Else
                If indexOfComma = -1 Then indexOfComma = sV.Length
                For i = 2 To sV.Length - indexOfComma
                    sL += "0"
                Next
            End If
            BigInteger.TryParse(sL, lower)
            BigInteger.TryParse(Replace(sV, MathHelper.comma, ""), upper)
        End Sub

        Sub New(ByVal value As Long)
            If value < 0 Then
                isNegative = True
                value *= -1
            End If
            BigInteger.TryParse(value.ToString, upper)
            BigInteger.TryParse("1", lower)
        End Sub

        Sub New(ByVal value As ULong)
            BigInteger.TryParse(value.ToString, upper)
            BigInteger.TryParse("1", lower)
        End Sub

        Sub New(ByVal value As Short)
            Me.New(CType(value, Long))
        End Sub

        Sub New(ByVal value As Integer)
            Me.New(CType(value, Long))
        End Sub

        Sub New(ByVal value As Single)
            Me.New(CType(value, Decimal))
        End Sub

        Sub New(ByVal value As Double)
            Me.New(CType(value, Decimal))
        End Sub

        Sub New(ByVal value As Byte)
            Me.New(CType(value, ULong))
        End Sub

        Sub New(ByVal value As UShort)
            Me.New(CType(value, ULong))
        End Sub

        Sub New(ByVal value As UInteger)
            Me.New(CType(value, ULong))
        End Sub

        Public Function isOne() As Boolean
            If isNegative Then
                Return False
            End If
            Return upper = lower
        End Function

        Public Function isZero() As Boolean
            Return upper.IsZero
        End Function

        Public Sub reduce()
            Dim a, b, h As BigInteger
            a = upper
            b = lower
            While Not b.isZero
                h = a Mod b
                a = b
                b = h
            End While
            upper = upper / a
            lower = lower / a
        End Sub

        Public Shared Operator +(ByVal v1 As Fraction, ByVal v2 As Fraction) As Fraction
            Dim frac As New Fraction()
            If v1.isZero Then
                v1.isNegative = False
                frac = v2.Clone
                Return frac
            End If
            If v2.isZero Then
                v2.isNegative = False
                frac = v1.Clone
                Return frac
            End If
            If v1.lower = v2.lower Then
                frac.lower = v1.lower
            Else
                frac.lower = v1.lower * v2.lower
                v1.upper *= v2.lower
                v2.upper *= v1.lower
                v1.lower = frac.lower
                v2.lower = frac.lower
            End If
            If v1.isNegative Then
                If v2.isNegative Then
                    frac.isNegative = True
                    frac.upper = v2.upper + v1.upper
                Else
                    If v1.upper > v2.upper Then
                        frac.isNegative = True
                        frac.upper = v1.upper - v2.upper
                    Else
                        frac.upper = v2.upper - v1.upper
                    End If
                End If
            Else
                If v2.isNegative Then
                    If v1.upper > v2.upper Then
                        frac.upper = v1.upper - v2.upper
                    Else
                        frac.isNegative = True
                        frac.upper = v2.upper - v1.upper
                    End If
                Else
                    frac.upper = v2.upper + v1.upper
                End If
            End If
            If frac.isNegative Then
                frac.isNegative = frac.isNegative Xor frac.isZero
            End If
            Return frac
        End Operator

        Public Shared Operator -(ByVal v1 As Fraction, ByVal v2 As Fraction) As Fraction
            Dim frac As New Fraction()
            frac = v2.Clone
            frac.isNegative = Not frac.isNegative
            Return v1 + frac
        End Operator

        Public Shared Operator *(ByVal v1 As Fraction, ByVal v2 As Fraction) As Fraction
            Dim frac As New Fraction()
            frac.upper = v1.upper * v2.upper
            frac.lower = v1.lower * v2.lower
            If v1.upper.IsZero Or v2.upper.IsZero Then
                frac.isNegative = False
                BigInteger.TryParse("0", frac.upper)
                BigInteger.TryParse("1", frac.lower)
            Else
                frac.isNegative = v1.isNegative Xor v2.isNegative
            End If
            Return frac
        End Operator

        Public Shared Operator /(ByVal v1 As Fraction, ByVal v2 As Fraction) As Fraction
            Dim frac As New Fraction()
            frac.upper = v1.upper * v2.lower
            frac.lower = v1.lower * v2.upper
            If v1.upper.isZero Then
                frac.isNegative = False
                BigInteger.TryParse("0", frac.upper)
                BigInteger.TryParse("1", frac.lower)
            Else
                frac.isNegative = v1.isNegative Xor v2.isNegative
            End If
            Return frac
        End Operator

        Private Shared mc As Deveel.Math.MathContext = New Deveel.Math.MathContext(128)

        Public Function ToDecimal() As Decimal
            reduce()

            Dim up As New Deveel.Math.BigDecimal(Deveel.Math.BigInteger.Parse(upper.ToString))
            Dim lo As New Deveel.Math.BigDecimal(Deveel.Math.BigInteger.Parse(lower.ToString))

            Dim result As Decimal
            Decimal.TryParse(up.Divide(lo, mc).Negate().ToString(), result)

            If isNegative Then
                Return -result
                'Return -(CDec(Math.Exp(BigInteger.Log(upper) - BigInteger.Log(lower))))
            Else
                Return result
                'Return CDec(Math.Exp(BigInteger.Log(upper) - BigInteger.Log(lower)))
            End If
            'If isNegative Then
            'Return -(CDec(Math.Exp(BigInteger.Log(upper) - BigInteger.Log(lower))))
            'Else
            'Return CDec(Math.Exp(BigInteger.Log(upper) - BigInteger.Log(lower)))
            'End If
        End Function

        Public Overrides Function toString() As String
            If isNegative Then
                Return "-" & upper.toString & " / " & lower.toString & " => " & Me.ToDecimal
            Else
                Return upper.toString & " / " & lower.toString & " => " & Me.ToDecimal
            End If
        End Function

    End Structure

End Module
