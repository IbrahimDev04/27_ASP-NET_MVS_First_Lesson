using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccessLayer;
using Pronia.Models;
using Pronia.ViewModels.Slider;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(ProniaContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var data = await _context.Sliders
                .Where(a => !a.isDeleted)
                .Select(s => new GetSliderVM
                {
                    Id = s.Id,
                    Title=s.Title,
                    SubTitle=s.SubTitle,
                    Discount=s.Discount,
                    ImageUrl=s.ImageUrl
                }).ToListAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM svm)
        {
            if (!ModelState.IsValid)
            {
                return View(svm);
            }

            Slider sliders = new Slider
            {
                Title=svm.Title,
                SubTitle=svm.SubTitle,
                Discount=svm.Discount,
                ImageUrl=svm.ImageUrl,
                isDeleted=false,
                CreatedTime=DateTime.Now
            };
            
            await _context.Sliders.AddAsync(sliders);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (data == null) return NotFound();

            UpdateSliderVM swm = new UpdateSliderVM
            {
                Title = data.Title,
                SubTitle=data.SubTitle,
                Discount=data.Discount,
                ImageUrl=data.ImageUrl,
            };

            return View(swm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSliderVM swmVM)
        {
            if(id == null || id < 1) return BadRequest();
            
            var data = _context.Sliders.FirstOrDefault(s => s.Id == id);

            if (data == null) return NotFound();

            data.Title = swmVM.Title;
            data.SubTitle = swmVM.SubTitle;
            data.Discount = swmVM.Discount;
            data.ImageUrl = swmVM.ImageUrl;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.Sliders.FindAsync(id);

            if (data == null) return NotFound();

            _context.Remove(data);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
