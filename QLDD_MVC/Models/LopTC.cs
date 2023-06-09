﻿namespace QLDD_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;

    [Table("LopTC")]
    public partial class LopTC : KetNoiSql
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da1 = new SqlDataAdapter();
        public LopTC()
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            string query = String.Format("SELECT * FROM LopTC ");
            da1.SelectCommand = new SqlCommand(query, conn);
            da1.TableMappings.Add("Table", "LopTC");
            da1.Fill(ds, "LopTC");
        }


        public int getStt(string mahp)//Lấy Id lớn nhất + 1
        {
            conn.Open();
            string query = String.Format("SELECT Max(sttlop) FROM LopTC WHERE mahp = '{0}';", mahp);
            SqlCommand cmd = new SqlCommand(query, conn);
            if (Int32.Parse(cmd.ExecuteScalar().ToString()) == 0)
            {
                conn.Close();
                return 1;
            }
            int id = Int32.Parse(cmd.ExecuteScalar().ToString()) + 1;
            conn.Close();
            return id;
        }

        public void EditTenLopTC(int maloptc,string mahp,int sttlop)
        {
            mahp = mahp.TrimEnd(' ');
            string query = String.Format("maloptc = '{0}'",maloptc );
            int stt = getStt(mahp);
            DataRow[] rows = ds.Tables["LopTC"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["tenltc"] = String.Format("{0}.{1}", mahp, sttlop); ;
                rows[0].EndEdit();
                da1.Update(ds, "LopTC");
                ds.AcceptChanges();
            }
        }

        [Key]
        [DisplayName("Mã lớp tín chỉ")]
        public int maloptc { get; set; }

        [StringLength(20, ErrorMessage = "Mã học phần phải dưới 10 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập mã học phần")]
        [DisplayName("Mã học phần")]
        public string mahp { get; set; }

        [DisplayName("Tên lớp tín chỉ")]
        public string tenltc { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập mã giảng viên")]
        [DisplayName("Mã giảng viên")]
        public int? magv { get; set; }

        [DisplayName("Trạng thái")]
        public bool trangthai { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập số thứ tự lớp")]
        [DisplayName("Số thứ tự lớp")]
        public int sttlop { get; set; }
    }
}
