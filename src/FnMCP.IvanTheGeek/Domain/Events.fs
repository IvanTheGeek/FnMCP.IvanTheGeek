namespace FnMCP.IvanTheGeek.Domain

open System

// Event type definitions for event-sourced Nexus
// Following F# conventions: discriminated unions and records

type EventType =
    | TechnicalDecision
    | DesignNote
    | ResearchFinding
    | FrameworkInsight

with
    member this.AsString =
        match this with
        | TechnicalDecision -> "TechnicalDecision"
        | DesignNote -> "DesignNote"
        | ResearchFinding -> "ResearchFinding"
        | FrameworkInsight -> "FrameworkInsight"

    static member Parse(str: string) =
        let s = if isNull str then "" else str
        match s.Trim().ToLowerInvariant() with
        | "technicaldecision" | "technical_decision" | "decision" -> TechnicalDecision
        | "designnote" | "design_note" | "note" -> DesignNote
        | "researchfinding" | "research_finding" | "finding" -> ResearchFinding
        | "frameworkinsight" | "framework_insight" | "insight" -> FrameworkInsight
        | other -> failwith ($"Unknown event type: {other}")

type TechnicalDecisionDetails = {
    Status: string option // e.g., proposed | decided | superseded
    Decision: string option
    Context: string option
    Consequences: string option
}

type EventMeta = {
    Id: Guid
    Type: EventType
    Title: string
    Summary: string option
    OccurredAt: DateTime
    Tags: string list
    Author: string option
    Links: string list
    Technical: TechnicalDecisionDetails option
}

type TimelineItem = {
    Path: string
    Title: string
    Type: string
    OccurredAt: DateTime
}
