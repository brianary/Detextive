namespace Detextive

open System.IO
open System.Management.Automation

/// Returns true if a file starts with the optional UTF-8 BOM/signature.
[<Cmdlet(VerbsDiagnostic.Test, "Utf8Signature")>]
[<OutputType(typeof<bool>)>]
type public TestUtf8SignatureCommand () =
    inherit PSCmdlet ()

    /// Returns true if a file starts with EF BB BF.
    static member public HasUtf8Signature (fs:FileStream) =
        let head = Array.zeroCreate 3
        FilePosition.Rewind fs
        match Array.take (fs.Read(head, 0, 3)) head with
        | [|0xEFuy;0xBBuy;0xBFuy|] -> true // UTF-8 with BOM/SIG
        | _ -> false

    /// A file to test.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        let item = x.GetItem x.Path
        x.WriteVerbose(sprintf "Testing %s for an explicit leading UTF-8 signature (EF BB BF)." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        TestUtf8SignatureCommand.HasUtf8Signature fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
