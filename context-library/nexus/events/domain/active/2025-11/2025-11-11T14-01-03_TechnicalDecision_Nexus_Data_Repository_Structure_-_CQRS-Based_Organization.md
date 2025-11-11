---
id: d9b10e12-1fd7-4e74-8f12-48bb0ba6fc83
type: TechnicalDecision
title: "Nexus Data Repository Structure - CQRS-Based Organization"
summary: "Redesigned Nexus repository with CQRS separation and project-scoped organization for optimal token efficiency"
occurred_at: 2025-11-11T14:01:03.855-05:00
tags:
  - architecture
  - cqrs
  - token-efficiency
  - repository-structure
technical_decision:
---

# Context

Nexus started as a single context-library folder with mixed documentation and events. As it evolved into a full knowledge system serving multiple projects (LaundryLog, PerDiemLog, FnMCP.Nexus), the structure needed to support:

1. **Token efficiency** - Load only relevant knowledge per conversation
2. **Project isolation** - Keep project-specific knowledge separate
3. **CQRS principles** - Separate write side (events) from read side (docs/projections)
4. **Distributed operations** - Support concurrent event creation without conflicts

# Decision

Restructure Nexus data repository with CQRS separation:

```
nexus/
├─ events/                           ← WRITE SIDE (source of truth)
│  ├─ core/                          (Nexus system events)
│  ├─ laundrylog/                    (LaundryLog events)
│  ├─ perdiem/                       (PerDiemLog events)
│  └─ fnmcp-nexus/                   (MCP server events)
│
├─ core/                             ← READ SIDE (shared across projects)
│  ├─ framework/
│  │  ├─ philosophy.md
│  │  ├─ methodology.md
│  │  └─ tech-stack.md
│  └─ projections/
│     ├─ timeline/
│     ├─ knowledge/
│     └─ metrics/
│
└─ projects/                         ← READ SIDE (project-specific)
   ├─ laundrylog/
   │  ├─ docs/
   │  └─ projections/
   ├─ perdiem/
   │  ├─ docs/
   │  └─ projections/
   └─ fnmcp-nexus/
      ├─ docs/
      └─ projections/
```

## Key Principles

**CQRS Separation:**
- **Write Side (events/):** Immutable source of truth, append-only
- **Read Side (core/ + projects/):** Derived/updated knowledge, optimized for querying

**Project Scoping:**
- Each project has isolated events, docs, and projections
- Core framework knowledge shared across all projects
- Token efficiency: Load only relevant project context

**Flat Event Storage:**
- Events stored flat within project folders (YAGNI on year/type folders)
- Sharding addressed at pain point, not prematurely
- Type information in filename and frontmatter

# Token Efficiency Impact

**Before (mixed structure):**
```
LaundryLog conversation loads:
├─ framework/ (~2K tokens) ✓
├─ apps/laundrylog/ (~4K tokens) ✓
├─ apps/perdiem/ (~3K tokens) ✗ Unnecessary!
└─ Mixed events (~3K tokens) ✗ Irrelevant events!
Total: ~12K tokens
```

**After (project-scoped):**
```
LaundryLog conversation loads:
├─ core/framework/ (~2K tokens) ✓
└─ projects/laundrylog/ (~4K tokens) ✓
Total: ~6K tokens (50% reduction!)
```

# Migration Path

1. Create new nexus/ repository structure
2. Migrate existing events to project-scoped folders
3. Update projections to query from new locations
4. Update MCP server paths
5. Archive old context-library structure

# Consequences

**Positive:**
- ✅ Token consumption scales per-project, not globally
- ✅ CQRS enables clear separation of concerns
- ✅ Project isolation supports multiple teams/workstreams
- ✅ Flat storage simplifies querying and maintenance

**Negative:**
- ⚠️ Requires migration of existing events
- ⚠️ Projection logic needs updating for new paths
- ⚠️ Cross-project queries slightly more complex

**Mitigations:**
- Create comprehensive migration script
- Update projection logic atomically
- Document cross-project query patterns
