namespace FnMCP.Nexus.Tools

open System
open System.Text
open System.Text.Json
open FnMCP.Nexus.Types
open FnMCP.Nexus.Domain
open FnMCP.Nexus.Domain.EventWriter

module SemanticSearch =

    /// Tool definition for semantic search
    let searchKnowledgeTool : Tool = {
        Name = "search_knowledge"
        Description = Some "Search past conversations and knowledge base using semantic similarity. Returns relevant conversations that match the query's meaning, not just keywords."
        InputSchema = box {|
            ``type`` = "object"
            properties = {|
                query = {|
                    ``type`` = "string"
                    description = "The search query - describe what you're looking for"
                |}
                limit = {|
                    ``type`` = "integer"
                    description = "Number of results to return (1-10, default 5)"
                    minimum = 1
                    maximum = 10
                    ``default`` = 5
                |}
            |}
            required = [| "query" |]
        |}
    }

    /// Helper to extract integer from JSON with default
    let private getIntOpt (elem: JsonElement) (name: string) (defaultValue: int) =
        let mutable prop = Unchecked.defaultof<JsonElement>
        if elem.TryGetProperty(name, &prop) && prop.ValueKind = JsonValueKind.Number then
            prop.GetInt32()
        else
            defaultValue

    /// Format search results as readable text for Claude
    let private formatResults (results: FnMCP.Nexus.QdrantClient.SearchResult list) : string =
        if List.isEmpty results then
            "No relevant conversations found."
        else
            let sb = StringBuilder()
            sb.AppendLine($"Found {results.Length} relevant conversations:") |> ignore
            sb.AppendLine() |> ignore

            results |> List.iteri (fun i result ->
                let index = i + 1
                sb.AppendLine($"[{index}] Score: {result.Score:F3} - \"{result.ConversationTitle}\"") |> ignore
                sb.AppendLine($"Created: {result.CreatedAt}") |> ignore
                sb.AppendLine($"UUID: {result.ConversationUuid}") |> ignore

                // Show preview of conversation text (first 500 chars)
                let preview =
                    if result.ConversationText.Length > 500 then
                        result.ConversationText.Substring(0, 500) + "..."
                    else
                        result.ConversationText

                sb.AppendLine($"Preview: {preview}") |> ignore
                sb.AppendLine() |> ignore
            )

            sb.ToString()

    /// Handle search_knowledge tool execution
    let handleSearchKnowledge (basePath: string) (args: JsonElement) : Result<string, string> =
        try
            // Extract parameters
            let query = args.GetProperty("query").GetString()
            let limit = getIntOpt args "limit" 5 |> min 10 |> max 1

            // Get Qdrant configuration
            let config = FnMCP.Nexus.QdrantClient.getConfig ()

            // Generate embedding for query
            let embedding =
                FnMCP.Nexus.EmbeddingService.embed query
                |> Async.RunSynchronously

            // Search Qdrant
            let results = FnMCP.Nexus.QdrantClient.search config embedding limit

            // Create system event for tracking
            let topResult = results |> List.tryHead
            let systemEvent : SystemEventMeta = {
                Id = Guid.NewGuid()
                Type = SemanticSearchPerformed
                OccurredAt = DateTime.Now
                EventId = None
                EventType = None
                ProjectionType = None
                Duration = None
                EventCount = None
                Staleness = None
                ToolName = None
                Success = None
                Query = Some query
                ResultCount = Some results.Length
                TopResultTitle = topResult |> Option.map (fun r -> r.ConversationTitle)
                TopResultScore = topResult |> Option.map (fun r -> r.Score)
            }

            // Write system event
            EventWriter.writeSystemEvent basePath None systemEvent |> ignore

            // Format and return results
            let formattedResults = formatResults results
            Ok formattedResults

        with
        | ex -> Error ($"Failed to search knowledge: {ex.Message}")
