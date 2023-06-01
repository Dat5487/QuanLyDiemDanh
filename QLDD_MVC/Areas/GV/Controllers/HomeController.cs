using QLDD_MVC.Controllers;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.GV.Controllers
{
    public class HomeController : Controller
    {
        // GET: CBDT/Home
        private DataContextDB db = new DataContextDB();
        public HomeController()
        {
            LoginController lg = new LoginController();
            ViewBag.hotengv = lg.Gethotengv();
        }
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

            ViewData["sllophcofgv"] = db.LopHCs.Where(x => x.magv == magv).Count();
            ViewData["slloptcofgv"] = model.Count();


            return View();
        }
    }
}