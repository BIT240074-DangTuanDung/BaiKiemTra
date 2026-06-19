using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using BaiKiemTra.Data;
using BaiKiemTra.Models;

namespace BaiKiemTra.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const int PageSize = 10;
        private const string CategoriesCacheKey = "EventCategories";

        public EventController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IActionResult> Index(
            string? searchString,
            int? categoryId,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            string? sortOrder,
            int pageIndex = 1)
        {
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategory"] = categoryId;
            ViewData["CurrentDateFrom"] = startDateFrom;
            ViewData["CurrentDateTo"] = startDateTo;

            var events = _context.Events_BIT240074
                .AsNoTracking()
                .Include(e => e.EventCategory_BIT240074)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                events = events.Where(e => e.Name.Contains(searchString) || e.Location.Contains(searchString));

            if (categoryId.HasValue)
                events = events.Where(e => e.EventCategoryId == categoryId.Value);

            if (startDateFrom.HasValue)
                events = events.Where(e => e.StartDate >= startDateFrom.Value);

            if (startDateTo.HasValue)
                events = events.Where(e => e.StartDate <= startDateTo.Value);

            events = sortOrder switch
            {
                "name" => events.OrderBy(e => e.Name),
                "name_desc" => events.OrderByDescending(e => e.Name),
                "date" => events.OrderBy(e => e.StartDate),
                "date_desc" => events.OrderByDescending(e => e.StartDate),
                "price" => events.OrderBy(e => e.Price),
                "price_desc" => events.OrderByDescending(e => e.Price),
                _ => events.OrderBy(e => e.StartDate)
            };

            ViewBag.Categories = await GetCachedCategoriesAsync();

            var pagedEvents = await PagedList<Event_BIT240074>.CreateAsync(events, pageIndex, PageSize);
            return View(pagedEvents);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.Events_BIT240074
                .AsNoTracking()
                .Include(e => e.EventCategory_BIT240074)
                .Include(e => e.EventImages_BIT240074)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await GetCachedCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event_BIT240074 eventItem)
        {
            if (ModelState.IsValid)
            {
                var categoryExists = await _context.EventCategories_BIT240074
                    .AsNoTracking()
                    .AnyAsync(c => c.Id == eventItem.EventCategoryId);

                if (!categoryExists)
                    ModelState.AddModelError("EventCategoryId", "EventCategoryId không tồn tại.");
            }

            if (ModelState.IsValid)
            {
                var overlapping = await _context.Events_BIT240074
                    .AsNoTracking()
                    .AnyAsync(e =>
                        e.Location == eventItem.Location &&
                        e.StartDate < eventItem.EndDate &&
                        e.EndDate > eventItem.StartDate);

                if (overlapping)
                    ModelState.AddModelError("StartDate", "Không được tạo hai sự kiện cùng địa điểm và trùng thời gian.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(eventItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await GetCachedCategoriesAsync();
            return View(eventItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.Events_BIT240074.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            ViewBag.Categories = await GetCachedCategoriesAsync();
            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event_BIT240074 eventItem)
        {
            if (id != eventItem.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var categoryExists = await _context.EventCategories_BIT240074
                    .AsNoTracking()
                    .AnyAsync(c => c.Id == eventItem.EventCategoryId);

                if (!categoryExists)
                    ModelState.AddModelError("EventCategoryId", "EventCategoryId không tồn tại.");
            }

            if (ModelState.IsValid)
            {
                var overlapping = await _context.Events_BIT240074
                    .AsNoTracking()
                    .AnyAsync(e =>
                        e.Id != eventItem.Id &&
                        e.Location == eventItem.Location &&
                        e.StartDate < eventItem.EndDate &&
                        e.EndDate > eventItem.StartDate);

                if (overlapping)
                    ModelState.AddModelError("StartDate", "Không được tạo hai sự kiện cùng địa điểm và trùng thời gian.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EventExistsAsync(eventItem.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await GetCachedCategoriesAsync();
            return View(eventItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.Events_BIT240074
                .AsNoTracking()
                .Include(e => e.EventCategory_BIT240074)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events_BIT240074
                .Include(e => e.EventCategory_BIT240074)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            if (eventItem.StartDate <= DateTime.Now && eventItem.EndDate >= DateTime.Now)
            {
                ModelState.AddModelError("", "Không cho phép xóa sự kiện đang diễn ra.");
                return View(eventItem);
            }

            _context.Events_BIT240074.Remove(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<EventCategory_BIT240074>> GetCachedCategoriesAsync()
        {
            if (!_cache.TryGetValue(CategoriesCacheKey, out List<EventCategory_BIT240074>? categories))
            {
                categories = await _context.EventCategories_BIT240074.AsNoTracking().ToListAsync();
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(CategoriesCacheKey, categories, cacheOptions);
            }
            return categories ?? new List<EventCategory_BIT240074>();
        }

        private void ClearCategoriesCache()
        {
            _cache.Remove(CategoriesCacheKey);
        }

        private async Task<bool> EventExistsAsync(int id)
        {
            return await _context.Events_BIT240074.AsNoTracking().AnyAsync(e => e.Id == id);
        }
    }
}
