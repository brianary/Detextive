namespace Detextive

open System
open System.IO
open System.Management.Automation
open System.Text.RegularExpressions

/// Returns true if a string contains a nonsense sequence of characters resulting from parsing text with the wrong encoding.
[<Cmdlet(VerbsDiagnostic.Test, "BrokenEncoding")>]
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

    /// A string to test.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipeline=true)>]
    [<ValidateNotNull>]
    member val InputObject : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        TestBrokenEncodingCommand.HasBrokenEncodingSequence x.InputObject |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
