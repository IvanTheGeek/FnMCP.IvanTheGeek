---
id: e438c252-151e-40aa-97b9-9cd537452623
type: TechnicalDecision
title: "UTC Timestamp Standard for Event Storage"
summary: "All event timestamps stored in UTC with Z suffix; timezone conversion happens at display layer"
occurred_at: 2025-11-11T14:01:40.842-05:00
tags:
  - timestamps
  - utc
  - timezone
  - distributed-systems
technical_decision:
---

# Problem

Timestamps in local timezone create ambiguity:
- Daylight Saving Time transitions cause confusion
- Events from different timezones can't be properly ordered
- "2025-11-11T02:30:00" is ambiguous during DST fallback
- Cross-timezone collaboration becomes error-prone

# Decision

**All event storage uses UTC:**
- Filenames include UTC timestamp with 'Z' suffix
- Event frontmatter stores occurredAt in ISO 8601 UTC
- System events track operations in UTC
- No local timezone information in storage layer

```
Filename:
2025-11-11T15-23-45Z_TechnicalDecision_Title_{guid}.md
                   ^ Z indicates UTC

Frontmatter:
occurredAt: 2025-11-11T15:23:45Z
```

**Timezone conversions happen at display layer:**
- Timeline projections show local time when rendered
- User interfaces convert to user's timezone
- Storage remains timezone-agnostic

# Rationale

**Why UTC:**
- Unambiguous global reference
- No DST complications
- Sortable chronologically regardless of origin
- Industry standard for distributed systems

**Why Z suffix:**
- ISO 8601 standard notation for UTC
- Clear visual indicator in filenames
- Distinguishes from local timestamps

**Why convert at display:**
- Separation of concerns (storage vs presentation)
- Single source of truth
- Enables different users to see same event in their local time
- Simplifies event comparison and ordering

# Examples

**Storage (always UTC):**
```
Event created in EST (UTC-5):
Filename: 2025-11-11T20-30-00Z_Entry_Created_{guid}.md
Local time was: 3:30 PM EST
```

**Display (converts to user timezone):**
```
EST user sees: "Nov 11, 2025 3:30 PM EST"
PST user sees: "Nov 11, 2025 12:30 PM PST"
Both reference same UTC moment
```

# Consequences

**Positive:**
- ✅ Unambiguous timestamps across all contexts
- ✅ Proper chronological ordering guaranteed
- ✅ Cross-timezone collaboration simplified
- ✅ No DST-related bugs

**Negative:**
- ⚠️ Filenames show UTC, not local time (less immediately recognizable)
- ⚠️ Requires timezone conversion in display layer

**Mitigations:**
- Projections handle conversion transparently
- Users rarely interact with raw filenames
- Benefits far outweigh minor usability trade-off
