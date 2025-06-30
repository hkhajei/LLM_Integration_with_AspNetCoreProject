using Microsoft.SemanticKernel.Embeddings; // Correct namespace for IEmbeddingClient

namespace AspNetCoreProject.Models.Services
{
    public class DocumentService
    {
        private List<Tuple<string, float[]>> _documentStore = new();
        private readonly ITextEmbeddingGenerationService _embeddingService; // Changed to IEmbeddingService as per newer SK

        public DocumentService(ITextEmbeddingGenerationService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        public async Task AddDocument(string documentText)
        {
            // Simple chunking (you'd want more sophisticated methods)
            var chunks = documentText.Split(new[] { "\n\n", "." }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var chunk in chunks)
            {
                // GenerateEmbeddingAsync now takes a list and returns an IList<ReadOnlyMemory<float>>
                var embeddings = await _embeddingService.GenerateEmbeddingsAsync(new List<string> { chunk });
                _documentStore.Add(Tuple.Create(chunk, embeddings[0].ToArray()));
            }
        }

        public List<string> RetrieveRelevantChunks(float[] queryEmbedding, int topK = 3)
        {
            // Simple similarity search (cosine similarity)
            return _documentStore
                .OrderByDescending(doc => CosineSimilarity(queryEmbedding, doc.Item2))
                .Take(topK)
                .Select(doc => doc.Item1)
                .ToList();
        }

        private double CosineSimilarity(float[] vec1, float[] vec2)
        {
            double dotProduct = 0;
            double magnitude1 = 0;
            double magnitude2 = 0;

            for (int i = 0; i < vec1.Length; i++)
            {
                dotProduct += vec1[i] * vec2[i];
                magnitude1 += vec1[i] * vec1[i];
                magnitude2 += vec2[i] * vec2[i];
            }

            return dotProduct / (Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2));
        }
    }
}