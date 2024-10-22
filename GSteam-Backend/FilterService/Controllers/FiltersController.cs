using FilterService.Models;
using FilterService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilterService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController(IFilterGameService filterGameService) : ControllerBase
    {
        private readonly IFilterGameService _filterGameService = filterGameService;

        [HttpPost]
        public async Task<ActionResult> FilterGameServ(GameFilterItem gameFilterItem)
        {
            var response = await _filterGameService.SearchAsync(gameFilterItem);
            return Ok(response);
        }
    }
}
