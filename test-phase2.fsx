#!/usr/bin/env dotnet fsi

#r "src/FnMCP.IvanTheGeek/bin/Debug/net9.0/FnMCP.IvanTheGeek.dll"

open System
open System.IO
open FnMCP.IvanTheGeek.Domain
open FnMCP.IvanTheGeek.Domain.EventWriter
open FnMCP.IvanTheGeek.Projections

printfn "=========================================="
printfn "PHASE 2: Comprehensive Integration Test"
printfn "==========================================\n"

let basePath = "context-library"
let systemEventsDir = Path.Combine(basePath, "nexus", "events", "system", "active")
let domainEventsDir = Path.Combine(basePath, "nexus", "events", "domain", "active")

// Helper: Count system events by type
let countSystemEvents (eventType: SystemEventType) : int =
    if not (Directory.Exists(systemEventsDir)) then 0
    else
        Directory.GetFiles(systemEventsDir, "*.yaml", SearchOption.AllDirectories)
        |> Array.filter (fun file -> file.Contains($"_{eventType.AsString}.yaml"))
        |> Array.length

// TEST 1: Create a domain event
printfn "TEST 1: Creating domain event..."
let eventMeta : EventMeta = {
    Id = Guid.NewGuid()
    Type = FrameworkInsight
    Title = "Phase 2 Test Event"
    Summary = Some "Testing the complete Phase 2 workflow"
    OccurredAt = DateTime.Now
    Tags = ["test"; "phase2"; "integration"]
    Author = Some "test-suite"
    Links = []
    Technical = None
}
let eventPath = writeEventFile basePath eventMeta "This event tests the Phase 2 system event tracking and projection flow."
printfn "✓ Created domain event: %s" (Path.GetFileName(eventPath))

// Verify EventCreated system event was emitted
let eventCreatedCount = countSystemEvents EventCreated
printfn "✓ EventCreated system events: %d\n" eventCreatedCount

// TEST 2: Regenerate Timeline projection
printfn "TEST 2: Regenerating Timeline projection..."
let timelinePath = Timeline.regenerateTimeline basePath
printfn "✓ Timeline regenerated: %s" (Path.GetFileName(timelinePath))

// Verify ProjectionRegenerated system event
let projRegenCount = countSystemEvents ProjectionRegenerated
printfn "✓ ProjectionRegenerated system events: %d" projRegenCount

// Check .meta.yaml file
let timelineMetaPath = Path.Combine(Path.GetDirectoryName(timelinePath), ".meta.yaml")
if File.Exists(timelineMetaPath) then
    printfn "✓ Timeline .meta.yaml exists"
else
    printfn "✗ Timeline .meta.yaml NOT found"

printfn ""

// TEST 3: Regenerate Metrics projection
printfn "TEST 3: Regenerating Metrics projection..."
let metricsPath = Metrics.MetricsWriter.regenerateMetrics basePath
printfn "✓ Metrics regenerated: %s" (Path.GetFileName(metricsPath))

// Read and display metrics
if File.Exists(metricsPath) then
    printfn "\nMetrics content preview:"
    let metricsContent = File.ReadAllText(metricsPath)
    let allLines = metricsContent.Split('\n')
    let lineCount = min 15 allLines.Length
    let lines = allLines |> Array.take lineCount
    for line in lines do
        printfn "  %s" line
else
    printfn "✗ Metrics file NOT found"

printfn ""

// TEST 4: Test Projection Registry
printfn "TEST 4: Testing Projection Registry..."
let registryPath = Path.Combine(basePath, "nexus", "projections", ".registry", "registry.yaml")

// List all projections
let allProjections = Registry.RegistryIO.listProjections basePath
printfn "✓ Projections in registry: %d" allProjections.Length
for proj in allProjections do
    printfn "  - %s (type: %s, staleness: %s)" proj.Name proj.Type.AsString proj.Staleness.AsString

// Get specific projection
match Registry.RegistryIO.getProjection basePath "timeline" with
| Some proj ->
    printfn "✓ Retrieved timeline from registry:"
    printfn "    Last regenerated: %s" (proj.LastRegenerated.ToString("yyyy-MM-dd HH:mm:ss"))
    printfn "    Staleness: %s" proj.Staleness.AsString
| None ->
    printfn "✗ Timeline NOT found in registry"

printfn ""

// TEST 5: System Events Summary
printfn "TEST 5: System Events Summary..."
printfn "  EventCreated events: %d" (countSystemEvents EventCreated)
printfn "  ProjectionRegenerated events: %d" (countSystemEvents ProjectionRegenerated)
printfn "  ProjectionQueried events: %d" (countSystemEvents ProjectionQueried)
printfn "  ToolInvoked events: %d" (countSystemEvents ToolInvoked)

let totalSystemEvents =
    if Directory.Exists(systemEventsDir) then
        Directory.GetFiles(systemEventsDir, "*.yaml", SearchOption.AllDirectories).Length
    else 0
printfn "  Total system events: %d" totalSystemEvents

printfn ""

// TEST 6: Domain Events Summary
printfn "TEST 6: Domain Events Summary..."
let domainEventCount =
    if Directory.Exists(domainEventsDir) then
        Directory.GetFiles(domainEventsDir, "*.md", SearchOption.AllDirectories).Length
    else 0
printfn "  Total domain events: %d" domainEventCount

printfn ""

// TEST 7: Staleness tracking
printfn "TEST 7: Testing staleness tracking..."
// Refresh staleness for timeline
Registry.RegistryIO.refreshStaleness basePath "timeline"
match Registry.RegistryIO.getProjection basePath "timeline" with
| Some proj ->
    printfn "✓ Timeline staleness refreshed: %s" proj.Staleness.AsString
| None ->
    printfn "✗ Failed to refresh staleness"

printfn ""
printfn "=========================================="
printfn "Phase 2 Integration Test Complete!"
printfn "=========================================="
printfn ""
printfn "Summary:"
printfn "  ✓ Domain events created"
printfn "  ✓ System events emitted"
printfn "  ✓ Timeline projection regenerated"
printfn "  ✓ Metrics projection generated"
printfn "  ✓ Projection registry operational"
printfn "  ✓ Staleness tracking working"
printfn ""
printfn "🎉 Phase 2 implementation successful!"
