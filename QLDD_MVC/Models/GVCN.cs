using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.Spatial;
using System.Data.SqlClient;

namespace QLDD_MVC.Models
{
    [Table("GVCN")]
    public partial class GVCN : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public GVCN()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM GVCN ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "GVCN");
            da1.Fill(ds, "GVCN");
        }

        public void CreateGVCN(int magv, int malophc,string username)
        {
            DataRow r = ds.Tables["GVCN"].NewRow();
            r["malophc"] = malophc;
            r["magv"] = magv;
            r["username"] = username;
            ds.Tables["GVCN"].Rows.Add(r);
            da1.Update(ds, "GVCN");
            ds.AcceptChanges();
        }

        public void DeleteGVCN(int magv, int malophc)
        {
            string query = String.Format("magv = {0} AND malophc = {1}", magv, malophc);
            DataRow[] rows = ds.Tables["GVCN"].Select(query);
            rows[0].Delete();
            da1.Update(ds, "GVCN");
            ds.AcceptChanges();
        }

        public void EditGVCN(int magv, int malophc, string username)
        {
            string query = String.Format("malophc = {0}", malophc);
            DataRow[] rows = ds.Tables["GVCN"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["malophc"] = malophc;
                rows[0]["magv"] = magv;
                rows[0]["username"] = username;
                rows[0].EndEdit();
                da1.Update(ds, "GVCN");
                ds.AcceptChanges();
            }
        }
        public int id { get; set; }

        public int? magv { get; set; }

        public int? malophc { get; set; }
        public string username { get; set; }

    }
}
