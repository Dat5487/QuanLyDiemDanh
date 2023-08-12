using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLDD_MVC.Areas.CBDT.Data;
using QLDD_MVC.Areas.QTV.Controllers;
using QLDD_MVC.Controllers;
using QLDD_MVC.Models;
using System.Data.Odbc;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Data.Entity.SqlServer;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    [Authorize]

    public class LopHCsController : BaseController
    {
        private DataContextDB db = new DataContextDB();
        LopHC lophc = new LopHC();

        public ActionResult Index()
        {
            var dao = new ListAllPaging();
            var model = dao.ListAllLopHCPaging();
            SetHotengv();
            return View(model);
        }

        public ActionResult ListLopHCofGV()
        {
            string magv = "";
            if (TempData["magv"] != null)
                magv = TempData["magv"] as string;

            TempData.Keep("magv");
            var dao = new ListAllPaging();
            var model = dao.ListAllLopHCofGVPaging(magv);
            if (model == null)
                return RedirectToAction("Index", "Error", new { error = "Bạn không chủ nhiệm lớp hành chính nào" });
            SetHotengv();
            return View(model);
        }

        public ActionResult Details(string malophc,string root)
        {
            if (malophc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(malophc);
            if (lopHC == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index", "Sinhviens", new { malophc = malophc, root = root });
        }

        public ActionResult Create()
        {
            ViewBag.GV = db.giangviens.ToList();
            SetHotengv();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "malophc,tenlophc,magv,khoa")] LopHC lopHC)
        {
            if (ModelState.IsValid)
            {

                if (db.giangviens.Find(lopHC.magv) == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Mã Giảng viên không tồn tại trong hệ thống" });
                }
                giangvien gv = db.giangviens.Find(lopHC.magv);
                if (db.TaiKhoans.Find(gv.username) == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Giảng viên này chưa có tài khoản trong hệ thống" });
                }
                if (db.giangviens.Where(x=>x.username == gv.username).FirstOrDefault() == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Giảng viên này chưa có tên đăng nhập trong danh sách giảng viên" });
                }
                if (db.TaiKhoans.Find(gv.username).phanquyen == "Quản trị viên")
                {
                    return RedirectToAction("Index", "Error", new { error = "Tên đăng nhập không được phân quyền phù hợp" });
                }
                lopHC.CreateLopHC(lopHC.tenlophc, lopHC.magv, lopHC.khoa);

                GVCN gvcn = new GVCN();
                string malophcmoitao = db.LopHCs.OrderByDescending(x => x.malophc).FirstOrDefault().malophc;
                gvcn.CreateGVCN(gv.magv, malophcmoitao, gv.username);

                SetAlert("Thêm lớp hành chính thành công", "success");
                return RedirectToAction("Index");
            }
            SetHotengv();
            return View(lopHC);
        }

        // GET: CBDT/LopHCs/Edit/5
        public ActionResult Edit(string malophc)
        {
            ViewBag.GV = db.giangviens.ToList();
            if (malophc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(malophc);
            if (lopHC == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(lopHC);
        }

        // POST: CBDT/LopHCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "malophc,tenlophc,magv,khoa")] LopHC lopHC)
        {
            if (lopHC.khoa == "Chọn khoa : ")
            {
                ModelState.AddModelError(nameof(lopHC.khoa), "Bắt buộc phải chọn khoa");
                return View(lopHC);
            }
            if (ModelState.IsValid)
            {
                if (db.giangviens.Find(lopHC.magv) == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Giảng viên không tồn tại trong hệ thống" });
                }
                giangvien gv = db.giangviens.Find(lopHC.magv);
                if (db.giangviens.Where(x => x.username == gv.username).FirstOrDefault() == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Giảng viên này chưa có tên đăng nhập" });
                }
                if (db.TaiKhoans.Find(gv.username).phanquyen.TrimEnd(' ') == "Quản trị viên")
                {
                    return RedirectToAction("Index", "Error", new { error = "Tên đăng nhập không được phân quyền phù hợp" });
                }
                lopHC.EditLopHC(lopHC.malophc,lopHC.tenlophc, lopHC.magv, lopHC.khoa);

                GVCN gvcn = new GVCN();
                gvcn.EditGVCN(gv.magv, lopHC.malophc, gv.username);
                SetAlert("Chỉnh sửa lớp hành chính thành công", "success");
                return RedirectToAction("Index");
            }
            SetHotengv();
            return View(lopHC);

        }


        // GET: CBDT/LopHCs/Delete/5
        public ActionResult Delete(string malophc)
        {
            if (malophc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(malophc);
            if (lopHC == null)  
            {
                return HttpNotFound();
            }
            if (db.Sinhviens.Where(c => c.malophc == malophc).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Error", new { error = "Không thể xóa lớp do vẫn còn sinh viên trong lớp" });
            }
            SetHotengv();
            return View(lopHC);
        }

        // POST: CBDT/LopHCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string malophc)
        {
            LopHC lopHC = db.LopHCs.Find(malophc);
            giangvien gv = db.giangviens.Find(lopHC.magv);
            db.LopHCs.Remove(lopHC);
            db.SaveChanges();

            GVCN gvcn = new GVCN();
            gvcn.DeleteGVCN(gv.magv, lopHC.malophc);
            SetAlert("Xóa lớp hành chính thành công", "success");
            return RedirectToAction("Index");
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
