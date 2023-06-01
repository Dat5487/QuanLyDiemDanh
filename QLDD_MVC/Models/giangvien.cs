namespace QLDD_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;

    [Table("giangvien")]
    public partial class giangvien : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public giangvien()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM giangvien ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "giangvien");
            da1.Fill(ds, "giangvien");
        }

        public void Creategiangvien(string username,string hoten)
        {
            DataRow r = ds.Tables["giangvien"].NewRow();
            r["username"] = username;
            r["hoten"] = hoten;
            r["gioitinh"] = "Nam";

            ds.Tables["giangvien"].Rows.Add(r);
            da1.Update(ds, "giangvien");
            ds.AcceptChanges();
        }

        public void Editgiangvien(int magv, string hoten)
        {
            string query = String.Format("magv = '{0}'", magv);
            DataRow[] rows = ds.Tables["giangvien"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["hoten"] = hoten;
                rows[0].EndEdit();
                da1.Update(ds, "giangvien");
                ds.AcceptChanges();
            }
        }
        [Key]
        [DisplayName("Mã giảng viên")]
        public int magv { get; set; }

        [StringLength(50, ErrorMessage = "Họ tên phải dưới 50 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập họ tên")]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [Required]
        [DisplayName("Giới tính")]
        public string gioitinh { get; set; }

        [StringLength(50)]
        [DisplayName("Địa chỉ")]
        public string diachi { get; set; }

        [StringLength(50)]
        [DisplayName("Email")]
        public string email { get; set; }

        [StringLength(11)]
        [DisplayName("Số điện thoại")]
        public string sdt { get; set; }

        [StringLength(20)]
        [DisplayName("Tên đăng nhập")]
        public string username { get; set; }
    }
}
