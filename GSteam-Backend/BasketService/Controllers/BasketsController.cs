﻿using BasketService.Models;
using BasketService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController(IBasketRepository _basketRepository) : ControllerBase
    {
        [HttpPost("AddBasketItem")]
        [Authorize]
        public async Task<IActionResult> AddBasketItem(BasketModel model)
        {
            var res = await _basketRepository.AddBasket(model);
            return Ok(res);
        }

        [HttpGet("BasketItems")]
        [Authorize]
        public async Task<IActionResult> GetListItems()
        {
            var res = await _basketRepository.GetBasketItems();
            return Ok(res);
        }

        [HttpGet("BasketItem/{index}")]
        [Authorize]
        public async Task<IActionResult> GetBasketItem([FromRoute] long index)
        {
            var res = await _basketRepository.GetBasketItem(index);
            return Ok(res);
        }

        [HttpDelete("{index}")]
        [Authorize]
        public async Task<IActionResult> RemoveBasketItem([FromRoute]long index)
        {
            var res = await _basketRepository.RemoveBasketItem(index);
            return Ok(res);
        }

        [HttpPost("Checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var res = await _basketRepository.Checkout();
            return Ok(res);
        }
    }
}
