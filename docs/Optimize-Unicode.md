---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Optimize-Unicode

## SYNOPSIS
Normalizes Unicode text.

## SYNTAX

```
Optimize-Unicode [-NormalizationForm <NormalizationForm>] -InputObject <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> "café" |Optimize-Unicode FormKD
```

```txt
café
```

The normalized form decomposes the single U+00E9 (`é`) character into U+0065 (`e`) and U+0301 (COMBINING ACUTE ACCENT),
which should generally render the same, but may be compatible in different circumstances.

## PARAMETERS

### -InputObject
The Unicode string to normalize.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -NormalizationForm
The type of normalization to perform.

```yaml
Type: NormalizationForm
Parameter Sets: (All)
Aliases: Form

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### System.String

## NOTES

## RELATED LINKS

[https://docs.microsoft.com/dotnet/api/system.string.normalize#system-string-normalize%28system-text-normalizationform%29](https://docs.microsoft.com/dotnet/api/system.string.normalize#system-string-normalize%28system-text-normalizationform%29)

[https://docs.microsoft.com/dotnet/api/system.text.normalizationform](https://docs.microsoft.com/dotnet/api/system.text.normalizationform)
