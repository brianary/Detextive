---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version: https://msdn.microsoft.com/library/s064f8w2aspx
schema: 2.0.0
---

# Add-Utf8Signature

## SYNOPSIS
Adds the utf-8 signature (BOM) to a file.

## SYNTAX

```
Add-Utf8Signature [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```ps1
Add-Utf8Signature README.md
```

Adds the EF BB BF at the beginning of the file, warns if it's already there.

## PARAMETERS

### -Path
The file to add the utf-8 signature to.

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

## NOTES

## RELATED LINKS

[https://msdn.microsoft.com/library/s064f8w2aspx](https://msdn.microsoft.com/library/s064f8w2aspx)

[https://msdn.microsoft.com/library/ms143376aspx](https://msdn.microsoft.com/library/ms143376aspx)

