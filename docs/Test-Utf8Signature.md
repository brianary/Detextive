---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-Utf8Signature

## SYNOPSIS
Returns true if a file starts with the optional UTF-8 BOM/signature.

## SYNTAX

```
Test-Utf8Signature [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
Test-Utf8Signature.ps1 README.md
```

False

## PARAMETERS

### -Path
The file to test.

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

### System.IO.FileInfo file or similar object to test for UTF-8 validity.
### None

## OUTPUTS

### System.Boolean indicating whether the file starts with a utf-8 signature (BOM).
### System.Boolean

## NOTES

## RELATED LINKS

[Test-FileTypeMagicNumber.ps1]()

