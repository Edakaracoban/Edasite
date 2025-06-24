using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edasit.Data.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace edasite.web.Controllers
{
    public class UserController : Controller
    {
        private readonly EdasiteContext _context;

        public UserController(EdasiteContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var user = from s in _context.Users select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                user = user.Where(s => s.AdSoyad != null && s.AdSoyad.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;

            return View(user.ToList());
        }



        public IActionResult Create() //oluşturma
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id) //güncelleme
        {
            var update = _context.Users.Find(id);
            return View(update);
        }

        [HttpPost]
        public IActionResult Edit(int id, User user)
        {
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(p => p.UserNo == id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}

