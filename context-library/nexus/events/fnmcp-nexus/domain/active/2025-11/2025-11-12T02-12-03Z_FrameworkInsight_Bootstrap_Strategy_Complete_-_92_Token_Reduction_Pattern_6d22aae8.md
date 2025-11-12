---
id: 96ab7cc5-7fcc-437f-ad84-0099b3e5e7f7
type: FrameworkInsight
title: "Bootstrap Strategy Complete - 92% Token Reduction Pattern"
summary: "Complete bootstrap strategy with implementation pattern and success metrics"
occurred_at: 2025-11-11T21:12:03.730-05:00
tags:
  - bootstrap
  - token-efficiency
  - architecture
  - pattern
  - implementation
---

# Complete Bootstrap Strategy

The bootstrap file pattern solves the token efficiency problem for Claude Projects while maintaining full access to detailed documentation.

## The Problem

**Before bootstrap:**
- Project Knowledge: 6,000 tokens of static documentation
- Loaded into every conversation automatically
- Reduces conversation capacity by 6,000 tokens
- Must update manually when docs change
- Same context whether needed or not

## The Solution

**Single bootstrap file (~500 tokens):**
```markdown
# {ProjectName} Development Project

**MCP Server:** FnMCP.Nexus (connected to Claude Desktop)
**Project Scope:** {project-specific areas}

## How to Start Any Conversation

### Quick Continuation
Click MCP prompts: continue-{project}

### Manual Continuation
1. Check recent work: recent_chats(n=3)
2. Load timeline: timeline_projection
3. Load docs: Use Nexus MCP to read relevant files

## Project Structure (via MCP)
- framework/* - Methodology docs
- apps/{project}/* - App-specific docs
- technical/* - Technical references

## Token Display Requirement
See technical/context-monitoring.md

## Event Sourcing
events/{project}/ - All project events
Use timeline_projection to see history

---
Everything else loaded on-demand via MCP!
```

## Token Savings

**Nexus Project:**
- Before: 6,000 tokens
- After: 500 tokens
- **Savings: 5,500 tokens (92%)**

**PerDiem Project (separate):**
- Only loads: core framework + PerDiem docs
- Doesn't load: LaundryLog or Nexus-specific content
- **Additional savings: ~4,000 tokens (50%)**

## Implementation Pattern

### Phase 1: Update Existing Project
1. Create bootstrap file
2. Remove old Project Knowledge files
3. Upload bootstrap only
4. Test in new chat
5. Verify token savings
6. Document in events

**Guide:** `technical/project-configuration-nexus.md`

### Phase 2: Create Separate Project
1. Create new Claude Project
2. Create project-specific bootstrap
3. Upload to new project
4. MCP server automatically available (global)
5. Verify project isolation
6. Test continuation prompts

**Guide:** `technical/project-creation-perdiem.md`

## Key Principles

**Bootstrap teaches Claude:**
- ✅ How to check context (recent_chats, timeline)
- ✅ How to load docs (MCP file paths)
- ✅ What tools are available (continuation prompts)
- ✅ Where events are stored (project scope)
- ✅ Critical requirements (token display format)

**Bootstrap does NOT contain:**
- ❌ Detailed methodology
- ❌ Complete specifications
- ❌ Historical decisions
- ❌ Implementation guides
- ❌ Current status details

All detail accessed on-demand via MCP.

## Project Separation Strategy

**Keep together (Nexus project):**
- Framework development
- FnMCP.Nexus server development
- LaundryLog dogfooding
- *Reason:* Tight feedback loops during active development

**Separate when ready:**
- PerDiem (test case for pattern)
- Future apps after patterns stabilize
- *Reason:* Token efficiency, focused context

## Success Metrics

**Immediate:**
- ✅ 92% token reduction in Project Knowledge
- ✅ Same access to all documentation
- ✅ Faster conversation startup
- ✅ Easy to update docs (just save file via MCP)

**Long-term:**
- ✅ Scales to unlimited documentation
- ✅ Clean project separation
- ✅ Reusable pattern for all apps
- ✅ Self-documenting through events
