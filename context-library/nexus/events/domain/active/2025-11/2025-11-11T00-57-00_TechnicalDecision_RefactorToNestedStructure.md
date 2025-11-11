---
id: 1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d
type: TechnicalDecision
title: "Refactor To Nested Structure"
summary: "Successfully refactored Phase 1 implementation from flat to nested folder structure matching the original plan"
occurred_at: 2025-11-11T00:57:00.000-05:00
tags:
  - refactoring
  - architecture
  - organization
author: Claude
technical_decision:
  status: decided
  decision: "Refactor from flat file structure to nested Domain/Projections/Tools structure"
  context: "Original implementation used flat structure for speed. User requested structure match the original architectural plan."
  consequences: "Better separation of concerns, clearer module boundaries, matches documented architecture, prepares for Phase 2 expansion"
---

# Refactor To Nested Structure

## Context

The Phase 1 implementation was done with a flat file structure:
```
src/FnMCP.IvanTheGeek/
├── Events.fs         # All event logic in one file
├── Tools.fs          # All tools together
└── Program.fs
```

The original plan specified nested structure:
```
src/
├── Domain/
│   ├── Events.fs
│   ├── EventWriter.fs
│   └── Projections.fs
├── Projections/
│   └── Timeline.fs
├── Tools/
│   └── EventTools.fs
```

## Decision

Refactor to match the planned nested structure.

## Implementation Steps

1. ✅ Created Domain/, Projections/, Tools/ directories
2. ✅ Split Events.fs into:
   - Domain/Events.fs (type definitions only)
   - Domain/EventWriter.fs (file writing logic)
   - Domain/Projections.fs (projection utilities)
3. ✅ Created Projections/Timeline.fs (timeline logic + evolution.md generation)
4. ✅ Created Tools/EventTools.fs (event tools)
5. ✅ Updated Tools.fs to use EventTools module
6. ✅ Updated .fsproj with correct file order
7. ✅ Renamed Tools module to ToolRegistry to avoid namespace conflict
8. ✅ Moved nexus/ from context-library/ to project root
9. ✅ Generated nexus/projections/timeline/evolution.md
10. ✅ Updated test scripts
11. ✅ Validated build and all tests pass

## Challenges

**F# namespace/module conflict:**
- Had both `module FnMCP.IvanTheGeek.Tools` and `namespace FnMCP.IvanTheGeek.Tools`
- F# doesn't allow same fully-qualified name for both
- Solution: Renamed main module to `ToolRegistry`

**Indentation requirements:**
- F# requires proper indentation for module contents within namespaces
- Had to rewrite files with correct indentation

## Results

**New structure:**
```
FnMCP.IvanTheGeek/
├── src/FnMCP.IvanTheGeek/
│   ├── Domain/
│   │   ├── Events.fs (57 lines - types only)
│   │   ├── EventWriter.fs (86 lines - file writing)
│   │   └── Projections.fs (48 lines - utilities)
│   ├── Projections/
│   │   └── Timeline.fs (58 lines - timeline + evolution)
│   ├── Tools/
│   │   └── EventTools.fs (154 lines - event tools)
│   ├── Tools.fs (ToolRegistry - updated)
│   ├── McpServer.fs (updated references)
│   └── Program.fs
├── nexus/                    # Moved to root!
│   ├── events/domain/active/2025-11/
│   └── projections/timeline/
│       └── evolution.md      # Auto-generated!
└── context-library/          # Separate from events
```

**All tests pass:**
- ✅ Build succeeds
- ✅ Timeline projection works
- ✅ Event creation works
- ✅ Evolution.md generation works

## Consequences

**Positive:**
- Clearer separation of concerns
- Domain types isolated from implementation
- Projections can grow independently
- Matches documented architecture
- Easier to onboard new contributors

**Negative:**
- More files to navigate (but better organized)
- Module naming had to change (ToolRegistry instead of Tools)

**Mitigation:**
- Good IDE support makes navigation easy
- Clear module hierarchy helps discoverability
- Well-documented structure in event timeline

## Next Steps

Phase 1 is now complete with proper structure. Ready for Phase 2:
- Implement enhance_nexus workflow
- Add more projection types
- Implement staleness tracking
- System events and metrics

---

*Refactoring complete. Structure now matches the plan and is ready for expansion.*
