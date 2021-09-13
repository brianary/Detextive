# Pester tests, see https://github.com/Pester/Pester/wiki
$TestRoot = $PSScriptRoot
$module = Import-Module (Resolve-Path ./src/*/bin/Debug/*/*.psd1) -PassThru -vb
Import-LocalizedData -BindingVariable manifest -BaseDirectory ./src/* -FileName (Split-Path $PWD -Leaf)
Describe $module.Name {
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
			$env:Path = '' # avoid testing the wrong cmdlets
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
			$env:Path = '' # avoid testing the wrong cmdlets
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
			$env:Path = '' # avoid testing the wrong cmdlets
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
			$env:Path = '' # avoid testing the wrong cmdlets
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
			$env:Path = '' # avoid testing the wrong cmdlets
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
			$env:Path = '' # avoid testing the wrong cmdlets
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
			$env:Path = '' # avoid testing the wrong cmdlets
			(Get-FileEncoding $File -vb).WebName |Should -BeExactly $Expected
		}
	}
}.GetNewClosure()
$env:Path = $envPath
