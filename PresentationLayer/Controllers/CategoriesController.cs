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

        [HttpGet("roots")]
        public async Task<IActionResult> GetRoots()
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

        [HttpGet("path")]
        public async Task<IActionResult> GetPath(Guid categoryId)
        {
            var path = new List<Category>();
            var node = await _categoryRepository.FindAll(x => x.Id == categoryId).FirstOrDefaultAsync();
            if (node == null) return Json(Array.Empty<object>());
            path.Add(node);
            while (node.ParentId.HasValue)
            {
                var parent =  await _categoryRepository.FindAll(x => x.Id == node.ParentId.Value).FirstOrDefaultAsync();
                if (parent == null) break;
                path.Add(parent);
                node = parent;
            }
            path.Reverse();
            var result = path.Select(c => new { id = c.Id, name = c.Name, parentId = c.ParentId }).ToList();
            return Json(result);
        }

        [HttpGet("parents")]
        public Task<IActionResult> GetParentsCompat() => Task.FromResult<IActionResult>(RedirectToAction(nameof(GetRoots)));
    }
}
