---
id: 42761e70-c32f-452d-b1aa-60388e52f3a6
type: DesignNote
title: "Cross-Project Idea Capture Mechanism"
summary: "Hybrid cross-project idea capture: manual + auto-detect with confirmation, stored as events with pending-ideas projection"
occurred_at: 2025-11-11T15:13:48.808-05:00
tags:
  - cross-project
  - idea-capture
  - continuation
  - workflow
---

# Problem

Working across multiple projects, ideas emerge for other projects but get lost:
- In LaundryLog work, realize PerDiemLog could use same pattern
- In Nexus work, think of LaundryLog improvement
- No mechanism to capture cross-project insights
- Ideas forgotten when switching projects

# Solution: Cross-Project Idea Capture

## Hybrid Capture Mechanism

**Manual capture:**
```
User: "Capture idea for PerDiemLog: [idea]"
→ Claude creates CrossProjectIdea event immediately
```

**Auto-detect + confirm:**
```
User mentions other project in context
→ Claude: "That sounds like an idea for X. Capture it?"
→ User confirms
→ Claude creates CrossProjectIdea event
```

## Event Structure

```yaml
type: CrossProjectIdea
source_project: laundrylog
target_project: perdiem
created_during: "GPS integration work"
idea: |
  The location picker with GPS + manual fallback
  would work perfectly for trip tracking.
  Consider sharing the component.
priority: consider | important | low
status: pending | exploring | implemented | rejected
context_link: [session/event that sparked idea]
```

## Projection: Pending Ideas

**Per-project projection:**
```
projections/perdiem/pending-ideas.md

Generated from: All CrossProjectIdea events where target=perdiem
Regenerable: Yes
Grouped by: source_project, priority, status
```

**Example output:**
```markdown
# Pending Ideas for PerDiemLog

## From LaundryLog (2 ideas)
### High Priority
- **Location picker component reuse**
  Captured: 2025-11-11 during GPS work
  Status: Pending exploration
  
### Consider
- **Validation pattern for date ranges**
  Captured: 2025-11-10 during entry validation
```

## Integration with Continuation

**When switching to PerDiemLog:**
```
User: "continue chat"

Continuation projection checks:
1. Latest SessionState (current work)
2. Pending ideas (CrossProjectIdea events)
3. Recent chat history

Returns:
"Welcome back to PerDiemLog. You were designing trip entry.
You also have 2 pending ideas from other projects:
- Location picker reuse (from LaundryLog)
- Date validation pattern (from LaundryLog)
Want to explore any of these?"
```

## Workflow Example

**Day 1: LaundryLog work**
```
You: "The GPS location picker we built is solid. 
     This exact pattern would work for PerDiemLog 
     trip start/end locations."

Claude: "That sounds like a cross-project idea for 
        PerDiemLog. Want me to capture it?"

You: "Yes"

Claude: [Creates CrossProjectIdea event in events/perdiem/]
"Captured! It'll show when you work on PerDiemLog next."
```

**Day 3: Switch to PerDiemLog**
```
You: "continue chat"

Claude: "Welcome back to PerDiemLog. You have a pending 
        idea from LaundryLog: reuse the location picker 
        component. The pattern handles GPS + manual fallback.
        Want to explore this now?"

You: "Yes, let's integrate it"

Claude: [Reads CrossProjectIdea event for full context]
        [Loads LaundryLog location picker implementation]
"Great! The LaundryLog picker has three key parts..."
```

## Event Sourcing Benefits

**Events provide:**
- Full context of when/why idea emerged
- Link back to originating work session
- Audit trail of cross-project learning
- Reproducible projections

**Projections provide:**
- Quick summary for continuation
- Filtered views (by priority, status)
- Token-efficient loading
- Regenerable as logic improves

## Status Lifecycle

```
pending → exploring → implemented | rejected

pending: Captured, not yet worked on
exploring: Actively investigating feasibility
implemented: Idea realized in target project
rejected: Decided not to pursue (with rationale)
```

Status updates create new events tracking decision.

# Benefits

**Cognitive offload:**
- ✅ Don't lose ideas when switching context
- ✅ Ideas surface at relevant time
- ✅ No manual tracking needed

**Knowledge transfer:**
- ✅ Patterns propagate across projects
- ✅ Learn from one project, apply to others
- ✅ Build shared component library organically

**Token efficiency:**
- ✅ Projection for quick continuation (~200 tokens)
- ✅ Events for deep context when needed
- ✅ No re-explaining cross-project connections

**Event history:**
- ✅ Timeline shows knowledge flow between projects
- ✅ Track which ideas were most valuable
- ✅ See evolution of shared patterns
