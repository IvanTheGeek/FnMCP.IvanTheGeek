---
id: f63cf080-ea0e-4eb2-85dc-6d17e34f3c34
type: MethodologyInsight
title: "Communication Pattern - Iterative Question Workflow"
summary: "Iterative question-answering pattern for multi-part decisions"
occurred_at: 2025-11-11T21:11:19.226-05:00
tags:
  - communication
  - workflow
  - pattern
  - user-preference
---

# Pattern Established

When asking multiple questions, use this workflow:

**Step 1: Present Overview**
Show all questions/options upfront so user understands full scope:
```
I have 3 questions about this:
1. Should we use approach A or B?
2. What timeline constraints exist?
3. Who needs to approve this?
```

**Step 2: Accept One Answer at a Time**
User will respond to one question per message:
```
User: "Use approach A"
```

**Step 3: Process and Iterate**
Acknowledge the answer, process it, then ask next question:
```
Claude: "Got it - approach A selected. Now, what timeline constraints exist?"
```

**Step 4: Continue Until Complete**
Repeat process for each question until all answered.

## Why This Works

**Supports scattered availability:**
- Complete one decision before moving to next
- Natural breakpoints between driving shifts
- Focused attention on single topic
- Easy to resume if interrupted

**Token efficiency:**
- Shorter exchanges per message
- Less need to re-summarize context
- Can complete partial progress

**Clarity:**
- User sees full scope upfront (overview)
- But only processes one decision at a time
- No cognitive overload from bulk questions
- Clear progression through topics

## Implementation

**Don't do this:**
```
Question 1?
Question 2?
Question 3?
[Wait for all three answers in one response]
```

**Do this:**
```
I have 3 questions: Q1, Q2, Q3.
Starting with Q1?
[Wait for Q1 answer]
Got it. Now Q2?
[Wait for Q2 answer]
Perfect. Finally, Q3?
```

## Related Patterns

Works well with:
- "Ask before expensive work" preference
- "Work in focused bursts" approach
- Truck driver schedule constraints
- Event-driven conversation flow
