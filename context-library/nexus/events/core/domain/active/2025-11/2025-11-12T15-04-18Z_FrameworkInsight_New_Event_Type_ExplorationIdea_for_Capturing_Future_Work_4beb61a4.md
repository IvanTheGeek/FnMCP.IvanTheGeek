---
id: 818ddefb-0f64-4fe9-afdb-d80273129e48
type: FrameworkInsight
title: "New Event Type: ExplorationIdea for Capturing Future Work"
summary: "Established ExplorationIdea event type for capturing future work ideas. Loose structure initially, captures \"sparks\" for later exploration. Separate from CrossProjectIdea."
occurred_at: 2025-11-12T10:04:18.721-05:00
tags:
  - event-types
  - exploration
  - future-work
  - methodology
---

## New Event Type: ExplorationIdea

**Purpose:** Capture "sparks" - ideas to explore later without heavy ceremony.

**Design Philosophy:**
- Events ARE how we create state in Nexus
- Capture the idea quickly, refine system later
- Low friction for recording thoughts
- Separate from CrossProjectIdea (which is for cross-app pattern sharing)

## Event Structure

```yaml
type: ExplorationIdea
project: [core | fnmcp-nexus | laundrylog | perdiem]
title: "Short description"
summary: "One-line what/why"
body: |
  Detailed exploration notes
  Context and rationale
  Initial thoughts
tags: ['category', 'area', 'keywords']
priority: [consider | important | critical]  # loose for now
status: [pending | exploring | implemented | rejected]  # loose for now
```

## Use Cases

**Nexus/Framework ideas:**
- Build custom filesystem MCP
- Remote VPS deployment
- New projection types

**App-specific ideas:**
- LaundryLog feature ideas
- PerDiem enhancements
- CheddarBooks concepts

**Infrastructure ideas:**
- Performance improvements
- Tooling enhancements
- Development workflow improvements

## What It's NOT

**Not CrossProjectIdea:** That's specifically for moving discovered patterns/components from one app to another (source → target relationship).

**Not TechnicalDecision:** Those document decisions that were made, not ideas to explore.

**Not DesignNote:** Those document design decisions and rationale, not future possibilities.

## Future Refinement Needed

The ExplorationIdea system itself needs refinement:
- Better status taxonomy
- Category/scope classifications
- Priority definitions
- Status workflow (pending → exploring → implemented)
- Projection to view as backlog

**Meta-note:** This refinement should be captured as an ExplorationIdea itself!

## Implementation Plan

**Phase 1 (Now):**
- Document the concept
- Use existing create_event tool with type="ExplorationIdea"
- Keep structure loose

**Phase 2 (Later):**
- Add ExplorationIdea to F# event type discriminated union
- Create capture_exploration_idea tool for convenience
- Define status/priority taxonomies

**Phase 3 (Later):**
- Build ideas-backlog projection
- Add filtering/sorting capabilities
- Status workflow support
