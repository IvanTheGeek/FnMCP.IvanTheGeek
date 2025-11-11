---
id: 6f5da785-f5b9-4f1e-803a-75aafa9947fe
type: MethodologyInsight
title: "Preference Establishment Workflow Pattern"
summary: "Three-step process: User states preference → Document as event → Claude adopts behavior"
occurred_at: 2025-11-11T17:21:42.044-05:00
tags:
  - methodology
  - preferences
  - workflow
  - documentation
  - event-sourcing
---

## MethodologyInsight: How Preferences Become Behavior

### The Pattern That Emerged
From this session, a clear workflow for establishing user preferences:

**Step 1: User States Preference**
> "I would like you (Claude Code) to do git commits when you complete changes to code"

**Step 2: Immediate Implementation**
- Claude creates git commit demonstrating the pattern
- Shows user what it looks like in practice

**Step 3: Document as Events**
Three types of events created:
1. **DesignNote**: Documents the preference itself
   - When to commit, when not to commit
   - Commit message format
   - Examples

2. **FrameworkInsight**: Documents the "why"
   - Git hygiene in event-sourced projects
   - Code vs data commits
   - Rollback strategy

3. **FrameworkInsight**: Documents the integration
   - Commits as phase checkpoints
   - Integration with master plan
   - Future implications

**Step 4: Behavior Adoption**
- Preference now persists in event store
- Future Claude sessions can read and adopt
- Behavior becomes standard practice

### Why This Works

**Immediate Feedback:**
- User sees preference in action right away
- Can correct or refine if needed

**Rich Documentation:**
- Not just "do X" but "why do X"
- Includes examples and context
- Covers edge cases

**Event-Sourced Preferences:**
- Preferences are immutable events
- Can track evolution over time
- Easy to query and review

### Comparison to Traditional Approaches

**Config File Approach:**

- No context
- No examples
- No "why"

**Nexus Approach:**
- 3 events with rich narrative
- Examples from actual use
- Rationale documented
- Use cases explained
- Integration with system documented

### Confidence
Successfully established 2 preferences this session:
1. Auto-commit after code changes
2. Save session summaries as events

Both are now documented and adoptable.
