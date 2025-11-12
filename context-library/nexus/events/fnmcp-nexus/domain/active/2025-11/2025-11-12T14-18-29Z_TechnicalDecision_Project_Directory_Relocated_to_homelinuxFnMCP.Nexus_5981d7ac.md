---
id: 973fe24b-ce2a-4246-aa5d-29dec07c672d
type: TechnicalDecision
title: "Project Directory Relocated to /home/linux/FnMCP.Nexus"
summary: "Project directory relocated from /home/linux/RiderProjects/FnMCP.IvanTheGeek/ to /home/linux/FnMCP.Nexus/. All paths updated, old location deprecated."
occurred_at: 2025-11-12T09:18:29.630-05:00
tags:
  - directory-structure
  - migration
  - phase-6
  - infrastructure
technical_decision:
---

## Directory Relocation

**From:** `/home/linux/RiderProjects/FnMCP.IvanTheGeek/`
**To:** `/home/linux/FnMCP.Nexus/`

**Rationale:**
- Cleaner path structure (remove nested RiderProjects)
- Matches renamed project identity (FnMCP.Nexus)
- Simpler absolute path for development and deployment

## Impact

**File paths updated:**
- Context library now at: `/home/linux/FnMCP.Nexus/context-library/`
- MCP server code at: `/home/linux/FnMCP.Nexus/src/`
- All events, projections, documentation moved

**Configuration:**
- MCP server config updated with new binary path
- Git repository location changed
- Any IDE/tooling configurations need updating

## Migration Complete

All files physically moved to new location. Old path should be ignored in all future references.

**Memory updated:** Added project location to user preferences for all future chats.

**Bootstrap files:** May need path updates if they reference absolute paths (checked separately).
