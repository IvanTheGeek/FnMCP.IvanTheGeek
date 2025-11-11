---
id: 15671799-4def-40a7-b7c5-738f1b5274ad
type: TechnicalDecision
title: "Grand Refactor Plan - CQRS Repository with Continuation System"
occurred_at: 2025-11-11T15:21:07.557-05:00
tags:
  - planning
  - migration
  - architecture
  - cqrs
---

# Overview

Massive evolution planned for Nexus based on iterative architecture discussion:

## Key Changes

1. **Repository restructure** - CQRS separation (events/ vs core/ vs projects/)
2. **Project scoping** - Per-project folders for token efficiency
3. **New event types** - MethodologyInsight, NexusInsight, SessionState, CrossProjectIdea
4. **Event naming** - Add GUID suffix, use UTC timestamps
5. **Continuation system** - MCP prompts + keyword + projections
6. **Bootstrap files** - Minimal Project Knowledge per Claude Project
7. **Rename** - FnMCP.IvanTheGeek → FnMCP.Nexus

## Success Criteria

- Token consumption reduced 50% for project-scoped conversations
- New chats can continue seamlessly with 'continue chat' keyword
- Events properly scoped per project
- All event types working
- Repository structure clean and scalable

## Implementation Strategy

Small, verifiable steps to avoid overwhelming Claude Code.
Each phase builds on previous, with testing between phases.
