namespace Quicksilvra.Auth.Config

open Microsoft.Extensions.Configuration
open Microsoft.VisualBasic.CompilerServices


[<AutoOpen>]
module ConfigurationParser =

    let toBoolCore (value: string option) : bool =
        match value with
        | Some v ->
            match v.Trim().ToLower() with
            | "true" -> true
            | "false" -> false
            | _ -> raise (IncompleteInitialization())
        | None -> raise (IncompleteInitialization())

    let toStringCore (value: string option) : string =
        match value with
        | Some v -> v
        | None -> raise (IncompleteInitialization())

    let toBool value = value |> Option.ofObj |> toBoolCore
    let toString value = value |> Option.ofObj |> toStringCore    

    let parseNetworkConfiguration (configSection: IConfigurationSection) =
        { IdpNetworkConfiguration.Proxied = configSection["Proxied"] |> toBool
          AuthorityInternalUri = configSection["AuthorityInternalUri"] |> toString
          AuthorityExternalUri = configSection["AuthorityExternalUri"] |> toString }

    let parseDataProtectionConfiguration (configSection: IConfigurationSection) =
            { X509__FileName = configSection["X509:FileName"] |> toString
              X509__Key = configSection["X509:Key"] |> toString
              X509__Path = configSection["X509:Path"] |> toString
              DataProtectionPath = configSection["DataProtectionPath"] |> toString }