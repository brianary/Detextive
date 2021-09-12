namespace ModuleName // use your module name for the namespace

open System.IO
open System.Management.Automation

/// Returns true if a file contains text.
[<Cmdlet(VerbsDiagnostic.Test, "TextFile")>]
[<OutputType(typeof<bool>)>]
type TestTextFileCommand () =
    inherit PSCmdlet ()

    /// A file to test.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    // optional: setup before pipeline input starts (e.g. Name is set, InputObject is not)
    override x.BeginProcessing () =
        base.BeginProcessing ()

    // optional: handle each pipeline value (e.g. InputObject)
    override x.ProcessRecord () =
        base.ProcessRecord ()
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.Read, FileShare.Read)
        fs.Seek(-1L, SeekOrigin.End) |> ignore
        let b = fs.ReadByte()
        x.WriteVerbose((sprintf "%s final byte: U+%04X" x.Path b))
        x.WriteObject((b = 0x0A))

    // optional: finish after all pipeline input
    override x.EndProcessing () =
        base.EndProcessing ()
