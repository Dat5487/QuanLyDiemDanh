using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace QLDD_MVC.Areas.QTV.Data
{
    public class ListAllPaging
    {
        DataContextDB db = null;
        public ListAllPaging()
        {
            db = new DataContextDB();
        }
        public IEnumerable<TaiKhoan> ListAllTaiKhoanPaging()
        {
            IQueryable<TaiKhoan> model = db.TaiKhoans;

            return model;
        }
    }
}