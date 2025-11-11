---
id: 9b6aebc5-d800-4307-b97a-f2f85fdacfff
type: PatternDiscovered
title: "Interpolated String Variable Extraction"
summary: "Extract complex expressions to variables before using in interpolated strings"
occurred_at: 2025-11-11T02:49:32.349-05:00
tags:
  - fsharp
  - syntax
  - strings
  - compilation
pattern_name: interpolated-string-variable-extraction
pattern_category: Syntax
use_count: 8
success_rate: 1
file_path: "Domain/Projections.fs"
conversation_context: "Phase 2 & 3 implementation"
---

## Pattern

When interpolated strings contain complex expressions (especially with format strings containing special characters like single quotes), extract the formatted value to a variable first.

## Example

```fsharp
// ✗ Fails with FS3373
$"timestamp: {dt.ToString(\"yyyy-MM-dd'T'HH:mm:ss\")}"

// ✓ Works
let timestamp = dt.ToString("yyyy-MM-dd'T'HH:mm:ss")
$"timestamp: {timestamp}"
```

## Reason

F# interpolated strings don't support format strings with special characters directly in the interpolation expression.

## Confidence

High - Used 8+ times during Phase 2/3, 100% success rate.

