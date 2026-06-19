using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using BaiKiemTra.Data;
using BaiKiemTra.Models;

namespace BaiKiemTra.Controllers
{
    public class EventCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string CategoriesCacheKey = "EventCategories";

        public EventCategoryController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        private void ClearCategoriesCache()
        {
            _cache.Remove(CategoriesCacheKey);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCategories_BIT240074.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.EventCategories_BIT240074
                .AsNoTracking()
                .Include(c => c.Events_BIT240074)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCategory_BIT240074 category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                ClearCategoriesCache();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.EventCategories_BIT240074.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventCategory_BIT240074 category)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    ClearCategoriesCache();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.EventCategories_BIT240074
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.EventCategories_BIT240074.FindAsync(id);
            if (category == null)
                return NotFound();

            if (_context.Events_BIT240074.Any(e => e.EventCategoryId == id))
            {
                ModelState.AddModelError("", "Không cho phép xóa loại sự kiện đang có sự kiện sử dụng");
                return View(category);
            }

            _context.EventCategories_BIT240074.Remove(category);
            await _context.SaveChangesAsync();
            ClearCategoriesCache();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.EventCategories_BIT240074.Any(e => e.Id == id);
        }
    }
}
