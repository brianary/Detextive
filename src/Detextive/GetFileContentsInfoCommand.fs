namespace Detextive

open System.IO
open System.Management.Automation
open System.Text

/// The details returned by the Get-FileContentsInfo cmdlet.
[<StructuredFormatDisplay("{StringValue}")>]
type public TextContentsResult =
    { Path : string
      IsBinary : bool
      Encoding : Encoding
      Utf8Signature : bool
      Indents : IndentsResult
      LineEndings : LineEndingsResult
      FinalNewline : bool }
    member x.StringValue =
        if x.IsBinary then sprintf "%s: binary file" x.Path
        else
            sprintf "%s: %s%s, %s indents, %s line endings%s"
                x.Path
                x.Encoding.WebName
                (if x.Utf8Signature then "-bom" else "")
                (string x.Indents.Indents)
                (string x.LineEndings.LineEndings)
                (if not x.FinalNewline then ", missing final newline" else "")
    override x.ToString () = x.StringValue
    static member BinaryDefault =
        { Path = ""
          IsBinary = true
          Encoding = null
          Utf8Signature = false
          Indents = { Path = ""; Indents = IndentType.None; Mixed = 0; Tabs = 0; Spaces = 0; Other = 0 }
          LineEndings = { Path = ""; LineEndings = LineEndingType.None; CRLF = 0; LF = 0; CR = 0; NEL = 0; LS = 0; PS = 0 }
          FinalNewline = false }

/// Returns formatting details about a text file's contents: encoding, indents, line endings, &c.
[<Cmdlet(VerbsCommon.Get, "FileContentsInfo")>]
[<OutputType(typeof<TextContentsResult>)>]
type public GetFileContentsInfoCommand () =
    inherit PSCmdlet ()

    /// Process an individual item.
    member x.ProcessItem item =
        x.WriteVerbose(sprintf "Examining file %s contents." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        let r =
            if TestTextFileCommand.IsTextFile fs |> not then { TextContentsResult.BinaryDefault with Path = item }
            else
                { Path = item
                  IsBinary = TestTextFileCommand.IsTextFile fs |> not
                  Encoding = GetFileEncodingCommand.DetectFileEncoding fs
                  Utf8Signature = TestUtf8SignatureCommand.HasUtf8Signature fs
                  Indents = GetFileIndentsCommand.DetectIndents x item fs
                  LineEndings = GetFileLineEndingsCommand.DetectLineEndings x item fs
                  FinalNewline = TestFinalNewlineCommand.HasFinalNewline fs }
        x.WriteVerbose(sprintf "File %s: %s" item (string r))
        r |> x.WriteObject

    /// A file to examine.
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
