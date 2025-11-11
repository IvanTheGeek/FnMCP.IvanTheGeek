module FnMCP.IvanTheGeek.Tools

open System
open System.IO
open System.Text
open System.Text.Json
open FnMCP.IvanTheGeek.Types
open FnMCP.IvanTheGeek.Events
open FnMCP.IvanTheGeek.Events.EventFiles

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
        };
        {
            Name = "create_event"
            Description = Some "Create a Nexus event file with YAML frontmatter and markdown body."
            InputSchema = box {|
                ``type`` = "object"
                properties = {|
                    ``type`` = {|
                        ``type`` = "string"
                        description = "Event type: TechnicalDecision | DesignNote | ResearchFinding"
                    |}
                    title = {|
                        ``type`` = "string"
                        description = "Short event title used in filename and frontmatter"
                    |}
                    summary = {|
                        ``type`` = "string"
                        description = "One-line summary"
                    |}
                    body = {|
                        ``type`` = "string"
                        description = "Markdown narrative body"
                    |}
                    tags = {|
                        ``type`` = "array"
                        items = {| ``type`` = "string" |}
                        description = "List of tag strings"
                    |}
                    author = {|
                        ``type`` = "string"
                        description = "Author name or id"
                    |}
                    links = {|
                        ``type`` = "array"
                        items = {| ``type`` = "string" |}
                        description = "Related links"
                    |}
                    occurredAt = {|
                        ``type`` = "string"
                        description = "ISO 8601 timestamp; defaults to now"
                    |}
                    decision_status = {|
                        ``type`` = "string"
                        description = "TechnicalDecision: status (proposed/decided/superseded)"
                    |}
                    decision = {|
                        ``type`` = "string"
                        description = "TechnicalDecision: decision statement"
                    |}
                    context = {|
                        ``type`` = "string"
                        description = "TechnicalDecision: context"
                    |}
                    consequences = {|
                        ``type`` = "string"
                        description = "TechnicalDecision: consequences"
                    |}
                |}
                required = [| "type"; "title" |]
            |}
        };
        {
            Name = "timeline_projection"
            Description = Some "Read Nexus domain events and return a simple time-ordered timeline."
            InputSchema = box {|
                ``type`` = "object"
                properties = {||}
            |}
        }
    ]

// Helper JSON extraction
let private tryGetProperty (elem: JsonElement) (name: string) =
    let mutable prop = Unchecked.defaultof<JsonElement>
    if elem.TryGetProperty(name, &prop) then Some prop else None

let private getStringOpt (elem: JsonElement) (name: string) =
    match tryGetProperty elem name with
    | Some v when v.ValueKind = JsonValueKind.String -> Some (v.GetString())
    | _ -> None

let private getArrayStrings (elem: JsonElement) (name: string) =
    match tryGetProperty elem name with
    | Some v when v.ValueKind = JsonValueKind.Array ->
        v.EnumerateArray() |> Seq.choose (fun x -> if x.ValueKind = JsonValueKind.String then Some (x.GetString()) else None) |> Seq.toList
    | _ -> []

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

let handleCreateEvent (contextLibraryPath: string) (args: JsonElement) : Result<string, string> =
    try
        let etypeStr = args.GetProperty("type").GetString()
        let etype = EventType.Parse(etypeStr)
        let title = args.GetProperty("title").GetString()
        let summary = getStringOpt args "summary"
        let body = getStringOpt args "body" |> Option.defaultValue ""
        let tags = getArrayStrings args "tags"
        let author = getStringOpt args "author"
        let links = getArrayStrings args "links"
        let occurredAt =
            match getStringOpt args "occurredAt" with
            | Some s ->
                match DateTime.TryParse(s) with
                | true, dt -> dt
                | _ -> DateTime.Now
            | None -> DateTime.Now
        let techDetails =
            match etype with
            | TechnicalDecision ->
                let status = getStringOpt args "decision_status"
                let decision = getStringOpt args "decision"
                let context = getStringOpt args "context"
                let consequences = getStringOpt args "consequences"
                Some { Status = status; Decision = decision; Context = context; Consequences = consequences }
            | _ -> None
        let meta : EventMeta = {
            Id = Guid.NewGuid()
            Type = etype
            Title = title
            Summary = summary
            OccurredAt = occurredAt
            Tags = tags
            Author = author
            Links = links
            Technical = techDetails
        }
        let fullPath = writeEventFile contextLibraryPath meta body
        Ok (sprintf "Event created: %s" (Path.GetRelativePath(contextLibraryPath, fullPath)))
    with
    | ex -> Error ($"Failed to create event: {ex.Message}")

let handleTimelineProjection (contextLibraryPath: string) : Result<string, string> =
    try
        let items = readTimeline contextLibraryPath
        if List.isEmpty items then Ok "No events found."
        else
            let sb = StringBuilder()
            sb.AppendLine("Nexus Timeline:") |> ignore
            for it in items do
                sb.AppendLine(sprintf "- %s | %s | %s" (it.OccurredAt.ToString("yyyy-MM-dd HH:mm:ss")) it.Type it.Title) |> ignore
            Ok (sb.ToString())
    with ex -> Error ($"Failed to read timeline: {ex.Message}")

// Main tool execution router
let executeTool (contextLibraryPath: string) (name: string) (arguments: obj option) : Result<obj list, string> =
    match arguments with
    | None -> Error "Missing tool arguments"
    | Some args ->
        let jsonElement = args :?> JsonElement
        
        match name with
        | "update_documentation" ->
            match handleUpdateDocumentation contextLibraryPath jsonElement with
            | Ok message -> Ok [ box {| ``type`` = "text"; text = message |} ]
            | Error err -> Error err
        | "create_event" ->
            match handleCreateEvent contextLibraryPath jsonElement with
            | Ok message -> Ok [ box {| ``type`` = "text"; text = message |} ]
            | Error err -> Error err
        | "timeline_projection" ->
            match handleTimelineProjection contextLibraryPath with
            | Ok txt -> Ok [ box {| ``type`` = "text"; text = txt |} ]
            | Error err -> Error err
        | _ -> Error $"Unknown tool: {name}"
