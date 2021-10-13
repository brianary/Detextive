---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-TextFile

## SYNOPSIS
Indicates that a file contains text.

## SYNTAX

```
Test-TextFile [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```ps1
Test-TextFile README.md
```

```
True
```

### EXAMPLE 2
```ps1
Test-TextFile Detextive.png
```

```
False
```

## PARAMETERS

### -Path
A file to test.

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

### `System.Boolean` indicating that the file appears to contain text.

## NOTES

## RELATED LINKS

