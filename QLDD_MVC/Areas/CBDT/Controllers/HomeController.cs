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

    public class HomeController : Controller
    {
        private DataContextDB db = new DataContextDB();
        public ActionResult Index()
        {
            string magv = "";
            if (TempData["magv"] != null)
                magv = TempData["magv"] as string;

            TempData.Keep("magv");
            var dao = new Data.ListAllPaging();
            ViewData["slsinhvien"] = db.Sinhviens.Count();
            ViewData["slgiangvien"] = db.giangviens.Count();
            ViewData["slhocphan"] = db.Hocphans.Count();
            ViewData["slloptc"] = db.LopTCs.Count();
            ViewData["sllophc"] = db.LopHCs.Count();
            if(dao.ListAllLopHCofGVPaging(magv) == null)
            {
                ViewData["sllophcofgv"] = 0;
            }
            else
            {
                ViewData["sllophcofgv"] = dao.ListAllLopHCofGVPaging(magv).Count();
            }
            ViewData["slloptcofgv"] = dao.ListAllLopTCofGVPaging(magv).Count();
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