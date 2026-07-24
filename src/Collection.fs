/// Functions for working with collections.
module Detextive.Collection

open System.Collections.ObjectModel

/// Converts a collection into a sequence.
let toList<'T> (collection:Collection<'T>) =
    [ for i in 0..(collection.Count-1) -> collection.Item(i) ]

/// Converts a collection into a sequence.
let toSeq<'T> (collection:Collection<'T>) =
    seq { for i in 0..(collection.Count-1) -> collection.Item(i) }
