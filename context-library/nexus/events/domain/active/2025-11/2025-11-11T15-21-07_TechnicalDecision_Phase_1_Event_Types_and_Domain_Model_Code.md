---
id: 2c028fef-11f6-4335-9cf6-0a9c502dbb97
type: TechnicalDecision
title: "Phase 1: Event Types and Domain Model (Code)"
occurred_at: 2025-11-11T15:21:07.559-05:00
tags:
  - phase-1
  - event-types
  - code
  - domain-model
---

# Phase 1: Update Event Types

## Goal
Add new event types to F# domain model without touching file structure yet.

## Code Changes

### 1. Update EventType discriminated union
```fsharp
// Domain/Events.fs
type EventType =
    | TechnicalDecision
    | DesignNote
    | ResearchFinding
    | FrameworkInsight
    | MethodologyInsight  // NEW
    | NexusInsight        // NEW
    | SessionState        // NEW
    | CrossProjectIdea    // NEW
```

### 2. Update event writer validation
```fsharp
// Domain/EventWriter.fs
let validateEventType = function
    | MethodologyInsight -> Ok ()
    | NexusInsight -> Ok ()
    | SessionState -> Ok ()
    | CrossProjectIdea -> Ok ()
    | other -> Ok ()
```

### 3. Update MCP tool parameter validation
```fsharp
// Tools/EventTools.fs
let validEventTypes = [
    "TechnicalDecision"
    "DesignNote"
    "ResearchFinding"
    "FrameworkInsight"
    "MethodologyInsight"  // NEW
    "NexusInsight"        // NEW
    "SessionState"        // NEW
    "CrossProjectIdea"    // NEW
]
```

## Testing

1. Build project: `dotnet build`
2. Test creating each new event type via CLI:
   ```bash
   nexus create-event --type MethodologyInsight --title "Test" --body "Test"
   nexus create-event --type NexusInsight --title "Test" --body "Test"
   nexus create-event --type SessionState --title "Test" --body "Test"
   nexus create-event --type CrossProjectIdea --title "Test" --body "Test"
   ```
3. Verify files created in current location
4. Deploy new binary

## Success Criteria
- ✅ All 4 new event types compile
- ✅ CLI accepts new types
- ✅ Files created successfully
- ✅ Timeline projection shows new types

## Rollback
If issues: revert to previous binary, no data loss (events stay where they are).
