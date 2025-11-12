---
id: df3bde9d-8130-47d0-8fe8-c6298f6293d3
type: TechnicalDecision
title: "Version 0.2.0 - MCP Prompts Capability"
occurred_at: 2025-11-11T19:00:02.012-05:00
technical_decision:
---

## Decision

Bumped server version to 0.2.0 to reflect major new capability: MCP Prompts.

## Version Scheme

Following semantic versioning for MCP server:
- **0.1.0** - Initial implementation (Resources + Tools)
- **0.2.0** - Added Prompts capability
- **0.3.0** - Will add advanced features (sampling, etc.)

## Capabilities Advertisement

Updated initialize handler to advertise new capability:

```fsharp
Capabilities = box {|
    resources = {| listChanged = false |}
    prompts = {| listChanged = false |}
|}
```

## Why This Matters

MCP clients (like Claude Desktop) use capabilities to:
1. Determine what features are available
2. Show/hide UI elements (e.g., prompts dropdown)
3. Validate protocol compatibility

## Backwards Compatibility

- Clients that don't support prompts will ignore the capability
- Resources and tools continue to work unchanged
- Progressive enhancement approach

## Testing Checkpoint

After deploying 0.2.0:
- [ ] Verify prompts capability advertised
- [ ] Verify Claude Desktop shows prompts UI
- [ ] Verify prompts/list returns 4 prompts
- [ ] Verify prompts/get returns continuation context

## Next Version

0.3.0 will likely add:
- Advanced prompt arguments
- Dynamic prompt generation
- Prompt templates
