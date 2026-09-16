using AI.CustomerSupport.Models;

namespace AI.CustomerSupport.Services
{
    public class DocumentIngestionService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly EmbeddingService _embeddingService;
        private readonly RagStoreService _ragStoreService;

        public DocumentIngestionService(
            IWebHostEnvironment environment,
            EmbeddingService embeddingService,
            RagStoreService ragStoreService)
        {
            _environment = environment;
            _embeddingService = embeddingService;
            _ragStoreService = ragStoreService;
        }

        public async Task IngestAsync()
        {
            var filePath = Path.Combine(
                _environment.ContentRootPath,
                "KnowledgeBase",
                "WarrantyPolicy.txt");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Warranty policy file was not found.",
                    filePath);
            }

            var content = await File.ReadAllTextAsync(filePath);

            var chunks = SplitIntoChunks(content);

            var documentChunks = new List<Documentchunk>();

            int id = 1;

            foreach (var chunk in chunks)
            {
                Console.WriteLine($"Generating embedding for chunk {id}...");

                var embedding =
                    await _embeddingService.GenerateEmbeddingAsync(chunk);

                documentChunks.Add(new Documentchunk
                {
                    Id = id,
                    Text = chunk,
                    Embedding = embedding
                });

                id++;
            }

            await _ragStoreService.SaveChunksAsync(documentChunks);

            Console.WriteLine(
                $"Ingestion completed. {documentChunks.Count} chunks stored.");
        }

        private List<string> SplitIntoChunks(string content)
        {
            var cleanedContent = content.Trim();

            if (string.IsNullOrWhiteSpace(cleanedContent))
            {
                return new List<string>();
            }

            return new List<string>
            {
                cleanedContent
            };
        }
    }
}