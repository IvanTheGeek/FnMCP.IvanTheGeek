---
id: 68a462a8-0378-4f4a-b44f-49757b4074a5
type: TechnicalDecision
title: "Phase 6: Rename and Bootstrap Files (Manual)"
occurred_at: 2025-11-11T15:21:07.560-05:00
tags:
  - phase-6
  - rename
  - bootstrap
  - manual
  - claude-projects
---

# Phase 6: Repository Rename and Bootstrap

## Goal
Rename to FnMCP.Nexus and create minimal bootstrap files.

## Manual Steps

### 1. GitHub repository rename
```bash
# On GitHub:
# Settings → Repository name → FnMCP.Nexus
# GitHub auto-redirects old URLs
```

### 2. Update local git remote
```bash
cd /home/linux/RiderProjects/FnMCP.IvanTheGeek
git remote set-url origin https://github.com/IvanTheGeek/FnMCP.Nexus.git
```

### 3. Rename local directory
```bash
cd /home/linux/RiderProjects/
mv FnMCP.IvanTheGeek FnMCP.Nexus
```

### 4. Update solution and project files
```bash
cd /home/linux/RiderProjects/FnMCP.Nexus/
# Edit .sln file: Replace IvanTheGeek with Nexus
# Edit src/FnMCP.IvanTheGeek/FnMCP.IvanTheGeek.fsproj
# Rename to: src/FnMCP.Nexus/FnMCP.Nexus.fsproj
mv src/FnMCP.IvanTheGeek src/FnMCP.Nexus
```

### 5. Update namespace in F# files
```fsharp
// Find/replace in all .fs files:
namespace FnMCP.IvanTheGeek → namespace FnMCP.Nexus
module FnMCP.IvanTheGeek. → module FnMCP.Nexus.
```

### 6. Create Claude Projects with bootstrap files

**Create LaundryLog Project:**
- New Claude Project: "LaundryLog"
- Add MCP server (same FnMCP.Nexus)
- Project Knowledge file:

```markdown
# LaundryLog - Nexus Bootstrap

**Project:** laundrylog
**MCP:** FnMCP.Nexus at /home/linux/RiderProjects/FnMCP.Nexus/nexus/

## Startup
1. Type "continue chat" or click Continue Session prompt
2. Load framework: Use MCP for core/framework/
3. Load project: projects/laundrylog/docs/

## Token Monitoring
Display after EVERY response. See core/framework/overview.md for format.

**Total:** ~500 tokens (vs 7K previously!)
```

**Repeat for PerDiemLog and Nexus Projects**

### 7. Update MCP server deployment path
```bash
# Binary already at /home/linux/Nexus/nexus ✓
# Update MCP config if paths changed
```

### 8. Add projection normalization
```fsharp
// In projection generators
let normalizeProjectName = function
    | "FnMCP.IvanTheGeek" -> "FnMCP.Nexus"
    | other -> other
```

## Testing

1. Build renamed project: `dotnet build`
2. Deploy new binary: `cp bin/.../nexus /home/linux/Nexus/nexus`
3. Restart Claude Desktop
4. Test MCP connection
5. Create test event, verify paths work
6. Test continuation in new Claude Projects
7. Verify bootstrap files load correctly

## Success Criteria
- ✅ Repository renamed on GitHub and locally
- ✅ Solution/project files updated
- ✅ Namespaces updated in all code
- ✅ Project compiles and deploys
- ✅ MCP server works with new name
- ✅ Claude Projects created with bootstrap files
- ✅ Token consumption reduced (~500 vs ~7000)
- ✅ Historical events display with normalized names

## Rollback
GitHub maintains redirect, code rename is reversible via git.
