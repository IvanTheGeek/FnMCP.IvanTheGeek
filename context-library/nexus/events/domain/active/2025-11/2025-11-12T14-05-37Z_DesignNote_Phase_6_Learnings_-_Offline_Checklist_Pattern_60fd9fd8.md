---
id: eb263a4d-5f82-4087-8814-716e75c0a8cd
type: DesignNote
title: "Phase 6 Learnings - Offline Checklist Pattern"
occurred_at: 2025-11-12T09:05:37.612-05:00
tags:
  - workflow
  - user-experience
  - documentation
  - offline-work
---

## Pattern: Offline Completion Checklist

**Context:** Phase 6 required steps outside Claude (Rider, filesystem, Claude Desktop restart).

**Solution:** Created downloadable markdown checklist with:
- Sequential numbered steps
- Checkboxes for progress tracking
- Verification points at each step
- Code snippets ready to copy/paste
- Troubleshooting section
- Success criteria

**User workflow:**
1. Claude generates comprehensive checklist
2. User downloads and works offline
3. User reports back when complete
4. Conversation resumes with next steps

## Benefits

**For user:**
- Work at own pace offline
- Clear verification at each step
- Troubleshooting available immediately
- Can reference while working in other tools
- No need to keep Claude window open

**For conversation efficiency:**
- Reduces back-and-forth for simple confirmations
- User can complete entire phase independently
- Conversation focuses on decisions, not "what's next?"
- Token savings from fewer intermediate messages

## When to Use This Pattern

**Good for:**
- Multi-step procedures in external tools (IDE, terminal, config files)
- Tasks requiring application restarts
- Filesystem operations
- Deployment procedures
- Testing sequences

**Not needed for:**
- Single tool calls
- Quick questions
- Decisions requiring input
- Work Claude can do directly (MCP tools, documentation)

## Implementation Template

```markdown
# [Phase/Task Name] - Completion Checklist

## Part 1: [Section Name]

### Step X: [Action]
- [ ] Specific action item
- [ ] Verification point
- [ ] Success criteria

**Code to run:**
```bash
# Commands here
```

**Verify:**
- [ ] Check this
- [ ] Confirm that

---

## Troubleshooting

**Problem: [Common issue]**
- Solution steps
```

## Application to Other Phases

This pattern could be valuable for:
- Framework setup procedures
- Deployment checklists
- Testing workflows
- Configuration migrations
- Database setup sequences

Consider using whenever user needs to complete connected steps outside Claude.
