---
id: 7bd22d5f-25d1-42ff-a94a-054be2bab8ef
type: FrameworkInsight
title: "Token Waste Analysis - Filesystem Operations Need Custom MCP"
summary: "Token waste analysis: filesystem operations cause repeated confusion. Solution: custom MCP + remote deployment. Need ideas backlog projection for tracking exploration ideas."
occurred_at: 2025-11-12T09:55:49.587-05:00
tags:
  - token-efficiency
  - filesystem-issues
  - future-planning
  - remote-deployment
---

## Observation: Token Waste on Filesystem Operations

**Problem:** Across multiple chats, Claude struggles with filesystem access leading to:
- Retries and confusion (10-20 tool calls wasted)
- Path resolution uncertainty
- Token consumption on figuring out what should be simple
- Bash tool limitations (container isolation)
- Filesystem MCP confusion (multiple paths, unclear errors)

**Impact:** Hundreds of tokens wasted per chat on filesystem operations that should be straightforward.

## Root Cause

**Tools optimized for general use, not Nexus workflows:**
- Generic filesystem MCP doesn't understand Nexus structure
- Bash tool runs in isolated container
- No Nexus-aware path resolution
- Error messages don't help Claude understand what's wrong

## Solution: Custom MCP for Local Operations

**Build FnMCP.LocalFileSystem** - purpose-built MCP server for Nexus development:

**Features:**
- Nexus-aware path resolution (knows about /home/linux/Nexus-Data structure)
- Better error messages ("File not found at /home/linux/Nexus-Data/apps/laundrylog/overview.md. Did you mean /home/linux/FnMCP.Nexus/context-library/apps/laundrylog/overview.md?")
- Atomic operations (copy-rename-organize in one call)
- Migration helpers (built-in understanding of structure changes)
- Claude-optimized responses (less verbose, more actionable)

**Benefits:**
- Reduce token waste on filesystem confusion
- Faster operations (fewer retries)
- Better error recovery
- Migration operations become single tool calls

## Future: Remote VPS Deployment

**Why remote:**
- Stable internet addressing
- More storage capacity
- Claude Android compatibility (requires remote MCP)
- Always-on availability
- Multi-device access

**Considerations:**
- Authentication/authorization
- Performance over network
- Data synchronization
- Backup strategies
- Multi-tenant architecture (future Cheddar services)

## Ideas Backlog System

**Need:** System to capture and track exploration ideas for future work.

**Existing capability:** CrossProjectIdea events already capture ideas with:
- Source/target projects
- Priority (consider/important/low)
- Status tracking
- Context links

**Missing:** Projection to aggregate ideas into prioritized backlog view.

**Proposal:** Create "Ideas Backlog" projection:
- Lists all CrossProjectIdea events
- Groups by priority
- Shows status (pending/exploring/implemented/rejected)
- Filterable by target project
- Quick reference for "what to work on next"

## Action Items Captured

1. ✅ Captured custom filesystem MCP idea
2. ✅ Captured remote VPS deployment idea
3. Need: Create ideas-backlog projection
4. Need: Document when to use CrossProjectIdea vs. other event types
