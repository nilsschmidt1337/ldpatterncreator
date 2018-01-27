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
Public Class Metadata
    Implements System.Runtime.Serialization.ISerializable

    Public mData(11) As String
    Public isMainMetadata As Boolean

    Public recommendedMode As Byte = 6
    Public additionalData As String = "<br>"

    Public matrix(3, 3) As Double

    Public mAlias As String = ""

    Sub New()
        For i As Byte = 0 To 11
            mData(i) = ""
        Next i
    End Sub

    Sub New(ByVal description As String, ByVal filename As String, ByVal realname As String, ByVal username As String, _
            ByVal parttype As String, ByVal licensetype As String, ByVal help As String, ByVal bfc As String, _
            ByVal category As String, ByVal keywords As String, ByVal history As String, ByVal comments As String)
        mData(0) = description
        mData(1) = filename
        mData(2) = realname
        mData(3) = username
        mData(4) = parttype
        mData(5) = licensetype
        mData(6) = help
        mData(7) = bfc
        mData(8) = category
        mData(9) = keywords
        mData(10) = history
        mData(11) = comments
    End Sub

    Public Sub show()
        MetadataDialog.TBDescription.Text = mData(0)
        MetadataDialog.TBFilename.Text = mData(1)
        If mData(2) = "" Then
            mData(2) = MainForm.mySettings.defaultName
        End If
        If mData(3) = "" Then
            mData(3) = MainForm.mySettings.defaultUser
        End If
        MetadataDialog.TBRealName.Text = mData(2)
        MetadataDialog.TBUserName.Text = mData(3)
        If Not isMainMetadata Then
            Select Case LPCFile.myMetadata.mData(4)
                Case "Subpart"
                    mData(4) = "Subpart"
                Case "Part"
                    mData(4) = "Subpart"
                Case "Unofficial_Subpart"
                    mData(4) = "Unofficial_Subpart"
                Case "Unofficial_Part"
                    mData(4) = "Unofficial_Subpart"
                Case Else
                    mData(4) = ""
            End Select
            mData(5) = LPCFile.myMetadata.mData(5)
            mData(7) = LPCFile.myMetadata.mData(7)
        Else
            If mData(5) = "" Then
                mData(5) = MainForm.mySettings.defaultLicense
            End If
        End If
        MetadataDialog.CBPartType.Text = mData(4)
        MetadataDialog.CBLicense.Text = mData(5)
        MetadataDialog.RTBHelp.Text = Replace(mData(6), "<br>", vbLf)
        MetadataDialog.CBBFC.Text = mData(7)
        MetadataDialog.TBCategory.Text = mData(8)
        MetadataDialog.RTBKeywords.Text = Replace(mData(9), "<br>", vbLf)
        MetadataDialog.RTBHistory.Text = Replace(mData(10), "<br>", vbLf)
        MetadataDialog.RTBComments.Text = Replace(mData(11), "<br>", vbLf)
    End Sub

    Shadows Function Clone() As Metadata
        Dim mData2(11) As String
        Dim matrix2(3, 3) As Double
        For i As Integer = 0 To 11
            mData2(i) = mData(i)
        Next
        For x As Integer = 0 To 3
            For y As Integer = 0 To 3
                matrix2(x, y) = matrix(x, y)
            Next
        Next
        Return New Metadata() With {.mData = mData2, .mAlias = mAlias.Clone, .additionalData = additionalData.Clone, .matrix = matrix2, .recommendedMode = recommendedMode, .isMainMetadata = isMainMetadata}
    End Function

    Private Sub ISerializable_GetObjectData(ByVal oInfo As SerializationInfo, ByVal oContext As StreamingContext) Implements ISerializable.GetObjectData
        oInfo.AddValue("mData", Me.mData, mData.GetType())
        oInfo.AddValue("recommendedMode", Me.recommendedMode, recommendedMode.GetType())
        oInfo.AddValue("additionalData", Me.additionalData, additionalData.GetType())
        oInfo.AddValue("matrix", Me.matrix, matrix.GetType())
        oInfo.AddValue("isMainMetadata", Me.isMainMetadata, isMainMetadata.GetType())
    End Sub

    Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        Dim tempstringArray(11) As String
        Dim tempbyte As Byte
        Dim tempstring As String = ""
        Dim tempmatrix(3, 3) As Double
        Dim tempbool As Boolean
        Me.mData = info.GetValue("mData", tempstringArray.GetType())
        Me.recommendedMode = info.GetValue("recommendedMode", tempbyte.GetType())
        Me.additionalData = info.GetValue("additionalData", tempstring.GetType())
        Me.matrix = info.GetValue("matrix", tempmatrix.GetType())
        Me.isMainMetadata = info.GetValue("isMainMetadata", tempbool.GetType())
    End Sub

End Class
