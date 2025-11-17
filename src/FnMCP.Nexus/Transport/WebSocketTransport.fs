module FnMCP.Nexus.Transport.WebSocketTransport

open System
open System.Net.WebSockets
open System.Text
open System.Text.Json
open System.Text.Json.Serialization
open Microsoft.AspNetCore.Http
open Oxpecker
open FnMCP.Nexus.Types
open FnMCP.Nexus.McpServer

// WebSocket transport for MCP (bidirectional streaming)
// Using Oxpecker 1.0 API: EndpointHandler = HttpContext -> Task

// JSON serialization options
let jsonOptions = JsonSerializerOptions()
jsonOptions.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase
jsonOptions.WriteIndented <- false
jsonOptions.DefaultIgnoreCondition <- JsonIgnoreCondition.WhenWritingNull

// Log to stderr
let log message =
    Console.Error.WriteLine($"[WebSocketTransport] {message}")

// Handle WebSocket connection
let handleWebSocket (server: McpServer) : EndpointHandler =
    fun (ctx: HttpContext) ->
        task {
            if ctx.WebSockets.IsWebSocketRequest then
                log "WebSocket connection request received"

                use! webSocket = ctx.WebSockets.AcceptWebSocketAsync()
                log "WebSocket connection established"

                let buffer = Array.zeroCreate<byte> (1024 * 4)
                let mutable receiveResult = Unchecked.defaultof<WebSocketReceiveResult>

                // Message receiving loop
                while not receiveResult.CloseStatus.HasValue do
                    try
                        let! result = webSocket.ReceiveAsync(ArraySegment<byte>(buffer), ctx.RequestAborted)
                        receiveResult <- result

                        if result.MessageType = WebSocketMessageType.Close then
                            log "WebSocket close requested"
                            do! webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ctx.RequestAborted)
                        elif result.MessageType = WebSocketMessageType.Text then
                            // Parse message
                            let message = Encoding.UTF8.GetString(buffer, 0, result.Count)
                            log $"Received: {message.Substring(0, min 100 message.Length)}..."

                            try
                                // Parse JSON-RPC request
                                let jsonRpcRequest = JsonSerializer.Deserialize<JsonRpcRequest>(message, jsonOptions)

                                // Process request
                                let! mcpResult = server.HandleRequest(jsonRpcRequest) |> Async.StartAsTask

                                // Send response
                                let response = server.CreateResponse(jsonRpcRequest.Id, mcpResult)
                                let responseJson = JsonSerializer.Serialize(response, jsonOptions)
                                let responseBytes = Encoding.UTF8.GetBytes(responseJson)

                                do! webSocket.SendAsync(
                                    ArraySegment<byte>(responseBytes),
                                    WebSocketMessageType.Text,
                                    true,
                                    ctx.RequestAborted)

                                log $"Sent response for method: {jsonRpcRequest.Method}"

                            with ex ->
                                log $"Error processing message: {ex.Message}"

                                // Send error response
                                let errorResponse = {
                                    Jsonrpc = "2.0"
                                    Id = None
                                    Result = None
                                    Error = Some (box {
                                        Code = ErrorCodes.InternalError
                                        Message = $"Error: {ex.Message}"
                                        Data = None
                                    })
                                }
                                let errorJson = JsonSerializer.Serialize(errorResponse, jsonOptions)
                                let errorBytes = Encoding.UTF8.GetBytes(errorJson)

                                do! webSocket.SendAsync(
                                    ArraySegment<byte>(errorBytes),
                                    WebSocketMessageType.Text,
                                    true,
                                    ctx.RequestAborted)

                    with
                    | :? System.OperationCanceledException ->
                        log "WebSocket operation cancelled"
                        receiveResult <- WebSocketReceiveResult(0, WebSocketMessageType.Close, true, WebSocketCloseStatus.NormalClosure, "Cancelled")
                    | ex ->
                        log $"WebSocket error: {ex.Message}"
                        receiveResult <- WebSocketReceiveResult(0, WebSocketMessageType.Close, true, WebSocketCloseStatus.InternalServerError, ex.Message)

                log "WebSocket connection closed"
            else
                log "Non-WebSocket request received at WebSocket endpoint"
                ctx.Response.StatusCode <- 400
                do! ctx |> text "This endpoint requires WebSocket connection"
        }
