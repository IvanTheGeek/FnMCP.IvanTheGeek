# Nexus MCP Connection Investigation - Protocol Version & SSE Transport Issues

**Date**: 2025-11-20
**Session**: Protocol Version 2025-06-18 Update & SSE Transport Investigation
**Status**: ⚠️ PARTIALLY RESOLVED - Connection still failing
**Commits**: TBD

---

## Problem Summary

The Nexus MCP server was not connecting to Claude Code despite the SSE endpoint URL fix from the previous session (commit 7dc9e2d). Investigation revealed multiple issues with protocol version mismatch and missing capabilities.

---

## Root Causes Identified

### 1. **Protocol Version Mismatch** ✅ FIXED
- **Issue**: Server was using MCP protocol version `2024-11-05` but Claude Code client expected `2025-06-18`
- **Location**: `src/FnMCP.Nexus/McpServer.fs:28` and `:78`
- **Impact**: Client immediately cancelled SSE connection after receiving initialize response with old protocol version

### 2. **Missing Tools Capability** ✅ FIXED
- **Issue**: Initialize response was missing the `tools` capability in the capabilities object
- **Location**: `src/FnMCP.Nexus/McpServer.fs:29-32`
- **Impact**: Client may reject servers that don't declare tool support

### 3. **Incorrect Content-Type Header** ✅ FIXED
- **Issue**: SSE message endpoint was returning `text/plain` instead of `application/json`
- **Location**: `src/FnMCP.Nexus/Transport/SseTransport.fs:106`
- **Root Cause**: Oxpecker's `text` function overrides the Content-Type header
- **Impact**: Client may reject responses with incorrect content type

### 4. **Missing Session ID Header** ✅ ADDED
- **Issue**: Newer MCP specifications require `Mcp-Session-Id` header in initialize responses
- **Location**: `src/FnMCP.Nexus/Transport/SseTransport.fs:105-109`
- **Impact**: May cause connection failures with newer MCP clients

### 5. **Build Dependency Warning** ✅ FIXED
- **Issue**: Oxpecker 0.20.0 not found, resolved to 1.0.0 causing build warnings
- **Location**: `src/FnMCP.Nexus/FnMCP.Nexus.fsproj:85`
- **Impact**: Build warnings, potential compatibility issues

### 6. **Missing .env File on VPS** ✅ FIXED
- **Issue**: VPS deployment didn't have .env file for configuration
- **Impact**: Server relied only on environment variables passed via docker run

---

## Diagnostic Steps Performed

1. **Checked MCP tool visibility**: Confirmed no `mcp__nexus__*` tools available
2. **Verified claude mcp list**: Showed "Failed to connect" status
3. **Tested SSE endpoint with curl**: Endpoint responded correctly
4. **Analyzed server logs**: Found protocol version mismatch and missing capabilities
5. **Tested direct HTTP POST**: Verified server responds correctly to initialize requests
6. **Checked content-type headers**: Discovered text/plain vs application/json issue
7. **Researched MCP specifications**: Found SSE transport deprecated in favor of Streamable HTTP

---

## Fixes Applied

### Fix 1: Update Protocol Version to 2025-06-18
**Files Modified**:
- `src/FnMCP.Nexus/McpServer.fs` (lines 28, 78)
- `src/FnMCP.Nexus/Program.fs` (line 153)

**Changes**:
```fsharp
// Before
ProtocolVersion = "2024-11-05"

// After
ProtocolVersion = "2025-06-18"
```

### Fix 2: Add Tools Capability
**File Modified**: `src/FnMCP.Nexus/McpServer.fs`

**Changes**:
```fsharp
// Before
Capabilities = box {|
    resources = {| listChanged = false |}
    prompts = {| listChanged = false |}
|}

// After
Capabilities = box {|
    resources = {| listChanged = false |}
    prompts = {| listChanged = false |}
    tools = {| listChanged = false |}
|}
```

### Fix 3: Correct Content-Type Header
**File Modified**: `src/FnMCP.Nexus/Transport/SseTransport.fs`

**Changes**:
```fsharp
// Before
ctx.Response.ContentType <- "application/json"
return! ctx |> text responseJson

// After
ctx.Response.ContentType <- "application/json"
do! ctx.Response.WriteAsync(responseJson)
return Some ctx
```

