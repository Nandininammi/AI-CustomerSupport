using System.Net.Http.Json;
using System.Text.Json;
using AI.CustomerSupport.Models;

namespace AI.CustomerSupport.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Existing method used by our RAG flow
        public async Task<string> GetResponseAsync(string message)
        {
            var request = new
            {
                model = "llama3.2",
                prompt = message,
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/generate",
                request);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<OllamaResponse>();

            return result?.Response
                   ?? "No response received from AI.";
        }

        // New method for tool calling
        public async Task<OllamaChatResponse?> GetChatResponseAsync(
            string message)
        {
            var request = new
            {
                model = "llama3.2",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = message
                    }
                },
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/chat",
                request);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<OllamaChatResponse>();
        }
    }

    public class OllamaResponse
    {
        public string Response { get; set; } = string.Empty;
    }

    public class OllamaChatResponse
    {
        public OllamaMessage? Message { get; set; }
    }

    public class OllamaMessage
    {
        public string Role { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public List<OllamaToolCall>? ToolCalls { get; set; }
    }

    public class OllamaToolCall
    {
        public string Function { get; set; } = string.Empty;
    }
}