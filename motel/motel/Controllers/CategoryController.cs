using Microsoft.AspNetCore.Mvc;
using motel.Data;
using motel.Models.DTO;
using motel.Repositories;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(AppDbContext appDbContext, ICategoryRepository categoryRepository)
        {
            _appDbContext = appDbContext;
            _categoryRepository = categoryRepository;
        }
        [HttpGet("get-all-category")]
        public IActionResult GetAllcategory()
        {
            var categorylist = _categoryRepository.GetAllCategory();
            return Ok(categorylist);
        }
        [HttpGet("get-category-with-id")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category != null)
            {
                return Ok(category);
            }
            else
                return NotFound("Data Empty");
        }
        [HttpPost("add - category")]
        public IActionResult AddCategory([FromBody] AddCategoryRequestDTO addCategory)
        {
            var categoryAdd = _categoryRepository.AddCategory(addCategory);
            return Ok(categoryAdd);
        }
        [HttpPut("update-category-with-id")]
        public IActionResult UpdateCategoryBy(int id, [FromBody] AddCategoryRequestDTO updateCategory)
        {
            var categoryUpdate = _categoryRepository.UpdateCategory(id, updateCategory);
            return Ok(categoryUpdate);
        }
        [HttpDelete("delete-category-with-id")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryDelete = _categoryRepository.DeleteCategory(id);
            if (categoryDelete == null)
            {
                return StatusCode(500);
            }
            else
            {
                return Ok("category deleted");
            }
        }
    }
}
