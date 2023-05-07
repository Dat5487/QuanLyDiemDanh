using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLDD_MVC.Areas.CBDT.Data;
using QLDD_MVC.Models;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    public class giangviensController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: CBDT/giangviens

        public ActionResult Index()
        {
            var dao = new ListAllPaging();
            var model = dao.ListAllGiangVienPaging();
            return View(model);
        }

        // GET: CBDT/giangviens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CBDT/giangviens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "magv,hoten,gioitinh,diachi,email,sdt,username")] giangvien giangvien)
        {
            if (ModelState.IsValid)
            {
                TaiKhoan tk = db.TaiKhoans.Find(giangvien.username);
                if (tk != null)
                {
                    if (tk.phanquyen == "Quản trị viên")
                    {
                        return RedirectToAction("Index", "Error", new { error = "Bạn không thể sử dụng tên đăng nhập của quản trị viên" });
                    }
                    if (db.giangviens.Where(x => x.username == giangvien.username).FirstOrDefault() != null)
                    {
                        return RedirectToAction("Index", "Error", new { error = "Đã giáo viên có tên đăng nhập này trong hệ thống" });
                    }
                    db.giangviens.Add(giangvien);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Index","Error", new { error = "Không có tài khoản trong hệ thống" });
            }

            return View(giangvien);
        }

        // GET: CBDT/giangviens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            giangvien giangvien = db.giangviens.Find(id);
            if (giangvien == null)
            {
                return HttpNotFound();
            }
            return View(giangvien);
        }

        // POST: CBDT/giangviens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "magv,hoten,gioitinh,diachi,email,sdt,username")] giangvien giangvien)
        {
            if (ModelState.IsValid)
            {
                if (db.TaiKhoans.Find(giangvien.username.TrimEnd(' ')) == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Không tìm thấy tên đăng nhập trong hệ thống" });
                }
                TaiKhoan tk = db.TaiKhoans.Find(giangvien.username.TrimEnd(' '));
                if (tk.phanquyen == "Quản trị viên")
                {
                    return RedirectToAction("Index", "Error", new { error = "Bạn không thể sử dụng tên đăng nhập của quản trị viên" });
                }
                //if (db.giangviens.Find(giangvien.magv).username.TrimEnd(' ') == giangvien.username)
                //{
                //    return RedirectToAction("Index", "Error", new { error = "Tên đăng nhập đã được sử dụng bởi giảng viên khác" });
                //}

                db.Entry(giangvien).State = EntityState.Modified;
                db.SaveChanges();

                if(db.giangviens.Find(giangvien.magv) != null)
                    tk.EditTaiKhoan(db.giangviens.Find(giangvien.magv).username,giangvien.hoten);
                return RedirectToAction("Index");
            }
            return View(giangvien);
        }

        // GET: CBDT/giangviens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.LopTCs.Where(x => x.magv == id).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Error", new { error = "Giảng viên này vẫn đang quản lý một lớp tín chỉ" });
            }
            if (db.LopHCs.Where(x => x.magv == id).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Error", new { error = "Giảng viên này vẫn đang quản lý một lớp hành chính" });
            }
            giangvien giangvien = db.giangviens.Find(id);
            if (giangvien == null)
            {
                return HttpNotFound();
            }
            return View(giangvien);
        }

        // POST: CBDT/giangviens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            giangvien giangvien = db.giangviens.Find(id);
            db.giangviens.Remove(giangvien);
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
