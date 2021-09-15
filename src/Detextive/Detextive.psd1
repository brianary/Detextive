# see https://docs.microsoft.com/powershell/scripting/developer/module/how-to-write-a-powershell-module-manifest
# and https://docs.microsoft.com/powershell/module/microsoft.powershell.core/new-modulemanifest
@{
RootModule = 'Detextive.dll'
ModuleVersion = '1.0.0'
CompatiblePSEditions = @('Core')
GUID = '2dd84299-7cd8-443d-86a8-16f82a834e65'
Author = 'Brian Lalonde'
CompanyName = 'Unknown'
Copyright = 'Copyright © 2019 Brian Lalonde'
Description = 'Investigates data to determine what the textual characteristics are.'
PowerShellVersion = '6.0'
FunctionsToExport = @()
CmdletsToExport = @('Test-TextFile','Test-BinaryFile','Test-Utf8Signature','Test-Utf8Encoding','Test-FinalNewline','Test-Windows1252',
    'Get-FileEncoding','Get-FileIndents','Get-FileLineEndings')
VariablesToExport = @()
AliasesToExport = @()
FileList = @('Detextive.dll','Detextive.dll-Help.xml')
PrivateData = @{
    PSData = @{
        Tags = @('Text','detect','encoding','line-endings','indents','bom','utf-sig','utf8-sig')
        LicenseUri = 'https://github.com/brianary/Detextive/blob/master/LICENSE'
        ProjectUri = 'https://github.com/brianary/Detextive/'
        IconUri = 'http://webcoder.info/images/Detextive.svg'
        # ReleaseNotes = ''
    }
}
}
