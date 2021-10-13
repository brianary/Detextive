---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Test-BrokenEncoding

## SYNOPSIS
Returns true if text contains a nonsense sequence of characters resulting from parsing text with the wrong encoding.

## SYNTAX

### InputObject (Default)
```
Test-BrokenEncoding [-InputObject] <String> [<CommonParameters>]
```

### Path
```
Test-BrokenEncoding -Path <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```ps1
'1.2.1 â€“ 1.3.4' |Test-BrokenEncoding
```

```
True
```

## PARAMETERS

### -InputObject
A string to test.

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
A file to test.

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

### `System.String` containing encoding failures to fix.

### Or any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

### `System.Boolean` indicating that the text or file contains a nonsense sequence of characters resulting from parsing text with the wrong encoding.

## NOTES

## RELATED LINKS
