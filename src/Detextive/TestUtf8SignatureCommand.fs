namespace ModuleName

open System.IO
open System.Management.Automation

/// Returns true if a file contains text.
[<Cmdlet(VerbsDiagnostic.Test, "Utf8Signature")>]
[<OutputType(typeof<bool>)>]
type TestUtf8SignatureCommand () =
    inherit PSCmdlet ()

    /// Returns true if a file starts with EF BB BF.
    static member HasUtf8Signature (fs:FileStream) =
        let head = Array.create 3 0uy
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        match Array.take (fs.Read(head, 0, 3)) head with
        | [|0xEFuy;0xBBuy;0xBFuy|] -> true // UTF-8 with BOM/SIG
        | _ -> false

    /// A file to test.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.Read, FileShare.Read)
        TestUtf8SignatureCommand.HasUtf8Signature fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
