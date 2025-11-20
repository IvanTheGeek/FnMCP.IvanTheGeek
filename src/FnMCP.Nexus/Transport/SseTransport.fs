module FnMCP.Nexus.Transport.SseTransport

open System
open System.IO
open System.Text.Json
open System.Text.Json.Serialization
open Microsoft.AspNetCore.Http
open Oxpecker
open FnMCP.Nexus.Types
open FnMCP.Nexus.McpServer

// Server-Sent Events (SSE) transport for MCP
// https://spec.modelcontextprotocol.io/specification/basic/transports/#server-sent-events-sse
// Using Oxpecker 1.0 API: EndpointHandler = HttpContext -> Task

// JSON serialization options
let jsonOptions = JsonSerializerOptions()
jsonOptions.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase
jsonOptions.WriteIndented <- false
jsonOptions.DefaultIgnoreCondition <- JsonIgnoreCondition.WhenWritingNull

// Log to stderr
let log message =
    Console.Error.WriteLine($"[SseTransport] {message}")

// SSE endpoint handler - establishes event stream
let handleSseEndpoint (server: McpServer) : EndpointHandler =
    fun (ctx: HttpContext) ->
        task {
            log "SSE connection established"

            // Set SSE headers
            ctx.Response.Headers.Append("Content-Type", "text/event-stream")
            ctx.Response.Headers.Append("Cache-Control", "no-cache")
            ctx.Response.Headers.Append("Connection", "keep-alive")
            ctx.Response.Headers.Append("X-Accel-Buffering", "no") // Disable nginx buffering

            // Keep connection alive
            // In a full implementation, this would:
            // 1. Maintain a registry of active SSE connections
            // 2. Send events when notifications occur
            // 3. Handle client disconnection gracefully

            // For now, send a welcome message and keep alive
            use writer = new StreamWriter(ctx.Response.Body)

            // Construct full URL for message endpoint
            let scheme = ctx.Request.Scheme
            let host = ctx.Request.Host.ToUriComponent()
            let messageEndpointUrl = $"{scheme}://{host}/sse/message"

            // Send initial event with full URL
            do! writer.WriteLineAsync("event: endpoint")
            do! writer.WriteLineAsync($"data: {messageEndpointUrl}")
            do! writer.WriteLineAsync("")
            do! writer.FlushAsync()

            log $"Sent endpoint URL: {messageEndpointUrl}"

            log "SSE welcome sent, keeping connection alive"

            // Keep alive loop (send ping every 30 seconds)
            let mutable shouldContinue = true
            while shouldContinue && not ctx.RequestAborted.IsCancellationRequested do
                try
                    do! System.Threading.Tasks.Task.Delay(30000, ctx.RequestAborted)
                    let timestamp = DateTime.UtcNow.ToString("o")
                    do! writer.WriteLineAsync("event: ping")
                    do! writer.WriteLineAsync($"data: {timestamp}")
                    do! writer.WriteLineAsync("")
                    do! writer.FlushAsync()
                with
                | :? System.OperationCanceledException ->
                    shouldContinue <- false
                    log "SSE connection cancelled by client"
                | ex ->
                    shouldContinue <- false
                    log $"SSE error: {ex.Message}"

            log "SSE connection closed"
        }

// Message POST endpoint - receives client messages
let handleMessagePost (server: McpServer) : EndpointHandler =
    fun (ctx: HttpContext) ->
        task {
            try
                use reader = new StreamReader(ctx.Request.Body)
                let! body = reader.ReadToEndAsync()

                log $"Received message: {body.Substring(0, min 100 body.Length)}..."

                // Parse JSON-RPC request
                let jsonRpcRequest = JsonSerializer.Deserialize<JsonRpcRequest>(body, jsonOptions)

                // Process request
                let! result = server.HandleRequest(jsonRpcRequest) |> Async.StartAsTask

                // Return response
                let response = server.CreateResponse(jsonRpcRequest.Id, result)
                let responseJson = JsonSerializer.Serialize(response, jsonOptions)

                ctx.Response.ContentType <- "application/json"
                return! ctx |> text responseJson

            with ex ->
                log $"Error processing message: {ex.Message}"
                ctx.Response.StatusCode <- 500
                return! ctx |> text $"Internal server error: {ex.Message}"
        }
