namespace Quicksilvra.Auth.Config

[<AutoOpen>]
module Common =
    let empty = ""


[<AutoOpen>]
module ConsoleLogging =
    let show message = printfn $"%s{message}"