#!/usr/bin/env dotnet fsi

#r "src/FnMCP.IvanTheGeek/bin/Debug/net9.0/FnMCP.IvanTheGeek.dll"

open System
open FnMCP.IvanTheGeek.Domain
open FnMCP.IvanTheGeek.Domain.EventWriter
open FnMCP.IvanTheGeek.Projections.Knowledge

printfn "=========================================="
printfn "Bootstrapping Knowledge Base"
printfn "=========================================="
printfn ""

let basePath = "context-library"

// Learning Event 1: Interpolated String Variable Extraction Pattern
printfn "Creating pattern: interpolated-string-variable-extraction..."
let pattern1 : LearningEventMeta = {
    Id = Guid.NewGuid()
    Type = PatternDiscovered
    Title = "Interpolated String Variable Extraction"
    Summary = Some "Extract complex expressions to variables before using in interpolated strings"
    OccurredAt = DateTime.Now
    Tags = ["fsharp"; "syntax"; "strings"; "compilation"]
    ErrorCode = None
    ErrorMessage = None
    PatternName = Some "interpolated-string-variable-extraction"
    PatternCategory = Some Syntax
    UseCount = Some 8
    SuccessRate = Some 1.0
    FilePath = Some "Domain/Projections.fs"
    ConversationContext = Some "Phase 2 & 3 implementation"
    RelatedPatterns = []
}

let pattern1Body = """## Pattern

When interpolated strings contain complex expressions (especially with format strings containing special characters like single quotes), extract the formatted value to a variable first.

## Example

```fsharp
// ✗ Fails with FS3373
$"timestamp: {dt.ToString(\"yyyy-MM-dd'T'HH:mm:ss\")}"

// ✓ Works
let timestamp = dt.ToString("yyyy-MM-dd'T'HH:mm:ss")
$"timestamp: {timestamp}"
```

## Reason

F# interpolated strings don't support format strings with special characters directly in the interpolation expression.

## Confidence

High - Used 8+ times during Phase 2/3, 100% success rate.
"""

writeLearningEvent basePath pattern1 pattern1Body |> ignore
printfn "✓ Pattern created\n"

// Learning Event 2: FS3373 Error Encountered
printfn "Creating error: FS3373..."
let error1 : LearningEventMeta = {
    Id = Guid.NewGuid()
    Type = ErrorEncountered
    Title = "FS3373: Invalid Interpolated String"
    Summary = Some "Single quote or verbatim string literals not allowed in interpolated expressions"
    OccurredAt = DateTime.Now
    Tags = ["fsharp"; "compilation"; "fs3373"; "strings"]
    ErrorCode = Some "FS3373"
    ErrorMessage = Some "Invalid interpolated string. Single quote or verbatim string literals may not be used in interpolated expressions"
    PatternName = None
    PatternCategory = None
    UseCount = Some 5
    SuccessRate = None
    FilePath = Some "Domain/Projections.fs"
    ConversationContext = Some "Phase 2 projection metadata"
    RelatedPatterns = ["interpolated-string-variable-extraction"]
}

let error1Body = """## Context

Hit this error 5 times during Phase 2 and 3 implementation when formatting DateTime values in YAML frontmatter.

## Original Code (Failed)

```fsharp
content.AppendLine($"last_regenerated: {meta.LastRegenerated.ToString(\"yyyy-MM-dd'T'HH:mm:ss.fffK\")}") |> ignore
```

## Error Message

> Invalid interpolated string. Single quote or verbatim string literals may not be used in interpolated expressions

## Cause

The format string contains single quotes (`'T'`) which conflicts with F# interpolated string syntax.
"""

writeLearningEvent basePath error1 error1Body |> ignore
printfn "✓ Error documented\n"

// Learning Event 3: Solution for FS3373
printfn "Creating solution: FS3373..."
let solution1 : LearningEventMeta = {
    Id = Guid.NewGuid()
    Type = SolutionApplied
    Title = "Fixed FS3373 by extracting timestamp variable"
    Summary = Some "Extract DateTime.ToString() to variable before interpolation"
    OccurredAt = DateTime.Now
    Tags = ["fsharp"; "solution"; "fs3373"; "strings"]
    ErrorCode = Some "FS3373"
    ErrorMessage = None
    PatternName = Some "interpolated-string-variable-extraction"
    PatternCategory = Some Syntax
    UseCount = None
    SuccessRate = Some 1.0
    FilePath = Some "Domain/Projections.fs"
    ConversationContext = Some "Phase 2 projection metadata"
    RelatedPatterns = []
}

let solution1Body = """## Solution

Extract the formatted value to a variable before using it in the interpolated string.

## Fixed Code (Success)

```fsharp
let timestamp = meta.LastRegenerated.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK")
content.AppendLine($"last_regenerated: {timestamp}") |> ignore
```

## Success Rate

100% - Applied 5 times, worked every time.

## Pattern

This is an instance of the `interpolated-string-variable-extraction` pattern.
"""

writeLearningEvent basePath solution1 solution1Body |> ignore
printfn "✓ Solution recorded\n"

// Learning Event 4: Percent Sign Escaping
printfn "Creating pattern: percent-sign-escaping..."
let pattern2 : LearningEventMeta = {
    Id = Guid.NewGuid()
    Type = PatternDiscovered
    Title = "Percent Sign Escaping in Interpolated Strings"
    Summary = Some "Escape % as %% in F# interpolated strings"
    OccurredAt = DateTime.Now
    Tags = ["fsharp"; "syntax"; "strings"]
    ErrorCode = None
    ErrorMessage = None
    PatternName = Some "percent-sign-escaping"
    PatternCategory = Some Syntax
    UseCount = Some 2
    SuccessRate = Some 1.0
    FilePath = Some "Projections/Knowledge.fs"
    ConversationContext = Some "Phase 3 knowledge projections"
    RelatedPatterns = []
}

let pattern2Body = """## Pattern

When using the `%` character in F# interpolated strings, it must be escaped as `%%`.

## Example

```fsharp
// ✗ Fails - F# interprets % as format specifier
$"Success rate: {pct}%"

// ✓ Works
$"Success rate: {pct}%%"
```

## Reason

F# uses `%` for format specifiers (printf-style), so literal percent signs must be escaped.

## Confidence

High - Used 2 times during Phase 3, 100% success rate.
"""

writeLearningEvent basePath pattern2 pattern2Body |> ignore
printfn "✓ Pattern created\n"

// Regenerate knowledge projections
printfn "Regenerating knowledge projections..."
let knowledgePath = KnowledgeWriter.regenerateKnowledge basePath
printfn "✓ Knowledge regenerated: %s\n" knowledgePath

printfn "=========================================="
printfn "Knowledge Base Bootstrap Complete!"
printfn "=========================================="
printfn ""
printfn "Created 4 learning events:"
printfn "  - 2 patterns (interpolated-string-variable-extraction, percent-sign-escaping)"
printfn "  - 1 error (FS3373)"
printfn "  - 1 solution (FS3373 fix)"
printfn ""
printfn "Knowledge projections generated:"
printfn "  - patterns.md"
printfn "  - error-solutions.md"
printfn "  - confidence-scores.yaml"
printfn ""
printfn "🧠 Your F# knowledge base is now operational!"
