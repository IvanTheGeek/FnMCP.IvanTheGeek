module FnMCP.IvanTheGeek.Tools

open System
open System.IO
open System.Text.Json
open FnMCP.IvanTheGeek.Types

// Tool definitions for the MCP server
let getTools (contextLibraryPath: string) : Tool list =
    [
        {
            Name = "update_documentation"
            Description = Some "Update or create markdown files in the context library. Supports full file rewrites or appending content."
            InputSchema = box {|
                ``type`` = "object"
                properties = {|
                    path = {|
                        ``type`` = "string"
                        description = "Relative path within context-library (e.g., 'framework/overview.md' or 'apps/laundrylog/overview.md')"
                    |}
                    content = {|
                        ``type`` = "string"
                        description = "The markdown content to write"
                    |}
                    mode = {|
                        ``type`` = "string"
                        description = "Update mode: 'overwrite' (replace entire file) or 'append' (add to end)"
                        ``enum`` = [| "overwrite"; "append" |]
                        ``default`` = "overwrite"
                    |}
                |}
                required = [| "path"; "content" |]
            |}
        }
    ]

// Tool execution handlers
let handleUpdateDocumentation (contextLibraryPath: string) (args: JsonElement) : Result<string, string> =
    try
        // Extract parameters
        let path = args.GetProperty("path").GetString()
        let content = args.GetProperty("content").GetString()
        let mutable modeProp = Unchecked.defaultof<JsonElement>
        let mode = 
            if args.TryGetProperty("mode", &modeProp) then
                modeProp.GetString()
            else
                "overwrite"

        // Validate path safety - must be within context library
        let fullPath = Path.Combine(contextLibraryPath, path)
        let normalizedPath = Path.GetFullPath(fullPath)
        let normalizedBase = Path.GetFullPath(contextLibraryPath)
        
        if not (normalizedPath.StartsWith(normalizedBase)) then
            Error "Invalid path: must be within context-library directory"
        else
            // Ensure directory exists
            let directory = Path.GetDirectoryName(normalizedPath)
            if not (Directory.Exists(directory)) then
                Directory.CreateDirectory(directory) |> ignore

            // Write content based on mode
            match mode with
            | "overwrite" ->
                File.WriteAllText(normalizedPath, content)
                Ok $"Successfully updated {path} (overwrite)"
            | "append" ->
                File.AppendAllText(normalizedPath, "\n" + content)
                Ok $"Successfully appended to {path}"
            | _ ->
                Error $"Invalid mode: {mode}. Use 'overwrite' or 'append'"

    with
    | ex -> Error $"Failed to update documentation: {ex.Message}"

// Main tool execution router
let executeTool (contextLibraryPath: string) (name: string) (arguments: obj option) : Result<obj list, string> =
    match arguments with
    | None -> Error "Missing tool arguments"
    | Some args ->
        let jsonElement = args :?> JsonElement
        
        match name with
        | "update_documentation" ->
            match handleUpdateDocumentation contextLibraryPath jsonElement with
            | Ok message ->
                Ok [ box {| ``type`` = "text"; text = message |} ]
            | Error err ->
                Error err
        | _ ->
            Error $"Unknown tool: {name}"
