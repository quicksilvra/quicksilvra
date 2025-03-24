namespace Quicksilvra.Auth.Config

open System.Security.Cryptography.X509Certificates

module Certificate =
    let create (dataProtectionConfiguration: DataProtectionConfiguration) isProxied =
        let location = dataProtectionConfiguration.X509Location(isProxied)
        X509CertificateLoader.LoadPkcs12FromFile(location, dataProtectionConfiguration.X509__Key)
