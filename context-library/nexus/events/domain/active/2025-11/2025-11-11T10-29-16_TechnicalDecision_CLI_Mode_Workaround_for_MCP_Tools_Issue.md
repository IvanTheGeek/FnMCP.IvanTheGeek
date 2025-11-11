---
id: 45610571-a54c-45c7-9c31-f0b43dfbb3c4
type: TechnicalDecision
title: "CLI Mode Workaround for MCP Tools Issue"
occurred_at: 2025-11-11T10:29:16.574-05:00
tags:
  - phase-3.5
  - cli
  - mcp
  - workaround
---

## Problem

Claude Code v2.0.37 does not expose MCP tools to the AI assistant, only resources. Despite Nexus correctly implementing 7 MCP tools, they are invisible and unusable.

## Decision

Implement dual-mode operation:
- **MCP Server Mode** (0-1 args): JSON-RPC over stdin/stdout for resources
- **CLI Mode** (2+ args): Command-line interface for tools

## Rationale

1. MCP resources work perfectly (documentation access)
2. MCP tools are blocked by Claude Code limitation
3. CLI provides immediate workaround without waiting for upstream fix
4. Same backend handlers - no duplication
5. Can remove CLI once MCP tools work

## Implementation

- CLI argument parser with hyphen/underscore normalization
- Auto-detection of context library path
- Direct routing to existing ToolRegistry handlers
- All 7 tools tested and working

## Consequences

✅ All Nexus functionality immediately usable
✅ No changes to MCP server or tool handlers
✅ Easy to remove when Claude Code fixes issue
⚠️ Temporary workaround - must monitor for upstream fix
