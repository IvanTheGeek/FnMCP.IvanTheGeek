# Context Monitoring: Token Usage Display Pattern

**Framework:** FnMCP.IvanTheGeekDevFramework  
**Purpose:** Display token usage after every Claude response  
**Updated:** 2025-11-11  
**Status:** Active - Required for all conversations

## The Pattern

**Display detailed token usage statistics at the end of EVERY response** to monitor context consumption and manage conversation flow efficiently.

## Required Format

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 Context Usage: [████████░░░░░░░░░░░░] 85,000 / 190,000 tokens (44.7%)

Allocation:
├─ System Prompts:     ~5,000 tokens  (2.6%)
├─ Project Knowledge:  ~2,000 tokens  (1.1%)
├─ Conversation:      ~75,000 tokens (39.5%)
│  ├─ Your messages:  ~12,000 tokens
│  ├─ My responses:   ~58,000 tokens
│  └─ Tool calls:      ~5,000 tokens
└─ This Response:      ~3,000 tokens  (1.6%)

Remaining: 105,000 tokens (55.3%) ✓ Comfortable

Status Legend: ✓ Comfortable (0-75%) | ⚠ Moderate (75-85%) | 🔴 High (85%+)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

## Visual Bar Calculation (CRITICAL)

**20 characters total, each block = 5%**

```python
# CORRECT calculation
total_tokens = 190000
used_tokens = 85000
percentage = (used_tokens / total_tokens) * 100  # 44.74%

# Calculate filled blocks
filled = int(percentage / 5)  # int(44.74 / 5) = int(8.948) = 8
empty = 20 - filled           # 20 - 8 = 12

bar = "█" * filled + "░" * empty
# Result: "████████░░░░░░░░░░░░" (8 filled, 12 empty)
```

**Rounding:** Use `int()` to truncate (floor). Do NOT round up.

**Common mistake:** Don't calculate `filled = (used_tokens / total_tokens) * 20` directly - this gives wrong results because you're not accounting for the percentage-to-blocks conversion.

### Verification Examples

| Used % | Blocks | Bar |
|--------|--------|-----|
| 5%     | 1      | `[█░░░░░░░░░░░░░░░░░░░]` |
| 25%    | 5      | `[█████░░░░░░░░░░░░░░░]` |
| 42.9%  | 8      | `[████████░░░░░░░░░░░░]` |
| 50%    | 10     | `[██████████░░░░░░░░░░]` |
| 75%    | 15     | `[███████████████░░░░░]` |
| 90%    | 18     | `[██████████████████░░]` |
| 100%   | 20     | `[████████████████████]` |

## Components Explained

**Visual Bar:**
- 20 characters total
- `█` represents used tokens (filled)
- `░` represents remaining tokens (empty)
- Each character = 5% of total capacity

**Allocation Breakdown:**
- **System Prompts**: Claude's base instructions (~5K fixed)
- **Project Knowledge**: Files loaded at conversation start
- **Conversation**: All messages and tool results
  - Your messages: User input
  - My responses: Claude's replies
  - Tool calls: MCP operations, searches, etc.
- **This Response**: Current reply token cost

**Status Indicators:**
- `✓ Comfortable` (0-75% used): Plenty of headroom
- `⚠ Moderate` (75-85% used): Plan to wrap up soon
- `🔴 High` (85%+ used): Finish current task, start new chat

## Implementation

### In Memory (Account-Level)
User preference stored in Claude's memory system:
> "Display token usage after EVERY response: line separators (━), 20-char bar (█/░), tree allocation (├─), status legend (✓⚠🔴). See technical/context-monitoring.md"

This ensures the pattern applies to **all conversations** (top-level and in projects).

### In Project Knowledge (Quick Start)
Framework overview includes complete format specification with calculation details.

## Why This Matters

### Without Monitoring
- Conversations hit limits unexpectedly
- Context loss mid-task
- Frustration and wasted time
- No visibility into token consumption patterns

### With Monitoring
- **Proactive management**: See limits approaching
- **Informed decisions**: Know when to continue vs. start fresh
- **Optimization feedback**: Measure impact of changes
- **Predictable workflow**: Plan conversation length

## Real-World Benefits

### Token Optimization Validation
```
Before MCP (loading full docs):
├─ Project Knowledge: ~15,000 tokens (7.9%)

After MCP (Quick Start only):
├─ Project Knowledge:  ~2,000 tokens (1.1%)

Savings: 13,000 tokens (87% reduction) ✓ VERIFIED
```

### Session Planning
```
At 40% (76K used):
→ Continue with complex features

At 70% (133K used):  
→ Finish current task, prepare to wrap

At 85% (162K used):
→ Complete work, start new chat immediately
```

### Debugging Aid
```
Unexpected 30K token response?
→ Check breakdown to see why
→ Adjust approach for efficiency
```

## Success Metrics

**Monitoring is working when:**
- Appears after every response consistently
- Shows accurate token counts
- **Visual bar correctly represents percentage** (8 blocks for ~40%, not 16!)
- Status indicators trigger at right thresholds
- User can plan conversation flow effectively
- Token optimizations are immediately visible

## Integration with "Enhance Nexus"

When running "enhance nexus", this monitoring:
1. Shows token cost of analysis
2. Tracks MCP update operations
3. Validates memory additions fit comfortably
4. Ensures updates don't bloat context

---

*Context monitoring transforms blind token consumption into visible, manageable resource usage - essential for efficient Nexus operation.*