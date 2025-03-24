namespace Quicksilvra.Auth.Config

module Hijacker =
    open Microsoft.AspNetCore.Authentication.OpenIdConnect
    open Microsoft.AspNetCore.Http.Extensions    
    
    let replaceUri (networkConfiguration: IdpNetworkConfiguration) (uri: string) =
        show $"Try to substitute: {uri}"

        let fromInternalToExternal =
            uri
                .Replace(networkConfiguration.AuthorityInternalUri, networkConfiguration.AuthorityExternalUri)
                .Replace("http://", "https://")

        show $"From internal to external URI result: {fromInternalToExternal}"
        fromInternalToExternal

    let Hijack (networkConfiguration: IdpNetworkConfiguration) (context: RedirectContext) =
        async {

            let maybePostLogoutRedirectUri =
                Option.ofObj context.ProtocolMessage.PostLogoutRedirectUri

            match maybePostLogoutRedirectUri with
            | Some uri ->
                let fullUrl = context.Request.GetDisplayUrl()
                show "PostLogoutRedirectUri Logging DisplayUri"
                show fullUrl
                context.ProtocolMessage.PostLogoutRedirectUri <- replaceUri networkConfiguration uri
            | _ -> ()

            let maybeIssuerAddress = Option.ofObj context.ProtocolMessage.IssuerAddress

            match maybeIssuerAddress with
            | Some address ->
                let fullUrl = context.Request.GetDisplayUrl()
                show "Issuer Address Logging DisplayUri"
                show fullUrl
                context.ProtocolMessage.IssuerAddress <- replaceUri networkConfiguration address
            | _ -> ()

            let maybeRedirectUri = Option.ofObj context.ProtocolMessage.RedirectUri

            match maybeRedirectUri with
            | Some redirectUri ->
                let fullUrl = context.Request.GetDisplayUrl()
                show "RedirectUri Logging DisplayUri"
                show fullUrl
                context.ProtocolMessage.RedirectUri <- replaceUri networkConfiguration redirectUri
            | _ -> ()

            return ()
        }
        |> Async.StartAsTask