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

'///////////
'//PowerRay 1.00 - Klasse für Strahl-Dreieck-Schnittpunktberechnung
'//(c) Nils Schmidt 2008-2009
'///////////
Public Class PowerRay
    Public Shared t, u, v As Decimal
    Const TOLERANZ As Double = 0.000001D


    Shared Function KREUZ(ByVal v1() As Decimal, ByVal v2() As Decimal) As Decimal() 'Kreuzprodukt
        Dim kreuzVektor(2) As Decimal
        kreuzVektor(0) = v1(1) * v2(2) - v1(2) * v2(1)
        kreuzVektor(1) = v1(2) * v2(0) - v1(0) * v2(2)
        kreuzVektor(2) = v1(0) * v2(1) - v1(1) * v2(0)
        Return kreuzVektor
    End Function

    Shared Function VPR(ByVal v1() As Decimal, ByVal v2() As Decimal) As Decimal 'Vektorprodukt
        Return v1(0) * v2(0) + v1(1) * v2(1) + v1(2) * v2(2)
    End Function

    Shared Function SUBST(ByVal v1() As Decimal, ByVal v2() As Decimal) As Decimal() 'Vektorsubtraktion
        Dim subVektor(2) As Decimal
        subVektor(0) = v1(0) - v2(0)
        subVektor(1) = v1(1) - v2(1)
        subVektor(2) = v1(2) - v2(2)
        Return subVektor
    End Function

    Shared Function SCHNITTPKT_DREIECK(ByVal orig() As Decimal, ByVal dir() As Decimal,
    ByVal vert0() As Decimal, ByVal vert1() As Decimal, ByVal vert2() As Decimal) As Boolean
        Dim ecke1(3), ecke2(3), tvec(3), pvec(3), qvec(3) As Decimal
        Dim diskr As Decimal = 0 : Dim inv_diskr As Decimal = 0

        'Setze Eckenvektoren AB AC
        ecke1 = SUBST(vert1, vert0)
        ecke2 = SUBST(vert2, vert0)

        'Berechne Diskriminante
        pvec = KREUZ(dir, ecke2)
        diskr = VPR(ecke1, pvec)

        'Prüfe Diskriminante um Division durch Null zu vermeiden
        If (diskr > -TOLERANZ AndAlso diskr < TOLERANZ) Then Return False
        inv_diskr = 1 / diskr

        'Abstand zwischen vert0 zu Strahl-Ursprung
        tvec = SUBST(orig, vert0)

        'Berechne den "ersten" baryzentrischen Parameter u
        u = VPR(tvec, pvec) * inv_diskr
        If u < 0 OrElse u > 1 Then Return False

        'Paramter v vorbereiten
        qvec = KREUZ(tvec, ecke1)

        'Berechne den "zweiten" baryzentrischen Parameter v
        v = VPR(dir, qvec) * inv_diskr
        If v < 0 OrElse u + v > 1 Then Return False

        'Berechne den "dritten" baryzentrischen Parameter t
        t = VPR(ecke2, qvec) * inv_diskr
        If t < 0 Then Return False
        'Strahl schneidet Dreieck!
        Return True
    End Function


    Shared Sub UV_KOORDINATEN(ByVal orig() As Decimal, ByVal dir() As Decimal,
    ByVal vert0() As Decimal, ByVal vert1() As Decimal, ByVal vert2() As Decimal)
        Dim ecke1(3), ecke2(3), tvec(3), pvec(3), qvec(3) As Decimal
        Dim diskr As Decimal = 0 : Dim inv_diskr As Decimal = 0

        'Setze Eckenvektoren AB AC
        ecke1 = SUBST(vert1, vert0)
        ecke2 = SUBST(vert2, vert0)

        'Berechne Diskriminante
        pvec = KREUZ(dir, ecke2)
        diskr = VPR(ecke1, pvec)

        'Prüfe Diskriminante um Division durch Null zu vermeiden
        If (diskr > -TOLERANZ AndAlso diskr < TOLERANZ) Then
            t = -1D
            Exit Sub
        End If
        t = 1D

        inv_diskr = 1 / diskr

        'Abstand zwischen vert0 zu Strahl-Ursprung
        tvec = SUBST(orig, vert0)

        'Berechne den "ersten" baryzentrischen Parameter u
        u = VPR(tvec, pvec) * inv_diskr

        'Paramter v vorbereiten
        qvec = KREUZ(tvec, ecke1)

        'Berechne den "zweiten" baryzentrischen Parameter v
        v = VPR(dir, qvec) * inv_diskr

    End Sub

End Class
