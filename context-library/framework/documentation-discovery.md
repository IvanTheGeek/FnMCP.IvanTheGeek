# Documentation Discovery Strategy

**Framework:** FnMCP.IvanTheGeekDevFramework  
**Purpose:** Solve discoverability gaps from on-demand MCP loading  
**Updated:** 2025-11-10  
**Status:** Implementation Ready

## The Problem

**Before Nexus (all docs in Project Knowledge):**
- Claude could see everything at once
- No need to guess which file to fetch
- 15K token overhead, but complete visibility

**After Nexus (on-demand via MCP):**
- 87% token savings ✓
- BUT: Claude must guess which files to fetch
- Holes and gaps in knowledge - can't see what exists
- "I documented mobile-first principles... where was that?"

## The Solution: INDEX.md + Frontmatter

### Auto-Generated Index File

Location: `context-library/INDEX.md` (~2K tokens)

```markdown
# Nexus Documentation Index

## Framework Methodology
- **Event Modeling** (`framework/event-modeling-approach.md`)
  - Commands, Events, Views, Slices, Visual collaboration
  - Related: Paths, GWT, Static-State Design
  
- **Paths & GWT** (`framework/paths-and-gwt.md`)
  - Choose Your Own Adventure navigation, concrete examples
  - Related: Event Modeling, Testing, Penpot
  
- **Static-State Design** (`framework/static-state-design.md`)
  - One screen = one state, explicit navigation
  - Related: Mobile-First, LaundryLog design

- **Mobile-First Principles** (`framework/mobile-first-principles.md`)
  - 72px touch targets, thumb zones, smart defaults
  - Related: LaundryLog, UX patterns

## Applications
- **LaundryLog Overview** (`apps/laundrylog/overview.md`)
  - Truck driver expense tracking
  - Related: design-v7-spec, mobile-first-principles, event-model
  
- **LaundryLog v7 Design** (`apps/laundrylog/design-v7-spec.md`)
  - Complete production-ready specification
  - Related: mobile-first-principles, implementation-notes

## Technical
- **F# Conventions** (`technical/f-sharp-conventions.md`)
  - Domain modeling, discriminated unions, modules
  
- **MCP Architecture** (`technical/mcp-architecture.md`)
  - Read-on-request pattern, tool design
  
- **Context Monitoring** (`technical/context-monitoring.md`)
  - Token usage display pattern

## Quick Concept Lookup
- **Mobile UX**: mobile-first-principles.md, laundrylog/design-v7-spec.md
- **Event Modeling**: event-modeling-approach.md, paths-and-gwt.md, domain-modeling-patterns.md
- **Testing**: paths-and-gwt.md, domain-modeling-patterns.md
- **Penpot**: penpot-integration.md, naming-conventions.md
- **GPS/Location**: laundrylog/implementation-notes.md
```

### YAML Frontmatter in Each File

```yaml
---
title: Event Modeling Approach
concepts: [Commands, Events, Views, Slices, Visual Collaboration]
related: 
  - paths-and-gwt
  - static-state-design
  - domain-modeling-patterns
apps: [LaundryLog, PerDiemLog]
updated: 2025-11-10
status: Core Methodology
---
```

**Benefits:**
- Claude can parse without reading full content
- Structured relationship data
- YAML is human-readable (user preference)
- Enables future tooling (graph visualization, etc.)

## Workflow Integration

### "Enhance Nexus" Updates Index
1. Analyze conversation insights
2. Update relevant documentation files
3. **Regenerate INDEX.md** from all frontmatter
4. Auto-commit with meaningful message

### Claude's Discovery Process
1. **Read INDEX.md first** (~2K tokens) - understand what exists
2. **Check frontmatter** if need quick overview
3. **Fetch full content** only when needed for answer
4. **Result:** No more guessing, no holes

## Token Economics

**Overhead:**
- INDEX.md: ~2,000 tokens (read once per conversation)
- Frontmatter: ~100 bytes per file (only if fetched)
- Total: ~2-3K tokens added
- **Still 84% savings vs old approach** (15K → 2.5K)

**Benefit:**
- Complete visibility into available knowledge
- Confident file selection
- Fixes discoverability gaps
- Enables future graph visualization

## Implementation Priority

**Phase 1 (Now):**
- Create initial INDEX.md manually
- Add frontmatter to existing docs
- Update "enhance nexus" to regenerate index

**Phase 2 (Future):**
- Automatic frontmatter updates during enhance
- Cross-reference validation
- Concept graph generation

---

*INDEX.md bridges the gap between token efficiency and knowledge discoverability - best of both worlds.*