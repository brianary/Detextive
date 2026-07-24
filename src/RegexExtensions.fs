namespace Detextive

open System
open System.Runtime.CompilerServices
open System.Text.RegularExpressions

/// Convenience methods for Regex objects.
[<Extension>]
type RegexExtensions() =

    /// Removes matching patterns from the input string.
    [<Extension>]
    static member RemoveMatches(re:Regex,s) =
        re.Replace(s, String.Empty)

    /// Returns the portion of the string that matches the pattern.
    [<Extension>]
    static member GetMatchValue(re:Regex,s) =
        re.Match(s).Value
