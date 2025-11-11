---
id: 8b9c0d1e-2f3a-4b5c-6d7e-8f9a0b1c2d3e
type: TechnicalDecision
title: "Nexus Back To Context Library"
summary: "Moved nexus back to context-library/ to align with MCP server configuration"
occurred_at: 2025-11-11T01:20:00.000-05:00
tags:
  - rollback
  - mcp-integration
  - pragmatism
  - course-correction
author: Claude
technical_decision:
  status: decided
  decision: "Move nexus/ from project root back to context-library/nexus/"
  context: "User reported events not visible in MCP server. MCP server is configured with contextLibraryPath = 'context-library'"
  consequences: "Events now accessible via MCP tools. Simplifies Phase 1. Can revisit separation in Phase 2 when needed."
---

# Nexus Back To Context Library

## The Problem

User reported:
> "I am not seeing the events in the directory that active Nexus MCP server you are connected to"

**Root cause:** Mismatch between event location and MCP server configuration.

## What Happened

During refactoring, I made a decision to move `nexus/` to project root based on architectural reasoning (see `NexusAtProjectRoot` event).

**However:**
- MCP server is configured with `contextLibraryPath = "context-library"`
- MCP tools use `contextLibraryPath` as base path for events
- Events were at `./nexus/` but MCP expected `context-library/nexus/`
- Result: **Events invisible to MCP server**

## The Decision

**Roll back the move. Keep nexus under context-library for Phase 1.**

## Reasoning

**Pragmatism over purity:**
- Architectural elegance doesn't matter if it breaks functionality
- MCP integration is more important than separation of concerns right now
- Phase 1 goal: validate event sourcing works
- Can refactor paths in Phase 2 when architecture stabilizes

**YAGNI principle:**
- "Nexus at root" was premature optimization
- No actual benefit in Phase 1
- Adds complexity (two paths to manage)
- Simpler to keep everything under one base path

**User-driven correction:**
- User discovered the issue immediately
- Fast feedback loop validated
- Quick course correction shows agility

## Implementation

Simple rollback:
```bash
mv nexus context-library/
```

Updated test scripts:
- `basePath = "."` → `basePath = "context-library"`

All tests still pass. MCP can now access events.

## Structure Now

```
FnMCP.IvanTheGeek/
└── context-library/          # MCP base path
    ├── framework/            # Static docs
    ├── apps/
    ├── technical/
    └── nexus/                # Back here!
        ├── events/
        │   └── domain/active/2025-11/
        └── projections/
            └── timeline/evolution.md
```

**One base path. Simple. Works.**

## Lesson Learned

**Don't separate prematurely:**
- Wait until you actually NEED separation
- Phase 1: validate concepts
- Phase 2: optimize structure
- Refactoring is cheap when you have tests

**Listen to users:**
- "I don't see events" = immediate signal
- Direct feedback is gift
- Course correct quickly

**YAGNI is real:**
- The "Nexus At Project Root" decision was theoretical
- No concrete benefit manifested
- Added actual complexity
- Rollback was correct move

## Future Consideration

**When to revisit:**
- If MCP needs to serve events AND docs separately
- If archival strategy requires different paths
- If performance demands separation
- If multiple consumers need different base paths

**Not now. Not Phase 1.**

## Meta-Lesson

**This is event sourcing working correctly:**
- Made decision → documented in event
- Discovered problem → documented in event
- Rolled back → documented in event
- **Complete history preserved**

Can see entire journey:
1. `TechnicalDecision_FlatStructureOverNested` - initial simplicity
2. `DesignDecision_NexusAtProjectRoot` - attempted separation
3. `TechnicalDecision_NexusBackToContextLibrary` - pragmatic rollback

Timeline shows evolution. Decisions aren't permanent. Learning is captured.

**Perfect example of framework in action.**

---

*Sometimes the best architecture is the one that actually works right now.*
