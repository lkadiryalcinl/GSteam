using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Helpers;
using SearchService.Models;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<GameItem>>> SearchItems([FromQuery]SearchParams searchParams)
        {
            var query = DB.PagedSearch<GameItem, GameItem>();
            if (!string.IsNullOrEmpty(searchParams.SearchWord))
            {
                query.Match(Search.Full, searchParams.SearchWord).SortByTextScore();
            }

            var result = await query.ExecuteAsync();

            return Ok(new
            {
                results = result.Results
            });
        }
    }
}
