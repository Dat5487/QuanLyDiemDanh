using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ApiLopTC
    {
        [Key]
        [DisplayName("Mã lớp tín chỉ")]
        public string maloptc { get; set; }

        public string tenhp { get; set; }

        [DisplayName("Mã học phần")]
        public string mahp { get; set; }

        [DisplayName("Tên lớp tín chỉ")]
        public string tenltc { get; set; }

        [DisplayName("Mã giảng viên")]
        public string magv { get; set; }

        [DisplayName("Trạng thái")]
        public bool trangthai { get; set; }

    }
}