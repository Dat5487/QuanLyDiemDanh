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

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    public class LopHCsController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: CBDT/LopHCs
        public LopHCsController()
        {
            LoginController lg = new LoginController();
            ViewBag.hotengv = lg.Gethotengv();
        }
        public ActionResult Index()
        {
            var dao = new ListAllPaging();
            var model = dao.ListAllLopHCPaging();
            return View(model);
        }


        public ActionResult ListLopHCofGV()
        {
            LoginController lg = new LoginController();
            int magv = lg.Getmagv();
            var dao = new ListAllPaging();
            var model = dao.ListAllLopHCofGVPaging(magv);
            if (model == null)
                return RedirectToAction("Index", "Error", new { error = "Bạn không chủ nhiệm lớp hành chính nào" });
            return View(model);
        }
        // GET: CBDT/LopHCs/Details/5
        public ActionResult Details(int? id,string root)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(id);
            if (lopHC == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index", "Sinhviens", new { id = id, root = root });
        }

        // GET: CBDT/LopHCs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CBDT/LopHCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                db.LopHCs.Add(lopHC);
                db.SaveChanges();

                GVCN gvcn = new GVCN();
                gvcn.CreateGVCN(gv.magv, lopHC.malophc, gv.username);
                return RedirectToAction("Index");
            }
            return View(lopHC);
        }

        // GET: CBDT/LopHCs/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(id);
            if (lopHC == null)
            {
                return HttpNotFound();
            }
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
                db.Entry(lopHC).State = EntityState.Modified;
                db.SaveChanges();
                GVCN gvcn = new GVCN();
                gvcn.EditGVCN(gv.magv, lopHC.malophc, gv.username);
                return RedirectToAction("Index");
            }
            return View(lopHC);

        }


        // GET: CBDT/LopHCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopHC lopHC = db.LopHCs.Find(id);
            if (lopHC == null)
            {
                return HttpNotFound();
            }
            if (db.Sinhviens.Where(c => c.malophc == id).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Error", new { error = "Không thể xóa lớp do vẫn còn sinh viên trong lớp" });
            }
            return View(lopHC);
        }

        // POST: CBDT/LopHCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LopHC lopHC = db.LopHCs.Find(id);
            giangvien gv = db.giangviens.Find(lopHC.magv);
            db.LopHCs.Remove(lopHC);
            db.SaveChanges();

            GVCN gvcn = new GVCN();
            gvcn.DeleteGVCN(gv.magv, lopHC.malophc);
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
