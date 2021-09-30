# Pester tests, see https://github.com/Pester/Pester/wiki
$TestRoot = $PSScriptRoot
$module = Import-Module (Resolve-Path ./src/*/bin/Debug/*/*.psd1) -PassThru -vb
Import-LocalizedData -BindingVariable manifest -BaseDirectory ./src/* -FileName (Split-Path $PWD -Leaf)
Describe $module.Name {
	$env:Path = $env:Path -replace ';A:\\Scripts'
	Context "$($module.Name) module" -Tag Module {
		It "Given the module, the version should match the manifest version" {
			$module.Version |Should -BeExactly $manifest.ModuleVersion
		}
		It "Given the module, the DLL file version should match the manifest version" {
			(Get-Item "$($module.ModuleBase)\$($module.Name).dll").VersionInfo.FileVersionRaw |
				Should -BeLike "$($manifest.ModuleVersion)*"
		}
		It "Given the module, the DLL product version should match the manifest version" {
			(Get-Item "$($module.ModuleBase)\$($module.Name).dll").VersionInfo.ProductVersionRaw |
				Should -BeLike "$($manifest.ModuleVersion)*"
		} -Pending
		It "Given the module, the DLL should have a valid semantic product version" {
			$v = (Get-Item "$($module.ModuleBase)\$($module.Name).dll").VersionInfo.ProductVersion
			[semver]::TryParse($v, [ref]$null) |Should -BeTrue
		} -Pending
	}
	Context 'Test-TextFile cmdlet' -Tag Cmdlet,Test-TextFile {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Expected = $true }
			@{ File = "$TestRoot\Detextive.png"; Expected = $false }
			@{ File = "$TestRoot\Detextive.svg"; Expected = $true }
		) {
			Param($File,$Expected)
			Test-TextFile $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-BinaryFile cmdlet' -Tag Cmdlet,Test-BinaryFile {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Expected = $false }
			@{ File = "$TestRoot\Detextive.png"; Expected = $true }
			@{ File = "$TestRoot\Detextive.svg"; Expected = $false }
		) {
			Param($File,$Expected)
			Test-BinaryFile $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-Utf8Signature cmdlet' -Tag Cmdlet,Test-Utf8Signature {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Expected = $false }
			@{ File = "$TestRoot\Detextive.png"; Expected = $false }
			@{ File = "$TestRoot\Detextive.svg"; Expected = $false }
			@{ File = "$TestRoot\Detextive.sln"; Expected = $true }
		) {
			Param($File,$Expected)
			Test-Utf8Signature $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-Utf8Encoding cmdlet' -Tag Cmdlet,Test-Utf8Encoding {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Expected = $true }
			@{ File = "$TestRoot\Detextive.png"; Expected = $false }
			@{ File = "$TestRoot\Detextive.svg"; Expected = $true }
			@{ File = "$TestRoot\Detextive.sln"; Expected = $true }
		) {
			Param($File,$Expected)
			Test-Utf8Encoding $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-FinalNewline cmdlet' -Tag Cmdlet,Test-FinalNewline {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Expected = $true }
			@{ File = "$TestRoot\Detextive.png"; Expected = $false }
			@{ File = "$TestRoot\Detextive.svg"; Expected = $true }
			@{ File = "$TestRoot\Detextive.sln"; Expected = $true }
		) {
			Param($File,$Expected)
			Test-FinalNewline $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-Windows1252 cmdlet' -Tag Cmdlet,Test-Windows1252 {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Expected = $false }
			@{ File = "$TestRoot\Detextive.png"; Expected = $true }
			@{ File = "$TestRoot\Detextive.svg"; Expected = $false }
			@{ File = "$TestRoot\Detextive.sln"; Expected = $false }
		) {
			Param($File,$Expected)
			Test-Windows1252 $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Get-FileEncoding cmdlet' -Tag Cmdlet,Get-FileEncoding {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Expected = 'utf-8' }
			@{ File = "$TestRoot\Detextive.png"; Expected = 'windows-1252' }
			@{ File = "$TestRoot\Detextive.svg"; Expected = 'utf-8' }
			@{ File = "$TestRoot\Detextive.sln"; Expected = 'utf-8' }
		) {
			Param($File,$Expected)
			(Get-FileEncoding $File -vb).WebName |Should -BeExactly $Expected
		}
	}
	Context 'Get-FileIndents cmdlet' -Tag Cmdlet,Get-FileIndents {
		It "Given the file '<File>', '<Indents>' should be returned." -TestCases @(
			@{ File = "$TestRoot\Detextive.Tests.ps1"; Indents = 'Tabs' }
			@{ File = "$TestRoot\Detextive.svg"; Indents = 'Spaces' }
		) {
			Param($File,$Indents)
			$e = Get-FileIndents $File -vb
			$e.Indents |Should -BeExactly $Indents
			$e.Tabs |Should -BeGreaterOrEqual 0
			$e.Spaces |Should -BeGreaterOrEqual 0
			$e.Mixed |Should -BeGreaterOrEqual 0
			$e.Other |Should -BeGreaterOrEqual 0
		}
	}
	Context 'Get-FileLineEndings cmdlet' -Tag Cmdlet,Get-FileLineEndings {
		It "Given the file '<File>', '<LineEndings>' should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; LineEndings = 'CRLF' }
			@{ File = "$TestRoot\Detextive.png"; LineEndings = 'Mixed' }
			@{ File = "$TestRoot\Detextive.svg"; LineEndings = 'LF' }
		) {
			Param($File,$LineEndings)
			$e = Get-FileLineEndings $File -vb
			$e.LineEndings |Should -BeExactly $LineEndings
			$e.CRLF |Should -BeGreaterOrEqual 0
			$e.LF |Should -BeGreaterOrEqual 0
			$e.CR |Should -BeGreaterOrEqual 0
			$e.NEL |Should -BeGreaterOrEqual 0
			$e.LS |Should -BeGreaterOrEqual 0
			$e.PS |Should -BeGreaterOrEqual 0
		}
	}
	Context 'Get-FileContentsInfo cmdlet' -Tag Cmdlet,Get-FileLineEndings {
		It "Given the file '<File>', IsBinary should be '<IsBinary>'." -TestCases @(
			@{ File = "$TestRoot\Detextive.png"; IsBinary = $true }
		) {
			Param($File,$IsBinary)
			$e = Get-FileContentsInfo $File -vb
			$e.IsBinary |Should -BeTrue
		}
		It "Given the file '<File>', {'<Encoding>' '<Indents>' '<LineEndings>'} should be returned." -TestCases @(
			@{ File = "$TestRoot\README.md"; Encoding = 'utf-8'; Utf8Signature = $false
				Indents = 'Mixed'; LineEndings = 'CRLF'; FinalNewline = $true }
			@{ File = "$TestRoot\Detextive.svg"; Encoding = 'utf-8'; Utf8Signature = $false
				Indents = 'Spaces'; LineEndings = 'LF'; FinalNewline = $true }
		) {
			Param($File,$Encoding,$Utf8Signature,$Indents,$LineEndings,$FinalNewline)
			$e = Get-FileContentsInfo $File -vb
			$e.IsBinary |Should -BeFalse
			$e.Encoding.WebName |Should -BeExactly $Encoding
			$e.Utf8Signature |Should -BeExactly $Utf8Signature
			$e.Indents |Should -BeExactly $Indents
			$e.LineEndings |Should -BeExactly $LineEndings
			$e.FinalNewline |Should -BeExactly $FinalNewline
		}
	}
}.GetNewClosure()
$env:Path = $envPath
