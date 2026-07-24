module Detextive.FilePosition

open System
open System.IO

/// The maximum buffer size to use when reading files.
let private MaxBufferSize = Int32.MaxValue

/// Seeks the current position to the beginning of the file.
let Rewind (fs:FileStream) =
    if fs.Position > 0L then fs.Seek(0L, SeekOrigin.Begin) |> ignore

/// Returns a buffer size based on the file size.
let GetBufferSize (fs:FileStream) =
    let size = match fs.Seek(0L, SeekOrigin.End) with
               | pos when pos > int64 MaxBufferSize -> MaxBufferSize
               | pos -> int pos
    Rewind fs
    size
