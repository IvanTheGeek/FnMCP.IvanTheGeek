---
id: 9bda01db-f34d-46f6-b4f4-1f119d483698
type: ErrorEncountered
title: "FS0001 - Type mismatch in function application"
occurred_at: 2025-11-11T11:30:55.702-05:00
tags:
  - types
  - testing
error_code: FS0001
pattern_category: Types
---

## Context
Attempted to pass a string to a function expecting an integer.

## Error Message
```
error FS0001: This expression was expected to have type 'int' but here has type 'string'
```

## Solution
Convert the string using Int32.Parse or use proper type from the start.
