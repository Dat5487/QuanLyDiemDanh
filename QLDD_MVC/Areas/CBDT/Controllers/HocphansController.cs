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
    public class HocphansController : Controller
    {
        private DataContextDB db = new DataContextDB();

        public HocphansController()
        {
            LoginController lg = new LoginController();
            ViewBag.hotengv = lg.Gethotengv();
        }

        // GET: CBDT/Hocphans
        public ActionResult Index()
        {
            return View(db.Hocphans.ToList());
        }

        // GET: CBDT/Hocphans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hocphan hocphan = db.Hocphans.Find(id);
            if (hocphan == null)
            {
                return HttpNotFound();
            }
            return View(hocphan);
        }

        // GET: CBDT/Hocphans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CBDT/Hocphans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mahp,tenhp,sotc")] Hocphan hocphan)
        {
            if (ModelState.IsValid)
            {
                db.Hocphans.Add(hocphan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hocphan);
        }

        // GET: CBDT/Hocphans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hocphan hocphan = db.Hocphans.Find(id);
            if (hocphan == null)
            {
                return HttpNotFound();
            }
            return View(hocphan);
        }

        // POST: CBDT/Hocphans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mahp,tenhp,sotc")] Hocphan hocphan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocphan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hocphan);
        }

        // GET: CBDT/Hocphans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hocphan hocphan = db.Hocphans.Find(id);
            if (hocphan == null)
            {
                return HttpNotFound();
            }
            return View(hocphan);
        }

        // POST: CBDT/Hocphans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Hocphan hocphan = db.Hocphans.Find(id);
            db.Hocphans.Remove(hocphan);
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
