#!/usr/bin/env dotnet fsi

#r "src/FnMCP.IvanTheGeek/bin/Debug/net9.0/FnMCP.IvanTheGeek.dll"

open System
open FnMCP.IvanTheGeek.Projections.Timeline

// Test evolution.md generation
let basePath = "context-library"

printfn "Generating evolution.md projection"
printfn "===================================\n"

try
    let filePath = writeEvolutionFile basePath
    printfn "✅ Evolution file written to: %s\n" filePath

    // Read and display it
    let content = System.IO.File.ReadAllText(filePath)
    printfn "%s" content

    exit 0
with ex ->
    printfn "❌ Error: %s" ex.Message
    exit 1
