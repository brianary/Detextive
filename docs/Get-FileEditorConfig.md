---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version: https://msdn.microsoft.com/library/s064f8w2aspx
schema: 2.0.0
---

# Get-FileEditorConfig

## SYNOPSIS
Looks up the editorconfig values set for a file.

## SYNTAX

```
Get-FileEditorConfig [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```ps1
Get-FileEditorConfig README.md
```

```
Path          : A:\Detextive\README.md
Encoding      : System.Text.UTF8Encoding+UTF8EncodingSealed
Utf8Signature : False
Indents       : Spaces
IndentSize    : 4
LineEndings   : CRLF
FinalNewline  : True
StringValue   : A:\Detextive\README.md: utf-8, Spaces indents, CRLF line endings
```

## PARAMETERS

### -Path
{{ Fill Path Description }}

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

### `Detextive.FileEditorConfigResult`

* **Path** `string`: The full path of the file.
* **Encoding** `Encoding`: Contains the desired encoding of the text file.
* **Utf8Signature** `bool`: Indicates the file should begin with a UTF-8 signature.
* **Indents** [`IndentType`][]: Indicates the type of indent characters desired for the text file.
* **IndentSize** `int`: The number of spaces in an indent, or the number of characters a tab should render as.
* **LineEndings** [`LineEndingType`][]: Indicates the type of line endings desired for the text file.
* **FinalNewline** `bool`: Indicates the file should end with a newline as required by the POSIX standard.
* **StringValue** `string`: A summary of the file config.

[`IndentType`]: Get-FileIndents.md#OUTPUTS
[`LineEndingType`]: Get-FileLineEndings.md#OUTPUTS

## NOTES

## RELATED LINKS
