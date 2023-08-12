using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLDD_MVC.Controllers;
using QLDD_MVC.Models;

namespace QLDD_MVC.Areas.GV.Controllers
{
    [Authorize]
    public class LopHCsController : Controller
    {
        private DataContextDB db = new DataContextDB();
        public ActionResult Index()
        {
            LoginController lg = new LoginController();
            string magv = "";
            if (TempData["magv"] != null)
                magv = TempData["magv"] as string;

            TempData.Keep("magv");
            if (db.LopHCs.Where(x => x.magv == magv).FirstOrDefault() == null)
                return RedirectToAction("Index", "Error", new { error = "Bạn không chủ nhiệm lớp hành chính nào" });

            IQueryable<LopHC> model = db.LopHCs.Where(x => x.magv == magv);
            SetHotengv();
            return View(model);
        }

        public ActionResult Details(string malophc)
        {
            if (malophc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(malophc);
            if (lopHC == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "Sinhviens", new { malophc = malophc });
        }
        public void SetHotengv()
        {
            string hotengv = "";
            if (TempData["hotengv"] != null)
                hotengv = TempData["hotengv"] as string;

            TempData.Keep("hotengv");
            ViewBag.hotengv = hotengv;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
