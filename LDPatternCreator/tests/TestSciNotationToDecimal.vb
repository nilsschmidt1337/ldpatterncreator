Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace MathTests

    <TestClass()>
    Public Class TestSciNotationToDecimal
        <TestInitialize()>
        Public Sub Initialize()

        End Sub

        <TestMethod()>
        Public Sub WrongScientificNotationToDecimal()

            Dim result As String
            Dim sciNotationA As String = "abc"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("abc", result)
        End Sub


        <TestMethod()>
        Public Sub SignedScientificNotationToDecimalWithPlus()

            Dim result As String
            Dim sciNotationA As String = "+1e0"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("+1", result)
        End Sub

        <TestMethod()>
        Public Sub SignedScientificNotationToDecimalWithMinus()

            Dim result As String
            Dim sciNotationA As String = "-1e0"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("-1", result)
        End Sub

        <TestMethod()>
        Public Sub SignedScientificNotationToDecimalWithPlusAndExponent()

            Dim result As String
            Dim sciNotationA As String = "+1e1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("+10", result)
        End Sub

        <TestMethod()>
        Public Sub SignedScientificNotationToDecimalWithPlusAndExponent2()

            Dim result As String
            Dim sciNotationA As String = "+12e1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("+120", result)
        End Sub

        <TestMethod()>
        Public Sub SignedScientificNotationToDecimalWithMinusAndExponent()

            Dim result As String
            Dim sciNotationA As String = "-1e-1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("-.1", result)
        End Sub

        <TestMethod()>
        Public Sub SignedScientificNotationToDecimalWithMinusAndExponent2()

            Dim result As String
            Dim sciNotationA As String = "-12e-1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("-1.2", result)
        End Sub

        <TestMethod()>
        Public Sub SignedScientificNotationToDecimalWithMinusAndExponent3()

            Dim result As String
            Dim sciNotationA As String = "-123e-1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("-12.3", result)
        End Sub

        <TestMethod()>
        Public Sub ScientificNotationToDecimal()

            Dim result As String
            Dim sciNotationA As String = "1.23e-1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual(".123", result)
        End Sub

        <TestMethod()>
        Public Sub ScientificNotationToDecimal2()

            Dim result As String
            Dim sciNotationA As String = "12.3e-1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("1.23", result)
        End Sub

        <TestMethod()>
        Public Sub ScientificNotationToDecimal3()

            Dim result As String
            Dim sciNotationA As String = "0.123e-1"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual(".0123", result)
        End Sub

        <TestMethod()>
        Public Sub ScientificNotationToDecimal4()

            Dim result As String
            Dim sciNotationA As String = "12.3e2"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("1230", result)
        End Sub

        <TestMethod()>
        Public Sub ScientificNotationToDecimalComplicated()

            Dim result As String
            Dim sciNotationA As String = "-1422.33521e-10"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("-.000000142233521", result)
        End Sub


        <TestMethod()>
        Public Sub ScientificNotationToDecimalComplicated2()

            Dim result As String
            Dim sciNotationA As String = "1422.33521e4"

            result = MathHelper.ScientificNotationToDecimal(sciNotationA)

            Assert.AreEqual("14223352.1", result)
        End Sub
    End Class
End Namespace