### Fix 4: Add Mcp-Session-Id Header
**File Modified**: `src/FnMCP.Nexus/Transport/SseTransport.fs`

**Changes**:
```fsharp
// Added session ID generation for initialize requests
if jsonRpcRequest.Method = "initialize" then
    let sessionId = System.Guid.NewGuid().ToString("N")
    ctx.Response.Headers.Add("Mcp-Session-Id", sessionId)
    log $"Generated session ID: {sessionId}"
```

### Fix 5: Update Oxpecker Dependency
**File Modified**: `src/FnMCP.Nexus/FnMCP.Nexus.fsproj`

**Changes**:
```xml
<!-- Before -->
<PackageReference Include="Oxpecker" Version="0.20.0" />

<!-- After -->
<PackageReference Include="Oxpecker" Version="1.0.0" />
```

### Fix 6: Add .env File Support
**New File**: `/data/.env` on VPS

**Docker Command Updated**:
```bash
docker run -d --name nexus-mcp \
  -v /data/event-store:/data/event-store \
  -v /data/.env:/app/.env:ro \
  -e NEXUS_TRANSPORT=http \
  -e ASPNETCORE_URLS=http://+:18080 \
  -p 18080:18080 \
  --restart unless-stopped \
  nexus-mcp:latest /data/event-store
```

**Environment Variables**:
- NEXUS_TRANSPORT=http
- NEXUS_MCP_PORT=18080
- ASPNETCORE_URLS=http://+:18080
- CONTEXT_LIBRARY_PATH=/data/event-store
- EMBEDDING_API_URL=http://66.179.208.238:5000/embed
- QDRANT_URL=http://66.179.208.238:6333
- QDRANT_COLLECTION=conversations

### Fix 7: Added Response Logging
**File Modified**: `src/FnMCP.Nexus/Transport/SseTransport.fs`

**Changes**:
```fsharp
log $"Sending response: {responseJson.Substring(0, min 500 responseJson.Length)}..."
```

---

## Verification Results

### ✅ Working
1. Server starts correctly with protocol version 2025-06-18
2. .env file is loaded successfully
3. SSE endpoint sends correct event format: `event: endpoint\ndata: http://66.179.208.238:18080/sse/message`
4. Initialize endpoint returns correct JSON-RPC response with:
   - Protocol version: 2025-06-18
   - Capabilities: tools, prompts, resources
   - Server info: name and version
   - Content-Type: application/json
   - Mcp-Session-Id header
5. No Oxpecker build warnings

### ⚠️ Still Failing
1. Claude Code MCP connection still shows "Failed to connect"
2. `claude mcp list` reports connection failure
3. No MCP tools visible in Claude Code

---

## Current Investigation Status

### Server Response Analysis
Direct curl test to /sse/message endpoint shows correct response:
```bash
$ curl -i -X POST -H "Authorization: Bearer ..." -H "Content-Type: application/json" \
  -d '{"jsonrpc":"2.0","id":1,"method":"initialize",...}' \
  http://66.179.208.238:18080/sse/message

HTTP/1.1 200 OK
Content-Type: application/json
Mcp-Session-Id: <guid>
Date: Thu, 20 Nov 2025 03:27:01 GMT
Server: Kestrel

{"jsonrpc":"2.0","id":1,"result":{"protocolVersion":"2025-06-18","capabilities":{...}}}
```

### Possible Remaining Issues

1. **SSE Transport Deprecation**: Research indicates that SSE transport was deprecated in MCP spec 2025-03-26 in favor of "Streamable HTTP" transport
   - Claude Code may no longer support SSE transport for protocol version 2025-06-18
   - Need to implement HTTP Stream Transport instead

2. **Client Compatibility**: Claude Code's MCP client may require specific additional fields or behaviors not yet implemented

3. **Network/Timeout Issues**: Connection may be timing out before completing handshake

---

## How This Could Have Been Avoided

1. **Follow Latest MCP Specification**: Should have checked the latest MCP specification (2025-06-18 or later) before implementing SSE transport
   - SSE was already deprecated by the time we implemented it
   - Should have used Streamable HTTP transport from the start

2. **Comprehensive Testing**: Should have tested with actual MCP clients (like Claude Code) during development
   - Would have caught protocol version mismatch immediately
   - Would have discovered SSE deprecation

