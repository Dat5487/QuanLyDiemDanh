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

    [Table("LopTC_SV")]
    public partial class LopTC_SV : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public LopTC_SV()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM LopTC_SV ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "LopTC_SV");
            da1.Fill(ds, "LopTC_SV");
        }


        public void DeleteLopTC_SV(int? maloptc, string masv)
        {
            string query = String.Format("maloptc = {0} AND masv = '{1}'", maloptc, masv);
            DataRow[] rows = ds.Tables["LopTC_SV"].Select(query);
            rows[0].Delete();
            da1.Update(ds, "LopTC_SV");
            ds.AcceptChanges();
        }
        public int id { get; set; }

        [DisplayName("Mã lớp tín chỉ")]
        public int maloptc { get; set; }

        [DisplayName("Mã sinh viên")]
        public string masv { get; set; }
    }
}
