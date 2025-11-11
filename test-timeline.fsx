#!/usr/bin/env dotnet fsi

#r "src/FnMCP.IvanTheGeek/bin/Debug/net9.0/FnMCP.IvanTheGeek.dll"

open System
open FnMCP.IvanTheGeek.Projections.Timeline

// Test timeline reading
let basePath = "context-library"

printfn "Testing Nexus Timeline Projection"
printfn "==================================\n"

let timeline = readTimeline basePath

if List.isEmpty timeline then
    printfn "❌ No events found!"
    exit 1
else
    printfn "✅ Found %d events:\n" timeline.Length

    for item in timeline do
        printfn "📅 %s" (item.OccurredAt.ToString("yyyy-MM-dd HH:mm:ss"))
        printfn "   Type: %s" item.Type
        printfn "   Title: %s" item.Title
        printfn "   File: %s\n" (System.IO.Path.GetFileName(item.Path))

    printfn "✅ Timeline projection working!"
    exit 0
