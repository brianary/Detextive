namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// The details returned by the cmdlet.
[<StructuredFormatDisplay("{Encoding} {Indents} {LineEndings}")>]
type public TextContentsResult =
    { Path : string
      IsBinary : bool
      Encoding : Encoding
      Utf8Signature : bool
      Indents : IndentsResult
      LineEndings : LineEndingsResult
      FinalNewline : bool }
    override x.ToString () =
        if x.IsBinary then "binary file"
        else
            sprintf "%s%s %s %s%s"
                x.Encoding.WebName
                (if x.Utf8Signature then " with signature" else "")
                (string x.Indents)
                (string x.LineEndings)
                (if not x.FinalNewline then " missing final newline" else "")

/// Returns formatting details about a text file's contents: encoding, indents, line endings, &c.
[<Cmdlet(VerbsCommon.Get, "FileContentsInfo")>]
[<OutputType(typeof<IndentsResult>)>]
type public GetFileContentsInfoCommand () =
    inherit PSCmdlet ()

    /// A file to examine.
    [<Parameter(Position=0)>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.WriteVerbose(sprintf "Examining file %s contents." x.Path)
        use fs = new FileStream(x.Path, FileMode.Open, FileAccess.Read, FileShare.Read)
        let counts = GetFileIndentsCommand.CountIndents fs
        counts |> sprintf "Indents for %s : %A" x.Path |> x.WriteVerbose
        let total e = Map.tryFind e counts |> Option.defaultValue 0
        { Path = x.Path
          IsBinary = TestTextFileCommand.IsTextFile fs |> not
          Encoding = GetFileEncodingCommand.DetectFileEncoding fs
          Utf8Signature = TestUtf8SignatureCommand.HasUtf8Signature fs
          Indents = GetFileIndentsCommand.DetectIndents x x.Path fs
          LineEndings = GetFileLineEndingsCommand.DetectLineEndings x x.Path fs
          FinalNewline = TestFinalNewlineCommand.HasFinalNewline fs } |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
