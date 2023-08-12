using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.QTV.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: QTV/Home
        public ActionResult Index()
        {
            ViewData["sltaikhoan"] = db.TaiKhoans.Count();
            ViewData["slgiangvien"] = db.giangviens.Count();

            return View();
        }
    }
}