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

/// Returns whether the file is binary or text, and what encoding, line endings, and indents text files contain.
[<Cmdlet(VerbsCommon.Get, "FileContentsInfo")>]
[<OutputType(typeof<TextContentsResult>)>]
type public GetFileContentsInfoCommand () =
    inherit PSCmdlet ()

    /// Process an individual item.
    static member public GetFileContentsInfo (cmdlet:PSCmdlet) item =
        cmdlet.WriteVerbose(sprintf "Examining file %s contents." item)
        use fs = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.Read)
        if TestTextFileCommand.IsTextFile fs |> not then
            { TextContentsResult.BinaryDefault with Path = item }
        else
            { Path = item
              IsBinary = TestTextFileCommand.IsTextFile fs |> not
              Encoding = GetFileEncodingCommand.DetectFileEncoding fs
              Utf8Signature = TestUtf8SignatureCommand.HasUtf8Signature fs
              Indents = GetFileIndentsCommand.DetectIndents cmdlet item fs
              LineEndings = GetFileLineEndingsCommand.DetectLineEndings cmdlet item fs
              FinalNewline = TestFinalNewlineCommand.HasFinalNewline fs false }

    /// A file to examine.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        x.GetItems x.Path
            |> Seq.map (GetFileContentsInfoCommand.GetFileContentsInfo x)
            |> Seq.toList
            |> List.iter x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
