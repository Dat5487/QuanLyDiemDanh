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
    public class LopTCsController : Controller
    {
        private DataContextDB db = new DataContextDB();
        public LopTCsController()
        {
            LoginController lg = new LoginController();
            ViewBag.hotengv = lg.Gethotengv();
        }
        // GET: CBDT/LopTCs
        public ActionResult Index()
        {
            LoginController lg = new LoginController();
            int magv = lg.Getmagv();
            IQueryable<LopTC> model = null;
            var listtemploptc = new List<LopTC>();
            List<int> ds_maloptc = null;
            ds_maloptc = db.GVTCs.Where(x => x.magv == magv).Select(x => x.maloptc).ToList();

            foreach (int ma1loptc in ds_maloptc)
            {
                if (db.LopTCs.Find(ma1loptc) != null)
                    listtemploptc.Add(db.LopTCs.Find(ma1loptc));
            }

            model = listtemploptc.AsQueryable();
            if (model == null)
                return RedirectToAction("Index", "Error", new { error = "Bạn không có lớp tín chỉ nào" });

            return View(model);
        }

        // GET: CBDT/LopTCs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTC lopTC = db.LopTCs.Find(id);
            if (lopTC == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index_LopTC","Sinhviens", new { id = id });
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
