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
using API.Models;

namespace API.Controllers
{
    public class LopHCsController : ApiController
    {
        private DataContextDB db = new DataContextDB();

        [Route("GetAllLopHC")]
        public IHttpActionResult GetAllLopHC()
        {
            var lopHCs = db.LopHCs;
            var listTempLopHC = new List<ApiLopHC>();
            foreach (LopHC lop in lopHCs)
            {
                ApiLopHC lophc = new ApiLopHC();
                //lophc.id = lop.id;
                lophc.malophc = lop.malophc;
                lophc.tenlophc = lop.tenlophc;
                //lophc.magv = lop.magv;
                //lophc.khoa = lop.khoa;
                lophc.sosv = db.Sinhviens.Where(x=> x.malophc == lop.malophc).Count();
                listTempLopHC.Add(lophc);
            }
            IEnumerable<ApiLopHC> model = listTempLopHC.AsQueryable();
            return Ok(model);
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