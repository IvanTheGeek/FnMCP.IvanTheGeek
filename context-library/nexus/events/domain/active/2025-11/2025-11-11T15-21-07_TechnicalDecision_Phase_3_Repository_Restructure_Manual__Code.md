---
id: 96eaa5a5-d979-4f38-b044-2e8fd27fe8b0
type: TechnicalDecision
title: "Phase 3: Repository Restructure (Manual + Code)"
occurred_at: 2025-11-11T15:21:07.559-05:00
tags:
  - phase-3
  - restructure
  - cqrs
  - migration
  - manual
---

# Phase 3: CQRS Repository Structure

## Goal
Reorganize file structure for project scoping and CQRS separation.

## Manual Steps

### 1. Create new directory structure
```bash
cd /home/linux/RiderProjects/FnMCP.Nexus/
mkdir -p nexus/events/core
mkdir -p nexus/events/laundrylog
mkdir -p nexus/events/perdiem
mkdir -p nexus/events/fnmcp-nexus
mkdir -p nexus/core/framework
mkdir -p nexus/core/projections/timeline
mkdir -p nexus/core/projections/knowledge
mkdir -p nexus/core/projections/metrics
mkdir -p nexus/projects/laundrylog/docs
mkdir -p nexus/projects/laundrylog/projections
mkdir -p nexus/projects/perdiem/docs
mkdir -p nexus/projects/perdiem/projections
mkdir -p nexus/projects/fnmcp-nexus/docs
mkdir -p nexus/projects/fnmcp-nexus/projections
```

### 2. Migrate existing events
```bash
# Review each event and move to appropriate project folder
# Core Nexus events → nexus/events/core/
# LaundryLog events → nexus/events/laundrylog/
# PerDiemLog events → nexus/events/perdiem/
# MCP server events → nexus/events/fnmcp-nexus/
```

### 3. Migrate documentation
```bash
# framework docs → nexus/core/framework/
cp context-library/framework/*.md nexus/core/framework/

# project docs
cp context-library/apps/laundrylog/*.md nexus/projects/laundrylog/docs/
cp context-library/apps/perdiem/*.md nexus/projects/perdiem/docs/
cp context-library/technical/*.md nexus/projects/fnmcp-nexus/docs/
```

## Code Changes

### 1. Update MCP server paths
```fsharp
// Update base paths
let eventsBasePath = Path.Combine(contextLibrary, "events")
let coreBasePath = Path.Combine(contextLibrary, "core")
let projectsBasePath = Path.Combine(contextLibrary, "projects")
```

### 2. Update event writer to use project-scoped paths
```fsharp
let getEventPath (project: string option) =
    match project with
    | Some proj -> Path.Combine(eventsBasePath, proj)
    | None -> Path.Combine(eventsBasePath, "core")
```

### 3. Update projections to read from new structure
```fsharp
// Timeline reads from all event folders
let allEventFolders = [
    "events/core"
    "events/laundrylog"
    "events/perdiem"
    "events/fnmcp-nexus"
]
```

## Testing

1. Build with new paths
2. Test event creation in each project:
   ```bash
   nexus create-event --project laundrylog --type TechnicalDecision --title "Test" --body "Test"
   nexus create-event --project core --type NexusInsight --title "Test" --body "Test"
   ```
3. Verify files land in correct folders
4. Test timeline projection reads all events
5. Test MCP resources serve from new paths

## Success Criteria
- ✅ New directory structure created
- ✅ All events migrated to project folders
- ✅ All docs migrated
- ✅ New events go to correct folders
- ✅ Projections work with new structure
- ✅ MCP resources accessible

## Rollback
Keep old context-library as backup until fully validated.
