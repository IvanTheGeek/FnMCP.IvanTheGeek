# Nexus: The Integrated Development Context System

**Term:** Nexus  
**Definition:** The complete integrated context system capturing methodology, knowledge, preferences, and philosophy  
**Access Mechanism:** FnMCP.IvanTheGeek MCP Server  
**Updated:** 2025-11-10

## What is Nexus?

**Nexus is not the MCP server** - it's the entire integrated context system that captures:
- Your development methodology (Event Modeling, static-state design, paths)
- Accumulated knowledge and decisions
- Personal preferences and patterns
- Philosophical approach to building software
- The evolution of your thinking over time

**The MCP server is just the access mechanism** - the tool that makes Nexus available to LLMs. Nexus itself is the integrated whole: your externalized mental model for development.

## Why "Nexus"?

The name emphasizes **integration** - the point where everything connects:
- Knowledge → Methodology → Tools → Philosophy
- Personal experience → Community feedback → Framework evolution
- Design → Implementation → Testing → Documentation
- Past conversations → Current work → Future direction

Short to type, captures the essence.

## Current Architecture (Transitional)

### Three-Layer System

**Layer 1: Memory (Account-Level)**
- Cross-project personal context
- Claude's memory system (userMemories)
- Personal preferences, general approach

**Layer 2: Project Knowledge**
- Quick-start essentials (~2K tokens)
- Framework overview, current focus, naming conventions
- Always loaded in conversations

**Layer 3: MCP Context Library**
- Detailed documentation served on-demand
- INDEX.md provides discovery map
- Only pay tokens for what you access

**Token efficiency:** ~4.5K tokens upfront (70% savings vs old approach)

See `framework/documentation-discovery.md` for details.

## Future Architecture: Event-Sourced Nexus

**The next evolution** - Event-source everything:

### Core Concept

**Events are the source of truth:**
```
nexus/
├── events/
│   ├── domain/     # Your insights, decisions, learnings
│   └── system/     # Operations, metrics, performance
└── projections/
    ├── current-docs/   # What MCP serves (generated)
    ├── timeline/       # Journey chronicle (generated)
    ├── metrics/        # Statistics (generated)
    └── blog-posts/     # Content drafts (generated)
```

**Everything becomes an event:**
- Technical decisions → Events with narratives
- Design iterations → Events with context
- Learning moments → Events with stories
- System operations → Events for metrics

**Everything becomes a projection:**
- Documentation → Generated from domain events
- Metrics → Generated from system events
- Blog posts → Generated from public events
- Timeline → Generated from all events

### Why Event Sourcing?

**Never lose information:**
- Traditional CRUD loses alternatives, context, journey
- Even git loses "why" and intermediate reasoning
- Event sourcing preserves complete history

**Content infrastructure:**
- One event stream
- Infinite projections (docs, blog, forum, video, timeline)
- Write authentic stories from real journey

**Framework validation:**
- Dogfooding at deepest level
- Proves patterns before production apps
- Foundation for LaundryLog, PerDiemLog

**See `framework/event-sourced-nexus-architecture.md` for complete design.**

## The "Enhance Nexus" Workflow

**Current implementation:**
1. Analyze conversation for insights
2. Update Memory (cross-project preferences)
3. Update MCP files (documentation)
4. Regenerate INDEX.md
5. Auto-commit with message
6. Report summary

**Future (event-sourced):**
1. Analyze conversation for insights
2. Create domain events (with narratives)
3. Update Memory (if needed)
4. Regenerate affected projections
5. Auto-commit with message
6. Report summary

**Goal:** Token-efficient capture without manual work

## Token Efficiency

**Current approach:**
- Memory: ~500 tokens
- Quick Start: ~2,000 tokens
- INDEX.md: ~2,000 tokens
- On-demand docs: 0-20K tokens
- **Total upfront: ~4,500 tokens**

**Event-sourced approach:**
- Memory: ~500 tokens
- Quick Start: ~2,000 tokens
- INDEX (projection): ~2,000 tokens
- On-demand projections: 0-20K tokens
- **Total upfront: ~4,500 tokens** (same!)

**Difference:** What's behind the scenes
- Current: Static files edited directly
- Future: Projections generated from events

## How Nexus Works Technically

### Read-on-Request Pattern (Current)
```fsharp
let getResource uri = async {
    let path = uriToPath uri
    let! content = File.ReadAllTextAsync(path)
    return { Uri = uri; Text = content; MimeType = "text/markdown" }
}
```

### Event-Sourced Pattern (Future)
```fsharp
let getProjection projType = async {
    // Check if stale
    let staleness = checkStaleness projType
    
    // Regenerate if needed
    if isStale staleness then
        do! regenerateFromEvents projType
    
    // Return cached projection
    return! loadProjection projType
}
```

## MCP Server Components

### Tools (Actions LLM Can Take)

**Current:**
- `update_documentation` - Edit files directly

**Future:**
- `create_event` - Create domain/system events
- `enhance_nexus` - High-level workflow
- `regenerate_projection` - Force refresh
- `query_events` - Search/filter events

### Resources (Content LLM Can Read)

**Current:**
- Documentation files from context-library

**Future:**
- Projections (docs, timeline, metrics)
- Event streams (filtered access)
- Projection status (staleness)

### Prompts (Guided Workflows)

**Current:**
- None defined

**Future:**
- `enhance_nexus` - Multi-step workflow
- `create_technical_decision` - Guided event creation
- `document_design_iteration` - Capture evolution

## Transition Strategy

**Phase 1: Current (Working)**
- CRUD documentation editing
- INDEX.md for discovery
- Hybrid projection caching design complete

**Phase 2: Hybrid Start**
- Keep current docs as-is
- Add events for NEW insights
- Build first projection (timeline or metrics)

**Phase 3: Gradual Migration**
- Slowly convert docs to event-sourced
- Validate projections working
- Learn what works

**Phase 4: Full Event-Sourced**
- All knowledge as events
- Documentation as projection
- Content strategy active

## Benefits

**Current system:**
- ✅ Token efficiency (70% savings)
- ✅ On-demand loading
- ✅ Always up-to-date
- ❌ Loses information (CRUD)
- ❌ Limited content reuse

**Event-sourced system:**
- ✅ Token efficiency (same)
- ✅ On-demand loading
- ✅ Always up-to-date
- ✅ Never loses information
- ✅ Content infrastructure
- ✅ Framework validation

## Success Metrics

**Nexus is working well when:**
- Conversations feel informed by shared history
- Insights captured without interrupting flow
- Token usage comfortable (< 75%)
- Evolution visible in git (current) or events (future)
- Finding information instant
- Updates happen automatically
- No "holes or gaps" in knowledge

---

*Nexus started as token optimization, evolved into integrated context, and will become narrative infrastructure through event sourcing - each step building on the last.*