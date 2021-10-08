---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-Windows1252

## SYNOPSIS
Returns true if a file contains at least one byte identified as a likely Windows-1252/CP1252 character value.

## SYNTAX

```
Test-Windows1252 [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```ps1
Test-Windows1252 README.md
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

Any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

`System.Boolean` indicating the file contains at least one byte identified as a likely Windows-1252/CP1252 character value.

## NOTES

## RELATED LINKS
