namespace Quicksilvra.Auth.Config

module Client =

    let private responseType = "code"
    let private callBackPath = "/signin-oidc"
    let private signOutCallBackPath = "/signout-callback-oidc"

    type Client =
        { Authority: string
          CallBackPath: string
          ClientName: string
          ClientId: string
          ClientSecret: string
          HashedClientSecret: string
          RedirectUris: string []
          ResponseType: string
          SignOutCallBackPath: string
          SignedOutCallbackPaths: string []
          AllowOfflineAccess: bool
          Scopes: string seq }

    let private defaultClient =
        { Authority = empty
          CallBackPath = empty
          ClientName = empty
          ClientId = empty
          ClientSecret = empty
          HashedClientSecret = empty
          RedirectUris = Array.empty
          ResponseType = responseType
          SignOutCallBackPath = empty
          SignedOutCallbackPaths = Array.empty
          AllowOfflineAccess = false
          Scopes =
            [ StandardScopes.Profile
              StandardScopes.OpenId
              StandardScopes.Email
              StandardScopes.OfflineAccess ] }

    let private createCore
        proxied
        authorityInternalUri
        authorityExternalUri
        serviceInternalUri
        serviceExternalUri
        name
        id
        secret
        =
        { defaultClient with
            Authority =
                if proxied then
                    authorityExternalUri
                else
                    authorityInternalUri
            CallBackPath = callBackPath
            ClientName = name
            ClientId = id
            ClientSecret = secret
            HashedClientSecret = sha256 <| Some secret
            AllowOfflineAccess = true
            RedirectUris =
                [| $"{serviceInternalUri}{callBackPath}"
                   $"{serviceExternalUri}{callBackPath}" |]
            SignOutCallBackPath = signOutCallBackPath
            SignedOutCallbackPaths =
                [| $"{serviceInternalUri}{signOutCallBackPath}"
                   $"{serviceExternalUri}{signOutCallBackPath}" |] }



    let create networkConfiguration name id secret =
        createCore
            networkConfiguration.IdpNetworkConfiguration.Proxied
            networkConfiguration.IdpNetworkConfiguration.AuthorityInternalUri
            networkConfiguration.IdpNetworkConfiguration.AuthorityExternalUri
            networkConfiguration.ServiceInternalUri
            networkConfiguration.ServiceExternalUri
            name
            id
            secret

    let webBffClient networkConfiguration secret =
        create networkConfiguration "The Grind Or Die Web Bff" "thegrindordiewebbffclient" secret
