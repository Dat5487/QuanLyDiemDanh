using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using APIVanTay.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIVanTay.Controllers
{

    public class diemdanhsController : ControllerBase
    {
        private readonly DataContextDB db;

        public diemdanhsController(DataContextDB context)
        {
            db = context;
        }
        //GV tạo hoạt động điểm danh trong n phút
        //Thêm mới ttdd cho tất cả sv trong lớp tc
        [HttpPost("TaoHdDD")]
        public ActionResult TaoHdDD(string maloptc)
        {
            var now = DateTime.Now.Date;
            if(db.LopTCs.Find(maloptc).trangthai == false)
                return BadRequest("Lớp này đang ở trạng thái không hoạt động");

            if (db.diemdanhs.Where(x =>  x.maloptc == maloptc && x.ngaydd == now ).FirstOrDefault() != null)
                return BadRequest("Lớp này đã tạo hoạt động điểm danh trong hôm nay");

            diemdanh dd = new diemdanh();
            dd.CreateDiemdanh(maloptc);
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd; //Lấy madd vừa tạo

            List<string> ds_masv = db.LopTC_SV.Where(i => i.maloptc == maloptc).Select(x => x.masv).ToList();
            foreach (string ma1sv in ds_masv)
            {
                if (db.Sinhviens.Find(ma1sv) != null)
                {
                    var sv = db.Sinhviens.Find(ma1sv);
                    //Gán giá trị
                    chitietdd ttdd = new chitietdd();
                    ttdd.CreateChitietdd(madd, ma1sv);
                }
            }

            return Ok();
        }
        [HttpPost("HuyHdDD")]

        public ActionResult HuyHdDD(string maloptc)
        {
            var now = DateTime.Now.Date;
            if(db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault() == null)
            {
                return BadRequest("Lớp này chưa tạo điểm danh");
            }
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd; //Lấy madd vừa tạo
            diemdanh dd = new diemdanh();
            dd.DeleteHDDiemdanh(maloptc);
            chitietdd ttdd = new chitietdd();
            ttdd.DeleteHDChitietDD(madd);
            return Ok();
        }

        [HttpPost("KetthucHdDD")]
        public ActionResult KetthucHdDD(string maloptc)
        {
            var now = DateTime.Now.Date;
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd; //Lấy madd vừa tạo
            diemdanh dd = new diemdanh();
            dd.KetthucHdDD(maloptc);
            return Ok();
        }
        //Nếu nhận diện sinh viên thành công thì sẽ cập nhật trạng thái diểm danh thành true
        [HttpPost("PutTrangThaiDD")]
        public ActionResult PutTrangThaiDD(string masv, string maloptc)
        {
            int madd;
            var now = DateTime.Now.Date;
            if (db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault() == null)
                return BadRequest("Lớp chưa tạo hoạt động điểm danh");
            else
                madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            chitietdd ttdd= new chitietdd();
            ttdd.EditChitietdd(madd, masv);
            return Ok();
        }

        //[Route("GetDsNgayDD")]
        //public IHttpActionResult GetDsNgayDD(int maloptc)
        //{
        //    List<DateTime> dsngay = new List<DateTime>();
        //    var model = db.diemdanhs.Where(x => x.maloptc == maloptc);

        //    foreach(var date in model)
        //    {
        //        dsngay.Add(date.ngaydd);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    return Ok(dsngay);
        //}
        [HttpGet("GetStatus")]

        public ActionResult GetDDStatus(string maloptc)
        {
            var now = DateTime.Now.Date;
            if (db.diemdanhs.FirstOrDefault(x => x.maloptc == maloptc && x.ngaydd == now) != null)
            {
                if (db.diemdanhs.FirstOrDefault(x => x.maloptc == maloptc && x.ngaydd == now).trangthaidd == true)
                    return Ok(true);
                else
                    return Ok(false);
            }
            else
                return Ok(false);
        }

    }
}