---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Get-FileContentsInfo

## SYNOPSIS
Returns whether the file is binary or text, and what encoding, line endings, and indents text files contain.

## SYNTAX

```
Get-FileContentsInfo [[-Path] <String>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
Get-FileContentsInfo.ps1 Get-FileContentsInfo.ps1
```

Path          : A:\scripts\Get-FileContentsInfo.ps1
IsBinary      : False
Encoding      : System.Text.ASCIIEncoding+ASCIIEncodingSealed
Utf8Signature : False
LineEndings   : CRLF
Indents       : Tabs
FinalNewline  : True

## PARAMETERS

### -Path
The location of a file.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Any object with a Path or FullName property to use for a file location.
### None

## OUTPUTS

### System.Management.Automation.PSObject with the following properties:
### * Path the full path of the file.
### * IsBinary indicates a binary (vs text) file.
### * Encoding contains the encoding of a text file.
### * LineEndings indicates the type of line endings used in a text file.
### * Indents indicates the type of indent characters used in a text file.
### Detextive.IndentsResult

## NOTES
TODO: indent size, trailing whitespace, max line length, 2σ (95% max line length)

## RELATED LINKS
