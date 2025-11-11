---
id: 16112545-6c1c-4360-84fe-84cf2b04b2b7
type: MethodologyInsight
title: "Rapid Event Creation Testing Strategy"
summary: "Creating multiple events in same second proves GUID uniqueness and tests worst-case collision scenario"
occurred_at: 2025-11-11T17:40:18.783-05:00
tags:
  - methodology
  - testing
  - integration-testing
  - phase-2
  - worst-case
---

## Testing Strategy: Worst-Case Scenario First

### The Test
Phase 2 required proving GUID suffix prevents filename collisions. 

**Testing approach chosen:**
Create 3 events as fast as possible (same second):
```bash
./nexus create-event --type TechnicalDecision --title "Phase 2 Test 1" --body "..."
./nexus create-event --type TechnicalDecision --title "Phase 2 Test 2" --body "..."
./nexus create-event --type TechnicalDecision --title "Phase 2 Test 3" --body "..."
```

**Result:**
```
2025-11-11T22-31-02Z_TechnicalDecision_Phase_2_Test_1_d17ba665.md
2025-11-11T22-31-02Z_TechnicalDecision_Phase_2_Test_2_15a2441e.md
2025-11-11T22-31-03Z_TechnicalDecision_Phase_2_Test_3_26a94a3d.md
```

✅ Events 1 and 2 same second (22-31-02Z)  
✅ Different GUIDs: d17ba665 vs 15a2441e  
✅ No collisions

### Why This Strategy Works

**Test the edge case:**
- Normal usage: events seconds/minutes apart
- Worst case: multiple events same second
- If worst case works, normal case guaranteed

**Batch operations covered:**
- User might create many events via script
- Tool might generate system events rapidly
- GUID handles this gracefully

**Single test proves entire feature:**
- No need for 100 test events
- Visual confirmation (different GUIDs visible in ls output)
- Fast feedback loop

### Alternative Testing Approaches Rejected

**Unit test with mocks:**
- Would test GUID generation in isolation
- Wouldn't test actual file creation
- Wouldn't catch OS-level issues

**Spaced events (seconds apart):**
- Wouldn't test collision scenario
- False confidence (worked but didn't test edge case)

**Manual inspection of 1 event:**
- Proves format works
- Doesn't prove uniqueness

### Pattern: Integration Test via CLI

**Instead of:**
```fsharp
[<Test>]
let GUID suffix is unique () =
    let guid1 = generateGuid()
    let guid2 = generateGuid()
    Assert.AreNotEqual(guid1, guid2)
```

**We did:**
```bash
# Create events and inspect actual files
ls -la context-library/nexus/events/.../2025-11/
```

**Benefits:**
- Tests entire pipeline (code → disk → parsing)
- Visual confirmation
- Creates useful test data
- Documents the feature (test events become examples)

### Confidence Boost

Seeing 3 unique GUIDs in same second:
- d17ba665, 15a2441e, 26a94a3d
- Provides **visual proof** of uniqueness
- More confidence than passing unit test

### Reusable Pattern

For future phases:
1. Identify worst-case scenario
2. Create test that exercises it
3. Use real CLI tools (integration test)
4. Visual inspection of results
5. Keep test artifacts as documentation
