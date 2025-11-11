module FnMCP.IvanTheGeek.Events

open System
open System.IO
open System.Text

// Event-sourced Nexus: Phase 1 domain and helpers
// F# conventions: discriminated unions and records

type EventType =
    | TechnicalDecision
    | DesignNote
    | ResearchFinding

with
    member this.AsString =
        match this with
        | TechnicalDecision -> "TechnicalDecision"
        | DesignNote -> "DesignNote"
        | ResearchFinding -> "ResearchFinding"

    static member Parse(str: string) =
        let s = if isNull str then "" else str
        match s.Trim().ToLowerInvariant() with
        | "technicaldecision" | "technical_decision" | "decision" -> TechnicalDecision
        | "designnote" | "design_note" | "note" -> DesignNote
        | "researchfinding" | "research_finding" | "finding" -> ResearchFinding
        | other -> failwith ($"Unknown event type: {other}")

and TechnicalDecisionDetails = {
    Status: string option // e.g., proposed | decided | superseded
    Decision: string option
    Context: string option
    Consequences: string option
}

and EventMeta = {
    Id: Guid
    Type: EventType
    Title: string
    Summary: string option
    OccurredAt: DateTime // local time preferred, will be formatted in filename
    Tags: string list
    Author: string option
    Links: string list
    Technical: TechnicalDecisionDetails option
}

module private Impl =
    let sanitizeFilePart (s: string) =
        // Keep letters, numbers, hyphens, and underscores; convert spaces to underscores
        let cleaned = s.Trim().Replace(" ", "_")
        let sb = StringBuilder(cleaned.Length)
        for ch in cleaned do
            if Char.IsLetterOrDigit(ch) || ch = '-' || ch = '_' then sb.Append(ch) |> ignore
            elif ch = '.' then sb.Append('.') |> ignore // allow dot in case
            else ()
        if sb.Length = 0 then "untitled" else sb.ToString()

    let monthFolder (dt: DateTime) = dt.ToString("yyyy-MM")

    let fileTimestamp (dt: DateTime) =
        // Format: YYYY-MM-DDTHH-MM-SS (no colons)
        dt.ToString("yyyy-MM-dd'T'HH-mm-ss")

    let yamlEscape (s: string) =
        if isNull s then "" else s.Replace("\\", "\\\\").Replace("\"", "\\\"")

    let toYaml (meta: EventMeta) (body: string) =
        let b = StringBuilder()
        let append (s: string) = b.AppendLine(s) |> ignore
        append "---"
        append ($"id: {meta.Id}")
        append ($"type: {meta.Type.AsString}")
        append ($"title: \"{yamlEscape meta.Title}\"")
        meta.Summary |> Option.iter (fun s -> append ($"summary: \"{yamlEscape s}\""))
        // ISO 8601 for readability, include offset if available
        let occurred = meta.OccurredAt.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK")
        append ($"occurred_at: {occurred}")
        if meta.Tags |> List.isEmpty |> not then
            append "tags:"
            for t in meta.Tags do append ($"  - {t}")
        meta.Author |> Option.iter (fun a -> append ($"author: {a}"))
        if meta.Links |> List.isEmpty |> not then
            append "links:"
            for l in meta.Links do append ($"  - {l}")
        match meta.Type, meta.Technical with
        | TechnicalDecision, Some td ->
            append "technical_decision:"
            td.Status |> Option.iter (fun v -> append ($"  status: {v}"))
            td.Decision |> Option.iter (fun v -> append ($"  decision: \"{yamlEscape v}\""))
            td.Context |> Option.iter (fun v -> append ($"  context: \"{yamlEscape v}\""))
            td.Consequences |> Option.iter (fun v -> append ($"  consequences: \"{yamlEscape v}\""))
        | _ -> ()
        append "---"
        append ""
        if String.IsNullOrWhiteSpace(body) then () else append body
        b.ToString()

    let parseFrontMatter (content: string) =
        // Very lightweight parser to extract a few keys
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
                    // ignore list values for projection
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

open Impl

module EventFiles =
    let ensureDirectory (path: string) =
        if not (Directory.Exists(path)) then Directory.CreateDirectory(path) |> ignore

    let eventDirectory (contextLibraryPath: string) (dt: DateTime) =
        Path.Combine(contextLibraryPath, "nexus", "events", "domain", "active", monthFolder dt)

    let buildFilename (etype: EventType) (title: string) (dt: DateTime) =
        let ts = fileTimestamp dt
        let et = etype.AsString
        let name = sanitizeFilePart title
        $"{ts}_{et}_{name}.md"

    let writeEventFile (contextLibraryPath: string) (meta: EventMeta) (body: string) =
        let dir = eventDirectory contextLibraryPath meta.OccurredAt
        ensureDirectory dir
        let filename = buildFilename meta.Type meta.Title meta.OccurredAt
        let fullPath = Path.Combine(dir, filename)
        let yaml = toYaml meta body
        File.WriteAllText(fullPath, yaml, Encoding.UTF8)
        fullPath

    type TimelineItem = {
        Path: string
        Title: string
        Type: string
        OccurredAt: DateTime
    }

    let tryParseEvent (path: string) : TimelineItem option =
        try
            let content = File.ReadAllText(path)
            let (fm, _) = parseFrontMatter content
            if fm.IsEmpty then None else
            let title = fm.TryFind("title") |> Option.defaultValue (Path.GetFileNameWithoutExtension(path))
            let etype = fm.TryFind("type") |> Option.defaultValue "Unknown"
            let occurredStr = fm.TryFind("occurred_at") |> Option.defaultValue ""
            let ok, dt = DateTime.TryParse(occurredStr)
            let dt' = if ok then dt else File.GetCreationTime(path)
            Some { Path = path; Title = title; Type = etype; OccurredAt = dt' }
        with _ -> None

    let readTimeline (contextLibraryPath: string) =
        let baseDir = Path.Combine(contextLibraryPath, "nexus", "events", "domain", "active")
        if not (Directory.Exists(baseDir)) then [] else
        Directory.GetFiles(baseDir, "*.md", SearchOption.AllDirectories)
        |> Array.choose (fun p -> tryParseEvent p |> Option.map id)
        |> Array.sortBy (fun i -> i.OccurredAt)
        |> Array.toList
