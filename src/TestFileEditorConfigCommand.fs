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
            let encOk = match expectedEnc, actualEnc with
                        | e, a when e = a -> true
                        | "latin1", "iso-8859-1" -> true // alias for same encoding
                        | "utf-8", "us-ascii" ->
                            sprintf "%s encoding expected %s but %s is fine" actual.Path expectedEnc actualEnc
                                |> x.WriteVerbose
                            true
                        | "latin1", "us-ascii" ->
                            sprintf "%s encoding expected %s but %s is fine" actual.Path expectedEnc actualEnc
                                |> x.WriteVerbose
                            true
                        | _ ->
                            sprintf "%s encoding should be %s but is %s" actual.Path expectedEnc actualEnc
                                |> x.WriteVerbose
                            false
            let indentOk = match expected.Indents, actual.Indents.Indents with
                           | e, a when e = a -> true
                           | IndentType.None, _ ->
                                sprintf "%s no expected indents so %A is fine" actual.Path actual.Indents.Indents
                                    |> x.WriteVerbose
                                true
                           | _, IndentType.None ->
                                sprintf "%s expected %A indents but none is fine" actual.Path expected.Indents
                                    |> x.WriteVerbose
                                true
                           | _ ->
                                sprintf "%s indents should be %A but is %A" actual.Path expected.Indents actual.Indents.Indents
                                    |> x.WriteVerbose
                                false
            let lineEndingsOk = match expected.LineEndings, actual.LineEndings.LineEndings with
                                | e, a when e = a -> true
                                | LineEndingType.None, _ ->
                                    sprintf "%s no expected line endings so %A is fine" actual.Path actual.LineEndings.LineEndings
                                        |> x.WriteVerbose
                                    true
                                | _, LineEndingType.None ->
                                    sprintf "%s no expected %A indents but none is fine" actual.Path expected.LineEndings
                                        |> x.WriteVerbose
                                    true
                                | _ ->
                                    sprintf "%s line endings should be %A but is %A" actual.Path expected.LineEndings actual.LineEndings.LineEndings
                                        |> x.WriteVerbose
                                    false
            if expected.FinalNewline <> actual.FinalNewline then
                sprintf "%s should%s include a final newline but does%s" expected.Path
                    (match expected.FinalNewline with | false -> " not" | _ -> "")
                    (match actual.FinalNewline with | false -> " not" | _ -> "")
                    |> x.WriteVerbose
            encOk && indentOk && lineEndingsOk && expected.FinalNewline = actual.FinalNewline

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
