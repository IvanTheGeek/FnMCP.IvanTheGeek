---
id: b09acc64-cb64-4229-8d8d-a1415f3f2810
type: DesignNote
title: "CLI Testing Pattern - Use Flags Not Positional Args"
summary: "Nexus CLI requires --flag value format, not positional arguments"
occurred_at: 2025-11-11T17:04:07.867-05:00
tags:
  - cli
  - lesson-learned
  - testing
---

## Lesson Learned

### The Issue:
Initially tried CLI commands with positional args:
```bash
nexus create-event MethodologyInsight "Title" "Body"  # WRONG
```

This failed with: "The given key was not present in the dictionary"

### The Solution:
Use flag-based arguments:
```bash
nexus create-event --type MethodologyInsight --title "Title" --body "Body"  # CORRECT
```

### Why:
Program.fs parseCliArgs() expects:
- Flags starting with --
- Format: --key value
- Converts hyphens to underscores
- Maps to JSON properties

### Pattern for Future:
Always use `--flag value` format for Nexus CLI commands.
