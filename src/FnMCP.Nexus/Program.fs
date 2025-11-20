module FnMCP.Nexus.Program

open System
open System.IO
open System.Text
open System.Text.Json
open System.Text.Json.Serialization
open FnMCP.Nexus
open FnMCP.Nexus.Types
open FnMCP.Nexus.ContentProvider
open FnMCP.Nexus.FileSystemProvider
open FnMCP.Nexus.McpServer
open FnMCP.Nexus.Transport.StdioTransport
open FnMCP.Nexus.Http.HttpServer

// Log to stderr (stdout is for JSON-RPC communication in CLI mode)
let log message =
    Console.Error.WriteLine($"[FnMCP.Nexus] {message}")

// CLI mode argument parser
let parseCliArgs (args: string array) =
    let rec parseArgs (remaining: string list) (acc: Map<string, obj>) =
        match remaining with
        | [] -> acc
        | flag :: value :: rest when flag.StartsWith("--") ->
            let key = flag.Substring(2).Replace("-", "_")  // Remove "--" and normalize hyphens to underscores
            // Try to parse as JSON array/object, otherwise treat as string
            let parsedValue =
                if value.StartsWith("[") || value.StartsWith("{") then
                    try
                        let jsonElement = JsonSerializer.Deserialize<JsonElement>(value)
                        box jsonElement
                    with _ -> box value
                else
                    box value
            parseArgs rest (Map.add key parsedValue acc)
        | flag :: rest when flag.StartsWith("--") ->
            // Flag without value - treat as true
            let key = flag.Substring(2).Replace("-", "_")  // Normalize hyphens to underscores
            parseArgs rest (Map.add key (box true) acc)
        | unexpected :: rest ->
            // Non-flag argument - skip or could error
            parseArgs rest acc

    parseArgs (Array.toList args) Map.empty

// CLI mode execution
let runCliMode (contextLibraryPath: string) (command: string) (args: string array) =
    try
        // Parse CLI arguments into map
        let argsMap = parseCliArgs args

        // Convert map to JsonElement for tool execution
        let jsonString = JsonSerializer.Serialize(argsMap)
        let jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonString)

        // Execute tool
        match ToolRegistry.executeTool contextLibraryPath command (Some (box jsonElement)) with
        | Ok content ->
            // Print tool output - serialize to JSON then extract text
            for item in content do
                let itemJson = JsonSerializer.Serialize(item)
                let itemElement = JsonSerializer.Deserialize<JsonElement>(itemJson)
                let mutable textProp = Unchecked.defaultof<JsonElement>
                if itemElement.TryGetProperty("text", &textProp) then
                    Console.WriteLine(textProp.GetString())
            0
        | Error err ->
            Console.Error.WriteLine($"Error: {err}")
            1
    with ex ->
        Console.Error.WriteLine($"CLI Error: {ex.Message}")
        Console.Error.WriteLine($"Stack trace: {ex.StackTrace}")
        1

