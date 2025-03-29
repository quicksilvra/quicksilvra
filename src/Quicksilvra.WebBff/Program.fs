namespace Quicksilvra.WebBff
#nowarn "20"
open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        let app = builder.Build()


        app.MapGet("/test", Func<HttpContext, Task<string>>(fun _ ->
            task {
                do! Task.Delay(50)
                return "Ok from F#"
            }
        )) |> ignore

        app.Run()

        exitCode