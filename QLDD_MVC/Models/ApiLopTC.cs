using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QLDD_MVC.Models
{
    public class ApiLopTC
    {
        [Key]
        [DisplayName("Mã lớp tín chỉ")]
        public int maloptc { get; set; }

        public string tenhp { get; set; }

        [DisplayName("Mã học phần")]
        public string mahp { get; set; }

        [DisplayName("Tên lớp tín chỉ")]
        public string tenltc { get; set; }

        [DisplayName("Mã giảng viên")]
        public int? magv { get; set; }

        [DisplayName("Trạng thái")]
        public bool trangthai { get; set; }

    }
}