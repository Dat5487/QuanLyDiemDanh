using QLDD_MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.GV.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string error)
        {
            ViewData["error"] = error;
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