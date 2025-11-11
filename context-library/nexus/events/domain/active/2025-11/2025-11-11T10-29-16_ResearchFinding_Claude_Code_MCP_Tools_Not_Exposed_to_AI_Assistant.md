---
id: 25e4dfe1-22ec-46b5-92b2-531df6db3e75
type: ResearchFinding
title: "Claude Code MCP Tools Not Exposed to AI Assistant"
occurred_at: 2025-11-11T10:29:16.597-05:00
tags:
  - mcp
  - claude-code
  - issue
  - research
---

## Discovery

While testing Phase 3 deployment, discovered Claude Code v2.0.37 has **partial MCP implementation**:

✅ **Working:** MCP Resources
- `resources/list` and `resources/read` fully functional
- All 28 documentation files accessible
- `ListMcpResourcesTool` and `ReadMcpResourceTool` available

❌ **Not Working:** MCP Tools
- `tools/list` and `tools/call` not exposed to AI
- No `ListMcpToolsTool` available
- Nexus tools invisible despite correct MCP implementation

## Evidence

1. Nexus MCP server responds correctly to `tools/list` JSON-RPC
2. Claude Code shows `Status: ✓ Connected`
3. But AI assistant has no access to the 7 tools

## Root Cause

Claude Code MCP client limitation - not a Nexus bug.

## Investigation

- Searched Claude Code changelog - no mention of tools/list fix
- Found GitHub issue requesting `claude tools list` command
- No timeline for resolution

## Action Taken

Documented in KNOWN_ISSUES.md with REVISIT checklist. Implemented CLI workaround (Phase 3.5).
