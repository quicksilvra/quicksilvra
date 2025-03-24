using Quicksilvra.Auth.Config;
using Quicksilvra.Idp;
using Serilog;
using static Quicksilvra.Auth.Config.ConfigurationParser;
using static Quicksilvra.Auth.Config.Network;
using static Quicksilvra.Auth.Config.DataProtection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var license = toString(builder.Configuration["DuendeIdentityServerLicense"]);

    var networkConfiguration = ParseNetworkConfiguration(builder);
    var dataProtectionConfiguration = ParseDataProtectionConfiguration(builder);
    
    var x509 = Certificate.create(dataProtectionConfiguration, networkConfiguration.Proxied);
    
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices(license,networkConfiguration,dataProtectionConfiguration,x509)
        .ConfigurePipeline();
    
    app.Run();
    
    x509.Dispose();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

return;


IdpNetworkConfiguration ParseNetworkConfiguration(WebApplicationBuilder webApplicationBuilder)
{
    return parseNetworkConfiguration(webApplicationBuilder.Configuration.GetSection(nameof(IdpNetworkConfiguration)));
}

DataProtectionConfiguration ParseDataProtectionConfiguration(
    WebApplicationBuilder webApplicationBuilder)
{
    return parseDataProtectionConfiguration(
        webApplicationBuilder.Configuration.GetSection(nameof(DataProtectionConfiguration)));
}