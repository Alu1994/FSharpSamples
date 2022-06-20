module HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Net.Http.Headers
open System.Globalization
open System.IO
open System.Threading.Tasks

let helloWorld200 : HttpHandler = text "Hello, world"

let helloWorld202 : HttpHandler = 
    setStatusCode 202 >=> xml "Hello, world"

let sayHello (text:string) (next:HttpFunc) (ctx:HttpContext) = 
    task {
        let bytes = System.Text.Encoding.UTF8.GetBytes text
        ctx.SetContentType "text/plain; charset=utf-8"
        ctx.SetHttpHeader (HeaderNames.ContentLength, bytes.Length)
        ctx.Response.StatusCode <- 202

        do! ctx.Response.Body.WriteAsync(bytes, 0, bytes.Length)  // 'F#' do! == await 'C#'

        return! next ctx
    }

let hello : HttpHandler = text "Hello, World"
let helloXml : HttpHandler = xml "Hello, World"
let helloJson : HttpHandler = json {| Message = "Hello, World" |}

let listFiles : HttpHandler =
    let options = EnumerationOptions(RecurseSubdirectories = true)
    let files = Directory.EnumerateFiles(path = ".", searchPattern = "*", enumerationOptions = options) |> Seq.toArray
    json
        {|  Message = "Goodbye!"
            NumberOfFiles = files.Length
            Files = files|}
        
let cultureHandler next (ctx:HttpContext) = 
    let cultureQuery = ctx.Request.Query.["Culture"]
    if (cultureQuery.Count = 1) then
        let culture = CultureInfo cultureQuery.[0]
        CultureInfo.CurrentCulture <- culture
        CultureInfo.CurrentUICulture <- culture
    next ctx

let reportCulture next ctx = 
    text $"Current culture is {CultureInfo.CurrentUICulture}" next ctx

let setAndReport : HttpHandler = cultureHandler >=> reportCulture

let basicRoute : HttpHandler = route "/hello" >=> GET >=> text "Hello, world"

let isFSharpCool (isItCool:bool) : HttpHandler =
    if isItCool then text $"F# is damn cool!"
    else text $"Try again! ;)"

let fsharpRoute : HttpHandler = routef "/isFSharpCool/%b" isFSharpCool

let webApp : HttpHandler =
    choose [
        route "/hello" >=> hello
        route "/xml" >=> helloXml
        route "/json" >=> helloJson
        route "/listFiles" >=> listFiles
        routef "/isFSharpCool/%b"  isFSharpCool
        route "/culture" >=> setAndReport
        route "/view" >=> htmlView MatthewsViewEngine.createTable
    ]