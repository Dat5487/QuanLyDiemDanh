using APIVanTay.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using APIVanTay.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIVanTay.Controllers
{
    public class TaiKhoansController : ControllerBase
    {
        private readonly DataContextDB db;

        public TaiKhoansController(DataContextDB context)
        {
            db = context;
        }

        [HttpGet("Login")]
        public ActionResult GetLogin(string username,string password)
        {
            if (ModelState.IsValid)
            {
                var a = db.TaiKhoans.ToList();
                if (db.TaiKhoans.Find(username) == null)
                    return BadRequest("Không có tên đăng nhập trong hệ thống");

                bool result = false;
                if (db.TaiKhoans.Count(x => x.username == username && x.password == Encryptor.MD5Hash(password)) > 0)
                {
                    result = true;
                }

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
