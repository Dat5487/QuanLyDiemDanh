using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using QLDD_MVC.Controllers;
using QLDD_MVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace QLDD_MVC.Areas.GV.Controllers
{
    [Authorize]
    public class LopTCsController : Controller
    {
        private DataContextDB db = new DataContextDB();
        public ActionResult Index()
        {
            LoginController lg = new LoginController();
            string magv = "";
            if (TempData["magv"] != null)
                magv = TempData["magv"] as string;

            TempData.Keep("magv");
            IQueryable<LopTC> model = null;
            var listtemploptc = new List<LopTC>();
            List<string> ds_maloptc = null;
            ds_maloptc = db.GVTCs.Where(x => x.magv == magv).Select(x => x.maloptc).ToList();

            foreach (string ma1loptc in ds_maloptc)
            {
                if (db.LopTCs.Find(ma1loptc) != null)
                    listtemploptc.Add(db.LopTCs.Find(ma1loptc));
            }

            model = listtemploptc.AsQueryable();
            if (model == null)
                return RedirectToAction("Index", "Error", new { error = "Bạn không có lớp tín chỉ nào" });
            SetHotengv();
            return View(model);
        }

        public ActionResult Details(string maloptc)
        {
            if (maloptc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTC lopTC = db.LopTCs.Find(maloptc);
            if (lopTC == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index_LopTC","Sinhviens", new { maloptc = maloptc });
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
