namespace Quicksilvra.Auth.Config

open System
open System.IO

[<AutoOpen>]
module DataProtection =
    type DataProtectionConfiguration =
        { X509__FileName: string
          X509__Key: string
          X509__Path: string
          DataProtectionPath: string }

        member this.homeDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)

        member this.X509Location isProxied =
            let basePath = if not isProxied then this.homeDirectory else ""
            String.concat
                (Path.DirectorySeparatorChar.ToString())
                (seq [ basePath
                       this.X509__Path
                       this.X509__FileName ])

        member this.HomeDataProtectionPath isProxied =
            let basePath = if not isProxied then this.homeDirectory else ""
            String.concat
                (Path.DirectorySeparatorChar.ToString())
                (seq [ basePath
                       this.DataProtectionPath ])
