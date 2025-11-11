---
id: 2dbb9766-2873-4ee1-b03e-a8e49da19b8e
type: TechnicalDecision
title: "GUID-Based Event Naming for Distributed Uniqueness"
summary: "Event filenames now include full GUID suffix to ensure uniqueness in distributed/concurrent scenarios"
occurred_at: 2025-11-11T14:01:21.854-05:00
tags:
  - distributed-systems
  - event-naming
  - guid
  - concurrency
technical_decision:
---

# Problem

In distributed or concurrent scenarios, multiple Nexus instances could create events simultaneously:
- Co-drivers both logging laundry at same time
- Multiple developers working on same project
- Offline event creation with later synchronization

Without unique identifiers, events with identical timestamps and titles would overwrite each other.

# Decision

Append full GUID (32 chars, no hyphens) to event filenames:

```
Format:
{timestamp}Z_{EventType}_{Title}_{guid}.md

Example:
2025-11-11T15-23-45Z_TechnicalDecision_GPS_Integration_a1b2c3d4e5f67890abcdef1234567890.md

Implementation:
Guid.NewGuid().ToString("N")
```

# Rationale

**Why GUID:**
- Built-in to .NET (no external dependencies)
- Cryptographically random (collision probability negligible)
- Universal uniqueness across instances

**Why full GUID (32 chars):**
- Guaranteed uniqueness (vs 8-char truncation)
- Standard format, well-understood
- Acceptable trade-off: filename length vs. collision risk

**Why no hyphens:**
- Cleaner filenames (hyphens already used as separators)
- Still easily identifiable as GUID
- Format("N") is standard .NET GUID representation

# Use Cases Enabled

**Concurrent Operations:**
```
Driver A creates: 2025-11-11T15-30-00Z_Entry_Dryer_{guid-A}.md
Driver B creates: 2025-11-11T15-30-00Z_Entry_Dryer_{guid-B}.md
→ No collision, both events preserved
```

**Offline + Sync:**
```
Laptop creates event offline: {guid-1}
Desktop creates event offline: {guid-2}
Both sync later → No conflicts
```

**Instance Traceability:**
```
GUID maps to instance ID (future feature)
Can trace which device/user created event
Enables distributed debugging
```

# Consequences

**Positive:**
- ✅ Eliminates filename collisions
- ✅ Supports distributed, offline-first workflows
- ✅ No external dependencies
- ✅ Enables future instance tracking

**Negative:**
- ⚠️ Longer filenames (extra 33 chars)
- ⚠️ Less human-readable at a glance

**Mitigations:**
- Projections display human-readable summaries
- Filenames still sort chronologically (timestamp first)
- GUID position at end maintains readability of meaningful parts
