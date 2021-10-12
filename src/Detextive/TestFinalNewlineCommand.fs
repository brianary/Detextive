namespace Detextive

open System.IO
open System.Management.Automation

/// Returns true if a file ends with a newline as required by the POSIX standard for text files.
[<Cmdlet(VerbsDiagnostic.Test, "FinalNewline")>]
[<OutputType(typeof<bool>)>]
type public TestFinalNewlineCommand () =
    inherit PSCmdlet ()

    /// Returns true if a file ends with a newline character.
    static member public HasFinalNewline (fs:FileStream) strict =
        let enc = GetFileEncodingCommand.DetectFileEncoding fs
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        use sr = new StreamReader(fs, enc, true, -1, true)
        match strict, sr.ReadToEnd() |> Seq.last with
        | _, '\n' -> true
        | false, c when List.contains c ['\r';'\u0085';'\u2028';'\u2029'] -> true
        | _ -> false

    /// A file to test.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    /// Indicates that only a trailing newline is recognized, no other line endings.
    [<Parameter()>]
    member val Strict : SwitchParameter = SwitchParameter(false) with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        let item = x.GetItem x.Path
        x.WriteVerbose(sprintf "Testing %s for a trailing newline." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        TestFinalNewlineCommand.HasFinalNewline fs (x.Strict.ToBool()) |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
