---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version: https://msdn.microsoft.com/library/dd383463.aspx
schema: 2.0.0
---

# Repair-FileEditorConfig

## SYNOPSIS
Corrects a file's editorconfig settings when they differ from the actual formatting found.

## SYNTAX

```
Repair-FileEditorConfig [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```ps1
Repair-FileEditorConfig README.md
```

Corrects the encoding, indents, and line endings if they differ from the editorconfig settings for the file.

## PARAMETERS

### -Path
A file to fix the editorconfig values for.

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

## NOTES

## RELATED LINKS
