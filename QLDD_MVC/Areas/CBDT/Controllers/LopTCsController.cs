using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using QLDD_MVC.Areas.CBDT.Data;
using QLDD_MVC.Areas.QTV.Controllers;
using QLDD_MVC.Areas.QTV.Data;
using QLDD_MVC.Controllers;
using QLDD_MVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    public class LopTCsController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: CBDT/LopTCs
        public ActionResult Index()
        {
            var dao = new Data.ListAllPaging();
            var model = dao.ListAllLopTCPaging();
            return View(model);
        }
        public ActionResult ListLopTCofGV()
        {
            LoginController lg = new LoginController();
            int magv = lg.Getmagv();
            var dao = new Data.ListAllPaging();
            var model = dao.ListAllLopTCofGVPaging(magv);
            return View(model);
        }
        // GET: CBDT/LopTCs/Details/5
        public ActionResult Details(int? id,string root)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTC lopTC = db.LopTCs.Find(id);
            if (lopTC == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index_LopTC","Sinhviens", new { id = id,root = root});
        }

        // GET: CBDT/LopTCs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CBDT/LopTCs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maloptc,mahp,magv,sttlop,trangthai")] LopTC lopTC)
        {

            if (ModelState.IsValid)
            {
                //Kiểm tra giáo viên có tồn tại trong bảng giaovien không
                if (db.giangviens.Find(lopTC.magv) == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Mã giáo viên không tồn tại trong hệ thống" });
                }
                giangvien gv = db.giangviens.Find(lopTC.magv);
                if (db.giangviens.Where(x => x.username == gv.username).FirstOrDefault() == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Giáo viên này chưa có tên đăng nhập" });
                }
                if (db.Hocphans.Find(lopTC.mahp) == null)
                    return RedirectToAction("Index", "Error", new { error = "Mã học phần không tồn tại trong hệ thống" });
                //Phải tạo lớp TC mới thì mới tìm được maloptc tự tạo
                db.LopTCs.Add(lopTC);
                db.SaveChanges();
                LopTC loptc = new LopTC();
                loptc.EditTenLopTC(lopTC.maloptc, lopTC.mahp,lopTC.sttlop);
                //Tạo bản ghi mới cho GVTC
                GVTC gvtc = new GVTC();
                gvtc.CreateGVTC(gv.magv, lopTC.maloptc, gv.username);
                return RedirectToAction("Index");
            }
            return View(lopTC);
        }

        // GET: CBDT/LopTCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTC lopTC = db.LopTCs.Find(id);
            if (lopTC == null)
            {
                return HttpNotFound();
            }
            ViewData["magv"] = lopTC.magv;
            return View(lopTC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maloptc,mahp,magv,sttlop,trangthai")] LopTC lopTC)
        {
            if (ModelState.IsValid)
            {
                giangvien gv = db.giangviens.Find(lopTC.magv);
                if (gv == null)
                    return RedirectToAction("Index", "Error", new { error = "Mã Giảng viên không tồn tại trong hệ thống" });
                if (db.giangviens.Where(x => x.username == gv.username).FirstOrDefault() == null)
                    return RedirectToAction("Index", "Error", new { error = "Giảng viên này chưa có tên đăng nhập" });
                if (db.Hocphans.Find(lopTC.mahp) == null)
                    return RedirectToAction("Index", "Error", new { error = "Mã học phần không tồn tại trong hệ thống" });

                db.Entry(lopTC).State = EntityState.Modified;
                db.SaveChanges();
                GVTC gvtc = new GVTC();
                gvtc.EditGVTC(gv.magv, lopTC.maloptc, gv.username);
                LopTC loptc = new LopTC();
                loptc.EditTenLopTC(lopTC.maloptc, lopTC.mahp, lopTC.sttlop);

                return RedirectToAction("Index");
            }
            return View(lopTC);
        }
            // GET: CBDT/LopTCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTC lopTC = db.LopTCs.Find(id);
            if (lopTC == null)
            {
                return HttpNotFound();
            }

            return View(lopTC);
        }

        // POST: CBDT/LopTCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            LopTC lopTC = db.LopTCs.Find(id); 
            giangvien gv = db.giangviens.Find(lopTC.magv);
            //Lấy danh sách mã sinh viên có cùng maloptc
            var dsmasv = db.LopTC_SV.Where(x => x.maloptc == id);

            //Nếu lớp tc đã tạo hoạt động điểm danh thì xóa dd
            if (db.diemdanhs.Where(x => x.maloptc == id).FirstOrDefault() != null)
            {
                //Tìm madd
                int madd = db.diemdanhs.Where(x => x.maloptc == id).FirstOrDefault().madd;
                //Xóa từng sv khỏi bảng chitietdd va LopTC_SV
                chitietdd ctdd = new chitietdd();
                foreach (LopTC_SV sv in dsmasv)
                {
                    ctdd.DeleteChitietdd(madd, sv.masv);
                }
                //Xóa bản ghi ở bảng diemdanh
                diemdanh dd = new diemdanh();
                dd.DeleteDiemdanh(id);
            }

            //Xóa từng sv khỏi bảng LopTC_SV
            LopTC_SV loptc_sv = new LopTC_SV();
            foreach (LopTC_SV sv in dsmasv)
            {
                loptc_sv.DeleteLopTC_SV(id, sv.masv);
            }
            //Xóa bản ghi ở bảng GVTC
            GVTC gvtc = new GVTC();
            gvtc.DeleteGVTC(gv.magv, lopTC.maloptc);
            //Xóa bản ghi ở bảng LopTC
            db.LopTCs.Remove(lopTC);
            db.SaveChanges();
            return RedirectToAction("Index");
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
