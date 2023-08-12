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

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    [Authorize]
    public class diemdanhsController : BaseController
    {
        private DataContextDB db = new DataContextDB();
        public ActionResult Index_LopHC(string masv,string root)
        {
            var listtemploptc = new List<LopTC>();
            List<string> dsmaloptc = db.LopTC_SV.Where(x => x.masv == masv).Select(x=>x.maloptc).ToList();
            foreach(string ma1loptc in dsmaloptc)
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
            SetHotengv();
            return View(model);
        }
        public ActionResult GetdiemdanhByDate(string maloptc, DateTime date,string root)
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
            ViewData["ngayddraw"] = date;
            ViewData["root"] = root;
            SetHotengv();
            return View(ttdd);
        }

        public ActionResult DDInfoOfSV_LopTC(string maloptc,string masv)
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
            SetHotengv();
            return View(model);
        }
        public ActionResult ChangeStatus(string maloptc, string masv, DateTime date)
        {
            chitietdd dd = new chitietdd();
            dd.ChangeStatus(maloptc, masv);
            return RedirectToAction("GetdiemdanhByDate", new { maloptc = maloptc, date = date, root= "DsLopTCofGV" });
        }
        public ActionResult TaoHdDD(string maloptc)
        {
            var now = DateTime.Now.Date;
            if (db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault() != null)
                return RedirectToAction("Index", "Error", new { error = "Lớp này đã tạo hoạt động điểm danh trong hôm nay" });

            diemdanh dd = new diemdanh();
            dd.CreateDiemdanh(maloptc);
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd; //Lấy madd vừa tạo

            List<string> ds_masv = db.LopTC_SV.Where(i => i.maloptc == maloptc).Select(x => x.masv).ToList();
            foreach (string ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                {
                    var sv = db.Sinhviens.Find(ma1sv);
                    chitietdd ttdd = new chitietdd();
                    ttdd.CreateChitietdd(madd, ma1sv);
                }
            }
            SetAlert("Tạo hoạt động điểm danh thành công", "success");
            return RedirectToAction("GetdiemdanhByDate", new { maloptc = maloptc, date = now, root = "DsLopTCofGV" });
        }
        public ActionResult HuyHdDD(string maloptc)
        {
            var now = DateTime.Now.Date;
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd; //Lấy madd vừa tạo
            diemdanh dd = new diemdanh();
            dd.DeleteHDDiemdanh(maloptc);
            chitietdd ttdd = new chitietdd();
            ttdd.DeleteHDChitietDD(madd);
            SetAlert("Hủy hoạt động điểm danh thành công", "success");
            return RedirectToAction("DSSVofLopTC", "Sinhviens", new { maloptc = maloptc });
        }

        public ActionResult KetthucHdDD(string maloptc)
        {
            var now = DateTime.Now.Date;
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd; //Lấy madd vừa tạo
            diemdanh dd = new diemdanh();
            dd.KetthucHdDD(maloptc);
            SetAlert("Kết thúc hoạt động điểm danh thành công", "success");
            return RedirectToAction("DSSVofLopTC", "Sinhviens", new { maloptc = maloptc });
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
