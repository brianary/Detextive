namespace Detextive

open System
open System.Management.Automation
open System.Runtime.CompilerServices

/// Convenience methods for PSCmdlet objects.
[<Extension>]
type PSCmdletExtensions() =

    /// Returns true if the item exists.
    [<Extension>]
    static member Exists(cmdlet:PSCmdlet, item) =
        cmdlet.InvokeProvider.Item.Exists(item)

    /// Returns all matching PSProvider items.
    [<Extension>]
    static member GetItems(cmdlet:PSCmdlet, itemglob) =
        cmdlet.InvokeProvider.Item.Get(itemglob)
            |> Collection.toSeq
            |> Seq.map string
            |> Seq.filter (cmdlet.InvokeProvider.Item.IsContainer >> not)

    /// Returns the first matching PSProvider item.
    [<Extension>]
    static member GetItem(cmdlet:PSCmdlet, item) =
        try Seq.head (cmdlet.GetItems item) with
        | :? ArgumentException -> sprintf "Unable to find %s" item |> ItemNotFoundException |> raise
