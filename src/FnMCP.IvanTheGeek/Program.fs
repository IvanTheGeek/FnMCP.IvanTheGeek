module FnMCP.IvanTheGeek.Program

open System
open System.IO
open System.Text.Json
open FnMCP.IvanTheGeek.Types
open FnMCP.IvanTheGeek.ContentProvider
open FnMCP.IvanTheGeek.FileSystemProvider
open FnMCP.IvanTheGeek.McpServer

// JSON serialization options
let jsonOptions = JsonSerializerOptions()
jsonOptions.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase
jsonOptions.WriteIndented <- false

// Log to stderr (stdout is for JSON-RPC communication)
let log message =
    Console.Error.WriteLine($"[FnMCP.IvanTheGeek] {message}")

// Process a single JSON-RPC request
let processRequest (server: McpServer) (requestLine: string) = async {
    try
        // Parse JSON-RPC request
        let jsonRpcRequest = JsonSerializer.Deserialize<JsonRpcRequest>(requestLine, jsonOptions)
        
        log $"Received request: {jsonRpcRequest.Method}"
        
        // Handle the request
        let! result = server.HandleRequest(jsonRpcRequest)
        
        // Create response
        let response = server.CreateResponse(jsonRpcRequest.Id, result)
        
        // Serialize and return
        let responseJson = JsonSerializer.Serialize(response, jsonOptions)
        return responseJson
        
    with
    | ex ->
        log $"Error processing request: {ex.Message}"
        // Return error response
        let errorResponse = {
            Jsonrpc = "2.0"
            Id = None
            Result = None
            Error = Some (box {
                Code = ErrorCodes.ParseError
                Message = $"Parse error: {ex.Message}"
                Data = None
            })
        }
        return JsonSerializer.Serialize(errorResponse, jsonOptions)
}

[<EntryPoint>]
let main argv =
    try
        // Get context library path from config or argument
        let contextLibraryPath = 
            match argv with
            | [| path |] -> path
            | _ -> 
                // Default to context-library in project root
                let projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(__SOURCE_DIRECTORY__))
                Path.Combine(projectRoot, "context-library")

        log "FnMCP.IvanTheGeek MCP Server starting..."
        log $"Protocol version: 2025-06-18"
        log $"Context library path: {contextLibraryPath}"

        // Check if context library exists
        if not (Directory.Exists(contextLibraryPath)) then
            log $"Warning: Context library directory does not exist: {contextLibraryPath}"
            log "Server will start but no resources will be available until the directory is created."

        // Create provider and server
        let provider = FileSystemProvider(contextLibraryPath) :> IContentProvider
        let server = McpServer(provider)

        log "Server initialized. Ready to receive JSON-RPC requests on stdin."
        log "Logging to stderr. JSON-RPC responses on stdout."

        // Main message loop - read from stdin, write to stdout
        let rec messageLoop () =
            let line = Console.ReadLine()
            if line <> null then
                // Process request
                let responseJson = processRequest server line |> Async.RunSynchronously
                
                // Write response to stdout
                Console.WriteLine(responseJson)
                Console.Out.Flush()
                
                // Continue loop
                messageLoop ()
            else
                log "EOF received, shutting down."

        // Start the message loop
        messageLoop ()

        log "Server shutting down."
        0  // Exit code

    with
    | ex ->
        log $"Fatal error: {ex.Message}"
        log $"Stack trace: {ex.StackTrace}"
        1  // Error exit code