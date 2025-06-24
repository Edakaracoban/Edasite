using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edasit.Data.Models;
using edasite.web.Views.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace edasite.web.Controllers
{
    public class ReportController : Controller
    {
        private readonly EdasiteContext _context;

        private List<string> reportOptions = new List<string> { "Site", "User", "Blok", "Daire", "Aidat" };

        public ReportController(EdasiteContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string reportType)
        {
            ViewData["ReportOptions"] = reportOptions;
            ViewData["SelectedReport"] = reportType;

            if (string.IsNullOrEmpty(reportType))
            {
                return View();
            }

            switch (reportType)
            {
                case "Site":
                    var sites = await _context.Sites
                        .OrderBy(s => s.SiteNo)
                        .Select(s => new SiteReportViewModel
                        {
                            SiteNo = s.SiteNo,
                            SiteAdi = s.SiteAdi,
                            Adres = s.Adres,
                            Bilgi = s.Bilgi
                        })
                        .ToListAsync();
                    return View("SiteReport", sites);

                case "User":
                    var users = await _context.Users
                        .OrderBy(u => u.UserNo)
                        .Select(u => new UserReportViewModel
                        {
                            UserNo = u.UserNo,
                            AdSoyad = u.AdSoyad,
                            Email = u.Email,
                            Telefon = u.Telefon,
                            Rol = u.Rol
                        })
                        .ToListAsync();
                    return View("UserReport", users);

                case "Blok":
                    var bloks = await (
                        from b in _context.Bloks
                        join s in _context.Sites on b.SiteNo equals s.SiteNo
                        orderby b.BlokNo
                        select new BlokReportViewModel
                        {
                            BlokNo = b.BlokNo,
                            SiteNo = s.SiteNo,
                            SiteAdi = s.SiteAdi,
                            BlokAdi = b.BlokAdi,
                            KatSayisi = b.KatSayisi
                        }
                    ).ToListAsync();
                    return View("BlokReport", bloks);

                case "Daire":
                    var daires = await (
                        from d in _context.Daires
                        join u in _context.Users on d.UserNo equals u.UserNo
                        orderby d.DaireNo
                        select new DaireReportViewModel
                        {
                            DaireNo = d.DaireNo,
                            MetreKare = d.MetreKare,
                            UserAdSoyad = u.AdSoyad,
                            Email = u.Email,
                            Telefon = u.Telefon
                        }
                    ).ToListAsync();
                    return View("DaireReport", daires);

                case "Aidat":
                    var aidatlar = await (
                        from a in _context.Aidats
                        join s in _context.Sites on a.SiteNo equals s.SiteNo
                        orderby a.SiteNo, a.Donem
                        select new AidatReportViewModel
                        {
                            AidatNo = a.AidatNo,
                            SiteNo = s.SiteNo,
                            SiteAdi = s.SiteAdi,
                            Donem = a.Donem,
                            Tutar = a.Tutar,
                            OdemeDurumu = a.OdemeDurumu
                        }
                    ).ToListAsync();
                    return View("AidatReport", aidatlar);

                default:
                    return View();
            }
        }
    }
}

