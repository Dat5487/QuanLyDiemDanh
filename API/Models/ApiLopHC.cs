using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ApiLopHC
    {
        public int id { get; set; }

        [Key]
        public string malophc { get; set; }

        public string tenlophc { get; set; }

        public string magv { get; set; }

        public string khoa { get; set; }
        public int sosv { get; set; }

    }
}