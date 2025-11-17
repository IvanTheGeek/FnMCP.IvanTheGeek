module FnMCP.Nexus.Auth.ApiKeyService

open System
open FnMCP.Nexus.Auth.Cryptography
open FnMCP.Nexus.Domain
open FnMCP.Nexus.Domain.EventWriter
open FnMCP.Nexus.Projections.ApiKeyProjection

// API key generation and validation service

/// Generate a new API key and write the generation event
/// Returns the unhashed key (only shown once) and the key ID
let generateApiKey
    (eventStorePath: string)
    (scope: ApiKeyScope)
    (description: string)
    (expiresInDays: int option)
    (generatedBy: string)
    : string * Guid =

    let keyId = Guid.NewGuid()
    let key = generateSecureKey()
    let keyHash = hashKey key

    let expiresAt =
        expiresInDays
        |> Option.map (fun days -> DateTime.UtcNow.AddDays(float days))

    let event : ApiKeyEventMeta = {
        Id = Guid.NewGuid()
        Type = ApiKeyGenerated
        OccurredAt = DateTime.UtcNow
        KeyId = Some keyId
        KeyHash = Some keyHash
        Scope = Some scope
        Description = Some description
        ExpiresAt = expiresAt
        GeneratedBy = Some generatedBy
        RevokedBy = None
        RevokedReason = None
        ClientIp = None
        UserAgent = None
        RejectionReason = None
    }

    // Write event to store
    let filePath = writeApiKeyEvent eventStorePath None event

    // Log to stderr (stdout is for JSON-RPC)
    Console.Error.WriteLine($"[ApiKeyService] Generated key {keyId} -> {filePath}")

    // Return the unhashed key (only time it's visible) and the ID
    (key, keyId)

/// Revoke an API key
let revokeApiKey
    (eventStorePath: string)
    (keyId: Guid)
    (revokedBy: string)
    (reason: string)
    : Result<unit, string> =

    // Check if key exists and is not already revoked
    let projection = buildProjection eventStorePath

    match Map.tryFind keyId projection with
    | None ->
        Error "API key not found or already revoked"
    | Some _ ->
        let event : ApiKeyEventMeta = {
            Id = Guid.NewGuid()
            Type = ApiKeyRevoked
            OccurredAt = DateTime.UtcNow
            KeyId = Some keyId
            KeyHash = None
            Scope = None
            Description = None
            ExpiresAt = None
            GeneratedBy = None
            RevokedBy = Some revokedBy
            RevokedReason = Some reason
            ClientIp = None
            UserAgent = None
            RejectionReason = None
        }

        let filePath = writeApiKeyEvent eventStorePath None event
        Console.Error.WriteLine($"[ApiKeyService] Revoked key {keyId} -> {filePath}")

        Ok ()

/// Validate an API key by checking against the projection
/// Returns the valid key details if authentication succeeds
let validateApiKey
    (eventStorePath: string)
    (providedKey: string)
    : Result<ValidApiKey, string> =

    let projection = buildProjection eventStorePath
    let providedHash = hashKey providedKey

    // Find key by hash
    match findKeyByHash providedHash projection with
    | Some validKey ->
        Ok validKey
    | None ->
        Error "Invalid or expired API key"

/// Log successful API key usage
let logApiKeyUsage
    (eventStorePath: string)
    (keyId: Guid)
    (clientIp: string option)
    (userAgent: string option)
    : unit =

    let event : ApiKeyEventMeta = {
        Id = Guid.NewGuid()
        Type = ApiKeyUsed
        OccurredAt = DateTime.UtcNow
        KeyId = Some keyId
        KeyHash = None
        Scope = None
        Description = None
        ExpiresAt = None
        GeneratedBy = None
        RevokedBy = None
        RevokedReason = None
        ClientIp = clientIp
        UserAgent = userAgent
        RejectionReason = None
    }

    let _ = writeApiKeyEvent eventStorePath None event
    ()

/// Log failed API key authentication attempt
let logApiKeyRejection
    (eventStorePath: string)
    (reason: string)
    (clientIp: string option)
    (userAgent: string option)
    : unit =

    let event : ApiKeyEventMeta = {
        Id = Guid.NewGuid()
        Type = ApiKeyRejected
        OccurredAt = DateTime.UtcNow
        KeyId = None
        KeyHash = None
        Scope = None
        Description = None
        ExpiresAt = None
        GeneratedBy = None
        RevokedBy = None
        RevokedReason = None
        ClientIp = clientIp
        UserAgent = userAgent
        RejectionReason = Some reason
    }

    let _ = writeApiKeyEvent eventStorePath None event
    ()
