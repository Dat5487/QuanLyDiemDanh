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
    public class LopHCsController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: CBDT/LopHCs
        public ActionResult Index()
        {
            LoginController lg = new LoginController();
            int magv = lg.Getmagv();
            if (db.LopHCs.Where(x => x.magv == magv).FirstOrDefault() == null)
                return RedirectToAction("Index", "Error", new { error = "Bạn không chủ nhiệm lớp hành chính nào" });

            IQueryable<LopHC> model = db.LopHCs.Where(x => x.magv == magv);
            return View(model);
        }

        // GET: CBDT/LopHCs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(id);
            if (lopHC == null)
            {
                return HttpNotFound();
            }
            string searchString = null;
            //string idtype;
            return RedirectToAction("Index", "Sinhviens", new { id = id });
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
