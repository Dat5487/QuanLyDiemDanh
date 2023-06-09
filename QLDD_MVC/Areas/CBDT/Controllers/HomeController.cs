﻿using QLDD_MVC.Controllers;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    public class HomeController : Controller
    {
        private DataContextDB db = new DataContextDB();
        // GET: CBDT/Home
        public HomeController()
        {
            LoginController lg = new LoginController();
            ViewBag.hotengv = lg.Gethotengv();
        }
        public ActionResult Index()
        {
            LoginController lg = new LoginController();
            int magv = lg.Getmagv();
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

            return View();
        }
    }
}