---
id: b6622ec3-c395-4bd0-bd1b-c1a1eefcc16b
type: TechnicalDecision
title: "New Event Types - MethodologyInsight and NexusInsight"
summary: "Added MethodologyInsight and NexusInsight event types to match Nexus hierarchy layers"
occurred_at: 2025-11-11T14:03:15.232-05:00
tags:
  - event-types
  - architecture
  - knowledge-organization
technical_decision:
---

# Context

The Nexus hierarchy clarification revealed four distinct layers: Philosophy, Methodology, Framework, and Projects. Current event types don't map cleanly to these layers:

**Current types:**
- TechnicalDecision
- DesignNote
- ResearchFinding
- FrameworkInsight ← Only one "Insight" type

**Gap:** No way to distinguish between:
- Framework discoveries (tech stack)
- Methodology discoveries (how to work)
- Nexus system discoveries (meta-level)

# Decision

Add two new event types:

## 1. MethodologyInsight
**Purpose:** Discoveries about how we think and work

**Examples:**
- "Event Modeling surfaces hidden requirements early"
- "Dogfooding reveals UX issues commercial users would miss"
- "Paths catch edge cases that unit tests miss"
- "Static-State Design eliminates mode confusion"
- "Done is done principle prevents scope creep"

## 2. NexusInsight
**Purpose:** Discoveries about the Nexus system itself

**Examples:**
- "Learning events compound AI effectiveness over time"
- "Projection normalization handles name evolution cleanly"
- "Two-tier architecture reduces token consumption 87%"
- "Project scoping enables exponential token savings"
- "Read-on-request pattern eliminates cache complexity"

# Complete Event Type Set

**Decisions & Notes:**
- TechnicalDecision (implementation choices)
- DesignNote (design choices)

**Research & Findings:**
- ResearchFinding (external research)

**Insights (by layer):**
- MethodologyInsight (HOW we work)
- FrameworkInsight (WHAT tools we use)
- NexusInsight (system meta-level)

# Rationale

**Semantic Clarity:**
- Clear distinction between types of knowledge
- Enables targeted queries ("show me all methodology insights")
- Projections can group by insight type

**Token Efficiency:**
- Filter events by relevance to current task
- LaundryLog work: Load MethodologyInsight + FrameworkInsight
- Nexus work: Load all insight types
- Reduces noise in event stream

**Knowledge Organization:**
- Mirrors the Nexus hierarchy
- Natural mapping: Methodology → MethodologyInsight
- Scales as system grows

# Implementation

**Event Type Definition (F#):**
```fsharp
type EventType =
    | TechnicalDecision
    | DesignNote
    | ResearchFinding
    | FrameworkInsight
    | MethodologyInsight  // NEW
    | NexusInsight        // NEW
```

**Validation:**
- Event writer accepts new types
- Timeline projection handles new types
- Knowledge projection groups by type

# Migration

**Existing events:** Review and potentially reclassify
- Some current FrameworkInsights may be MethodologyInsights
- Some may be NexusInsights
- Use projection normalization to handle transition
