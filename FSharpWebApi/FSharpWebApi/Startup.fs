namespace FsharpWebApplication

open Giraffe
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting

type Startup() = 
    member _.ConfigureServices(services: IServiceCollection) = 
        services.AddGiraffe() |> ignore

    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        //app.UseGiraffe (HttpHandlers.sayHello "Hello from Giraffe")
        app.UseGiraffe HttpHandlers.webApp

