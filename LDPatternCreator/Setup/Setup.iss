#define MyAppName "LD Pattern Creator"
#define MyAppVersion "1.7.3"
#define MyAppPublisher "Nils Schmidt"
#define MyAppExeName "LDPatternCreator.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId=8475C4ED-3F16-4CE6-8A06-5ACAD392E575
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\LDPatternCreator
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\License.rtf
OutputDir=C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\Setup\Final
OutputBaseFilename=LPC_1_7_3_Setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "danish"; MessagesFile: "compiler:Languages\Danish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\LDPatternCreator.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\Deveel.Math.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\changelog.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\925 Brick  1 x 8 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\973 Torso (Front and Black Neck Mark).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\973 Torso (Front and Neck mark).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\973 Torso (Front and White Neck Mark).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\973 Torso (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\2431 Tile  1 x  4 with Groove.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\2465 Brick  1 x 16 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\2525 Flag  6 x  4 (Front and Back).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\2586 Minifig Shield Ovoid.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3001 Brick  2 x  4 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3002 Brick  2 x  3 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3003 Brick  2 x  2 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3004 Brick  1 x  2 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3005 Brick  1 x  1 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3007 Brick  2 x  8 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3008 Brick  1 x  8 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3010 Brick  1 x  4 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3024 Plate  1 x  1 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3037 Slope Brick 45  2 x  4.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3038 Slope Brick 45  2 x  3.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3039 Slope Brick 45  2 x  2.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3040 Slope Brick 45  2 x  1.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3067 Brick  1 x  6 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3068b Tile  2 x  2 with Groove.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3069b Tile  1 x  2 with Groove.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3070b Tile  1 x  1 with Groove.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3297 Slope Brick 33  3 x  4.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3298 Slope Brick 33  3 x  2.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3622 Brick  1 x 3 (Front).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3846 Minifig Shield Triangular.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\3939 Slope Brick 33  3 x  6.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\30350 Tile  2 x  3 with Horizontal Clips.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\98138 Tile  1 x  1 Round with Groove.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\85984 Slope 1 x  1 x 0.667 31 (Top).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion

Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\6636 Tile  1 x  6 (Top).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\2335 Flag  2 x  2 (Right).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\98138 Tile  1 x  1 Round with Groove.txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\template\4162 Tile  1 x  8 (Top).txt"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\template"; Flags: ignoreversion

Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\form.dat"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\License.rtf"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\Template_HowTo.pdf"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\Setup\LPC.ico"; DestDir: "{app}"; Flags: ignoreversion

Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\readme.htm"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\html\01_01.png"; DestDir: "{app}\html"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\html\01_02.png"; DestDir: "{app}\html"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\html\02.png"; DestDir: "{app}\html"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\html\03.png"; DestDir: "{app}\html"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\html\04.png"; DestDir: "{app}\html"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\html\05.png"; DestDir: "{app}\html"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\html\06.png"; DestDir: "{app}\html"; Flags: ignoreversion


Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\lang\lang_fr_FR.csv"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\lang"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\lang\lang_de_DE.csv"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\lang"; Flags: ignoreversion
Source: "C:\Users\nilss\Documents\Visual Studio 2015\Projects\LDPatternCreator\LDPatternCreator\bin\Release\lang\lang_en_GB.csv"; DestDir: "{userappdata}\Nils Schmidt\LDPatternCreator\lang"; Flags: ignoreversion

[UninstallDelete]
Type: filesandordirs; Name: "{app}"
Type: filesandordirs; Name: "{userappdata}\Nils Schmidt\LDPatternCreator\{#MyAppVersion}"
Type: filesandordirs; Name: "{userappdata}\Nils Schmidt\LDPatternCreator\lang\*"
Type: filesandordirs; Name: "{userappdata}\Nils Schmidt\LDPatternCreator\*.exe"
Type: filesandordirs; Name: "{userappdata}\Nils Schmidt\LDPatternCreator\*.dll"
Type: filesandordirs; Name: "{userappdata}\Nils Schmidt\LDPatternCreator\*.txt"
Type: filesandordirs; Name: "{userappdata}\Nils Schmidt\LDPatternCreator\Primitives.cfg"
Type: filesandordirs; Name: "{userappdata}\Nils Schmidt\LDPatternCreator\*.ldr"

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
Root: HKCR; Subkey: ".lpc"; ValueType: string; ValueName: ; ValueData: "lpcfile"; Flags: uninsdeletekey
Root: HKCR; Subkey: "lpcfile\DefaultIcon"; ValueType: string; ValueName: ; ValueData: "{app}\LPC.ico"; Flags: uninsdeletekey
Root: HKCR; Subkey: "lpcfile\shell\open\command"; ValueType: string; ValueName: ; ValueData: "{app}\{#MyAppExeName} %1"; Flags: uninsdeletekey



[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, "&", "&&")}}"; Flags: nowait postinstall skipifsilent

