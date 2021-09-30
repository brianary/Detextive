namespace Detextive

open System.IO
open System.Management.Automation

/// End of line digraph or character.
type public LineEndingType =
    | None = 0
    | Mixed = 1
    | CRLF = 2
    | LF = 3
    | CR = 4
    | NEL = 5
    | LS = 6
    | PS = 7

/// The details returned by the cmdlet.
[<StructuredFormatDisplay("{LineEndings}")>]
type public LineEndingsResult =
    { Path : string
      LineEndings : LineEndingType
      CRLF : int
      LF : int
      CR : int
      NEL : int
      LS : int
      PS : int }
    override x.ToString () = x.LineEndings.ToString()

/// Returns details about a file's line endings.
[<Cmdlet(VerbsCommon.Get, "FileLineEndings")>]
[<OutputType(typeof<LineEndingsResult>)>]
type public GetFileLineEndingsCommand () =
    inherit PSCmdlet ()

    /// Counts the line ending digraph or characters.
    static member public CountLineEndings (fs:FileStream) =
        let enc = GetFileEncodingCommand.DetectFileEncoding fs
        if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore
        use sr = new StreamReader(fs, enc, true, -1, true)
        let text = sr.ReadToEnd()
        let endings = [|'\u000A';'\u000D';'\u0085';'\u2028';'\u2029'|]
        List.unfold
            (function
                | -1 -> None
                |  i -> match text.[i] with
                        | '\r' when text.[i+1] = '\n' -> Some(LineEndingType.CRLF, text.IndexOfAny(endings, i+2))
                        | '\r' -> Some(LineEndingType.CR, text.IndexOfAny(endings, i+1))
                        | '\n' -> Some(LineEndingType.LF, text.IndexOfAny(endings, i+1))
                        | '\u0085' -> Some(LineEndingType.NEL, text.IndexOfAny(endings, i+1))
                        | '\u2028' -> Some(LineEndingType.LS, text.IndexOfAny(endings, i+1))
                        | '\u2029' -> Some(LineEndingType.PS, text.IndexOfAny(endings, i+1))
                        | _ -> Some(LineEndingType.Mixed, text.IndexOfAny(endings, i+1)) )
            (text.IndexOfAny(endings))
            |> List.countBy id
            |> Map.ofList

    /// Returns the line ending details detected.
    static member public DetectLineEndings (x:PSCmdlet) (p:string) (fs:FileStream) =
        let counts = GetFileLineEndingsCommand.CountLineEndings fs
        counts |> sprintf "Line endings for %s : %A" p |> x.WriteVerbose
        let total e = Map.tryFind e counts |> Option.defaultValue 0
        { Path = p
          LineEndings = match Map.count counts with
                        | 0 -> LineEndingType.None
                        | 1 -> Map.toList counts |> List.head |> fst
                        | _ -> LineEndingType.Mixed
          CRLF = total LineEndingType.CRLF
          LF = total LineEndingType.LF
          CR = total LineEndingType.CR
          NEL = total LineEndingType.NEL
          LS = total LineEndingType.LS
          PS = total LineEndingType.PS }

    /// A file to test.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.WriteVerbose(sprintf "Getting line endings from %s." x.Path)
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.Read, FileShare.Read)
        GetFileLineEndingsCommand.DetectLineEndings x x.Path fs |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
