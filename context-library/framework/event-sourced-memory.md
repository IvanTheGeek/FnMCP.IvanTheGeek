# Event-Sourced Personal Memory

**Framework:** FnMCP.IvanTheGeekDevFramework  
**Purpose:** Personal memory as part of event-sourced Nexus  
**Updated:** 2025-11-10  
**Status:** Subsumed into Event-Sourced Nexus Architecture

## Note

**This concept has evolved!** Personal memory is now integrated into the complete event-sourced Nexus architecture as domain events with `visibility: Private`.

See `framework/event-sourced-nexus-architecture.md` for the complete vision.

## Personal Memory Event Types

These domain events capture personal context:

```fsharp
// Part of DomainEvent union
| PreferenceDiscovered of concept:string * value:string * context:string
| InterestAdded of topic:string * context:string
| InterestLost of topic:string * reason:string
| SkillLearned of skill:string * level:string
| GoalSet of goal:string * deadline:DateTime option
| GoalCompleted of goal:string
```

## Integration with Broader Nexus

**Personal memory events are just domain events with different visibility:**

```markdown
---
type: PreferenceDiscovered
timestamp: 2025-11-10T14:30:00Z
visibility: Private  # Not published publicly
tags: [preferences, data-formats]
concept: DataFormat
value: YAML
---

# Preference for YAML Over JSON

Discovered during conversation about event-sourced memory design...
[Full narrative]
```

**Current state projection includes personal memory:**
```yaml
# projections/personal-memory/current.yaml
preferences:
  DataFormat: YAML
  ProgrammingLanguage: F#
  
interests:
  - Event Sourcing
  - Mobile-First UX
  
skills:
  F#: Intermediate
  Event Modeling: Advanced
```

## Why Integration Is Better

**Originally considered:** Separate Memory MCP (inspired by article)

**Final decision:** Integrate into Nexus because:
1. **Same patterns** - Events, projections, snapshots
2. **Shared infrastructure** - One MCP server, one event store
3. **Natural relationships** - Personal preferences influence technical decisions
4. **Simpler** - Don't maintain two event-sourced systems
5. **Content strategy** - Some personal stories become public (silly stories, learning moments)

## Visibility as the Key Differentiator

```fsharp
type EventVisibility =
    | Public      // Can share - blog, forum, video
    | Community   // Contributors only
    | Private     // Just for Claude, never published
```

**Private events:**
- Personal preferences (data format preferences, tool choices)
- Learning progress (skill development, courses taken)
- Personal goals (not related to public projects)
- Private context (family, personal life, health)

**Can become public:**
- Learning moments with valuable lessons
- Silly stories that teach something
- Problem-solved narratives worth sharing

## Projections for Personal Memory

**Current state (for Claude):**
```yaml
# projections/personal-memory/current.yaml
# Generated from PreferenceDiscovered, SkillLearned, etc.
# Used by Claude for personalization
```

**Timeline (personal journey):**
```markdown
# projections/personal-memory/timeline.md
# How interests and skills evolved
```

**Concept graph (learning connections):**
```yaml
# projections/personal-memory/concepts.yaml
# Discovered through event co-occurrence
relationships:
  - concepts: [F#, Event Sourcing, MCP]
    strength: High
```

## Migration from Original Concept

**Original design:**
- Separate JSON file with CRUD operations
- Inspired by Memory MCP article

**Current design:**
- Part of event-sourced Nexus
- Markdown + YAML frontmatter
- Same tools, resources, prompts as other events
- Just different visibility level

**Benefits of integration:**
- Unified "enhance nexus" workflow
- Personal context informs technical decisions
- Can promote private events to public if valuable
- One system to maintain

---

*Personal memory is simply private-visibility domain events in the event-sourced Nexus - elegant integration rather than separate system.*