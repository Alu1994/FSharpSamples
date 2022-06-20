namespace FsharpWebApplication

open Giraffe
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting

module Handlers = 
    let helloWorld200 : HttpHandler = text "Hello, world"

    let helloWorld202 : HttpHandler = 
        setStatusCode 202 >=> xml "Hello, world"

type Startup() = 
    member _.ConfigureServices(services: IServiceCollection) = 
        services.AddGiraffe() |> ignore

    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        app.UseGiraffe Handlers.helloWorld202

