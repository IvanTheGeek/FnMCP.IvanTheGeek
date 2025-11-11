---
id: 58cc9cdd-5a56-4ce3-995a-51a1264cb656
type: FrameworkInsight
title: "Project Scoping Reduces Token Consumption Exponentially"
summary: "Project-scoped organization reduces token consumption 50% now, scales to 98.5% savings with growth"
occurred_at: 2025-11-11T14:02:28.585-05:00
tags:
  - token-efficiency
  - scalability
  - architecture
---

# The Insight

Organizing knowledge by project scope rather than global structure dramatically reduces token consumption while maintaining full access to needed information.

# Discovery

During architecture discussion, we realized LaundryLog conversations don't need PerDiemLog knowledge, but ALL projects need core framework principles. The token cost was loading everything regardless of relevance.

# The Pattern

**Before: Global Loading**
- Any conversation loads all framework docs, all app docs, all events
- LaundryLog conversation: ~12K tokens loaded, only ~6K relevant

**After: Scoped Loading**
- LaundryLog loads core/framework/ (shared) + projects/laundrylog/ (specific)
- Result: ~6K tokens loaded, 100% relevant

# Token Savings

**Current (3 projects):** 50% savings per conversation
**Future (10 projects):** 85% savings
**Future (100 projects):** 98.5% savings

# Why This Matters

Token efficiency enables:
- Longer, more productive conversations
- More complex queries without hitting limits
- Unlimited project scaling without context bloat
- Focused, relevant knowledge per conversation

**The principle:** Structure knowledge by usage patterns, not just logical organization.
