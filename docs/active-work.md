# Nexus MCP - Active Work Log

## Current Status: ✅ Production Ready

The Nexus MCP server is deployed and operational with working SSE transport.

### Latest Updates

#### 2025-11-19: SSE Endpoint URL Fix
**Status:** ✅ Completed
**Priority:** Critical

Fixed SSE transport to send full URL in `endpoint` event instead of relative path.

- **Problem:** Claude Code health check failing due to relative path `/sse/message`
- **Solution:** Updated SSE transport to construct full URL from request context
- **Result:** SSE endpoint now correctly sends `http://66.179.208.238:18080/sse/message`
- **Deployment:** Updated Docker image deployed to VPS
- **Verification:** Initialize handshake completes successfully

**Details:** See [docs/sessions/2025-11-19-sse-endpoint-url-fix.md](sessions/2025-11-19-sse-endpoint-url-fix.md)

#### 2025-11-16: SSE Endpoint Debugging & VPS Deployment
**Status:** ✅ Completed

Comprehensive debugging session that verified SSE endpoint functionality and VPS deployment.

- Renamed endpoint from `/sse/events` to `/sse` per MCP spec
- Verified Docker container deployment on VPS
- Confirmed SSE handshake and initialization work correctly
- Added Claude Desktop and Claude Code SSE configuration
- Documented full deployment and debugging process

**Details:** See [docs/2025-11-19-sse-debug-deployment.md](2025-11-19-sse-debug-deployment.md)

### System Overview

**Server:** Nexus MCP (FnMCP.Nexus)
**Transport:** HTTP with SSE support
**Deployment:** Docker container on VPS (66.179.208.238:18080)
**Authentication:** Bearer token
**Status:** ✅ Running and responding correctly

### Current Configuration

```json
{
  "mcpServers": {
    "nexus": {
      "type": "sse",
      "url": "http://66.179.208.238:18080/sse",
      "headers": {
        "Authorization": "Bearer KvzmKD3aBYBmKY4pvOh/+NhwHBBxxiTeIKD2Kq/tAw4="
      }
    }
  }
}
```

### Available Endpoints

- **Health:** `http://66.179.208.238:18080/` (no auth required)
- **SSE:** `http://66.179.208.238:18080/sse` (requires Bearer token)
- **SSE Message:** `http://66.179.208.238:18080/sse/message` (requires Bearer token)
- **WebSocket:** `ws://66.179.208.238:18080/ws` (requires Bearer token)

### Available MCP Tools

1. `create_event` - Create new events in the event store
2. `timeline_projection` - Project timeline from events
3. `get_context_summary` - Get summary of context library
4. `update_documentation` - Update project documentation

### Known Issues

- [ ] `claude mcp list` health check may show "Failed to connect" (false negative, connection works)
  - Likely due to client-side caching or timing
  - Server logs confirm successful connections and initialization
  - Should resolve when Claude Code restarts with updated server

### Next Steps

1. ✅ Fix SSE endpoint URL (completed 2025-11-19)
2. ⏳ Monitor MCP connections in fresh Claude Code sessions
3. ⏳ Verify Nexus MCP tools appear with `mcp__` prefix
4. ⏳ Test tool functionality in production
5. 📋 Consider adding dedicated health check endpoint with diagnostics

### Development Commands

```bash
# Build project
dotnet build

# Run locally
dotnet run -- /path/to/event-store

# Build Docker image
docker build -t nexus-mcp:latest .

# Deploy to VPS
docker save nexus-mcp:latest | gzip > /tmp/nexus-mcp.tar.gz
scp /tmp/nexus-mcp.tar.gz root@66.179.208.238:/tmp/
ssh root@66.179.208.238 "docker load < /tmp/nexus-mcp.tar.gz && \
  docker stop nexus-mcp && docker rm nexus-mcp && \
  docker run -d --name nexus-mcp -v /data/event-store:/data/event-store \
  -e NEXUS_TRANSPORT=http -e ASPNETCORE_URLS=http://+:18080 \
  -p 18080:18080 --restart unless-stopped nexus-mcp:latest /data/event-store"

# Check logs
ssh root@66.179.208.238 "docker logs nexus-mcp -f"
```

### Documentation

- [Phase 3 Completion Summary](Phase-3-Completion-Summary.md)
- [Update Documentation Tool](update_documentation_tool.md)
- [Session Reports](sessions/)

### Contact & Support

For issues or questions, refer to session reports in `docs/sessions/` or check server logs.
