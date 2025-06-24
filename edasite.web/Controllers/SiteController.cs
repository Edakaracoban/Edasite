using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edasit.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace edasite.web.Controllers
{
    public class SiteController : Controller
    {
        private readonly EdasiteContext _context;

        public SiteController(EdasiteContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchString)
        {
            var sites = from s in _context.Sites select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                sites = sites.Where(s => s.SiteAdi != null && s.SiteAdi.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;

            return View(sites.ToList());
        }



        public IActionResult Create() //oluşturma
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Site site)
        {
            _context.Add(site);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id) //güncelleme
        {
            var update = _context.Sites.Find(id);
            return View(update);
        }

        [HttpPost]
        public IActionResult Edit(int id, Site site)
        {
            _context.Update(site);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var site = _context.Sites.FirstOrDefault(p => p.SiteNo == id);
            if (site == null)
                return NotFound();
            return View(site);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var site = _context.Sites.Find(id);
            if (site != null)
            {
                _context.Sites.Remove(site);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}

