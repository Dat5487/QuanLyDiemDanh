using SourceAFIS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
namespace APIVanTay.Models
{

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
        public void EditVanTay(string masv, byte[] MauVanTay)
        {
            string query = String.Format("masv = '{0}'", masv);
            DataRow[] rows = ds.Tables["Sinhvien"].Select(query);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                rows[0]["MauVanTay"] = MauVanTay;
                rows[0].EndEdit();
                da1.Update(ds, "SinhVien");
                ds.AcceptChanges();
            }
        }

        public void UpdateVanTay(string masv)
        {
            string location = "D:/image/" + masv + ".bmp";
            var template = new FingerprintTemplate(new FingerprintImage(File.ReadAllBytes(location)));
            var storeTemplate = template.ToByteArray();

            EditVanTay(masv, storeTemplate);
        }

        public string NhanDienVanTay(IQueryable<Sinhvien> canidate)
        {
            string imagePath = "D:/image/temp.bmp";
            var template = new FingerprintTemplate(new FingerprintImage(File.ReadAllBytes(imagePath)));
            var sv = Identify(template, canidate);
            if (sv == null)
                return null;
            return sv.masv;
        }
        public Sinhvien Identify(FingerprintTemplate probe, IEnumerable<Sinhvien> candidates)
        {
            var matcher = new FingerprintMatcher(probe);
            Sinhvien match = null;
            double max = Double.NegativeInfinity;
            foreach (var candidate in candidates)
            {
                double similarity = matcher.Match(new FingerprintTemplate(candidate.MauVanTay));
                if (similarity > max)
                {
                    max = similarity;
                    match = candidate;
                }
            }
            double threshold = 40;
            return max >= threshold ? match : null;
        }


        const int WIDTH = 256;
        const int HEIGHT = 288;
        const int DEPTH = 8;
        const int HEADER_SZ = 54;

        static byte[] AssembleHeader(int width, int height, int depth, bool cTable)
        {
            byte[] header = new byte[HEADER_SZ];
            header[0] = (byte)'B';
            header[1] = (byte)'M';   // bmp signature
            int byte_width = ((depth * width + 31) / 32) * 4;
            if (cTable)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(byte_width * height + (1 << depth) * 4 + HEADER_SZ), 0, header, 2, 4);
                Buffer.BlockCopy(BitConverter.GetBytes((1 << depth) * 4 + HEADER_SZ), 0, header, 10, 4);
            }
            else
            {
                Buffer.BlockCopy(BitConverter.GetBytes(byte_width * height + HEADER_SZ), 0, header, 2, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(HEADER_SZ), 0, header, 10, 4); //offset
            }

            Buffer.BlockCopy(BitConverter.GetBytes(40), 0, header, 14, 4);    //file header size
            Buffer.BlockCopy(BitConverter.GetBytes(width), 0, header, 18, 4); //width
            Buffer.BlockCopy(BitConverter.GetBytes(-height), 0, header, 22, 4); //height
            Buffer.BlockCopy(BitConverter.GetBytes((short)1), 0, header, 26, 2); //no of planes
            Buffer.BlockCopy(BitConverter.GetBytes((short)depth), 0, header, 28, 2); //depth
            Buffer.BlockCopy(BitConverter.GetBytes(byte_width * height), 0, header, 34, 4); //image size
            Buffer.BlockCopy(BitConverter.GetBytes(1), 0, header, 38, 4); //resolution
            Buffer.BlockCopy(BitConverter.GetBytes(1), 0, header, 42, 4);

            return header;
        }

        public void CreateImage(string filename)
        {
            string bmp = @"D:/image/" + filename + ".bmp";
            string bmpp = @"D:/image/" + filename + ".bmpp";
            using (BinaryWriter fingerpint = new BinaryWriter(File.Open(bmp, FileMode.Create)))
            {
                fingerpint.Write(AssembleHeader(WIDTH, HEIGHT, DEPTH, true));
                for (int i = 0; i < 256; i++)
                {
                    fingerpint.Write((byte)i);
                    fingerpint.Write((byte)i);
                    fingerpint.Write((byte)i);
                    fingerpint.Write((byte)i);
                }

                filename += ".bmpp";
                using (BinaryReader file = new BinaryReader(File.Open(bmpp, FileMode.Open)))
                {
                    byte byteData;
                    while (file.BaseStream.Position < file.BaseStream.Length)
                    {
                        byteData = file.ReadByte();
                        fingerpint.Write(byteData);
                        fingerpint.Write(byteData);
                    }
                }
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

        public byte[]? MauVanTay { get; set; }

    }
}
