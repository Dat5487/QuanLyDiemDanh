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

    [Table("chitietdd")]
    public partial class chitietdd : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public chitietdd()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM chitietdd ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "chitietdd");
            da1.Fill(ds, "chitietdd");
        }

        public void CreateChitietdd(int madd, string masv)
        {
            DataRow r = ds.Tables["chitietdd"].NewRow();
            r["madd"] = madd;
            r["masv"] = masv;
            r["thoigiandd"] = DateTime.Now;
            r["trangthai"] = false;

            ds.Tables["chitietdd"].Rows.Add(r);
            da1.Update(ds, "chitietdd");
            ds.AcceptChanges();
        }

        public void DeleteChitietdd(int madd, string masv)
        {
            string query = String.Format("madd = {0} AND masv = {1}", madd, masv);
            DataRow[] rows = ds.Tables["chitietdd"].Select(query);
            rows[0].Delete();
            da1.Update(ds, "chitietdd");
            ds.AcceptChanges();
        }

        public void DeleteHDChitietDD(int madd)
        {
            string query = String.Format("madd = {0}", madd);
            DataRow[] rows = ds.Tables["chitietdd"].Select(query);
            try
            {
                rows[0].Delete();
            }
            catch (Exception ex) { }
            da1.Update(ds, "chitietdd");
            ds.AcceptChanges();
        }
        public void EditChitietdd(int madd, string masv)
        {
            var now = DateTime.Now.Date;
            string query = String.Format("madd = '{0}' AND masv = '{1}'", madd, masv);
            DataRow[] rows = ds.Tables["chitietdd"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["thoigiandd"] = DateTime.Now;
                rows[0]["trangthai"] = true;
                rows[0].EndEdit();
                da1.Update(ds, "chitietdd");
                ds.AcceptChanges();
            }
        }
        [Key]
        public int id { get; set; }

        [DisplayName("Mã điểm danh")]
        public int madd { get; set; }

        [DisplayName("Mã sinh viên")]
        public string masv { get; set; }

        public DateTime thoigiandd { get; set; }

        public bool trangthai { get; set; }
    }
}
