namespace Detextive

open System
open System.IO
open System.Management.Automation
open System.Text
open System.Text.RegularExpressions
open EditorConfig.Core

/// Corrects a file's editorconfig settings when they differ from the actual formatting found.
[<Cmdlet(VerbsDiagnostic.Repair, "FileEditorConfig")>]
type public RepairFileEditorConfigCommand () =
    inherit PSCmdlet ()

    /// Process an individual item.
    member x.ProcessItem item (cfg:FileConfiguration) =
        let expected = GetFileEditorConfigCommand.GetFileEditorConfig x item cfg
        let actual = GetFileContentsInfoCommand.GetFileContentsInfo x item
        if actual.IsBinary then
            sprintf "%s was detected as binary (ignored)" actual.Path |> x.WriteVerbose
        else
            sprintf "%s from %A to %A" item actual expected |> x.WriteVerbose
            let encodingName name utf8bom = match name with | "utf-8" when utf8bom -> "utf-8-bom" | n -> n
            let expectedEnc = encodingName expected.Encoding.WebName expected.Utf8Signature
            let actualEnc = encodingName actual.Encoding.WebName actual.Utf8Signature
            use fs = new FileStream(item, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            let len = FilePosition.GetBufferSize fs
            let mutable text =
                sprintf "Reading %s text from %s" actualEnc item |> x.WriteVerbose
                use sr = new StreamReader(fs, actual.Encoding, true, len, true)
                sr.ReadToEnd()
            if expected.Indents = IndentType.None || actual.Indents.Indents = IndentType.None then
                sprintf "Indents in %s will be ignored" item |> x.WriteVerbose
            elif expected.Indents <> actual.Indents.Indents then
                text <- match expected.Indents with
                        | IndentType.Tabs ->
                            Regex.Replace(text, sprintf @"(?<=^\s*?) {%d}" expected.IndentSize, "\t", RegexOptions.Multiline)
                        | IndentType.Spaces ->
                            Regex.Replace(text, @"(?<=^\s*?)\t", String(' ',expected.IndentSize), RegexOptions.Multiline)
                        | _ -> text
                sprintf "Indents in %s converted from %A to %A" item actual.Indents.Indents expected.Indents
                    |> x.WriteVerbose
            else
                sprintf "Indents for %s look fine (%A = %A)" item actual.Indents.Indents expected.Indents
                    |> x.WriteVerbose
            let eol = match expected.LineEndings with
                      | LineEndingType.CR -> "\r"
                      | LineEndingType.CRLF -> "\r\n"
                      | LineEndingType.LF -> "\n"
                      | LineEndingType.LS -> "\u2028"
                      | LineEndingType.NEL -> "\u0085"
                      | LineEndingType.PS -> "\u2029"
                      | _ -> Environment.NewLine
            if expected.LineEndings = LineEndingType.None || actual.LineEndings.LineEndings = LineEndingType.None then
                sprintf "Line endings in %s will be ignored" item |> x.WriteVerbose
            elif expected.LineEndings <> actual.LineEndings.LineEndings then
                text <- Regex.Replace(text.Replace("\r\n", "\n"), @"[\r\n\u0085\u2028\u2029]", eol)
                sprintf "Line endings in %s converted from %A to %A" expected.Path actual.LineEndings.LineEndings
                    expected.LineEndings |> x.WriteVerbose
            if expected.FinalNewline <> actual.FinalNewline then
                text <- if expected.FinalNewline then text + eol
                        else Regex.Replace(text, @"[\r\n\u0085\u2028\u2029]+\z", "")
            fs.SetLength 0L
            use sw = new StreamWriter(fs, expected.Encoding)
            sw.Write(text)

    /// A file to fix the editorconfig values for.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        let getCfg = GetFileEditorConfigCommand.GetFileConfiguration <| EditorConfigParser()
        let item = x.GetItem x.Path
        x.ProcessItem item (getCfg item) |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
