using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLDD_MVC.Models;
using QLDD_MVC.Dao;
using QLDD_MVC.Common;
using System.Web.Routing;
using QLDD_MVC.Areas.QTV.Data;
using QLDD_MVC.Code;
using QLDD_MVC.Controllers;

namespace QLDD_MVC.Areas.QTV.Controllers
{
    [Authorize]
    public class TaiKhoansController : BaseController
    {
        private DataContextDB db = new DataContextDB();

        // GET: QTV/TaiKhoans

        public ActionResult Index()
        {
            var model = db.TaiKhoans;
            return View(model);
        }

        // GET: QTV/TaiKhoans/Details/5
        public ActionResult Details(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(username);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: QTV/TaiKhoans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QTV/TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,hoten,phanquyen,password")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                taiKhoan.password = Encryptor.MD5Hash(taiKhoan.password);
                if(db.TaiKhoans.Find(taiKhoan.username) != null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Tên đăng nhập đã tồn tại trong hệ thống" });
                }
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                if(taiKhoan.phanquyen == "Cán bộ quản lý đào tạo")
                {
                    giangvien gv = new giangvien();
                    gv.Creategiangvien(taiKhoan.username,taiKhoan.hoten);
                }
                SetAlert("Tạo tài khoản thành công", "success");
                return RedirectToAction("Index");
            }

            return View(taiKhoan);
        }

        // GET: QTV/TaiKhoans/Edit/5
        public ActionResult Edit(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(username);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: QTV/TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,hoten,phanquyen,password")] TaiKhoan taiKhoan,string pass)
        {
            if (ModelState.IsValid)
            {
                if (taiKhoan.password == null)
                    taiKhoan.password = pass;
                else
                    taiKhoan.password = Encryptor.MD5Hash(taiKhoan.password);
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                giangvien gv = new giangvien();
                if(taiKhoan.phanquyen== "Cán bộ quản lý đào tạo" && db.giangviens.Where(x=>x.username == taiKhoan.username).FirstOrDefault() != null )
                    gv.Editgiangvien(db.giangviens.Where(x=>x.username == taiKhoan.username).FirstOrDefault().magv, taiKhoan.hoten);
                SetAlert("Chỉnh sửa tài khoản thành công", "success");
                return RedirectToAction("Index");
            }
            return View(taiKhoan);
        }

        // GET: QTV/TaiKhoans/Delete/5
        public ActionResult Delete(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(username);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: QTV/TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string username)
        {
            if(db.giangviens.FirstOrDefault(x=>x.username == username) != null)
            {
                return RedirectToAction("Index", "Error", new { error = "Tài khoản này đang được giảng viên sử dụng"});
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(username);
            db.TaiKhoans.Remove(taiKhoan);
            db.SaveChanges();
            SetAlert("Xóa tài khoản thành công", "success");
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
