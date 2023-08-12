using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using QLDD_MVC.Areas.CBDT.Data;
using QLDD_MVC.Controllers;
using QLDD_MVC.Models;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    [Authorize]

    public class giangviensController : BaseController
    {
        private DataContextDB db = new DataContextDB();
        giangvien gv = new giangvien();

        public ActionResult Index()
        {
            var dao = new ListAllPaging();
            var model = dao.ListAllGiangVienPaging();
            SetHotengv();
            return View(model);
        }

        public ActionResult Create()
        {
            SetHotengv();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,magv,hoten,gioitinh,diachi,email,sdt,username,UserPhoto")] giangvien giangvien, HttpPostedFileBase postedFile)
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
                    gv.CreateGV(giangvien.hoten, giangvien.gioitinh, giangvien.diachi, giangvien.email, giangvien.sdt, giangvien.username, Upload(postedFile));
                    SetAlert("Thêm giảng viên thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Index", "Error", new { error = "Không có tài khoản trong hệ thống" });
            }
            SetHotengv();
            return View(giangvien);
        }

        public byte[] Upload(HttpPostedFileBase postedFile)
        {
            byte[] bytes = null;
            if(postedFile != null)
            {
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }
            }
            return bytes;
        }
        public ActionResult Edit(string magv)
        {
            if (magv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            giangvien giangvien = db.giangviens.Find(magv);
            if (giangvien == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(giangvien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "magv,hoten,gioitinh,diachi,email,sdt,username")] giangvien giangvien, HttpPostedFileBase postedFile)
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
                gv.EditGV(giangvien.magv, giangvien.hoten, giangvien.gioitinh, giangvien.diachi, giangvien.email, giangvien.sdt, giangvien.username, Upload(postedFile));
                //Đổi tên gv thì đổi tên cả tài khoản
                if (db.giangviens.Find(giangvien.magv) != null)
                    tk.EditTaiKhoan(db.giangviens.Find(giangvien.magv).username, giangvien.hoten);

                SetAlert("Chỉnh sửa thông tin giảng viên thành công", "success");
                return RedirectToAction("Index");
            }
            SetHotengv();
            return View(giangvien);
        }

        public ActionResult Delete(string magv)
        {
            if (magv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.LopTCs.Where(x => x.magv == magv).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Error", new { error = "Giảng viên này vẫn đang quản lý một lớp tín chỉ" });
            }
            if (db.LopHCs.Where(x => x.magv == magv).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Error", new { error = "Giảng viên này vẫn đang quản lý một lớp hành chính" });
            }
            giangvien giangvien = db.giangviens.Find(magv);
            if (giangvien == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(giangvien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string magv)
        {

            giangvien giangvien = db.giangviens.Find(magv);
            db.giangviens.Remove(giangvien);
            db.SaveChanges();
            SetAlert("Xóa giảng viên thành công", "success");
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
