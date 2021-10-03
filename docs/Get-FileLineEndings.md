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
Get-FileLineEndings.ps1 Get-FileLineEndings.ps1
```

Path        : A:\scripts\Get-FileLineEndings.ps1
LineEndings : CRLF
CRLF        : 90
LF          : 0
CR          : 0

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

### Any object with a Path or FullName property to use for a file location.
### None

## OUTPUTS

### Detextive.LineEndingsResult with the following properties:
### * Path, a string containing the location of the file.
### * LineEndings, one of: CRLF, LF, CR, NEL, LS, PS, or Mixed.
### * CRLF, a count of the U+000D U+000A CR LF line endings found.
### * LF, a count of the U+000A LF line endings found.
### * CR, a count of the U+000D CR line endings found.
### * NEL, a count of the U+0085 NEL line endings found.
### * LS, a count of the U+2028 LS line endings found.
### * PS, a count of the U+2029 PS line endings found.

## NOTES

## RELATED LINKS

[Get-FileEncoding.ps1]()

[Get-Content]()

