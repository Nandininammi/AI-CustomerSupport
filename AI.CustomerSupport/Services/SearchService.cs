using AI.CustomerSupport.Models;

namespace AI.CustomerSupport.Services
{
    public class SearchService
    {
        private readonly EmbeddingService _embeddingService;
        private readonly RagStoreService _ragStoreService;

        public SearchService(
            EmbeddingService embeddingService,
            RagStoreService ragStoreService)
        {
            _embeddingService = embeddingService;
            _ragStoreService = ragStoreService;
        }

        //public async Task<List<Documentchunk>> SearchAsync(
        //    string question,
        //    int topK = 3)
        //{
        //    // Generate embedding for the customer's question
        //    var questionEmbedding =
        //        await _embeddingService.GenerateEmbeddingAsync(question);

        //    // Load our stored document chunks and embeddings
        //    var chunks =
        //        await _ragStoreService.LoadChunksAsync();

        //    if (chunks.Count == 0)
        //    {
        //        return new List<Documentchunk>();
        //    }

        //    // Calculate similarity between question and every chunk
        //    var results = chunks
        //        .Select(chunk => new
        //        {
        //            Chunk = chunk,
        //            Score = CosineSimilarity(
        //                questionEmbedding,
        //                chunk.Embedding)
        //        })
        //        .OrderByDescending(x => x.Score)
        //        .Take(topK)
        //        .Select(x => x.Chunk)
        //        .ToList();

        //    return results;
        //}

        public async Task<List<Documentchunk>> SearchAsync(
    string question,
    int topK = 1)
        {
            // 1. Generate embedding for the customer's question
            var questionEmbedding =
                await _embeddingService.GenerateEmbeddingAsync(question);

            // 2. Load stored document chunks
            var chunks =
                await _ragStoreService.LoadChunksAsync();

            if (chunks.Count == 0)
            {
                return new List<Documentchunk>();
            }

            // 3. Calculate similarity for every chunk
            const double similarityThreshold = 0.70;

            var results = chunks
                .Select(chunk => new
                {
                    Chunk = chunk,
                    Score = CosineSimilarity(
                        questionEmbedding,
                        chunk.Embedding)
                })
                .Where(x => x.Score >= similarityThreshold)
                .OrderByDescending(x => x.Score)
                .Take(topK)
                .ToList();

            // TEMPORARY DEBUG
            Console.WriteLine("====================================");
            Console.WriteLine("SEARCH RESULTS");
            Console.WriteLine("Question: " + question);

            foreach (var result in results)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine($"Chunk ID: {result.Chunk.Id}");
                Console.WriteLine($"Similarity Score: {result.Score:F4}");
                Console.WriteLine(result.Chunk.Text);
            }

            Console.WriteLine("====================================");

            return results
                .Select(x => x.Chunk)
                .ToList();
        }

        private double CosineSimilarity(
            float[] vectorA,
            float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
            {
                return 0;
            }

            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];

                magnitudeA += vectorA[i] * vectorA[i];

                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
            {
                return 0;
            }

            return dotProduct /
                   (Math.Sqrt(magnitudeA) *
                    Math.Sqrt(magnitudeB));
        }
    }
}