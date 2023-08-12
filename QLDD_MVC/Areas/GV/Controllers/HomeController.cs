using QLDD_MVC.Controllers;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QLDD_MVC.Areas.GV.Controllers
{
    [Authorize]
    public class HomeController : Controller
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

            ViewData["sllophcofgv"] = db.LopHCs.Where(x => x.magv == magv).Count();
            ViewData["slloptcofgv"] = model.Count();
            SetHotengv();
            return View();
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