/// Functions for working with IList.
module Detextive.IList

open System.Collections

/// Converts an IList into a sequence.
let toStringList (ilist:IList) =
    [ for i in 0..(ilist.Count-1) -> ilist.Item(i) |> string ]
