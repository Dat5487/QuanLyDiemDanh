using QLDD_MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    public class ErrorController : Controller
    {
        // GET: CBDT/Error
        public ActionResult Index(string error)
        {
            ViewData["error"] = error;
            return View();
        }
        public ErrorController()
        {
            LoginController lg = new LoginController();
            ViewBag.hotengv = lg.Gethotengv();
        }
    }
}