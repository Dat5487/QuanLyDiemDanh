using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLDD_MVC.Controllers;
using QLDD_MVC.Models;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    [Authorize]

    public class HocphansController : BaseController
    {
        private DataContextDB db = new DataContextDB();

        public ActionResult Index()
        {
            return View(db.Hocphans.ToList());
        }

        public ActionResult Details(string mahp)
        {
            if (mahp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hocphan hocphan = db.Hocphans.Find(mahp);
            if (hocphan == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(hocphan);
        }

        public ActionResult Create()
        {
            SetHotengv();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mahp,tenhp,sotc")] Hocphan hocphan)
        {
            if (ModelState.IsValid)
            {
                db.Hocphans.Add(hocphan);
                db.SaveChanges();
                SetAlert("Thêm học phần thành công", "success");
                return RedirectToAction("Index");
            }
            SetHotengv();
            return View(hocphan);
        }

        public ActionResult Edit(string mahp)
        {
            if (mahp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hocphan hocphan = db.Hocphans.Find(mahp);
            if (hocphan == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(hocphan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mahp,tenhp,sotc")] Hocphan hocphan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocphan).State = EntityState.Modified;
                db.SaveChanges();
                SetAlert("Chỉnh sửa học phần thành công", "success");
                return RedirectToAction("Index");
            }
            SetHotengv();
            return View(hocphan);
        }

        public ActionResult Delete(string mahp)
        {
            if (mahp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hocphan hocphan = db.Hocphans.Find(mahp);
            if (hocphan == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(hocphan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string mahp)
        {
            Hocphan hocphan = db.Hocphans.Find(mahp);
            db.Hocphans.Remove(hocphan);
            db.SaveChanges();
            SetAlert("Xóa học phần thành công", "success");
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
