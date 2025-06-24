using System;
using System.Linq;
using System.Threading.Tasks;
using edasit.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace edasite.web.Controllers
{
    public class AidatController : Controller
    {
        private readonly EdasiteContext _context;

        public AidatController(EdasiteContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string odemeDurumu)
        {
            ViewData["CurrentOdemeDurumu"] = odemeDurumu;

            var aidatlar = _context.Aidats
                                   .Include(a => a.SiteNoNavigation)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(odemeDurumu))
            {
                aidatlar = aidatlar.Where(a => a.OdemeDurumu == odemeDurumu);
            }

            return View(await aidatlar.ToListAsync());
        }



        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Aidat aidat)
        {
            if (!ModelState.IsValid)
            {
                ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi", aidat.SiteNo);
                return View(aidat);
            }

            _context.Add(aidat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var aidat = await _context.Aidats.FindAsync(id);
            if (aidat == null)
                return NotFound();

            ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi", aidat.SiteNo);
            return View(aidat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Aidat aidat)
        {
            if (id != aidat.AidatNo)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi", aidat.SiteNo);
                return View(aidat);
            }

            try
            {
                _context.Update(aidat);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AidatExists(aidat.AidatNo))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var aidat = await _context.Aidats
                                      .Include(a => a.SiteNoNavigation)
                                      .FirstOrDefaultAsync(m => m.AidatNo == id);
            if (aidat == null)
                return NotFound();

            return View(aidat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aidat = await _context.Aidats.FindAsync(id);
            if (aidat != null)
            {
                _context.Aidats.Remove(aidat);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AidatExists(int id)
        {
            return _context.Aidats.Any(e => e.AidatNo == id);
        }
    }
}
