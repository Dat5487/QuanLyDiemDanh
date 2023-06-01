using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using QLDD_MVC.Models;
using System.Diagnostics;
using QLDD_MVC.Controllers;

namespace QLDD_MVC.Areas.GV.Controllers
{
    public class diemdanhsController : Controller
    {
        private DataContextDB db = new DataContextDB();
        public diemdanhsController()
        {
            LoginController lg = new LoginController();
            ViewBag.hotengv = lg.Gethotengv();
        }
        public ActionResult Index_LopHC(string masv, string root)
        {
            var listtemploptc = new List<LopTC>();
            List<int> dsmaloptc = db.LopTC_SV.Where(x => x.masv == masv).Select(x => x.maloptc).ToList();
            foreach (int ma1loptc in dsmaloptc)
            {
                if (db.LopTCs.Find(ma1loptc) != null)
                {
                    listtemploptc.Add(db.LopTCs.Find(ma1loptc));
                }
            }

            IEnumerable<LopTC> model = listtemploptc.AsQueryable();
            ViewData["masv"] = masv;
            ViewData["hoten"] = db.Sinhviens.Find(masv).hoten;
            ViewData["gioitinh"] = db.Sinhviens.Find(masv).gioitinh;
            ViewData["tenlophc"] = db.LopHCs.Find(db.Sinhviens.Find(masv).malophc).tenlophc;
            ViewData["malophc"] = db.LopHCs.Find(db.Sinhviens.Find(masv).malophc).malophc;
            ViewData["root"] = root;

            return View(model);
        }
        public ActionResult GetdiemdanhByDate(int maloptc, DateTime date)
        {
            date = date.Date;
            var listtempsv = new List<DSSVxChitietdd>();
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == date).FirstOrDefault().madd;
            var ttddofdate = db.chitietdds.Where(x => x.madd == madd);
            var ds_masv = ttddofdate.Select(x => x.masv).ToList();

            foreach (string ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                {
                    DSSVxChitietdd svdd = new DSSVxChitietdd { };
                    var sv = db.Sinhviens.Find(ma1sv);
                    //Gán giá trị
                    svdd.masv = sv.masv;
                    svdd.hoten = sv.hoten;
                    svdd.gioitinh = sv.gioitinh;
                    svdd.khoa = sv.khoa;
                    svdd.thoigiandd = db.chitietdds.Where(x => x.madd == madd && x.masv.Equals(sv.masv)).FirstOrDefault().thoigiandd;
                    svdd.tenlophc = db.LopHCs.Find(sv.malophc).tenlophc;
                    svdd.trangthai = ttddofdate.Where(x => x.masv.Equals(ma1sv)).FirstOrDefault().trangthai;
                    listtempsv.Add(svdd);
                }
            }
            IEnumerable<DSSVxChitietdd> ttdd = listtempsv.AsQueryable();
            ViewData["tentc"] = db.LopTCs.Find(maloptc).tenltc;
            ViewData["maloptc"] = maloptc;
            ViewData["ngaydd"] = date.ToString("dd/MM/yyyy");

            return View(ttdd);
        }

        public ActionResult DDInfoOfSV_LopTC(int? maloptc, string masv)
        {
            List<int> dsmadd = db.diemdanhs.Where(x => x.maloptc == maloptc).Select(x => x.madd).ToList();
            var dsttdd = new List<chitietdd>();
            foreach (int madd in dsmadd)
            {
                dsttdd.Add(db.chitietdds.FirstOrDefault(x => x.masv.Equals(masv) && x.madd == madd));
            }
            IEnumerable<chitietdd> model = dsttdd.AsQueryable();
            ViewData["hoten"] = db.Sinhviens.Find(masv).hoten;
            ViewData["gioitinh"] = db.Sinhviens.Find(masv).gioitinh;
            ViewData["tenlophc"] = db.LopHCs.Find(db.Sinhviens.Find(masv).malophc).tenlophc;
            ViewData["malophc"] = db.LopHCs.Find(db.Sinhviens.Find(masv).malophc).malophc;
            ViewData["khoa"] = db.Sinhviens.Find(masv).khoa;
            ViewData["tenloptc"] = db.LopTCs.Find(maloptc).tenltc;

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
