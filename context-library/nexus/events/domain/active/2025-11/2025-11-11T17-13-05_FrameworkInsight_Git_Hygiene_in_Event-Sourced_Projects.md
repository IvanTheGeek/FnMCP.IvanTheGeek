---
id: 4b0fa411-d819-44cd-bec8-106512c18f3e
type: FrameworkInsight
title: "Git Hygiene in Event-Sourced Projects"
summary: "Separate commits for code vs data in event-sourced systems"
occurred_at: 2025-11-11T17:13:05.883-05:00
tags:
  - git
  - event-sourcing
  - best-practices
  - code-review
---

## Insight: Code vs Data Commits

### Pattern:
In event-sourced systems like Nexus, maintain separation between code and data commits:

**Code Commits:**
- Changes to .fs, .fsproj, .sln files
- New features, bug fixes, refactoring
- Should be atomic and well-described
- Example: Phase 1 added 4 new event types (13 insertions, 1 deletion)

**Data Commits:**
- Event files (.md, .yaml in context-library/)
- System events, domain events
- Can be batched or committed separately
- Usually tracked but not critical to commit immediately

### Why This Matters:
1. **Code history is structural** - breaks the system if reverted incorrectly
2. **Data history is narrative** - can be regenerated or imported
3. **Code changes need review** - should be small, focused commits
4. **Data accumulates naturally** - events stream in continuously

### Git Strategy for Nexus:
- Commit code changes at phase boundaries
- Event files can be committed in batches
- Keep commits focused on one type of change
- Use descriptive messages for code commits

### Phase 1 Example:
```bash
# Code commit (what we did):
git add src/FnMCP.IvanTheGeek/Domain/Events.fs
git add src/FnMCP.IvanTheGeek/Tools/EventTools.fs
git commit -m "feat: Add 4 new event types..."

# Data commit (could do later):
git add context-library/nexus/events/domain/active/2025-11/*.md
git commit -m "docs: Add Phase 1 implementation events"
```

### Benefit:
Clear separation makes code review easier and rollback safer.
