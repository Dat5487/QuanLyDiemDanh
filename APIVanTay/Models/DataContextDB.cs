using Microsoft.EntityFrameworkCore;
using APIVanTay.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace APIVanTay.Models
{
    public class DataContextDB : DbContext
    {
        public DataContextDB(DbContextOptions<DataContextDB> options) : base(options) { }

        public DbSet<Sinhvien> Sinhviens { get; set; }
        public DbSet<chitietdd> chitietdds { get; set; }
        public DbSet<diemdanh> diemdanhs { get; set; }
        public DbSet<giangvien> giangviens { get; set; }
        public DbSet<GVCN> GVCNs { get; set; }
        public DbSet<GVTC> GVTCs { get; set; }
        public DbSet<LopHC> LopHCs { get; set; }
        public DbSet<LopTC> LopTCs { get; set; }
        public DbSet<LopTC_SV> LopTC_SV { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<Hocphan> Hocphans { get; set; }
        public DbSet<ApiDSSVChitietdd> ApiDSSVChitietdds { get; set; }
        public DbSet<ApiDSSVDiemdanh> ApiDSSVDiemdanhs { get; set; }
        public DbSet<ApiLopHC> ApiLopHCs { get; set; }
        public DbSet<ApiLopTC> ApiLopTCs { get; set; }
        public DbSet<ApiSinhvien> ApiSinhviens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sinhvien>().ToTable("Sinhvien");
            modelBuilder.Entity<chitietdd>().ToTable("chitietdd");
            modelBuilder.Entity<diemdanh>().ToTable("diemdanh");
            modelBuilder.Entity<giangvien>().ToTable("giangvien");
            modelBuilder.Entity<GVCN>().ToTable("GVCN");
            modelBuilder.Entity<GVTC>().ToTable("GVTC");
            modelBuilder.Entity<LopHC>().ToTable("LopHC");
            modelBuilder.Entity<LopTC>().ToTable("LopTC");
            modelBuilder.Entity<LopTC_SV>().ToTable("LopTC_SV");
            modelBuilder.Entity<TaiKhoan>().ToTable("TaiKhoan");
            modelBuilder.Entity<Hocphan>().ToTable("Hocphan");
            modelBuilder.Entity<ApiSinhvien>().HasNoKey();
            modelBuilder.Entity<ApiDSSVChitietdd>().HasNoKey();
            modelBuilder.Entity<ApiDSSVDiemdanh>().HasNoKey();
            modelBuilder.Entity<ApiLopHC>().HasNoKey();
            modelBuilder.Entity<ApiLopTC>().HasNoKey();
        }


    }
}
