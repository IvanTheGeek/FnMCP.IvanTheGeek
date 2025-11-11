---
id: bac7bd80-894d-4ce2-9d52-ee41aa25d982
type: FrameworkInsight
title: "Optional Parameters Enable Backward Compatibility"
summary: "Using Option<string> for project parameter allowed gradual migration without breaking existing code"
occurred_at: 2025-11-11T18:04:50.128-05:00
tags:
  - framework-insight
  - option-types
  - backward-compatibility
  - phase-3
  - f-sharp
---

## Insight: Option Types for Non-Breaking Schema Evolution

### The Challenge
Phase 3 needed to add project-scoped paths without breaking existing event creation.

### The Solution
**Use F# Option type for new parameter:**
```fsharp
let writeEventFile (basePath: string) (project: string option) (meta: EventMeta) (body: string) =
    let dir = eventDirectory basePath project meta.OccurredAt
    //                                 ↑
    //                         Option<string>
```

**Pattern match determines behavior:**
```fsharp
let eventDirectory (basePath: string) (project: string option) (dt: DateTime) =
    match project with
    | Some proj -> Path.Combine(basePath, "nexus", "events", proj, "domain", "active", monthFolder dt)
    | None -> Path.Combine(basePath, "nexus", "events", "domain", "active", monthFolder dt)
```

### Why This Works

**Existing callers can pass None:**
- No migration needed immediately
- Old code continues working
- New feature opt-in, not mandatory

**New callers can pass Some(project):**
- Gradual adoption
- Project-by-project migration
- Coexistence of old and new

**Example:**
```fsharp
// Old code (still works):
writeEventFile basePath None meta body

// New code (uses project):
writeEventFile basePath (Some "laundrylog") meta body

// CLI automatically converts:
let project = getStringOpt args "project"  // Returns Option<string>
writeEventFile basePath project meta body    // Passes through directly
```

### Benefits

1. **Zero breaking changes** - all existing code compiles
2. **Type safety** - compiler enforces Option handling
3. **Self-documenting** - signature shows parameter is optional
4. **Gradual migration** - no big-bang cutover needed
5. **Testing flexibility** - can test new and old paths simultaneously

### Contrast: Alternative Approaches

**Default parameter (not idiomatic F#):**
```fsharp
let writeEventFile (basePath: string) (meta: EventMeta) (body: string) (project: string = "") =
    // Harder to distinguish "no project" from "empty project"
```

**Overloads (verbose):**
```fsharp
let writeEventFile (basePath: string) (meta: EventMeta) (body: string) = ...
let writeEventFileWithProject (basePath: string) (project: string) (meta: EventMeta) (body: string) = ...
// Two functions to maintain
```

**Breaking change (risky):**
```fsharp
let writeEventFile (basePath: string) (project: string) (meta: EventMeta) (body: string) = ...
// All callers must update immediately
```

### Phase 3 Application

We updated **7 files** with existing writeSystemEvent calls:
```bash
sed -i 's/writeSystemEvent basePath systemEvent/writeSystemEvent basePath None systemEvent/g'
```

Single sed command migrated all callers to new signature\!

### Pattern for Future Phases

When adding optional features:
1. Use Option<T> for new parameters
2. Pattern match to handle Some/None
3. Default to None preserves old behavior
4. Gradually migrate callers as needed

### Confidence: Very High
Phase 3 proved this pattern works perfectly for non-breaking evolution.
