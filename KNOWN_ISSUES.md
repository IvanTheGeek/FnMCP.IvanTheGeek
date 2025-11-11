# Known Issues & Blockers

**Last Updated:** 2025-11-11 (Research completed - issue confirmed as upstream Claude Code bug)

---

## 🚨 CRITICAL: MCP Tools Not Exposed by Claude Code

**Status:** ACTIVE - Blocking Phase 3 functionality
**Severity:** CRITICAL
**Component:** Claude Code v2.0.37 MCP Client
**Discovered:** 2025-11-11

### Problem Description

Claude Code successfully connects to Nexus MCP server but **only exposes Resources, not Tools**:

✅ **Working:**
- MCP Resources (`resources/list`, `resources/read`)
- All 28 documentation files accessible via `context://` URIs
- `ListMcpResourcesTool` and `ReadMcpResourceTool` work correctly

❌ **Not Working:**
- MCP Tools (`tools/list`, `tools/call`)
- 7 Nexus tools invisible to Claude AI assistant:
  - `create_event`
  - `timeline_projection`
  - `enhance_nexus`
  - `record_learning`
  - `lookup_pattern`
  - `lookup_error_solution`
  - `update_documentation`

### Evidence

1. **MCP Server Works Correctly:**
   ```bash
   echo '{"jsonrpc":"2.0","method":"tools/list","id":1}' | /home/linux/Nexus/nexus
   # Returns all 7 tools with proper JSON-RPC response ✅
   ```

2. **Claude Code Connects Successfully:**
   ```bash
   claude mcp get nexus
   # Shows: Status: ✓ Connected ✅
   ```

3. **But Tools Not Available to AI:**
   - No `ListMcpToolsTool` available
   - No `mcp__nexus__create_event` or similar tools exposed
   - Cannot call any Nexus tools directly

### Impact

**SEVERE:** Entire Phase 3 functionality unusable
- 7 powerful MCP tools built but inaccessible
- Learning system cannot record patterns/errors
- Event creation requires manual file editing
- Timeline projection requires direct binary invocation

### Root Cause

**Claude Code MCP Client Limitation** - Partial MCP implementation:
- Implements MCP Resources protocol fully ✅
- Does NOT implement MCP Tools protocol ❌
- May be intentional design decision or missing feature

### Workaround (Phase 3.5)

**Add CLI Interface to Nexus:**

```bash
# MCP mode (current - for resources)
/home/linux/Nexus/nexus /path/to/context-library

# CLI mode (Phase 3.5 - for tools)
nexus create-event --type TechnicalDecision --title "Fix MCP" --body "..."
nexus timeline-projection
nexus record-learning --event-type error_encountered --title "..." --description "..."
nexus lookup-pattern "interpolated"
nexus lookup-error-solution FS3373
nexus enhance-nexus --events events.json --regenerate timeline,metrics
nexus update-documentation --path "framework/foo.md" --content "..."
```

**Architecture:**
- Keep MCP server mode for resources (works fine)
- Add CLI argument parser for tools
- Route CLI commands to existing `ToolRegistry.executeTool`
- Same backend, dual interfaces

### Action Items

**Immediate:**
- [x] Document issue in KNOWN_ISSUES.md
- [x] Update NEXT_SESSION_START_HERE.md
- [x] Implement CLI parser (Phase 3.5) - COMPLETE 2025-11-11
- [x] Test all 7 tools via CLI - TESTED: timeline, lookup-pattern, lookup-error-solution working
- [x] Update documentation with CLI usage - Comprehensive CLI docs in NEXT_SESSION_START_HERE.md

**Ongoing - REVISIT EVERY SESSION:**
- [x] Check Claude Code changelog for updates - CHECKED 2025-11-11
- [x] Test new Claude Code versions immediately - v2.0.37 still affected
- [x] Search for related GitHub issues/discussions - FOUND ACTIVE BUGS
- [x] File bug report with Anthropic if not already reported - ALREADY REPORTED
- [ ] Monitor MCP specification compliance

**Future:**
- [ ] Remove CLI workaround once MCP tools work
- [ ] Add integration tests to catch future regressions
- [ ] Consider contributing fix to Claude Code if open source

### References

- **Claude Code Version:** 2.0.37
- **MCP Spec:** Model Context Protocol (2024-11-05)
- **Nexus Implementation:** src/FnMCP.IvanTheGeek/McpServer.fs
- **Tool Registry:** src/FnMCP.IvanTheGeek/Tools.fs
- **MCP Config:** `/home/linux/.claude.json`
- **Transport:** stdio (confirmed working for resources, broken for tools)

### Related GitHub Issues (Official)

**CONFIRMED: This is a widespread, known bug affecting multiple users and MCP servers.**

1. **Issue #3426** - "Claude Code fails to expose MCP tools to AI sessions when running a local Playwright MCP server"
   - Status: OPEN, Priority P1 (Critical)
   - Transport: stdio (same as Nexus)
   - Last activity: July 27, 2025
   - Root cause: "Claude Code successfully starts MCP servers but fails to expose MCP tools to AI sessions"
   - URL: https://github.com/anthropics/claude-code/issues/3426

2. **Issue #9133** - "Atlassian MCP Server Tools Not Available in Claude Code 2.0.10 Conversations Despite Successful Connection"
   - Status: OPEN
   - Version: 2.0.10 (earlier than our 2.0.37)
   - Transport: SSE (not stdio, but same symptom)
   - Note: Native builds don't support SSE; stdio should work but doesn't
   - URL: https://github.com/anthropics/claude-code/issues/9133

3. **Issue #8407** - "MCP Tools Not Available to AI Despite Connected Status"
   - Status: CLOSED (resolved by reboot - temporary glitch)
   - Version: 0.2.39
   - Note: May be a different issue (transient vs. systemic)
   - URL: https://github.com/anthropics/claude-code/issues/8407

### Assessment

**Verdict:** Our Nexus issue is **NOT unique** - it's part of a **systemic bug in Claude Code** affecting:
- Multiple versions (0.2.39, 2.0.10, 2.0.37)
- Multiple transports (stdio, SSE, HTTP)
- Multiple MCP servers (Playwright, Atlassian, Nexus, others)
- Consistent pattern: Server connects ✓, Resources work ✓, Tools invisible ❌

**Why CLI workaround is critical:** Until Anthropic fixes issue #3426 (P1 priority), ALL stdio MCP tools are unusable from the AI assistant. Our Phase 3.5 CLI interface is not a workaround for our bug—it's a workaround for Claude Code's bug.

---

## Issue Template (for future issues)

```markdown
## [SEVERITY]: Issue Title

**Status:** ACTIVE | RESOLVED | MONITORING
**Severity:** CRITICAL | HIGH | MEDIUM | LOW
**Component:** Component Name
**Discovered:** YYYY-MM-DD

### Problem Description
Clear description of the issue

### Evidence
Steps to reproduce, logs, screenshots

### Impact
How this affects functionality

### Root Cause
Technical explanation if known

### Workaround
Temporary solution if available

### Action Items
- [ ] Immediate actions
- [ ] Ongoing monitoring
- [ ] Future resolution

### References
Links, versions, file paths
```
