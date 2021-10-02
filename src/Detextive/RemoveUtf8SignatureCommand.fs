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

    /// Process an individual item.
    member x.ProcessItem item =
        match (FileInfo item).Length |> int with
        | 0 -> x.WriteVerbose(sprintf "%s doesn't start with EF BB BF" item)
        | length ->
            use fs = new FileStream(item, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            let data = Array.zeroCreate<byte> length
            match fs.Read(data, 0, data.Length) with
            | c when c <> length ->
                ErrorRecord((InvalidOperationException <| sprintf "Only read %d of %d bytes of %s" c data.Length item),
                    "UnableToAddUTF8SIG:Read",ErrorCategory.InvalidOperation,item)
                |> x.WriteError
            | c when c < 3 ->
                x.WriteVerbose(sprintf "%s doesn't start with EF BB BF" item)
            | _ ->
                match Array.take 3 data with
                | [|0xEFuy;0xBBuy;0xBFuy|] ->
                    fs.SetLength 0L
                    fs.Write(data, 3, data.Length - 3)
                    x.WriteVerbose(sprintf "Removed EF BB BF from %s" item)
                | _ ->
                    x.WriteVerbose(sprintf "%s doesn't start with EF BB BF" item)

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.GetItems x.Path |> List.iter x.ProcessItem

    override x.EndProcessing () =
        base.EndProcessing ()
