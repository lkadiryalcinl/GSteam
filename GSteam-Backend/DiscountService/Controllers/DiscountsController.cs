using DiscountService.Models;
using DiscountService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController(IDiscountRepository repository) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateDiscount(DiscountModel model)
        {
            var response = await repository.CreateDiscount(model);
            return Ok(response);
        }
    }
}
