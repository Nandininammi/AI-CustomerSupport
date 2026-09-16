using AI.CustomerSupport.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI.CustomerSupport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmbeddingController : ControllerBase
    {
        private readonly EmbeddingService _embeddingService;

        public EmbeddingController(EmbeddingService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateEmbedding([FromBody] string text)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(text);

            return Ok(new
            {
                Text = text,
                Dimensions = embedding.Length,
                Embedding = embedding
            });
        }
    }
}
