---
id: 73bd5b63-f4c7-44c4-993e-2985e976188e
type: FrameworkInsight
title: "YAML Frontmatter Decoupling Enables Zero-Impact Schema Changes"
summary: "Because projections read YAML frontmatter not filenames, changing filename format had zero breaking changes"
occurred_at: 2025-11-11T17:39:26.858-05:00
tags:
  - framework-insight
  - yaml
  - schema-evolution
  - phase-2
  - architecture
---

## Insight: Separation of Concerns in Event Storage

### The Discovery
Phase 2 changed filename format from:
```
2025-11-11T17-00-21_Type_Title.md
```
to:
```
2025-11-11T22-31-02Z_Type_Title_d17ba665.md
```

**Expected impact:** Update all projection parsers to handle GUID suffix

**Actual impact:** Zero changes to projection code

### Why This Worked

**Filenames are just storage keys** - not part of the data model:
- Event metadata lives in YAML frontmatter
- Projections read frontmatter, not filename
- Filename parsing happens only for directory scanning
- Content parsing is format-agnostic

### Code Evidence
```fsharp
// Projections/Timeline.fs - UNCHANGED
let tryParseEvent (path: string) : TimelineItem option =
    let content = File.ReadAllText(path)
    let (fm, _) = FrontMatterParser.parseFrontMatter content
    let title = fm.TryFind("title")  // Read from YAML, not filename!
    let etype = fm.TryFind("type")   // Read from YAML, not filename!
    let occurredStr = fm.TryFind("occurred_at")  // Read from YAML!
```

### Design Principle Validated

**Filenames are storage implementation details:**
- Change format without breaking readers
- Migrate incrementally (old + new formats coexist)
- No versioning needed
- No migration scripts needed

### Contrast: If We Read Filenames

Hypothetical bad design:
```fsharp
// DON'T DO THIS
let getEventType (path: string) =
    let parts = Path.GetFileName(path).Split('_')
    parts.[1]  // BRITTLE! Breaks when format changes
```

### Benefit for Phase 2
- Changed 1 file (EventWriter.fs)
- Touched 0 projection files
- Broke 0 existing functionality
- Migration was instant (new events use new format)
- Old events still readable

### Generalization: Event-Sourced Schema Evolution

In event-sourced systems:
1. **Events are immutable** - don't change existing files
2. **Readers are flexible** - handle multiple formats
3. **Storage is opaque** - filename/path is implementation detail
4. **Schema lives in data** - YAML frontmatter, not file structure

### Confidence: Extremely High
Phase 2 proved this design decision was correct.
