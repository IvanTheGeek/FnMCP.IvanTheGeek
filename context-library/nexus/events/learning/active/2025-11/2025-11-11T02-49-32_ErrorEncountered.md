---
id: 0dad21c5-8958-48d2-b457-be58c024f0b1
type: ErrorEncountered
title: "FS3373: Invalid Interpolated String"
summary: "Single quote or verbatim string literals not allowed in interpolated expressions"
occurred_at: 2025-11-11T02:49:32.372-05:00
tags:
  - fsharp
  - compilation
  - fs3373
  - strings
error_code: FS3373
error_message: "Invalid interpolated string. Single quote or verbatim string literals may not be used in interpolated expressions"
use_count: 5
file_path: "Domain/Projections.fs"
conversation_context: "Phase 2 projection metadata"
related_patterns:
  - interpolated-string-variable-extraction
---

## Context

Hit this error 5 times during Phase 2 and 3 implementation when formatting DateTime values in YAML frontmatter.

## Original Code (Failed)

```fsharp
content.AppendLine($"last_regenerated: {meta.LastRegenerated.ToString(\"yyyy-MM-dd'T'HH:mm:ss.fffK\")}") |> ignore
```

## Error Message

> Invalid interpolated string. Single quote or verbatim string literals may not be used in interpolated expressions

## Cause

The format string contains single quotes (`'T'`) which conflicts with F# interpolated string syntax.

