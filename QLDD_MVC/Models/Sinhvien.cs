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

    [Table("Sinhvien")]
    public partial class Sinhvien : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public Sinhvien()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM Sinhvien ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "Sinhvien");
            da1.Fill(ds, "Sinhvien");
        }

        public void EditSinhvien(string masv, string EmbFace)
        {
            string query = String.Format("masv = '{0}'", masv);
            DataRow[] rows = ds.Tables["Sinhvien"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["EmbFace"] = EmbFace;
                rows[0].EndEdit();
                da1.Update(ds, "Sinhvien");
                ds.AcceptChanges();
            }
        }



        [Key]
        [DisplayName("Mã sinh viên")]
        public string masv { get; set; }

        [StringLength(50, ErrorMessage = "Họ tên phải dưới 50 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập họ tên")]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [DisplayName("Giới tính")]
        public string gioitinh { get; set; }

        [DisplayName("Mã lớp hành chính")]
        public string malophc { get; set; }

        [StringLength(50)]
        [DisplayName("Khoa")]
        public string khoa { get; set; }
        
        public string EmbFace { get; set; }

    }
}
