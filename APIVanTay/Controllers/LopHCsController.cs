using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using APIVanTay.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIVanTay.Controllers
{

    public class LopHCsController : ControllerBase
    {
        private readonly DataContextDB db;

        public LopHCsController(DataContextDB context)
        {
            db = context;
        }

        [HttpGet("GetAllLopHC")]
        public ActionResult GetAllLopHC()
        {
            var lopHCs = db.LopHCs.ToList();
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

    }
}