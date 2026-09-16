using AI.CustomerSupport.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI.CustomerSupport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngestionController : ControllerBase
    {
        private readonly DocumentIngestionService _ingestionService;

        public IngestionController(
            DocumentIngestionService ingestionService)
        {
            _ingestionService = ingestionService;
        }

        [HttpPost]
        public async Task<IActionResult> Ingest()
        {
            await _ingestionService.IngestAsync();

            return Ok(new
            {
                message = "Document ingestion completed successfully."
            });
        }
    }
}
