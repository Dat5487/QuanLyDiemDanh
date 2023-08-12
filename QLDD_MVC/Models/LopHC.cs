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

    [Table("LopHC")]
    public partial class LopHC : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public LopHC()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM LopHC ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "LopHC");
            da1.Fill(ds, "LopHC");
        }
        public void CreateLopHC(string tenlophc, string magv, string khoa)
        {
            DataRow r = ds.Tables["LopHC"].NewRow();
            r["tenlophc"] = tenlophc;
            r["magv"] = magv;
            r["khoa"] = khoa;
            ds.Tables["LopHC"].Rows.Add(r);
            da1.Update(ds, "LopHC");
            ds.AcceptChanges();
        }
        public void EditLopHC(string malophc, string tenlophc, string magv, string khoa)
        {
            string query = String.Format("malophc = '{0}'", malophc);
            DataRow[] rows = ds.Tables["LopHC"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["tenlophc"] = tenlophc;
                rows[0]["magv"] = magv;
                rows[0]["khoa"] = khoa;
                rows[0].EndEdit();
                da1.Update(ds, "LopHC");
                ds.AcceptChanges();
            }
        }
        public int id { get; set; }

        [Key]
        [DisplayName("Mã lớp hành chính")]
        public string malophc { get; set; }

        [StringLength(50, ErrorMessage = "Tên lớp hành chính phải dưới 50 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập tên lớp hành chính")]
        [DisplayName("Tên lớp hành chính")]
        public string tenlophc { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập mã giảng viên")]
        [DisplayName("Mã giảng viên")]
        public string magv { get; set; }

        [DisplayName("Khoa")]
        public string khoa { get; set; }
    }
}
