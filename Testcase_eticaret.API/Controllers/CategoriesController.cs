using Microsoft.AspNetCore.Mvc;
using Testcase_eticaret.API.DTOs;
using Testcase_eticaret.API.Services;

namespace Testcase_eticaret.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Category not found." });

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create(CategoryCreateDto dto)
        {
            var category = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> Update(int id, CategoryUpdateDto dto)
        {
            var category = await _categoryService.UpdateAsync(id, dto);
            if (category == null)
                return NotFound(new { message = "Category not found." });

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result == null)
                return NotFound(new { message = "Category not found." });

            if (result == false)
                return BadRequest(new { message = "Category cannot be deleted because it has related products." });

            return NoContent();
        }
    }
}
