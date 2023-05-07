using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Controllers
{
    public class ErrorMController : Controller
    {
        // GET: Error
        public ActionResult Index(string error)
        {
            ViewData["error"] = error;
            return View();
        }
    }
}