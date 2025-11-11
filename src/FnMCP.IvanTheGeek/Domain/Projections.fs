module FnMCP.IvanTheGeek.Domain.Projections

open System
open System.IO
open FnMCP.IvanTheGeek.Domain

// Projection interfaces and common utilities

// Helper to parse YAML frontmatter
module FrontMatterParser =
    let parseFrontMatter (content: string) =
        // Lightweight parser to extract key-value pairs from YAML frontmatter
        // Returns (map, body)
        let lines = content.Replace("\r\n", "\n").Split('\n')
        if lines.Length = 0 || lines[0].Trim() <> "---" then Map.empty, content
        else
            let mutable i = 1
            let mutable map = Map.empty<string, string>
            let mutable inFront = true
            while i < lines.Length && inFront do
                let line = lines[i]
                if line.Trim() = "---" then inFront <- false
                elif line.StartsWith("  - ") then
                    // ignore list values for simple projection
                    ()
                else
                    let idx = line.IndexOf(':')
                    if idx > 0 then
                        let key = line.Substring(0, idx).Trim()
                        let value = line.Substring(idx + 1).Trim().Trim('"')
                        map <- map.Add(key, value)
                i <- i + 1
            // body
            let body = String.Join("\n", lines[i..])
            map, body

// Common projection utilities
let tryParseEvent (path: string) : TimelineItem option =
    try
        let content = File.ReadAllText(path)
        let (fm, _) = FrontMatterParser.parseFrontMatter content
        if fm.IsEmpty then None else
        let title = fm.TryFind("title") |> Option.defaultValue (Path.GetFileNameWithoutExtension(path))
        let etype = fm.TryFind("type") |> Option.defaultValue "Unknown"
        let occurredStr = fm.TryFind("occurred_at") |> Option.defaultValue ""
        let ok, dt = DateTime.TryParse(occurredStr)
        let dt' = if ok then dt else File.GetCreationTime(path)
        Some { Path = path; Title = title; Type = etype; OccurredAt = dt' }
    with _ -> None
