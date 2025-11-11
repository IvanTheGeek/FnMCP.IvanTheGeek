---
id: 887d611a-6899-42ac-be4a-7f11ed9278e0
type: FrameworkInsight
title: "List.collect for Multi-Source Aggregation"
summary: "F# List.collect elegantly aggregates events from multiple project folders into single timeline"
occurred_at: 2025-11-11T18:08:04.698-05:00
tags:
  - f-sharp
  - list-collect
  - aggregation
  - functional-programming
  - phase-3
---

## Insight: List.collect for Multi-Source Data Aggregation

### The Challenge
Phase 3 Timeline projection needs to read events from 5 different directories:
- Old path (backward compat)
- Core project
- LaundryLog project
- PerDiem project
- FnMCP-Nexus project

All events must be combined into single chronological timeline.

### The Solution
**Use List.collect to flatten multi-source reads:**

```fsharp
let readTimeline (basePath: string) : TimelineItem list =
    let allDirs = [
        Path.Combine(basePath, "nexus", "events", "domain", "active")
        Path.Combine(basePath, "nexus", "events", "core", "domain", "active")
        Path.Combine(basePath, "nexus", "events", "laundrylog", "domain", "active")
        Path.Combine(basePath, "nexus", "events", "perdiem", "domain", "active")
        Path.Combine(basePath, "nexus", "events", "fnmcp-nexus", "domain", "active")
    ]

    allDirs
    |> List.collect (fun dir ->
        if Directory.Exists(dir) then
            Directory.GetFiles(dir, "*.md", SearchOption.AllDirectories) 
            |> Array.toList
        else [])
    |> List.choose tryParseEvent
    |> List.sortBy (fun i -> i.OccurredAt)
```

### How It Works

**List.collect signature:**
```fsharp
List.collect : ('T -> 'U list) -> 'T list -> 'U list
```

Maps each element to a list, then flattens (concatenates) all lists.

**Step by step:**
```fsharp
allDirs = ["path1"; "path2"; "path3"]

// After collect:
[
  ["path1/event1.md"; "path1/event2.md"]  // From path1
  ["path2/event3.md"]                       // From path2  
  []                                         // From path3 (doesn't exist)
]

// Flattened to:
["path1/event1.md"; "path1/event2.md"; "path2/event3.md"]
```

### Why This is Elegant

**Single pipeline:**
- Read from all directories
- Filter non-existent
- Parse events
- Sort chronologically
- Return combined timeline

**No intermediate variables:**
- No mutable lists
- No explicit loops
- Declarative, not imperative

**Compare to imperative approach:**
```fsharp
// NOT THIS:
let mutable allEvents = []
for dir in allDirs do
    if Directory.Exists(dir) then
        let files = Directory.GetFiles(dir, "*.md", SearchOption.AllDirectories)
        for file in files do
            match tryParseEvent file with
            | Some event -> allEvents <- event :: allEvents
            | None -> ()
allEvents |> List.sortBy (fun i -> i.OccurredAt)
```

**Functional version is clearer:**
- Intent obvious: collect files from all dirs
- No mutation
- Composable (pipe operator)
- Easier to test individual steps

### Key Functions in Pipeline

1. **List.collect**: Flatten multiple sources
2. **List.choose**: Filter + map (parse events, drop failures)
3. **List.sortBy**: Chronological order

Each function does one thing well.

### Handling Missing Directories

**Graceful degradation:**
```fsharp
if Directory.Exists(dir) then
    Directory.GetFiles(...) |> Array.toList
else []
```

Returns empty list if directory doesn't exist, so collect continues with other directories.

### Performance Consideration

**For large numbers of events:**
- Seq.collect could be more efficient (lazy evaluation)
- But for Nexus use case (hundreds of events max), List is fine
- List.sortBy at end does full sort anyway (not lazy)

### Extensibility

**Adding new project:**
```fsharp
let allDirs = [
    // ... existing ...
    Path.Combine(basePath, "nexus", "events", "newproject", "domain", "active")  // Just add to list!
]
```

No change to pipeline logic needed.

### Pattern for Future

When aggregating from multiple sources:
1. List all sources
2. List.collect to read from each
3. List.choose to parse/filter
4. List.sortBy/groupBy/etc for final structure

**Works for:**
- Multiple directories
- Multiple file types
- Multiple APIs/databases
- Any scenario with multiple data sources

### Confidence: Very High
List.collect is the perfect tool for multi-source aggregation in F#.
