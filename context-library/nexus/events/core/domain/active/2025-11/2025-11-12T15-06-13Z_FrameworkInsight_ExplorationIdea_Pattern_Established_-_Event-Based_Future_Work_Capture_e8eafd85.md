---
id: 69e34a6b-ef24-4664-b253-3cee08e843c3
type: FrameworkInsight
title: "ExplorationIdea Pattern Established - Event-Based Future Work Capture"
summary: "Established ExplorationIdea pattern for capturing future work. Using DesignNote with tags temporarily. Three ideas captured. Implementation phases planned."
occurred_at: 2025-11-12T10:06:13.885-05:00
tags:
  - exploration-idea
  - event-types
  - methodology
  - framework-decision
---

## ExplorationIdea Pattern Established

**Decision:** Use events to capture future work ideas, not separate artifacts.

## New Event Type Concept: ExplorationIdea

**Purpose:** Low-friction capture of "sparks" - ideas to explore later.

**Philosophy:**
- Events ARE the way we create state
- Capture quickly, refine system later
- Separate from CrossProjectIdea (which is for cross-app patterns)
- Separate from TechnicalDecision (which documents made decisions)

## Current Implementation (Phase 1)

**Workaround:** Using DesignNote with tags until F# code updated:
- Tag: `exploration-idea`
- Tags for priority: `priority-important`, `priority-consider`, `priority-critical`
- Tags for status: `status-pending`, `status-exploring`, etc.
- Include priority/status in body text

**Structure (loose for now):**
```yaml
type: DesignNote  # Will become ExplorationIdea
project: [core | fnmcp-nexus | laundrylog | perdiem]
title: "ExplorationIdea: [Description]"
body: |
  Priority: [consider | important | critical]
  Status: [pending | exploring | implemented | rejected]
  
  [Idea details]
tags: ['exploration-idea', 'priority-X', 'status-Y', ...]
```

## Ideas Captured So Far

1. **Custom Filesystem MCP** (fnmcp-nexus, important, pending)
   - Eliminate token waste on filesystem operations
   - Nexus-aware tooling

2. **Remote VPS Deployment** (fnmcp-nexus, important, pending)
   - Android support
   - Stable architecture

3. **Refine ExplorationIdea System** (core, consider, pending)
   - Proper F# implementation
   - Status/priority taxonomy
   - Ideas backlog projection

## Future Phases

**Phase 2:** Proper Implementation
- Add ExplorationIdea to F# discriminated union
- Create capture_exploration_idea tool
- Define taxonomy (status, priority, categories)

**Phase 3:** Tooling and Projections
- Build ideas-backlog projection
- Add filtering/sorting
- Status workflow support

## Usage Pattern

**When to create ExplorationIdea:**
- Future work ideas
- "Wouldn't it be nice if..."
- Problems to solve later
- Research topics
- Architecture improvements

**When NOT to use:**
- Decisions already made (use TechnicalDecision)
- Cross-app pattern sharing (use CrossProjectIdea)
- Current work (use SessionState)
- Design rationale (use DesignNote)

## Success Criteria

✅ Established event-based approach
✅ Documented concept and philosophy
✅ Captured initial ideas
✅ Planned implementation phases
✅ Set up for future refinement

**The system is now ready to capture sparks as they occur!**
