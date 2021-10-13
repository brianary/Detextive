---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-BinaryFile

## SYNOPSIS
Returns true if a file does not appear to contain parseable text, and presumably contains binary data.

## SYNTAX

```
Test-BinaryFile [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```ps1
Test-BinaryFile Detextive.png
```

```
True
```

### EXAMPLE 2
```
Test-BinaryFile.ps1 README.md
```

False

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

### `System.Boolean` indicating that the file does not appear to contain parseable text, and presumably contains binary data.

## NOTES

## RELATED LINKS

[Test-TextFile.ps1]()

