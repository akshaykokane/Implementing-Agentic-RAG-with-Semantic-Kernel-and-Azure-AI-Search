using AgenticRAG.Models;
using Azure;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AzureSearchService
{
    private readonly SearchIndexClient _indexClient;
    private readonly string _indexName;
    private readonly AzureOpenAITextEmbeddingGenerationService azureOpenAITextEmbeddingGenerationService;

    public AzureSearchService()
    {
        _indexClient = new SearchIndexClient(new Uri("https://azure-ai-search-medium.search.windows.net"), new AzureKeyCredential("<ai-search-key>"));
        _indexName = "hotels-data-index";

        azureOpenAITextEmbeddingGenerationService = new(
            deploymentName: "text-embedding-3-small",
            endpoint: "https://<instance_name>.openai.azure.com",
            apiKey: "<key>"
         );

    }

    [KernelFunction]
    public async Task<string> SearchHotels(string query, float minScoreThreshold = 0.60f)
    {
        IList<ReadOnlyMemory<float>> queryEmbedding =
            await azureOpenAITextEmbeddingGenerationService.GenerateEmbeddingsAsync(new List<string>() { query });

        float[] queryVector = queryEmbedding[0].ToArray();

        List<SearchDocumentModel> documents = await this.VectorSearchAsync(queryVector, minScoreThreshold: minScoreThreshold);

        List<string> documentContents = new List<string>();

        foreach (var doc in documents)
        {
            documentContents.Add(doc.chunk);
        }

        return string.Join("\n", documentContents);
    }

    public async Task<List<SearchDocumentModel>> VectorSearchAsync(float[] queryVector, int topK = 3, float minScoreThreshold = 0.75f)
    {
        var searchClient = _indexClient.GetSearchClient(_indexName);

        var vectorQuery = new VectorizedQuery(queryVector)
        {
            KNearestNeighborsCount = topK,
            Fields = { "text_vector" }
        };

        var options = new SearchOptions
        {
            VectorSearch = new VectorSearchOptions
            {
                Queries = { vectorQuery }
            },
            Size = topK
        };

        var response = await searchClient.SearchAsync<SearchDocumentModel>(null, options);

        // Filter results by score and extract only the document
        var filteredDocuments = new List<SearchDocumentModel>();
        await foreach (var result in response.Value.GetResultsAsync())
        {
            if (result.Score >= minScoreThreshold)
            {
                filteredDocuments.Add(result.Document);
            }
        }

        return filteredDocuments;
    }

}