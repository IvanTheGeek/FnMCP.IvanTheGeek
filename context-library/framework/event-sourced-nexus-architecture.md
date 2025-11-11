# Event-Sourced Nexus Architecture

**Framework:** FnMCP.IvanTheGeekDevFramework  
**Purpose:** Complete event-sourced system for development knowledge and personal memory  
**Updated:** 2025-11-10  
**Status:** Design Complete - Ready for Implementation

## The Vision

**Event-source the entire Nexus** - not just personal memory, but all development knowledge, decisions, insights, and system operations. One source of truth (events), infinite projections (documentation, blog posts, timeline, metrics, etc.).

## Core Principles

1. **Events are stories** - Markdown narratives with YAML frontmatter
2. **Single file per event** - Git-friendly, browsable, human-readable
3. **Everything is an event** - Domain insights AND system operations
4. **Everything is a projection** - Documentation, metrics, timeline, all computed from events
5. **Never lose data** - Append-only, archive old events, always available
6. **Content infrastructure** - One event stream → blog, forum, video, docs

## Architecture Overview

### Event Structure

**Two event categories:**

**Domain Events** - Your insights, decisions, learnings
```
nexus/events/domain/
├── active/
│   ├── 2025-11/
│   │   ├── 2025-11-10T14-30-00_PreferenceDiscovered_DataFormat.md
│   │   ├── 2025-11-10T16-00-00_TechnicalDecision_EventSourcedNexus.md
│   │   └── 2025-11-10T16-15-00_DesignIteration_LaundryLogV7.md
│   └── 2025-12/
└── archive/
    ├── 2024.tar.gz
    └── 2025-Q1.tar.gz
```

**System Events** - Operations, performance, usage
```
nexus/events/system/
├── active/
│   └── 2025-11/
│       ├── 2025-11-10T16-30-00_EventCreated.yaml
│       ├── 2025-11-10T16-30-05_ProjectionRegenerated.yaml
│       └── 2025-11-10T16-31-00_ToolInvoked.yaml
└── archive/
    └── 2024.tar.gz
```

### Event Format: Markdown + YAML Frontmatter

```markdown
---
type: TechnicalDecision
timestamp: 2025-11-10T16:00:00Z
visibility: Public
tags: [event-sourcing, architecture, nexus]
title: "Event-Source the Entire Nexus"
author: Ivan
related_events: 
  - event_2025-11-10_FrameworkInsight_EventsAsNarratives
  - event_2025-11-10_TechnicalInsight_SingleFilePerEvent
---

# Event-Source the Entire Nexus

## The Problem

Traditional CRUD loses data - even with git, we lose:
- Why we made intermediate changes
- Alternatives we considered
- Learning journey context
- Failed experiments (valuable!)

## The Insight

If we're event-sourcing personal memory, why not event-source ALL knowledge?
Development insights, design decisions, technical patterns, everything.

## The Decision

**Event-source the complete Nexus:**
- Domain events: Insights, decisions, learnings
- System events: Operations, metrics, performance
- All projections computed from events
- Never lose information
- Infinite views from same data

## What This Enables

**Content infrastructure:**
- One event stream
- → Documentation (MCP projection)
- → Blog posts (content projection)
- → Forum discussions (community projection)  
- → Video scripts (narrative projection)
- → Timeline (journey projection)

**True dogfooding:**
- Validates event sourcing before production apps
- Proves patterns work at scale
- Foundation for LaundryLog, PerDiemLog, etc.

## Implementation Strategy

Start hybrid:
1. Keep current context-library
2. Add events for NEW insights
3. Slowly migrate existing docs
4. Learn what works

---

*This decision transforms Nexus from documentation system to narrative infrastructure - perfect event sourcing validation.*
```

### Domain Event Types

```fsharp
type DomainEvent =
    // Core insights
    | FrameworkInsight of insight:string * context:string * impact:string
    | TechnicalDecision of decision:string * alternatives:string list * reasoning:string
    | DesignDecision of decision:string * alternatives:string list * reasoning:string
    | TechnicalPattern of pattern:string * problem:string * solution:string
    
    // Learning journey
    | LearningMoment of lesson:string * story:string * application:string
    | ExperimentFailed of approach:string * why:string * learned:string
    | ExperimentSucceeded of approach:string * results:string * impact:string
    
    // Evolution
    | DesignIteration of app:string * version:int * changes:string * reasoning:string
    | ConceptRefined of concept:string * oldUnderstanding:string * newUnderstanding:string
    | PatternDiscovered of pattern:string * occurrences:string list * abstraction:string
    
    // Personal memory (from Memory MCP inspiration)
    | PreferenceDiscovered of concept:string * value:string * context:string
    | InterestAdded of topic:string * context:string
    | SkillLearned of skill:string * level:string
    | GoalSet of goal:string * deadline:DateTime option
    
    // Content creation
    | PathNarrative of path:string * narrative:string
    | SillyStory of story:string * lesson:string option
    | ProblemSolved of problem:string * solution:string * journey:string
```

