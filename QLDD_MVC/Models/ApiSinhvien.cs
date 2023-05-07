namespace QLDD_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ApiSinhvien
    {
        [Key]
        [DisplayName("Mã sinh viên")]
        public int masv { get; set; }

        [StringLength(50)]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [DisplayName("Giới tính")]
        public bool gioitinh { get; set; }

        [DisplayName("Tên lớp hành chính")]
        public string tenlophc { get; set; }

        [StringLength(50)]
        [DisplayName("Khoa")]
        public string khoa { get; set; }

    }
}
