---
id: 307e6f82-1241-4697-bd1e-37d45bb8e350
type: DesignNote
title: "User Preference: Auto-Commit After Code Completion"
summary: "Claude Code should create git commits when completing code changes to repository"
occurred_at: 2025-11-11T17:13:05.584-05:00
tags:
  - preference
  - git
  - workflow
  - automation
---

## User Preference

### Rule:
When Claude Code completes code changes to files in the repository, create a git commit with an appropriate title and message.

### When to Commit:
- After completing a feature or phase implementation
- After fixing a bug
- After refactoring that compiles and tests successfully
- When code changes are stable and tested

### When NOT to Commit:
- During active development/debugging
- When tests are failing
- When user explicitly asks not to commit
- For experimental/exploratory changes

### Commit Message Format:
```
<type>: <Short descriptive title>

<Body with details about what changed and why>

🤖 Generated with [Claude Code](https://claude.com/claude-code)

Co-Authored-By: Claude <noreply@anthropic.com>
```

### Types:
- feat: New feature
- fix: Bug fix
- refactor: Code restructuring
- docs: Documentation changes
- test: Test additions/changes
- chore: Build/tooling changes

### Example from Phase 1:
```
feat: Add 4 new event types to domain model (Phase 1)

Added MethodologyInsight, NexusInsight, SessionState, and CrossProjectIdea...
```

### Rationale:
- Creates clear history of changes
- Makes rollback easier
- Documents decision points
- Complements event-sourced data with code history
