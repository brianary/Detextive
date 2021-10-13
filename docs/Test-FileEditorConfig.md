---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-FileEditorConfig

## SYNOPSIS
Validates a file's editorconfig settings against the actual formatting found.

## SYNTAX

```
Test-FileEditorConfig [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```ps1
Test-FileEditorConfig README.md
```

```
True
```

## PARAMETERS

### -Path
A file to test the editorconfig values for.

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

### `System.Boolean` indicating the file matches the editorconfig settings.

## NOTES

## RELATED LINKS
