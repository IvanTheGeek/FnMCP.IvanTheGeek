---
id: 095cfc84-ded7-43b6-ac5b-ab1012305ac3
type: DesignNote
title: "Continuation Mechanism - Hybrid MCP Prompt + Keyword Approach"
summary: "Hybrid continuation mechanism: MCP prompt (primary) + \"continue chat\" keyword (fallback) + SessionState events (data)"
occurred_at: 2025-11-11T14:41:32.416-05:00
tags:
  - continuation
  - session-state
  - ux
  - mcp-prompt
  - planning
---

# Problem

New chats start with no context from previous sessions. Users must:
- Re-explain what they were working on
- Provide background context
- Waste tokens getting AI up to speed
- Copy/paste from previous chats

This breaks workflow continuity and makes Nexus feel disconnected across sessions.

# Proposed Solution: Hybrid Continuation Mechanism

## Layer 1: MCP Prompt (Primary)
```
Prompt name: "Continue from last session"
User clicks in UI → MCP returns context
```

## Layer 2: Keyword Fallback
```
User types: "continue chat"
Claude recognizes → Calls tools manually
```

## Layer 3: SessionState Events (Data)
```
Both approaches read from:
- Latest SessionState events per project
- Recent chat history
- Relevant project events
```

# Implementation Components

## A) SessionState Event Type
```fsharp
type EventType =
    | SessionState  // NEW
    | TechnicalDecision
    | DesignNote
    // ... etc
```

**SessionState captures:**
- Current task/feature being worked on
- Status (in_progress, blocked, completed)
- Next steps
- Blockers/questions
- Relevant files/events
- Last updated timestamp

## B) MCP Prompt
```fsharp
prompt "continue_session" {
    description = "Continue from where you left off"
    arguments = [
        { name = "project"; required = false }
    ]
}
```

**Returns:**
- Latest SessionState event
- Recent chat URIs (last 3-5)
- Recent project events
- Raw data for Claude to synthesize

## C) Keyword Recognition
**Trigger phrases:**
- "continue chat"
- "resume"
- "continue from last session"

**Claude behavior:**
1. Recognizes continuation request
2. Calls recent_chats(n=5)
3. Calls conversation_search for project context
4. Reads latest SessionState events
5. Synthesizes: "You were working on X. Current status: Y. Next: Z."

# Workflow Example

**End of productive session:**
```
User (or Claude): "Let's save our progress"
→ Creates SessionState event:
  - Task: "LaundryLog GPS integration"
  - Status: in_progress
  - Next: "Test GPS fallback on mobile"
  - Files: [gps-integration.md, events/...]
```

**New session (next day):**
```
User clicks: "Continue from last session" (MCP prompt)
OR
User types: "continue chat"

→ MCP/Tools return context
→ Claude: "Welcome back! You were implementing GPS 
   integration for LaundryLog. You completed the service 
   layer and need to test the fallback mechanism on mobile. 
   Ready to continue?"
```

# Benefits

**Token Efficiency:**
- ✅ No re-explaining context (saves ~2K tokens per chat)
- ✅ Focused continuation (load only relevant context)
- ✅ Minimal initial prompt needed

**User Experience:**
- ✅ Seamless continuation across sessions
- ✅ Works in both Claude Desktop and Code
- ✅ Natural language + UI option
- ✅ Always knows where you left off

**Data Quality:**
- ✅ SessionState events create audit trail
- ✅ Timeline shows progress over time
- ✅ Multiple paths through work visible

# Open Questions

1. **Auto-save SessionState?** End of every long conversation, or manual?
2. **Project inference?** Auto-detect which project based on chat content?
3. **Multiple tasks?** Support session with multiple parallel work items?
4. **Timing:** Implement now or after CQRS restructure?

# Status

**Planning phase** - Design agreed upon, implementation pending full system restructure.
