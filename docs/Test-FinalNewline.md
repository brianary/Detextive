---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-FinalNewline

## SYNOPSIS
Returns true if a file ends with a newline as required by the POSIX standard for text files.

## SYNTAX

```
Test-FinalNewline [[-Path] <String>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
Test-FinalNewline.ps1 README.md
```

True

## PARAMETERS

### -Path
The file to test.

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

### System.IO.FileInfo file or similar object to test for UTF-8 validity.
### None

## OUTPUTS

### System.Boolean indicating whether the file ends with a newline.
### System.Boolean

## NOTES

## RELATED LINKS

[Test-MagicNumber.ps1]()

