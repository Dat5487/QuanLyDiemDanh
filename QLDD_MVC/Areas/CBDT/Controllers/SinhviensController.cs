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
using QLDD_MVC.Controllers;

namespace QLDD_MVC.Areas.CBDT.Controllers
{
    [Authorize]

    public class SinhviensController : BaseController
    {
        private DataContextDB db = new DataContextDB();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DataContextDB"].ConnectionString);
        OleDbConnection Econ;

        public ActionResult DsAllSinhVien()
        {
            string idtype = "None";
            var dao = new Data.ListAllPaging();
            IEnumerable<Sinhvien> model = dao.ListAllSinhvienPaging("0", idtype);
            SetHotengv();
            return View(model);
        }
        public ActionResult Index(string malophc,string root)
        {
            var dao = new Data.ListAllPaging();
            string idtype = "LopHC";
            IEnumerable<Sinhvien> model = dao.ListAllSinhvienPaging(malophc, idtype);
            var sinhvien = db.Sinhviens.Where(i => i.malophc == malophc);

            if (sinhvien == null)
                return RedirectToAction("Error",new {error = "Lỗi"});

            //Hien thi ten giang vien
            var lop = db.LopHCs.Find(malophc);
            if (lop != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenlophc"] = db.LopHCs.Find(malophc).tenlophc;
            ViewData["malophc"] = malophc;
            ViewData["root"] = root;
            SetHotengv();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string malophc)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/Dao/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/Dao/"), filename));
            if(InsertExceldataToLopHC(filepath, filename, malophc)==1)
            {
                return RedirectToAction("DuplicateErrorLopHC", "Error", new { error = "Danh sách sinh viên trong File Excel có sinh viên đã tồn tại trong hệ thống", malophc = malophc });
            }
            else if(InsertExceldataToLopHC(filepath, filename, malophc) == 2)
            {
                return RedirectToAction("Index", "Error", new { error = "Bảng trong File Excel không đúng quy định" });
            }
            SetAlert("Nhập danh sách sinh viên thành công", "success");
            return RedirectToAction("Index", new { malophc = malophc, root= "DsLopHC" });
        }

        public ActionResult Index_LopTC(string maloptc, string root)
        {
            var dao = new Data.ListAllPaging();
            IEnumerable<Sinhvien> model = dao.ListAllSinhvienPaging(maloptc, "LopTC");

            var lop = db.LopTCs.Find(maloptc);
            if (db.giangviens.Where(c => c.magv == lop.magv).FirstOrDefault() != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";

            ViewData["tentc"] = lop.tenltc;
            ViewData["maloptc"] = maloptc;
            ViewData["root"] = root;
            SetHotengv();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index_LopTC(HttpPostedFileBase file, string maloptc)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/Dao/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/Dao/"), filename));
            int kq = InsertExceldataToLopTC(filepath, filename, maloptc);
            if (kq == 1)
            {
                return RedirectToAction("DuplicateErrorLopTC", "Error", new { error = "Danh sách sinh viên trong File Excel có sinh viên đã tồn tại trong lớp tín chỉ này", maloptc = maloptc });
            }
            else if (kq == 2)
            {
                return RedirectToAction("Index", "Error", new { error = "Bảng trong File Excel không đúng quy định" });
            }
            var dsmasv = db.LopTC_SV.Where(x => x.maloptc == maloptc).Select(x => x.masv);
            //Kiểm tra nếu excel lỡ Insert sv không có trong hệ thống thì xóa sv đi
            LopTC_SV sv = new LopTC_SV();
            foreach (string masv in dsmasv)
            {
                if (db.Sinhviens.Find(masv) == null)
                    sv.DeleteLopTC_SV(maloptc, masv);
            }
            SetAlert("Nhập danh sách sinh viên thành công", "success");
            return RedirectToAction("Index_LopTC", new { maloptc = maloptc, root = "DsLopTC" });
        }

        public ActionResult Add_SVtoLopTC(string maloptc, string tentc)
        {
            ViewBag.SV = db.Sinhviens.ToList();
            ViewData["tentc"] = tentc;
            ViewData["maloptc"] = maloptc;
            SetHotengv();
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
                    SetAlert("Thêm sinh viên thành công", "success");
                    return RedirectToAction("Index_LopTC", new { id = loptc_sv.maloptc});
                }

                else
                {
                    return RedirectToAction("Index", "Error", new { error = "Sinh viên không tồn tại trong hệ thống" });
                }
            }
            SetHotengv();
            return View(loptc_sv);
        }

