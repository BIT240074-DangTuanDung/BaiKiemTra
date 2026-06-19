using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiKiemTra.Data;
using BaiKiemTra.Models;

namespace BaiKiemTra.Controllers
{
    public class EventImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var images = _context.EventImages_BIT240074
                .AsNoTracking()
                .Include(i => i.Event_BIT240074)
                .ToListAsync();
            return View(await images);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var image = await _context.EventImages_BIT240074
                .AsNoTracking()
                .Include(i => i.Event_BIT240074)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (image == null)
                return NotFound();

            return View(image);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Events = await _context.Events_BIT240074.AsNoTracking().ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventImage_BIT240074 image)
        {
            if (!_context.Events_BIT240074.Any(e => e.Id == image.EventId))
                ModelState.AddModelError("EventId", "EventId không tồn tại.");

            if (image.IsThumbnail)
            {
                var existingThumbnail = await _context.EventImages_BIT240074
                    .FirstOrDefaultAsync(i => i.EventId == image.EventId && i.IsThumbnail);
                if (existingThumbnail != null && existingThumbnail.Id != image.Id)
                {
                    existingThumbnail.IsThumbnail = false;
                    _context.Update(existingThumbnail);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Events = await _context.Events_BIT240074.AsNoTracking().ToListAsync();
            return View(image);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var image = await _context.EventImages_BIT240074.FindAsync(id);
            if (image == null)
                return NotFound();

            ViewBag.Events = await _context.Events_BIT240074.AsNoTracking().ToListAsync();
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventImage_BIT240074 image)
        {
            if (id != image.Id)
                return NotFound();

            if (!_context.Events_BIT240074.Any(e => e.Id == image.EventId))
                ModelState.AddModelError("EventId", "EventId không tồn tại.");

            if (image.IsThumbnail)
            {
                var existingThumbnail = await _context.EventImages_BIT240074
                    .FirstOrDefaultAsync(i => i.EventId == image.EventId && i.IsThumbnail);
                if (existingThumbnail != null && existingThumbnail.Id != image.Id)
                {
                    existingThumbnail.IsThumbnail = false;
                    _context.Update(existingThumbnail);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Events = await _context.Events_BIT240074.AsNoTracking().ToListAsync();
            return View(image);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var image = await _context.EventImages_BIT240074
                .AsNoTracking()
                .Include(i => i.Event_BIT240074)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
                return NotFound();

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.EventImages_BIT240074.FindAsync(id);
            if (image == null)
                return NotFound();

            _context.EventImages_BIT240074.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.EventImages_BIT240074.Any(e => e.Id == id);
        }
    }
}
