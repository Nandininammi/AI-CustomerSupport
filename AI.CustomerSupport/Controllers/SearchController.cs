using AI.CustomerSupport.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI.CustomerSupport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody] string question)
        {
            var results = await _searchService.SearchAsync(question);

            return Ok(results);
        }
    }
}
