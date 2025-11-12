---
id: 684ab049-ab82-4c12-99cf-d4eb98f8058e
type: FrameworkInsight
title: "MCP Prompts Enable Zero-Context Session Continuation"
occurred_at: 2025-11-11T18:59:35.782-05:00
---

## Insight

MCP prompts capability allows Claude to resume conversations with full context automatically, without manually re-reading documentation.

## Implementation Pattern

```
1. User creates SessionState events during work
2. MCP server exposes project-specific prompts
3. Claude Desktop shows prompts in UI
4. User clicks prompt → server reads latest SessionState
5. Context injected as user message
```

## Why This Matters

**Before:** User manually pastes context or re-reads docs (1000+ tokens)
**After:** Click prompt, context auto-loaded from latest SessionState event (~500 tokens)

## Token Savings

- Previous session context: Embedded in SessionState event
- No need to re-read framework docs
- No need to explain what you were working on
- Target: 50%+ token reduction per new session

## Architecture Benefits

- SessionState events are reusable (can be read by tools, projections, prompts)
- Project-scoped: Each project has its own continuation
- Versioned: Historical SessionStates preserved
- Self-documenting: Context is already captured as events

## Next Evolution

Phase 5 will add CrossProjectIdea events that surface in continuation context, creating cross-pollination between projects.
