namespace Quicksilvra.Auth.Config

open System
open System.Security.Cryptography
open System.Text

[<AutoOpen>]
module Hashing =

    let sha256 (input: string Option) =
        match input with
        | None -> empty
        | Some toHash ->
            use sha = SHA256.Create()
            let bytes = Encoding.UTF8.GetBytes toHash
            let hash = sha.ComputeHash bytes
            Convert.ToBase64String hash
