using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.HttpOverrides;
using Quicksilvra.Auth.Config;
using Serilog;

namespace Quicksilvra.Idp;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, 
        string license,
        Network.IdpNetworkConfiguration networkConfiguration,
        DataProtection.DataProtectionConfiguration dataProtectionConfiguration,
        X509Certificate2 x509)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                options.LicenseKey = license;

                options.EmitStaticAudienceClaim = true;
                options.KeyManagement.Enabled = false;

                if (networkConfiguration.Proxied)
                {
                    options.IssuerUri = networkConfiguration.AuthorityExternalUri;
                }
            })
            .AddSigningCredential(x509)
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddTestUsers(TestUsers.Users);
        
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        
        
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
        }
        
        app.UseForwardedHeaders();

        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();


        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}