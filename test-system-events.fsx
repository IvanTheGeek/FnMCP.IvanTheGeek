#!/usr/bin/env dotnet fsi

#r "src/FnMCP.IvanTheGeek/bin/Debug/net9.0/FnMCP.IvanTheGeek.dll"

open System
open FnMCP.IvanTheGeek.Domain
open FnMCP.IvanTheGeek.Domain.EventWriter

printfn "Testing System Event Creation"
printfn "==============================\n"

let basePath = "context-library"

// Test 1: EventCreated
printfn "Test 1: Creating EventCreated system event..."
let eventCreatedMeta = {
    Id = Guid.NewGuid()
    Type = EventCreated
    OccurredAt = DateTime.Now
    EventId = Some (Guid.NewGuid())
    EventType = Some "TechnicalDecision"
    ProjectionType = None
    Duration = None
    EventCount = None
    Staleness = None
    ToolName = None
    Success = None
}

let path1 = writeSystemEvent basePath eventCreatedMeta
printfn "✅ Created: %s\n" path1

// Test 2: ProjectionRegenerated
printfn "Test 2: Creating ProjectionRegenerated system event..."
let projRegenMeta = {
    Id = Guid.NewGuid()
    Type = ProjectionRegenerated
    OccurredAt = DateTime.Now
    EventId = None
    EventType = None
    ProjectionType = Some Timeline
    Duration = Some (TimeSpan.FromMilliseconds(523.0))
    EventCount = Some 15
    Staleness = None
    ToolName = None
    Success = None
}

let path2 = writeSystemEvent basePath projRegenMeta
printfn "✅ Created: %s\n" path2

// Test 3: ToolInvoked
printfn "Test 3: Creating ToolInvoked system event..."
let toolInvokedMeta = {
    Id = Guid.NewGuid()
    Type = ToolInvoked
    OccurredAt = DateTime.Now
    EventId = None
    EventType = None
    ProjectionType = None
    Duration = None
    EventCount = None
    Staleness = None
    ToolName = Some "create_event"
    Success = Some true
}

let path3 = writeSystemEvent basePath toolInvokedMeta
printfn "✅ Created: %s\n" path3

// Verify files exist
printfn "Verification:"
printfn "  File 1 exists: %b" (System.IO.File.Exists(path1))
printfn "  File 2 exists: %b" (System.IO.File.Exists(path2))
printfn "  File 3 exists: %b" (System.IO.File.Exists(path3))

// Read and display one
printfn "\nSample content (EventCreated):"
printfn "================================"
let content = System.IO.File.ReadAllText(path1)
printfn "%s" content

printfn "\n🎉 System events working!"
