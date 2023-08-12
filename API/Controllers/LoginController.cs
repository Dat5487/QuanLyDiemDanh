using QLDD_MVC.Dao;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;
using QLDD_MVC.Code;

namespace API.Controllers
{
    //public class LoginModel
    //{
    //    public string username { get; set; }
    //    public string password { get; set; }

    //}
    public class LoginController : ApiController
    {
        [Route("Login")]
        public IHttpActionResult GetLogin(string username,string password)
        {
            DataContextDB db = new DataContextDB();
            if (ModelState.IsValid)
            {
                var dao = new LoginDao();
                if (db.TaiKhoans.Find(username) == null)
                    return BadRequest("Không có tên đăng nhập trong hệ thống");

                var result = dao.Login(username, Encryptor.MD5Hash(password));
                TaiKhoan tk = db.TaiKhoans.Find(username);
                if (result)
                {
                    string phanquyen = tk.phanquyen;
                    if (phanquyen.TrimEnd().Equals("Quản trị viên"))
                        return BadRequest("Không thể đăng nhập bằng tài khoản QTV");

                    magv = db.giangviens.Where(x => x.username == username).FirstOrDefault().magv;
                    if (db.giangviens.Where(x => x.username == username).FirstOrDefault() == null)
                        return BadRequest("Giảng viên có tên đăng nhập này không có trong danh sách giảng viên, hãy liên hệ với CBĐT để thêm giảng viên");

                    IQueryable<ApiLopTC> model = null;
                    var listtemploptc = new List<ApiLopTC>();
                    List<string> ds_maloptc = null;
                    ds_maloptc = db.GVTCs.Where(x => x.magv == magv).Select(x => x.maloptc).ToList();
                    foreach (string ma1loptc in ds_maloptc)
                    {
                        var temp = db.LopTCs.Find(ma1loptc);
                        ApiLopTC loptc = new ApiLopTC();
                        loptc.maloptc = temp.maloptc;
                        loptc.magv = temp.magv;
                        loptc.mahp = temp.mahp;
                        loptc.tenltc = temp.tenltc;
                        loptc.trangthai = temp.trangthai;
                        loptc.tenhp = db.Hocphans.Find(temp.mahp).tenhp;
                        listtemploptc.Add(loptc);
                    }
                    model = listtemploptc.AsQueryable();
                    return Ok(model);
                }

            }
            return BadRequest("Đăng nhập không đúng");
        }

        public static string magv = "0";
        public string Getmagv()
        {
            return magv;
        }
    }
}
