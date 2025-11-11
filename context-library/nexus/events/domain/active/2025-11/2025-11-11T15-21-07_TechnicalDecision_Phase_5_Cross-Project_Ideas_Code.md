---
id: 691cca29-7db3-4ec9-a0f3-d3c5bdd74fce
type: TechnicalDecision
title: "Phase 5: Cross-Project Ideas (Code)"
occurred_at: 2025-11-11T15:21:07.560-05:00
tags:
  - phase-5
  - cross-project
  - ideas
  - code
---

# Phase 5: Cross-Project Idea Capture

## Goal
Implement auto-detection and capture of cross-project ideas.

## Code Changes

### 1. Add CrossProjectIdea event structure
```fsharp
type CrossProjectIdea = {
    SourceProject: string
    TargetProject: string
    Idea: string
    Priority: Priority
    Status: IdeaStatus
    ContextLink: string option
}

type Priority = Consider | Important | Low
type IdeaStatus = Pending | Exploring | Implemented | Rejected
```

### 2. Add capture_idea tool
```fsharp
let captureCrossProjectIdea sourceProject targetProject idea priority =
    let event = {
        Type = CrossProjectIdea
        SourceProject = sourceProject
        TargetProject = targetProject
        Idea = idea
        Priority = priority
        Status = Pending
        ContextLink = None
    }
    writeEvent targetProject event
```

### 3. Add pending-ideas projection
```fsharp
let generatePendingIdeasProjection project =
    let ideas = 
        readAllCrossProjectIdeas project
        |> List.filter (fun i -> i.Status = Pending || i.Status = Exploring)
        |> List.groupBy (fun i -> i.SourceProject)
    
    formatPendingIdeas ideas
```

### 4. Integrate with continuation
```fsharp
let handleContinueSession project =
    let sessionState = readLatestSessionState project
    let pendingIdeas = readPendingIdeas project
    
    sprintf """
    %s
    
    You also have %d pending ideas from other projects:
    %s
    """ sessionState.Summary (List.length pendingIdeas) 
        (formatIdeasSummary pendingIdeas)
```

## Testing

1. Create test idea:
   ```bash
   nexus capture-idea --source laundrylog --target perdiem \
     --idea "Reuse location picker" --priority consider
   ```
2. Verify event created in events/perdiem/
3. Generate projection: `nexus regenerate-projection pending-ideas perdiem`
4. Test continuation shows idea
5. Test in Claude: mention cross-project idea, verify auto-detection

## Success Criteria
- ✅ CrossProjectIdea events create successfully
- ✅ Pending-ideas projection generates correctly
- ✅ Continuation includes pending ideas
- ✅ Ideas correctly scoped to target project
- ✅ Status lifecycle works (pending → exploring → etc)

## Rollback
New event type, doesn't affect existing functionality.
