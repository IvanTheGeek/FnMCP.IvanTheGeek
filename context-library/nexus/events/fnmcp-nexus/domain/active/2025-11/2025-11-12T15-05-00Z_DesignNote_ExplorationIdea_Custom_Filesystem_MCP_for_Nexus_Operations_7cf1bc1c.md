---
id: f51c5b12-c960-4115-9cc6-69c70ec6c73e
type: DesignNote
title: "ExplorationIdea: Custom Filesystem MCP for Nexus Operations"
summary: "Build custom filesystem MCP optimized for Nexus workflows to eliminate token waste on retries and path confusion."
occurred_at: 2025-11-12T10:05:00.789-05:00
tags:
  - exploration-idea
  - filesystem
  - mcp
  - tooling
  - token-efficiency
  - priority-important
  - status-pending
---

## Exploration: Custom Filesystem MCP

**Type:** ExplorationIdea (using DesignNote until type implemented)
**Priority:** important
**Status:** pending

**Spark:** Build FnMCP.LocalFileSystem - purpose-built MCP server for Nexus file operations.

## Context

Across multiple chats, Claude wastes significant tokens on filesystem operations:
- Bash tool runs in isolated container (limited access)
- Generic Filesystem MCP not optimized for Nexus workflows
- Path resolution confusion
- 10-20 retries per operation
- Hundreds of tokens wasted per chat

## The Idea

Create custom MCP server that understands Nexus:
- Nexus-aware path resolution
- Better error messages ("Did you mean X?" suggestions)
- Atomic operations (copy-rename-organize in one call)
- Migration helpers (structure changes as single operations)
- Claude-optimized responses (actionable, concise)

## Benefits

- Massive token savings (reduce retries)
- Faster development (fewer tool calls)
- Better error recovery
- Enable complex operations (migration scripts as single calls)
- Foundation for future automation

## Initial Thoughts

Could start with:
1. Read Nexus-Data structure
2. Write/update markdown files
3. Atomic file operations
4. Migration helpers

Later expand to:
- Event creation shortcuts
- Projection triggers
- Validation helpers
- Structure reorganization

## Next Steps (When Explored)

1. Design tool interface
2. Implement basic F# MCP server
3. Test with real operations
4. Iterate based on usage
5. Deprecate generic filesystem tools
