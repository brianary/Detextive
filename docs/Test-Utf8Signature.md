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
```ps1
Test-Utf8Signature README.md
```

```
False
```

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

Any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

`System.Boolean` indicating that the file begins with the bytes `EF BB BF`.

## NOTES

## RELATED LINKS

