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

Public Class I18N

    Public Shared trl8 As New Dictionary(Of Short, String)

    Public Enum lk As Short
        '#Menu Captions#########
        MenuFile
        MenuEdit
        MenuView
        MenuOptions
        MenuLanguage
        MenuHelp
        '#File Menu#########
        NewFile
        Load
        Save
        SaveAs
        LoadTemplate
        DeleteTemplateData
        ConvertProjection
        CreateTemplate
        EditTemplate
        LoadBGImage
        AdjustBGImage
        ImportDAT
        ExportDAT
        ImportColours
        ExportColours
        StickerGenerator
        ProjectOnYZRight
        ProjectOnYZLeft
        ProjectOnXZTop
        ProjectOnXZBottom
        ProjectOnXYFront
        ProjectOnXYBack
        ReplaceColours
        Metadata
        ImportBitmap
        CreateBitmap
        ExitApp
        '#Edit Menu#########
        Undo
        Redo
        Cut
        Copy
        Paste
        Delete
        SelectAll
        SelectSame
        SelectConnected
        SelectTouching
        PlusSameColour
        DetectOverlaps
        PerfMode
        FastTriangulation
        '#View Menu#########
        ResetView
        ShowGrid
        ShowAxis
        ShowImage
        Image
        ViewPreferences
        Zoom
        Unit
        '#Options Menu#########
        AllWarnings
        AllWarningsTip
        ResetLDraw
        AdvancedOpt
        AdvancedOptTip
        '#Help Menu##########
        About
        Update
        NoUpdates
        UpdateAvailable
        '#Actions#########
        DoSelect
        Move
        Rotate
        Scale
        AddVertex
        AddTriangle
        AddReferenceLine
        AddHint
        TriangleAutoCompletion
        Tcut
        Tcopy
        Tpaste
        Group
        Ungroup
        ShowColours
        '#Modes########
        TriangleMode
        VertexMode
        PrimitiveMode
        ReferenceLineMode
        '#Mirror########
        Mirror
        OnX
        OnY
        OnXLeft
        OnXRight
        OnYTop
        OnYBottom
        '#CSG##########
        CSG
        CSGTip
        CSGUnion
        CSGUnionTip
        CSGSubdivide
        CSGRotate
        CSGIntersect
        CSGIntersectTip
        '#Merge/Split##########
        MergeSplit
        ToAverage
        ToAverageTip
        ToAverageX
        ToAverageY
        ToLastSelected
        ToLastSelectedTip
        ToNearestPrim
        ToNearestLineTemplate
        ToNearestLineTriangle
        Split
        RemoveIsolated
        '#Primitives#########
        AddPrimitive
        Solid
        Hollow
        Frame
        Segments
        Radius
        InnerRadius
        LPCTriangle
        LPCRectangle
        LPCCircle
        LPCOval
        LPCChain
        LDrawPrimitive
        CircDiscSector
        InvCircDiscSector
        InvTangCircDiscSector
        CircDiscSegment
        CircRingSegment
        HighRes
        AdaptorRing
        '#Units###########
        LDUnit
        Millimetre
        InchUnit
        LDU
        mm
        Inch
        '#Colour Toolbar##############
        Colour
        More
        TakeColour
        Preview
        Opacity
        '#Context Menu##########
        CMSActions
        CMSMode
        Tdelete
        '#Single Vertex Data Manipulator########
        VertexData
        VertexX
        VertexY
        '#Image########
        ImageTitle
        ImageFile
        OffsetX
        OffsetY
        ImageScale
        AdjustImageBtn
        '#View-Preferences######
        PrefTitle
        MoveSnap
        RotateSnap
        ScaleSnap
        GridSize
        '#Zoom Dialog#########
        ZoomTitle
        ZoomParam
        Viewport
        UpperLeft
        LowerRight
        '#Template Editor Dialog########
        EditorTitle
        TemplateTitle
        DataGroup
        ProjectOn
        Right
        Top
        Front
        Left
        Bottom
        Back
        TemplateOffset
        TemplateMatrix
        PolygonData
        AdditionalLines
        ShowExample
        Tsave
        ParsingError
        ParsingErrorDescription
        CtrlEnterHint
        '#Template Manager#########
        ManagerTitle
        Edit
        '#Colour-Palette########
        ColourTitle
        ColourTip
        DirectColour
        SetColour
        '#Misc Dialogs########
        LDrawDir
        BitmapImport
        BitmapExport
        OpenLPC
        OpenImage
        UnsavedChanges
        NewPattern
        InvalidImage
        AllFiles
        ImageFiles
        DATFiles
        AboutTitle
        AboutInfo
        LDrawDirInfo
        Center
        Width
        Height
        TriangleChainAdd
        '#Import DAT##########
        ImportDATRight
        ImportDATLeft
        ImportDATTop
        ImportDATBottom
        ImportDATFront
        ImportDATBack
        NoInfoInDAT1
        NoInfoInDAT2
        InvalidDAT
        '#Export DAT#########
        ExportDATRight
        ExportDATLeft
        ExportDATTop
        ExportDATBottom
        ExportDATFront
        ExportDATBack
        ExportStatus1
        ExportSubfileHint
        ExportStatus2
        ExportStatus3
        ExportStatus4
        Plane1
        Plane2
        Plane3
        Plane4
        Plane5
        Plane6
        PlaneHint
        '#Import Dialog########
        ImportTitle
        ImportMode
        ImportOverwrite
        ImportAppend
        ImportProjection
        Import
        '#Colour Replacer Dialog########
        ReplacerTitle
        ReplacerTip
        ReplacerOldColour
        ReplacerNewColour
        ReplacerInsertRule
        ReplacerPick
        '#Overlap Detector########
        NoOverlaps
        OverlapsFound
        PrevOverlap
        NextOverlap
        '#Import Bitmap########;"
        ImportStatus1
        ImportStatus2
        ImportStatus3
        ImportStatus4
        ImportStatus5
        ImportStatus6
        '#Metadata Dialog##########
        MetadataTitle
        MPartTree
        MMainFile
        MNoSubFiles
        MPreview
        MEnablePreview
        MSetAsDefault
        MPart
        MFile
        MAuthor
        MRealName
        MUser
        MType
        MLicense
        MHelp
        MBFC
        MCategory
        MKeywords
        MHistory
        MComments
        MExport
        '#Options Settings########
        Options
        SettingsTitle
        SMetaDefaults
        SUndoRedo
        SMaxUndo
        SDisableUndo
        SStartup
        SStartFullscreen
        SImage
        SPrefs
        SMisc
        SAlternative
        STemplateTop
        SAddLock
        SRestore
        '#Options Hotkeys########
        Hotkeys
        Tfunction
        KeyCombo
        Context1
        Context2
        LMB
        RMB
        Key00
        Key01
        Key02
        Key03
        Key04
        Key05
        Key06
        Key07
        Key08
        Key09
        Key10
        Key11
        Key12
        Key13
        Key14
        Key15
        Key16
        Key17
        Key18
        Key19
        Key20
        Key21
        Key22
        Key23
        Key24
        Key25
        Key26
        Key27
        InvalidKey
        KeyInUse
        SetKey
        KeyMsg
        '#Options Colours########
        Tobject
        ObjectColour
        Colours
        C00
        C01
        C02
        C03
        C04
        C05
        C06
        C07
        C08
        C09
        C10
        C11
        Old
        Tnew
        '#Primitive Creation########
        POrigin
        PHeight
        PWidth
        PFrameSize
        PRingTitle
        PRadius
        PResolution
        PName
        PFilename
        '#Bitmap Export
        BTitle
        BLdrawColours
        BChoosenColours
        BDimension
        BFree
        '#MsgBox Titles########
        Info
        Question
        Fatal
        Warning
        '#OK, Cancel and Abort Button#######
        OK
        ABORT
        Cancel
        Create
        Apply
        '#Spliner########
        SplineMenu
        SplineStart
        SplineFirstDir
        SplineNext
        SplineNextDir
        SplineSegCount
    End Enum

    Public Shared Function stringToShort(ByVal s As String) As Short
        Select Case s
            '#Menu Captions#########
            Case "MenuFile" : Return lk.MenuFile
            Case "MenuEdit" : Return lk.MenuEdit
            Case "MenuView" : Return lk.MenuView
            Case "MenuOptions" : Return lk.MenuOptions
            Case "MenuLanguage" : Return lk.MenuLanguage
            Case "MenuHelp" : Return lk.MenuHelp
                '#File Menu#########
            Case "NewFile" : Return lk.NewFile
            Case "Load" : Return lk.Load
            Case "Save" : Return lk.Save
            Case "SaveAs" : Return lk.SaveAs
            Case "LoadTemplate" : Return lk.LoadTemplate
            Case "DeleteTemplateData" : Return lk.DeleteTemplateData
            Case "ConvertProjection" : Return lk.ConvertProjection
            Case "CreateTemplate" : Return lk.CreateTemplate
            Case "EditTemplate" : Return lk.EditTemplate
            Case "LoadBGImage" : Return lk.LoadBGImage
            Case "AdjustBGImage" : Return lk.AdjustBGImage
            Case "ImportDAT" : Return lk.ImportDAT
            Case "ExportDAT" : Return lk.ExportDAT
            Case "ImportColours" : Return lk.ImportColours
            Case "ExportColours" : Return lk.ExportColours
            Case "StickerGenerator" : Return lk.StickerGenerator
            Case "ProjectOnYZRight" : Return lk.ProjectOnYZRight
            Case "ProjectOnYZLeft" : Return lk.ProjectOnYZLeft
            Case "ProjectOnXZTop" : Return lk.ProjectOnXZTop
            Case "ProjectOnXZBottom" : Return lk.ProjectOnXZBottom
            Case "ProjectOnXYFront" : Return lk.ProjectOnXYFront
            Case "ProjectOnXYBack" : Return lk.ProjectOnXYBack
            Case "ReplaceColours" : Return lk.ReplaceColours
            Case "Metadata" : Return lk.Metadata
            Case "ImportBitmap" : Return lk.ImportBitmap
            Case "CreateBitmap" : Return lk.CreateBitmap
            Case "ExitApp" : Return lk.ExitApp
                '#Edit Menu#########
            Case "Undo" : Return lk.Undo
            Case "Redo" : Return lk.Redo
            Case "Cut" : Return lk.Cut
            Case "Copy" : Return lk.Copy
            Case "Paste" : Return lk.Paste
            Case "Delete" : Return lk.Delete
            Case "SelectAll" : Return lk.SelectAll
            Case "SelectSame" : Return lk.SelectSame
            Case "SelectConnected" : Return lk.SelectConnected
            Case "SelectTouching" : Return lk.SelectTouching
            Case "PlusSameColour" : Return lk.PlusSameColour
            Case "DetectOverlaps" : Return lk.DetectOverlaps
            Case "PerfMode" : Return lk.PerfMode
            Case "FastTriangulation" : Return lk.FastTriangulation
                '#View Menu#########
            Case "ResetView" : Return lk.ResetView
            Case "ShowGrid" : Return lk.ShowGrid
            Case "ShowAxis" : Return lk.ShowAxis
            Case "ShowImage" : Return lk.ShowImage
            Case "Image" : Return lk.Image
            Case "ViewPreferences" : Return lk.ViewPreferences
            Case "Zoom" : Return lk.Zoom
            Case "Unit" : Return lk.Unit
                '#Options Menu#########
            Case "AllWarnings" : Return lk.AllWarnings
            Case "AllWarningsTip" : Return lk.AllWarningsTip
            Case "ResetLDraw" : Return lk.ResetLDraw
            Case "AdvancedOpt" : Return lk.AdvancedOpt
            Case "AdvancedOptTip" : Return lk.AdvancedOptTip
                '#Help Menu##########
            Case "About" : Return lk.About
            Case "Update" : Return lk.Update
            Case "NoUpdates" : Return lk.NoUpdates
            Case "UpdateAvailable" : Return lk.UpdateAvailable
                '#Actions#########
            Case "DoSelect" : Return lk.DoSelect
            Case "Move" : Return lk.Move
            Case "Rotate" : Return lk.Rotate
            Case "Scale" : Return lk.Scale
            Case "AddVertex" : Return lk.AddVertex
            Case "AddTriangle" : Return lk.AddTriangle
            Case "AddReferenceLine" : Return lk.AddReferenceLine
            Case "AddHint" : Return lk.AddHint
            Case "TriangleAutoCompletion" : Return lk.TriangleAutoCompletion
            Case "Tcut" : Return lk.Tcut
            Case "Tcopy" : Return lk.Tcopy
            Case "Tpaste" : Return lk.Tpaste
            Case "Group" : Return lk.Group
            Case "Ungroup" : Return lk.Ungroup
            Case "ShowColours" : Return lk.ShowColours
                '#Modes########
            Case "TriangleMode" : Return lk.TriangleMode
            Case "VertexMode" : Return lk.VertexMode
            Case "PrimitiveMode" : Return lk.PrimitiveMode
            Case "ReferenceLineMode" : Return lk.ReferenceLineMode
                '#Mirror########
            Case "Mirror" : Return lk.Mirror
            Case "OnX" : Return lk.OnX
            Case "OnY" : Return lk.OnY
            Case "OnXLeft" : Return lk.OnXLeft
            Case "OnXRight" : Return lk.OnXRight
            Case "OnYTop" : Return lk.OnYTop
            Case "OnYBottom" : Return lk.OnYBottom
                '#CSG##########
            Case "CSG" : Return lk.CSG
            Case "CSGTip" : Return lk.CSGTip
            Case "CSGUnion" : Return lk.CSGUnion
            Case "CSGUnionTip" : Return lk.CSGUnionTip
            Case "CSGSubdivide" : Return lk.CSGSubdivide
            Case "CSGRotate" : Return lk.CSGRotate
            Case "CSGIntersect" : Return lk.CSGIntersect
            Case "CSGIntersectTip" : Return lk.CSGIntersectTip
                '#Merge/Split##########
            Case "MergeSplit" : Return lk.MergeSplit
            Case "ToAverage" : Return lk.ToAverage
            Case "ToAverageTip" : Return lk.ToAverageTip
            Case "ToAverageX" : Return lk.ToAverageX
            Case "ToAverageY" : Return lk.ToAverageY
            Case "ToLastSelected" : Return lk.ToLastSelected
            Case "ToLastSelectedTip" : Return lk.ToLastSelectedTip
            Case "ToNearestPrim" : Return lk.ToNearestPrim
            Case "ToNearestLineTemplate" : Return lk.ToNearestLineTemplate
            Case "ToNearestLineTriangle" : Return lk.ToNearestLineTriangle
            Case "Split" : Return lk.Split
            Case "RemoveIsolated" : Return lk.RemoveIsolated
                '#Primitives#########
            Case "AddPrimitive" : Return lk.AddPrimitive
            Case "Solid" : Return lk.Solid
            Case "Hollow" : Return lk.Hollow
            Case "Frame" : Return lk.Frame
            Case "Segments" : Return lk.Segments
            Case "Radius" : Return lk.Radius
            Case "InnerRadius" : Return lk.InnerRadius
            Case "LPCTriangle" : Return lk.LPCTriangle
            Case "LPCRectangle" : Return lk.LPCRectangle
            Case "LPCCircle" : Return lk.LPCCircle
            Case "LPCOval" : Return lk.LPCOval
            Case "LPCChain" : Return lk.LPCChain
            Case "LDrawPrimitive" : Return lk.LDrawPrimitive
            Case "CircDiscSector" : Return lk.CircDiscSector
            Case "InvCircDiscSector" : Return lk.InvCircDiscSector
            Case "InvTangCircDiscSector" : Return lk.InvTangCircDiscSector
            Case "CircDiscSegment" : Return lk.CircDiscSegment
            Case "CircRingSegment" : Return lk.CircRingSegment
            Case "HighRes" : Return lk.HighRes
            Case "AdaptorRing" : Return lk.AdaptorRing
                '#Units###########
            Case "LDUnit" : Return lk.LDUnit
            Case "Millimetre" : Return lk.Millimetre
            Case "InchUnit" : Return lk.InchUnit
            Case "LDU" : Return lk.LDU
            Case "mm" : Return lk.mm
            Case "Inch" : Return lk.Inch
                '#Colour Toolbar##############
            Case "Colour" : Return lk.Colour
            Case "More" : Return lk.More
            Case "TakeColour" : Return lk.TakeColour
            Case "Preview" : Return lk.Preview
            Case "Opacity" : Return lk.Opacity
                '#Context Menu##########
            Case "CMSActions" : Return lk.CMSActions
            Case "CMSMode" : Return lk.CMSMode
            Case "Tdelete" : Return lk.Tdelete
                '#Single Vertex Data Manipulator########
            Case "VertexData" : Return lk.VertexData
            Case "VertexX" : Return lk.VertexX
            Case "VertexY" : Return lk.VertexY
                '#Image########
            Case "ImageTitle" : Return lk.ImageTitle
            Case "ImageFile" : Return lk.ImageFile
            Case "OffsetX" : Return lk.OffsetX
            Case "OffsetY" : Return lk.OffsetY
            Case "ImageScale" : Return lk.ImageScale
            Case "AdjustImageBtn" : Return lk.AdjustImageBtn
                '#View-Preferences######
            Case "PrefTitle" : Return lk.PrefTitle
            Case "MoveSnap" : Return lk.MoveSnap
            Case "RotateSnap" : Return lk.RotateSnap
            Case "ScaleSnap" : Return lk.ScaleSnap
            Case "GridSize" : Return lk.GridSize
                '#Zoom Dialog#########
            Case "ZoomTitle" : Return lk.ZoomTitle
            Case "ZoomParam" : Return lk.ZoomParam
            Case "Viewport" : Return lk.Viewport
            Case "UpperLeft" : Return lk.UpperLeft
            Case "LowerRight" : Return lk.LowerRight
                '#Template Editor Dialog########
            Case "EditorTitle" : Return lk.EditorTitle
            Case "TemplateTitle" : Return lk.TemplateTitle
            Case "DataGroup" : Return lk.DataGroup
            Case "ProjectOn" : Return lk.ProjectOn
            Case "Right" : Return lk.Right
            Case "Top" : Return lk.Top
            Case "Front" : Return lk.Front
            Case "Left" : Return lk.Left
            Case "Bottom" : Return lk.Bottom
            Case "Back" : Return lk.Back
            Case "TemplateOffset" : Return lk.TemplateOffset
            Case "TemplateMatrix" : Return lk.TemplateMatrix
            Case "PolygonData" : Return lk.PolygonData
            Case "AdditionalLines" : Return lk.AdditionalLines
            Case "ShowExample" : Return lk.ShowExample
            Case "Tsave" : Return lk.Tsave
            Case "ParsingError" : Return lk.ParsingError
            Case "ParsingErrorDescription" : Return lk.ParsingErrorDescription
            Case "CtrlEnterHint" : Return lk.CtrlEnterHint
                '#Template Manager#########
            Case "ManagerTitle" : Return lk.ManagerTitle
            Case "Edit" : Return lk.Edit
                '#Colour-Palette########
            Case "ColourTitle" : Return lk.ColourTitle
            Case "ColourTip" : Return lk.ColourTip
            Case "DirectColour" : Return lk.DirectColour
            Case "SetColour" : Return lk.SetColour
                '#Misc Dialogs########
            Case "LDrawDir" : Return lk.LDrawDir
            Case "BitmapImport" : Return lk.BitmapImport
            Case "BitmapExport" : Return lk.BitmapExport
            Case "OpenLPC" : Return lk.OpenLPC
            Case "OpenImage" : Return lk.OpenImage
            Case "UnsavedChanges" : Return lk.UnsavedChanges
            Case "NewPattern" : Return lk.NewPattern
            Case "InvalidImage" : Return lk.InvalidImage
            Case "AllFiles" : Return lk.AllFiles
            Case "ImageFiles" : Return lk.ImageFiles
            Case "DATFiles" : Return lk.DATFiles
            Case "AboutTitle" : Return lk.AboutTitle
            Case "AboutInfo" : Return lk.AboutInfo
            Case "LDrawDirInfo" : Return lk.LDrawDirInfo
            Case "Center" : Return lk.Center
            Case "Width" : Return lk.Width
            Case "Height" : Return lk.Height
            Case "TriangleChainAdd" : Return lk.TriangleChainAdd
                '#Import DAT##########
            Case "ImportDATRight" : Return lk.ImportDATRight
            Case "ImportDATLeft" : Return lk.ImportDATLeft
            Case "ImportDATTop" : Return lk.ImportDATTop
            Case "ImportDATBottom" : Return lk.ImportDATBottom
            Case "ImportDATFront" : Return lk.ImportDATFront
            Case "ImportDATBack" : Return lk.ImportDATBack
            Case "NoInfoInDAT1" : Return lk.NoInfoInDAT1
            Case "NoInfoInDAT2" : Return lk.NoInfoInDAT2
            Case "InvalidDAT" : Return lk.InvalidDAT
                '#Export DAT#########
            Case "ExportDATRight" : Return lk.ExportDATRight
            Case "ExportDATLeft" : Return lk.ExportDATLeft
            Case "ExportDATTop" : Return lk.ExportDATTop
            Case "ExportDATBottom" : Return lk.ExportDATBottom
            Case "ExportDATFront" : Return lk.ExportDATFront
            Case "ExportDATBack" : Return lk.ExportDATBack
            Case "ExportStatus1" : Return lk.ExportStatus1
            Case "ExportSubfileHint" : Return lk.ExportSubfileHint
            Case "ExportStatus2" : Return lk.ExportStatus2
            Case "ExportStatus3" : Return lk.ExportStatus3
            Case "ExportStatus4" : Return lk.ExportStatus4
            Case "Plane1" : Return lk.Plane1
            Case "Plane2" : Return lk.Plane2
            Case "Plane3" : Return lk.Plane3
            Case "Plane4" : Return lk.Plane4
            Case "Plane5" : Return lk.Plane5
            Case "Plane6" : Return lk.Plane6
            Case "PlaneHint" : Return lk.PlaneHint
                '#Import Dialog########
            Case "ImportTitle" : Return lk.ImportTitle
            Case "ImportMode" : Return lk.ImportMode
            Case "ImportOverwrite" : Return lk.ImportOverwrite
            Case "ImportAppend" : Return lk.ImportAppend
            Case "ImportProjection" : Return lk.ImportProjection
            Case "Import" : Return lk.Import
                '#Colour Replacer Dialog########
            Case "ReplacerTitle" : Return lk.ReplacerTitle
            Case "ReplacerTip" : Return lk.ReplacerTip
            Case "ReplacerOldColour" : Return lk.ReplacerOldColour
            Case "ReplacerNewColour" : Return lk.ReplacerNewColour
            Case "ReplacerInsertRule" : Return lk.ReplacerInsertRule
            Case "ReplacerPick" : Return lk.ReplacerPick
                '#Overlap Detector########
            Case "NoOverlaps" : Return lk.NoOverlaps
            Case "OverlapsFound" : Return lk.OverlapsFound
            Case "PrevOverlap" : Return lk.PrevOverlap
            Case "NextOverlap" : Return lk.NextOverlap
                '#Import Bitmap########
            Case "ImportStatus1" : Return lk.ImportStatus1
            Case "ImportStatus2" : Return lk.ImportStatus2
            Case "ImportStatus3" : Return lk.ImportStatus3
            Case "ImportStatus4" : Return lk.ImportStatus4
            Case "ImportStatus5" : Return lk.ImportStatus5
            Case "ImportStatus6" : Return lk.ImportStatus6
                '#Metadata Dialog##########
            Case "MetadataTitle" : Return lk.MetadataTitle
            Case "MPartTree" : Return lk.MPartTree
            Case "MMainFile" : Return lk.MMainFile
            Case "MNoSubFiles" : Return lk.MNoSubFiles
            Case "MPreview" : Return lk.MPreview
            Case "MEnablePreview" : Return lk.MEnablePreview
            Case "MSetAsDefault" : Return lk.MSetAsDefault
            Case "MPart" : Return lk.MPart
            Case "MFile" : Return lk.MFile
            Case "MAuthor" : Return lk.MAuthor
            Case "MRealName" : Return lk.MRealName
            Case "MUser" : Return lk.MUser
            Case "MType" : Return lk.MType
            Case "MLicense" : Return lk.MLicense
            Case "MHelp" : Return lk.MHelp
            Case "MBFC" : Return lk.MBFC
            Case "MCategory" : Return lk.MCategory
            Case "MKeywords" : Return lk.MKeywords
            Case "MHistory" : Return lk.MHistory
            Case "MComments" : Return lk.MComments
            Case "MExport" : Return lk.MExport
                '#Options Settings########;
            Case "Options" : Return lk.Options
            Case "SettingsTitle" : Return lk.SettingsTitle
            Case "SMetaDefaults" : Return lk.SMetaDefaults
            Case "SUndoRedo" : Return lk.SUndoRedo
            Case "SMaxUndo" : Return lk.SMaxUndo
            Case "SDisableUndo" : Return lk.SDisableUndo
            Case "SStartup" : Return lk.SStartup
            Case "SStartFullscreen" : Return lk.SStartFullscreen
            Case "SImage" : Return lk.SImage
            Case "SPrefs" : Return lk.SPrefs
            Case "SMisc" : Return lk.SMisc
            Case "SAlternative" : Return lk.SAlternative
            Case "STemplateTop" : Return lk.STemplateTop
            Case "SAddLock" : Return lk.SAddLock
            Case "SRestore" : Return lk.SRestore
                '#Options Hotkeys########
            Case "Hotkeys" : Return lk.Hotkeys
            Case "Tfunction" : Return lk.Tfunction
            Case "KeyCombo" : Return lk.KeyCombo
            Case "Context1" : Return lk.Context1
            Case "Context2" : Return lk.Context2
            Case "LMB" : Return lk.LMB
            Case "RMB" : Return lk.RMB
            Case "Key00" : Return lk.Key00
            Case "Key01" : Return lk.Key01
            Case "Key02" : Return lk.Key02
            Case "Key03" : Return lk.Key03
            Case "Key04" : Return lk.Key04
            Case "Key05" : Return lk.Key05
            Case "Key06" : Return lk.Key06
            Case "Key07" : Return lk.Key07
            Case "Key08" : Return lk.Key08
            Case "Key09" : Return lk.Key09
            Case "Key10" : Return lk.Key10
            Case "Key11" : Return lk.Key11
            Case "Key12" : Return lk.Key12
            Case "Key13" : Return lk.Key13
            Case "Key14" : Return lk.Key14
            Case "Key15" : Return lk.Key15
            Case "Key16" : Return lk.Key16
            Case "Key17" : Return lk.Key17
            Case "Key18" : Return lk.Key18
            Case "Key19" : Return lk.Key19
            Case "Key20" : Return lk.Key20
            Case "Key21" : Return lk.Key21
            Case "Key22" : Return lk.Key22
            Case "Key23" : Return lk.Key23
            Case "Key24" : Return lk.Key24
            Case "Key25" : Return lk.Key25
            Case "Key26" : Return lk.Key26
            Case "Key27" : Return lk.Key27
            Case "InvalidKey" : Return lk.InvalidKey
            Case "KeyInUse" : Return lk.KeyInUse
            Case "SetKey" : Return lk.SetKey
            Case "KeyMsg" : Return lk.KeyMsg
                '#Options Colours########
            Case "Tobject" : Return lk.Tobject
            Case "ObjectColour" : Return lk.ObjectColour
            Case "Colours" : Return lk.Colours
            Case "C00" : Return lk.C00
            Case "C01" : Return lk.C01
            Case "C02" : Return lk.C02
            Case "C03" : Return lk.C03
            Case "C04" : Return lk.C04
            Case "C05" : Return lk.C05
            Case "C06" : Return lk.C06
            Case "C07" : Return lk.C07
            Case "C08" : Return lk.C08
            Case "C09" : Return lk.C09
            Case "C10" : Return lk.C10
            Case "C11" : Return lk.C11
            Case "Old" : Return lk.Old
            Case "Tnew" : Return lk.Tnew
                '#Primitive Creation########
            Case "POrigin" : Return lk.POrigin
            Case "PHeight" : Return lk.PHeight
            Case "PWidth" : Return lk.PWidth
            Case "PFrameSize" : Return lk.PFrameSize
            Case "PRingTitle" : Return lk.PRingTitle
            Case "PRadius" : Return lk.PRadius
            Case "PResolution" : Return lk.PResolution
            Case "PName" : Return lk.PName
            Case "PFilename" : Return lk.PFilename
                '#Bitmap Export
            Case "BTitle" : Return lk.BTitle
            Case "BLdrawColours" : Return lk.BLdrawColours
            Case "BChoosenColours" : Return lk.BChoosenColours
            Case "BDimension" : Return lk.BDimension
            Case "BFree" : Return lk.BFree
                '#MsgBox Titles########
            Case "Info" : Return lk.Info
            Case "Question" : Return lk.Question
            Case "Fatal" : Return lk.Fatal
            Case "Warning" : Return lk.Warning
                '#OK, Cancel and Abort Button#######
            Case "OK" : Return lk.OK
            Case "ABORT" : Return lk.ABORT
            Case "Cancel" : Return lk.Cancel
            Case "Create" : Return lk.Create
            Case "Apply" : Return lk.Apply
                '#Spliner########
            Case "SplineMenu" : Return lk.SplineMenu
            Case "SplineStart" : Return lk.SplineStart
            Case "SplineFirstDir" : Return lk.SplineFirstDir
            Case "SplineNext" : Return lk.SplineNext
            Case "SplineNextDir" : Return lk.SplineNextDir
            Case "SplineSegCount" : Return lk.SplineSegCount
        End Select
        Return -1
    End Function

    Public Shared Function parse(ByVal s As String) As String
        Return Replace(s, "<br>", vbCrLf)
    End Function

    Public Shared Function parseVersion(ByVal s As String) As String
        Return Replace(Replace(s, "<Version>", Mid(My.Application.Info.Version.ToString(), 1, 5)), "<Year>", My.Application.Info.Copyright.Split(CChar(" "))(5))
    End Function

    Public Shared Function globalize(ByVal s As String) As String
        Return Replace(s, MathHelper.comma, ".")
    End Function

End Class
