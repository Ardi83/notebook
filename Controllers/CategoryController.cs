using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using notebook.DataLayer.Abstracts;
using notebook.Models;

namespace notebook.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("add")]
        [Authorize(Policy = Helpers.Constants.Strings.JwtClaims.Admin)]
        public async Task<IActionResult> CreateCategory([FromBody]Category category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                    return BadRequest("Name not allowed!");
                var result = await _categoryService.AddCategory(category);
                if (result)
                {
                    return Ok("Category added.");
                }
                return BadRequest("Category exict!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryService.GetAllCategories();
        }
    }
}