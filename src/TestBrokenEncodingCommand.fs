namespace Detextive

open System
open System.IO
open System.Management.Automation
open System.Text
open System.Text.RegularExpressions

/// Returns true if text contains a nonsense sequence of characters resulting from parsing text with the wrong encoding.
[<Cmdlet(VerbsDiagnostic.Test, "BrokenEncoding",DefaultParameterSetName="InputObject")>]
[<OutputType(typeof<bool>)>]
type public TestBrokenEncodingCommand () =
    inherit PSCmdlet ()

    static member BrokenEncodingPattern =
        Regex("""\xC2[\x81\x8D\x8F\x90\x9D]
                 |\xC5[\xA0\xA1\xB8\xBD\xBE\u2019\u201C]
                 |\xC6\u2019
                 |\xCB[\u0153\u2020]
                 |\xE2
                 (?:
                   \u201A\xAC
                   |\u201E\xA2
                   |\u20AC[\x9D\xA0\xA1\xA2\xA6\xB0\xB9\xBA\u0153\u0161\u017E\u02DC\u201C\u201D\u2122]
                 )""", RegexOptions.IgnorePatternWhitespace)

    /// True for a range of byte values more likely to be used by Windows-1252.
    static member public HasBrokenEncodingSequence s =
        TestBrokenEncodingCommand.BrokenEncodingPattern.IsMatch(s)

    /// Process an individual item.
    member x.ProcessItem item =
        x.WriteVerbose(sprintf "Testing %s for a broken encoding." item)
        let text =
            use sr = new StreamReader(item, Encoding.UTF8)
            sr.ReadToEnd()
        TestBrokenEncodingCommand.HasBrokenEncodingSequence text |> x.WriteObject

    /// A string to test.
    [<Parameter(ParameterSetName="InputObject",Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNull>]
    member val InputObject : string = "" with get, set

    /// A file to test.
    [<Parameter(ParameterSetName="Path",Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        match x.ParameterSetName with
        | "Path" -> x.GetItem x.Path |> x.ProcessItem
        | _ -> TestBrokenEncodingCommand.HasBrokenEncodingSequence x.InputObject |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
