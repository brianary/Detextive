namespace Detextive

open System
open System.IO
open System.Management.Automation

/// Adds the utf-8 signature (BOM) to a file.
[<Cmdlet(VerbsCommon.Add, "Utf8Signature")>]
type public AddUtf8SignatureCommand () =
    inherit PSCmdlet ()

    /// Adds the UTF-8 signature (BOM)
    member x.AddUtf8Signature item (fs:FileStream) data =
        fs.Seek(0L, SeekOrigin.Begin) |> ignore
        fs.Write([|0xEFuy;0xBBuy;0xBFuy|], 0, 3)
        fs.Write(data, 0, data.Length)
        x.WriteVerbose(sprintf "Prepended EF BB BF to %s" item)

    /// Process an individual item.
    member x.ProcessItem item =
        let length = (FileInfo item).Length |> int
        use fs = new FileStream(item, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
        match length with
        | 0 -> fs.Write([|0xEFuy;0xBBuy;0xBFuy|], 0, 3)
        | _ ->
            let data = Array.zeroCreate<byte> length
            match fs.Read(data, 0, data.Length) with
            | c when c <> length ->
                ErrorRecord((InvalidOperationException <| sprintf "Only read %d of %d bytes of %s" c data.Length item),
                    "UnableToAddUTF8SIG:Read",ErrorCategory.InvalidOperation,item)
                |> x.WriteError
            | c when c < 3 -> x.AddUtf8Signature item fs data
            | _ ->
                match Array.take 3 data with
                | [|0xEFuy;0xBBuy;0xBFuy|] ->
                    x.WriteVerbose(sprintf "%s already starts with EF BB BF" item)
                | _ -> x.AddUtf8Signature item fs data

    /// A file to add the utf-8 signature to.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.GetItems x.Path |> Seq.iter x.ProcessItem

    override x.EndProcessing () =
        base.EndProcessing ()
