# FnMCP.IvanTheGeek

An MCP (Model Context Protocol) server implementation in F# that exposes context library documentation as resources.

## Overview

This MCP server provides access to a context library containing documentation about applications and frameworks. It implements the MCP protocol version 2025-06-18 and communicates via JSON-RPC over stdio.

### Features

- **Resources**: Exposes markdown files from the context library as MCP resources
- **Protocol**: Standard MCP JSON-RPC over stdio
- **Content Provider**: File system-based content provider for documentation

### Current Capabilities

- `resources/list` - Lists all available documentation resources
- `resources/read` - Reads specific resource content by URI
- `tools/list` - Placeholder for future tool implementations
- `prompts/list` - Placeholder for future prompt implementations

## Building the Server

```bash
dotnet build src/FnMCP.IvanTheGeek/FnMCP.IvanTheGeek.fsproj
```

## Running the Server

The server accepts an optional command-line argument for the context library path:

```bash
dotnet run --project src/FnMCP.IvanTheGeek [context-library-path]
```

If no path is provided, it defaults to `context-library` in the project root.

## MCP Client Configuration

### Claude Desktop / Junie

The server is configured in `~/.config/Claude/claude_desktop_config.json` (Linux):

```json
{
  "mcpServers": {
    "FnMCP.IvanTheGeek": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/home/linux/RiderProjects/FnMCP.IvanTheGeek/src/FnMCP.IvanTheGeek"
      ],
      "env": {}
    }
  }
}
```

**Note**: After updating the configuration, restart Claude Desktop for changes to take effect.

### Other MCP Clients

For other MCP-compatible clients, configure them to run:
- **Command**: `dotnet`
- **Arguments**: `run --project <full-path-to-project>`
- **Communication**: stdio (JSON-RPC)

## Context Library Structure

The context library is organized as:

```
context-library/
├── apps/
│   └── laundrylog/
│       └── overview.md
└── framework/
    └── overview.md
```

Each markdown file becomes an MCP resource accessible via its URI path.

## Development

### Project Structure

- `src/FnMCP.IvanTheGeek/` - Main source code
  - `McpServer.fs` - Core MCP server implementation
  - `Program.fs` - Entry point and JSON-RPC message loop
  - `Types.fs` - MCP protocol types
  - `ContentProvider.fs` - Content provider interface
  - `FileSystemProvider.fs` - File system-based provider
  - `Resources.fs` - Resource management
  - `Tools.fs` - Tools implementation (placeholder)
  - `Prompts.fs` - Prompts implementation (placeholder)

### Requirements

- .NET 9.0 SDK
- F# compiler

## Protocol Details

The server implements MCP protocol version `2025-06-18` using JSON-RPC 2.0:
- **Input**: JSON-RPC requests via stdin
- **Output**: JSON-RPC responses via stdout
- **Logging**: Diagnostic logs via stderr

## Status

✅ **Currently Available for Junie**

The server is configured and ready to use with Junie. Resources from the context library can be accessed through the MCP protocol.