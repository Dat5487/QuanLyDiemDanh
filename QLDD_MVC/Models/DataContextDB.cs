using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace QLDD_MVC.Models
{
    public partial class DataContextDB : DbContext
    {
        public DataContextDB()
            : base("name=DataContextDB")
        {
        }

        public virtual DbSet<chitietdd> chitietdds { get; set; }
        public virtual DbSet<diemdanh> diemdanhs { get; set; }
        public virtual DbSet<giangvien> giangviens { get; set; }
        public virtual DbSet<GVCN> GVCNs { get; set; }
        public virtual DbSet<GVTC> GVTCs { get; set; }
        public virtual DbSet<LopHC> LopHCs { get; set; }
        public virtual DbSet<LopTC> LopTCs { get; set; }
        public virtual DbSet<LopTC_SV> LopTC_SV { get; set; }
        public virtual DbSet<Sinhvien> Sinhviens { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<Hocphan> Hocphans { get; set; }
        public virtual DbSet<TempSV> TempSV { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<giangvien>()
                .Property(e => e.email)
                .IsFixedLength();

            modelBuilder.Entity<giangvien>()
                .Property(e => e.username)
                .IsFixedLength();

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.username)
                .IsFixedLength();

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.password)
                .IsFixedLength();
        }
    }
}
