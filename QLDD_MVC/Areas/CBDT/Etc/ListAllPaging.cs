using QLDD_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data.Entity;
using System.Data;
using System.Dynamic;
using System.Web.Mvc;

namespace QLDD_MVC.Areas.CBDT.Data
{
    public class ListAllPaging
    {
        DataContextDB db = null;
        public ListAllPaging()
        {
            db = new DataContextDB();
        }
        public IEnumerable<Sinhvien> ListAllSinhvienPaging(int? id, string idtype)
        {
            IQueryable<Sinhvien> model = null;

            if (idtype == "LopHC")
            {
                model = db.Sinhviens.Where(i => i.malophc == id);
            }
            else if (idtype == "LopTC")
            {
                var listtempsv = new List<Sinhvien>();
                List<int> ds_masv = null;

                ds_masv = db.LopTC_SV.Where(i => i.maloptc == id).Select(x => x.masv).ToList();

                foreach (int ma1sv in ds_masv)
                {
                    if (db.Sinhviens.Find(ma1sv) != null)
                    {
                        listtempsv.Add(db.Sinhviens.Find(ma1sv));
                    }
                }

                model = listtempsv.AsQueryable();
                if (model == null)
                {
                    return model;
                }
            }
            else if (idtype == "None")
            {
                model = db.Sinhviens;
            }
            return model;
        }

        public IEnumerable<LopHC> ListAllLopHCPaging()
        {
            IQueryable<LopHC> model = db.LopHCs;
            return model;
        }

        public IEnumerable<LopTC> ListAllLopTCPaging()
        {
            IQueryable<LopTC> model = db.LopTCs;
            return model;
        }
        public IEnumerable<giangvien> ListAllGiangVienPaging()
        {
            IQueryable<giangvien> model = db.giangviens;
            return model;
        }

        public IEnumerable<diemdanh> ListAllDiemDanhPaging(int? masv,int maloptc)
        {

            //IQueryable<chitietdd> dsdd = null;
            var listtempdd = new List<diemdanh>();

            List<int> ds_madd = db.chitietdds.Where(c => c.masv == masv).Select(x => x.madd).ToList();

            foreach (int madd in ds_madd)
            {
                if (db.diemdanhs.Where(c => c.madd == madd && c.maloptc == maloptc).FirstOrDefault() != null)
                {
                    //listtempdd.Add(db.diemdanhs.Find(madd));
                    listtempdd.Add(db.diemdanhs.Where(c => c.madd == madd && c.maloptc == maloptc).FirstOrDefault());
                }
            }

            IQueryable<diemdanh>  chitietdd = listtempdd.AsQueryable();

            if (chitietdd == null)
            {
                return chitietdd;
            }

            return chitietdd;

        }

        public IEnumerable<diemdanh> ListAllDiemDanhPaging2(int? masv)
        {
            IQueryable<diemdanh> chitietdd = null;
            List<int> ds_madd = null;

            //IQueryable<chitietdd> dsdd = null;
            var listtempdd = new List<diemdanh>();

            ds_madd = db.chitietdds.Where(c => c.masv == masv).Select(x => x.madd).ToList();

            foreach (int madd in ds_madd)
            {
                if (db.diemdanhs.Find(madd) != null)
                {
                    //listtempdd.Add(db.diemdanhs.Find(madd));
                    listtempdd.Add(db.diemdanhs.Where(c => c.madd == madd).FirstOrDefault());
                }
            }

            chitietdd = listtempdd.AsQueryable();

            if (chitietdd == null)
            {
                return chitietdd;
            }

            return chitietdd;

        }

        public IEnumerable<LopHC> ListAllLopHCofGVPaging(int magv)
        {
            if (db.LopHCs.Where(x => x.magv == magv).FirstOrDefault() == null)
                return null;

            IQueryable<LopHC> model = db.LopHCs.Where(x => x.magv == magv);

            return model;
        }

        public IEnumerable<LopTC> ListAllLopTCofGVPaging(int magv)
        {
            IQueryable<LopTC> model = null;
            var listtemploptc = new List<LopTC>();
            List<int> ds_maloptc = null;

            ds_maloptc = db.GVTCs.Where(x => x.magv == magv).Select(x => x.maloptc).ToList();

            foreach (int ma1loptc in ds_maloptc)
            {
                if (db.LopTCs.Find(ma1loptc) != null)
                    listtemploptc.Add(db.LopTCs.Find(ma1loptc));
            }
            model = listtemploptc.AsQueryable();
            if (model == null)
                return null;


            return model;
        }

    }
}
