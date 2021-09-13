namespace Detextive

open System.IO
open System.Management.Automation

/// Returns true if a file does not appear to contain parseable text, and presumably contains binary data.
[<Cmdlet(VerbsDiagnostic.Test, "BinaryFile")>]
[<OutputType(typeof<bool>)>]
type public TestBinaryFileCommand () =
    inherit PSCmdlet ()

    /// A file to test.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.WriteVerbose($"Testing {x.Path} for binariness.")
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.Read, FileShare.Read)
        TestTextFileCommand.IsTextFile fs |> not |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
