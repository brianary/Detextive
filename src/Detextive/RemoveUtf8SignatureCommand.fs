namespace Detextive

open System
open System.IO
open System.Management.Automation

/// Removes the utf-8 signature (BOM) from a file.
[<Cmdlet(VerbsCommon.Remove, "Utf8Signature")>]
type public RemoveUtf8SignatureCommand () =
    inherit PSCmdlet ()

    /// A file to remove the utf-8 signature from.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        match (FileInfo x.Path).Length |> int with
        | 0 -> x.WriteVerbose(sprintf "%s doesn't start with EF BB BF" x.Path)
        | length ->
            use fs = new FileStream(x.Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            let data = Array.zeroCreate<byte> length
            match fs.Read(data, 0, data.Length) with
            | c when c <> length ->
                ErrorRecord((InvalidOperationException <| sprintf "Only read %d of %d bytes of %s" c data.Length x.Path),
                    "UnableToAddUTF8SIG:Read",ErrorCategory.InvalidOperation,x.Path)
                |> x.WriteError
            | c when c < 3 ->
                x.WriteVerbose(sprintf "%s doesn't start with EF BB BF" x.Path)
            | _ ->
                match Array.take 3 data with
                | [|0xEFuy;0xBBuy;0xBFuy|] ->
                    fs.SetLength 0L
                    fs.Write(data, 3, data.Length - 3)
                    x.WriteVerbose(sprintf "Removed EF BB BF from %s" x.Path)
                | _ ->
                    x.WriteVerbose(sprintf "%s doesn't start with EF BB BF" x.Path)

    override x.EndProcessing () =
        base.EndProcessing ()
