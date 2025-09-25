using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    [Route("Categories")]
    public class CategoriesController : Controller
    {
        private readonly IBaseRepository<Category,Guid> _categoryRepository;
        public CategoriesController(IBaseRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("parents")]
        public async Task<IActionResult> GetCategoriesParent()
        {
            var parents = await _categoryRepository
                .FindAll(c => c.ParentId == null)
                .OrderBy(c => c.SortOrder)
                .ThenBy(e => e.Name)
                .Select(e => new
                {
                    id = e.Id,
                    name = e.Name
                })
                .ToListAsync();
            return Json(parents);
        }

        [HttpGet("childrens")]
        public async Task<IActionResult> GetCategiesChildren(Guid parentId)
        {
            var childrens = await _categoryRepository.FindAll(c => c.ParentId == parentId)
                .OrderBy(c => c.SortOrder)
                .ThenBy(e => e.Name)
                .Select(e => new
                {
                    id = e.Id,
                    name = e.Name
                })
                .ToListAsync();
            return Json(childrens);
        }
    }
}
