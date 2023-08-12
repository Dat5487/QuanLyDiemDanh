using QLDD_MVC.Code;
using QLDD_MVC.Common;
using QLDD_MVC.Dao;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace QLDD_MVC.Controllers
{
    public class LoginController : Controller
    {
        public string magv = "0";
        public string hotengv="a";
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
                var result = dao.Login(model.username, Encryptor.MD5Hash(model.password));
                TaiKhoan tk = db.TaiKhoans.Find(model.username);

                if (result)
                {
                    string phanquyen = tk.phanquyen;
                    string username = tk.username;
                    FormsAuthentication.SetAuthCookie(tk.username, false);

                    if (phanquyen.TrimEnd().Equals("Quản trị viên"))
                        return RedirectToAction("Index", "Home", new { area = "QTV" });
                    if (db.giangviens.Where(x => x.username == username).FirstOrDefault() == null)
                        return RedirectToAction("Index", "ErrorM", new { error = "Tên đăng nhập này chưa được gán cho giảng viên nào" });

                    magv = db.giangviens.Where(x => x.username == username).FirstOrDefault().magv;
                    TempData["magv"] = magv;

                    hotengv = db.giangviens.Find(magv).hoten;
                    TempData["hotengv"] = hotengv;

                    if (phanquyen.TrimEnd() == "Cán bộ quản lý đào tạo")
                        return RedirectToAction("Index", "Home", new { area = "CBDT" });
                    if (db.giangviens.Where(x => x.username == username).FirstOrDefault() == null)
                        return RedirectToAction("Index", "ErrorM", new { error = "Giảng viên không có trong danh sách giảng viên, hãy liên hệ với CBQLĐT để thêm giảng viên" });

                    return RedirectToAction("Index", "Home", new { area = "GV" });
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
            }
            return View("Index");
        }
        public void SetHotengv()
        {
            string hotengv = "";
            if (TempData["hotengv"] != null)
                hotengv = TempData["hotengv"] as string;

            TempData.Keep("hotengv");
            ViewBag.hotengv = hotengv;
        }
    }
}