namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// Returns true if a file contains text.
[<Cmdlet(VerbsDiagnostic.Test, "TextFile")>]
[<OutputType(typeof<bool>)>]
type public TestTextFileCommand () =
    inherit PSCmdlet ()

    /// Judge the encoding based on counts of byte values.
    static member public ByteFrequencyTextAnalysis bytes =
        let zeros, lowcontrols, ebcdicy =
            Array.fold
                (fun (z,c,e) -> // counting Zero, low Controls, EBCDIC-y chars
                    function
                    | 0uy    -> z+1, c, e
                    | 0x05uy -> z, c, e+1
                    | 0x40uy -> z, c, e+1
                    | 0x7Fuy -> z, c, e+1
                    | b when List.contains b [0x01uy;0x02uy;0x03uy;0x04uy;0x06uy;0x07uy;0x08uy;
                                              0x0Buy;0x0Cuy;0x0Euy;0x0Fuy;0x10uy;0x11uy;0x12uy;
                                              0x13uy;0x14uy;0x15uy;0x16uy;0x17uy;0x18uy;0x19uy] -> z, c+1, e
                    | _ -> z, c, e ) (0,0,0) bytes
        let len = double bytes.Length
        // ASCII or ISO-8859-1 or Windows-1252, probably:
        if zeros = 0 && lowcontrols = 0 then true
        // assume Benford's Law applies to some degree for binary data:
        elif ((double lowcontrols) / len) >= Ratio.MinBinaryLowControls then false
        // UTF-16 or UTF-32, probably:
        elif ((double zeros) / len) >= Ratio.MinDoubleByteZeros then true
        // EBCDIC, probably:
        else ((double ebcdicy) / len) >= Ratio.MinCommonEbcdic

    /// Returns true if a file is determined to contain text.
    static member public IsTextFile (fs:FileStream) =
        let head = Array.zeroCreate 4
        FilePosition.Rewind fs
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
            let bytes = fs.Seek(0L, SeekOrigin.End) |> int |> Array.zeroCreate<byte>
            FilePosition.Rewind fs
            fs.Read(bytes, 0, bytes.Length) |> ignore
            TestTextFileCommand.ByteFrequencyTextAnalysis bytes


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
