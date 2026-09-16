
using AI.CustomerSupport.Models;
using System.Text.Json;

namespace AI.CustomerSupport.Services
{
    public class RagStoreService
    {
        private readonly string _storePath;

        public RagStoreService(IWebHostEnvironment environment)
        {
            _storePath = Path.Combine(
                environment.ContentRootPath,
                "KnowledgeBase",
                "vectorstore.json");
        }

        public async Task SaveChunksAsync(List<Documentchunk> chunks)
        {
            var directory = Path.GetDirectoryName(_storePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            var json = JsonSerializer.Serialize(
                chunks,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            await File.WriteAllTextAsync(_storePath, json);
        }

        public async Task<List<Documentchunk>> LoadChunksAsync()
        {
            if (!File.Exists(_storePath))
            {
                return new List<Documentchunk>();
            }

            var json = await File.ReadAllTextAsync(_storePath);

            return JsonSerializer.Deserialize<List<Documentchunk>>(json)
                   ?? new List<Documentchunk>();
        }
    }
}
