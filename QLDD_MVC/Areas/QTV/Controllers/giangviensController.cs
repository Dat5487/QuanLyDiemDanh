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

namespace QLDD_MVC.Areas.QTV.Controllers
{
    public class giangviensController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: CBDT/giangviens

        public ActionResult Index()
        {
            var model = db.giangviens;
            return View(model);
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
                db.Entry(giangvien).State = EntityState.Modified;
                db.SaveChanges();
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
