---
id: 6f7a8b9c-0d1e-2f3a-4b5c-6d7e8f9a0b1c
type: DesignDecision
title: "Added FrameworkInsight and LearningMoment Event Types"
summary: "Expanded from 3 to 5+ event types to capture different kinds of knowledge"
occurred_at: 2025-11-11T01:12:00.000-05:00
tags:
  - event-types
  - domain-model
  - evolution
author: Claude
---

# Added FrameworkInsight and LearningMoment Event Types

## Original Design (Phase 1)

Three event types:
```fsharp
type EventType =
    | TechnicalDecision
    | DesignNote
    | ResearchFinding
```

## Expansion During Implementation

Added during refactoring:
```fsharp
type EventType =
    | TechnicalDecision
    | DesignNote
    | ResearchFinding
    | FrameworkInsight      // NEW
    | LearningMoment        // NEW (in TechnicalPattern event)
```

## Why The Expansion?

**FrameworkInsight:**
- Captures meta-level insights about framework itself
- Examples:
  - "User Driven Architecture" - discipline matters
  - "Phase 1 Already Implemented" - discovery moments
  - "Phase 1 Validation Complete" - milestones

**LearningMoment** (mentioned in events):
- Documents specific lessons learned
- "Refactoring Validates Tests" - testing insights
- Learning through doing
- Could add officially later

**TechnicalPattern:**
- Reusable solutions to common problems
- "F# Namespace Module Conflict Resolution"
- More specific than TechnicalDecision

**DesignDecision:**
- Used for "Nexus At Project Root"
- Different from TechnicalDecision
- More about structure than implementation

## Pattern Observed

**Event types evolve with usage:**
- Start with 3 basic types
- Encounter situations that don't fit
- Add new types organically
- Refine as patterns emerge

## Event Type Semantics

**TechnicalDecision:**
- Binary choice made
- Has alternatives considered
- Has consequences
- Example: "Refactor To Nested Structure"

**DesignDecision:**
- Structural/organizational choice
- About "how things are arranged"
- Example: "Nexus At Project Root"

**FrameworkInsight:**
- Meta-level realization
- About development process itself
- Example: "User Driven Architecture"

**ResearchFinding:**
- Discovery through investigation
- Example: "MCP Tools Already Registered"

**DesignNote:**
- Implementation detail or observation
- Example: "Event Creation Test Success"

**TechnicalPattern:**
- Reusable solution
- Example: "F# Namespace Module Conflict"

## Future Event Types (from architecture doc)

From event-sourced-nexus-architecture.md:
```fsharp
type DomainEvent =
    // Already implemented
    | FrameworkInsight
    | TechnicalDecision
    | DesignDecision
    | TechnicalPattern

    // Learning journey (proposed)
    | LearningMoment of lesson:string * story:string * application:string
    | ExperimentFailed of approach:string * why:string * learned:string
    | ExperimentSucceeded of approach:string * results:string * impact:string

    // Evolution (proposed)
    | DesignIteration of app:string * version:int * changes:string
    | ConceptRefined of concept:string * oldUnderstanding:string * newUnderstanding:string
    | PatternDiscovered of pattern:string * occurrences:string list

    // Personal memory (proposed)
    | PreferenceDiscovered of concept:string * value:string * context:string
    | InterestAdded of topic:string * context:string
    | SkillLearned of skill:string * level:string
    | GoalSet of goal:string * deadline:DateTime option

    // Content creation (proposed)
    | PathNarrative of path:string * narrative:string
    | SillyStory of story:string * lesson:string option
    | ProblemSolved of problem:string * solution:string * journey:string
```

## Decision: Organic Growth

**Don't implement all types upfront:**
- Add types when we need them
- Real usage reveals what's useful
- Some proposed types may never be needed
- Some new types will emerge

**Current implementation is minimal:**
- Just enough types for Phase 1
- Easy to add more in Domain/Events.fs
- EventWriter handles all types generically (via YAML)
- No breaking changes to add types

## Adding A New Type

Simple process:
1. Add to `Domain/Events.fs` discriminated union
2. Update `EventType.Parse` if needed
3. Use it in events
4. Done

Example adding LearningMoment officially:
```fsharp
type EventType =
    | TechnicalDecision
    | DesignNote
    | ResearchFinding
    | FrameworkInsight
    | LearningMoment      // Add here
    | TechnicalPattern
    | DesignDecision
```

No other changes needed! YAML serialization handles it automatically.

## Lesson

**Start simple. Grow organically.**

Better to:
- Add 1 type when needed
- Than implement 20 types speculatively
- And never use 15 of them

Event sourcing makes this safe:
- Old events keep their types
- New types added easily
- No migration needed
- Events immutable

---

*Domain model grows with understanding. Perfect for event-sourced system.*
