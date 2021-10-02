namespace Detextive

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
    static member public IndentPattern = Regex(@"\A(?<Indent>\s*)")

    /// Converts a line of text to an Indent enum.
    static member LineToIndent (s:string) =
        let m = GetFileIndentsCommand.IndentPattern.Match(s)
        match m.Groups.[0].Captures.[0].Value |> Set.ofSeq |> Set.toList with
        | [] -> None
        | ['\t'] -> Some(IndentType.Tabs)
        | [' '] -> Some(IndentType.Spaces)
        | [c] -> Some(IndentType.Other)
        | _ -> Some(IndentType.Mixed)

    /// Counts the line ending digraph or characters.
    static member public CountIndents (fs:FileStream) =
        let enc = GetFileEncodingCommand.DetectFileEncoding fs
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        use sr = new StreamReader(fs, enc, true, -1, true)
        Seq.initInfinite (fun _ -> sr.ReadLine())
            |> Seq.takeWhile ((<>) null)
            |> Seq.choose GetFileIndentsCommand.LineToIndent
            |> Seq.countBy id
            |> Map.ofSeq

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
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.GetItems x.Path |> Seq.iter x.ProcessItem

    override x.EndProcessing () =
        base.EndProcessing ()