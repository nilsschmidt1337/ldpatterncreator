Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Deveel.Math

Namespace DeveelTests

    <TestClass()>
    Public Class TestDeveel

        <TestInitialize()>
        Public Sub Initialize()

        End Sub

        <TestMethod()>
        Public Sub DecimalToBigDecimal()

            Dim bd As BigDecimal
            Dim d As Decimal = 123456789123456789123.45678912D

            bd = decToBigDecimal(d)


            Assert.AreEqual("" & d, bd.ToString())
        End Sub

        Private Const SIGN_MASK As Integer = Not Int32.MinValue

        Private Function getScale(ByVal dec As Decimal) As Integer
            Return CInt(Decimal.GetBits(dec)(3) And SIGN_MASK) >> 16
        End Function

        Private Function decToBigDecimal(ByVal dec As Decimal) As BigDecimal
            Dim scale As Integer = getScale(dec)
            dec = CDec(10.0 ^ getScale(dec)) * dec
            Return New BigDecimal(BigInteger.Parse(Decimal.Truncate(dec)), scale)
        End Function


        <TestMethod()>
        Public Sub DoubleToBigDecimal()

            Dim d1 As BigDecimal
            Dim d2 As BigDecimal

            d1 = New BigDecimal(45)
        End Sub

    End Class

End Namespace
