using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace QLDD_MVC.Models
{
    [Table("GVTC")]
    public partial class GVTC : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public GVTC()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM GVTC ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "GVTC");
            da1.Fill(ds, "GVTC");
        }

        public void CreateGVTC(string magv, string maloptc, string username)
        {
            DataRow r = ds.Tables["GVTC"].NewRow();
            r["maloptc"] = maloptc;
            r["magv"] = magv;
            r["username"] = username;

            ds.Tables["GVTC"].Rows.Add(r);
            da1.Update(ds, "GVTC");
            ds.AcceptChanges();
        }

        public void DeleteGVTC(string magv, string maloptc)
        {
            string query = String.Format("magv = '{0}' AND maloptc = '{1}'", magv, maloptc);
            DataRow[] rows = ds.Tables["GVTC"].Select(query);
            rows[0].Delete();
            da1.Update(ds, "GVTC");
            ds.AcceptChanges();
        }

        public void EditGVTC(string magv, string maloptc, string username)
        {
            string query = String.Format("maloptc = '{0}'", maloptc);
            DataRow[] rows = ds.Tables["GVTC"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["maloptc"] = maloptc;
                rows[0]["magv"] = magv;
                rows[0]["username"] = username;
                rows[0].EndEdit();
                da1.Update(ds, "GVTC");
                ds.AcceptChanges();
            }
        }
        public int id { get; set; }

        public string magv { get; set; }

        public string maloptc { get; set; }
        public string username { get; set; }

    }
}
