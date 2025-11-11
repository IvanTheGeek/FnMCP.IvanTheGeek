---
id: 56c5a566-b3ac-4225-8fc7-71353d4121ec
type: PatternDiscovered
title: "Percent Sign Escaping in Interpolated Strings"
summary: "Escape % as %% in F# interpolated strings"
occurred_at: 2025-11-11T02:49:32.372-05:00
tags:
  - fsharp
  - syntax
  - strings
pattern_name: percent-sign-escaping
pattern_category: Syntax
use_count: 2
success_rate: 1
file_path: "Projections/Knowledge.fs"
conversation_context: "Phase 3 knowledge projections"
---

## Pattern

When using the `%` character in F# interpolated strings, it must be escaped as `%%`.

## Example

```fsharp
// ✗ Fails - F# interprets % as format specifier
$"Success rate: {pct}%"

// ✓ Works
$"Success rate: {pct}%%"
```

## Reason

F# uses `%` for format specifiers (printf-style), so literal percent signs must be escaped.

## Confidence

High - Used 2 times during Phase 3, 100% success rate.

