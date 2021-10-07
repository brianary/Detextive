namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// Re-encodes text in a file that has been misinterpreted as the wrong encoding.
[<Cmdlet(VerbsDiagnostic.Repair, "FileEncoding")>]
type public RepairFileEncodingCommand () =
    inherit PSCmdlet ()

    /// Process an individual item.
    member x.ProcessItem item =
        x.WriteVerbose(sprintf "Repairing encoding for %s." item)
        let text =
            use sr = new StreamReader(item, Encoding.UTF8)
            sr.ReadToEnd()
            |> Encoding.GetEncoding("Windows-1252").GetBytes
            |> Encoding.UTF8.GetString
            |> (fun s -> s.Normalize(NormalizationForm.FormKD))
        use sw = new StreamWriter(item, false, Encoding.UTF8)
        sw.Write(text)

    /// A file to fix.
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
