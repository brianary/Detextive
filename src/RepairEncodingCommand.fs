namespace Detextive

open System
open System.IO
open System.Management.Automation
open System.Text

/// Re-encodes commonly mis-encoded text.
[<Cmdlet(VerbsDiagnostic.Repair, "Encoding", DefaultParameterSetName="InputObject")>]
[<OutputType(typeof<string>, ParameterSetName=[|"InputObject"|])>]
type public RepairEncodingCommand () =
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

    /// The string containing encoding failures to fix.
    [<Parameter(ParameterSetName="InputObject",Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNull>]
    member val InputObject : string = "" with get, set

    /// A mis-encoded file to fix.
    [<Parameter(ParameterSetName="Path",Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        match x.ParameterSetName with
        | "Path" -> x.GetItems x.Path |> Seq.toList |> List.iter x.ProcessItem
        | _ ->
            Encoding.GetEncoding("Windows-1252").GetBytes(x.InputObject)
                |> Encoding.UTF8.GetString
                |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
