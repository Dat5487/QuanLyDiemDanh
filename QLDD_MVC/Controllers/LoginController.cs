using QLDD_MVC.Common;
using QLDD_MVC.Dao;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLDD_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: QTV/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(TaiKhoan model)
        {
            DataContextDB db = new DataContextDB();
            if (ModelState.IsValid)
            {
                var dao = new LoginDao();
                if (db.TaiKhoans.Find(model.username) == null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập không đúng.");
                }
                var result = dao.Login(model.username, model.password);
                TaiKhoan tk = db.TaiKhoans.Find(model.username);

                if (result)
                {
                    string phanquyen = tk.phanquyen;
                    string username = tk.username;
                    if (phanquyen.TrimEnd().Equals("Quản trị viên"))
                        return RedirectToAction("Index", "Home", new { area = "QTV" });
                    if (db.giangviens.Where(x => x.username == username).FirstOrDefault() == null)
                        return RedirectToAction("Index", "ErrorM", new { error = "Tên đăng nhập này chưa được gán cho giảng viên nào" });

                    magv = db.giangviens.Where(x => x.username == username).FirstOrDefault().magv;
                    if (phanquyen.TrimEnd() == "Cán bộ đào tạo")
                        return RedirectToAction("Index", "Home", new { area = "CBDT" });
                    if (db.giangviens.Where(x => x.username == username).FirstOrDefault() == null)
                        return RedirectToAction("Index", "ErrorM", new { error = "Giảng viên không có trong danh sách giảng viên, hãy liên hệ với CBĐT để thêm giảng viên" });

                    return RedirectToAction("Index", "Home", new { area = "GV" });
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng.");
                }
            }
            return View("Index");
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        public static int magv = 0;
        public int Getmagv()
        {
            return magv;
        }
    }
}