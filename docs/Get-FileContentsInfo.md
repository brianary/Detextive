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
Get-FileContentsInfo [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```ps1
Get-FileContentsInfo README.md
```

```
Path          : A:\Detextive\README.md
IsBinary      : False
Encoding      : System.Text.UTF8Encoding+UTF8EncodingSealed
Utf8Signature : False
Indents       : None
LineEndings   : CRLF
FinalNewline  : True
StringValue   : A:\Detextive\README.md: utf-8, None indents, CRLF line endings
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

Any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

`Detextive.TextContentsResult`

* **Path** `string`: The full path of the file.
* **IsBinary** `bool`: Indicates a binary (vs text) file.
* **Encoding** `Encoding`: Contains the encoding of the text file.
* **Utf8Signature** `bool`: Indicates the file begins with a UTF-8 signature.
* **Indents** [`IndentsResult`][]: Details about the type of indent characters used in the text file.
* **LineEndings** [`LineEndingsResult`][]: Details about the type of line endings used in the text file.
* **FinalNewline** `bool`: Indicate the file ends with a newline as required by the POSIX standard.
* **StringValue** `string`: A summary of the file contents.

[`IndentsResult`]: Get-FileIndents.md#OUTPUTS
[`LineEndingsResult`]: Get-FileLineEndings.md#OUTPUTS

## NOTES

## RELATED LINKS
