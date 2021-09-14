namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// End of line digraph or character.
type public LineEnding =
    | Mixed = 0
    | CRLF = 1
    | LF = 2
    | CR = 3
    | NEL = 4
    | LS = 5
    | PS = 6

/// The details returned by the cmdlet.
type public LineEndingsResult =
    { Path : string; LineEndings : LineEnding;
         CRLF : int; LF : int; CR : int; NEL : int; LS : int; PS : int }

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
                        | '\r' when text.[i+1] = '\n' -> Some(LineEnding.CRLF, text.IndexOfAny(endings, i+2))
                        | '\r' -> Some(LineEnding.CR, text.IndexOfAny(endings, i+1))
                        | '\n' -> Some(LineEnding.LF, text.IndexOfAny(endings, i+1))
                        | '\u0085' -> Some(LineEnding.NEL, text.IndexOfAny(endings, i+1))
                        | '\u2028' -> Some(LineEnding.LS, text.IndexOfAny(endings, i+1))
                        | '\u2029' -> Some(LineEnding.PS, text.IndexOfAny(endings, i+1))
                        | _ -> Some(LineEnding.Mixed, text.IndexOfAny(endings, i+1)) )
            (text.IndexOfAny(endings))
            |> List.countBy id
            |> Map.ofList

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
        let counts = GetFileLineEndingsCommand.CountLineEndings fs
        counts |> sprintf "Line endings for %s : %A" x.Path |> x.WriteVerbose
        let total e = Map.tryFind e counts |> Option.defaultValue 0
        { Path = x.Path;
            LineEndings = (if (Map.count counts) = 1 then Map.toList counts |> List.head |> fst else LineEnding.Mixed);
            CRLF = total LineEnding.CRLF;
            LF = total LineEnding.LF;
            CR = total LineEnding.CR;
            NEL = total LineEnding.NEL;
            LS = total LineEnding.LS;
            PS = total LineEnding.PS } |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
