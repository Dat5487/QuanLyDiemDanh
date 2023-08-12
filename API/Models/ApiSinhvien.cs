namespace API.Models
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
        public string masv { get; set; }

        [StringLength(50)]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [DisplayName("Giới tính")]
        public string gioitinh { get; set; }
        [DisplayName("Mã lớp hành chính")]
        public string malophc { get; set; }

        [DisplayName("Tên lớp hành chính")]
        public string tenlophc { get; set; }

        [StringLength(50)]
        [DisplayName("Khoa")]
        public string khoa { get; set; }
        public string EmbFace { get; set; }

    }
}
