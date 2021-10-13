---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Get-FileLineEndings

## SYNOPSIS
Determines a file's line endings.

## SYNTAX

```
Get-FileLineEndings [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
Get-FileLineEndings README.md
```

```
Path        : A:\Detextive\README.md
LineEndings : CRLF
CRLF        : 48
LF          : 0
CR          : 0
NEL         : 0
LS          : 0
PS          : 0
StringValue : A:\Detextive\README.md: CRLF
```

## PARAMETERS

### -Path
The location of a file.

```yaml
Type: String
Parameter Sets: (All)
Aliases: FullName

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

### `Detextive.LineEndingsResult`

* **Path** `string`: The full path of the file.
* **LineEndings** `LineEndingType`: The file's indent style: `None`, `Mixed`, `CRLF`, `LF`, `CR`, `NEL`, `LS`, or `PS`.
* **CRLF** `int`: The number of lines ending with a Windows-style U+000D carriage return and U+000A line feed.
* **LF** `int`: The number of lines ending with a Linux/BSD-style U+000A line feed.
* **CR** `int`: The number of lines ending with an old-Mac-style U+000D carriage return.
* **NEL** `int`: The number of lines ending with a Unicode U+0085 next line character.
* **LS** `int`: The number of lines ending with a Unicode U+2028 line separator character.
* **PS** `int`: The number of lines ending with a Unicode U+2029 paragraph separator character.
* **StringValue** `string`: A summary of the file line endings.

## NOTES

## RELATED LINKS

[Get-FileEncoding.ps1]()

[Get-Content]()

