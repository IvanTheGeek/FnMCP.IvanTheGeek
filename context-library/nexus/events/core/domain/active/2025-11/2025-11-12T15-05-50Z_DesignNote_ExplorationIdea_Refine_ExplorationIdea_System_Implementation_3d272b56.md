---
id: cb1e2b2a-f6d9-4d97-b0f0-f13ac00f9b8e
type: DesignNote
title: "ExplorationIdea: Refine ExplorationIdea System Implementation"
summary: "Properly implement ExplorationIdea event type with taxonomy, tooling, and backlog projection for managing future work ideas."
occurred_at: 2025-11-12T10:05:50.981-05:00
tags:
  - exploration-idea
  - event-types
  - methodology
  - tooling
  - priority-consider
  - status-pending
  - meta
---

## Exploration: Refine ExplorationIdea System

**Type:** ExplorationIdea (using DesignNote until type implemented)
**Priority:** consider
**Status:** pending

**Spark:** Define and implement proper ExplorationIdea event type system.

## Context

We've established ExplorationIdea as a concept for capturing "sparks" - ideas to explore later. Currently using DesignNote with tags as workaround since ExplorationIdea isn't implemented in F# code yet.

## The Idea

Properly implement ExplorationIdea as first-class event type with:

**1. F# Type Implementation**
- Add to event type discriminated union
- Define proper structure
- Create capture_exploration_idea tool

**2. Status Taxonomy**
Define clear status workflow:
- pending - Captured but not started
- exploring - Actively investigating
- paused - Started but on hold
- implemented - Completed and deployed
- rejected - Decided not to pursue
- superseded - Better approach found

**3. Priority Definitions**
Clear priority meanings:
- critical - Blocking other work
- important - Should do soon
- consider - Nice to have
- someday - Interesting but no urgency

**4. Category/Scope**
Classify ideas by area:
- tooling - Development tools
- infrastructure - System architecture
- feature - New capabilities
- performance - Optimization
- research - Investigation needed
- methodology - Process improvements

**5. Ideas Backlog Projection**
Create projection that:
- Lists all ExplorationIdea events
- Groups by priority
- Filters by status
- Shows by project
- Provides "what to work on next" view

## Benefits

- Proper type safety in F# code
- Better tooling support
- Clearer taxonomy
- Useful backlog view
- Foundation for idea management

## Initial Thoughts

Phase 1:
- Add ExplorationIdea to F# DU
- Update create_event validation
- Create capture_exploration_idea tool

Phase 2:
- Define status/priority/category enums
- Update existing "exploration-idea" tagged events

Phase 3:
- Build ideas-backlog projection
- Add filtering capabilities
- Status transition support

## Next Steps (When Explored)

1. Design F# type structure
2. Update Domain.fs
3. Update Tools.fs
4. Test event creation
5. Migrate existing tagged events
6. Build projection
7. Document usage patterns
