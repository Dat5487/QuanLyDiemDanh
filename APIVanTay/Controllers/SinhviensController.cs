using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using APIVanTay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace APIVanTay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SinhviensController : ControllerBase
    {
        private readonly DataContextDB db;

        public SinhviensController(DataContextDB context)
        {
            db = context;
        }
        [HttpGet("GetAllSinhviens")]
        public ActionResult GetAllSinhviens()
        {
            var sinhvien = db.Sinhviens.ToList();
            var listtempsv = new List<ApiSinhvien>();
            foreach (Sinhvien x in sinhvien)
            {
                ApiSinhvien sv = new ApiSinhvien();
                sv.hoten = x.hoten;
                //sv.khoa = x.khoa;
                sv.tenlophc = db.LopHCs.FirstOrDefault(a => a.malophc == x.malophc).tenlophc;
                sv.malophc = x.malophc;
                //sv.gioitinh = x.gioitinh;
                sv.masv = x.masv;
                listtempsv.Add(sv);
            }
            IEnumerable<ApiSinhvien> model = listtempsv.AsQueryable();
            return Ok(model);
        }

        [HttpGet("GetSinhviensList")]
        public ActionResult GetSinhviensList(string malophc)
        {
            var sinhvien = db.Sinhviens.Where(x=>x.malophc == malophc).ToList();
            var listtempsv = new List<ApiSinhvien>();
            foreach (Sinhvien x in sinhvien)
            {
                ApiSinhvien sv = new ApiSinhvien();
                sv.hoten = x.hoten;
                //sv.khoa = x.khoa;
                sv.tenlophc = db.LopHCs.FirstOrDefault(a => a.malophc == x.malophc).tenlophc;
                sv.malophc = x.malophc;
                //sv.gioitinh = x.gioitinh;
                sv.masv = x.masv;
                listtempsv.Add(sv);
            }
            IEnumerable<ApiSinhvien> model = listtempsv.AsQueryable();
            return Ok(model);
        }


        //Lấy danh sách sinh viên kết hợp có thông tin điểm danh của bảng của 1 lớp TC
        //Đây là bảng thông tin điểm danh chung gồm ttsv và số buổi mà sv đã điểm danh
        [HttpGet("GetDSSVofLopTC")]
        public ActionResult GetDSSVofLopTC(string maloptc)
        {
            IQueryable<ApiDSSVDiemdanh> dssv = null; ;
            var listtempsv = new List<ApiDSSVDiemdanh>();
            List<int> dsmadd = db.diemdanhs.Where(x => x.maloptc == maloptc).Select(x => x.madd).ToList();
            List<string> ds_masv = db.LopTC_SV.Where(i => i.maloptc == maloptc).Select(x => x.masv).ToList();

            foreach (string ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                {
                    ApiDSSVDiemdanh svdd = new ApiDSSVDiemdanh { };
                    var sv = db.Sinhviens.Find(ma1sv);
                    //Gán giá trị
                    svdd.hoten = sv.hoten;
                    //svdd.gioitinh = sv.gioitinh;
                    int sobuoidd = 0;
                    foreach (int madd in dsmadd)
                    {
                        if (db.chitietdds.Where(x => x.madd == madd && x.masv.Equals(ma1sv)).FirstOrDefault() != null)
                            if (db.chitietdds.Where(x => x.madd == madd && x.masv.Equals(ma1sv)).FirstOrDefault().trangthai == true)
                                sobuoidd++;
                    }
                    svdd.sobuoidd = sobuoidd;
                    listtempsv.Add(svdd);
                }
            }
            IEnumerable<ApiDSSVDiemdanh> model = listtempsv.AsQueryable();

            return Ok(model);
        }

        [HttpGet("GetDSSVofLopHC")]
        public ActionResult GetDSSVofLopHC(string malophc)
        {
            var dssv = db.Sinhviens.Where(x => x.malophc == malophc).ToList();
            var listtempsv = new List<ApiSinhvien>();
            foreach (Sinhvien x in dssv)
            {
                ApiSinhvien sv = new ApiSinhvien();
                sv.hoten = x.hoten;
                //sv.khoa = x.khoa;
                sv.tenlophc = db.LopHCs.FirstOrDefault(a => a.malophc == x.malophc).tenlophc;
                sv.malophc = x.malophc;
                //sv.gioitinh = x.gioitinh;
                sv.masv = x.masv;
                listtempsv.Add(sv);
            }
            IEnumerable<ApiSinhvien> model = listtempsv.AsQueryable();
            return Ok(model);
        }

        [HttpPost("fingerprint")]
        public ActionResult fingerprint()
        {
            var file = Request.Form.Files[0];
            var filePath = "D:/image/" + file.FileName + ".bmpp";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Sinhvien sv = new Sinhvien();
            sv.CreateImage(file.FileName);
            if (file.FileName == "temp")
            {
                var canidate = db.Sinhviens.Where(x => x.MauVanTay != null);
                string masv = sv.NhanDienVanTay(canidate);
                DateTime currentTime = DateTime.Now;
                string now = currentTime.ToString("yyyy-MM-dd");
                now += " 12:00:00 AM";
                currentTime = DateTime.Parse(now);
                if (masv != null)
                {
                    //Danh sách các lớp đã tạo hoạt động DD trong hôm nay
                    var listLop = db.diemdanhs.Where(x => x.ngaydd == currentTime).ToList();
                    if (listLop.Count() == 0)
                    {
                        return Ok("Lop chua tao hoat   dong diem danh");
                    }
                    foreach(diemdanh lop in listLop)
                    {
                        //Kiểm tra từng lớp xem có sinh viên đó không
                        var lopOfSV = db.LopTC_SV.Where(x => x.maloptc == lop.maloptc && x.masv == masv).ToList();
                        if (lopOfSV == null)
                        {
                            break;
                        }
                        if(lopOfSV.Count() > 1)
                        {
                            return Ok("Tim thay sinh vien tai nhieu lop");
                        }
                        chitietdd ctdd = new chitietdd();
                        ctdd.EditChitietdd(lop.madd,masv);
                        return Ok("Sinh vien:" + masv + " Da duoc diem danh");
                    }
                    return Ok("Lop chua tao hoat   dong diem danh hoac khong tim thay SV");
                    //String lastTime = db.diemdanhs.Where(x => x.masv == masv).Select(x => x.thoigiandd).OrderBy(x => x).Last();
                    //DateTime dateTime = DateTime.Parse(lastTime);
                    //string time = dateTime.ToString("dd/MM/yyyy");
                    //if (time != now)
                    //{

                    //}
                }
                return Ok("Khong tim thay SV");
            }
            else
            {
                if (db.Sinhviens.Find(file.FileName) == null)
                    return Ok("Sinh vien:" + file.FileName + " Khong trong he thong");
                sv.UpdateVanTay(file.FileName);
                string response = "Sinh vien:" + file.FileName + " Cap nhat thanh cong";
                return Ok(response);
            }
        }
    }
}