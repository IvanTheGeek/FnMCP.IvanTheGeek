---
id: e3836a19-9e10-4a1c-b2d0-8432fc3cfff6
type: DesignNote
title: "F# Discriminated Union Extension Checklist"
summary: "Complete checklist for adding new cases to F# discriminated unions"
occurred_at: 2025-11-11T17:07:25.238-05:00
tags:
  - f-sharp
  - pattern
  - discriminated-unions
  - phase-1
---

## Pattern: Extending F# Discriminated Unions

When adding new cases to an F# discriminated union in this codebase, follow this exhaustive checklist:

### 1. Update the Union Type Definition
```fsharp
type EventType =
    | TechnicalDecision
    | DesignNote
    | MethodologyInsight  // NEW
    | NexusInsight        // NEW
```

### 2. Update the AsString Member
Add pattern match cases to convert union to string:
```fsharp
member this.AsString =
    match this with
    | TechnicalDecision -> "TechnicalDecision"
    | DesignNote -> "DesignNote"
    | MethodologyInsight -> "MethodologyInsight"  // NEW
    | NexusInsight -> "NexusInsight"              // NEW
```

### 3. Update the Parse Static Member
Add parsing logic with case-insensitive and underscore variants:
```fsharp
static member Parse(str: string) =
    match s.Trim().ToLowerInvariant() with
    | "technicaldecision" | "technical_decision" -> TechnicalDecision
    | "methodologyinsight" | "methodology_insight" -> MethodologyInsight  // NEW
    | "nexusinsight" | "nexus_insight" -> NexusInsight  // NEW
```

### 4. Update Tool Schema Descriptions
Find all MCP tool definitions that reference the union and update their descriptions:
```fsharp
description = "Event type: TechnicalDecision | DesignNote | MethodologyInsight | NexusInsight"
```

### Why This Works:
- F# compiler enforces exhaustive pattern matching
- Missing cases cause compilation errors, not runtime errors
- Type safety guarantees correctness
- No possibility of typos at runtime

### Confidence Level: **Very High**
Successfully used in Phase 1 with 4 new types added, zero compilation errors, zero runtime errors.