[<EntryPoint>]
let main argv =
    try
        // Load environment variables from .env file (if it exists)
        // This must be done before any configuration is read
        try
            // Try to load .env from current directory first
            let currentDirEnv = Path.Combine(Directory.GetCurrentDirectory(), ".env")
            // Also try binary directory
            let binaryDirEnv = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), ".env")

            if File.Exists(currentDirEnv) then
                DotNetEnv.Env.Load(currentDirEnv) |> ignore
                log $"Loaded environment variables from .env file: {currentDirEnv}"
            elif File.Exists(binaryDirEnv) then
                DotNetEnv.Env.Load(binaryDirEnv) |> ignore
                log $"Loaded environment variables from .env file: {binaryDirEnv}"
            else
                log "No .env file found - using environment variables and defaults"
        with
        | ex ->
            log $"Warning: Failed to load .env file: {ex.Message}"

        // Detect mode: CLI (2+ args) vs MCP (0-1 args)
        match argv with
        | args when args.Length >= 2 ->
            // CLI Mode
            let knownCommands = Set.ofList [
                "create-event"; "create_event"
                "timeline-projection"; "timeline_projection"
                "capture-idea"; "capture_idea"
                "enhance-nexus"; "enhance_nexus"
                "record-learning"; "record_learning"
                "lookup-pattern"; "lookup_pattern"
                "lookup-error-solution"; "lookup_error_solution"
                "update-documentation"; "update_documentation"
            ]

            // Determine if first arg is context path or command
            let contextPath, command, remainingArgs =
                if knownCommands.Contains(args.[0]) || knownCommands.Contains(args.[0].Replace("-", "_")) then
                    // First arg is command, use default context path
                    let projectRoot = Path.GetDirectoryName(AppContext.BaseDirectory)
                    let defaultPath = Path.Combine(projectRoot, "context-library")
                    let cmd = args.[0].Replace("-", "_")  // Normalize to underscore
                    defaultPath, cmd, args.[1..]
                else
                    // First arg is context path, second is command
                    let cmd = if args.Length > 1 then args.[1].Replace("-", "_") else ""
                    args.[0], cmd, if args.Length > 2 then args.[2..] else [||]

            if String.IsNullOrEmpty(command) then
                Console.Error.WriteLine("Error: No command specified")
                Console.Error.WriteLine("Usage: nexus [context-path] <command> [--arg value ...]")
                Console.Error.WriteLine("Commands: create-event, timeline-projection, enhance-nexus, record-learning, lookup-pattern, lookup-error-solution, update-documentation")
                1
            else
                runCliMode contextPath command remainingArgs

        | _ ->
            // MCP Server Mode (stdio or HTTP)
            let contextLibraryPath =
                match argv with
                | [| path |] ->
                    // Command-line argument takes precedence
                    path
                | _ ->
                    // Check environment variable from .env file
                    let envPath = Environment.GetEnvironmentVariable("CONTEXT_LIBRARY_PATH")
                    if not (String.IsNullOrWhiteSpace(envPath)) then
                        envPath
                    else
                        // Default to context-library relative to binary location
                        let projectRoot = Path.GetDirectoryName(AppContext.BaseDirectory)
                        Path.Combine(projectRoot, "context-library")

            log "FnMCP.Nexus MCP Server starting..."
            log $"Protocol version: 2025-06-18"
            log $"Context library path: {contextLibraryPath}"

            if not (Directory.Exists(contextLibraryPath)) then
                log $"Warning: Context library directory does not exist: {contextLibraryPath}"
                log "Server will start but no resources will be available until the directory is created."

            // Create provider and server
            let provider = FileSystemProvider(contextLibraryPath) :> IContentProvider
            let server = McpServer(provider, contextLibraryPath)

            // Detect transport mode: use NEXUS_TRANSPORT env var or fall back to stdin detection
            let transportMode =
                Environment.GetEnvironmentVariable("NEXUS_TRANSPORT")
                |> Option.ofObj
                |> Option.map (fun s -> s.ToLowerInvariant())

            match transportMode with
            | Some "http" ->
                // Explicit HTTP mode
                let mcpPort =
                    Environment.GetEnvironmentVariable("NEXUS_MCP_PORT")
                    |> Option.ofObj
                    |> Option.bind (fun s ->
                        match Int32.TryParse(s) with
                        | true, port -> Some port
                        | false, _ -> None)
                    |> Option.defaultValue 18080

                log $"Transport: HTTP (SSE/WebSocket) [explicit]"
                log $"Port: {mcpPort}"
                log "Starting HTTP server..."

                startHttpServer mcpPort contextLibraryPath server
                |> Async.RunSynchronously

                0

            | _ ->
                // Default: stdio mode (for SSH, Claude Desktop, or when stdin is redirected)
                log "Transport: stdio (SSH/Desktop)"
                log "Ready to receive JSON-RPC requests on stdin."
                log "Logging to stderr. JSON-RPC responses on stdout."
                runStdioTransport server
    with ex ->
        Console.Error.WriteLine($"Fatal error: {ex.Message}")
        Console.Error.WriteLine($"Stack trace: {ex.StackTrace}")
        1