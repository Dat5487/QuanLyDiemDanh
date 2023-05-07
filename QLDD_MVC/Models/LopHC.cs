namespace QLDD_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LopHC")]
    public partial class LopHC
    {
        [Key]
        [DisplayName("Mã lớp hành chính")]
        public int malophc { get; set; }

        [StringLength(50, ErrorMessage = "Tên lớp hành chính phải dưới 50 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập tên lớp hành chính")]
        [DisplayName("Tên lớp hành chính")]
        public string tenlophc { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập mã giảng viên")]
        [DisplayName("Mã giảng viên")]
        public int? magv { get; set; }

        [DisplayName("Khoa")]
        public string khoa { get; set; }
    }
}
