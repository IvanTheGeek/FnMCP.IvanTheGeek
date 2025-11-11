---
id: 5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b
type: DesignDecision
title: "Nexus At Project Root"
summary: "Moved nexus/ from context-library/ to project root, separating event storage from documentation"
occurred_at: 2025-11-11T01:09:00.000-05:00
tags:
  - file-structure
  - separation-of-concerns
  - organization
author: Claude
links:
  - context://framework/event-sourced-nexus-architecture.md
---

# Nexus At Project Root

## The Decision

Move `nexus/` from `context-library/nexus/` to project root `nexus/`.

## Before

```
FnMCP.IvanTheGeek/
└── context-library/
    ├── framework/
    ├── apps/
    ├── technical/
    └── nexus/              # Mixed with static docs
        └── events/
```

## After

```
FnMCP.IvanTheGeek/
├── nexus/                  # Separate! At root!
│   ├── events/
│   └── projections/
└── context-library/        # Static docs only
    ├── framework/
    ├── apps/
    └── technical/
```

## Reasoning

**Different lifecycles:**
- `context-library/` = curated documentation (stable)
- `nexus/events/` = append-only event log (growing)
- `nexus/projections/` = computed views (regenerated)

**Different purposes:**
- Static docs: human-written, edited, refined
- Events: machine-written, immutable, timestamped
- Projections: computed from events, ephemeral

**Different access patterns:**
- Docs: MCP resources, read by Claude
- Events: written by tools, read by projections
- Projections: generated artifacts, may be gitignored

**Separation of concerns:**
- Events = source of truth
- Docs = one possible projection
- Shouldn't be nested inside each other

## Future Benefits

**Archive strategy:**
When we implement "closing the books":
```
nexus/
├── events/
│   ├── active/         # Hot data
│   └── archive/        # Compressed old quarters
└── projections/
    ├── current-docs/   # Fresh projection
    └── snapshots/      # Point-in-time
```

Easy to:
- Compress `archive/` independently
- Regenerate `projections/` without touching events
- Backup `events/` separately from `context-library/`

**CI/CD integration:**
```yaml
# .gitignore
nexus/projections/  # Regenerate on demand
```

Projections are computed, not stored. Only events committed.

## Alternative Considered

**Option: Keep nested**
```
context-library/
├── nexus/
│   ├── events/
│   └── projections/
└── static/
    ├── framework/
    └── apps/
```

**Rejected because:**
- Implies `nexus` is subset of `context-library`
- Actually reverse: `context-library` becomes projection
- Muddies the event-sourcing model
- Wrong mental model

## The Right Model

```
SOURCE:      nexus/events/          (immutable truth)
              ↓
PROJECTIONS: nexus/projections/     (computed views)
              ├── current-docs/
              ├── timeline/
              ├── blog-posts/
              └── metrics/
```

One of those projections MAY write to `context-library/`, but they're separate concerns.

## Implementation

Simple `mv`:
```bash
mv context-library/nexus nexus
```

Update code to look at `.` (project root) instead of `context-library/` for events.

All tests still pass. Structure clearer.

## Impact on MCP Server

**Before:**
- MCP server base path: `context-library/`
- Events nested inside: `context-library/nexus/events/`

**After:**
- MCP server base path: still `context-library/` (for docs)
- Events separate: `nexus/events/` (sibling to context-library)
- Functions take `basePath` parameter: `"."` for root

Clean separation. MCP serves docs. Events live independently.

---

*Events at root. Projections as siblings. Docs as artifacts. Correct model.*
