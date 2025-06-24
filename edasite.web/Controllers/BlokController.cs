using edasit.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class BlokController : Controller
{
    private readonly EdasiteContext _context;
    public BlokController(EdasiteContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;

        var blokList = _context.Bloks
                               .Include(p => p.SiteNoNavigation)
                               .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            blokList = blokList.Where(p => p.BlokAdi != null && p.BlokAdi.Contains(searchString));

        }

        return View(await blokList.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi");
        return View();
    }
    [HttpPost]
    public IActionResult Create(Blok blok)
    {
        if (string.IsNullOrWhiteSpace(blok.BlokAdi))
        {
            ModelState.AddModelError("BlokAdi", "Blok Adı boş olamaz.");
        }
        if (blok.KatSayisi.HasValue && (blok.KatSayisi < 1 || blok.KatSayisi > 50))
        {
            ModelState.AddModelError("KatSayisi", "Kat Sayısı 1 ile 50 arasında olmalıdır.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi", blok.SiteNo);
            return View(blok);
        }

        _context.Add(blok);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var blok = _context.Bloks.Find(id);
        if (blok == null)
            return NotFound();

        ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi", blok.SiteNo);
        return View(blok);
    }

    [HttpPost]
    public IActionResult Edit(int id, Blok blok)
    {
        if (id != blok.BlokNo)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            ViewData["SiteNo"] = new SelectList(_context.Sites, "SiteNo", "SiteAdi", blok.SiteNo);
            return View(blok);
        }

        _context.Update(blok);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var blok = _context.Bloks
            .Include(b => b.SiteNoNavigation)
            .FirstOrDefault(p => p.BlokNo == id);

        if (blok == null)
            return NotFound();

        return View(blok);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var blok = _context.Bloks.Find(id);
        if (blok != null)
        {
            _context.Bloks.Remove(blok);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}
