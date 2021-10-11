<#
.Synopsis
	Writes test files.
#>

#Requires -Version 7
[CmdletBinding()] Param()

function Export-TestFile($encoding,$indent,$lineending)
{
	# guard against invalid or troublesome combos
	if($indent -eq 'emsp' -and $encoding -in 'ascii','latin1','windows-1252','ebcdic') {return}
	if($lineending -in 'nel','ls','ps' -and $encoding -in 'ascii','latin1','windows-1252','ebcdic') {return}
	if($encoding -eq 'ebcdic' -and $lineending -eq 'mixed') {return}

	$enc = switch($encoding)
	{
		utf-8-bom {New-Object Text.Utf8Encoding $true}
		ebcdic    {[text.encoding]::GetEncoding('IBM037')}
		default   {[text.encoding]::GetEncoding($encoding)}
	}
	[byte[]] $pre = switch($encoding){'utf-8'{@()}default{$enc.GetPreamble()}}
	${    } = switch($indent){tab{"`t"}space{'    '}emsp{Get-Unicode.ps1 0x2003}default{"`t    "}}
	$mdash = switch($encoding){ascii{'--'}default{Get-Unicode.ps1 0x2014}}
	$ldquo = switch($encoding){ascii{'\"'}default{Get-Unicode.ps1 0x201C}}
	$rdquo = switch($encoding){ascii{'\"'}default{Get-Unicode.ps1 0x201D}}
	$tm = switch($encoding){ascii{'(tm)'}default{Get-Unicode.ps1 0x2122}}
	$eol = switch($lineending)
	{
		crlf    {"`r`n"}
		lf      {"`n"}
		cr      {"`r"}
		nel     {Get-Unicode.ps1 0x0085}
		ls      {Get-Unicode.ps1 0x2028}
		ps      {Get-Unicode.ps1 0x2029}
		default {[System.Environment]::NewLine}
	}
	$eol2 = switch($lineending)
	{
		mixedle {"`r`n"}
		default {$eol}
	}
	$ext = if($encoding -eq 'ebcdic'){'ebcdic'}else{'txt'}
	$pre + $enc.GetBytes(((@(
		"{"
		"${    }`"id`": 1,"
		"${    }`"title`": `"Testing $mdash for ${ldquo}Item$rdquo`","
		"${    }`"description`": `"Data$tm for test.`","
		"${    }`"options`": {$eol2${    }${    }`"size`": [`"S`",`"M`",`"L`"],$eol2${    }${    }`"style`": [`"shaker`",`"baroque`"]$eol2${    }}"
		"}"
	) -join $eol) + $eol)) |Set-Content "test/$encoding-$indent-$lineending.$ext" -AsByteStream -vb
}

# enumerate the values to permute
$encodings = 'ascii','latin1','utf-8-bom','utf-8','utf-16le','utf-16be','utf-32le','utf-32be','windows-1252','ebcdic'
$indents = 'tab','space','emsp','mixedi'
$lineendings = 'crlf','lf','cr','nel','ls','ps','mixedle'

Remove-Item test/*.txt,test/*.ebcdic -vb

foreach($encoding in $encodings)
{
	foreach($indent in $indents)
	{
		foreach($lineending in $lineendings)
		{
			Export-TestFile $encoding $indent $lineending
		}
	}
}
