---
id: 855938c7-29c6-4f51-a31f-35e427982f18
type: ResearchFinding
title: "Claude Code MCP Tools Bug Confirmed as Upstream Issue"
summary: "Confirmed MCP tools issue is upstream Claude Code bug (#3426), not Nexus issue"
occurred_at: 2025-11-11T11:19:07.367-05:00
tags:
  - claude-code
  - mcp
  - debugging
  - research
  - phase-3.5
---

## Investigation Summary

Confirmed that Nexus MCP tools not being exposed is NOT a Nexus bug, but a **widespread, critical bug in Claude Code** affecting multiple users and MCP servers.

## Evidence

### GitHub Issues Found
1. **Issue #3426** (P1 Critical, OPEN) - stdio MCP servers fail to expose tools
2. **Issue #9133** (OPEN) - SSE servers not supported in native builds
3. **Issue #8407** (CLOSED) - Transient connection issue (reboot fixed)

### Affected Versions
- 0.2.39, 2.0.10, 2.0.37 (our version)
- All versions show same pattern

### Affected Transports
- stdio (Nexus, Playwright)
- SSE (Atlassian)
- HTTP (various)

### Consistent Pattern
- MCP server connects successfully ✓
- Resources exposed and accessible ✓
- Tools completely invisible to AI assistant ❌

## Root Cause

Claude Code issue #3426 documents: "Claude Code successfully starts MCP servers but fails to expose MCP tools to AI sessions"

The client never executes the MCP specification-required `tools/list` request, making all integrated tools unavailable regardless of transport type or configuration.

## Impact on Nexus

Our Phase 3.5 CLI workaround is **essential and justified** - it's not working around our bug, but working around Claude Code's bug.

The 7 MCP tools we built are correctly implemented. They work via direct JSON-RPC calls. The issue is purely in Claude Code's AI assistant integration layer.

## Next Steps

1. Monitor issue #3426 for updates (P1 priority suggests active work)
2. Keep CLI interface until upstream fix
3. Test new Claude Code versions immediately when released
4. Remove CLI workaround once tools are properly exposed
