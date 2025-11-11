# FnMCP.IvanTheGeek Framework

## Core Philosophy

This is Ivan's personal development framework focused on:
- Event Modeling methodology
- F# + Bolero for web apps
- Penpot for design
- Mobile-first principles
- Privacy-first approach

**Guiding principle:** Maximum freedom + practical capitalism. Build for yourself first, then share. If you wouldn't use it yourself, don't build it.

## Current Focus

Building LaundryLog - mobile expense tracker for truck drivers.

## Technical Stack

- F# with Bolero (web framework)
- Event Modeling (visual specification)
- Penpot (UI source of truth)
- MCP Protocol (tool integration)
- SQLite (local-first storage)

## Key Concepts

**Nexus:** The integrated development context system capturing methodology, knowledge, preferences, and philosophy.

**Paths:** Complete execution traces through the system using concrete example data. Each path is simultaneously a narrative, test case, documentation, and Penpot prototype.

**Static-State Design:** Each screen represents exactly one state. No hidden modes. Navigation is explicit, validation is visual.

**Cheddar Ecosystem:** Suite of financial tools for independent workers (LaundryLog, PerDiemLog, CheddarBooks).

---

## Token Monitoring Specification

**CRITICAL: Display this format after EVERY response:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 Context Usage: [████████░░░░░░░░░░░░] 85,000 / 190,000 tokens (44.7%)

Allocation:
├─ System Prompts:     ~5,000 tokens  (2.6%)
├─ Project Knowledge:  ~2,000 tokens  (1.1%)
├─ Conversation:      ~75,000 tokens (39.5%)
│  ├─ Your messages:  ~12,000 tokens
│  ├─ My responses:   ~58,000 tokens
│  └─ Tool calls:      ~5,000 tokens
└─ This Response:      ~3,000 tokens  (1.6%)

Remaining: 105,000 tokens (55.3%) ✓ Comfortable

Status Legend: ✓ Comfortable (0-75%) | ⚠ Moderate (75-85%) | 🔴 High (85%+)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

**Format Requirements:**
- Line separators: `━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━` (35 chars)
- Visual bar: 20 characters total (each = 5%)
  - `█` for used tokens
  - `░` for remaining tokens
- Tree structure: `├─` `└─` `│` for hierarchy
- Status indicators: ✓ ⚠ 🔴
- Always show: total used, total available, percentage, remaining

**Purpose:** Monitor context consumption to enable proactive management and efficient Nexus operation.

See `technical/context-monitoring.md` for complete specification.