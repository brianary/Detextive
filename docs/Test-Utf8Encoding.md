---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version: https://docs.microsoft.com/en-us/dotnet/api/system.text.utf8encoding.-ctor?view=netframework-4.8
schema: 2.0.0
---

# Test-Utf8Encoding

## SYNOPSIS
Determines whether a file can be parsed as UTF-8 successfully.

## SYNTAX

```
Test-Utf8Encoding [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### EXAMPLE 1
```ps1
Test-Utf8Encoding README.md
```

```
True
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

### Any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

### `System.Boolean` indicating that the file can be successfully parsed as UTF-8.

## NOTES

## RELATED LINKS

[https://docs.microsoft.com/en-us/dotnet/api/system.text.utf8encoding.-ctor?view=netframework-4.8](https://docs.microsoft.com/en-us/dotnet/api/system.text.utf8encoding.-ctor?view=netframework-4.8)

