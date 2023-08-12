using QLDD_MVC.Controllers;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        private DataContextDB db = new DataContextDB();
        public ActionResult Index(string error)
        {
            ViewData["error"] = error;
            return View();
        }
        public ActionResult DuplicateErrorLopTC(string error, string maloptc)
        {
            List<string> duplicatesv = new List<string>();
            var ds_masv = db.LopTC_SV.Where(x => x.maloptc == maloptc).Select(x => x.masv).ToList();
            foreach (string masv in ds_masv)
            {
                if (db.TempSV.Where(x => x.masv.Equals(masv)).FirstOrDefault() != null)
                {
                    duplicatesv.Add(masv);
                }
            }
            foreach (string masv in db.TempSV.Select(x => x.masv))
            {
                db.TempSV.Remove(db.TempSV.Find(masv));
            }
            db.SaveChanges();
            ViewData["error"] = error;
            return View(duplicatesv);
        }
        public ActionResult DuplicateErrorLopHC(string error, string malophc)
        {
            List<string> duplicatesv = new List<string>();
            var ds_masv = db.Sinhviens.Select(x => x.masv).ToList();
            foreach (string masv in ds_masv)
            {
                if (db.TempSV.Where(x => x.masv.Equals(masv)).FirstOrDefault() != null)
                {
                    duplicatesv.Add(masv);
                }
            }
            foreach (string masv in db.TempSV.Select(x => x.masv))
            {
                db.TempSV.Remove(db.TempSV.Find(masv));
            }
            db.SaveChanges();
            ViewData["error"] = error;
            return View(duplicatesv);
        }
        public ErrorController()
        {
            SetHotengv();
        }
        public void SetHotengv()
        {
            string hotengv = "";
            if (TempData["hotengv"] != null)
                hotengv = TempData["hotengv"] as string;

            TempData.Keep("hotengv");
            ViewBag.hotengv = hotengv;
        }
    }
}