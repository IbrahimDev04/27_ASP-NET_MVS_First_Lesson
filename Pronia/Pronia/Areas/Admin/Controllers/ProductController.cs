using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccessLayer;
using Pronia.Extensions;
using Pronia.Models;
using Pronia.ViewModels.Product;
using System.Text;

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
            bool isImageValid = true;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in cVM.ImagesFile)
            {
                if (!item.IsValidType("image"))
                {
                    stringBuilder.Append("{ " + item.FileName + " is not picture. }");
                    isImageValid = false;
                    //ModelState.AddModelError("ImagesFile", item.FileName + " is not picture.");
                }
                if (!item.IsValidSize(200))
                {
                    stringBuilder.Append("{ " + item.FileName + " size is a lot then 200. }");
                    isImageValid = false;
                    //ModelState.AddModelError("ImagesFile", item.FileName + " size is a lot then 200.");
                }
                if (!isImageValid)
                {
                    ModelState.AddModelError("ImagesFile", stringBuilder.ToString());
                }
            }

            if (!ModelState.IsValid)
                return View(cVM);

            string fileName = await cVM.ImageFile.FileManageAsync(Path.Combine(_env.WebRootPath,"imgs","products"));

            Product product = new Models.Product
            {
                Name = cVM.Name,
                SellPrice = cVM.SellPrice,
                CostPrice = cVM.CostPrice,
                StockCount = cVM.StockCount,
                Raiting = cVM.Raiting,
                Discount = cVM.Discount,
                CreatedTime = DateTime.Now,
                ImageUrl = Path.Combine("imgs", "products", fileName),
                isDeleted = false,
                Images = new List<ProductImage>()
            };

            foreach (var item in cVM.ImagesFile)
            {
                string imgFileName = await item.FileManageAsync(Path.Combine(_env.WebRootPath, "imgs", "products"));
                product.Images.Add(new ProductImage
                {
                    ImageUrl = Path.Combine("imgs", "products", imgFileName),
                    CreatedTime = DateTime.Now,
                    isDeleted = false,
                }); ;
            }


            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

    }
}
