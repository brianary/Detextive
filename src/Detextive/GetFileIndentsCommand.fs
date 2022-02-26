namespace Detextive

open System
open System.IO
open System.Management.Automation
open System.Text.RegularExpressions

/// End of line digraph or character.
type public IndentType =
    | None = 0
    | Mixed = 1
    | Tabs = 2
    | Spaces = 4
    | Other = 8

/// The details returned by the Get-FileIndents cmdlet.
[<StructuredFormatDisplay("{Indents}")>]
type public IndentsResult =
    { Path : string
      Indents : IndentType
      Mixed : int
      Tabs : int
      Spaces : int
      Other : int }
    member x.StringValue = sprintf "%s: %s" x.Path (string x.Indents)
    override x.ToString () = string x.Indents

/// Returns details about a file's indentation characters.
[<Cmdlet(VerbsCommon.Get, "FileIndents")>]
[<OutputType(typeof<IndentsResult>)>]
type public GetFileIndentsCommand () =
    inherit PSCmdlet ()

    /// The regular expression for matching indents.
    static member public IndentPattern = Regex(@"\A\s*")

    /// F# and Python allow triple-quoted multiline strings that shouldn't count in code indentation.
    static member public TripleQuotedStrings = Regex(@""""""".*?""""""", RegexOptions.Singleline)

    /// PowerShell here-strings are multiline strings that shouldn't count in code indendation.
    static member public PowerShellHereStrings =
        Regex(@"@""[\r\n\u0085\u2028\u2029].*?[\r\n\u0085\u2028\u2029]""@|@'[\r\n\u0085\u2028\u2029].*?[\r\n\u0085\u2028\u2029]'@",
              RegexOptions.Singleline)

    /// C# and F# support verbatim strings that may be multiline and shouldn't count in code indentation.
    static member public VerbatimStrings = Regex(@"@""(?:[^""]|"""")*?""", RegexOptions.Singleline)

    /// Splits a string into lines, using any supported line endings, and ignoring empty lines.
    static member public SplitIntoLines (s:string) =
        s.Split([|'\u000A';'\u000D';'\u0085';'\u2028';'\u2029'|], StringSplitOptions.RemoveEmptyEntries)

    /// Converts a line of text to an Indent enum.
    static member LineToIndent (s:string) =
        let m = GetFileIndentsCommand.IndentPattern.Match(s)
        match m.Groups.[0].Captures.[0].Value |> Set.ofSeq |> Set.toList with
        | [] -> None
        | ['\t'] -> Some(IndentType.Tabs)
        | [' '] -> Some(IndentType.Spaces)
        | [c] -> Some(IndentType.Other)
        | _ -> Some(IndentType.Mixed)

    /// Counts the indent characters.
    static member public CountIndents (fs:FileStream) =
        let enc = GetFileEncodingCommand.DetectFileEncoding fs
        let len = FilePosition.GetBufferSize fs
        use sr = new StreamReader(fs, enc, true, len, true)
        sr.ReadToEnd()
            |> GetFileIndentsCommand.TripleQuotedStrings.RemoveMatches
            |> GetFileIndentsCommand.PowerShellHereStrings.RemoveMatches
            |> GetFileIndentsCommand.VerbatimStrings.RemoveMatches
            |> GetFileIndentsCommand.SplitIntoLines
            |> Array.choose GetFileIndentsCommand.LineToIndent
            |> Array.countBy id
            |> Map.ofArray

    /// Returns the indent details detected.
    static member public DetectIndents (x:PSCmdlet) (p:string) (fs:FileStream) =
        let counts = GetFileIndentsCommand.CountIndents fs
        counts |> sprintf "Indents for %s : %A" p |> x.WriteVerbose
        let total e = Map.tryFind e counts |> Option.defaultValue 0
        { Path = p
          Indents = match Map.count counts with
                    | 0 -> IndentType.None
                    | 1 -> Map.toList counts |> List.head |> fst
                    | _ -> IndentType.Mixed
          Mixed = total IndentType.Mixed
          Tabs = total IndentType.Tabs
          Spaces = total IndentType.Spaces
          Other = total IndentType.Other }

    /// Process an individual item.
    member x.ProcessItem item =
        x.WriteVerbose(sprintf "Getting indents from %s." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        GetFileIndentsCommand.DetectIndents x item fs |> x.WriteObject

    /// A file to test.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.GetItems x.Path |> Seq.toList |> List.iter x.ProcessItem

    override x.EndProcessing () =
        base.EndProcessing ()
