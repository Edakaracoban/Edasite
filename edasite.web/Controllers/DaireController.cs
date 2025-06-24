using System;
using System.Linq;
using System.Threading.Tasks;
using edasit.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace edasite.web.Controllers
{
    public class DaireController : Controller
    {
        private readonly EdasiteContext _context;

        public DaireController(EdasiteContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var daireList = _context.Daires.Include(d => d.UserNoNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int metreKareValue))
                {
                    daireList = daireList.Where(d => d.MetreKare == metreKareValue);
                }
                else
                {
                    daireList = daireList.Where(d => false);
                }
            }

            return View(await daireList.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["UserNo"] = new SelectList(_context.Users, "UserNo", "AdSoyad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Daire daire)
        {
            if (daire.MetreKare <= 0)
            {
                ModelState.AddModelError("MetreKare", "Metrekare pozitif bir sayı olmalıdır.");
            }
            if (daire.UserNo <= 0)
            {
                ModelState.AddModelError("UserNo", "Kullanıcı seçimi zorunludur.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["UserNo"] = new SelectList(_context.Users, "UserNo", "AdSoyad", daire.UserNo);
                return View(daire);
            }

            _context.Add(daire);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var daire = _context.Daires.Find(id);
            if (daire == null)
                return NotFound();

            ViewData["UserNo"] = new SelectList(_context.Users, "UserNo", "AdSoyad", daire.UserNo);
            return View(daire);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Daire daire)
        {
            if (id != daire.DaireNo)
                return BadRequest();

            if (daire.MetreKare <= 0)
            {
                ModelState.AddModelError("MetreKare", "Metrekare pozitif bir sayı olmalıdır.");
            }
            if (daire.UserNo <= 0)
            {
                ModelState.AddModelError("UserNo", "Kullanıcı seçimi zorunludur.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["UserNo"] = new SelectList(_context.Users, "UserNo", "AdSoyad", daire.UserNo);
                return View(daire);
            }

            try
            {
                _context.Update(daire);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Daires.Any(e => e.DaireNo == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var daire = _context.Daires
                                .Include(d => d.UserNoNavigation)
                                .FirstOrDefault(d => d.DaireNo == id);

            if (daire == null)
                return NotFound();

            return View(daire);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var daire = _context.Daires.Find(id);
            if (daire != null)
            {
                _context.Daires.Remove(daire);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
