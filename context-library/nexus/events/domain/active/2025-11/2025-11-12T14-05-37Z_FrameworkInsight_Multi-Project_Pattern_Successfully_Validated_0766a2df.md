---
id: 72d3d271-e357-40f6-8511-ca646b320911
type: FrameworkInsight
title: "Multi-Project Pattern Successfully Validated"
occurred_at: 2025-11-12T09:05:37.612-05:00
tags:
  - multi-project
  - architecture
  - bootstrap-pattern
  - token-efficiency
---

## Pattern Overview

**Goal:** Separate applications into their own Claude Projects while sharing Nexus infrastructure (MCP server, documentation, framework).

**Implementation:** PerDiem moved to separate project with minimal bootstrap file.

## Architecture

**Shared Infrastructure:**
- FnMCP.Nexus MCP server (globally available)
- context-library documentation (served via MCP)
- Framework patterns and methodology
- Event sourcing system

**Project-Specific:**
- Bootstrap file (~500 tokens each)
- Project instructions
- Conversation focus
- SessionState events

**Projects Created:**
1. **Nexus Project** - Framework + FnMCP.Nexus server + LaundryLog dogfooding
2. **PerDiemLog Project** - PerDiem app development only

## Token Efficiency Comparison

**Before separation (single Nexus project):**
- Project Knowledge: ~6,000 tokens (all apps + framework)
- Every conversation loads everything

**After separation:**
- Nexus Project Knowledge: ~500 tokens (framework-focused bootstrap)
- PerDiem Project Knowledge: ~500 tokens (app-focused bootstrap)
- Documentation accessed on-demand via MCP

**Per-conversation savings:**
- Nexus conversations: 92% reduction (6,000 → 500)
- PerDiem conversations: Start minimal (500 tokens)
- More conversation capacity for actual development

## Project Isolation Benefits

**Conversation focus:**
- Nexus chats naturally discuss framework and LaundryLog
- PerDiem chats naturally focus on PerDiem features
- Cross-project references explicit (via MCP)

**Timeline filtering:**
- Each project can view its own event history
- Reduces noise from unrelated work
- Easier to track project-specific progress

**Continuation prompts:**
- continue-core, continue-laundrylog, continue-nexus → Nexus project
- continue-perdiem → PerDiem project
- Each loads appropriate SessionState

## When to Graduate Applications

**Keep in Nexus project when:**
- Heavy framework development (dogfooding patterns)
- Frequent cross-project refactoring
- Testing new framework concepts
- Active pattern validation

**Graduate to separate project when:**
- Framework patterns stabilized
- App entering focused development phase
- Less frequent cross-cutting changes
- Ready for sustained feature work

**LaundryLog status:** Currently in Nexus (dogfooding v7 patterns). Ready to graduate after v7 implementation complete.

## Bootstrap File Requirements

**Consistent structure across projects:**
- Project scope and status
- Continuation mechanisms
- File paths (MCP access)
- **Token calculation formula** (critical!)
- Context management
- Event sourcing location

**Project-specific content:**
- PerDiem focuses on apps/perdiem/ docs
- Nexus focuses on framework/ docs
- Both can access any documentation via MCP when needed

## Pattern Scalability

**Validated for future Cheddar apps:**
- Each app gets own project
- All share Nexus MCP infrastructure
- Bootstrap pattern proven
- Token efficiency demonstrated

**Ready to create:**
- LaundryLog project (when ready)
- CheddarBooks project (future)
- Any other Cheddar ecosystem apps
