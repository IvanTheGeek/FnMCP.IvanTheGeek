---
id: 7d21d3d9-f465-487e-99cc-c9ac001e7d37
type: PatternDiscovered
title: "CLI argument normalization pattern"
occurred_at: 2025-11-11T11:31:11.552-05:00
tags:
  - cli
  - patterns
  - testing
pattern_name: cli-argument-normalization
pattern_category: Architecture
---

## Pattern
When building CLI tools, normalize argument names to support both hyphens and underscores for better user experience.

## Implementation
```fsharp
let normalizeArgName (name: string) =
    name.Replace('-', '_').ToLowerInvariant()
```

## Benefits
- Users can use --arg-name or --arg_name
- More forgiving CLI interface
- Follows Unix conventions
