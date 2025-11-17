module FnMCP.Nexus.Http.HttpServer

open System
open System.Text
open System.Text.Json
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Oxpecker
open FnMCP.Nexus.McpServer
open FnMCP.Nexus.Transport.SseTransport
open FnMCP.Nexus.Transport.WebSocketTransport
open FnMCP.Nexus.Auth.AuthMiddleware

// HTTP server for remote MCP access via SSE and WebSocket
// Using Oxpecker 1.0 API: EndpointHandler = HttpContext -> Task

let log message =
    Console.Error.WriteLine($"[HttpServer] {message}")

// Configure application routes and middleware
let configureApp (eventStorePath: string) (server: McpServer) (app: WebApplication) =

    // Health check endpoint (no auth required)
    let healthCheck : EndpointHandler =
        fun ctx ->
            task {
                ctx.Response.ContentType <- "application/json"
                let response = {|
                    status = "ok"
                    service = "Nexus MCP Server"
                    version = "0.3.0"
                    transport = "http"
                    timestamp = DateTime.UtcNow.ToString("o")
                |}
                let jsonString = JsonSerializer.Serialize(response)
                let bytes = Encoding.UTF8.GetBytes(jsonString)
                do! ctx.Response.Body.WriteAsync(bytes, 0, bytes.Length)
            }

    // SSE routes (auth required)
    let sseRoutes = [
        // SSE event stream endpoint
        GET [
            route "/events" (requireApiKey eventStorePath >=> handleSseEndpoint server)
        ]
        // Message POST endpoint
        POST [
            route "/message" (requireApiKey eventStorePath >=> handleMessagePost server)
        ]
    ]

    // Main routes
    let routes = [
        // Health check (no auth)
        GET [
            route "/" healthCheck
            route "/health" healthCheck
        ]

        // SSE endpoints
        subRoute "/sse" sseRoutes

        // WebSocket endpoint (auth required)
        route "/ws" (requireApiKey eventStorePath >=> handleWebSocket server)
    ]

    // Enable WebSocket support
    app.UseWebSockets() |> ignore

    // Use routing
    app.UseRouting() |> ignore

    // Use Oxpecker with routes
    app.UseOxpecker(routes) |> ignore

    app

// Start HTTP server
let startHttpServer (port: int) (eventStorePath: string) (server: McpServer) : Async<unit> =
    async {
        log $"Starting HTTP server on port {port}..."

        let builder = WebApplication.CreateBuilder()

        // Add services
        builder.Services.AddRouting() |> ignore

        // Build app
        let app = builder.Build()

        // Configure app
        configureApp eventStorePath server app |> ignore

        // Set listening URL
        app.Urls.Clear()
        app.Urls.Add($"http://0.0.0.0:{port}")

        log $"HTTP server configured:"
        log $"  - Health check: http://0.0.0.0:{port}/"
        log $"  - SSE endpoint: http://0.0.0.0:{port}/sse/events"
        log $"  - SSE message: http://0.0.0.0:{port}/sse/message"
        log $"  - WebSocket: ws://0.0.0.0:{port}/ws"
        log $"All endpoints except health require Bearer token authentication"

        // Run server
        do! app.RunAsync() |> Async.AwaitTask
    }
