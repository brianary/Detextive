namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// Returns the detected encoding of a file.
[<Cmdlet(VerbsCommon.Get, "FileEncoding")>]
[<OutputType(typeof<Encoding>)>]
type public GetFileEncodingCommand () =
    inherit PSCmdlet ()

    /// Attempt parsing bytes using encoding, returned encoding option indicates success.
    static member public TryEncodingParse (enc:Encoding) (bytes:byte[]) =
        let encopy = enc.Clone() :?> Encoding
        encopy.DecoderFallback <- DecoderExceptionFallback()
        try
            encopy.GetString(bytes) |> ignore
            Some(enc)
        with
        | :? DecoderFallbackException as ex -> None

    /// Judge the encoding based on counts of byte values.
    static member public ByteFrequencyEncodingAnalysis bytes =
        match Array.fold
                (fun (z,e,h,x) -> // counting Zero, EBCDIC space, High, and eXcluded bytes
                    function
                    | 0uy -> z+1, e, h, x
                    | 0x40uy -> z, e+1, h, x
                    | b when b >= 0x80uy && b <= 0x9Fuy -> z, e, h+1, x+1
                    | b when b >= 0xA0uy -> z, e, h+1, x
                    | _ -> z, e, h, x ) (0,0,0,0) bytes with
        | _, e, _, _  when e > (bytes.Length/6) &&
                           Option.isSome
                           <| GetFileEncodingCommand.TryEncodingParse (Encoding.GetEncoding("IBM037")) bytes ->
                                Some(Encoding.GetEncoding("IBM037"))
        | 0, _, 0, 0 -> Some(Encoding.ASCII)
        | 0, _, _, 0 -> Some(Encoding.GetEncoding("Latin1"))
        | 0, _, _, _ -> GetFileEncodingCommand.TryEncodingParse Encoding.UTF8 bytes
                        |> Option.defaultValue (Encoding.GetEncoding("Windows-1252"))
                        |> Some
        | z, _, _, _ -> match (double z) / (double bytes.Length) with
                        | r when r >= 0.72 -> match Array.take 4 bytes with
                                              | [|_;_;_;0uy|] -> Some(Encoding.UTF32)
                                              | [|0uy;_;_;_|] -> Some(Encoding.GetEncoding("UTF-32BE"))
                                              | _ -> None
                        | r when r >= 0.46 -> match Array.take 2 bytes with
                                              | [|_;0uy|] -> Some(Encoding.Unicode)
                                              | [|0uy;_|] -> Some(Encoding.BigEndianUnicode)
                                              | _ -> None
                        | _ -> None

    /// Returns the detected encoding of a file.
    static member public DetectFileEncoding (fs:FileStream) =
        let head = Array.zeroCreate 4
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        match Array.take (fs.Read(head, 0, 4)) head with
        | [||] -> Encoding.UTF8
        | [|0x0Auy|] -> Encoding.UTF8 // empty LF POSIX ASCII/UTF-8 without BOM/SIG
        | [|0x0Duy;0x0Auy|] -> Encoding.UTF8 // empty CR LF POSIX ASCII/UTF-8 without BOM/SIG
        | [|0xFFuy;0xFEuy;0uy;0uy|] -> Encoding.UTF32 // UTF-32
        | [|0uy;0uy;0xFEuy;0xFFuy|] -> Encoding.GetEncoding("UTF-32BE") // UTF-32BE
        | [|0xFFuy;0xFEuy;_;_|] -> Encoding.Unicode // UTF-16
        | [|0xFEuy;0xFFuy;_;_|] -> Encoding.BigEndianUnicode // UTF-16BE
        | [|0xEFuy;0xBBuy;0xBFuy;_|] -> UTF8Encoding(true) :> Encoding // UTF-8 with BOM/SIG
        | _ ->
            let bytes = fs.Seek(0L, SeekOrigin.End) |> int |> Array.zeroCreate<byte>
            fs.Seek(0L, SeekOrigin.Begin) |> ignore
            fs.Read(bytes, 0, bytes.Length) |> ignore
            match GetFileEncodingCommand.ByteFrequencyEncodingAnalysis bytes with
            | Some(enc) -> enc
            | None -> GetFileEncodingCommand.TryEncodingParse Encoding.UTF8 bytes
                      |> Option.defaultValue (Encoding.GetEncoding("Windows-1252"))

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
        x.WriteVerbose(sprintf "Getting encoding of %s." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        GetFileEncodingCommand.DetectFileEncoding fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
