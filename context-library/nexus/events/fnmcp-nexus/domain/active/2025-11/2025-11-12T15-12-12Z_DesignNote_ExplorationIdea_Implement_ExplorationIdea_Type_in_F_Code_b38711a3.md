---
id: 279c1178-d9fc-4c33-8d15-7c5c9fae665d
type: DesignNote
title: "ExplorationIdea: Implement ExplorationIdea Type in F# Code"
summary: "Add ExplorationIdea as proper F# type. Currently using DesignNote workaround. Plan for CCode implementation after data migration complete."
occurred_at: 2025-11-12T10:12:12.634-05:00
tags:
  - exploration-idea
  - f-sharp
  - event-types
  - implementation
  - priority-important
  - status-pending
  - blocked-by-migration
---

## Exploration: Implement ExplorationIdea Type in F# Code

**Type:** ExplorationIdea (using DesignNote until implemented)
**Priority:** important
**Status:** pending

**Spark:** Add ExplorationIdea to F# discriminated union so it's a real event type, not a workaround.

## Context

Currently using DesignNote with `exploration-idea` tags as workaround. This causes:
- Failed create_event attempts when I forget it's not implemented
- Error toasts in Claude Desktop
- Token waste on retries
- Less type safety
- Can't create dedicated `capture_exploration_idea` tool

## The Idea

**Phase 1: Add to F# Code**
```fsharp
// In Domain.fs, add to EventType DU:
type EventType =
    | TechnicalDecision
    | DesignNote
    | FrameworkInsight
    | SessionState
    | CrossProjectIdea
    | MethodologyInsight
    | NexusInsight
    | ExplorationIdea  // <-- Add this
    | ResearchFinding
```

**Phase 2: Enhanced Validation**
- Better error messages when invalid type used
- Return list of valid types to help Claude
- Reduce token waste on failed attempts

**Phase 3: Dedicated Tool**
Create `capture_exploration_idea` tool with:
- Simpler parameters (focus on capturing the spark)
- Status/priority enums
- Category/scope options
- Better DX for idea capture

**Phase 4: Taxonomy & Projection**
- Define status workflow
- Define priority levels
- Define category classifications
- Build ideas-backlog projection

## Benefits

- Type safety in F# code
- No more failed attempts
- Dedicated tooling
- Better error messages
- Foundation for ideas backlog system

## Next Steps

**Waiting on:** Data migration completion (user will handle)

**After migration:**
1. Create implementation plan for CCode
2. CCode adds ExplorationIdea to Domain.fs
3. Update validation and tools
4. Rebuild and deploy
5. Test with real event creation
6. Migrate existing tagged DesignNote events
7. Build projection (later phase)

## Dependencies

- **Blocked by:** Data migration (user completing)
- **Enables:** Ideas backlog system, better tooling, type safety

## Implementation Timing

Wait for user signal: "Data migration complete, ready for CCode plan"

Then create detailed step-by-step checklist for CCode similar to Phase 1-6 patterns.
