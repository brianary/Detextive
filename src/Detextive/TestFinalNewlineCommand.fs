namespace Detextive

open System.IO
open System.Management.Automation

/// Returns true if a file ends with a newline as required by the POSIX standard for text files.
[<Cmdlet(VerbsDiagnostic.Test, "FinalNewline")>]
[<OutputType(typeof<bool>)>]
type public TestFinalNewlineCommand () =
    inherit PSCmdlet ()

    /// Returns true if a file ends with a newline character.
    static member public HasFinalNewline (fs:FileStream) =
        fs.Seek(-1L, SeekOrigin.End) |> ignore
        match fs.ReadByte() with
        | 0x0A -> true
        | _ -> false

    /// A file to test.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        let item = x.GetItem x.Path
        x.WriteVerbose(sprintf "Testing %s for a trailing newline." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        TestFinalNewlineCommand.HasFinalNewline fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
