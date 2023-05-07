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

    [Table("TaiKhoan")]
    public partial class TaiKhoan : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public TaiKhoan()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM TaiKhoan ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "TaiKhoan");
            da1.Fill(ds, "TaiKhoan");
        }
        public void EditTaiKhoan(string username, string hoten)
        {
            string query = String.Format("username = '{0}'", username);
            DataRow[] rows = ds.Tables["TaiKhoan"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["hoten"] = hoten;
                rows[0].EndEdit();
                da1.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
        }

        [Key]
        [StringLength(20, ErrorMessage = "Tên đăng nhập phải dưới 20 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập tên đăng nhập")]
        [DisplayName("Tên đăng nhập")]
        public string username { get; set; }

        [StringLength(50, ErrorMessage = "Họ tên phải dưới 50 ký tự")]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [DisplayName("Phân quyền")]
        public string phanquyen { get; set; }

        [StringLength(20, ErrorMessage = "Mật khẩu phải dưới 20 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập mật khẩu")]
        [DisplayName("Mật khẩu")]
        public string password { get; set; }
    }
}
