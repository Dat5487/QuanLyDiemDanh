namespace QLDD_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hocphan")]
    public partial class Hocphan
    {
        [Key]
        [StringLength(10, ErrorMessage = "Mã học phần phải dưới 10 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập mã học phần")]
        [DisplayName("Mã học phần")]
        public string mahp { get; set; }

        [StringLength(50, ErrorMessage = "Tên học phần phải dưới 50 ký tự")]
        [Required(ErrorMessage = "Bắt buộc phải nhập tên học phần")]
        [DisplayName("Tên học phần")]
        public string tenhp { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập số tín chỉ")]
        [DisplayName("Số tín chỉ")]
        public int sotc { get; set; }


    }
}
