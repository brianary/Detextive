namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// Returns true if a file is parseable as UTF-8.
[<Cmdlet(VerbsDiagnostic.Test, "Utf8Encoding")>]
[<OutputType(typeof<bool>)>]
type public TestUtf8EncodingCommand () =
    inherit PSCmdlet ()

    /// Returns true if a file is parseable as UTF-8.
    static member public IsUtf8Readable (x:PSCmdlet) (fs:FileStream) =
        let head = Array.zeroCreate 3
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        let utf8 = UTF8Encoding(true,true)
        use sr = new StreamReader(fs, UTF8Encoding(true,true), true, -1, true)
        try
            sr.ReadToEnd() |> ignore
            true
        with
        | :? DecoderFallbackException as ex ->
            x.WriteVerbose(ex.Message)
            false

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
        x.WriteVerbose(sprintf "Testing %s for UTF-8-ness." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        TestUtf8EncodingCommand.IsUtf8Readable x fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
