namespace Detextive

open System
open System.IO
open System.Management.Automation

/// Adds the utf-8 signature (BOM) to a file.
[<Cmdlet(VerbsCommon.Add, "Utf8Signature")>]
type public AddUtf8SignatureCommand () =
    inherit PSCmdlet ()

    /// Adds the UTF-8 signature (BOM)
    member x.AddUtf8Signature (fs:FileStream) (data:byte[]) =
        fs.Seek(0L, SeekOrigin.Begin) |> ignore
        fs.Write([|0xEFuy;0xBBuy;0xBFuy|], 0, 3)
        fs.Write(data, 0, data.Length)
        x.WriteVerbose(sprintf "Prepended EF BB BF to %s" x.Path)

    /// A file to add the utf-8 signature to.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        let length = (FileInfo x.Path).Length |> int
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
        match length with
        | 0 -> fs.Write([|0xEFuy;0xBBuy;0xBFuy|], 0, 3)
        | _ ->
            let data = Array.zeroCreate<byte> length
            match fs.Read(data, 0, data.Length) with
            | c when c <> length ->
                ErrorRecord((InvalidOperationException <| sprintf "Only read %d of %d bytes of %s" c data.Length x.Path),
                    "UnableToAddUTF8SIG:Read",ErrorCategory.InvalidOperation,x.Path)
                |> x.WriteError
            | c when c < 3 -> x.AddUtf8Signature fs data
            | _ ->
                match Array.take 3 data with
                | [|0xEFuy;0xBBuy;0xBFuy|] ->
                    x.WriteVerbose(sprintf "%s already starts with EF BB BF" x.Path)
                | _ -> x.AddUtf8Signature fs data

    override x.EndProcessing () =
        base.EndProcessing ()
