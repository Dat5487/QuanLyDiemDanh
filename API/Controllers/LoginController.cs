using QLDD_MVC.Dao;
using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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

                var result = dao.Login(username, password);
                TaiKhoan tk = db.TaiKhoans.Find(username);
                if (result)
                {
                    string phanquyen = tk.phanquyen;
                    if (phanquyen.TrimEnd().Equals("Quản trị viên"))
                        return BadRequest("Không thể đăng nhập bằng tài khoản QTV");

                    magv = db.giangviens.Where(x => x.username == username).FirstOrDefault().magv;
                    if (db.giangviens.Where(x => x.username == username).FirstOrDefault() == null)
                        return BadRequest("Giảng viên có tên đăng nhập này không có trong danh sách giảng viên, hãy liên hệ với CBĐT để thêm giảng viên");

                    IQueryable<LopTC> model = null;
                    var listtemploptc = new List<LopTC>();
                    List<int> ds_maloptc = null;
                    ds_maloptc = db.GVTCs.Where(x => x.magv == magv).Select(x => x.maloptc).ToList();
                    foreach (int ma1loptc in ds_maloptc)
                    {
                        //if (db.Sinhviens.Find(ma1loptc) != null)
                            listtemploptc.Add(db.LopTCs.Find(ma1loptc));
                    }

                    model = listtemploptc.AsQueryable();
                    //if (model == null)
                    //    return BadRequest("Bạn không có lớp tín chỉ nào");

                    return Ok(model);
                }

            }
            return BadRequest("Đăng nhập không đúng");
        }

        public static int magv = 0;
        public int Getmagv()
        {
            return magv;
        }
    }
}
