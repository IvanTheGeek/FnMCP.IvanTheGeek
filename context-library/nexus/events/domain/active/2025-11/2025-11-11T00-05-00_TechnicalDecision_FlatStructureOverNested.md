---
id: f6e5d4c3-b2a1-4f5e-8d9c-0e1f2a3b4c5d
type: TechnicalDecision
title: "Flat Structure Over Nested"
summary: "Chose flat file structure for Phase 1 instead of nested Domain/Projections folders"
occurred_at: 2025-11-11T00:05:00.000-05:00
tags:
  - architecture
  - file-structure
  - simplicity
author: Ivan
technical_decision:
  status: decided
  decision: "Use flat structure in src/FnMCP.IvanTheGeek/ for Phase 1 implementation"
  context: "Original prompt suggested nested Domain/Events.fs, Domain/EventWriter.fs, Projections/Timeline.fs structure"
  consequences: "Simpler to maintain initially, easier to refactor later if needed, follows YAGNI principle"
---

# Flat Structure Over Nested

## Context

The event-sourced architecture document and the implementation prompt suggested a nested folder structure:
```
src/
├── Domain/
│   ├── Events.fs
│   ├── EventWriter.fs
│   └── Projections.fs
├── Projections/
│   └── Timeline.fs
```

## Decision

Implemented with flat structure instead:
```
src/FnMCP.IvanTheGeek/
├── Events.fs    # Contains types, writer, and timeline reading
├── Tools.fs     # Contains tool definitions
```

## Reasoning

1. **YAGNI**: Don't need the complexity yet
2. **Faster iteration**: Fewer files to navigate during development
3. **F# module system**: Modules provide logical separation without needing folders
4. **Easy refactoring**: Can split later if files grow large

## Consequences

**Positive:**
- Faster to build Phase 1
- Easier to understand for new contributors
- Less ceremony

**Negative:**
- May need refactoring if Events.fs grows beyond ~300 lines
- Less obvious separation of concerns at filesystem level

**Mitigation:**
- Monitor file sizes
- Use clear module boundaries within files
- Refactor to nested structure in Phase 2 if needed
