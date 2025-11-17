module FnMCP.Nexus.Auth.Cryptography

open System
open System.Security.Cryptography
open System.Text

// Secure API key generation and hashing functions

/// Generate a cryptographically secure random API key
/// Returns a base64-encoded 256-bit (32 byte) random value
let generateSecureKey () : string =
    // Generate 32 random bytes (256 bits)
    let bytes = Array.zeroCreate<byte> 32
    use rng = RandomNumberGenerator.Create()
    rng.GetBytes(bytes)

    // Convert to base64 for easy transmission
    // Result will be 44 characters long (base64 encoding of 32 bytes)
    Convert.ToBase64String(bytes)

/// Hash an API key using SHA256
/// Returns hex-encoded hash (64 characters)
let hashKey (key: string) : string =
    use sha256 = SHA256.Create()
    let bytes = Encoding.UTF8.GetBytes(key)
    let hashBytes = sha256.ComputeHash(bytes)

    // Convert to hex string for storage
    hashBytes
    |> Array.map (fun b -> b.ToString("x2"))
    |> String.concat ""

/// Verify an API key against a stored hash using constant-time comparison
/// This prevents timing attacks that could leak information about the hash
let verifyKey (providedKey: string) (storedHash: string) : bool =
    let providedHash = hashKey providedKey

    // Constant-time comparison to prevent timing attacks
    // Even if lengths don't match, we compare all characters
    let mutable result = providedHash.Length = storedHash.Length
    let maxLen = max providedHash.Length storedHash.Length

    for i in 0 .. maxLen - 1 do
        let providedChar = if i < providedHash.Length then providedHash.[i] else '\000'
        let storedChar = if i < storedHash.Length then storedHash.[i] else '\000'
        result <- result && (providedChar = storedChar)

    result
