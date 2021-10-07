namespace Detextive

open System.IO
open System.Management.Automation
open System.Text
open System.Text.RegularExpressions

/// Returns true if a file contains a nonsense sequence of characters resulting from parsing text with the wrong encoding.
[<Cmdlet(VerbsDiagnostic.Test, "FileBrokenEncoding")>]
[<OutputType(typeof<bool>)>]
type public TestFileBrokenEncodingCommand () =
    inherit PSCmdlet ()

    /// Process an individual item.
    member x.ProcessItem item =
        x.WriteVerbose(sprintf "Testing %s for a broken encoding." item)
        let text =
            use sr = new StreamReader(item, Encoding.UTF8)
            sr.ReadToEnd()
        TestBrokenEncodingCommand.HasBrokenEncodingSequence text |> x.WriteObject

    /// A file to test.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.GetItem x.Path |> x.ProcessItem

    override x.EndProcessing () =
        base.EndProcessing ()
