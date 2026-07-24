namespace Detextive

open System.Management.Automation
open System.Text

/// Normalizes Unicode text.
[<Cmdlet(VerbsCommon.Optimize, "Unicode")>]
[<OutputType(typeof<string>)>]
type public OptimizeUnicodeCommand () =
    inherit PSCmdlet ()

    /// The type of normalization to perform.
    [<Parameter(Position=0)>]
    [<Alias("Form")>]
    member val NormalizationForm = NormalizationForm.FormC with get, set

    /// The Unicode string to normalize.
    [<Parameter(ParameterSetName="InputObject",Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNull>]
    member val InputObject : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.InputObject.Normalize(x.NormalizationForm) |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
