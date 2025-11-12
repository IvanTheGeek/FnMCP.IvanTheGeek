---
id: 995c47ac-5cb3-4ce3-b5e5-2ff9fbf4ac12
type: MethodologyInsight
title: "Latest Event Query Pattern for Projections"
occurred_at: 2025-11-11T18:59:48.981-05:00
---

## Pattern Discovered

Reading the latest event of a specific type from a project folder is a recurring pattern in event-sourced projections.

## Implementation

```fsharp
let readLatestSessionState (basePath: string) (project: string) : TimelineItem option =
    let projectPath = Path.Combine(basePath, "nexus", "events", project, "domain", "active")
    
    if Directory.Exists(projectPath) then
        Directory.GetFiles(projectPath, "*SessionState*.md", SearchOption.AllDirectories)
        |> Array.toList
        |> List.choose tryParseEvent
        |> List.filter (fun e -> e.Type = "SessionState")
        |> List.sortByDescending (fun e -> e.OccurredAt)
        |> List.tryHead
    else
        None
```

## Why This Pattern Works

1. **Filename filtering** - "*SessionState*.md" reduces parsing overhead
2. **tryParseEvent** - Gracefully handles malformed events
3. **Type filtering** - Ensures we only get SessionState events
4. **Sort descending** - Most recent event first
5. **tryHead** - Returns Option type (None if no events)

## Reusability

This pattern can be generalized for any event type:
- Latest TechnicalDecision for project status
- Latest DesignNote for current design
- Latest CrossProjectIdea for pending ideas

## Performance

- Only parses matching filenames
- Sorts in memory (acceptable for ~100s of events per project)
- Could be optimized with timestamp-based binary search if needed

## Next Application

Phase 5 will use this pattern to read pending CrossProjectIdea events and surface them in continuation context.
