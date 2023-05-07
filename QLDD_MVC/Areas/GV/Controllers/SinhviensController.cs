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

namespace QLDD_MVC.Areas.GV.Controllers
{
    public class SinhviensController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: CBDT/Sinhviens
        public ActionResult DsAllSinhVien()
        {
            IEnumerable<Sinhvien> model = model = db.Sinhviens;
            return View(model);
        }
        public ActionResult Index(int? id)
        {
            //Lấy danh sách
            IEnumerable<Sinhvien> model = db.Sinhviens.Where(i => i.malophc == id);
            var sinhvien = db.Sinhviens.Where(i => i.malophc == id);

            if (sinhvien == null)
            {
                return RedirectToAction("Error",new {error = "Giáo viên không tồn tại trong hệ thống"});
            }

            //Hiển thị/ Kiểm tra
            var lop = db.LopHCs.Find(id);
            if (lop != null)
            {
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            }
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenlophc"] = db.LopHCs.Find(id).tenlophc;
            ViewData["malophc"] = id;
            return View(model);
        }

        public ActionResult Index_LopTC(int? id)
        {
            //Lấy danh sách
            IQueryable<Sinhvien> dssv = null; ;
            var listtempsv = new List<Sinhvien>();
            List<int> ds_masv = null;
            ds_masv = db.LopTC_SV.Where(i => i.maloptc == id).Select(x => x.masv).ToList();

            foreach (int ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                    listtempsv.Add(db.Sinhviens.Find(ma1sv));
            }
            IEnumerable<Sinhvien> model = listtempsv.AsQueryable();
            //Kiểm tra
            var sinhvien = db.Sinhviens.Where(i => i.malophc == id);
            if (sinhvien == null)
                return RedirectToAction("NoResult");

            var lop = db.LopTCs.Find(id);
            if (db.giangviens.Where(c => c.magv == lop.magv).FirstOrDefault() != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";
            
            ViewData["tenltc"] = lop.tenltc;
            ViewData["maloptc"] = id;

            return View(model);
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
