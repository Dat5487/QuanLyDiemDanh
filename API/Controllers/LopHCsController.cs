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
using QLDD_MVC.Areas.QTV.Controllers;
using QLDD_MVC.Models;

namespace API.Controllers
{
    public class LopHCsController : ApiController
    {
        private DataContextDB db = new DataContextDB();

        //[ResponseType(typeof(LopHC))]
        ////Lấy danh sách lớp HC của gv đăng nhập
        //public IHttpActionResult GetLopHC()
        //{
        //    //Lấy magv từ LoginController
        //    LoginController lg = new LoginController();
        //    int magv = lg.Getmagv();
        //    if (db.LopHCs.Where(x => x.magv == magv).FirstOrDefault() == null)
        //        return BadRequest("Bạn không chủ nhiệm lớp hành chính nào");
        //    IQueryable<LopHC> model = db.LopHCs.Where(x => x.magv == magv);

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

        private bool LopHCExists(int id)
        {
            return db.LopHCs.Count(e => e.malophc == id) > 0;
        }
    }
}