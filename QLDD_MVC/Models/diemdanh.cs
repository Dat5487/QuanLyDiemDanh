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

    [Table("diemdanh")]
    public partial class diemdanh : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public diemdanh()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM diemdanh ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "diemdanh");
            da1.Fill(ds, "diemdanh");
        }

        public void CreateDiemdanh(int? maloptc, string diadiem)
        {
            DataRow r = ds.Tables["diemdanh"].NewRow();
            DataRow[] rows = ds.Tables["diemdanh"].Select();
            r["maloptc"] = maloptc;
            r["ngaydd"] = DateTime.Now;
            r["diadiem"] = diadiem;

            ds.Tables["diemdanh"].Rows.Add(r);
            da1.Update(ds, "diemdanh");
            ds.AcceptChanges();
        }

        public void DeleteDiemdanh(int? maloptc)
        {
            string query = String.Format("maloptc = {0}", maloptc);
            DataRow[] rows = ds.Tables["diemdanh"].Select(query);
            rows[0].Delete();
            da1.Update(ds, "diemdanh");
            ds.AcceptChanges();
        }

        public void DeleteHDDiemdanh(int? maloptc)
        {
            string query = String.Format("maloptc = {0} AND ngaydd = '{1}'", maloptc,DateTime.Now.Date);
            DataRow[] rows = ds.Tables["diemdanh"].Select(query);
            rows[0].Delete();
            da1.Update(ds, "diemdanh");
            ds.AcceptChanges();
        }

        [Key]
        [DisplayName("Mã điểm danh")]
        public int madd { get; set; }

        [DisplayName("Mã lớp tín chỉ")]
        public int? maloptc { get; set; }

        public DateTime ngaydd { get; set; }

        [StringLength(50)]
        [DisplayName("Địa điểm")]
        public string diadiem { get; set; }



    }
}