### System Event Types

```fsharp
type SystemEvent =
    // Event operations
    | EventCreated of eventId:EventId * eventType:string * timestamp:DateTime
    | EventValidated of eventId:EventId * valid:bool * errors:string list
    
    // Projection operations  
    | ProjectionRegenerated of 
        projectionType:ProjectionType * 
        duration:TimeSpan * 
        strategy:ProjectionStrategy * 
        eventCount:int * 
        timestamp:DateTime
        
    | ProjectionQueried of 
        projectionType:ProjectionType * 
        staleness:Staleness * 
        timestamp:DateTime
    
    // MCP operations
    | ToolInvoked of toolName:string * parameters:Map<string,obj> * timestamp:DateTime
    | ToolCompleted of toolName:string * duration:TimeSpan * success:bool
    | ResourceAccessed of resourceUri:string * freshness:FreshnessLevel * timestamp:DateTime
    
    // Archive operations
    | BooksClosedof period:string * eventCount:int * compressedSize:int64 * timestamp:DateTime
    | SnapshotCreated of projections:ProjectionType list * timestamp:DateTime
```

## Projection System

### Projection Types

```
nexus/projections/
├── current-docs/        # What MCP serves (from domain events)
│   ├── .meta.yaml      # Staleness tracking
│   ├── INDEX.md
│   ├── framework/
│   ├── apps/
│   └── technical/
│
├── timeline/           # Journey chronicle (from domain events)
│   ├── .meta.yaml
│   └── evolution.md
│
├── decisions/          # Decision log (from domain events)
│   └── why-we-chose.md
│
├── metrics/            # System metrics (from SYSTEM events)
│   └── statistics.yaml
│
├── blog-posts/         # Content ready to publish (from domain events)
│   └── drafts/
│
├── forum-content/      # Discussion starters (from domain events)
│   └── weekly-insights.md
│
└── snapshots/          # Point-in-time state at book closing
    ├── 2024-12-31/
    └── 2025-06-30/
```

### Hybrid Projection Strategy

**Cached projections with staleness tracking:**

```yaml
# projections/registry.yaml - Generated from system events
projections:
  - type: Index
    path: projections/current-docs/INDEX.md
    strategy: FullRegeneration
    auto_refresh:
      trigger: OnEventCreate
      threshold: 1  # Always fresh
    status:
      last_updated: 2025-11-10T16:32:00Z
      staleness: Fresh
      source_event_count: 47
      
  - type: CurrentDocumentation
    path: projections/current-docs/
    strategy: IncrementalUpdate
    auto_refresh:
      trigger: OnEventCreate
      threshold: 3  # Every 3 events
    status:
      last_updated: 2025-11-10T16:30:00Z
      staleness: Stale(2)
      source_event_count: 47
      
  - type: Timeline
    strategy: IncrementalUpdate
    auto_refresh:
      trigger: OnDemand
    status:
      last_updated: 2025-11-09T10:00:00Z
      staleness: Stale(8)
      
  - type: Metrics
    strategy: FullRegeneration
    auto_refresh:
      trigger: OnDemand
    status:
      staleness: Stale(15)  # System events since last generation
```

**Staleness computation:**
```fsharp
let getStaleness projType =
    let lastRegen = 
        getSystemEvents()
        |> filterMap (function 
            | ProjectionRegenerated(pt, _, _, _, ts) when pt = projType -> Some ts
            | _ -> None)
        |> maxBy id
    
    let newEvents = 
        getDomainEvents()
        |> filter (fun e -> e.Timestamp > lastRegen)
        |> length
    
    match newEvents with
    | 0 -> Fresh
    | n -> Stale n
```

## Archival Strategy ("Closing the Books")

### Quarterly Book Closing

**Process:**
1. Create snapshots of all projections at quarter-end
2. Compress events from the quarter (tar.gz)
3. Verify archive integrity
4. Move to archive directory
5. Create manifest

**Result:**
- Active events: Last 3 months (uncompressed, fast)
- Archived events: Older quarters (compressed, slower but accessible)
- Snapshots: Projection state at each quarter-end

### Storage Economics

**Compression ratios:**
- Markdown + YAML: ~20:1 compression
- 2 years of events: ~5 MB total storage
- Negligible compared to modern drive sizes

**Access performance:**
- Active events: Instant
- Archived events: ~1-2 seconds (decompress + load)
- Never lose data, always available

### Archive Structure

```
nexus/events/domain/archive/
├── manifest.yaml
├── 2024.tar.gz           # 342 events, 4.2 MB compressed
├── 2025-Q1.tar.gz        # 98 events, 1.8 MB compressed
└── 2025-Q2.tar.gz        # 112 events, 2.1 MB compressed

nexus/projections/snapshots/
├── 2024-12-31/           # Year-end snapshot
│   ├── current-docs/
│   ├── timeline/
│   └── metrics.yaml
└── 2025-06-30/           # Q2 snapshot
    ├── current-docs/
    └── timeline/
```

