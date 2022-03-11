# see https://docs.microsoft.com/powershell/scripting/developer/module/how-to-write-a-powershell-module-manifest
# and https://docs.microsoft.com/powershell/module/microsoft.powershell.core/new-modulemanifest
@{
RootModule = 'Detextive.dll'
ModuleVersion = '1.0.2'
CompatiblePSEditions = @('Core','Desktop')
GUID = '2dd84299-7cd8-443d-86a8-16f82a834e65'
Author = 'Brian Lalonde'
#CompanyName = 'Unknown'
Copyright = 'Copyright © 2019 Brian Lalonde; EditorConfig .NET library is © EditorConfig Team, used under MIT License'
Description = 'Investigates data to determine what the textual characteristics are.'
PowerShellVersion = '5.1'
FunctionsToExport = @()
CmdletsToExport = @('Test-TextFile','Test-BinaryFile','Test-Utf8Signature','Test-Utf8Encoding','Test-FinalNewline','Get-FileEncoding',
	'Get-FileIndents','Get-FileLineEndings','Get-FileContentsInfo','Add-Utf8Signature','Remove-Utf8Signature','Test-BrokenEncoding',
	'Repair-Encoding','Get-FileEditorConfig','Test-FileEditorConfig','Repair-FileEditorConfig')
VariablesToExport = @()
AliasesToExport = @()
FileList = @('Detextive.dll','EditorConfig.Core.dll','Detextive.dll-Help.xml')
PrivateData = @{
    PSData = @{
        Tags = @('text','detect','encoding','line-endings','indents','bom','utf-sig','utf8-sig')
        LicenseUri = 'https://github.com/brianary/Detextive/blob/master/LICENSE'
        ProjectUri = 'https://github.com/brianary/Detextive/'
        IconUri = 'http://webcoder.info/images/Detextive.svg'
        # ReleaseNotes = ''
    }
}
}
