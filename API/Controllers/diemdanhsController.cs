using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using QLDD_MVC.Models;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace API.Controllers
{
    public class diemdanhsController : ApiController
    {
        private DataContextDB db = new DataContextDB();
        //GV tạo hoạt động điểm danh trong n phút
        //Thêm mới ttdd cho tất cả sv trong lớp tc
        [Route("TaoHdDD")]
        public IHttpActionResult TaoHdDD(int maloptc, string diadiem)
        {
            var now = DateTime.Now.Date;
            if(db.LopTCs.Find(maloptc).trangthai == false)
                return BadRequest("Lớp này đang ở trạng thái không hoạt động");

            if (db.diemdanhs.Where(x =>  x.maloptc == maloptc && x.ngaydd == now ).FirstOrDefault() != null)
                return BadRequest("Lớp này đã tạo hoạt động điểm danh trong hôm nay");

            diemdanh dd = new diemdanh();
            dd.CreateDiemdanh(maloptc, diadiem);
            int madd = db.diemdanhs.Where(x => x.maloptc == maloptc && x.ngaydd == now).FirstOrDefault().madd; //Lấy madd vừa tạo

            List<int> ds_masv = db.LopTC_SV.Where(i => i.maloptc == maloptc).Select(x => x.masv).ToList();
            foreach (int ma1sv in ds_masv)
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

        //Nếu nhận diện sinh viên thành công thì sẽ cập nhật trạng thái diểm danh thành true
        [Route("PutTrangThaiDD")]
        public IHttpActionResult PutTrangThaiDD(int masv,int maloptc)
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

        [Route("GetDsNgayDD")]
        public IHttpActionResult GetDsNgayDD(int maloptc)
        {
            List<DateTime> dsngay = new List<DateTime>();
            var model = db.diemdanhs.Where(x => x.maloptc == maloptc);

            foreach(var date in model)
            {
                dsngay.Add(date.ngaydd);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(dsngay);
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