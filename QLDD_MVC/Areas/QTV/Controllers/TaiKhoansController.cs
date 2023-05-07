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

namespace QLDD_MVC.Areas.QTV.Controllers
{
    public class TaiKhoansController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: QTV/TaiKhoans

        public ActionResult Index()
        {
            var model = db.TaiKhoans;
            return View(model);
        }

        // GET: QTV/TaiKhoans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
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
                if(db.TaiKhoans.Find(taiKhoan.username) != null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Tên đăng nhập đã tồn tại trong hệ thống" });
                }
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                if(taiKhoan.phanquyen == "Cán bộ đào tạo")
                {
                    giangvien gv = new giangvien();
                    gv.Creategiangvien(taiKhoan.username,taiKhoan.hoten);
                }    
                return RedirectToAction("Index");
            }

            return View(taiKhoan);
        }

        // GET: QTV/TaiKhoans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
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
        public ActionResult Edit([Bind(Include = "username,hoten,phanquyen,password")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                giangvien gv = new giangvien();
                if(taiKhoan.phanquyen== "Cán bộ đào tạo" && db.giangviens.Where(x=>x.username == taiKhoan.username).FirstOrDefault() != null )
                    gv.Editgiangvien(db.giangviens.Where(x=>x.username == taiKhoan.username).FirstOrDefault().magv, taiKhoan.hoten);
                return RedirectToAction("Index");
            }
            return View(taiKhoan);
        }

        // GET: QTV/TaiKhoans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: QTV/TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
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
