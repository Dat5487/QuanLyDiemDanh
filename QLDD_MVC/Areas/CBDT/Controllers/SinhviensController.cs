using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using QLDD_MVC.Areas.QTV.Data;
using QLDD_MVC.Models;
using QLDD_MVC.Areas.CBDT.Data;
using System.Drawing.Printing;
using System.Web.UI;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Ajax.Utilities;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    public class SinhviensController : Controller
    {
        private DataContextDB db = new DataContextDB();

        // GET: CBDT/Sinhviens
        public ActionResult DsAllSinhVien()
        {
            string idtype = "None";
            var dao = new Data.ListAllPaging();
            IEnumerable<Sinhvien> model = dao.ListAllSinhvienPaging(0, idtype);
            return View(model);
        }
        public ActionResult Index(int? id,string root)
        {
            var dao = new Data.ListAllPaging();
            string idtype = "LopHC";
            IEnumerable<Sinhvien> model = dao.ListAllSinhvienPaging(id,idtype);
            var sinhvien = db.Sinhviens.Where(i => i.malophc == id);

            if (sinhvien == null)
                return RedirectToAction("Error",new {error = "Lỗi"});

            //Hien thi ten giang vien
            var lop = db.LopHCs.Find(id);
            if (lop != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenlophc"] = db.LopHCs.Find(id).tenlophc;
            ViewData["malophc"] = id;
            ViewData["root"] = root;

            return View(model);
        }

        public ActionResult Index_LopTC(int? id,string root)
        {
            var dao = new Data.ListAllPaging();
            IEnumerable<Sinhvien> model = dao.ListAllSinhvienPaging(id, "LopTC");
            var sinhvien = db.Sinhviens.Where(i => i.malophc == id);

            if (sinhvien == null)
                return RedirectToAction("Error", new { error = "Lỗi" });

            var lop = db.LopTCs.Find(id);
            if (db.giangviens.Where(c => c.magv == lop.magv).FirstOrDefault() != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";

            ViewData["telntc"] = lop.tenltc;
            ViewData["maloptc"] = id;
            ViewData["root"] = root;

            return View(model);
        }

        public ActionResult Add_SVtoLopTC(int? id, string tentc)
        {
            ViewData["tentc"] = tentc;
            ViewData["maloptc"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_SVtoLopTC([Bind(Include = "maloptc,masv")] LopTC_SV loptc_sv)
        {
            if (ModelState.IsValid)
            {
                if (db.Sinhviens.Find(loptc_sv.masv)!=null)
                {
                    if (db.LopTC_SV.Where(x => x.maloptc == loptc_sv.maloptc && x.masv == loptc_sv.masv).FirstOrDefault() != null)
                    {
                        return RedirectToAction("Index", "Error", new { error = "Sinh viên đã tồn tại trong lớp tín chỉ" });
                    }
                    //Nếu lớp tín chỉ đã đặt hoạt động điểm danh
                    if(db.diemdanhs.Where(x => x.maloptc == loptc_sv.maloptc).FirstOrDefault() != null)
                    {
                        chitietdd ctdd = new chitietdd();
                        int madd = db.diemdanhs.Where(x => x.maloptc == loptc_sv.maloptc).FirstOrDefault().madd;
                        ctdd.CreateChitietdd(madd, loptc_sv.masv);
                    }    
                    db.LopTC_SV.Add(loptc_sv);
                    db.SaveChanges();
                    return RedirectToAction("Index_LopTC", new { id = loptc_sv.maloptc});
                }

                else
                {
                    return RedirectToAction("Index", "Error", new { error = "Sinh viên không tồn tại trong hệ thống" });
                }
            }

            return View(loptc_sv);
        }

        public ActionResult DeleteSVFromLopTC(int? masv, int maloptc)
        {
            if (masv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LopTC_SV sv_loptc = db.LopTC_SV.Where(i => i.masv == masv).FirstOrDefault();

            if (sv_loptc == null)
            {
                return HttpNotFound();
            }
            ViewData["hoten"] = db.Sinhviens.Find(masv).hoten;
            ViewData["maloptc"] = maloptc;

            return View(sv_loptc);
        }

        // POST: CBDT/Sinhviens/Delete/5
        [HttpPost, ActionName("DeleteSVFromLopTC")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSVFromLopTCConfirmed(int masv,int maloptc)
        {
            LopTC_SV sv_loptc = db.LopTC_SV.Where(i => i.masv == masv && i.maloptc == maloptc).FirstOrDefault();
            db.LopTC_SV.Remove(sv_loptc);
            db.SaveChanges();
            return RedirectToAction("Index_LopTC", new { id = sv_loptc.maloptc, idtype = "LopTC"});


        }

        // GET: CBDT/Sinhviens/Create
        public ActionResult Create(int? id, string tenlophc)
        {
            ViewData["tenlophc"] = tenlophc;
            ViewData["malophc"] = id;
            return View();
        }

        // POST: CBDT/Sinhviens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "masv,hoten,gioitinh,malophc,khoa")] Sinhvien sinhvien)
        {
            if (ModelState.IsValid)
            {
                if(db.LopHCs.Find(sinhvien.malophc) == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Mã lớp hành chính không tồn tại" });
                }
                db.Sinhviens.Add(sinhvien);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = sinhvien.malophc});
            }

            return View(sinhvien);
        }

        // GET: CBDT/Sinhviens/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewData["hoten"]= db.Sinhviens.Find(id).hoten;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            return View(sinhvien);
        }

        // POST: CBDT/Sinhviens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "masv,hoten,gioitinh,malophc,khoa")] Sinhvien sinhvien)
        {
            var malop = sinhvien.malophc;
            if (ModelState.IsValid)
            {
                if (db.LopHCs.Find(malop) == null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Lớp hành chính không tồn tại" });
                }
                db.Entry(sinhvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "LopHCs", new { id = malop});
            }
            if (malop == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "LopHCs", new { id = malop });
        }

        // GET: CBDT/Sinhviens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            return View(sinhvien);
        }

        // POST: CBDT/Sinhviens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            db.Sinhviens.Remove(sinhvien);
            db.SaveChanges();
            if(db.LopHCs.Find(sinhvien.malophc) != null)
            {
                string name = db.LopHCs.Find(sinhvien.malophc).tenlophc;
                return RedirectToAction("Index", new { id = sinhvien.malophc, name = name });
            }
            else
                return RedirectToAction("DsAllSinhVien");

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
