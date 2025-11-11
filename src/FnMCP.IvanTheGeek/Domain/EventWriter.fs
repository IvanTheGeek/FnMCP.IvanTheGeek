module FnMCP.IvanTheGeek.Domain.EventWriter

open System
open System.IO
open System.Text
open FnMCP.IvanTheGeek.Domain

// Event file writing with YAML frontmatter and markdown body

module private Helpers =
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

open Helpers

let ensureDirectory (path: string) =
    if not (Directory.Exists(path)) then Directory.CreateDirectory(path) |> ignore

let eventDirectory (basePath: string) (dt: DateTime) =
    Path.Combine(basePath, "nexus", "events", "domain", "active", monthFolder dt)

let buildFilename (etype: EventType) (title: string) (dt: DateTime) =
    let ts = fileTimestamp dt
    let et = etype.AsString
    let name = sanitizeFilePart title
    $"{ts}_{et}_{name}.md"

let writeEventFile (basePath: string) (meta: EventMeta) (body: string) =
    let dir = eventDirectory basePath meta.OccurredAt
    ensureDirectory dir
    let filename = buildFilename meta.Type meta.Title meta.OccurredAt
    let fullPath = Path.Combine(dir, filename)
    let yaml = toYaml meta body
    File.WriteAllText(fullPath, yaml, Encoding.UTF8)
    fullPath