3. **Automated Tests**: Need integration tests that verify:
   - Protocol version compatibility
   - All required capabilities are present
   - Response content types are correct
   - Response headers match spec requirements

4. **Documentation Review**: Should have reviewed official MCP documentation and examples before implementation
   - Official spec clearly documents transport deprecations
   - Example servers use newer transports

5. **Version Alignment**: Should have kept server and client protocol versions in sync from the start
   - Track which protocol version Claude Code uses
   - Update server when client updates

6. **CI/CD Validation**: Automated build should catch:
   - Dependency version mismatches (Oxpecker warning)
   - Protocol version inconsistencies
   - Missing required fields in responses

7. **Configuration Management**: .env file should have been part of initial deployment
   - Include in Docker image or deployment scripts
   - Document all required environment variables

---

## Next Steps

### High Priority
1. **Implement Streamable HTTP Transport**: Replace deprecated SSE transport with modern HTTP Stream transport
   - Follow MCP spec 2025-06-18 or later
   - Support both SSE-style responses and chunked JSON

2. **Test with Claude Code**: Verify connection works with actual client

### Medium Priority
1. **Add Integration Tests**: Create automated tests for MCP protocol compliance
2. **Update Documentation**: Document supported protocol versions and transports
3. **Version Detection**: Add logic to detect client protocol version and adapt

### Low Priority
1. **Transport Fallback**: Support both SSE (legacy) and HTTP Stream (modern) transports
2. **Protocol Negotiation**: Implement proper protocol version negotiation

---

## Files Changed

- `src/FnMCP.Nexus/McpServer.fs` - Protocol version, capabilities
- `src/FnMCP.Nexus/Program.fs` - Startup log message
- `src/FnMCP.Nexus/Transport/SseTransport.fs` - Content-type, session ID, logging
- `src/FnMCP.Nexus/FnMCP.Nexus.fsproj` - Oxpecker version
- `/data/.env` - VPS environment configuration (new file)

---

## Deployment Details

### Build Commands
```bash
docker build -t nexus-mcp:session-id-fix .
docker save nexus-mcp:session-id-fix | gzip > /tmp/nexus-mcp-session-id.tar.gz
```

### Deployment to VPS
```bash
scp /tmp/nexus-mcp-session-id.tar.gz root@66.179.208.238:/tmp/
ssh root@66.179.208.238 "docker load < /tmp/nexus-mcp-session-id.tar.gz && \
  docker tag nexus-mcp:session-id-fix nexus-mcp:latest && \
  docker stop nexus-mcp && docker rm nexus-mcp && \
  docker run -d --name nexus-mcp \
    -v /data/event-store:/data/event-store \
    -v /data/.env:/app/.env:ro \
    -e NEXUS_TRANSPORT=http \
    -e ASPNETCORE_URLS=http://+:18080 \
    -p 18080:18080 \
    --restart unless-stopped \
    nexus-mcp:latest /data/event-store"
```

---

## Server Logs (Relevant Excerpts)

```
[FnMCP.Nexus] Loaded environment variables from .env file: /app/.env
[FnMCP.Nexus] Protocol version: 2025-06-18
[FnMCP.Nexus] Context library path: /data/event-store
[SseTransport] SSE connection established
[SseTransport] Sent endpoint URL: http://66.179.208.238:18080/sse/message
[SseTransport] Received message: {"method":"initialize","params":{"protocolVersion":"2025-06-18"...
[SseTransport] Sending response: {"jsonrpc":"2.0","id":0,"result":{"protocolVersion":"2025-06-18","capabilities":{"prompts":{...},"resources":{...},"tools":{...}}...
[SseTransport] Generated session ID: <guid>
[SseTransport] SSE connection cancelled by client
```

---

## Conclusion

While we successfully fixed multiple issues (protocol version, capabilities, content-type, session ID, Oxpecker warning, .env file), the connection still fails. The root cause appears to be that **SSE transport was deprecated** in the MCP specification, and Claude Code may no longer support it for protocol version 2025-06-18.

**Recommendation**: Implement the Streamable HTTP transport as specified in MCP 2025-03-26 or later to achieve compatibility with modern MCP clients like Claude Code.
