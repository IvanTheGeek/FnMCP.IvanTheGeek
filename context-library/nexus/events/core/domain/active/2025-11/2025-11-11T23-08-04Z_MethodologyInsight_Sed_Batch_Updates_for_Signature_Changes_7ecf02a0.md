---
id: 60d700f4-9d84-4f49-ae1a-508eb0a8e33a
type: MethodologyInsight
title: "Sed Batch Updates for Signature Changes"
summary: "Using sed -i to update all function call sites simultaneously during refactoring"
occurred_at: 2025-11-11T18:08:04.075-05:00
tags:
  - methodology
  - refactoring
  - sed
  - batch-updates
  - phase-3
---

## Methodology: Batch Refactoring with Sed

### The Challenge
Phase 3 changed function signatures - added project parameter to:
- writeEventFile
- writeSystemEvent  
- writeLearningEvent

This broke **11 call sites** across 7 files.

### The Solution
**Use sed to update all callers at once:**
```bash
# Fix writeSystemEvent calls (added None parameter)
sed -i 's/EventWriter\.writeSystemEvent basePath systemEvent/EventWriter.writeSystemEvent basePath None systemEvent/g' \
  src/FnMCP.IvanTheGeek/Projections/*.fs \
  src/FnMCP.IvanTheGeek/Tools/EnhanceNexus.fs

# Fix writeEventFile call
sed -i 's/EventWriter\.writeEventFile basePath meta narrative/EventWriter.writeEventFile basePath None meta narrative/g' \
  src/FnMCP.IvanTheGeek/Tools/EnhanceNexus.fs

# Fix writeLearningEvent call
sed -i 's/writeLearningEvent basePath meta description/writeLearningEvent basePath None meta description/g' \
  src/FnMCP.IvanTheGeek/Tools/Learning.fs
```

### Why This Works

**Compiler-driven workflow:**
1. Change function signature
2. Attempt build - get compilation errors listing all call sites
3. Write sed command based on error pattern
4. Run sed to fix all instances
5. Build succeeds!

**Benefits:**
- **Fast**: Fix 11 call sites in seconds
- **Accurate**: Pattern matching ensures correctness
- **Consistent**: All sites updated identically
- **Auditable**: Can review changes with git diff

### Example from Phase 3

**Compilation error:**
```
error FS0001: This expression was expected to have type 'string option' 
but here has type 'SystemEventMeta'
```

**Pattern identified:**
```fsharp
EventWriter.writeSystemEvent basePath systemEvent
//                                    ↑
//                            Missing: None parameter
```

**Sed command:**
```bash
sed -i 's/writeSystemEvent basePath systemEvent/writeSystemEvent basePath None systemEvent/g'
```

**Result:** All 5 instances fixed instantly.

### Safety Considerations

**Escape special characters:**
- Use  for literal dots
- Quote the pattern string
- Test on one file first

**Verify changes:**
```bash
git diff  # Review before committing
```

**Multiple files:**
```bash
sed -i 's/pattern/replacement/g' file1.fs file2.fs file3.fs
# Or with wildcards:
sed -i 's/pattern/replacement/g' src/**/*.fs
```

### When NOT to Use Sed

**Complex logic changes:**
- If replacement requires context-specific decisions
- If different call sites need different fixes
- If pattern is ambiguous

**Single occurrence:**
- Just edit manually (faster than writing sed command)

**Phase 3 was perfect use case:**
- Simple mechanical change (add None parameter)
- Same fix needed at all call sites
- Pattern unambiguous

### Comparison: Manual Editing

**Without sed (manual):**
- Open 7 files one by one
- Find each call site
- Edit each instance
- Risk: inconsistency, missing one
- Time: ~10 minutes

**With sed:**
- Write 3 sed commands
- Run in 5 seconds
- Guaranteed consistency
- Time: ~2 minutes

### Pattern for Future

When changing function signatures:
1. Update signature in definition
2. Run build to find all call sites
3. Identify the pattern
4. Write sed command
5. Test on one file
6. Apply to all files
7. Verify with git diff
8. Build should succeed!

### Confidence: High
Sed batch updates are perfect for mechanical refactoring tasks.
