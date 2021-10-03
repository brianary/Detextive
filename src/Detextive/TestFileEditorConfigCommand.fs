namespace Detextive

open System
open System.Management.Automation
open System.Text
open EditorConfig.Core

/// Validates a file's editorconfig settings against the actual formatting found.
[<Cmdlet(VerbsDiagnostic.Test, "FileEditorConfig")>]
[<OutputType(typeof<bool>)>]
type public TestFileEditorConfigCommand () =
    inherit PSCmdlet ()

    /// Process an individual item.
    member x.ProcessItem item (cfg:FileConfiguration) =
        let expected = GetFileEditorConfigCommand.GetFileEditorConfig x item cfg
        let actual = GetFileContentsInfoCommand.GetFileContentsInfo x item
        if actual.IsBinary then
            sprintf "%s was detected as binary (ignored)" actual.Path |> x.WriteVerbose
            true
        else
            let encodingName name utf8bom = match name with | "utf-8" when utf8bom -> "utf-8-bom" | n -> n
            let expectedEnc = encodingName expected.Encoding.WebName expected.Utf8Signature
            let actualEnc = encodingName actual.Encoding.WebName actual.Utf8Signature
            if expectedEnc <> actualEnc then
                sprintf "%s encoding should be %s but is %s" expected.Path expectedEnc actualEnc
                    |> x.WriteVerbose
            if expected.Indents = IndentType.None || actual.Indents.Indents = IndentType.None then
                sprintf "%s indents expected %A, actual is %A (ignored)" expected.Path expected.Indents actual.Indents.Indents
                    |> x.WriteVerbose
            elif expected.Indents <> actual.Indents.Indents then
                sprintf "%s indents should be %A but is %A" expected.Path expected.Indents actual.Indents.Indents
                    |> x.WriteVerbose
            if expected.LineEndings = LineEndingType.None || actual.LineEndings.LineEndings = LineEndingType.None then
                sprintf "%s line endings expected %A, actual is %A (ignored)" expected.Path expected.LineEndings actual.LineEndings.LineEndings
                    |> x.WriteVerbose
            elif expected.LineEndings <> actual.LineEndings.LineEndings then
                sprintf "%s line endings should be %A but is %A" expected.Path expected.LineEndings actual.LineEndings.LineEndings
                    |> x.WriteVerbose
            if expected.FinalNewline <> actual.FinalNewline then
                sprintf "%s should%s include a final newline but does%s" expected.Path
                    (match expected.FinalNewline with | false -> " not" | _ -> "")
                    (match actual.FinalNewline with | false -> " not" | _ -> "")
                    |> x.WriteVerbose
            let indentOk = match expected.Indents, actual.Indents.Indents with
                           | IndentType.None, _ -> true
                           | _, IndentType.None -> true
                           | e, a when e = a -> true
                           | _ -> false
            let lineEndingsOk = match expected.LineEndings, actual.LineEndings.LineEndings with
                                | LineEndingType.None, _ -> true
                                | _, LineEndingType.None -> true
                                | e, a when e = a -> true
                                | _ -> false
            expectedEnc = actualEnc && indentOk && lineEndingsOk && expected.FinalNewline = actual.FinalNewline

    /// A file to test the editorconfig values for.
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
