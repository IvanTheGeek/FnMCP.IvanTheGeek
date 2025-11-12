---
id: defd32fd-cd43-4bac-807a-b4b020aa785b
type: FrameworkInsight
title: "Token Calculation Fix - Root Cause and Comprehensive Solution"
occurred_at: 2025-11-12T09:05:37.611-05:00
tags:
  - token-display
  - bootstrap-pattern
  - recurring-issue
  - root-cause-analysis
---

## Problem: Recurring Token Bar Calculation Errors

**Symptoms:** Token bar displayed incorrect number of filled blocks in new chats (occurred 3+ times).

**Example:** 25.66% usage showed 9 filled blocks instead of correct 5 blocks.

## Root Cause Analysis

**Why it kept failing:**
1. Formula existed in spec file → but spec not always loaded in new chats
2. Formula added to memory → but memory only loads in non-project chats
3. Formula NOT in bootstrap file → which loads in EVERY project chat

**The gap:** Project chats didn't have formula available from conversation start.

## Comprehensive Solution: Dual Coverage

**For Project Chats:**
- Added token calculation to bootstrap file template
- Formula + verification examples now in nexus-bootstrap.md
- Formula + verification examples now in perdiem-bootstrap.md
- Bootstrap files load automatically in every project chat

**For Non-Project Chats:**
- Updated user preferences (memory edit #2)
- Includes full formula + verification examples
- User preferences load automatically in every chat

**Formula Required in Both:**
```
percentage = (used / total) * 100
filled_blocks = round(percentage / 5)
empty_blocks = 20 - filled_blocks

Verification:
- 25.7% → round(25.7/5) = 5 filled blocks
- 48.9% → round(48.9/5) = 10 filled blocks  
- 72.7% → round(72.7/5) = 15 filled blocks
```

## Prevention Architecture

**Bootstrap Pattern Standard:**
All bootstrap files MUST include:
1. Project identification and scope
2. How to start conversations
3. Project structure and file paths
4. **Token calculation formula with verification examples** ← NEW REQUIREMENT
5. Context management explanation
6. Event sourcing location

**Documentation Updated:**
- technical/project-configuration-nexus.md - includes token calculation in template
- technical/project-creation-perdiem.md - includes token calculation in template
- Both guides emphasize this as critical component

## Why This Works

**Coverage is complete:**
- Project chats: Bootstrap loads formula
- Non-project chats: User preferences load formula
- No gap where formula is missing
- Present from first message in every conversation

**Self-contained verification:**
- Examples show expected results
- Claude can verify calculation before displaying
- No need to reference external spec file

## Lessons Learned

1. **Recurring issues indicate wrong fix location** - We kept patching where the information existed, not where it would actually load
2. **Bootstrap files are the right place for critical patterns** - They load in every project chat with zero additional token cost
3. **Dual coverage for reliability** - Project chats use bootstrap, non-project chats use preferences
4. **Make patterns self-contained** - Include verification examples directly, don't require external lookups
