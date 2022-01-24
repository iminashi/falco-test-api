module Program

open System.Threading.Tasks
open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.Builder

let exceptionHandler : HttpHandler =
    Response.withStatusCode 500
    >> Response.ofPlainText "Server error"

let getAllHandler : HttpHandler =
    fun ctx ->
        task {
            let! data = Db.getAll ()
            do! Response.ofJson data ctx
        }

[<EntryPoint>]
let main args =
    webHost args {
        use_compression
        use_if    FalcoExtensions.IsDevelopment DeveloperExceptionPageExtensions.UseDeveloperExceptionPage
        use_ifnot FalcoExtensions.IsDevelopment (FalcoExtensions.UseFalcoExceptionHandler exceptionHandler)

        endpoints [
            get "/" getAllHandler
        ]
    }

    0
