using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccessLayer;
using Pronia.Extensions;
using Pronia.ViewModels.Product;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(ProniaContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var data = await _context.Products
                .Where(p => !p.isDeleted)
                .Select(s => new GetProductAdminVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    SellPrice = s.SellPrice,
                    CostPrice = s.CostPrice,
                    StockCount = s.StockCount,
                    Discount = s.Discount,
                    ImageUrl = s.ImageUrl,
                    Raiting = s.Raiting
                }).ToListAsync();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM cVM)
        {
            if (cVM.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Can Not Be Empty");
            if(!ModelState.IsValid)
                return View(cVM);
            if (!cVM.ImageFile.IsValidType("image"))
                ModelState.AddModelError("ImageFile","Type Error");
            if (!cVM.ImageFile.IsValidSize(200))
                ModelState.AddModelError("ImageFile", "Size Error");
            if(!ModelState.IsValid)
                return View(cVM);

            string fileName = await cVM.ImageFile.FileManageAsync(Path.Combine(_env.WebRootPath,"imgs","products"));


            await _context.Products.AddAsync(new Models.Product
            {
                Name = cVM.Name,
                SellPrice = cVM.SellPrice,
                CostPrice = cVM.CostPrice,
                StockCount = cVM.StockCount,
                Raiting = cVM.Raiting,
                Discount = cVM.Discount,
                CreatedTime = DateTime.Now,
                ImageUrl = Path.Combine("imgs", "products", fileName),
                isDeleted = false
            });

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

    }
}
