module FnMCP.Nexus.EmbeddingService

open System

/// Generate embedding vector for a text query
///
/// PLACEHOLDER: This will be implemented once we add an embedding API endpoint
/// to the VPS that wraps the sentence-transformers model (all-MiniLM-L6-v2).
///
/// For now, returns a dummy 384-dimensional vector for testing purposes.
/// The real implementation will call the embedding API via HTTP.
let embed (text: string) : Async<float list> =
    async {
        // TODO: Implement actual embedding API call
        // For now, return a dummy vector of 384 dimensions (matching all-MiniLM-L6-v2)
        // This allows testing the integration without the embedding service

        // Generate a simple deterministic vector based on text hash for testing
        // This is NOT semantically meaningful, just for infrastructure testing
        let rng = Random(text.GetHashCode())
        let dummyVector = List.init 384 (fun _ -> rng.NextDouble() * 2.0 - 1.0)

        return dummyVector
    }

/// Future implementation note:
///
/// When the embedding API is ready on the VPS, this function should:
/// 1. Make HTTP POST request to VPS embedding endpoint (e.g., http://66.179.208.238:8000/embed)
/// 2. Send JSON body: { "text": "query string" }
/// 3. Parse response: { "embedding": [0.1, 0.2, ...] }
/// 4. Return the 384-dimensional vector
///
/// Example implementation (when ready):
///
/// open System.Net.Http
/// open System.Text
/// open System.Text.Json
///
/// let embedReal (text: string) : Async<float list> =
///     async {
///         let httpClient = new HttpClient()
///         let embeddingUrl = "http://66.179.208.238:8000/embed"
///
///         let requestBody = {| text = text |}
///         let jsonBody = JsonSerializer.Serialize(requestBody)
///         use content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
///
///         let! response = httpClient.PostAsync(embeddingUrl, content) |> Async.AwaitTask
///         let! responseJson = response.Content.ReadAsStringAsync() |> Async.AwaitTask
///
///         let doc = JsonDocument.Parse(responseJson)
///         let embeddingArray = doc.RootElement.GetProperty("embedding")
///         let embedding =
///             embeddingArray.EnumerateArray()
///             |> Seq.map (fun e -> e.GetDouble())
///             |> Seq.toList
///
///         return embedding
///     }
