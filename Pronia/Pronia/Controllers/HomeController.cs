using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccessLayer;
using Pronia.ViewModels.Categories;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProniaContext _context;

        public HomeController(ProniaContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _context.Categories
                .Where(a => !a.isDeleted)
                .Select(a => new GetCategoryVM
                {
                    Id = a.Id,
                    Name = a.Name,
                })
                .ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> DeleteTest(int id)
        {
            if (id == null || id < 1) BadRequest();
            var data = await _context.Categories.FindAsync(id);
            if (data == null) return NotFound();
            _context.Remove(data);
            await _context.SaveChangesAsync();
            return Content(data.Name);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
