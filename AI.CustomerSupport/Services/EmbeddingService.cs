using System.Net.Http.Json;
using System.Text.Json;

namespace AI.CustomerSupport.Services
{
    public class EmbeddingService
    {
        private readonly HttpClient _httpClient;

        public EmbeddingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            var request = new
            {
                model = "nomic-embed-text",
                prompt = text
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/embeddings",
                request);
            

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<EmbeddingResponse>();

            return result?.Embedding ?? Array.Empty<float>();
        }
    }

    public class EmbeddingResponse
    {
        public float[] Embedding { get; set; } = Array.Empty<float>();
    }
}
