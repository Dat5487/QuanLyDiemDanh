using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QLDD_MVC.Models
{
    public class DSSVxDiemDanh
    {
        [Key]
        [DisplayName("Mã sinh viên")]
        public string masv { get; set; }

        [StringLength(50)]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [DisplayName("Giới tính")]
        public string gioitinh { get; set; }

        [StringLength(50)]
        [DisplayName("Khoa")]
        public string khoa { get; set; }

        [DisplayName("Số ngày điểm danh")]
        public int soluongdd { get; set; }

        [DisplayName("Tên lớp hành chính")]
        public string tenlophc { get; set; }

    }
}