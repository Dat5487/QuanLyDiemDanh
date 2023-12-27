using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace APIVanTay.Models
{
    public class ApiDSSVDiemdanh
    {
        [StringLength(50)]
        [DisplayName("Họ tên")]
        public string hoten { get; set; }

        [DisplayName("Giới tính")]
        public string gioitinh { get; set; }

        [DisplayName("Số buổi điểm danh")]
        public int sobuoidd { get; set; }

    }
}