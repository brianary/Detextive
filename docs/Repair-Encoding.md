---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version: https://msdn.microsoft.com/library/dd383463.aspx
schema: 2.0.0
---

# Repair-Encoding

## SYNOPSIS
Re-encodes Windows-1252 text that has been misinterpreted as UTF-8.

## SYNTAX

```
Repair-Encoding [-InputObject] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```
Repair-Encoding.ps1 'SmartQuotes Arenâ€™t'
```

SmartQuotes Aren't

## PARAMETERS

### -InputObject
The string containing encoding failures to fix.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String containing encoding failures to fix.
### System.String

## OUTPUTS

### System.String containing the corrected string data.
### System.Object
## NOTES

## RELATED LINKS
