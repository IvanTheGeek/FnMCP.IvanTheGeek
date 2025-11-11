---
id: 3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f
type: TechnicalPattern
title: "F# Namespace Module Conflict Resolution"
summary: "F# doesn't allow module and namespace with same fully-qualified name in one assembly"
occurred_at: 2025-11-11T01:03:00.000-05:00
tags:
  - fsharp
  - namespaces
  - modules
  - compiler-errors
  - patterns
author: Claude
---

# F# Namespace Module Conflict Resolution

## The Problem

During refactoring, encountered this error:

```
error FS0247: A namespace and a module named 'FnMCP.IvanTheGeek.Tools'
both occur in two parts of this assembly
```

## The Cause

**Two files with conflicting declarations:**

`Tools/EventTools.fs`:
```fsharp
namespace FnMCP.IvanTheGeek.Tools

module EventTools =
    // ...
```
Full name: `FnMCP.IvanTheGeek.Tools.EventTools`

`Tools.fs`:
```fsharp
module FnMCP.IvanTheGeek.Tools

let getTools = ...
```
Full name: `FnMCP.IvanTheGeek.Tools`

**Conflict:** Both `namespace` and `module` trying to claim `FnMCP.IvanTheGeek.Tools`.

## Why F# Prohibits This

**Modules ARE namespaces** in F#:
- `module Foo.Bar` creates namespace `Foo.Bar` containing module `Bar`
- `namespace Foo.Bar` + `module Baz` creates namespace `Foo.Bar` containing module `Baz`
- Can't have both competing for same name

**Type system clarity:**
- `FnMCP.IvanTheGeek.Tools` must resolve to ONE thing
- Is it a module? Is it a namespace?
- F# requires unambiguous resolution

## The Solution

**Option 1: Rename the module**
```fsharp
// Tools.fs
namespace FnMCP.IvanTheGeek

module ToolRegistry =  // Different name!
    let getTools = ...
```

**Option 2: Use nested modules**
```fsharp
// Tools.fs
namespace FnMCP.IvanTheGeek.Tools

module Registry =  // Nested under namespace
    let getTools = ...
```

**Option 3: Flatten to namespace only**
```fsharp
// Tools.fs
namespace FnMCP.IvanTheGeek.Tools

// No module declaration, just functions
let getTools = ...
```

## Chosen Solution

**Option 1: Rename to ToolRegistry**

Reasons:
- Clear intent (it's a registry of tools)
- Minimal refactoring needed
- Maintains existing call sites pattern
- EventTools keeps its natural name

## Pattern for Future

**When organizing F# code with folders:**

```
Tools/
├── EventTools.fs    → namespace Foo.Tools / module EventTools
├── OtherTools.fs    → namespace Foo.Tools / module OtherTools
└── Tools.fs         → namespace Foo / module ToolRegistry
```

OR

```
Tools/
├── EventTools.fs    → namespace Foo.Tools / module EventTools
├── OtherTools.fs    → namespace Foo.Tools / module OtherTools
└── Registry.fs      → namespace Foo.Tools / module Registry
```

**Key principle:** Namespace hierarchy != Module hierarchy

## Related Patterns

**C# doesn't have this issue:**
- Namespaces and classes are distinct
- `namespace Foo.Bar` + `class Tools` = `Foo.Bar.Tools` (the class)
- No conflict

**F# modules are different:**
- Modules ARE compilation units
- Module name becomes part of type path
- Must be globally unique within assembly

## Debugging Tips

**When you see FS0247:**
1. Search for `namespace X` across all files
2. Search for `module X` across all files
3. Look for duplicate fully-qualified names
4. Rename one of them

**Quick fix:**
- Add suffix: `Tools` → `ToolRegistry`
- Add prefix: `Tools` → `ToolsModule`
- Nest deeper: `module Tools` → `module Core.Tools`

---

*F# namespace/module duality: powerful but requires discipline in naming.*