        public ActionResult DeleteSVFromLopTC(string masv, string maloptc)
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
            SetHotengv();
            return View(sv_loptc);
        }

        [HttpPost, ActionName("DeleteSVFromLopTC")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSVFromLopTCConfirmed(string masv,string maloptc)
        {
            LopTC_SV sv_loptc = db.LopTC_SV.Where(i => i.masv.Equals(masv)  && i.maloptc == maloptc).FirstOrDefault();
            db.LopTC_SV.Remove(sv_loptc);
            db.SaveChanges();
            return RedirectToAction("Index_LopTC", new { id = sv_loptc.maloptc, idtype = "LopTC"});
        }

        public ActionResult Create(string malophc, string tenlophc)
        {
            ViewData["tenlophc"] = tenlophc;
            ViewData["malophc"] = malophc;
            ViewBag.lopHC = db.LopHCs.ToList();
            SetHotengv();
            return View();
        }

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
                SetAlert("Thêm sinh viên thành công", "success");
                return RedirectToAction("Index", new { malophc = sinhvien.malophc});
            }
            SetHotengv();
            return View(sinhvien);
        }

        public ActionResult Edit(string masv)
        {
            ViewData["hoten"]= db.Sinhviens.Find(masv).hoten;
            ViewBag.lopHC = db.LopHCs.ToList();

            if (masv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(masv);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(sinhvien);
        }

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
                SetAlert("Chỉnh sửa thông tin sinh viên thành công", "success");
                return RedirectToAction("Details", "LopHCs", new { malophc = malop});
            }
            if (malop == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "LopHCs", new { malophc = malop });
        }

        public ActionResult Delete(string masv)
        {
            if (masv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(masv);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            SetHotengv();
            return View(sinhvien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string masv)
        {
            Sinhvien sinhvien = db.Sinhviens.Find(masv);
            db.Sinhviens.Remove(sinhvien);
            SetAlert("Thêm sinh viên thành công", "success");
            db.SaveChanges();
            if(db.LopHCs.Find(sinhvien.malophc) != null)
            {
                string name = db.LopHCs.Find(sinhvien.malophc).tenlophc;
                return RedirectToAction("Index", new { malophc = sinhvien.malophc, name = name });
            }
            else
                return RedirectToAction("DsAllSinhVien");

        }
        public ActionResult DSSVofLopHC(string malophc,string root)
        {
            //Lấy danh sách
            IEnumerable<Sinhvien> model = db.Sinhviens.Where(i => i.malophc == malophc);
            var sinhvien = db.Sinhviens.Where(i => i.malophc == malophc);

            if (sinhvien == null)
            {
                return RedirectToAction("Error", new { error = "Sinh viên không tồn tại trong hệ thống" });
            }

            //Hiển thị/ Kiểm tra
            var lop = db.LopHCs.Find(malophc);
            if (lop != null)
            {
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            }
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenlophc"] = db.LopHCs.Find(malophc).tenlophc;
            ViewData["malophc"] = malophc;
            ViewData["root"] = root;
            SetHotengv();
            return View(model);
        }

        public ActionResult DSSVofLopTC(string maloptc, string root)
        {
            //Lấy danh sách
            IQueryable<Sinhvien> dssv = null; ;
            var listtempsv = new List<Sinhvien>();
            List<string> ds_masv = null;
            ds_masv = db.LopTC_SV.Where(i => i.maloptc == maloptc).Select(x => x.masv).ToList();

            foreach (string ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                    listtempsv.Add(db.Sinhviens.Find(ma1sv));
            }
            IEnumerable<Sinhvien> model = listtempsv.AsQueryable();
            //Kiểm tra
            var sinhvien = db.Sinhviens.Where(i => i.malophc == maloptc);
            if (sinhvien == null)
                return RedirectToAction("NoResult");

            var lop = db.LopTCs.Find(maloptc);
            if (db.giangviens.Where(c => c.magv == lop.magv).FirstOrDefault() != null)
                ViewData["tengv"] = db.giangviens.Find(lop.magv).hoten;
            else
                ViewData["tengv"] = "Not found";

            ViewData["tenltc"] = lop.tenltc;
            ViewData["maloptc"] = maloptc;
            ViewData["root"] = root;
            SetHotengv();
            return View(model);
        }
        private int InsertExceldataToLopHC(string filepath, string filename, string malophc)
        {
            string path = string.Concat(Server.MapPath("/Dao/") + filename);
            string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            using (OleDbConnection con = new OleDbConnection(excelCS))
            {
                OleDbCommand cmd = new OleDbCommand("select *,'" + malophc + "' as [malophc]" + " from [Sheet1$]", con);
                con.Open();
                DbDataReader dr = cmd.ExecuteReader();
                string CS = ConfigurationManager.ConnectionStrings["DataContextDB"].ConnectionString;
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
                    InsertExceTemp(filepath, filename);
                    return 1;
                }
                catch(InvalidOperationException)
                {
                    return 2;
                }
            }
            return 0;
        }
        private int InsertExceldataToLopTC(string filepath, string filename, string maloptc)
        {
            int slbefore = db.LopTC_SV.Where(x => x.maloptc == maloptc).Count();
            string path = string.Concat(Server.MapPath("/Dao/") + filename);
            // Connection String to Excel Workbook  
            string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            using (OleDbConnection con = new OleDbConnection(excelCS))
            {
                OleDbCommand cmd = new OleDbCommand("select *,'" + maloptc + "' as [maloptc]" + " from [Sheet1$]", con);
                con.Open();
                DbDataReader dr = cmd.ExecuteReader();
                string CS = ConfigurationManager.ConnectionStrings["DataContextDB"].ConnectionString;
                // Bulk Copy to SQL Server   
                SqlBulkCopy bulkInsert = new SqlBulkCopy(CS);
                try
                {
                    bulkInsert.DestinationTableName = "LopTC_SV";
                    bulkInsert.ColumnMappings.Add("Mã sinh viên", "masv");
                    bulkInsert.ColumnMappings.Add("maloptc", "maloptc");
                    bulkInsert.WriteToServer(dr);
                    con.Close();
                }
                catch (SqlException)
                {
                    if (db.LopTC_SV.Where(x => x.maloptc == maloptc).Count() == slbefore)
                    {
                        InsertExceTemp(filepath, filename);
                        return 1;
                    }
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

        private int InsertExceTemp(string filepath, string filename)
        {
            string path = string.Concat(Server.MapPath("/Dao/") + filename);
            // Connection String to Excel Workbook  
            string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            using (OleDbConnection con = new OleDbConnection(excelCS))
            {
                OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", con);
                con.Open();
                DbDataReader dr = cmd.ExecuteReader();
                string CS = ConfigurationManager.ConnectionStrings["DataContextDB"].ConnectionString;
                // Bulk Copy to SQL Server   
                SqlBulkCopy bulkInsert = new SqlBulkCopy(CS);
                try
                {
                    bulkInsert.DestinationTableName = "TempSV";
                    bulkInsert.ColumnMappings.Add("Mã sinh viên", "masv");
                    bulkInsert.WriteToServer(dr);
                }
                catch (InvalidOperationException){}
            }
            return 0;
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
