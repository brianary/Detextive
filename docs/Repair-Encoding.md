---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version: https://msdn.microsoft.com/library/dd383463.aspx
schema: 2.0.0
---

# Repair-Encoding

## SYNOPSIS
Re-encodes commonly mis-encoded text.

## SYNTAX

### InputObject (Default)
```
Repair-Encoding [-InputObject] <String> [<CommonParameters>]
```

### Path
```
Repair-Encoding -Path <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```ps1
Repair-Encoding.ps1 'SmartQuotes Arenâ€™t'
```

```
SmartQuotes Aren't
```

## PARAMETERS

### -InputObject
The string containing encoding failures to fix.

```yaml
Type: String
Parameter Sets: InputObject
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Path
A mis-encoded file to fix.

```yaml
Type: String
Parameter Sets: Path
Aliases: FullName

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

`System.String` containing encoding failures to fix.

Or any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

`System.String` with the encoding corrected, if `InputObject` is used.

`Path` is written back to the file.

## NOTES

## RELATED LINKS
