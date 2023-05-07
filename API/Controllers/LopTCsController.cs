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
using QLDD_MVC.Models;

namespace API.Controllers
{
    public class LopTCsController : ApiController
    {
        private DataContextDB db = new DataContextDB();
        //Lấy danh sách lớp TC của gv đăng nhập
        //[ResponseType(typeof(LopTC))]
        //public IHttpActionResult GetLopTC()
        //{
        //    LoginController lg = new LoginController();
        //    int magv = lg.Getmagv();
        //    IQueryable<LopTC> model = null;
        //    var listtemploptc = new List<LopTC>();
        //    List<int> ds_maloptc = null;
        //    ds_maloptc = db.GVTCs.Where(x => x.magv == magv).Select(x => x.maloptc).ToList();

        //    foreach (int ma1loptc in ds_maloptc)
        //    {
        //        if (db.Sinhviens.Find(ma1loptc) != null)
        //            listtemploptc.Add(db.LopTCs.Find(ma1loptc));
        //    }

        //    model = listtemploptc.AsQueryable();
        //    if (model == null)
        //        return BadRequest("Bạn không có lớp tín chỉ nào");

        //    return Ok(model);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LopTCExists(int id)
        {
            return db.LopTCs.Count(e => e.maloptc == id) > 0;
        }
    }
}