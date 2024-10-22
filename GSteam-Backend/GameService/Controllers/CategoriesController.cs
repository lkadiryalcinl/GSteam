using GameService.DTOs;
using GameService.Repositories.ForCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
            var response = await _categoryRepository.CreateCategory(category);
            return Ok(response);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> RemoveCategory([FromRoute]Guid categoryId)
        {
            var response = await _categoryRepository.RemoveCategory(categoryId);
            return Ok(response);
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult> UpdateCategory([FromBody]CategoryDTO model, [FromRoute]Guid categoryId)
        {
            var response = await _categoryRepository.UpdateCategory(model, categoryId);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllCategories()
        {
            var response = await _categoryRepository.GetAllCategories();
            return Ok(response);
        }

    }
}
