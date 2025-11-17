module FnMCP.Nexus.Auth.AuthMiddleware

open System
open Microsoft.AspNetCore.Http
open Oxpecker
open FnMCP.Nexus.Auth.ApiKeyService
open FnMCP.Nexus.Projections.ApiKeyProjection

// Oxpecker authentication middleware for API key validation
// Using Oxpecker 1.0 API: EndpointMiddleware = EndpointHandler -> HttpContext -> Task

/// Extract client IP from request
let private getClientIp (ctx: HttpContext) : string option =
    // Try X-Forwarded-For header first (for reverse proxies)
    match ctx.Request.Headers.TryGetValue("X-Forwarded-For") with
    | true, values ->
        values
        |> Seq.tryHead
        |> Option.bind (fun v ->
            // X-Forwarded-For can contain multiple IPs, take the first
            let ip = v.Split(',').[0].Trim()
            if String.IsNullOrWhiteSpace(ip) then None else Some ip)
    | false, _ ->
        // Fall back to direct connection IP
        match ctx.Connection.RemoteIpAddress with
        | null -> None
        | ip -> Some (ip.ToString())

/// Extract user agent from request
let private getUserAgent (ctx: HttpContext) : string option =
    match ctx.Request.Headers.TryGetValue("User-Agent") with
    | true, values -> values |> Seq.tryHead
    | false, _ -> None

/// Require valid API key for request
/// On success, stores ValidApiKey in context.Items["ApiKey"]
let requireApiKey (eventStorePath: string) : EndpointMiddleware =
    fun next (ctx: HttpContext) ->
        task {
            let clientIp = getClientIp ctx
            let userAgent = getUserAgent ctx

            match ctx.Request.Headers.TryGetValue("Authorization") with
            | true, values ->
                let authHeader = values |> Seq.tryHead
                match authHeader with
                | Some header when header.StartsWith("Bearer ") ->
                    let key = header.Substring(7)

                    match validateApiKey eventStorePath key with
                    | Ok validKey ->
                        // Store valid key in context items for later use
                        ctx.Items.["ApiKey"] <- box validKey

                        // Log successful usage
                        logApiKeyUsage eventStorePath validKey.KeyId clientIp userAgent

                        // Continue to next handler
                        return! next ctx

                    | Error msg ->
                        // Log rejection
                        logApiKeyRejection eventStorePath msg clientIp userAgent

                        ctx.Response.StatusCode <- 401
                        ctx.Response.Headers.Append("WWW-Authenticate", "Bearer")
                        return! ctx |> text $"Unauthorized: {msg}"

                | Some _ ->
                    logApiKeyRejection eventStorePath "Invalid Authorization header format" clientIp userAgent
                    ctx.Response.StatusCode <- 401
                    ctx.Response.Headers.Append("WWW-Authenticate", "Bearer")
                    return! ctx |> text "Unauthorized: Invalid Authorization header format (expected 'Bearer <token>')"

                | None ->
                    logApiKeyRejection eventStorePath "Missing Authorization header value" clientIp userAgent
                    ctx.Response.StatusCode <- 401
                    ctx.Response.Headers.Append("WWW-Authenticate", "Bearer")
                    return! ctx |> text "Unauthorized: Missing Authorization header value"

            | false, _ ->
                logApiKeyRejection eventStorePath "Missing Authorization header" clientIp userAgent
                ctx.Response.StatusCode <- 401
                ctx.Response.Headers.Append("WWW-Authenticate", "Bearer")
                return! ctx |> text "Unauthorized: Missing Authorization header. Use 'Authorization: Bearer <your-api-key>'"
        }

/// Get the authenticated API key from context
let getAuthenticatedKey (ctx: HttpContext) : ValidApiKey option =
    match ctx.Items.TryGetValue("ApiKey") with
    | true, value ->
        match value with
        | :? ValidApiKey as key -> Some key
        | _ -> None
    | false, _ -> None
