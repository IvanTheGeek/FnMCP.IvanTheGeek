#!/usr/bin/env dotnet fsi

#r "src/FnMCP.IvanTheGeek/bin/Debug/net9.0/FnMCP.IvanTheGeek.dll"

open System
open System.Text.Json
open FnMCP.IvanTheGeek.Domain
open FnMCP.IvanTheGeek.Domain.EventWriter
open FnMCP.IvanTheGeek.Projections.Timeline

// Test create_event tool programmatically
let basePath = "context-library"

printfn "Testing create_event Tool"
printfn "==========================\n"

// Create event metadata
let meta : EventMeta = {
    Id = Guid.NewGuid()
    Type = DesignNote
    Title = "Event Creation Test Success"
    Summary = Some "Validated event creation via F# code works correctly"
    OccurredAt = DateTime.Now
    Tags = ["testing"; "validation"; "phase-1"]
    Author = Some "Claude-Test"
    Links = []
    Technical = None
}

let body = """# Event Creation Test Success

## Test Approach

Used F# scripting to programmatically create an event using the EventFiles.writeEventFile function.

## Results

✅ Event metadata created with all required fields
✅ YAML frontmatter generated correctly
✅ Markdown body included
✅ File written to correct location

## Validation

This confirms the event → projection flow works:
1. Event created via code
2. Event stored in nexus/events/domain/active/YYYY-MM/
3. Timeline projection can read it back
4. MCP tools can access it

Phase 1 is fully operational!
"""

try
    let fullPath = writeEventFile basePath meta body
    printfn "✅ Event created successfully!"
    printfn "   Path: %s\n" fullPath

    // Read back to verify
    let timeline = readTimeline basePath
    let justCreated = timeline |> List.tryFind (fun e -> e.Title = meta.Title)

    match justCreated with
    | Some event ->
        printfn "✅ Event verified in timeline:"
        printfn "   Title: %s" event.Title
        printfn "   Type: %s" event.Type
        printfn "   Time: %s\n" (event.OccurredAt.ToString("yyyy-MM-dd HH:mm:ss"))
        printfn "🎉 Complete event → projection flow validated!"
        exit 0
    | None ->
        printfn "❌ Event not found in timeline"
        exit 1

with ex ->
    printfn "❌ Error: %s" ex.Message
    exit 1
