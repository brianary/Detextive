namespace Detextive

open System
open System.Management.Automation
open System.Text

/// Re-encodes Windows-1252 text that has been misinterpreted as UTF-8.
[<Cmdlet(VerbsDiagnostic.Repair, "Encoding")>]
type public RepairEncodingCommand () =
    inherit PSCmdlet ()

    /// The string containing encoding failures to fix.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNullOrEmpty>]
    member val InputObject : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        Encoding.GetEncoding("Windows-1252").GetBytes(x.InputObject)
            |> Encoding.UTF8.GetString
            |> (fun s -> s.Normalize(NormalizationForm.FormKD))
            |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
