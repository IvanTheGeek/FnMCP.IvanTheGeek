---
id: b7e69b2a-5611-4726-971a-59cfdf0745c2
type: FrameworkInsight
title: "Nexus Hierarchy - Philosophy, Methodology, Framework, Projects"
summary: "Clarified Nexus as four-layer hierarchy: Philosophy (WHY), Methodology (HOW), Framework (WHAT tools), Projects (WHAT we build)"
occurred_at: 2025-11-11T14:02:55.152-05:00
tags:
  - architecture
  - hierarchy
  - organization
  - philosophy
  - methodology
---

# Context

Through iterative discussion, we clarified what "Nexus" encompasses and how its components relate. Previously, there was confusion between "framework," "methodology," and the overall system.

# The Hierarchy

**Nexus = Everything (Ivan's digital self)**

Four distinct layers emerged:

## 1. Philosophy/Values (WHY we build)
- Maximum freedom + practical capitalism
- Privacy-first (no data monetization)
- AGPLv3 licensing
- Build for yourself first
- "If you wouldn't use it, don't build it"

## 2. Methodology (HOW we think/work)
- Event Modeling
- Dogfooding
- Paths (execution traces)
- Design iteration
- Static-State Design
- "Done is done" (slice completion)

## 3. Framework (WHAT we use - Tech Stack)
- F# language
- Bolero (web apps)
- Blazor Hybrid + Bolero (desktop)
- fsHTTP (networking)
- Event sourcing (data pattern)
- Penpot (design tool)
- Opinionated library choices

## 4. Projects (WHAT we build)
- LaundryLog (expense tracking)
- PerDiemLog (per diem tracking)
- FnMCP.Nexus (MCP server - provides access to Nexus)
- CheddarBooks (future QuickBooks competitor)

# Key Distinctions

**Methodology vs Framework:**
- Methodology = Abstract approaches (Event Modeling, dogfooding)
- Framework = Concrete tools (F#, Bolero, Penpot)

**Philosophy vs Methodology:**
- Philosophy = Core values and principles (WHY)
- Methodology = How we work and think (HOW)

**Projects vs Framework:**
- Projects = Applications built USING the Framework & Methodology
- Framework = The tools and tech stack used to build projects
- Similar to: React apps vs React framework

# Event Type Mapping

This hierarchy suggests new event types:

- **PhilosophyInsight** → Discoveries about values and principles
- **MethodologyInsight** → Discoveries about how to think/work
- **FrameworkInsight** → Discoveries about tech stack choices
- **NexusInsight** → Discoveries about the system itself
- **TechnicalDecision** → Implementation decisions (any layer)
- **DesignNote** → Design choices (any layer)

# Why This Matters

**For Token Efficiency:**
- Philosophy layer rarely changes → Load once, reference often
- Methodology layer evolves slowly → Update strategically
- Framework layer changes with tech evolution → Track carefully
- Projects layer most active → Scope per conversation

**For Knowledge Organization:**
- Clear semantic boundaries
- Appropriate event types per layer
- Predictable information location
- Scalable as system grows

# Consequences

**Documentation Structure:**
```
core/framework/
├─ philosophy.md     (WHY - values)
├─ methodology.md    (HOW - approaches)
└─ tech-stack.md     (WHAT - tools)
```

**Event Classification:**
- Each layer has appropriate insight types
- Cross-layer decisions use TechnicalDecision/DesignNote
- Clear distinction aids querying and projection generation
