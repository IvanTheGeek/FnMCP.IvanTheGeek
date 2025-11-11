---
id: b258bc49-3a45-4f9d-b63b-f66a65b2a464
type: SolutionApplied
title: "Fixed type mismatch with explicit conversion"
occurred_at: 2025-11-11T11:31:03.321-05:00
tags:
  - types
  - solution
  - testing
error_code: FS0001
pattern_category: Types
---

## Problem
Function expected int but received string

## Solution Applied
```fsharp
let value = Int32.Parse(stringValue)
processNumber value
```

## Outcome
Compilation succeeded, function worked correctly
