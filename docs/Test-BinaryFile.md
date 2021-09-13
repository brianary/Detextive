---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-BinaryFile

## SYNOPSIS
Indicates that a file contains binary data.

## SYNTAX

```
Test-BinaryFile [[-Path] <String>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
Test-BinaryFile.ps1 avatar.jpg
```

True

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

### None

## OUTPUTS

### System.Boolean indicating that the file contains binary data.
### System.Boolean

## NOTES

## RELATED LINKS

[Test-TextFile.ps1]()

