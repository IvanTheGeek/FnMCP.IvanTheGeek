---
id: 949fa5cf-e634-4f13-8300-5b2ac1f1d338
type: TechnicalDecision
title: "Phase 2: Add GUID to Event Naming (Code)"
occurred_at: 2025-11-11T15:21:07.559-05:00
tags:
  - phase-2
  - guid
  - naming
  - utc
  - code
---

# Phase 2: GUID-Based Event Naming

## Goal
Update event filename generation to include GUID suffix.

## Code Changes

### 1. Update filename generation
```fsharp
// Domain/EventWriter.fs
let generateFilename (eventType: EventType) (title: string) (timestamp: DateTime) =
    let guid = Guid.NewGuid().ToString("N") // 32 char, no hyphens
    let formattedTime = timestamp.ToString("yyyy-MM-ddTHH-mm-ss")
    let sanitizedTitle = sanitizeTitle title
    sprintf "%sZ_%s_%s_%s.md" formattedTime (eventType.ToString()) sanitizedTitle guid
```

### 2. Update timestamp to UTC
```fsharp
// Ensure all timestamps use UTC
let timestamp = DateTime.UtcNow
```

### 3. Update projection readers to handle GUID suffix
```fsharp
// Projections need to parse filenames with GUID
// Regex: (\d{4}-\d{2}-\d{2}T\d{2}-\d{2}-\d{2}Z)_(.+?)_(.+?)_([a-f0-9]{32})\.md
```

## Testing

1. Build project
2. Create test event:
   ```bash
   nexus create-event --type TechnicalDecision --title "GUID Test" --body "Testing GUID suffix"
   ```
3. Verify filename format: `2025-11-11T15-30-00Z_TechnicalDecision_GUID_Test_a1b2c3d4...32chars.md`
4. Verify timeline still displays correctly
5. Create multiple events quickly to verify no collisions

## Success Criteria
- ✅ Filenames include 32-char GUID
- ✅ UTC timestamp with Z suffix
- ✅ No filename collisions
- ✅ Projections parse correctly
- ✅ Timeline displays properly

## Rollback
Revert binary if parsing breaks projections.
