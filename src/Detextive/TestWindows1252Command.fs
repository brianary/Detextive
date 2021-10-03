namespace Detextive

open System
open System.IO
open System.Management.Automation

/// Returns true if a file contains at least one byte identified as a likely Windows-1252/CP1252 character value.
[<Cmdlet(VerbsDiagnostic.Test, "Windows1252")>]
[<OutputType(typeof<bool>)>]
type public TestWindows1252Command () =
    inherit PSCmdlet ()

    /// True for a range of byte values more likely to be used by Windows-1252.
    static member IsLikelyWindows1252Byte b =
        b >= 0x80uy && b <= 0x9Fuy // technically 81 8D 8F 90 9D aren't Windows-1252

    /// Reads some bytes and checks for likely Windows-1252 characters.
    static member IsNextPartWindows1252 (fs:FileStream) =
        let buffer = Array.zeroCreate 0xFFFF
        if (fs.Read(buffer, 0, buffer.Length)) < buffer.Length then
            Array.exists TestWindows1252Command.IsLikelyWindows1252Byte buffer
        elif Array.exists TestWindows1252Command.IsLikelyWindows1252Byte buffer then
            true
        else
            TestWindows1252Command.IsNextPartWindows1252 fs

    /// Returns true if a file contains a likely Windows-1252 character.
    static member public IsWindows1252 (fs:FileStream) =
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        TestWindows1252Command.IsNextPartWindows1252 fs

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
        x.WriteVerbose(sprintf "Testing %s for Windows-1252 bytes." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        if TestUtf8EncodingCommand.IsUtf8Readable x fs then
            x.WriteObject false
        else
            TestWindows1252Command.IsWindows1252 fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
