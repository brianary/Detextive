<#
.Synopsis
	Publish the module in this repo to the PowerShell Gallery.
#>

#Requires -Version 7.2
[CmdletBinding()] Param()

dotnet publish -c Release

$MSBuildProjectName = [io.path]::GetFileNameWithoutExtension("$(Resolve-Path ./src/*/*.fsproj)")
$env:PSModulePath -split ';' |
	ForEach-Object {"$_/$MSBuildProjectName"} |
	Where-Object {Test-Path $_ -Type Container} |
	Remove-Item -Recurse -Force

Push-Location "$(Resolve-Path src/*/bin/Release/*/publish)"
Import-LocalizedData Module -FileName $MSBuildProjectName
$Version = $Module.ModuleVersion
$InstallPath = "$env:UserProfile/Documents/PowerShell/Modules/$MSBuildProjectName/$Version"
if(!(Test-Path $InstallPath -Type Container)) {mkdir $InstallPath}
Copy-Item * -Destination $InstallPath
Pop-Location

Publish-Module -Name $MSBuildProjectName -NuGetApiKey $env:gallerykey
