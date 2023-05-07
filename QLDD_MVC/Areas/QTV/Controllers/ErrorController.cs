using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.QTV.Controllers
{
    public class ErrorController : Controller
    {
        // GET: QTV/Error
        public ActionResult Index(string error)
        {
            ViewData["error"] = error;
            return View();
        }
    }
}