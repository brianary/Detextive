# Pester tests, see https://github.com/Pester/Pester/wiki
$TestRoot = "$PSScriptRoot\test"
$AsByteStream =
	if((Get-Command Get-Content -ParameterName AsByteStream -ErrorAction SilentlyContinue)) {@{AsByteStream=$true}}
	else {@{Encoding='Byte'}}
$psd1 = Resolve-Path ./src/*/bin/Debug/*/*.psd1
if(1 -lt ($psd1 |Measure-Object).Count) {throw "Too many module binaries found: $psd1"}
Import-LocalizedData -BindingVariable manifest -BaseDirectory ./src/* -FileName (Split-Path $PWD -Leaf)
# ensure the right cmdlets are tested
$manifest.CmdletsToExport |Get-Command -CommandType Cmdlet -EA 0 |Remove-Item
$module = Import-Module (Resolve-Path ./src/*/bin/Debug/*/*.psd1) -PassThru -vb
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
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\* -File |
				foreach {@{ File = $_.FullName; Expected = $_.Name -notlike 'binary.*' }}
		) {
			Param($File,$Expected)
			Test-TextFile $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-BinaryFile cmdlet' -Tag Cmdlet,Test-BinaryFile {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\* -File |
				foreach {@{ File = $_.FullName; Expected = $_.Name -like 'binary.*' }}
		) {
			Param($File,$Expected)
			Test-BinaryFile $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-Utf8Signature cmdlet' -Tag Cmdlet,Test-Utf8Signature {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\* -File |
				foreach {@{ File = $_.FullName; Expected = $_.Name -like 'utf-8-bom-*' }}
		) {
			Param($File,$Expected)
			Test-Utf8Signature $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-Utf8Encoding cmdlet' -Tag Cmdlet,Test-Utf8Encoding {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\* -File |
				foreach {@{ File = $_.FullName; Expected = $_.Name -like 'utf-8-*' -or $_.Name -like 'ascii-*' }}
		) {
			Param($File,$Expected)
			Test-Utf8Encoding $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Test-FinalNewline cmdlet' -Tag Cmdlet,Test-FinalNewline {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\* -File |
				foreach {@{ File = $_.FullName; Expected = $_.Name -notlike 'binary.*' -and $_.Name -notlike '*-none-none.txt' }}
		) {
			Param($File,$Expected)
			Test-FinalNewline $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Get-FileEncoding cmdlet' -Tag Cmdlet,Get-FileEncoding {
		It "Given the file '<File>', '<Expected>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\*.txt,$TestRoot\*.ebcdic -File |
				foreach {
					@{ File = $_.FullName; Expected = switch -Wildcard ($_.Name) {
						ascii-*        {@('us-ascii')}
						ebcdic-*       {@('ibm037')}
						latin1-*       {@('iso-8859-1','us-ascii')}
						utf-8-*        {@('utf-8','us-ascii')}
						utf-16be-*     {@('utf-16BE')}
						utf-16le-*     {@('utf-16')}
						utf-32be-*     {@('utf-32BE')}
						utf-32le-*     {@('utf-32')}
						windows-1252-* {@('windows-1252','iso-8859-1','us-ascii')}
						default        {@([Text.Encoding]::Default.WebName)}
					} }
				}
		) {
			Param($File,$Expected)
			(Get-FileEncoding $File -vb).WebName |Should -BeIn $Expected
		}
	}
	Context 'Get-FileIndents cmdlet' -Tag Cmdlet,Get-FileIndents {
		It "Given the file '<File>', '<Indents>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\*.txt -File |
				foreach {
					$ind = ([io.path]::GetFileNameWithoutExtension($_.Name) -split '-')[-2]
					@{ File = $_.FullName; Indents = switch($ind){ mixedi {'Mixed'} tab {'Tabs'} space {'Spaces'} none {'None'} default {'Other'} } }
				}
		) {
			Param($File,$Indents)
			$e = Get-FileIndents $File -vb
			$e.Indents |Should -BeExactly $Indents
			# not really distinguishing intra-line mixed and inter-line mixed, so skip that check
			if($Indents -notin 'Mixed','None') {$e.$Indents |Should -BeGreaterThan 0}
			$e.Tabs |Should -BeGreaterOrEqual 0
			$e.Spaces |Should -BeGreaterOrEqual 0
			$e.Mixed |Should -BeGreaterOrEqual 0
			$e.Other |Should -BeGreaterOrEqual 0
		}
		It "Given the code file '<File>', '<Indents>' should be returned." -TestCases @(
			@{ File = "$TestRoot\*.ps1"; Indents = 'Tabs' }
		) {
			Param($File,$Indents)
			$e = Get-FileIndents $File -vb
			$e.Indents |Should -BeExactly $Indents
			# not really distinguishing intra-line mixed and inter-line mixed, so skip that check
			if($Indents -notin 'Mixed','None') {$e.$Indents |Should -BeGreaterThan 0}
			$e.Tabs |Should -BeGreaterOrEqual 0
			$e.Spaces |Should -BeGreaterOrEqual 0
			$e.Mixed |Should -BeGreaterOrEqual 0
			$e.Other |Should -BeGreaterOrEqual 0
		}
	}
	Context 'Get-FileLineEndings cmdlet' -Tag Cmdlet,Get-FileLineEndings {
		It "Given the file '<File>', '<LineEndings>' should be returned." -TestCases (
			Get-ChildItem $TestRoot\*.txt -File |
				foreach {
					$end = ([io.path]::GetFileNameWithoutExtension($_.Name) -split '-')[-1]
					@{ File = $_.FullName; LineEndings = switch($end){ mixedle {'Mixed'} none {'None'} default {$end.ToUpperInvariant()} } }
				}
		) {
			Param($File,$LineEndings)
			$e = Get-FileLineEndings $File -vb
			$e.LineEndings |Should -BeExactly $LineEndings
			if($LineEndings -notin 'Mixed','None') {$e.$LineEndings |Should -BeGreaterThan 0}
			$e.CRLF |Should -BeGreaterOrEqual 0
			$e.LF |Should -BeGreaterOrEqual 0
			$e.CR |Should -BeGreaterOrEqual 0
			$e.NEL |Should -BeGreaterOrEqual 0
			$e.LS |Should -BeGreaterOrEqual 0
			$e.PS |Should -BeGreaterOrEqual 0
		}
	}
	Context 'Get-FileContentsInfo cmdlet' -Tag Cmdlet,Get-FileContentsInfo {
		It "Given the file '<File>', IsBinary should be '<IsBinary>'." -TestCases (
			Get-ChildItem $TestRoot\binary.* -File |
				foreach {@{ File = $_.FullName; IsBinary = $true }}
		) {
			Param($File,$IsBinary)
			$e = Get-FileContentsInfo $File -vb
			$e.IsBinary |Should -BeTrue
		}
		It "Given the file '<File>', {'<Encoding>' '<Indents>' '<LineEndings>'} should be returned." -TestCases (
			Get-ChildItem $TestRoot\binary.* -File |
				where {$_.Name -notlike 'binary.*'} |
				foreach {@{
					File = $_.FullName
					IsBinary = $false
					Utf8Signature = $_.Name -like 'utf-8-bom-*'
					Indents = switch -Wildcard ($_.Name)
					{
						*-tab-*    {'Tabs'}
						*-space-*  {'Spaces'}
						*-emsp-*   {'Other'}
						*-mixedi-* {'Mixed'}
						*-none-*   {'None'}
					}
					LineEndings = switch -Wildcard ($_.Name)
					{
						*-crlf.*    {'CRLF'}
						*-lf.*      {'LF'}
						*-cr.*      {'CR'}
						*-nel.*     {'NEL'}
						*-ls.*      {'LS'}
						*-ps.*      {'PS'}
						*-mixedle.* {'Mixed'}
						*-none.*    {'None'}
					}
					FinalNewline = $_.Name -notlike '*-none.*'
				}}
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
	Context 'Get-FileEditorConfig cmdlet' -Tag Cmdlet,Get-FileEditorConfig {
		It "Given the file '<File>', {'<Encoding>' '<Indents>' '<LineEndings>'} should be returned." -TestCases @(
			@{ File = "$TestRoot\..\README.md"; Encoding = 'utf-8'; Utf8Signature = $false
				Indents = 'Spaces'; LineEndings = 'CRLF'; FinalNewline = $true }
			@{ File = "$TestRoot\..\Detextive.svg"; Encoding = 'utf-8'; Utf8Signature = $false
				Indents = 'Spaces'; LineEndings = 'CRLF'; FinalNewline = $true }
		) {
			Param($File,$Encoding,$Utf8Signature,$Indents,$LineEndings,$FinalNewline)
			$e = Get-FileEditorConfig $File -vb
			$e.Encoding.WebName |Should -BeExactly $Encoding
			$e.Utf8Signature |Should -BeExactly $Utf8Signature
			$e.Indents |Should -BeExactly $Indents
			$e.LineEndings |Should -BeExactly $LineEndings
			$e.FinalNewline |Should -BeExactly $FinalNewline
		}
	}
	Context 'Add-Utf8Signature cmdlet' -Tag Cmdlet,Add-Utf8Signature {
		It "Given the bytes '<Bytes>', the updated file should contain bytes '<Content>'." -TestCases @(
			@{ Bytes = [byte[]]@(); Content = [byte[]]@(0xEF,0xBB,0xBF) }
			@{ Bytes = [byte[]]@(0x0A); Content = [byte[]]@(0xEF,0xBB,0xBF,0x0A) }
			@{ Bytes = [byte[]]@(0xEF,0xBB,0xBF,0x0A); Content = [byte[]]@(0xEF,0xBB,0xBF,0x0A) }
			@{ Bytes = [byte[]]@(0x4F,0x4B,0x0A); Content = [byte[]]@(0xEF,0xBB,0xBF,0x4F,0x4B,0x0A) }
			@{ Bytes = [byte[]]@(0xF0,0x9F,0x86,0x97,0x0A); Content = [byte[]]@(0xEF,0xBB,0xBF,0xF0,0x9F,0x86,0x97,0x0A) }
		) {
			Param($Bytes,$Content)
			$file = [io.path]::GetTempFileName()
			$Bytes |Set-Content $file @AsByteStream
			Add-Utf8Signature $file -vb
			Get-Content $file @AsByteStream |Should -Be $Content
			Remove-Item $file
		}
	}
	Context 'Remove-Utf8Signature cmdlet' -Tag Cmdlet,Remove-Utf8Signature {
		It "Given the bytes '<Bytes>', the updated file should contain bytes '<Content>'." -TestCases @(
			@{ Bytes = [byte[]]@(); Content = [byte[]]@() }
			@{ Bytes = [byte[]]@(0xEF,0xBB,0xBF); Content = [byte[]]@() }
			@{ Bytes = [byte[]]@(0xEF,0xBB,0xBF,0x0A); Content = [byte[]]@(0x0A) }
			@{ Bytes = [byte[]]@(0x0A); Content = [byte[]]@(0x0A) }
			@{ Bytes = [byte[]]@(0xEF,0xBB,0xBF,0x4F,0x4B,0x0A); Content = [byte[]]@(0x4F,0x4B,0x0A) }
			@{ Bytes = [byte[]]@(0xEF,0xBB,0xBF,0xF0,0x9F,0x86,0x97,0x0A); Content = [byte[]]@(0xF0,0x9F,0x86,0x97,0x0A) }
		) {
			Param($Bytes,$Content)
			$file = [io.path]::GetTempFileName()
			$Bytes |Set-Content $file @AsByteStream
			Remove-Utf8Signature $file -vb
			Get-Content $file @AsByteStream |Should -Be $Content
			Remove-Item $file
		}
	}
	Context 'Test-BrokenEncoding cmdlet' -Tag Cmdlet,Test-BrokenEncoding {
		It "Given the string '<InputObject>', the value '<Expected>' should be returned." -TestCases @(
			@{ InputObject = ' '; Expected = $false }
			@{ InputObject = 'SmartQuotes Arenâ€™t'; Expected = $true }
			@{ InputObject = '1.2.1 â€“ 1.3.4'; Expected = $true }
			@{ InputObject = 'Test'; Expected = $false }
		) {
			Param($InputObject,$Expected)
			$InputObject |Test-BrokenEncoding -vb |Should -BeExactly $Expected
		}
		It "Given the file contents '<InputObject>', the file should be updated to contain '<Expected>'." -TestCases @(
			@{ InputObject = ' '; Expected = $false }
			@{ InputObject = 'SmartQuotes Arenâ€™t'; Expected = $true }
			@{ InputObject = '1.2.1 â€“ 1.3.4'; Expected = $true }
			@{ InputObject = 'Test'; Expected = $false }
		) {
			Param($InputObject,$Expected)
			$file = [io.path]::GetTempFileName()
			$InputObject |Set-Content $file -Encoding utf8
			Test-BrokenEncoding $file -vb |Should -BeExactly $Expected
			Remove-Item $file
		}
	}
	Context 'Repair-Encoding cmdlet' -Tag Cmdlet,Repair-Encoding {
		It "Given the string '<InputObject>', the string '<Expected>' should be returned." -TestCases @(
			@{ InputObject = 'SmartQuotes Arenâ€™t'; Expected = "SmartQuotes Aren’t" }
			@{ InputObject = '1.2.1 â€“ 1.3.4'; Expected = "1.2.1 – 1.3.4" }
		) {
			Param($InputObject,$Expected)
			$InputObject |Repair-Encoding -vb |Should -BeExactly $Expected
		}
		It "Given the file contents '<InputObject>', the file should be updated to contain '<Expected>'." -TestCases @(
			@{ InputObject = 'SmartQuotes Arenâ€™t'; Expected = "SmartQuotes Aren’t" }
			@{ InputObject = '1.2.1 â€“ 1.3.4'; Expected = "1.2.1 – 1.3.4" }
		) {
			Param($InputObject,$Expected)
			$file = [io.path]::GetTempFileName()
			$InputObject |Set-Content $file -Encoding utf8
			Repair-Encoding $file -vb
			Get-Content $file -Encoding utf8 -TotalCount 1 |Should -BeExactly $Expected
			Remove-Item $file
		}
	}
	Context 'Test-FileEditorConfig cmdlet' -Tag Cmdlet,Test-FileEditorConfig {
		It "Given the file '<File>', the result '<Expected>' should be returned." -TestCases @(
			@{ File = "$TestRoot\..\README.md"; Expected = $true }
			@{ File = "$TestRoot\..\Detextive.Tests.ps1"; Expected = $false }
			@{ File = "$TestRoot\..\test.cmd"; Expected = $true }
		) {
			Param($File,$Expected)
			Test-FileEditorConfig $File -vb |Should -BeExactly $Expected
		}
	}
	Context 'Repair-FileEditorConfig cmdlet' -Tag Cmdlet,Test-FileEditorConfig {
		It "Given the text '<InputObject>', the result '<Expected>' should be returned." -TestCases @(
			@{ InputObject = "    # test`n"; Extension = 'ps1'; Encoding = New-Object Text.UTF8Encoding $true;
				Expected = "`t# test`r`n" }
		) {
			Param($InputObject,$Extension,$Encoding,$Expected)
			$file = "$([guid]::NewGuid()).$Extension"
			$Encoding.GetBytes($InputObject) |Set-Content $file @AsByteStream
			Get-Content $file -Raw |Should -BeExactly $InputObject
			Repair-FileEditorConfig $file -vb
			Get-Content $file -Raw |Should -BeExactly $Expected
			Remove-Item $file
		}
	}
}.GetNewClosure()
$env:Path = $envPath
