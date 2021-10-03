namespace Detextive

open System.IO
open System.Management.Automation

/// Returns true if a file contains text.
[<Cmdlet(VerbsDiagnostic.Test, "TextFile")>]
[<OutputType(typeof<bool>)>]
type public TestTextFileCommand () =
    inherit PSCmdlet ()

    /// Returns true if a file is determined to contain text.
    static member public IsTextFile (fs:FileStream) =
        let head = Array.zeroCreate 4
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        match Array.take (fs.Read(head, 0, 4)) head with
        | [||] -> false
        | [|0x0Auy|] -> true // empty LF POSIX ASCII/UTF-8 without BOM/SIG
        | [|0x0Duy;0x0Auy|] -> true // empty CR LF POSIX ASCII/UTF-8 without BOM/SIG
        | [|0xFFuy;0xFEuy;0uy;0uy|] -> true // UTF-32
        | [|0uy;0uy;0xFEuy;0xFFuy|] -> true // UTF-32BE
        | [|0xFFuy;0xFEuy;_;_|] -> true // UTF-16
        | [|0xFEuy;0xFFuy;_;_|] -> true // UTF-16BE
        | [|0xEFuy;0xBBuy;0xBFuy;_|] -> true // UTF-8 with BOM/SIG
        | _ ->
            fs.Seek(-1L, SeekOrigin.End) |> ignore
            match fs.ReadByte() with
            | 0x0A -> true
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
        x.WriteVerbose(sprintf "Testing %s for textiness." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        TestTextFileCommand.IsTextFile fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
