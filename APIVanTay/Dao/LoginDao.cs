using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIVanTay.Models;
using Microsoft.AspNetCore.Mvc;
using PagedList;

namespace APIVanTay.Dao
{
    public class LoginDao : ControllerBase
    {
        private readonly DataContextDB db;
        public LoginDao()
        {

        }
      
        public LoginDao(DataContextDB context)
        {
            db = context;
        }
        public string Insert(TaiKhoan entity)
        {
            db.TaiKhoans.Add(entity);
            db.SaveChanges();
            return entity.username;
        }

        public IEnumerable<TaiKhoan> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<TaiKhoan> model = db.TaiKhoans;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.username.Contains(searchString));
            }

            return model.OrderByDescending(x => x.username).ToPagedList(page, pageSize);
        }

        public TaiKhoan GetById(string userName)
        {
            return db.TaiKhoans.SingleOrDefault(x => x.username == userName);
        }

        public bool Login(string userName, string passWord)
        {
            if(db.TaiKhoans.Count(x => x.username == userName && x.password == passWord) > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
