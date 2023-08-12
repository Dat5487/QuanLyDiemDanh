using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using QLDD_MVC.Models;
using System.Drawing.Printing;
using System.Web.UI;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;
using QLDD_MVC.Controllers;

namespace QLDD_MVC.Areas.GV.Controllers
{
    [Authorize]
    public class SinhviensController : Controller
    {
        private DataContextDB db = new DataContextDB();
        public ActionResult DsAllSinhVien()
        {
            IEnumerable<Sinhvien> model = db.Sinhviens;
            SetHotengv();
            return View(model);
        }
        public ActionResult Index(string malophc)
        {
            //Lấy danh sách
            IEnumerable<Sinhvien> model = db.Sinhviens.Where(i => i.malophc == malophc);
            var sinhvien = db.Sinhviens.Where(i => i.malophc == malophc);

            if (sinhvien == null)
            {
                return RedirectToAction("Error",new {error = "Giáo viên không tồn tại trong hệ thống"});
            }

            //Hiển thị/ Kiểm tra
            var lop = db.LopHCs.Find(malophc);
            if (lop != null)
            {
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            }
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenlophc"] = db.LopHCs.Find(malophc).tenlophc;
            ViewData["malophc"] = malophc;
            SetHotengv();
            return View(model);
        }

        public ActionResult Index_LopTC(string maloptc)
        {
            //Lấy danh sách
            IQueryable<Sinhvien> dssv = null; ;
            var listtempsv = new List<Sinhvien>();
            List<string> ds_masv = null;
            ds_masv = db.LopTC_SV.Where(i => i.maloptc == maloptc).Select(x => x.masv).ToList();

            foreach (string ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                    listtempsv.Add(db.Sinhviens.Find(ma1sv));
            }
            IEnumerable<Sinhvien> model = listtempsv.AsQueryable();

            var lop = db.LopTCs.Find(maloptc);
            if (db.giangviens.Where(c => c.magv == lop.magv).FirstOrDefault() != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";
            
            ViewData["tenltc"] = lop.tenltc;
            ViewData["maloptc"] = maloptc;
            SetHotengv();
            return View(model);
        }
        public void SetHotengv()
        {
            string hotengv = "";
            if (TempData["hotengv"] != null)
                hotengv = TempData["hotengv"] as string;

            TempData.Keep("hotengv");
            ViewBag.hotengv = hotengv;
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
