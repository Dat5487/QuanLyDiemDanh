using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace APIVanTay.Models
{

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
        public void Editgiangvien(string magv, string hoten)
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

        //Vì EF Code First không chấp nhận cột không có datatype (cột magv là computed columns)
        //Nên phải dùng cách khác để insert cột
        public void CreateGV(string hoten, string gioitinh, string diachi,string email, string sdt, string username, byte[] UserPhoto)
        {
            DataRow r = ds.Tables["giangvien"].NewRow();
            r["hoten"] = hoten;
            r["gioitinh"] = gioitinh;
            r["diachi"] = diachi;
            r["email"] = email;
            r["sdt"] = sdt;
            r["username"] = username;
            r["UserPhoto"] = UserPhoto;
            ds.Tables["giangvien"].Rows.Add(r);
            da1.Update(ds, "giangvien");
            ds.AcceptChanges();
        }
        public void EditGV(string magv,string hoten, string gioitinh, string diachi, string email, string sdt, string username, byte[] UserPhoto)
        {
            string query = String.Format("magv = '{0}'", magv);
            DataRow[] rows = ds.Tables["giangvien"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                if(UserPhoto != null)
                {
                    rows[0]["hoten"] = hoten;
                    rows[0]["gioitinh"] = gioitinh;
                    rows[0]["diachi"] = diachi;
                    rows[0]["email"] = email;
                    rows[0]["sdt"] = sdt;
                    rows[0]["username"] = username;
                    rows[0]["UserPhoto"] = UserPhoto;
                }
                else
                {
                    rows[0]["hoten"] = hoten;
                    rows[0]["gioitinh"] = gioitinh;
                    rows[0]["diachi"] = diachi;
                    rows[0]["email"] = email;
                    rows[0]["sdt"] = sdt;
                    rows[0]["username"] = username;
                }
                rows[0].EndEdit();
                da1.Update(ds, "giangvien");
                ds.AcceptChanges();
            }
        }


        public int id { get; set; }

        [Key]
        public string magv { get; set; }

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

        [DisplayName("Ảnh")]
        public byte[] UserPhoto { get; set; }

    }
}
