using Microsoft.AspNetCore.Mvc;
using Tarantula.Indexer;
// this is not used because i have used a different method for testing 
namespace Tarantula.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SQLiteIndexer _indexer;

        public SearchController(SQLiteIndexer indexer )
        {
            _indexer = indexer;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query parameter is required.");

            var results = await _indexer.Search(query);
            return Ok(results.Select(r => new {
                Url = r.Url,
                Score = r.Score
            }));
        }
    }
}
