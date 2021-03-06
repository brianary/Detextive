namespace Detextive

open System
open System.Management.Automation
open System.Text
open EditorConfig.Core

/// The details returned by the Get-FileEditorConfig cmdlet.
[<StructuredFormatDisplay("{StringValue}")>]
type public FileEditorConfigResult =
    { Path : string
      Encoding : Encoding
      Utf8Signature : bool
      Indents : IndentType
      IndentSize : int
      LineEndings : LineEndingType
      FinalNewline : bool }
    member x.StringValue =
        sprintf "%s: %s%s, %s indents, %s line endings%s"
            x.Path
            x.Encoding.WebName
            (if x.Utf8Signature then "-bom" else "")
            (string x.Indents)
            (string x.LineEndings)
            (if not x.FinalNewline then ", missing final newline" else "")
    override x.ToString () = x.StringValue

/// Looks up the editorconfig values set for a file.
[<Cmdlet(VerbsCommon.Get, "FileEditorConfig")>]
type public GetFileEditorConfigCommand () =
    inherit PSCmdlet ()

    static member public GetFileConfiguration (parser:EditorConfigParser) =
        Array.singleton >> parser.Parse >> Seq.head

    /// Process an individual item.
    static member public GetFileEditorConfig (cmdlet:PSCmdlet) item (cfg:FileConfiguration) =
        sprintf "Config: %s = { %A, %A, %A, %A }" item cfg.Charset cfg.IndentStyle cfg.EndOfLine cfg.InsertFinalNewline
            |> cmdlet.WriteVerbose
        { Path          = item
          Encoding      = if not cfg.Charset.HasValue then Encoding.Default else
                          match cfg.Charset.Value with
                          | Charset.UTF8BOM -> UTF8Encoding(true) :> Encoding
                          | Charset.UTF8 -> Encoding.UTF8
                          | Charset.UTF16LE -> Encoding.Unicode
                          | Charset.UTF16BE -> Encoding.BigEndianUnicode
                          | Charset.Latin1 -> Encoding.GetEncoding("latin-1")
                          | _ -> Encoding.Default
          Utf8Signature = cfg.Charset.HasValue && cfg.Charset.Value = Charset.UTF8BOM
          Indents       = if not cfg.IndentStyle.HasValue then IndentType.None else
                          match cfg.IndentStyle.Value with
                          | IndentStyle.Tab -> IndentType.Tabs
                          | IndentStyle.Space -> IndentType.Spaces
                          | _ -> IndentType.None
          IndentSize    = if cfg.IndentSize.UseTabWidth then (if cfg.TabWidth.HasValue then cfg.TabWidth.Value else 0)
                          elif cfg.IndentSize.NumberOfColumns.HasValue then cfg.IndentSize.NumberOfColumns.Value else 0
          LineEndings   = if not cfg.EndOfLine.HasValue then LineEndingType.None else
                          match cfg.EndOfLine.Value with
                          | EndOfLine.LF -> LineEndingType.LF
                          | EndOfLine.CR -> LineEndingType.CR
                          | EndOfLine.CRLF -> LineEndingType.CRLF
                          | _ -> LineEndingType.None
          FinalNewline  = (not cfg.InsertFinalNewline.HasValue) || cfg.InsertFinalNewline.Value }

    /// A file to retrieve the editorconfig values for.
    [<Parameter(Position=0,Mandatory=true,ValueFromPipelineByPropertyName=true)>]
    [<Alias("FullName")>]
    [<ValidateNotNullOrEmpty>]
    member val Path : string = "" with get, set

    override x.BeginProcessing () =
        base.BeginProcessing ()

    override x.ProcessRecord () =
        base.ProcessRecord ()
        let getCfg = GetFileEditorConfigCommand.GetFileConfiguration <| EditorConfigParser()
        x.GetItems x.Path
            |> Seq.map (fun item -> GetFileEditorConfigCommand.GetFileEditorConfig x item (getCfg item))
            |> x.WriteObject

    override x.EndProcessing () =
        base.EndProcessing ()
