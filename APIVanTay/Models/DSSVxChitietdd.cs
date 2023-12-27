using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace APIVanTay.Models
{
    public class DSSVxChitietdd
    {
        [DisplayName("Mã sinh viên")]
        public string masv { get; set; }

        [StringLength(50)]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [DisplayName("Giới tính")]
        public string gioitinh { get; set; }

        [DisplayName("Tên lớp hành chính")]
        public string tenlophc { get; set; }

        [DisplayName("Khoa")]
        public string khoa { get; set; }

        [DisplayName("Thời gian điểm danh")]
        public DateTime thoigiandd { get; set; }

        [DisplayName("Trạng thái")]
        public bool trangthai { get; set; }

    }
}