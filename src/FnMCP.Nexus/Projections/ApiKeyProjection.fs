module FnMCP.Nexus.Projections.ApiKeyProjection

open System
open System.IO
open FnMCP.Nexus.Domain

// Valid API key projection for authentication

type ValidApiKey = {
    KeyId: Guid
    KeyHash: string
    Scope: ApiKeyScope
    Description: string
    ExpiresAt: DateTime option
    GeneratedAt: DateTime
    GeneratedBy: string
}

// Parse YAML frontmatter from API key event files
let private parseYamlField (lines: string array) (fieldName: string) : string option =
    lines
    |> Array.tryFind (fun line -> line.StartsWith($"{fieldName}:"))
    |> Option.map (fun line ->
        let value = line.Substring(fieldName.Length + 1).Trim()
        // Remove quotes if present
        if value.StartsWith("\"") && value.EndsWith("\"") then
            value.Substring(1, value.Length - 2)
        else
            value)

let private parseGuidField (lines: string array) (fieldName: string) : Guid option =
    parseYamlField lines fieldName
    |> Option.bind (fun s ->
        match Guid.TryParse(s) with
        | true, g -> Some g
        | false, _ -> None)

let private parseDateTimeField (lines: string array) (fieldName: string) : DateTime option =
    parseYamlField lines fieldName
    |> Option.bind (fun s ->
        match DateTime.TryParse(s) with
        | true, dt -> Some dt
        | false, _ -> None)

let private parseApiKeyScopeField (lines: string array) (fieldName: string) : ApiKeyScope option =
    parseYamlField lines fieldName
    |> Option.bind (fun s ->
        try Some (ApiKeyScope.Parse(s))
        with _ -> None)

// Build projection by reading all API key events
let buildProjection (eventStorePath: string) : Map<Guid, ValidApiKey> =
    let apiKeyDir = Path.Combine(eventStorePath, "nexus", "events", "system", "api-keys")

    if not (Directory.Exists apiKeyDir) then
        Map.empty
    else
        // Get all YAML files recursively (across all month folders)
        let files = Directory.GetFiles(apiKeyDir, "*.yaml", SearchOption.AllDirectories)

        // Parse events and build map
        let generatedKeys =
            files
            |> Array.choose (fun filePath ->
                try
                    let content = File.ReadAllLines(filePath)
                    let eventType = parseYamlField content "type"

                    match eventType with
                    | Some "ApiKeyGenerated" ->
                        // Parse all required fields
                        let keyId = parseGuidField content "key_id"
                        let keyHash = parseYamlField content "key_hash"
                        let scope = parseApiKeyScopeField content "scope"
                        let description = parseYamlField content "description"
                        let expiresAt = parseDateTimeField content "expires_at"
                        let generatedAt = parseDateTimeField content "occurred_at"
                        let generatedBy = parseYamlField content "generated_by"

                        match keyId, keyHash, scope, generatedAt with
                        | Some kid, Some kh, Some s, Some ga ->
                            Some (kid, {
                                KeyId = kid
                                KeyHash = kh
                                Scope = s
                                Description = description |> Option.defaultValue "No description"
                                ExpiresAt = expiresAt
                                GeneratedAt = ga
                                GeneratedBy = generatedBy |> Option.defaultValue "unknown"
                            })
                        | _ -> None
                    | _ -> None
                with
                | _ -> None)
            |> Map.ofArray

        // Get all revoked key IDs
        let revokedKeyIds =
            files
            |> Array.choose (fun filePath ->
                try
                    let content = File.ReadAllLines(filePath)
                    let eventType = parseYamlField content "type"

                    match eventType with
                    | Some "ApiKeyRevoked" ->
                        parseGuidField content "key_id"
                    | _ -> None
                with
                | _ -> None)
            |> Set.ofArray

        // Filter out revoked keys
        generatedKeys
        |> Map.filter (fun keyId _ -> not (Set.contains keyId revokedKeyIds))

// Check if a key is valid (exists and not expired)
let isKeyValid (keyId: Guid) (projection: Map<Guid, ValidApiKey>) : bool =
    match Map.tryFind keyId projection with
    | None -> false
    | Some key ->
        match key.ExpiresAt with
        | None -> true
        | Some expiry -> DateTime.UtcNow < expiry

// Find a key by its hash (for authentication)
let findKeyByHash (keyHash: string) (projection: Map<Guid, ValidApiKey>) : ValidApiKey option =
    projection
    |> Map.tryPick (fun keyId validKey ->
        if validKey.KeyHash = keyHash && isKeyValid keyId projection then
            Some validKey
        else
            None)
