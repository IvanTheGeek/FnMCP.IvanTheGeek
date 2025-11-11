---
type: TechnicalDecision
title: Master Implementation Plan - Complete Checklist
occurredAt: 2025-11-11T15:21:39Z
tags:
  - planning
  - checklist
  - implementation
  - master-plan
summary: Master implementation plan with 6 phases, checklists, and success criteria for complete Nexus evolution
---

# Complete Implementation Plan

## Overview
Comprehensive plan to evolve Nexus with CQRS structure, continuation system, and project scoping. Broken into 6 phases with small, verifiable steps.

## Phase Checklist

### Phase 1: Event Types ✓ Ready to Start
- [ ] Update EventType discriminated union
- [ ] Update event writer validation
- [ ] Update MCP tool validation
- [ ] Build and test
- [ ] Create test events for each new type
- [ ] Verify timeline displays correctly
- [ ] Deploy new binary
- [ ] **Checkpoint:** All 4 new types working

### Phase 2: GUID Naming ✓ After Phase 1
- [ ] Update filename generation with GUID
- [ ] Convert timestamps to UTC with Z suffix
- [ ] Update projection parsers for GUID
- [ ] Build and test
- [ ] Create multiple test events (verify no collisions)
- [ ] Verify timeline parsing
- [ ] Deploy new binary
- [ ] **Checkpoint:** All new events have GUID + UTC

### Phase 3: Repository Restructure ✓ After Phase 2
**Manual steps:**
- [ ] Create new directory structure (nexus/events/, nexus/core/, nexus/projects/)
- [ ] Review and migrate existing events to project folders
- [ ] Migrate documentation to new locations
- [ ] Keep old context-library as backup

**Code steps:**
- [ ] Update MCP server base paths
- [ ] Update event writer project-scoped paths
- [ ] Update projections to read new structure
- [ ] Build and test
- [ ] Test event creation in each project folder
- [ ] Verify timeline reads all projects
- [ ] Verify MCP resources work
- [ ] Deploy new binary
- [ ] **Checkpoint:** All paths working, events in correct folders

### Phase 4: Continuation System ✓ After Phase 3
**Code steps:**
- [ ] Add MCP Prompts capability to server
- [ ] Implement prompt handlers
- [ ] Add SessionState projection generator
- [ ] Add continuation keyword recognition
- [ ] Build and test

**Manual steps:**
- [ ] Create bootstrap file for LaundryLog Project
- [ ] Create bootstrap file for PerDiemLog Project  
- [ ] Create bootstrap file for Nexus Project
- [ ] Update MCP config if needed
- [ ] Create test SessionState event
- [ ] Test MCP prompt in Claude Desktop
- [ ] Test 'continue chat' keyword
- [ ] Measure token savings
- [ ] **Checkpoint:** Continuation working, token savings verified

### Phase 5: Cross-Project Ideas ✓ After Phase 4
- [ ] Add CrossProjectIdea event structure
- [ ] Add capture_idea tool
- [ ] Add pending-ideas projection generator
- [ ] Integrate with continuation handler
- [ ] Build and test
- [ ] Create test cross-project idea
- [ ] Verify projection generates
- [ ] Test continuation shows ideas
- [ ] Test auto-detection (if implemented)
- [ ] Deploy new binary
- [ ] **Checkpoint:** Cross-project ideas captured and surfaced

### Phase 6: Rename & Bootstrap ✓ After Phase 5
**Manual steps:**
- [ ] Rename GitHub repository to FnMCP.Nexus
- [ ] Update local git remote URL
- [ ] Rename local directory
- [ ] Update solution file (.sln)
- [ ] Rename project directory and .fsproj
- [ ] Find/replace namespace in all .fs files
- [ ] Add projection normalization for old name
- [ ] Build and test
- [ ] Deploy binary
- [ ] Create LaundryLog Claude Project with bootstrap
- [ ] Create PerDiemLog Claude Project with bootstrap
- [ ] Update Nexus Claude Project with bootstrap
- [ ] Test all three Projects
- [ ] Verify token consumption (~500 vs ~7000)
- [ ] **Checkpoint:** Complete system operational

## Success Metrics

After all phases complete:
- ✅ Repository renamed to FnMCP.Nexus
- ✅ Events use GUID + UTC naming
- ✅ Events organized by project (CQRS structure)
- ✅ 4 new event types working
- ✅ Continuation system functional
- ✅ Cross-project ideas captured
- ✅ Claude Projects created with bootstrap files
- ✅ Token consumption reduced 50%+ per conversation
- ✅ New chats continue seamlessly
- ✅ All projections working

## Risk Mitigation

**After each phase:**
1. Test thoroughly before moving to next phase
2. Keep previous binary as rollback option
3. Backup data before manual migrations
4. Commit code changes to git
5. Document any issues encountered

**If problems occur:**
- Rollback to previous binary
- Restore from backup if needed
- Address issues before continuing
- Small steps = easier debugging

## Notes

- Don't rush - each phase builds on previous
- Test extensively between phases
- Claude Code tools may need CLI workarounds
- Session events create natural checkpoints
- Cross-project ideas emerge organically during work
- Work happens when opportunity allows
- Phases show progression, not timeline