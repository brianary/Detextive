namespace Detextive

open System.IO
open System.Management.Automation
open System.Text.RegularExpressions

/// End of line digraph or character.
type public IndentType =
    | Mixed = 0
    | Tabs = 1
    | Spaces = 2
    | Other = 4

/// The details returned by the cmdlet.
[<StructuredFormatDisplay("{Indents}")>]
type public IndentsResult =
    { Path : string
      Indents : IndentType
      Mixed : int
      Tabs : int
      Spaces : int
      Other : int }
    override x.ToString () = x.Indents.ToString()

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
        | ['\u0009'] -> Some(IndentType.Tabs)
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
          Indents = (if (Map.count counts) = 1 then Map.toList counts |> List.head |> fst else IndentType.Mixed)
          Mixed = total IndentType.Mixed
          Tabs = total IndentType.Tabs
          Spaces = total IndentType.Spaces
          Other = total IndentType.Other }

    /// A file to test.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.WriteVerbose(sprintf "Getting indents from %s." x.Path)
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.Read, FileShare.Read)
        GetFileIndentsCommand.DetectIndents x x.Path fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
