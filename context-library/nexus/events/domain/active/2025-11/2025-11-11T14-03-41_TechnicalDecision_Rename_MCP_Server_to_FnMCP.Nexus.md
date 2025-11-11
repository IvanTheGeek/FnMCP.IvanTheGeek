---
id: c67fd46c-27ae-4a83-9914-1b3d7e4d41c2
type: TechnicalDecision
title: "Rename MCP Server to FnMCP.Nexus"
summary: "Renamed MCP server from FnMCP.IvanTheGeek to FnMCP.Nexus to reflect purpose over author identity"
occurred_at: 2025-11-11T14:03:41.720-05:00
tags:
  - naming
  - repository
  - identity
  - mcp-server
technical_decision:
---

# Context

The MCP server project was initially named "FnMCP.IvanTheGeek" as a personal identifier. As Nexus evolved into a comprehensive system encompassing philosophy, methodology, framework, and projects, the server's name no longer reflected its purpose.

# Problem

**Name mismatch:**
- Repository: FnMCP.IvanTheGeek
- What it does: Provides MCP access to Nexus knowledge system
- Confusion: "IvanTheGeek" is the developer, not the system

**Inconsistent terminology:**
- System is called "Nexus"
- Access mechanism called "IvanTheGeek"
- Documentation refers to both names

# Decision

**Rename MCP server project to: FnMCP.Nexus**

**Scope:**
- Git repository: FnMCP.IvanTheGeek → FnMCP.Nexus
- .NET Solution file
- .NET Project file
- All documentation references
- Binary deployment (already named "nexus" ✓)

**Keep as-is:**
- Developer identity: "IvanTheGeek" (that's me, the person)
- User paths: `/home/linux/IvanTheGeek/` (personal directories)

# Rationale

**Semantic Accuracy:**
- The server provides access TO Nexus
- Name should reflect purpose, not author
- "FnMCP.Nexus" clearly states: "F# MCP server for Nexus"

**Consistency:**
- Aligns with "Nexus" terminology throughout system
- Memory: "User prefers 'Nexus' as term for integrated development context system"
- Reduces cognitive load switching between names

**Scalability:**
- Future contributors understand purpose from name
- Marketing/community: "Nexus MCP Server" is clear
- Avoids personality cult (it's about the system, not me)

# Identity Distinction

**Three separate concepts:**
1. **IvanTheGeek** = Me, the developer/person
2. **Nexus** = The integrated development context system
3. **FnMCP.Nexus** = The MCP server that provides access to Nexus

Analogy:
- Linux Torvalds ≠ Linux
- Linus built Linux
- IvanTheGeek builds Nexus

# Migration Steps

1. **Create TechnicalDecision event** (this event) ✓
2. **Rename Git repository** (GitHub supports renames with automatic redirects)
3. **Update solution/project files** (.sln, .fsproj)
4. **Update documentation** (all references to old name)
5. **Add name mapping to projections** (handle historical events)
6. **Update MCP server paths** (if needed)
7. **Test build and deployment**

# Historical Handling

**Event Sourcing Approach: Projection Normalization (Option C)**

Events remain unchanged (immutable history):
```
2025-11-11: "Built FnMCP.IvanTheGeek Phase 2"
2025-11-11: "FnMCP.IvanTheGeek binary deployed"
```

Projections translate during generation:
```fsharp
let normalizeProjectName = function
    | "FnMCP.IvanTheGeek" -> "FnMCP.Nexus"
    | other -> other
```

Timeline displays:
```
"Built FnMCP.Nexus Phase 2 (formerly IvanTheGeek)"
```

**Rationale:** Preserves historical accuracy while providing current clarity.

# Consequences

**Positive:**
- ✅ Name accurately reflects purpose
- ✅ Consistent terminology across system
- ✅ Clearer for contributors and users
- ✅ Scalable beyond personal project

**Negative:**
- ⚠️ Requires repository rename and migration
- ⚠️ Breaks some external references (URLs)
- ⚠️ Historical events reference old name

**Mitigations:**
- GitHub auto-redirects old URLs
- Projection normalization handles historical references
- Migration documented in this event
