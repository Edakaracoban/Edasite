using edasit.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace edaninsite.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly EdasiteContext _context;

        public AccountController(EdasiteContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.Email == user.Email && u.Sifre == user.Sifre);
                if (existingUser != null)
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz email veya şifre.");
                    return View(user);
                }
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var userExists = _context.Users.Any(u => u.Email == user.Email);
                if (userExists)
                {
                    ModelState.AddModelError("", "Bu email ile zaten kayıtlı bir kullanıcı var.");
                    return View(user);
                }
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
