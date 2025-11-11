---
id: fed323c3-1669-474c-b9f5-cb8e6ee57088
type: SolutionApplied
title: "Fixed FS3373 by extracting timestamp variable"
summary: "Extract DateTime.ToString() to variable before interpolation"
occurred_at: 2025-11-11T02:49:32.372-05:00
tags:
  - fsharp
  - solution
  - fs3373
  - strings
error_code: FS3373
pattern_name: interpolated-string-variable-extraction
pattern_category: Syntax
success_rate: 1
file_path: "Domain/Projections.fs"
conversation_context: "Phase 2 projection metadata"
---

## Solution

Extract the formatted value to a variable before using it in the interpolated string.

## Fixed Code (Success)

```fsharp
let timestamp = meta.LastRegenerated.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK")
content.AppendLine($"last_regenerated: {timestamp}") |> ignore
```

## Success Rate

100% - Applied 5 times, worked every time.

## Pattern

This is an instance of the `interpolated-string-variable-extraction` pattern.

