using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using QLDD_MVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers
{
    public class SinhviensController : ApiController
    {
        private DataContextDB db = new DataContextDB();

        [Route("GetAllSinhviens")]
        public IHttpActionResult GetAllSinhviens()
        {
            var sinhvien = db.Sinhviens;
            return Ok(sinhvien);
        }

        //Lấy danh sách sinh viên của 1 lớp HC
        //[Route("GetSinhviensOfLopHC")]
        //public IHttpActionResult GetSinhviensOfLopHC(int malophc)
        //{
        //    var listtempsv = new List<ApiSinhvien>();
        //    var sinhvien = db.Sinhviens.Where(i => i.malophc == malophc);
        //    foreach(var sv in sinhvien)
        //    {
        //        ApiSinhvien ttsv = new ApiSinhvien { };
        //        ttsv.masv = sv.masv;
        //        ttsv.hoten = sv.hoten;
        //        ttsv.gioitinh = sv.gioitinh;
        //        ttsv.tenlophc = db.LopHCs.Find(malophc).tenlophc;
        //        ttsv.khoa = sv.khoa;
        //        listtempsv.Add(ttsv);
        //    }
        //    IEnumerable<ApiSinhvien> model = listtempsv.AsQueryable();

        //    return Ok(model);
        //}

        //Lấy danh sách sinh viên kết hợp có thông tin điểm danh của bảng của 1 lớp TC
        //Đây là bảng thông tin điểm danh chung gồm ttsv và số buổi mà sv đã điểm danh
        [Route("GetDSSVofLopTC")]
        public IHttpActionResult GetDSSVofLopTC(int maloptc)
        {
            IQueryable<ApiDSSVDiemdanh> dssv = null; ;
            var listtempsv = new List<ApiDSSVDiemdanh>();
            List<int> dsmadd = db.diemdanhs.Where(x => x.maloptc == maloptc).Select(x => x.madd).ToList();
            List<int> ds_masv = db.LopTC_SV.Where(i => i.maloptc == maloptc).Select(x => x.masv).ToList();

            foreach (int ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                {
                    ApiDSSVDiemdanh svdd = new ApiDSSVDiemdanh { };
                    var sv = db.Sinhviens.Find(ma1sv);
                    //Gán giá trị
                    svdd.hoten = sv.hoten;
                    svdd.gioitinh = sv.gioitinh;
                    int sobuoidd = 0;
                    foreach (int madd in dsmadd)
                    {
                        if (db.chitietdds.Where(x => x.madd == madd && x.masv == ma1sv).FirstOrDefault() != null)
                            if (db.chitietdds.Where(x => x.madd == madd && x.masv == ma1sv).FirstOrDefault().trangthai == true)
                                sobuoidd++;
                    }
                    svdd.sobuoidd = sobuoidd;
                    listtempsv.Add(svdd);
                }
            }
            IEnumerable<ApiDSSVDiemdanh> model = listtempsv.AsQueryable();

            return Ok(model);
        }

        //DSSV X thongtindd
        //Lấy thông tin điểm danh của 1 lớp trong 1 ngày được chọn
        //[Route("GetdiemdanhByDate")]
        //public IHttpActionResult GetdiemdanhByDate(int maloptc, DateTime date)
        //{
        //    date = date.Date;
        //    var listtempsv = new List<ApiDSSVChitietdd>();
        //    if (db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == date).FirstOrDefault() == null)
        //        return BadRequest("Không có buổi học vào ngày này");
        //    int madd = db.diemdanhs.Where(x=>x.maloptc == maloptc && x.ngaydd == date).FirstOrDefault().madd;
        //    var ttddofdate = db.chitietdds.Where(x => x.madd == madd);
        //    var ds_masv = ttddofdate.Select(x => x.masv).ToList();

        //    foreach (int ma1sv in ds_masv)
        //    {
        //        if (db.Sinhviens.Find(ma1sv) != null)
        //        {
        //            ApiDSSVChitietdd svdd = new ApiDSSVChitietdd { };
        //            var sv = db.Sinhviens.Find(ma1sv);
        //            //Gán giá trị
        //            svdd.hoten = sv.hoten;
        //            svdd.gioitinh = sv.gioitinh;
        //            svdd.thoigiandd = db.chitietdds.Where(x=>x.madd == madd && x.masv == sv.masv).FirstOrDefault().thoigiandd;
        //            //svdd.tenlophc = db.LopHCs.Find(sv.malophc).tenlophc;
        //            svdd.trangthai = ttddofdate.Where(x => x.masv == ma1sv).FirstOrDefault().trangthai;
        //            listtempsv.Add(svdd);
        //        }
        //    }
        //    IEnumerable<ApiDSSVChitietdd> ttdd = listtempsv.AsQueryable();
        //    return Ok(ttdd);
        //}

        [Route("UpdateSinhvien")]

        public IHttpActionResult UpdateSinhvien(UpdateModel updateModel)
        {
            Sinhvien sv = new Sinhvien();
            if(db.Sinhviens.FirstOrDefault(x => x.masv == updateModel.masv) !=null)
            {
                sv = db.Sinhviens.FirstOrDefault(x => x.masv == updateModel.masv);
            }
            else
            {
                return BadRequest("Không tìm thấy sinh viên");
            }

            Sinhvien SV = new Sinhvien();
            SV.EditSinhvien(updateModel.masv, updateModel.EmbFace);
            return Ok("Update thành công");
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