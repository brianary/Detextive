/// Constants used for ratios in byte value data analysis.
module Detextive.Ratio

/// The minimum ratio of low control char byte values in a probably binary file,
/// assuming Benford's Law applies to some degree.
[<Literal>]
let MinBinaryLowControls = 1E-2

/// The minimum ratio of common EBCDIC character byte values (space, tab)
/// or unlikely non-EBCDIC value (7F is quote, but delete in most other encodings)
/// in a probably EBCDIC file.
[<Literal>]
let MinCommonEbcdic = 17E-2

/// The minimum ratio of zeros in UTF-16LE or UTF-16BE encoding,
/// assuming a preponderance of ASCII-range chars in the data.
[<Literal>]
let MinDoubleByteZeros = 46E-2

/// The minimum ratio of zeros in UTF-32LE or UTF-32BE encoding,
/// assuming a preponderance of ASCII-range chars in the data.
[<Literal>]
let MinFourByteZeros = 72E-2
