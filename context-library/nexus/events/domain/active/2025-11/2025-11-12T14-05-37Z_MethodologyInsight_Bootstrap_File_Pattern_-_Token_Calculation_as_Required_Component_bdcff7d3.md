---
id: a8644716-42c6-4347-bc3f-ea7a086be8d1
type: MethodologyInsight
title: "Bootstrap File Pattern - Token Calculation as Required Component"
occurred_at: 2025-11-12T09:05:37.611-05:00
tags:
  - bootstrap-pattern
  - methodology
  - standards
  - token-display
---

## Pattern Evolution

**Original bootstrap concept:** Minimal file pointing to MCP documentation.

**Problem discovered:** Critical display patterns failed in new chats despite being documented.

**Solution:** Bootstrap files must include not just pointers but also critical patterns that need to work from message #1.

## Bootstrap Template Standard (Updated)

Every bootstrap file MUST include these components:

1. **Project identification and scope**
   - What project is this?
   - What's the current status?

2. **How to start conversations**
   - Quick continuation (MCP prompts)
   - Manual continuation (tools to use)

3. **Project structure and file paths**
   - Where to find documentation via MCP
   - Clear organization of available resources

4. **Token calculation formula with verification examples** ← CRITICAL NEW REQUIREMENT
   - Full formula with clear variable names
   - 3+ verification examples with step-by-step math
   - Reference to detailed spec for complete format

5. **Context management explanation**
   - What loads vs. what's accessed on-demand
   - Token savings achieved

6. **Event sourcing location**
   - Where events are stored
   - How to access timeline

## Why Token Calculation Belongs in Bootstrap

**Timing requirement:** Token display must work correctly from first response in new chat.

**Frequency:** Every single response requires correct calculation.

**Cost:** Formula + examples = ~100 tokens, which is negligible in ~500 token bootstrap.

**Alternative approaches failed:**
- Spec file: Not always loaded
- Memory/preferences: Only works for non-project chats
- Documentation: Requires explicit access

**Bootstrap is correct location because:**
- Loads automatically in every project chat
- Zero additional cost (already loading bootstrap)
- Available from conversation start
- Self-contained (no external lookups needed)

## Implementation Guidelines

**When creating new project bootstrap files:**
1. Copy template from project-creation guide
2. Verify token calculation section is present
3. Test in completely new chat
4. Confirm first message displays bar correctly

**Tool design philosophy:**
- Keep update_documentation tool simple (no logic)
- Pattern documented in guides (single source of truth)
- Human/Claude follows template from guide
- Don't add pattern enforcement to tool code

## Pattern Validation

**Tested on:**
- Nexus project (framework + apps)
- PerDiem project (single app)

**Results:**
- Both projects load ~500 token bootstrap
- Token calculation works from first message
- Pattern scales to any new project

**Ready for:** LaundryLog graduation, future Cheddar apps
