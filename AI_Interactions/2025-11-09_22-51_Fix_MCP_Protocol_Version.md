# Fix MCP Protocol Version

**Date:** 2025-11-09 22:51  
**Status:** Completed

## User Prompt

```
in Rider setting under Junie > MCP Settings I have added the config for the FnMCP.IvanTheGeek server. The status column is giving the following error:

Error connecting to transport: Server's protocol version is not supported: 2025-06-18

[FnMCP.IvanTheGeek] FnMCP.IvanTheGeek MCP Server starting... [FnMCP.IvanTheGeek] Protocol version: 2025-06-18 [FnMCP.IvanTheGeek] Context library path: /home/linux/RiderProjects/FnMCP.IvanTheGeek/context-library [FnMCP.IvanTheGeek] Server initialized. Ready to receive JSON-RPC requests on stdin. [FnMCP.IvanTheGeek] Logging to stderr. JSON-RPC responses on stdout. [FnMCP.IvanTheGeek] Received request: initialize [FnMCP.IvanTheGeek] EOF received, shutting down. [FnMCP.IvanTheGeek] Server shutting down.

how do we fix this?
```

## Problem Analysis

The FnMCP.IvanTheGeek MCP server was using protocol version "2025-06-18", which is not supported by Rider's Junie MCP client. The error indicated that Rider rejected the connection due to the unsupported protocol version. The server would initialize, receive the initialize request, but then Rider would terminate the connection.

## Solution

Updated the MCP protocol version from "2025-06-18" to "2024-11-05", which is the current stable version supported by Rider and other MCP clients.

### Changes Made

1. **src/FnMCP.IvanTheGeek/McpServer.fs**
   - Line 13: Updated `ProtocolVersion = "2025-06-18"` to `ProtocolVersion = "2024-11-05"` in HandleInitialize response
   - Line 55: Updated `ProtocolVersion = "2025-06-18"` to `ProtocolVersion = "2024-11-05"` in HandleRequest method

2. **src/FnMCP.IvanTheGeek/Program.fs**
   - Line 100: Updated log message from `Protocol version: 2025-06-18` to `Protocol version: 2024-11-05`

### Verification

After rebuilding the project, the server now correctly reports:
```
[FnMCP.IvanTheGeek] FnMCP.IvanTheGeek MCP Server starting...
[FnMCP.IvanTheGeek] Protocol version: 2024-11-05
[FnMCP.IvanTheGeek] Context library path: /home/linux/RiderProjects/FnMCP.IvanTheGeek/context-library
[FnMCP.IvanTheGeek] Server initialized. Ready to receive JSON-RPC requests on stdin.
[FnMCP.IvanTheGeek] Logging to stderr. JSON-RPC responses on stdout.
```

The server should now be compatible with Rider's Junie MCP Settings and connect without errors.

## Files Modified

- `src/FnMCP.IvanTheGeek/McpServer.fs` - Updated protocol version in 2 locations
- `src/FnMCP.IvanTheGeek/Program.fs` - Updated protocol version in startup log

## Next Steps

The user should now restart or reconnect the MCP server in Rider's Junie > MCP Settings to verify the connection works properly without the protocol version error.
