---
id: f8408b5c-6e9b-447b-a8f1-0af5acec5422
type: FrameworkInsight
title: "Event-Sourced Testing Validates Entire Pipeline"
summary: "Creating test events proves the full event creation and projection workflow works"
occurred_at: 2025-11-11T17:04:08.249-05:00
tags:
  - testing
  - event-sourcing
  - integration-testing
  - phase-1
---

## Testing Insight

### The Approach:
Instead of unit testing individual components, create actual test events to validate the entire system:

1. **Create test events** via CLI for each new type
2. **Run timeline projection** to verify parsing
3. **Visual confirmation** in timeline output

### Why This Works:
- Tests the complete workflow: write → store → read → project
- Catches integration issues unit tests miss
- Natural for event-sourced systems
- Test events become documentation
- Provides immediate visual feedback

### Example from Phase 1:
```bash
nexus create-event --type MethodologyInsight --title "Test" --body "Testing"
nexus timeline-projection | tail -10
# See: - 2025-11-11 17:00:21 | MethodologyInsight | Test
```

### Benefit:
If the event appears in timeline with correct type, we know:
- Event type parsing works
- File writing works  
- Frontmatter format correct
- Projection reading works
- Timeline generation works

One test validates five components.
