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
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Data.Common;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    public class SinhviensController : Controller
    {
        private DataContextDB db = new DataContextDB();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DataContextDB"].ConnectionString);
        OleDbConnection Econ;

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

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int malophc)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/Dao/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/Dao/"), filename));
            if(InsertExceldataToLopHC(filepath, filename, malophc)==1)
            {
                return RedirectToAction("Index", "Error", new { error = "Danh sách sinh viên trong File Excel có sinh viên đã tồn tại trong hệ thống" });
            }
            else if(InsertExceldataToLopHC(filepath, filename, malophc) == 2)
            {
                return RedirectToAction("Index", "Error", new { error = "Bảng trong File Excel không đúng quy định" });
            }
            return RedirectToAction("Index", new { id = malophc, root= "DsLopHC" });
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

            ViewData["tentc"] = lop.tenltc;
            ViewData["maloptc"] = id;
            ViewData["root"] = root;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index_LopTC(HttpPostedFileBase file, int maloptc)

        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/Dao/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/Dao/"), filename));

            if (InsertExceldataToLopTC(filepath, filename, maloptc) == 1)
            {
                return RedirectToAction("Index", "Error", new { error = "Danh sách sinh viên trong File Excel có sinh viên đã tồn tại trong lớp tín chỉ này" });
            }
            else if (InsertExceldataToLopTC(filepath, filename, maloptc) == 2)
            {
                return RedirectToAction("Index", "Error", new { error = "Bảng trong File Excel không đúng quy định" });
            }
            return RedirectToAction("Index_LopTC", new { id = maloptc, root= "DsLopTC" });
        }

        public ActionResult Add_SVtoLopTC(int? id, string tentc)
        {
            ViewData["tentc"] = tentc;
            ViewData["maloptc"] = id;
            return View();
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_SVtoLopTC([Bind(Include = "maloptc,masv")] LopTC_SV loptc_sv)
        {
            if (ModelState.IsValid)
            {
                if (db.Sinhviens.Find(loptc_sv.masv)!=null)
                {
                    if (db.LopTC_SV.Where(x => x.maloptc == loptc_sv.maloptc && x.masv.Equals(loptc_sv.masv)).FirstOrDefault() != null)
                    {
                        return RedirectToAction("Index", "Error", new { error = "Sinh viên đã tồn tại trong lớp tín chỉ" });
                    }
                    //if(db.diemdanhs.Where(x => x.maloptc == loptc_sv.maloptc).FirstOrDefault() != null)
                    //{
                    //    chitietdd ctdd = new chitietdd();
                    //    int madd = db.diemdanhs.Where(x => x.maloptc == loptc_sv.maloptc).FirstOrDefault().madd;
                    //    ctdd.CreateChitietdd(madd, loptc_sv.masv);
                    //}    
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

        public ActionResult DeleteSVFromLopTC(string masv, int maloptc)
        {
            if (masv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LopTC_SV sv_loptc = db.LopTC_SV.Where(i => i.masv.Equals(masv)).FirstOrDefault();

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
        public ActionResult DeleteSVFromLopTCConfirmed(string masv,int maloptc)
        {
            LopTC_SV sv_loptc = db.LopTC_SV.Where(i => i.masv.Equals(masv)  && i.maloptc == maloptc).FirstOrDefault();
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
                if(db.Sinhviens.Find(sinhvien.masv) != null)
                {
                    return RedirectToAction("Index", "Error", new { error = "Mã sinh viên đã tồn tại trong hệ thống" });
                }

                if (db.LopHCs.Find(sinhvien.malophc) == null)
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
        public ActionResult Edit(string id)
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
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
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
        public ActionResult DSSVofLopHC(int? id,string root)
        {
            //Lấy danh sách
            IEnumerable<Sinhvien> model = db.Sinhviens.Where(i => i.malophc == id);
            var sinhvien = db.Sinhviens.Where(i => i.malophc == id);

            if (sinhvien == null)
            {
                return RedirectToAction("Error", new { error = "Sinh viên không tồn tại trong hệ thống" });
            }

            //Hiển thị/ Kiểm tra
            var lop = db.LopHCs.Find(id);
            if (lop != null)
            {
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            }
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenlophc"] = db.LopHCs.Find(id).tenlophc;
            ViewData["malophc"] = id;
            ViewData["root"] = root;
            return View(model);
        }

        public ActionResult DSSVofLopTC(int? id,string root)
        {
            //Lấy danh sách
            IQueryable<Sinhvien> dssv = null; ;
            var listtempsv = new List<Sinhvien>();
            List<string> ds_masv = null;
            ds_masv = db.LopTC_SV.Where(i => i.maloptc == id).Select(x => x.masv).ToList();

            foreach (string ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                    listtempsv.Add(db.Sinhviens.Find(ma1sv));
            }
            IEnumerable<Sinhvien> model = listtempsv.AsQueryable();
            //Kiểm tra
            var sinhvien = db.Sinhviens.Where(i => i.malophc == id);
            if (sinhvien == null)
                return RedirectToAction("NoResult");

            var lop = db.LopTCs.Find(id);
            if (db.giangviens.Where(c => c.magv == lop.magv).FirstOrDefault() != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenltc"] = lop.tenltc;
            ViewData["maloptc"] = id;
            ViewData["root"] = root;

            return View(model);
        }
        private int InsertExceldataToLopHC(string filepath, string filename, int malophc)
        {
            string path = string.Concat(Server.MapPath("/Dao/") + filename);
            // Connection String to Excel Workbook  
            string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            using (OleDbConnection con = new OleDbConnection(excelCS))
            {
                OleDbCommand cmd = new OleDbCommand("select *," + malophc + " as [malophc]" + " from [Sheet1$]", con);
                con.Open();
                // Create DbDataReader to Data Worksheet  
                DbDataReader dr = cmd.ExecuteReader();
                // SQL Server Connection String  
                string CS = ConfigurationManager.ConnectionStrings["DataContextDB"].ConnectionString;
                // Bulk Copy to SQL Server   
                SqlBulkCopy bulkInsert = new SqlBulkCopy(CS);
                try
                {
                    bulkInsert.DestinationTableName = "Sinhvien";
                    bulkInsert.ColumnMappings.Add("Mã sinh viên", "masv");
                    bulkInsert.ColumnMappings.Add("Họ tên", "hoten");
                    bulkInsert.ColumnMappings.Add("malophc", "malophc");
                    bulkInsert.ColumnMappings.Add("Giới tính", "gioitinh");
                    bulkInsert.ColumnMappings.Add("Tên khoa", "khoa");
                    bulkInsert.WriteToServer(dr);
                }
                catch (SqlException)
                {
                    return 1;
                }
                catch(InvalidOperationException)
                {
                    return 2;
                }
            }
            return 0;
        }
        private int InsertExceldataToLopTC(string filepath, string filename, int maloptc)
        {
            int slbefore = db.LopTC_SV.Where(x => x.maloptc == maloptc).Count();
            string path = string.Concat(Server.MapPath("/Dao/") + filename);
            // Connection String to Excel Workbook  
            string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            using (OleDbConnection con = new OleDbConnection(excelCS))
            {
                //OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", con);
                OleDbCommand cmd = new OleDbCommand("select *," + maloptc + " as [maloptc]" + " from [Sheet1$]", con);
                con.Open();
                // Create DbDataReader to Data Worksheet  
                DbDataReader dr = cmd.ExecuteReader();
                // SQL Server Connection String  
                string CS = ConfigurationManager.ConnectionStrings["DataContextDB"].ConnectionString;
                // Bulk Copy to SQL Server   
                SqlBulkCopy bulkInsert = new SqlBulkCopy(CS);
                try
                {
                    bulkInsert.DestinationTableName = "LopTC_SV";
                    bulkInsert.ColumnMappings.Add("Mã sinh viên", "masv");
                    bulkInsert.ColumnMappings.Add("maloptc", "maloptc");
                    bulkInsert.WriteToServer(dr);
                }
                catch (SqlException)
                {
                    if (db.LopTC_SV.Where(x => x.maloptc == maloptc).Count() == slbefore)
                        return 1;
                    else
                        return 0;
                }
                catch (InvalidOperationException)
                {
                    return 2;
                }
            }
            return 0;
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
