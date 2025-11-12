---
id: 2344768b-6ffd-4bb8-87f0-c199ffc64813
type: DesignNote
title: "Memory Content Updated - Phase 4 and Bootstrap Pattern"
summary: "Complete memory update reflecting Phase 4 completion and architectural patterns"
occurred_at: 2025-11-11T20:34:23.638-05:00
tags:
  - memory
  - documentation
  - phase-4
  - architecture
---

# Updated Memory Content

## Purpose & Context
Ivan is building a comprehensive development framework called "FnMCP.IvanTheGeekDevFramework" centered on Event Modeling methodology, F# programming with Bolero web framework, and Penpot for design. As a professional truck driver who develops open-source software, Ivan creates tools to solve real problems he encounters, particularly financial applications under the "Cheddar" ecosystem brand. His development philosophy emphasizes "maximum freedom + practical capitalism" with AGPLv3 licensing, privacy-first design, and building software that's free with optional paid services.

The framework uses innovative concepts including "paths" (inspired by Choose Your Own Adventure books where one system contains multiple possible storylines), multi-layer Event Models with perspective filtering, and static-state design where each screen represents a discrete state. Ivan is using LaundryLog (expense tracking for truck drivers) and PerDiemLog (per diem tracking for DOT HOS compliance) as proving ground applications to test and refine framework concepts. The broader vision includes CheddarBooks (QuickBooks competitor), FnTools (technical infrastructure), and an "influence system" where user contributions earn feature request prioritization.

## Current State

**Phase 4 Complete:** FnMCP.Nexus MCP server successfully deployed with continuation prompt capability. The server provides four prompts (continue-core, continue-laundrylog, continue-nexus, continue-perdiem) that load latest SessionState events for zero-context session continuation. This enables 60-80% token savings per new session by automatically loading relevant context.

**Bootstrap File Pattern Established:** Single minimal file (~500 tokens) for Project Knowledge that teaches Claude the Nexus protocol: check recent work via recent_chats(), load timeline via timeline_projection(), fetch documentation on-demand via MCP. Achieves ~92% token reduction in Project Knowledge (6,000 → 500 tokens) while maintaining full access to detailed documentation stored in context-library.

**Project Separation Strategy:** Active development stays together in Nexus project (framework + FnMCP.Nexus server + LaundryLog dogfooding) for tight feedback loops. Apps graduate to separate Claude Projects when patterns stabilize, using minimal bootstrap files that share the Nexus MCP infrastructure. PerDiem will serve as test case for multi-project pattern.

**LaundryLog v7:** Complete HTML prototype with mobile-first design, thumb-friendly controls, GPS location tracking, and validation states. Ready for F# + Bolero implementation. Seven design iterations have established critical mobile UX patterns.

**Documentation Architecture:** Two-tier system with Quick Start files for essential context and detailed resources served on-demand via MCP. Event sourcing captures all decisions in domain events (stored in nexus/events/) with projections generated for different views (timeline, knowledge, metrics). All documentation cross-referenced for cohesive knowledge base.

## On the Horizon

**Next implementations:** SessionState projection generation for automatic continuation context, creating PerDiemLog in separate project to validate multi-project pattern, beginning LaundryLog F# + Bolero implementation to dogfood framework patterns.

**PerDiemLog Phase 1:** Design focuses on manual per diem tracking with IRS rate calculations, trip duration handling, and DOT HOS compliance. Future phases will add automated PDF downloads (Phase 2) and potential PDF parsing (Phase 3).

**Forum evolution:** Moving from markdown files through interim solutions to eventually a custom event-sourced forum as central content and community hub.

## Key Learnings & Principles

**MCP Server Patterns:** Use AppContext.BaseDirectory (not __SOURCE_DIRECTORY__) for deployed binaries. Read-on-request pattern perfect for markdown documentation - provides microsecond performance while eliminating caching complexity. Proper path validation essential for security.

**Paths Concept:** Deterministic execution traces using concrete example data, serving simultaneously as narratives, Penpot prototypes, test cases, documentation, and event model validation. Each path follows one complete route through all decision points (happy, error, edge, alternative paths).

**Event Sourcing:** All work captured in domain events. Write side (events/) is immutable source of truth. Read side (projections, docs) derived and optimized for querying. CQRS separation enables project-scoped filtering and token efficiency.

**Token Management:** Context monitoring critical for truck driver workflow where sessions happen in scattered pockets of time. Visual bar with exact formula: `filled = round(percentage / 5)` for 20-character display. See technical/context-monitoring.md for complete specification.

## Approach & Patterns

**Development methodology:** Event Modeling with static-state design where each screen represents discrete state. Phased approach for major features, moving from manual to automated solutions. Design progresses through iterative HTML mockups with mobile-first focus.

**Documentation strategy:** Modular structure with cross-references. Concrete examples over abstract placeholders. YAML for data files (more human-readable than JSON). Never include time estimates - focus on phases and steps instead of duration.

**Project structure:** Gradual separation pattern keeps active development together until patterns stabilize. Apps graduate to separate Claude Projects using minimal bootstrap files that share Nexus MCP infrastructure. Each project loads only relevant context (50%+ token savings).

**Quality focus:** Excellent UX critical. Building tools for personal use ensures authentic problem-solving. Mobile-first with 72px touch targets, thumb-friendly placement, button-based interaction (95% of actions), smart defaults that learn from usage.

## Tools & Resources

**Stack:** F# with Bolero web framework, Penpot for design (UI source of truth), MCP protocol for Claude integration, Event Modeling methodology, SQLite for local-first storage.

**MCP Integration:** FnMCP.Nexus server enables direct documentation updates (update_documentation tool), on-demand resource loading, timeline projections, and continuation prompts. Claude Desktop integration with proper binary deployment workflows.

**Penpot workflow:** Use penpotUtils.getPages() for structure exploration, findShape/findShapes for component discovery. Meaningful component names ("button / submit" not generic names) crucial for code generation. Interactions data contains rich prototype flows with timing/animation mapping to path concept.

**Context-library structure:**
```
nexus/
├─ events/          (write side - source of truth)
│  ├─ core/         (Nexus framework)
│  ├─ laundrylog/   (LaundryLog app)
│  ├─ perdiem/      (PerDiem app)
│  └─ fnmcp-nexus/  (MCP server)
├─ framework/       (shared methodology docs)
├─ apps/            (app-specific docs)
└─ technical/       (technical references)
```

## Critical Reminders for Claude

**Starting conversations:**
1. Check continuation prompts for SessionState context
2. Use recent_chats(n=3) to see latest discussions  
3. Call timeline_projection for recent events
4. Load documentation via MCP as needed

**Every response must:**
- Display token usage with accurate 20-char bar
- Never include time estimates
- Use YAML over JSON for data files
- Create events for significant decisions/learnings

**File operations:**
- Use Nexus MCP update_documentation tool
- Reference files by path for on-demand loading
- Bootstrap files are ~500 tokens in Project Knowledge
- Detailed docs served via MCP (no token cost until accessed)

---

This memory structure optimized for session continuation and effective collaboration on Nexus development.
