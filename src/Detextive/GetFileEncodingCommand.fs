namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// Returns the detected encoding of a file.
[<Cmdlet(VerbsCommon.Get, "FileEncoding")>]
[<OutputType(typeof<bool>)>]
type public GetFileEncodingCommand () =
    inherit PSCmdlet ()

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
        | [|0xFEuy;0xFFuy;_;_|] -> Encoding.GetEncoding("UTF-16BE") // UTF-16BE
        | [|0xEFuy;0xBBuy;0xBFuy;_|] -> UTF8Encoding(true) :> Encoding // UTF-8 with BOM/SIG
        | _ ->
            if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
            let utf8 = UTF8Encoding(true,true)
            use sr = new StreamReader(fs, UTF8Encoding(true,true), true, -1, true)
            try
                sr.ReadToEnd() |> ignore
                Encoding.UTF8
            with
            | :? DecoderFallbackException as ex ->
                Encoding.GetEncoding("windows-1252")

    /// A file to test.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.WriteVerbose($"Testing {x.Path} for textiness.")
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.Read, FileShare.Read)
        GetFileEncodingCommand.DetectFileEncoding fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
