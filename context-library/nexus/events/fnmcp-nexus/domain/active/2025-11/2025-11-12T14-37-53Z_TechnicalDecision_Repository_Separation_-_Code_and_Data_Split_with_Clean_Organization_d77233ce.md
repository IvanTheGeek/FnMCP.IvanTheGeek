---
id: 0acd41c4-2cef-402e-a952-6402cf2877b0
type: TechnicalDecision
title: "Repository Separation - Code and Data Split with Clean Organization"
summary: "Comprehensive restructuring plan: separate code and data into independent repos, clean organization, automated migration script, manual config instructions."
occurred_at: 2025-11-12T09:37:53.706-05:00
tags:
  - restructuring
  - repository-separation
  - data-organization
  - migration
  - phase-6-extension
technical_decision:
---

## Repository Separation Plan

**Goal:** Separate code and data into independent git repositories with clean organization.

## New Structure

### Three Distinct Locations

1. **Development (code):** `/home/linux/FnMCP.Nexus/`
   - F# MCP server source code
   - Solution and project files
   - Tests
   - Git repository for code

2. **Deployment (binary):** `/home/linux/Nexus/`
   - Compiled binary `nexus`
   - Where MCP server runs from
   - Referenced in Claude Desktop config

3. **Data (knowledge):** `/home/linux/Nexus-Data/`
   - All documentation (apps, framework, technical)
   - All events (domain, system, learning)
   - All projections (timeline, knowledge, metrics)
   - Bootstrap files
   - Git repository for knowledge

## Data Repository Structure

```
/home/linux/Nexus-Data/
├── apps/ (laundrylog, perdiem)
├── framework/ (methodology docs)
├── technical/ (patterns and conventions)
├── quick-start/ (quick reference)
├── bootstrap/ (Project Knowledge files)
├── events/ (project-scoped: core, fnmcp-nexus, laundrylog, perdiem)
├── projections/ (timeline, knowledge, metrics)
└── README.md
```

## Cleanup Actions

**Removed legacy content:**
- Old event structure (pre-Phase 3 flat organization)
- Root-level framework-overview.md duplicate
- test/ directory
- Consolidated bootstrap files into bootstrap/

**Retained only:**
- Current project-scoped event structure (Phase 3+)
- Clean documentation hierarchy
- Active projections

## Migration Process

**Automated:**
- Created migration script: `migrate-to-nexus-data.sh`
- Copies and reorganizes all content
- Skips legacy/duplicate content
- User runs script when ready

**Manual (user handled):**
- Update MCP server config to point to Nexus-Data
- Restart Claude Desktop
- Test MCP access
- Initialize git in Nexus-Data
- Remove old context-library when confirmed

## Benefits

1. **Code vs. Data separation**
   - Independent git repositories
   - Different commit rhythms
   - Clear ownership boundaries

2. **Cleaner organization**
   - No legacy cruft
   - Logical hierarchy
   - Easy navigation

3. **GitHub backup**
   - Data repo can be pushed to GitHub
   - Separate from MCP server code repo
   - Knowledge preserved independently

4. **Flexibility**
   - Data repo can be cloned separately
   - Multiple MCP servers can read same data
   - Documentation independent of code

## Files Created

1. `migrate-to-nexus-data.sh` - Migration script
2. `mcp-config-instructions.md` - Configuration update guide
3. `NEXUS-DATA-README.md` - Repository documentation

## Next Steps

User will:
1. Review migration script
2. Run migration: `bash migrate-to-nexus-data.sh`
3. Update MCP config per instructions
4. Restart Claude Desktop
5. Test in new chat
6. Initialize git in Nexus-Data
7. Create GitHub repository
8. Push to GitHub
9. Remove old context-library

## Memory Updated

Updated user preferences to reflect three distinct locations:
- Code at `/home/linux/FnMCP.Nexus`
- Data at `/home/linux/Nexus-Data`
- Binary at `/home/linux/Nexus/`
