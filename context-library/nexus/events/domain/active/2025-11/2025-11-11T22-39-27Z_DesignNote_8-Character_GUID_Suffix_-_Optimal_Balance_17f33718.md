---
id: 5ca956a4-b81a-4ba8-9135-087e089dba70
type: DesignNote
title: "8-Character GUID Suffix - Optimal Balance"
summary: "8 chars provides uniqueness while maintaining filename readability"
occurred_at: 2025-11-11T17:39:27.309-05:00
tags:
  - design
  - guid
  - naming
  - phase-2
  - optimization
---

## Design Decision: Why 8 Characters?

### The Options Considered

**Full 32-char GUID:**
```
2025-11-11T22-31-02Z_TechnicalDecision_Test_d17ba665e4f84a3c91b2c8d5f3e7a9b1.md
                                           ↑ Too long, hard to read
```

**4-char suffix:**
```
2025-11-11T22-31-02Z_TechnicalDecision_Test_d17b.md
                                           ↑ Risk of collisions
```

**8-char suffix (chosen):**
```
2025-11-11T22-31-02Z_TechnicalDecision_Test_d17ba665.md
                                           ↑ Just right
```

### Why 8 Characters?

**Collision Probability:**
- 4 chars = 16^4 = 65,536 possibilities
- 8 chars = 16^8 = 4,294,967,296 possibilities (4.2 billion)
- For events created in same second, 8 chars provides safety

**Readability:**
- Still fits in terminal/file browser
- Distinguishable at a glance: d17ba665 vs 15a2441e
- Not overwhelming like full GUID

**Practical Testing:**
Created 3 events in same second:
- d17ba665 ✅ unique
- 15a2441e ✅ unique
- 26a94a3d ✅ unique

### Birthday Paradox Math

With 8-char hex GUID:
- Need ~65,000 events before 1% collision chance
- Nexus won't hit that in same second

Even if we create 1000 events/second:
- 8 chars provides 4.2M possibilities
- Collision risk negligible

### Implementation Detail
```fsharp
Guid.NewGuid().ToString("N").Substring(0, 8)
```
- "N" format = no hyphens (32 hex chars)
- Substring first 8 chars
- Lowercase hex

### Alternatives Rejected

**Full GUID in filename:**
- Too long for readability
- GUID already in YAML frontmatter (`id` field)
- Redundant

**Timestamp-only (no GUID):**
- Phase 1 approach
- Collisions possible in same second
- Risky for batch operations

### Confidence: High
8 chars is sweet spot between uniqueness and usability.