## MCP Server Design

### Tools (Actions)

**Event creation:**
- `create_event(type, data, narrative)` - Generic event creation
- `enhance_nexus(analysis)` - High-level workflow, creates multiple events

**Projection management:**
- `regenerate_projection(type)` - Force regeneration
- `query_projection(type, filters)` - Query with filters

**Event queries:**
- `search_events(query, filters)` - Full-text + structured search
- `get_related_events(eventId, depth)` - Traverse relationships
- `get_timeline(dateRange, tags)` - Filtered timeline

### Resources (Content)

**Current projections:**
- `projections/current-docs/*` - What MCP serves (context-library replacement)
- `projections/index` - INDEX.md for discovery
- `projections/timeline` - Journey chronicle

**Event access:**
- `events/domain/*` - Domain events (filtered by date/tag/type)
- `events/system/*` - System events (for debugging, metrics)

**Meta information:**
- `event-schema` - What event types exist
- `projection-status` - Freshness, staleness
- `metrics` - System statistics

### Prompts (Guided Workflows)

**Primary:**
- `enhance_nexus` - Multi-step: analyze → create events → regenerate → commit

**Event creation:**
- `create_technical_decision` - Guided decision documentation
- `document_design_iteration` - Capture design evolution
- `record_experiment` - Track what we tried

## Content Strategy

### One Event Stream → Multiple Outputs

**Example event:**
```markdown
---
type: DesignIteration
app: LaundryLog  
version: 7
visibility: Public
tags: [mobile-ux, touch-targets, real-world-testing]
---

# LaundryLog v7: The Touch Target Revelation

[Full narrative about discovering 72px touch targets through real-world testing...]
```

**Generates:**
1. **Documentation** → `framework/mobile-first-principles.md` (technical detail)
2. **Blog post** → "From Figma to Truck Stop: Real-World UX Testing" (story)
3. **Forum post** → "Design Decision: 72px Touch Targets" (community discussion)
4. **Video script** → "Why Our Buttons Are Huge (And Yours Should Be Too)" (entertaining)
5. **Timeline entry** → "Nov 10: Touch target breakthrough" (journey)
6. **Path narrative** → Concrete example in LaundryLog path docs

### Visibility Levels

```fsharp
type EventVisibility =
    | Public      // Can share anywhere - blog, forum, video
    | Community   // Forum/contributors only
    | Private     // Just for Claude context, not published
```

### Content Projections

**Blog drafts projection:**
- Filter: `visibility=Public AND tags contains 'story'`
- Group related events into coherent narratives
- Output: Ready-to-publish markdown

**Forum content projection:**
- Filter: `visibility IN [Public, Community] AND type=TechnicalDecision`
- Format as discussion starters
- Output: Forum-ready posts

**Video script projection:**
- Filter: `visibility=Public AND (type=SillyStory OR tags contains 'real-world')`
- Extract narrative arcs
- Output: Screenplay format

## Implementation Roadmap

### Phase 1: Foundation (Now)
1. Design event schemas (F# types)
2. Design MCP tools/resources/prompts  
3. Document archival strategy
4. Plan transition from current context-library

### Phase 2: Hybrid Start (Week 1-2)
1. Keep current context-library as-is
2. Implement basic `create_event` tool
3. Start adding events for NEW insights only
4. Build simple projection (timeline or metrics)
5. Validate event → projection flow

### Phase 3: Expand (Week 3-4)
1. Implement `enhance_nexus` workflow
2. Add more projection types
3. Implement staleness tracking
4. Test hybrid projection strategy

### Phase 4: Migration (Month 2)
1. Slowly migrate existing docs to events
2. Retroactively create events from git history
3. Validate all projections working
4. Performance tuning

### Phase 5: Content Strategy (Month 3+)
1. Blog post projection
2. Forum content projection
3. Video script generation
4. Community contribution events

## Benefits Summary

**Never lose information:**
- Every decision, every alternative, every failure
- Complete audit trail
- Time travel to any point

**Content infrastructure:**
- Write once (event)
- Publish everywhere (projections)
- Authentic stories from real journey

**Framework validation:**
- Dogfooding at deepest level
- Proves patterns before production
- Foundation for LaundryLog, PerDiemLog

**Developer experience:**
- "enhance nexus" captures insights automatically
- Git history shows evolution
- Documentation always fresh

**Community enablement:**
- Contributors create events
- All histories merge naturally
- Influence system integrated

**Maximum freedom:**
- Own your data (events)
- Use it however you want (projections)
- Archive but never lose
- Privacy control (visibility levels)

---

*Event-sourced Nexus transforms development knowledge from static documentation into living narrative infrastructure - the ultimate dogfooding of the framework.*