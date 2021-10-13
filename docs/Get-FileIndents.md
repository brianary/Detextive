---
external help file: Detextive.dll-Help.xml
Module Name: Detextive
online version:
schema: 2.0.0
---

# Get-FileIndents

## SYNOPSIS
Returns details about a file's indentation characters.

## SYNTAX

```
Get-FileIndents [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```ps1
Get-FileIndents src\Detextive\GetFileIndentsCommand.fs
```

```
Path        : A:\Detextive\src\Detextive\GetFileIndentsCommand.fs
Indents     : Spaces
Mixed       : 0
Tabs        : 0
Spaces      : 66
Other       : 0
StringValue : A:\Detextive\src\Detextive\GetFileIndentsCommand.fs: Spaces
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

### Any object with a `Path` or `FullName` property to use for a file location.

## OUTPUTS

### `Detextive.IndentsResult`

* **Path** `string`: The full path of the file.
* **Indents** `IndentType`: The file's indent style: `None`, `Mixed`, `Tabs`, `Spaces`, or `Other`.
* **Mixed** `int`: The number of lines indented with multiple inconsistent indent characters
  (like spaces and tabs together).
* **Tabs** `int`: The number of lines indented with tabs.
* **Spaces** `int`: The number of lines indented with spaces.
* **Other** `int`: The number of lines indented with non-space, non-tab whitespace.
  This means something unusual such as U+00A0 NO-BREAK SPACE, U+2003 EM SPACE, U+200A HAIR SPACE
  or any other weird, unexpected space character.
* **StringValue** `string`: A summary of the file indents.

## NOTES

## RELATED LINKS
